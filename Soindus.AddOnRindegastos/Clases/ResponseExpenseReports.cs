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
    public partial class ResponseExpenseReports
    {
        [JsonProperty("Records")]
        public RecordsExpenseReports Records { get; set; }

        [JsonProperty("ExpenseReports")]
        public ExpenseReport[] ExpenseReports { get; set; }
    }

    public partial class ExpenseReport
    {
        [JsonProperty("Id")]
        public long Id { get; set; }

        [JsonProperty("Title")]
        public string Title { get; set; }

        [JsonProperty("ReportNumber")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long ReportNumber { get; set; }

        [JsonProperty("SendDate")]
        public DateTimeOffset SendDate { get; set; }

        [JsonProperty("CloseDate")]
        public DateTimeOffset CloseDate { get; set; }

        [JsonProperty("EmployeeId")]
        public long EmployeeId { get; set; }

        [JsonProperty("EmployeeName")]
        public string EmployeeName { get; set; }

        [JsonProperty("EmployeeIdentification")]
        public string EmployeeIdentification { get; set; }

        [JsonProperty("ApproverId")]
        public long ApproverId { get; set; }

        [JsonProperty("ApproverName")]
        public string ApproverName { get; set; }

        [JsonProperty("PolicyId")]
        public long PolicyId { get; set; }

        [JsonProperty("PolicyName")]
        public string PolicyName { get; set; }

        [JsonProperty("Status")]
        public long Status { get; set; }

        [JsonProperty("CustomStatus")]
        public string CustomStatus { get; set; }

        [JsonProperty("FundId")]
        public long FundId { get; set; }

        [JsonProperty("FundName")]
        public string FundName { get; set; }

        [JsonProperty("ReportTotal")]
        public double ReportTotal { get; set; }

        [JsonProperty("ReportTotalApproved")]
        public double ReportTotalApproved { get; set; }

        [JsonProperty("Currency")]
        public string Currency { get; set; }

        [JsonProperty("Note")]
        public string Note { get; set; }

        [JsonProperty("Integrated")]
        public string Integrated { get; set; }

        [JsonProperty("IntegrationDate")]
        public string IntegrationDate { get; set; }

        [JsonProperty("IntegrationExternalCode")]
        public string IntegrationExternalCode { get; set; }

        [JsonProperty("IntegrationInternalCode")]
        public string IntegrationInternalCode { get; set; }

        [JsonProperty("NbrExpenses")]
        public long NbrExpenses { get; set; }

        [JsonProperty("NbrApprovedExpenses")]
        public long NbrApprovedExpenses { get; set; }

        [JsonProperty("NbrRejectedExpenses")]
        public long NbrRejectedExpenses { get; set; }

        [JsonProperty("ExtraFields")]
        public ExtraFieldExpenseReports[] ExtraFields { get; set; }

        [JsonProperty("Files")]
        public object[] Files { get; set; }
    }

    public partial class ExtraFieldExpenseReports
    {
        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Value")]
        public string Value { get; set; }

        [JsonProperty("Code")]
        public string Code { get; set; }
    }

    public partial class RecordsExpenseReports
    {
        [JsonProperty("TotalRecords")]
        public long TotalRecords { get; set; }

        [JsonProperty("Reports")]
        public long Reports { get; set; }

        [JsonProperty("Page")]
        public long Page { get; set; }

        [JsonProperty("Pages")]
        public long Pages { get; set; }
    }

    public partial class ResponseExpenseReports
    {
        public static ResponseExpenseReports FromJson(string json) => JsonConvert.DeserializeObject<ResponseExpenseReports>(json, ConverterExpenseReports.Settings);
    }

    public static class SerializeExpenseReports
    {
        public static string ToJson(this ResponseExpenseReports self) => JsonConvert.SerializeObject(self, ConverterExpenseReports.Settings);
    }

    internal static class ConverterExpenseReports
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

    internal class ParseStringConverter : JsonConverter
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

        public static readonly ParseStringConverter Singleton = new ParseStringConverter();
    }
}
