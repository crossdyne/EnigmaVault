using Crossdyne.Toolkit.Results;
using Shared.Kernel.Exceptions;

namespace EnigmaVault.PasswordService.Domain.Exception
{
    public sealed class EmptyIdentifierException(Error error) : DomainException(error);
}