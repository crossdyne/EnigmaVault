FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["dotnet/Directory.Packages.props", "./"]

COPY ["dotnet/src/services/Secret/EnigmaVault.Secret.Service.Api/EnigmaVault.Secret.Service.Api.csproj", "dotnet/src/services/Secret/EnigmaVault.Secret.Service.Api/"]
COPY ["dotnet/src/services/Secret/EnigmaVault.Secret.Service.Application/EnigmaVault.Secret.Service.Application.csproj", "dotnet/src/services/Secret/EnigmaVault.Secret.Service.Application/"]
COPY ["dotnet/src/services/Secret/EnigmaVault.Secret.Service.Infrastructure/EnigmaVault.Secret.Service.Infrastructure.csproj", "dotnet/src/services/Secret/EnigmaVault.Secret.Service.Infrastructure/"]
COPY ["dotnet/src/services/Secret/EnigmaVault.Secret.Service.Domain/EnigmaVault.Secret.Service.Domain.csproj", "dotnet/src/services/Secret/EnigmaVault.Secret.Service.Domain/"]

COPY ["dotnet/src/shared/infrastructure/Shared.Messaging/Shared.Messaging.csproj", "dotnet/src/shared/infrastructure/Shared.Messaging/"]
COPY ["dotnet/src/shared/contracts/Shared.Contracts/Shared.Contracts.csproj", "dotnet/src/shared/contracts/Shared.Contracts/"]
COPY ["dotnet/src/shared/core/Shared.Application/Shared.Application.csproj", "dotnet/src/shared/core/Shared.Application/"]
COPY ["dotnet/src/shared/core/Shared.Kernel/Shared.Kernel.csproj", "dotnet/src/shared/core/Shared.Kernel/"]
COPY ["dotnet/src/shared/observability/Shared.Logging/Shared.Logging.csproj", "dotnet/src/shared/observability/Shared.Logging/"]

RUN dotnet restore "dotnet/src/services/Secret/EnigmaVault.Secret.Service.Api/EnigmaVault.Secret.Service.Api.csproj"

COPY . .

WORKDIR "/src/dotnet/src/services/Secret/EnigmaVault.Secret.Service.Api"
RUN dotnet publish -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
EXPOSE 80

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "EnigmaVault.Secret.Service.Api.dll"]