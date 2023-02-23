using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM.Framework;

namespace Soindus.AddOnFactoringCob.Formularios
{
    [FormAttribute("Soindus.AddOnFactoringCob.Formularios.frmFacSelect", "Formularios/frmFacSelect.b1f")]
    class frmFacSelect : UserFormBase
    {
        // Objetos de formulario
        #region Objetos de formulario
        private static SAPbouiCOM.Form oForm;
        private static SAPbouiCOM.EditText txtCodSN;
        private static SAPbouiCOM.EditText txtNomSN;
        private static SAPbouiCOM.EditText txtFVencI;
        private static SAPbouiCOM.EditText txtFVencF;
        private static SAPbouiCOM.EditText txtTotal;
        private static SAPbouiCOM.EditText txtTotalChk;
        private static SAPbouiCOM.EditText txtTotalAsg;
        private static SAPbouiCOM.EditText txtTotalSy;
        private static SAPbouiCOM.EditText txtTotalSyChk;
        private static SAPbouiCOM.EditText txtTotalSyAsg;
        private static SAPbouiCOM.ComboBox cbxTipo;
        private static SAPbouiCOM.Button btnBuscar;
        private static SAPbouiCOM.Folder tab01;
        private static SAPbouiCOM.Folder tab02;
        private static SAPbouiCOM.Matrix mtxDocsSF;
        private static SAPbouiCOM.Matrix mtxDocsCF;
        private static SAPbouiCOM.Button btnMarcar;
        private static SAPbouiCOM.Button btnAsign;
        private static SAPbouiCOM.Button btnDesas;
        private static SAPbouiCOM.Button btnPrelim;
        #endregion

        public frmFacSelect()
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
                        // Matrix de documentos sin factoring
                        if (pVal.ItemUID.Equals("mtxDocsSF"))
                        {
                            if (pVal.ColUID.Equals("DocEntry"))
                            {
                                string sObjectType =
                                    ((SAPbouiCOM.EditText)(mtxDocsSF.Columns.Item("ObjType").Cells.Item(pVal.Row).Specific)).String;
                                SAPbouiCOM.Column oColumn = (SAPbouiCOM.Column)mtxDocsSF.Columns.Item(pVal.ColUID);
                                SAPbouiCOM.LinkedButton oLink = ((SAPbouiCOM.LinkedButton)(oColumn.ExtendedObject));
                                oLink.LinkedObjectType = sObjectType;
                            }
                        }

