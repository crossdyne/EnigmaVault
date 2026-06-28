using Crossdyne.Toolkit.Results;
using EnigmaVault.PasswordService.Application.Common;
using EnigmaVault.PasswordService.Application.Common.Repositories;
using EnigmaVault.PasswordService.Domain.Models;
using MediatR;
using Shared.Kernel.Exceptions;

namespace EnigmaVault.PasswordService.Application.Features.Folders.Commands.CreateSubFolder
{
    internal sealed class CreateSubFolderCommandHandler(
        IFolderRepository repository, 
        IUnitOfWork unitOfWork) : IRequestHandler<CreateSubFolderCommand, Result<Unit>>
    {
        private readonly IFolderRepository _repository = repository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result<Unit>> Handle(CreateSubFolderCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (await _repository.Exist(request.Name, request.UserId, request.ParentFolderId, cancellationToken))
                    return new Error(ErrorCode.Conflict, "Папка с данными именем уже есть.");

                var folder = Folder.CreateSubfolder(request.UserId, request.ParentFolderId, request.Name, request.Color);

                await _repository.AddAsync(folder, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return Unit.Value;
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