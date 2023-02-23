using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soindus.AddOnMonitorProveedores.Azurian
{
    public class XML_descargarArchivo
    {
        public get_descargarArchivo Generar(string RutEmisor = "", string Tipo = "", string Folio = "")
        {
            get_descargarArchivo descargararchivo = new Azurian.get_descargarArchivo();
            descargararchivo.request.rutEmisor = RutEmisor;
            descargararchivo.request.idTipoDocumento = Tipo;
            descargararchivo.request.folio = Folio;
            return descargararchivo;
        }
    }

    public class get_descargarArchivo
    {
        public request request { get; set; }

        public get_descargarArchivo()
        {
            request = new Azurian.request();
        }
    }
}
