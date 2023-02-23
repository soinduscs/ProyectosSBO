using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Soindus.AddOnMonitorEmision.DBNet
{
    public class ConsultaEstadoResponse
    {
        [JsonProperty("root")]
        public RootConsultaEstado Root { get; set; }
    }

    public class RootConsultaEstado
    {
        [JsonProperty("ConsultaEstadoResult")]
        public ConsultaEstadoResult ConsultaEstadoResult { get; set; }
    }

    public partial class ConsultaEstadoResult
    {
        [JsonProperty("#text")]
        public string Estado { get; set; }
    }
}
