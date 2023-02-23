using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM.Framework;
using System.Globalization;
using EASendMail;

namespace Soindus.AddOnFactoringCob.Formularios
{
    [FormAttribute("Soindus.AddOnFactoringCob.Formularios.frmFactoring", "Formularios/frmFactoring.b1f")]
    class frmFactoring : UserFormBase
    {
        // Objetos de formulario
        #region Objetos de formulario
        private static SAPbouiCOM.Form oForm;
        private static SAPbouiCOM.EditText txtEntidad;
        private static SAPbouiCOM.EditText txtMoneda;
        private static SAPbouiCOM.EditText txtValor;
        private static SAPbouiCOM.EditText txtFecha;
        private static SAPbouiCOM.EditText txtNumOper;
        private static SAPbouiCOM.ComboBox cbxTipoRes;
        private static SAPbouiCOM.EditText txtId;
        private static SAPbouiCOM.EditText txtEstado;
        private static SAPbouiCOM.EditText txtTotal;
        private static SAPbouiCOM.EditText txtTotalChk;
        private static SAPbouiCOM.EditText txtTotalSy;
        private static SAPbouiCOM.EditText txtTotalSyChk;
        private static SAPbouiCOM.Button btnBuscar;
        private static SAPbouiCOM.Button btnGuardar;
        private static SAPbouiCOM.Folder tab01;
        private static SAPbouiCOM.Folder tab02;
        private static SAPbouiCOM.Matrix mtxDocs;
        private static SAPbouiCOM.Button btnMarcar;
        private static SAPbouiCOM.Button btnLinea;
        private static SAPbouiCOM.Button btnElim;
        private static SAPbouiCOM.Button btnDef;
        private static int globalId;
        private static string SepDecimal;
        private static string SepMiles;
        #endregion

