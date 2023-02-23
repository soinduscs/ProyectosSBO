using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soindus.Clases.DTE
{
    public class OtraMoneda
    {
        [JsonProperty("TpoMoneda")]
        public string TpoMoneda { get; set; }
        [JsonProperty("TpoCambio")]
        public double TpoCambio { get; set; }
        [JsonProperty("MntNetoOtrMnda")]
        public double MntNetoOtrMnda { get; set; }
        [JsonProperty("MntExeOtrMnda")]
        public double MntExeOtrMnda { get; set; }
        [JsonProperty("MntFaeCarneOtrMnda")]
        public double MntFaeCarneOtrMnda { get; set; }
        [JsonProperty("MntMargComOtrMnda")]
        public double MntMargComOtrMnda { get; set; }
        [JsonProperty("IVAOtrMnda")]
        public double IVAOtrMnda { get; set; }
        [JsonProperty("ImpRetOtrMnda")]
        [JsonConverter(typeof(ObjectToArrayConverter<ImpRetOtrMnda>))]
        public ImpRetOtrMnda[] ImpRetOtrMnda { get; set; }
        [JsonProperty("IVANoRetOtrMnda")]
        public double IVANoRetOtrMnda { get; set; }
        [JsonProperty("MntTotOtrMnda")]
        public double MntTotOtrMnda { get; set; }
    }

    public class ImpRetOtrMnda
    {
        [JsonProperty("TipoImpOtrMnda")]
        public string TipoImpOtrMnda { get; set; }
        [JsonProperty("TasaImpOtrMnda")]
        public double TasaImpOtrMnda { get; set; }
        [JsonProperty("VlrImpOtrMnda")]
        public long VlrImpOtrMnda { get; set; }
    }
}
