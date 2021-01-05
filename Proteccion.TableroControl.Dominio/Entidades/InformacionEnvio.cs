using System;
using System.Collections.Generic;
using System.Text;

namespace Proteccion.TableroControl.Dominio.Entidades
{
    public class InformacionEnvio
    {
        public InformacionEnvio()
        {
            CopiaA = new List<string>();
            CopiaOculta = new List<string>();
        }

        public string IdEnvio { get; set; }

        public string IdMensaje { get; set; }

        public string Asunto { get; set; }

        public string CuerpoHtml { get; set; }

        public string EmailOrigen { get; set; }

        public string AliasEmailOrigen { get; set; }

        public string EmailCliente { get; set; }

        public List<string> CopiaA { get; set; }

        public List<string> CopiaOculta { get; set; }

        public string NombreAdjunto { get; set; }

        public string TipoContenidoAdjunto { get; set; }

        public byte[] ContenidoAdjunto { get; set; }
    }
}
