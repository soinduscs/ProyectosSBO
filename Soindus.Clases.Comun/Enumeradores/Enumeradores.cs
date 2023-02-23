using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soindus.Clases.Comun
{
    public class Enumeradores
    {
        public enum ProcesosSvcDTE
        {
            Conexion = 0,
            Inicio = 1,
            Proceso = 2,
            Pausa = 3,
            Re_Inicio = 4,
            Stop = 5,
            Error = -1
        }

        public enum EstadosComercialesComunes
        {
            Acuse_Recibo = 0, //Acuse de recibo para indicar que una factura ya fue recibida
            Aceptacion_Comercial = 1, //Aceptación Comercial del documento
            Rechazo_Comercial = 2, //Rechazo comercial del documento por alguna discrepancia en la formalidad del documento
            Aceptacion_Recibo_Mercaderias = 3, //Recibo de Mercaderías o Servicios conforme
            Reclamo_Parcial = 4, //Reclamo parcial del documento por inconformidad de la entrega del servicio o mercadería
            Reclamo_Total = 5  //Reclamo total del documento por inconformidad de la entrega del servicio o mercadería
        }

        public enum EstadosComerciales
        {
            sinaccion           = 0,
            aceptado            = 1,
            aceptadoensii       = 2,
            prerechazado        = 3,
            rechazadoensii      = 4,
            reclamoparcialensii = 5,
            reclamototalensii   = 6,
            recibomercaderias   = 7
        }

        public static string GetEstadoComercial(EstadosComerciales estado)
        {
            string[] EstadosComercialesPalabras = new string[]
            {
            "Sin acción",
            "Aceptado",
            "Aceptado en el SII",
            "Pre-rechazado",
            "Rechazado en el SII",
            "Reclamo parcial en el SII",
            "Reclamo total en el SII",
            "Recibo de mercaderías en el SII"
            };
            return EstadosComercialesPalabras[(int)estado];
        }

        public static int GetEstadoComercial(string estado)
        {
            string[] EstadosComercialesPalabras = new string[]
            {
            "Sin acción",
            "Aceptado",
            "Aceptado en el SII",
            "Pre-rechazado",
            "Rechazado en el SII",
            "Reclamo parcial en el SII",
            "Reclamo total en el SII",
            "Recibo de mercaderías en el SII"
            };
            return Array.IndexOf(EstadosComercialesPalabras, estado);
        }

        public enum EstadosSii
        {
            sinestado           = 0,
            pendienteenvio      = 1,
            enviado             = 2,
            errorenvio          = 3,
            aceptado            = 4,
            aceptadoreparo      = 5,
            rechazado           = 6,
            pendienteconsulta   = 7,
            errorconsulta       = 8,
            ensobrado           = 9
        }

        public static string GetEstadoSii(EstadosSii estado)
        {
            string[] EstadosSiiPalabras = new string[]
            {
            "Sin Estado",
            "Pendiente de envío al SII",
            "Enviado al SII",
            "Error al enviar",
            "Aceptado por el SII",
            "Aceptado con reparos por el SII",
            "Rechazado por el SII",
            "Pendiente de consulta en el SII",
            "Error al consultar en el SII",
            "Ensobrado"
            };
            return EstadosSiiPalabras[(int)estado];
        }

        public static int GetEstadoSii(string estado)
        {
            string[] EstadosSiiPalabras = new string[]
           {
            "Sin Estado",
            "Pendiente de envío al SII",
            "Enviado al SII",
            "Error al enviar",
            "Aceptado por el SII",
            "Aceptado con reparos por el SII",
            "Rechazado por el SII",
            "Pendiente de consulta en el SII",
            "Error al consultar en el SII",
            "Ensobrado"
           };
            return Array.IndexOf(EstadosSiiPalabras, estado);
        }

        public enum EstadosEmision
        {
            SES = 0,    //Sin Estado
            ING = 1,    //DTE Ingresado interno
            ERR = 2,    //DTE con errores internos
            FIR = 3,    //DTE firmado
            DOK = 4,    //DTE OK
            RPR = 5,    //Aprobado con Reparos por el SII
            RLV = 6,    //DTE Aceptado con Reparos Leves
            RCH = 7,    //Rechazado por el SII
            SCM = 8,    //DTE con error de Schema
            INI = 9,    //DTE Inicializado - Inicializado
            ACC = 10,   //Aprobado Contablemente por el Contribuyente
            ACD = 11,   //Aprobado por Contribuyente con Discrepancias
            RDC = 12,   //Rechazado por Otro Contribuyente
            EPR = 13    //Envío procesado
        }

        public static string GetEstadoEmision (EstadosEmision estado)
        {
            string[] EstadosEmisionPalabras = new string[]
            {
            "Sin Estado",
            "DTE Ingresado interno",
            "DTE con errores internos",
            "DTE firmado",
            "DTE OK",
            "Aprobado con Reparos por el SII",
            "DTE Aceptado con Reparos Leves",
            "Rechazado por el SII",
            "DTE con error de Schema",
            "Inicializado",
            "Aprobado Contablemente por el Contribuyente",
            "Aprobado por Contribuyente con Discrepancias",
            "Rechazado por Otro Contribuyente",
            "Envío procesado"
            };
            return EstadosEmisionPalabras[(int)estado];
        }

        public static string GetEstadoEmision(string estado)
        {
            EstadosEmision estadoemision = (EstadosEmision)Enum.Parse(typeof(EstadosEmision), estado);
            return GetEstadoEmision(estadoemision);
        }

    }
}
