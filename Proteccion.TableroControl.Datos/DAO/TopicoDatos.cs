using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Proteccion.TableroControl.Datos.DataContext;
using Proteccion.TableroControl.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Proteccion.TableroControl.Datos.DAO
{
    public class TopicoDatos : ITopicoDatos
    {
        private readonly TableroControlContext context;

        #region "Constructor"

        public TopicoDatos(TableroControlContext context)
        {
            this.context = context;
        }

        #endregion

        public IEnumerable<Topico> ObtenerTopicos()
        {
            var listado = context.Topico;
            return listado;
        }

        public Topico ObtenerTopico(int idTopico)
        {
            var topico = context.Topico.Single(x=> x.IdTopico == idTopico);
            return topico;
        }

        public IEnumerable<string> ObtenerIdentificadoresTopicos()
        {
            var listado = context.Topico.Select(x=> x.Identificador).Distinct();
            return listado;
        }


        public bool InsertarTopico(Topico topico)
        {
            topico.FechaActualizacion = DateTime.Now;
            context.Topico.Add(topico);
            return context.SaveChanges() > 0;
        }

        public bool ActualizarTopico(Topico topico)
        {
            var topicoActualizar = context.Topico.Single(x => x.IdTopico == topico.IdTopico);
            topicoActualizar.FechaActualizacion = DateTime.Now;
            topicoActualizar.Identificador = topico.Identificador;
            topicoActualizar.Valor = topico.Valor;
            topicoActualizar.Orden = topico.Orden;
            topicoActualizar.TextoMostrar = topico.TextoMostrar;
            return context.SaveChanges() > 0;
        }

        public bool EliminarTopico(int idTopico)
        {
            var topicoEliminar = context.Topico.Single(x => x.IdTopico == idTopico);
            context.Topico.Remove(topicoEliminar);
            return context.SaveChanges() > 0;
        }

        public bool ExisteTopico(string identificador,string valor)
        {
            var existe = context.Topico.Any(x => x.Valor.ToLower(CultureInfo.InvariantCulture) == valor.ToLower(CultureInfo.InvariantCulture) && x.Identificador == identificador);
            return existe;
        }

        public int ObtenerUltimoOrden(string identificador)
        {
            var orden = context.Topico.Where(x => x.Identificador == identificador).Max(x => x.Orden);
            return (int)orden + 1;
        }
    }
}
