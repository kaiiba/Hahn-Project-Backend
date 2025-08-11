Script of migration from SQL Server to C# Entities
Scaffold-DbContext Name="DefaultConnection" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Persistence -Context AppDbContext -Project Project.Infrastructure -StartupProject Project.API -Force

Please move out from the solution folder:
- Project.API
- Project.Application
- Project.Application.Tests
- Project.Domain.Tests
- Project.Infrastructure

Then rebuild the solution. The references already added.
