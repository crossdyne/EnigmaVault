using EnigmaVault.Secret.Service.Application.Features.Folders.Validators;
using EnigmaVault.Secret.Service.Application.Features.Validators;
using FluentValidation;

namespace EnigmaVault.Secret.Service.Application.Features.Folders.Commands.CreateSubFolder
{
    public sealed class CreateSubFolderCommandValidator : AbstractValidator<CreateSubFolderCommand>
    {
        public CreateSubFolderCommandValidator()
        {
            Include(new NameFolderValidator());
            Include(new MustUserIdValidator());
            Include(new HexColorValidator());

            RuleFor(sf => sf.ParentFolderId).NotEmpty().WithMessage("Не указа родительская папка.");
        }
    }
}