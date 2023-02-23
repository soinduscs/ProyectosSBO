using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soindus.Clases.DTE
{
    public class Emisor
    {
        //public Emisor()
        //{
        //    Acteco = new string[10];
        //}

        [JsonProperty("RUTEmisor")]
        public string RUTEmisor { get; set; }
        [JsonProperty("RznSoc")]
        public string RznSoc { get; set; }
        [JsonProperty("GiroEmis")]
        public string GiroEmis { get; set; }
        [JsonProperty("Telefono")]
        [JsonConverter(typeof(ObjectToArrayConverter<string>))]
        public string[] Telefono { get; set; }
        [JsonProperty("CorreoEmisor")]
        public string CorreoEmisor { get; set; }
        [JsonProperty("Acteco")]
        [JsonConverter(typeof(ObjectToArrayConverter<string>))]
        public string[] Acteco { get; set; }
        [JsonProperty("CdgTraslado")]
        public int CdgTraslado { get; set; }
        [JsonProperty("FolioAut")]
        public int FolioAut { get; set; }
        [JsonProperty("FchAut")]
        public string FchAut { get; set; }
        [JsonProperty("Sucursal")]
        public string Sucursal { get; set; }
        [JsonProperty("CdgSIISucur")]
        public string CdgSIISucur { get; set; }
        [JsonProperty("DirOrigen")]
        public string DirOrigen { get; set; }
        [JsonProperty("CmnaOrigen")]
        public string CmnaOrigen { get; set; }
        [JsonProperty("CiudadOrigen")]
        public string CiudadOrigen { get; set; }
        [JsonProperty("CdgVendedor")]
        public string CdgVendedor { get; set; }
        [JsonProperty("IdAdicEmisor")]
        public string IdAdicEmisor { get; set; }
    }
}
