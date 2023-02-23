using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soindus.AddOnEmisionDTE.DBNet
{
    public class putCustomerETDLoad
    {
        public Extras Extras { get; set; }
        public Encabezado Encabezado { get; set; }
        public Detallesb Detalles { get; set; }
        public DescuentosRecargosyOtros DescuentosRecargosyOtros { get; set; }

        public putCustomerETDLoad()
        {
            Extras = new Extras();
            Encabezado = new Encabezado();
            Detalles = new Detallesb();
            DescuentosRecargosyOtros = new DescuentosRecargosyOtros();
        }
    }

    public class Extras
    {
        public ValoresLibres ValoresLibres { get; set; }

        public Extras()
        {
            ValoresLibres = new ValoresLibres();
        }
    }

    public class ValoresLibres
    {
        public string ImprDestino { get; set; }
        public string ValorLibre2 { get; set; }
        public string ValorLibre3 { get; set; }
        public string ValorLibre4 { get; set; }
        public string ValorLibre5 { get; set; }
        public string ValorLibre6 { get; set; }
        public string ValorLibre7 { get; set; }
        public string ValorLibre8 { get; set; }
        public string ValorLibre9 { get; set; }
        public string ValorLibre10 { get; set; }
    }

    public class Encabezado
    {
        public CamposHead camposEncabezado { get; set; }
        public ActivEcon ActivEcon { get; set; }
        public ImptoRetenb ImptoReten { get; set; }
        public TipoBultosb TipoBultos { get; set; }
        public ImpRetOtrb ImpRetOtr { get; set; }
        public ComiOtrosb Comi { get; set; }


        public Encabezado()
        {
            camposEncabezado = new CamposHead();
            ActivEcon = new ActivEcon();
            ImptoReten = new ImptoRetenb();
            TipoBultos = new TipoBultosb();
            ImpRetOtr = new ImpRetOtrb();
            Comi = new ComiOtrosb();
        }
    }

    public class CamposHead
    {
        public string TipoDTE { get; set; }
        public string Version { get; set; }
        public string Folio { get; set; }
        public string FchEmis { get; set; }
        public string IndNoRebaja { get; set; }
        public string TipoDespacho { get; set; }
        public string IndTraslado { get; set; }
        public string IndServicio { get; set; }
        public string MntBruto { get; set; }
        public string FmaPago { get; set; }
        public string FchCancel { get; set; }
        public string PeriodoDesde { get; set; }
        public string PeriodoHasta { get; set; }
        public string MedioPago { get; set; }
        public string TermPagoCdg { get; set; }
        public string TermPagoDias { get; set; }
        public string FchVenc { get; set; }
        public string RUTEmisor { get; set; }
        public string RznSoc { get; set; }
        public string GiroEmis { get; set; }
        public string Sucursal { get; set; }
        public string CdgSIISucur { get; set; }
        public string DirOrigen { get; set; }
        public string CmnaOrigen { get; set; }
        public string CiudadOrigen { get; set; }
        public string CdgVendedor { get; set; }
        public string RUTMandante { get; set; }
        public string RUTRecep { get; set; }
        public string CdgIntRecep { get; set; }
        public string RznSocRecep { get; set; }
        public string GiroRecep { get; set; }
        public string Telefono { get; set; }
        public string DirRecep { get; set; }
        public string CmnaRecep { get; set; }
        public string CiudadRecep { get; set; }
        public string DirPostal { get; set; }
        public string CmnaPostal { get; set; }
        public string CiudadPostal { get; set; }
        public string RUTSolicita { get; set; }
        public string Patente { get; set; }
        public string RUTTrans { get; set; }
        public string DirDest { get; set; }
        public string CmnaDest { get; set; }
        public string CiudadDest { get; set; }
        public string MntNeto { get; set; }
        public string MntExe { get; set; }
        public string MntBase { get; set; }
        public string TasaIVA { get; set; }
        public string IVA { get; set; }
        public string IVANoRet { get; set; }
        public string CredEC { get; set; }
        public string MontoPeriodo { get; set; }
        public string GrntDep { get; set; }
        public string MontoNF { get; set; }
        public string MntTotal { get; set; }
        public string SaldoAnterior { get; set; }
        public string VlrPagar { get; set; }
        public string TpoImpresion { get; set; }
        public string MntCancel { get; set; }
        public string SaldoInsol { get; set; }
        public string FmaPagExp { get; set; }
        public string TipoCtaPago { get; set; }
        public string NumCtaPago { get; set; }
        public string BcoPago { get; set; }
        public string GlosaPagos { get; set; }
        public string CdgTraslado { get; set; }
        public string FolioAut { get; set; }
        public string FchAut { get; set; }
        public string CodAdicSucur { get; set; }
        public string IdAdicEmisor { get; set; }
        public string NumId { get; set; }
        public string Nacionalidad { get; set; }
        public string IdAdicRecep { get; set; }
        public string CorreoRecep { get; set; }
        public string RUTChofer { get; set; }
        public string NombreChofer { get; set; }
        public string CodModVenta { get; set; }
        public string CodClauVenta { get; set; }
        public string TotClauVenta { get; set; }
        public string CodViaTransp { get; set; }
        public string NombreTransp { get; set; }
        public string RUTCiaTransp { get; set; }
        public string NomCiaTransp { get; set; }
        public string IdAdicTransp { get; set; }
        public string Booking { get; set; }
        public string Operador { get; set; }
        public string CodPtoEmbarque { get; set; }
        public string IdAdicPtoEmb { get; set; }
        public string CodPtoDesemb { get; set; }
        public string IdAdicPtoDesemb { get; set; }
        public string Tara { get; set; }
        public string CodUnidMedTara { get; set; }
        public string PesoBruto { get; set; }
        public string CodUnidPesoBruto { get; set; }
        public string PesoNeto { get; set; }
        public string CodUnidPesoNeto { get; set; }
        public string TotItems { get; set; }
        public string TotBultos { get; set; }
        public string MntFlete { get; set; }
        public string MntSeguro { get; set; }
        public string CodPaisRecep { get; set; }
        public string CodPaisDestin { get; set; }
        public string TpoMoneda { get; set; }
        public string MntMargenCom { get; set; }
        public string IVAProp { get; set; }
        public string IVATerc { get; set; }
        public string TpoMonedaOtrMnda { get; set; }
        public string TpoCambio { get; set; }
        public string MntNetoOtrMnda { get; set; }
        public string MntExeOtrMnda { get; set; }
        public string MntFaeCarneOtrMnda { get; set; }
        public string MntMargComOtrMnda { get; set; }
        public string IVAOtrMnda { get; set; }
        public string IVANoRetOtrMnda { get; set; }
        public string MntTotOtrMnda { get; set; }
    }

    public class ActivEcon
    {
        public List<ActividadEconomica> ActividadEconomica { get; set; }

        public ActivEcon()
        {
            ActividadEconomica = new List<ActividadEconomica>();
        }
    }

    public class ActividadEconomica
    {
        public string ActivEcon { get; set; }
    }

    public class ImptoRetenb
    {
        public List<ImptoRetenc> ImptoReten { get; set; }

        public ImptoRetenb()
        {
            ImptoReten = new List<ImptoRetenc>();
        }
    }

    public class ImptoRetenc
    {
        public string TipoImp { get; set; }
        public string TasaImp { get; set; }
        public string MontoImp { get; set; }

    }

    public class TipoBultosb
    {
        public List<TipoBultosc> TipoBultos { get; set; }

        public TipoBultosb()
        {
            TipoBultos = new List<TipoBultosc>();
        }
    }

    public class TipoBultosc
    {
        public string CodTpoBultos { get; set; }
        public string CantBultos { get; set; }
        public string Marcas { get; set; }
        public string IdContainer { get; set; }
        public string Sello { get; set; }
        public string EmisorSello { get; set; }
    }

    public class ImpRetOtrb
    {
        public List<ImpRetOtrMnda> ImpRetOtrMnda { get; set; }

        public ImpRetOtrb()
        {
            ImpRetOtrMnda = new List<ImpRetOtrMnda>();
        }
    }

    public class ImpRetOtrMnda
    {
        public string TipoImpOtrMnda { get; set; }
        public string VlrImpOtrMnda { get; set; }
        public string TasaImpOtrMnda { get; set; }
    }

    public class ComiOtrosb
    {
        public List<ComiOtros> ComiOtros { get; set; }

        public ComiOtrosb()
        {
            ComiOtros = new List<ComiOtros>();
        }
    }

    public class ComiOtros
    {
        public string ValComNeto { get; set; }
        public string ValComExe { get; set; }
        public string ValComIVA { get; set; }
    }

    public class Detallesb
    {
        public List<Detalle> Detalle { get; set; }

        public Detallesb()
        {
            Detalle = new List<Detalle>();
        }
    }

    public class Detalle
    {
        public CamposDetalle Detalles { get; set; }
        public List<SubDescuentosb> SubDescuentos { get; set; }
        public List<CodItemsb> CodItems { get; set; }
        public List<SubRecargob> SubRecargos { get; set; }
        public List<SubCantidadb> SubCantidades { get; set; }
        public string TpoDocLiq { get; set; }


        public Detalle()
        {
            Detalles = new CamposDetalle();
            SubDescuentos = new List<SubDescuentosb>();
            CodItems = new List<CodItemsb>();
            SubRecargos = new List<SubRecargob>();
            SubCantidades = new List<SubCantidadb>();
        }
    }

    public class CamposDetalle
    {
        public string NroLinDet { get; set; }
        public string IndExe { get; set; }
        public string NmbItem { get; set; }
        public string DscItem { get; set; }
        public string QtyRef { get; set; }
        public string UnmdRef { get; set; }
        public string PrcRef { get; set; }
        public string QtyItem { get; set; }
        public string FchElabor { get; set; }
        public string FchVencim { get; set; }
        public string UnmdItem { get; set; }
        public string PrcItem { get; set; }
        public string PrcOtrMon { get; set; }
        public string FctConv { get; set; }
        public string ValorDscto { get; set; }
        public string DescuentoPct { get; set; }
        public string DescuentoMonto { get; set; }
        public string RecargoPct { get; set; }
        public string RecargoMonto { get; set; }
        public string CodImpAdic { get; set; }
        public string MontoItem { get; set; }
        public string DctoOtrMnda { get; set; }
        public string RecargoOtrMnda { get; set; }
        public string MontoItemOtrMnda { get; set; }
        public string IndAgente { get; set; }
        public string MntBaseFaena { get; set; }
        public string MntMargComer { get; set; }
        public string PrcConsFinal { get; set; }
        public string Moneda { get; set; }
    }

    public class SubDescuentosb
    {
        public List<SubDescuentos> SubDescuentos { get; set; }

        public SubDescuentosb()
        {
            SubDescuentos = new List<SubDescuentos>();
        }
    }

    public class SubDescuentos
    {
        public string TipoDscto { get; set; }
        public string ValorDscto { get; set; }
    }

    public class CodItemsb
    {
        public List<CodItems> CodItems { get; set; }

        public CodItemsb()
        {
            CodItems = new List<CodItems>();
        }
    }

    public class CodItems
    {
        public string TpoCodigo { get; set; }
        public string CodItem { get; set; }
    }

    public class SubRecargob
    {
        public List<SubRecargo> SubRecargo { get; set; }

        public SubRecargob()
        {
            SubRecargo = new List<SubRecargo>();
        }
    }

    public class SubRecargo
    {
        public string TipoRecargo { get; set; }
        public string ValorRecargo { get; set; }
    }

    public class SubCantidadb
    {
        public List<SubCantidad> SubCantidad { get; set; }

        public SubCantidadb()
        {
            SubCantidad = new List<SubCantidad>();
        }
    }

    public class SubCantidad
    {
        public string SubQty { get; set; }
        public string SubCod { get; set; }
        public string TipCodSubQty { get; set; }
    }

    public class DescuentosRecargosyOtros
    {
        public Descuentosb Descuentos { get; set; }
        public Referenciasb Referencias { get; set; }
        public ComisionesOtrosCargosb ComisionesOtrosCargos { get; set; }
        public SubTotalesInformativosb SubTotalesInformativos { get; set; }
        public LineaDetalleSubTotalb LineaDetalleSubTotal { get; set; }

        public DescuentosRecargosyOtros()
        {
            Descuentos = new Descuentosb();
            Referencias = new Referenciasb();
            ComisionesOtrosCargos = new ComisionesOtrosCargosb();
            SubTotalesInformativos = new SubTotalesInformativosb();
            LineaDetalleSubTotal = new LineaDetalleSubTotalb();
        }
    }

    public class Descuentosb
    {
        public List<Descuentos> Descuentos { get; set; }

        public Descuentosb()
        {
            Descuentos = new List<Descuentos>();
        }
    }

    public class Descuentos
    {
        public string NroLinDR { get; set; }
        public string TpoMov { get; set; }
        public string GlosaDR { get; set; }
        public string TpoValor { get; set; }
        public string ValorDR { get; set; }
        public string IndExeDR { get; set; }
    }

    public class Referenciasb
    {
        public List<Referencias> Referencias { get; set; }

        public Referenciasb()
        {
            Referencias = new List<Referencias>();
        }
    }

    public class Referencias
    {
        public string NroLinRef { get; set; }
        public string TpoDocRef { get; set; }
        public string IndGlobal { get; set; }
        public string FolioRef { get; set; }
        public string RUTOtr { get; set; }
        public string FchRef { get; set; }
        public string CodRef { get; set; }
        public string RazonRef { get; set; }
    }

    public class ComisionesOtrosCargosb
    {
        public List<ComisionesOtrosCargos> ComisionesOtrosCargos { get; set; }

        public ComisionesOtrosCargosb()
        {
            ComisionesOtrosCargos = new List<ComisionesOtrosCargos>();
        }
    }

    public class ComisionesOtrosCargos
    {
        public string NroLinCom { get; set; }
        public string TipoMovim { get; set; }
        public string Glosa { get; set; }
        public string TasaComision { get; set; }
        public string ValComNeto { get; set; }
        public string ValComExe { get; set; }
        public string ValComIVA { get; set; }
    }

    public class SubTotalesInformativosb
    {
        public List<SubTotalesInformativos> SubTotalesInformativos { get; set; }

        public SubTotalesInformativosb()
        {
            SubTotalesInformativos = new List<SubTotalesInformativos>();
        }
    }

    public class SubTotalesInformativos
    {
        public string NroLinea { get; set; }
        public string Glosa { get; set; }
        public string Orden { get; set; }
        public string SubTotNeto { get; set; }
        public string SubTotIVA { get; set; }
        public string SubTotAdic { get; set; }
        public string SubTotExe { get; set; }
        public string ValSubtot { get; set; }
        public string LineaDeta { get; set; }
    }

    public class LineaDetalleSubTotalb
    {
        public List<LineaDetalleSubTotal> LineaDetalleSubTotal { get; set; }

        public LineaDetalleSubTotalb()
        {
            LineaDetalleSubTotal = new List<LineaDetalleSubTotal>();
        }
    }

    public class LineaDetalleSubTotal
    {
        public string String { get; set; }
    }
}
