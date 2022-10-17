using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Swashbuckle.AspNetCore.Annotations;

using GermonenkoBy.Common.Web.Http;
using GermonenkoBy.Common.Web.Middleware;
using GermonenkoBy.UserTermination.Core;
using GermonenkoBy.UserTermination.Core.Repositories;
using GermonenkoBy.UserTermination.Infrastructure.Options;
using GermonenkoBy.UserTermination.Infrastructure.Repositories;

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

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.EnableAnnotations();
    options.SwaggerDoc("v1", new()
    {
        Title = "User Termination Microservice"
    });
});

builder.Services.Configure<RoutingOptions>(builder.Configuration.GetSection("Routing"));
builder.Services.AddHttpClient();
builder.Services.AddTransient<HttpClientFacade>();

builder.Services.AddScoped<UserTerminationService>();
builder.Services.AddScoped<IUserSessionsRepository, UserSessionsRepository>();
builder.Services.AddScoped<IUsersRepository, UsersRepository>();

var app = builder.Build();

var errorHandlingBehaviour = app.Environment.IsDevelopment()
    ? ExceptionHandlingBehavior.RethrowDetailedError
    : ExceptionHandlingBehavior.RethrowGenericError;

app.UseMiddleware<ExceptionHandlingMiddleware>(errorHandlingBehaviour);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}

app.MapDelete("api/user/{userId:int}",
    [SwaggerOperation("Terminates User with The given ID")]
    [SwaggerResponse(204, "No Content Success Response")]
    async (
        [FromRoute, SwaggerParameter("ID of a user to be terminated.")] int userId,
        [FromServices] UserTerminationService userTerminationService
    ) =>
    {
        await userTerminationService.TerminateAsync(userId);
        return Results.NoContent();
    }
);

app.Run();