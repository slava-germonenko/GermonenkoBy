using Grpc.Core;

using Microsoft.Extensions.Configuration.AzureAppConfiguration;

using GermonenkoBy.Common.Web.Extensions;
using GermonenkoBy.Contacts.Api.Mapping.Profiles;
using GermonenkoBy.Contacts.Api.Services;
using GermonenkoBy.Contacts.Core.Contracts;
using GermonenkoBy.Contacts.Core.Services;
using GermonenkoBy.Contacts.Infrastructure.Clients;
using GermonenkoBy.Contacts.Infrastructure.Clients.Grpc;
using GermonenkoBy.Contacts.Infrastructure.Repos;
using GermonenkoBy.Users.Api.Grpc;

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
}

builder.Services.AddGrpc();
builder.Services.AddAutoMapper(options =>
{
    options.AddProfile<ContactEmailResponseProfile>();
    options.AddProfile<ContactResponseProfile>();
});
builder.Services.AddGrpcClient<UsersService.UsersServiceClient>(options =>
{
    var usersServiceUrl = builder.Configuration.GetValueUnsafe<string>("Routing:Grpc:UsersServiceUrl");
    options.Address = new Uri(usersServiceUrl);
    options.ChannelOptionsActions.Add(o =>
    {
        o.Credentials = ChannelCredentials.Insecure;
    });
});

builder.Services.AddScoped<ContactsService>();
builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<IContactsRepository, ContactsRepository>();
builder.Services.AddScoped<IUsersClient, GrpcUsersClient>();

var app = builder.Build();

app.MapGrpcService<ContactsGrpcService>();
app.Run();