using System.Collections.Generic;
using Proteccion.TableroControl.Dominio.Entidades;

namespace Proteccion.TableroControl.Datos.DAO
{
    public interface IComunDatos
    {
        Parametro ObtenerParametro(string identificador);
        List<Topico> ObtenerTopicos(string identificador);
        bool ValidarExistenciaSp(string esquema, string procedimiento);
        bool ValidarExistenciaTabla(string esquema, string tabla);
    }
}