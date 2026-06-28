using Crossdyne.Toolkit.Results;
using MediatR;

namespace EnigmaVault.PasswordService.Application.Features.VaultItems.Commands.Update
{
    public sealed record UpdateVaultItemCommand(Guid UserId, Guid VaultItemId, Guid IconId, byte[] EncryptedOverview, byte[] EncryptedDetails) : IRequest<Result<string>>;
}