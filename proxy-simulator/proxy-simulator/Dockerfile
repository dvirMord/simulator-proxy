FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 5264
ENV ASPNETCORE_HTTP_PORTS=5264
ENV ASPNETCORE_ENVIRONMENT=Development

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["proxy-simulator.csproj", "./"]
RUN dotnet restore "./proxy-simulator.csproj"
COPY . .
RUN dotnet build "proxy-simulator.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "proxy-simulator.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "proxy-simulator.dll"]