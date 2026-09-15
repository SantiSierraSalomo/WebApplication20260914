using Microsoft.AspNetCore.Mvc;
using WebApplication20260914.Masterdata.Dtos;
using WebApplication20260914.Masterdata.Service;

namespace WebApplication20260914.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class ItemController(IItemService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ItemDto>>> GetAll(CancellationToken cancellationToken)
    {
        return Ok(await service.GetAllAsync(cancellationToken));
    }

    [HttpGet("{itemId}")]
    public async Task<ActionResult<ItemDto>> GetById(string itemId, CancellationToken cancellationToken)
    {
        var result = await service.GetByIdAsync(itemId, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<ItemDto>> Create([FromBody] CreateItemDto request, CancellationToken cancellationToken)
    {
        var created = await service.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { itemId = created.ItemId }, created);
    }

    [HttpPut("{itemId}")]
    public async Task<ActionResult<ItemDto>> Update(string itemId, [FromBody] UpdateItemDto request, CancellationToken cancellationToken)
    {
        try
        {
            var updated = await service.UpdateAsync(itemId, request, cancellationToken);
            return updated is null ? NotFound() : Ok(updated);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{itemId}")]
    public async Task<ActionResult> Delete(string itemId, CancellationToken cancellationToken)
    {
        var deleted = await service.DeleteAsync(itemId, cancellationToken);
        return deleted ? NoContent() : NotFound();
    }
}
