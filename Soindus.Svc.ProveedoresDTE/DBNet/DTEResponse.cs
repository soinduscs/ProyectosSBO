using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Soindus.Clases.DTE;

namespace Soindus.Svc.ProveedoresDTE.DBNet
{
    public class RootDTEResponse
    {
        [JsonProperty("root")]
        public RootRescataListadoDTE Root { get; set; }
    }

    public class RootRescataListadoDTE
    {
        [JsonProperty("RescataListadoDTEResult")]
        public DTEResponse DTEResponse { get; set; }
    }

    public class DTEResponse
    {
        [JsonProperty("Codigo")]
        public string Codigo { get; set; }

        [JsonProperty("Mensaje")]
        public string Mensaje { get; set; }

        [JsonProperty("Mensajes")]
        public string Mensajes { set { Mensaje = value; } }

        [JsonProperty("totalElementos")]
        public int? TotalElementos { get; set; }

        [JsonProperty("cantDTE")]
        [JsonConverter(typeof(ParseStringToIntegerConverter))]
        public int? CantDTE { set { TotalElementos = value; TotalPaginas = value / 100; } }

        [JsonProperty("totalPaginas")]
        public int? TotalPaginas { get; set; }

        //[JsonProperty("documentos")]
        //[JsonConverter(typeof(ObjectToArrayConverter<DocuDTE>))]
        //public DocuDTE[] Documentos { get; set; }

        //[JsonProperty("Documento")]
        //[JsonConverter(typeof(ObjectToArrayConverter<DocuDTE>))]
        //public DocuDTE[] Documento { set { Documentos = value; } }

        //[JsonProperty("Documentos")]
        ////[JsonConverter(typeof(ObjectToArrayConverter<DocuDTE>))]
        //public Docums Docs { set {
        //        var ddd = Newtonsoft.Json.JsonConvert.SerializeObject(value);
        //        Documentos = Newtonsoft.Json.JsonConvert.DeserializeObject<DocuDTE[]>(Newtonsoft.Json.JsonConvert.SerializeObject(value)); } }


        //[JsonProperty("documentos")]
        //[JsonConverter(typeof(ObjectToArrayConverter<DocuDTE>))]
        public Docums Documentos { get; set; }
    }

    public class RootXMLDTEResponse
    {
        [JsonProperty("root")]
        public RootRescataXMLDTE Root { get; set; }
    }

    public class RootRescataXMLDTE
    {
        [JsonProperty("RescataXMLDTEResult")]
        public DocuDTE Documento { get; set; }
    }

    public class Docums
    {
        //[JsonProperty("Documentos")]
        //[JsonConverter(typeof(ObjectToArrayConverter<DocuDTE>))]
        //public DocuDTE[] Documentos { get; set; }

        [JsonProperty("Documento")]
        [JsonConverter(typeof(ObjectToArrayConverter<DocuDTE>))]
        public DocuDTE[] Documento { get; set; }

        //dynamic 
    }

    public class DocuDTE
    {
        private string CreaDocId()
        {
            return string.Format("{0}{1}{2}", RutEmisor.ToString().PadLeft(10, '0'), Tipo.ToString().PadLeft(3, '0'), Folio.ToString().PadLeft(15, '0'));
        }

        [JsonProperty("Tipo")]
        public int? Tipo { get; set; }

        [JsonProperty("TipoDocumento")]
        public int? TipoDocumento { get { return Tipo; } set { Tipo = value; } }

        [JsonProperty("Folio")]
        [JsonConverter(typeof(ParseStringToIntegerConverter))]
        public int? Folio { get; set; }

        [JsonProperty("FolioDocumento")]
        [JsonConverter(typeof(ParseStringToIntegerConverter))]
        public int? FolioDocumento { set { Folio = value; } }

        [JsonProperty("FechaEmision")]
        public DateTime? FechaEmision { get; set; }

        //[JsonProperty("formaDePago")]
        //public int? FormaDePago { get; set; }

