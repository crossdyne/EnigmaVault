using Crossdyne.Toolkit.Results;
using MediatR;
using Shared.Contracts.PasswordService.Responses;

namespace EnigmaVault.Password.Service.Application.Features.VaultItems.Queries.GetById
{
    public sealed record GetVaultByIdQuery(Guid Id, Guid UserId) : IRequest<Result<EncryptedVaultResponse>>;
}