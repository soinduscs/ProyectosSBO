using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM.Framework;
using EASendMail;

namespace Soindus.AddOnFactoringCob.Formularios
{
    [FormAttribute("Soindus.AddOnFactoringCob.Formularios.frmCobSelect", "Formularios/frmCobSelect.b1f")]
    class frmCobSelect : UserFormBase
    {
        // Objetos de formulario
        #region Objetos de formulario
        private static SAPbouiCOM.Form oForm;
        private static SAPbouiCOM.EditText txtCodSN;
        private static SAPbouiCOM.EditText txtNomSN;
        private static SAPbouiCOM.EditText txtFVcto;
        private static SAPbouiCOM.EditText txtObs;
        private static SAPbouiCOM.ComboBox cbxNivel;
        private static SAPbouiCOM.ComboBox cbxIdioma;
        private static SAPbouiCOM.Button btnFiltrar;
        private static SAPbouiCOM.Button btnMarcar;
        private static SAPbouiCOM.Button btnGenerar;
        private static SAPbouiCOM.Matrix mtxDet;
        public static List<string> Clientes = new List<string>();
        public static string Vencimiento = DateTime.Now.Date.ToString("yyyyMMdd");
        #endregion

        public frmCobSelect(List<string> _Clientes = null, string _Vencimiento = "")
        {
            Clientes = _Clientes;
            Vencimiento = _Vencimiento;
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

                            if (pVal.ColUID.Equals("CreatedBy"))
                            {
                                string sObjectType =
                                    ((SAPbouiCOM.EditText)(mtxDet.Columns.Item("TransType").Cells.Item(pVal.Row).Specific)).String;
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

                        // Boton generar
                        #region generar
                        if (pVal.ItemUID.Equals("btnGenerar"))
                        {
                            int resp = Application.SBO_Application.MessageBox("¿Seguro desea generar la cobranza?", 2, "Si", "No");
                            if (resp.Equals(1))
                            {
                                // Documentos
                                string NombreDT = "dtDet";
                                SAPbouiCOM.DataTable datatable = oForm.DataSources.DataTables.Item(NombreDT);
                                GenerarCobranza(datatable, "Chk");
                                Comun.FuncionesComunes.LiberarObjetoGenerico(datatable);
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
            oForm = ((SAPbouiCOM.Form)(Application.SBO_Application.Forms.Item("frmCobSelect")));
            txtCodSN = ((SAPbouiCOM.EditText)(GetItem("CodSN").Specific));
            txtNomSN = ((SAPbouiCOM.EditText)(GetItem("NomSN").Specific));
            txtFVcto = ((SAPbouiCOM.EditText)(GetItem("FVcto").Specific));
            txtObs = ((SAPbouiCOM.EditText)(GetItem("Obs").Specific));
            cbxNivel = ((SAPbouiCOM.ComboBox)(GetItem("cbxNivel").Specific));
            cbxIdioma = ((SAPbouiCOM.ComboBox)(GetItem("cbxIdioma").Specific));
            btnFiltrar = ((SAPbouiCOM.Button)(GetItem("btnFiltrar").Specific));
            btnMarcar = ((SAPbouiCOM.Button)(GetItem("btnMarcar").Specific));
            btnGenerar = ((SAPbouiCOM.Button)(GetItem("btnGenerar").Specific));
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

            // Combo nivel
            oForm.DataSources.UserDataSources.Add("Nivel", SAPbouiCOM.BoDataType.dt_SHORT_TEXT);
            cbxNivel.ValidValues.Add("0-30", "0-30");
            cbxNivel.ValidValues.Add("31-60", "31-60");
            cbxNivel.ValidValues.Add("60+", "60+");
            cbxNivel.DataBind.SetBound(true, "", "Nivel");
            cbxNivel.Select("0-30");

            // Combo idioma
            oForm.DataSources.UserDataSources.Add("Idioma", SAPbouiCOM.BoDataType.dt_SHORT_TEXT);
            cbxIdioma.ValidValues.Add("Español", "Español");
            cbxIdioma.ValidValues.Add("Inglés", "Inglés");
            cbxIdioma.DataBind.SetBound(true, "", "Idioma");
            cbxIdioma.Select("Español");

            // Observaciones
            oForm.DataSources.UserDataSources.Add("Obs", SAPbouiCOM.BoDataType.dt_LONG_TEXT);
            txtObs.DataBind.SetBound(true, "", "Obs");

            txtCodSN.Value = string.Empty;

            oForm.DataSources.DataTables.Add("dtDet");

            EstructuraMatrixDetalle();

            mtxDet.SelectionMode = SAPbouiCOM.BoMatrixSelect.ms_Single;
            oForm.Visible = true;
        }

        private static void EstructuraMatrixDetalle()
        {
            List<string> clientes = new List<string>();
            string FiltroSN3 = @" AND  T4.""CardCode"" IN (";
            string FiltroSN4 = @" AND  T2.""CardCode"" IN (";
            foreach (var cliente in Clientes)
            {
                clientes.Add(string.Format("'{0}'", cliente));
            }
            string joined = string.Join(",", clientes);
            FiltroSN3 += @joined + @")";
            FiltroSN4 += @joined + @")";

            string FechaVcto = Vencimiento;
            string NombreDT = "dtDet";
            SAPbouiCOM.DataTable datatable = oForm.DataSources.DataTables.Item(NombreDT);
            string _query = @"
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
                CASE WHEN MAX(T0.""DueDate"") < '" + FechaVcto + @"' AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') >= 121 THEN CASE WHEN MAX(T4.""Currency"") = MAX(TA.""MainCurncy"") THEN MAX(T0.""Debit"") - MAX(T0.""Credit"") ELSE MAX(T0.""FCDebit"") - MAX(T0.""FCCredit"") END ELSE 0 END AS ""Saldo121"",
                */
                CASE WHEN MAX(T0.""DueDate"") < '" + FechaVcto + @"' THEN CASE WHEN MAX(T4.""Currency"") = MAX(TA.""MainCurncy"") THEN (MAX(T0.""BalDueCred"") + SUM(T1.""ReconSum"")) * -1 ELSE (MAX(T0.""BalFcCred"") + SUM(T1.""ReconSumFC"")) * -1 END ELSE 0 END AS ""SaldoVenc"",
                CASE WHEN MAX(T0.""DueDate"") >= '" + FechaVcto + @"' THEN CASE WHEN MAX(T4.""Currency"") = MAX(TA.""MainCurncy"") THEN (MAX(T0.""BalDueCred"") + SUM(T1.""ReconSum"")) * -1 ELSE (MAX(T0.""BalFcCred"") + SUM(T1.""ReconSumFC"")) * -1 END ELSE 0 END AS ""AbonoFut"",
                CASE WHEN MAX(T0.""DueDate"") < '" + FechaVcto + @"' AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') >= 0 AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') <= 30 THEN CASE WHEN MAX(T4.""Currency"") = MAX(TA.""MainCurncy"") THEN (MAX(T0.""BalDueCred"") + SUM(T1.""ReconSum"")) * -1 ELSE (MAX(T0.""BalFcCred"") + SUM(T1.""ReconSumFC"")) * -1 END ELSE 0 END AS ""Saldo30"",
                CASE WHEN MAX(T0.""DueDate"") < '" + FechaVcto + @"' AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') >= 31 AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') <= 60 THEN CASE WHEN MAX(T4.""Currency"") = MAX(TA.""MainCurncy"") THEN (MAX(T0.""BalDueCred"") + SUM(T1.""ReconSum"")) * -1 ELSE (MAX(T0.""BalFcCred"") + SUM(T1.""ReconSumFC"")) * -1 END ELSE 0 END AS ""Saldo60"",
                CASE WHEN MAX(T0.""DueDate"") < '" + FechaVcto + @"' AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') >= 61 AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') <= 90 THEN CASE WHEN MAX(T4.""Currency"") = MAX(TA.""MainCurncy"") THEN (MAX(T0.""BalDueCred"") + SUM(T1.""ReconSum"")) * -1 ELSE (MAX(T0.""BalFcCred"") + SUM(T1.""ReconSumFC"")) * -1 END ELSE 0 END AS ""Saldo90"",
                CASE WHEN MAX(T0.""DueDate"") < '" + FechaVcto + @"' AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') >= 91 AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') <= 120 THEN CASE WHEN MAX(T4.""Currency"") = MAX(TA.""MainCurncy"") THEN (MAX(T0.""BalDueCred"") + SUM(T1.""ReconSum"")) * -1 ELSE (MAX(T0.""BalFcCred"") + SUM(T1.""ReconSumFC"")) * -1 END ELSE 0 END AS ""Saldo120"",
                CASE WHEN MAX(T0.""DueDate"") < '" + FechaVcto + @"' AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') >= 121 THEN CASE WHEN MAX(T4.""Currency"") = MAX(TA.""MainCurncy"") THEN (MAX(T0.""BalDueCred"") + SUM(T1.""ReconSum"")) * -1 ELSE (MAX(T0.""BalFcCred"") + SUM(T1.""ReconSumFC"")) * -1 END ELSE 0 END AS ""Saldo121"",
                MAX(FCRNG.""U_FACTORINGID"") AS ""FacID"", MAX(FCRNG.""U_INSTITU"") AS ""FacEntidad"", MAX(FCRNG.""U_FACTORINGVCTO"") AS ""FacFecha"", MAX(FCRNG.""U_NUMOPER"") AS ""FacOper"", MAX(FCRNG.""U_RESFAC"") AS ""FacResp""
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
                LEFT OUTER JOIN (
                SELECT ""DocEntry"", ""DocNum"", ""TransId"", ""U_FACTORINGID"", ""U_INSTITU"", ""U_FACTORINGVCTO"", ""U_NUMOPER"", ""U_RESFAC"" FROM OINV 
                UNION ALL
                SELECT ""DocEntry"", ""DocNum"", ""TransId"", ""U_FACTORINGID"", ""U_INSTITU"", ""U_FACTORINGVCTO"", ""U_NUMOPER"", ""U_RESFAC"" FROM ORIN
                ) FCRNG ON T0.""TransId"" = FCRNG.""TransId""
                WHERE T0.""RefDate"" <= '" + FechaVcto + @"'  
                AND  T4.""CardType"" = 'C'  AND  T4.""Balance"" <> 0 
                " + FiltroSN3 + @" 
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
                CASE WHEN MAX(T0.""DueDate"") < '" + FechaVcto + @"' AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') >= 121 THEN CASE WHEN MAX(T4.""Currency"") = MAX(TA.""MainCurncy"") THEN MAX(T0.""Debit"") - MAX(T0.""Credit"") ELSE MAX(T0.""FCDebit"") - MAX(T0.""FCCredit"") END ELSE 0 END AS ""Saldo121"",
                */
                CASE WHEN MAX(T0.""DueDate"") < '" + FechaVcto + @"' THEN CASE WHEN MAX(T4.""Currency"") = MAX(TA.""MainCurncy"") THEN (- MAX(T0.""BalDueDeb"") - SUM(T1.""ReconSum"")) * -1 ELSE (- MAX(T0.""BalFcDeb"") - SUM(T1.""ReconSumFC"")) * -1 END ELSE 0 END AS ""SaldoVenc"",
                CASE WHEN MAX(T0.""DueDate"") >= '" + FechaVcto + @"' THEN CASE WHEN MAX(T4.""Currency"") = MAX(TA.""MainCurncy"") THEN (- MAX(T0.""BalDueDeb"") - SUM(T1.""ReconSum"")) * -1 ELSE (- MAX(T0.""BalFcDeb"") - SUM(T1.""ReconSumFC"")) * -1 END ELSE 0 END AS ""AbonoFut"",
                CASE WHEN MAX(T0.""DueDate"") < '" + FechaVcto + @"' AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') >= 0 AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') <= 30 THEN CASE WHEN MAX(T4.""Currency"") = MAX(TA.""MainCurncy"") THEN (- MAX(T0.""BalDueDeb"") - SUM(T1.""ReconSum"")) * -1 ELSE (- MAX(T0.""BalFcDeb"") - SUM(T1.""ReconSumFC"")) * -1 END ELSE 0 END AS ""Saldo30"",
                CASE WHEN MAX(T0.""DueDate"") < '" + FechaVcto + @"' AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') >= 31 AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') <= 60 THEN CASE WHEN MAX(T4.""Currency"") = MAX(TA.""MainCurncy"") THEN (- MAX(T0.""BalDueDeb"") - SUM(T1.""ReconSum"")) * -1 ELSE (- MAX(T0.""BalFcDeb"") - SUM(T1.""ReconSumFC"")) * -1 END ELSE 0 END AS ""Saldo60"",
                CASE WHEN MAX(T0.""DueDate"") < '" + FechaVcto + @"' AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') >= 61 AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') <= 90 THEN CASE WHEN MAX(T4.""Currency"") = MAX(TA.""MainCurncy"") THEN (- MAX(T0.""BalDueDeb"") - SUM(T1.""ReconSum"")) * -1 ELSE (- MAX(T0.""BalFcDeb"") - SUM(T1.""ReconSumFC"")) * -1 END ELSE 0 END AS ""Saldo90"",
                CASE WHEN MAX(T0.""DueDate"") < '" + FechaVcto + @"' AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') >= 91 AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') <= 120 THEN CASE WHEN MAX(T4.""Currency"") = MAX(TA.""MainCurncy"") THEN (- MAX(T0.""BalDueDeb"") - SUM(T1.""ReconSum"")) * -1 ELSE (- MAX(T0.""BalFcDeb"") - SUM(T1.""ReconSumFC"")) * -1 END ELSE 0 END AS ""Saldo120"",
                CASE WHEN MAX(T0.""DueDate"") < '" + FechaVcto + @"' AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') >= 121 THEN CASE WHEN MAX(T4.""Currency"") = MAX(TA.""MainCurncy"") THEN (- MAX(T0.""BalDueDeb"") - SUM(T1.""ReconSum"")) * -1 ELSE (- MAX(T0.""BalFcDeb"") - SUM(T1.""ReconSumFC"")) * -1 END ELSE 0 END AS ""Saldo121"",
                MAX(FCRNG.""U_FACTORINGID"") AS ""FacID"", MAX(FCRNG.""U_INSTITU"") AS ""FacEntidad"", MAX(FCRNG.""U_FACTORINGVCTO"") AS ""FacFecha"", MAX(FCRNG.""U_NUMOPER"") AS ""FacOper"", MAX(FCRNG.""U_RESFAC"") AS ""FacResp""
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
                LEFT OUTER JOIN (
                SELECT ""DocEntry"", ""DocNum"", ""TransId"", ""U_FACTORINGID"", ""U_INSTITU"", ""U_FACTORINGVCTO"", ""U_NUMOPER"", ""U_RESFAC"" FROM OINV 
                UNION ALL
                SELECT ""DocEntry"", ""DocNum"", ""TransId"", ""U_FACTORINGID"", ""U_INSTITU"", ""U_FACTORINGVCTO"", ""U_NUMOPER"", ""U_RESFAC"" FROM ORIN
                ) FCRNG ON T0.""TransId"" = FCRNG.""TransId""
                WHERE T0.""RefDate"" <= '" + FechaVcto + @"'
                AND  T4.""CardType"" = 'C'  AND  T4.""Balance"" <> 0  
                " + FiltroSN3 + @" 
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
                CASE WHEN MAX(T0.""DueDate"") < '" + FechaVcto + @"' AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') >= 121 THEN CASE WHEN MAX(T2.""Currency"") = MAX(TA.""MainCurncy"") THEN MAX(T0.""Debit"") - MAX(T0.""Credit"") ELSE MAX(T0.""FCDebit"") - MAX(T0.""FCCredit"") END ELSE 0 END AS ""Saldo121"",
                */
                CASE WHEN MAX(T0.""DueDate"") < '" + FechaVcto + @"' THEN CASE WHEN MAX(T2.""Currency"") = MAX(TA.""MainCurncy"") THEN (MAX(T0.""BalDueCred"") - MAX(T0.""BalDueDeb"")) * -1 ELSE (MAX(T0.""BalFcCred"") - MAX(T0.""BalFcDeb"")) * -1 END ELSE 0 END AS ""SaldoVenc"",
                CASE WHEN MAX(T0.""DueDate"") >= '" + FechaVcto + @"' THEN CASE WHEN MAX(T2.""Currency"") = MAX(TA.""MainCurncy"") THEN (MAX(T0.""BalDueCred"") - MAX(T0.""BalDueDeb"")) * -1 ELSE (MAX(T0.""BalFcCred"") - MAX(T0.""BalFcDeb"")) * -1 END ELSE 0 END AS ""AbonoFut"",
                CASE WHEN MAX(T0.""DueDate"") < '" + FechaVcto + @"' AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') >= 0 AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') <= 30 THEN CASE WHEN MAX(T2.""Currency"") = MAX(TA.""MainCurncy"") THEN (MAX(T0.""BalDueCred"") - MAX(T0.""BalDueDeb"")) * -1 ELSE (MAX(T0.""BalFcCred"") - MAX(T0.""BalFcDeb"")) * -1 END ELSE 0 END AS ""Saldo30"",
                CASE WHEN MAX(T0.""DueDate"") < '" + FechaVcto + @"' AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') >= 31 AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') <= 60 THEN CASE WHEN MAX(T2.""Currency"") = MAX(TA.""MainCurncy"") THEN (MAX(T0.""BalDueCred"") - MAX(T0.""BalDueDeb"")) * -1 ELSE (MAX(T0.""BalFcCred"") - MAX(T0.""BalFcDeb"")) * -1 END ELSE 0 END AS ""Saldo60"",
                CASE WHEN MAX(T0.""DueDate"") < '" + FechaVcto + @"' AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') >= 61 AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') <= 90 THEN CASE WHEN MAX(T2.""Currency"") = MAX(TA.""MainCurncy"") THEN (MAX(T0.""BalDueCred"") - MAX(T0.""BalDueDeb"")) * -1 ELSE (MAX(T0.""BalFcCred"") - MAX(T0.""BalFcDeb"")) * -1 END ELSE 0 END AS ""Saldo90"",
                CASE WHEN MAX(T0.""DueDate"") < '" + FechaVcto + @"' AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') >= 91 AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') <= 120 THEN CASE WHEN MAX(T2.""Currency"") = MAX(TA.""MainCurncy"") THEN (MAX(T0.""BalDueCred"") - MAX(T0.""BalDueDeb"")) * -1 ELSE (MAX(T0.""BalFcCred"") - MAX(T0.""BalFcDeb"")) * -1 END ELSE 0 END AS ""Saldo120"",
                CASE WHEN MAX(T0.""DueDate"") < '" + FechaVcto + @"' AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') >= 121 THEN CASE WHEN MAX(T2.""Currency"") = MAX(TA.""MainCurncy"") THEN (MAX(T0.""BalDueCred"") - MAX(T0.""BalDueDeb"")) * -1 ELSE (MAX(T0.""BalFcCred"") - MAX(T0.""BalFcDeb"")) * -1 END ELSE 0 END AS ""Saldo121"",
                MAX(FCRNG.""U_FACTORINGID"") AS ""FacID"", MAX(FCRNG.""U_INSTITU"") AS ""FacEntidad"", MAX(FCRNG.""U_FACTORINGVCTO"") AS ""FacFecha"", MAX(FCRNG.""U_NUMOPER"") AS ""FacOper"", MAX(FCRNG.""U_RESFAC"") AS ""FacResp""
                FROM  ""JDT1"" T0  
                INNER  JOIN ""OJDT"" T1  ON  T1.""TransId"" = T0.""TransId""   
                INNER  JOIN ""OCRD"" T2  ON  T2.""CardCode"" = T0.""ShortName""    
                LEFT OUTER  JOIN ""B1_JournalTransSourceView"" T3  
                ON  T3.""ObjType"" = T0.""TransType""  AND  T3.""DocEntry"" = T0.""CreatedBy""  
                AND  (T3.""TransType"" <> 'I'  OR  (T3.""TransType"" = 'I'  
                AND  T3.""InstlmntID"" = T0.""SourceLine"" ))  
				INNER JOIN ""OADM"" TA ON 1 = 1
                LEFT OUTER JOIN (
                SELECT ""DocEntry"", ""DocNum"", ""TransId"", ""U_FACTORINGID"", ""U_INSTITU"", ""U_FACTORINGVCTO"", ""U_NUMOPER"", ""U_RESFAC"" FROM OINV 
                UNION ALL
                SELECT ""DocEntry"", ""DocNum"", ""TransId"", ""U_FACTORINGID"", ""U_INSTITU"", ""U_FACTORINGVCTO"", ""U_NUMOPER"", ""U_RESFAC"" FROM ORIN
                ) FCRNG ON T0.""TransId"" = FCRNG.""TransId""
                WHERE T0.""RefDate"" <= '" + FechaVcto + @"' 
                AND  T2.""CardType"" = 'C'  AND  T2.""Balance"" <> 0  
                " + FiltroSN4 + @" 
                AND  (T0.""BalDueCred"" <> T0.""BalDueDeb""  OR  T0.""BalFcCred"" <> T0.""BalFcDeb"" ) 
                AND   NOT EXISTS 
                (SELECT U0.""TransId"", U0.""TransRowId"" 
                FROM  ""ITR1"" U0  
                INNER  JOIN ""OITR"" U1  ON  U1.""ReconNum"" = U0.""ReconNum""   
                WHERE T0.""TransId"" = U0.""TransId""  AND  T0.""Line_ID"" = U0.""TransRowId""  AND  U1.""ReconDate"" > '" + FechaVcto + @"'   
                GROUP BY U0.""TransId"", U0.""TransRowId"")   
                GROUP BY T0.""TransId"", T0.""Line_ID"", T0.""BPLName""
                ORDER BY 5
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

            oColumn = oColumns.Add("BalanceVis", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Saldo total";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 100;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "BalanceVis");

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
            oColumn.Width = 60;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "BaseRef");

            oColumn = oColumns.Add("FolioNum", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Folio";
            oColumn.TitleObject.Sortable = false;
            oColumn.Editable = false;
            oColumn.Visible = false;
            oColumn.Width = 60;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "FolioNum");

            oColumn = oColumns.Add("Origen", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Origen";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 120;
            oColumn.RightJustified = false;
            oColumn.DataBind.Bind(NombreDT, "Origen");

            oColumn = oColumns.Add("RefDate", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Fecha Contable";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = false;
            oColumn.Width = 70;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "RefDate");

            oColumn = oColumns.Add("DueDate", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Vencimiento";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 70;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "DueDate");

            oColumn = oColumns.Add("TaxDate", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Fecha Documento";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = false;
            oColumn.Width = 70;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "TaxDate");

            oColumn = oColumns.Add("Currency", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Moneda";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 40;
            oColumn.RightJustified = false;
            oColumn.DataBind.Bind(NombreDT, "Currency");

            oColumn = oColumns.Add("FCCurrency", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Moneda FC";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 40;
            oColumn.RightJustified = false;
            oColumn.DataBind.Bind(NombreDT, "FCCurrency");

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

            oColumn = oColumns.Add("FacID", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Fact. ID";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 70;
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
        }

        private static void CargarMatrixDetalle()
        {
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

            List<string> clientes = new List<string>();
            string FiltroSN3 = @" AND  T4.""CardCode"" IN (";
            string FiltroSN4 = @" AND  T2.""CardCode"" IN (";
            foreach (var cliente in Clientes)
            {
                clientes.Add(string.Format("'{0}'", cliente));
            }
            string joined = string.Join(",", clientes);
            FiltroSN3 += @joined + @")";
            FiltroSN4 += @joined + @")";

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

            string NombreDT = "dtDet";
            SAPbouiCOM.DataTable datatable = oForm.DataSources.DataTables.Item(NombreDT);

            string _query = @"
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
                CASE WHEN MAX(T0.""DueDate"") < '" + FechaVcto + @"' AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') >= 121 THEN CASE WHEN MAX(T4.""Currency"") = MAX(TA.""MainCurncy"") THEN MAX(T0.""Debit"") - MAX(T0.""Credit"") ELSE MAX(T0.""FCDebit"") - MAX(T0.""FCCredit"") END ELSE 0 END AS ""Saldo121"",
                */
                CASE WHEN MAX(T0.""DueDate"") < '" + FechaVcto + @"' THEN CASE WHEN MAX(T4.""Currency"") = MAX(TA.""MainCurncy"") THEN (MAX(T0.""BalDueCred"") + SUM(T1.""ReconSum"")) * -1 ELSE (MAX(T0.""BalFcCred"") + SUM(T1.""ReconSumFC"")) * -1 END ELSE 0 END AS ""SaldoVenc"",
                CASE WHEN MAX(T0.""DueDate"") >= '" + FechaVcto + @"' THEN CASE WHEN MAX(T4.""Currency"") = MAX(TA.""MainCurncy"") THEN (MAX(T0.""BalDueCred"") + SUM(T1.""ReconSum"")) * -1 ELSE (MAX(T0.""BalFcCred"") + SUM(T1.""ReconSumFC"")) * -1 END ELSE 0 END AS ""AbonoFut"",
                CASE WHEN MAX(T0.""DueDate"") < '" + FechaVcto + @"' AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') >= 0 AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') <= 30 THEN CASE WHEN MAX(T4.""Currency"") = MAX(TA.""MainCurncy"") THEN (MAX(T0.""BalDueCred"") + SUM(T1.""ReconSum"")) * -1 ELSE (MAX(T0.""BalFcCred"") + SUM(T1.""ReconSumFC"")) * -1 END ELSE 0 END AS ""Saldo30"",
                CASE WHEN MAX(T0.""DueDate"") < '" + FechaVcto + @"' AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') >= 31 AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') <= 60 THEN CASE WHEN MAX(T4.""Currency"") = MAX(TA.""MainCurncy"") THEN (MAX(T0.""BalDueCred"") + SUM(T1.""ReconSum"")) * -1 ELSE (MAX(T0.""BalFcCred"") + SUM(T1.""ReconSumFC"")) * -1 END ELSE 0 END AS ""Saldo60"",
                CASE WHEN MAX(T0.""DueDate"") < '" + FechaVcto + @"' AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') >= 61 AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') <= 90 THEN CASE WHEN MAX(T4.""Currency"") = MAX(TA.""MainCurncy"") THEN (MAX(T0.""BalDueCred"") + SUM(T1.""ReconSum"")) * -1 ELSE (MAX(T0.""BalFcCred"") + SUM(T1.""ReconSumFC"")) * -1 END ELSE 0 END AS ""Saldo90"",
                CASE WHEN MAX(T0.""DueDate"") < '" + FechaVcto + @"' AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') >= 91 AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') <= 120 THEN CASE WHEN MAX(T4.""Currency"") = MAX(TA.""MainCurncy"") THEN (MAX(T0.""BalDueCred"") + SUM(T1.""ReconSum"")) * -1 ELSE (MAX(T0.""BalFcCred"") + SUM(T1.""ReconSumFC"")) * -1 END ELSE 0 END AS ""Saldo120"",
                CASE WHEN MAX(T0.""DueDate"") < '" + FechaVcto + @"' AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') >= 121 THEN CASE WHEN MAX(T4.""Currency"") = MAX(TA.""MainCurncy"") THEN (MAX(T0.""BalDueCred"") + SUM(T1.""ReconSum"")) * -1 ELSE (MAX(T0.""BalFcCred"") + SUM(T1.""ReconSumFC"")) * -1 END ELSE 0 END AS ""Saldo121"",
                MAX(FCRNG.""U_FACTORINGID"") AS ""FacID"", MAX(FCRNG.""U_INSTITU"") AS ""FacEntidad"", MAX(FCRNG.""U_FACTORINGVCTO"") AS ""FacVcto"", MAX(FCRNG.""U_NUMOPER"") AS ""FacOper"", MAX(FCRNG.""U_RESFAC"") AS ""FacResp""
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
                LEFT OUTER JOIN (
                SELECT ""DocEntry"", ""DocNum"", ""TransId"", ""U_FACTORINGID"", ""U_INSTITU"", ""U_FACTORINGVCTO"", ""U_NUMOPER"", ""U_RESFAC"" FROM OINV 
                UNION ALL
                SELECT ""DocEntry"", ""DocNum"", ""TransId"", ""U_FACTORINGID"", ""U_INSTITU"", ""U_FACTORINGVCTO"", ""U_NUMOPER"", ""U_RESFAC"" FROM ORIN
                ) FCRNG ON T0.""TransId"" = FCRNG.""TransId""
                WHERE T0.""RefDate"" <= '" + FechaVcto + @"'  
                AND  T4.""CardType"" = 'C'  AND  T4.""Balance"" <> 0 
                " + FiltroSN + @" 
                " + FiltroSN3 + @" 
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
                CASE WHEN MAX(T0.""DueDate"") < '" + FechaVcto + @"' AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') >= 121 THEN CASE WHEN MAX(T4.""Currency"") = MAX(TA.""MainCurncy"") THEN MAX(T0.""Debit"") - MAX(T0.""Credit"") ELSE MAX(T0.""FCDebit"") - MAX(T0.""FCCredit"") END ELSE 0 END AS ""Saldo121"",
                */
                CASE WHEN MAX(T0.""DueDate"") < '" + FechaVcto + @"' THEN CASE WHEN MAX(T4.""Currency"") = MAX(TA.""MainCurncy"") THEN (- MAX(T0.""BalDueDeb"") - SUM(T1.""ReconSum"")) * -1 ELSE (- MAX(T0.""BalFcDeb"") - SUM(T1.""ReconSumFC"")) * -1 END ELSE 0 END AS ""SaldoVenc"",
                CASE WHEN MAX(T0.""DueDate"") >= '" + FechaVcto + @"' THEN CASE WHEN MAX(T4.""Currency"") = MAX(TA.""MainCurncy"") THEN (- MAX(T0.""BalDueDeb"") - SUM(T1.""ReconSum"")) * -1 ELSE (- MAX(T0.""BalFcDeb"") - SUM(T1.""ReconSumFC"")) * -1 END ELSE 0 END AS ""AbonoFut"",
                CASE WHEN MAX(T0.""DueDate"") < '" + FechaVcto + @"' AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') >= 0 AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') <= 30 THEN CASE WHEN MAX(T4.""Currency"") = MAX(TA.""MainCurncy"") THEN (- MAX(T0.""BalDueDeb"") - SUM(T1.""ReconSum"")) * -1 ELSE (- MAX(T0.""BalFcDeb"") - SUM(T1.""ReconSumFC"")) * -1 END ELSE 0 END AS ""Saldo30"",
                CASE WHEN MAX(T0.""DueDate"") < '" + FechaVcto + @"' AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') >= 31 AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') <= 60 THEN CASE WHEN MAX(T4.""Currency"") = MAX(TA.""MainCurncy"") THEN (- MAX(T0.""BalDueDeb"") - SUM(T1.""ReconSum"")) * -1 ELSE (- MAX(T0.""BalFcDeb"") - SUM(T1.""ReconSumFC"")) * -1 END ELSE 0 END AS ""Saldo60"",
                CASE WHEN MAX(T0.""DueDate"") < '" + FechaVcto + @"' AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') >= 61 AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') <= 90 THEN CASE WHEN MAX(T4.""Currency"") = MAX(TA.""MainCurncy"") THEN (- MAX(T0.""BalDueDeb"") - SUM(T1.""ReconSum"")) * -1 ELSE (- MAX(T0.""BalFcDeb"") - SUM(T1.""ReconSumFC"")) * -1 END ELSE 0 END AS ""Saldo90"",
                CASE WHEN MAX(T0.""DueDate"") < '" + FechaVcto + @"' AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') >= 91 AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') <= 120 THEN CASE WHEN MAX(T4.""Currency"") = MAX(TA.""MainCurncy"") THEN (- MAX(T0.""BalDueDeb"") - SUM(T1.""ReconSum"")) * -1 ELSE (- MAX(T0.""BalFcDeb"") - SUM(T1.""ReconSumFC"")) * -1 END ELSE 0 END AS ""Saldo120"",
                CASE WHEN MAX(T0.""DueDate"") < '" + FechaVcto + @"' AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') >= 121 THEN CASE WHEN MAX(T4.""Currency"") = MAX(TA.""MainCurncy"") THEN (- MAX(T0.""BalDueDeb"") - SUM(T1.""ReconSum"")) * -1 ELSE (- MAX(T0.""BalFcDeb"") - SUM(T1.""ReconSumFC"")) * -1 END ELSE 0 END AS ""Saldo121"",
                MAX(FCRNG.""U_FACTORINGID"") AS ""FacID"", MAX(FCRNG.""U_INSTITU"") AS ""FacEntidad"", MAX(FCRNG.""U_FACTORINGVCTO"") AS ""FacVcto"", MAX(FCRNG.""U_NUMOPER"") AS ""FacOper"", MAX(FCRNG.""U_RESFAC"") AS ""FacResp""
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
                LEFT OUTER JOIN (
                SELECT ""DocEntry"", ""DocNum"", ""TransId"", ""U_FACTORINGID"", ""U_INSTITU"", ""U_FACTORINGVCTO"", ""U_NUMOPER"", ""U_RESFAC"" FROM OINV 
                UNION ALL
                SELECT ""DocEntry"", ""DocNum"", ""TransId"", ""U_FACTORINGID"", ""U_INSTITU"", ""U_FACTORINGVCTO"", ""U_NUMOPER"", ""U_RESFAC"" FROM ORIN
                ) FCRNG ON T0.""TransId"" = FCRNG.""TransId""
                WHERE T0.""RefDate"" <= '" + FechaVcto + @"'
                AND  T4.""CardType"" = 'C'  AND  T4.""Balance"" <> 0  
                " + FiltroSN + @" 
                " + FiltroSN3 + @" 
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
                CASE WHEN MAX(T0.""DueDate"") < '" + FechaVcto + @"' AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') >= 121 THEN CASE WHEN MAX(T2.""Currency"") = MAX(TA.""MainCurncy"") THEN MAX(T0.""Debit"") - MAX(T0.""Credit"") ELSE MAX(T0.""FCDebit"") - MAX(T0.""FCCredit"") END ELSE 0 END AS ""Saldo121"",
                */
                CASE WHEN MAX(T0.""DueDate"") < '" + FechaVcto + @"' THEN CASE WHEN MAX(T2.""Currency"") = MAX(TA.""MainCurncy"") THEN (MAX(T0.""BalDueCred"") - MAX(T0.""BalDueDeb"")) * -1 ELSE (MAX(T0.""BalFcCred"") - MAX(T0.""BalFcDeb"")) * -1 END ELSE 0 END AS ""SaldoVenc"",
                CASE WHEN MAX(T0.""DueDate"") >= '" + FechaVcto + @"' THEN CASE WHEN MAX(T2.""Currency"") = MAX(TA.""MainCurncy"") THEN (MAX(T0.""BalDueCred"") - MAX(T0.""BalDueDeb"")) * -1 ELSE (MAX(T0.""BalFcCred"") - MAX(T0.""BalFcDeb"")) * -1 END ELSE 0 END AS ""AbonoFut"",
                CASE WHEN MAX(T0.""DueDate"") < '" + FechaVcto + @"' AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') >= 0 AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') <= 30 THEN CASE WHEN MAX(T2.""Currency"") = MAX(TA.""MainCurncy"") THEN (MAX(T0.""BalDueCred"") - MAX(T0.""BalDueDeb"")) * -1 ELSE (MAX(T0.""BalFcCred"") - MAX(T0.""BalFcDeb"")) * -1 END ELSE 0 END AS ""Saldo30"",
                CASE WHEN MAX(T0.""DueDate"") < '" + FechaVcto + @"' AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') >= 31 AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') <= 60 THEN CASE WHEN MAX(T2.""Currency"") = MAX(TA.""MainCurncy"") THEN (MAX(T0.""BalDueCred"") - MAX(T0.""BalDueDeb"")) * -1 ELSE (MAX(T0.""BalFcCred"") - MAX(T0.""BalFcDeb"")) * -1 END ELSE 0 END AS ""Saldo60"",
                CASE WHEN MAX(T0.""DueDate"") < '" + FechaVcto + @"' AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') >= 61 AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') <= 90 THEN CASE WHEN MAX(T2.""Currency"") = MAX(TA.""MainCurncy"") THEN (MAX(T0.""BalDueCred"") - MAX(T0.""BalDueDeb"")) * -1 ELSE (MAX(T0.""BalFcCred"") - MAX(T0.""BalFcDeb"")) * -1 END ELSE 0 END AS ""Saldo90"",
                CASE WHEN MAX(T0.""DueDate"") < '" + FechaVcto + @"' AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') >= 91 AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') <= 120 THEN CASE WHEN MAX(T2.""Currency"") = MAX(TA.""MainCurncy"") THEN (MAX(T0.""BalDueCred"") - MAX(T0.""BalDueDeb"")) * -1 ELSE (MAX(T0.""BalFcCred"") - MAX(T0.""BalFcDeb"")) * -1 END ELSE 0 END AS ""Saldo120"",
                CASE WHEN MAX(T0.""DueDate"") < '" + FechaVcto + @"' AND DATEDIFF(DAY, MAX(T0.""DueDate""), '" + FechaVcto + @"') >= 121 THEN CASE WHEN MAX(T2.""Currency"") = MAX(TA.""MainCurncy"") THEN (MAX(T0.""BalDueCred"") - MAX(T0.""BalDueDeb"")) * -1 ELSE (MAX(T0.""BalFcCred"") - MAX(T0.""BalFcDeb"")) * -1 END ELSE 0 END AS ""Saldo121"",
                MAX(FCRNG.""U_FACTORINGID"") AS ""FacID"", MAX(FCRNG.""U_INSTITU"") AS ""FacEntidad"", MAX(FCRNG.""U_FACTORINGVCTO"") AS ""FacVcto"", MAX(FCRNG.""U_NUMOPER"") AS ""FacOper"", MAX(FCRNG.""U_RESFAC"") AS ""FacResp""
                FROM  ""JDT1"" T0  
                INNER  JOIN ""OJDT"" T1  ON  T1.""TransId"" = T0.""TransId""   
                INNER  JOIN ""OCRD"" T2  ON  T2.""CardCode"" = T0.""ShortName""    
                LEFT OUTER  JOIN ""B1_JournalTransSourceView"" T3  
                ON  T3.""ObjType"" = T0.""TransType""  AND  T3.""DocEntry"" = T0.""CreatedBy""  
                AND  (T3.""TransType"" <> 'I'  OR  (T3.""TransType"" = 'I'  
                AND  T3.""InstlmntID"" = T0.""SourceLine"" ))  
				INNER JOIN ""OADM"" TA ON 1 = 1
                LEFT OUTER JOIN (
                SELECT ""DocEntry"", ""DocNum"", ""TransId"", ""U_FACTORINGID"", ""U_INSTITU"", ""U_FACTORINGVCTO"", ""U_NUMOPER"", ""U_RESFAC"" FROM OINV 
                UNION ALL
                SELECT ""DocEntry"", ""DocNum"", ""TransId"", ""U_FACTORINGID"", ""U_INSTITU"", ""U_FACTORINGVCTO"", ""U_NUMOPER"", ""U_RESFAC"" FROM ORIN
                ) FCRNG ON T0.""TransId"" = FCRNG.""TransId""
                WHERE T0.""RefDate"" <= '" + FechaVcto + @"' 
                AND  T2.""CardType"" = 'C'  AND  T2.""Balance"" <> 0  
                " + FiltroSN2 + @" 
                " + FiltroSN4 + @" 
                AND  (T0.""BalDueCred"" <> T0.""BalDueDeb""  OR  T0.""BalFcCred"" <> T0.""BalFcDeb"" ) 
                AND   NOT EXISTS 
                (SELECT U0.""TransId"", U0.""TransRowId"" 
                FROM  ""ITR1"" U0  
                INNER  JOIN ""OITR"" U1  ON  U1.""ReconNum"" = U0.""ReconNum""   
                WHERE T0.""TransId"" = U0.""TransId""  AND  T0.""Line_ID"" = U0.""TransRowId""  AND  U1.""ReconDate"" > '" + FechaVcto + @"'   
                GROUP BY U0.""TransId"", U0.""TransRowId"")   
                GROUP BY T0.""TransId"", T0.""Line_ID"", T0.""BPLName""
                ORDER BY 5
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

        private static void GenerarCobranza(SAPbouiCOM.DataTable dtDoc, string columna)
        {
            int resp = 0;
            var cobranza = ObtenerCobranzaSeleccionada(dtDoc, columna);
            if (cobranza.Documentos != null && cobranza.Documentos.Count > 0)
            {
                Application.SBO_Application.StatusBar.SetText("Generando cobranza. Espere...", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);
                GuardarLogCobranza(cobranza);
                Application.SBO_Application.StatusBar.SetText(string.Format("Log de cobranza guardado correctamente."), SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success);
                resp = Application.SBO_Application.MessageBox("¿Desea enviar correos de notificación?", 2, "Si", "No");
                if (resp.Equals(1))
                {
                    Application.SBO_Application.StatusBar.SetText("Enviando correos de notificación. Espere...", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);
                    EnviarCorreos(cobranza);
                    Application.SBO_Application.StatusBar.SetText("Cobranza generada correctamente.", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success);
                }
            }
        }

        private static Clases.Cobranza ObtenerCobranzaSeleccionada(SAPbouiCOM.DataTable dtDoc, string columna)
        {
            string nivel = oForm.DataSources.UserDataSources.Item("Nivel").Value;
            string idioma = oForm.DataSources.UserDataSources.Item("Idioma").Value;
            string obs = oForm.DataSources.UserDataSources.Item("Obs").Value;

            Clases.Cobranza cobranza = new Clases.Cobranza();
            mtxDet.FlushToDataSource();
            if (!dtDoc.IsEmpty)
            {
                for (int i = 0; i < dtDoc.Rows.Count; i++)
                {
                    if (dtDoc.GetValue(columna, i).ToString().Equals("Y"))
                    {
                        Clases.CobranzaDocs documento = new Clases.CobranzaDocs();
                        documento.U_OBJTYPE = dtDoc.GetValue("TransType", i).ToString();
                        documento.U_DOCENTRY = (int)dtDoc.GetValue("CreatedBy", i);
                        string tipodoc = string.Empty;
                        switch (dtDoc.GetValue("TransType", i).ToString())
                        {
                            case "13":
                                tipodoc = "FC";
                                break;
                            case "14":
                                tipodoc = "NC";
                                break;
                            default:
                                tipodoc = dtDoc.GetValue("TransType", i).ToString();
                                break;
                        }
                        documento.U_TIPODOC = tipodoc;
                        documento.U_DOCNUM = Convert.ToInt32(dtDoc.GetValue("BaseRef", i));
                        documento.U_FOLIONUM = Convert.ToInt32(dtDoc.GetValue("FolioNum", i));
                        var docdate = dtDoc.GetValue("RefDate", i);
                        documento.U_DOCDATE = (DateTime)docdate;
                        var docduedate = dtDoc.GetValue("DueDate", i);
                        documento.U_DOCDUEDATE = (DateTime)docduedate;
                        var taxdate = dtDoc.GetValue("TaxDate", i);
                        documento.U_TAXDATE = (DateTime)taxdate;
                        documento.U_DOCCUR = dtDoc.GetValue("Currency", i).ToString();
                        documento.U_DOCCURFC = dtDoc.GetValue("FCCurrency", i).ToString();
                        documento.U_DOCTOTAL = (double)dtDoc.GetValue("SaldoVenc", i);
                        documento.U_FUTDUE = (double)dtDoc.GetValue("AbonoFut", i);
                        documento.U_CARDCODE = dtDoc.GetValue("CardCode", i).ToString();
                        documento.U_CARDNAME = SBO.ConsultasSBO.ObtenerNombreSN(dtDoc.GetValue("CardCode", i).ToString());
                        documento.U_EMAIL = SBO.ConsultasSBO.ObtenerMailSN(dtDoc.GetValue("CardCode", i).ToString());
                        documento.U_EMAILTYPE = nivel;
                        documento.U_EMAILLANG = idioma;
                        documento.U_OBS = obs;
                        documento.U_FACID= (int)dtDoc.GetValue("FacID", i);
                        documento.U_FACENTIDAD = dtDoc.GetValue("FacEntidad", i).ToString();
                        var facfecha = dtDoc.GetValue("FacFecha", i);
                        if (facfecha != null)
                        {
                            documento.U_FACFECHA = (DateTime)facfecha;
                        }
                        documento.U_FACOPER = dtDoc.GetValue("FacOper", i).ToString();
                        documento.U_FACRESP = dtDoc.GetValue("FacResp", i).ToString();
                        cobranza.Documentos.Add(documento);
                    }
                }
            }
            oForm.Freeze(true);
            mtxDet.LoadFromDataSource();
            oForm.Freeze(false);
            return cobranza;
        }

        private static void GuardarLogCobranza(Clases.Cobranza cobranza)
        {
            var result = SBO.ModeloSBO.AddLogCobranza(cobranza);
        }

        private static void EnviarCorreos(Clases.Cobranza cobranza)
        {
            var listDocumentos = cobranza.Documentos;
            var listClientes = listDocumentos.GroupBy(x => x.U_CARDCODE).ToList();

            listClientes.ForEach(cliente =>
            {
                var lineas = listDocumentos.Where(z => z.U_CARDCODE == cliente.Key).ToList();
                // Cuerpo del Correo
                string nivel = oForm.DataSources.UserDataSources.Item("Nivel").Value;
                string idioma = oForm.DataSources.UserDataSources.Item("Idioma").Value;
                string obs = oForm.DataSources.UserDataSources.Item("Obs").Value;
                string bodyhtml = CreateBodyHtml(cliente.Key, cobranza, lineas, nivel, idioma, obs);
                // Envío de Correo
                if (idioma.Equals("Español"))
                {
                    SendMailES(cliente.Key, bodyhtml);
                }
                else
                {
                    SendMailUS(cliente.Key, bodyhtml);
                }
            });
        }

        private static string CreateBodyHtml_old(string clienteKey, Clases.Cobranza cobranza, List<Clases.CobranzaDocs> lineas, string nivel = "0-30", string idioma = "Español", string observaciones = "")
        {
            var conFactList = lineas.Where(x => x.U_FACENTIDAD != string.Empty).ToList();
            bool conFact = conFactList.Count > 0 ? true : false;
            string titulofactoring = @"<th scope = ""col"" > Factoring </th>";
            string valorfactoring = @"<td style = ""text-align:center"" > @FACTORING@ </td>";
            string bodyhtml = @"";
            switch (nivel)
            {
                case "31-60":
                    if (idioma.Equals("Español"))
                    {
                        bodyhtml = @"
                            <p>Estimado cliente, al día de hoy su cuenta por cobrar registra facturas vencidas de más de 30 días.</p>
                            <table border=""0"" cellpadding=""1"" cellspacing=""1"" style=""width: 800px"" summary=""Documentos"">
                            <thead>
                            <tr>
                                <th scope = ""col"" > N° de Factura </th>
                                <th scope = ""col"" > Fecha emisión </th>
                                <th scope = ""col"" > Fecha de Vencimiento </th>
                                <th scope = ""col"" > Días de mora </th>
                                <th scope = ""col"" > Moneda </th>
                                <th scope = ""col"" > Saldo Vencido </th>
                                @COLUMNAFACTORING@
                            </tr>
                            </thead>
                            <tbody>";
                        bodyhtml = conFact ? bodyhtml.Replace("@COLUMNAFACTORING@", @titulofactoring) : bodyhtml.Replace("@COLUMNAFACTORING@", string.Empty);

                        lineas.ForEach(documento =>
                        {
                            string folio = documento.U_FOLIONUM.ToString();
                            string fecha = documento.U_DOCDATE.ToString("dd-MM-yyyy");
                            string fechavcto = documento.U_DOCDUEDATE.ToString("dd-MM-yyyy");
                            TimeSpan diffdate = DateTime.Now.Date - documento.U_DOCDUEDATE.Date;
                            string mora = diffdate.Days.ToString();
                            string moneda = documento.U_DOCCUR;
                            string monto = documento.U_DOCTOTAL.ToString("N", new System.Globalization.CultureInfo("es-CL"));
                            string factoring = documento.U_FACENTIDAD;
                            string lineabody = @"
                            <tr>
                            <td style = ""text-align:center"" > @FOLIO@ </td>
                            <td style = ""text-align:center"" > @FECHA@ </td>
                            <td style = ""text-align:center"" > @FECHAVCTO@ </td>
                            <td style = ""text-align:center"" > @MORA@ </td>
                            <td style = ""text-align:center"" > @MONEDA@ </td>
                            <td style = ""text-align:center"" > @MONTO@ </td>
                            @VALORFACTORING@
                            </tr>";
                            lineabody = conFact ? lineabody.Replace("@VALORFACTORING@", valorfactoring) : lineabody.Replace("@VALORFACTORING@", string.Empty);
                            lineabody = lineabody.Replace("@FOLIO@", folio);
                            lineabody = lineabody.Replace("@FECHA@", fecha);
                            lineabody = lineabody.Replace("@FECHAVCTO@", fechavcto);
                            lineabody = lineabody.Replace("@MORA@", mora);
                            lineabody = lineabody.Replace("@MONEDA@", moneda);
                            lineabody = lineabody.Replace("@MONTO@", monto);
                            lineabody = lineabody.Replace("@FACTORING@", factoring);
                            bodyhtml += @lineabody;
                        });
                        bodyhtml += @"
                        </tbody>
                        </table>
                        <p>Dado lo anterior, favor indicar fecha de pago en calidad de urgente, a fin de evitar inconvenientes con futuros despachos.</>
                        <p>@OBSERVACIONES@</>
                        <p>Dudas o consultas contactar con el departamento de crédito y cobranzas de empresas Agrotop.</p>
                        <p>En espera de sus comentarios, saluda atentamente;</p>
                        <p><img alt="""" src=""https://empresasagrotop.cl/firma_cobranza.jpg"" /></p>
                        ";
                    }
                    else
                    {
                        bodyhtml = @"
                            <p>Dear customer.</p>
                            <p>We would like to remember you that your account debt status shows invoices overdue for more than 30 days.</p>
                            <table border=""0"" cellpadding=""1"" cellspacing=""1"" style=""width: 800px"" summary=""Documentos"">
                            <thead>
                            <tr>
                                <th scope = ""col"" > Invoice Number </th>
                                <th scope = ""col"" > Date of Issue </th>
                                <th scope = ""col"" > Expiration Date </th>
                                <th scope = ""col"" > Days Past Due </th>
                                <th scope = ""col"" > Currency </th>
                                <th scope = ""col"" > Overdue Amount </th>
                                @COLUMNAFACTORING@
                            </tr>
                            </thead>
                            <tbody>";
                        bodyhtml = conFact ? bodyhtml.Replace("@COLUMNAFACTORING@", @titulofactoring) : bodyhtml.Replace("@COLUMNAFACTORING@", string.Empty);

                        lineas.ForEach(documento =>
                        {
                            string folio = documento.U_FOLIONUM.ToString();
                            string fecha = documento.U_DOCDATE.ToString("dd-MM-yyyy");
                            string fechavcto = documento.U_DOCDUEDATE.ToString("dd-MM-yyyy");
                            TimeSpan diffdate = DateTime.Now.Date - documento.U_DOCDUEDATE.Date;
                            string mora = diffdate.Days.ToString();
                            string moneda = documento.U_DOCCUR;
                            string monto = documento.U_DOCTOTAL.ToString("N", new System.Globalization.CultureInfo("es-CL"));
                            string factoring = documento.U_FACENTIDAD;
                            string lineabody = @"
                            <tr>
                            <td style = ""text-align:center"" > @FOLIO@ </td>
                            <td style = ""text-align:center"" > @FECHA@ </td>
                            <td style = ""text-align:center"" > @FECHAVCTO@ </td>
                            <td style = ""text-align:center"" > @MORA@ </td>
                            <td style = ""text-align:center"" > @MONEDA@ </td>
                            <td style = ""text-align:center"" > @MONTO@ </td>
                            @VALORFACTORING@
                            </tr>";
                            lineabody = conFact ? lineabody.Replace("@VALORFACTORING@", valorfactoring) : lineabody.Replace("@VALORFACTORING@", string.Empty);
                            lineabody = lineabody.Replace("@FOLIO@", folio);
                            lineabody = lineabody.Replace("@FECHA@", fecha);
                            lineabody = lineabody.Replace("@FECHAVCTO@", fechavcto);
                            lineabody = lineabody.Replace("@MORA@", mora);
                            lineabody = lineabody.Replace("@MONEDA@", moneda);
                            lineabody = lineabody.Replace("@MONTO@", monto);
                            lineabody = lineabody.Replace("@FACTORING@", factoring);
                            bodyhtml += @lineabody;
                        });
                        bodyhtml += @"
                        </tbody>
                        </table>
                        <pConsidering the above, please let us know your payment commitment dates in order to avoid any problem on future deliveries.</>
                        <p>@OBSERVACIONES@</>
                        <p>If you have any questions please feel free to get in touch with us.</p>
                        <p>Awaiting your comments,</p>
                        <p>Best regards,</p>
                        <p><img alt="""" src=""https://empresasagrotop.cl/firma_cobranza.jpg"" /></p>
                        ";
                    }
                    break;
                case "60+":
                    if (idioma.Equals("Español"))
                    {
                        bodyhtml = @"
                            <p>Estimado cliente, al día de hoy su cuenta por cobrar registra facturas vencidas de más de 60 días.</p>
                            <table border=""0"" cellpadding=""1"" cellspacing=""1"" style=""width: 800px"" summary=""Documentos"">
                            <thead>
                            <tr>
                                <th scope = ""col"" > N° de Factura </th>
                                <th scope = ""col"" > Fecha emisión </th>
                                <th scope = ""col"" > Fecha de Vencimiento </th>
                                <th scope = ""col"" > Días de mora </th>
                                <th scope = ""col"" > Moneda </th>
                                <th scope = ""col"" > Saldo Vencido </th>
                                @COLUMNAFACTORING@
                            </tr>
                            </thead>
                            <tbody>";
                        bodyhtml = conFact ? bodyhtml.Replace("@COLUMNAFACTORING@", @titulofactoring) : bodyhtml.Replace("@COLUMNAFACTORING@", string.Empty);

                        lineas.ForEach(documento =>
                        {
                            string folio = documento.U_FOLIONUM.ToString();
                            string fecha = documento.U_DOCDATE.ToString("dd-MM-yyyy");
                            string fechavcto = documento.U_DOCDUEDATE.ToString("dd-MM-yyyy");
                            TimeSpan diffdate = DateTime.Now.Date - documento.U_DOCDUEDATE.Date;
                            string mora = diffdate.Days.ToString();
                            string moneda = documento.U_DOCCUR;
                            string monto = documento.U_DOCTOTAL.ToString("N", new System.Globalization.CultureInfo("es-CL"));
                            string factoring = documento.U_FACENTIDAD;
                            string lineabody = @"
                            <tr>
                            <td style = ""text-align:center"" > @FOLIO@ </td>
                            <td style = ""text-align:center"" > @FECHA@ </td>
                            <td style = ""text-align:center"" > @FECHAVCTO@ </td>
                            <td style = ""text-align:center"" > @MORA@ </td>
                            <td style = ""text-align:center"" > @MONEDA@ </td>
                            <td style = ""text-align:center"" > @MONTO@ </td>
                            @VALORFACTORING@
                            </tr>";
                            lineabody = conFact ? lineabody.Replace("@VALORFACTORING@", valorfactoring) : lineabody.Replace("@VALORFACTORING@", string.Empty);
                            lineabody = lineabody.Replace("@FOLIO@", folio);
                            lineabody = lineabody.Replace("@FECHA@", fecha);
                            lineabody = lineabody.Replace("@FECHAVCTO@", fechavcto);
                            lineabody = lineabody.Replace("@MORA@", mora);
                            lineabody = lineabody.Replace("@MONEDA@", moneda);
                            lineabody = lineabody.Replace("@MONTO@", monto);
                            lineabody = lineabody.Replace("@FACTORING@", factoring);
                            bodyhtml += @lineabody;
                        });
                        bodyhtml += @"
                        </tbody>
                        </table>
                        <p>Dado lo anterior, es imprescindible regularice en el más breve plazo a fin de evitar la cancelación de la línea de crédito y publicación de la deuda.</>
                        <p>@OBSERVACIONES@</>
                        <p>Ante consultas contactar con el departamento de crédito y cobranzas de empresas Agrotop.</p>
                        <p>Sin otro particular, saluda atentamente;</p>
                        <p><img alt="""" src=""https://empresasagrotop.cl/firma_cobranza.jpg"" /></p>
                        ";
                    }
                    else
                    {
                        bodyhtml = @"
                            <p>Dear customer.</p>
                            <p>We would like to warn you that your account debt status shows invoices overdue for more than 60 days.</p>
                            <table border=""0"" cellpadding=""1"" cellspacing=""1"" style=""width: 800px"" summary=""Documentos"">
                            <thead>
                            <tr>
                                <th scope = ""col"" > Invoice Number </th>
                                <th scope = ""col"" > Date of Issue </th>
                                <th scope = ""col"" > Expiration Date </th>
                                <th scope = ""col"" > Days Past Due </th>
                                <th scope = ""col"" > Currency </th>
                                <th scope = ""col"" > Overdue Amount </th>
                                @COLUMNAFACTORING@
                            </tr>
                            </thead>
                            <tbody>";
                        bodyhtml = conFact ? bodyhtml.Replace("@COLUMNAFACTORING@", @titulofactoring) : bodyhtml.Replace("@COLUMNAFACTORING@", string.Empty);

                        lineas.ForEach(documento =>
                        {
                            string folio = documento.U_FOLIONUM.ToString();
                            string fecha = documento.U_DOCDATE.ToString("dd-MM-yyyy");
                            string fechavcto = documento.U_DOCDUEDATE.ToString("dd-MM-yyyy");
                            TimeSpan diffdate = DateTime.Now.Date - documento.U_DOCDUEDATE.Date;
                            string mora = diffdate.Days.ToString();
                            string moneda = documento.U_DOCCUR;
                            string monto = documento.U_DOCTOTAL.ToString("N", new System.Globalization.CultureInfo("es-CL"));
                            string factoring = documento.U_FACENTIDAD;
                            string lineabody = @"
                            <tr>
                            <td style = ""text-align:center"" > @FOLIO@ </td>
                            <td style = ""text-align:center"" > @FECHA@ </td>
                            <td style = ""text-align:center"" > @FECHAVCTO@ </td>
                            <td style = ""text-align:center"" > @MORA@ </td>
                            <td style = ""text-align:center"" > @MONEDA@ </td>
                            <td style = ""text-align:center"" > @MONTO@ </td>
                            @VALORFACTORING@
                            </tr>";
                            lineabody = conFact ? lineabody.Replace("@VALORFACTORING@", valorfactoring) : lineabody.Replace("@VALORFACTORING@", string.Empty);
                            lineabody = lineabody.Replace("@FOLIO@", folio);
                            lineabody = lineabody.Replace("@FECHA@", fecha);
                            lineabody = lineabody.Replace("@FECHAVCTO@", fechavcto);
                            lineabody = lineabody.Replace("@MORA@", mora);
                            lineabody = lineabody.Replace("@MONEDA@", moneda);
                            lineabody = lineabody.Replace("@MONTO@", monto);
                            lineabody = lineabody.Replace("@FACTORING@", factoring);
                            bodyhtml += @lineabody;
                        });
                        bodyhtml += @"
                        </tbody>
                        </table>
                        <pConsidering the above, it is mandatory that you may pay off your due invoices as soon as possible in order to avoid suspension of your line of credit.</>
                        <p>@OBSERVACIONES@</>
                        <p>If you have any questions please feel free to get in touch with us.</p>
                        <p>Awaiting your comments,</p>
                        <p>Best regards,</p>
                        <p><img alt="""" src=""https://empresasagrotop.cl/firma_cobranza.jpg"" /></p>
                        ";
                    }
                    break;
                default:
                    if (idioma.Equals("Español"))
                    {
                        bodyhtml = @"
                            <p>Estimado cliente, recordamos vencimientos asociados a su cuenta por cobrar.</p>
                            <table border=""0"" cellpadding=""1"" cellspacing=""1"" style=""width: 800px"" summary=""Documentos"">
                            <thead>
                            <tr>
                                <th scope = ""col"" > N° de Factura </th>
                                <th scope = ""col"" > Fecha emisión </th>
                                <th scope = ""col"" > Fecha de Vencimiento </th>
                                <th scope = ""col"" > Días de mora </th>
                                <th scope = ""col"" > Moneda </th>
                                <th scope = ""col"" > Saldo Vencido </th>
                                @COLUMNAFACTORING@
                            </tr>
                            </thead>
                            <tbody>";
                        bodyhtml = conFact ? bodyhtml.Replace("@COLUMNAFACTORING@", @titulofactoring) : bodyhtml.Replace("@COLUMNAFACTORING@", string.Empty);

                        lineas.ForEach(documento =>
                        {
                            string folio = documento.U_FOLIONUM.ToString();
                            string fecha = documento.U_DOCDATE.ToString("dd-MM-yyyy");
                            string fechavcto = documento.U_DOCDUEDATE.ToString("dd-MM-yyyy");
                            TimeSpan diffdate = DateTime.Now.Date - documento.U_DOCDUEDATE.Date;
                            string mora = diffdate.Days.ToString();
                            string moneda = documento.U_DOCCUR;
                            string monto = documento.U_DOCTOTAL.ToString("N", new System.Globalization.CultureInfo("es-CL"));
                            string factoring = documento.U_FACENTIDAD;
                            string lineabody = @"
                            <tr>
                            <td style = ""text-align:center"" > @FOLIO@ </td>
                            <td style = ""text-align:center"" > @FECHA@ </td>
                            <td style = ""text-align:center"" > @FECHAVCTO@ </td>
                            <td style = ""text-align:center"" > @MORA@ </td>
                            <td style = ""text-align:center"" > @MONEDA@ </td>
                            <td style = ""text-align:center"" > @MONTO@ </td>
                            @VALORFACTORING@
                            </tr>";
                            lineabody = conFact ? lineabody.Replace("@VALORFACTORING@", valorfactoring) : lineabody.Replace("@VALORFACTORING@", string.Empty);
                            lineabody = lineabody.Replace("@FOLIO@", folio);
                            lineabody = lineabody.Replace("@FECHA@", fecha);
                            lineabody = lineabody.Replace("@FECHAVCTO@", fechavcto);
                            lineabody = lineabody.Replace("@MORA@", mora);
                            lineabody = lineabody.Replace("@MONEDA@", moneda);
                            lineabody = lineabody.Replace("@MONTO@", monto);
                            lineabody = lineabody.Replace("@FACTORING@", factoring);
                            bodyhtml += @lineabody;
                        });
                        bodyhtml += @"
                        </tbody>
                        </table>
                        <p>Agradeceremos gestionar los pagos correspondientes a la brevedad.</>
                        <p>@OBSERVACIONES@</>
                        <p>Dudas o consultas contactar con el departamento de crédito y cobranzas de empresas Agrotop.</p>
                        <p>Quedo atenta, saludos;</p>
                        <p><img alt="""" src=""https://empresasagrotop.cl/firma_cobranza.jpg"" /></p>
                        ";
                    }
                    else
                    {
                        bodyhtml = @"
                            <p>Dear customer.</p>
                            <p>We would like to remember you the following overdue invoices associated to your debt status.</p>
                            <table border=""0"" cellpadding=""1"" cellspacing=""1"" style=""width: 800px"" summary=""Documentos"">
                            <thead>
                            <tr>
                                <th scope = ""col"" > Invoice Number </th>
                                <th scope = ""col"" > Date of Issue </th>
                                <th scope = ""col"" > Expiration Date </th>
                                <th scope = ""col"" > Days Past Due </th>
                                <th scope = ""col"" > Currency </th>
                                <th scope = ""col"" > Overdue Amount </th>
                                @COLUMNAFACTORING@
                            </tr>
                            </thead>
                            <tbody>";
                        bodyhtml = conFact ? bodyhtml.Replace("@COLUMNAFACTORING@", @titulofactoring) : bodyhtml.Replace("@COLUMNAFACTORING@", string.Empty);

                        lineas.ForEach(documento =>
                        {
                            string folio = documento.U_FOLIONUM.ToString();
                            string fecha = documento.U_DOCDATE.ToString("dd-MM-yyyy");
                            string fechavcto = documento.U_DOCDUEDATE.ToString("dd-MM-yyyy");
                            TimeSpan diffdate = DateTime.Now.Date - documento.U_DOCDUEDATE.Date;
                            string mora = diffdate.Days.ToString();
                            string moneda = documento.U_DOCCUR;
                            string monto = documento.U_DOCTOTAL.ToString("N", new System.Globalization.CultureInfo("es-CL"));
                            string factoring = documento.U_FACENTIDAD;
                            string lineabody = @"
                            <tr>
                            <td style = ""text-align:center"" > @FOLIO@ </td>
                            <td style = ""text-align:center"" > @FECHA@ </td>
                            <td style = ""text-align:center"" > @FECHAVCTO@ </td>
                            <td style = ""text-align:center"" > @MORA@ </td>
                            <td style = ""text-align:center"" > @MONEDA@ </td>
                            <td style = ""text-align:center"" > @MONTO@ </td>
                            @VALORFACTORING@
                            </tr>";
                            lineabody = conFact ? lineabody.Replace("@VALORFACTORING@", valorfactoring) : lineabody.Replace("@VALORFACTORING@", string.Empty);
                            lineabody = lineabody.Replace("@FOLIO@", folio);
                            lineabody = lineabody.Replace("@FECHA@", fecha);
                            lineabody = lineabody.Replace("@FECHAVCTO@", fechavcto);
                            lineabody = lineabody.Replace("@MORA@", mora);
                            lineabody = lineabody.Replace("@MONEDA@", moneda);
                            lineabody = lineabody.Replace("@MONTO@", monto);
                            lineabody = lineabody.Replace("@FACTORING@", factoring);
                            bodyhtml += @lineabody;
                        });
                        bodyhtml += @"
                        </tbody>
                        </table>
                        <p>If you have any questions please feel free to get in touch with us.</p>
                        <p>@OBSERVACIONES@</>
                        <p>Best regards,</p>
                        <p><img alt="""" src=""https://empresasagrotop.cl/firma_cobranza.jpg"" /></p>
                        ";
                    }
                    break;
            }
            bodyhtml = bodyhtml.Replace("@OBSERVACIONES@", @observaciones);
            return bodyhtml;
        }

        private static string CreateBodyHtml(string clienteKey, Clases.Cobranza cobranza, List<Clases.CobranzaDocs> lineas, string nivel = "0-30", string idioma = "Español", string observaciones = "")
        {
            var conFactList = lineas.Where(x => x.U_FACENTIDAD != string.Empty).ToList();
            bool conFact = conFactList.Count > 0 ? true : false;
            string titulofactoring = @"<th scope = ""col"" > Factoring </th>";
            string valorfactoring = @"<td style = ""text-align:center"" > @FACTORING@ </td>";
            string bodyhtml = @"";
            double summonto = 0;
            double summontoxvenc = 0;
            double montoadeudado = 0;
            string monedagral = string.Empty;
            switch (nivel)
            {
                case "31-60":
                    if (idioma.Equals("Español"))
                    {
                        bodyhtml = @"
                            <p>Estimado cliente, al día de hoy su cuenta por cobrar registra facturas vencidas de más de 30 días.</p>
                            <table border=""0"" cellpadding=""1"" cellspacing=""1"" style=""width: 800px"" summary=""Documentos"">
                            <thead>
                            <tr>
                                <th scope = ""col"" > N° de Factura </th>
                                <th scope = ""col"" > Fecha emisión </th>
                                <th scope = ""col"" > Fecha de Vencimiento </th>
                                <th scope = ""col"" > Días de mora </th>
                                <th scope = ""col"" > Moneda </th>
                                <th scope = ""col"" > Monto Vencido </th>
                                <th scope = ""col"" > Monto por Vencer </th>
                                @COLUMNAFACTORING@
                            </tr>
                            </thead>
                            <tbody>";
                        bodyhtml = conFact ? bodyhtml.Replace("@COLUMNAFACTORING@", @titulofactoring) : bodyhtml.Replace("@COLUMNAFACTORING@", string.Empty);

                        lineas.ForEach(documento =>
                        {
                            string folio = documento.U_FOLIONUM.ToString();
                            string fecha = documento.U_DOCDATE.ToString("dd-MM-yyyy");
                            string fechavcto = documento.U_DOCDUEDATE.ToString("dd-MM-yyyy");
                            TimeSpan diffdate = DateTime.Now.Date - documento.U_DOCDUEDATE.Date;
                            string mora = diffdate.Days.ToString();
                            string moneda = documento.U_DOCCUR;
                            monedagral = moneda;
                            string monto = documento.U_DOCTOTAL.ToString("N", new System.Globalization.CultureInfo("es-CL"));
                            summonto += documento.U_DOCTOTAL;
                            string montoxvenc = documento.U_FUTDUE.ToString("N", new System.Globalization.CultureInfo("es-CL"));
                            summontoxvenc+= documento.U_FUTDUE;
                            string factoring = documento.U_FACENTIDAD;
                            string lineabody = @"
                            <tr>
                            <td style = ""text-align:center"" > @FOLIO@ </td>
                            <td style = ""text-align:center"" > @FECHA@ </td>
                            <td style = ""text-align:center"" > @FECHAVCTO@ </td>
                            <td style = ""text-align:center"" > @MORA@ </td>
                            <td style = ""text-align:center"" > @MONEDA@ </td>
                            <td style = ""text-align:right"" > @MONTO@ </td>
                            <td style = ""text-align:right"" > @MONTOXVENC@ </td>
                            @VALORFACTORING@
                            </tr>";
                            lineabody = conFact ? lineabody.Replace("@VALORFACTORING@", valorfactoring) : lineabody.Replace("@VALORFACTORING@", string.Empty);
                            lineabody = lineabody.Replace("@FOLIO@", folio);
                            lineabody = lineabody.Replace("@FECHA@", fecha);
                            lineabody = lineabody.Replace("@FECHAVCTO@", fechavcto);
                            lineabody = lineabody.Replace("@MORA@", mora);
                            lineabody = lineabody.Replace("@MONEDA@", moneda);
                            lineabody = lineabody.Replace("@MONTO@", monto);
                            lineabody = lineabody.Replace("@MONTOXVENC@", montoxvenc);
                            lineabody = lineabody.Replace("@FACTORING@", factoring);
                            bodyhtml += @lineabody;
                        });
                        bodyhtml += @"
                            <tr>
                            <td style = ""text-align:center"" > </td>
                            <td style = ""text-align:center"" > </td>
                            <td style = ""text-align:center"" > <b>Totales</b> </td>
                            <td style = ""text-align:center"" > </td>
                            <td style = ""text-align:center"" > <b>@MONEDAGRAL@</b> </td>
                            <td style = ""text-align:right"" > <b>@SUMMONTO@</b> </td>
                            <td style = ""text-align:right"" > <b>@SUMMONTOXVENC@</b> </td>
                            </tr>
                        </ tbody>
                        </table>";
                        bodyhtml += @"
                        </br>
                        <table border=""0"" cellpadding=""1"" cellspacing=""1"" style=""width: 800px"" summary=""Totales"">
                        <thead>
                        <tr>
                            <th scope = ""col"" > </th>
                            <th scope = ""col"" > </th>
                            <th scope = ""col"" > Monto Adeudado </th>
                            <th scope = ""col"" > </th>
                            <th scope = ""col"" > @MONEDAGRAL@ </th>
                            <th scope = ""col"" > </th>
                            <th scope = ""col"" > @MONTOADEUDADO@ </th>
                        </tr>
                        </thead>
                        </table>";
                        bodyhtml += @"
                        <p>Dado lo anterior, favor indicar fecha de pago en calidad de urgente, a fin de evitar inconvenientes con futuros despachos.</>
                        <p>@OBSERVACIONES@</>
                        <p>Dudas o consultas contactar con el departamento de crédito y cobranzas de empresas Agrotop.</p>
                        <p>En espera de sus comentarios, saluda atentamente;</p>
                        <p><img alt="""" src=""https://empresasagrotop.cl/firma_cobranza.jpg"" /></p>
                        ";
                    }
                    else
                    {
                        bodyhtml = @"
                            <p>Dear customer.</p>
                            <p>We would like to remember you that your account debt status shows invoices overdue for more than 30 days.</p>
                            <table border=""0"" cellpadding=""1"" cellspacing=""1"" style=""width: 800px"" summary=""Documentos"">
                            <thead>
                            <tr>
                                <th scope = ""col"" > Invoice Number </th>
                                <th scope = ""col"" > Date of Issue </th>
                                <th scope = ""col"" > Expiration Date </th>
                                <th scope = ""col"" > Days Past Due </th>
                                <th scope = ""col"" > Currency </th>
                                <th scope = ""col"" > Overdue Amount </th>
                                <th scope = ""col"" > Future Due Amount </th>
                                @COLUMNAFACTORING@
                            </tr>
                            </thead>
                            <tbody>";
                        bodyhtml = conFact ? bodyhtml.Replace("@COLUMNAFACTORING@", @titulofactoring) : bodyhtml.Replace("@COLUMNAFACTORING@", string.Empty);

                        lineas.ForEach(documento =>
                        {
                            string folio = documento.U_FOLIONUM.ToString();
                            string fecha = documento.U_DOCDATE.ToString("dd-MM-yyyy");
                            string fechavcto = documento.U_DOCDUEDATE.ToString("dd-MM-yyyy");
                            TimeSpan diffdate = DateTime.Now.Date - documento.U_DOCDUEDATE.Date;
                            string mora = diffdate.Days.ToString();
                            string moneda = documento.U_DOCCUR;
                            monedagral = moneda;
                            string monto = documento.U_DOCTOTAL.ToString("N", new System.Globalization.CultureInfo("es-CL"));
                            summonto += documento.U_DOCTOTAL;
                            string montoxvenc = documento.U_FUTDUE.ToString("N", new System.Globalization.CultureInfo("es-CL"));
                            summontoxvenc += documento.U_FUTDUE;
                            string factoring = documento.U_FACENTIDAD;
                            string lineabody = @"
                            <tr>
                            <td style = ""text-align:center"" > @FOLIO@ </td>
                            <td style = ""text-align:center"" > @FECHA@ </td>
                            <td style = ""text-align:center"" > @FECHAVCTO@ </td>
                            <td style = ""text-align:center"" > @MORA@ </td>
                            <td style = ""text-align:center"" > @MONEDA@ </td>
                            <td style = ""text-align:right"" > @MONTO@ </td>
                            <td style = ""text-align:right"" > @MONTOXVENC@ </td>
                            @VALORFACTORING@
                            </tr>";
                            lineabody = conFact ? lineabody.Replace("@VALORFACTORING@", valorfactoring) : lineabody.Replace("@VALORFACTORING@", string.Empty);
                            lineabody = lineabody.Replace("@FOLIO@", folio);
                            lineabody = lineabody.Replace("@FECHA@", fecha);
                            lineabody = lineabody.Replace("@FECHAVCTO@", fechavcto);
                            lineabody = lineabody.Replace("@MORA@", mora);
                            lineabody = lineabody.Replace("@MONEDA@", moneda);
                            lineabody = lineabody.Replace("@MONTO@", monto);
                            lineabody = lineabody.Replace("@MONTOXVENC@", montoxvenc);
                            lineabody = lineabody.Replace("@FACTORING@", factoring);
                            bodyhtml += @lineabody;
                        });
                        bodyhtml += @"
                            <tr>
                            <td style = ""text-align:center"" > </td>
                            <td style = ""text-align:center"" > </td>
                            <td style = ""text-align:center"" > <b>Totals</b> </td>
                            <td style = ""text-align:center"" > </td>
                            <td style = ""text-align:center"" > <b>@MONEDAGRAL@</b> </td>
                            <td style = ""text-align:right"" > <b>@SUMMONTO@</b> </td>
                            <td style = ""text-align:right"" > <b>@SUMMONTOXVENC@</b> </td>
                            </tr>
                        </ tbody>
                        </table>";
                        bodyhtml += @"
                        </br>
                        <table border=""0"" cellpadding=""1"" cellspacing=""1"" style=""width: 800px"" summary=""Totales"">
                        <thead>
                        <tr>
                            <th scope = ""col"" > </th>
                            <th scope = ""col"" > </th>
                            <th scope = ""col"" > Balance Due </th>
                            <th scope = ""col"" > </th>
                            <th scope = ""col"" > @MONEDAGRAL@ </th>
                            <th scope = ""col"" > </th>
                            <th scope = ""col"" > @MONTOADEUDADO@ </th>
                        </tr>
                        </thead>
                        </table>";
                        bodyhtml += @"
                        <p>Considering the above, please let us know your payment commitment dates in order to avoid any problem on future deliveries.</>
                        <p>@OBSERVACIONES@</>
                        <p>If you have any questions please feel free to get in touch with us.</p>
                        <p>Awaiting your comments,</p>
                        <p>Best regards,</p>
                        <p><img alt="""" src=""https://empresasagrotop.cl/firma_cobranza.jpg"" /></p>
                        ";
                    }
                    break;
                case "60+":
                    if (idioma.Equals("Español"))
                    {
                        bodyhtml = @"
                            <p>Estimado cliente, al día de hoy su cuenta por cobrar registra facturas vencidas de más de 60 días.</p>
                            <table border=""0"" cellpadding=""1"" cellspacing=""1"" style=""width: 800px"" summary=""Documentos"">
                            <thead>
                            <tr>
                                <th scope = ""col"" > N° de Factura </th>
                                <th scope = ""col"" > Fecha emisión </th>
                                <th scope = ""col"" > Fecha de Vencimiento </th>
                                <th scope = ""col"" > Días de mora </th>
                                <th scope = ""col"" > Moneda </th>
                                <th scope = ""col"" > Monto Vencido </th>
                                <th scope = ""col"" > Monto por Vencer </th>
                                @COLUMNAFACTORING@
                            </tr>
                            </thead>
                            <tbody>";
                        bodyhtml = conFact ? bodyhtml.Replace("@COLUMNAFACTORING@", @titulofactoring) : bodyhtml.Replace("@COLUMNAFACTORING@", string.Empty);

                        lineas.ForEach(documento =>
                        {
                            string folio = documento.U_FOLIONUM.ToString();
                            string fecha = documento.U_DOCDATE.ToString("dd-MM-yyyy");
                            string fechavcto = documento.U_DOCDUEDATE.ToString("dd-MM-yyyy");
                            TimeSpan diffdate = DateTime.Now.Date - documento.U_DOCDUEDATE.Date;
                            string mora = diffdate.Days.ToString();
                            string moneda = documento.U_DOCCUR;
                            monedagral = moneda;
                            string monto = documento.U_DOCTOTAL.ToString("N", new System.Globalization.CultureInfo("es-CL"));
                            summonto += documento.U_DOCTOTAL;
                            string montoxvenc = documento.U_FUTDUE.ToString("N", new System.Globalization.CultureInfo("es-CL"));
                            summontoxvenc += documento.U_FUTDUE;
                            string factoring = documento.U_FACENTIDAD;
                            string lineabody = @"
                            <tr>
                            <td style = ""text-align:center"" > @FOLIO@ </td>
                            <td style = ""text-align:center"" > @FECHA@ </td>
                            <td style = ""text-align:center"" > @FECHAVCTO@ </td>
                            <td style = ""text-align:center"" > @MORA@ </td>
                            <td style = ""text-align:center"" > @MONEDA@ </td>
                            <td style = ""text-align:right"" > @MONTO@ </td>
                            <td style = ""text-align:right"" > @MONTOXVENC@ </td>
                            @VALORFACTORING@
                            </tr>";
                            lineabody = conFact ? lineabody.Replace("@VALORFACTORING@", valorfactoring) : lineabody.Replace("@VALORFACTORING@", string.Empty);
                            lineabody = lineabody.Replace("@FOLIO@", folio);
                            lineabody = lineabody.Replace("@FECHA@", fecha);
                            lineabody = lineabody.Replace("@FECHAVCTO@", fechavcto);
                            lineabody = lineabody.Replace("@MORA@", mora);
                            lineabody = lineabody.Replace("@MONEDA@", moneda);
                            lineabody = lineabody.Replace("@MONTO@", monto);
                            lineabody = lineabody.Replace("@MONTOXVENC@", montoxvenc);
                            lineabody = lineabody.Replace("@FACTORING@", factoring);
                            bodyhtml += @lineabody;
                        });
                        bodyhtml += @"
                            <tr>
                            <td style = ""text-align:center"" > </td>
                            <td style = ""text-align:center"" > </td>
                            <td style = ""text-align:center"" > <b>Totales</b> </td>
                            <td style = ""text-align:center"" > </td>
                            <td style = ""text-align:center"" > <b>@MONEDAGRAL@</b> </td>
                            <td style = ""text-align:right"" > <b>@SUMMONTO@</b> </td>
                            <td style = ""text-align:right"" > <b>@SUMMONTOXVENC@</b> </td>
                            </tr>
                        </ tbody>
                        </table>";
                        bodyhtml += @"
                        </br>
                        <table border=""0"" cellpadding=""1"" cellspacing=""1"" style=""width: 800px"" summary=""Totales"">
                        <thead>
                        <tr>
                            <th scope = ""col"" > </th>
                            <th scope = ""col"" > </th>
                            <th scope = ""col"" > Monto Adeudado </th>
                            <th scope = ""col"" > </th>
                            <th scope = ""col"" > @MONEDAGRAL@ </th>
                            <th scope = ""col"" > </th>
                            <th scope = ""col"" > @MONTOADEUDADO@ </th>
                        </tr>
                        </thead>
                        </table>";
                        bodyhtml += @"
                        <p>Dado lo anterior, es imprescindible regularice en el más breve plazo a fin de evitar la cancelación de la línea de crédito y publicación de la deuda.</>
                        <p>@OBSERVACIONES@</>
                        <p>Ante consultas contactar con el departamento de crédito y cobranzas de empresas Agrotop.</p>
                        <p>Sin otro particular, saluda atentamente;</p>
                        <p><img alt="""" src=""https://empresasagrotop.cl/firma_cobranza.jpg"" /></p>
                        ";
                    }
                    else
                    {
                        bodyhtml = @"
                            <p>Dear customer.</p>
                            <p>We would like to warn you that your account debt status shows invoices overdue for more than 60 days.</p>
                            <table border=""0"" cellpadding=""1"" cellspacing=""1"" style=""width: 800px"" summary=""Documentos"">
                            <thead>
                            <tr>
                                <th scope = ""col"" > Invoice Number </th>
                                <th scope = ""col"" > Date of Issue </th>
                                <th scope = ""col"" > Expiration Date </th>
                                <th scope = ""col"" > Days Past Due </th>
                                <th scope = ""col"" > Currency </th>
                                <th scope = ""col"" > Overdue Amount </th>
                                <th scope = ""col"" > Future Due Amount </th>
                                @COLUMNAFACTORING@
                            </tr>
                            </thead>
                            <tbody>";
                        bodyhtml = conFact ? bodyhtml.Replace("@COLUMNAFACTORING@", @titulofactoring) : bodyhtml.Replace("@COLUMNAFACTORING@", string.Empty);

                        lineas.ForEach(documento =>
                        {
                            string folio = documento.U_FOLIONUM.ToString();
                            string fecha = documento.U_DOCDATE.ToString("dd-MM-yyyy");
                            string fechavcto = documento.U_DOCDUEDATE.ToString("dd-MM-yyyy");
                            TimeSpan diffdate = DateTime.Now.Date - documento.U_DOCDUEDATE.Date;
                            string mora = diffdate.Days.ToString();
                            string moneda = documento.U_DOCCUR;
                            monedagral = moneda;
                            string monto = documento.U_DOCTOTAL.ToString("N", new System.Globalization.CultureInfo("es-CL"));
                            summonto += documento.U_DOCTOTAL;
                            string montoxvenc = documento.U_FUTDUE.ToString("N", new System.Globalization.CultureInfo("es-CL"));
                            summontoxvenc += documento.U_FUTDUE;
                            string factoring = documento.U_FACENTIDAD;
                            string lineabody = @"
                            <tr>
                            <td style = ""text-align:center"" > @FOLIO@ </td>
                            <td style = ""text-align:center"" > @FECHA@ </td>
                            <td style = ""text-align:center"" > @FECHAVCTO@ </td>
                            <td style = ""text-align:center"" > @MORA@ </td>
                            <td style = ""text-align:center"" > @MONEDA@ </td>
                            <td style = ""text-align:right"" > @MONTO@ </td>
                            <td style = ""text-align:right"" > @MONTOXVENC@ </td>
                            @VALORFACTORING@
                            </tr>";
                            lineabody = conFact ? lineabody.Replace("@VALORFACTORING@", valorfactoring) : lineabody.Replace("@VALORFACTORING@", string.Empty);
                            lineabody = lineabody.Replace("@FOLIO@", folio);
                            lineabody = lineabody.Replace("@FECHA@", fecha);
                            lineabody = lineabody.Replace("@FECHAVCTO@", fechavcto);
                            lineabody = lineabody.Replace("@MORA@", mora);
                            lineabody = lineabody.Replace("@MONEDA@", moneda);
                            lineabody = lineabody.Replace("@MONTO@", monto);
                            lineabody = lineabody.Replace("@MONTOXVENC@", montoxvenc);
                            lineabody = lineabody.Replace("@FACTORING@", factoring);
                            bodyhtml += @lineabody;
                        });
                        bodyhtml += @"
                            <tr>
                            <td style = ""text-align:center"" > </td>
                            <td style = ""text-align:center"" > </td>
                            <td style = ""text-align:center"" > <b>Totals</b> </td>
                            <td style = ""text-align:center"" > </td>
                            <td style = ""text-align:center"" > <b>@MONEDAGRAL@</b> </td>
                            <td style = ""text-align:right"" > <b>@SUMMONTO@</b> </td>
                            <td style = ""text-align:right"" > <b>@SUMMONTOXVENC@</b> </td>
                            </tr>
                        </ tbody>
                        </table>";
                        bodyhtml += @"
                        </br>
                        <table border=""0"" cellpadding=""1"" cellspacing=""1"" style=""width: 800px"" summary=""Totales"">
                        <thead>
                        <tr>
                            <th scope = ""col"" > </th>
                            <th scope = ""col"" > </th>
                            <th scope = ""col"" > Balance Due </th>
                            <th scope = ""col"" > </th>
                            <th scope = ""col"" > @MONEDAGRAL@ </th>
                            <th scope = ""col"" > </th>
                            <th scope = ""col"" > @MONTOADEUDADO@ </th>
                        </tr>
                        </thead>
                        </table>";
                        bodyhtml += @"
                        <p>Considering the above, it is mandatory that you may pay off your due invoices as soon as possible in order to avoid suspension of your line of credit.</>
                        <p>@OBSERVACIONES@</>
                        <p>If you have any questions please feel free to get in touch with us.</p>
                        <p>Awaiting your comments,</p>
                        <p>Best regards,</p>
                        <p><img alt="""" src=""https://empresasagrotop.cl/firma_cobranza.jpg"" /></p>
                        ";
                    }
                    break;
                default:
                    if (idioma.Equals("Español"))
                    {
                        bodyhtml = @"
                            <p>Estimado cliente, recordamos vencimientos asociados a su cuenta por cobrar.</p>
                            <table border=""0"" cellpadding=""1"" cellspacing=""1"" style=""width: 800px"" summary=""Documentos"">
                            <thead>
                            <tr>
                                <th scope = ""col"" > N° de Factura </th>
                                <th scope = ""col"" > Fecha emisión </th>
                                <th scope = ""col"" > Fecha de Vencimiento </th>
                                <th scope = ""col"" > Días de mora </th>
                                <th scope = ""col"" > Moneda </th>
                                <th scope = ""col"" > Monto Vencido </th>
                                <th scope = ""col"" > Monto por Vencer </th>
                                @COLUMNAFACTORING@
                            </tr>
                            </thead>
                            <tbody>";
                        bodyhtml = conFact ? bodyhtml.Replace("@COLUMNAFACTORING@", @titulofactoring) : bodyhtml.Replace("@COLUMNAFACTORING@", string.Empty);

                        lineas.ForEach(documento =>
                        {
                            string folio = documento.U_FOLIONUM.ToString();
                            string fecha = documento.U_DOCDATE.ToString("dd-MM-yyyy");
                            string fechavcto = documento.U_DOCDUEDATE.ToString("dd-MM-yyyy");
                            TimeSpan diffdate = DateTime.Now.Date - documento.U_DOCDUEDATE.Date;
                            string mora = diffdate.Days.ToString();
                            string moneda = documento.U_DOCCUR;
                            monedagral = moneda;
                            string monto = documento.U_DOCTOTAL.ToString("N", new System.Globalization.CultureInfo("es-CL"));
                            summonto += documento.U_DOCTOTAL;
                            string montoxvenc = documento.U_FUTDUE.ToString("N", new System.Globalization.CultureInfo("es-CL"));
                            summontoxvenc += documento.U_FUTDUE;
                            string factoring = documento.U_FACENTIDAD;
                            string lineabody = @"
                            <tr>
                            <td style = ""text-align:center"" > @FOLIO@ </td>
                            <td style = ""text-align:center"" > @FECHA@ </td>
                            <td style = ""text-align:center"" > @FECHAVCTO@ </td>
                            <td style = ""text-align:center"" > @MORA@ </td>
                            <td style = ""text-align:center"" > @MONEDA@ </td>
                            <td style = ""text-align:right"" > @MONTO@ </td>
                            <td style = ""text-align:right"" > @MONTOXVENC@ </td>
                            @VALORFACTORING@
                            </tr>";
                            lineabody = conFact ? lineabody.Replace("@VALORFACTORING@", valorfactoring) : lineabody.Replace("@VALORFACTORING@", string.Empty);
                            lineabody = lineabody.Replace("@FOLIO@", folio);
                            lineabody = lineabody.Replace("@FECHA@", fecha);
                            lineabody = lineabody.Replace("@FECHAVCTO@", fechavcto);
                            lineabody = lineabody.Replace("@MORA@", mora);
                            lineabody = lineabody.Replace("@MONEDA@", moneda);
                            lineabody = lineabody.Replace("@MONTO@", monto);
                            lineabody = lineabody.Replace("@MONTOXVENC@", montoxvenc);
                            lineabody = lineabody.Replace("@FACTORING@", factoring);
                            bodyhtml += @lineabody;
                        });
                        bodyhtml += @"
                            <tr>
                            <td style = ""text-align:center"" > </td>
                            <td style = ""text-align:center"" > </td>
                            <td style = ""text-align:center"" > <b>Totales</b> </td>
                            <td style = ""text-align:center"" > </td>
                            <td style = ""text-align:center"" > <b>@MONEDAGRAL@</b> </td>
                            <td style = ""text-align:right"" > <b>@SUMMONTO@</b> </td>
                            <td style = ""text-align:right"" > <b>@SUMMONTOXVENC@</b> </td>
                            </tr>
                        </ tbody>
                        </table>";
                        bodyhtml += @"
                        </br>
                        <table border=""0"" cellpadding=""1"" cellspacing=""1"" style=""width: 800px"" summary=""Totales"">
                        <thead>
                        <tr>
                            <th scope = ""col"" > </th>
                            <th scope = ""col"" > </th>
                            <th scope = ""col"" > Monto Adeudado </th>
                            <th scope = ""col"" > </th>
                            <th scope = ""col"" > @MONEDAGRAL@ </th>
                            <th scope = ""col"" > </th>
                            <th scope = ""col"" > @MONTOADEUDADO@ </th>
                        </tr>
                        </thead>
                        </table>";
                        bodyhtml += @"
                        <p>Agradeceremos gestionar los pagos correspondientes a la brevedad.</>
                        <p>@OBSERVACIONES@</>
                        <p>Dudas o consultas contactar con el departamento de crédito y cobranzas de empresas Agrotop.</p>
                        <p>Quedamos atentos, saludos;</p>
                        <p><img alt="""" src=""https://empresasagrotop.cl/firma_cobranza.jpg"" /></p>
                        ";
                    }
                    else
                    {
                        bodyhtml = @"
                            <p>Dear customer.</p>
                            <p>We would like to remember you the following overdue invoices associated to your debt status.</p>
                            <table border=""0"" cellpadding=""1"" cellspacing=""1"" style=""width: 800px"" summary=""Documentos"">
                            <thead>
                            <tr>
                                <th scope = ""col"" > Invoice Number </th>
                                <th scope = ""col"" > Date of Issue </th>
                                <th scope = ""col"" > Expiration Date </th>
                                <th scope = ""col"" > Days Past Due </th>
                                <th scope = ""col"" > Currency </th>
                                <th scope = ""col"" > Overdue Amount </th>
                                <th scope = ""col"" > Future Due Amount </th>
                                @COLUMNAFACTORING@
                            </tr>
                            </thead>
                            <tbody>";
                        bodyhtml = conFact ? bodyhtml.Replace("@COLUMNAFACTORING@", @titulofactoring) : bodyhtml.Replace("@COLUMNAFACTORING@", string.Empty);

                        lineas.ForEach(documento =>
                        {
                            string folio = documento.U_FOLIONUM.ToString();
                            string fecha = documento.U_DOCDATE.ToString("dd-MM-yyyy");
                            string fechavcto = documento.U_DOCDUEDATE.ToString("dd-MM-yyyy");
                            TimeSpan diffdate = DateTime.Now.Date - documento.U_DOCDUEDATE.Date;
                            string mora = diffdate.Days.ToString();
                            string moneda = documento.U_DOCCUR;
                            monedagral = moneda;
                            string monto = documento.U_DOCTOTAL.ToString("N", new System.Globalization.CultureInfo("es-CL"));
                            summonto += documento.U_DOCTOTAL;
                            string montoxvenc = documento.U_FUTDUE.ToString("N", new System.Globalization.CultureInfo("es-CL"));
                            summontoxvenc += documento.U_FUTDUE;
                            string factoring = documento.U_FACENTIDAD;
                            string lineabody = @"
                            <tr>
                            <td style = ""text-align:center"" > @FOLIO@ </td>
                            <td style = ""text-align:center"" > @FECHA@ </td>
                            <td style = ""text-align:center"" > @FECHAVCTO@ </td>
                            <td style = ""text-align:center"" > @MORA@ </td>
                            <td style = ""text-align:center"" > @MONEDA@ </td>
                            <td style = ""text-align:right"" > @MONTO@ </td>
                            <td style = ""text-align:right"" > @MONTOXVENC@ </td>
                            @VALORFACTORING@
                            </tr>";
                            lineabody = conFact ? lineabody.Replace("@VALORFACTORING@", valorfactoring) : lineabody.Replace("@VALORFACTORING@", string.Empty);
                            lineabody = lineabody.Replace("@FOLIO@", folio);
                            lineabody = lineabody.Replace("@FECHA@", fecha);
                            lineabody = lineabody.Replace("@FECHAVCTO@", fechavcto);
                            lineabody = lineabody.Replace("@MORA@", mora);
                            lineabody = lineabody.Replace("@MONEDA@", moneda);
                            lineabody = lineabody.Replace("@MONTO@", monto);
                            lineabody = lineabody.Replace("@MONTOXVENC@", montoxvenc);
                            lineabody = lineabody.Replace("@FACTORING@", factoring);
                            bodyhtml += @lineabody;
                        });
                        bodyhtml += @"
                            <tr>
                            <td style = ""text-align:center"" > </td>
                            <td style = ""text-align:center"" > </td>
                            <td style = ""text-align:center"" > <b>Totals</b> </td>
                            <td style = ""text-align:center"" > </td>
                            <td style = ""text-align:center"" > <b>@MONEDAGRAL@</b> </td>
                            <td style = ""text-align:right"" > <b>@SUMMONTO@</b> </td>
                            <td style = ""text-align:right"" > <b>@SUMMONTOXVENC@</b> </td>
                            </tr>
                        </ tbody>
                        </table>";
                        bodyhtml += @"
                        </br>
                        <table border=""0"" cellpadding=""1"" cellspacing=""1"" style=""width: 800px"" summary=""Totales"">
                        <thead>
                        <tr>
                            <th scope = ""col"" > </th>
                            <th scope = ""col"" > </th>
                            <th scope = ""col"" > Balance Due </th>
                            <th scope = ""col"" > </th>
                            <th scope = ""col"" > @MONEDAGRAL@ </th>
                            <th scope = ""col"" > </th>
                            <th scope = ""col"" > @MONTOADEUDADO@ </th>
                        </tr>
                        </thead>
                        </table>";
                        bodyhtml += @"
                        <p>If you have any questions please feel free to get in touch with us.</p>
                        <p>@OBSERVACIONES@</>
                        <p>Best regards,</p>
                        <p><img alt="""" src=""https://empresasagrotop.cl/firma_cobranza.jpg"" /></p>
                        ";
                    }
                    break;
            }
            string s_summonto = summonto.ToString("N", new System.Globalization.CultureInfo("es-CL"));
            string s_summontoxvenc = summontoxvenc.ToString("N", new System.Globalization.CultureInfo("es-CL"));
            bodyhtml = bodyhtml.Replace("@SUMMONTO@", s_summonto);
            bodyhtml = bodyhtml.Replace("@SUMMONTOXVENC@", s_summontoxvenc);
            montoadeudado = summonto + summontoxvenc;
            string s_montoadeudado = montoadeudado.ToString("N", new System.Globalization.CultureInfo("es-CL"));
            bodyhtml = bodyhtml.Replace("@MONEDAGRAL@", monedagral);
            bodyhtml = bodyhtml.Replace("@MONTOADEUDADO@", s_montoadeudado);
            bodyhtml = bodyhtml.Replace("@OBSERVACIONES@", @observaciones);
            return bodyhtml;
        }

        private static void SendMailES(string clienteKey, string bodyhtml)
        {
            string host = SBO.ConsultasSBO.ObtenerParametro("CORREO_SERVIDOR");
            string port = SBO.ConsultasSBO.ObtenerParametro("CORREO_SERVIDOR_PUERTO");
            string ssl = SBO.ConsultasSBO.ObtenerParametro("CORREO_SERVIDOR_SSL");
            string from = SBO.ConsultasSBO.ObtenerParametro("CORREO_ENVIO");
            string pass = SBO.ConsultasSBO.ObtenerParametro("CORREO_PASS");
            string alias = SBO.ConsultasSBO.ObtenerParametro("CORREO_ALIAS");
            string asunto = SBO.ConsultasSBO.ObtenerParametro("CORREO_ASUNTO_ES");
            string to = SBO.ConsultasSBO.ObtenerMailConcatenadoSN(clienteKey);
            string NombreSN = SBO.ConsultasSBO.ObtenerNombreSN(clienteKey);
            asunto += @" / " + NombreSN;

            //////GMAIL
            //////System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage();
            //////msg.To.Add(to);
            ////////msg.To.Add("pruebascobranzas@gmail.com");
            //////msg.From = new System.Net.Mail.MailAddress(from, alias, System.Text.Encoding.UTF8);
            //////msg.Subject = asunto;
            //////msg.SubjectEncoding = System.Text.Encoding.UTF8;
            //////msg.Body = @bodyhtml;
            //////msg.BodyEncoding = System.Text.Encoding.UTF8;
            //////msg.IsBodyHtml = true;

            ////////Aquí es donde se hace lo especial
            //////System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
            //////client.Credentials = new System.Net.NetworkCredential(from, pass);
            ////////client.Port = 587;
            ////////client.Host = "smtp.gmail.com";
            ////////client.EnableSsl = true; //Esto es para que vaya a través de SSL que es obligatorio con GMail
            //////client.Host = host;
            //////client.Port = Convert.ToInt32(port);
            //////client.EnableSsl = (ssl.Equals("Y") || ssl.Equals("S") || ssl.ToLower().Equals("true") || ssl.Equals("1")) ? true : false;
            //////try
            //////{
            //////    client.Send(msg);
            //////}
            //////catch (System.Net.Mail.SmtpException ex)
            //////{
            //////    Console.WriteLine(ex.Message);
            //////    Console.ReadLine();
            //////}

            //////Office365
            SmtpMail oMail = new SmtpMail("TryIt");
            oMail.From = string.Format("{0} <{1}>", alias, from);
            // Set recipient email address
            oMail.To = to;
            // Set email subject
            oMail.Subject = asunto;
            // If bodyText contains the html tags, please use
            oMail.HtmlBody = @bodyhtml;
            //oMail.TextBody = @bodyhtml;
            // Hotmail SMTP server address
            SmtpServer oServer = new SmtpServer(host);
            // Hotmail user authentication should use your
            // email address as the user name.
            oServer.User = from;
            // If you got authentication error, try to create an app password instead of your user password.
            // https://support.microsoft.com/en-us/account-billing/using-app-passwords-with-apps-that-don-t-support-two-step-verification-5896ed9b-4263-e681-128a-a6f2979a7944
            oServer.Password = pass;
            // Set 587 port, if you want to use 25 port, please change 587 to 25
            oServer.Port = Convert.ToInt32(port);
            bool useSsl = (ssl.Equals("Y") || ssl.Equals("S") || ssl.ToLower().Equals("true") || ssl.Equals("1")) ? true : false;
            // ConnectTryTLS means if server supports SSL/TLS, SSL/TLS will be used automatically.
            oServer.ConnectType = (useSsl) ? SmtpConnectType.ConnectSSLAuto : SmtpConnectType.ConnectTryTLS;
            // detect SSL/TLS connection automatically
            //oServer.ConnectType = SmtpConnectType.ConnectSSLAuto;
            SmtpClient oSmtp = new SmtpClient();
            try
            {
                oSmtp.SendMail(oServer, oMail);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
        }

        private static void SendMailUS(string clienteKey, string bodyhtml)
        {
            string host = SBO.ConsultasSBO.ObtenerParametro("CORREO_SERVIDOR");
            string port = SBO.ConsultasSBO.ObtenerParametro("CORREO_SERVIDOR_PUERTO");
            string ssl = SBO.ConsultasSBO.ObtenerParametro("CORREO_SERVIDOR_SSL");
            string from = SBO.ConsultasSBO.ObtenerParametro("CORREO_ENVIO");
            string pass = SBO.ConsultasSBO.ObtenerParametro("CORREO_PASS");
            string alias = SBO.ConsultasSBO.ObtenerParametro("CORREO_ALIAS");
            string asunto = SBO.ConsultasSBO.ObtenerParametro("CORREO_ASUNTO_US");
            string to = SBO.ConsultasSBO.ObtenerMailConcatenadoSN(clienteKey);
            string NombreSN = SBO.ConsultasSBO.ObtenerNombreSN(clienteKey);
            asunto += @" / " + NombreSN;

            //////GMAIL
            //////System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage();
            //////msg.To.Add(to);
            ////////msg.To.Add("pruebascobranzas@gmail.com");
            //////msg.From = new System.Net.Mail.MailAddress(from, alias, System.Text.Encoding.UTF8);
            //////msg.Subject = asunto;
            //////msg.SubjectEncoding = System.Text.Encoding.UTF8;
            //////msg.Body = @bodyhtml;
            //////msg.BodyEncoding = System.Text.Encoding.UTF8;
            //////msg.IsBodyHtml = true;

            ////////Aquí es donde se hace lo especial
            //////System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
            //////client.Credentials = new System.Net.NetworkCredential(from, pass);
            ////////client.Port = 587;
            ////////client.Host = "smtp.gmail.com";
            ////////client.EnableSsl = true; //Esto es para que vaya a través de SSL que es obligatorio con GMail
            //////client.Host = host;
            //////client.Port = Convert.ToInt32(port);
            //////client.EnableSsl = (ssl.Equals("Y") || ssl.Equals("S") || ssl.ToLower().Equals("true") || ssl.Equals("1")) ? true : false;
            //////try
            //////{
            //////    client.Send(msg);
            //////}
            //////catch (System.Net.Mail.SmtpException ex)
            //////{
            //////    Console.WriteLine(ex.Message);
            //////    Console.ReadLine();
            //////}

            //////Office365
            SmtpMail oMail = new SmtpMail("TryIt");
            oMail.From = string.Format("{0} <{1}>", alias, from);
            // Set recipient email address
            oMail.To = to;
            // Set email subject
            oMail.Subject = asunto;
            // If bodyText contains the html tags, please use
            oMail.HtmlBody = @bodyhtml;
            //oMail.TextBody = @bodyhtml;
            // Hotmail SMTP server address
            SmtpServer oServer = new SmtpServer(host);
            // Hotmail user authentication should use your
            // email address as the user name.
            oServer.User = from;
            // If you got authentication error, try to create an app password instead of your user password.
            // https://support.microsoft.com/en-us/account-billing/using-app-passwords-with-apps-that-don-t-support-two-step-verification-5896ed9b-4263-e681-128a-a6f2979a7944
            oServer.Password = pass;
            // Set 587 port, if you want to use 25 port, please change 587 to 25
            oServer.Port = Convert.ToInt32(port);
            bool useSsl = (ssl.Equals("Y") || ssl.Equals("S") || ssl.ToLower().Equals("true") || ssl.Equals("1")) ? true : false;
            // ConnectTryTLS means if server supports SSL/TLS, SSL/TLS will be used automatically.
            oServer.ConnectType = (useSsl) ? SmtpConnectType.ConnectSSLAuto : SmtpConnectType.ConnectTryTLS;
            // detect SSL/TLS connection automatically
            //oServer.ConnectType = SmtpConnectType.ConnectSSLAuto;
            SmtpClient oSmtp = new SmtpClient();
            try
            {
                oSmtp.SendMail(oServer, oMail);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
        }
    }
}
