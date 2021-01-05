using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proteccion.TableroControl.Dominio.Entidades
{
    public class ParametroOrigen
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "idOrigenDato")]
        public int IdOrigenDato { get; set; }

        [JsonProperty(PropertyName = "nombre")]
        public string Nombre { get; set; }

        [JsonProperty(PropertyName = "valor")]
        public string Valor { get; set; }

        [JsonProperty(PropertyName = "dinamico")]
        public bool? Dinamico { get; set; }

        [JsonProperty(PropertyName = "tipoDato")]
        public string TipoDato { get; set; }

        [JsonProperty(PropertyName = "orden")]
        public int Orden { get; set; }

        [JsonProperty(PropertyName = "esFechaProceso")]
        public bool EsFechaProceso { get; set; }

        public OrigenDato OrigenDato { get; set; }
    }
}
