using Crossdyne.Toolkit.Results;
using EnigmaVault.PasswordService.Application.Features.Validators;
using MediatR;

namespace EnigmaVault.PasswordService.Application.Features.Tags.Commands.Update
{
    public sealed record UpdateTagCommand(Guid Id, Guid UserId, string Name, string Color) : IRequest<Result<Unit>>,
        IHasName;
}