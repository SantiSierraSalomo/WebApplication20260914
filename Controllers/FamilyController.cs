using Microsoft.AspNetCore.Mvc;
using WebApplication20260914.Masterdata.Dtos;
using WebApplication20260914.Masterdata.Service;

namespace WebApplication20260914.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class FamilyController(IFamilyService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<FamilyDto>>> GetAll(CancellationToken cancellationToken)
    {
        return Ok(await service.GetAllAsync(cancellationToken));
    }

    [HttpGet("{familyId}")]
    public async Task<ActionResult<FamilyDto>> GetById(string familyId, CancellationToken cancellationToken)
    {
        var result = await service.GetByIdAsync(familyId, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<FamilyDto>> Create([FromBody] CreateFamilyDto request, CancellationToken cancellationToken)
    {
        var created = await service.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { familyId = created.FamilyId }, created);
    }

    [HttpPut("{familyId}")]
    public async Task<ActionResult<FamilyDto>> Update(string familyId, [FromBody] UpdateFamilyDto request, CancellationToken cancellationToken)
    {
        try
        {
            var updated = await service.UpdateAsync(familyId, request, cancellationToken);
            return updated is null ? NotFound() : Ok(updated);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{familyId}")]
    public async Task<ActionResult> Delete(string familyId, CancellationToken cancellationToken)
    {
        var deleted = await service.DeleteAsync(familyId, cancellationToken);
        return deleted ? NoContent() : NotFound();
    }
}
