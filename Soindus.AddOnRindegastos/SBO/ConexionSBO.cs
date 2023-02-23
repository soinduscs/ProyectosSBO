using System;
using SAPbouiCOM.Framework;

namespace Soindus.AddOnRindegastos.SBO
{
    public class ConexionSBO
    {
        //public static SAPbouiCOM.Application m_SBO_Appl = null;
        public static SAPbobsCOM.Company oCompany = null;
        public static Application oApp = null;

        public ConexionSBO(string[] args)
        {
            // Conectar UIAPI
            ConectarInterface(args);
            // Conectar DIAPI
            ConectarCompany();
            // Verificar estructura de datos
            bool verifestruct = SBO.ConsultasSBO.VerificaEstructura();
            if (!verifestruct)
            {
                Application.SBO_Application.StatusBar.SetText("Verificando estructura de la Base de Datos...", SAPbouiCOM.BoMessageTime.bmt_Medium, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);
                EstructuraSBO.VerificarEstructuraMD();
            }
        }

        public static void ConectarInterface(string[] args)
        {
            if (args.Length < 1)
            {
                oApp = new Application();
            }
            else
            {
                oApp = new Application(args[0]);
            }
        }

        /// <summary>
        /// Metodo para conectar a la compañia de la instacia de SBO que se esta ejecutando (DIAPI)
        /// </summary>
        public static void ConectarCompany()
        {
            try
            {
                var m_SBO_Appl = Application.SBO_Application.Company;
                oCompany = (SAPbobsCOM.Company)m_SBO_Appl.GetDICompany();
                //oCompany.XmlExportType = SAPbobsCOM.BoXmlExportTypes.xet_ExportImportMode;
            }
            catch (Exception ex)
            {
                Comun.Mensajes.Errores(5, ex.Message);
            }
        }

        /// <summary>
        /// Metodo para desconeconectar la compañia de la instacia de SBO que se esta ejecutando (DIAPI)
        /// </summary>
        public static void DesconectarCompany()
        {
            try
            {
                oCompany.Disconnect();
            }
            catch (Exception ex)
            {
                Comun.Mensajes.Errores(6, ex.Message);
            }
        }
    }
}
