using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.Clients.AssetsService;
using Shared.Web.Extensions;

namespace EnigmaVault.Web.Bff.Features.IconCategories
{
    public static class IconCategoryQueryEndpoints
    {
        public static void MapIconCategoryEndpoints(this IEndpointRouteBuilder builder)
        {
            builder.MapGet("api/v1/icon/category", async ([FromServices] IAssetCategoryClient client) 
                => await client.GetIconCategories().MapErrorOrOkAsync()).RequireAuthorization();
        }
    }
}