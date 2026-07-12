using Crossdyne.Toolkit.Results;
using MediatR;

namespace EnigmaVault.Password.Service.Application.Features.VaultItems.Commands.Update
{
    public sealed record UpdateVaultItemCommand(Guid UserId, Guid VaultItemId, Guid IconId, string EncryptedOverview, string EncryptedDetails) : IRequest<Result<string>>;
}