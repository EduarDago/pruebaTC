using System.Collections.Generic;
using Proteccion.TableroControl.Dominio.Entidades;

namespace Proteccion.TableroControl.Proxy.BL
{
    public interface IParametroProxy
    {
        bool ActualizarFechaProceso(string fecha);
        bool ActualizarParametro(Parametro parametro);
        bool EliminarParametro(int idParametro);
        bool ExisteParametro(string nombre);
        bool InsertarParametro(Parametro parametro);
        Parametro ObtenerParametro(int idParametro);
        Parametro ObtenerParametro(string identificador);
        IEnumerable<Parametro> ObtenerParametros();
    }
}