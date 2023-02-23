using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Soindus.Clases.DTE
{
    public class ConsultaEstadoResponse
    {
        public RootConsultaEstado Root { get; set; }
    }

    public class RootConsultaEstado
    {
        public ConsultaEstadoResult ConsultaEstadoResult { get; set; }
    }

    public partial class ConsultaEstadoResult
    {
        [JsonProperty("#text")]
        public string Estado { get; set; }
    }

    public class EstadosEmision
    {
        public string Estado1 { get; set; }
        public string MsgEstado1 { get; set; }
        public string Estado2 { get; set; }
        public string MsgEstado2 { get; set; }
        public string Estado3 { get; set; }
        public string MsgEstado3 { get; set; }
    }
}
