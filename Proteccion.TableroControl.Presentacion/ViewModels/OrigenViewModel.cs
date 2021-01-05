using Proteccion.TableroControl.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proteccion.TableroControl.Presentacion.ViewModels
{
    public class OrigenViewModel
    {
        public OrigenDato OrigenDato { get; set; }

        public string Accion { get; set; }

        public List<Topico> TiposDato { get; set; }

        public List<Topico> TiposOrigen { get; set; }
    }
}
