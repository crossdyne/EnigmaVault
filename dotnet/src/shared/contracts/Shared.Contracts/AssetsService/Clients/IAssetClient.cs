using Crossdyne.Toolkit.Results;
using Shared.Contracts.AssetsService.Responses;

namespace Shared.Contracts.AssetsService.Clients
{
    public interface IAssetClient
    {
        Task<Result<List<IconMetadataResponse>>> GetFilesMetadata(CancellationToken cancellationToken = default);
    }
}