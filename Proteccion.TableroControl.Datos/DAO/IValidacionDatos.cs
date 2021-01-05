using System.Collections.Generic;
using Proteccion.TableroControl.Dominio.Entidades;

namespace Proteccion.TableroControl.Datos.DAO
{
    public interface IValidacionDatos
    {
        bool ActualizarValidacion(Validacion validacion);
        ResultadoEjecucionValidacion EjecutarValidacion(int idValidacion, string usuario);
        bool EliminarValidacion(int idvalidacion);
        bool Existe_Validacion(string nombre);
        bool InsertarValidacion(Validacion validacion);
        List<Inconsistencia> ObtenerInconsistencias(int idValidacion, string usuario);
        Validacion ObtenerValidacion(int idvalidacion);
        IEnumerable<Validacion> ObtenerValidaciones();
        IEnumerable<Validacion> ObtenerValidacionesActivas(int equipo, int tipo);
    }
}