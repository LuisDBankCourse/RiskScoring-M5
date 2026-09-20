FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/RiskScoring.Api/RiskScoring.Api.csproj", "src/RiskScoring.Api/"]
COPY ["src/RiskScoring.Application/RiskScoring.Application.csproj", "src/RiskScoring.Application/"]
COPY ["src/RiskScoring.Domain/RiskScoring.Domain.csproj", "src/RiskScoring.Domain/"]
COPY ["src/RiskScoring.Infrastructure/RiskScoring.Infrastructure.csproj", "src/RiskScoring.Infrastructure/"]
RUN dotnet restore "src/RiskScoring.Api/RiskScoring.Api.csproj"
COPY . .
WORKDIR "/src/src/RiskScoring.Api"
RUN dotnet build "./RiskScoring.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./RiskScoring.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RiskScoring.Api.dll"]
