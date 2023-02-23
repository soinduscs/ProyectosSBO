using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soindus.Clases.DTE
{
    public class SubTotInfo
    {
        [JsonProperty("NroSTI")]
        public int NroSTI { get; set; }
        [JsonProperty("GlosaSTI")]
        public string GlosaSTI { get; set; }
        [JsonProperty("OrdenSTI")]
        public int OrdenSTI { get; set; }
        [JsonProperty("SubTotNetoSTI")]
        public double SubTotNetoSTI { get; set; }
        [JsonProperty("SubTotIVASTI")]
        public double SubTotIVASTI { get; set; }
        [JsonProperty("SubTotAdicSTI")]
        public double SubTotAdicSTI { get; set; }
        [JsonProperty("SubTotExeSTI")]
        public double SubTotExeSTI { get; set; }
        [JsonProperty("ValSubtotSTI")]
        public double ValSubtotSTI { get; set; }
        //[JsonProperty("LineasDeta")]
        //[JsonConverter(typeof(ObjectToArrayConverter<LineasDeta>))]
        //public LineasDeta[] LineasDeta { get; set; }
    }

    public class LineasDeta
    {
        [JsonProperty("iLineasDeta")]
        public int iLineasDeta { get; set; }
    }
}
