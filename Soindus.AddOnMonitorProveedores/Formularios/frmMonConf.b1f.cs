using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM.Framework;
using System.Globalization;

namespace Soindus.AddOnMonitorProveedores.Formularios
{
    [FormAttribute("Soindus.AddOnMonitorProveedores.Formularios.frmMonConf", "Formularios/frmMonConf.b1f")]
    class frmMonConf : UserFormBase
    {
        // Objetos de formulario
        #region Objetos de formulario
        private static SAPbouiCOM.Form oForm;
        private static SAPbouiCOM.Folder Folder1;
        private static SAPbouiCOM.Folder Folder2;
        private static SAPbouiCOM.Folder Folder3;
        private static SAPbouiCOM.Folder Folder4;
        private static SAPbouiCOM.ComboBox cbxProvFE;
        private static SAPbouiCOM.EditText txtToken;
        private static SAPbouiCOM.EditText txtRutRece;
        private static SAPbouiCOM.CheckBox chkResp;
        private static SAPbouiCOM.CheckBox chkRespD;
        private static SAPbouiCOM.CheckBox chkCesion;
        private static SAPbouiCOM.CheckBox chkOC;
        private static SAPbouiCOM.CheckBox chkEM;
        private static SAPbouiCOM.CheckBox chkMto;
        private static SAPbouiCOM.EditText txtMto;
        private static SAPbouiCOM.CheckBox ckhOCMan;
        private static SAPbouiCOM.CheckBox chkEncDTE;
        private static SAPbouiCOM.CheckBox chkMtoOC;
        private static SAPbouiCOM.CheckBox chkToler;
        private static SAPbouiCOM.EditText txtMtoRME;
        private static SAPbouiCOM.EditText txtMtoRMA;
        private static SAPbouiCOM.EditText txtMtoAME;
        private static SAPbouiCOM.EditText txtMtoAMA;
        private static SAPbouiCOM.CheckBox chkMB;
        private static SAPbouiCOM.Button btnSave;
        private static SAPbouiCOM.Button btnClose;


        private Local.Configuracion ExtConf;
        private static string SepDecimal;
        private static string SepMiles;
        #endregion

