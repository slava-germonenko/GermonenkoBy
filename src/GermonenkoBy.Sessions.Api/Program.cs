using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;

using GermonenkoBy.Common.HostedServices;
using GermonenkoBy.Common.Web.Http;
using GermonenkoBy.Common.Web.Middleware;
using GermonenkoBy.Sessions.Core;
using GermonenkoBy.Sessions.Core.Repositories;
using GermonenkoBy.Sessions.Core.Services;
using GermonenkoBy.Sessions.Infrastructure.Options;
using GermonenkoBy.Sessions.Infrastructure.Repositories;

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

builder.Services.Configure<RoutingOptions>(builder.Configuration.GetSection("Routing"));

builder.Services.AddControllers();
builder.Services.AddSwaggerGen(options =>
{
    options.EnableAnnotations();
    options.SwaggerDoc("v1", new()
    {
        Title = "Sessions Microservice"
    });
});

var connectionString = builder.Configuration.GetValue<string>("CoreDatabaseConnectionString");
builder.Services.AddDbContext<SessionsContext>(options =>
{
    options.UseSqlServer(connectionString);
});

builder.Services.AddHttpClient();
builder.Services.AddTransient<HttpClientFacade>();

builder.Services.AddScoped<UserSessionsSearchService>();
builder.Services.AddScoped<UserSessionsService>();
builder.Services.AddScoped<IUsersRepository, UsersRepository>();

builder.Services.RegisterHostedService<UserSessionsCleanupService>(TimeSpan.FromMinutes(5));

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