using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soindus.AddOnEmisionDTE.SoindusFE
{
    public class GenerarDocumentoElectronico
    {
        public GenerarDocumentoElectronico()
        {
            objDTE = new objDTE();
        }

        public string intCodigo { get; set; }
        public string strClave { get; set; }
        public objDTE objDTE { get; set; }
    }

    public class GenerarBoletaElectronica
    {
        public GenerarBoletaElectronica()
        {
            objDTE = new objDTE();
        }

        public string intCodigo { get; set; }
        public string strClave { get; set; }
        public objDTE objDTE { get; set; }
    }

    public class objDTE
    {
        public objDTE()
        {
            DTE = new DTE();
            Boleta = new Boleta();
        }

        [JsonProperty("DTE")]
        public DTE DTE { get; set; }
        [JsonProperty("Boleta")]
        public Boleta Boleta { get; set; }
        public string NroResolucion { get; set; }
        public string FechaResolucion { get; set; }
        public string AmbienteProduccion { get; set; }
    }

    public class DTE
    {
        public DTE()
        {
            Documento = new Documento();
        }

        [JsonProperty("Documento")]
        public Documento Documento { get; set; }
    }

    public class Boleta
    {
        public Boleta()
        {
            Documento = new Documento();
        }

        [JsonProperty("Documento")]
        public Documento Documento { get; set; }
    }

    public class Documento
    {
        public Documento()
        {
            Encabezado = new Encabezado();
            //Extras = new Extras();
        }

        //public Extras Extras { get; set; }

        [JsonProperty("Encabezado")]
        public Encabezado Encabezado { get; set; }

        [JsonProperty("Detalle")]
        [JsonConverter(typeof(ObjectToArrayConverter<Detalle>))]
        public Detalle[] Detalle { get; set; }

        [JsonProperty("SubTotInfo")]
        [JsonConverter(typeof(ObjectToArrayConverter<SubTotInfo>))]
        public SubTotInfo[] SubTotInfo { get; set; }

        [JsonProperty("DscRcgGlobal")]
        [JsonConverter(typeof(ObjectToArrayConverter<DscRcgGlobal>))]
        public DscRcgGlobal[] DscRcgGlobal { get; set; }

        [JsonProperty("Referencia")]
        [JsonConverter(typeof(ObjectToArrayConverter<Referencia>))]
        public Referencia[] Referencia { get; set; }

        [JsonProperty("Comisiones")]
        [JsonConverter(typeof(ObjectToArrayConverter<Comisiones>))]
        public Comisiones[] Comisiones { get; set; }
    }

    //public class Extras
    //{
    //    public EnvioPdfb EnvioPdf { get; set; }
    //    public Extras()
    //    {
    //        EnvioPdf = new EnvioPdfb();
    //    }
    //}

    //public class EnvioPdfb
    //{
    //    public List<EnvioPdf> EnvioPdf { get; set; }
    //    public EnvioPdfb()
    //    {
    //        EnvioPdf = new List<EnvioPdf>();
    //    }
    //}

    //public class EnvioPdf
    //{
    //    public List<CamposEnvioPdf> CamposEnvioPdf { get; set; }
    //    public EnvioPdf()
    //    {
    //        CamposEnvioPdf = new List<CamposEnvioPdf>();
    //    }
    //}

    //public class CamposEnvioPdf
    //{
    //    public string nroLinea { get; set; }
    //    public string mailEnvio { get; set; }
    //    public string mailCopia { get; set; }
    //    public string mailCopiaOculta { get; set; }
    //    public string mailMensaje { get; set; }
    //}

    public class Encabezado
    {
        public Encabezado()
        {
            IdDoc = new IdDoc();
            Emisor = new Emisor();
            Receptor = new Receptor();
            Transporte = new Transporte();
            Totales = new Totales();
            OtraMoneda = new OtraMoneda();
        }

        [JsonProperty("IdDoc")]
        public IdDoc IdDoc { get; set; }
        [JsonProperty("Emisor")]
        public Emisor Emisor { get; set; }
        [JsonProperty("Receptor")]
        public Receptor Receptor { get; set; }
        [JsonProperty("Transporte")]
        public Transporte Transporte { get; set; }
        [JsonProperty("Totales")]
        public Totales Totales { get; set; }
        [JsonProperty("OtraMoneda")]
        public OtraMoneda OtraMoneda { get; set; }
    }

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
        public string IndNoRebaja { get; set; }
        [JsonProperty("TipoDespacho")]
        public string TipoDespacho { get; set; }
        [JsonProperty("IndTraslado")]
        public string IndTraslado { get; set; }
        [JsonProperty("TpoImpresion")]
        public string TpoImpresion { get; set; }
        [JsonProperty("IndServicio")]
        public string IndServicio { get; set; }
        [JsonProperty("MntBruto")]
        public string MntBruto { get; set; }
        [JsonProperty("FmaPago")]
        public string FmaPago { get; set; }
        [JsonProperty("FmaPagExp")]
        public string FmaPagExp { get; set; }
        [JsonProperty("FchCancel")]
        public string FchCancel { get; set; }
        [JsonProperty("MntCancel")]
        public string MntCancel { get; set; }
        [JsonProperty("SaldoInsol")]
        public string SaldoInsol { get; set; }
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
        public string MntPago { get; set; }
        [JsonProperty("GlosaPagos")]
        public string GlosaPagos { get; set; }
    }

    public class Emisor
    {
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
        public string CdgTraslado { get; set; }
        [JsonProperty("FolioAut")]
        public string FolioAut { get; set; }
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

    public class Transporte
    {
        public Transporte()
        {
            Aduana = new Aduana();
        }

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
        public string CodModVenta { get; set; }
        [JsonProperty("CodClauVenta")]
        public string CodClauVenta { get; set; }
        [JsonProperty("TotClauVenta")]
        public string TotClauVenta { get; set; }
        [JsonProperty("CodViaTransp")]
        public string CodViaTransp { get; set; }
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
        public string CodPtoEmbarque { get; set; }
        [JsonProperty("IdAdicPtoEmb")]
        public string IdAdicPtoEmb { get; set; }
        [JsonProperty("CodPtoDesemb")]
        public string CodPtoDesemb { get; set; }
        [JsonProperty("IdAdicPtoDesemb")]
        public string IdAdicPtoDesemb { get; set; }
        [JsonProperty("Tara")]
        public string Tara { get; set; }
        [JsonProperty("CodUnidMedTara")]
        public string CodUnidMedTara { get; set; }
        [JsonProperty("PesoBruto")]
        public string PesoBruto { get; set; }
        [JsonProperty("CodUnidPesoBruto")]
        public string CodUnidPesoBruto { get; set; }
        [JsonProperty("PesoNeto")]
        public string PesoNeto { get; set; }
        [JsonProperty("CodUnidPesoNeto")]
        public string CodUnidPesoNeto { get; set; }
        [JsonProperty("TotItems")]
        public string TotItems { get; set; }
        [JsonProperty("TotBultos")]
        public string TotBultos { get; set; }
        [JsonProperty("TipoBultos")]
        [JsonConverter(typeof(ObjectToArrayConverter<TipoBultos>))]
        public TipoBultos[] TipoBultos { get; set; }
        [JsonProperty("MntFlete")]
        public string MntFlete { get; set; }
        [JsonProperty("MntSeguro")]
        public string MntSeguro { get; set; }
        [JsonProperty("CodPaisRecep")]
        public string CodPaisRecep { get; set; }
        [JsonProperty("CodPaisDestin")]
        public string CodPaisDestin { get; set; }
    }

    public class TipoBultos
    {
        [JsonProperty("CodTpoBultos")]
        public string CodTpoBultos { get; set; }
        [JsonProperty("CantBultos")]
        public string CantBultos { get; set; }
        [JsonProperty("Marcas")]
        public string Marcas { get; set; }
        [JsonProperty("IdContainer")]
        public string IdContainer { get; set; }
        [JsonProperty("Sello")]
        public string Sello { get; set; }
        [JsonProperty("EmisorSello")]
        public string EmisorSello { get; set; }
    }

    public class Totales
    {
        [JsonProperty("MntNeto")]
        public string MntNeto { get; set; }
        [JsonProperty("MntExe")]
        public string MntExe { get; set; }
        [JsonProperty("MntBase")]
        public string MntBase { get; set; }
        [JsonProperty("MntMargenCom")]
        public string MntMargenCom { get; set; }
        [JsonProperty("TasaIVA")]
        public string TasaIVA { get; set; }
        [JsonProperty("IVA")]
        public string IVA { get; set; }
        [JsonProperty("IVAProp")]
        public string IVAProp { get; set; }
        [JsonProperty("IVATerc")]
        public string IVATerc { get; set; }
        [JsonProperty("ImptoReten")]
        [JsonConverter(typeof(ObjectToArrayConverter<ImptoReten>))]
        public ImptoReten[] ImptoReten { get; set; }
        [JsonProperty("IVANoRet")]
        public string IVANoRet { get; set; }
        [JsonProperty("CredEC")]
        public string CredEC { get; set; }
        [JsonProperty("GrntDep")]
        public string GrntDep { get; set; }
        [JsonProperty("ComisionesTotal")]
        public ComisionesTotal ComisionesTotal { get; set; }
        [JsonProperty("MntTotal")]
        public string MntTotal { get; set; }
        [JsonProperty("MontoNF")]
        public string MontoNF { get; set; }
        [JsonProperty("MontoPeriodo")]
        public string MontoPeriodo { get; set; }
        [JsonProperty("SaldoAnterior")]
        public string SaldoAnterior { get; set; }
        [JsonProperty("VlrPagar")]
        public string VlrPagar { get; set; }
    }

    public class ImptoReten
    {
        [JsonProperty("TipoImp")]
        public string TipoImp { get; set; }
        [JsonProperty("TasaImp")]
        public string TasaImp { get; set; }
        [JsonProperty("MontoImp")]
        public string MontoImp { get; set; }
    }

    public class ComisionesTotal
    {
        [JsonProperty("ValComNeto")]
        public string ValComNeto { get; set; }
        [JsonProperty("ValComExe")]
        public string ValComExe { get; set; }
        [JsonProperty("ValComIVA")]
        public string ValComIVA { get; set; }
    }

    public class OtraMoneda
    {
        [JsonProperty("TpoMoneda")]
        public string TpoMoneda { get; set; }
        [JsonProperty("TpoMonedaOtrMnda")]
        public string TpoMonedaOtrMnda { get; set; }
        [JsonProperty("TpoCambio")]
        public string TpoCambio { get; set; }
        [JsonProperty("MntNetoOtrMnda")]
        public string MntNetoOtrMnda { get; set; }
        [JsonProperty("MntExeOtrMnda")]
        public string MntExeOtrMnda { get; set; }
        [JsonProperty("MntFaeCarneOtrMnda")]
        public string MntFaeCarneOtrMnda { get; set; }
        [JsonProperty("MntMargComOtrMnda")]
        public string MntMargComOtrMnda { get; set; }
        [JsonProperty("IVAOtrMnda")]
        public string IVAOtrMnda { get; set; }
        [JsonProperty("ImpRetOtrMnda")]
        [JsonConverter(typeof(ObjectToArrayConverter<ImpRetOtrMnda>))]
        public ImpRetOtrMnda[] ImpRetOtrMnda { get; set; }
        [JsonProperty("IVANoRetOtrMnda")]
        public string IVANoRetOtrMnda { get; set; }
        [JsonProperty("MntTotOtrMnda")]
        public string MntTotOtrMnda { get; set; }
    }

    public class ImpRetOtrMnda
    {
        [JsonProperty("TipoImpOtrMnda")]
        public string TipoImpOtrMnda { get; set; }
        [JsonProperty("TasaImpOtrMnda")]
        public string TasaImpOtrMnda { get; set; }
        [JsonProperty("VlrImpOtrMnda")]
        public string VlrImpOtrMnda { get; set; }
    }

    public class Detalle
    {
        [JsonProperty("NroLinDet")]
        public string NroLinDet { get; set; }
        [JsonProperty("CdgItem")]
        [JsonConverter(typeof(ObjectToArrayConverter<CdgItem>))]
        public CdgItem[] CdgItem { get; set; }
        [JsonProperty("TpoDocLiq")]
        public string TpoDocLiq { get; set; }
        [JsonProperty("IndExe")]
        public string IndExe { get; set; }
        [JsonProperty("IndAgente")]
        public string IndAgente { get; set; }
        [JsonProperty("MntBaseFaenaRet")]
        public string MntBaseFaenaRet { get; set; }
        [JsonProperty("MntMargComer")]
        public string MntMargComer { get; set; }
        [JsonProperty("PrcConsFinal")]
        public string PrcConsFinal { get; set; }
        [JsonProperty("NmbItem")]
        public string NmbItem { get; set; }
        [JsonProperty("DscItem")]
        public string DscItem { get; set; }
        [JsonProperty("QtyRef")]
        public string QtyRef { get; set; }
        [JsonProperty("UnmdRe")]
        public string UnmdRe { get; set; }
        [JsonProperty("PrcRef")]
        public string PrcRef { get; set; }
        [JsonProperty("QtyItem")]
        public string QtyItem { get; set; }
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
        public string PrcItem { get; set; }
        [JsonProperty("OtrMnda")]
        [JsonConverter(typeof(ObjectToArrayConverter<OtrMnda>))]
        public OtrMnda[] OtrMnda { get; set; }
        [JsonProperty("DescuentoPct")]
        public string DescuentoPct { get; set; }
        [JsonProperty("DescuentoMonto")]
        public string DescuentoMonto { get; set; }
        [JsonProperty("SubDscto")]
        [JsonConverter(typeof(ObjectToArrayConverter<SubDscto>))]
        public SubDscto[] SubDscto { get; set; }
        [JsonProperty("RecargoPct")]
        public string RecargoPct { get; set; }
        [JsonProperty("RecargoMonto")]
        public string RecargoMonto { get; set; }
        [JsonProperty("SubRecargo")]
        [JsonConverter(typeof(ObjectToArrayConverter<SubRecargo>))]
        public SubRecargo[] SubRecargo { get; set; }
        [JsonProperty("CodImpAdic")]
        public string CodImpAdic { get; set; }
        //[JsonConverter(typeof(ObjectToArrayConverter<CodImpAdic>))]
        //public CodImpAdic[] CodImpAdic { get; set; }
        [JsonProperty("MontoItem")]
        public string MontoItem { get; set; }
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
        public string SubQty { get; set; }
        [JsonProperty("SubCod")]
        public string SubCod { get; set; }
        [JsonProperty("TipCodSubQty")]
        public string TipCodSubQty { get; set; }
    }

    public class OtrMnda
    {
        [JsonProperty("PrcOtrMon")]
        public string PrcOtrMon { get; set; }
        [JsonProperty("Moneda")]
        public string Moneda { get; set; }
        [JsonProperty("FctConv")]
        public string FctConv { get; set; }
        [JsonProperty("DctoOtrMnda")]
        public string DctoOtrMnda { get; set; }
        [JsonProperty("RecargoOtrMnda")]
        public string RecargoOtrMnda { get; set; }
        [JsonProperty("MontoItemOtrMnda")]
        public string MontoItemOtrMnda { get; set; }
    }

    public class SubDscto
    {
        [JsonProperty("TipoDscto")]
        public string TipoDscto { get; set; }
        [JsonProperty("ValorDscto")]
        public string ValorDscto { get; set; }
    }

    public class SubRecargo
    {
        [JsonProperty("TipoRecargo")]
        public string TipoRecargo { get; set; }
        [JsonProperty("ValorRecargo")]
        public string ValorRecargo { get; set; }
    }

    public class CodImpAdic
    {
        [JsonProperty("CodImpAdic")]
        public string sCodImpAdic { get; set; }
    }

    public class SubTotInfo
    {
        [JsonProperty("NroSTI")]
        public string NroSTI { get; set; }
        [JsonProperty("GlosaSTI")]
        public string GlosaSTI { get; set; }
        [JsonProperty("OrdenSTI")]
        public string OrdenSTI { get; set; }
        [JsonProperty("SubTotNetoSTI")]
        public string SubTotNetoSTI { get; set; }
        [JsonProperty("SubTotIVASTI")]
        public string SubTotIVASTI { get; set; }
        [JsonProperty("SubTotAdicSTI")]
        public string SubTotAdicSTI { get; set; }
        [JsonProperty("SubTotExeSTI")]
        public string SubTotExeSTI { get; set; }
        [JsonProperty("ValSubtotSTI")]
        public string ValSubtotSTI { get; set; }
        //[JsonProperty("LineasDeta")]
        //[JsonConverter(typeof(ObjectToArrayConverter<LineasDeta>))]
        //public LineasDeta[] LineasDeta { get; set; }
    }

    public class LineasDeta
    {
        [JsonProperty("iLineasDeta")]
        public string iLineasDeta { get; set; }
    }

    public class DscRcgGlobal
    {
        [JsonProperty("NroLinDR")]
        public string NroLinDR { get; set; }
        [JsonProperty("TpoMov")]
        public string TpoMov { get; set; }
        [JsonProperty("GlosaDR")]
        public string GlosaDR { get; set; }
        [JsonProperty("TpoValor")]
        public string TpoValor { get; set; }
        [JsonProperty("ValorDR")]
        public string ValorDR { get; set; }
        [JsonProperty("ValorDROtrMnda")]
        public string ValorDROtrMnda { get; set; }
        [JsonProperty("IndExeDR")]
        public string IndExeDR { get; set; }
    }

    public class Referencia
    {
        [JsonProperty("NroLinRef")]
        public string NroLinRef { get; set; }
        [JsonProperty("TpoDocRef")]
        public string TpoDocRef { get; set; }
        [JsonProperty("IndGlobal")]
        public string IndGlobal { get; set; }
        [JsonProperty("FolioRef")]
        public string FolioRef { get; set; }
        [JsonProperty("RUTOtr")]
        public string RUTOtr { get; set; }
        [JsonProperty("FchRef")]
        public string FchRef { get; set; }
        [JsonProperty("CodRef")]
        public string CodRef { get; set; }
        [JsonProperty("RazonRef")]
        public string RazonRef { get; set; }
    }

    public class Comisiones
    {
        [JsonProperty("NroLinCom")]
        public string NroLinCom { get; set; }
        [JsonProperty("TipoMovim")]
        public string TipoMovim { get; set; }
        [JsonProperty("Glosa")]
        public string Glosa { get; set; }
        [JsonProperty("TasaComision")]
        public string TasaComision { get; set; }
        [JsonProperty("ValComNeto")]
        public string ValComNeto { get; set; }
        [JsonProperty("ValComExe")]
        public string ValComExe { get; set; }
        [JsonProperty("ValComIVA")]
        public string ValComIVA { get; set; }
    }

}
