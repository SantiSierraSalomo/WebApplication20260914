using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication20260914.Data;

namespace WebApplication20260914.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DatabaseInfoController(AppDbContext dbContext) : ControllerBase
    {
        [HttpGet("current")]
        public async Task<ActionResult<DatabaseInfoDto>> GetCurrentDatabase(CancellationToken cancellationToken)
        {
            var databaseName = await dbContext.Database
                .SqlQueryRaw<string>("SELECT DB_NAME()")
                .FirstOrDefaultAsync(cancellationToken);

            if (string.IsNullOrWhiteSpace(databaseName))
            {
                return NotFound();
            }

            return Ok(new DatabaseInfoDto(databaseName));
        }

        public sealed record DatabaseInfoDto(string DatabaseName);
    }
}
