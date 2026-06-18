using Common.Core.Results;
using EnigmaVault.PasswordService.Application.Features.Validators;
using MediatR;
using Unit = Common.Core.Results.Unit;

namespace EnigmaVault.PasswordService.Application.Features.Tags.Commands.Update
{
    public sealed record UpdateTagCommand(Guid Id, Guid UserId, string Name, string Color) : IRequest<Result<Unit>>,
        IHasName;
}