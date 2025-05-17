using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace qs.EmployeesApp.Infrastructure.Data;

public class EmpDbContextFactory: IDesignTimeDbContextFactory<EmpDbContext>
{
    public EmpDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../qs.EmployeesApp.Web"))
            .AddJsonFile("appsettings.Development.json", optional: true)
            .AddJsonFile("appsettings.json", optional: false)
            .AddEnvironmentVariables()
            .Build();

        var connectionString = configuration.GetConnectionString("EmpConnection");

        var optionsBuilder = new DbContextOptionsBuilder<EmpDbContext>();
        optionsBuilder.UseNpgsql(connectionString);

        return new EmpDbContext(optionsBuilder.Options);
    }
}