                        // Matrix de documentos con factoring
                        if (pVal.ItemUID.Equals("mtxDocsCF"))
                        {
                            if (pVal.ColUID.Equals("DocEntry"))
                            {
                                string sObjectType =
                                    ((SAPbouiCOM.EditText)(mtxDocsCF.Columns.Item("ObjType").Cells.Item(pVal.Row).Specific)).String;
                                SAPbouiCOM.Column oColumn = (SAPbouiCOM.Column)mtxDocsCF.Columns.Item(pVal.ColUID);
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
                        // Marcar manual
                        #region marcar manual
                        if (pVal.ItemUID.Equals("mtxDocsSF") && pVal.ColUID.Equals("Chk") && pVal.Row != 0)
                        {
                            // Documentos sin factoring
                            SumarMontos();
                        }
                        #endregion

                        // Boton buscar
                        #region buscar
                        if (pVal.ItemUID.Equals("btnBuscar"))
                        {
                            // Documentos sin factoring
                            CargarMatrixDocumentosSinFactoring();
                            Application.SBO_Application.StatusBar.SetText("Documentos cargados correctamente.", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success);
                            string NombreDT = "dtDocsCF";
                            SAPbouiCOM.DataTable datatable = oForm.DataSources.DataTables.Item(NombreDT);
                            datatable.Rows.Clear();
                            mtxDocsCF.LoadFromDataSource();
                            Comun.FuncionesComunes.LiberarObjetoGenerico(datatable);
                        }
                        #endregion

                        // Boton marcar
                        #region marcar
                        if (pVal.ItemUID.Equals("btnMarcar"))
                        {
                            // Documentos sin factoring
                            if (tab01.Selected)
                            {
                                string NombreDT = "dtDocsSF";
                                SAPbouiCOM.DataTable datatable = oForm.DataSources.DataTables.Item(NombreDT);
                                MarcarRegistrosMatrix(mtxDocsSF, datatable, "Chk");
                                Comun.FuncionesComunes.LiberarObjetoGenerico(datatable);
                            }
                            // Documentos con factoring
                            if (tab02.Selected)
                            {
                                string NombreDT = "dtDocsCF";
                                SAPbouiCOM.DataTable datatable = oForm.DataSources.DataTables.Item(NombreDT);
                                MarcarRegistrosMatrix(mtxDocsCF, datatable, "Chk");
                                Comun.FuncionesComunes.LiberarObjetoGenerico(datatable);
                            }
                        }
                        #endregion

                        // Boton asignar
                        #region asignar
                        if (pVal.ItemUID.Equals("btnAsign"))
                        {
                            // Documentos sin factoring
                            if (tab01.Selected)
                            {
                                string NombreDT = "dtDocsSF";
                                SAPbouiCOM.DataTable datatable = oForm.DataSources.DataTables.Item(NombreDT);
                                AsignarAFactoring(mtxDocsSF, datatable, "Chk");
                                Comun.FuncionesComunes.LiberarObjetoGenerico(datatable);
                            }
                        }
                        #endregion

                        // Boton desasignar
                        #region desasignar
                        if (pVal.ItemUID.Equals("btnDesas"))
                        {
                            // Documentos con factoring
                            if (tab02.Selected)
                            {
                                string NombreDT = "dtDocsCF";
                                SAPbouiCOM.DataTable datatable = oForm.DataSources.DataTables.Item(NombreDT);
                                DesasignarAFactoring(mtxDocsCF, datatable, "Chk");
                                Comun.FuncionesComunes.LiberarObjetoGenerico(datatable);
                            }
                        }
                        #endregion

                        // Boton guardar preliminar
                        #region guardar preliminar
                        if (pVal.ItemUID.Equals("btnPrelim"))
                        {
                            // Documentos con factoring
                            if (tab02.Selected)
                            {
                                string NombreDT = "dtDocsCF";
                                SAPbouiCOM.DataTable datatable = oForm.DataSources.DataTables.Item(NombreDT);
                                GuardarPreliminar(datatable, "Chk");
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
            oForm = ((SAPbouiCOM.Form)(Application.SBO_Application.Forms.Item("frmFacSelect")));
            txtCodSN = ((SAPbouiCOM.EditText)(GetItem("CodSN").Specific));
            txtNomSN = ((SAPbouiCOM.EditText)(GetItem("NomSN").Specific));
            txtFVencI = ((SAPbouiCOM.EditText)(GetItem("FVencI").Specific));
            txtFVencF = ((SAPbouiCOM.EditText)(GetItem("FVencF").Specific));
            txtTotal = ((SAPbouiCOM.EditText)(GetItem("Total").Specific));
            txtTotalChk = ((SAPbouiCOM.EditText)(GetItem("TotalChk").Specific));
            txtTotalAsg = ((SAPbouiCOM.EditText)(GetItem("TotalAsg").Specific));
            txtTotalSy = ((SAPbouiCOM.EditText)(GetItem("TotalSy").Specific));
            txtTotalSyChk = ((SAPbouiCOM.EditText)(GetItem("TotalSyChk").Specific));
            txtTotalSyAsg = ((SAPbouiCOM.EditText)(GetItem("TotalSyAsg").Specific));
            //cbxTipo = ((SAPbouiCOM.ComboBox)(GetItem("cbxTipo").Specific));
            btnBuscar = ((SAPbouiCOM.Button)(GetItem("btnBuscar").Specific));
            tab01 = ((SAPbouiCOM.Folder)(GetItem("tab01").Specific));
            tab02 = ((SAPbouiCOM.Folder)(GetItem("tab02").Specific));
            tab01.Select();
            mtxDocsSF = ((SAPbouiCOM.Matrix)(GetItem("mtxDocsSF").Specific));
            mtxDocsCF = ((SAPbouiCOM.Matrix)(GetItem("mtxDocsCF").Specific));
            btnMarcar = ((SAPbouiCOM.Button)(GetItem("btnMarcar").Specific));
            btnAsign = ((SAPbouiCOM.Button)(GetItem("btnAsign").Specific));
            btnDesas = ((SAPbouiCOM.Button)(GetItem("btnDesas").Specific));
            btnPrelim = ((SAPbouiCOM.Button)(GetItem("btnPrelim").Specific));
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
            oForm.DataSources.UserDataSources.Add("FVencI", SAPbouiCOM.BoDataType.dt_DATE);
            txtFVencI.DataBind.SetBound(true, "", "FVencI");
            // Fecha hasta
            oForm.DataSources.UserDataSources.Add("FVencF", SAPbouiCOM.BoDataType.dt_DATE);
            txtFVencF.DataBind.SetBound(true, "", "FVencF");

            // Total
            oForm.DataSources.UserDataSources.Add("Total", SAPbouiCOM.BoDataType.dt_PRICE);
            txtTotal.DataBind.SetBound(true, "", "Total");
            // Total Chk
            oForm.DataSources.UserDataSources.Add("TotalChk", SAPbouiCOM.BoDataType.dt_PRICE);
            txtTotalChk.DataBind.SetBound(true, "", "TotalChk");
            // Total Asg
            oForm.DataSources.UserDataSources.Add("TotalAsg", SAPbouiCOM.BoDataType.dt_PRICE);
            txtTotalAsg.DataBind.SetBound(true, "", "TotalAsg");
            // Total Sy
            oForm.DataSources.UserDataSources.Add("TotalSy", SAPbouiCOM.BoDataType.dt_PRICE);
            txtTotalSy.DataBind.SetBound(true, "", "TotalSy");
            // Total Sy Chk
            oForm.DataSources.UserDataSources.Add("TotalSyChk", SAPbouiCOM.BoDataType.dt_PRICE);
            txtTotalSyChk.DataBind.SetBound(true, "", "TotalSyChk");
            // Total Sy Asg
            oForm.DataSources.UserDataSources.Add("TotalSyAsg", SAPbouiCOM.BoDataType.dt_PRICE);
            txtTotalSyAsg.DataBind.SetBound(true, "", "TotalSyAsg");

            oForm.DataSources.DataTables.Add("dtDocsSF");
            oForm.DataSources.DataTables.Add("dtDocsCF");

            EstructuraMatrixDocumentosSinFactoring();
            EstructuraMatrixDocumentosConFactoring();

            mtxDocsSF.SelectionMode = SAPbouiCOM.BoMatrixSelect.ms_Single;
            mtxDocsCF.SelectionMode = SAPbouiCOM.BoMatrixSelect.ms_Single;
            oForm.Visible = true;
        }

        private static void EstructuraMatrixDocumentosSinFactoring()
        {
            string NombreDT = "dtDocsSF";
            SAPbouiCOM.DataTable datatable = oForm.DataSources.DataTables.Item(NombreDT);
            string _query = @"
                SELECT 'N' AS ""Chk"",
                ""ObjType"" AS ""ObjType"",
                ""DocEntry"" AS ""DocEntry"",
                ""DocEntry"" AS ""BaseEntry"",
                '00000000000000' AS ""BaseRef"",
                'FC' AS ""TipoDoc"",
                ""DocNum"" AS ""DocNum"",
                ""FolioNum"" AS ""FolioNum"",
                ""isIns"" AS ""isIns"",
                ""Indicator"" AS ""Indicator"",
                ""DocDate"" AS ""DocDate"",
                ""DocDueDate"" AS ""DocDueDate"",
                ""TaxDate"" AS ""TaxDate"",
                ""DocCur"" AS ""DocCur"",
                ""DocTotal"" AS ""DocTotal"",
                ""DocTotalSy"" AS ""DocTotalSy"",
                ""LicTradNum"" AS ""LicTradNum"",
                ""CardCode"" AS ""CardCode"",
                ""CardName"" AS ""CardName"",
                REPLICATE(' ', 250) AS ""Obs"",
                0 AS ""Otro""
                FROM ""OINV""
                WHERE ""DocStatus"" = 'O'
                AND 1 = 0";
            datatable.ExecuteQuery(_query);

            SAPbouiCOM.Columns oColumns;
            oColumns = mtxDocsSF.Columns;
            SAPbouiCOM.Column oColumn;

            oColumn = oColumns.Add("Chk", SAPbouiCOM.BoFormItemTypes.it_CHECK_BOX);
            oColumn.TitleObject.Caption = String.Empty;
            oColumn.Editable = true;
            oColumn.Width = 15;
            oColumn.ValOn = "Y";
            oColumn.ValOff = "N";
            oColumn.DataBind.Bind(NombreDT, "Chk");

            oColumn = oColumns.Add("ObjType", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "ObjType";
            oColumn.TitleObject.Sortable = false;
            oColumn.Editable = false;
            oColumn.Visible = false;
            oColumn.Width = 20;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "ObjType");

            oColumn = oColumns.Add("TipoDoc", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Tipo";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 60;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "TipoDoc");

            oColumn = oColumns.Add("DocEntry", SAPbouiCOM.BoFormItemTypes.it_LINKED_BUTTON);
            oColumn.TitleObject.Caption = "Link";
            oColumn.TitleObject.Sortable = false;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 15;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "DocEntry");

            oColumn = oColumns.Add("BaseEntry", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "BaseEntry";
            oColumn.TitleObject.Sortable = false;
            oColumn.Editable = false;
            oColumn.Visible = false;
            oColumn.Width = 15;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "BaseEntry");

            oColumn = oColumns.Add("BaseRef", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Origen";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 60;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "BaseRef");

            oColumn = oColumns.Add("DocNum", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Número";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 60;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "DocNum");

            oColumn = oColumns.Add("FolioNum", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Folio";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 60;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "FolioNum");

            oColumn = oColumns.Add("isIns", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Reserva";
            oColumn.TitleObject.Sortable = false;
            oColumn.Editable = false;
            oColumn.Visible = false;
            oColumn.Width = 20;
            oColumn.RightJustified = false;
            oColumn.DataBind.Bind(NombreDT, "isIns");

            oColumn = oColumns.Add("Indicator", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Indicador";
            oColumn.TitleObject.Sortable = false;
            oColumn.Editable = false;
            oColumn.Visible = false;
            oColumn.Width = 20;
            oColumn.RightJustified = false;
            oColumn.DataBind.Bind(NombreDT, "Indicator");

            oColumn = oColumns.Add("DocDate", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Fecha contable";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 80;
            oColumn.RightJustified = false;
            oColumn.DataBind.Bind(NombreDT, "DocDate");

            oColumn = oColumns.Add("DocDueDate", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Fecha vencimiento";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 80;
            oColumn.RightJustified = false;
            oColumn.DataBind.Bind(NombreDT, "DocDueDate");

            oColumn = oColumns.Add("TaxDate", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Fecha documento";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 80;
            oColumn.RightJustified = false;
            oColumn.DataBind.Bind(NombreDT, "TaxDate");

            oColumn = oColumns.Add("DocCur", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Moneda";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 80;
            oColumn.RightJustified = false;
            oColumn.DataBind.Bind(NombreDT, "DocCur");

            oColumn = oColumns.Add("DocTotal", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Monto";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 80;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "DocTotal");

            oColumn = oColumns.Add("DocTotalSy", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Monto (Sy)";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 80;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "DocTotalSy");

            oColumn = oColumns.Add("LicTradNum", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Rut SN";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 60;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "LicTradNum");

            oColumn = oColumns.Add("CardCode", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Código SN";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 80;
            oColumn.RightJustified = false;
            oColumn.DataBind.Bind(NombreDT, "CardCode");

            oColumn = oColumns.Add("CardName", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Nombre SN";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 120;
            oColumn.RightJustified = false;
            oColumn.DataBind.Bind(NombreDT, "CardName");

            oColumn = oColumns.Add("Obs", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Comentario";
            oColumn.TitleObject.Sortable = false;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 200;
            oColumn.RightJustified = false;
            oColumn.DataBind.Bind(NombreDT, "Obs");

            oColumn = oColumns.Add("Otro", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Otro";
            oColumn.TitleObject.Sortable = false;
            oColumn.Editable = false;
            oColumn.Visible = false;
            oColumn.Width = 20;
            oColumn.RightJustified = false;
            oColumn.DataBind.Bind(NombreDT, "Otro");

            mtxDocsSF.Clear();
            mtxDocsSF.LoadFromDataSource();
            Comun.FuncionesComunes.LiberarObjetoGenerico(datatable);
        }

        private static void EstructuraMatrixDocumentosConFactoring()
        {
            string NombreDT = "dtDocsCF";
            SAPbouiCOM.DataTable datatable = oForm.DataSources.DataTables.Item(NombreDT);
            string _query = @"
                SELECT 'N' AS ""Chk"",
                ""ObjType"" AS ""ObjType"",
                ""DocEntry"" AS ""DocEntry"",
                ""DocEntry"" AS ""BaseEntry"",
                '00000000000000' AS ""BaseRef"",
                'FC' AS ""TipoDoc"",
                ""DocNum"" AS ""DocNum"",
                ""FolioNum"" AS ""FolioNum"",
                ""isIns"" AS ""isIns"",
                ""Indicator"" AS ""Indicator"",
                ""DocDate"" AS ""DocDate"",
                ""DocDueDate"" AS ""DocDueDate"",
                ""TaxDate"" AS ""TaxDate"",
                ""DocCur"" AS ""DocCur"",
                ""DocTotal"" AS ""DocTotal"",
                ""DocTotalSy"" AS ""DocTotalSy"",
                ""LicTradNum"" AS ""LicTradNum"",
                ""CardCode"" AS ""CardCode"",
                ""CardName"" AS ""CardName"",
                REPLICATE(' ', 250) AS ""Obs"",
                0 AS ""Otro""
                FROM ""OINV""
                WHERE ""DocStatus"" = 'O'
                AND 1 = 0";
            datatable.ExecuteQuery(_query);

            SAPbouiCOM.Columns oColumns;
            oColumns = mtxDocsCF.Columns;
            SAPbouiCOM.Column oColumn;

            oColumn = oColumns.Add("Chk", SAPbouiCOM.BoFormItemTypes.it_CHECK_BOX);
            oColumn.TitleObject.Caption = String.Empty;
            oColumn.Editable = true;
            oColumn.Width = 15;
            oColumn.ValOn = "Y";
            oColumn.ValOff = "N";
            oColumn.DataBind.Bind(NombreDT, "Chk");

            oColumn = oColumns.Add("ObjType", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "ObjType";
            oColumn.TitleObject.Sortable = false;
            oColumn.Editable = false;
            oColumn.Visible = false;
            oColumn.Width = 20;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "ObjType");

            oColumn = oColumns.Add("TipoDoc", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Tipo";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 60;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "TipoDoc");

            oColumn = oColumns.Add("DocEntry", SAPbouiCOM.BoFormItemTypes.it_LINKED_BUTTON);
            oColumn.TitleObject.Caption = "Link";
            oColumn.TitleObject.Sortable = false;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 15;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "DocEntry");

            oColumn = oColumns.Add("BaseEntry", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "BaseEntry";
            oColumn.TitleObject.Sortable = false;
            oColumn.Editable = false;
            oColumn.Visible = false;
            oColumn.Width = 15;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "BaseEntry");

            oColumn = oColumns.Add("BaseRef", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Origen";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 60;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "BaseRef");

            oColumn = oColumns.Add("DocNum", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Número";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 60;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "DocNum");

            oColumn = oColumns.Add("FolioNum", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Folio";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 60;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "FolioNum");

            oColumn = oColumns.Add("isIns", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Reserva";
            oColumn.TitleObject.Sortable = false;
            oColumn.Editable = false;
            oColumn.Visible = false;
            oColumn.Width = 20;
            oColumn.RightJustified = false;
            oColumn.DataBind.Bind(NombreDT, "isIns");

            oColumn = oColumns.Add("Indicator", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Indicador";
            oColumn.TitleObject.Sortable = false;
            oColumn.Editable = false;
            oColumn.Visible = false;
            oColumn.Width = 20;
            oColumn.RightJustified = false;
            oColumn.DataBind.Bind(NombreDT, "Indicator");

            oColumn = oColumns.Add("DocDate", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Fecha contable";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 80;
            oColumn.RightJustified = false;
            oColumn.DataBind.Bind(NombreDT, "DocDate");

            oColumn = oColumns.Add("DocDueDate", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Fecha vencimiento";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 80;
            oColumn.RightJustified = false;
            oColumn.DataBind.Bind(NombreDT, "DocDueDate");

            oColumn = oColumns.Add("TaxDate", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Fecha documento";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 80;
            oColumn.RightJustified = false;
            oColumn.DataBind.Bind(NombreDT, "TaxDate");

            oColumn = oColumns.Add("DocCur", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Moneda";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 80;
            oColumn.RightJustified = false;
            oColumn.DataBind.Bind(NombreDT, "DocCur");

            oColumn = oColumns.Add("DocTotal", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Monto";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 80;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "DocTotal");

            oColumn = oColumns.Add("DocTotalSy", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Monto (Sy)";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 80;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "DocTotalSy");

            oColumn = oColumns.Add("LicTradNum", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Rut SN";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 60;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "LicTradNum");

            oColumn = oColumns.Add("CardCode", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Código SN";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 80;
            oColumn.RightJustified = false;
            oColumn.DataBind.Bind(NombreDT, "CardCode");

            oColumn = oColumns.Add("CardName", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Nombre SN";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 120;
            oColumn.RightJustified = false;
            oColumn.DataBind.Bind(NombreDT, "CardName");

            oColumn = oColumns.Add("Obs", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Comentario";
            oColumn.TitleObject.Sortable = false;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 200;
            oColumn.RightJustified = false;
            oColumn.DataBind.Bind(NombreDT, "Obs");

            oColumn = oColumns.Add("Otro", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Otro";
            oColumn.TitleObject.Sortable = false;
            oColumn.Editable = false;
            oColumn.Visible = false;
            oColumn.Width = 20;
            oColumn.RightJustified = false;
            oColumn.DataBind.Bind(NombreDT, "Otro");

            mtxDocsCF.Clear();
            mtxDocsCF.LoadFromDataSource();
            Comun.FuncionesComunes.LiberarObjetoGenerico(datatable);
        }

        private static void CargarMatrixDocumentosSinFactoring()
        {
            // Filtro por Socio de Negocio
            SAPbouiCOM.EditText oSN = (SAPbouiCOM.EditText)oForm.Items.Item("CodSN").Specific;
            string FiltroSN = oSN.Value;
            string FiltroSNZ = oSN.Value;
            if (!string.IsNullOrEmpty(FiltroSN))
            {
                FiltroSN = string.Format(@" AND ""CardCode"" = '{0}'", FiltroSN);
                FiltroSNZ = string.Format(@" AND TZ.""CardCode"" = '{0}'", FiltroSNZ);
            }
            else
            {
                FiltroSN = string.Empty;
                FiltroSNZ = string.Empty;
            }

            // Filtro por Fechas
            string FechaInicial = string.Empty;
            string FechaFinal = string.Empty;
            DateTime dt;
            string Mes = string.Empty;
            string Dia = string.Empty;

            // Fecha de vencimiento
            SAPbouiCOM.EditText oFDesde = (SAPbouiCOM.EditText)oForm.Items.Item("FVencI").Specific;
            string DesdeFecha = oFDesde.Value;
            SAPbouiCOM.EditText oFHasta = (SAPbouiCOM.EditText)oForm.Items.Item("FVencF").Specific;
            string HastaFecha = oFHasta.Value;
            // Fechas en formato AAAA-MM-DD
            if (!string.IsNullOrEmpty(DesdeFecha) && !string.IsNullOrEmpty(HastaFecha))
            {
                FechaInicial = string.Format("{0}{1}{2}", DesdeFecha.Substring(0, 4), DesdeFecha.Substring(4, 2), DesdeFecha.Substring(6, 2));
                FechaFinal = string.Format("{0}{1}{2}", HastaFecha.Substring(0, 4), HastaFecha.Substring(4, 2), HastaFecha.Substring(6, 2));
            }
            else
            {
                // Por defecto trae hasta 8 días
                dt = new DateTime(DateTime.Now.Year, DateTime.Today.Month, DateTime.Today.Day);
                Mes = (dt.Month.ToString().Length == 1) ? dt.Month.ToString().PadLeft(2, '0') : dt.Month.ToString();
                Dia = (dt.Day.ToString().Length == 1) ? dt.Day.ToString().PadLeft(2, '0') : dt.Day.ToString();

                FechaInicial = string.Format("{0}{1}{2}", dt.Year.ToString(), Mes, Dia);

                dt = DateTime.Today.AddDays(8);
                Mes = (dt.Month.ToString().Length == 1) ? dt.Month.ToString().PadLeft(2, '0') : dt.Month.ToString();
                Dia = (dt.Day.ToString().Length == 1) ? dt.Day.ToString().PadLeft(2, '0') : dt.Day.ToString();

                FechaFinal = string.Format("{0}{1}{2}", dt.Year.ToString(), Mes, Dia);
            }
            string FiltroFechaVence = string.Format(@" AND ""DocDueDate"" BETWEEN '{0}' AND '{1}'", FechaInicial, FechaFinal);
            string FiltroFechaVenceZ = string.Format(@" AND TZ.""DocDueDate"" BETWEEN '{0}' AND '{1}'", FechaInicial, FechaFinal);

            string NombreDT = "dtDocsSF";
            SAPbouiCOM.DataTable datatable = oForm.DataSources.DataTables.Item(NombreDT);

            string _query = @"
                SELECT 'N' AS ""Chk"",
                ""ObjType"" AS ""ObjType"",
                ""DocEntry"" AS ""DocEntry"",
                ""DocEntry"" AS ""BaseEntry"",
                '--' AS ""BaseRef"",
                'FC' AS ""TipoDoc"",
                ""DocNum"" AS ""DocNum"",
                ""FolioNum"" AS ""FolioNum"",
                ""isIns"" AS ""isIns"",
                ""Indicator"" AS ""Indicator"",
                ""DocDate"" AS ""DocDate"",
                ""DocDueDate"" AS ""DocDueDate"",
                ""TaxDate"" AS ""TaxDate"",
                ""DocCur"" AS ""DocCur"",
                ""DocTotal"" AS ""DocTotal"",
                ""DocTotalSy"" AS ""DocTotalSy"",
                ""LicTradNum"" AS ""LicTradNum"",
                ""CardCode"" AS ""CardCode"",
                ""CardName"" AS ""CardName"",
                REPLICATE(' ', 250) AS ""Obs"",
                0 AS ""Otro""
                FROM ""OINV""
                WHERE DATEDIFF(D, ""DocDate"", GETDATE()) > 8 AND ""DocStatus"" = 'O'";
            _query += FiltroSN + FiltroFechaVence + @"
                AND ""DocEntry"" NOT IN (SELECT DISTINCT ""U_DOCENTRY"" FROM ""@SO_FCTRNGL"" WHERE ""U_TIPODOC"" = 'FC')";

            _query += @" UNION ALL
                SELECT 'N' AS ""Chk"",
                T0.""ObjType"" AS ""ObjType"",
                T0.""DocEntry"" AS ""DocEntry"",
                T1.""BaseEntry"" AS ""BaseEntry"",
                T1.""BaseRef"" AS ""BaseRef"",
                'NC' AS ""TipoDoc"",
                T0.""DocNum"" AS ""DocNum"",
                T0.""FolioNum"" AS ""FolioNum"",
                T0.""isIns"" AS ""isIns"",
                T0.""Indicator"" AS ""Indicator"",
                T0.""DocDate"" AS ""DocDate"",
                T0.""DocDueDate"" AS ""DocDueDate"",
                T0.""TaxDate"" AS ""TaxDate"",
                T0.""DocCur"" AS ""DocCur"",
                T0.""DocTotal"" AS ""DocTotal"",
                T0.""DocTotalSy"" AS ""DocTotalSy"",
                T0.""LicTradNum"" AS ""LicTradNum"",
                T0.""CardCode"" AS ""CardCode"",
                T0.""CardName"" AS ""CardName"",
                REPLICATE(' ', 250) AS ""Obs"",
                0 AS ""Otro""
                FROM ""ORIN"" T0
                JOIN ""RIN1"" T1 ON T0.""DocEntry"" = T1.""DocEntry"" AND T1.""BaseLine"" = (SELECT MIN(T2.""BaseLine"") FROM ""RIN1"" T2 WHERE T2.""DocEntry"" = T0.""DocEntry"" AND T2.""BaseType"" = 13)
                JOIN ""OINV"" TZ ON T1.""BaseEntry"" = TZ.""DocEntry"" AND DATEDIFF(D, TZ.""DocDate"", GETDATE()) > 8" + FiltroSNZ + FiltroFechaVenceZ + @"
                WHERE T0.""DocStatus"" = 'O'
                AND T0.""DocEntry"" NOT IN (SELECT DISTINCT ""U_DOCENTRY"" FROM ""@SO_FCTRNGL"" WHERE ""U_TIPODOC"" = 'NC')
                ";

            _query = @"SELECT * FROM (" + _query + @") X
                ORDER BY X.""BaseEntry"" ASC, X.""TipoDoc"" ASC, X.""DocDueDate"" DESC";
            datatable.ExecuteQuery(_query);
            oForm.Freeze(true);
            mtxDocsSF.Clear();
            mtxDocsSF.LoadFromDataSource();
            Comun.FuncionesComunes.LiberarObjetoGenerico(datatable);
            SumarMontos();
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
            SumarMontos();
            oForm.Freeze(false);
        }

        private static void SumarMontos()
        {
            double Total = 0;
            double TotalChk = 0;
            double TotalSy = 0;
            double TotalSyChk = 0;
            string NombreDT = "dtDocsSF";
            SAPbouiCOM.DataTable datatable = oForm.DataSources.DataTables.Item(NombreDT);
            mtxDocsSF.FlushToDataSource();
            if (!datatable.IsEmpty)
            {
                for (int i = 0; i < datatable.Rows.Count; i++)
                {
                    string TipoDoc = datatable.GetValue("TipoDoc", i).ToString();
                    if (TipoDoc.Equals("FC"))
                    {
                        Total += (double)datatable.GetValue("DocTotal", i);
                        TotalSy += (double)datatable.GetValue("DocTotalSy", i);
                    }
                    else
                    {
                        Total -= (double)datatable.GetValue("DocTotal", i);
                        TotalSy -= (double)datatable.GetValue("DocTotalSy", i);
                    }
                    if (datatable.GetValue("Chk", i).ToString().Equals("Y"))
                    {
                        if (TipoDoc.Equals("FC"))
                        {
                            TotalChk += (double)datatable.GetValue("DocTotal", i);
                            TotalSyChk += (double)datatable.GetValue("DocTotalSy", i);
                        }
                        else
                        {
                            TotalChk -= (double)datatable.GetValue("DocTotal", i);
                            TotalSyChk -= (double)datatable.GetValue("DocTotalSy", i);
                        }
                    }
                }
            }
            else
            {
                Total = 0;
                TotalChk = 0;
                TotalSy = 0;
                TotalSyChk = 0;
            }
            Comun.FuncionesComunes.LiberarObjetoGenerico(datatable);
            double TotalAsg = 0;
            double TotalSyAsg = 0;
            NombreDT = "dtDocsCF";
            datatable = oForm.DataSources.DataTables.Item(NombreDT);
            mtxDocsCF.FlushToDataSource();
            if (!datatable.IsEmpty)
            {
                for (int i = 0; i < datatable.Rows.Count; i++)
                {
                    string TipoDoc = datatable.GetValue("TipoDoc", i).ToString();
                    if (TipoDoc.Equals("FC"))
                    {
                        TotalAsg += (double)datatable.GetValue("DocTotal", i);
                        TotalSyAsg += (double)datatable.GetValue("DocTotalSy", i);
                    }
                    else
                    {
                        TotalAsg -= (double)datatable.GetValue("DocTotal", i);
                        TotalSyAsg -= (double)datatable.GetValue("DocTotalSy", i);
                    }
                }
            }
            else
            {
                TotalAsg = 0;
                TotalSyAsg = 0;
            }
            Comun.FuncionesComunes.LiberarObjetoGenerico(datatable);
            oForm.DataSources.UserDataSources.Item("Total").Value = Total.ToString();
            oForm.DataSources.UserDataSources.Item("TotalChk").Value = TotalChk.ToString();
            oForm.DataSources.UserDataSources.Item("TotalSy").Value = TotalSy.ToString();
            oForm.DataSources.UserDataSources.Item("TotalSyChk").Value = TotalSyChk.ToString();
            oForm.DataSources.UserDataSources.Item("TotalAsg").Value = TotalAsg.ToString();
            oForm.DataSources.UserDataSources.Item("TotalSyAsg").Value = TotalSyAsg.ToString();
        }

        private static void AsignarAFactoring(SAPbouiCOM.Matrix oMatrix, SAPbouiCOM.DataTable dtDoc, string columna)
        {
            oForm.Freeze(true);
            oMatrix.FlushToDataSource();
            mtxDocsCF.FlushToDataSource();
            string NombreDT = "dtDocsCF";
            SAPbouiCOM.DataTable dtAux = oForm.DataSources.DataTables.Item(NombreDT);

            int index = 0;
            if (!dtAux.IsEmpty)
            {
                index = dtAux.Rows.Count;
            }
            for (int i = 0; i < dtDoc.Rows.Count; i++)
            {
                if (dtDoc.GetValue(columna, i).ToString().Equals("Y"))
                {
                    if (index > 0)
                    {
                        dtAux.Rows.Add();
                    }
                    for (int k = 0; k < dtDoc.Columns.Count; k++)
                    {
                        dtAux.SetValue(k, index, dtDoc.GetValue(k, i));
                    }
                    index = index + 1;
                    dtDoc.Rows.Remove(i);
                    i = i - 1;
                }
            }
            oMatrix.LoadFromDataSource();
            mtxDocsCF.LoadFromDataSource();
            Comun.FuncionesComunes.LiberarObjetoGenerico(dtAux);
            SumarMontos();
            oForm.Freeze(false);
        }

        private static void DesasignarAFactoring(SAPbouiCOM.Matrix oMatrix, SAPbouiCOM.DataTable dtDoc, string columna)
        {
            oForm.Freeze(true);
            oMatrix.FlushToDataSource();
            mtxDocsSF.FlushToDataSource();
            string NombreDT = "dtDocsSF";
            SAPbouiCOM.DataTable dtAux = oForm.DataSources.DataTables.Item(NombreDT);

            int index = 0;
            if (!dtAux.IsEmpty)
            {
                index = dtAux.Rows.Count;
            }
            for (int i = 0; i < dtDoc.Rows.Count; i++)
            {
                if (dtDoc.GetValue(columna, i).ToString().Equals("Y"))
                {
                    if (index > 0)
                    {
                        dtAux.Rows.Add();
                    }
                    for (int k = 0; k < dtDoc.Columns.Count; k++)
                    {
                        dtAux.SetValue(k, index, dtDoc.GetValue(k, i));
                    }
                    index = index + 1;
                    dtDoc.Rows.Remove(i);
                    i = i - 1;
                }
            }
            oMatrix.LoadFromDataSource();
            mtxDocsSF.LoadFromDataSource();
            Comun.FuncionesComunes.LiberarObjetoGenerico(dtAux);
            SumarMontos();
            oForm.Freeze(false);
        }

        private static void GuardarPreliminar(SAPbouiCOM.DataTable dtDoc, string columna)
        {
            mtxDocsCF.FlushToDataSource();
            if (!dtDoc.IsEmpty)
            {
                Clases.Factoring factoring = new Clases.Factoring();
                factoring.U_FECHA = DateTime.Now.Date;
                factoring.U_ESTADO = "P";
                for (int i = 0; i < dtDoc.Rows.Count; i++)
                {
                    Clases.FactoringLines documento = new Clases.FactoringLines();
                    documento.U_OBJTYPE = dtDoc.GetValue("ObjType", i).ToString();
                    documento.U_DOCENTRY = (int)dtDoc.GetValue("DocEntry", i);
                    documento.U_BASEENTRY = (int)dtDoc.GetValue("BaseEntry", i);
                    documento.U_BASEREF = dtDoc.GetValue("BaseRef", i).ToString();
                    documento.U_TIPODOC = dtDoc.GetValue("TipoDoc", i).ToString();
                    documento.U_DOCNUM = (int)dtDoc.GetValue("DocNum", i);
                    documento.U_FOLIONUM = (int)dtDoc.GetValue("FolioNum", i);
                    documento.U_ISINS = dtDoc.GetValue("isIns", i).ToString();
                    documento.U_INDICATOR = dtDoc.GetValue("Indicator", i).ToString();
                    var docdate = dtDoc.GetValue("DocDate", i);
                    documento.U_DOCDATE = (DateTime)docdate;
                    var docduedate = dtDoc.GetValue("DocDueDate", i);
                    documento.U_DOCDUEDATE = (DateTime)docduedate;
                    var taxdate = dtDoc.GetValue("TaxDate", i);
                    documento.U_TAXDATE = (DateTime)taxdate;
                    documento.U_DOCCUR = dtDoc.GetValue("DocCur", i).ToString();
                    documento.U_DOCTOTAL = (double)dtDoc.GetValue("DocTotal", i);
                    documento.U_DOCTOTALSY = (double)dtDoc.GetValue("DocTotalSy", i);
                    documento.U_LICTRADNUM = dtDoc.GetValue("LicTradNum", i).ToString();
                    documento.U_CARDCODE = dtDoc.GetValue("CardCode", i).ToString();
                    documento.U_CARDNAME = dtDoc.GetValue("CardName", i).ToString();
                    factoring.Documentos.Add(documento);
                }
                if (factoring.Documentos != null && factoring.Documentos.Count > 0)
                {
                    var result = SBO.ModeloSBO.AddFactoring(factoring);
                    if (result.Success)
                    {
                        Application.SBO_Application.StatusBar.SetText(string.Format("Factoring Preliminar guardado correctamente. Id: {0}", result.DocEntry), SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success);
                        dtDoc.Rows.Clear();
                    }
                    else
                    {
                        Application.SBO_Application.StatusBar.SetText(string.Format("Factoring Preliminar no guardado. Mensaje: {0}", result.Mensaje), SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
                    }
                }
            }
            oForm.Freeze(true);
            mtxDocsCF.LoadFromDataSource();
            oForm.Freeze(false);
        }
    }
}
