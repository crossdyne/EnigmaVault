FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["backend/Directory.Packages.props", "./"]

COPY ["backend/src/services/Password/EnigmaVault.Password.Service.Api/EnigmaVault.Password.Service.Api.csproj", "backend/src/services/Password/EnigmaVault.Password.Service.Api/"]
COPY ["backend/src/services/Password/EnigmaVault.Password.Service.Application/EnigmaVault.Password.Service.Application.csproj", "backend/src/services/Password/EnigmaVault.Password.Service.Application/"]
COPY ["backend/src/services/Password/EnigmaVault.Password.Service.Infrastructure/EnigmaVault.Password.Service.Infrastructure.csproj", "backend/src/services/Password/EnigmaVault.Password.Service.Infrastructure/"]
COPY ["backend/src/services/Password/EnigmaVault.Password.Service.Domain/EnigmaVault.Password.Service.Domain.csproj", "backend/src/services/Password/EnigmaVault.Password.Service.Domain/"]

COPY ["backend/src/shared/core/Shared.Application/Shared.Application.csproj", "backend/src/shared/core/Shared.Application/"]
COPY ["backend/src/shared/core/Shared.Kernel/Shared.Kernel.csproj", "backend/src/shared/core/Shared.Kernel/"]
COPY ["backend/src/shared/observability/Shared.Logging/Shared.Logging.csproj", "backend/src/shared/observability/Shared.Logging/"]

RUN dotnet restore "backend/src/services/Password/EnigmaVault.Password.Service.Api/EnigmaVault.Password.Service.Api.csproj"

COPY . .

WORKDIR "/src/backend/src/services/Password/EnigmaVault.Password.Service.Api"
RUN dotnet publish -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
EXPOSE 80

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "EnigmaVault.Password.Service.Api.dll"]