        public frmMonConf()
        {
            ExtConf = new Local.Configuracion();
            SepDecimal = SBO.ConsultasSBO.ObtenerSeparadorDecimal();
            SepMiles = SBO.ConsultasSBO.ObtenerSeparadorMiles();
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

        /// <summary>
        /// Eventos SB1
        /// </summary>
        /// <param name="FormUID"></param>
        /// <param name="pVal"></param>
        /// <param name="BubbleEvent"></param>
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
                    if (pVal.ItemUID.Equals("btnSave"))
                    {
                        GuardarCambios();
                    }
                    #endregion

                    // Checks
                    #region check
                    if (pVal.ItemUID.Equals("chkOC"))
                    {
                        if (chkOC.Checked)
                        {
                            oForm.DataSources.UserDataSources.Item("EM").Value = "N";
                        }
                    }

                    if (pVal.ItemUID.Equals("chkEM"))
                    {
                        if (!chkEM.Checked)
                        {
                            oForm.DataSources.UserDataSources.Item("OC").Value = "Y";
                        }
                    }

                    if (pVal.ItemUID.Equals("chkMtoOC"))
                    {
                        if (chkMtoOC.Checked)
                        {
                            oForm.DataSources.UserDataSources.Item("Toler").Value = "N";
                        }
                    }

                    if (pVal.ItemUID.Equals("chkToler"))
                    {
                        if (!chkToler.Checked)
                        {
                            oForm.DataSources.UserDataSources.Item("MtoOC").Value = "Y";
                        }
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
            oForm = ((SAPbouiCOM.Form)(Application.SBO_Application.Forms.Item("frmMonConf")));
            Folder1 = ((SAPbouiCOM.Folder)(GetItem("tab01").Specific));
            Folder2 = ((SAPbouiCOM.Folder)(GetItem("tab02").Specific));
            Folder3 = ((SAPbouiCOM.Folder)(GetItem("tab03").Specific));
            Folder4 = ((SAPbouiCOM.Folder)(GetItem("tab04").Specific));
            Folder1.Select();
            cbxProvFE = ((SAPbouiCOM.ComboBox)(GetItem("cbxProvFE").Specific));
            txtToken = ((SAPbouiCOM.EditText)(GetItem("txtToken").Specific));
            txtRutRece = ((SAPbouiCOM.EditText)(GetItem("txtRutRece").Specific));
            chkResp = ((SAPbouiCOM.CheckBox)(GetItem("chkResp").Specific));
            chkRespD = ((SAPbouiCOM.CheckBox)(GetItem("chkRespD").Specific));
            chkCesion = ((SAPbouiCOM.CheckBox)(GetItem("chkCesion").Specific));
            chkOC = ((SAPbouiCOM.CheckBox)(GetItem("chkOC").Specific));
            chkEM = ((SAPbouiCOM.CheckBox)(GetItem("chkEM").Specific));
            chkMto = ((SAPbouiCOM.CheckBox)(GetItem("chkMto").Specific));
            txtMto = ((SAPbouiCOM.EditText)(GetItem("txtMto").Specific));
            ckhOCMan = ((SAPbouiCOM.CheckBox)(GetItem("chkOCMan").Specific));
            chkEncDTE = ((SAPbouiCOM.CheckBox)(GetItem("chkEncDTE").Specific));
            chkMtoOC = ((SAPbouiCOM.CheckBox)(GetItem("chkMtoOC").Specific));
            chkToler = ((SAPbouiCOM.CheckBox)(GetItem("chkToler").Specific));
            txtMtoRME = ((SAPbouiCOM.EditText)(GetItem("txtMtoRME").Specific));
            txtMtoRMA = ((SAPbouiCOM.EditText)(GetItem("txtMtoRMA").Specific));
            txtMtoAME = ((SAPbouiCOM.EditText)(GetItem("txtMtoAME").Specific));
            txtMtoAMA = ((SAPbouiCOM.EditText)(GetItem("txtMtoAMA").Specific));
            chkMB = ((SAPbouiCOM.CheckBox)(GetItem("chkMB").Specific));
            btnSave = ((SAPbouiCOM.Button)(GetItem("btnSave").Specific));
            btnClose = ((SAPbouiCOM.Button)(GetItem("2").Specific));
        }

        private void CargarFormulario()
        {
            // Blindear objetos de formulario
            oForm.DataSources.UserDataSources.Add("ProvFE", SAPbouiCOM.BoDataType.dt_LONG_TEXT, 100);
            oForm.DataSources.UserDataSources.Add("Token", SAPbouiCOM.BoDataType.dt_LONG_TEXT, 254);
            oForm.DataSources.UserDataSources.Add("RRecep", SAPbouiCOM.BoDataType.dt_LONG_TEXT, 20);
            oForm.DataSources.UserDataSources.Add("Resp", SAPbouiCOM.BoDataType.dt_SHORT_TEXT, 1);
            oForm.DataSources.UserDataSources.Add("RespD", SAPbouiCOM.BoDataType.dt_SHORT_TEXT, 1);
            oForm.DataSources.UserDataSources.Add("Cesion", SAPbouiCOM.BoDataType.dt_SHORT_TEXT, 1);
            oForm.DataSources.UserDataSources.Add("OC", SAPbouiCOM.BoDataType.dt_SHORT_TEXT, 1);
            oForm.DataSources.UserDataSources.Add("EM", SAPbouiCOM.BoDataType.dt_SHORT_TEXT, 1);
            oForm.DataSources.UserDataSources.Add("Mto", SAPbouiCOM.BoDataType.dt_SHORT_TEXT, 1);
            oForm.DataSources.UserDataSources.Add("MtoV", SAPbouiCOM.BoDataType.dt_PRICE, 30);
            oForm.DataSources.UserDataSources.Add("OCMan", SAPbouiCOM.BoDataType.dt_SHORT_TEXT, 1);
            oForm.DataSources.UserDataSources.Add("EncDTE", SAPbouiCOM.BoDataType.dt_SHORT_TEXT, 1);
            oForm.DataSources.UserDataSources.Add("MtoOC", SAPbouiCOM.BoDataType.dt_SHORT_TEXT, 1);
            oForm.DataSources.UserDataSources.Add("Toler", SAPbouiCOM.BoDataType.dt_SHORT_TEXT, 1);
            oForm.DataSources.UserDataSources.Add("MtoRME", SAPbouiCOM.BoDataType.dt_PRICE, 30);
            oForm.DataSources.UserDataSources.Add("MtoRMA", SAPbouiCOM.BoDataType.dt_PRICE, 30);
            oForm.DataSources.UserDataSources.Add("MtoAME", SAPbouiCOM.BoDataType.dt_PRICE, 30);
            oForm.DataSources.UserDataSources.Add("MtoAMA", SAPbouiCOM.BoDataType.dt_PRICE, 30);
            oForm.DataSources.UserDataSources.Add("MB", SAPbouiCOM.BoDataType.dt_SHORT_TEXT, 1);
            cbxProvFE.DataBind.SetBound(true, "", "ProvFE");
            txtToken.DataBind.SetBound(true, "", "Token");
            txtRutRece.DataBind.SetBound(true, "", "RRecep");
            chkResp.DataBind.SetBound(true, "", "Resp");
            chkRespD.DataBind.SetBound(true, "", "RespD");
            chkCesion.DataBind.SetBound(true, "", "Cesion");
            chkOC.DataBind.SetBound(true, "", "OC");
            chkEM.DataBind.SetBound(true, "", "EM");
            chkMto.DataBind.SetBound(true, "", "Mto");
            txtMto.DataBind.SetBound(true, "", "MtoV");
            ckhOCMan.DataBind.SetBound(true, "", "OCMan");
            chkEncDTE.DataBind.SetBound(true, "", "EncDTE");
            chkMtoOC.DataBind.SetBound(true, "", "MtoOC");
            chkToler.DataBind.SetBound(true, "", "Toler");
            txtMtoRME.DataBind.SetBound(true, "", "MtoRME");
            txtMtoRMA.DataBind.SetBound(true, "", "MtoRMA");
            txtMtoAME.DataBind.SetBound(true, "", "MtoAME");
            txtMtoAMA.DataBind.SetBound(true, "", "MtoAMA");
            chkMB.DataBind.SetBound(true, "", "MB");

            cbxProvFE.ValidValues.Add("SO", "SoindusFE");
            cbxProvFE.ValidValues.Add("FEB", "Febos");
            cbxProvFE.ValidValues.Add("DBN", "DbNet");
            cbxProvFE.ValidValues.Add("FAC", "Facele");
            cbxProvFE.ValidValues.Add("AZU", "Azurian");
            cbxProvFE.ValidValues.Add("SID", "Sidge");

            oForm.Visible = true;

            CargarConfiguracion();
        }

        private void CargarConfiguracion()
        {
            oForm.DataSources.UserDataSources.Item("ProvFE").Value = ExtConf.Parametros.Proveedor_FE.ToString();
            oForm.DataSources.UserDataSources.Item("Token").Value = ExtConf.Parametros.Token.ToString();
            oForm.DataSources.UserDataSources.Item("RRecep").Value = ExtConf.Parametros.Rut_Receptor.ToString();
            oForm.DataSources.UserDataSources.Item("Resp").Value = ExtConf.Parametros.Visualiza_Responsable == true ? "Y" : "N";
            oForm.DataSources.UserDataSources.Item("RespD").Value = ExtConf.Parametros.Visualiza_Responsable_Doc == true ? "Y" : "N";
            oForm.DataSources.UserDataSources.Item("Cesion").Value = ExtConf.Parametros.Visualiza_Cesion == true ? "Y" : "N";
            oForm.DataSources.UserDataSources.Item("OC").Value = ExtConf.Parametros.Valida_ExisteOC == true ? "Y" : "N";
            oForm.DataSources.UserDataSources.Item("EM").Value = ExtConf.Parametros.Valida_ExisteEntrada == true ? "Y" : "N";
            oForm.DataSources.UserDataSources.Item("Mto").Value = ExtConf.Parametros.Valida_MontoMaximo == true ? "Y" : "N";
            oForm.DataSources.UserDataSources.Item("MtoV").Value = ExtConf.Parametros.Valida_ValorMontoMaximo.ToString();
            oForm.DataSources.UserDataSources.Item("OCMan").Value = ExtConf.Parametros.Valida_PermiteOCManual == true ? "Y" : "N";
            oForm.DataSources.UserDataSources.Item("EncDTE").Value = ExtConf.Parametros.Valida_EncabezadosDTE == true ? "Y" : "N";
            oForm.DataSources.UserDataSources.Item("MtoOC").Value = ExtConf.Parametros.Valida_MontoOC == true ? "Y" : "N";
            oForm.DataSources.UserDataSources.Item("Toler").Value = ExtConf.Parametros.Valida_PermiteTolerancias == true ? "Y" : "N";
            oForm.DataSources.UserDataSources.Item("MtoRME").Value = ExtConf.Parametros.Valida_ValorRechazoMontoMenor.ToString();
            oForm.DataSources.UserDataSources.Item("MtoRMA").Value = ExtConf.Parametros.Valida_ValorRechazoMontoMayor.ToString();
            oForm.DataSources.UserDataSources.Item("MtoAME").Value = ExtConf.Parametros.Valida_ValorAprobacionMontoMenor.ToString();
            oForm.DataSources.UserDataSources.Item("MtoAMA").Value = ExtConf.Parametros.Valida_ValorAprobacionMontoMayor.ToString();
            oForm.DataSources.UserDataSources.Item("MB").Value = ExtConf.Parametros.Integracion_SolicitaMultiBranch == true ? "Y" : "N";
        }

        private static void GuardarCambios()
        {
            SAPbobsCOM.GeneralService oGeneralService = null;
            SAPbobsCOM.GeneralData oGeneralData = null;
            SAPbobsCOM.GeneralData oChild = null;
            SAPbobsCOM.GeneralDataCollection oChildren = null;
            SAPbobsCOM.GeneralDataParams oGeneralParams = null;
            SAPbobsCOM.CompanyService oCompanyService = null;

            NumberFormatInfo provider = new NumberFormatInfo();
            provider.NumberDecimalSeparator = SepDecimal;
            provider.NumberGroupSeparator = SepMiles;

            try
            {
                oCompanyService = SBO.ConexionSBO.oCompany.GetCompanyService();
                // Get GeneralService (oCmpSrv is the CompanyService)
                oGeneralService = oCompanyService.GetGeneralService("SO_MONCONF");
                // Create data for new row in main UDO
                oGeneralParams = ((SAPbobsCOM.GeneralDataParams)(oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)));
                oGeneralData = ((SAPbobsCOM.GeneralData)(oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralData)));
                bool existeconf = SBO.ConsultasSBO.ExisteConfiguracion();
                if (existeconf)
                {
                    oGeneralParams.SetProperty("Code", "CONF");
                    oGeneralData = oGeneralService.GetByParams(oGeneralParams);
                    oGeneralData.SetProperty("Code", "CONF");
                    oGeneralData.SetProperty("Name", "Configuración Monitor Proveedores");
                    oGeneralData.SetProperty("U_PROVFE", oForm.DataSources.UserDataSources.Item("ProvFE").Value);
                    oGeneralData.SetProperty("U_TOKEN", oForm.DataSources.UserDataSources.Item("Token").Value);
                    oGeneralData.SetProperty("U_RRECEP", oForm.DataSources.UserDataSources.Item("RRecep").Value);
                    oGeneralData.SetProperty("U_G_RESP", oForm.DataSources.UserDataSources.Item("Resp").Value);
                    oGeneralData.SetProperty("U_G_RESPD", oForm.DataSources.UserDataSources.Item("RespD").Value);
                    oGeneralData.SetProperty("U_G_CESION", oForm.DataSources.UserDataSources.Item("Cesion").Value);
                    oGeneralData.SetProperty("U_V_OC", oForm.DataSources.UserDataSources.Item("OC").Value);
                    oGeneralData.SetProperty("U_V_ENT", oForm.DataSources.UserDataSources.Item("EM").Value);
                    oGeneralData.SetProperty("U_V_MTO", oForm.DataSources.UserDataSources.Item("Mto").Value);
                    string smonto = oForm.DataSources.UserDataSources.Item("MtoV").Value;
                    double dmonto = Convert.ToDouble(smonto, provider);
                    oGeneralData.SetProperty("U_V_MTOVAL", dmonto.ToString("G",new System.Globalization.CultureInfo("en-US")));
                    oGeneralData.SetProperty("U_V_OCMAN", oForm.DataSources.UserDataSources.Item("OCMan").Value);
                    oGeneralData.SetProperty("U_V_ENCDTE", oForm.DataSources.UserDataSources.Item("EncDTE").Value);
                    oGeneralData.SetProperty("U_V_MTOOC", oForm.DataSources.UserDataSources.Item("MtoOC").Value);
                    oGeneralData.SetProperty("U_V_TOLER", oForm.DataSources.UserDataSources.Item("Toler").Value);
                    smonto = oForm.DataSources.UserDataSources.Item("MtoRME").Value;
                    dmonto = Convert.ToDouble(smonto, provider);
                    oGeneralData.SetProperty("U_V_RMEVAL", dmonto.ToString("G", new System.Globalization.CultureInfo("en-US")));
                    smonto = oForm.DataSources.UserDataSources.Item("MtoRMA").Value;
                    dmonto = Convert.ToDouble(smonto, provider);
                    oGeneralData.SetProperty("U_V_RMAVAL", dmonto.ToString("G", new System.Globalization.CultureInfo("en-US")));
                    smonto = oForm.DataSources.UserDataSources.Item("MtoAME").Value;
                    dmonto = Convert.ToDouble(smonto, provider);
                    oGeneralData.SetProperty("U_V_AMEVAL", dmonto.ToString("G", new System.Globalization.CultureInfo("en-US")));
                    smonto = oForm.DataSources.UserDataSources.Item("MtoAMA").Value;
                    dmonto = Convert.ToDouble(smonto, provider);
                    oGeneralData.SetProperty("U_V_AMAVAL", dmonto.ToString("G", new System.Globalization.CultureInfo("en-US")));
                    oGeneralData.SetProperty("U_I_MB", oForm.DataSources.UserDataSources.Item("MB").Value);
                    oGeneralService.Update(oGeneralData);
                }
                else
                {
                    oGeneralData.SetProperty("Code", "CONF");
                    oGeneralData.SetProperty("Name", "Configuración Monitor Proveedores");
                    oGeneralData.SetProperty("U_PROVFE", oForm.DataSources.UserDataSources.Item("ProvFE").Value);
                    oGeneralData.SetProperty("U_TOKEN", oForm.DataSources.UserDataSources.Item("Token").Value);
                    oGeneralData.SetProperty("U_RRECEP", oForm.DataSources.UserDataSources.Item("RRecep").Value);
                    oGeneralData.SetProperty("U_G_RESP", oForm.DataSources.UserDataSources.Item("Resp").Value);
                    oGeneralData.SetProperty("U_G_RESPD", oForm.DataSources.UserDataSources.Item("RespD").Value);
                    oGeneralData.SetProperty("U_G_CESION", oForm.DataSources.UserDataSources.Item("Cesion").Value);
                    oGeneralData.SetProperty("U_V_OC", oForm.DataSources.UserDataSources.Item("OC").Value);
                    oGeneralData.SetProperty("U_V_ENT", oForm.DataSources.UserDataSources.Item("EM").Value);
                    oGeneralData.SetProperty("U_V_MTO", oForm.DataSources.UserDataSources.Item("Mto").Value);
                    string smonto = oForm.DataSources.UserDataSources.Item("MtoV").Value;
                    double dmonto = Convert.ToDouble(smonto, provider);
                    oGeneralData.SetProperty("U_V_MTOVAL", dmonto.ToString("G", new System.Globalization.CultureInfo("en-US")));
                    oGeneralData.SetProperty("U_V_OCMAN", oForm.DataSources.UserDataSources.Item("OCMan").Value);
                    oGeneralData.SetProperty("U_V_ENCDTE", oForm.DataSources.UserDataSources.Item("EncDTE").Value);
                    oGeneralData.SetProperty("U_V_MTOOC", oForm.DataSources.UserDataSources.Item("MtoOC").Value);
                    oGeneralData.SetProperty("U_V_TOLER", oForm.DataSources.UserDataSources.Item("Toler").Value);
                    smonto = oForm.DataSources.UserDataSources.Item("MtoRME").Value;
                    dmonto = Convert.ToDouble(smonto, provider);
                    oGeneralData.SetProperty("U_V_RMEVAL", dmonto.ToString("G", new System.Globalization.CultureInfo("en-US")));
                    smonto = oForm.DataSources.UserDataSources.Item("MtoRMA").Value;
                    dmonto = Convert.ToDouble(smonto, provider);
                    oGeneralData.SetProperty("U_V_RMAVAL", dmonto.ToString("G", new System.Globalization.CultureInfo("en-US")));
                    smonto = oForm.DataSources.UserDataSources.Item("MtoAME").Value;
                    dmonto = Convert.ToDouble(smonto, provider);
                    oGeneralData.SetProperty("U_V_AMEVAL", dmonto.ToString("G", new System.Globalization.CultureInfo("en-US")));
                    smonto = oForm.DataSources.UserDataSources.Item("MtoAMA").Value;
                    dmonto = Convert.ToDouble(smonto, provider);
                    oGeneralData.SetProperty("U_V_AMAVAL", dmonto.ToString("G", new System.Globalization.CultureInfo("en-US")));
                    oGeneralData.SetProperty("U_I_MB", oForm.DataSources.UserDataSources.Item("MB").Value);
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
