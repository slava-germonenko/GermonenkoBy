FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/GermonenkoBy.Authorization.Api/GermonenkoBy.Authorization.Api.csproj", "GermonenkoBy.Authorization.Api/"]
RUN dotnet restore "src/GermonenkoBy.Authorization.Api/GermonenkoBy.Authorization.Api.csproj"
COPY . .
WORKDIR "/src/GermonenkoBy.Authorization.Api"
RUN dotnet build "GermonenkoBy.Authorization.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GermonenkoBy.Authorization.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GermonenkoBy.Authorization.Api.dll"]
