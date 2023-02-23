using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soindus.Svc.ProveedoresDTE.DBNet
{
    public class XML_RescataXMLDTE
    {
        public get_RescataXMLDTE Generar(string RutEmpresa = "", string RutEmisor = "", string Tipo = "", string Folio = "")
        {
            get_RescataXMLDTE rescataxmldte = new DBNet.get_RescataXMLDTE();
            rescataxmldte.RutEmpresa = RutEmpresa;
            rescataxmldte.RutEmisor = RutEmisor;
            rescataxmldte.Tipo = Tipo;
            rescataxmldte.Folio = Folio;
            return rescataxmldte;
        }
    }

    public class get_RescataXMLDTE
    {
        public string RutEmpresa { get; set; }
        public string RutEmisor { get; set; }
        public string Tipo { get; set; }
        public string Folio { get; set; }
    }
}
