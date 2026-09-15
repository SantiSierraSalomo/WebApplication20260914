using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using WebApplication20260914.Configuration;

namespace WebApplication20260914.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RabbitMqController(IOptions<RabbitMqOptions> rabbitMqOptions) : ControllerBase
{
    private const string ExchangeName = "ExchangeSanti";
    private const string QueueName = "QueueSanti";
    private const int MaxReadCountLimit = 1000;

    [HttpPost("publish")]
    public async Task<ActionResult> Publish([FromBody] RabbitMqPublishRequestDto request, CancellationToken cancellationToken)
    {
        if (request is null)
        {
            return BadRequest("Request body is required.");
        }

        //if (string.IsNullOrWhiteSpace(request.RoutingKey))
        //{
        //    return BadRequest("RoutingKey is required.");
        //}

        if (request.Body is null)
        {
            return BadRequest("Body is required.");
        }

        if (string.IsNullOrWhiteSpace(request.Header1))
        {
            return BadRequest("Header1 is required.");
        }

        if (string.IsNullOrWhiteSpace(request.Header2))
        {
            return BadRequest("Header2 is required.");
        }

        if (string.IsNullOrWhiteSpace(request.UserId))
        {
            return BadRequest("UserId is required.");
        }

        if (string.IsNullOrWhiteSpace(request.AppId))
        {
            return BadRequest("AppId is required.");
        }

        try
        {
            var factory = CreateConnectionFactory();

            if (!string.Equals(request.UserId, factory.UserName, StringComparison.Ordinal))
            {
                return BadRequest($"UserId must match the RabbitMQ authenticated user '{factory.UserName}'.");
            }

            await using var connection = await factory.CreateConnectionAsync(cancellationToken);
            await using var channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);

            await EnsureTopologyAsync(channel, request.RoutingKey, cancellationToken);

            var basicProperties = new BasicProperties();
            basicProperties.Headers = ToHeaderTable(request.Header1, request.Header2);
            basicProperties.UserId = request.UserId;
            basicProperties.AppId = request.AppId;

            var bodyBytes = Encoding.UTF8.GetBytes(request.Body);

            string exchangeName = ExchangeName;

            await channel.BasicPublishAsync(
                exchange: exchangeName,
                routingKey: request.RoutingKey,
                mandatory: false,
                basicProperties: basicProperties,
                body: bodyBytes,
                cancellationToken: cancellationToken);

            return Ok(new
            {
                Published = true,
                Exchange = ExchangeName,
                request.RoutingKey
            });
        }
        catch (BrokerUnreachableException ex)
        {
            return StatusCode(StatusCodes.Status502BadGateway, ex.Message);
        }
        catch (OperationInterruptedException ex)
        {
            var reason = ex.ShutdownReason?.ReplyText ?? ex.Message;
            return BadRequest(reason);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpGet("diagnostics/queue")]
    public async Task<ActionResult> GetQueueDiagnostics(CancellationToken cancellationToken)
    {
        try
        {
            var factory = CreateConnectionFactory();

            await using var connection = await factory.CreateConnectionAsync(cancellationToken);
            await using var channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);

            var queue = await channel.QueueDeclareAsync(
                queue: QueueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null,
                cancellationToken: cancellationToken);

            return Ok(new
            {
                Queue = QueueName,
                MessageCount = queue.MessageCount,
                ConsumerCount = queue.ConsumerCount
            });
        }
        catch (BrokerUnreachableException ex)
        {
            return StatusCode(StatusCodes.Status502BadGateway, ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpGet("read")]
    public async Task<ActionResult<IReadOnlyList<RabbitMqReadResponseDto>>> Read([FromQuery] int maxCount = 10, CancellationToken cancellationToken = default)
    {
        if (maxCount < 1 || maxCount > MaxReadCountLimit)
        {
            return BadRequest($"maxCount must be between 1 and {MaxReadCountLimit}.");
        }

        try
        {
            var factory = CreateConnectionFactory();

            await using var connection = await factory.CreateConnectionAsync(cancellationToken);
            await using var channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);

            await EnsureQueueAsync(channel, cancellationToken);

            var messages = new List<RabbitMqReadResponseDto>();
            var deliveryTagsToRequeue = new List<ulong>();

            for (var i = 0; i < maxCount; i++)
            {
                var result = await channel.BasicGetAsync(queue: QueueName, autoAck: false, cancellationToken: cancellationToken);
                if (result is null || IsEmptyGetResult(result))
                {
                    break;
                }

                var body = Encoding.UTF8.GetString(result.Body.ToArray());
                var headers = ToStringHeaders(result.BasicProperties.Headers);
                var props = ToStringProps(result.BasicProperties);

                messages.Add(new RabbitMqReadResponseDto(
                    DeliveryTag: result.DeliveryTag,
                    RoutingKey: result.RoutingKey,
                    Headers: headers,
                    Props: props,
                    Body: body));

                deliveryTagsToRequeue.Add(result.DeliveryTag);
            }

            foreach (var deliveryTag in deliveryTagsToRequeue)
            {
                await channel.BasicNackAsync(deliveryTag: deliveryTag, multiple: false, requeue: true, cancellationToken: cancellationToken);
            }

            return Ok(messages);
        }
        catch (BrokerUnreachableException ex)
        {
            return StatusCode(StatusCodes.Status502BadGateway, ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    private ConnectionFactory CreateConnectionFactory()
    {
        var options = rabbitMqOptions.Value;
        return new ConnectionFactory
        {
            HostName = options.Host,
            Port = options.Port,
            UserName = options.UserName,
            Password = options.Password
        };
    }

    private static IDictionary<string, object?> ToHeaderTable(string header1, string header2)
    {
        var table = new Dictionary<string, object?>
        {
            ["Header1"] = Encoding.UTF8.GetBytes(header1),
            ["Header2"] = Encoding.UTF8.GetBytes(header2)
        };

        return table;
    }

    private static async Task EnsureTopologyAsync(IChannel channel, string routingKey, CancellationToken cancellationToken)
    {
        await channel.ExchangeDeclareAsync(
            exchange: ExchangeName,
            type: ExchangeType.Direct,
            durable: true,
            autoDelete: false,
            arguments: null,
            cancellationToken: cancellationToken);

        await channel.QueueDeclareAsync(
            queue: QueueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null,
            cancellationToken: cancellationToken);

        await channel.QueueBindAsync(
            queue: QueueName,
            exchange: ExchangeName,
            routingKey: routingKey,
            arguments: null,
            cancellationToken: cancellationToken);
    }

    private static async Task EnsureQueueAsync(IChannel channel, CancellationToken cancellationToken)
    {
        await channel.QueueDeclareAsync(
            queue: QueueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null,
            cancellationToken: cancellationToken);
    }

    private static Dictionary<string, string> ToStringHeaders(IDictionary<string, object?>? headers)
    {
        var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        if (headers is null)
        {
            return result;
        }

        foreach (var pair in headers)
        {
            result[pair.Key] = pair.Value switch
            {
                byte[] bytes => Encoding.UTF8.GetString(bytes),
                ReadOnlyMemory<byte> memory => Encoding.UTF8.GetString(memory.Span),
                null => string.Empty,
                _ => pair.Value.ToString() ?? string.Empty
            };
        }

        return result;
    }

    private static Dictionary<string, string> ToStringProps(IReadOnlyBasicProperties basicProperties)
    {
        var props = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        AddIfHasValue(props, "ContentType", basicProperties.ContentType);
        AddIfHasValue(props, "ContentEncoding", basicProperties.ContentEncoding);
        AddIfHasValue(props, "CorrelationId", basicProperties.CorrelationId);
        AddIfHasValue(props, "ReplyTo", basicProperties.ReplyTo);
        AddIfHasValue(props, "Expiration", basicProperties.Expiration);
        AddIfHasValue(props, "MessageId", basicProperties.MessageId);
        AddIfHasValue(props, "Type", basicProperties.Type);
        AddIfHasValue(props, "UserId", basicProperties.UserId);
        AddIfHasValue(props, "AppId", basicProperties.AppId);
        AddIfHasValue(props, "ClusterId", basicProperties.ClusterId);

        if (basicProperties.IsPriorityPresent())
        {
            props["Priority"] = basicProperties.Priority.ToString();
        }

        if (basicProperties.IsDeliveryModePresent())
        {
            props["DeliveryMode"] = basicProperties.DeliveryMode.ToString();
        }

        if (basicProperties.IsTimestampPresent())
        {
            props["Timestamp"] = basicProperties.Timestamp.UnixTime.ToString();
        }

        return props;
    }

    private static void AddIfHasValue(Dictionary<string, string> props, string key, string? value)
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            props[key] = value;
        }
    }

    private static bool IsEmptyGetResult(BasicGetResult result)
    {
        return result.DeliveryTag == 0;
    }
}
