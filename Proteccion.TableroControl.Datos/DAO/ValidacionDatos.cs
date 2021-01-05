using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Proteccion.TableroControl.Datos.DataContext;
using Proteccion.TableroControl.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace Proteccion.TableroControl.Datos.DAO
{
    public class ValidacionDatos : IValidacionDatos
    {
        #region Variables

        private readonly TableroControlContext context;
        private readonly Database Contexto;
        private readonly string connectionString;

        #endregion

        #region Constructor

        public ValidacionDatos(TableroControlContext context)
        {
            this.context = context;
            connectionString = context.Database?.GetDbConnection()?.ConnectionString;
            Contexto = new SqlDatabase(connectionString ?? "Prueba");
        }

        #endregion

        #region "Metodos"

        /// <summary>
        /// Permite consultar el detalle de una validación especifica
        /// </summary>
        /// <param name="idValidacion"></param>
        /// <returns></returns>
        public ResultadoEjecucionValidacion EjecutarValidacion(int idValidacion, string usuario)
        {
            try
            {
                using (DbCommand cmd = Contexto.GetStoredProcCommand("Validacion_EjecutarValidaciones"))
                {
                    cmd.CommandTimeout = 0;
                    Contexto.AddInParameter(cmd, "@IdValidacion", DbType.Int32, idValidacion);
                    Contexto.AddInParameter(cmd, "@Usuario", DbType.String, usuario);

                    using (var reader = Contexto.ExecuteReader(cmd))
                    {
                        var ejecucion = new ResultadoEjecucionValidacion();

                        int cantidad = 0;
                        if (reader.Read())
                        {

                            ejecucion.Estado = reader["Estado"].ToString();
                            ejecucion.NombreValidacion = reader["Nombre"].ToString();

                            cantidad = Convert.ToInt32(reader["Cantidad"].ToString());
                            ejecucion.Exitoso = cantidad == 0;
                            ejecucion.IdValidacion = idValidacion;

                            if (cantidad > 0)
                                ejecucion.Inconsistencias = ObtenerInconsistencias(idValidacion, usuario);
                        }

                        return ejecucion;
                    }
                }
            }
            catch
            {
                return null;
            }
            
        }

        /// <summary>
        /// Permite consultar el detalle de una validación especifica
        /// </summary>
        /// <param name="idValidacion"></param>
        /// <returns></returns>
        public List<Inconsistencia> ObtenerInconsistencias(int idValidacion, string usuario)
        {
            try
            {
                using (var ctx = new TableroControlContext(connectionString))
                {
                    var listado = ctx.Inconsistencia.Where(x => x.IdValidacion == idValidacion && x.Fecha.ToShortDateString() == DateTime.Now.ToShortDateString() && x.Usuario == usuario).ToList();
                    return listado;
                }
            }
            catch (Exception)
            {
                return new List<Inconsistencia>();
            }
            
        }

        /// <summary>
        /// Lista de registros de la tabla validaciones
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Validacion> ObtenerValidaciones()
        {
            var listado = context.Validacion
                .Include(x => x.Equipo)
                .Include(x => x.TipoValidacion);

            return listado;
        }

        /// <summary>
        /// Lista de registros de la tabla validaciones
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Validacion> ObtenerValidacionesActivas(int equipo, int tipo)
        {
            var listado = context.Validacion
                .Where(x => x.Activo
                        && (tipo == 0 || x.IdTipoValidacion == tipo)
                        && (equipo == 0 || x.IdEquipo == equipo)
                        );

            return listado;
        }

        public Validacion ObtenerValidacion(int idvalidacion)
        {
            var validacion = context.Validacion
                .Include(x => x.Equipo)
                .Include(x => x.TipoValidacion)
                .Single(v => v.IdValidacion.Equals(idvalidacion));

            return validacion;
        }

        /// <summary>
        /// Inserta un nuevo item en la tabla de validaciones
        /// </summary>
        /// <param name="validacion"></param>
        /// <returns></returns>
        public bool InsertarValidacion(Validacion validacion)
        {
            context.Validacion.Add(validacion);
            return context.SaveChanges() > 0;
        }

        /// <summary>
        /// Actualiza un registro de la tabla validaciones
        /// </summary>
        /// <param name="validacion"></param>
        /// <returns></returns>
        public bool ActualizarValidacion(Validacion validacion)
        {
            var validacion_datos = context.Validacion.Single(v => v.IdValidacion.Equals(validacion.IdValidacion));
            validacion_datos.IdTipoValidacion = validacion.IdTipoValidacion;
            validacion_datos.IdEquipo = validacion.IdEquipo;
            validacion_datos.Nombre = validacion.Nombre;
            validacion_datos.Esquema = validacion.Esquema;
            validacion_datos.Sp = validacion.Sp;
            validacion_datos.Activo = validacion.Activo;
            return context.SaveChanges() > 0;
        }

        /// <summary>
        /// Elimina un registro de la tabla validaciones, segun el id del registro
        /// </summary>
        /// <param name="idvalidacion"></param>
        /// <returns></returns>
        public bool EliminarValidacion(int idvalidacion)
        {
            var validacion = context.Validacion.FirstOrDefault(v => v.IdValidacion.Equals(idvalidacion));
            context.Validacion.Remove(validacion);
            return context.SaveChanges() > 0;
        }

        /// <summary>
        /// Valida la existencia de validaciones con un nombre
        /// </summary>
        /// <param name="nombre"></param>
        /// <returns></returns>
        public bool Existe_Validacion(string nombre)
        {
            return context.Validacion.Any(x => x.Nombre.Equals(nombre));
        }

        #endregion
    }
}
