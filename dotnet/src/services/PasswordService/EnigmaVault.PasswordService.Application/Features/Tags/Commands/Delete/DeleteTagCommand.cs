using Crossdyne.Toolkit.Results;
using MediatR;

namespace EnigmaVault.PasswordService.Application.Features.Tags.Commands.Delete
{
    public sealed record DeleteTagCommand(Guid Id, Guid UserId) : IRequest<Result<Unit>>;
}