using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soindus.AddOnEmisionDTE.SoindusFE
{
    public class XML_RecuperarPDF
    {
        public get_RecuperarPDF Generar(string RutEmisor = "", string TipoDoc = "", string Folio = "", string AmbienteProduccion = "", string Cedible = "")
        {
            get_RecuperarPDF recuperarpdf = new SoindusFE.get_RecuperarPDF();
            recuperarpdf.objDatos.RutEmisor = RutEmisor;
            recuperarpdf.objDatos.TipoDoc = TipoDoc;
            recuperarpdf.objDatos.Folio = Folio;
            recuperarpdf.objDatos.AmbienteProduccion = AmbienteProduccion;
            recuperarpdf.objDatos.Cedible = Cedible;
            return recuperarpdf;
        }
    }

    public class get_RecuperarPDF
    {
        public string intCodigo { get; set; }
        public string strClave { get; set; }
        public objDatos objDatos { get; set; }

        public get_RecuperarPDF()
        {
            objDatos = new SoindusFE.objDatos();
        }
    }
}
