using FluentValidation;
using qs.EmployeesApp.Application.Contracts.Commands;
using qs.EmployeesApp.Application.Contracts.Repositories;
using qs.EmployeesApp.Infrastructure.Data;
using qs.EmployeesApp.Infrastructure.Data.Repositories;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

namespace qs.EmployeesApp.Web.Infrastructure.Configuration;

public static class ConfigureInfrastructureServices
{
    public static void AddInfrastructureServices(this WebApplicationBuilder appBuilder)
    {
        appBuilder.Services.AddEmpDbContext(appBuilder.Configuration);
        appBuilder.Services.AddRepositories();
        appBuilder.Services.AddMediator();
        appBuilder.Services.AddFluentValidation();
    }

    private static void AddEmpDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("EmpConnection");
        
        if (string.IsNullOrEmpty(connectionString)) 
            throw new ApplicationException("Connection string not found");
        /*
         * cd qs.EmployeesApp.Web/ 
         * dotnet user-secrets init
         * dotnet user-secrets set "ConnectionStrings:EmpConnection" "Host=etc..."
         */
        
        services.AddNpgsql<EmpDbContext>(connectionString);
    }

    private static void AddMediator(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateEmployeeCommand).Assembly));
    }

    private static void AddRepositories(this IServiceCollection services)
    {
        services.AddTransient<IEmployeeRepository, EmployeeRepository>();
        services.AddTransient<IPositionRepository, PositionRepository>();
        services.AddTransient<IDepartmentRepository, DepartmentRepository>();
    }

    private static void AddFluentValidation(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<CreateEmployeeCommand>();
        services.AddFluentValidationAutoValidation();
    }
}