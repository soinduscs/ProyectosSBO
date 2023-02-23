using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soindus.AddOnMonitorProveedores.Azurian
{
    public class request
    {
        public string fechaDesde { get; set; }
        public string fechaHasta { get; set; }
        public string nroResolucion { get; set; }
        public string rutEmpresa { get; set; }
        public string wsApiKey { get; set; }
        //dergargarArchivo
        public string folio { get; set; }
        public string idTipoDocumento { get; set; }
        public string rutEmisor { get; set; }
        public string tipoArchivo { get; set; }
        public string tipoEntrega { get; set; }

    }
}
