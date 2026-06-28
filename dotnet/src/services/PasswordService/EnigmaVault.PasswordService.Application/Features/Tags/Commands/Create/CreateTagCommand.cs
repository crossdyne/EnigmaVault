using Crossdyne.Toolkit.Results;
using EnigmaVault.PasswordService.Application.Features.Validators;
using MediatR;

namespace EnigmaVault.PasswordService.Application.Features.Tags.Commands.Create
{
    public sealed record CreateTagCommand(Guid UserId, string Name, string Color) : IRequest<Result<Guid>>,
        IMustHasUserId,
        IHasName;
}