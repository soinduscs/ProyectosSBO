using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM.Framework;

namespace Soindus.AddOnFactoringCob.Formularios
{
    [FormAttribute("Soindus.AddOnFactoringCob.Formularios.frmCobGestion", "Formularios/frmCobGestion.b1f")]
    class frmCobGestion : UserFormBase
    {
        // Objetos de formulario
        #region Objetos de formulario
        private static SAPbouiCOM.Form oForm;
        private static SAPbouiCOM.EditText txtCodSN;
        private static SAPbouiCOM.EditText txtNomSN;
        private static SAPbouiCOM.EditText txtFVcto;
        private static SAPbouiCOM.Button btnFiltrar;
        private static SAPbouiCOM.Button btnMarcar;
        private static SAPbouiCOM.Button btnSelect;
        private static SAPbouiCOM.Matrix mtxDet;
        private static List<string> Clientes = new List<string>();
        private static string Vencimiento = DateTime.Now.Date.ToString("yyyyMMdd");
        #endregion

        public frmCobGestion()
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
                            if (pVal.ColUID.Equals("CardCode"))
                            {
                                SAPbouiCOM.Column oColumn = (SAPbouiCOM.Column)mtxDet.Columns.Item(pVal.ColUID);
                                SAPbouiCOM.LinkedButton oLink = ((SAPbouiCOM.LinkedButton)(oColumn.ExtendedObject));
                                oLink.LinkedObjectType = "2";
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
                            CargarMatrixDetalle();
                        }
                        #endregion

                        // Boton marcar
                        #region marcar
                        if (pVal.ItemUID.Equals("btnMarcar"))
                        {
                            // Documentos
                            string NombreDT = "dtDet";
                            SAPbouiCOM.DataTable datatable = oForm.DataSources.DataTables.Item(NombreDT);
                            MarcarRegistrosMatrix(mtxDet, datatable, "Chk");
                            Comun.FuncionesComunes.LiberarObjetoGenerico(datatable);
                        }
                        #endregion

