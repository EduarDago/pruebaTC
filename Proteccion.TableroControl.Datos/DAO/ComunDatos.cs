using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Newtonsoft.Json.Linq;
using Proteccion.TableroControl.Datos.DataContext;
using Proteccion.TableroControl.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace Proteccion.TableroControl.Datos.DAO
{
    public class ComunDatos : IComunDatos
    {
        #region "Variables"

        private readonly Database Contexto;
        private readonly string connectionString;

        #endregion

        #region "Constructor"

        public ComunDatos(TableroControlContext context)
        {
            connectionString = context.Database?.GetDbConnection()?.ConnectionString;
            Contexto = new SqlDatabase(connectionString ?? "Prueba");
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="esquema"></param>
        /// <param name="procedimiento"></param>
        /// <returns></returns>
        public bool ValidarExistenciaSp(string esquema, string procedimiento)
        {
            try
            {
                bool existe = false;
                using (DbCommand cmd = Contexto.GetStoredProcCommand("Configuracion_ValidarSP"))
                {
                    Contexto.AddInParameter(cmd, "@Esquema", DbType.String, esquema);
                    Contexto.AddInParameter(cmd, "@Sp", DbType.String, procedimiento);

                    using (var reader = Contexto.ExecuteReader(cmd))
                    {
                        if (reader.Read())
                        {
                            existe = Convert.ToBoolean(reader["Resultado"].ToString());
                        }
                    }
                }

                return existe;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="esquema"></param>
        /// <param name="procedimiento"></param>
        /// <returns></returns>
        public bool ValidarExistenciaTabla(string esquema, string tabla)
        {
            try
            {
                bool existe = false;
                using (DbCommand cmd = Contexto.GetStoredProcCommand("Configuracion_ValidarTabla"))
                {
                    Contexto.AddInParameter(cmd, "@Esquema", DbType.String, esquema);
                    Contexto.AddInParameter(cmd, "@NombreTabla", DbType.String, tabla);

                    using (var reader = Contexto.ExecuteReader(cmd))
                    {
                        if (reader.Read())
                        {
                            existe = Convert.ToBoolean(reader["Resultado"].ToString());
                        }
                    }
                }

                return existe;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Permite consultar los topicos asociados a un identificador
        /// </summary>
        /// <param name="identificador"></param>
        /// <returns></returns>
        public List<Topico> ObtenerTopicos(string identificador)
        {
            try
            {
                using (var ctx = new TableroControlContext(connectionString))
                {
                    var listado = ctx.Topico.Where(x => x.Identificador == identificador).ToList();
                    return listado;
                }
            }
            catch
            {
                return new List<Topico>();
            }
        }

        /// <summary>
        /// Permite consultar los topicos asociados a un identificador
        /// </summary>
        /// <param name="identificador"></param>
        /// <returns></returns>
        public Parametro ObtenerParametro(string identificador)
        {
            try
            {
                using (var ctx = new TableroControlContext(connectionString))
                {
                    var parametro = ctx.Parametro.Single(x => x.Nombre == identificador);
                    return parametro;
                }
            }
            catch
            {
                return null;
            }
        }
    }
}