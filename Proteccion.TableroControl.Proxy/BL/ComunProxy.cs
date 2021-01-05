using Microsoft.Extensions.Configuration;
using Proteccion.TableroControl.Datos.DAO;
using Proteccion.TableroControl.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proteccion.TableroControl.Proxy.BL
{
    public class ComunProxy : IComunProxy
    {
        private readonly IComunDatos comun;

        public ComunProxy(IComunDatos comun)
        {
            this.comun = comun;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="esquema"></param>
        /// <param name="procedimiento"></param>
        /// <returns></returns>
        public bool ValidarExistenciaSp(string esquema, string procedimiento)
        {
            return comun.ValidarExistenciaSp(esquema, procedimiento);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="esquema"></param>
        /// <param name="tabla"></param>
        /// <returns></returns>
        public bool ValidarExistenciaTabla(string esquema, string tabla)
        {
            return comun.ValidarExistenciaTabla(esquema, tabla);
        }

        /// <summary>
        /// Permite consultar los topicos asociados a un identificador
        /// </summary>
        /// <param name="identificador"></param>
        /// <returns></returns>
        public List<Topico> ObtenerTopicos(string identificador)
        {
            return comun.ObtenerTopicos(identificador);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="identificador"></param>
        /// <returns></returns>
        public Parametro ObtenerParametro(string identificador)
        {
            return comun.ObtenerParametro(identificador);
        }
    }
}
