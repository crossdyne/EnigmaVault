using Crossdyne.Toolkit.Results;
using EnigmaVault.Password.Service.Application.Features.Validators;
using MediatR;

namespace EnigmaVault.Password.Service.Application.Features.Tags.Commands.Update
{
    public sealed record UpdateTagCommand(Guid Id, Guid UserId, string Name, string Color) : IRequest<Result<Unit>>,
        IHasName;
}