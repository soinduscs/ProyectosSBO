using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM.Framework;

namespace Soindus.AddOnRindegastos.Formularios
{
    [FormAttribute("Soindus.AddOnRindegastos.Formularios.frmMonConfRG", "Formularios/frmMonConfRG.b1f")]
    class frmMonConfRG : UserFormBase
    {
        // Objetos de formulario
        #region Objetos de formulario
        private static SAPbouiCOM.Form oForm;
        private static SAPbouiCOM.Folder Folder1;
        private static SAPbouiCOM.Folder Folder2;
        private static SAPbouiCOM.Folder Folder3;
        private static SAPbouiCOM.EditText txtTOKEN;
        private static SAPbouiCOM.EditText txtRUTSOC;
        private static SAPbouiCOM.EditText txtLOCALIZ;
        private static SAPbouiCOM.EditText txtG_CCOST;
        private static SAPbouiCOM.ComboBox cbxG_CCOSTV;
        private static SAPbouiCOM.EditText txtG_TIPOD;
        private static SAPbouiCOM.ComboBox cbxG_TIPODV;
        private static SAPbouiCOM.EditText txtG_CODFA;
        private static SAPbouiCOM.EditText txtG_CODFE;
        private static SAPbouiCOM.EditText txtG_CODFM;
        private static SAPbouiCOM.EditText txtG_RUTP;
        private static SAPbouiCOM.ComboBox cbxG_RUTPV;
        private static SAPbouiCOM.EditText txtG_NDOC;
        private static SAPbouiCOM.ComboBox cbxG_NDOCV;
        private static SAPbouiCOM.EditText txtC_CTAANTP;
        private static SAPbouiCOM.EditText txtC_CTASERP;
        private static SAPbouiCOM.EditText txtC_CTACMAT;
        private static SAPbouiCOM.EditText txtC_CTACFAC;
        private static SAPbouiCOM.EditText txtC_CTAPREM;
        private static SAPbouiCOM.Button btnSave;
        private static SAPbouiCOM.Button btnClose;

        private Clases.Configuracion ExtConf = new Clases.Configuracion();
        private static bool EsPlanDeCuentasSegmentado = false;
        private static string FormatoCuentasSegmentado = string.Empty;
        #endregion

        public frmMonConfRG()
        {
            ExtConf = new Clases.Configuracion();
            EsPlanDeCuentasSegmentado = SBO.ConsultasSBO.PlanDeCuentasSegmentado();
            if (EsPlanDeCuentasSegmentado)
            {
                FormatoCuentasSegmentado = SBO.ConsultasSBO.ObtenerFormatoCuentasSegmentado();
            }
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
                }

