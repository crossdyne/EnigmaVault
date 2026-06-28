using Crossdyne.Toolkit.Results;
using Crossdyne.Toolkit.Validation;
using Shared.Kernel.Errors;
using Shared.Kernel.Exceptions;
using System.Collections.Immutable;

namespace EnigmaVault.PasswordService.Domain.ValueObjects.Password
{
    public readonly record struct EncryptedData
    {
        public const int MAX_LENGTH = 256 * 1024;
        public const int MIN_LENGTH = 28;

        public ImmutableArray<byte> Value { get; }

        private EncryptedData(ImmutableArray<byte> value) => Value = value;

        /// <exception cref="DomainException"></exception>
        public static EncryptedData Create(byte[] value)
        {
            Guard.Against.That(value is null, () => new DomainException(new Error(AppErrors.Validation, "Данные не могут быть null.")));

            Guard.Against.That(value!.Length > MAX_LENGTH || value.Length < MIN_LENGTH, () => new DomainException(new Error(AppErrors.Validation, $"Размер зашифрованных данных некорректен. Получено: {value.Length} байт. Ожидается: {MIN_LENGTH}-{MAX_LENGTH} байт.")));

            return new EncryptedData(value.ToImmutableArray());
        }

        public static implicit operator byte[](EncryptedData data) => [.. data.Value];
    }
}