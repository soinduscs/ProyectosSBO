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
    public partial class ResponseFunds
    {
        [JsonProperty("Records")]
        public RecordsFunds Records { get; set; }

        [JsonProperty("Funds")]
        public Fund[] Funds { get; set; }
    }

    public partial class Fund
    {
        [JsonProperty("Id")]
        public long Id { get; set; }

        [JsonProperty("Title")]
        public string Title { get; set; }

        [JsonProperty("Code")]
        public string Code { get; set; }

        [JsonProperty("Currency")]
        public string Currency { get; set; }

        [JsonProperty("IdAssignTo")]
        public long IdAssignTo { get; set; }

        [JsonProperty("IdCreator")]
        public long IdCreator { get; set; }

        [JsonProperty("Deposits")]
        public double Deposits { get; set; }

        [JsonProperty("Charges")]
        public double Charges { get; set; }

        [JsonProperty("Withdrawals")]
        public double Withdrawals { set { Charges = value; } }

        [JsonProperty("Balance")]
        public double Balance { get; set; }

        [JsonProperty("Status")]
        public long Status { get; set; }

        [JsonProperty("CreatedAt")]
        public DateTimeOffset CreatedAt { get; set; }

        [JsonProperty("ExpirationDate")]
        public string ExpirationDate { get; set; }

        [JsonProperty("FlexibleFund")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long FlexibleFund { get; set; }

        [JsonProperty("ManualDeposit")]
        public bool ManualDeposit { get; set; }

        [JsonProperty("AutomaticBlock")]
        public bool AutomaticBlock { get; set; }
    }

    public partial class RecordsFunds
    {
        [JsonProperty("TotalRecords")]
        public long TotalRecords { get; set; }

        [JsonProperty("Funds")]
        public long Funds { get; set; }

        [JsonProperty("Page")]
        public long Page { get; set; }

        [JsonProperty("Pages")]
        public long Pages { get; set; }
    }

    public partial class ResponseFunds
    {
        public static ResponseFunds FromJson(string json) => JsonConvert.DeserializeObject<ResponseFunds>(json, ConverterFunds.Settings);
    }

    public static class SerializeFunds
    {
        public static string ToJson(this ResponseFunds self) => JsonConvert.SerializeObject(self, ConverterFunds.Settings);
    }

    internal static class ConverterFunds
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
}
