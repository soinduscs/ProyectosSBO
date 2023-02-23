using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soindus.Clases.DTE
{
    public class IdDoc
    {
        [JsonProperty("TipoDTE")]
        public string TipoDTE { get; set; }
        [JsonProperty("Folio")]
        public string Folio { get; set; }
        [JsonProperty("FchEmis")]
        public string FchEmis { get; set; }
        [JsonProperty("TpoTranCompra")]
        public string TpoTranCompra { get; set; }
        [JsonProperty("TpoTranVenta")]
        public string TpoTranVenta { get; set; }
        [JsonProperty("IndNoRebaja")]
        public int IndNoRebaja { get; set; }
        [JsonProperty("TipoDespacho")]
        public int TipoDespacho { get; set; }
        [JsonProperty("IndTraslado")]
        public int IndTraslado { get; set; }
        [JsonProperty("TpoImpresion")]
        public string TpoImpresion { get; set; }
        [JsonProperty("IndServicio")]
        public int IndServicio { get; set; }
        [JsonProperty("MntBruto")]
        public int MntBruto { get; set; }
        [JsonProperty("FmaPago")]
        public string FmaPago { get; set; }
        [JsonProperty("FmaPagExp")]
        public int FmaPagExp { get; set; }
        [JsonProperty("FchCancel")]
        public string FchCancel { get; set; }
        [JsonProperty("MntCancel")]
        public long MntCancel { get; set; }
        [JsonProperty("SaldoInsol")]
        public long SaldoInsol { get; set; }
        [JsonProperty("MntPagos")]
        [JsonConverter(typeof(ObjectToArrayConverter<MntPagos>))]
        public MntPagos[] MntPagos { get; set; }
        [JsonProperty("PeriodoDesde")]
        public string PeriodoDesde { get; set; }
        [JsonProperty("PeriodoHasta")]
        public string PeriodoHasta { get; set; }
        [JsonProperty("MedioPago")]
        public string MedioPago { get; set; }
        [JsonProperty("TipoCtaPago")]
        public string TipoCtaPago { get; set; }
        [JsonProperty("NumCtaPago")]
        public string NumCtaPago { get; set; }
        [JsonProperty("BcoPago")]
        public string BcoPago { get; set; }
        [JsonProperty("TermPagoCdg")]
        public string TermPagoCdg { get; set; }
        [JsonProperty("TermPagoGlosa")]
        public string TermPagoGlosa { get; set; }
        [JsonProperty("TermPagoDias")]
        public string TermPagoDias { get; set; }
        [JsonProperty("FchVenc")]
        public string FchVenc { get; set; }
    }

    public class MntPagos
    {
        [JsonProperty("FchPago")]
        public string FchPago { get; set; }
        [JsonProperty("MntPago")]
        public long MntPago { get; set; }
        [JsonProperty("GlosaPagos")]
        public string GlosaPagos { get; set; }
    }
}
