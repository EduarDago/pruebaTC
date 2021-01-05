using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Proteccion.TableroControl.Dominio.Entidades;

namespace Proteccion.TableroControl.Presentacion.ViewModels
{
    public class DetalleValidacionViewModel
    {
        public Validacion Validacion { get; set; }

        public List<Topico> TiposValidacion { get; set; }

        public List<Topico> Equipos { get; set; }

        public string Accion { get; set; }
    }
}
