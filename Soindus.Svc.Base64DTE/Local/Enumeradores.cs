using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soindus.Svc.Base64DTE.Local
{
    public class Enumeradores
    {
        public enum ProcesosSvcBase64DTE
        {
            Conexion = 0,
            Inicio = 1,
            Proceso = 2,
            Pausa = 3,
            Re_Inicio = 4,
            Stop = 5,
            Error = 9
        }
    }
}
