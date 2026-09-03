using EnigmaVault.Secret.Service.Domain.ValueObjects.Common;
using Xunit;

namespace EnigmaVault.Secret.Service.Unit.Tests.ValueObjects
{
    public class CommonTests
    {
        #region Color

        [Fact]
        public void Color_FromHex_ReturnColor()
        {
            string hex = "#000000";
            Color color = Color.FromHex(hex);

            Assert.Equal(hex, color.Value);
        }
        
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Color_EmptyValue_ThrowArgumentNullException(string? hex)
        {
            Assert.Throws<ArgumentNullException>(() =>  Color.FromHex(hex!));
        }

        [Theory]
        [InlineData("format")]
        [InlineData("000000")]
        [InlineData("#00000-0")]
        public void Color_IncorrectFormat_ThrowFormatException(string hex)
        {
            Assert.Throws<FormatException>(() =>  Color.FromHex(hex));
        }

        #endregion
    }
}