using Crossdyne.Toolkit.Results;
using EnigmaVault.Password.Service.Application.Common;
using EnigmaVault.Password.Service.Application.Common.Repositories;
using EnigmaVault.Password.Service.Domain.Models;
using MediatR;

namespace EnigmaVault.Password.Service.Application.Features.Tags.Commands.Create
{
    internal sealed class CreateTagCommandHandler(
        ITagRepository repository,
        IUnitOfWork unitOfWork) : IRequestHandler<CreateTagCommand, Result<Guid>>
    {
        private readonly ITagRepository _repository = repository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result<Guid>> Handle(CreateTagCommand request, CancellationToken cancellationToken)
        {
            var tag = Tag.Create(request.UserId, request.Name, request.Color);

            await _repository.AddAsync(tag, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return tag.Id.Value;
        }
    }
}
