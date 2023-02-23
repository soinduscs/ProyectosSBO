using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM.Framework;

namespace Soindus.AddOnTipoCambio.Formularios
{
    [FormAttribute("Soindus.AddOnTipoCambio.Formularios.frmTipoCambio", "Formularios/frmTipoCambio.b1f")]
    class frmTipoCambio : UserFormBase
    {
        // Objetos de formulario
        #region Objetos de formulario
        private static SAPbouiCOM.Form oForm;
        private static SAPbouiCOM.Folder Folder1;
        private static SAPbouiCOM.Folder Folder2;
        private static SAPbouiCOM.EditText txtFecIni;
        private static SAPbouiCOM.EditText txtFecTer;
        private static SAPbouiCOM.Button btnOK;

        private static Local.Configuracion ExtConf = new Local.Configuracion();
        #endregion

        public frmTipoCambio()
        {
            AsignarObjetos();
            if (!ExtConf.Parametros.RUTSOC.Equals(ExtConf.Parametros.Rut_Sociedad))
            {
                Application.SBO_Application.MessageBox("La sociedad actual no se encuentra habilitada. Verifique Token...");
                oForm.Close();
                return;
            }
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
                    // Boton cargar
                    #region cargar
                    if (pVal.ItemUID.Equals("btnOK"))
                    {
                        if (!ExtConf.Parametros.RUTSOC.Equals(ExtConf.Parametros.Rut_Sociedad))
                        {
                            Application.SBO_Application.MessageBox("La sociedad actual no se encuentra habilitada. Verifique Token...");
                            oForm.Close();
                            return;
                        }
                        SBO.IntegracionSBO integracionSBO = new SBO.IntegracionSBO();
                        integracionSBO.ObtenerIndicadores(txtFecIni.Value.ToString(), txtFecTer.Value.ToString());
                        Application.SBO_Application.SetStatusBarMessage("Tipos de Cambio - Proceso finalizado correctamente...", SAPbouiCOM.BoMessageTime.bmt_Short, false);
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
            oForm = ((SAPbouiCOM.Form)(Application.SBO_Application.Forms.Item("frmTipoCambio")));
            Folder1 = ((SAPbouiCOM.Folder)(GetItem("tab01").Specific));
            Folder2 = ((SAPbouiCOM.Folder)(GetItem("tab02").Specific));
            Folder1.Select();
            txtFecIni = ((SAPbouiCOM.EditText)(GetItem("txtFecIni").Specific));
            txtFecTer = ((SAPbouiCOM.EditText)(GetItem("txtFecTer").Specific));
            btnOK = ((SAPbouiCOM.Button)(GetItem("btnOK").Specific));
        }

        private void CargarFormulario()
        {
            // Blindear objetos de formulario
            // Fecha desde
            oForm.DataSources.UserDataSources.Add("FecIni", SAPbouiCOM.BoDataType.dt_DATE);
            txtFecIni.DataBind.SetBound(true, "", "FecIni");

            // Fecha termino
            oForm.DataSources.UserDataSources.Add("FecTer", SAPbouiCOM.BoDataType.dt_DATE);
            txtFecTer.DataBind.SetBound(true, "", "FecTer");

            txtFecTer.Value = DateTime.Now.ToString("yyyyMMdd");
            txtFecIni.Value = DateTime.Now.ToString("yyyyMMdd");

            oForm.Visible = true;
        }
    }
}
