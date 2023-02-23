using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soindus.Clases.DTE
{
    public class Referencia
    {
        [JsonProperty("NroLinRef")]
        public int NroLinRef { get; set; }
        [JsonProperty("TpoDocRef")]
        public string TpoDocRef { get; set; }
        [JsonProperty("IndGlobal")]
        public int IndGlobal { get; set; }
        [JsonProperty("FolioRef")]
        public string FolioRef { get; set; }
        [JsonProperty("RUTOtr")]
        public string RUTOtr { get; set; }
        [JsonProperty("FchRef")]
        public string FchRef { get; set; }
        [JsonProperty("CodRef")]
        public int CodRef { get; set; }
        [JsonProperty("RazonRef")]
        public string RazonRef { get; set; }
    }
}
