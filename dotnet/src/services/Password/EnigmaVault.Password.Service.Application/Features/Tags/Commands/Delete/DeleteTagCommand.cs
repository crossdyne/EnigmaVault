using Crossdyne.Toolkit.Results;
using MediatR;

namespace EnigmaVault.Password.Service.Application.Features.Tags.Commands.Delete
{
    public sealed record DeleteTagCommand(Guid Id, Guid UserId) : IRequest<Result<Unit>>;
}