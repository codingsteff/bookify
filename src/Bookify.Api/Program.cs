using Bookify.Api.Extensions;
using Bookify.Application;
using Bookify.Infrastructure;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddOpenApi();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddEndpoints();

var app = builder.Build();

app.MapApiEndpoints();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.ApplyMigrations();
    app.SeedData();
}

app.UseRequestContextLogging();
app.UseSerilogRequestLogging();

app.UseRootRouteOk();
app.UseCustomExceptionHandler();

app.UseAuthentication();
app.UseAuthorization();

app.MapHealthChecks();

app.Run();

// This class is only used for integration testing
public partial class Program;