using EnigmaVault.Password.Service.Application.Features.Folders.Validators;
using EnigmaVault.Password.Service.Application.Features.Validators;
using FluentValidation;

namespace EnigmaVault.Password.Service.Application.Features.Folders.Commands.CreateRoot
{
    public sealed class CreateRootFolderCommandValidator : AbstractValidator<CreateRootFolderCommand>
    {
        public CreateRootFolderCommandValidator()
        {
            Include(new NameFolderValidator());
            Include(new MustUserIdValidator());
            Include(new HexColorValidator());
        }
    }
}