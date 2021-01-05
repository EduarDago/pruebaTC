using Proteccion.TableroControl.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proteccion.TableroControl.Presentacion.ViewModels
{
    public class TopicoViewModel
    {
        public Topico Topico { get; set; }

        public IEnumerable<string> Identificadores { get; set; }
    }
}
