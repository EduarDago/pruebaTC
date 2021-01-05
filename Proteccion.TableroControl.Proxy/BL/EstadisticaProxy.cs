using Microsoft.Extensions.Configuration;
using Proteccion.TableroControl.Datos.DAO;
using Proteccion.TableroControl.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proteccion.TableroControl.Proxy.BL
{
    public class EstadisticaProxy : IEstadisticaProxy
    {
        private readonly IEstadisticaDatos datos;

        public EstadisticaProxy(IEstadisticaDatos datos)
        {
            this.datos = datos;
        }
        
        public bool InsertarEjecucionValidacion(EjecucionValidacion ejecucion)
        {
            return datos.InsertarEjecucionValidacion(ejecucion);
        }

        public void GenerarEstadisticas()
        {
            datos.GenerarEstadisticas();
        }

        public List<EjecucionValidacion> ObtenerUltimaEjecucionValidacion(int[] IdValidacion)
        {
            return datos.ObtenerUltimaEjecucionValidacion(IdValidacion);
        }
    }
}
