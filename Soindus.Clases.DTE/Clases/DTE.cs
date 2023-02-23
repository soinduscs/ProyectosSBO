using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soindus.Clases.DTE
{
    public class EnvioDTEXML
    {
        [JsonProperty("EnvioDTE")]
        public SetDTEXML EnvioDTE { get; set; }
    }

    public class SetDTEXML
    {
        [JsonProperty("SetDTE")]
        public JsonDTE SetDTE { get; set; }
    }

    public class JsonDTE
    {
        [JsonProperty("DTE")]
        public DTE DTE { get; set; }
    }

    public class DTE
    {
        [JsonProperty("Documento")]
        public Documento Documento { get; set; }
    }
}
