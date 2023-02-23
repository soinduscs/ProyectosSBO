using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Soindus.Svc.TipoCambio.BCC
{
    public partial class GetSeriesResponse
    {
        [JsonProperty("root")]
        public Root Root { get; set; }
    }

    public partial class Root
    {
        [JsonProperty("GetSeriesResult")]
        public GetSeriesResult GetSeriesResult { get; set; }
    }

    public partial class GetSeriesResult
    {
        [JsonProperty("Codigo")]
        [JsonConverter(typeof(PurpleParseStringConverter))]
        public long Codigo { get; set; }
        [JsonProperty("Descripcion")]
        public object Descripcion { get; set; }
        [JsonProperty("Series")]
        public Series Series { get; set; }
        [JsonProperty("SeriesInfos")]
        public object SeriesInfos { get; set; }
    }

    public partial class Series
    {
        [JsonProperty("fameSeries")]
        public FameSeries FameSeries { get; set; }
    }

    public partial class FameSeries
    {
        [JsonProperty("header")]
        public Header Header { get; set; }
        [JsonProperty("seriesKey")]
        public SeriesKey SeriesKey { get; set; }
        [JsonProperty("precision")]
        public Header Precision { get; set; }
        [JsonProperty("obs")]
        [JsonConverter(typeof(ObjectToArrayConverter<Obs>))]
        public Obs[] Obs { get; set; }
    }

    public partial class Header
    {
        [JsonProperty("#text")]
        public string Text { get; set; }
    }

    public partial class Obs
    {
        public DateTime Fecha { get; set; }
        //[JsonProperty("indexDateString")]
        //public string IndexDateString { set { Fecha = Convert.ToDateTime(string.Format("{0:DD-MM-YYYY}", value), CultureInfo.CurrentCulture); } }
        [JsonProperty("indexDateString")]
        public string IndexDateString { set { Fecha = DateTime.ParseExact(value, "dd-MM-yyyy", CultureInfo.CurrentCulture); } }
        [JsonProperty("seriesKey")]
        public SeriesKey SeriesKey { get; set; }
        [JsonProperty("statusCode")]
        public string StatusCode { get; set; }
        [JsonProperty("value")]
        public string Value { get; set; }
    }

    public partial class SeriesKey
    {
        [JsonProperty("keyFamilyId")]
        public string KeyFamilyId { get; set; }
        [JsonProperty("seriesId")]
        public string SeriesId { get; set; }
        [JsonProperty("dataStage")]
        public string DataStage { get; set; }
        [JsonProperty("exists")]
        [JsonConverter(typeof(FluffyParseStringConverter))]
        public bool Exists { get; set; }
    }

    public partial class GetSeriesResponse
    {
        public static GetSeriesResponse FromJson(string json) => JsonConvert.DeserializeObject<GetSeriesResponse>(json, Soindus.Svc.TipoCambio.BCC.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this GetSeriesResponse self) => JsonConvert.SerializeObject(self, Soindus.Svc.TipoCambio.BCC.Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class PurpleParseStringConverter : JsonConverter
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

        public static readonly PurpleParseStringConverter Singleton = new PurpleParseStringConverter();
    }

    internal class FluffyParseStringConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(bool) || t == typeof(bool?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            bool b;
            if (Boolean.TryParse(value, out b))
            {
                return b;
            }
            throw new Exception("Cannot unmarshal type bool");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (bool)untypedValue;
            var boolString = value ? "true" : "false";
            serializer.Serialize(writer, boolString);
            return;
        }

        public static readonly FluffyParseStringConverter Singleton = new FluffyParseStringConverter();
    }

    public class ObjectToArrayConverter<T> : Newtonsoft.Json.Converters.CustomCreationConverter<T[]>
    {
        public override T[] Create(Type objectType)
        {
            return new T[0];
        }

        public override object ReadJson(Newtonsoft.Json.JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            if (reader.TokenType == Newtonsoft.Json.JsonToken.StartArray)
            {
                return serializer.Deserialize(reader, objectType);
            }
            else
            {
                return new T[] { serializer.Deserialize<T>(reader) };
            }
        }
    }
}
