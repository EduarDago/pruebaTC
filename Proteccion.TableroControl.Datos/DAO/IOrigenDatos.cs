using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Proteccion.TableroControl.Dominio.Entidades;
using Proteccion.TableroControl.Dominio.Enumeraciones;

namespace Proteccion.TableroControl.Datos.DAO
{
    public interface IOrigenDatos
    {
        bool ActualizarConfiguracionOrigen(OrigenDato origen);
        List<EjecucionImportacion> ConsultarEjecuciones();
        void CrearEstructura(int idOrigen);
        EjecucionImportacion EjecutarImportacion(int idOrigen, TipoOrigen tipo, bool validarDatos);
        bool EliminarOrigen(int idOrigen);
        bool ExisteOrigen(string nombre);
        bool GuardarEjecucion(EjecucionOrigen ejecucion);
        bool InsertarConfiguracionOrigen(OrigenDato origen);
        IEnumerable<OrigenDato> ObtenerConfiguracionesOrigen();
        OrigenDato ObtenerConfiguracionOrigen(int idOrigen);
        JArray ObtenerDatosTabla(int idOrigen);
        IEnumerable<OrigenDato> ObtenerOrigenes();
    }
}