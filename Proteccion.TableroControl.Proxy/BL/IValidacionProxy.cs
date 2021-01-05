using System.Collections.Generic;
using Proteccion.TableroControl.Dominio.Entidades;

namespace Proteccion.TableroControl.Proxy.BL
{
    public interface IValidacionProxy
    {
        bool ActualizarValidacion(Validacion validacion);
        ResultadoEjecucionValidacion EjecutarValidacion(int idValidacion, string usuario);
        bool EliminarValidacion(int idvalidacion);
        bool InsertarValidacion(Validacion validacion);
        List<Inconsistencia> ObtenerInconsistencias(int idValidacion, string usuairo);
        Validacion ObtenerValidacion(int id);
        IEnumerable<Validacion> ObtenerValidaciones();
        IEnumerable<Validacion> ObtenerValidacionesActivas(int equipo, int tipo);
        bool ValidarExistencia(string nombre);
    }
}