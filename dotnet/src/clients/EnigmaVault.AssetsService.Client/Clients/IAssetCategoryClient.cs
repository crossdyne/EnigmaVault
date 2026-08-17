using Crossdyne.Toolkit.Results;
using EnigmaVault.AssetsService.Client.Models;

namespace EnigmaVault.AssetsService.Client.Clients
{
    public interface IAssetCategoryClient
    {
        Task<Result<List<IconCategoryResponse>>> GetIconCategories(CancellationToken cancellationToken = default);
    }
}