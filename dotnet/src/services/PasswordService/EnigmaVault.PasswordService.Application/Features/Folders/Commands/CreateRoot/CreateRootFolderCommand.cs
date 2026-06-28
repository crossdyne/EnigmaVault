using Crossdyne.Toolkit.Results;
using EnigmaVault.PasswordService.Application.Features.Folders.Validators;
using EnigmaVault.PasswordService.Application.Features.Validators;
using MediatR;

namespace EnigmaVault.PasswordService.Application.Features.Folders.Commands.CreateRoot
{
    public sealed record CreateRootFolderCommand(Guid UserId, string Name, string Color) : IRequest<Result<Unit>>,
        IMustHasUserId,
        IHasName,
        IHasHexColor;
}