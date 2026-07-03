using Crossdyne.Toolkit.Results;
using EnigmaVault.Password.Service.Application.Features.Validators;
using MediatR;

namespace EnigmaVault.Password.Service.Application.Features.Folders.Commands.Update
{
    public sealed record UpdateFolderCommand(Guid Id, Guid UserId, Guid? ParentFolderId, string Name) : IRequest<Result<Unit>>,
        IHasGuidId,
        IMustHasUserId,
        IHasName;
}