using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soindus.Svc.ProveedoresDTE.Local
{
    public class Mensajes
    {
        public static Message Result = new Message();

        #region Metodos
        /// <summary>
        /// Metodo que retorna el mensaje de error para un codigo determinado.
        /// </summary>
        /// <param name="errorCode">Codigo del error</param>
        /// <param name="errMsj">Mensaje del error(Complementario)</param>
        public static Local.Message Errores(int errorCode, string errMsj)
        {
            string Msj = "";
            try
            {
                switch (errorCode)
                {
                    case -2:
                        Msj = errMsj;
                        Result = MostrarMsjWF(errorCode, Msj, false);
                        break;
                    case -1:
                        Msj = errMsj;
                        Result = MostrarMsjWF(errorCode, Msj, false);
                        break;
                    case 1:
                        Msj = "Error. Folio no asignado.";
                        Result = MostrarMsjWF(errorCode, Msj, false);
                        break;
                    case 2:
                        Msj = "Error. Indicador no asignado.";
                        Result = MostrarMsjWF(errorCode, Msj, false);
                        break;
                    case 3:
                        Msj = "Error. DocEntry no existe.";
                        Result = MostrarMsjWF(errorCode, Msj, false);
                        break;
                    case 8:
                        Msj = "Error. No se pudo crear tabla de usuario.";
                        Result = MostrarMsjWF(errorCode, Msj, false);
                        break;
                    case 14:
                        Msj = "Error originado en: " + errMsj;
                        Result = MostrarMsjWF(errorCode, Msj, false);
                        break;
                    default:
                        Msj = errMsj;
                        Result = MostrarMsjWF(errorCode, Msj, false);
                        break;
                }
                return Result;
            }
            catch (Exception ex)
            {
                Msj = ex.Message;
                Result = MostrarMsjWF(1000, Msj, false);
                return Result;
            }
        }

        /// <summary>
        /// Metodo que retorna un mensaje de Exito para un codigo especifico
        /// </summary>
        /// <param name="SuccesCode">Codigo del Mensaje</param>
        /// <param name="Msj">Mensaje del Exito (Complementario)</param>
        public static Local.Message Exitos(int SuccesCode, string Msj)
        {
            try
            {
                switch (SuccesCode)
                {
                    case 1:
                        Msj = "Conexión realizada con exito";
                        Result = MostrarMsjWF(SuccesCode, Msj, true);
                        break;
                    case 2:
                        Msj = "Parametros correctos";
                        Result = MostrarMsjWF(SuccesCode, Msj, true);
                        break;
                    case 6:
                        Msj = "Documento de texto creado con exito en la ruta: " + Msj;
                        Result = MostrarMsjWF(SuccesCode, Msj, true);
                        break;
                    case 7:
                        Msj = "Documento Enviado con exito";
                        Result = MostrarMsjWF(SuccesCode, Msj, true);
                        break;
                    default:
                        Result = MostrarMsjWF(SuccesCode, Msj, true);
                        break;
                }
                return Result;
            }
            catch (Exception ex)
            {
                Msj = ex.Message;
                Result = MostrarMsjWF(1000, Msj, false);
                return Result;
            }
        }

        /// <summary>
        /// Metodo para mostrar los mensajes en formularios estandar de windows "System.Windows.Forms"
        /// </summary>
        /// <param name="Msj">Mensaje</param>
        private static Local.Message MostrarMsjWF(int id, string Msj, bool tipo)
        {
            Local.Message response = new Local.Message();
            response.Id = id;
            response.Mensaje = Msj;
            response.Success = tipo;

            return response;
            //System.Windows.Forms.MessageBox.Show(Msj);
        }
        #endregion
    }
}
