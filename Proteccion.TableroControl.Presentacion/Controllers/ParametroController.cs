using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Proteccion.TableroControl.Dominio.Entidades;
using Proteccion.TableroControl.Dominio.Enumeraciones;
using Proteccion.TableroControl.Dominio.Interfaces;
using Proteccion.TableroControl.Presentacion.ViewModels;
using Proteccion.TableroControl.Proxy.BL;
using System;
using System.Linq;

namespace Proteccion.TableroControl.Presentacion.Controllers
{
    [Authorize]
    public class ParametroController : Controller
    {
        private readonly IParametroProxy parametroProxy;
        private readonly ILogger<ParametroController> _logger;
        private readonly IManagerSession managerSession;

        public ParametroController(ILogger<ParametroController> logger, IParametroProxy proxy, IManagerSession manager)
        {
            parametroProxy = proxy;
            _logger = logger;
            managerSession = manager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            try
            {
                var notificacion = managerSession.Notificacion;
                if (notificacion != null)
                {
                    ViewBag.Notificacion = notificacion;
                    managerSession.EliminarSession("Notificacion");
                }

                var parametros = parametroProxy.ObtenerParametros().ToList();
                return View(parametros);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return View();
            }
        }

        [HttpGet]
        public IActionResult Nuevo()
        {
            var viewModel = new ParametroViewModel
            {
                Accion = "Nuevo"
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Nuevo(ParametroViewModel viewModel)
        {
            try
            {
                viewModel.Accion = "Nuevo";
                if (parametroProxy.ExisteParametro(viewModel.Parametro.Nombre))
                {
                    ViewBag.Notificacion = GenerarNotificacion("El nombre de la parametro ya se encuentra registrado", false, TipoNotificacion.Error, "Lo sentimos!");
                    return View("Nuevo", viewModel);
                }

                if (parametroProxy.InsertarParametro(viewModel.Parametro))
                {
                    var notificacion = GenerarNotificacion("El parametro se ha guardado correctamente", false, TipoNotificacion.Exito, "Muy Bien!");
                    managerSession.Notificacion = notificacion;

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewBag.Notificacion = GenerarNotificacion("Ha ocurrido un error guardando el parametro", false, TipoNotificacion.Error, "Lo sentimos!");
                    return View("Nuevo", viewModel);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return View();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Editar(int id)
        {
            try
            {
                var parametro = parametroProxy.ObtenerParametro(id);
                var viewModel = new ParametroViewModel
                {
                    Accion = "Editar",
                    Parametro = parametro
                };

                return View("Nuevo", viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return View("Nuevo");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Editar(ParametroViewModel viewModel)
        {
            try
            {
                viewModel.Accion = "Editar";
                var idParametro = viewModel.Parametro.IdParametro;
                var parametroExistente = parametroProxy.ObtenerParametro(idParametro);

                //Si el nombre cambió se valida que no este en uso
                if (!parametroExistente.Nombre.Equals(viewModel.Parametro.Nombre) && parametroProxy.ExisteParametro(viewModel.Parametro.Nombre))
                {
                    ViewBag.Notificacion = GenerarNotificacion("El nombre del parametro ya se encuentra registrado", false, TipoNotificacion.Error, "Lo sentimos!");
                    return View(viewModel);
                }

                if (parametroProxy.ActualizarParametro(viewModel.Parametro))
                {
                    var notificacion = GenerarNotificacion("El parametro se ha actualizado correctamente", false, TipoNotificacion.Exito, "Muy Bien!");
                    managerSession.Notificacion = notificacion;

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewBag.Notificacion = GenerarNotificacion("Ha ocurrido un error guardando la validación", false, TipoNotificacion.Error, "Lo sentimos!");
                    return View("Nuevo", viewModel);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return View("Nuevo");
            }
        }

        public IActionResult ObtenerFechaProceso()
        {
            string fecha = managerSession.FechaProceso;
            if (string.IsNullOrEmpty(fecha))
            {
                fecha = parametroProxy.ObtenerParametro("FechaProceso")?.Valor;
                managerSession.FechaProceso = fecha;
            }

            return Json(new { fecha });
        }

        private Notificacion GenerarNotificacion(string mensaje, bool popup, TipoNotificacion tipoNotificacion, string header, bool redireccionar = false, string controller = "", string action = "")
        {
            if (popup)
            {
                return new Notificacion { Mensaje = mensaje, TipoVentana = TipoVentana.Mensaje, Header = header, Redireccionar = redireccionar, ControllerName = controller, ActionName = action };
            }
            else
            {
                return new Notificacion { Mensaje = mensaje, TipoVentana = TipoVentana.Notificacion, Header = header, TipoMensaje = tipoNotificacion };
            }
        }
    }
}