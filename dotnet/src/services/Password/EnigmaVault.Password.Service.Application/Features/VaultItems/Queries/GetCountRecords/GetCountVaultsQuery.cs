using Crossdyne.Toolkit.Results;
using MediatR;

namespace EnigmaVault.Password.Service.Application.Features.VaultItems.Queries.GetCountRecords
{
    public sealed record GetCountVaultsQuery(Guid UserId) : IRequest<Result<int>>;
}