using Proteccion.TableroControl.Dominio.Entidades;
using System.Collections.Generic;

namespace Proteccion.TableroControl.Proxy.BL
{
    public interface IEstadisticaProxy
    {
        void GenerarEstadisticas();
        bool InsertarEjecucionValidacion(EjecucionValidacion ejecucion);
        List<EjecucionValidacion> ObtenerUltimaEjecucionValidacion(int[] IdValidacion);
    }
}