using Crossdyne.Toolkit.Results;
using MediatR;
using Shared.Contracts.PasswordService.Responses;

namespace EnigmaVault.Password.Service.Application.Features.VaultItems.Queries.GetAll
{
    public sealed record GetAllVaultsQuery(Guid UserId) : IRequest<Result<List<EncryptedVaultResponse>>>;
}