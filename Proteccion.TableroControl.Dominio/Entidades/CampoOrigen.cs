using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proteccion.TableroControl.Dominio.Entidades
{
    public class CampoOrigen
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "idOrigenDato")]
        public int IdOrigenDato { get; set; }

        [JsonProperty(PropertyName = "nombre")]
        public string Nombre { get; set; }

        [JsonProperty(PropertyName = "tipoDato")]
        public string TipoDato { get; set; }

        [JsonProperty(PropertyName = "nombreExcel")]
        public string NombreExcel { get; set; }

        [JsonProperty(PropertyName = "posicionInicial")]
        public short PosicionInicial { get; set; }

        [JsonProperty(PropertyName = "longitudCampo")]
        public short LongitudCampo  { get; set; }

        [JsonProperty(PropertyName = "eliminarCamposBlanco")]
        public bool EliminarCamposBlanco { get; set; }


        [JsonProperty(PropertyName = "orden")]
        public int Orden { get; set; }

        public OrigenDato OrigenDato { get; set; }
    }
}
