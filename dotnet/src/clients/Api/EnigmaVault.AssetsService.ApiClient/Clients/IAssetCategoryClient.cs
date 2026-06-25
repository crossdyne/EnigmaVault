using Common.Core.Results;
using EnigmaVault.AssetsService.ApiClient.Models;

namespace EnigmaVault.AssetsService.ApiClient.Clients
{
    public interface IAssetCategoryClient
    {
        Task<Result<List<IconCategoryResponse>>> GetIconCategories();
    }
}