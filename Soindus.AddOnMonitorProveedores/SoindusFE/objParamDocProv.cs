using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soindus.AddOnMonitorProveedores.SoindusFE
{
    public class objParamDocProv
    {
        //ListaResumenDocumento
        public string Desde { get; set; }
        public string Hasta { get; set; }
        //TraerDocumentoXML
        public string RutEmisor { get; set; }
        public string DV { get; set; }
        public string TipoDoc { get; set; }
        public string Folio { get; set; }
        public string Accion { get; set; }
    }
}
