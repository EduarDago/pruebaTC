using System;
using System.Collections.Generic;
using System.Text;

namespace Proteccion.TableroControl.Dominio.Entidades
{
    public class Topico
    {
        public Topico()
        {
            OrigenesDatos = new HashSet<OrigenDato>();
            Validaciones = new HashSet<Validacion>();
            Validaciones2 = new HashSet<Validacion>();
        }

        public int IdTopico { get; set; }
        public string Identificador { get; set; }
        public string Valor { get; set; }
        public string TextoMostrar { get; set; }
        public int? Orden { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }

        public ICollection<OrigenDato> OrigenesDatos { get; set; }
        public ICollection<Validacion> Validaciones { get; set; }
        public ICollection<Validacion> Validaciones2 { get; set; }

    }
}
