using MediatR;
using Shared.Contracts.PasswordService.Responses;

namespace EnigmaVault.Password.Service.Application.Features.Tags.Queries.GetAll
{
    public sealed record GetAllTagsQuery(Guid UserId) : IRequest<List<TagResponse>>;
}