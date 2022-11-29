# Builds image based on DLLS previously built on host machine

FROM mcr.microsoft.com/dotnet/aspnet:7.0

EXPOSE 443

ENV ASPNETCORE_URLS="http://+:443"

COPY ./bin/Release/net7.0 /app
WORKDIR /app

ENTRYPOINT ["dotnet", "GermonenkoBy.Contacts.Api.dll"]
