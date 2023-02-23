using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soindus.Clases.DTE
{
    public class Encabezado
    {
        [JsonProperty("IdDoc")]
        public IdDoc IdDoc { get; set; }
        [JsonProperty("Emisor")]
        public Emisor Emisor { get; set; }
        [JsonProperty("Receptor")]
        public Receptor Receptor { get; set; }
        [JsonProperty("Transporte")]
        public Transporte TRansporte { get; set; }
        [JsonProperty("Totales")]
        public Totales Totales { get; set; }
        [JsonProperty("OtraMoneda")]
        public OtraMoneda OtraMoneda { get; set; }
    }
}
