using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soindus.AddOnMonitorProveedores.SoindusFE
{
    public class XML_TraerDocumentoXML
    {
        public get_TraerDocumentoXML Generar(string RutEmisor = "", string TipoDoc = "", string Folio = "")
        {
            get_TraerDocumentoXML traerdocumentoxml = new SoindusFE.get_TraerDocumentoXML();
            traerdocumentoxml.objParamDocProv.RutEmisor = RutEmisor;
            traerdocumentoxml.objParamDocProv.TipoDoc = TipoDoc;
            traerdocumentoxml.objParamDocProv.Folio = Folio;
            return traerdocumentoxml;
        }
    }

    public class get_TraerDocumentoXML
    {
        public string intCodigo { get; set; }
        public string strClave { get; set; }
        public objParamDocProv objParamDocProv { get; set; }

        public get_TraerDocumentoXML()
        {
            objParamDocProv = new SoindusFE.objParamDocProv();
        }
    }
}
