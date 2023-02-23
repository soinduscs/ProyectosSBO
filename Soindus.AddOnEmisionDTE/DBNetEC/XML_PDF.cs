using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soindus.AddOnEmisionDTE.DBNetEC
{
    public class XML_PDF
    {
        public get_pdf Generar(string rutt = "", string folio = "", string doc = "", string monto = "", string fecha = "", string merito = "false")
        {
            get_pdf pdf = new DBNetEC.get_pdf();
            pdf.rutt = rutt;
            pdf.folio = folio;
            pdf.doc = doc;
            pdf.monto = monto;
            pdf.fecha = fecha;
            pdf.merito = merito;
            return pdf;
        }
    }

    public class get_pdf
    {
        public string rutt { get; set; }
        public string folio { get; set; }
        public string doc { get; set; }
        public string monto { get; set; }
        public string fecha { get; set; }
        public string merito { get; set; }
    }
}
