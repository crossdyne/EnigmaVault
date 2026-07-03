FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["dotnet/Directory.Packages.props", "./"]

COPY ["dotnet/src/apps/EnigmaVault.Web.Bff/EnigmaVault.Web.Bff.csproj", "dotnet/src/apps/EnigmaVault.Web.Bff/"]

COPY ["dotnet/src/clients/EnigmaVault.Authentication.ApiClient/EnigmaVault.Authentication.ApiClient.csproj", "dotnet/src/clients/EnigmaVault.Authentication.ApiClient/"]
COPY ["dotnet/src/shared/contracts/Shared.Contracts/Shared.Contracts.csproj", "dotnet/src/shared/contracts/Shared.Contracts/"]
COPY ["dotnet/src/shared/infrastructure/Shared.Redis/Shared.Redis.csproj", "dotnet/src/shared/infrastructure/Shared.Redis/"]

RUN dotnet restore "dotnet/src/apps/EnigmaVault.Web.Bff/EnigmaVault.Web.Bff.csproj"

COPY . .

WORKDIR "/src/dotnet/src/apps/EnigmaVault.Web.Bff"
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0  AS runtime
WORKDIR /app
EXPOSE 80

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "EnigmaVault.Web.Bff.dll"]