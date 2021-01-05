using Microsoft.EntityFrameworkCore;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Proteccion.TableroControl.Datos.DataContext;
using Proteccion.TableroControl.Dominio.Entidades;
using System;
using System.Data.Common;

namespace Proteccion.TableroControl.Datos.DAO
{
    public class LogEjecucionDatos : ILogEjecucionDatos
    {
        private readonly TableroControlContext context;
        private readonly Database Contexto;

        #region "Constructor"

        public LogEjecucionDatos(TableroControlContext context)
        {
            this.context = context;
            string connectionString = context.Database?.GetDbConnection()?.ConnectionString;
            Contexto = new SqlDatabase(connectionString ?? "Prueba");
        }

        #endregion

        public string EjecutarQuery(string sql)
        {
            string resultado = string.Empty;

            using (DbCommand cmd = Contexto.GetSqlStringCommand(sql))
            {
                try
                {
                    Contexto.ExecuteNonQuery(cmd);
                    resultado = "Comandos ejecutados correctamente";
                }
                catch (Exception ex)
                {
                    resultado = ex.Message;
                }
            }

            return resultado;
        }

        public bool InsertarLog(LogEjecucion logEjecucion)
        {
            context.LogEjecucion.Add(logEjecucion);
            return context.SaveChanges() > 0;
        }
    }
}
