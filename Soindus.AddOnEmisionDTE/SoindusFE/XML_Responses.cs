using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soindus.AddOnEmisionDTE.SoindusFE
{
    public class RootGenerarDocumentoElectronico
    {
        [JsonProperty("root")]
        public RootGenerarDocumentoElectronicoResult Root { get; set; }
    }

    public class RootGenerarDocumentoElectronicoResult
    {
        [JsonProperty("GenerarDocumentoElectronicoResult")]
        public GenerarDocumentoElectronicoResult GenerarDocumentoElectronicoResult { get; set; }
    }

    public class GenerarDocumentoElectronicoResult
    {
        [JsonProperty("Error")]
        public MsgError Error { get; set; }

        [JsonProperty("Resultado")]
        public string Resultado { get; set; }
    }

    public class RootGenerarBoletaElectronica
    {
        [JsonProperty("root")]
        public RootGenerarBoletaElectronicaResult Root { get; set; }
    }

    public class RootGenerarBoletaElectronicaResult
    {
        [JsonProperty("GenerarBoletaElectronicaResult")]
        public GenerarBoletaElectronicaResult GenerarBoletaElectronicaResult { get; set; }
    }

    public class GenerarBoletaElectronicaResult
    {
        [JsonProperty("Error")]
        public MsgError Error { get; set; }

        [JsonProperty("Resultado")]
        public string Resultado { get; set; }
    }

    public class MsgError
    {
        [JsonProperty("Mensaje")]
        public string Mensaje { get; set; }
        [JsonProperty("Numero")]
        public string Numero { get; set; }
    }

    public class RootRecuperarPDF
    {
        [JsonProperty("root")]
        public RootRecuperarPDFResult Root { get; set; }
    }

    public class RootRecuperarPDFResult
    {
        [JsonProperty("RecuperarPDFResult")]
        public RecuperarPDFResult RecuperarPDFResult { get; set; }
    }

    public class RecuperarPDFResult
    {
        [JsonProperty("Error")]
        public MsgError Error { get; set; }

        [JsonProperty("Resultado")]
        public string Resultado { get; set; }
        //public string Resultado { set { ImagenLink = value; } }

        //public string ImagenLink { get; set; }
    }
}
