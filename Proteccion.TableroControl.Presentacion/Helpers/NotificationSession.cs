using Microsoft.AspNetCore.Http;
using Proteccion.TableroControl.Dominio.Entidades;
using Proteccion.TableroControl.Dominio.Interfaces;

namespace Proteccion.TableroControl.Presentacion.Helpers
{
    public class ManagerSession : IManagerSession
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;

        public ManagerSession(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string FechaProceso { get => _session.GetString("FechaProceso"); set => _session.SetString("FechaProceso", value); }
        public Notificacion Notificacion { get => _session.GetObjectFromJson<Notificacion>("Notificacion"); set => _session.SetObjectAsJson("Notificacion", value); }

        public void EliminarSession(string key)
        {
            _httpContextAccessor.HttpContext.Session.Remove(key);
        }
    }
}
