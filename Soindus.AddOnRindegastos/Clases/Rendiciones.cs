using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Soindus.AddOnRindegastos.Clases
{
    public partial class Rendiciones
    {
        public List<Rendicion> Items { get; set; }

        public Rendiciones()
        {
            Items = new List<Rendicion>();
        }
    }

    public partial class Rendicion
    {
        [JsonProperty("RendicionesRG")]
        public RendicionesRg Detalle { get; set; }
    }

    public partial class RendicionesRg
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

        [JsonProperty("U_TITLE")]
        public string UTitle { get; set; }

        [JsonProperty("U_REPNUM")]
        public long URepnum { get; set; }

        [JsonProperty("U_SDATE")]
        public DateTimeOffset USdate { get; set; }

        [JsonProperty("U_CDATE")]
        public DateTimeOffset UCdate { get; set; }

        [JsonProperty("U_EMPID")]
        public long UEmpid { get; set; }

        [JsonProperty("U_EMPNAME")]
        public string UEmpname { get; set; }

        [JsonProperty("U_EMPIDE")]
        public string UEmpide { get; set; }

        [JsonProperty("U_APPID")]
        public long UAppid { get; set; }

        [JsonProperty("U_APPNAME")]
        public string UAppname { get; set; }

        [JsonProperty("U_POLID")]
        public long UPolid { get; set; }

        [JsonProperty("U_POLNAME")]
        public string UPolname { get; set; }

        [JsonProperty("U_STATUS")]
        public long UStatus { get; set; }

        [JsonProperty("U_CSTATUS")]
        public string UCstatus { get; set; }

        [JsonProperty("U_FUNDID")]
        public long UFundid { get; set; }

        [JsonProperty("U_FUNDNAME")]
        public string UFundname { get; set; }

        [JsonProperty("U_REPTOT")]
        public double UReptot { get; set; }

        [JsonProperty("U_REPTOTA")]
        public double UReptota { get; set; }

        [JsonProperty("U_CUR")]
        public string UCur { get; set; }

        [JsonProperty("U_NOTE")]
        public string UNote { get; set; }

        [JsonProperty("U_INTEG")]
        public bool UInteg { get; set; }

        [JsonProperty("U_INTDATE")]
        public string UIntdate { get; set; }

        [JsonProperty("U_INTECODE")]
        public string UIntecode { get; set; }

        [JsonProperty("U_INTICODE")]
        public string UInticode { get; set; }

        [JsonProperty("U_NBREXP")]
        public long UNbrexp { get; set; }

        [JsonProperty("U_NBRAEXP")]
        public long UNbraexp { get; set; }

        [JsonProperty("U_NBRREXP")]
        public long UNbrrexp { get; set; }

        [JsonProperty("U_ESTADO")]
        public long UEstado { get; set; }

        [JsonProperty("U_OBS")]
        public string UObs { get; set; }

        [JsonProperty("U_GESTION")]
        public string UGestion { get; set; }

        [JsonProperty("SO_RENDICECollection")]
        public SoRendiceCollection CamposExtra { get; set; }

        public Gastos Gastos { get; set; }
    }

    public partial class LogInst
    {
        [JsonProperty("@nil")]
        [JsonConverter(typeof(FluffyParseStringConverter))]
        public bool Nil { get; set; }
    }

    public partial class SoRendiceCollection
    {
        [JsonProperty("SO_RENDICE")]
        public SoRendice[] Detalle { get; set; }
    }

    public partial class SoRendice
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

}
