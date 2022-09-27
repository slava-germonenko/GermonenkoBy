using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

using GermonenkoBy.Common.Utils.Hashing;
using GermonenkoBy.Common.Web.Middleware;
using GermonenkoBy.Users.Api.Options;
using GermonenkoBy.Users.Api.Services;
using GermonenkoBy.Users.Core;
using GermonenkoBy.Users.Core.Contracts;

var builder = WebApplication.CreateBuilder(args);

var appConfigConnectionString = builder.Configuration.GetValue<string>("AppConfigConnectionString");
if (!string.IsNullOrEmpty(appConfigConnectionString))
{
    builder.Configuration.AddAzureAppConfiguration(appConfigConnectionString);
}

builder.Services.AddControllers();
builder.Services.AddSwaggerGen(options =>
{
    options.EnableAnnotations();
    options.SwaggerDoc("v1", new()
    {
        Title = "Users microservice"
    });
});

var connectionString = builder.Configuration.GetValue<string>("CoreDatabaseConnectionString");
builder.Services.AddDbContext<UsersContext>(contextOptionsBuilder =>
{
    contextOptionsBuilder.UseSqlServer(connectionString);
});

builder.Services.Configure<SecurityOptions>(
    builder.Configuration.GetSection("Security")
);
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

app.UseMiddleware<ExceptionHandlingMiddleware>(errorHandlingBehaviour);
app.MapControllers();
app.Run();