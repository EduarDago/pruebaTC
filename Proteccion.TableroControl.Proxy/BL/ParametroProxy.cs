using Microsoft.Extensions.Configuration;
using Proteccion.TableroControl.Datos.DAO;
using Proteccion.TableroControl.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proteccion.TableroControl.Proxy.BL
{
    public class ParametroProxy : IParametroProxy
    {
        private readonly IParametroDatos datos;

        public ParametroProxy(IParametroDatos datos)
        {
            this.datos = datos;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Parametro> ObtenerParametros()
        {
            return datos.ObtenerParametros();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idParametro"></param>
        /// <returns></returns>
        public Parametro ObtenerParametro(int idParametro)
        {
            return datos.ObtenerParametro(idParametro);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idParametro"></param>
        /// <returns></returns>
        public Parametro ObtenerParametro(string identificador)
        {
            return datos.ObtenerParametro(identificador);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parametro"></param>
        /// <returns></returns>
        public bool InsertarParametro(Parametro parametro)
        {
            return datos.InsertarParametro(parametro);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parametro"></param>
        /// <returns></returns>
        public bool ActualizarParametro(Parametro parametro)
        {
            return datos.ActualizarParametro(parametro);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idParametro"></param>
        /// <returns></returns>
        public bool EliminarParametro(int idParametro)
        {
            return datos.EliminarParametro(idParametro);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nombre"></param>
        /// <returns></returns>
        public bool ExisteParametro(string nombre)
        {
            return datos.ExisteParametro(nombre);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public bool ActualizarFechaProceso(string fecha)
        {
            return datos.ActualizarFechaProceso(fecha);
        }
    }
}
