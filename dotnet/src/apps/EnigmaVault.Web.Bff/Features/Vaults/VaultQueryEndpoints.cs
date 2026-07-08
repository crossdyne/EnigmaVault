using EnigmaVault.PasswordService.Client.Clients;
using Microsoft.AspNetCore.Mvc;
using Shared.Web.Extensions;

namespace EnigmaVault.Web.Bff.Features.Vaults
{
    public static class VaultQueryEndpoints
    {
        public static void MapVaultQueryEndpoints(this IEndpointRouteBuilder builder)
        {
            builder.MapGet("api/v1/vault", async ([FromServices] IVaultService service) 
                => await service.GetAllAsync().MapErrorOrOkAsync()).RequireAuthorization();
        }
    }
}