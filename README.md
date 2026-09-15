# WebApplication20260914

## Entity Framework Core (SQL Server)

This project is configured to use EF Core SQL Server with a DI-registered `AppDbContext`.

### Regenerate model (database-first)

From the solution root:

`dotnet dotnet-ef dbcontext scaffold "<your-connection-string>" Microsoft.EntityFrameworkCore.SqlServer --context AppDbContext --context-dir Data --output-dir Data/Entities --namespace WebApplication20260914.Data.Entities --context-namespace WebApplication20260914.Data --no-onconfiguring --force`

Notes:
- `--output-dir Data/Entities` writes generated entity classes.
- `--context-dir Data` writes the generated DbContext file.
- `--force` overwrites generated files when the schema changes.
- Ensure the SQL login and database are accessible before scaffolding.
