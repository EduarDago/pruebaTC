using Microsoft.Extensions.Configuration;
using Proteccion.TableroControl.Datos.DAO;
using Proteccion.TableroControl.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proteccion.TableroControl.Proxy.BL
{
    public class LogEjecucionProxy : ILogEjecucionProxy
    {
        private readonly ILogEjecucionDatos datos;

        public LogEjecucionProxy(ILogEjecucionDatos datos)
        {
            this.datos = datos;
        }

        public bool InsertarLog(string usuario, string script, string resultado)
        {
            var log = new LogEjecucion() { FechaEjecucion = DateTime.Now,
                Usuario = usuario,
                Script = script,
                ResultadoScript = resultado};
            return datos.InsertarLog(log);
        }

        public string EjecutarQuery(string sql)
        {
            return datos.EjecutarQuery(sql);
        }
    }
}
