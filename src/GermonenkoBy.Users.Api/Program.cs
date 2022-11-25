using System.Net;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.Options;

using GermonenkoBy.Common.Utils.Hashing;
using GermonenkoBy.Common.Web.Extensions;
using GermonenkoBy.Common.Web.Middleware;
using GermonenkoBy.Users.Api.Grpc;
using GermonenkoBy.Users.Api.MapperProfiles;
using GermonenkoBy.Users.Api.Options;
using GermonenkoBy.Users.Api.Services;
using GermonenkoBy.Users.Core;
using GermonenkoBy.Users.Core.Contracts;
using UsersService = GermonenkoBy.Users.Core.UsersService;

var builder = WebApplication.CreateBuilder(args);

var restPort = builder.Configuration.GetValue<int>("Hosting:RestPort");
var grpcPort = builder.Configuration.GetValue<int>("Hosting:GrpcPort");
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(restPort);
    options.ListenAnyIP(grpcPort, opt =>
    {
        opt.Protocols = HttpProtocols.Http2;
    });
});

var appConfigConnectionString = builder.Configuration.GetValueUnsafe<string>("APP_CONFIG_CONNECTION_STRING");
if (!string.IsNullOrEmpty(appConfigConnectionString))
{
    builder.Configuration.AddAzureAppConfiguration(options =>
    {
        options.Connect(appConfigConnectionString)
            .Select(KeyFilter.Any)
            .Select(KeyFilter.Any, builder.Environment.EnvironmentName);
    });
}

builder.Services.AddGrpcReflection();
builder.Services.AddGrpc();
builder.Services.AddControllers();

builder.Services.AddSwaggerGen(options =>
{
    options.EnableAnnotations();
    options.SwaggerDoc("v1", new()
    {
        Title = "Users microservice"
    });
});

builder.Services.AddAutoMapper(options =>
{
    options.AddProfile<GrpcSearchUsersRequestProfile>();
    options.AddProfile<GrpcCreateUserRequestProfile>();
    options.AddProfile<GrpcUpdateUserRequestProfile>();
    options.AddProfile<GrpcUserResponseProfile>();
});

var connectionString = builder.Configuration.GetValueUnsafe<string>("CoreDatabaseConnectionString");
builder.Services.AddDbContext<UsersContext>(contextOptionsBuilder =>
{
    contextOptionsBuilder.UseSqlServer(connectionString);
});

builder.Services.Configure<SecurityOptions>(options =>
{
    // Simply set to default as there is no need to store in the app configuration
    // (at least now)
    options.MinPasswordLength = 8;
    options.DefaultSaltLenght = 32;
    options.PasswordHashBytesLenght = 32;
    options.PasswordHashIterationsCount = 10000;
});
builder.Services.AddScoped<PasswordValidationService>();
builder.Services.AddScoped<UsersSearchService>();
builder.Services.AddScoped<UsersService>();
builder.Services.AddScoped<IHasher, Pbkdf2Hasher>(services =>
{
    var options = services.GetRequiredService<IOptionsSnapshot<SecurityOptions>>();
    return new Pbkdf2Hasher(options.Value);
});

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddScoped<IPasswordPolicy, SimplePasswordPolicy>();
}
else
{
    builder.Services.AddScoped<IPasswordPolicy, StrongPasswordPolicy>();
}

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

app.MapGrpcReflectionService();
app.MapGrpcService<UsersGrpcService>();
app.UseMiddleware<ExceptionHandlingMiddleware>(errorHandlingBehaviour);
app.MapControllers();
app.Run();