        //[JsonProperty("formaPago")]
        //public string FormaPago
        //{
        //    set
        //    {
        //        switch (value)
        //        {
        //            case "CONTADO":
        //                FormaDePago = 1;
        //                break;
        //            case "CREDITO":
        //                FormaDePago = 2;
        //                break;
        //            default:
        //                FormaDePago = 3;
        //                break;
        //        }
        //    }
        //}

        //[JsonProperty("indicadorDeTraslado")]
        //public int? IndicadorDeTraslado { get; set; }

        [JsonProperty("RutEmisor")]
        public string RutEmisor { get; set; }

        [JsonProperty("RuttEmisor")]
        public string RuttEmisor { set { RutEmisor = value; } }

        //[JsonProperty("razonSocialEmisor")]
        //public string RazonSocialEmisor { get; set; }

        [JsonProperty("RutReceptor")]
        public string RutReceptor { get; set; }

        [JsonProperty("RuttReceptor")]
        public string RuttReceptor { set { RutReceptor = value; } }

        //[JsonProperty("razonSocialReceptor")]
        //public string RazonSocialReceptor { get; set; }

        //[JsonProperty("contacto")]
        //public string Contacto { get; set; }

        [JsonProperty("Iva")]
        public double? Iva { get; set; }

        [JsonProperty("ImpuestoVaag")]
        [JsonConverter(typeof(ParseStringToDoubleConverter))]
        public double? ImpuestoVaag { set { Iva = value; } }

        [JsonProperty("MontoTotal")]
        [JsonConverter(typeof(ParseStringToDoubleConverter))]
        public double? MontoTotal { get; set; }

        //[JsonProperty("estadoSii")]
        //public int? EstadoSii { get; set; }

        //[JsonProperty("estadoComercial")]
        //public int? EstadoComercial { get; set; }

        //[JsonProperty("fechaRecepcion")]
        //public DateTime? FechaRecepcion { get; set; }

        ////[JsonProperty("tipo")]
        ////public int? Tipo { get; set; }

        //[JsonProperty("codigoSii")]
        //public string CodigoSii { get; set; }

        //[JsonProperty("tieneNc")]
        //public string TieneNc { get; set; }

        //[JsonProperty("tieneNd")]
        //public string TieneNd { get; set; }

        //[JsonProperty("rutCesionario")]
        //public string RutCesionario { get; set; }

        //[JsonProperty("razonSocialCesionario")]
        //public string RazonSocialCesionario { get; set; }

        //[JsonProperty("fechaCesion")]
        //public DateTime FechaCesion { get { return new DateTime(); } set { FechaCesion = value; } }

        [JsonProperty("Plazo")]
        public int? Plazo { get { return (FechaEmision - DateTime.Today).Value.Days + 8; } set { Plazo = value; } }

        ////[JsonProperty("codigoSucursalReceptor")]
        ////[JsonConverter(typeof(ParseStringConverter))]
        ////public long CodigoSucursalReceptor { get; set; }

        ////[JsonProperty("fechaFirma")]
        ////public DateTimeOffset FechaFirma { get; set; }

        //[JsonProperty("fechaRegistroSII")]
        //public DateTimeOffset FechaRegistroSii { set { FechaRecepcion = new DateTime(value.Year, value.Month, value.Day); } }

        [JsonProperty("MontoNeto")]
        [JsonConverter(typeof(ParseStringToDoubleConverter))]
        public double? MontoNeto { get; set; }

        [JsonProperty("MontoExento")]
        [JsonConverter(typeof(ParseStringToDoubleConverter))]
        public double? MontoExento { get; set; }

        //[JsonProperty("estadoRecepcion")]
        //public string EstadoRecepcion
        //{
        //    set
        //    {
        //        int estadosii = 0;
        //        switch (value)
        //        {
        //            case "Recepcion_Por_Confirmar":
        //                estadosii = 7;
        //                break;
        //            case "Recepcion_Confirmada":
        //                estadosii = 4;
        //                break;
        //            case "Recepcion_Rechazada":
        //                estadosii = 6;
        //                break;
        //            default:
        //                estadosii = 0;
        //                break;
        //        }
        //        EstadoSii = estadosii;
        //    }
        //}