                if (pVal.EventType == SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST)
                {
                    // Choose from list
                    #region Choosefromlist
                    SAPbouiCOM.IChooseFromListEvent oCFLEvento = ((SAPbouiCOM.IChooseFromListEvent)(pVal));
                    String CflID = oCFLEvento.ChooseFromListUID;
                    SAPbouiCOM.ChooseFromList oCFL = oForm.ChooseFromLists.Item(CflID);
                    SAPbouiCOM.DataTable oDataTable = oCFLEvento.SelectedObjects;

                    if (pVal.ItemUID.Equals("C_CTAANTP"))
                    {
                        if (oDataTable != null)
                        {
                            oForm.DataSources.UserDataSources.Item("C_CTAANTP").Value = string.Empty;

                            string FormatCode = string.Empty;
                            FormatCode = EsPlanDeCuentasSegmentado ? string.Format(@"{0:" + @FormatoCuentasSegmentado + @"}", Convert.ToInt64(oDataTable.GetValue("FormatCode", 0).ToString())) : oDataTable.GetValue("FormatCode", 0).ToString();

                            oForm.DataSources.UserDataSources.Item("C_CTAANTP").Value = FormatCode;
                        }
                    }

                    if (pVal.ItemUID.Equals("C_CTASERP"))
                    {
                        if (oDataTable != null)
                        {
                            oForm.DataSources.UserDataSources.Item("C_CTASERP").Value = string.Empty;

                            string FormatCode = string.Empty;
                            FormatCode = EsPlanDeCuentasSegmentado ? string.Format(@"{0:" + @FormatoCuentasSegmentado + @"}", Convert.ToInt64(oDataTable.GetValue("FormatCode", 0).ToString())) : oDataTable.GetValue("FormatCode", 0).ToString();

                            oForm.DataSources.UserDataSources.Item("C_CTASERP").Value = FormatCode;
                        }
                    }

                    if (pVal.ItemUID.Equals("C_CTACMAT"))
                    {
                        if (oDataTable != null)
                        {
                            oForm.DataSources.UserDataSources.Item("C_CTACMAT").Value = string.Empty;

                            string FormatCode = string.Empty;
                            FormatCode = EsPlanDeCuentasSegmentado ? string.Format(@"{0:" + @FormatoCuentasSegmentado + @"}", Convert.ToInt64(oDataTable.GetValue("FormatCode", 0).ToString())) : oDataTable.GetValue("FormatCode", 0).ToString();

                            oForm.DataSources.UserDataSources.Item("C_CTACMAT").Value = FormatCode;
                        }
                    }

                    if (pVal.ItemUID.Equals("C_CTACFAC"))
                    {
                        if (oDataTable != null)
                        {
                            oForm.DataSources.UserDataSources.Item("C_CTACFAC").Value = string.Empty;

                            string FormatCode = string.Empty;
                            FormatCode = EsPlanDeCuentasSegmentado ? string.Format(@"{0:" + @FormatoCuentasSegmentado + @"}", Convert.ToInt64(oDataTable.GetValue("FormatCode", 0).ToString())) : oDataTable.GetValue("FormatCode", 0).ToString();

                            oForm.DataSources.UserDataSources.Item("C_CTACFAC").Value = FormatCode;
                        }
                    }

                    if (pVal.ItemUID.Equals("C_CTAPREM"))
                    {
                        if (oDataTable != null)
                        {
                            oForm.DataSources.UserDataSources.Item("C_CTAPREM").Value = string.Empty;

                            string FormatCode = string.Empty;
                            FormatCode = EsPlanDeCuentasSegmentado ? string.Format(@"{0:" + @FormatoCuentasSegmentado + @"}", Convert.ToInt64(oDataTable.GetValue("FormatCode", 0).ToString())) : oDataTable.GetValue("FormatCode", 0).ToString();

                            oForm.DataSources.UserDataSources.Item("C_CTAPREM").Value = FormatCode;
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
            oForm = ((SAPbouiCOM.Form)(Application.SBO_Application.Forms.Item("frmMonConfRG")));
            Folder1 = ((SAPbouiCOM.Folder)(GetItem("tab01").Specific));
            Folder2 = ((SAPbouiCOM.Folder)(GetItem("tab02").Specific));
            Folder3 = ((SAPbouiCOM.Folder)(GetItem("tab03").Specific));
            Folder1.Select();
            txtTOKEN = ((SAPbouiCOM.EditText)(GetItem("TOKEN").Specific));
            txtRUTSOC = ((SAPbouiCOM.EditText)(GetItem("RUTSOC").Specific));
            txtLOCALIZ = ((SAPbouiCOM.EditText)(GetItem("LOCALIZ").Specific));
            txtG_CCOST = ((SAPbouiCOM.EditText)(GetItem("G_CCOST").Specific));
            cbxG_CCOSTV = ((SAPbouiCOM.ComboBox)(GetItem("G_CCOSTV").Specific));
            txtG_TIPOD = ((SAPbouiCOM.EditText)(GetItem("G_TIPOD").Specific));
            cbxG_TIPODV = ((SAPbouiCOM.ComboBox)(GetItem("G_TIPODV").Specific));
            txtG_CODFA = ((SAPbouiCOM.EditText)(GetItem("G_CODFA").Specific));
            txtG_CODFE = ((SAPbouiCOM.EditText)(GetItem("G_CODFE").Specific));
            txtG_CODFM = ((SAPbouiCOM.EditText)(GetItem("G_CODFM").Specific));
            txtG_RUTP = ((SAPbouiCOM.EditText)(GetItem("G_RUTP").Specific));
            cbxG_RUTPV = ((SAPbouiCOM.ComboBox)(GetItem("G_RUTPV").Specific));
            txtG_NDOC = ((SAPbouiCOM.EditText)(GetItem("G_NDOC").Specific));
            cbxG_NDOCV = ((SAPbouiCOM.ComboBox)(GetItem("G_NDOCV").Specific));
            txtC_CTAANTP = ((SAPbouiCOM.EditText)(GetItem("C_CTAANTP").Specific));
            txtC_CTASERP = ((SAPbouiCOM.EditText)(GetItem("C_CTASERP").Specific));
            txtC_CTACMAT = ((SAPbouiCOM.EditText)(GetItem("C_CTACMAT").Specific));
            txtC_CTACFAC = ((SAPbouiCOM.EditText)(GetItem("C_CTACFAC").Specific));
            txtC_CTAPREM = ((SAPbouiCOM.EditText)(GetItem("C_CTAPREM").Specific));
            btnSave = ((SAPbouiCOM.Button)(GetItem("btnSave").Specific));
            btnClose = ((SAPbouiCOM.Button)(GetItem("2").Specific));
        }

        private void CargarFormulario()
        {
            // Choose from list cuentas contables
            SAPbouiCOM.ChooseFromListCollection oCFLs = null;
            SAPbouiCOM.ChooseFromList oCFL = null;
            SAPbouiCOM.ChooseFromListCreationParams oCFLCreationParams = null;
            SAPbouiCOM.Conditions oCons = null;

            // Blindear objetos de formulario
            oForm.DataSources.UserDataSources.Add("TOKEN", SAPbouiCOM.BoDataType.dt_SHORT_TEXT, 254);
            oForm.DataSources.UserDataSources.Add("RUTSOC", SAPbouiCOM.BoDataType.dt_SHORT_TEXT, 100);
            oForm.DataSources.UserDataSources.Add("LOCALIZ", SAPbouiCOM.BoDataType.dt_SHORT_TEXT, 100);
            oForm.DataSources.UserDataSources.Add("G_CCOST", SAPbouiCOM.BoDataType.dt_SHORT_TEXT, 100);
            oForm.DataSources.UserDataSources.Add("G_CCOSTV", SAPbouiCOM.BoDataType.dt_SHORT_TEXT, 100);
            oForm.DataSources.UserDataSources.Add("G_TIPOD", SAPbouiCOM.BoDataType.dt_SHORT_TEXT, 100);
            oForm.DataSources.UserDataSources.Add("G_TIPODV", SAPbouiCOM.BoDataType.dt_SHORT_TEXT, 100);
            oForm.DataSources.UserDataSources.Add("G_CODFA", SAPbouiCOM.BoDataType.dt_SHORT_TEXT, 100);
            oForm.DataSources.UserDataSources.Add("G_CODFE", SAPbouiCOM.BoDataType.dt_SHORT_TEXT, 100);
            oForm.DataSources.UserDataSources.Add("G_CODFM", SAPbouiCOM.BoDataType.dt_SHORT_TEXT, 100);
            oForm.DataSources.UserDataSources.Add("G_RUTP", SAPbouiCOM.BoDataType.dt_SHORT_TEXT, 100);
            oForm.DataSources.UserDataSources.Add("G_RUTPV", SAPbouiCOM.BoDataType.dt_SHORT_TEXT, 100);
            oForm.DataSources.UserDataSources.Add("G_NDOC", SAPbouiCOM.BoDataType.dt_SHORT_TEXT, 100);
            oForm.DataSources.UserDataSources.Add("G_NDOCV", SAPbouiCOM.BoDataType.dt_SHORT_TEXT, 100);
            oForm.DataSources.UserDataSources.Add("C_CTAANTP", SAPbouiCOM.BoDataType.dt_SHORT_TEXT, 100);
            oForm.DataSources.UserDataSources.Add("C_CTASERP", SAPbouiCOM.BoDataType.dt_SHORT_TEXT, 100);
            oForm.DataSources.UserDataSources.Add("C_CTACMAT", SAPbouiCOM.BoDataType.dt_SHORT_TEXT, 100);
            oForm.DataSources.UserDataSources.Add("C_CTACFAC", SAPbouiCOM.BoDataType.dt_SHORT_TEXT, 100);
            oForm.DataSources.UserDataSources.Add("C_CTAPREM", SAPbouiCOM.BoDataType.dt_SHORT_TEXT, 100);

            cbxG_CCOSTV.ValidValues.Add("VALUE", "Value");
            cbxG_CCOSTV.ValidValues.Add("CODE", "Code");

            cbxG_TIPODV.ValidValues.Add("VALUE", "Value");
            cbxG_TIPODV.ValidValues.Add("CODE", "Code");

            cbxG_RUTPV.ValidValues.Add("VALUE", "Value");
            cbxG_RUTPV.ValidValues.Add("CODE", "Code");

            cbxG_NDOCV.ValidValues.Add("VALUE", "Value");
            cbxG_NDOCV.ValidValues.Add("CODE", "Code");

            txtTOKEN.DataBind.SetBound(true, "", "TOKEN");
            txtRUTSOC.DataBind.SetBound(true, "", "RUTSOC");
            txtLOCALIZ.DataBind.SetBound(true, "", "LOCALIZ");
            txtG_CCOST.DataBind.SetBound(true, "", "G_CCOST");
            cbxG_CCOSTV.DataBind.SetBound(true, "", "G_CCOSTV");
            txtG_TIPOD.DataBind.SetBound(true, "", "G_TIPOD");
            cbxG_TIPODV.DataBind.SetBound(true, "", "G_TIPODV");
            txtG_CODFA.DataBind.SetBound(true, "", "G_CODFA");
            txtG_CODFE.DataBind.SetBound(true, "", "G_CODFE");
            txtG_CODFM.DataBind.SetBound(true, "", "G_CODFM");
            txtG_RUTP.DataBind.SetBound(true, "", "G_RUTP");
            cbxG_RUTPV.DataBind.SetBound(true, "", "G_RUTPV");
            txtG_NDOC.DataBind.SetBound(true, "", "G_NDOC");
            cbxG_NDOCV.DataBind.SetBound(true, "", "G_NDOCV");
            txtC_CTAANTP.DataBind.SetBound(true, "", "C_CTAANTP");
            txtC_CTASERP.DataBind.SetBound(true, "", "C_CTASERP");
            txtC_CTACMAT.DataBind.SetBound(true, "", "C_CTACMAT");
            txtC_CTACFAC.DataBind.SetBound(true, "", "C_CTACFAC");
            txtC_CTAPREM.DataBind.SetBound(true, "", "C_CTAPREM");

            oCFLs = oForm.ChooseFromLists;
            oCFLCreationParams = ((SAPbouiCOM.ChooseFromListCreationParams)(Application.SBO_Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_ChooseFromListCreationParams)));
            oCFLCreationParams.MultiSelection = false;
            oCFLCreationParams.ObjectType = "1";
            oCFLCreationParams.UniqueID = "cflCANT";
            oCFL = oCFLs.Add(oCFLCreationParams);
            ////Dar condiciones al ChooseFromList
            //oCons = new SAPbouiCOM.Conditions();
            //oCons = oCFL.GetConditions();
            //SAPbouiCOM.Condition oCon = oCons.Add();
            //oCon.Alias = "CardType";
            //oCon.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
            //oCon.CondVal = "S";
            //oCFL.SetConditions(oCons);
            //Asignamos el ChoosefromList al campo de texto
            txtC_CTAANTP.ChooseFromListUID = "cflCANT";
            txtC_CTAANTP.ChooseFromListAlias = "FormatCode";

            oCFLs = oForm.ChooseFromLists;
            oCFLCreationParams = ((SAPbouiCOM.ChooseFromListCreationParams)(Application.SBO_Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_ChooseFromListCreationParams)));
            oCFLCreationParams.MultiSelection = false;
            oCFLCreationParams.ObjectType = "1";
            oCFLCreationParams.UniqueID = "cflCSER";
            oCFL = oCFLs.Add(oCFLCreationParams);
            ////Dar condiciones al ChooseFromList
            //oCons = new SAPbouiCOM.Conditions();
            //oCons = oCFL.GetConditions();
            //SAPbouiCOM.Condition oCon = oCons.Add();
            //oCon.Alias = "CardType";
            //oCon.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
            //oCon.CondVal = "S";
            //oCFL.SetConditions(oCons);
            //Asignamos el ChoosefromList al campo de texto
            txtC_CTASERP.ChooseFromListUID = "cflCSER";
            txtC_CTASERP.ChooseFromListAlias = "FormatCode";

            oCFLs = oForm.ChooseFromLists;
            oCFLCreationParams = ((SAPbouiCOM.ChooseFromListCreationParams)(Application.SBO_Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_ChooseFromListCreationParams)));
            oCFLCreationParams.MultiSelection = false;
            oCFLCreationParams.ObjectType = "1";
            oCFLCreationParams.UniqueID = "cflCMAT";
            oCFL = oCFLs.Add(oCFLCreationParams);
            ////Dar condiciones al ChooseFromList
            //oCons = new SAPbouiCOM.Conditions();
            //oCons = oCFL.GetConditions();
            //SAPbouiCOM.Condition oCon = oCons.Add();
            //oCon.Alias = "CardType";
            //oCon.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
            //oCon.CondVal = "S";
            //oCFL.SetConditions(oCons);
            //Asignamos el ChoosefromList al campo de texto
            txtC_CTACMAT.ChooseFromListUID = "cflCMAT";
            txtC_CTACMAT.ChooseFromListAlias = "FormatCode";

