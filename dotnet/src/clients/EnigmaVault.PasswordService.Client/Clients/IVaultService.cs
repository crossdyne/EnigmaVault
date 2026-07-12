using Crossdyne.Toolkit.Primitives;
using Crossdyne.Toolkit.Results;
using Shared.Contracts.Requests.PasswordService;
using Shared.Contracts.Responses.PasswordService;

namespace EnigmaVault.PasswordService.Client.Clients
{
    public interface IVaultService
    {
        Task<Result<string>> CreateAsync(CreateVaultItemRequest request);
        Task<Result<string>> UpdateAsync(UpdateVaultItemRequest request);
        Task<Result<Unit>> AddToFavoritesAsync(string vaultId);
        Task<Result<Unit>> RemoveFromFavoritesAsync(string vaultId);
        Task<Result<Unit>> ArchiveAsync(string vaultId);
        Task<Result<Unit>> UnArchiveAsync(string vaultId);
        Task<Result<Unit>> RestoreAllFromArchiveAsync();
        Task<Result<Unit>> DeleteAsync(string vaultId);
        Task<Result<DateTime>> MoveToTrashAsync(string vaultId);
        Task<Result<Unit>> RestoreFromTrashAsync(string vaultId);
        Task<Result<Unit>> RestoreAllFromTrashAsync();
        Task<Result<Unit>> EmptyTrashAsync();
        Task<Result<List<EncryptedVaultResponse>>> GetAllAsync();
        Task<Result<EncryptedVaultResponse>> GetById(string id);
        Task<Result<Unit>> AddTagAsync(string vaultId, string tagId);
        Task<Result<Unit>> RemoveTagAsync(string vaultId, string tagId);
    }
}