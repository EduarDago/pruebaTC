using System;
using System.Collections.Generic;
using System.Text;

namespace Proteccion.TableroControl.Dominio.Entidades
{
    public class DetalleEjecucionValidacion
    {
        public int IdDetalle { get; set; }

        public int IdEjecucion { get; set; }

        public int IdValidacion { get; set; }

        public int Cantidad { get; set; }

        public EjecucionValidacion EjecucionValidacion { get; set; }
    }
}
