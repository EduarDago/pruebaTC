using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Proteccion.TableroControl.Dominio.Entidades;
using System.Threading.Tasks;

namespace Proteccion.TableroControl.Presentacion.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        public AzureConfig AzureConfig { get; }

        public AccountController(IOptions<AzureConfig> azureConfig)
        {
            AzureConfig = azureConfig.Value;
        }

        [HttpGet]
        public IActionResult AccessDenied() => View();

        [HttpGet]
        public IActionResult SignIn()
        {
            var urlRedirect = string.Format(AzureConfig.UrlAuthorizacion, AzureConfig.TenantId, AzureConfig.ClientId, AzureConfig.UrlRedirect);
            return Redirect(urlRedirect);
        }

        [HttpGet]
        public async Task<IActionResult> SignOut()
        {
            await ControllerContext.HttpContext.SignOutAsync(Microsoft.AspNetCore.Authentication.OpenIdConnect.OpenIdConnectDefaults.AuthenticationScheme);
            await ControllerContext.HttpContext.SignOutAsync(Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme);
            return View();
        }
    }
}