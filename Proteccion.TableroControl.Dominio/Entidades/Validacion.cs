using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Proteccion.TableroControl.Dominio.Entidades
{
    public class Validacion
    {
        public Validacion()
        {
            Estadisticas = new HashSet<Estadistica>();
            Inconsistencias = new HashSet<Inconsistencia>();
        }

        public int IdValidacion { get; set; }
        public int IdTipoValidacion { get; set; }
        public int IdEquipo { get; set; }
        public string Nombre { get; set; }
        public string Esquema { get; set; }
        public string Sp { get; set; }
        public bool Activo { get; set; }

        [ForeignKey("IdTipoValidacion")]
        public Topico TipoValidacion { get; set; }

        [ForeignKey("IdEquipo")]
        public Topico Equipo { get; set; }

        public ICollection<Estadistica> Estadisticas { get; set; }
        public ICollection<Inconsistencia> Inconsistencias { get; set; }
    }
}
