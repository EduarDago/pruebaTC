using Proteccion.TableroControl.Dominio.Enumeraciones;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proteccion.TableroControl.Dominio.Entidades
{
    public class EjecucionImportacion
    {
        public EjecucionImportacion()
        {
            Validaciones = new List<ErrorDato>();
        }

        public int IdEjecucion { get; set; }

        public int IdOrigenDato { get; set; }

        public string NombreOrigen { get; set; }

        public DateTime? FechaEjecucion { get; set; }

        public string Estado { get; set; }

        public int CantidadRegistros { get; set; }

        public bool UltimaEjecucion { get; set; }

        public TipoOrigen TipoOrigen { get; set; }

        public string Errores { get; set; }

        public List<ErrorDato> Validaciones { get; set; }

        public bool Exitoso { get; set; }
    }

    public class ErrorDato
    {
        public int Fila { get; set; }

        public string Errores { get; set; }
    }
}
