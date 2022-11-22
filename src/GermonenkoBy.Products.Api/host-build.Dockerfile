# Builds image based on DLLS previously built on host machine

FROM mcr.microsoft.com/dotnet/aspnet:7.0
COPY ./bin/Release/net7.0 /app
WORKDIR /app
ENTRYPOINT ["dotnet", "GermonenkoBy.Products.Api.dll"]
