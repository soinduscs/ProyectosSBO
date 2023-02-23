using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Soindus.Clases.DTE;

namespace Soindus.AddOnMonitorProveedores.SoindusFE
{
    public class RootDTEResponse
    {
        [JsonProperty("root")]
        public RootListaResumenDocumento Root { get; set; }
    }

    public class RootListaResumenDocumento
    {
        [JsonProperty("ListaResumenDocumentoResult")]
        public DTEResponse DTEResponse { get; set; }
    }

    public class DTEResponse
    {
        [JsonProperty("ListaDocumentos")]
        public Docums Documentos { get; set; }
    }

    public class RootTraerDocumentoXMLResponse
    {
        [JsonProperty("root")]
        public RootTraerDocumentoXML Root { get; set; }
    }

    public class RootTraerDocumentoXML
    {
        [JsonProperty("TraerDocumentoXMLResult")]
        public DocuDTEXML Documento { get; set; }
    }

    public class Docums
    {
        [JsonProperty("DocProvResumen")]
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

        [JsonProperty("TipoDoc")]
        public int? TipoDoc { get { return Tipo; } set { Tipo = value; } }

        [JsonProperty("TipoDocumento")]
        public int? TipoDocumento { get { return Tipo; } set { Tipo = value; } }

        [JsonProperty("Folio")]
        [JsonConverter(typeof(ParseStringToLongConverter))]
        public long? Folio { get; set; }

        [JsonProperty("FechaEmision")]
        public DateTime? FechaEmision { get; set; }

        [JsonProperty("RutEmisor")]
        public string RutEmisor { get; set; }

        [JsonProperty("RazonSocial")]
        public string RazonSocial { get; set; }

        [JsonProperty("RazonSocialEmisor")]
        public string RazonSocialEmisor { get { return RazonSocial; } set { RazonSocial = value; } }

        [JsonProperty("MontoTotal")]
        [JsonConverter(typeof(ParseStringToDoubleConverter))]
        public double? MontoTotal { get; set; }

        [JsonProperty("FechaRecepcion")]
        public DateTime? FechaRecepcion { get; set; }

        [JsonProperty("Plazo")]
        public int? Plazo { get { return (FechaEmision - DateTime.Today).Value.Days + 8; } set { Plazo = value; } }

        public string DocId { get { return CreaDocId(); } }

        [JsonProperty("TieneXML")]
        public bool TieneXML { get; set; }

        [JsonProperty("ListaRespuestaas")]
        public ListaRespuestaas ListaRespuestass { get; set; }

        [JsonProperty("XmlData")]
        public string XmlData { get; set; }

        [JsonProperty("DTE")]
        public string DTE { set { XmlData = value; } }
    }

    public class DocuDTEXML
    {
        [JsonProperty("XmlData")]
        public string XmlData { get; set; }

        [JsonProperty("DTE")]
        public string DTE { set { XmlData = value; } }
    }

    public class ListaRespuestaas
    {
        [JsonProperty("DocProvRespuesta")]
        [JsonConverter(typeof(ObjectToArrayConverter<DocProvRespuesta>))]
        public DocProvRespuesta[] DocProvRespuesta { get; set; }
    }

    public class DocProvRespuesta
    {
        [JsonProperty("IdResumen")]
        public long? IdResumen { get; set; }
        [JsonProperty("Codigo")]
        public string Codigo { get; set; }
        [JsonProperty("Descripcion")]
        public string Descripcion { get; set; }
        [JsonProperty("FechaAcepRec")]
        public object FechaAcepRec { get; set; }
    }

    public class RootRecuperarPDF
    {
        [JsonProperty("root")]
        public RootRecuperarPDFResult Root { get; set; }
    }

    public class RootRecuperarPDFResult
    {
        [JsonProperty("RecuperarPDFResult")]
        public RecuperarPDFResult RecuperarPDFResult { get; set; }
    }

    public class RecuperarPDFResult
    {
        public int? Codigo { get; set; }

        [JsonProperty("vCodigo")]
        public string vCodigo { set { Codigo = value == "DOK" ? 1 : 0; } }

        public string Mensaje { get; set; }

        [JsonProperty("vMensaje")]
        public string vMensaje { set { Mensaje = value; } }

        public string ImagenLink { get; set; }

        [JsonProperty("Resultado")]
        public string Resultado { set { ImagenLink = value; } }

        public string Archivo { get; set; }

        [JsonProperty("vArchivo")]
        public string vArchivo { set { Archivo = value; } }
    }

    public class RootAceptacionRechazo
    {
        [JsonProperty("root")]
        public RootAceptacionRechazoResult Root { get; set; }
    }

    public class RootAceptacionRechazoResult
    {
        [JsonProperty("AceptacionRechazoResult")]
        public AceptacionRechazoResult AceptacionRechazoResult { get; set; }
    }

    public class AceptacionRechazoResult
    {
        [JsonProperty("Resultado")]
        public string Resultado { get; set; }
        [JsonProperty("Error")]
        public Error Error { get; set; }
    }

    public class Error
    {
        [JsonProperty("Mensaje")]
        public string Mensaje { get; set; }
        [JsonProperty("Numero")]
        public string Numero { get; set; }
    }

    //public partial class FechaAcepRecClass
    //{
    //    [JsonProperty("@nil")]
    //    [JsonConverter(typeof(ParseStringConverter))]
    //    public bool Nil { get; set; }
    //}

    //public partial struct FechaAcepRecUnion
    //{
    //    public DateTimeOffset? DateTime;
    //    public FechaAcepRecClass FechaAcepRecClass;

    //    public static implicit operator FechaAcepRecUnion(DateTimeOffset DateTime) => new FechaAcepRecUnion { DateTime = DateTime };
    //    public static implicit operator FechaAcepRecUnion(FechaAcepRecClass FechaAcepRecClass) => new FechaAcepRecUnion { FechaAcepRecClass = FechaAcepRecClass };
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

    //internal class ParseStringConverter : JsonConverter
    //{
    //    public override bool CanConvert(Type t) => t == typeof(bool) || t == typeof(bool?);

    //    public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
    //    {
    //        if (reader.TokenType == JsonToken.Null) return null;
    //        var value = serializer.Deserialize<string>(reader);
    //        bool b;
    //        if (Boolean.TryParse(value, out b))
    //        {
    //            return b;
    //        }
    //        throw new Exception("Cannot unmarshal type bool");
    //    }

    //    public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
    //    {
    //        if (untypedValue == null)
    //        {
    //            serializer.Serialize(writer, null);
    //            return;
    //        }
    //        var value = (bool)untypedValue;
    //        var boolString = value ? "true" : "false";
    //        serializer.Serialize(writer, boolString);
    //        return;
    //    }

    //    public static readonly ParseStringConverter Singleton = new ParseStringConverter();
    //}
}