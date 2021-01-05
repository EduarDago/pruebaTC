using System;
using System.Collections.Generic;
using System.Text;

namespace Proteccion.TableroControl.Dominio.Entidades
{
    public class ResultadoEjecucionValidacion
    {
        public ResultadoEjecucionValidacion()
        {
            Inconsistencias = new List<Inconsistencia>();
        }

        public int IdValidacion { get; set; }

        public string NombreValidacion { get; set; }

        public string Estado { get; set; }

        public bool Exitoso { get; set; }

        public List<Inconsistencia> Inconsistencias { get; set; }
    }
}
