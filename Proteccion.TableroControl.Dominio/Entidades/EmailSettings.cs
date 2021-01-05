using System;
using System.Collections.Generic;
using System.Text;

namespace Proteccion.TableroControl.Dominio.Entidades
{
    public class EmailSettings
    {
        public string PrimaryDomain { get; set; }

        public string FromEmail { get; set; }

        public string AliasEmail { get; set; }
    }
}
