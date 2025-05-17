using qs.EmployeesApp.Web.Infrastructure.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.AddInfrastructureServices();
builder.Services.AddWebServices();

var app = builder.Build();

await app.ApplyMigrations();

app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();