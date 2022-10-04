using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;

using GermonenkoBy.Common.Web.Middleware;
using GermonenkoBy.Products.Core;
using GermonenkoBy.Products.Core.Contracts;
using GermonenkoBy.Products.Infrastructure.Contracts;

var builder = WebApplication.CreateBuilder(args);

var appConfigConnectionString = builder.Configuration.GetValue<string>("AppConfigConnectionString");
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

var connectionString = builder.Configuration.GetValue<string>("CoreDatabaseConnectionString");
builder.Services.AddDbContext<ProductsContext>(contextOptionsBuilder =>
{
    contextOptionsBuilder.UseSqlServer(connectionString);
});

builder.Services.AddScoped<IBulkCategoriesRepository, BulkCategoriesRepository>();
builder.Services.AddScoped<IBulkMaterialsRepository, BulkMaterialsRepository>();
builder.Services.AddScoped<CategoriesSearchService>();
builder.Services.AddScoped<CategoriesService>();
builder.Services.AddScoped<MaterialsSearchService>();
builder.Services.AddScoped<MaterialsService>();
builder.Services.AddScoped<ProductsSearchService>();
builder.Services.AddScoped<ProductsService>();

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