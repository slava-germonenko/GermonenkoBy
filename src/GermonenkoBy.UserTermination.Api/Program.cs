using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Swashbuckle.AspNetCore.Annotations;

using GermonenkoBy.Common.Web.Extensions;
using GermonenkoBy.Common.Web.Middleware;
using GermonenkoBy.UserTermination.Core;
using GermonenkoBy.UserTermination.Core.Repositories;
using GermonenkoBy.UserTermination.Infrastructure.Clients;

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

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.EnableAnnotations();
    options.SwaggerDoc("v1", new()
    {
        Title = "User Termination Microservice"
    });
});

var usersServiceBaseAddress = builder.Configuration.GetValueUnsafe<string>("Routing:Http:UsersServiceUrl");
builder.Services.AddHttpClient<IUsersClient, UsersClient>(
    UsersClient.ClientName,
    options =>
    {
        options.BaseAddress = new Uri(usersServiceBaseAddress);
    }
);

var sessionsServiceBaseAddress = builder.Configuration.GetValueUnsafe<string>("Routing:Http:SessionsServiceUrl");
builder.Services.AddHttpClient<IUserSessionsClient, UserSessionsClient>(
    UserSessionsClient.ClientName,
    options =>
    {
        options.BaseAddress = new Uri(sessionsServiceBaseAddress);
    }
);

builder.Services.AddScoped<UserTerminationService>();

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
    [SwaggerOperation("Terminate user.", "Removes all user associated items and terminates a user.")]
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