using Bookify.Api.Extensions;
using Bookify.Application;
using Bookify.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.ApplyMigrations();
    // app.SeedData();
}

app.UseRootRouteOk();
app.UseCustomExceptionHandler();

app.MapControllers();

app.Run();