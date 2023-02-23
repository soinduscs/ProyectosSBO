using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Converters;

namespace Soindus.AddOnRindegastos.Clases
{
    public partial class Gastos
    {
        public List<Gasto> Items { get; set; }

        public Gastos()
        {
            Items = new List<Gasto>();
        }
    }

    public partial class Gasto
    {
        [JsonProperty("GastosRendicionesRG")]
        public GastosRendicionesRg Detalle { get; set; }
    }

    public partial class GastosRendicionesRg
    {
        [JsonProperty("DocNum")]
        public long DocNum { get; set; }

        //[JsonProperty("Period")]
        //public long Period { get; set; }

        //[JsonProperty("Instance")]
        //public long Instance { get; set; }

        //[JsonProperty("Series")]
        //public long Series { get; set; }

        //[JsonProperty("Handwrtten")]
        //public string Handwrtten { get; set; }

        //[JsonProperty("Status")]
        //public string Status { get; set; }

        //[JsonProperty("RequestStatus")]
        //public string RequestStatus { get; set; }

        //[JsonProperty("Creator")]
        //public string Creator { get; set; }

        //[JsonProperty("Remark")]
        //public LogInst Remark { get; set; }

        [JsonProperty("DocEntry")]
        public long DocEntry { get; set; }

        //[JsonProperty("Canceled")]
        //public string Canceled { get; set; }

        //[JsonProperty("Object")]
        //public string Object { get; set; }

        //[JsonProperty("LogInst")]
        //public LogInst LogInst { get; set; }

        //[JsonProperty("UserSign")]
        //public long UserSign { get; set; }

        //[JsonProperty("Transfered")]
        //public string Transfered { get; set; }

        //[JsonProperty("CreateDate")]
        //public DateTimeOffset CreateDate { get; set; }

        //[JsonProperty("CreateTime")]
        //public DateTimeOffset CreateTime { get; set; }

        //[JsonProperty("UpdateDate")]
        //public LogInst UpdateDate { get; set; }

        //[JsonProperty("UpdateTime")]
        //public LogInst UpdateTime { get; set; }

        //[JsonProperty("DataSource")]
        //public string DataSource { get; set; }

        [JsonProperty("U_ID")]
        public long UId { get; set; }

        [JsonProperty("U_STATUS")]
        public long UStatus { get; set; }

        [JsonProperty("U_SUPPLIER")]
        public string USupplier { get; set; }

        [JsonProperty("U_IDATE")]
        public DateTimeOffset UIdate { get; set; }

        [JsonProperty("U_OAMOUNT")]
        public double UOamount { get; set; }

        [JsonProperty("U_OCUR")]
        public string UOcur { get; set; }

        [JsonProperty("U_EXRATE")]
        public double UExrate { get; set; }

        [JsonProperty("U_NET")]
        public double UNet { get; set; }

        [JsonProperty("U_TAX")]
        public double UTax { get; set; }

        [JsonProperty("U_TAXNAME")]
        public string UTaxname { get; set; }

        [JsonProperty("U_OTAX")]
        public double UOtax { get; set; }

        [JsonProperty("U_RETNAME")]
        public string URetname { get; set; }

        [JsonProperty("U_RET")]
        public double URet { get; set; }

        [JsonProperty("U_TOTAL")]
        public double UTotal { get; set; }

        [JsonProperty("U_CUR")]
        public string UCur { get; set; }

        [JsonProperty("U_REIMB")]
        public long UReimb { get; set; }

        [JsonProperty("U_CATEGORY")]
        public string UCategory { get; set; }

        [JsonProperty("U_CATCODE")]
        //[JsonConverter(typeof(PurpleParseStringConverter))]
        public string UCatcode { get; set; }

        [JsonProperty("U_CATGRP")]
        public string UCatgrp { get; set; }

        [JsonProperty("U_CATGRPC")]
        public string UCatgrpc { get; set; }

        [JsonProperty("U_NOTE")]
        public string UNote { get; set; }

        [JsonProperty("U_INTDATE")]
        public DateTimeOffset UIntdate { get; set; }

        [JsonProperty("U_INTECODE")]
        public string UIntecode { get; set; }

        [JsonProperty("U_NBRFILES")]
        public long UNbrfiles { get; set; }

        [JsonProperty("U_RID")]
        public long URid { get; set; }

        [JsonProperty("U_EXPOLID")]
        public long UExpolid { get; set; }

        [JsonProperty("U_USERID")]
        public long UUserid { get; set; }

        [JsonProperty("SO_RENDIGCECollection")]
        public SoRendigceCollection CamposExtra { get; set; }
    }

    public partial class SoRendigceCollection
    {
        [JsonProperty("SO_RENDIGCE")]
        [JsonConverter(typeof(ObjectToArrayConverter<SoRendigce>))]
        public SoRendigce[] Detalle { get; set; }
    }

    public partial class SoRendigce
    {
        [JsonProperty("DocEntry")]
        public long DocEntry { get; set; }

        [JsonProperty("LineId")]
        public long LineId { get; set; }

        [JsonProperty("VisOrder")]
        public long VisOrder { get; set; }

        [JsonProperty("Object")]
        public string Object { get; set; }

        [JsonProperty("LogInst")]
        public LogInst LogInst { get; set; }

        [JsonProperty("U_NAME")]
        public string UName { get; set; }

        [JsonProperty("U_VALUE")]
        public string UValue { get; set; }

        [JsonProperty("U_CODE")]
        public string UCode { get; set; }
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
            return l;
            //throw new Exception("Cannot unmarshal type long");
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
