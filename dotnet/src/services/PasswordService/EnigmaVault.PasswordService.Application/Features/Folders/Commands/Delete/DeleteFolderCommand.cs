using Crossdyne.Toolkit.Results;
using EnigmaVault.PasswordService.Application.Features.Validators;
using MediatR;

namespace EnigmaVault.PasswordService.Application.Features.Folders.Commands.Delete
{
    public sealed record DeleteFolderCommand(Guid Id, Guid UserId) : IRequest<Result<Unit>>,
        IHasGuidId,
        IMustHasUserId;
}