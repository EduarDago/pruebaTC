using System;
using System.Collections.Generic;
using System.Text;

namespace Proteccion.TableroControl.Dominio.Entidades
{
    public class EjecucionOrigen
    {
        public int IdEjecucion { get; set; }
        public int IdOrigenDato { get; set; }
        public DateTime FechaEjecucion { get; set; }
        public string Estado { get; set; }
        public int CantidadRegistros { get; set; }

        public OrigenDato OrigenDato { get; set; }
    }
}
