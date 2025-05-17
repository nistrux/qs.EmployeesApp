namespace qs.EmployeesApp.Web.Infrastructure.Configuration;

public static class ConfigureWebServices
{
    public static void AddWebServices(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }
}