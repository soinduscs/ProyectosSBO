using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soindus.AddOnMonitorProveedores.Local
{
    public class FuncionesComunes
    {
        /// <summary>
        /// Metodo para liberar objetos COM de memoria
        /// </summary>
        /// <param name="objeto">Objeto que se desea liberar de memoria</param> n 
        public static void LiberarObjetoGenerico(Object objeto)
        {
            try
            {
                if (objeto != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(objeto);//Desacoplamos el objeto de la aplicacion
                    GC.Collect();
                }
            }
            catch (Exception ex)
            {
                Mensajes.Errores(14, "FucionesComunes.cs -> LiberarObjetoGenerico: " + ex.Message);//Mensaje de errors
            }
        }
    }
}
