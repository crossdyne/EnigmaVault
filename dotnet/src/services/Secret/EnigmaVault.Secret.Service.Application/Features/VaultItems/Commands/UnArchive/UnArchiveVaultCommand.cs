using Crossdyne.Toolkit.Results;
using MediatR;

namespace EnigmaVault.Secret.Service.Application.Features.VaultItems.Commands.UnArchive
{
    public sealed record UnArchiveVaultCommand(Guid VaultItemId, Guid UserId) : IRequest<Result>;
}