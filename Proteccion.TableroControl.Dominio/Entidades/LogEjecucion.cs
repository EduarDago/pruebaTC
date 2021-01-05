using System;
using System.Collections.Generic;
using System.Text;

namespace Proteccion.TableroControl.Dominio.Entidades
{
    public class LogEjecucion
    {
        public int IdLogEjecucion { get; set; }
        public string Usuario { get; set; }
        public DateTime FechaEjecucion { get; set; }
        public string Script { get; set; }
        public string ResultadoScript { get; set; }
    }
}
