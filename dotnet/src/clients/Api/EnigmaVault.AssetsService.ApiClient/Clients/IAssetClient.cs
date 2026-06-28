using Crossdyne.Toolkit.Results;
using EnigmaVault.AssetsService.ApiClient.Models;

namespace EnigmaVault.AssetsService.ApiClient.Clients
{
    public interface IAssetClient
    {
        Task<Result<List<IconMetadataResponse>>> GetFilesMetadata();
    }
}