using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM.Framework;

namespace Soindus.AddOnMonitorEmision.Formularios
{
    [FormAttribute("Soindus.AddOnMonitorEmision.Formularios.frmXMonConf", "Formularios/frmXMonConf.b1f")]
    class frmXMonConf : UserFormBase
    {
        // Objetos de formulario
        #region Objetos de formulario
        private static SAPbouiCOM.Form oForm;
        private static SAPbouiCOM.Folder Folder1;
        private static SAPbouiCOM.Folder Folder2;
        private static SAPbouiCOM.EditText txtToken;
        private static SAPbouiCOM.Button btnOK;

        private Local.Configuracion ExtConf = new Local.Configuracion();
        #endregion

        public frmXMonConf()
        {
            AsignarObjetos();
            CargarFormulario();
        }

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.OnCustomInitialize();
        }

        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {
        }

        public static void Form_ItemEvent(string FormUID, ref SAPbouiCOM.ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            if (pVal.BeforeAction.Equals(true))
            {

            }
            else
            {
                if (pVal.EventType.Equals(SAPbouiCOM.BoEventTypes.et_CLICK))
                {
                    // Boton grabar
                    #region grabar
                    if (pVal.ItemUID.Equals("btnOK"))
                    {
                        GuardarCambios();
                    }
                    #endregion
                }
            }
        }

        private void OnCustomInitialize()
        {

        }

        private void AsignarObjetos()
        {
            oForm = ((SAPbouiCOM.Form)(Application.SBO_Application.Forms.Item("frmXMonConf")));
            Folder1 = ((SAPbouiCOM.Folder)(GetItem("tab01").Specific));
            Folder2 = ((SAPbouiCOM.Folder)(GetItem("tab02").Specific));
            Folder1.Select();
            txtToken = ((SAPbouiCOM.EditText)(GetItem("txtToken").Specific));
            btnOK = ((SAPbouiCOM.Button)(GetItem("btnOK").Specific));
        }

        private void CargarFormulario()
        {
            // Blindear objetos de formulario
            oForm.DataSources.UserDataSources.Add("Token", SAPbouiCOM.BoDataType.dt_SHORT_TEXT, 254);

            txtToken.DataBind.SetBound(true, "", "Token");

            oForm.Visible = true;

            CargarConfiguracion();
        }

        private void CargarConfiguracion()
        {
            oForm.DataSources.UserDataSources.Item("Token").Value = ExtConf.Parametros.TOKEN;
        }

        private static void GuardarCambios()
        {
            SAPbobsCOM.GeneralService oGeneralService = null;
            SAPbobsCOM.GeneralData oGeneralData = null;
            SAPbobsCOM.GeneralData oChild = null;
            SAPbobsCOM.GeneralDataCollection oChildren = null;
            SAPbobsCOM.GeneralDataParams oGeneralParams = null;
            SAPbobsCOM.CompanyService oCompanyService = null;

            try
            {
                oCompanyService = SBO.ConexionSBO.oCompany.GetCompanyService();
                // Get GeneralService (oCmpSrv is the CompanyService)
                oGeneralService = oCompanyService.GetGeneralService("SO_XMONCONF");
                // Create data for new row in main UDO
                oGeneralParams = ((SAPbobsCOM.GeneralDataParams)(oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)));
                oGeneralData = ((SAPbobsCOM.GeneralData)(oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralData)));
                bool existeconf = SBO.ConsultasSBO.ExisteConfiguracion();
                if (existeconf)
                {
                    oGeneralParams.SetProperty("Code", "CONF");
                    oGeneralData = oGeneralService.GetByParams(oGeneralParams);
                    oGeneralData.SetProperty("Code", "CONF");
                    oGeneralData.SetProperty("Name", "Configuración Monitor de Emisión");
                    oGeneralData.SetProperty("U_TOKEN", oForm.DataSources.UserDataSources.Item("Token").Value);
                    oGeneralService.Update(oGeneralData);
                }
                else
                {
                    oGeneralData.SetProperty("Code", "CONF");
                    oGeneralData.SetProperty("Name", "Configuración Monitor de Emisión");
                    oGeneralData.SetProperty("U_TOKEN", oForm.DataSources.UserDataSources.Item("Token").Value);
                    oGeneralService.Add(oGeneralData);
                }
                Application.SBO_Application.StatusBar.SetText(String.Format("Configuración guardada correctamente."), SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success);
            }
            catch (Exception ex)
            {
                Application.SBO_Application.StatusBar.SetText(String.Format("Error al guardar la nueva configuración: {0}", ex.Message), SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
            }
        }
    }
}
