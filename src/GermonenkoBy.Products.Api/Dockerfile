FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/GermonenkoBy.Products.Api/GermonenkoBy.Products.Api.csproj", "GermonenkoBy.Products.Api/"]
RUN dotnet restore "src/GermonenkoBy.Products.Api/GermonenkoBy.Products.Api.csproj"
COPY . .
WORKDIR "/src/GermonenkoBy.Products.Api"
RUN dotnet build "GermonenkoBy.Products.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GermonenkoBy.Products.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GermonenkoBy.Products.Api.dll"]
