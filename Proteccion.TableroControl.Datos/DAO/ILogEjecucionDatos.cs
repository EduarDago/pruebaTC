using Proteccion.TableroControl.Dominio.Entidades;

namespace Proteccion.TableroControl.Datos.DAO
{
    public interface ILogEjecucionDatos
    {
        string EjecutarQuery(string sql);
        bool InsertarLog(LogEjecucion logEjecucion);
    }
}