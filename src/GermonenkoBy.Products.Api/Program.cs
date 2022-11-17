using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;

using GermonenkoBy.Common.HostedServices;
using GermonenkoBy.Common.Web.Extensions;
using GermonenkoBy.Common.Web.Middleware;
using GermonenkoBy.Products.Core;
using GermonenkoBy.Products.Core.Contracts.Clients;
using GermonenkoBy.Products.Core.Contracts.Repositories;
using GermonenkoBy.Products.Core.HostedServices;
using GermonenkoBy.Products.Infrastructure.Clients;
using GermonenkoBy.Products.Infrastructure.Contracts;
using GermonenkoBy.Products.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

var appConfigConnectionString = builder.Configuration.GetValueUnsafe<string>("AppConfigConnectionString");
if (!string.IsNullOrEmpty(appConfigConnectionString))
{
    builder.Configuration.AddAzureAppConfiguration(options =>
    {
        options.Connect(appConfigConnectionString)
            .Select(KeyFilter.Any)
            .Select(KeyFilter.Any, builder.Environment.EnvironmentName);
    });
}

builder.Services.AddControllers();
builder.Services.AddSwaggerGen(options =>
{
    options.EnableAnnotations();
    options.SwaggerDoc("v1", new()
    {
        Title = "Products microservice"
    });
});

var sqlConnectionString = builder.Configuration.GetValueUnsafe<string>("CoreDatabaseConnectionString");
builder.Services.AddDbContext<ProductsContext>(contextOptionsBuilder =>
{
    contextOptionsBuilder.UseSqlServer(sqlConnectionString);
});

var storageConnectionString = builder.Configuration.GetValueUnsafe<string>("StorageAccountConnectionString");
builder.Services.AddAzureClients(azureBuilder =>
{
    azureBuilder.AddBlobServiceClient(storageConnectionString);
});

builder.Services.AddScoped<IAssetsBlobClient, AzureAssetsBlobClient>();
builder.Services.AddScoped<AssetsService>();
builder.Services.AddScoped<IBulkCategoriesRepository, BulkCategoriesRepository>();
builder.Services.AddScoped<IBulkMaterialsRepository, BulkMaterialsRepository>();
builder.Services.AddScoped<CategoriesSearchService>();
builder.Services.AddScoped<CategoriesService>();
builder.Services.AddScoped<MaterialsSearchService>();
builder.Services.AddScoped<MaterialsService>();
builder.Services.AddScoped<ProductsSearchService>();
builder.Services.AddScoped<ProductsService>();

builder.Services.RegisterHostedService<AssetsCleanupService>(TimeSpan.FromHours(1));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}

var errorHandlingBehaviour = app.Environment.IsDevelopment()
    ? ExceptionHandlingBehavior.RethrowDetailedError
    : ExceptionHandlingBehavior.RethrowGenericError;

app.UseMiddleware<ExceptionHandlingMiddleware>(errorHandlingBehaviour);
app.MapControllers();
app.Run();