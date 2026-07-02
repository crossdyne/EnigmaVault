FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["dotnet/Directory.Packages.props", "./"]

COPY ["dotnet/src/services/Password/EnigmaVault.Password.Service.Api/EnigmaVault.Password.Service.Api.csproj", "dotnet/src/services/Password/EnigmaVault.Password.Service.Api/"]
COPY ["dotnet/src/services/Password/EnigmaVault.Password.Service.Application/EnigmaVault.Password.Service.Application.csproj", "dotnet/src/services/Password/EnigmaVault.Password.Service.Application/"]
COPY ["dotnet/src/services/Password/EnigmaVault.Password.Service.Infrastructure/EnigmaVault.Password.Service.Infrastructure.csproj", "dotnet/src/services/Password/EnigmaVault.Password.Service.Infrastructure/"]
COPY ["dotnet/src/services/Password/EnigmaVault.Password.Service.Domain/EnigmaVault.Password.Service.Domain.csproj", "dotnet/src/services/Password/EnigmaVault.Password.Service.Domain/"]

COPY ["dotnet/src/shared/core/Shared.Application/Shared.Application.csproj", "dotnet/src/shared/core/Shared.Application/"]
COPY ["dotnet/src/shared/core/Shared.Kernel/Shared.Kernel.csproj", "dotnet/src/shared/core/Shared.Kernel/"]
COPY ["dotnet/src/shared/observability/Shared.Logging/Shared.Logging.csproj", "dotnet/src/shared/observability/Shared.Logging/"]

RUN dotnet restore "dotnet/src/services/Password/EnigmaVault.Password.Service.Api/EnigmaVault.Password.Service.Api.csproj"

COPY . .

WORKDIR "/src/dotnet/src/services/Password/EnigmaVault.Password.Service.Api"
RUN dotnet publish -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
EXPOSE 80

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "EnigmaVault.Password.Service.Api.dll"]