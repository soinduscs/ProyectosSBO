using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soindus.Clases.DTE
{
    public class Documento
    {
        [JsonProperty("Encabezado")]
        public Encabezado Encabezado { get; set; }

        [JsonProperty("Detalle")]
        [JsonConverter(typeof(ObjectToArrayConverter<Detalle>))]
        public Detalle[] Detalle { get; set; }

        [JsonProperty("SubTotInfo")]
        [JsonConverter(typeof(ObjectToArrayConverter<SubTotInfo>))]
        public SubTotInfo[] SubTotInfo { get; set; }

        [JsonProperty("DscRcgGlobal")]
        [JsonConverter(typeof(ObjectToArrayConverter<DscRcgGlobal>))]
        public DscRcgGlobal[] DscRcgGlobal { get; set; }

        [JsonProperty("Referencia")]
        [JsonConverter(typeof(ObjectToArrayConverter<Referencia>))]
        public Referencia[] Referencia { get; set; }

        [JsonProperty("Comisiones")]
        [JsonConverter(typeof(ObjectToArrayConverter<Comisiones>))]
        public Comisiones[] Comisiones { get; set; }
    }
}
