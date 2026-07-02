FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["backend/Directory.Packages.props", "./"]

COPY ["backend/src/apps/EnigmaVault.Web.Bff/EnigmaVault.Web.Bff.csproj", "backend/src/apps/EnigmaVault.Web.Bff/"]

COPY ["backend/src/clients/EnigmaVault.Authentication.ApiClient/EnigmaVault.Authentication.ApiClient.csproj", "backend/src/clients/EnigmaVault.Authentication.ApiClient/"]
COPY ["backend/src/shared/contracts/Shared.Contracts/Shared.Contracts.csproj", "backend/src/shared/contracts/Shared.Contracts/"]
COPY ["backend/src/shared/infrastructure/Shared.Redis/Shared.Redis.csproj", "backend/src/shared/infrastructure/Shared.Redis/"]

RUN dotnet restore "backend/src/apps/EnigmaVault.Web.Bff/EnigmaVault.Web.Bff.csproj"

COPY . .

WORKDIR "/src/backend/src/apps/EnigmaVault.Web.Bff"
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0  AS runtime
WORKDIR /app
EXPOSE 80

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "EnigmaVault.Web.Bff.dll"]