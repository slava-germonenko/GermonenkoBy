using System.Text;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.FeatureManagement;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;

using GermonenkoBy.Common.Web.Extensions;
using GermonenkoBy.Common.Web.Middleware;
using GermonenkoBy.Gateway.Api.Extensions;
using GermonenkoBy.Gateway.Api.Mapping.Profiles.Contacts;
using GermonenkoBy.Gateway.Api.Mapping.Profiles.Users;
using GermonenkoBy.Gateway.Api.Options;
using GermonenkoBy.Gateway.Api.Services;

var builder = WebApplication.CreateBuilder(args);

var appConfigConnectionString = builder.Configuration.GetValueUnsafe<string>("APP_CONFIG_CONNECTION_STRING");
if (!string.IsNullOrEmpty(appConfigConnectionString))
{
    builder.Configuration.AddAzureAppConfiguration(options =>
    {
        options.Connect(appConfigConnectionString)
            .Select(KeyFilter.Any)
            .Select(KeyFilter.Any, builder.Environment.EnvironmentName);
    });
    builder.Services.AddFeatureManagement();
}

builder.Services.Configure<SecurityOptions>(builder.Configuration.GetSection("Security"));

var jwtSecret = builder.Configuration.GetValueUnsafe<string>("Security:JwtSecret");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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
        Version = "0.1.0",
    });

    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        BearerFormat = "JWT",
        Name = "JWT Authentication",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        Description = "Put ",

        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    options.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtSecurityScheme, Array.Empty<string>() }
    });
});

builder.Services.AddAutoMapper(options =>
{
    options.AddProfile<GrpcCreateUserRequestProfile>();
    options.AddProfile<GrpcUpdateUserRequestProfile>();
    options.AddProfile<GrpcUserFilterProfile>();
    options.AddProfile<GrpcUserResponseProfile>();

    options.AddProfile<CreateContactDtoProfile>();
});

builder.Services.RegisterHttpClients(builder.Configuration);
builder.Services.RegisterGrpcClients(builder.Configuration);

builder.Services.AddScoped<AccessTokenService>();
builder.Services.AddScoped<UserAuthService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.DocExpansion(DocExpansion.None);
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = "swagger";
});

var errorHandlingBehaviour = app.Environment.IsDevelopment()
    ? ExceptionHandlingBehavior.RethrowDetailedError
    : ExceptionHandlingBehavior.RethrowGenericError;

app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<ExceptionHandlingMiddleware>(errorHandlingBehaviour);
app.MapControllers();
app.Run();