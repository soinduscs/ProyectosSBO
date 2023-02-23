using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soindus.Svc.ProveedoresDTE.DBNet
{
    public class XML_RescataListadoDTE
    {
        public get_RescataListadoDTE Generar(string RutEmpresa = "")
        {
            get_RescataListadoDTE rescatalistadodte = new DBNet.get_RescataListadoDTE();
            rescatalistadodte.RutEmpresa = RutEmpresa;
            return rescatalistadodte;
        }
    }

    public class get_RescataListadoDTE
    {
        public string RutEmpresa { get; set; }
    }
}
