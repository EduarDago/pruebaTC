using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proteccion.TableroControl.Dominio.Entidades
{
    public class OrigenDato
    {
        public OrigenDato()
        {
            Campos = new HashSet<CampoOrigen>();
            Ejecuciones = new HashSet<EjecucionOrigen>();
            Parametros = new HashSet<ParametroOrigen>();
        }

        [JsonProperty(PropertyName = "idOrigenDato")]
        public int IdOrigenDato { get; set; }

        [JsonProperty(PropertyName = "idTipoOrigen")]
        public int IdTipoOrigen { get; set; }

        [JsonProperty(PropertyName = "nombre")]
        public string Nombre { get; set; }

        [JsonProperty(PropertyName = "descripcion")]
        public string Descripcion { get; set; }

        [JsonProperty(PropertyName = "procedimiento")]
        public string Procedimiento { get; set; }

        [JsonProperty(PropertyName = "esquemaProcedimiento")]
        public string EsquemaProcedimiento { get; set; }

        [JsonProperty(PropertyName = "esquemaTabla")]
        public string EsquemaTabla { get; set; }

        [JsonProperty(PropertyName = "nombreTabla")]
        public string NombreTabla { get; set; }

        [JsonProperty(PropertyName = "rutaArchivo")]
        public string RutaArchivo { get; set; }

        [JsonProperty(PropertyName = "nombreArchivo")]
        public string NombreArchivo { get; set; }

        [JsonProperty(PropertyName = "filaEncabezado")]
        public byte FilaEncabezado { get; set; }

        [JsonProperty(PropertyName = "lineaInicioLectura")]
        public short LineaInicioLectura { get; set; }

        [JsonProperty(PropertyName = "columnaInicioLectura")]
        public short ColumnaInicioLectura { get; set; }

        [JsonProperty(PropertyName = "activo")]
        public bool Activo { get; set; }

        [JsonProperty(PropertyName = "eliminarFila")]
        public bool EliminarFila { get; set; }

        [JsonProperty(PropertyName = "separador")]
        public string Separador { get; set; }

        [JsonProperty(PropertyName = "concatenarFecha")]
        public bool? ConcatenarFecha { get; set; }

        [JsonProperty(PropertyName = "campos")]
        public ICollection<CampoOrigen> Campos { get; set; }

        [JsonProperty(PropertyName = "parametros")]
        public ICollection<ParametroOrigen> Parametros { get; set; }

        public Topico TipoOrigen { get; set; }

        public ICollection<EjecucionOrigen> Ejecuciones { get; set; }


        [JsonProperty(PropertyName = "rutaOrigenSftp")]
        public string RutaOrigenSftp { get; set; }

        [JsonProperty(PropertyName = "rutaDestinoSftp")]
        public string RutaDestinoSftp { get; set; }
    }
}
