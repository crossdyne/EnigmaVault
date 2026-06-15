using Common.Core.Results;
using MediatR;
using Unit = Common.Core.Results.Unit;

namespace EnigmaVault.PasswordService.Application.Features.Tags.Commands.Delete
{
    public sealed record DeleteTagCommand(Guid Id, Guid UserId) : IRequest<Result<Unit>>;
}