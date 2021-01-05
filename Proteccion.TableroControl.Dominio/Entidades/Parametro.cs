using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proteccion.TableroControl.Dominio.Entidades
{
    public class Parametro
    {

        public int IdParametro { get; set; }

        public string Nombre { get; set; }

        public string Valor { get; set; }

        public string UsuarioCreacion { get; set; }

        public DateTime? FechaActualizacion { get; set; }
    }
}
