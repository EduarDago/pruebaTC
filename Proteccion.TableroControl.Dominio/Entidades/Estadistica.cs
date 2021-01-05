using System;
using System.Collections.Generic;
using System.Text;

namespace Proteccion.TableroControl.Dominio.Entidades
{
    public class Estadistica
    {
        public int IdEstadistica { get; set; }

        public int IdValidacion { get; set; }

        public DateTime Fecha { get; set; }

        public int Cantidad { get; set; }

        public Validacion Validacion { get; set; }
    }
}
