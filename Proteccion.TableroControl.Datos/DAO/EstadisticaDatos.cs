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
using System.Text;

namespace Proteccion.TableroControl.Datos.DAO
{
    public class EstadisticaDatos : IEstadisticaDatos
    {
        private readonly TableroControlContext context;
        private readonly Database Contexto;

        #region "Constructor"

        public EstadisticaDatos(TableroControlContext context)
        {
            this.context = context;
            string connectionString = context.Database?.GetDbConnection()?.ConnectionString;
            Contexto = new SqlDatabase(connectionString ?? "Prueba");
        }

        #endregion

        #region Métodos

        public bool InsertarEjecucionValidacion(EjecucionValidacion ejecucion)
        {
            try
            {
                context.EjecucionValidacion.Add(ejecucion);
                context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Obtiene la ultma Ejecucion de una validacion para un tipo de validacion especifico
        /// </summary>
        /// <param name="ejecucion"></param>
        /// <returns>Object EjecucionValidacion</returns>
        public List<EjecucionValidacion> ObtenerUltimaEjecucionValidacion(int[] IdValidacion)
        {
            List<EjecucionValidacion> _lista = new List<EjecucionValidacion>();

            foreach (var item in IdValidacion)
            {
                try
                {
                    using (DbCommand cmd = Contexto.GetStoredProcCommand("Validacion_ObtenerUltimasValidaciones"))
                    {
                        cmd.CommandTimeout = 0;
                        Contexto.AddInParameter(cmd, "@IdValidacion", DbType.Int32, item);

                        using (var reader = Contexto.ExecuteReader(cmd))
                        {
                            var ejecucion = new EjecucionValidacion();
                            if (reader.Read())
                            {
                                ejecucion.Usuario = reader["Usuario"].ToString();
                                ejecucion.FechaEjecucion = Convert.ToDateTime(reader["FechaEjecucion"].ToString());
                                ejecucion.TipoValidacion = reader["IdValidacion"].ToString();
                                _lista.Add(ejecucion);
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    return new List<EjecucionValidacion>();
                }
            }

            return _lista;
        }

        /// <summary>
        /// Permite consultar el detalle de una validación especifica
        /// </summary>
        /// <param name="idValidacion"></param>
        /// <returns></returns>
        public void GenerarEstadisticas()
        {
            using (DbCommand cmd = Contexto.GetStoredProcCommand("Validacion_GenerarEstadisticas"))
            {
                Contexto.ExecuteNonQuery(cmd);
            }
        }

        #endregion
    }
}
