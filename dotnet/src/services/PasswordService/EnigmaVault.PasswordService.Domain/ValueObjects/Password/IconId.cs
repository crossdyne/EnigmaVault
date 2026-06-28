using EnigmaVault.PasswordService.Domain.Exception;

namespace EnigmaVault.PasswordService.Domain.ValueObjects.Password
{
    public readonly record struct IconId
    {
        public readonly Guid Value { get; }

        private IconId(Guid value) => Value = value;

        /// <exception cref="EmptyIdentifierException"></exception>
        public static IconId Create(Guid value)
        {
            // Guard.Against.That(value == Guid.Empty, () => new EmptyIdentifierException(Error.New(ErrorCode.Null, $"Был передан пустой {typeof(Guid)} в качестве идентификатора в {nameof(IconId)}")));

            return new IconId(value);
        }

        public static IconId New() => new(Guid.NewGuid());

        public override string ToString() => Value.ToString();

        public static implicit operator Guid(IconId value) => value.Value;
    }
}