FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["TicketWaveAz.Api/TicketWaveAz.Api.csproj", "TicketWaveAz.Api/"]
COPY ["TicketWaveAz.Domain/TicketWaveAz.Domain.csproj", "TicketWaveAz.Domain/"]
COPY ["TicketWaveAz.Shared/TicketWaveAz.Shared.csproj", "TicketWaveAz.Shared/"]
COPY ["TicketWaveAz.Infrastructure/TicketWaveAz.Infrastructure.csproj", "TicketWaveAz.Infrastructure/"]
COPY ["TicketWaveAz.Application/TicketWaveAz.Application.csproj", "TicketWaveAz.Application/"]

RUN dotnet restore "./TicketWaveAz.Api/TicketWaveAz.Api.csproj"
COPY . .
WORKDIR "/src/TicketWaveAz.Api"
RUN dotnet build "./TicketWaveAz.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TicketWaveAz.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TicketWaveAz.Api.dll"]