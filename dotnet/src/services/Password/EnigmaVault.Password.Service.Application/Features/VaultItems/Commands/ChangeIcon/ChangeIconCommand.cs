using Crossdyne.Toolkit.Results;
using MediatR;

namespace EnigmaVault.Password.Service.Application.Features.VaultItems.Commands.ChangeIcon
{
    public sealed record ChangeIconCommand(Guid UseId, Guid VaultId, Guid IconId) : IRequest<Result<DateTime>>;
}