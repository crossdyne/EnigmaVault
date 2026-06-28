using Crossdyne.Toolkit.Results;
using EnigmaVault.PasswordService.Application.Common;
using EnigmaVault.PasswordService.Application.Common.Repositories;
using EnigmaVault.PasswordService.Domain.ValueObjects.Folder;
using MediatR;
using Shared.Kernel.Exceptions;

namespace EnigmaVault.PasswordService.Application.Features.Folders.Commands.Update
{
    internal sealed class UpdateFolderCommandHandler(
        IFolderRepository repository,
        IUnitOfWork unitOfWork) : IRequestHandler<UpdateFolderCommand, Result<Unit>>
    {
        private readonly IFolderRepository _repository = repository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result<Unit>> Handle(UpdateFolderCommand request, CancellationToken cancellationToken)
        {
            var maybeFolder = await _repository.GetAsync(request.Id, request.UserId, cancellationToken);

            if (maybeFolder.IsNone)
                return new Error(ErrorCode.NotFound, $"Папка {request.Name} - { request.Id} не была найдена");

            maybeFolder.Value.Rename(FolderName.Create(request.Name));
            maybeFolder.Value.Move(request.ParentFolderId != null ? FolderId.Create(request.ParentFolderId.Value) : null);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}