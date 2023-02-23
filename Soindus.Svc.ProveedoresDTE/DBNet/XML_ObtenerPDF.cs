using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soindus.Svc.ProveedoresDTE.DBNet
{
    public class XML_ObtenerPDF
    {
        public get_ObtenerPDF Generar(string RutEmpresa = "", string RutEmisor = "", string Tipo = "", string Folio = "")
        {
            get_ObtenerPDF obtenerpdf = new DBNet.get_ObtenerPDF();
            obtenerpdf.sRuttRece = RutEmpresa;
            obtenerpdf.sRuttEmis = RutEmisor;
            obtenerpdf.sTipoDocu = Tipo;
            obtenerpdf.sFoliDocu = Folio;
            return obtenerpdf;
        }
    }

    public class get_ObtenerPDF
    {
        public string sRuttRece { get; set; }
        public string sRuttEmis { get; set; }
        public string sTipoDocu { get; set; }
        public string sFoliDocu { get; set; }
    }
}
