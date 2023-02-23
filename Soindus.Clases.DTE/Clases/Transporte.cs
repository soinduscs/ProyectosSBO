using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soindus.Clases.DTE
{
    public class Transporte
    {
        [JsonProperty("Patente")]
        public string Patente { get; set; }
        [JsonProperty("RUTTrans")]
        public string RUTTrans { get; set; }
        [JsonProperty("RUTChofer")]
        public string RUTChofer { get; set; }
        [JsonProperty("NombreChofer")]
        public string NombreChofer { get; set; }
        [JsonProperty("DirDest")]
        public string DirDest { get; set; }
        [JsonProperty("CmnaDest")]
        public string CmnaDest { get; set; }
        [JsonProperty("CiudadDest")]
        public string CiudadDest { get; set; }
        [JsonProperty("Aduana")]
        public Aduana Aduana { get; set; }
    }

    public class Aduana
    {
        [JsonProperty("CodModVenta")]
        public int CodModVenta { get; set; }
        [JsonProperty("CodClauVenta")]
        public int CodClauVenta { get; set; }
        [JsonProperty("TotClauVenta")]
        public double TotClauVenta { get; set; }
        [JsonProperty("CodViaTransp")]
        public int CodViaTransp { get; set; }
        [JsonProperty("NombreTransp")]
        public string NombreTransp { get; set; }
        [JsonProperty("RUTCiaTransp")]
        public string RUTCiaTransp { get; set; }
        [JsonProperty("NomCiaTransp")]
        public string NomCiaTransp { get; set; }
        [JsonProperty("IdAdicTransp")]
        public string IdAdicTransp { get; set; }
        [JsonProperty("Booking")]
        public string Booking { get; set; }
        [JsonProperty("Operador")]
        public string Operador { get; set; }
        [JsonProperty("CodPtoEmbarque")]
        public int CodPtoEmbarque { get; set; }
        [JsonProperty("IdAdicPtoEmb")]
        public string IdAdicPtoEmb { get; set; }
        [JsonProperty("CodPtoDesemb")]
        public int CodPtoDesemb { get; set; }
        [JsonProperty("IdAdicPtoDesemb")]
        public string IdAdicPtoDesemb { get; set; }
        [JsonProperty("Tara")]
        public int Tara { get; set; }
        [JsonProperty("CodUnidMedTara")]
        public int CodUnidMedTara { get; set; }
        [JsonProperty("PesoBruto")]
        public double PesoBruto { get; set; }
        [JsonProperty("CodUnidPesoBruto")]
        public int CodUnidPesoBruto { get; set; }
        [JsonProperty("PesoNeto")]
        public double PesoNeto { get; set; }
        [JsonProperty("CodUnidPesoNeto")]
        public int CodUnidPesoNeto { get; set; }
        [JsonProperty("TotItems")]
        public long TotItems { get; set; }
        [JsonProperty("TotBultos")]
        public long TotBultos { get; set; }
        [JsonProperty("TipoBultos")]
        [JsonConverter(typeof(ObjectToArrayConverter<TipoBultos>))]
        public TipoBultos[] TipoBultos { get; set; }
        [JsonProperty("MntFlete")]
        public double MntFlete { get; set; }
        [JsonProperty("MntSeguro")]
        public double MntSeguro { get; set; }
        [JsonProperty("CodPaisRecep")]
        public string CodPaisRecep { get; set; }
        [JsonProperty("CodPaisDestin")]
        public string CodPaisDestin { get; set; }
    }

    public class TipoBultos
    {
        [JsonProperty("CodTpoBultos")]
        public int CodTpoBultos { get; set; }
        [JsonProperty("CantBultos")]
        public long CantBultos { get; set; }
        [JsonProperty("Marcas")]
        public string Marcas { get; set; }
        [JsonProperty("IdContainer")]
        public string IdContainer { get; set; }
        [JsonProperty("Sello")]
        public string Sello { get; set; }
        [JsonProperty("EmisorSello")]
        public string EmisorSello { get; set; }
    }

}
