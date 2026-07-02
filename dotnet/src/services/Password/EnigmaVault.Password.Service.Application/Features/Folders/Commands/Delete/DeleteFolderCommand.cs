using Crossdyne.Toolkit.Results;
using EnigmaVault.Password.Service.Application.Features.Validators;
using MediatR;

namespace EnigmaVault.Password.Service.Application.Features.Folders.Commands.Delete
{
    public sealed record DeleteFolderCommand(Guid Id, Guid UserId) : IRequest<Result<Unit>>,
        IHasGuidId,
        IMustHasUserId;
}