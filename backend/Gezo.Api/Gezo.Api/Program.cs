using Gezo.Api.Extensions;
using Gezo.Api.MiddleWares;
using Gezo.Application;
using Gezo.Infrastructure;
using Gezo.Infrastructure.Persistence.Seeding;
using Scalar.AspNetCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddAuthorization();
builder.Services.AddScalarApi();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    try
    {
        await DatabaseSeeder.SeedAsync(app.Services);
    }
    catch (Exception ex)
    {
        var logger = app.Services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}
// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();            // /openapi/v1.json
    app.MapScalarApiReference(); // /scalar/v1
}
app.MapGet("/", () => Results.Redirect("/scalar/v1"));
app.UseMiddleware<GlobalExceptionHanlderMiddleware>();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
