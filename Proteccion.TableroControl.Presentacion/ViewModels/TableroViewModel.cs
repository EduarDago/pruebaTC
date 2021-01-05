using Proteccion.TableroControl.Dominio.Entidades;
using System.Collections.Generic;

namespace Proteccion.TableroControl.Presentacion.ViewModels
{
    public class TableroViewModel
    {
        public List<Validacion> Validaciones { get; set; }

        public List<Topico> TiposValidacion { get; set; }

        public List<Topico> Equipos { get; set; }

        public int IdEquipo { get; set; }

        public int IdTipoValidacion { get; set; }
        public List<EjecucionValidacion> UltimaEjecucionValidacion { get; set; }
    }
}
