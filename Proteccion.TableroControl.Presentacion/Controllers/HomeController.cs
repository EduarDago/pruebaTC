using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Proteccion.TableroControl.Dominio.Entidades;
using Proteccion.TableroControl.Dominio.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Proteccion.TableroControl.Presentacion.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IAzure _azure;

        public HomeController(IAzure azure)
        {
            _azure = azure;
        }
        /*
        [AllowAnonymous]
        public async Task<IActionResult> Index(string code)
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Tablero");
            }

            if (!string.IsNullOrEmpty(code))
            {
                _azure.Code = code;

                var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                var userInfo = await _azure.ObtenerInformacionUsuario();

                if (userInfo.IsAdmin || userInfo.IsEjecutor)
                {
                    var claims = new List<Claim>
                    {
                        new Claim("name", userInfo.DisplayName),
                        new Claim("email", userInfo.UserPrincipalName),
                        new Claim("role", userInfo.IsAdmin ? "Administrador":"Ejecutor"),
                        new Claim("username", userInfo.MailNickname)
                    };

                    ClaimsIdentity userIdentity = new ClaimsIdentity(claims, "login");
                    ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);

                    await HttpContext.S
                    return RedirectToAction("Tablero");
                }
                else
                {
                    return RedirectToAction("AccessDenied", "Account");
                }
            }
            else
            {
                return RedirectToAction("SignIn", "Account");
            }
        }*/

        public IActionResult Tablero() => View();

        public IActionResult GetLoggedUser()
        {
            var loggedUser = new UsuarioLogueado();
            /*
            var email = User.Claims.FirstOrDefault(x => x.Type == "email")?.Value;
            var nombreUsuario = User.Claims.FirstOrDefault(x => x.Type == "name")?.Value;
            var usuario = User.Claims.FirstOrDefault(x => x.Type == "username")?.Value;
            loggedUser.Login = usuario;
            loggedUser.NombreUsuario = nombreUsuario;
            loggedUser.Email = email;*/

            return Json(loggedUser);
        }
    }
}
