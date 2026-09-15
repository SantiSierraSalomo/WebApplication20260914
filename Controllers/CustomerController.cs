using Microsoft.AspNetCore.Mvc;
using WebApplication20260914.Masterdata.Dtos;
using WebApplication20260914.Masterdata.Service;

namespace WebApplication20260914.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class CustomerController(ICustomerService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<CustomerDto>>> GetAll(CancellationToken cancellationToken)
    {
        return Ok(await service.GetAllAsync(cancellationToken));
    }

    [HttpGet("{customerId}")]
    public async Task<ActionResult<CustomerDto>> GetById(string customerId, CancellationToken cancellationToken)
    {
        var result = await service.GetByIdAsync(customerId, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<CustomerDto>> Create([FromBody] CreateCustomerDto request, CancellationToken cancellationToken)
    {
        var created = await service.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { customerId = created.CustomerId }, created);
    }

    [HttpPut("{customerId}")]
    public async Task<ActionResult<CustomerDto>> Update(string customerId, [FromBody] UpdateCustomerDto request, CancellationToken cancellationToken)
    {
        try
        {
            var updated = await service.UpdateAsync(customerId, request, cancellationToken);
            return updated is null ? NotFound() : Ok(updated);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{customerId}")]
    public async Task<ActionResult> Delete(string customerId, CancellationToken cancellationToken)
    {
        var deleted = await service.DeleteAsync(customerId, cancellationToken);
        return deleted ? NoContent() : NotFound();
    }
}
