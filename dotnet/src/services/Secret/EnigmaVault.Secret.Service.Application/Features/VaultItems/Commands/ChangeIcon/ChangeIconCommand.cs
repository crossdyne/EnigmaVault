using Crossdyne.Toolkit.Results;
using MediatR;

namespace EnigmaVault.Secret.Service.Application.Features.VaultItems.Commands.ChangeIcon
{
    public sealed record ChangeIconCommand(Guid UseId, Guid VaultId, Guid IconId) : IRequest<Result<DateTime>>;
}