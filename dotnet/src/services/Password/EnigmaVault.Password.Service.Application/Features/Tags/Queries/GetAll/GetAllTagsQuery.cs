using MediatR;
using Shared.Contracts.Responses.PasswordService;

namespace EnigmaVault.Password.Service.Application.Features.Tags.Queries.GetAll
{
    public sealed record GetAllTagsQuery(Guid UserId) : IRequest<List<TagResponse>>;
}