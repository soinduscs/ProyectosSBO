using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soindus.Clases.DTE
{
    public class Totales
    {
        [JsonProperty("MntNeto")]
        public double MntNeto { get; set; }
        [JsonProperty("MntExe")]
        public long MntExe { get; set; }
        [JsonProperty("MntBase")]
        public long MntBase { get; set; }
        [JsonProperty("MntMargenCom")]
        public long MntMargenCom { get; set; }
        [JsonProperty("TasaIVA")]
        public double TasaIVA { get; set; }
        [JsonProperty("IVA")]
        public double IVA { get; set; }
        [JsonProperty("IVAProp")]
        public long IVAProp { get; set; }
        [JsonProperty("IVATerc")]
        public long IVATerc { get; set; }
        [JsonProperty("ImptoReten")]
        [JsonConverter(typeof(ObjectToArrayConverter<ImptoReten>))]
        public ImptoReten[] ImptoReten { get; set; }
        [JsonProperty("IVANoRet")]
        public long IVANoRet { get; set; }
        [JsonProperty("CredEC")]
        public long CredEC { get; set; }
        [JsonProperty("GrntDep")]
        public long GrntDep { get; set; }
        [JsonProperty("ComisionesTotal")]
        public ComisionesTotal ComisionesTotal { get; set; }
        [JsonProperty("MntTotal")]
        public double MntTotal { get; set; }
        [JsonProperty("MontoNF")]
        public long MontoNF { get; set; }
        [JsonProperty("MontoPeriodo")]
        public long MontoPeriodo { get; set; }
        [JsonProperty("SaldoAnterior")]
        public long SaldoAnterior { get; set; }
        [JsonProperty("VlrPagar")]
        public long VlrPagar { get; set; }
    }

    public class ImptoReten
    {
        [JsonProperty("TipoImp")]
        public string TipoImp { get; set; }
        [JsonProperty("TasaImp")]
        public double TasaImp { get; set; }
        [JsonProperty("MontoImp")]
        public long MontoImp { get; set; }
    }

    public class ComisionesTotal
    {
        [JsonProperty("ValComNeto")]
        public long ValComNeto { get; set; }
        [JsonProperty("ValComExe")]
        public long ValComExe { get; set; }
        [JsonProperty("ValComIVA")]
        public long ValComIVA { get; set; }
    }
}
