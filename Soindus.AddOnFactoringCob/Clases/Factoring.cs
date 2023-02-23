using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soindus.AddOnFactoringCob.Clases
{
    public class Factoring
    {
        public Factoring()
        {
            Documentos = new List<FactoringLines>();
        }

        public int DocEntry { get; set; }
        public int U_ID { get; set; }
        public string U_ENTIDAD { get; set; }
        public string U_MONEDA { get; set; }
        public DateTime U_FECHA { get; set; }
        public string U_NUMOPER { get; set; }
        public double U_VALOR { get; set; }
        public string U_TIPORES { get; set; }
        public string U_ESTADO { get; set; }
        public List<FactoringLines> Documentos { get; set; }
    }

    public class FactoringLines
    {
        public int DocEntry { get; set; }
        public int LineId { get; set; }
        public string U_OBJTYPE { get; set; }
        public int U_DOCENTRY { get; set; }
        public int U_BASEENTRY { get; set; }
        public string U_BASEREF { get; set; }
        public string U_TIPODOC { get; set; }
        public int U_DOCNUM { get; set; }
        public int U_FOLIONUM { get; set; }
        public string U_ISINS { get; set; }
        public string U_INDICATOR { get; set; }
        public DateTime U_DOCDATE { get; set; }
        public DateTime U_DOCDUEDATE { get; set; }
        public DateTime U_TAXDATE { get; set; }
        public string U_DOCCUR { get; set; }
        public string U_DOCCURSY { get; set; }
        public double U_DOCTOTAL { get; set; }
        public double U_DOCTOTALSY { get; set; }
        public string U_LICTRADNUM { get; set; }
        public string U_CARDCODE { get; set; }
        public string U_CARDNAME { get; set; }
    }
}
