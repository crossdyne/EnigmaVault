using Crossdyne.Toolkit.Results;
using MediatR;

namespace EnigmaVault.Secret.Service.Application.Features.VaultItems.Commands.UpdateTags
{
    public sealed record UpdateTagsCommand(Guid UserId, Guid VaultId, List<Guid> TagIds) : IRequest<Result<DateTime>>;
}