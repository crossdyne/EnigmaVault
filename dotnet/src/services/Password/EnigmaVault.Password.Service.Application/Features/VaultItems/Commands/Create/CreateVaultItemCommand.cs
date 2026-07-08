using Crossdyne.Toolkit.Results;
using MediatR;

namespace EnigmaVault.Password.Service.Application.Features.VaultItems.Commands.Create
{
    public sealed record CreateVaultItemCommand(Guid UserId, string PasswordType, Guid IconId, string EncryptedOverview, string EncryptedDetails) : IRequest<Result<string>>;
}