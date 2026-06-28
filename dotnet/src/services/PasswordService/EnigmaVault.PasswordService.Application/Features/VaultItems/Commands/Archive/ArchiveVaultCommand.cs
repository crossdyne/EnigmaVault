using Crossdyne.Toolkit.Results;
using MediatR;

namespace EnigmaVault.PasswordService.Application.Features.VaultItems.Commands.Archive
{
    public sealed record ArchiveVaultCommand(Guid VaultItemId, Guid UserId) : IRequest<Result<Unit>>;
}