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
    public partial class ResponseUsers
    {
        [JsonProperty("Records")]
        public RecordsUsers Records { get; set; }

        [JsonProperty("Users")]
        public User[] Users { get; set; }
    }

    public partial class RecordsUsers
    {
        [JsonProperty("TotalRecords")]
        public long TotalRecords { get; set; }

        [JsonProperty("Users")]
        public long Users { get; set; }

        [JsonProperty("Page")]
        public long Page { get; set; }

        [JsonProperty("Pages")]
        public long Pages { get; set; }
    }

    public partial class User
    {
        [JsonProperty("Id")]
        public long Id { get; set; }

        [JsonProperty("FirstName")]
        public string FirstName { get; set; }

        [JsonProperty("LastName")]
        public string LastName { get; set; }

        [JsonProperty("Email")]
        public string Email { get; set; }

        [JsonProperty("Identification")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long Identification { get; set; }

        [JsonProperty("CostingCode")]
        public string CostingCode { get; set; }

        [JsonProperty("EmployeeNumber")]
        public string EmployeeNumber { get; set; }

        [JsonProperty("Department")]
        public string Department { get; set; }

        [JsonProperty("Position")]
        public string Position { get; set; }

        [JsonProperty("CreatedAt")]
        public DateTimeOffset CreatedAt { get; set; }

        [JsonProperty("LastLogin")]
        public DateTimeOffset LastLogin { get; set; }

        [JsonProperty("Role")]
        public Role[] Role { get; set; }
    }

    public partial class Role
    {
        [JsonProperty("Admin")]
        public bool Admin { get; set; }

        [JsonProperty("Management")]
        public bool Management { get; set; }
    }

    public partial class ResponseUsers
    {
        public static ResponseUsers FromJson(string json) => JsonConvert.DeserializeObject<ResponseUsers>(json, ConverterUsers.Settings);
    }

    public static class SerializeUsers
    {
        public static string ToJson(this ResponseUsers self) => JsonConvert.SerializeObject(self, ConverterUsers.Settings);
    }

    internal static class ConverterUsers
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
