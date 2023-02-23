using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soindus.AddOnFactoringCob.Clases
{
    public class Cobranza
    {
        public Cobranza()
        {
            Documentos = new List<Clases.CobranzaDocs>();
        }

        public List<CobranzaDocs> Documentos { get; set; }
    }

    public class CobranzaDocs
    {
        public int DocEntry { get; set; }
        public string U_OBJTYPE { get; set; }
        public int U_DOCENTRY { get; set; }
        public string U_TIPODOC { get; set; }
        public int U_DOCNUM { get; set; }
        public int U_FOLIONUM { get; set; }
        public DateTime U_DOCDATE { get; set; }
        public DateTime U_DOCDUEDATE { get; set; }
        public DateTime U_TAXDATE { get; set; }
        public string U_DOCCUR { get; set; }
        public string U_DOCCURFC { get; set; }
        public double U_DOCTOTAL { get; set; }
        public double U_FUTDUE { get; set; }
        public string U_CARDCODE { get; set; }
        public string U_CARDNAME { get; set; }
        public string U_EMAIL { get; set; }
        public string U_EMAILTYPE { get; set; }
        public string U_EMAILLANG { get; set; }
        public string U_OBS { get; set; }
        public DateTime U_SENDDATE { get; set; }
        public string U_SENDTIME { get; set; }
        public int U_FACID { get; set; }
        public string U_FACENTIDAD { get; set; }
        public DateTime U_FACFECHA { get; set; }
        public string U_FACOPER { get; set; }
        public string U_FACRESP { get; set; }
    }
}
