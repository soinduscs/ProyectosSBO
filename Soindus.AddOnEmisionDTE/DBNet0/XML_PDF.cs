using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soindus.AddOnEmisionDTE.DBNet0
{
    public class XML_PDF
    {
        public get_pdf Generar(string rutt = "", string folio = "", string doc = "", string monto = "", string fecha = "", string merito = "false")
        {
            get_pdf pdf = new DBNet0.get_pdf();
            pdf.sRutt = rutt;
            pdf.sFolio = folio;
            pdf.sDoc = doc;
            pdf.sMonto = monto;
            pdf.sFecha = fecha;
            pdf.bMerito = merito;
            return pdf;
        }
    }

    public class get_pdf
    {
        public string sRutt { get; set; }
        public string sFolio { get; set; }
        public string sDoc { get; set; }
        public string sMonto { get; set; }
        public string sFecha { get; set; }
        public string bMerito { get; set; }
    }
}
