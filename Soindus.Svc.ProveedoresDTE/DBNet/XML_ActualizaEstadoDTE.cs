using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soindus.Svc.ProveedoresDTE.DBNet
{
    public class XML_ActualizaEstadoDTE
    {
        public get_ActualizaEstadoDTE Generar(string RutEmpresa = "", string RutEmisor = "", string Tipo = "", string Folio = "", string Estado = "")
        {
            get_ActualizaEstadoDTE actualizaestadodte = new DBNet.get_ActualizaEstadoDTE();
            actualizaestadodte.RutEmpresa = RutEmpresa;
            actualizaestadodte.RutEmisor = RutEmisor;
            actualizaestadodte.Tipo = Tipo;
            actualizaestadodte.Folio = Folio;
            actualizaestadodte.Estado = Estado;
            return actualizaestadodte;
        }
    }

    public class get_ActualizaEstadoDTE
    {
        public string RutEmpresa { get; set; }
        public string RutEmisor { get; set; }
        public string Tipo { get; set; }
        public string Folio { get; set; }
        public string Estado { get; set; }
    }
}
