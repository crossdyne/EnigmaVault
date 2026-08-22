using MediatR;
using Shared.Contracts.SecretService.Responses;

namespace EnigmaVault.Secret.Service.Application.Features.Tags.Queries.GetAll
{
    public sealed record GetAllTagsQuery(Guid UserId) : IRequest<List<TagResponse>>;
}