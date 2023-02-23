using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM.Framework;

namespace Soindus.AddOnFactoringCob.Formularios
{
    [FormAttribute("Soindus.AddOnFactoringCob.Formularios.frmFacCuentaSN", "Formularios/frmFacCuentaSN.b1f")]
    class frmFacCuentaSN : UserFormBase
    {
        // Objetos de formulario
        #region Objetos de formulario
        private static SAPbouiCOM.Form oForm;
        private static SAPbouiCOM.EditText txtCodSN;
        private static SAPbouiCOM.EditText txtNomSN;
        private static SAPbouiCOM.EditText txtFDesde;
        private static SAPbouiCOM.EditText txtFHasta;
        private static SAPbouiCOM.EditText txtTotML;
        private static SAPbouiCOM.EditText txtTotME;
        //private static SAPbouiCOM.ComboBox cbxTipo;
        private static SAPbouiCOM.Button btnFiltrar;
        private static SAPbouiCOM.CheckBox chkNoRec;
        //private static SAPbouiCOM.Folder tab01;
        //private static SAPbouiCOM.Folder tab02;
        private static SAPbouiCOM.Matrix mtxDet;
        //private static SAPbouiCOM.Button btnMarcar;
        //private static SAPbouiCOM.Button btnAsign;
        //private static SAPbouiCOM.Button btnDesas;
        //private static SAPbouiCOM.Button btnPrelim;
        #endregion

        public frmFacCuentaSN()
        {
            AsignarObjetos();
            CargarFormulario();
        }

        public override void OnInitializeComponent()
        {
            this.OnCustomInitialize();
        }

        public override void OnInitializeFormEvents()
        {
        }

        private void OnCustomInitialize()
        {

        }

