using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM.Framework;

namespace Soindus.AddOnFactoringCob.Formularios
{
    [FormAttribute("Soindus.AddOnFactoringCob.Formularios.frmCobLog", "Formularios/frmCobLog.b1f")]
    class frmCobLog : UserFormBase
    {
        // Objetos de formulario
        #region Objetos de formulario
        private static SAPbouiCOM.Form oForm;
        private static SAPbouiCOM.EditText txtCodSN;
        private static SAPbouiCOM.EditText txtNomSN;
        private static SAPbouiCOM.EditText txtObs;
        private static SAPbouiCOM.Button btnFiltrar;
        private static SAPbouiCOM.Button btnLog;
        private static SAPbouiCOM.Matrix mtxDet;
        #endregion

        public frmCobLog()
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
                    // Link objeto base en matrix
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

                        // Boton generar
                        #region generar
                        if (pVal.ItemUID.Equals("btnLog"))
                        {
                            NuevoRegistroCobranza();
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
            oForm = ((SAPbouiCOM.Form)(Application.SBO_Application.Forms.Item("frmCobLog")));
            txtCodSN = ((SAPbouiCOM.EditText)(GetItem("CodSN").Specific));
            txtNomSN = ((SAPbouiCOM.EditText)(GetItem("NomSN").Specific));
            txtObs = ((SAPbouiCOM.EditText)(GetItem("Obs").Specific));
            btnFiltrar = ((SAPbouiCOM.Button)(GetItem("btnFiltrar").Specific));
            btnLog = ((SAPbouiCOM.Button)(GetItem("btnLog").Specific));
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
            string NombreDT = "dtDet";
            SAPbouiCOM.DataTable datatable = oForm.DataSources.DataTables.Item(NombreDT);
            string _query = @"
                SELECT
                ""U_CARDCODE"" AS ""CardCode"",
                ""U_CARDNAME"" AS ""CardName"",
                ""U_EMAIL"" AS ""EMail"",
                ""U_EMAILTYPE"" AS ""Nivel"",
                ""U_EMAILLANG"" AS ""Idioma"",
                ""U_DOCS"" AS ""Docs"",
                ""U_FOLIOS"" AS ""Folios"",
                ""U_OBS"" AS ""Obs"",
                ""U_SENDDATE"" AS ""Date"",
                ""U_SENDTIME"" AS ""Time""
                FROM ""@SO_FCTRNGLOG""
                WHERE 1 = 0";

            datatable.ExecuteQuery(_query);

            SAPbouiCOM.Columns oColumns;
            oColumns = mtxDet.Columns;
            SAPbouiCOM.Column oColumn;

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

            oColumn = oColumns.Add("EMail", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "EMail";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 130;
            oColumn.RightJustified = false;
            oColumn.DataBind.Bind(NombreDT, "EMail");

            oColumn = oColumns.Add("Nivel", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Nivel";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 80;
            oColumn.RightJustified = false;
            oColumn.DataBind.Bind(NombreDT, "Nivel");

            oColumn = oColumns.Add("Idioma", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Idioma";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 80;
            oColumn.RightJustified = false;
            oColumn.DataBind.Bind(NombreDT, "Idioma");

            oColumn = oColumns.Add("Docs", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Documentos";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 100;
            oColumn.RightJustified = false;
            oColumn.DataBind.Bind(NombreDT, "Docs");

            oColumn = oColumns.Add("Folios", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Folios";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 100;
            oColumn.RightJustified = false;
            oColumn.DataBind.Bind(NombreDT, "Folios");

            oColumn = oColumns.Add("Obs", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Obs";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 180;
            oColumn.RightJustified = false;
            oColumn.DataBind.Bind(NombreDT, "Obs");

            oColumn = oColumns.Add("Date", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Fecha";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 80;
            oColumn.RightJustified = false;
            oColumn.DataBind.Bind(NombreDT, "Date");

            oColumn = oColumns.Add("Time", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Hora";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 100;
            oColumn.RightJustified = false;
            oColumn.DataBind.Bind(NombreDT, "Time");

            mtxDet.Clear();
            mtxDet.LoadFromDataSource();
            Comun.FuncionesComunes.LiberarObjetoGenerico(datatable);
        }

        private static void CargarMatrixDetalle()
        {
            // Filtro por Socio de Negocio
            SAPbouiCOM.EditText oSN = (SAPbouiCOM.EditText)oForm.Items.Item("CodSN").Specific;
            string FiltroSN = oSN.Value;
            if (!string.IsNullOrEmpty(FiltroSN))
            {
                FiltroSN = string.Format(@" AND  ""U_CARDCODE"" = '{0}' ", oSN.Value);
            }
            else
            {
                FiltroSN = string.Empty;
            }

            string NombreDT = "dtDet";
            SAPbouiCOM.DataTable datatable = oForm.DataSources.DataTables.Item(NombreDT);
            string _query = @"
                SELECT
                ""U_CARDCODE"" AS ""CardCode"",
                ""U_CARDNAME"" AS ""CardName"",
                ""U_EMAIL"" AS ""EMail"",
                ""U_EMAILTYPE"" AS ""Nivel"",
                ""U_EMAILLANG"" AS ""Idioma"",
                ""U_DOCS"" AS ""Docs"",
                ""U_FOLIOS"" AS ""Folios"",
                ""U_OBS"" AS ""Obs"",
                ""U_SENDDATE"" AS ""Date"",
                ""U_SENDTIME"" AS ""Time""
                FROM ""@SO_FCTRNGLOG""
                WHERE 1 = 1" + FiltroSN;
            datatable.ExecuteQuery(_query);
            oForm.Freeze(true);
            mtxDet.Clear();
            mtxDet.LoadFromDataSource();
            Comun.FuncionesComunes.LiberarObjetoGenerico(datatable);
            oForm.Freeze(false);
        }

        private static void NuevoRegistroCobranza()
        {
            string sn = oForm.DataSources.UserDataSources.Item("CodSN").Value;
            string obs = oForm.DataSources.UserDataSources.Item("Obs").Value;
            if (!string.IsNullOrEmpty(sn) && !string.IsNullOrEmpty(obs))
            {
                Clases.Cobranza cobranza = new Clases.Cobranza();
                Clases.CobranzaDocs documento = new Clases.CobranzaDocs();
                documento.U_CARDCODE = sn;
                documento.U_CARDNAME = SBO.ConsultasSBO.ObtenerNombreSN(sn);
                documento.U_OBS = obs;
                cobranza.Documentos.Add(documento);
                GuardarLogCobranza(cobranza);
                Application.SBO_Application.StatusBar.SetText(string.Format("Log de cobranza guardado correctamente."), SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success);
                CargarMatrixDetalle();
            }
            else
            {
                var resp = Application.SBO_Application.MessageBox("Socio de negocios y observaciones requeridos.");
            }
        }

        private static void GuardarLogCobranza(Clases.Cobranza cobranza)
        {
            var result = SBO.ModeloSBO.AddLogCobranza(cobranza);
        }
    }
}