            oCFLs = oForm.ChooseFromLists;
            oCFLCreationParams = ((SAPbouiCOM.ChooseFromListCreationParams)(Application.SBO_Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_ChooseFromListCreationParams)));
            oCFLCreationParams.MultiSelection = false;
            oCFLCreationParams.ObjectType = "1";
            oCFLCreationParams.UniqueID = "cflCFAC";
            oCFL = oCFLs.Add(oCFLCreationParams);
            ////Dar condiciones al ChooseFromList
            //oCons = new SAPbouiCOM.Conditions();
            //oCons = oCFL.GetConditions();
            //SAPbouiCOM.Condition oCon = oCons.Add();
            //oCon.Alias = "CardType";
            //oCon.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
            //oCon.CondVal = "S";
            //oCFL.SetConditions(oCons);
            //Asignamos el ChoosefromList al campo de texto
            txtC_CTACFAC.ChooseFromListUID = "cflCFAC";
            txtC_CTACFAC.ChooseFromListAlias = "FormatCode";

            oCFLs = oForm.ChooseFromLists;
            oCFLCreationParams = ((SAPbouiCOM.ChooseFromListCreationParams)(Application.SBO_Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_ChooseFromListCreationParams)));
            oCFLCreationParams.MultiSelection = false;
            oCFLCreationParams.ObjectType = "1";
            oCFLCreationParams.UniqueID = "cflCREM";
            oCFL = oCFLs.Add(oCFLCreationParams);
            ////Dar condiciones al ChooseFromList
            //oCons = new SAPbouiCOM.Conditions();
            //oCons = oCFL.GetConditions();
            //SAPbouiCOM.Condition oCon = oCons.Add();
            //oCon.Alias = "CardType";
            //oCon.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
            //oCon.CondVal = "S";
            //oCFL.SetConditions(oCons);
            //Asignamos el ChoosefromList al campo de texto
            txtC_CTAPREM.ChooseFromListUID = "cflCREM";
            txtC_CTAPREM.ChooseFromListAlias = "FormatCode";

            oForm.Visible = true;

            CargarConfiguracion();
        }

        private void CargarConfiguracion()
        {
            oForm.DataSources.UserDataSources.Item("TOKEN").Value = ExtConf.Parametros.Token;
            oForm.DataSources.UserDataSources.Item("RUTSOC").Value = ExtConf.Parametros.Rut_Sociedad;
            oForm.DataSources.UserDataSources.Item("LOCALIZ").Value = ExtConf.Parametros.Localizacion;
            oForm.DataSources.UserDataSources.Item("G_CCOST").Value = ExtConf.Parametros.Campo_Centro_Costo;
            oForm.DataSources.UserDataSources.Item("G_CCOSTV").Value = ExtConf.Parametros.Campo_Centro_Costo_Valor;
            oForm.DataSources.UserDataSources.Item("G_TIPOD").Value = ExtConf.Parametros.Campo_Tipo_Documento;
            oForm.DataSources.UserDataSources.Item("G_TIPODV").Value = ExtConf.Parametros.Campo_Tipo_Documento_Valor;
            oForm.DataSources.UserDataSources.Item("G_CODFA").Value = ExtConf.Parametros.Codigo_Factura_Afecta;
            oForm.DataSources.UserDataSources.Item("G_CODFE").Value = ExtConf.Parametros.Codigo_Factura_Exenta;
            oForm.DataSources.UserDataSources.Item("G_CODFM").Value = ExtConf.Parametros.Codigo_Factura_Materiales;
            oForm.DataSources.UserDataSources.Item("G_RUTP").Value = ExtConf.Parametros.Campo_Rut_Proveedor;
            oForm.DataSources.UserDataSources.Item("G_RUTPV").Value = ExtConf.Parametros.Campo_Rut_Proveedor_Valor;
            oForm.DataSources.UserDataSources.Item("G_NDOC").Value = ExtConf.Parametros.Campo_Numero_Documento;
            oForm.DataSources.UserDataSources.Item("G_NDOCV").Value = ExtConf.Parametros.Campo_Numero_Documento_Valor;
            oForm.DataSources.UserDataSources.Item("C_CTAANTP").Value = ExtConf.Parametros.Cuenta_Anticipo_Proveedores;
            oForm.DataSources.UserDataSources.Item("C_CTASERP").Value = ExtConf.Parametros.Cuenta_Servicios_Proveedores;
            oForm.DataSources.UserDataSources.Item("C_CTACMAT").Value = ExtConf.Parametros.Cuenta_Compra_Materiales;
            oForm.DataSources.UserDataSources.Item("C_CTACFAC").Value = ExtConf.Parametros.Cuenta_Compensacion_Facturas;
            oForm.DataSources.UserDataSources.Item("C_CTAPREM").Value = ExtConf.Parametros.Cuenta_Pago_Reembolsos;
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
                oGeneralService = oCompanyService.GetGeneralService("SO_RENDICF");
                // Create data for new row in main UDO
                oGeneralParams = ((SAPbobsCOM.GeneralDataParams)(oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)));
                oGeneralData = ((SAPbobsCOM.GeneralData)(oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralData)));
                bool existeconf = SBO.ConsultasSBO.ExisteConfiguracion();
                if (existeconf)
                {
                    oGeneralParams.SetProperty("Code", "CONF");
                    oGeneralData = oGeneralService.GetByParams(oGeneralParams);
                    oGeneralData.SetProperty("Code", "CONF");
                    oGeneralData.SetProperty("Name", "Configuración Monitor Rindegastos");
                    oGeneralData.SetProperty("U_TOKEN", oForm.DataSources.UserDataSources.Item("TOKEN").Value);
                    oGeneralData.SetProperty("U_RUTSOC", oForm.DataSources.UserDataSources.Item("RUTSOC").Value);
                    oGeneralData.SetProperty("U_LOCALIZ", oForm.DataSources.UserDataSources.Item("LOCALIZ").Value);
                    oGeneralData.SetProperty("U_G_CCOST", oForm.DataSources.UserDataSources.Item("G_CCOST").Value);
                    oGeneralData.SetProperty("U_G_CCOSTV", oForm.DataSources.UserDataSources.Item("G_CCOSTV").Value);
                    oGeneralData.SetProperty("U_G_TIPOD", oForm.DataSources.UserDataSources.Item("G_TIPOD").Value);
                    oGeneralData.SetProperty("U_G_TIPODV", oForm.DataSources.UserDataSources.Item("G_TIPODV").Value);
                    oGeneralData.SetProperty("U_G_CODFA", oForm.DataSources.UserDataSources.Item("G_CODFA").Value);
                    oGeneralData.SetProperty("U_G_CODFE", oForm.DataSources.UserDataSources.Item("G_CODFE").Value);
                    oGeneralData.SetProperty("U_G_CODFM", oForm.DataSources.UserDataSources.Item("G_CODFM").Value);
                    oGeneralData.SetProperty("U_G_RUTP", oForm.DataSources.UserDataSources.Item("G_RUTP").Value);
                    oGeneralData.SetProperty("U_G_RUTPV", oForm.DataSources.UserDataSources.Item("G_RUTPV").Value);
                    oGeneralData.SetProperty("U_G_NDOC", oForm.DataSources.UserDataSources.Item("G_NDOC").Value);
                    oGeneralData.SetProperty("U_G_NDOCV", oForm.DataSources.UserDataSources.Item("G_NDOCV").Value);
                    oGeneralData.SetProperty("U_C_CTAANTP", oForm.DataSources.UserDataSources.Item("C_CTAANTP").Value);
                    oGeneralData.SetProperty("U_C_CTASERP", oForm.DataSources.UserDataSources.Item("C_CTASERP").Value);
                    oGeneralData.SetProperty("U_C_CTACMAT", oForm.DataSources.UserDataSources.Item("C_CTACMAT").Value);
                    oGeneralData.SetProperty("U_C_CTACFAC", oForm.DataSources.UserDataSources.Item("C_CTACFAC").Value);
                    oGeneralData.SetProperty("U_C_CTAPREM", oForm.DataSources.UserDataSources.Item("C_CTAPREM").Value);
                    oGeneralService.Update(oGeneralData);
                }
                else
                {
                    oGeneralData.SetProperty("Code", "CONF");
                    oGeneralData.SetProperty("Name", "Configuración Monitor Rindegastos");
                    oGeneralData.SetProperty("U_TOKEN", oForm.DataSources.UserDataSources.Item("TOKEN").Value);
                    oGeneralData.SetProperty("U_RUTSOC", oForm.DataSources.UserDataSources.Item("RUTSOC").Value);
                    oGeneralData.SetProperty("U_LOCALIZ", oForm.DataSources.UserDataSources.Item("LOCALIZ").Value);
                    oGeneralData.SetProperty("U_G_CCOST", oForm.DataSources.UserDataSources.Item("G_CCOST").Value);
                    oGeneralData.SetProperty("U_G_CCOSTV", oForm.DataSources.UserDataSources.Item("G_CCOSTV").Value);
                    oGeneralData.SetProperty("U_G_TIPOD", oForm.DataSources.UserDataSources.Item("G_TIPOD").Value);
                    oGeneralData.SetProperty("U_G_TIPODV", oForm.DataSources.UserDataSources.Item("G_TIPODV").Value);
                    oGeneralData.SetProperty("U_G_CODFA", oForm.DataSources.UserDataSources.Item("G_CODFA").Value);
                    oGeneralData.SetProperty("U_G_CODFE", oForm.DataSources.UserDataSources.Item("G_CODFE").Value);
                    oGeneralData.SetProperty("U_G_CODFM", oForm.DataSources.UserDataSources.Item("G_CODFM").Value);
                    oGeneralData.SetProperty("U_G_RUTP", oForm.DataSources.UserDataSources.Item("G_RUTP").Value);
                    oGeneralData.SetProperty("U_G_RUTPV", oForm.DataSources.UserDataSources.Item("G_RUTPV").Value);
                    oGeneralData.SetProperty("U_G_NDOC", oForm.DataSources.UserDataSources.Item("G_NDOC").Value);
                    oGeneralData.SetProperty("U_G_NDOCV", oForm.DataSources.UserDataSources.Item("G_NDOCV").Value);
                    oGeneralData.SetProperty("U_C_CTAANTP", oForm.DataSources.UserDataSources.Item("C_CTAANTP").Value);
                    oGeneralData.SetProperty("U_C_CTASERP", oForm.DataSources.UserDataSources.Item("C_CTASERP").Value);
                    oGeneralData.SetProperty("U_C_CTACMAT", oForm.DataSources.UserDataSources.Item("C_CTACMAT").Value);
                    oGeneralData.SetProperty("U_C_CTACFAC", oForm.DataSources.UserDataSources.Item("C_CTACFAC").Value);
                    oGeneralData.SetProperty("U_C_CTAPREM", oForm.DataSources.UserDataSources.Item("C_CTAPREM").Value);
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
