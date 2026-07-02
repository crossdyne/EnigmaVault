using Crossdyne.Toolkit.Results;
using EnigmaVault.Password.Service.Application.Features.Folders.Validators;
using EnigmaVault.Password.Service.Application.Features.Validators;
using MediatR;

namespace EnigmaVault.Password.Service.Application.Features.Folders.Commands.CreateRoot
{
    public sealed record CreateRootFolderCommand(Guid UserId, string Name, string Color) : IRequest<Result<Unit>>,
        IMustHasUserId,
        IHasName,
        IHasHexColor;
}