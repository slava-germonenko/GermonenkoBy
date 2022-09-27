FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/GermonenkoBy.Users.Api/GermonenkoBy.Users.Api.csproj", "GermonenkoBy.Users.Api/"]
RUN dotnet restore "src/GermonenkoBy.Users.Api/GermonenkoBy.Users.Api.csproj"
COPY . .
WORKDIR "/src/GermonenkoBy.Users.Api"
RUN dotnet build "GermonenkoBy.Users.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GermonenkoBy.Users.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GermonenkoBy.Users.Api.dll"]
