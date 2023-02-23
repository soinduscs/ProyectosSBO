using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soindus.Clases.DTE
{
    public class Comisiones
    {
        [JsonProperty("NroLinCom")]
        public int NroLinCom { get; set; }
        [JsonProperty("TipoMovim")]
        public string TipoMovim { get; set; }
        [JsonProperty("Glosa")]
        public string Glosa { get; set; }
        [JsonProperty("TasaComision")]
        public double TasaComision { get; set; }
        [JsonProperty("ValComNeto")]
        public long ValComNeto { get; set; }
        [JsonProperty("ValComExe")]
        public long ValComExe { get; set; }
        [JsonProperty("ValComIVA")]
        public long ValComIVA { get; set; }
    }
}
