using Proteccion.TableroControl.Dominio.Enumeraciones;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proteccion.TableroControl.Dominio.Entidades
{
    public class Notificacion
    {
        public string Mensaje { get; set; }
        public TipoNotificacion TipoMensaje { get; set; }
        public TipoVentana TipoVentana { get; set; }
        public string Header { get; set; }
        public bool Redireccionar { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
    }
}