        public static void Form_ItemEvent(string FormUID, ref SAPbouiCOM.ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            try
            {
                if (pVal.BeforeAction.Equals(true))
                {
                    // Link documento base en matrix
                    #region link documento base en matrix
                    if (pVal.EventType.Equals(SAPbouiCOM.BoEventTypes.et_MATRIX_LINK_PRESSED))
                    {
                        // Matrix de detalle
                        if (pVal.ItemUID.Equals("mtxDet"))
                        {
                            if (pVal.ColUID.Equals("CreatedBy"))
                            {
                                string sObjectType =
                                    ((SAPbouiCOM.EditText)(mtxDet.Columns.Item("ObjType").Cells.Item(pVal.Row).Specific)).String;
                                SAPbouiCOM.Column oColumn = (SAPbouiCOM.Column)mtxDet.Columns.Item(pVal.ColUID);
                                SAPbouiCOM.LinkedButton oLink = ((SAPbouiCOM.LinkedButton)(oColumn.ExtendedObject));
                                oLink.LinkedObjectType = sObjectType;
                            }
                        }
                    }
                    #endregion
                }
                else
                {
                    if (pVal.EventType.Equals(SAPbouiCOM.BoEventTypes.et_LOST_FOCUS))
                    {
                        // Texto SN
                        #region txtCodSN
                        if (pVal.ItemUID.Equals("CodSN"))
                        {
                            SAPbouiCOM.EditText oCodSN = (SAPbouiCOM.EditText)oForm.Items.Item("CodSN").Specific;
                            String Prov = oCodSN.Value;

                            if (String.IsNullOrEmpty(Prov))
                            {
                                SAPbouiCOM.EditText oNomSN = (SAPbouiCOM.EditText)oForm.Items.Item("NomSN").Specific;
                                oNomSN.Value = string.Empty;
                            }
                        }
                        #endregion
                    }

                    if (pVal.EventType.Equals(SAPbouiCOM.BoEventTypes.et_CLICK))
                    {
                        // Botón Filtrar
                        #region Filtrar
                        if (pVal.ItemUID.Equals("btnFiltrar"))
                        {
                            SAPbouiCOM.EditText oCodSN = (SAPbouiCOM.EditText)oForm.Items.Item("CodSN").Specific;
                            String Prov = oCodSN.Value;

                            if (!String.IsNullOrEmpty(Prov))
                            {
                                CargarMatrixDetalle();
                            }
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

                        if (pVal.ItemUID.Equals("CodSN"))
                        {
                            if (oDataTable != null)
                            {
                                oForm.DataSources.UserDataSources.Item("CodSN").Value = string.Empty;
                                oForm.DataSources.UserDataSources.Item("NomSN").Value = string.Empty;

                                string CardCode = string.Empty;
                                string CardName = string.Empty;
                                CardCode = oDataTable.GetValue(0, 0).ToString();
                                CardName = oDataTable.GetValue("CardName", 0).ToString();

                                oForm.DataSources.UserDataSources.Item("CodSN").Value = CardCode;
                                oForm.DataSources.UserDataSources.Item("NomSN").Value = CardName;
                            }
                        }
                        Comun.FuncionesComunes.LiberarObjetoGenerico(oDataTable);
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void AsignarObjetos()
        {
            oForm = ((SAPbouiCOM.Form)(Application.SBO_Application.Forms.Item("frmFacCuentaSN")));
            txtCodSN = ((SAPbouiCOM.EditText)(GetItem("CodSN").Specific));
            txtNomSN = ((SAPbouiCOM.EditText)(GetItem("NomSN").Specific));
            txtFDesde = ((SAPbouiCOM.EditText)(GetItem("FDesde").Specific));
            txtFHasta = ((SAPbouiCOM.EditText)(GetItem("FHasta").Specific));
            txtTotML = ((SAPbouiCOM.EditText)(GetItem("TotML").Specific));
            txtTotME = ((SAPbouiCOM.EditText)(GetItem("TotME").Specific));
            ////cbxTipo = ((SAPbouiCOM.ComboBox)(GetItem("cbxTipo").Specific));
            btnFiltrar = ((SAPbouiCOM.Button)(GetItem("btnFiltrar").Specific));
            chkNoRec= ((SAPbouiCOM.CheckBox)(GetItem("NoRec").Specific));
            //tab01 = ((SAPbouiCOM.Folder)(GetItem("tab01").Specific));
            //tab02 = ((SAPbouiCOM.Folder)(GetItem("tab02").Specific));
            //tab01.Select();
            mtxDet = ((SAPbouiCOM.Matrix)(GetItem("mtxDet").Specific));
            //mtxDocsCF = ((SAPbouiCOM.Matrix)(GetItem("mtxDocsCF").Specific));
            //btnMarcar = ((SAPbouiCOM.Button)(GetItem("btnMarcar").Specific));
            //btnAsign = ((SAPbouiCOM.Button)(GetItem("btnAsign").Specific));
            //btnDesas = ((SAPbouiCOM.Button)(GetItem("btnDesas").Specific));
            //btnPrelim = ((SAPbouiCOM.Button)(GetItem("btnPrelim").Specific));
        }

        private void CargarFormulario()
        {
            // Propiedades del formulario

            // Choose from list socio de negocio
            SAPbouiCOM.ChooseFromListCollection oCFLs = null;
            SAPbouiCOM.ChooseFromList oCFL = null;
            SAPbouiCOM.ChooseFromListCreationParams oCFLCreationParams = null;
            SAPbouiCOM.Conditions oCons = null;

            // Socio de negocios
            oForm.DataSources.UserDataSources.Add("CodSN", SAPbouiCOM.BoDataType.dt_SHORT_TEXT, 200);
            txtCodSN.DataBind.SetBound(true, "", "CodSN");
            oForm.DataSources.UserDataSources.Add("NomSN", SAPbouiCOM.BoDataType.dt_SHORT_TEXT, 200);
            txtNomSN.DataBind.SetBound(true, "", "NomSN");

            oCFLs = oForm.ChooseFromLists;
            oCFLCreationParams = ((SAPbouiCOM.ChooseFromListCreationParams)(Application.SBO_Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_ChooseFromListCreationParams)));
            oCFLCreationParams.MultiSelection = false;
            oCFLCreationParams.ObjectType = "2";
            oCFLCreationParams.UniqueID = "cflSN";

            oCFL = oCFLs.Add(oCFLCreationParams);

            oCons = new SAPbouiCOM.Conditions();
            //Dar condiciones al ChooseFromList
            oCons = oCFL.GetConditions();

            SAPbouiCOM.Condition oCon = oCons.Add();
            oCon.Alias = "CardType";
            oCon.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
            oCon.CondVal = "C";

            oCFL.SetConditions(oCons);

            //Asignamos el ChoosefromList al campo de texto
            txtCodSN.ChooseFromListUID = "cflSN";
            txtCodSN.ChooseFromListAlias = "CardCode";

            // Fecha desde
            oForm.DataSources.UserDataSources.Add("FDesde", SAPbouiCOM.BoDataType.dt_DATE);
            txtFDesde.DataBind.SetBound(true, "", "FDesde");
            // Fecha hasta
            oForm.DataSources.UserDataSources.Add("FHasta", SAPbouiCOM.BoDataType.dt_DATE);
            txtFHasta.DataBind.SetBound(true, "", "FHasta");

            // Ver sólo operaciones no reconciliadas
            oForm.DataSources.UserDataSources.Add("NoRec", SAPbouiCOM.BoDataType.dt_SHORT_TEXT);
            chkNoRec.DataBind.SetBound(true, "", "NoRec");

            // Total ML
            oForm.DataSources.UserDataSources.Add("TotML", SAPbouiCOM.BoDataType.dt_PRICE);
            txtTotML.DataBind.SetBound(true, "", "TotML");
            // Total ME
            oForm.DataSources.UserDataSources.Add("TotME", SAPbouiCOM.BoDataType.dt_PRICE);
            txtTotME.DataBind.SetBound(true, "", "TotME");

            oForm.DataSources.DataTables.Add("dtDet");

            EstructuraMatrixDetalle();

            mtxDet.SelectionMode = SAPbouiCOM.BoMatrixSelect.ms_Single;
            oForm.Visible = true;
        }

        private static void EstructuraMatrixDetalle()
        {
            string NombreDT = "dtDet";
            SAPbouiCOM.DataTable datatable = oForm.DataSources.DataTables.Item(NombreDT);
            string _query = @"
                SELECT 
                000000 AS ""Num"",
                CASE 	
                WHEN T0.""TransType"" = -2  THEN 30 
                WHEN T0.""TransType"" = -3  THEN 30 
                ELSE T0.""TransType""
                END AS ""ObjType"",
                CASE 	
                WHEN T0.""TransType"" = -2  THEN 'SI-Saldo Inicial' 
                WHEN T0.""TransType"" = -3  THEN 'CB-Cierre Período' 
                WHEN T0.""TransType"" = 13  THEN 'RF-Factura Deudores' 
                WHEN T0.""TransType"" = 14  THEN 'RC-Nota Credito Clientes'
                WHEN T0.""TransType"" = 15  THEN 'NE-Entrega'
                WHEN T0.""TransType"" = 16  THEN 'DV-Devolucion'
                WHEN T0.""TransType"" = 18  THEN 'TT-Factura Proveedores'
                WHEN T0.""TransType"" = 19  THEN 'PC-Nota Credito Proveedores'
                WHEN T0.""TransType"" = 20  THEN 'EP-Entrada Mercancias'
                WHEN T0.""TransType"" = 21  THEN 'DM-Devolucion Mercancías'
                WHEN T0.""TransType"" = 24  THEN 'PR-Pagos Recibidos'
                WHEN T0.""TransType"" = 30  THEN 'AS-Asiento'
                WHEN T0.""TransType"" = 46  THEN 'PP-Pagos Efectuados'
                WHEN T0.""TransType"" = 59  THEN 'EM-Entrada Mercancías'
                WHEN T0.""TransType"" = 60  THEN 'OA-Emisión para producción'
                WHEN T0.""TransType"" = 67  THEN 'IM-Transferencia de Stock'
                WHEN T0.""TransType"" = 69  THEN 'DI-Precio Entrega'
                WHEN T0.""TransType"" = 162 THEN 'RI-Revalorización Inventario'
                WHEN T0.""TransType"" = 202 THEN 'OF-Orden de Fabricación'
                WHEN T0.""TransType"" = 204 THEN 'AN-F Anticipo Proveedores'
                WHEN T0.""TransType"" = 321 THEN 'ID-Reconciliación Interna'
                ELSE T0.""TransType""
                END AS ""Origen"",
                T0.""TransId"", T0.""Line_ID"", T0.""Account"", T0.""Debit"", T0.""Credit"", T0.""SYSCred"", T0.""SYSDeb"", T0.""FCDebit"", T0.""FCCredit"",
                T0.""FCCurrency"", T0.""DueDate"", T0.""SourceID"", T0.""SourceLine"", T0.""ShortName"", T0.""IntrnMatch"", T0.""ExtrMatch"", T0.""ContraAct"",
                T0.""LineMemo"", T0.""Ref3Line"", T0.""TransType"", T0.""RefDate"", T0.""Ref2Date"", T0.""Ref1"", T0.""Ref2"", T0.""CreatedBy"", T0.""BaseRef"",
                T0.""Project"", T0.""TransCode"", T0.""ProfitCode"", T0.""TaxDate"", T0.""SystemRate"", T0.""MthDate"", T0.""ToMthSum"", T0.""UserSign"",
                T0.""BatchNum"", T0.""FinncPriod"", T0.""RelTransId"", T0.""RelLineID"", T0.""RelType"", T0.""LogInstanc"", T0.""VatGroup"", T0.""BaseSum"",
                T0.""VatRate"", T0.""Indicator"", T0.""AdjTran"", T0.""RevSource"", T0.""ObjType"", T0.""VatDate"", T0.""PaymentRef"", T0.""SYSBaseSum"", 
                T0.""MultMatch"", T0.""VatLine"", T0.""VatAmount"", T0.""SYSVatSum"", T0.""Closed"", T0.""GrossValue"", T0.""CheckAbs"", T0.""LineType"",
                T0.""DebCred"", T0.""SequenceNr"", T0.""StornoAcc"", T0.""BalDueDeb"", T0.""BalDueCred"", T0.""BalFcDeb"", T0.""BalFcCred"", T0.""BalScDeb"",
                T0.""BalScCred"", T0.""IsNet"", T0.""DunWizBlck"", T0.""DunnLevel"", T0.""DunDate"", T0.""TaxType"", T0.""TaxPostAcc"", T0.""StaCode"",
                T0.""StaType"", T0.""TaxCode"", T0.""ValidFrom"", T0.""GrossValFc"", T0.""LvlUpdDate"", T0.""OcrCode2"", T0.""OcrCode3"", T0.""OcrCode4"",
                T0.""OcrCode5"", T0.""MIEntry"", T0.""MIVEntry"", T0.""ClsInTP"", T0.""CenVatCom"", T0.""MatType"", T0.""PstngType"", T0.""ValidFrom2"",
                T0.""ValidFrom3"", T0.""ValidFrom4"", T0.""ValidFrom5"", T0.""Location"", T0.""WTaxCode"", T0.""EquVatRate"", T0.""EquVatSum"", T0.""SYSEquSum"",
                T0.""TotalVat"", T0.""SYSTVat"", T0.""WTLiable"", T0.""WTLine"", T0.""WTApplied"", T0.""WTAppliedS"", T0.""WTAppliedF"", T0.""WTSum"",
                T0.""WTSumFC"", T0.""WTSumSC"", T0.""PayBlock"", T0.""PayBlckRef"", T0.""LicTradNum"", T0.""InterimTyp"", T0.""DprId"", T0.""MatchRef"",
                T0.""Ordered"", T0.""CUP"", T0.""CIG"", T0.""BPLId"", T0.""BPLName"", T0.""VatRegNum"", T0.""SLEDGERF"", T0.""InitRef2"", T0.""InitRef3Ln"",
                T0.""ExpUUID"", T0.""ExpOPType"", T0.""ExTransId"", T0.""DocArr"", T0.""DocLine"",
                CASE
                WHEN T0.""DebCred"" = 'D' THEN ISNULL(T5.""MainCurncy"", '') + ' ' + FORMAT(T0.""Debit"", 'N', CASE WHEN T5.""DecSep"" = ',' THEN 'de-DE' ELSE 'en-US' END)
                WHEN T0.""DebCred"" = 'C' THEN ISNULL(T5.""MainCurncy"", '') + ' (' + FORMAT(T0.""Credit"", 'N', CASE WHEN T5.""DecSep"" = ',' THEN 'de-DE' ELSE 'en-US' END) + ')'
                ELSE ''
                END AS ""SaldoML"",
                CASE
                WHEN T0.""DebCred"" = 'D' THEN T0.""Debit""
                WHEN T0.""DebCred"" = 'C' THEN (T0.""Credit"" * -1)
                ELSE ''
                END AS ""SaldoMLTot"",
                CASE
                WHEN T0.""DebCred"" = 'D' THEN ISNULL(T5.""MainCurncy"", '') + ' ' + FORMAT(T0.""BalDueDeb"", 'N', CASE WHEN T5.""DecSep"" = ',' THEN 'de-DE' ELSE 'en-US' END)
                WHEN T0.""DebCred"" = 'C' THEN ISNULL(T5.""MainCurncy"", '') + ' (' + FORMAT(T0.""BalDueCred"", 'N', CASE WHEN T5.""DecSep"" = ',' THEN 'de-DE' ELSE 'en-US' END) + ')'
                ELSE ''
                END AS ""SaldoVML"",
                CASE
                WHEN T0.""DebCred"" = 'D' AND T0.""FCDebit"" <> 0 THEN ISNULL(T0.""FCCurrency"", '') + ' ' + FORMAT(T0.""FCDebit"", 'N0', CASE WHEN T5.""DecSep"" = ',' THEN 'de-DE' ELSE 'en-US' END)
                WHEN T0.""DebCred"" = 'C' AND T0.""FCCredit"" <> 0 THEN ISNULL(T0.""FCCurrency"", '') + ' (' + FORMAT(T0.""FCCredit"", 'N0', CASE WHEN T5.""DecSep"" = ',' THEN 'de-DE' ELSE 'en-US' END) + ')'
                ELSE ''
                END AS ""SaldoME"",
                CASE
                WHEN T0.""DebCred"" = 'D' THEN T0.""FCDebit""
                WHEN T0.""DebCred"" = 'C' THEN (T0.""FCCredit"" * -1)
                ELSE ''
                END AS ""SaldoMETot"",
                CASE
                WHEN T0.""DebCred"" = 'D' AND T0.""FCDebit"" <> 0 THEN ISNULL(T0.""FCCurrency"", '') + ' ' + FORMAT(T0.""BalFcDeb"", 'N0', CASE WHEN T5.""DecSep"" = ',' THEN 'de-DE' ELSE 'en-US' END)
                WHEN T0.""DebCred"" = 'C' AND T0.""FCCredit"" <> 0 THEN ISNULL(T0.""FCCurrency"", '') + ' (' + FORMAT(T0.""BalFcCred"", 'N0', CASE WHEN T5.""DecSep"" = ',' THEN 'de-DE' ELSE 'en-US' END) + ')'
                ELSE ''
                END AS ""SaldoVME"",
                CASE
                WHEN T0.""DebCred"" = 'D' THEN ISNULL(T5.""MainCurncy"", '') + ' ' + FORMAT(T0.""Debit"", 'N', CASE WHEN T5.""DecSep"" = ',' THEN 'de-DE' ELSE 'en-US' END)
                ELSE ''
                END AS ""CargoML"",
                CASE
                WHEN T0.""DebCred"" = 'C' THEN ISNULL(T5.""MainCurncy"", '') + ' ' + FORMAT(T0.""Credit"", 'N', CASE WHEN T5.""DecSep"" = ',' THEN 'de-DE' ELSE 'en-US' END)
                ELSE ''
                END AS ""AbonoML"",
                T1.""USER_CODE"", T2.""MaxReconNum"", T3.""AgrNo"",
                CASE
                WHEN T3.""FolioNum"" > 0 THEN T3.""FolioPref"" + '-' + CONVERT(NVARCHAR(20), T3.""FolioNum"")
                ELSE ''
                END AS ""Folio"",
                T4.""Number"",
                T5.""MainCurncy"",
                FCRNG.""U_FACTORINGID"" AS ""FacID"", FCRNG.""U_INSTITU"" AS ""FacEntidad"", FCRNG.""U_FACTORINGVCTO"" AS ""FacFecha"", FCRNG.""U_NUMOPER"" AS ""FacOper"", FCRNG.""U_RESFAC"" AS ""FacResp""
                FROM ""JDT1"" T0
                LEFT OUTER JOIN ""OUSR"" T1 ON T1.""USERID"" = T0.""UserSign""    
                LEFT OUTER JOIN 
                (SELECT T0.""TransId"" AS ""TransId"", T0.""TransRowId"" AS ""TransRowId"", MAX(T0.""ReconNum"") AS ""MaxReconNum"" FROM ""ITR1"" T0 GROUP BY T0.""TransId"", T0.""TransRowId"")
                T2 ON T2.""TransId"" = T0.""TransId"" AND T2.""TransRowId"" = T0.""Line_ID""   
                INNER JOIN ""OJDT"" T3 ON T3.""TransId"" = T0.""TransId""    
                LEFT OUTER JOIN ""OOAT"" T4 ON T4.""AbsID"" = T3.""AgrNo""   
                INNER JOIN ""OADM"" T5 ON 1 = 1
                LEFT OUTER JOIN (
                SELECT ""DocEntry"", ""DocNum"", ""TransId"", ""U_FACTORINGID"", ""U_INSTITU"", ""U_FACTORINGVCTO"", ""U_NUMOPER"", ""U_RESFAC"" FROM OINV 
                UNION ALL
                SELECT ""DocEntry"", ""DocNum"", ""TransId"", ""U_FACTORINGID"", ""U_INSTITU"", ""U_FACTORINGVCTO"", ""U_NUMOPER"", ""U_RESFAC"" FROM ORIN
                ) FCRNG ON T0.""TransId"" = FCRNG.""TransId""
                WHERE 1 = 0";
            datatable.ExecuteQuery(_query);

            SAPbouiCOM.Columns oColumns;
            oColumns = mtxDet.Columns;
            SAPbouiCOM.Column oColumn;

            oColumn = oColumns.Add("Num", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "#";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 15;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "Num");

            oColumn = oColumns.Add("RefDate", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Contabilización";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 90;
            oColumn.RightJustified = false;
            oColumn.DataBind.Bind(NombreDT, "RefDate");

            oColumn = oColumns.Add("DueDate", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Vencimiento";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 90;
            oColumn.RightJustified = false;
            oColumn.DataBind.Bind(NombreDT, "DueDate");

            oColumn = oColumns.Add("Origen", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Origen";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 120;
            oColumn.RightJustified = false;
            oColumn.DataBind.Bind(NombreDT, "Origen");

            oColumn = oColumns.Add("Folio", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "N° Folio";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 60;
            oColumn.RightJustified = false;
            oColumn.DataBind.Bind(NombreDT, "Folio");

            //oColumn = oColumns.Add("TipoDoc", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            //oColumn.TitleObject.Caption = "Tipo";
            //oColumn.TitleObject.Sortable = true;
            //oColumn.Editable = false;
            //oColumn.Visible = true;
            //oColumn.Width = 60;
            //oColumn.RightJustified = true;
            //oColumn.DataBind.Bind(NombreDT, "TipoDoc");

            oColumn = oColumns.Add("ObjType", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "ObjType";
            oColumn.TitleObject.Sortable = false;
            oColumn.Editable = false;
            oColumn.Visible = false;
            oColumn.Width = 20;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "ObjType");

            oColumn = oColumns.Add("TransType", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "TransType";
            oColumn.TitleObject.Sortable = false;
            oColumn.Editable = false;
            oColumn.Visible = false;
            oColumn.Width = 20;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "TransType");

            oColumn = oColumns.Add("CreatedBy", SAPbouiCOM.BoFormItemTypes.it_LINKED_BUTTON);
            oColumn.TitleObject.Caption = "Link";
            oColumn.TitleObject.Sortable = false;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 15;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "CreatedBy");

            oColumn = oColumns.Add("BaseRef", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Número de origen";
            oColumn.TitleObject.Sortable = false;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 80;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "BaseRef");

            oColumn = oColumns.Add("ContraAct", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Cuenta de contrapartida";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 100;
            oColumn.RightJustified = false;
            oColumn.DataBind.Bind(NombreDT, "ContraAct");

            oColumn = oColumns.Add("LineMemo", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Info. detallada";
            oColumn.TitleObject.Sortable = false;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 200;
            oColumn.RightJustified = false;
            oColumn.DataBind.Bind(NombreDT, "LineMemo");

            oColumn = oColumns.Add("SaldoML", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "C/D (ML)";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 90;
            oColumn.RightJustified = true;
            oColumn.ForeColor = 2;
            oColumn.DataBind.Bind(NombreDT, "SaldoML");

            oColumn = oColumns.Add("SaldoVML", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Saldo vencido (ML)";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 90;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "SaldoVML");

            oColumn = oColumns.Add("SaldoME", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "C/D (ME)";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 90;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "SaldoME");

            oColumn = oColumns.Add("SaldoVME", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Saldo vencido (ME)";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 90;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "SaldoVME");

            oColumn = oColumns.Add("CargoML", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Cargo (ML)";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 90;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "CargoML");

            oColumn = oColumns.Add("AbonoML", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Abono (ML)";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 90;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "AbonoML");

            oColumn = oColumns.Add("Number", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Acuerdo global";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = false;
            oColumn.Width = 90;
            oColumn.RightJustified = false;
            oColumn.DataBind.Bind(NombreDT, "Number");

            oColumn = oColumns.Add("SaldoMLTot", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "C/D (ML)";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = false;
            oColumn.Width = 90;
            oColumn.RightJustified = true;
            oColumn.ForeColor = 2;
            oColumn.DataBind.Bind(NombreDT, "SaldoMLTot");

            oColumn = oColumns.Add("SaldoMETot", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "C/D (ME)";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = false;
            oColumn.Width = 90;
            oColumn.RightJustified = true;
            oColumn.ForeColor = 2;
            oColumn.DataBind.Bind(NombreDT, "SaldoMETot");

            oColumn = oColumns.Add("FacID", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Fact. ID";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 50;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "FacID");

            oColumn = oColumns.Add("FacEntidad", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Fact. Entidad";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 80;
            oColumn.RightJustified = false;
            oColumn.DataBind.Bind(NombreDT, "FacEntidad");

            oColumn = oColumns.Add("FacFecha", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Fact. Otorg.";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 80;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "FacFecha");

            oColumn = oColumns.Add("FacOper", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Fact. Operación";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 80;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "FacOper");

            oColumn = oColumns.Add("FacResp", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Fact. Resp.";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 80;
            oColumn.RightJustified = false;
            oColumn.DataBind.Bind(NombreDT, "FacResp");

            mtxDet.Clear();
            mtxDet.LoadFromDataSource();
            Comun.FuncionesComunes.LiberarObjetoGenerico(datatable);
            Colorear(11, 30000, "(");
            Colorear(12, 30000, "(");
            Colorear(13, 30000, "(");
            Colorear(14, 30000, "(");
            Numerar(0);
        }

        private static void CargarMatrixDetalle()
        {
            // Filtro por Socio de Negocio
            SAPbouiCOM.EditText oSN = (SAPbouiCOM.EditText)oForm.Items.Item("CodSN").Specific;
            string FiltroSN = oSN.Value;
            if (!string.IsNullOrEmpty(FiltroSN))
            {
                FiltroSN = string.Format(@" AND T0.""ShortName"" = '{0}'", FiltroSN);
            }
            else
            {
                FiltroSN = string.Empty;
            }

            // Filtro por Fechas
            string FechaInicial = string.Empty;
            string FechaFinal = string.Empty;
            DateTime dt;
            string Mes = string.Empty;
            string Dia = string.Empty;

            // Fecha de vencimiento
            SAPbouiCOM.EditText oFDesde = (SAPbouiCOM.EditText)oForm.Items.Item("FDesde").Specific;
            string DesdeFecha = oFDesde.Value;
            SAPbouiCOM.EditText oFHasta = (SAPbouiCOM.EditText)oForm.Items.Item("FHasta").Specific;
            string HastaFecha = oFHasta.Value;
            // Fechas en formato AAAA-MM-DD
            if (!string.IsNullOrEmpty(DesdeFecha) && !string.IsNullOrEmpty(HastaFecha))
            {
                FechaInicial = string.Format("{0}{1}{2}", DesdeFecha.Substring(0, 4), DesdeFecha.Substring(4, 2), DesdeFecha.Substring(6, 2));
                FechaFinal = string.Format("{0}{1}{2}", HastaFecha.Substring(0, 4), HastaFecha.Substring(4, 2), HastaFecha.Substring(6, 2));
            }
            else
            {
                // Por defecto trae desde el día 1ro del mes
                dt = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
                Mes = (dt.Month.ToString().Length == 1) ? dt.Month.ToString().PadLeft(2, '0') : dt.Month.ToString();
                Dia = "01";

                FechaInicial = string.Format("{0}{1}{2}", dt.Year.ToString(), Mes, Dia);

                dt = DateTime.Today;
                Mes = (dt.Month.ToString().Length == 1) ? dt.Month.ToString().PadLeft(2, '0') : dt.Month.ToString();
                Dia = (dt.Day.ToString().Length == 1) ? dt.Day.ToString().PadLeft(2, '0') : dt.Day.ToString();

                FechaFinal = string.Format("{0}{1}{2}", dt.Year.ToString(), Mes, Dia);
            }
            string FiltroFecha = string.Format(@" AND T0.""RefDate"" BETWEEN '{0}' AND '{1}'", FechaInicial, FechaFinal);

            // Filtro ver sólo operaciones no reconciliadas
            SAPbouiCOM.CheckBox oNoRec = (SAPbouiCOM.CheckBox)oForm.Items.Item("NoRec").Specific;
            string FiltroNoRec = string.Empty;
            if (oNoRec.Checked)
            {
                FiltroNoRec = string.Format(@" AND (T0.""BalDueDeb"" <> {0}  OR  T0.""BalDueCred"" <> {0}  OR  T0.""BalFcDeb"" <> {0}  OR  T0.""BalFcCred"" <> {0})", 0);
            }
            else
            {
                FiltroNoRec = string.Empty;
            }

            string NombreDT = "dtDet";
            SAPbouiCOM.DataTable datatable = oForm.DataSources.DataTables.Item(NombreDT);

            string _query = @"
                SELECT 
                000000 AS ""Num"",
                CASE 	
                WHEN T0.""TransType"" = -2  THEN 30 
                WHEN T0.""TransType"" = -3  THEN 30 
                ELSE T0.""TransType""
                END AS ""ObjType"",
                CASE 	
                WHEN T0.""TransType"" = -2  THEN 'SI-Saldo Inicial' 
                WHEN T0.""TransType"" = -3  THEN 'CB-Cierre Período' 
                WHEN T0.""TransType"" = 13  THEN 'RF-Factura Deudores' 
                WHEN T0.""TransType"" = 14  THEN 'RC-Nota Credito Clientes'
                WHEN T0.""TransType"" = 15  THEN 'NE-Entrega'
                WHEN T0.""TransType"" = 16  THEN 'DV-Devolucion'
                WHEN T0.""TransType"" = 18  THEN 'TT-Factura Proveedores'
                WHEN T0.""TransType"" = 19  THEN 'PC-Nota Credito Proveedores'
                WHEN T0.""TransType"" = 20  THEN 'EP-Entrada Mercancias'
                WHEN T0.""TransType"" = 21  THEN 'DM-Devolucion Mercancías'
                WHEN T0.""TransType"" = 24  THEN 'PR-Pagos Recibidos'
                WHEN T0.""TransType"" = 30  THEN 'AS-Asiento'
                WHEN T0.""TransType"" = 46  THEN 'PP-Pagos Efectuados'
                WHEN T0.""TransType"" = 59  THEN 'EM-Entrada Mercancías'
                WHEN T0.""TransType"" = 60  THEN 'OA-Emisión para producción'
                WHEN T0.""TransType"" = 67  THEN 'IM-Transferencia de Stock'
                WHEN T0.""TransType"" = 69  THEN 'DI-Precio Entrega'
                WHEN T0.""TransType"" = 162 THEN 'RI-Revalorización Inventario'
                WHEN T0.""TransType"" = 202 THEN 'OF-Orden de Fabricación'
                WHEN T0.""TransType"" = 204 THEN 'AN-F Anticipo Proveedores'
                WHEN T0.""TransType"" = 321 THEN 'ID-Reconciliación Interna'
                ELSE T0.""TransType""
                END AS ""Origen"",
                T0.""TransId"", T0.""Line_ID"", T0.""Account"", T0.""Debit"", T0.""Credit"", T0.""SYSCred"", T0.""SYSDeb"", T0.""FCDebit"", T0.""FCCredit"",
                T0.""FCCurrency"", T0.""DueDate"", T0.""SourceID"", T0.""SourceLine"", T0.""ShortName"", T0.""IntrnMatch"", T0.""ExtrMatch"", T0.""ContraAct"",
                T0.""LineMemo"", T0.""Ref3Line"", T0.""TransType"", T0.""RefDate"", T0.""Ref2Date"", T0.""Ref1"", T0.""Ref2"", T0.""CreatedBy"", T0.""BaseRef"",
                T0.""Project"", T0.""TransCode"", T0.""ProfitCode"", T0.""TaxDate"", T0.""SystemRate"", T0.""MthDate"", T0.""ToMthSum"", T0.""UserSign"",
                T0.""BatchNum"", T0.""FinncPriod"", T0.""RelTransId"", T0.""RelLineID"", T0.""RelType"", T0.""LogInstanc"", T0.""VatGroup"", T0.""BaseSum"",
                T0.""VatRate"", T0.""Indicator"", T0.""AdjTran"", T0.""RevSource"", T0.""ObjType"", T0.""VatDate"", T0.""PaymentRef"", T0.""SYSBaseSum"", 
                T0.""MultMatch"", T0.""VatLine"", T0.""VatAmount"", T0.""SYSVatSum"", T0.""Closed"", T0.""GrossValue"", T0.""CheckAbs"", T0.""LineType"",
                T0.""DebCred"", T0.""SequenceNr"", T0.""StornoAcc"", T0.""BalDueDeb"", T0.""BalDueCred"", T0.""BalFcDeb"", T0.""BalFcCred"", T0.""BalScDeb"",
                T0.""BalScCred"", T0.""IsNet"", T0.""DunWizBlck"", T0.""DunnLevel"", T0.""DunDate"", T0.""TaxType"", T0.""TaxPostAcc"", T0.""StaCode"",
                T0.""StaType"", T0.""TaxCode"", T0.""ValidFrom"", T0.""GrossValFc"", T0.""LvlUpdDate"", T0.""OcrCode2"", T0.""OcrCode3"", T0.""OcrCode4"",
                T0.""OcrCode5"", T0.""MIEntry"", T0.""MIVEntry"", T0.""ClsInTP"", T0.""CenVatCom"", T0.""MatType"", T0.""PstngType"", T0.""ValidFrom2"",
                T0.""ValidFrom3"", T0.""ValidFrom4"", T0.""ValidFrom5"", T0.""Location"", T0.""WTaxCode"", T0.""EquVatRate"", T0.""EquVatSum"", T0.""SYSEquSum"",
                T0.""TotalVat"", T0.""SYSTVat"", T0.""WTLiable"", T0.""WTLine"", T0.""WTApplied"", T0.""WTAppliedS"", T0.""WTAppliedF"", T0.""WTSum"",
                T0.""WTSumFC"", T0.""WTSumSC"", T0.""PayBlock"", T0.""PayBlckRef"", T0.""LicTradNum"", T0.""InterimTyp"", T0.""DprId"", T0.""MatchRef"",
                T0.""Ordered"", T0.""CUP"", T0.""CIG"", T0.""BPLId"", T0.""BPLName"", T0.""VatRegNum"", T0.""SLEDGERF"", T0.""InitRef2"", T0.""InitRef3Ln"",
                T0.""ExpUUID"", T0.""ExpOPType"", T0.""ExTransId"", T0.""DocArr"", T0.""DocLine"",
                CASE
                WHEN T0.""DebCred"" = 'D' THEN ISNULL(T5.""MainCurncy"", '') + ' ' + FORMAT(T0.""Debit"", 'N', CASE WHEN T5.""DecSep"" = ',' THEN 'de-DE' ELSE 'en-US' END)
                WHEN T0.""DebCred"" = 'C' THEN ISNULL(T5.""MainCurncy"", '') + ' (' + FORMAT(T0.""Credit"", 'N', CASE WHEN T5.""DecSep"" = ',' THEN 'de-DE' ELSE 'en-US' END) + ')'
                ELSE ''
                END AS ""SaldoML"",
                CASE
                WHEN T0.""DebCred"" = 'D' THEN T0.""Debit""
                WHEN T0.""DebCred"" = 'C' THEN (T0.""Credit"" * -1)
                ELSE ''
                END AS ""SaldoMLTot"",
                CASE
                WHEN T0.""DebCred"" = 'D' THEN ISNULL(T5.""MainCurncy"", '') + ' ' + FORMAT(T0.""BalDueDeb"", 'N', CASE WHEN T5.""DecSep"" = ',' THEN 'de-DE' ELSE 'en-US' END)
                WHEN T0.""DebCred"" = 'C' THEN ISNULL(T5.""MainCurncy"", '') + ' (' + FORMAT(T0.""BalDueCred"", 'N', CASE WHEN T5.""DecSep"" = ',' THEN 'de-DE' ELSE 'en-US' END) + ')'
                ELSE ''
                END AS ""SaldoVML"",
                CASE
                WHEN T0.""DebCred"" = 'D' AND T0.""FCDebit"" <> 0 THEN ISNULL(T0.""FCCurrency"", '') + ' ' + FORMAT(T0.""FCDebit"", 'N0', CASE WHEN T5.""DecSep"" = ',' THEN 'de-DE' ELSE 'en-US' END)
                WHEN T0.""DebCred"" = 'C' AND T0.""FCCredit"" <> 0 THEN ISNULL(T0.""FCCurrency"", '') + ' (' + FORMAT(T0.""FCCredit"", 'N0', CASE WHEN T5.""DecSep"" = ',' THEN 'de-DE' ELSE 'en-US' END) + ')'
                ELSE ''
                END AS ""SaldoME"",
                CASE
                WHEN T0.""DebCred"" = 'D' THEN T0.""FCDebit""
                WHEN T0.""DebCred"" = 'C' THEN (T0.""FCCredit"" * -1)
                ELSE ''
                END AS ""SaldoMETot"",
                CASE
                WHEN T0.""DebCred"" = 'D' AND T0.""FCDebit"" <> 0 THEN ISNULL(T0.""FCCurrency"", '') + ' ' + FORMAT(T0.""BalFcDeb"", 'N0', CASE WHEN T5.""DecSep"" = ',' THEN 'de-DE' ELSE 'en-US' END)
                WHEN T0.""DebCred"" = 'C' AND T0.""FCCredit"" <> 0 THEN ISNULL(T0.""FCCurrency"", '') + ' (' + FORMAT(T0.""BalFcCred"", 'N0', CASE WHEN T5.""DecSep"" = ',' THEN 'de-DE' ELSE 'en-US' END) + ')'
                ELSE ''
                END AS ""SaldoVME"",
                CASE
                WHEN T0.""DebCred"" = 'D' THEN ISNULL(T5.""MainCurncy"", '') + ' ' + FORMAT(T0.""Debit"", 'N', CASE WHEN T5.""DecSep"" = ',' THEN 'de-DE' ELSE 'en-US' END)
                ELSE ''
                END AS ""CargoML"",
                CASE
                WHEN T0.""DebCred"" = 'C' THEN ISNULL(T5.""MainCurncy"", '') + ' ' + FORMAT(T0.""Credit"", 'N', CASE WHEN T5.""DecSep"" = ',' THEN 'de-DE' ELSE 'en-US' END)
                ELSE ''
                END AS ""AbonoML"",
                T1.""USER_CODE"", T2.""MaxReconNum"", T3.""AgrNo"",
                CASE
                WHEN T3.""FolioNum"" > 0 THEN T3.""FolioPref"" + '-' + CONVERT(NVARCHAR(20), T3.""FolioNum"")
                ELSE ''
                END AS ""Folio"",
                T4.""Number"",
                T5.""MainCurncy"",
                FCRNG.""U_FACTORINGID"" AS ""FacID"", FCRNG.""U_INSTITU"" AS ""FacEntidad"", FCRNG.""U_FACTORINGVCTO"" AS ""FacFecha"", FCRNG.""U_NUMOPER"" AS ""FacOper"", FCRNG.""U_RESFAC"" AS ""FacResp""
                FROM ""JDT1"" T0
                LEFT OUTER JOIN ""OUSR"" T1 ON T1.""USERID"" = T0.""UserSign""    
                LEFT OUTER JOIN 
                (SELECT T0.""TransId"" AS ""TransId"", T0.""TransRowId"" AS ""TransRowId"", MAX(T0.""ReconNum"") AS ""MaxReconNum"" FROM ""ITR1"" T0 GROUP BY T0.""TransId"", T0.""TransRowId"")
                T2 ON T2.""TransId"" = T0.""TransId"" AND T2.""TransRowId"" = T0.""Line_ID""   
                INNER JOIN ""OJDT"" T3 ON T3.""TransId"" = T0.""TransId""    
                LEFT OUTER JOIN ""OOAT"" T4 ON T4.""AbsID"" = T3.""AgrNo""   
                INNER JOIN ""OADM"" T5 ON 1 = 1
                LEFT OUTER JOIN (
                SELECT ""DocEntry"", ""DocNum"", ""TransId"", ""U_FACTORINGID"", ""U_INSTITU"", ""U_FACTORINGVCTO"", ""U_NUMOPER"", ""U_RESFAC"" FROM OINV 
                UNION ALL
                SELECT ""DocEntry"", ""DocNum"", ""TransId"", ""U_FACTORINGID"", ""U_INSTITU"", ""U_FACTORINGVCTO"", ""U_NUMOPER"", ""U_RESFAC"" FROM ORIN
                ) FCRNG ON T0.""TransId"" = FCRNG.""TransId""
                WHERE 1 = 1" + FiltroSN + FiltroNoRec + FiltroFecha;

            _query += @" ORDER BY T0.[RefDate] ASC,T0.[TransId] ASC,T0.[Line_ID] ASC";
            datatable.ExecuteQuery(_query);
            oForm.Freeze(true);
            mtxDet.Clear();
            mtxDet.LoadFromDataSource();
            Comun.FuncionesComunes.LiberarObjetoGenerico(datatable);
            Colorear(11, 30000, "(");
            Colorear(12, 30000, "(");
            Colorear(13, 30000, "(");
            Colorear(14, 30000, "(");
            Numerar(0);
            SumarTotales();
            oForm.Freeze(false);
        }

        private static void Colorear(int colNum, int rgbColor, string condicion)
        {
            if (mtxDet.VisualRowCount > 0)
            {
                for (int i = 1; i <= mtxDet.VisualRowCount; i++)
                {
                    SAPbouiCOM.EditText Text = (SAPbouiCOM.EditText)mtxDet.Columns.Item(colNum).Cells.Item(i).Specific;
                    string valor = Text.String;
                    if (valor.IndexOf(condicion) > 0)
                    {
                        mtxDet.CommonSetting.SetCellFontColor(i, colNum, rgbColor);
                    }
                }
            }
            oForm.Update();
        }

        private static void Numerar(int colNum)
        {
            if (mtxDet.VisualRowCount > 0)
            {
                for (int i = 1; i <= mtxDet.VisualRowCount; i++)
                {
                    SAPbouiCOM.EditText Text = (SAPbouiCOM.EditText)mtxDet.Columns.Item(colNum).Cells.Item(i).Specific;
                    Text.String = i.ToString();
                }
            }
            oForm.Update();
        }

        private static void SumarTotales()
        {
            string NombreDT = "dtDet";
            SAPbouiCOM.DataTable datatable = oForm.DataSources.DataTables.Item(NombreDT);
            double TotML = 0;
            double TotME = 0;
            if (!datatable.IsEmpty)
            {
                for (int i = 0; i < datatable.Rows.Count; i++)
                {
                    TotML += (double)datatable.GetValue("SaldoMLTot", i);
                    TotME += (double)datatable.GetValue("SaldoMETot", i);
                }
            }
            Comun.FuncionesComunes.LiberarObjetoGenerico(datatable);
            oForm.DataSources.UserDataSources.Item("TotML").Value = TotML.ToString();
            oForm.DataSources.UserDataSources.Item("TotME").Value = TotME.ToString();
        }
    }
}