                        // Botón Cobranza
                        #region Cobranza
                        if (pVal.ItemUID.Equals("btnSelect"))
                        {
                            string NombreDT = "dtDet";
                            SAPbouiCOM.DataTable datatable = oForm.DataSources.DataTables.Item(NombreDT);
                            CargarClientes(mtxDet, datatable, "Chk");
                            Comun.FuncionesComunes.LiberarObjetoGenerico(datatable);
                            if (Clientes.Count > 0)
                            {
                                try
                                {
                                    Application.SBO_Application.Forms.Item("frmCobSelect").Close();
                                }
                                catch
                                {
                                }
                                Formularios.frmCobSelect activeForm = new Formularios.frmCobSelect(Clientes, Vencimiento);
                                activeForm.Show();
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
            oForm = ((SAPbouiCOM.Form)(Application.SBO_Application.Forms.Item("frmCobGestion")));
            txtCodSN = ((SAPbouiCOM.EditText)(GetItem("CodSN").Specific));
            txtNomSN = ((SAPbouiCOM.EditText)(GetItem("NomSN").Specific));
            txtFVcto = ((SAPbouiCOM.EditText)(GetItem("FVcto").Specific));
            btnFiltrar = ((SAPbouiCOM.Button)(GetItem("btnFiltrar").Specific));
            btnMarcar = ((SAPbouiCOM.Button)(GetItem("btnMarcar").Specific));
            btnSelect = ((SAPbouiCOM.Button)(GetItem("btnSelect").Specific));
            mtxDet = ((SAPbouiCOM.Matrix)(GetItem("mtxDet").Specific));
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

            var grupos = SBO.ConsultasSBO.ObtenerGroupCodesSN();
            if (grupos.Count > 0)
            {
                oCon.Relationship = SAPbouiCOM.BoConditionRelationship.cr_AND;

                oCon = oCons.Add();
                oCon.BracketOpenNum = 1;
                oCon.Alias = "GroupCode";
                oCon.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
                oCon.CondVal = "";
                foreach (var item in grupos)
                {
                    oCon.Relationship = SAPbouiCOM.BoConditionRelationship.cr_OR;
                    oCon = oCons.Add();
                    oCon.Alias = "GroupCode";
                    oCon.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
                    oCon.CondVal = item;
                }
                oCon.BracketCloseNum = 1;
            }

            oCFL.SetConditions(oCons);

            //Asignamos el ChoosefromList al campo de texto
            txtCodSN.ChooseFromListUID = "cflSN";
            txtCodSN.ChooseFromListAlias = "CardCode";

            // Fecha vencimiento
            oForm.DataSources.UserDataSources.Add("FVcto", SAPbouiCOM.BoDataType.dt_DATE);
            txtFVcto.DataBind.SetBound(true, "", "FVcto");

            oForm.DataSources.DataTables.Add("dtDet");

            EstructuraMatrixDetalle();

            mtxDet.SelectionMode = SAPbouiCOM.BoMatrixSelect.ms_Single;
            oForm.Visible = true;
        }

        private static void EstructuraMatrixDetalle()
        {
            string FiltroGrupoSN = string.Empty;
            string joined = string.Empty;
            var grupos = SBO.ConsultasSBO.ObtenerGroupCodesSN();
            if (grupos.Count > 0)
            {
                FiltroGrupoSN = @" AND  T4.""GroupCode"" IN (";
                joined = string.Join(",", grupos);
                FiltroGrupoSN += @joined + @")";
            }

            string FechaVcto = DateTime.Now.Date.ToString("yyyyMMdd");
            string NombreDT = "dtDet";
            SAPbouiCOM.DataTable datatable = oForm.DataSources.DataTables.Item(NombreDT);
            string _query = @"
                SELECT 
                'N' AS ""Chk"", ""CardCode"", MAX(""CardName"") AS ""CardName"", MAX(""Currency"") AS ""Currency"", MAX(""FCCurrency"") AS ""FCCurrency"", MAX(""BalanceVis"") AS ""BalanceVis"", SUM(""SaldoVenc"") AS ""SaldoVenc"", SUM(""AbonoFut"") AS ""AbonoFut"", SUM(""Saldo30"") AS ""Saldo30"", SUM(""Saldo60"") AS ""Saldo60"", SUM(""Saldo90"") AS ""Saldo90"", SUM(""Saldo120"") AS ""Saldo120"", SUM(""Saldo121"") AS ""Saldo121""
                FROM (
                SELECT 'N' AS ""Chk"", T0.""TransId"", T0.""Line_ID"", MAX(T0.""Account"") AS ""Account"", MAX(T0.""ShortName"") AS ""ShortName"",
                CASE 	
                WHEN MAX(T0.""TransType"") = -2  THEN 30 
                WHEN MAX(T0.""TransType"") = -3  THEN 30 
                ELSE MAX(T0.""TransType"")
                END AS ""TransType"",
                CASE 	
                WHEN MAX(T0.""TransType"") = -2  THEN 'SI-Saldo Inicial' 
                WHEN MAX(T0.""TransType"") = -3  THEN 'CB-Cierre Período' 
                WHEN MAX(T0.""TransType"") = 13  THEN 'RF-Factura Deudores' 
                WHEN MAX(T0.""TransType"") = 14  THEN 'RC-Nota Credito Clientes'
                WHEN MAX(T0.""TransType"") = 15  THEN 'NE-Entrega'
                WHEN MAX(T0.""TransType"") = 16  THEN 'DV-Devolucion'
                WHEN MAX(T0.""TransType"") = 18  THEN 'TT-Factura Proveedores'
                WHEN MAX(T0.""TransType"") = 19  THEN 'PC-Nota Credito Proveedores'
                WHEN MAX(T0.""TransType"") = 20  THEN 'EP-Entrada Mercancias'
                WHEN MAX(T0.""TransType"") = 21  THEN 'DM-Devolucion Mercancías'
                WHEN MAX(T0.""TransType"") = 24  THEN 'PR-Pagos Recibidos'
                WHEN MAX(T0.""TransType"") = 30  THEN 'AS-Asiento'
                WHEN MAX(T0.""TransType"") = 46  THEN 'PP-Pagos Efectuados'
                WHEN MAX(T0.""TransType"") = 59  THEN 'EM-Entrada Mercancías'
                WHEN MAX(T0.""TransType"") = 60  THEN 'OA-Emisión para producción'
                WHEN MAX(T0.""TransType"") = 67  THEN 'IM-Transferencia de Stock'
                WHEN MAX(T0.""TransType"") = 69  THEN 'DI-Precio Entrega'
                WHEN MAX(T0.""TransType"") = 162 THEN 'RI-Revalorización Inventario'
                WHEN MAX(T0.""TransType"") = 202 THEN 'OF-Orden de Fabricación'
                WHEN MAX(T0.""TransType"") = 204 THEN 'AN-F Anticipo Proveedores'
                WHEN MAX(T0.""TransType"") = 321 THEN 'ID-Reconciliación Interna'
                ELSE MAX(T0.""TransType"")
                END AS ""Origen"",
                MAX(T0.""CreatedBy"") AS ""CreatedBy"", MAX(T0.""BaseRef"") AS ""BaseRef"", 
                MAX(T0.""SourceLine"") AS ""SourceLine"", MAX(T0.""RefDate"") AS ""RefDate"", MAX(T0.""DueDate"") AS ""DueDate"", MAX(T0.""TaxDate"") AS ""TaxDate"", MAX(T0.""BalDueCred"") + SUM(T1.""ReconSum"") AS ""Saldo"", MAX(T0.""BalFcCred"") 
                + SUM(T1.""ReconSumFC"") AS ""SaldoFC"", MAX(T0.""BalScCred"") + SUM(T1.""ReconSumSC"") AS ""SaldoSC"", MAX(T0.""LineMemo"") AS ""LineMemo"", MAX(T3.""FolioPref"") AS ""FolioPref"", MAX(T3.""FolioNum"") AS ""FolioNum"", MAX(T0.""Indicator"") AS ""Indicator"", 
                MAX(T4.""CardName"") AS ""CardName"", MAX(T5.""CardCode"") AS ""CardCode5"", MAX(T5.""CardName"") AS ""CardName5"", MAX(T4.""Balance"") AS ""Balance"", MAX(T5.""NumAtCard"") AS ""NumAtCard"", MAX(T5.""SlpCode"") AS ""SlpCode"", MAX(T0.""Project"") AS ""Project"", 
                MAX(T0.""Debit"") - MAX(T0.""Credit"") AS ""Saldo2"", MAX(T0.""FCDebit"") - MAX(T0.""FCCredit"") AS ""SaldoFC2"", MAX(T0.""SYSDeb"") - MAX(T0.""SYSCred"") AS ""SaldoSC2"", MAX(T4.""PymCode"") AS ""PymCode"", 
                MAX(T5.""BlockDunn"") AS ""BlockDunn"", MAX(T5.""DunnLevel"") AS ""DunnLevel"", MAX(T5.""TransType"") AS ""TransType5"", MAX(T5.""IsSales"") AS ""IsSales"", MAX(T4.""Currency"") AS ""Currency"", MAX(T0.""FCCurrency"") AS ""FCCurrency"", T0.""TransId"" AS ""TransId2"", 
                MAX(T4.""DunTerm"") AS ""DunTerm"", MAX(T0.""DunnLevel"") AS ""DunnLevel2"", T0.""BPLName"" AS ""BPLName"", MAX(T4.""ConnBP"") AS ""ConnBP"", MAX(T4.""CardCode"") AS ""CardCode"", T0.""TransId"" AS ""TransId3"", MAX(T3.""AgrNo"") AS ""AgrNo"",
                CASE WHEN MAX(T4.""Currency"") = MAX(TA.""MainCurncy"") THEN MAX(T4.""Balance"") ELSE MAX(T4.""BalanceFC"") END AS ""BalanceVis"",
                /*
                CASE WHEN MAX(T0.""DueDate"") < '" + FechaVcto + @"' THEN CASE WHEN MAX(T4.""Currency"") = MAX(TA.""MainCurncy"") THEN MAX(T0.""Debit"") - MAX(T0.""Credit"") ELSE MAX(T0.""FCDebit"") - MAX(T0.""FCCredit"") END ELSE 0 END AS ""SaldoVenc"",
                CASE WHEN MAX(T0.""DueDate"") >= '" + FechaVcto + @"' THEN CASE WHEN MAX(T4.""Currency"") = MAX(TA.""MainCurncy"") THEN MAX(T0.""Debit"") - MAX(T0.""Credit"") ELSE MAX(T0.""FCDebit"") - MAX(T0.""FCCredit"") END ELSE 0 END AS ""AbonoFut"",
                CASE WHEN MAX(T0.""DueDate"") < '" + FechaVcto + @"' AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') >= 0 AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') <= 30 THEN CASE WHEN MAX(T4.""Currency"") = MAX(TA.""MainCurncy"") THEN MAX(T0.""Debit"") - MAX(T0.""Credit"") ELSE MAX(T0.""FCDebit"") - MAX(T0.""FCCredit"") END ELSE 0 END AS ""Saldo30"",
                CASE WHEN MAX(T0.""DueDate"") < '" + FechaVcto + @"' AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') >= 31 AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') <= 60 THEN CASE WHEN MAX(T4.""Currency"") = MAX(TA.""MainCurncy"") THEN MAX(T0.""Debit"") - MAX(T0.""Credit"") ELSE MAX(T0.""FCDebit"") - MAX(T0.""FCCredit"") END ELSE 0 END AS ""Saldo60"",
                CASE WHEN MAX(T0.""DueDate"") < '" + FechaVcto + @"' AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') >= 61 AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') <= 90 THEN CASE WHEN MAX(T4.""Currency"") = MAX(TA.""MainCurncy"") THEN MAX(T0.""Debit"") - MAX(T0.""Credit"") ELSE MAX(T0.""FCDebit"") - MAX(T0.""FCCredit"") END ELSE 0 END AS ""Saldo90"",
                CASE WHEN MAX(T0.""DueDate"") < '" + FechaVcto + @"' AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') >= 91 AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') <= 120 THEN CASE WHEN MAX(T4.""Currency"") = MAX(TA.""MainCurncy"") THEN MAX(T0.""Debit"") - MAX(T0.""Credit"") ELSE MAX(T0.""FCDebit"") - MAX(T0.""FCCredit"") END ELSE 0 END AS ""Saldo120"",
                CASE WHEN MAX(T0.""DueDate"") < '" + FechaVcto + @"' AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') >= 121 THEN CASE WHEN MAX(T4.""Currency"") = MAX(TA.""MainCurncy"") THEN MAX(T0.""Debit"") - MAX(T0.""Credit"") ELSE MAX(T0.""FCDebit"") - MAX(T0.""FCCredit"") END ELSE 0 END AS ""Saldo121""
                */
                CASE WHEN MAX(T0.""DueDate"") < '" + FechaVcto + @"' THEN CASE WHEN MAX(T4.""Currency"") = MAX(TA.""MainCurncy"") THEN (MAX(T0.""BalDueCred"") + SUM(T1.""ReconSum"")) * -1 ELSE (MAX(T0.""BalFcCred"") + SUM(T1.""ReconSumFC"")) * -1 END ELSE 0 END AS ""SaldoVenc"",
                CASE WHEN MAX(T0.""DueDate"") >= '" + FechaVcto + @"' THEN CASE WHEN MAX(T4.""Currency"") = MAX(TA.""MainCurncy"") THEN (MAX(T0.""BalDueCred"") + SUM(T1.""ReconSum"")) * -1 ELSE (MAX(T0.""BalFcCred"") + SUM(T1.""ReconSumFC"")) * -1 END ELSE 0 END AS ""AbonoFut"",
                CASE WHEN MAX(T0.""DueDate"") < '" + FechaVcto + @"' AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') >= 0 AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') <= 30 THEN CASE WHEN MAX(T4.""Currency"") = MAX(TA.""MainCurncy"") THEN (MAX(T0.""BalDueCred"") + SUM(T1.""ReconSum"")) * -1 ELSE (MAX(T0.""BalFcCred"") + SUM(T1.""ReconSumFC"")) * -1 END ELSE 0 END AS ""Saldo30"",
                CASE WHEN MAX(T0.""DueDate"") < '" + FechaVcto + @"' AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') >= 31 AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') <= 60 THEN CASE WHEN MAX(T4.""Currency"") = MAX(TA.""MainCurncy"") THEN (MAX(T0.""BalDueCred"") + SUM(T1.""ReconSum"")) * -1 ELSE (MAX(T0.""BalFcCred"") + SUM(T1.""ReconSumFC"")) * -1 END ELSE 0 END AS ""Saldo60"",
                CASE WHEN MAX(T0.""DueDate"") < '" + FechaVcto + @"' AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') >= 61 AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') <= 90 THEN CASE WHEN MAX(T4.""Currency"") = MAX(TA.""MainCurncy"") THEN (MAX(T0.""BalDueCred"") + SUM(T1.""ReconSum"")) * -1 ELSE (MAX(T0.""BalFcCred"") + SUM(T1.""ReconSumFC"")) * -1 END ELSE 0 END AS ""Saldo90"",
                CASE WHEN MAX(T0.""DueDate"") < '" + FechaVcto + @"' AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') >= 91 AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') <= 120 THEN CASE WHEN MAX(T4.""Currency"") = MAX(TA.""MainCurncy"") THEN (MAX(T0.""BalDueCred"") + SUM(T1.""ReconSum"")) * -1 ELSE (MAX(T0.""BalFcCred"") + SUM(T1.""ReconSumFC"")) * -1 END ELSE 0 END AS ""Saldo120"",
                CASE WHEN MAX(T0.""DueDate"") < '" + FechaVcto + @"' AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') >= 121 THEN CASE WHEN MAX(T4.""Currency"") = MAX(TA.""MainCurncy"") THEN (MAX(T0.""BalDueCred"") + SUM(T1.""ReconSum"")) * -1 ELSE (MAX(T0.""BalFcCred"") + SUM(T1.""ReconSumFC"")) * -1 END ELSE 0 END AS ""Saldo121""
                FROM  ""JDT1"" T0  
                INNER  JOIN ""ITR1"" T1  ON  T1.""TransId"" = T0.""TransId""  AND  T1.""TransRowId"" = T0.""Line_ID""   
                INNER  JOIN ""OITR"" T2  ON  T2.""ReconNum"" = T1.""ReconNum""   
                INNER  JOIN ""OJDT"" T3  ON  T3.""TransId"" = T0.""TransId""   
                INNER  JOIN ""OCRD"" T4  ON  T4.""CardCode"" = T0.""ShortName""    
                LEFT OUTER  JOIN ""B1_JournalTransSourceView"" T5  
                ON  T5.""ObjType"" = T0.""TransType""  AND  T5.""DocEntry"" = T0.""CreatedBy""  
                AND  (T5.""TransType"" <> 'I'  OR  (T5.""TransType"" = 'I'  
                AND  T5.""InstlmntID"" = T0.""SourceLine"" ))  
				INNER JOIN ""OADM"" TA ON 1 = 1
                WHERE T0.""RefDate"" <= '" + FechaVcto + @"'  
                AND  T4.""CardType"" = 'C'  AND  T4.""Balance"" <> 0 
                " + FiltroGrupoSN + @" 
                AND  T2.""ReconDate"" > '" + FechaVcto + @"'  AND  T1.""IsCredit"" = 'C'   
                AND 1 = 0
                GROUP BY T0.""TransId"", T0.""Line_ID"", T0.""BPLName"" 
                HAVING MAX(T0.""BalFcCred"") <>- SUM(T1.""ReconSumFC"")  OR  MAX(T0.""BalDueCred"") <>- SUM(T1.""ReconSum"")  
                ) T10
                GROUP BY ""CardCode""
                ";

            datatable.ExecuteQuery(_query);

            SAPbouiCOM.Columns oColumns;
            oColumns = mtxDet.Columns;
            SAPbouiCOM.Column oColumn;

            oColumn = oColumns.Add("Chk", SAPbouiCOM.BoFormItemTypes.it_CHECK_BOX);
            oColumn.TitleObject.Caption = "#";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = true;
            oColumn.Visible = true;
            oColumn.Width = 15;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "Chk");

            oColumn = oColumns.Add("CardCode", SAPbouiCOM.BoFormItemTypes.it_LINKED_BUTTON);
            oColumn.TitleObject.Caption = "Cliente";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 100;
            oColumn.RightJustified = false;
            oColumn.DataBind.Bind(NombreDT, "CardCode");

            oColumn = oColumns.Add("CardName", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Nombre";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 180;
            oColumn.RightJustified = false;
            oColumn.DataBind.Bind(NombreDT, "CardName");

            oColumn = oColumns.Add("Currency", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Moneda";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 40;
            oColumn.RightJustified = false;
            oColumn.DataBind.Bind(NombreDT, "Currency");

            oColumn = oColumns.Add("BalanceVis", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Saldo total";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 100;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "BalanceVis");

            oColumn = oColumns.Add("AbonoFut", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Por vencer";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 100;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "AbonoFut");

            oColumn = oColumns.Add("SaldoVenc", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Saldo vencido";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 100;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "SaldoVenc");

            oColumn = oColumns.Add("Saldo30", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "0 - 30";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 100;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "Saldo30");

            oColumn = oColumns.Add("Saldo60", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "31 - 60";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 100;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "Saldo60");

            oColumn = oColumns.Add("Saldo90", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "61 - 90";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 100;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "Saldo90");

            oColumn = oColumns.Add("Saldo120", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "91 - 120";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 100;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "Saldo120");

            oColumn = oColumns.Add("Saldo121", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "121+";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 100;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "Saldo121");

            mtxDet.Clear();
            mtxDet.LoadFromDataSource();
            Comun.FuncionesComunes.LiberarObjetoGenerico(datatable);
        }

        private static void CargarMatrixDetalle()
        {
            string FiltroGrupoSN = string.Empty;
            string FiltroGrupoSN2 = string.Empty;
            string joined = string.Empty;
            var grupos = SBO.ConsultasSBO.ObtenerGroupCodesSN();
            if (grupos.Count > 0)
            {
                FiltroGrupoSN = @" AND  T4.""GroupCode"" IN (";
                FiltroGrupoSN2 = @" AND  T2.""GroupCode"" IN (";
                joined = string.Join(",", grupos);
                FiltroGrupoSN += @joined + @")";
                FiltroGrupoSN2 += @joined + @")";
            }

            // Filtro por Socio de Negocio
            SAPbouiCOM.EditText oSN = (SAPbouiCOM.EditText)oForm.Items.Item("CodSN").Specific;
            string FiltroSN = oSN.Value;
            string FiltroSN2 = string.Empty;
            if (!string.IsNullOrEmpty(FiltroSN))
            {
                FiltroSN = string.Format(@" AND  T4.""CardCode"" >= '{0}'  AND  T4.""CardCode"" <= '{0}'", oSN.Value);
                FiltroSN2 = string.Format(@" AND  T2.""CardCode"" >= '{0}'  AND  T2.""CardCode"" <= '{0}'", oSN.Value);
            }
            else
            {
                FiltroSN = string.Empty;
                FiltroSN2 = string.Empty;
            }

            // Filtro por Fechas
            string FechaVcto = string.Empty;
            DateTime dt;

            // Fecha de vencimiento
            SAPbouiCOM.EditText oFVcto = (SAPbouiCOM.EditText)oForm.Items.Item("FVcto").Specific;
            string VctoFecha = oFVcto.Value;
            // Fechas en formato AAAA-MM-DD
            if (!string.IsNullOrEmpty(VctoFecha))
            {
                FechaVcto = string.Format("{0}{1}{2}", VctoFecha.Substring(0, 4), VctoFecha.Substring(4, 2), VctoFecha.Substring(6, 2));
            }
            else
            {
                // Por defecto trae fecha de hoy
                dt = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
                FechaVcto = dt.ToString("yyyyMMdd");
            }
            Vencimiento = FechaVcto;

            string NombreDT = "dtDet";
            SAPbouiCOM.DataTable datatable = oForm.DataSources.DataTables.Item(NombreDT);

            string _query = @"
                SELECT 
                'N' AS ""Chk"", ""CardCode"", MAX(""CardName"") AS ""CardName"", MAX(""Currency"") AS ""Currency"", MAX(""FCCurrency"") AS ""FCCurrency"", MAX(""BalanceVis"") AS ""BalanceVis"", SUM(""SaldoVenc"") AS ""SaldoVenc"", SUM(""AbonoFut"") AS ""AbonoFut"", SUM(""Saldo30"") AS ""Saldo30"", SUM(""Saldo60"") AS ""Saldo60"", SUM(""Saldo90"") AS ""Saldo90"", SUM(""Saldo120"") AS ""Saldo120"", SUM(""Saldo121"") AS ""Saldo121""
                FROM (
                SELECT 'N' AS ""Chk"", T0.""TransId"", T0.""Line_ID"", MAX(T0.""Account"") AS ""Account"", MAX(T0.""ShortName"") AS ""ShortName"",
                CASE 	
                WHEN MAX(T0.""TransType"") = -2  THEN 30 
                WHEN MAX(T0.""TransType"") = -3  THEN 30 
                ELSE MAX(T0.""TransType"")
                END AS ""TransType"",
                CASE 	
                WHEN MAX(T0.""TransType"") = -2  THEN 'SI-Saldo Inicial' 
                WHEN MAX(T0.""TransType"") = -3  THEN 'CB-Cierre Período' 
                WHEN MAX(T0.""TransType"") = 13  THEN 'RF-Factura Deudores' 
                WHEN MAX(T0.""TransType"") = 14  THEN 'RC-Nota Credito Clientes'
                WHEN MAX(T0.""TransType"") = 15  THEN 'NE-Entrega'
                WHEN MAX(T0.""TransType"") = 16  THEN 'DV-Devolucion'
                WHEN MAX(T0.""TransType"") = 18  THEN 'TT-Factura Proveedores'
                WHEN MAX(T0.""TransType"") = 19  THEN 'PC-Nota Credito Proveedores'
                WHEN MAX(T0.""TransType"") = 20  THEN 'EP-Entrada Mercancias'
                WHEN MAX(T0.""TransType"") = 21  THEN 'DM-Devolucion Mercancías'
                WHEN MAX(T0.""TransType"") = 24  THEN 'PR-Pagos Recibidos'
                WHEN MAX(T0.""TransType"") = 30  THEN 'AS-Asiento'
                WHEN MAX(T0.""TransType"") = 46  THEN 'PP-Pagos Efectuados'
                WHEN MAX(T0.""TransType"") = 59  THEN 'EM-Entrada Mercancías'
                WHEN MAX(T0.""TransType"") = 60  THEN 'OA-Emisión para producción'
                WHEN MAX(T0.""TransType"") = 67  THEN 'IM-Transferencia de Stock'
                WHEN MAX(T0.""TransType"") = 69  THEN 'DI-Precio Entrega'
                WHEN MAX(T0.""TransType"") = 162 THEN 'RI-Revalorización Inventario'
                WHEN MAX(T0.""TransType"") = 202 THEN 'OF-Orden de Fabricación'
                WHEN MAX(T0.""TransType"") = 204 THEN 'AN-F Anticipo Proveedores'
                WHEN MAX(T0.""TransType"") = 321 THEN 'ID-Reconciliación Interna'
                ELSE MAX(T0.""TransType"")
                END AS ""Origen"",
                MAX(T0.""CreatedBy"") AS ""CreatedBy"", MAX(T0.""BaseRef"") AS ""BaseRef"", 
                MAX(T0.""SourceLine"") AS ""SourceLine"", MAX(T0.""RefDate"") AS ""RefDate"", MAX(T0.""DueDate"") AS ""DueDate"", MAX(T0.""TaxDate"") AS ""TaxDate"", MAX(T0.""BalDueCred"") + SUM(T1.""ReconSum"") AS ""Saldo"", MAX(T0.""BalFcCred"") 
                + SUM(T1.""ReconSumFC"") AS ""SaldoFC"", MAX(T0.""BalScCred"") + SUM(T1.""ReconSumSC"") AS ""SaldoSC"", MAX(T0.""LineMemo"") AS ""LineMemo"", MAX(T3.""FolioPref"") AS ""FolioPref"", MAX(T3.""FolioNum"") AS ""FolioNum"", MAX(T0.""Indicator"") AS ""Indicator"", 
                MAX(T4.""CardName"") AS ""CardName"", MAX(T5.""CardCode"") AS ""CardCode5"", MAX(T5.""CardName"") AS ""CardName5"", MAX(T4.""Balance"") AS ""Balance"", MAX(T5.""NumAtCard"") AS ""NumAtCard"", MAX(T5.""SlpCode"") AS ""SlpCode"", MAX(T0.""Project"") AS ""Project"", 
                MAX(T0.""Debit"") - MAX(T0.""Credit"") AS ""Saldo2"", MAX(T0.""FCDebit"") - MAX(T0.""FCCredit"") AS ""SaldoFC2"", MAX(T0.""SYSDeb"") - MAX(T0.""SYSCred"") AS ""SaldoSC2"", MAX(T4.""PymCode"") AS ""PymCode"", 
                MAX(T5.""BlockDunn"") AS ""BlockDunn"", MAX(T5.""DunnLevel"") AS ""DunnLevel"", MAX(T5.""TransType"") AS ""TransType5"", MAX(T5.""IsSales"") AS ""IsSales"", MAX(T4.""Currency"") AS ""Currency"", MAX(T0.""FCCurrency"") AS ""FCCurrency"", T0.""TransId"" AS ""TransId2"", 
                MAX(T4.""DunTerm"") AS ""DunTerm"", MAX(T0.""DunnLevel"") AS ""DunnLevel2"", T0.""BPLName"" AS ""BPLName"", MAX(T4.""ConnBP"") AS ""ConnBP"", MAX(T4.""CardCode"") AS ""CardCode"", T0.""TransId"" AS ""TransId3"", MAX(T3.""AgrNo"") AS ""AgrNo"",
                CASE WHEN MAX(T4.""Currency"") = MAX(TA.""MainCurncy"") THEN MAX(T4.""Balance"") ELSE MAX(T4.""BalanceFC"") END AS ""BalanceVis"",
                /*
                CASE WHEN MAX(T0.""DueDate"") < '" + FechaVcto + @"' THEN CASE WHEN MAX(T4.""Currency"") = MAX(TA.""MainCurncy"") THEN MAX(T0.""Debit"") - MAX(T0.""Credit"") ELSE MAX(T0.""FCDebit"") - MAX(T0.""FCCredit"") END ELSE 0 END AS ""SaldoVenc"",
                CASE WHEN MAX(T0.""DueDate"") >= '" + FechaVcto + @"' THEN CASE WHEN MAX(T4.""Currency"") = MAX(TA.""MainCurncy"") THEN MAX(T0.""Debit"") - MAX(T0.""Credit"") ELSE MAX(T0.""FCDebit"") - MAX(T0.""FCCredit"") END ELSE 0 END AS ""AbonoFut"",
                CASE WHEN MAX(T0.""DueDate"") < '" + FechaVcto + @"' AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') >= 0 AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') <= 30 THEN CASE WHEN MAX(T4.""Currency"") = MAX(TA.""MainCurncy"") THEN MAX(T0.""Debit"") - MAX(T0.""Credit"") ELSE MAX(T0.""FCDebit"") - MAX(T0.""FCCredit"") END ELSE 0 END AS ""Saldo30"",
                CASE WHEN MAX(T0.""DueDate"") < '" + FechaVcto + @"' AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') >= 31 AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') <= 60 THEN CASE WHEN MAX(T4.""Currency"") = MAX(TA.""MainCurncy"") THEN MAX(T0.""Debit"") - MAX(T0.""Credit"") ELSE MAX(T0.""FCDebit"") - MAX(T0.""FCCredit"") END ELSE 0 END AS ""Saldo60"",
                CASE WHEN MAX(T0.""DueDate"") < '" + FechaVcto + @"' AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') >= 61 AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') <= 90 THEN CASE WHEN MAX(T4.""Currency"") = MAX(TA.""MainCurncy"") THEN MAX(T0.""Debit"") - MAX(T0.""Credit"") ELSE MAX(T0.""FCDebit"") - MAX(T0.""FCCredit"") END ELSE 0 END AS ""Saldo90"",
                CASE WHEN MAX(T0.""DueDate"") < '" + FechaVcto + @"' AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') >= 91 AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') <= 120 THEN CASE WHEN MAX(T4.""Currency"") = MAX(TA.""MainCurncy"") THEN MAX(T0.""Debit"") - MAX(T0.""Credit"") ELSE MAX(T0.""FCDebit"") - MAX(T0.""FCCredit"") END ELSE 0 END AS ""Saldo120"",
                CASE WHEN MAX(T0.""DueDate"") < '" + FechaVcto + @"' AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') >= 121 THEN CASE WHEN MAX(T4.""Currency"") = MAX(TA.""MainCurncy"") THEN MAX(T0.""Debit"") - MAX(T0.""Credit"") ELSE MAX(T0.""FCDebit"") - MAX(T0.""FCCredit"") END ELSE 0 END AS ""Saldo121""
                */
                CASE WHEN MAX(T0.""DueDate"") < '" + FechaVcto + @"' THEN CASE WHEN MAX(T4.""Currency"") = MAX(TA.""MainCurncy"") THEN (MAX(T0.""BalDueCred"") + SUM(T1.""ReconSum"")) * -1 ELSE (MAX(T0.""BalFcCred"") + SUM(T1.""ReconSumFC"")) * -1 END ELSE 0 END AS ""SaldoVenc"",
                CASE WHEN MAX(T0.""DueDate"") >= '" + FechaVcto + @"' THEN CASE WHEN MAX(T4.""Currency"") = MAX(TA.""MainCurncy"") THEN (MAX(T0.""BalDueCred"") + SUM(T1.""ReconSum"")) * -1 ELSE (MAX(T0.""BalFcCred"") + SUM(T1.""ReconSumFC"")) * -1 END ELSE 0 END AS ""AbonoFut"",
                CASE WHEN MAX(T0.""DueDate"") < '" + FechaVcto + @"' AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') >= 0 AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') <= 30 THEN CASE WHEN MAX(T4.""Currency"") = MAX(TA.""MainCurncy"") THEN (MAX(T0.""BalDueCred"") + SUM(T1.""ReconSum"")) * -1 ELSE (MAX(T0.""BalFcCred"") + SUM(T1.""ReconSumFC"")) * -1 END ELSE 0 END AS ""Saldo30"",
                CASE WHEN MAX(T0.""DueDate"") < '" + FechaVcto + @"' AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') >= 31 AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') <= 60 THEN CASE WHEN MAX(T4.""Currency"") = MAX(TA.""MainCurncy"") THEN (MAX(T0.""BalDueCred"") + SUM(T1.""ReconSum"")) * -1 ELSE (MAX(T0.""BalFcCred"") + SUM(T1.""ReconSumFC"")) * -1 END ELSE 0 END AS ""Saldo60"",
                CASE WHEN MAX(T0.""DueDate"") < '" + FechaVcto + @"' AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') >= 61 AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') <= 90 THEN CASE WHEN MAX(T4.""Currency"") = MAX(TA.""MainCurncy"") THEN (MAX(T0.""BalDueCred"") + SUM(T1.""ReconSum"")) * -1 ELSE (MAX(T0.""BalFcCred"") + SUM(T1.""ReconSumFC"")) * -1 END ELSE 0 END AS ""Saldo90"",
                CASE WHEN MAX(T0.""DueDate"") < '" + FechaVcto + @"' AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') >= 91 AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') <= 120 THEN CASE WHEN MAX(T4.""Currency"") = MAX(TA.""MainCurncy"") THEN (MAX(T0.""BalDueCred"") + SUM(T1.""ReconSum"")) * -1 ELSE (MAX(T0.""BalFcCred"") + SUM(T1.""ReconSumFC"")) * -1 END ELSE 0 END AS ""Saldo120"",
                CASE WHEN MAX(T0.""DueDate"") < '" + FechaVcto + @"' AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') >= 121 THEN CASE WHEN MAX(T4.""Currency"") = MAX(TA.""MainCurncy"") THEN (MAX(T0.""BalDueCred"") + SUM(T1.""ReconSum"")) * -1 ELSE (MAX(T0.""BalFcCred"") + SUM(T1.""ReconSumFC"")) * -1 END ELSE 0 END AS ""Saldo121""
                FROM  ""JDT1"" T0  
                INNER  JOIN ""ITR1"" T1  ON  T1.""TransId"" = T0.""TransId""  AND  T1.""TransRowId"" = T0.""Line_ID""   
                INNER  JOIN ""OITR"" T2  ON  T2.""ReconNum"" = T1.""ReconNum""   
                INNER  JOIN ""OJDT"" T3  ON  T3.""TransId"" = T0.""TransId""   
                INNER  JOIN ""OCRD"" T4  ON  T4.""CardCode"" = T0.""ShortName""    
                LEFT OUTER  JOIN ""B1_JournalTransSourceView"" T5  
                ON  T5.""ObjType"" = T0.""TransType""  AND  T5.""DocEntry"" = T0.""CreatedBy""  
                AND  (T5.""TransType"" <> 'I'  OR  (T5.""TransType"" = 'I'  
                AND  T5.""InstlmntID"" = T0.""SourceLine"" ))  
				INNER JOIN ""OADM"" TA ON 1 = 1
                WHERE T0.""RefDate"" <= '" + FechaVcto + @"'  
                AND  T4.""CardType"" = 'C'  AND  T4.""Balance"" <> 0 
                " + FiltroSN + @" 
                " + FiltroGrupoSN + @" 
                AND  T2.""ReconDate"" > '" + FechaVcto + @"'  AND  T1.""IsCredit"" = 'C'   
                GROUP BY T0.""TransId"", T0.""Line_ID"", T0.""BPLName"" 
                HAVING MAX(T0.""BalFcCred"") <>- SUM(T1.""ReconSumFC"")  OR  MAX(T0.""BalDueCred"") <>- SUM(T1.""ReconSum"")  

                UNION ALL 
                SELECT 'N' AS ""Chk"", T0.""TransId"", T0.""Line_ID"", MAX(T0.""Account""), MAX(T0.""ShortName""),
                CASE 	
                WHEN MAX(T0.""TransType"") = -2  THEN 30 
                WHEN MAX(T0.""TransType"") = -3  THEN 30 
                ELSE MAX(T0.""TransType"")
                END AS ""TransType"",
                CASE 	
                WHEN MAX(T0.""TransType"") = -2  THEN 'SI-Saldo Inicial' 
                WHEN MAX(T0.""TransType"") = -3  THEN 'CB-Cierre Período' 
                WHEN MAX(T0.""TransType"") = 13  THEN 'RF-Factura Deudores' 
                WHEN MAX(T0.""TransType"") = 14  THEN 'RC-Nota Credito Clientes'
                WHEN MAX(T0.""TransType"") = 15  THEN 'NE-Entrega'
                WHEN MAX(T0.""TransType"") = 16  THEN 'DV-Devolucion'
                WHEN MAX(T0.""TransType"") = 18  THEN 'TT-Factura Proveedores'
                WHEN MAX(T0.""TransType"") = 19  THEN 'PC-Nota Credito Proveedores'
                WHEN MAX(T0.""TransType"") = 20  THEN 'EP-Entrada Mercancias'
                WHEN MAX(T0.""TransType"") = 21  THEN 'DM-Devolucion Mercancías'
                WHEN MAX(T0.""TransType"") = 24  THEN 'PR-Pagos Recibidos'
                WHEN MAX(T0.""TransType"") = 30  THEN 'AS-Asiento'
                WHEN MAX(T0.""TransType"") = 46  THEN 'PP-Pagos Efectuados'
                WHEN MAX(T0.""TransType"") = 59  THEN 'EM-Entrada Mercancías'
                WHEN MAX(T0.""TransType"") = 60  THEN 'OA-Emisión para producción'
                WHEN MAX(T0.""TransType"") = 67  THEN 'IM-Transferencia de Stock'
                WHEN MAX(T0.""TransType"") = 69  THEN 'DI-Precio Entrega'
                WHEN MAX(T0.""TransType"") = 162 THEN 'RI-Revalorización Inventario'
                WHEN MAX(T0.""TransType"") = 202 THEN 'OF-Orden de Fabricación'
                WHEN MAX(T0.""TransType"") = 204 THEN 'AN-F Anticipo Proveedores'
                WHEN MAX(T0.""TransType"") = 321 THEN 'ID-Reconciliación Interna'
                ELSE MAX(T0.""TransType"")
                END AS ""Origen"",
                MAX(T0.""CreatedBy""), MAX(T0.""BaseRef""), 
                MAX(T0.""SourceLine""), MAX(T0.""RefDate""), MAX(T0.""DueDate""), MAX(T0.""TaxDate""),  - MAX(T0.""BalDueDeb"") - SUM(T1.""ReconSum""),  - MAX(T0.""BalFcDeb"") 
                - SUM(T1.""ReconSumFC""),  - MAX(T0.""BalScDeb"") - SUM(T1.""ReconSumSC""), MAX(T0.""LineMemo""), MAX(T3.""FolioPref""), MAX(T3.""FolioNum""), MAX(T0.""Indicator""), 
                MAX(T4.""CardName""), MAX(T5.""CardCode""), MAX(T5.""CardName""), MAX(T4.""Balance""), MAX(T5.""NumAtCard""), MAX(T5.""SlpCode""), MAX(T0.""Project""), 
                MAX(T0.""Debit"") - MAX(T0.""Credit""), MAX(T0.""FCDebit"") - MAX(T0.""FCCredit""), MAX(T0.""SYSDeb"") - MAX(T0.""SYSCred""), MAX(T4.""PymCode""), 
                MAX(T5.""BlockDunn""), MAX(T5.""DunnLevel""), MAX(T5.""TransType""), MAX(T5.""IsSales""), MAX(T4.""Currency""), MAX(T0.""FCCurrency""), T0.""TransId"", 
                MAX(T4.""DunTerm""), MAX(T0.""DunnLevel""), T0.""BPLName"", MAX(T4.""ConnBP""), MAX(T4.""CardCode""), T0.""TransId"", MAX(T3.""AgrNo"") ,
                CASE WHEN MAX(T4.""Currency"") = MAX(TA.""MainCurncy"") THEN MAX(T4.""Balance"") ELSE MAX(T4.""BalanceFC"") END AS ""BalanceVis"",
                /*
                CASE WHEN MAX(T0.""DueDate"") < '" + FechaVcto + @"' THEN CASE WHEN MAX(T4.""Currency"") = MAX(TA.""MainCurncy"") THEN MAX(T0.""Debit"") - MAX(T0.""Credit"") ELSE MAX(T0.""FCDebit"") - MAX(T0.""FCCredit"") END ELSE 0 END AS ""SaldoVenc"",
                CASE WHEN MAX(T0.""DueDate"") >= '" + FechaVcto + @"' THEN CASE WHEN MAX(T4.""Currency"") = MAX(TA.""MainCurncy"") THEN MAX(T0.""Debit"") - MAX(T0.""Credit"") ELSE MAX(T0.""FCDebit"") - MAX(T0.""FCCredit"") END ELSE 0 END AS ""AbonoFut"",
                CASE WHEN MAX(T0.""DueDate"") < '" + FechaVcto + @"' AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') >= 0 AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') <= 30 THEN CASE WHEN MAX(T4.""Currency"") = MAX(TA.""MainCurncy"") THEN MAX(T0.""Debit"") - MAX(T0.""Credit"") ELSE MAX(T0.""FCDebit"") - MAX(T0.""FCCredit"") END ELSE 0 END AS ""Saldo30"",
                CASE WHEN MAX(T0.""DueDate"") < '" + FechaVcto + @"' AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') >= 31 AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') <= 60 THEN CASE WHEN MAX(T4.""Currency"") = MAX(TA.""MainCurncy"") THEN MAX(T0.""Debit"") - MAX(T0.""Credit"") ELSE MAX(T0.""FCDebit"") - MAX(T0.""FCCredit"") END ELSE 0 END AS ""Saldo60"",
                CASE WHEN MAX(T0.""DueDate"") < '" + FechaVcto + @"' AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') >= 61 AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') <= 90 THEN CASE WHEN MAX(T4.""Currency"") = MAX(TA.""MainCurncy"") THEN MAX(T0.""Debit"") - MAX(T0.""Credit"") ELSE MAX(T0.""FCDebit"") - MAX(T0.""FCCredit"") END ELSE 0 END AS ""Saldo90"",
                CASE WHEN MAX(T0.""DueDate"") < '" + FechaVcto + @"' AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') >= 91 AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') <= 120 THEN CASE WHEN MAX(T4.""Currency"") = MAX(TA.""MainCurncy"") THEN MAX(T0.""Debit"") - MAX(T0.""Credit"") ELSE MAX(T0.""FCDebit"") - MAX(T0.""FCCredit"") END ELSE 0 END AS ""Saldo120"",
                CASE WHEN MAX(T0.""DueDate"") < '" + FechaVcto + @"' AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') >= 121 THEN CASE WHEN MAX(T4.""Currency"") = MAX(TA.""MainCurncy"") THEN MAX(T0.""Debit"") - MAX(T0.""Credit"") ELSE MAX(T0.""FCDebit"") - MAX(T0.""FCCredit"") END ELSE 0 END AS ""Saldo121""
                */
                CASE WHEN MAX(T0.""DueDate"") < '" + FechaVcto + @"' THEN CASE WHEN MAX(T4.""Currency"") = MAX(TA.""MainCurncy"") THEN (- MAX(T0.""BalDueDeb"") - SUM(T1.""ReconSum"")) * -1 ELSE (- MAX(T0.""BalFcDeb"") - SUM(T1.""ReconSumFC"")) * -1 END ELSE 0 END AS ""SaldoVenc"",
                CASE WHEN MAX(T0.""DueDate"") >= '" + FechaVcto + @"' THEN CASE WHEN MAX(T4.""Currency"") = MAX(TA.""MainCurncy"") THEN (- MAX(T0.""BalDueDeb"") - SUM(T1.""ReconSum"")) * -1 ELSE (- MAX(T0.""BalFcDeb"") - SUM(T1.""ReconSumFC"")) * -1 END ELSE 0 END AS ""AbonoFut"",
                CASE WHEN MAX(T0.""DueDate"") < '" + FechaVcto + @"' AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') >= 0 AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') <= 30 THEN CASE WHEN MAX(T4.""Currency"") = MAX(TA.""MainCurncy"") THEN (- MAX(T0.""BalDueDeb"") - SUM(T1.""ReconSum"")) * -1 ELSE (- MAX(T0.""BalFcDeb"") - SUM(T1.""ReconSumFC"")) * -1 END ELSE 0 END AS ""Saldo30"",
                CASE WHEN MAX(T0.""DueDate"") < '" + FechaVcto + @"' AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') >= 31 AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') <= 60 THEN CASE WHEN MAX(T4.""Currency"") = MAX(TA.""MainCurncy"") THEN (- MAX(T0.""BalDueDeb"") - SUM(T1.""ReconSum"")) * -1 ELSE (- MAX(T0.""BalFcDeb"") - SUM(T1.""ReconSumFC"")) * -1 END ELSE 0 END AS ""Saldo60"",
                CASE WHEN MAX(T0.""DueDate"") < '" + FechaVcto + @"' AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') >= 61 AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') <= 90 THEN CASE WHEN MAX(T4.""Currency"") = MAX(TA.""MainCurncy"") THEN (- MAX(T0.""BalDueDeb"") - SUM(T1.""ReconSum"")) * -1 ELSE (- MAX(T0.""BalFcDeb"") - SUM(T1.""ReconSumFC"")) * -1 END ELSE 0 END AS ""Saldo90"",
                CASE WHEN MAX(T0.""DueDate"") < '" + FechaVcto + @"' AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') >= 91 AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') <= 120 THEN CASE WHEN MAX(T4.""Currency"") = MAX(TA.""MainCurncy"") THEN (- MAX(T0.""BalDueDeb"") - SUM(T1.""ReconSum"")) * -1 ELSE (- MAX(T0.""BalFcDeb"") - SUM(T1.""ReconSumFC"")) * -1 END ELSE 0 END AS ""Saldo120"",
                CASE WHEN MAX(T0.""DueDate"") < '" + FechaVcto + @"' AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') >= 121 THEN CASE WHEN MAX(T4.""Currency"") = MAX(TA.""MainCurncy"") THEN (- MAX(T0.""BalDueDeb"") - SUM(T1.""ReconSum"")) * -1 ELSE (- MAX(T0.""BalFcDeb"") - SUM(T1.""ReconSumFC"")) * -1 END ELSE 0 END AS ""Saldo121""
                FROM  ""JDT1"" T0  
                INNER  JOIN ""ITR1"" T1  ON  
                T1.""TransId"" = T0.""TransId""  AND  T1.""TransRowId"" = T0.""Line_ID""   
                INNER  JOIN ""OITR"" T2  ON  T2.""ReconNum"" = T1.""ReconNum""   
                INNER  JOIN ""OJDT"" T3  ON  T3.""TransId"" = T0.""TransId""   
                INNER  JOIN ""OCRD"" T4  ON  T4.""CardCode"" = T0.""ShortName""    
                LEFT OUTER  JOIN ""B1_JournalTransSourceView"" T5  ON  
                T5.""ObjType"" = T0.""TransType""  AND  T5.""DocEntry"" = T0.""CreatedBy""  
                AND  (T5.""TransType"" <> 'I'  OR  (T5.""TransType"" = 'I'  
                AND  T5.""InstlmntID"" = T0.""SourceLine"" ))  
				INNER JOIN ""OADM"" TA ON 1 = 1
                WHERE T0.""RefDate"" <= '" + FechaVcto + @"'
                AND  T4.""CardType"" = 'C'  AND  T4.""Balance"" <> 0  
                " + FiltroSN + @" 
                " + FiltroGrupoSN + @" 
                AND  T2.""ReconDate"" > '" + FechaVcto + @"'  AND  T1.""IsCredit"" = 'D'   
                GROUP BY T0.""TransId"", T0.""Line_ID"", T0.""BPLName"" 
                HAVING MAX(T0.""BalFcDeb"") <>- SUM(T1.""ReconSumFC"")  OR  MAX(T0.""BalDueDeb"") <>- SUM(T1.""ReconSum"")   

                UNION ALL 
                SELECT 'N' AS ""Chk"", T0.""TransId"", T0.""Line_ID"", MAX(T0.""Account""), MAX(T0.""ShortName""),
                CASE 	
                WHEN MAX(T0.""TransType"") = -2  THEN 30 
                WHEN MAX(T0.""TransType"") = -3  THEN 30 
                ELSE MAX(T0.""TransType"")
                END AS ""TransType"",
                CASE 	
                WHEN MAX(T0.""TransType"") = -2  THEN 'SI-Saldo Inicial' 
                WHEN MAX(T0.""TransType"") = -3  THEN 'CB-Cierre Período' 
                WHEN MAX(T0.""TransType"") = 13  THEN 'RF-Factura Deudores' 
                WHEN MAX(T0.""TransType"") = 14  THEN 'RC-Nota Credito Clientes'
                WHEN MAX(T0.""TransType"") = 15  THEN 'NE-Entrega'
                WHEN MAX(T0.""TransType"") = 16  THEN 'DV-Devolucion'
                WHEN MAX(T0.""TransType"") = 18  THEN 'TT-Factura Proveedores'
                WHEN MAX(T0.""TransType"") = 19  THEN 'PC-Nota Credito Proveedores'
                WHEN MAX(T0.""TransType"") = 20  THEN 'EP-Entrada Mercancias'
                WHEN MAX(T0.""TransType"") = 21  THEN 'DM-Devolucion Mercancías'
                WHEN MAX(T0.""TransType"") = 24  THEN 'PR-Pagos Recibidos'
                WHEN MAX(T0.""TransType"") = 30  THEN 'AS-Asiento'
                WHEN MAX(T0.""TransType"") = 46  THEN 'PP-Pagos Efectuados'
                WHEN MAX(T0.""TransType"") = 59  THEN 'EM-Entrada Mercancías'
                WHEN MAX(T0.""TransType"") = 60  THEN 'OA-Emisión para producción'
                WHEN MAX(T0.""TransType"") = 67  THEN 'IM-Transferencia de Stock'
                WHEN MAX(T0.""TransType"") = 69  THEN 'DI-Precio Entrega'
                WHEN MAX(T0.""TransType"") = 162 THEN 'RI-Revalorización Inventario'
                WHEN MAX(T0.""TransType"") = 202 THEN 'OF-Orden de Fabricación'
                WHEN MAX(T0.""TransType"") = 204 THEN 'AN-F Anticipo Proveedores'
                WHEN MAX(T0.""TransType"") = 321 THEN 'ID-Reconciliación Interna'
                ELSE MAX(T0.""TransType"")
                END AS ""Origen"",
                MAX(T0.""CreatedBy""), MAX(T0.""BaseRef""), 
                MAX(T0.""SourceLine""), MAX(T0.""RefDate""), MAX(T0.""DueDate""), MAX(T0.""TaxDate""), MAX(T0.""BalDueCred"") - MAX(T0.""BalDueDeb""), MAX(T0.""BalFcCred"") 
                - MAX(T0.""BalFcDeb""), MAX(T0.""BalScCred"") - MAX(T0.""BalScDeb""), MAX(T0.""LineMemo""), MAX(T1.""FolioPref""), MAX(T1.""FolioNum""), MAX(T0.""Indicator""), 
                MAX(T2.""CardName""), MAX(T3.""CardCode""), MAX(T3.""CardName""), MAX(T2.""Balance""), MAX(T3.""NumAtCard""), MAX(T3.""SlpCode""), MAX(T0.""Project""), 
                MAX(T0.""Debit"") - MAX(T0.""Credit""), MAX(T0.""FCDebit"") - MAX(T0.""FCCredit""), MAX(T0.""SYSDeb"") - MAX(T0.""SYSCred""), MAX(T2.""PymCode""), 
                MAX(T3.""BlockDunn""), MAX(T3.""DunnLevel""), MAX(T3.""TransType""), MAX(T3.""IsSales""), MAX(T2.""Currency""), MAX(T0.""FCCurrency""), T0.""TransId"", 
                MAX(T2.""DunTerm""), MAX(T0.""DunnLevel""), T0.""BPLName"", MAX(T2.""ConnBP""), MAX(T2.""CardCode""), T0.""TransId"", MAX(T1.""AgrNo"") ,
                CASE WHEN MAX(T2.""Currency"") = MAX(TA.""MainCurncy"") THEN MAX(T2.""Balance"") ELSE MAX(T2.""BalanceFC"") END AS ""BalanceVis"",
                /*
                CASE WHEN MAX(T0.""DueDate"") < '" + FechaVcto + @"' THEN CASE WHEN MAX(T2.""Currency"") = MAX(TA.""MainCurncy"") THEN MAX(T0.""Debit"") - MAX(T0.""Credit"") ELSE MAX(T0.""FCDebit"") - MAX(T0.""FCCredit"") END ELSE 0 END AS ""SaldoVenc"",
                CASE WHEN MAX(T0.""DueDate"") >= '" + FechaVcto + @"' THEN CASE WHEN MAX(T2.""Currency"") = MAX(TA.""MainCurncy"") THEN MAX(T0.""Debit"") - MAX(T0.""Credit"") ELSE MAX(T0.""FCDebit"") - MAX(T0.""FCCredit"") END ELSE 0 END AS ""AbonoFut"",
                CASE WHEN MAX(T0.""DueDate"") < '" + FechaVcto + @"' AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') >= 0 AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') <= 30 THEN CASE WHEN MAX(T2.""Currency"") = MAX(TA.""MainCurncy"") THEN MAX(T0.""Debit"") - MAX(T0.""Credit"") ELSE MAX(T0.""FCDebit"") - MAX(T0.""FCCredit"") END ELSE 0 END AS ""Saldo30"",
                CASE WHEN MAX(T0.""DueDate"") < '" + FechaVcto + @"' AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') >= 31 AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') <= 60 THEN CASE WHEN MAX(T2.""Currency"") = MAX(TA.""MainCurncy"") THEN MAX(T0.""Debit"") - MAX(T0.""Credit"") ELSE MAX(T0.""FCDebit"") - MAX(T0.""FCCredit"") END ELSE 0 END AS ""Saldo60"",
                CASE WHEN MAX(T0.""DueDate"") < '" + FechaVcto + @"' AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') >= 61 AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') <= 90 THEN CASE WHEN MAX(T2.""Currency"") = MAX(TA.""MainCurncy"") THEN MAX(T0.""Debit"") - MAX(T0.""Credit"") ELSE MAX(T0.""FCDebit"") - MAX(T0.""FCCredit"") END ELSE 0 END AS ""Saldo90"",
                CASE WHEN MAX(T0.""DueDate"") < '" + FechaVcto + @"' AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') >= 91 AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') <= 120 THEN CASE WHEN MAX(T2.""Currency"") = MAX(TA.""MainCurncy"") THEN MAX(T0.""Debit"") - MAX(T0.""Credit"") ELSE MAX(T0.""FCDebit"") - MAX(T0.""FCCredit"") END ELSE 0 END AS ""Saldo120"",
                CASE WHEN MAX(T0.""DueDate"") < '" + FechaVcto + @"' AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') >= 121 THEN CASE WHEN MAX(T2.""Currency"") = MAX(TA.""MainCurncy"") THEN MAX(T0.""Debit"") - MAX(T0.""Credit"") ELSE MAX(T0.""FCDebit"") - MAX(T0.""FCCredit"") END ELSE 0 END AS ""Saldo121""
                */
                CASE WHEN MAX(T0.""DueDate"") < '" + FechaVcto + @"' THEN CASE WHEN MAX(T2.""Currency"") = MAX(TA.""MainCurncy"") THEN (MAX(T0.""BalDueCred"") - MAX(T0.""BalDueDeb"")) * -1 ELSE (MAX(T0.""BalFcCred"") - MAX(T0.""BalFcDeb"")) * -1 END ELSE 0 END AS ""SaldoVenc"",
                CASE WHEN MAX(T0.""DueDate"") >= '" + FechaVcto + @"' THEN CASE WHEN MAX(T2.""Currency"") = MAX(TA.""MainCurncy"") THEN (MAX(T0.""BalDueCred"") - MAX(T0.""BalDueDeb"")) * -1 ELSE (MAX(T0.""BalFcCred"") - MAX(T0.""BalFcDeb"")) * -1 END ELSE 0 END AS ""AbonoFut"",
                CASE WHEN MAX(T0.""DueDate"") < '" + FechaVcto + @"' AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') >= 0 AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') <= 30 THEN CASE WHEN MAX(T2.""Currency"") = MAX(TA.""MainCurncy"") THEN (MAX(T0.""BalDueCred"") - MAX(T0.""BalDueDeb"")) * -1 ELSE (MAX(T0.""BalFcCred"") - MAX(T0.""BalFcDeb"")) * -1 END ELSE 0 END AS ""Saldo30"",
                CASE WHEN MAX(T0.""DueDate"") < '" + FechaVcto + @"' AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') >= 31 AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') <= 60 THEN CASE WHEN MAX(T2.""Currency"") = MAX(TA.""MainCurncy"") THEN (MAX(T0.""BalDueCred"") - MAX(T0.""BalDueDeb"")) * -1 ELSE (MAX(T0.""BalFcCred"") - MAX(T0.""BalFcDeb"")) * -1 END ELSE 0 END AS ""Saldo60"",
                CASE WHEN MAX(T0.""DueDate"") < '" + FechaVcto + @"' AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') >= 61 AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') <= 90 THEN CASE WHEN MAX(T2.""Currency"") = MAX(TA.""MainCurncy"") THEN (MAX(T0.""BalDueCred"") - MAX(T0.""BalDueDeb"")) * -1 ELSE (MAX(T0.""BalFcCred"") - MAX(T0.""BalFcDeb"")) * -1 END ELSE 0 END AS ""Saldo90"",
                CASE WHEN MAX(T0.""DueDate"") < '" + FechaVcto + @"' AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') >= 91 AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') <= 120 THEN CASE WHEN MAX(T2.""Currency"") = MAX(TA.""MainCurncy"") THEN (MAX(T0.""BalDueCred"") - MAX(T0.""BalDueDeb"")) * -1 ELSE (MAX(T0.""BalFcCred"") - MAX(T0.""BalFcDeb"")) * -1 END ELSE 0 END AS ""Saldo120"",
                CASE WHEN MAX(T0.""DueDate"") < '" + FechaVcto + @"' AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') >= 121 THEN CASE WHEN MAX(T2.""Currency"") = MAX(TA.""MainCurncy"") THEN (MAX(T0.""BalDueCred"") - MAX(T0.""BalDueDeb"")) * -1 ELSE (MAX(T0.""BalFcCred"") - MAX(T0.""BalFcDeb"")) * -1 END ELSE 0 END AS ""Saldo121""
                FROM  ""JDT1"" T0  
                INNER  JOIN ""OJDT"" T1  ON  T1.""TransId"" = T0.""TransId""   
                INNER  JOIN ""OCRD"" T2  ON  T2.""CardCode"" = T0.""ShortName""    
                LEFT OUTER  JOIN ""B1_JournalTransSourceView"" T3  
                ON  T3.""ObjType"" = T0.""TransType""  AND  T3.""DocEntry"" = T0.""CreatedBy""  
                AND  (T3.""TransType"" <> 'I'  OR  (T3.""TransType"" = 'I'  
                AND  T3.""InstlmntID"" = T0.""SourceLine"" ))  
				INNER JOIN ""OADM"" TA ON 1 = 1
                WHERE T0.""RefDate"" <= '" + FechaVcto + @"' 
                AND  T2.""CardType"" = 'C'  AND  T2.""Balance"" <> 0  
                " + FiltroSN2 + @" 
                " + FiltroGrupoSN2 + @" 
                AND  (T0.""BalDueCred"" <> T0.""BalDueDeb""  OR  T0.""BalFcCred"" <> T0.""BalFcDeb"" ) 
                AND   NOT EXISTS 
                (SELECT U0.""TransId"", U0.""TransRowId"" 
                FROM  ""ITR1"" U0  
                INNER  JOIN ""OITR"" U1  ON  U1.""ReconNum"" = U0.""ReconNum""   
                WHERE T0.""TransId"" = U0.""TransId""  AND  T0.""Line_ID"" = U0.""TransRowId""  AND  U1.""ReconDate"" > '" + FechaVcto + @"'   
                GROUP BY U0.""TransId"", U0.""TransRowId"")   
                GROUP BY T0.""TransId"", T0.""Line_ID"", T0.""BPLName""
                ) T10
                GROUP BY ""CardCode""
                ";
            datatable.ExecuteQuery(_query);
            oForm.Freeze(true);
            mtxDet.Clear();
            mtxDet.LoadFromDataSource();
            Comun.FuncionesComunes.LiberarObjetoGenerico(datatable);
            oForm.Freeze(false);
        }

        private static void MarcarRegistrosMatrix(SAPbouiCOM.Matrix oMatrix, SAPbouiCOM.DataTable dtDoc, string columna)
        {
            string accion = "Y";
            for (int i = 0; i < dtDoc.Rows.Count; i++)
            {
                if (i == 0 && dtDoc.GetValue(columna, i).ToString().Equals("Y"))
                {
                    accion = "N";
                }
                dtDoc.SetValue(columna, i, accion);
            }
            oForm.Freeze(true);
            oMatrix.LoadFromDataSource();
            oForm.Freeze(false);
        }

        private static void CargarClientes(SAPbouiCOM.Matrix oMatrix, SAPbouiCOM.DataTable dtDoc, string columna)
        {
            oForm.Freeze(true);
            oMatrix.FlushToDataSource();
            Clientes = new List<string>();
            for (int i = 0; i < dtDoc.Rows.Count; i++)
            {
                if (dtDoc.GetValue(columna, i).ToString().Equals("Y"))
                {
                    Clientes.Add(dtDoc.GetValue("CardCode", i).ToString());
                }
            }
            oMatrix.LoadFromDataSource();
            oForm.Freeze(false);
        }
    }
}
