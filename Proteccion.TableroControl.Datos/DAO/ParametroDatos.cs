using Microsoft.Extensions.Configuration;
using Proteccion.TableroControl.Datos.DataContext;
using Proteccion.TableroControl.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Proteccion.TableroControl.Datos.DAO
{
    public class ParametroDatos : IParametroDatos
    {
        private readonly TableroControlContext context;
       
        #region "Constructor"

        public ParametroDatos(TableroControlContext context)
        {
            this.context = context;
        }

        #endregion

        public IEnumerable<Parametro> ObtenerParametros()
        {
            var listado = context.Parametro;
            return listado;
        }

        public Parametro ObtenerParametro(int idParametro)
        {
            var parametro = context.Parametro.Single(x=> x.IdParametro == idParametro);
            return parametro;
        }

        public Parametro ObtenerParametro(string identificador)
        {
            var parametro = context.Parametro.Single(x => x.Nombre == identificador);
            return parametro;
        }

        public bool InsertarParametro(Parametro parametro)
        {
            parametro.FechaActualizacion = DateTime.Now;
            context.Parametro.Add(parametro);
            return context.SaveChanges() > 0;
        }

        public bool ActualizarParametro(Parametro parametro)
        {
            var parametroActualizar = context.Parametro.Single(x => x.IdParametro == parametro.IdParametro);
            parametroActualizar.FechaActualizacion = DateTime.Now;
            parametroActualizar.Nombre = parametro.Nombre;
            parametroActualizar.Valor = parametro.Valor;
            return context.SaveChanges() > 0;
        }

        public bool EliminarParametro(int idParametro)
        {
            var parametroEliminar = context.Parametro.Single(x => x.IdParametro == idParametro);
            context.Parametro.Remove(parametroEliminar);
            return context.SaveChanges() > 0;
        }

        public bool ExisteParametro(string nombre)
        {
            var existe = context.Parametro.Any(x => x.Nombre.ToLower(CultureInfo.InvariantCulture) == nombre.ToLower(CultureInfo.InvariantCulture));
            return existe;
        }

        public bool ActualizarFechaProceso(string fecha)
        {
            var parametro = ObtenerParametro("FechaProceso");
            parametro.Valor = fecha;
            parametro.FechaActualizacion = DateTime.Now;
            return context.SaveChanges() > 0;
        }
    }
}
