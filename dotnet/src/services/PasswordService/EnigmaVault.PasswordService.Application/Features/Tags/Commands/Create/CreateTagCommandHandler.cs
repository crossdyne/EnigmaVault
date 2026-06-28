using Crossdyne.Toolkit.Results;
using EnigmaVault.PasswordService.Application.Common;
using EnigmaVault.PasswordService.Application.Common.Repositories;
using EnigmaVault.PasswordService.Domain.Models;
using MediatR;
using Shared.Kernel.Exceptions;

namespace EnigmaVault.PasswordService.Application.Features.Tags.Commands.Create
{
    internal sealed class CreateTagCommandHandler(
        ITagRepository repository,
        IUnitOfWork unitOfWork) : IRequestHandler<CreateTagCommand, Result<Guid>>
    {
        private readonly ITagRepository _repository = repository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result<Guid>> Handle(CreateTagCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var tag = Tag.Create(request.UserId, request.Name, request.Color);

                await _repository.AddAsync(tag, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return tag.Id.Value;
            }
            catch (DomainException ex)
            {
                return ex.Error;
            }
            catch (Exception)
            {
                return new Error(ErrorCode.Server, "Произошла ошибка на стороне сервера");
            }
        }
    }
}
