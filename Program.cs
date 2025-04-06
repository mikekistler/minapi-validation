using Microsoft.EntityFrameworkCore;
using eShop.Catalog.API;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<CatalogContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("CatalogConnection")));

builder.Services.AddOptions<CatalogOptions>()
    .BindConfiguration(nameof(CatalogOptions));

// REVIEW: This is done for development ease but shouldn't be here in production
builder.Services.AddMigration<CatalogContext, CatalogContextSeed>();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.NumberHandling = JsonNumberHandling.Strict;
});

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddProblemDetails();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseStatusCodePages();

app.MapCatalogApi();

app.Run();
