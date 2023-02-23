using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Soindus.Clases.DTE;

namespace Soindus.AddOnMonitorProveedores.Azurian
{
    public class RootDTEResponse
    {
        [JsonProperty("root")]
        public RootTransferenciaDocumentos Root { get; set; }
    }

    public class RootTransferenciaDocumentos
    {
        [JsonProperty("transferenciaDocumentosReturn")]
        public DTEResponse DTEResponse { get; set; }
    }

    public class DTEResponse
    {
        [JsonProperty("documentos")]
        public Docums Documentos { get; set; }
    }

    public class RootDescargarArchivoResponse
    {
        [JsonProperty("root")]
        public RootDescargarArchivo Root { get; set; }
    }

    public class RootDescargarArchivo
    {
        [JsonProperty("descargarArchivoReturn")]
        public DocuDTEXML Documento { get; set; }
    }

    public class Docums
    {
        [JsonProperty("documentos")]
        [JsonConverter(typeof(ObjectToArrayConverter<DocuDTE>))]
        public DocuDTE[] Documento { get; set; }
    }

    public class DocuDTE
    {
        private string CreaDocId()
        {
            return string.Format("{0}{1}{2}", RutEmisor.ToString().PadLeft(10, '0'), Tipo.ToString().PadLeft(3, '0'), Folio.ToString().PadLeft(15, '0'));
        }

        [JsonProperty("Id")]
        public long? Id { get; set; }

        [JsonProperty("Tipo")]
        public int? Tipo { get; set; }

        [JsonProperty("idTipoDocuemnto")]
        public int? idTipoDocuemnto { get { return Tipo; } set { Tipo = value; } }

        [JsonProperty("TipoDocumento")]
        public int? TipoDocumento { get { return Tipo; } set { Tipo = value; } }

        [JsonProperty("Folio")]
        [JsonConverter(typeof(ParseStringToLongConverter))]
        public long? Folio { get; set; }

        [JsonProperty("folio")]
        [JsonConverter(typeof(ParseStringToLongConverter))]
        public long? folio { get { return Folio; } set { Folio = value; } }

        [JsonProperty("FechaEmision")]
        public DateTime? FechaEmision { get; set; }

        [JsonProperty("fechaEmision")]
        public DateTime? fechaEmision { get { return FechaEmision; } set { FechaEmision = value; } }

        [JsonProperty("RutEmisor")]
        public string RutEmisor { get; set; }

        [JsonProperty("rutEmisor")]
        public string rutEmisor { get { return RutEmisor; } set { RutEmisor = value; } }

        [JsonProperty("RazonSocial")]
        public string RazonSocial { get; set; }

        [JsonProperty("razonSocial")]
        public string razonSocial { get { return RazonSocial; } set { RazonSocial = value; } }

        [JsonProperty("MontoTotal")]
        [JsonConverter(typeof(ParseStringToDoubleConverter))]
        public double? MontoTotal { get; set; }

        [JsonProperty("montoTotal")]
        [JsonConverter(typeof(ParseStringToDoubleConverter))]
        public double? montoTotal { get { return MontoTotal; } set { MontoTotal = value; } }

        [JsonProperty("FechaRecepcion")]
        public DateTime? FechaRecepcion { get; set; }

        [JsonProperty("fechaRecepcion")]
        public DateTime? fechaRecepcion { get { return FechaRecepcion; } set { FechaRecepcion = value; } }

        [JsonProperty("Plazo")]
        public int? Plazo { get { return (FechaEmision - DateTime.Today).Value.Days + 8; } set { Plazo = value; } }

        public string DocId { get { return CreaDocId(); } }

        [JsonProperty("XmlData")]
        public string XmlData { get; set; }

        [JsonProperty("DTE")]
        public string DTE { set { XmlData = value; } }
    }

    public class DocuDTEXML
    {
        [JsonProperty("XmlData")]
        public string XmlData { get; set; }

        public string ImagenLink { get; set; }

        [JsonProperty("contenido")]
        public string contenido { set { XmlData = value; ImagenLink = value; } }
    }

    public class RootDescargarArchivoValidarResponse
    {
        [JsonProperty("root")]
        public RootDescargarArchivoValidar Root { get; set; }
    }

    public class RootDescargarArchivoValidar
    {
        [JsonProperty("descargarArchivoReturn")]
        public DocuDTEXMLValidar Documento { get; set; }
    }

    public class DocuDTEXMLValidar
    {
        [JsonProperty("codigoRespuesta")]
        public string codigoRespuesta { get; set; }
    }

    //public class RootRecuperarPDF
    //{
    //    [JsonProperty("root")]
    //    public RootRecuperarPDFResult Root { get; set; }
    //}

    //public class RootRecuperarPDFResult
    //{
    //    [JsonProperty("RecuperarPDFResult")]
    //    public RecuperarPDFResult RecuperarPDFResult { get; set; }
    //}

    //public class RecuperarPDFResult
    //{
    //    public int? Codigo { get; set; }

    //    [JsonProperty("vCodigo")]
    //    public string vCodigo { set { Codigo = value == "DOK" ? 1 : 0; } }

    //    public string Mensaje { get; set; }

    //    [JsonProperty("vMensaje")]
    //    public string vMensaje { set { Mensaje = value; } }

    //    public string ImagenLink { get; set; }

    //    [JsonProperty("Resultado")]
    //    public string Resultado { set { ImagenLink = value; } }

    //    public string Archivo { get; set; }

    //    [JsonProperty("vArchivo")]
    //    public string vArchivo { set { Archivo = value; } }
    //}

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
