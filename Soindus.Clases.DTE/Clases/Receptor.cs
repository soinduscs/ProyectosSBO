using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soindus.Clases.DTE
{
    public class Receptor
    {
        [JsonProperty("RUTRecep")]
        public string RUTRecep { get; set; }
        [JsonProperty("CdgIntRecep")]
        public string CdgIntRecep { get; set; }
        [JsonProperty("RznSocRecep")]
        public string RznSocRecep { get; set; }
        [JsonProperty("NumId")]
        public string NumId { get; set; }
        [JsonProperty("Nacionalidad")]
        public string Nacionalidad { get; set; }
        [JsonProperty("IdAdicRecep")]
        public string IdAdicRecep { get; set; }
        [JsonProperty("GiroRecep")]
        public string GiroRecep { get; set; }
        [JsonProperty("Contacto")]
        public string Contacto { get; set; }
        [JsonProperty("CorreoRecep")]
        public string CorreoRecep { get; set; }
        [JsonProperty("DirRecep")]
        public string DirRecep { get; set; }
        [JsonProperty("CmnaRecep")]
        public string CmnaRecep { get; set; }
        [JsonProperty("CiudadRecep")]
        public string CiudadRecep { get; set; }
        [JsonProperty("DirPostal")]
        public string DirPostal { get; set; }
        [JsonProperty("CmnaPostal")]
        public string CmnaPostal { get; set; }
        [JsonProperty("CiudadPostal")]
        public string CiudadPostal { get; set; }
    }
}
