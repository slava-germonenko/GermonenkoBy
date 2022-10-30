# Builds image based on DLLS previously built on host machine

FROM mcr.microsoft.com/dotnet/aspnet:6.0
COPY ./bin/Release/net6.0 /app
WORKDIR /app
ENTRYPOINT ["dotnet", "GermonenkoBy.Users.Api.dll"]
