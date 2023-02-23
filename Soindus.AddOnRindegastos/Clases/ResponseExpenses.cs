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
    public partial class ResponseExpenses
    {
        [JsonProperty("Records")]
        public RecordsExpenses Records { get; set; }

        [JsonProperty("Expenses")]
        public Expense[] Expenses { get; set; }
    }

    public partial class Expense
    {
        [JsonProperty("Id")]
        public long Id { get; set; }

        [JsonProperty("Status")]
        public long Status { get; set; }

        [JsonProperty("Supplier")]
        public string Supplier { get; set; }

        [JsonProperty("IssueDate")]
        public DateTimeOffset IssueDate { get; set; }

        [JsonProperty("OriginalAmount")]
        public double OriginalAmount { get; set; }

        [JsonProperty("OriginalCurrency")]
        public string OriginalCurrency { get; set; }

        [JsonProperty("ExchangeRate")]
        public double ExchangeRate { get; set; }

        [JsonProperty("Net")]
        public double Net { get; set; }

        [JsonProperty("Tax")]
        public double Tax { get; set; }

        [JsonProperty("TaxName")]
        public string TaxName { get; set; }

        [JsonProperty("OtherTaxes")]
        public double OtherTaxes { get; set; }

        [JsonProperty("RetentionName")]
        public string RetentionName { get; set; }

        [JsonProperty("Retention")]
        public double Retention { get; set; }

        [JsonProperty("Total")]
        public double Total { get; set; }

        [JsonProperty("Currency")]
        public string Currency { get; set; }

        [JsonProperty("Reimbursable")]
        public bool Reimbursable { get; set; }

        [JsonProperty("Category")]
        public string Category { get; set; }

        [JsonProperty("CategoryCode")]
        public string CategoryCode { get; set; }

        [JsonProperty("CategoryGroup")]
        public string CategoryGroup { get; set; }

        [JsonProperty("CategoryGroupCode")]
        public string CategoryGroupCode { get; set; }

        [JsonProperty("Note")]
        public string Note { get; set; }

        [JsonProperty("IntegrationDate")]
        public string IntegrationDate { get; set; }

        [JsonProperty("IntegrationExternalCode")]
        public string IntegrationExternalCode { get; set; }

        [JsonProperty("ExtraFields")]
        public ExtraFieldExpenses[] ExtraFields { get; set; }

        [JsonProperty("Files")]
        public FileExpenses[] Files { get; set; }

        [JsonProperty("NbrFiles")]
        public long NbrFiles { get; set; }

        [JsonProperty("ReportId")]
        public long ReportId { get; set; }

        [JsonProperty("ExpensePolicyId")]
        public long ExpensePolicyId { get; set; }

        [JsonProperty("UserId")]
        public long UserId { get; set; }
    }

    public partial class ExtraFieldExpenses
    {
        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Value")]
        public string Value { get; set; }

        [JsonProperty("Code")]
        public string Code { get; set; }
    }

    public partial class FileExpenses
    {
        [JsonProperty("FileName")]
        public string FileName { get; set; }

        [JsonProperty("Extension")]
        public string Extension { get; set; }

        [JsonProperty("Original")]
        public Uri Original { get; set; }

        [JsonProperty("Large")]
        public Uri Large { get; set; }

        [JsonProperty("Medium")]
        public Uri Medium { get; set; }

        [JsonProperty("Small")]
        public Uri Small { get; set; }
    }

    public partial class RecordsExpenses
    {
        [JsonProperty("TotalRecords")]
        public long TotalRecords { get; set; }

        [JsonProperty("Expenses")]
        public long Expenses { get; set; }

        [JsonProperty("Page")]
        public long Page { get; set; }

        [JsonProperty("Pages")]
        public long Pages { get; set; }
    }

    public partial class ResponseExpenses
    {
        public static ResponseExpenses FromJson(string json) => JsonConvert.DeserializeObject<ResponseExpenses>(json, ConverterExpenses.Settings);
    }

    public static class SerializeExpenses
    {
        public static string ToJson(this ResponseExpenses self) => JsonConvert.SerializeObject(self, ConverterExpenses.Settings);
    }

    internal static class ConverterExpenses
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

