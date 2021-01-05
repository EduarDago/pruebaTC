using System.Collections.Generic;
using Proteccion.TableroControl.Dominio.Entidades;

namespace Proteccion.TableroControl.Proxy.BL
{
    public interface ITopicoProxy
    {
        bool ActualizarTopico(Topico topico);
        bool EliminarTopico(int idTopico);
        bool ExisteTopico(string identificador, string valor);
        bool InsertarTopico(Topico topico);
        IEnumerable<string> ObtenerIdentificadoresTopicos();
        Topico ObtenerTopico(int idTopico);
        IEnumerable<Topico> ObtenerTopicos();
        int ObtenerUltimoOrden(string identificador);
    }
}