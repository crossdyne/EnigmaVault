using EnigmaVault.PasswordService.Client.Clients;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.Requests.PasswordService;
using Shared.Web.Extensions;

namespace EnigmaVault.Web.Bff.Features.Vaults
{
    public static class VaultCommandEndpoints
    {
        public static void MapVaultCommandEndpoints(this IEndpointRouteBuilder builder)
        {
            builder.MapPost("api/v1/vault", async ([FromBody] CreateVaultItemRequest request, [FromServices] IVaultService service) 
                => await service.CreateAsync(request).MapErrorOrOkAsync()).RequireAuthorization();

            builder.MapPut("api/v1/vault", async ([FromBody] UpdateVaultItemRequest request, [FromServices] IVaultService service) 
                => await service.UpdateAsync(request).MapErrorOrOkAsync()).RequireAuthorization();
        }
    }
}