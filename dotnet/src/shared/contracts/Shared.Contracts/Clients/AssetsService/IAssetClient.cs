using Crossdyne.Toolkit.Results;
using Shared.Contracts.Responses.Assets;

namespace Shared.Contracts.Clients.AssetsService
{
    public interface IAssetClient
    {
        Task<Result<List<IconMetadataResponse>>> GetFilesMetadata(CancellationToken cancellationToken = default);
    }
}