using Crossdyne.Toolkit.Results;
using MediatR;

namespace EnigmaVault.PasswordService.Application.Features.VaultItems.Commands.UnArchive
{
    public sealed record UnArchiveVaultCommand(Guid VaultItemId, Guid UserId) : IRequest<Result>;
}