        ////[JsonProperty("glosaEstadoRecepcion")]
        ////public string GlosaEstadoRecepcion { get; set; }

        ////[JsonProperty("estadoLey19983")]
        ////public string EstadoLey19983 { get; set; }

        ////[JsonProperty("glosaEstadoLey19983")]
        ////public string GlosaEstadoLey19983 { get; set; }

        //[JsonProperty("estadoLey20956")]
        //public string EstadoLey20956
        //{
        //    set
        //    {
        //        int estadocomercial = 0;
        //        switch (value)
        //        {
        //            case "Por_Aceptar":
        //                estadocomercial = 0;
        //                break;
        //            case "Acepta_Contenido_Documento":
        //                estadocomercial = 1;
        //                break;
        //            case "Reclamo_Contenido_Documento":
        //                estadocomercial = 3;
        //                break;
        //            case "Reclamo_Falta_Parcial_Mercaderias":
        //                estadocomercial = 5;
        //                break;
        //            case "Reclamo_Falta_Total_Mercaderias":
        //                estadocomercial = 6;
        //                break;
        //            case "Otorga_Recibo_Mercaderias":
        //                estadocomercial = 7;
        //                break;
        //            default:
        //                estadocomercial = 0;
        //                break;
        //        }
        //        EstadoComercial = estadocomercial;
        //    }
        //}

        ////[JsonProperty("glosaEstadoLey20956")]
        ////public string GlosaEstadoLey20956 { get; set; }

        ////[JsonProperty("caracterizacion")]
        ////public Caracterizacion Caracterizacion { get; set; }

        //[JsonProperty("docId")]
        public string DocId { get { return CreaDocId(); } }

        [JsonProperty("FechaCarga")]
        public DateTime? FechaCarga { get; set; }

        [JsonProperty("XmlData")]
        public string XmlData { get; set; }

        [JsonProperty("XMLDocumento")]
        public string XMLDocumento { set { XmlData = value; } }
    }

    public partial class Caracterizacion
    {
        [JsonProperty("tipoTransaccion")]
        public string TipoTransaccion { get; set; }

        [JsonProperty("tipoImpuestoRecuperable")]
        public string TipoImpuestoRecuperable { get; set; }
    }

    public class Respuesta
    {
        [JsonProperty("estado")]
        [JsonConverter(typeof(ParseStringToLongConverter))]
        public long Estado { get; set; }

        [JsonProperty("descripcion")]
        public string Descripcion { get; set; }
    }

    public class RootDetalleDocuDTE
    {
        [JsonProperty("root")]
        public DetalleDocuDTE Root { get; set; }
    }

    public class DetalleDocuDTE
    {
        [JsonProperty("DocId")]
        public string DocId { get; set; }

        [JsonProperty("RutEmisor")]
        public string RutEmisor { get; set; }

        [JsonProperty("RutReceptor")]
        public string RutReceptor { get; set; }

        //[JsonProperty("razonSocialEmisor")]
        //public string RazonSocialEmisor { get; set; }

        //[JsonProperty("razonSocialReceptor")]
        //public string RazonSocialReceptor { get; set; }

        [JsonProperty("FechaEmision")]
        public DateTime? FechaEmision { get; set; }

        [JsonProperty("Folio")]
        public int? Folio { get; set; }

        [JsonProperty("Tipo")]
        public int? Tipo { get; set; }

        [JsonProperty("TipoDocumento")]
        public int? TipoDocumento { get { return Tipo; } set { Tipo = value; } }

        [JsonProperty("FechaCarga")]
        public DateTime? FechaCarga { get; set; }

        [JsonProperty("respuesta")]
        public Respuesta Respuesta { get; set; }

        [JsonProperty("xmlData")]
        public string XmlData { get; set; }

        //[JsonProperty("XMLDocumento")]
        //public string XMLDocumento { set { XmlData = value; } }

        [JsonProperty("imagenLink")]
        public string ImagenLink { get; set; }

