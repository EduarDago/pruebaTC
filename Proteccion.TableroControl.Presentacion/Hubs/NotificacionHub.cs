using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Proteccion.TableroControl.Dominio.Entidades;
using Proteccion.TableroControl.Dominio.Interfaces;
using Proteccion.TableroControl.Proxy.BL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proteccion.TableroControl.Presentacion.Hubs
{
    public class NotificacionHub : Hub
    {
        private readonly IValidacionProxy validacionProxy;
        private readonly IOrigenDatosProxy origenProxy;
        private readonly IEstadisticaProxy estadisticaProxy;
        private readonly IComunProxy comunProxy;
        private readonly IParametroProxy parametroProxy;
        private readonly IEmail envioCorreo;
        private readonly IHostingEnvironment environment;
        private readonly ILogger<NotificacionHub> _logger;

        public NotificacionHub(IEmail envioCorreo
            , IHostingEnvironment hostingEnvironment
            , ILogger<NotificacionHub> logger
            , IParametroProxy parametroProxy
            , IOrigenDatosProxy origenProxy
            , IComunProxy comunProxy
            , IValidacionProxy validacionProxy
            , IEstadisticaProxy estadisticaProxy)
        {
            this.validacionProxy = validacionProxy;
            this.origenProxy = origenProxy;
            this.comunProxy = comunProxy;
            this.parametroProxy = parametroProxy;
            this.estadisticaProxy = estadisticaProxy;
            this.envioCorreo = envioCorreo;
            environment = hostingEnvironment;
            _logger = logger;
        }

        public void EjecutarValidaciones(List<int> ids, string equipo, string tipo, UsuarioLogueado userLogged)
        {
            string usuario = userLogged.Login;
            var validaciones = validacionProxy.ObtenerValidaciones();
            var seleccionadas = validaciones.Where(x => ids.Contains(x.IdValidacion)).ToList();
            List<ResultadoEjecucionValidacion> ejecuciones = new List<ResultadoEjecucionValidacion>();

            Parallel.ForEach(seleccionadas, async (validacion) =>
            {
                try
                {
                    var resultado = validacionProxy.EjecutarValidacion(validacion.IdValidacion, usuario);
                    ejecuciones.Add(resultado);
                    await Clients.All.SendAsync("notificarEjecucion", resultado);
                }
                catch (Exception ex)
                {
                    var resultado = new ResultadoEjecucionValidacion()
                    {
                        IdValidacion = validacion.IdValidacion,
                        Exitoso = false,
                        Estado = ex.Message
                    };

                    await Clients.All.SendAsync("notificarEjecucion", resultado);
                }

            });

            // Se generan las estadisticas de la ejecucion 
            GenerarEstadisticas(ejecuciones, equipo, tipo, usuario);

            try
            {
                //Solo se envia correo cuando falla alguna ejecucion
                var ejecucionesFallidas = ejecuciones.Where(x => !x.Exitoso).ToList();
                if (ejecucionesFallidas.Count > 0)
                {
                    //Envio correo de finalizacion
                    var email = comunProxy.ObtenerParametro("CorreoValidacion")?.Valor;
                    var contenido = ObtenerPlantillaValidaciones(ejecucionesFallidas, userLogged.NombreUsuario);
                    envioCorreo.SendEmailAzureAsync("Finalización Validaciones", contenido, email);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            //Notificar finalizacion del proceso
            Clients.All.SendAsync("notificarFinEjecucion");
        }

        private void GenerarEstadisticas(List<ResultadoEjecucionValidacion> ejecuciones, string equipo, string tipo, string usuario)
        {
            var ejecucion = new EjecucionValidacion
            {
                FechaEjecucion = DateTime.Now,
                Usuario = usuario,
                Equipo = equipo,
                TipoValidacion = tipo
            };

            var detalleEjecuciones = ejecuciones.Select(x => new DetalleEjecucionValidacion
            {
                IdValidacion = x.IdValidacion,
                Cantidad = x.Inconsistencias.Count
            }).ToList();

            ejecucion.DetalleEjecuciones = detalleEjecuciones;

            if (detalleEjecuciones?.Count > 0)
            {
                ejecucion.Cantidad = ejecucion.DetalleEjecuciones.Sum(x => x.Cantidad);
            }

            estadisticaProxy.InsertarEjecucionValidacion(ejecucion);

            //Se generan las estadisticas
            estadisticaProxy.GenerarEstadisticas();
        }

        public void EjecutarImportaciones(List<int> ids, List<int> idsValidar, UsuarioLogueado userLogged)
        {
            var origenes = origenProxy.ConsultarEjecuciones();
            var seleccionados = origenes.Where(x => ids.Contains(x.IdOrigenDato)).ToList();
            List<EjecucionImportacion> ejecuciones = new List<EjecucionImportacion>();

            Parallel.ForEach(seleccionados, async (origen) =>
            {
                try
                {
                    var resultado = origenProxy.EjecutarImportacion(origen.IdOrigenDato, origen.TipoOrigen, idsValidar.Any(x => x == origen.IdOrigenDato));
                    ejecuciones.Add(resultado);
                    await Clients.All.SendAsync("notificarImportacion", resultado);
                }
                catch (Exception ex)
                {
                    var resultado = new EjecucionImportacion()
                    {
                        IdOrigenDato = origen.IdOrigenDato,
                        TipoOrigen = origen.TipoOrigen,
                        Exitoso = false,
                        Estado = ex.Message
                    };

                    await Clients.All.SendAsync("notificarImportacion", resultado);
                }

            });

            try
            {
                //Solo se envia correo cuando falla alguna ejecucion
                var ejecucionesFallidas = ejecuciones.Where(x => !x.Exitoso).ToList();
                if (ejecucionesFallidas.Count > 0)
                {
                    //Envio correo de finalizacion
                    var email = comunProxy.ObtenerParametro("CorreoImportacion")?.Valor;
                    var contenido = ObtenerPlantillaImportacion(ejecucionesFallidas, userLogged.NombreUsuario);
                    envioCorreo.SendEmailAzureAsync("Finalización Importación", contenido, email);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                var resultado = new EjecucionImportacion()
                {
                    IdOrigenDato = ids[0],
                    Exitoso = false,
                    Estado = "Error enviando correo : " + ex.Message
                };

                Clients.All.SendAsync("notificarErrorImportacion", resultado);
            }

            //Notificar finalizacion del proceso
            Clients.All.SendAsync("notificarFinImportacion");
        }

        public void CambiarFechaProceso(DateTime fecha, UsuarioLogueado userLogged)
        {
            var fechatexto = fecha.ToString("yyyy-MM-dd");
            var fechaActual = parametroProxy.ObtenerParametro("FechaProceso")?.Valor;

            if (fechaActual == fechatexto)
            {
                //Notificar finalizacion del proceso
                Clients.All.SendAsync("notificarCambioFecha", new { exitoso = false });
                return;
            }

            var exitoso = parametroProxy.ActualizarFechaProceso(fechatexto);

            var resultado = new { fecha = fechatexto, usuario = userLogged.Login, exitoso };

            //Solo se envia correo si el cambio de fecha fue exitoso
            if (exitoso)
            {
                try
                {
                    //Envio correo de finalizacion
                    string contenido = ObtenerPlantillaCambioCorreo(fechaActual, fechatexto, userLogged.NombreUsuario);
                    string email = comunProxy.ObtenerParametro("CorreoCambioFecha")?.Valor;
                    envioCorreo.SendEmailAzureAsync("Cambio en Fecha Proceso", contenido, email);

                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                }
            }

            //Notificar finalizacion del proceso
            Clients.All.SendAsync("notificarCambioFecha", resultado);
        }

        private string ObtenerPlantillaValidaciones(List<ResultadoEjecucionValidacion> ejecuciones, string nombreUsuario)
        {
            string webRootPath = environment.WebRootPath;
            string url = Path.Combine(webRootPath, "templates/CorreoValidacion.html");

            var plantilla = File.ReadAllText(url);

            StringBuilder cuerpoTabla = new StringBuilder();
            foreach (var ejecucion in ejecuciones)
            {
                int cantidadInconsistencias = ejecucion.Inconsistencias.Count;
                if (cantidadInconsistencias > 0)
                {
                    int indice = 0;
                    foreach (var item in ejecucion.Inconsistencias)
                    {
                        if (indice == 0)
                        {
                            cuerpoTabla.Append("<tr>");
                            cuerpoTabla.Append(string.Format("<td rowspan='{1}'>{0}</td>", ejecucion.NombreValidacion, cantidadInconsistencias));
                            cuerpoTabla.Append(string.Format("<td rowspan='{0}'>{0}</td>", cantidadInconsistencias));
                            cuerpoTabla.Append(string.Format("<td>{0}</td>", item.Detalle));
                            cuerpoTabla.Append("</tr>");
                            indice++;
                        }
                        else
                        {
                            cuerpoTabla.Append("<tr>");
                            cuerpoTabla.Append(string.Format("<td>{0}</td>", item.Detalle));
                            cuerpoTabla.Append("</tr>");
                        }
                    }
                }
                else
                {
                    cuerpoTabla.Append("<tr>");
                    cuerpoTabla.Append(string.Format("<td>{0}</td>", ejecucion.NombreValidacion));
                    cuerpoTabla.Append(string.Format("<td>{0}</td>", ejecucion.Exitoso ? cantidadInconsistencias : -1));
                    cuerpoTabla.Append(string.Format("<td>{0}</td>", ejecucion.Estado));
                    cuerpoTabla.Append("</tr>");
                }
            }

            plantilla = plantilla.Replace("@Usuario", nombreUsuario);
            plantilla = plantilla.Replace("@Fecha", DateTime.Now.ToShortDateString());
            plantilla = plantilla.Replace("@CuerpoTabla", cuerpoTabla.ToString());

            return plantilla;
        }

        private string ObtenerPlantillaImportacion(List<EjecucionImportacion> ejecuciones, string nombreUsuario)
        {
            string webRootPath = environment.WebRootPath;
            string url = Path.Combine(webRootPath, "templates/CorreoImportacion.html");

            var plantilla = File.ReadAllText(url);

            StringBuilder cuerpoTabla = new StringBuilder();
            foreach (var ejecucion in ejecuciones)
            {
                cuerpoTabla.Append("<tr>");
                cuerpoTabla.Append(string.Format("<td>{0}</td>", ejecucion.NombreOrigen));
                cuerpoTabla.Append(string.Format("<td>{0}</td>", ejecucion.Estado));
                cuerpoTabla.Append("</tr>");
            }

            plantilla = plantilla.Replace("@Usuario", nombreUsuario);
            plantilla = plantilla.Replace("@Fecha", DateTime.Now.ToShortDateString());
            plantilla = plantilla.Replace("@CuerpoTabla", cuerpoTabla.ToString());

            return plantilla;
        }

        private string ObtenerPlantillaCambioCorreo(string fechaAnterior, string fechaNueva, string nombreUsuario)
        {
            string webRootPath = environment.WebRootPath;
            string url = Path.Combine(webRootPath, "templates/CorreoCambioFecha.html");

            var plantilla = File.ReadAllText(url);

            plantilla = plantilla.Replace("@Usuario", nombreUsuario);
            plantilla = plantilla.Replace("@FechaAnterior", fechaAnterior);
            plantilla = plantilla.Replace("@FechaNueva", fechaNueva);

            return plantilla;
        }
    }
}
