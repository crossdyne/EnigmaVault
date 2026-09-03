using Crossdyne.Toolkit.Results;
using EnigmaVault.Secret.Service.Application.Features.Validators;
using MediatR;

namespace EnigmaVault.Secret.Service.Application.Features.Tags.Commands.Create
{
    public sealed record CreateTagCommand(Guid UserId, string Name, string Color) : IRequest<Result<Guid>>,
        IMustHasUserId,
        IHasName;
}