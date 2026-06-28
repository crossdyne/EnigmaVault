using Crossdyne.Toolkit.Results;
using EnigmaVault.PasswordService.Application.Features.Folders.Validators;
using EnigmaVault.PasswordService.Application.Features.Validators;
using MediatR;

namespace EnigmaVault.PasswordService.Application.Features.Folders.Commands.CreateSubFolder
{
    public sealed record CreateSubFolderCommand(Guid UserId, Guid ParentFolderId, string Name, string Color) : IRequest<Result<Unit>>,
        IHasName,
        IHasHexColor,
        IMustHasUserId;
}