using System;
using System.Collections.Generic;
using System.Text;

namespace Proteccion.TableroControl.Dominio.Entidades
{
    public class SftpSettings
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int TimeOut { get; set; }

        public string RemoteDirectory { get; set; }
        public string FileName { get; set; }
    }
}
