using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soindus.AddOnEmisionDTE.Local
{
    public class Respuesta
    {
        public string Folio { get; set; }
        public string Codigo { get; set; }
        public string Mensajes { get; set; }
    }

    public class RespuestaPDF
    {
        public string[] String { get; set; }
    }

    public class RespuestaPDFCloud
    {
        public string[] String { get; set; }
    }

}
