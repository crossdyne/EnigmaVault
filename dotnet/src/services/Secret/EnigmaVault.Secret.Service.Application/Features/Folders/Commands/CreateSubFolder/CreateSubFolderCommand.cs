using Crossdyne.Toolkit.Results;
using EnigmaVault.Secret.Service.Application.Features.Folders.Validators;
using EnigmaVault.Secret.Service.Application.Features.Validators;
using MediatR;

namespace EnigmaVault.Secret.Service.Application.Features.Folders.Commands.CreateSubFolder
{
    public sealed record CreateSubFolderCommand(Guid UserId, Guid ParentFolderId, string Name, string Color) : IRequest<Result<Unit>>,
        IHasName,
        IHasHexColor,
        IMustHasUserId;
}