        public frmFactoring(int Id = 0)
        {
            globalId = Id;
            SepDecimal = SBO.ConsultasSBO.ObtenerSeparadorDecimal();
            SepMiles = SBO.ConsultasSBO.ObtenerSeparadorMiles();
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
                        if (pVal.ItemUID.Equals("mtxDocs"))
                        {
                            if (pVal.ColUID.Equals("DocEntry"))
                            {
                                string sObjectType =
                                    ((SAPbouiCOM.EditText)(mtxDocs.Columns.Item("ObjType").Cells.Item(pVal.Row).Specific)).String;
                                SAPbouiCOM.Column oColumn = (SAPbouiCOM.Column)mtxDocs.Columns.Item(pVal.ColUID);
                                SAPbouiCOM.LinkedButton oLink = ((SAPbouiCOM.LinkedButton)(oColumn.ExtendedObject));
                                oLink.LinkedObjectType = sObjectType;
                            }
                        }
                    }
                    #endregion
                }
                else
                {
                    if (pVal.EventType.Equals(SAPbouiCOM.BoEventTypes.et_CLICK))
                    {
                        // Marcar manual
                        #region marcar manual
                        if (pVal.ItemUID.Equals("mtxDocs") && pVal.ColUID.Equals("Chk") && pVal.Row != 0)
                        {
                            // Documentos en factoring
                            SumarMontos();
                        }
                        #endregion

                        // Boton marcar
                        #region marcar
                        if (pVal.ItemUID.Equals("btnMarcar"))
                        {
                            // Documentos en factoring
                            if (tab01.Selected)
                            {
                                string NombreDT = "dtDocs";
                                SAPbouiCOM.DataTable datatable = oForm.DataSources.DataTables.Item(NombreDT);
                                MarcarRegistrosMatrix(mtxDocs, datatable, "Chk");
                                Comun.FuncionesComunes.LiberarObjetoGenerico(datatable);
                            }
                        }
                        #endregion

                        // Boton eliminar línea
                        #region eliminar lénea
                        if (pVal.ItemUID.Equals("btnLinea"))
                        {
                            // Documentos en factoring
                            if (tab01.Selected)
                            {
                                if (!oForm.DataSources.UserDataSources.Item("Estado").Value.Equals("Definitivo"))
                                {
                                    string NombreDT = "dtDocs";
                                    SAPbouiCOM.DataTable datatable = oForm.DataSources.DataTables.Item(NombreDT);
                                    EliminarRegistroMatrix(mtxDocs, datatable, "Chk");
                                    Comun.FuncionesComunes.LiberarObjetoGenerico(datatable);
                                }
                            }
                        }
                        #endregion

                        // Boton guardar
                        #region guardar
                        if (pVal.ItemUID.Equals("btnGuardar"))
                        {
                            // Documentos en factoring
                            if (tab01.Selected)
                            {
                                if (!oForm.DataSources.UserDataSources.Item("Estado").Value.Equals("Definitivo"))
                                {
                                    string NombreDT = "dtDocs";
                                    SAPbouiCOM.DataTable datatable = oForm.DataSources.DataTables.Item(NombreDT);
                                    ActualizarFactoring(datatable, "Chk");
                                    Comun.FuncionesComunes.LiberarObjetoGenerico(datatable);
                                }
                            }
                        }
                        #endregion

                        // Boton eliminar factoring
                        #region eliminar factoring
                        if (pVal.ItemUID.Equals("btnElim"))
                        {
                            if (!oForm.DataSources.UserDataSources.Item("Estado").Value.Equals("Definitivo"))
                            {
                                int resp = Application.SBO_Application.MessageBox("¿Seguro desea eliminar el documento de factoring preliminar?", 2, "Si", "No");
                                if (resp.Equals(1))
                                {
                                    EliminarDocumentoFactoring();
                                }
                            }
                        }
                        #endregion

                        // Boton crear definitivo
                        #region crear definitivo
                        if (pVal.ItemUID.Equals("btnDef"))
                        {
                            if (!oForm.DataSources.UserDataSources.Item("Estado").Value.Equals("Definitivo"))
                            {
                                int resp = Application.SBO_Application.MessageBox("¿Seguro desea guardar el documento de factoring como definitivo?", 2, "Si", "No");
                                if (resp.Equals(1))
                                {
                                    CrearFactoring();
                                }
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

                        if (pVal.ItemUID.Equals("Entidad"))
                        {
                            if (oDataTable != null)
                            {
                                oForm.DataSources.UserDataSources.Item("Entidad").Value = string.Empty;
                                string _Value = string.Empty;
                                _Value = oDataTable.GetValue(0, 0).ToString();
                                oForm.DataSources.UserDataSources.Item("Entidad").Value = _Value;
                            }
                        }
                        if (pVal.ItemUID.Equals("Moneda"))
                        {
                            if (oDataTable != null)
                            {
                                oForm.DataSources.UserDataSources.Item("Moneda").Value = string.Empty;
                                string _Value = string.Empty;
                                _Value = oDataTable.GetValue(0, 0).ToString();
                                oForm.DataSources.UserDataSources.Item("Moneda").Value = _Value;
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
            oForm = ((SAPbouiCOM.Form)(Application.SBO_Application.Forms.Item("frmFactoring")));
            txtEntidad = ((SAPbouiCOM.EditText)(GetItem("Entidad").Specific));
            txtMoneda = ((SAPbouiCOM.EditText)(GetItem("Moneda").Specific));
            txtValor = ((SAPbouiCOM.EditText)(GetItem("Valor").Specific));
            txtFecha = ((SAPbouiCOM.EditText)(GetItem("Fecha").Specific));
            txtNumOper = ((SAPbouiCOM.EditText)(GetItem("NumOper").Specific));
            cbxTipoRes = ((SAPbouiCOM.ComboBox)(GetItem("TipoRes").Specific));
            txtId = ((SAPbouiCOM.EditText)(GetItem("Id").Specific));
            txtEstado = ((SAPbouiCOM.EditText)(GetItem("Estado").Specific));
            txtTotal = ((SAPbouiCOM.EditText)(GetItem("Total").Specific));
            txtTotalChk = ((SAPbouiCOM.EditText)(GetItem("TotalChk").Specific));
            txtTotalSy = ((SAPbouiCOM.EditText)(GetItem("TotalSy").Specific));
            txtTotalSyChk = ((SAPbouiCOM.EditText)(GetItem("TotalSyChk").Specific));
            btnGuardar = ((SAPbouiCOM.Button)(GetItem("btnGuardar").Specific));
            tab01 = ((SAPbouiCOM.Folder)(GetItem("tab01").Specific));
            tab02 = ((SAPbouiCOM.Folder)(GetItem("tab02").Specific));
            tab01.Select();
            mtxDocs = ((SAPbouiCOM.Matrix)(GetItem("mtxDocs").Specific));
            btnMarcar = ((SAPbouiCOM.Button)(GetItem("btnMarcar").Specific));
            btnLinea = ((SAPbouiCOM.Button)(GetItem("btnLinea").Specific));
            btnElim = ((SAPbouiCOM.Button)(GetItem("btnElim").Specific));
            btnDef = ((SAPbouiCOM.Button)(GetItem("btnDef").Specific));
        }

        private void CargarFormulario()
        {
            // Propiedades del formulario

            // Choose from list
            SAPbouiCOM.ChooseFromListCollection oCFLs = null;
            SAPbouiCOM.ChooseFromList oCFL = null;
            SAPbouiCOM.ChooseFromListCreationParams oCFLCreationParams = null;
            SAPbouiCOM.Conditions oCons = null;

            //// Socio de negocios
            //oForm.DataSources.UserDataSources.Add("CodSN", SAPbouiCOM.BoDataType.dt_SHORT_TEXT, 200);
            //txtCodSN.DataBind.SetBound(true, "", "CodSN");
            //oForm.DataSources.UserDataSources.Add("NomSN", SAPbouiCOM.BoDataType.dt_SHORT_TEXT, 200);
            //txtNomSN.DataBind.SetBound(true, "", "NomSN");

            //oCFLs = oForm.ChooseFromLists;
            //oCFLCreationParams = ((SAPbouiCOM.ChooseFromListCreationParams)(Application.SBO_Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_ChooseFromListCreationParams)));
            //oCFLCreationParams.MultiSelection = false;
            //oCFLCreationParams.ObjectType = "2";
            //oCFLCreationParams.UniqueID = "cflSN";

            //oCFL = oCFLs.Add(oCFLCreationParams);

            //oCons = new SAPbouiCOM.Conditions();
            ////Dar condiciones al ChooseFromList
            //oCons = oCFL.GetConditions();

            //SAPbouiCOM.Condition oCon = oCons.Add();
            //oCon.Alias = "CardType";
            //oCon.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
            //oCon.CondVal = "C";

            //oCFL.SetConditions(oCons);

            ////Asignamos el ChoosefromList al campo de texto
            //txtCodSN.ChooseFromListUID = "cflSN";
            //txtCodSN.ChooseFromListAlias = "CardCode";

            // Entidad
            oForm.DataSources.UserDataSources.Add("Entidad", SAPbouiCOM.BoDataType.dt_SHORT_TEXT);
            txtEntidad.DataBind.SetBound(true, "", "Entidad");
            oCFLs = oForm.ChooseFromLists;
            oCFLCreationParams = ((SAPbouiCOM.ChooseFromListCreationParams)(Application.SBO_Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_ChooseFromListCreationParams)));
            oCFLCreationParams.MultiSelection = false;
            oCFLCreationParams.ObjectType = "3";
            oCFLCreationParams.UniqueID = "cflBanco";
            oCFL = oCFLs.Add(oCFLCreationParams);
            txtEntidad.ChooseFromListUID = "cflBanco";
            txtEntidad.ChooseFromListAlias = "AbsEntry";
            // Moneda
            oForm.DataSources.UserDataSources.Add("Moneda", SAPbouiCOM.BoDataType.dt_SHORT_TEXT);
            txtMoneda.DataBind.SetBound(true, "", "Moneda");
            oCFLs = oForm.ChooseFromLists;
            oCFLCreationParams = ((SAPbouiCOM.ChooseFromListCreationParams)(Application.SBO_Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_ChooseFromListCreationParams)));
            oCFLCreationParams.MultiSelection = false;
            oCFLCreationParams.ObjectType = "37";
            oCFLCreationParams.UniqueID = "cflMoneda";
            oCFL = oCFLs.Add(oCFLCreationParams);
            txtMoneda.ChooseFromListUID = "cflMoneda";
            txtMoneda.ChooseFromListAlias = "CurrCode";
            // Valor
            oForm.DataSources.UserDataSources.Add("Valor", SAPbouiCOM.BoDataType.dt_PRICE);
            txtValor.DataBind.SetBound(true, "", "Valor");
            // Vencimiento
            oForm.DataSources.UserDataSources.Add("Fecha", SAPbouiCOM.BoDataType.dt_DATE);
            txtFecha.DataBind.SetBound(true, "", "Fecha");
            // Número operación
            oForm.DataSources.UserDataSources.Add("NumOper", SAPbouiCOM.BoDataType.dt_LONG_TEXT);
            txtNumOper.DataBind.SetBound(true, "", "NumOper");
            // Tipo responsabilidad
            oForm.DataSources.UserDataSources.Add("TipoRes", SAPbouiCOM.BoDataType.dt_SHORT_TEXT);
            cbxTipoRes.DataBind.SetBound(true, "", "TipoRes");
            cbxTipoRes.ValidValues.Add("", "");
            cbxTipoRes.ValidValues.Add("CR", "Con responsabilidad");
            cbxTipoRes.ValidValues.Add("SR", "Sin responsabilidad");
            cbxTipoRes.ValidValues.Add("FN", "Financiero");
            cbxTipoRes.ExpandType = SAPbouiCOM.BoExpandType.et_DescriptionOnly;
            // Id
            oForm.DataSources.UserDataSources.Add("Id", SAPbouiCOM.BoDataType.dt_SHORT_TEXT);
            txtId.DataBind.SetBound(true, "", "Id");
            // Estado
            oForm.DataSources.UserDataSources.Add("Estado", SAPbouiCOM.BoDataType.dt_SHORT_TEXT);
            txtEstado.DataBind.SetBound(true, "", "Estado");
            // Total
            oForm.DataSources.UserDataSources.Add("Total", SAPbouiCOM.BoDataType.dt_PRICE);
            txtTotal.DataBind.SetBound(true, "", "Total");
            // Total Chk
            oForm.DataSources.UserDataSources.Add("TotalChk", SAPbouiCOM.BoDataType.dt_PRICE);
            txtTotalChk.DataBind.SetBound(true, "", "TotalChk");
            // Total Sy
            oForm.DataSources.UserDataSources.Add("TotalSy", SAPbouiCOM.BoDataType.dt_PRICE);
            txtTotalSy.DataBind.SetBound(true, "", "TotalSy");
            // Total Sy Chk
            oForm.DataSources.UserDataSources.Add("TotalSyChk", SAPbouiCOM.BoDataType.dt_PRICE);
            txtTotalSyChk.DataBind.SetBound(true, "", "TotalSyChk");

            oForm.DataSources.DataTables.Add("dtFact");
            oForm.DataSources.DataTables.Add("dtDocs");

            EstructuraMatrixDocumentos();
            CargarCabecera();
            CargarDocumentos();

            if (oForm.DataSources.UserDataSources.Item("Estado").Value.Equals("Definitivo"))
            {
                txtEntidad.Item.Enabled = false;
                txtMoneda.Item.Enabled = false;
                txtValor.Item.Enabled = false;
                txtFecha.Item.Enabled = false;
                txtNumOper.Item.Enabled = false;
                cbxTipoRes.Item.Enabled = false;
                btnGuardar.Item.Enabled = false;
                btnLinea.Item.Enabled = false;
                btnElim.Item.Enabled = false;
                btnDef.Item.Enabled = false;
            }

            mtxDocs.SelectionMode = SAPbouiCOM.BoMatrixSelect.ms_Single;
            oForm.Visible = true;
        }

        private static void EstructuraMatrixDocumentos()
        {
            string NombreDT = "dtDocs";
            SAPbouiCOM.DataTable datatable = oForm.DataSources.DataTables.Item(NombreDT);
            string _query = @"
                SELECT 'N' AS ""Chk"",
                ""U_OBJTYPE"",
                ""U_TIPODOC"",
                ""U_DOCENTRY"",
                ""U_BASEENTRY"",
                ""U_BASEREF"",
                ""U_DOCNUM"",
                ""U_FOLIONUM"",
                ""U_ISINS"",
                ""U_INDICATOR"",
                ""U_DOCDATE"",
                ""U_DOCDUEDATE"",
                ""U_TAXDATE"",
                ""U_DOCCUR"",
                ""U_DOCTOTAL"",
                ""U_DOCTOTALSY"",
                ""U_LICTRADNUM"",
                ""U_CARDCODE"",
                ""U_CARDNAME""
                FROM ""@SO_FCTRNGL""
                WHERE 1 = 0";
            datatable.ExecuteQuery(_query);

            SAPbouiCOM.Columns oColumns;
            oColumns = mtxDocs.Columns;
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
            oColumn.DataBind.Bind(NombreDT, "U_OBJTYPE");

            oColumn = oColumns.Add("TipoDoc", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Tipo";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 60;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "U_TIPODOC");

            oColumn = oColumns.Add("DocEntry", SAPbouiCOM.BoFormItemTypes.it_LINKED_BUTTON);
            oColumn.TitleObject.Caption = "Link";
            oColumn.TitleObject.Sortable = false;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 15;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "U_DOCENTRY");

            oColumn = oColumns.Add("BaseEntry", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "BaseEntry";
            oColumn.TitleObject.Sortable = false;
            oColumn.Editable = false;
            oColumn.Visible = false;
            oColumn.Width = 15;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "U_BASEENTRY");

            oColumn = oColumns.Add("BaseRef", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Origen";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 60;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "U_BASEREF");

            oColumn = oColumns.Add("DocNum", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Número";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 60;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "U_DOCNUM");

            oColumn = oColumns.Add("FolioNum", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Folio";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 60;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "U_FOLIONUM");

            oColumn = oColumns.Add("isIns", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Reserva";
            oColumn.TitleObject.Sortable = false;
            oColumn.Editable = false;
            oColumn.Visible = false;
            oColumn.Width = 20;
            oColumn.RightJustified = false;
            oColumn.DataBind.Bind(NombreDT, "U_ISINS");

            oColumn = oColumns.Add("Indicator", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Indicador";
            oColumn.TitleObject.Sortable = false;
            oColumn.Editable = false;
            oColumn.Visible = false;
            oColumn.Width = 20;
            oColumn.RightJustified = false;
            oColumn.DataBind.Bind(NombreDT, "U_INDICATOR");

            oColumn = oColumns.Add("DocDate", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Fecha contable";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 80;
            oColumn.RightJustified = false;
            oColumn.DataBind.Bind(NombreDT, "U_DOCDATE");

            oColumn = oColumns.Add("DocDueDate", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Fecha vencimiento";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 80;
            oColumn.RightJustified = false;
            oColumn.DataBind.Bind(NombreDT, "U_DOCDUEDATE");

            oColumn = oColumns.Add("TaxDate", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Fecha documento";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 80;
            oColumn.RightJustified = false;
            oColumn.DataBind.Bind(NombreDT, "U_TAXDATE");

            oColumn = oColumns.Add("DocCur", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Moneda";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 80;
            oColumn.RightJustified = false;
            oColumn.DataBind.Bind(NombreDT, "U_DOCCUR");

            oColumn = oColumns.Add("DocTotal", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Monto";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 80;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "U_DOCTOTAL");

            oColumn = oColumns.Add("DocTotalSy", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Monto (Sy)";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 80;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "U_DOCTOTALSY");

            oColumn = oColumns.Add("LicTradNum", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Rut SN";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 60;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "U_LICTRADNUM");

            oColumn = oColumns.Add("CardCode", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Código SN";
            oColumn.TitleObject.Sortable = false;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 80;
            oColumn.RightJustified = false;
            oColumn.DataBind.Bind(NombreDT, "U_CARDCODE");

            oColumn = oColumns.Add("CardName", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Nombre SN";
            oColumn.TitleObject.Sortable = false;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 120;
            oColumn.RightJustified = false;
            oColumn.DataBind.Bind(NombreDT, "U_CARDNAME");

            mtxDocs.Clear();
            mtxDocs.LoadFromDataSource();
            Comun.FuncionesComunes.LiberarObjetoGenerico(datatable);
        }

        private static void CargarCabecera()
        {
            string NombreDT = "dtFact";
            SAPbouiCOM.DataTable datatable = oForm.DataSources.DataTables.Item(NombreDT);
            string _query = @"
                SELECT
                ""U_ID"",
                ""U_ENTIDAD"",
                ""U_MONEDA"",
                ""U_VALOR"",
                ""U_FECHA"",
                ""U_NUMOPER"",
                ""U_TIPORES"",
                ""U_ESTADO""
                FROM ""@SO_FCTRNG""
                WHERE ""U_ID"" = " + globalId;
            datatable.ExecuteQuery(_query);

            if (!datatable.IsEmpty)
            {
                NumberFormatInfo provider = new NumberFormatInfo();
                provider.NumberDecimalSeparator = SepDecimal;
                provider.NumberGroupSeparator = SepMiles;

                oForm.DataSources.UserDataSources.Item("Entidad").Value = datatable.GetValue("U_ENTIDAD", 0).ToString();
                oForm.DataSources.UserDataSources.Item("Moneda").Value = datatable.GetValue("U_MONEDA", 0).ToString();
                string svalor = datatable.GetValue("U_VALOR", 0).ToString();
                double dvalor = Convert.ToDouble(svalor, provider);
                //oForm.DataSources.UserDataSources.Item("Valor").Value = dvalor.ToString("G", new System.Globalization.CultureInfo("en-US"));
                oForm.DataSources.UserDataSources.Item("Valor").Value = datatable.GetValue("U_VALOR", 0).ToString();
                var vrfdate = datatable.GetValue("U_FECHA", 0);
                if (vrfdate != null)
                {
                    DateTime fecha = (DateTime)datatable.GetValue("U_FECHA", 0);
                    string _fecha = fecha.ToString("yyyyMMdd");
                    oForm.DataSources.UserDataSources.Item("Fecha").Value = _fecha;
                }
                oForm.DataSources.UserDataSources.Item("NumOper").Value = datatable.GetValue("U_NUMOPER", 0).ToString();
                oForm.DataSources.UserDataSources.Item("TipoRes").Value = datatable.GetValue("U_TIPORES", 0).ToString();
                oForm.DataSources.UserDataSources.Item("Id").Value = datatable.GetValue("U_ID", 0).ToString();
                string estado = datatable.GetValue("U_ESTADO", 0).ToString();
                switch (estado)
                {
                    case "P":
                        oForm.DataSources.UserDataSources.Item("Estado").Value = "Preliminar";
                        break;
                    case "D":
                        oForm.DataSources.UserDataSources.Item("Estado").Value = "Definitivo";
                        break;
                    default:
                        oForm.DataSources.UserDataSources.Item("Estado").Value = "Preliminar";
                        break;
                }
            }
            Comun.FuncionesComunes.LiberarObjetoGenerico(datatable);
        }

        private static void CargarDocumentos()
        {
            string NombreDT = "dtDocs";
            SAPbouiCOM.DataTable datatable = oForm.DataSources.DataTables.Item(NombreDT);

            string _query = @"
                SELECT 'N' AS ""Chk"",
                ""U_OBJTYPE"",
                ""U_TIPODOC"",
                ""U_DOCENTRY"",
                ""U_BASEENTRY"",
                ""U_BASEREF"",
                ""U_DOCNUM"",
                ""U_FOLIONUM"",
                ""U_ISINS"",
                ""U_INDICATOR"",
                ""U_DOCDATE"",
                ""U_DOCDUEDATE"",
                ""U_TAXDATE"",
                ""U_DOCCUR"",
                ""U_DOCTOTAL"",
                ""U_DOCTOTALSY"",
                ""U_LICTRADNUM"",
                ""U_CARDCODE"",
                ""U_CARDNAME""
                FROM ""@SO_FCTRNGL""
                WHERE ""DocEntry"" = " + globalId;

            datatable.ExecuteQuery(_query);
            oForm.Freeze(true);
            mtxDocs.Clear();
            mtxDocs.LoadFromDataSource();
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
            string NombreDT = "dtDocs";
            SAPbouiCOM.DataTable datatable = oForm.DataSources.DataTables.Item(NombreDT);
            mtxDocs.FlushToDataSource();
            if (!datatable.IsEmpty)
            {
                for (int i = 0; i < datatable.Rows.Count; i++)
                {
                    string TipoDoc = datatable.GetValue("U_TIPODOC", i).ToString();
                    if (TipoDoc.Equals("FC"))
                    {
                        Total += (double)datatable.GetValue("U_DOCTOTAL", i);
                        TotalSy += (double)datatable.GetValue("U_DOCTOTALSY", i);
                    }
                    else
                    {
                        Total -= (double)datatable.GetValue("U_DOCTOTAL", i);
                        TotalSy -= (double)datatable.GetValue("U_DOCTOTALSY", i);
                    }
                    if (datatable.GetValue("Chk", i).ToString().Equals("Y"))
                    {
                        if (TipoDoc.Equals("FC"))
                        {
                            TotalChk += (double)datatable.GetValue("U_DOCTOTAL", i);
                            TotalSyChk += (double)datatable.GetValue("U_DOCTOTALSY", i);
                        }
                        else
                        {
                            TotalChk -= (double)datatable.GetValue("U_DOCTOTAL", i);
                            TotalSyChk -= (double)datatable.GetValue("U_DOCTOTALSY", i);
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
            oForm.DataSources.UserDataSources.Item("Total").Value = Total.ToString();
            oForm.DataSources.UserDataSources.Item("TotalChk").Value = TotalChk.ToString();
            oForm.DataSources.UserDataSources.Item("TotalSy").Value = TotalSy.ToString();
            oForm.DataSources.UserDataSources.Item("TotalSyChk").Value = TotalSyChk.ToString();
        }

        private static void EliminarRegistroMatrix(SAPbouiCOM.Matrix oMatrix, SAPbouiCOM.DataTable dtDoc, string columna)
        {
            oMatrix.FlushToDataSource();
            for (int i = 0; i < dtDoc.Rows.Count; i++)
            {
                if (dtDoc.GetValue(columna, i).ToString().Equals("Y"))
                {
                    dtDoc.Rows.Remove(i);
                    i = i - 1;
                }
            }
            oForm.Freeze(true);
            oMatrix.LoadFromDataSource();
            SumarMontos();
            oForm.Freeze(false);
        }

        private static void ActualizarFactoring(SAPbouiCOM.DataTable dtDoc, string columna)
        {
            NumberFormatInfo provider = new NumberFormatInfo();
            provider.NumberDecimalSeparator = SepDecimal;
            provider.NumberGroupSeparator = SepMiles;

            mtxDocs.FlushToDataSource();
            if (!dtDoc.IsEmpty)
            {
                Clases.Factoring factoring = new Clases.Factoring();
                int DocEntry = int.Parse(oForm.DataSources.UserDataSources.Item("Id").Value);
                factoring.U_ID = int.Parse(oForm.DataSources.UserDataSources.Item("Id").Value);
                factoring.U_ENTIDAD = oForm.DataSources.UserDataSources.Item("Entidad").Value;
                factoring.U_MONEDA = oForm.DataSources.UserDataSources.Item("Moneda").Value;
                string svalor = oForm.DataSources.UserDataSources.Item("Valor").Value;
                double dvalor = Convert.ToDouble(svalor, provider);
                factoring.U_VALOR = dvalor;
                factoring.U_FECHA = string.IsNullOrEmpty(txtFecha.Value) ? DateTime.Now.Date : new DateTime(int.Parse(txtFecha.Value.Substring(0, 4)), int.Parse(txtFecha.Value.Substring(4, 2)), int.Parse(txtFecha.Value.Substring(6, 2)));
                factoring.U_NUMOPER = oForm.DataSources.UserDataSources.Item("NumOper").Value;
                factoring.U_TIPORES = oForm.DataSources.UserDataSources.Item("TipoRes").Value;
                factoring.U_ESTADO = "P";
                for (int i = 0; i < dtDoc.Rows.Count; i++)
                {
                    Clases.FactoringLines documento = new Clases.FactoringLines();
                    documento.U_OBJTYPE = dtDoc.GetValue("U_OBJTYPE", i).ToString();
                    documento.U_DOCENTRY = (int)dtDoc.GetValue("U_DOCENTRY", i);
                    documento.U_BASEENTRY = (int)dtDoc.GetValue("U_BASEENTRY", i);
                    documento.U_BASEREF = dtDoc.GetValue("U_BASEREF", i).ToString();
                    documento.U_TIPODOC = dtDoc.GetValue("U_TIPODOC", i).ToString();
                    documento.U_DOCNUM = (int)dtDoc.GetValue("U_DOCNUM", i);
                    documento.U_FOLIONUM = (int)dtDoc.GetValue("U_FOLIONUM", i);
                    documento.U_ISINS = dtDoc.GetValue("U_ISINS", i).ToString();
                    documento.U_INDICATOR = dtDoc.GetValue("U_INDICATOR", i).ToString();
                    var docdate = dtDoc.GetValue("U_DOCDATE", i);
                    documento.U_DOCDATE = (DateTime)docdate;
                    var docduedate = dtDoc.GetValue("U_DOCDUEDATE", i);
                    documento.U_DOCDUEDATE = (DateTime)docduedate;
                    var taxdate = dtDoc.GetValue("U_TAXDATE", i);
                    documento.U_TAXDATE = (DateTime)taxdate;
                    documento.U_DOCCUR = dtDoc.GetValue("U_DOCCUR", i).ToString();
                    documento.U_DOCTOTAL = (double)dtDoc.GetValue("U_DOCTOTAL", i);
                    documento.U_DOCTOTALSY = (double)dtDoc.GetValue("U_DOCTOTALSY", i);
                    documento.U_LICTRADNUM = dtDoc.GetValue("U_LICTRADNUM", i).ToString();
                    documento.U_CARDCODE = dtDoc.GetValue("U_CARDCODE", i).ToString();
                    documento.U_CARDNAME = dtDoc.GetValue("U_CARDNAME", i).ToString();
                    factoring.Documentos.Add(documento);
                }
                if (factoring.Documentos != null && factoring.Documentos.Count > 0)
                {
                    var result = SBO.ModeloSBO.UpdateFactoring(DocEntry, factoring);
                    if (result.Success)
                    {
                        Application.SBO_Application.StatusBar.SetText(string.Format("Factoring Preliminar actualizado correctamente. Id: {0}", DocEntry), SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success);
                    }
                    else
                    {
                        Application.SBO_Application.StatusBar.SetText(string.Format("Factoring Preliminar no guardado. Mensaje: {0}", result.Mensaje), SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
                    }
                }
                oForm.Freeze(true);
                mtxDocs.LoadFromDataSource();
                oForm.Freeze(false);
            }
        }

        private static void EliminarDocumentoFactoring()
        {
            int DocEntry = int.Parse(oForm.DataSources.UserDataSources.Item("Id").Value);
            var result = SBO.ModeloSBO.DeleteFactoring(DocEntry);
            if (result.Success)
            {
                Application.SBO_Application.StatusBar.SetText(string.Format("Factoring Preliminar eliminado correctamente."), SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success);
                oForm.Close();
            }
            else
            {
                Application.SBO_Application.StatusBar.SetText(string.Format("Factoring Preliminar no eliminado. Mensaje: {0}", result.Mensaje), SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
            }
        }

        private static void CrearFactoring()
        {
            int DocEntry = int.Parse(oForm.DataSources.UserDataSources.Item("Id").Value);
            int resp = 0;
            var oFactoring = SBO.ModeloSBO.GetFactoring(DocEntry);
            if (oFactoring.Success)
            {
                var factoring = oFactoring.Factoring;

                switch (oFactoring.Factoring.U_TIPORES)
                {
                    case "CR":
                        Application.SBO_Application.StatusBar.SetText("Actualizando documentos relacionados. Espere...", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);
                        ActualizarDocumentos(factoring);
                        Application.SBO_Application.StatusBar.SetText("Contabilizando factoring con responsabilidad. Espere...", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);
                        FactoringConResponsabilidad(factoring);
                        resp = Application.SBO_Application.MessageBox("¿Desea enviar correos de notificación?", 2, "Si", "No");
                        if (resp.Equals(1))
                        {
                            Application.SBO_Application.StatusBar.SetText("Enviando correos de notificación. Espere...", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);
                            EnviarCorreos(factoring);
                        }
                        break;
                    case "SR":
                        Application.SBO_Application.StatusBar.SetText("Actualizando documentos relacionados. Espere...", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);
                        ActualizarDocumentos(factoring);
                        Application.SBO_Application.StatusBar.SetText("Contabilizando factoring sin responsabilidad. Espere...", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);
                        FactoringSinResponsabilidad(factoring);
                        resp = Application.SBO_Application.MessageBox("¿Desea enviar correos de notificación?", 2, "Si", "No");
                        if (resp.Equals(1))
                        {
                            Application.SBO_Application.StatusBar.SetText("Enviando correos de notificación. Espere...", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);
                            EnviarCorreos(factoring);
                        }
                        break;
                    case "FN":
                        Application.SBO_Application.StatusBar.SetText("Actualizando documentos relacionados. Espere...", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);
                        ActualizarDocumentos(factoring);
                        break;
                    default:
                        break;
                }
                //var result = SBO.ModeloSBO.UpdateFactoringStatus(DocEntry, "P");
                var result = SBO.ModeloSBO.UpdateFactoringStatus(DocEntry, "D");
                if (result.Success)
                {
                    Application.SBO_Application.StatusBar.SetText("Factoring actualizado correctamente como definitivo.", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success);
                }
                else
                {
                    Application.SBO_Application.StatusBar.SetText("No fue posible actualizar el estado del factoring a definitivo.", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
                }
            }
            else
            {
                Application.SBO_Application.StatusBar.SetText("No fue posible recuperar la información del factoring seleccionado.", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
            }
        }

        private static void ActualizarDocumentos(Clases.Factoring factoring)
        {
            var resp = SBO.ModeloSBO.UpdateDocuments(factoring);
        }

        private static void FactoringConResponsabilidad(Clases.Factoring factoring)
        {
            bool borrador = true;
            string memo = string.Empty;
            int errCode = 0;
            string errMsg = string.Empty;
            int ret = 0;
            string monedalocal = SBO.ConsultasSBO.ObtenerMonedaLocal();
            var listDocumentos = factoring.Documentos;
            var listClientes = listDocumentos.GroupBy(x => x.U_CARDCODE).ToList();

            var lineas = listDocumentos;
            if (borrador)
            {
                //preliminar
                SAPbobsCOM.JournalVouchers oVoucher = (SAPbobsCOM.JournalVouchers)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oJournalVouchers);
                //fecha documento
                oVoucher.JournalEntries.ReferenceDate = DateTime.Now;
                oVoucher.JournalEntries.TaxDate = DateTime.Now;
                oVoucher.JournalEntries.DueDate = DateTime.Now;
                oVoucher.JournalEntries.Reference = factoring.U_ID.ToString();
                string aliasbanco = SBO.ConsultasSBO.ObtenerAliasBanco(factoring.U_ENTIDAD);
                memo = string.Empty;
                memo = string.Format("Otorg. Factoring con responsabilidad {0}", aliasbanco);
                memo = memo.PadRight(50).Substring(0, 50).Trim();
                oVoucher.JournalEntries.Memo = memo;

                int lin = 0;
                double total = 0;
                double totalsy = 0;
                lineas.ForEach(documento =>
                {
                    total += documento.U_DOCTOTAL;
                    totalsy += documento.U_DOCTOTALSY;
                });
                oVoucher.JournalEntries.SetCurrentLine(lin);
                string ccbancofactoring = SBO.ConsultasSBO.ObtenerCuentaContableBanco(factoring.U_ENTIDAD);
                oVoucher.JournalEntries.Lines.AccountCode = ccbancofactoring;
                if (factoring.U_MONEDA.Equals(monedalocal))
                {
                    oVoucher.JournalEntries.Lines.Credit = total;
                }
                else
                {
                    oVoucher.JournalEntries.Lines.FCCurrency = factoring.U_MONEDA;
                    oVoucher.JournalEntries.Lines.FCCredit = totalsy;
                }
                lin++;
                oVoucher.JournalEntries.Lines.Add();
                //oVoucher.JournalEntries.SetCurrentLine(lin);
                string ccpagosmasivos = SBO.ConsultasSBO.ObtenerParametro("CTA_PAGOS_MASIVOS");
                oVoucher.JournalEntries.Lines.AccountCode = ccpagosmasivos;
                if (factoring.U_MONEDA.Equals(monedalocal))
                {
                    oVoucher.JournalEntries.Lines.Debit = total;
                }
                else
                {
                    oVoucher.JournalEntries.Lines.FCCurrency = factoring.U_MONEDA;
                    oVoucher.JournalEntries.Lines.FCDebit = totalsy;
                }

                oVoucher.JournalEntries.UserFields.Fields.Item("U_TIPOOPER").Value = "FA";
                oVoucher.JournalEntries.UserFields.Fields.Item("U_INSTITU").Value = SBO.ConsultasSBO.ObtenerAliasBanco(factoring.U_ENTIDAD);
                oVoucher.JournalEntries.UserFields.Fields.Item("U_NUMOPER").Value = factoring.U_NUMOPER.ToString();
                oVoucher.JournalEntries.UserFields.Fields.Item("U_RESFAC").Value = factoring.U_TIPORES.ToString();
                oVoucher.JournalEntries.UserFields.Fields.Item("U_FACTORINGID").Value = factoring.U_ID;

                errCode = 0;
                errMsg = "";
                ret = oVoucher.Add();
            }
            else
            {
                //final
                //SAPbobsCOM.JournalEntries oVoucher = (SAPbobsCOM.JournalEntries)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oJournalEntries);
            }

            if (ret != 0)
            {
                SBO.ConexionSBO.oCompany.GetLastError(out errCode, out errMsg);
                Application.SBO_Application.MessageBox(string.Format("{0}:{1}", errCode, errMsg));
            }
            else
            {
                var voucher = SBO.ConexionSBO.oCompany.GetNewObjectKey();

                //Application.SBO_Application.MessageBox(msg);
            }
        }

        private static void FactoringConResponsabilidadPorCliente(Clases.Factoring factoring)
        {
            bool borrador = true;
            string memo = string.Empty;
            int errCode = 0;
            string errMsg = string.Empty;
            int ret = 0;
            string monedalocal = SBO.ConsultasSBO.ObtenerMonedaLocal();
            var listDocumentos = factoring.Documentos;
            var listClientes = listDocumentos.GroupBy(x => x.U_CARDCODE).ToList();

            listClientes.ForEach(cliente =>
            {
                var lineas = listDocumentos.Where(z => z.U_CARDCODE == cliente.Key).ToList();
                if (borrador)
                {
                    //preliminar
                    SAPbobsCOM.JournalVouchers oVoucher = (SAPbobsCOM.JournalVouchers)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oJournalVouchers);
                    //fecha documento
                    oVoucher.JournalEntries.ReferenceDate = DateTime.Now;
                    oVoucher.JournalEntries.TaxDate = DateTime.Now;
                    oVoucher.JournalEntries.DueDate = DateTime.Now;
                    oVoucher.JournalEntries.Reference = factoring.U_ID.ToString();
                    string aliasbanco = SBO.ConsultasSBO.ObtenerAliasBanco(factoring.U_ENTIDAD);
                    memo = string.Empty;
                    memo = string.Format("Otorg. Factoring con responsabilidad {0}", aliasbanco);
                    memo = memo.PadRight(50).Substring(0, 50).Trim();
                    oVoucher.JournalEntries.Memo = memo;

                    int lin = 0;
                    double total = 0;
                    double totalsy = 0;
                    lineas.ForEach(documento =>
                    {
                        total += documento.U_DOCTOTAL;
                        totalsy += documento.U_DOCTOTALSY;
                    });
                    oVoucher.JournalEntries.SetCurrentLine(lin);
                    string ccbancofactoring = SBO.ConsultasSBO.ObtenerCuentaContableBanco(factoring.U_ENTIDAD);
                    oVoucher.JournalEntries.Lines.AccountCode = ccbancofactoring;
                    if (factoring.U_MONEDA.Equals(monedalocal))
                    {
                        oVoucher.JournalEntries.Lines.Credit = total;
                    }
                    else
                    {
                        oVoucher.JournalEntries.Lines.FCCurrency = factoring.U_MONEDA;
                        oVoucher.JournalEntries.Lines.FCCredit = totalsy;
                    }
                    lin++;
                    oVoucher.JournalEntries.Lines.Add();
                    //oVoucher.JournalEntries.SetCurrentLine(lin);
                    string ccpagosmasivos = SBO.ConsultasSBO.ObtenerParametro("CTA_PAGOS_MASIVOS");
                    oVoucher.JournalEntries.Lines.AccountCode = ccpagosmasivos;
                    if (factoring.U_MONEDA.Equals(monedalocal))
                    {
                        oVoucher.JournalEntries.Lines.Debit = total;
                    }
                    else
                    {
                        oVoucher.JournalEntries.Lines.FCCurrency = factoring.U_MONEDA;
                        oVoucher.JournalEntries.Lines.FCDebit = totalsy;
                    }

                    oVoucher.JournalEntries.UserFields.Fields.Item("U_TIPOOPER").Value = "FA";
                    oVoucher.JournalEntries.UserFields.Fields.Item("U_INSTITU").Value = SBO.ConsultasSBO.ObtenerAliasBanco(factoring.U_ENTIDAD);
                    oVoucher.JournalEntries.UserFields.Fields.Item("U_NUMOPER").Value = factoring.U_NUMOPER.ToString();
                    oVoucher.JournalEntries.UserFields.Fields.Item("U_RESFAC").Value = factoring.U_TIPORES.ToString();
                    oVoucher.JournalEntries.UserFields.Fields.Item("U_FACTORINGID").Value = factoring.U_ID;

                    errCode = 0;
                    errMsg = "";
                    ret = oVoucher.Add();
                }
                else
                {
                    //final
                    //SAPbobsCOM.JournalEntries oVoucher = (SAPbobsCOM.JournalEntries)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oJournalEntries);
                }

                if (ret != 0)
                {
                    SBO.ConexionSBO.oCompany.GetLastError(out errCode, out errMsg);
                    Application.SBO_Application.MessageBox(string.Format("{0}:{1}", errCode, errMsg));
                }
                else
                {
                    var voucher = SBO.ConexionSBO.oCompany.GetNewObjectKey();

                    //Application.SBO_Application.MessageBox(msg);
                }
            });
        }

        private static void FactoringSinResponsabilidad(Clases.Factoring factoring)
        {
            bool borrador = true;
            SAPbobsCOM.Payments oPayment;
            string monedalocal = SBO.ConsultasSBO.ObtenerMonedaLocal();
            var listDocumentos = factoring.Documentos;
            var listClientes = listDocumentos.GroupBy(x => x.U_CARDCODE).ToList();

            listClientes.ForEach(cliente =>
            {
                var lineas = listDocumentos.Where(z => z.U_CARDCODE == cliente.Key).ToList();
                if (borrador)
                {
                    //preliminar
                    oPayment = (SAPbobsCOM.Payments)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPaymentsDrafts);
                }
                else
                {
                    //final
                    oPayment = (SAPbobsCOM.Payments)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oIncomingPayments);
                }
                oPayment.DocObjectCode = SAPbobsCOM.BoPaymentsObjectType.bopot_IncomingPayments;
                oPayment.DocType = SAPbobsCOM.BoRcptTypes.rCustomer;
                string memo = string.Empty;
                memo = string.Format("FACTOR.{0} S/R", SBO.ConsultasSBO.ObtenerAliasBanco(factoring.U_ENTIDAD));
                oPayment.Remarks = memo.PadRight(250).Substring(0, 250).Trim();
                oPayment.JournalRemarks = memo.PadRight(50).Substring(0, 50).Trim();
                oPayment.CardCode = cliente.Key;
                oPayment.DocDate = DateTime.Now;
                oPayment.DueDate = DateTime.Now;
                oPayment.TaxDate = DateTime.Now;
                oPayment.CounterReference = factoring.U_ID.ToString();
                //oPayment.CashAccount = "20102010106"; //CTA_PAGOS_MASIVOS
                string ccpagosmasivos = SBO.ConsultasSBO.ObtenerParametro("CTA_PAGOS_MASIVOS");
                //oPayment.CashAccount = ccpagosmasivos;
                oPayment.TransferAccount = ccpagosmasivos;
                oPayment.TransferDate = DateTime.Now;
                string refer = string.Empty;
                refer = string.Format("Dp {0} Factoring sin Responsabilidad", SBO.ConsultasSBO.ObtenerAliasBanco(factoring.U_ENTIDAD));
                oPayment.TransferReference = refer.PadRight(27).Substring(0, 27).Trim();

                int lin = 0;
                double total = 0;
                double totalsy = 0;
                lineas.ForEach(documento =>
                {

                    oPayment.Invoices.Add();
                    oPayment.Invoices.SetCurrentLine(lin);
                    oPayment.Invoices.DocEntry = documento.U_DOCENTRY;
                    total += documento.U_DOCTOTAL;
                    totalsy += documento.U_DOCTOTALSY;
                    lin++;
                });
                if (factoring.U_MONEDA.Equals(monedalocal))
                {
                    oPayment.LocalCurrency = SAPbobsCOM.BoYesNoEnum.tYES;
                    oPayment.DocCurrency = factoring.U_MONEDA;
                    //oPayment.CashSum = total;
                    oPayment.TransferSum = total;
                }
                else
                {
                    oPayment.LocalCurrency = SAPbobsCOM.BoYesNoEnum.tNO;
                    oPayment.DocCurrency = factoring.U_MONEDA;
                    //oPayment.CashSum = totalsy;
                    oPayment.TransferSum = totalsy;
                }

                oPayment.UserFields.Fields.Item("U_FACTORINGID").Value = factoring.U_ID;

                int errCode = 0;
                string errMsg = "";
                int ret = oPayment.Add();
                if (ret != 0)
                {
                    SBO.ConexionSBO.oCompany.GetLastError(out errCode, out errMsg);
                    Application.SBO_Application.MessageBox(string.Format("{0}:{1}", errCode, errMsg));
                }
                else
                {
                    var pago = SBO.ConexionSBO.oCompany.GetNewObjectKey();

                    //Application.SBO_Application.MessageBox(msg);
                }
            });
        }

        private static void EnviarCorreos(Clases.Factoring factoring)
        {
            var listDocumentos = factoring.Documentos;
            var listClientes = listDocumentos.GroupBy(x => x.U_CARDCODE).ToList();

            listClientes.ForEach(cliente =>
            {
                var lineas = listDocumentos.Where(z => z.U_CARDCODE == cliente.Key).ToList();
                // Cuerpo del Correo
                string bodyhtml = CreateBodyHtml(cliente.Key, factoring, lineas);
                // Envío de Correo
                SendMail(cliente.Key, bodyhtml);
            });
        }

        private static string CreateBodyHtml_old(string clienteKey, Clases.Factoring factoring, List<Clases.FactoringLines> lineas)
        {
            string NombreSN = SBO.ConsultasSBO.ObtenerNombreSN(clienteKey);
            string bodyhtml = @"
                <p>Se&ntilde;ores @CLIENTE@</p>
                <p>Informamos a ustedes que hemos enviado a Factoring de la entidad @ENTIDAD@, bajo el n&uacute;mero de operaci&oacute;n @OPERACION@, los documentos que se detallan a continuaci&oacute;n:</p>
                <table border=""0"" cellpadding=""1"" cellspacing=""1"" style=""width: 800px"" summary=""Documentos"">
                <caption> Documentos </caption>
                <thead>
                <tr>
                    <th scope = ""col"" > Tipo documento </th>
                    <th scope = ""col"" > Folio </th>
                    <th scope = ""col"" > Fecha emisión </th>
                    <th scope = ""col"" > Fecha vencimiento</th>
                    <th scope = ""col"" > Moneda </th>
                    <th scope = ""col"" > Monto </th>
                </tr>
                </thead>
                <tbody>";
            bodyhtml = bodyhtml.Replace("@CLIENTE@", NombreSN.ToString());
            bodyhtml = bodyhtml.Replace("@ENTIDAD@", SBO.ConsultasSBO.ObtenerNombreBanco(factoring.U_ENTIDAD.ToString()));
            bodyhtml = bodyhtml.Replace("@OPERACION@", factoring.U_NUMOPER.ToString());

            lineas.ForEach(documento =>
            {
                string tipo = documento.U_TIPODOC;
                string folio = documento.U_FOLIONUM.ToString();
                string fecha = documento.U_DOCDATE.ToString("dd-MM-yyyy");
                string fechavcto = documento.U_DOCDUEDATE.ToString("dd-MM-yyyy");
                string moneda = documento.U_DOCCUR;
                string monto = documento.U_DOCTOTALSY.ToString("N", new System.Globalization.CultureInfo("es-CL"));
                string lineabody = @"
                <tr>
                <td style = ""text-align:center"" > @TIPO@ </td>
                <td style = ""text-align:center"" > @FOLIO@ </td>
                <td style = ""text-align:center"" > @FECHA@ </td>
                <td style = ""text-align:center"" > @FECHAVCTO@ </td>
                <td style = ""text-align:center"" > @MONEDA@ </td>
                <td style = ""text-align:center"" > @MONTO@ </td>
                </tr>";
                lineabody = lineabody.Replace("@TIPO@", tipo);
                lineabody = lineabody.Replace("@FOLIO@", folio);
                lineabody = lineabody.Replace("@FECHA@", fecha);
                lineabody = lineabody.Replace("@FECHAVCTO@", fechavcto);
                lineabody = lineabody.Replace("@MONEDA@", moneda);
                lineabody = lineabody.Replace("@MONTO@", monto);
                bodyhtml += @lineabody;
            });
            bodyhtml += @"
            </tbody>
            </table>
            <p>Sin otro particular,</p>
            <p><img alt="""" src=""https://empresasagrotop.cl/firma_cobranza.jpg"" /></p>
            ";
            return bodyhtml;
        }

        private static void SendMail_old(string to, string bodyhtml)
        {
            string from = SBO.ConsultasSBO.ObtenerParametro("CORREO_ENVIO");
            string pass = SBO.ConsultasSBO.ObtenerParametro("CORREO_PASS");
            string alias = SBO.ConsultasSBO.ObtenerParametro("CORREO_ALIAS");
            string asunto = SBO.ConsultasSBO.ObtenerParametro("CORREO_ASUNTO");
            System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage();
            //msg.To.Add(to);
            msg.To.Add("pruebascobranzas@gmail.com");
            msg.From = new System.Net.Mail.MailAddress(from, alias, System.Text.Encoding.UTF8);
            msg.Subject = asunto;
            msg.SubjectEncoding = System.Text.Encoding.UTF8;
            msg.Body = @bodyhtml;
            msg.BodyEncoding = System.Text.Encoding.UTF8;
            msg.IsBodyHtml = true;

            //Aquí es donde se hace lo especial
            System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
            client.Credentials = new System.Net.NetworkCredential(from, pass);
            client.Port = 587;
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true; //Esto es para que vaya a través de SSL que es obligatorio con GMail

            try
            {
                client.Send(msg);
            }
            catch (System.Net.Mail.SmtpException ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
        }

        private static string CreateBodyHtml(string clienteKey, Clases.Factoring factoring, List<Clases.FactoringLines> lineas)
        {
            string monedalocal = SBO.ConsultasSBO.ObtenerMonedaLocal();
            string monedasistema = SBO.ConsultasSBO.ObtenerMonedaSistema();
            string bodyhtml = @"
                <p>Estimado cliente, informamos a usted que las facturas abajo detalladas, se encuentran cedidas al factoring @ENTIDAD@ en moneda @MONFACT@:</p>
                <table border=""0"" cellpadding=""1"" cellspacing=""1"" style=""width: 800px"" summary=""Documentos"">
                <thead>
                <tr>
                    <th scope = ""col"" > N° Factura </th>
                    <th scope = ""col"" > Emisión </th>
                    <th scope = ""col"" > Vencimiento</th>
                    <th scope = ""col"" > Valor @MLOCAL@ </th>
                    <th scope = ""col"" > Valor @MSY@ </th>
                </tr>
                </thead>
                <tbody>";
            bodyhtml = bodyhtml.Replace("@ENTIDAD@", SBO.ConsultasSBO.ObtenerNombreBanco(factoring.U_ENTIDAD.ToString()));
            bodyhtml = bodyhtml.Replace("@MONFACT@", factoring.U_MONEDA.ToString());
            bodyhtml = bodyhtml.Replace("@MLOCAL@", monedalocal);
            bodyhtml = bodyhtml.Replace("@MSY@", monedasistema);

            lineas.ForEach(documento =>
            {
                string folio = documento.U_FOLIONUM.ToString();
                string fecha = documento.U_DOCDATE.ToString("dd-MM-yyyy");
                string fechavcto = documento.U_DOCDUEDATE.ToString("dd-MM-yyyy");
                string moneda = documento.U_DOCCUR;
                string monto = documento.U_DOCTOTAL.ToString("N", new System.Globalization.CultureInfo("es-CL"));
                string montosy = documento.U_DOCTOTALSY.ToString("N", new System.Globalization.CultureInfo("es-CL"));
                string lineabody = @"
                <tr>
                <td style = ""text-align:center"" > @FOLIO@ </td>
                <td style = ""text-align:center"" > @FECHA@ </td>
                <td style = ""text-align:center"" > @FECHAVCTO@ </td>
                <td style = ""text-align:center"" > @MONTO@ </td>
                <td style = ""text-align:center"" > @MONTOSY@ </td>
                </tr>";
                lineabody = lineabody.Replace("@FOLIO@", folio);
                lineabody = lineabody.Replace("@FECHA@", fecha);
                lineabody = lineabody.Replace("@FECHAVCTO@", fechavcto);
                lineabody = lineabody.Replace("@MONTO@", monto);
                lineabody = lineabody.Replace("@MONTOSY@", montosy);
                bodyhtml += @lineabody;
            });
            bodyhtml += @"
            </tbody>
            </table>
            <p>Ante consultas contactar con el departamento de crédito y cobranzas de empresas Agrotop.</p>
            <p>Saluda atentamente;</p>
            <p><img alt="""" src=""https://empresasagrotop.cl/firma_cobranza.jpg"" /></p>
            ";
            return bodyhtml;
        }

        private static void SendMail(string clienteKey, string bodyhtml)
        {
            string host = SBO.ConsultasSBO.ObtenerParametro("CORREO_SERVIDOR");
            string port = SBO.ConsultasSBO.ObtenerParametro("CORREO_SERVIDOR_PUERTO");
            string ssl = SBO.ConsultasSBO.ObtenerParametro("CORREO_SERVIDOR_SSL");
            string from = SBO.ConsultasSBO.ObtenerParametro("CORREO_ENVIO");
            string pass = SBO.ConsultasSBO.ObtenerParametro("CORREO_PASS");
            string alias = SBO.ConsultasSBO.ObtenerParametro("CORREO_ALIAS");
            string asunto = SBO.ConsultasSBO.ObtenerParametro("CORREO_ASUNTO");
            string to = SBO.ConsultasSBO.ObtenerMailSN(clienteKey);
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
