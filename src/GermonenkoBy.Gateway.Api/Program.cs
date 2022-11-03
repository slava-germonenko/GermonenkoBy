using System.Text;

using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.IdentityModel.Tokens;

using GermonenkoBy.Common.Web.Middleware;
using GermonenkoBy.Gateway.Api.Contracts.Clients;
using GermonenkoBy.Gateway.Api.Options;
using GermonenkoBy.Gateway.Api.Services;

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

builder.Services.Configure<SecurityOptions>(builder.Configuration.GetSection("Security"));

var jwtSecret = builder.Configuration.GetValue<string>("Security:JwtSecret");
builder.Services.AddAuthentication()
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            RequireExpirationTime = true,
            ClockSkew = TimeSpan.FromSeconds(5),
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
        };
    });

builder.Services.AddControllers();
builder.Services.AddSwaggerGen(options =>
{
    options.EnableAnnotations();
    options.SwaggerDoc("v1", new()
    {
        Title = "Web API Gateway",
        Description = "Web API gateway that is used by the desktop web app.",
        Version = "0.1.0"
    });
});

var authServiceUrl = builder.Configuration.GetValue<string>("Routing:AuthorizationServiceUrl");
builder.Services.AddHttpClient<IAuthClient, HttpAuthClient>(options =>
{
    options.BaseAddress = new Uri(authServiceUrl);
});

var usersServiceUrl = builder.Configuration.GetValue<string>("Routing:UsersServiceUrl");
builder.Services.AddHttpClient<IUsersClient, HttpUsersClient>(options =>
{
    options.BaseAddress = new Uri(usersServiceUrl);
});

var sessionServiceUrl = builder.Configuration.GetValue<string>("Routing:SessionsServiceUrl");
builder.Services.AddHttpClient<IUserSessionsClient, HttpUserSessionsClient>(options =>
{
    options.BaseAddress = new Uri(sessionServiceUrl);
});

builder.Services.AddScoped<AccessTokenService>();
builder.Services.AddScoped<UserAuthService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
});

var errorHandlingBehaviour = app.Environment.IsDevelopment()
    ? ExceptionHandlingBehavior.RethrowDetailedError
    : ExceptionHandlingBehavior.RethrowGenericError;

app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<ExceptionHandlingMiddleware>(errorHandlingBehaviour);
app.MapControllers();
app.Run();