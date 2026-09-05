using EnigmaVault.Secret.Service.Application.Features.Validators;
using FluentValidation;

namespace EnigmaVault.Secret.Service.Application.Features.Folders.Commands.Delete
{
    public sealed class DeleteFolderCommandValidator : AbstractValidator<DeleteFolderCommand>
    {
        public DeleteFolderCommandValidator()
        {
            Include(new GuidValidator());
            Include(new MustUserIdValidator());
        }
    }
}