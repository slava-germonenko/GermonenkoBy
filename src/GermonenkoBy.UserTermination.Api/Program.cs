using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Swashbuckle.AspNetCore.Annotations;

using GermonenkoBy.Common.Web.Extensions;
using GermonenkoBy.Common.Web.Middleware;
using GermonenkoBy.UserTermination.Api;
using GermonenkoBy.UserTermination.Core;
using GermonenkoBy.UserTermination.Api.Extensions;

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

builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.EnableAnnotations();
    options.SwaggerDoc("v1", new()
    {
        Title = "User Termination Microservice"
    });
});

builder.Services.RegisterHttpClients(builder.Configuration);
builder.Services.RegisterGrpcClients(builder.Configuration);

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

app.MapGrpcReflectionService();
app.MapGrpcService<GrpcUserTerminationService>();

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