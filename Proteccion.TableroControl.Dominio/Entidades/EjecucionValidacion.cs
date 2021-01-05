using System;
using System.Collections.Generic;
using System.Text;

namespace Proteccion.TableroControl.Dominio.Entidades
{
    public class EjecucionValidacion
    {

        public EjecucionValidacion()
        {
            DetalleEjecuciones = new HashSet<DetalleEjecucionValidacion>();
        }

        public int IdEjecucion { get; set; }

        public string Usuario { get; set; }

        public DateTime FechaEjecucion { get; set; }

        public string Equipo { get; set; }

        public string TipoValidacion { get; set; }

        public int Cantidad { get; set; }

        public ICollection<DetalleEjecucionValidacion> DetalleEjecuciones { get; set; }
    }
}
