using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soindus.AddOnMonitorProveedores.Azurian
{
    public class XML_transferenciaDocumentos
    {
        public get_transferenciaDocumentos Generar(string FechaIni = "", string FechaFin = "")
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
            get_transferenciaDocumentos transferenciadocumentos = new Azurian.get_transferenciaDocumentos();
            transferenciadocumentos.request.fechaDesde = FechaIni;
            transferenciadocumentos.request.fechaHasta = FechaFin;
            return transferenciadocumentos;
        }
    }

    public class get_transferenciaDocumentos
    {
        public request request { get; set; }

        public get_transferenciaDocumentos()
        {
            request = new Azurian.request();
        }
    }
}
