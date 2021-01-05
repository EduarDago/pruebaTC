using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Proteccion.TableroControl.Dominio.Entidades;
using Proteccion.TableroControl.Dominio.Enumeraciones;
using Proteccion.TableroControl.Dominio.Interfaces;
using Proteccion.TableroControl.Presentacion.Helpers;
using Proteccion.TableroControl.Presentacion.ViewModels;
using Proteccion.TableroControl.Proxy.BL;
using System;
using System.Linq;

namespace Proteccion.TableroControl.Presentacion.Controllers
{
    [Authorize]
    public class ValidacionController : Controller
    {
        private readonly IValidacionProxy validacionProxy;
        private readonly IComunProxy comunProxy;
        private readonly ILogger<ValidacionController> _logger;
        private readonly IManagerSession managerSession;

        public ValidacionController(ILogger<ValidacionController> logger, IComunProxy comunProxy, IValidacionProxy validacionProxy, IManagerSession manager)
        {
            this.validacionProxy = validacionProxy;
            this.comunProxy = comunProxy;
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

                var validaciones = validacionProxy.ObtenerValidaciones().ToList();
                return View(validaciones);
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
            try
            {
                var validacion = new DetalleValidacionViewModel();
                validacion.Validacion = new Validacion();
                validacion.Equipos = comunProxy.ObtenerTopicos("Equipo");
                validacion.TiposValidacion = comunProxy.ObtenerTopicos("TipoValidacion");
                validacion.Accion = "Nueva";
                return View(validacion);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return View();
            }
        }


        [HttpPost]
        public IActionResult GuardarValidacion(DetalleValidacionViewModel viewModel)
        {
            try
            {
                //Se cargan las listas desplegables
                viewModel.Equipos = comunProxy.ObtenerTopicos("Equipo");
                viewModel.TiposValidacion = comunProxy.ObtenerTopicos("TipoValidacion");

                //Se valida la existencia del Sp
                if (!string.IsNullOrEmpty(viewModel.Validacion.Sp))
                {
                    var existeSP = comunProxy.ValidarExistenciaSp(viewModel.Validacion.Esquema, viewModel.Validacion.Sp);
                    if (!existeSP)
                    {
                        ViewBag.Notificacion = GenerarNotificacion("El procedimiento almacenado no se encuentra en la BD", false, TipoNotificacion.Error, "Lo sentimos!");
                        return View("Nuevo", viewModel);
                    }
                }

                var idValidacion = viewModel.Validacion.IdValidacion;

                //Se identifica si se actualiza o inserta una validacion
                if (idValidacion > 0)
                {
                    return ActualizarValidacion(viewModel);
                }
                else
                {
                    return InsertarValidacion(viewModel);
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
        /// <param name="viewModel"></param>
        /// <returns></returns>
        private IActionResult ActualizarValidacion(DetalleValidacionViewModel viewModel)
        {
            var idValidacion = viewModel.Validacion.IdValidacion;
            var validacionExistente = validacionProxy.ObtenerValidacion(idValidacion);

            //Si el nombre cambió se valida que no este en uso
            if (!validacionExistente.Nombre.Equals(viewModel.Validacion.Nombre) && validacionProxy.ValidarExistencia(viewModel.Validacion.Nombre))
            {
                ViewBag.Notificacion = GenerarNotificacion("El nombre de la validación ya se encuentra registrado", false, TipoNotificacion.Error, "Lo sentimos!");
                return View("Nuevo", viewModel);
            }

            if (validacionProxy.ActualizarValidacion(viewModel.Validacion))
            {
                var notificacion = GenerarNotificacion("La validación se ha actualizado correctamente", false, TipoNotificacion.Exito, "Muy Bien!");
                managerSession.Notificacion = notificacion;

                return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewBag.Notificacion = GenerarNotificacion("Ha ocurrido un error guardando la validación", false, TipoNotificacion.Error, "Lo sentimos!");
                return View("Nuevo", viewModel);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        private IActionResult InsertarValidacion(DetalleValidacionViewModel viewModel)
        {
            if (validacionProxy.ValidarExistencia(viewModel.Validacion.Nombre))
            {
                ViewBag.Notificacion = GenerarNotificacion("El nombre de la validación ya se encuentra registrado", false, TipoNotificacion.Error, "Lo sentimos!");
                return View("Nuevo", viewModel);
            }

            if (validacionProxy.InsertarValidacion(viewModel.Validacion))
            {
                var notificacion = GenerarNotificacion("La validación se ha guardado correctamente", false, TipoNotificacion.Exito, "Muy Bien!");
                managerSession.Notificacion = notificacion;

                return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewBag.Notificacion = GenerarNotificacion("Ha ocurrido un error guardando la validación", false, TipoNotificacion.Error, "Lo sentimos!");
                return View("Nuevo", viewModel);
            }
        }

        [HttpGet]
        public IActionResult Detalle(int id)
        {
            var validacion = new DetalleValidacionViewModel();
            validacion.Validacion = validacionProxy.ObtenerValidacion(id);
            return View(validacion);
        }

        [HttpGet]
        public IActionResult Editar(int id)
        {
            var validacion = new DetalleValidacionViewModel();
            validacion.Validacion = validacionProxy.ObtenerValidacion(id);
            validacion.Equipos = comunProxy.ObtenerTopicos("Equipo");
            validacion.TiposValidacion = comunProxy.ObtenerTopicos("TipoValidacion");
            validacion.Accion = "Editar";

            return View("Nuevo", validacion);
        }

        [HttpGet]
        public IActionResult Eliminar(int id)
        {
            try
            {
                validacionProxy.EliminarValidacion(id);
                var notificacion = GenerarNotificacion("La validación se ha eliminado correctamente", false, TipoNotificacion.Exito, "Muy Bien!");
                managerSession.Notificacion = notificacion;
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return View();
            }
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