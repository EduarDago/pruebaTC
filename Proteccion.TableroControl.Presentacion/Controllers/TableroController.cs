using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Proteccion.TableroControl.Presentacion.ViewModels;
using Proteccion.TableroControl.Proxy.BL;
using Microsoft.SqlServer.TransactSql.ScriptDom;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace Proteccion.TableroControl.Presentacion.Controllers
{
    [Authorize]
    public class TableroController : Controller
    {
        private readonly IComunProxy topicoProxy;
        private readonly IValidacionProxy validacionProxy;
        private readonly ILogger<TableroController> _logger;
        private readonly ILogEjecucionProxy logProxy;
        private readonly IHttpContextAccessor accessor;
        private readonly IEstadisticaProxy estadisticaProxy;

        private string Login { get; set; }
        private string NombreUsuario { get; set; }
        private string Email { get; set; }

        public TableroController(ILogger<TableroController> logger, IComunProxy comunProxy, IValidacionProxy validacionProxy, ILogEjecucionProxy logProxy, IHttpContextAccessor accessor, IEstadisticaProxy estadisticaProxy)
        {
            topicoProxy = comunProxy;
            this.logProxy = logProxy;
            this.validacionProxy = validacionProxy;
            _logger = logger;
            this.estadisticaProxy = estadisticaProxy;
            this.accessor = accessor;
        }

        [HttpGet]
        public IActionResult Index(int? idEquipo, int? idTipoValidacion)
        {
            try
            {
                int equipo = idEquipo ?? 0;
                int tipo = idTipoValidacion ?? 0;

                var viewModel = new TableroViewModel();
                viewModel.TiposValidacion = topicoProxy.ObtenerTopicos("TipoValidacion");
                viewModel.Equipos = topicoProxy.ObtenerTopicos("Equipo");
                viewModel.Validaciones = validacionProxy.ObtenerValidacionesActivas(equipo, tipo).ToList();
                viewModel.IdEquipo = equipo;
                viewModel.IdTipoValidacion = tipo;
                viewModel.UltimaEjecucionValidacion = estadisticaProxy.ObtenerUltimaEjecucionValidacion(viewModel.Validaciones.Select(a => a.IdValidacion).ToArray());

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return View();
            }
        }

        [HttpGet]
        public IActionResult DetalleInconsistencias(int idValidacion)
        {
            GetLoggedUser();
            string usuario = Login;
            var inconsistencias = validacionProxy.ObtenerInconsistencias(idValidacion, usuario);
            return Json(inconsistencias);
        }

        [HttpGet]
        public IActionResult EjecucionScripts()
        {
            return View();
        }

        public bool ValidacionSql(string sql, out List<string> errores)
        {
            errores = new List<string>();
            TSql140Parser parser = new TSql140Parser(false);
            IList<ParseError> parseErrors;

            using (TextReader reader = new StringReader(sql))
            {
                parser.Parse(reader, out parseErrors);
                if (parseErrors != null && parseErrors.Count > 0)
                {
                    errores = parseErrors.Select(e => e.Message).ToList();
                    return false;
                }
            }
            return true;
        }

        [HttpGet]
        public JsonResult ValidarScript(string sql)
        {
            List<string> errores;
            bool resultado = ValidacionSql(sql, out errores);
            if (resultado)
            {
                errores.Insert(0, "Sintaxis correcta");
            }
            else
            {
                errores.Insert(0, "El query tiene los siguientes errores en  la sintaxis: ");
            }

            return Json(new { Resultado = resultado, Errores = string.Join("\n", errores.ToArray()) });
        }

        [HttpGet]
        public JsonResult EjecutarScript(string sql)
        {
            GetLoggedUser();
            string resultado = logProxy.EjecutarQuery(sql);
            logProxy.InsertarLog(Login, sql, resultado);
            return Json(new { resultado });
        }

        private void GetLoggedUser()
        {
            var email = accessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "email")?.Value;
            var nombreUsuario = accessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "name")?.Value;
            var usuario = accessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "username")?.Value;

            Login = usuario;
            NombreUsuario = nombreUsuario;
            Email = email;
        }
    }
}