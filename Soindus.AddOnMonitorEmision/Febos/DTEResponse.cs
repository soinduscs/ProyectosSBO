using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soindus.AddOnMonitorEmision.Febos
{
    public class DTEResponse
    {
        [JsonProperty("totalElementos")]
        public int? TotalElementos { get; set; }

        [JsonProperty("totalPaginas")]
        public int? TotalPaginas { get; set; }

        [JsonProperty("paginaActual")]
        public int? PaginaActual { get; set; }

        [JsonProperty("elementosPorPagina")]
        public int? ElementosPorPagina { get; set; }

        [JsonProperty("documentos")]
        public List<DocuDTE> Documentos { get; set; }

        [JsonProperty("codigo")]
        public int? Codigo { get; set; }

        [JsonProperty("mensaje")]
        public string Mensaje { get; set; }

        [JsonProperty("seguimientoId")]
        public string SeguimientoId { get; set; }

        [JsonProperty("duracion")]
        public int? Duracion { get; set; }

        [JsonProperty("hora")]
        public DateTime Hora { get; set; }
    }

    public class DocuDTE
    {
        [JsonProperty("febosId")]
        public string FebosId { get; set; }

        [JsonProperty("tipoDocumento")]
        public int? TipoDocumento { get; set; }

        [JsonProperty("folio")]
        public int? Folio { get; set; }

        [JsonProperty("fechaEmision")]
        public DateTime? FechaEmision { get; set; }

        [JsonProperty("formaDePago")]
        public int? FormaDePago { get; set; }

        [JsonProperty("indicadorDeTraslado")]
        public int? IndicadorDeTraslado { get; set; }

        [JsonProperty("rutEmisor")]
        public string RutEmisor { get; set; }

        [JsonProperty("razonSocialEmisor")]
        public string RazonSocialEmisor { get; set; }

        [JsonProperty("rutReceptor")]
        public string RutReceptor { get; set; }

        [JsonProperty("razonSocialReceptor")]
        public string RazonSocialReceptor { get; set; }

        [JsonProperty("contacto")]
        public string Contacto { get; set; }

        [JsonProperty("iva")]
        public double? Iva { get; set; }

        [JsonProperty("montoTotal")]
        public double MontoTotal { get; set; }

        [JsonProperty("estadoSii")]
        public int? EstadoSii { get; set; }

        [JsonProperty("estadoComercial")]
        public int? EstadoComercial { get; set; }

        [JsonProperty("fechaRecepcion")]
        public DateTime? FechaRecepcion { get; set; }

        [JsonProperty("tipo")]
        public int? Tipo { get; set; }

        [JsonProperty("codigoSii")]
        public string CodigoSii { get; set; }

        [JsonProperty("tieneNc")]
        public string TieneNc { get; set; }

        [JsonProperty("tieneNd")]
        public string TieneNd { get; set; }
    }

    public class DetalleDocuDTE
    {
        [JsonProperty("febosId")]
        public string FebosId { get; set; }

        [JsonProperty("rutEmisor")]
        public string RutEmisor { get; set; }

        [JsonProperty("rutReceptor")]
        public string RutReceptor { get; set; }

        [JsonProperty("razonSocialEmisor")]
        public string RazonSocialEmisor { get; set; }

        [JsonProperty("razonSocialReceptor")]
        public string RazonSocialReceptor { get; set; }

        [JsonProperty("fechaEmision")]
        public DateTime? FechaEmision { get; set; }

        [JsonProperty("folio")]
        public int? Folio { get; set; }

        [JsonProperty("tipo")]
        public int? Tipo { get; set; }

        [JsonProperty("fechaRecepcion")]
        public DateTime? FechaRecepcion { get; set; }

        [JsonProperty("xmlData")]
        public string XmlData { get; set; }

        [JsonProperty("imagenLink")]
        public string ImagenLink { get; set; }

        [JsonProperty("codigo")]
        public int? Codigo { get; set; }

        [JsonProperty("mensaje")]
        public string Mensaje { get; set; }

        [JsonProperty("seguimientoId")]
        public string SeguimientoId { get; set; }

        [JsonProperty("duracion")]
        public int? Duracion { get; set; }

        [JsonProperty("estadoComercial")]
        public int? EstadoComercial { get; set; }

        [JsonProperty("hora")]
        public DateTime? Hora { get; set; }
    }

}
