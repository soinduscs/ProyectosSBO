using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soindus.AddOnMonitorProveedores.SoindusFE
{
    public class XML_ListaResumenDocumento
    {
        public get_ListaResumenDocumento Generar(string FechaIni = "", string FechaFin = "")
        {
            if (string.IsNullOrEmpty(FechaIni))
            {
                FechaIni = DateTime.Today.ToString("yyyy-MM-dd");
            }
            else
            {
                FechaIni = string.Format("{0}-{1}-{2}", FechaIni.Substring(0, 4), FechaIni.Substring(4, 2), FechaIni.Substring(6, 2));
            }
            if (string.IsNullOrEmpty(FechaFin))
            {
                FechaFin = DateTime.Today.ToString("yyyy-MM-dd");
            }
            else
            {
                FechaFin = string.Format("{0}-{1}-{2}", FechaFin.Substring(0, 4), FechaFin.Substring(4, 2), FechaFin.Substring(6, 2));
            }
            get_ListaResumenDocumento listaresumendocumento = new SoindusFE.get_ListaResumenDocumento();
            listaresumendocumento.objParamDocProv.Desde = FechaIni;
            listaresumendocumento.objParamDocProv.Hasta = FechaFin;
            return listaresumendocumento;
        }
    }

    public class get_ListaResumenDocumento
    {
        public string intCodigo { get; set; }
        public string strClave { get; set; }
        public objParamDocProv objParamDocProv { get; set; }

        public get_ListaResumenDocumento()
        {
            objParamDocProv = new SoindusFE.objParamDocProv();
        }
    }
}
