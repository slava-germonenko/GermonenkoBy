using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.EntityFrameworkCore;

using GermonenkoBy.Authorization.Core;
using GermonenkoBy.Authorization.Core.Contracts;
using GermonenkoBy.Authorization.Core.Contracts.Clients;
using GermonenkoBy.Authorization.Core.Services;
using GermonenkoBy.Authorization.Infrastructure.Contracts;
using GermonenkoBy.Authorization.Infrastructure.Contracts.Clients;
using GermonenkoBy.Common.HostedServices;
using GermonenkoBy.Common.Web.Middleware;

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
        Title = "Authorization Microservice"
    });
});

var usersServiceUrl = builder.Configuration.GetValue<string>("Routing:UsersServiceUrl");
builder.Services.AddHttpClient<IUsersClient, HttpUsersClient>(options =>
{
    options.BaseAddress = new Uri(usersServiceUrl);
});

var sessionsServiceUrl = builder.Configuration.GetValue<string>("Routing:SessionsServiceUrl");
builder.Services.AddHttpClient<IUserSessionsClient, HttpUserSessionsClient>(options =>
{
    options.BaseAddress = new Uri(sessionsServiceUrl);
});

var coreConnectionString = builder.Configuration.GetValue<string>("CoreDatabaseConnectionString");
builder.Services.AddDbContext<AuthorizationContext>(options =>
{
    options.UseSqlServer(coreConnectionString);
});

builder.Services.AddScoped<DefaultUserAuthorizationService>();
builder.Services.AddScoped<IExpireDateGenerator, EndOfNextDayExpireDateGenerator>();
builder.Services.AddScoped<IRefreshTokenGenerator, RandomHexadecimalStringTokenGenerator>();

builder.Services.RegisterHostedService<RefreshTokensCleanupService>(TimeSpan.FromMinutes(10));

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
