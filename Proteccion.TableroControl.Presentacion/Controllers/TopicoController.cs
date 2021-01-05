using Microsoft.AspNetCore.Authorization;
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
    public class TopicoController : Controller
    {
        private readonly ITopicoProxy topicoProxy;
        private readonly ILogger<TopicoController> _logger;
        private readonly IManagerSession managerSession;

        public TopicoController(ILogger<TopicoController> logger, ITopicoProxy topicoProxy, IManagerSession manager)
        {
            this.topicoProxy = topicoProxy;
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

                var topicos = topicoProxy.ObtenerTopicos().ToList();
                return View(topicos);
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
            var viewModel = new TopicoViewModel();
            viewModel.Identificadores = topicoProxy.ObtenerIdentificadoresTopicos();
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Nuevo(TopicoViewModel viewModel)
        {
            try
            {
                viewModel.Identificadores = topicoProxy.ObtenerIdentificadoresTopicos();

                if (topicoProxy.ExisteTopico(viewModel.Topico.Identificador, viewModel.Topico.Valor))
                {
                    ViewBag.Notificacion = GenerarNotificacion("El valor del topico ya se encuentra registrado", false, TipoNotificacion.Error, "Lo sentimos!");
                    return View(viewModel);
                }

                if (topicoProxy.InsertarTopico(viewModel.Topico))
                {
                    var notificacion = GenerarNotificacion("El topico se ha guardado correctamente", false, TipoNotificacion.Exito, "Muy Bien!");
                    managerSession.Notificacion = notificacion;

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewBag.Notificacion = GenerarNotificacion("Ha ocurrido un error guardando el topico", false, TipoNotificacion.Error, "Lo sentimos!");
                    return View(viewModel);
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
                var viewModel = new TopicoViewModel();
                viewModel.Identificadores = topicoProxy.ObtenerIdentificadoresTopicos();
                viewModel.Topico = topicoProxy.ObtenerTopico(id);
                return View(viewModel);
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
        [HttpPost]
        public IActionResult Editar(TopicoViewModel viewModel)
        {
            try
            {
                viewModel.Identificadores = topicoProxy.ObtenerIdentificadoresTopicos();

                var idTopico = viewModel.Topico.IdTopico;
                var topicoExistente = topicoProxy.ObtenerTopico(idTopico);

                //Si el nombre cambió se valida que no este en uso
                if (!topicoExistente.Valor.Equals(viewModel.Topico.Valor) && topicoProxy.ExisteTopico(viewModel.Topico.Identificador, viewModel.Topico.Valor))
                {
                    ViewBag.Notificacion = GenerarNotificacion("El valor del topico ya se encuentra registrado", false, TipoNotificacion.Error, "Lo sentimos!");
                    return View(viewModel);
                }

                if (topicoProxy.ActualizarTopico(viewModel.Topico))
                {
                    var notificacion = GenerarNotificacion("El topico se ha actualizado correctamente", false, TipoNotificacion.Exito, "Muy Bien!");
                    managerSession.Notificacion = notificacion;

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewBag.Notificacion = GenerarNotificacion("Ha ocurrido un error guardando la validación", false, TipoNotificacion.Error, "Lo sentimos!");
                    return View(viewModel);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return View();
            }
        }

        [HttpGet]
        public IActionResult Eliminar(int id)
        {
            try
            {
                topicoProxy.EliminarTopico(id);
                var notificacion = GenerarNotificacion("El topico se ha eliminado correctamente", false, TipoNotificacion.Exito, "Muy Bien!");
                managerSession.Notificacion = notificacion;
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return View();
            }
        }

        [HttpGet]
        public IActionResult ObtenerOrden(string identificador)
        {
            try
            {
                var orden = topicoProxy.ObtenerUltimoOrden(identificador);
                return Json(orden);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Json(null);
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