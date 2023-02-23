using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soindus.AddOnMonitorProveedores.SoindusFE
{
    public class XML_AceptacionRechazo
    {
        public get_AceptacionRechazo Generar(string RutEmisor = "", string DV = "", string TipoDoc = "", string Folio = "", string Accion = "")
        {
            get_AceptacionRechazo aceptacionrechazo = new SoindusFE.get_AceptacionRechazo();
            aceptacionrechazo.objParamDocProv.RutEmisor = RutEmisor;
            aceptacionrechazo.objParamDocProv.DV = DV;
            aceptacionrechazo.objParamDocProv.TipoDoc = TipoDoc;
            aceptacionrechazo.objParamDocProv.Folio = Folio;
            aceptacionrechazo.objParamDocProv.Accion = Accion;
            return aceptacionrechazo;
        }
    }

    public class get_AceptacionRechazo
    {
        public string intCodigo { get; set; }
        public string strClave { get; set; }
        public objParamDocProv objParamDocProv { get; set; }

        public get_AceptacionRechazo()
        {
            objParamDocProv = new SoindusFE.objParamDocProv();
        }
    }
}
