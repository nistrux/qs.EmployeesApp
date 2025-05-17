using Microsoft.EntityFrameworkCore;
using qs.EmployeesApp.Infrastructure.Data;

namespace qs.EmployeesApp.Web.Infrastructure.Configuration;

public static class MigrationsApplier
{
    public static async Task ApplyMigrations(this WebApplication webApplication)
    {
        await using var scope = webApplication.Services.CreateAsyncScope();
        var services = scope.ServiceProvider;
        try
        {
            var context = services.GetRequiredService<EmpDbContext>();
            var logger = services.GetRequiredService<ILogger<Program>>();

            logger.LogInformation("trying to apply migrations");
            await context.Database.MigrateAsync();
            logger.LogInformation("migrations applied (if any)");
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred while migrating the database.");

            Environment.ExitCode = -1;
            Environment.Exit(Environment.ExitCode);
        }
    }
}