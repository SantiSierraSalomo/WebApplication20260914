using Microsoft.AspNetCore.Mvc;
using WebApplication20260914.Promotion.Dtos;
using WebApplication20260914.Promotion.Service;

namespace WebApplication20260914.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class PromotionController(IPromotionService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<PromotionDto>>> GetAll(CancellationToken cancellationToken)
    {
        return Ok(await service.GetAllAsync(cancellationToken));
    }

    [HttpGet("{promotionId}")]
    public async Task<ActionResult<PromotionDto>> GetById(string promotionId, CancellationToken cancellationToken)
    {
        var result = await service.GetByIdAsync(promotionId, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<PromotionDto>> Create([FromBody] CreatePromotionDto request, CancellationToken cancellationToken)
    {
        var created = await service.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { promotionId = created.PromotionId }, created);
    }

    [HttpPut("{promotionId}")]
    public async Task<ActionResult<PromotionDto>> Update(string promotionId, [FromBody] UpdatePromotionDto request, CancellationToken cancellationToken)
    {
        try
        {
            var updated = await service.UpdateAsync(promotionId, request, cancellationToken);
            return updated is null ? NotFound() : Ok(updated);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{promotionId}")]
    public async Task<ActionResult> Delete(string promotionId, CancellationToken cancellationToken)
    {
        var deleted = await service.DeleteAsync(promotionId, cancellationToken);
        return deleted ? NoContent() : NotFound();
    }
}
