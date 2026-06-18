using Microsoft.AspNetCore.Mvc;

namespace EnigmaVault.PasswordService.Models
{
    public sealed class ExtractData() 
    {
        public Guid UserId { get; set; }
        public IActionResult Result { get; set; } = null!;
    }
}