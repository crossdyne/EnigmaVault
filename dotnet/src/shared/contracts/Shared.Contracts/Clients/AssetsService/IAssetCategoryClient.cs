using Crossdyne.Toolkit.Results;
using Shared.Contracts.Responses.Assets;

namespace Shared.Contracts.Clients.AssetsService
{
    public interface IAssetCategoryClient
    {
        Task<Result<List<IconCategoryResponse>>> GetIconCategories(CancellationToken cancellationToken = default);
    }
}