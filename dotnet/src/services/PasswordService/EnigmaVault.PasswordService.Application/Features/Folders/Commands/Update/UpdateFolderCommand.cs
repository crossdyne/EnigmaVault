using Crossdyne.Toolkit.Results;
using EnigmaVault.PasswordService.Application.Features.Validators;
using MediatR;

namespace EnigmaVault.PasswordService.Application.Features.Folders.Commands.Update
{
    public sealed record UpdateFolderCommand(Guid Id, Guid UserId, Guid? ParentFolderId, string Name) : IRequest<Result<Unit>>,
        IHasGuidId,
        IMustHasUserId,
        IHasName;
}