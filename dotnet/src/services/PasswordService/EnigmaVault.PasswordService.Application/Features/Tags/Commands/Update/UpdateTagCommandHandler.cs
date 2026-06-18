using Common.Core.Results;
using EnigmaVault.PasswordService.Application.Common;
using EnigmaVault.PasswordService.Application.Common.Repositories;
using EnigmaVault.PasswordService.Domain.ValueObjects.Common;
using EnigmaVault.PasswordService.Domain.ValueObjects.Tag;
using MediatR;
using Shared.Kernel.Exceptions;
using Unit = Common.Core.Results.Unit;

namespace EnigmaVault.PasswordService.Application.Features.Tags.Commands.Update
{
    internal sealed class UpdateTagCommandHandler(IUnitOfWork unitOfWork, ITagRepository repository) : IRequestHandler<UpdateTagCommand, Result<Unit>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ITagRepository _repository = repository;

        public async Task<Result<Unit>> Handle(UpdateTagCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var maybeTag = await _repository.GetAsync(request.Id, request.UserId, token: cancellationToken);

                if (maybeTag.HasValue)
                {
                    maybeTag.Value.UpdateName(TagName.Create(request.Name));
                    maybeTag.Value.UpdateColor(Color.FromHex(request.Color));
                    await _unitOfWork.SaveChangesAsync(cancellationToken);

                    return Unit.Value;
                }

                return Error.NotFound(request.Name, request.Id);
            }
            catch (DomainException ex)
            {
                return ex.Error;
            }
            catch (Exception)
            {
                return Error.New(ErrorCode.Server, "Произошла ошибка на стороне сервера");
            }
        }
    }
}