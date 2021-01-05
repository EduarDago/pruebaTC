using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using Proteccion.TableroControl.Datos.DAO;
using Proteccion.TableroControl.Datos.DataContext;
using Proteccion.TableroControl.Dominio.Entidades;
using Proteccion.TableroControl.Dominio.Enumeraciones;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Proteccion.TableroControl.Proxy.BL
{
    public class OrigenDatosProxy : IOrigenDatosProxy
    {
        private readonly IOrigenDatos datos;

        public OrigenDatosProxy(IOrigenDatos datos)
        {
            this.datos = datos;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<OrigenDato> ObtenerConfiguracionesOrigen()
        {
            return datos.ObtenerConfiguracionesOrigen();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<OrigenDato> ObtenerOrigenes()
        {
            return datos.ObtenerOrigenes();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public OrigenDato ObtenerConfiguracionOrigen(int idConfiguracion)
        {
            return datos.ObtenerConfiguracionOrigen(idConfiguracion);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idOrigen"></param>
        /// <returns></returns>
        public bool EliminarOrigen(int idOrigen)
        {
            return datos.EliminarOrigen(idOrigen);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idConfiguracion"></param>
        /// <returns></returns>
        public List<EjecucionImportacion> ConsultarEjecuciones()
        {
            return datos.ConsultarEjecuciones();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idConfiguracion"></param>
        /// <returns></returns>
        public EjecucionImportacion EjecutarImportacion(int idConfiguracion,TipoOrigen tipo, bool validarDatos)
        {
            return datos.EjecutarImportacion(idConfiguracion, tipo, validarDatos);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuracion"></param>
        /// <returns></returns>
        public bool InsertarConfiguracionOrigen(OrigenDato configuracion)
        {
            return datos.InsertarConfiguracionOrigen(configuracion);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuracion"></param>
        /// <returns></returns>
        public bool ActualizarConfiguracionOrigen(OrigenDato configuracion)
        {
            return datos.ActualizarConfiguracionOrigen(configuracion);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idOrigen"></param>
        public void CrearEstructura(int idOrigen)
        {
            datos.CrearEstructura(idOrigen);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idOrigen"></param>
        /// <returns></returns>
        public JArray ObtenerDatosTabla(int idOrigen)
        {
            return datos.ObtenerDatosTabla(idOrigen);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idOrigen"></param>
        /// <returns></returns>
        public bool ExisteOrigen(string nombre)
        {
            return datos.ExisteOrigen(nombre);
        }
    }
}
