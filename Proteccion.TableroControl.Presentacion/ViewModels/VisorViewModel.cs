using Proteccion.TableroControl.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proteccion.TableroControl.Presentacion.ViewModels
{
    public class VisorViewModel
    {
        public List<OrigenDato> Origenes { get; set; }

        public int OrigenSeleccionado { get; set; }
    }
}
