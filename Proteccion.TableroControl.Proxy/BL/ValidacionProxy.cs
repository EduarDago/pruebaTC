using Proteccion.TableroControl.Datos.DAO;
using Proteccion.TableroControl.Dominio.Entidades;
using System.Collections.Generic;

namespace Proteccion.TableroControl.Proxy.BL
{
    public class ValidacionProxy : IValidacionProxy
    {
        private readonly IValidacionDatos datos;

        public ValidacionProxy(IValidacionDatos datos)
        {
            this.datos = datos;
        }

        /// <summary>
        /// Permite consultar las validaciones configuradas
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Validacion> ObtenerValidaciones()
        {
            return datos.ObtenerValidaciones();
        }

        /// <summary>
        /// Permite consultar las validaciones configuradas
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Validacion> ObtenerValidacionesActivas(int equipo, int tipo)
        {
            return datos.ObtenerValidacionesActivas(equipo, tipo);
        }

        public Validacion ObtenerValidacion(int id)
        {
            return datos.ObtenerValidacion(id);
        }

        /// <summary>
        /// Permite insertar un registro nuevo en la tabla de validaciones
        /// </summary>
        /// <param name="validacion"></param>
        /// <returns></returns>
        public bool InsertarValidacion(Validacion validacion)
        {
            return datos.InsertarValidacion(validacion);
        }

        /// <summary>
        /// Actualiza un registro de la tabla validaciones
        /// </summary>
        /// <param name="validacion"></param>
        /// <returns></returns>
        public bool ActualizarValidacion(Validacion validacion)
        {
            return datos.ActualizarValidacion(validacion);
        }

        /// <summary>
        /// Elimina un registro de la tabla validaciones
        /// </summary>
        /// <param name="idvalidacion"></param>
        /// <returns></returns>
        public bool EliminarValidacion(int idvalidacion)
        {
            return datos.EliminarValidacion(idvalidacion);
        }

        /// <summary>
        /// Valida la existencia de un nombre de validación 
        /// </summary>
        /// <param name="nombre"></param>
        /// <returns></returns>
        public bool ValidarExistencia(string nombre)
        {
            return datos.Existe_Validacion(nombre);
        }

        public ResultadoEjecucionValidacion EjecutarValidacion(int idValidacion, string usuario)
        {
            return datos.EjecutarValidacion(idValidacion, usuario);
        }

        public List<Inconsistencia> ObtenerInconsistencias(int idValidacion, string usuairo)
        {
            return datos.ObtenerInconsistencias(idValidacion, usuairo);
        }
    }
}
