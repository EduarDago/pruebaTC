using Proteccion.TableroControl.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proteccion.TableroControl.Dominio.Interfaces
{
    public interface IManagerSession
    {
        Notificacion Notificacion { get; set; }

        string FechaProceso { get; set; }

        void EliminarSession(string key);
    }
}
