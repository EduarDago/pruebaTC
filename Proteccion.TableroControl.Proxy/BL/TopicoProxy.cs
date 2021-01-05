using Microsoft.Extensions.Configuration;
using Proteccion.TableroControl.Datos.DAO;
using Proteccion.TableroControl.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proteccion.TableroControl.Proxy.BL
{
    public class TopicoProxy : ITopicoProxy
    {
        private readonly ITopicoDatos datos;

        public TopicoProxy(ITopicoDatos datos)
        {
            this.datos = datos;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Topico> ObtenerTopicos()
        {
            return datos.ObtenerTopicos();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idTopico"></param>
        /// <returns></returns>
        public Topico ObtenerTopico(int idTopico)
        {
            return datos.ObtenerTopico(idTopico);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> ObtenerIdentificadoresTopicos()
        {
            return datos.ObtenerIdentificadoresTopicos();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="topico"></param>
        /// <returns></returns>
        public bool InsertarTopico(Topico topico)
        {
            return datos.InsertarTopico(topico);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="topico"></param>
        /// <returns></returns>
        public bool ActualizarTopico(Topico topico)
        {
            return datos.ActualizarTopico(topico);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idTopico"></param>
        /// <returns></returns>
        public bool EliminarTopico(int idTopico)
        {
            return datos.EliminarTopico(idTopico);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="identificador"></param>
        /// <param name="valor"></param>
        /// <returns></returns>
        public bool ExisteTopico(string identificador, string valor)
        {
            return datos.ExisteTopico(identificador, valor);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="identificador"></param>
        /// <returns></returns>
        public int ObtenerUltimoOrden(string identificador)
        {
            return datos.ObtenerUltimoOrden(identificador);
        }
    }
}
