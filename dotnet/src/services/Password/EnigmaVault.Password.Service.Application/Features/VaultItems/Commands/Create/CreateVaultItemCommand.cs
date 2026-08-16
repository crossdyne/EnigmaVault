using Crossdyne.Toolkit.Results;
using MediatR;

namespace EnigmaVault.Password.Service.Application.Features.VaultItems.Commands.Create
{
    public sealed record CreateVaultItemCommand(Guid UserId, int PasswordType, Guid IconId, string EncryptedOverview, string EncryptedDetails, int CryptoVersion) : IRequest<Result<string>>;
}