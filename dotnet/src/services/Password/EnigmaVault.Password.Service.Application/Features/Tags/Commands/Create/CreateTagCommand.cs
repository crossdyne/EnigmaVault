using Crossdyne.Toolkit.Results;
using EnigmaVault.Password.Service.Application.Features.Validators;
using MediatR;

namespace EnigmaVault.Password.Service.Application.Features.Tags.Commands.Create
{
    public sealed record CreateTagCommand(Guid UserId, string Name, string Color) : IRequest<Result<Guid>>,
        IMustHasUserId,
        IHasName;
}