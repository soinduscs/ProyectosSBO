using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soindus.Clases.DTE
{
    public class Detalle
    {
        [JsonProperty("NroLinDet")]
        public int NroLinDet { get; set; }
        [JsonProperty("CdgItem")]
        [JsonConverter(typeof(ObjectToArrayConverter<CdgItem>))]
        public CdgItem[] CdgItem { get; set; }
        [JsonProperty("TpoDocLiq")]
        public string TpoDocLiq { get; set; }
        [JsonProperty("IndExe")]
        public int IndExe { get; set; }
        [JsonProperty("IndAgente")]
        public string IndAgente { get; set; }
        [JsonProperty("MntBaseFaenaRet")]
        public long MntBaseFaenaRet { get; set; }
        [JsonProperty("MntMargComer")]
        public long MntMargComer { get; set; }
        [JsonProperty("PrcConsFinal")]
        public long PrcConsFinal { get; set; }
        [JsonProperty("NmbItem")]
        public string NmbItem { get; set; }
        [JsonProperty("DscItem")]
        public string DscItem { get; set; }
        [JsonProperty("QtyRef")]
        public double QtyRef { get; set; }
        [JsonProperty("UnmdRe")]
        public string UnmdRe { get; set; }
        [JsonProperty("PrcRef")]
        public double PrcRef { get; set; }
        [JsonProperty("QtyItem")]
        public double QtyItem { get; set; }
        [JsonProperty("Subcantidad")]
        [JsonConverter(typeof(ObjectToArrayConverter<Subcantidad>))]
        public Subcantidad[] Subcantidad { get; set; }
        [JsonProperty("FchElabor")]
        public string FchElabor { get; set; }
        [JsonProperty("FchVencim")]
        public string FchVencim { get; set; }
        [JsonProperty("UnmdItem")]
        public string UnmdItem { get; set; }
        [JsonProperty("PrcItem")]
        public double PrcItem { get; set; }
        [JsonProperty("OtrMnda")]
        [JsonConverter(typeof(ObjectToArrayConverter<OtrMnda>))]
        public OtrMnda[] OtrMnda { get; set; }
        [JsonProperty("DescuentoPct")]
        public double DescuentoPct { get; set; }
        [JsonProperty("DescuentoMonto")]
        public long DescuentoMonto { get; set; }
        [JsonProperty("SubDscto")]
        [JsonConverter(typeof(ObjectToArrayConverter<SubDscto>))]
        public SubDscto[] SubDscto { get; set; }
        [JsonProperty("RecargoPct")]
        public double RecargoPct { get; set; }
        [JsonProperty("RecargoMonto")]
        public long RecargoMonto { get; set; }
        [JsonProperty("SubRecargo")]
        [JsonConverter(typeof(ObjectToArrayConverter<SubRecargo>))]
        public SubRecargo[] SubRecargo { get; set; }
        [JsonProperty("CodImpAdic")]
        public string CodImpAdic { get; set; }
        //[JsonConverter(typeof(ObjectToArrayConverter<CodImpAdic>))]
        //public CodImpAdic[] CodImpAdic { get; set; }
        [JsonProperty("MontoItem")]
        public double MontoItem { get; set; }
        [JsonProperty("ItemCode")]
        public string ItemCode { get; set; }
    }

    public class CdgItem
    {
        [JsonProperty("TpoCodigo")]
        public string TpoCodigo { get; set; }
        [JsonProperty("VlrCodigo")]
        public string VlrCodigo { get; set; }
    }

    public class Subcantidad
    {
        [JsonProperty("SubQty")]
        public double SubQty { get; set; }
        [JsonProperty("SubCod")]
        public string SubCod { get; set; }
        [JsonProperty("TipCodSubQty")]
        public string TipCodSubQty { get; set; }
    }

    public class OtrMnda
    {
        [JsonProperty("PrcOtrMon")]
        public double PrcOtrMon { get; set; }
        [JsonProperty("Moneda")]
        public string Moneda { get; set; }
        [JsonProperty("FctConv")]
        public double FctConv { get; set; }
        [JsonProperty("DctoOtrMnda")]
        public double DctoOtrMnda { get; set; }
        [JsonProperty("RecargoOtrMnda")]
        public double RecargoOtrMnda { get; set; }
        [JsonProperty("MontoItemOtrMnda")]
        public double MontoItemOtrMnda { get; set; }
    }

    public class SubDscto
    {
        [JsonProperty("TipoDscto")]
        public string TipoDscto { get; set; }
        [JsonProperty("ValorDscto")]
        public double ValorDscto { get; set; }
    }

    public class SubRecargo
    {
        [JsonProperty("TipoRecargo")]
        public string TipoRecargo { get; set; }
        [JsonProperty("ValorRecargo")]
        public double ValorRecargo { get; set; }
    }

    public class CodImpAdic
    {
        [JsonProperty("CodImpAdic")]
        public string sCodImpAdic { get; set; }
    }
}
