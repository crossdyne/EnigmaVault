using Crossdyne.Toolkit.Primitives;
using Crossdyne.Toolkit.Results;
using Shared.Contracts.Requests.PasswordService;
using Shared.Contracts.Responses.PasswordService;

namespace EnigmaVault.PasswordService.Client.Clients
{
    public interface IVaultService
    {
        Task<Result<string>> CreateAsync(CreateVaultItemRequest request, CancellationToken cancellationToken = default);
        Task<Result<string>> UpdateAsync(UpdateVaultItemRequest request, CancellationToken cancellationToken = default);
        Task<Result<Unit>> AddToFavoritesAsync(string vaultId, CancellationToken cancellationToken = default);
        Task<Result<Unit>> RemoveFromFavoritesAsync(string vaultId, CancellationToken cancellationToken = default);
        Task<Result<Unit>> ArchiveAsync(string vaultId, CancellationToken cancellationToken = default);
        Task<Result<Unit>> UnArchiveAsync(string vaultId, CancellationToken cancellationToken = default);
        Task<Result<Unit>> RestoreAllFromArchiveAsync(CancellationToken cancellationToken = default);
        Task<Result<Unit>> DeleteAsync(string vaultId, CancellationToken cancellationToken = default);
        Task<Result<DateTime>> MoveToTrashAsync(string vaultId, CancellationToken cancellationToken = default);
        Task<Result<Unit>> RestoreFromTrashAsync(string vaultId, CancellationToken cancellationToken = default);
        Task<Result<Unit>> RestoreAllFromTrashAsync(CancellationToken cancellationToken = default);
        Task<Result<Unit>> EmptyTrashAsync(CancellationToken cancellationToken = default);
        Task<Result<List<EncryptedVaultResponse>>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Result<EncryptedVaultResponse>> GetById(string id, CancellationToken cancellationToken = default);
        Task<Result<Unit>> AddTagAsync(string vaultId, string tagId, CancellationToken cancellationToken = default);
        Task<Result<Unit>> RemoveTagAsync(string vaultId, string tagId, CancellationToken cancellationToken = default);
        Task<Result<DateUpdateResponse>> ChangeIcon(string vaultId, string iconId, CancellationToken cancellationToken = default);
        Task<Result<DateUpdateResponse>> UpdateTagsAsync(string vaultId, UpdateTagsRequest detachTagsRequest, CancellationToken cancellationToken = default);
        Task<Result<PasswordsCountRecordsResponse>> CountRecordsAsync(CancellationToken cancellationToken = default);
    }
}