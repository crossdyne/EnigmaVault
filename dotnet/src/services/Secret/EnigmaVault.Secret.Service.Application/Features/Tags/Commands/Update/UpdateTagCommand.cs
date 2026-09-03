using Crossdyne.Toolkit.Results;
using EnigmaVault.Secret.Service.Application.Features.Validators;
using MediatR;
using Unit = Crossdyne.Toolkit.Primitives.Unit;

namespace EnigmaVault.Secret.Service.Application.Features.Tags.Commands.Update
{
    public sealed record UpdateTagCommand(Guid UserId, Guid Id, string Name, string Color) : IRequest<Result<Unit>>,
        IHasName;
}