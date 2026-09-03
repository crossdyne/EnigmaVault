using AutoMapper;
using AutoMapper.QueryableExtensions;
using EnigmaVault.Secret.Service.Application.Common;
using EnigmaVault.Secret.Service.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts.SecretService.Responses;

namespace EnigmaVault.Secret.Service.Application.Features.Tags.Queries.GetAll
{
    internal sealed class GetAllTagsQueryHandler(
        IApplicationDbContext context,
        IMapper mapper) : IRequestHandler<GetAllTagsQuery, List<TagResponse>>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<List<TagResponse>> Handle(GetAllTagsQuery request, CancellationToken cancellationToken)
            => await _context.Set<Tag>()
                .Where(t => t.UserId == request.UserId)
                    .ProjectTo<TagResponse>(_mapper.ConfigurationProvider)
                        .ToListAsync(cancellationToken);
    }
}