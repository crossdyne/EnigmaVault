using Crossdyne.Toolkit.Validation;
using System.Diagnostics.CodeAnalysis;

namespace EnigmaVault.Password.Service.Domain.ValueObjects.Password
{
    public readonly record struct SvgCode
    {
        public string Value { get; }

        private SvgCode(string value) => Value = value;

        /// <summary>
        /// Создает экземпляр SvgCode из строки. Выбрасывает исключение при невалидных данных.
        /// </summary>
        /// <exception cref="ArgumentNullException">Если svgString равен null.</exception>
        /// <exception cref="ArgumentException">Если svgString не является валидным SVG-фрагментом.</exception>
        public static SvgCode Create(string svgString)
        {
            Guard.Against.That(!TryValidate(svgString, out var validatedValue, out var errorMessage), () => new ArgumentException(errorMessage, nameof(svgString)));

            return new SvgCode(validatedValue!);
        }

        /// <summary>
        /// Пытается создать экземпляр SvgCode из строки. Не выбрасывает исключений.
        /// </summary>
        /// <param name="svgString"></param>
        /// <param name="svgCode"></param>
        /// <returns>True, если создание прошло успешно, иначе False.</returns>
        public static bool TryCreate(string? svgString, [MaybeNullWhen(false)] out SvgCode svgCode)
        {
            if (TryValidate(svgString, out var validatedValue, out _))
            {
                svgCode = new SvgCode(validatedValue!);
                return true;
            }

            svgCode = default;
            return false;
        }

        private static bool TryValidate(
            string? rawValue,
            [MaybeNullWhen(false)] out string validatedValue,
            [MaybeNullWhen(true)] out string? errorMessage)
        {
            if (string.IsNullOrWhiteSpace(rawValue))
            {
                validatedValue = null;
                errorMessage = "SVG code не может быть null или пустым.";
                return false;
            }

            var trimmedValue = rawValue.Trim();

            bool startsWithSvg = trimmedValue.StartsWith("<svg", StringComparison.OrdinalIgnoreCase);
            bool endsWithSvg = trimmedValue.EndsWith("</svg>", StringComparison.OrdinalIgnoreCase);

            if (!startsWithSvg || !endsWithSvg)
            {
                validatedValue = null;
                errorMessage = "Предоставленная строка не является допустимым фрагментом SVG. Она должна начинаться с '<svg' и заканчиваться на '</svg>'.";
                return false;
            }

            validatedValue = trimmedValue;
            errorMessage = null;
            return true;
        }

        public static implicit operator string(SvgCode code) => code.Value;

        public override string ToString() => Value;
    }
}