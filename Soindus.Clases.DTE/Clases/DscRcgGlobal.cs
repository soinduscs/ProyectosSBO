using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soindus.Clases.DTE
{
    public class DscRcgGlobal
    {
        [JsonProperty("NroLinDR")]
        public int NroLinDR { get; set; }
        [JsonProperty("TpoMov")]
        public string TpoMov { get; set; }
        [JsonProperty("GlosaDR")]
        public string GlosaDR { get; set; }
        [JsonProperty("TpoValor")]
        public string TpoValor { get; set; }
        [JsonProperty("ValorDR")]
        public double ValorDR { get; set; }
        [JsonProperty("ValorDROtrMnda")]
        public double ValorDROtrMnda { get; set; }
        [JsonProperty("IndExeDR")]
        public int IndExeDR { get; set; }
    }
}
