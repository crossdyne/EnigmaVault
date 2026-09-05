using MediatR;
using Shared.Contracts.SecretService.Responses;

namespace EnigmaVault.Secret.Service.Application.Features.Folders.Queries.GetAll
{
    public sealed record GetAllFoldersQuery(Guid UserId) : IRequest<List<FolderResponse>>; 
}