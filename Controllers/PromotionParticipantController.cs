using Microsoft.AspNetCore.Mvc;
using WebApplication20260914.Promotion.Dtos;
using WebApplication20260914.Promotion.Service;

namespace WebApplication20260914.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class PromotionParticipantController(IPromotionParticipantService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<PromotionParticipantDto>>> GetAll(CancellationToken cancellationToken)
    {
        return Ok(await service.GetAllAsync(cancellationToken));
    }

    [HttpGet("{promotionId}/{itemId}/{customerId}")]
    public async Task<ActionResult<PromotionParticipantDto>> GetById(string promotionId, string itemId, string customerId, CancellationToken cancellationToken)
    {
        var result = await service.GetByIdAsync(promotionId, itemId, customerId, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<PromotionParticipantDto>> Create([FromBody] CreatePromotionParticipantDto request, CancellationToken cancellationToken)
    {
        var created = await service.CreateAsync(request, cancellationToken);
        return CreatedAtAction(
            nameof(GetById),
            new { promotionId = created.PromotionId, itemId = created.ItemId, customerId = created.CustomerId },
            created);
    }

    [HttpPut("{promotionId}/{itemId}/{customerId}")]
    public async Task<ActionResult<PromotionParticipantDto>> Update(
        string promotionId,
        string itemId,
        string customerId,
        [FromBody] UpdatePromotionParticipantDto request,
        CancellationToken cancellationToken)
    {
        try
        {
            var updated = await service.UpdateAsync(promotionId, itemId, customerId, request, cancellationToken);
            return updated is null ? NotFound() : Ok(updated);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{promotionId}/{itemId}/{customerId}")]
    public async Task<ActionResult> Delete(string promotionId, string itemId, string customerId, CancellationToken cancellationToken)
    {
        var deleted = await service.DeleteAsync(promotionId, itemId, customerId, cancellationToken);
        return deleted ? NoContent() : NotFound();
    }

    [HttpPost("CreateParticipants")]
    public async Task<ActionResult> CreateParticipants(CancellationToken cancellationToken)
    {
        var recordCount = await service.CreateParticipantsAsync(cancellationToken);
        return CreatedAtAction(
            nameof(CreateParticipants),
            new { recordCount = recordCount },
            recordCount);
    }
}