        [JsonProperty("PDF")]
        public string pdf { set { ImagenLink = value; } }
    }

    public class RootObtenerPDF
    {
        [JsonProperty("root")]
        public RootObtenerPDFResult Root { get; set; }
    }

    public class RootObtenerPDFResult
    {
        [JsonProperty("ObtenerPDFResult")]
        public ObtenerPDFResult ObtenerPDFResult { get; set; }
    }

    public class ObtenerPDFResult
    {
        public int? Codigo { get; set; }

        [JsonProperty("vCodigo")]
        public string vCodigo { set { Codigo = value == "DOK" ? 1 : 0; } }

        public string Mensaje { get; set; }

        [JsonProperty("vMensaje")]
        public string vMensaje { set { Mensaje = value; } }

        public string ImagenLink { get; set; }

        [JsonProperty("vPDF")]
        public string vPDF { set { ImagenLink = value; } }

        public string Archivo { get; set; }

        [JsonProperty("vArchivo")]
        public string vArchivo { set { Archivo = value; } }
    }

    public class RootsetSupplierETDBusinessState
    {
        [JsonProperty("root")]
        public RootsetSupplierETDBusinessStateResult Root { get; set; }
    }

    public class RootsetSupplierETDBusinessStateResult
    {
        [JsonProperty("setSupplierETDBusinessStateResult")]
        public setSupplierETDBusinessStateResult setSupplierETDBusinessStateResult { get; set; }
    }

    public class setSupplierETDBusinessStateResult
    {
        [JsonProperty("Codigo")]
        public string Codigo { get; set; }

        [JsonProperty("Mensaje")]
        public string Mensaje { get; set; }
    }

    public class RootsetRejection
    {
        [JsonProperty("root")]
        public RootsetRejectionResult Root { get; set; }
    }

    public class RootsetRejectionResult
    {
        [JsonProperty("setRejectionResult")]
        public setRejectionResult setRejectionResult { get; set; }
    }

    public class setRejectionResult
    {
        [JsonProperty("CodigoSolicitud")]
        public string CodigoSolicitud { get; set; }

        [JsonProperty("Codigo")]
        public string Codigo { get; set; }

        [JsonProperty("Mensaje")]
        public string Mensaje { get; set; }
    }

    public class RootActualizaEstadoDTE
    {
        [JsonProperty("root")]
        public RootActualizaEstadoDTEResult Root { get; set; }
    }

    public class RootActualizaEstadoDTEResult
    {
        [JsonProperty("ActualizaEstadoDTEResult")]
        public ActualizaEstadoDTEResult ActualizaEstadoDTEResult { get; set; }
    }

    public class ActualizaEstadoDTEResult
    {
        [JsonProperty("Codigo")]
        public string Codigo { get; set; }

        [JsonProperty("Mensajes")]
        public string Mensajes { get; set; }
    }

    internal class ParseStringToLongConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(long) || t == typeof(long?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            long l;
            if (Int64.TryParse(value, out l))
            {
                return l;
            }
            throw new Exception("Cannot unmarshal type long");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (long)untypedValue;
            serializer.Serialize(writer, value.ToString());
            return;
        }

        public static readonly ParseStringToLongConverter Singleton = new ParseStringToLongConverter();
    }

    internal class ParseStringToIntegerConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(int) || t == typeof(int?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            Int32 l;
            if (Int32.TryParse(value, out l))
            {
                return l;
            }
            throw new Exception("Cannot unmarshal type int");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Int32)untypedValue;
            serializer.Serialize(writer, value.ToString());
            return;
        }

        public static readonly ParseStringToIntegerConverter Singleton = new ParseStringToIntegerConverter();
    }

    internal class ParseStringToDoubleConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(double) || t == typeof(double?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            double l;
            if (double.TryParse(value, out l))
            {
                return l;
            }
            throw new Exception("Cannot unmarshal type double");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (double)untypedValue;
            serializer.Serialize(writer, value.ToString());
            return;
        }

        public static readonly ParseStringToDoubleConverter Singleton = new ParseStringToDoubleConverter();
    }
}
