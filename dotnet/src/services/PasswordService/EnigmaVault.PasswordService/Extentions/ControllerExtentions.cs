using Common.Core.Results;
using EnigmaVault.PasswordService.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EnigmaVault.PasswordService.Extentions
{
    public static class ControllerExtentions
    {
        public static Result<ExtractData> ExtactCredentials(this Controller controller, ClaimsPrincipal user)
        {
            var extractData = new ExtractData();

            var userIdString = user.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdString))
                extractData.Result = controller.Unauthorized("User ID не найден в токене.");

            if (!Guid.TryParse(userIdString, out var userIdGuid))
                extractData.Result = controller.BadRequest("Не верный User ID формат.");

            extractData.UserId = userIdGuid;

            return Result<ExtractData>.Success(extractData);
        }
    }
}