# Builds image based on DLLS previously built on host machine

FROM mcr.microsoft.com/dotnet/aspnet:7.0

EXPOSE 80
EXPOSE 443

ENV ASPNETCORE_URLS="http://+:80;http://+:443"
ENV Hosting__RestPort="80"
ENV Hosting__GrpcPort="443"

COPY ./bin/Release/net7.0 /app
WORKDIR /app

ENTRYPOINT ["dotnet", "GermonenkoBy.Users.Api.dll"]
