using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Proteccion.TableroControl.Dominio.Entidades;
using Proteccion.TableroControl.Dominio.Enumeraciones;

namespace Proteccion.TableroControl.Proxy.BL
{
    public interface IOrigenDatosProxy
    {
        bool ActualizarConfiguracionOrigen(OrigenDato configuracion);
        List<EjecucionImportacion> ConsultarEjecuciones();
        void CrearEstructura(int idOrigen);
        EjecucionImportacion EjecutarImportacion(int idConfiguracion, TipoOrigen tipo, bool validarDatos);
        bool EliminarOrigen(int idOrigen);
        bool ExisteOrigen(string nombre);
        bool InsertarConfiguracionOrigen(OrigenDato configuracion);
        IEnumerable<OrigenDato> ObtenerConfiguracionesOrigen();
        OrigenDato ObtenerConfiguracionOrigen(int idConfiguracion);
        JArray ObtenerDatosTabla(int idOrigen);
        IEnumerable<OrigenDato> ObtenerOrigenes();
    }
}