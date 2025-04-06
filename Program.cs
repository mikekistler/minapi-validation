using Microsoft.EntityFrameworkCore;
using eShop.Catalog.API;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<CatalogContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("CatalogConnection")));

builder.Services.AddOptions<CatalogOptions>()
    .BindConfiguration(nameof(CatalogOptions));

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
