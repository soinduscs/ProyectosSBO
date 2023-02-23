using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soindus.Clases.DTE
{
    public class DTEResponse
    {
        public int? TotalElementos { get; set; }

        public int? TotalPaginas { get; set; }

        public int? PaginaActual { get; set; }

        public int? ElementosPorPagina { get; set; }

        public List<DocuDTE> Documentos { get; set; }

        public int? Codigo { get; set; }

        public string Mensaje { get; set; }

        public string SeguimientoId { get; set; }

        public int? Duracion { get; set; }

        public DateTime Hora { get; set; }
    }

    public class DocuDTE
    {
        public string DocId { get; set; }

        public int? TipoDocumento { get; set; }

        public long? Folio { get; set; }

        public DateTime? FechaEmision { get; set; }

        public int? FormaDePago { get; set; }

        public int? IndicadorDeTraslado { get; set; }

        public string RutEmisor { get; set; }

        public string RazonSocialEmisor { get; set; }

        public string RutReceptor { get; set; }

        public string RazonSocialReceptor { get; set; }

        public string Contacto { get; set; }

        public double? Iva { get; set; }

        public double MontoTotal { get; set; }

        public int? EstadoSii { get; set; }

        public int? EstadoComercial { get; set; }

        public DateTime? FechaRecepcion { get; set; }

        public int? Tipo { get; set; }

        public string CodigoSii { get; set; }

        public string TieneNc { get; set; }

        public string TieneNd { get; set; }

        public string RutCesionario { get; set; }

        public string RazonSocialCesionario { get; set; }

        public DateTime FechaCesion { get; set; }

        public int? Plazo { get; set; }
    }

    public class DetalleDocuDTE
    {
        public string DocId { get; set; }

        public string RutEmisor { get; set; }

        public string RutReceptor { get; set; }

        public string RazonSocialEmisor { get; set; }

        public string RazonSocialReceptor { get; set; }

        public DateTime? FechaEmision { get; set; }

        public long? Folio { get; set; }

        public int? Tipo { get; set; }

        public DateTime? FechaRecepcion { get; set; }

        public string XmlData { get; set; }

        public string ImagenLink { get; set; }

        public int? Codigo { get; set; }

        public string Mensaje { get; set; }

        public string SeguimientoId { get; set; }

        public int? Duracion { get; set; }

        public int? EstadoComercial { get; set; }

        public DateTime? Hora { get; set; }
    }
}
