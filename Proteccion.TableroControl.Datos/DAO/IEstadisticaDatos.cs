using Proteccion.TableroControl.Dominio.Entidades;
using System.Collections.Generic;

namespace Proteccion.TableroControl.Datos.DAO
{
    public interface IEstadisticaDatos
    {
        void GenerarEstadisticas();
        bool InsertarEjecucionValidacion(EjecucionValidacion ejecucion);
        List<EjecucionValidacion> ObtenerUltimaEjecucionValidacion(int[] IdValidacion);
    }
}