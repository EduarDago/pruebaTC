using System;
using System.Collections.Generic;
using System.Text;

namespace Proteccion.TableroControl.Dominio.Entidades
{
    public class Inconsistencia
    {
        public int IdInconsistencia { get; set; }
        public int IdValidacion { get; set; }
        public DateTime Fecha { get; set; }
        public string Detalle { get; set; }
        public string Usuario { get; set; }
        public string Identificador { get; set; }

        public Validacion Validacion { get; set; }
    }
}
