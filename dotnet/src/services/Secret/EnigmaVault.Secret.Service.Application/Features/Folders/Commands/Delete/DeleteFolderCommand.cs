using Crossdyne.Toolkit.Results;
using EnigmaVault.Secret.Service.Application.Features.Validators;
using MediatR;

namespace EnigmaVault.Secret.Service.Application.Features.Folders.Commands.Delete
{
    public sealed record DeleteFolderCommand(Guid Id, Guid UserId) : IRequest<Result<Unit>>,
        IHasGuidId,
        IMustHasUserId;
}