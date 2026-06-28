using Crossdyne.Toolkit.Results;
using MediatR;

namespace EnigmaVault.PasswordService.Application.Features.VaultItems.Commands.Create
{
    public sealed record CreateVaultItemCommand(Guid UserId, string PasswordType, Guid IconId, byte[] EncryptedOverview, byte[] EncryptedDetails) : IRequest<Result<string>>;
}