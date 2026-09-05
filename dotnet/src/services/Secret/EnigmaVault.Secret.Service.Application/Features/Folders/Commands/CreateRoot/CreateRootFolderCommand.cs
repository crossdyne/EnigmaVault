using Crossdyne.Toolkit.Results;
using EnigmaVault.Secret.Service.Application.Features.Folders.Validators;
using EnigmaVault.Secret.Service.Application.Features.Validators;
using MediatR;

namespace EnigmaVault.Secret.Service.Application.Features.Folders.Commands.CreateRoot
{
    public sealed record CreateRootFolderCommand(Guid UserId, string Name, string Color) : IRequest<Result<Unit>>,
        IMustHasUserId,
        IHasName,
        IHasHexColor;
}