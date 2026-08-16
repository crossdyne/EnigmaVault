using Crossdyne.Toolkit.Results;
using EnigmaVault.AssetsService.Client.Models;

namespace EnigmaVault.AssetsService.Client.Clients
{
    public interface IAssetClient
    {
        Task<Result<List<IconMetadataResponse>>> GetFilesMetadata();
    }
}