FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base

USER $APP_UID
WORKDIR /app

ENV ASPNETCORE_HTTP_PORTS=5000
EXPOSE 5000


FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

ARG BUILD_CONFIGURATION=Release

WORKDIR /src

COPY ["src/GarageLog.API/GarageLog.API.csproj", "src/GarageLog.API/"]
COPY ["src/GarageLog.Application/GarageLog.Application.csproj", "src/GarageLog.Application/"]
COPY ["src/GarageLog.Core/GarageLog.Core.csproj", "src/GarageLog.Core/"]
COPY ["src/GarageLog.Infrastructure/GarageLog.Infrastructure.csproj", "src/GarageLog.Infrastructure/"]

RUN dotnet restore "src/GarageLog.API/GarageLog.API.csproj"

COPY . .

WORKDIR "/src/src/GarageLog.API"

RUN dotnet build "./GarageLog.API.csproj" -c $BUILD_CONFIGURATION -o /app/build


FROM build AS publish

ARG BUILD_CONFIGURATION=Release

RUN dotnet publish "./GarageLog.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false


FROM base AS final

WORKDIR /app

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "GarageLog.API.dll"]
