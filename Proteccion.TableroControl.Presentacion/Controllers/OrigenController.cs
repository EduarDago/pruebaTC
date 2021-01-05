using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Proteccion.TableroControl.Dominio.Entidades;
using Proteccion.TableroControl.Presentacion.ViewModels;
using Proteccion.TableroControl.Proxy.BL;
using System;
using System.Linq;

namespace Proteccion.TableroControl.Presentacion.Controllers
{
    [Authorize]
    public class OrigenController : Controller
    {
        private readonly IOrigenDatosProxy origenProxy;
        private readonly IComunProxy comunProxy;
        private readonly ILogger<OrigenController> _logger;

        public OrigenController(ILogger<OrigenController> logger, IOrigenDatosProxy origenProxy, IComunProxy comunProxy)
        {
            this.origenProxy = origenProxy;
            this.comunProxy = comunProxy;
            _logger = logger;
        }

        /// <summary>
        /// Prueba
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            try
            {
                var origenes = origenProxy.ObtenerConfiguracionesOrigen().ToList();
                return View(origenes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return View();
            }
        }

        /// <summary>
        /// Otra prueba
        /// </summary>
        /// <returns></returns>
        public IActionResult Nuevo()
        {
            try
            {
                var viewModel = new OrigenViewModel
                {
                    OrigenDato = new OrigenDato(),
                    Accion = "Nuevo",
                    TiposDato = comunProxy.ObtenerTopicos("TipoDato"),
                    TiposOrigen = comunProxy.ObtenerTopicos("TipoOrigen")
                };
                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return View();
            }

        }

        /// <summary>
        /// Prueba CI
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Detalle(int id)
        {
            try
            {
                var viewModel = new OrigenViewModel
                {
                    OrigenDato = origenProxy.ObtenerConfiguracionOrigen(id),
                    Accion = "Detalle",
                    TiposDato = comunProxy.ObtenerTopicos("TipoDato"),
                    TiposOrigen = comunProxy.ObtenerTopicos("TipoOrigen")
                };
                return View(nameof(Nuevo), viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return View();
            }

        }

        /// <summary>
        /// Prueba edicion
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Editar(int id)
        {
            try
            {
                var viewModel = new OrigenViewModel
                {
                    OrigenDato = origenProxy.ObtenerConfiguracionOrigen(id)
                };
                viewModel.OrigenDato.Campos = viewModel.OrigenDato.Campos.OrderBy(x => x.Orden).ToList();
                viewModel.Accion = "Editar";
                viewModel.TiposDato = comunProxy.ObtenerTopicos("TipoDato");
                viewModel.TiposOrigen = comunProxy.ObtenerTopicos("TipoOrigen");
                return View(nameof(Nuevo), viewModel);
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
                origenProxy.EliminarOrigen(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return View();
            }

        }

        public IActionResult Importar()
        {
            try
            {
                var origenes = origenProxy.ConsultarEjecuciones();
                return View(origenes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return View();
            }

        }

        [HttpPost]
        public IActionResult GuardarConfiguracion(OrigenDato origen)
        {
            bool resultado = false;

            try
            {
                //Se valida la existencia del Sp
                if (!string.IsNullOrEmpty(origen.Procedimiento))
                {
                    var existeSP = comunProxy.ValidarExistenciaSp(origen.EsquemaProcedimiento, origen.Procedimiento);
                    if (!existeSP)
                    {
                        var respuestaSP = new
                        {
                            resultado = true,
                            validacion = 2
                        };

                        return Json(respuestaSP);
                    }
                }

                if (origen.IdOrigenDato > 0)
                {
                    var origenAnterior = origenProxy.ObtenerConfiguracionOrigen(origen.IdOrigenDato);

                    //Solo se valida el nombre si se ha cambiado
                    if (origenAnterior.Nombre != origen.Nombre && origenProxy.ExisteOrigen(origen.Nombre))
                    {
                        var respuestaSP = new
                        {
                            resultado = true,
                            validacion = 3
                        };

                        return Json(respuestaSP);
                    }

                    resultado = origenProxy.ActualizarConfiguracionOrigen(origen);
                }
                else
                {
                    //Se valida que no exista un origen con el mismo nombre
                    var existeOrigen = origenProxy.ExisteOrigen(origen.Nombre);
                    if (existeOrigen)
                    {
                        var respuestaSP = new
                        {
                            resultado = true,
                            validacion = 3
                        };

                        return Json(respuestaSP);
                    }

                    //Se valida la existencia de la tabla
                    var existeTabla = comunProxy.ValidarExistenciaTabla(origen.EsquemaTabla, origen.NombreTabla);
                    if (existeTabla)
                    {
                        var respuestaSP = new
                        {
                            resultado = true,
                            validacion = 1
                        };

                        return Json(respuestaSP);
                    }

                    resultado = origenProxy.InsertarConfiguracionOrigen(origen);
                }

                origenProxy.CrearEstructura(origen.IdOrigenDato);

                var respuesta = new
                {
                    resultado,
                    validacion = 0
                };

                return Json(respuesta);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                var respuesta = new
                {
                    resultado = false
                };

                return Json(respuesta);

            }
        }

        public IActionResult Visor()
        {
            try
            {
                var viewModel = new VisorViewModel
                {
                    Origenes = origenProxy.ObtenerOrigenes().ToList()
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return View();
            }

        }

        public IActionResult ObtenerDatos(int id)
        {
            try
            {
                var origen = origenProxy.ObtenerConfiguracionOrigen(id);

                var columnas = origen.Campos.OrderBy(x => x.Orden).Select(x => new Columna
                {
                    Field = string.Format("{0}{1}", char.ToLowerInvariant(x.Nombre[0]), x.Nombre.Substring(1)),
                    HeaderName = x.Nombre,
                    Width = 120
                }).ToList();

                var datos = origenProxy.ObtenerDatosTabla(id).ToList();

                return Json(new { datos, columnas });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Json(null);
            }
        }
    }
}