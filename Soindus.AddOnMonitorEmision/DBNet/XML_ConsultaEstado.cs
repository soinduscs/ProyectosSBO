using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soindus.AddOnMonitorEmision.DBNet
{
    public class XML_ConsultaEstado
    {
        public get_ConsultaEstado Generar(string rut = "", string tipodoc = "", string folio = "")
        {
            get_ConsultaEstado consultaestado = new DBNet.get_ConsultaEstado();
            consultaestado.rut = rut;
            consultaestado.tipo_docu = tipodoc;
            consultaestado.foli_docu = folio;
            return consultaestado;
        }
    }

    public class get_ConsultaEstado
    {
        public string rut { get; set; }
        public string tipo_docu { get; set; }
        public string foli_docu { get; set; }
    }
}
