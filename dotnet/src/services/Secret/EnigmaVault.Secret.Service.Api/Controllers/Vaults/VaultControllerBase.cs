using Crossdyne.Toolkit.Results;
using EnigmaVault.Secret.Service.Api.Extensions;
using EnigmaVault.Secret.Service.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnigmaVault.Secret.Service.Api.Controllers.Vaults
{    
    [ApiController]
    [Route("api/vault")]
    [Authorize]
    public abstract class VaultControllerBase : Controller
    {
        protected Result<ExtractData> ExtractCredentials() 
            => this.ExtractCredentials(User);
    }
}