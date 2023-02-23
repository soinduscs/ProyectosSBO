using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM.Framework;
using ProvDTE = Soindus.Interfaces.ProveedoresDTE;
using ClasesDTE = Soindus.Clases.DTE;
using Comun = Soindus.Clases.Comun;

namespace Soindus.AddOnMonitorEmision.Formularios
{
    [FormAttribute("Soindus.AddOnMonitorEmision.Formularios.frmXMonitor", "Formularios/frmXMonitor.b1f")]
    class frmXMonitor : UserFormBase
    {
        // Objetos de formulario
        #region Objetos de formulario
        private static SAPbouiCOM.Form oForm;
        private static SAPbouiCOM.Matrix oMatrixDE;
        private static SAPbouiCOM.Folder Folder1;
        private static SAPbouiCOM.Folder Folder2;
        private static SAPbouiCOM.EditText txtCli;
        private static SAPbouiCOM.EditText txtFDesde;
        private static SAPbouiCOM.EditText txtFHasta;
        private static SAPbouiCOM.ComboBox cmbTipoD;
        private static SAPbouiCOM.EditText txtRut;
        private static SAPbouiCOM.EditText txtNomCli;
        private static SAPbouiCOM.Button btnFiltrar;
        private static SAPbouiCOM.Button btnMarcar;
        private static SAPbouiCOM.Button btnEstado;

        private static Local.Configuracion ExtConf;
        #endregion

        public frmXMonitor()
        {
            ExtConf = new Local.Configuracion();
            AsignarObjetos();
            CargaFormulario();
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
            try
            {
                if (pVal.BeforeAction.Equals(true))
                {
                    // Link documento base en matrix
                    #region link documento base en matrix
                    if (pVal.EventType.Equals(SAPbouiCOM.BoEventTypes.et_MATRIX_LINK_PRESSED))
                    {
                        // Matrix de documentos no foliados
                        if (pVal.ItemUID.Equals("mtxDocsE"))
                        {
                            if (pVal.ColUID.Equals("DocEntry"))
                            {
                                string sObjectType =
                                    ((SAPbouiCOM.EditText)(oMatrixDE.Columns.Item("ObjType").Cells.Item(pVal.Row).Specific)).String;
                                SAPbouiCOM.Column oColumn = (SAPbouiCOM.Column)oMatrixDE.Columns.Item(pVal.ColUID);
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
                        #region txtProv
                        if (pVal.ItemUID.Equals("txtCli"))
                        {
                            SAPbouiCOM.EditText oCli = (SAPbouiCOM.EditText)oForm.Items.Item("txtCli").Specific;
                            String Cliente = oCli.Value;

                            if (String.IsNullOrEmpty(Cliente))
                            {
                                SAPbouiCOM.EditText oRut = (SAPbouiCOM.EditText)oForm.Items.Item("txtRut").Specific;
                                SAPbouiCOM.EditText oNomCli = (SAPbouiCOM.EditText)oForm.Items.Item("txtNomCli").Specific;
                                oRut.Value = string.Empty;
                                oNomCli.Value = string.Empty;
                                //oForm.DataSources.UserDataSources.Item("Cli").Value = string.Empty;
                                //oForm.DataSources.UserDataSources.Item("Rut").Value = string.Empty;
                                //oForm.DataSources.UserDataSources.Item("NomCli").Value = string.Empty;
                            }
                        }
                        #endregion
                    }

                    if (pVal.EventType.Equals(SAPbouiCOM.BoEventTypes.et_CLICK))
                    {
                        // Imagen PDF
                        #region Imagen PDF
                        if (pVal.ItemUID.Contains("mtx"))
                        {
                            if (pVal.ColUID.Equals("co_pdf"))
                            {
                                if (pVal.Row > 0)
                                {
                                    SAPbouiCOM.DataTable dtDoc = null;
                                    switch (pVal.ItemUID)
                                    {
                                        case "mtxDE":
                                            dtDoc = oForm.DataSources.DataTables.Item("DOCUMENTOSDE");
                                            break;
                                    }
                                    VisualizarPdf(dtDoc, pVal.Row - 1);
                                }
                            }
                        }
                        #endregion

                        // Boton actualizar
                        #region actualizar
                        if (pVal.ItemUID.Equals("btnFiltrar"))
                        {
                            // Documentos emitidos
                            if (Folder1.Selected)
                            {
                                CargarMatrixDocumentosEmitidos();
                                Application.SBO_Application.StatusBar.SetText("Documentos cargados correctamente.", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success);
                            }

                            // Otros
                            if (Folder2.Selected)
                            {
                            }
                        }
                        #endregion

                        // Boton marcar
                        #region marcar
                        if (pVal.ItemUID.Equals("btnMarcar"))
                        {
                            // Documentos emitidos
                            if (Folder1.Selected)
                            {
                                string NombreDT = "dtDocsE";
                                SAPbouiCOM.DataTable datatable = oForm.DataSources.DataTables.Item(NombreDT);
                                MarcarRegistrosMatrix(oMatrixDE, datatable, "Chk");
                            }

                            // Otros
                            if (Folder2.Selected)
                            {
                            }
                        }
                        #endregion

                        // Boton actualizar estados
                        #region actualizar estados
                        if (pVal.ItemUID.Equals("btnEstado"))
                        {
                            // Documentos emitidos
                            if (Folder1.Selected)
                            {
                                string NombreDT = "dtDocsE";
                                SAPbouiCOM.DataTable datatable = oForm.DataSources.DataTables.Item(NombreDT);
                                ActualizarEstados(oMatrixDE, datatable, "Chk");
                            }

                            // Otros
                            if (Folder2.Selected)
                            {
                            }
                        }
                        #endregion

                        // Panel documentos emitidos
                        #region click en panel documentos emitidos
                        if (pVal.ItemUID.Equals("tab01"))
                        {
                        }
                        #endregion

                        // Panel otros
                        #region click en panel otros
                        if (pVal.ItemUID.Equals("tab02"))
                        {
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
                        if (oDataTable != null)
                        {
                            oForm.DataSources.UserDataSources.Item("Cli").Value = string.Empty;
                            oForm.DataSources.UserDataSources.Item("Rut").Value = string.Empty;
                            oForm.DataSources.UserDataSources.Item("NomCli").Value = string.Empty;

                            string CardCode = string.Empty;
                            string LicTradNum = string.Empty;
                            string CardName = string.Empty;
                            CardCode = oDataTable.GetValue(0, 0).ToString();
                            LicTradNum = oDataTable.GetValue(23, 0).ToString();
                            CardName = oDataTable.GetValue("CardName", 0).ToString();

                            oForm.DataSources.UserDataSources.Item("Cli").Value = CardCode;
                            oForm.DataSources.UserDataSources.Item("Rut").Value = LicTradNum;
                            oForm.DataSources.UserDataSources.Item("NomCli").Value = CardName;
                        }
                        #endregion
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void OnCustomInitialize()
        {

        }

        /// <summary>
        /// Asignación de objetos de formulario SB1
        /// </summary>
        private void AsignarObjetos()
        {
            oForm = ((SAPbouiCOM.Form)(Application.SBO_Application.Forms.Item("frmXMonitor")));
            oMatrixDE = ((SAPbouiCOM.Matrix)(GetItem("mtxDocsE").Specific));
            Folder1 = ((SAPbouiCOM.Folder)(GetItem("tab01").Specific));
            Folder2 = ((SAPbouiCOM.Folder)(GetItem("tab02").Specific));
            Folder1.Select();
            txtCli = ((SAPbouiCOM.EditText)(GetItem("txtCli").Specific));
            txtFDesde = ((SAPbouiCOM.EditText)(GetItem("txtFDesde").Specific));
            txtFHasta = ((SAPbouiCOM.EditText)(GetItem("txtFHasta").Specific));
            cmbTipoD = ((SAPbouiCOM.ComboBox)(GetItem("cmbTipoD").Specific));
            txtRut = ((SAPbouiCOM.EditText)(GetItem("txtRut").Specific));
            txtNomCli = ((SAPbouiCOM.EditText)(GetItem("txtNomCli").Specific));
            btnFiltrar = ((SAPbouiCOM.Button)(GetItem("btnFiltrar").Specific));
            btnMarcar = ((SAPbouiCOM.Button)(GetItem("btnMarcar").Specific));
            btnEstado = ((SAPbouiCOM.Button)(GetItem("btnEstado").Specific));
        }

        /// <summary>
        /// Carga formulario
        /// </summary>
        private void CargaFormulario()
        {
            // Propiedades del formulario

            // Choose from list socio de negocio
            SAPbouiCOM.ChooseFromListCollection oCFLs = null;
            SAPbouiCOM.ChooseFromList oCFL = null;
            SAPbouiCOM.ChooseFromListCreationParams oCFLCreationParams = null;
            SAPbouiCOM.Conditions oCons = null;

            // Blindear edittext Socio de negocio
            oForm.DataSources.UserDataSources.Add("Cli", SAPbouiCOM.BoDataType.dt_SHORT_TEXT, 200);
            oForm.DataSources.UserDataSources.Add("Rut", SAPbouiCOM.BoDataType.dt_SHORT_TEXT, 200);
            oForm.DataSources.UserDataSources.Add("NomCli", SAPbouiCOM.BoDataType.dt_SHORT_TEXT, 200);
            txtCli.DataBind.SetBound(true, "", "Cli");
            txtRut.DataBind.SetBound(true, "", "Rut");
            txtNomCli.DataBind.SetBound(true, "", "NomCli");

            oCFLs = oForm.ChooseFromLists;
            oCFLCreationParams = ((SAPbouiCOM.ChooseFromListCreationParams)(Application.SBO_Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_ChooseFromListCreationParams)));
            oCFLCreationParams.MultiSelection = false;
            oCFLCreationParams.ObjectType = "2";
            oCFLCreationParams.UniqueID = "cflCli";

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
            txtCli.ChooseFromListUID = "cflCli";
            txtCli.ChooseFromListAlias = "CardCode";

            // Fecha desde
            oForm.DataSources.UserDataSources.Add("FDesde", SAPbouiCOM.BoDataType.dt_DATE);
            txtFDesde.DataBind.SetBound(true, "", "FDesde");

            // Fecha hasta
            oForm.DataSources.UserDataSources.Add("FHasta", SAPbouiCOM.BoDataType.dt_DATE);
            txtFHasta.DataBind.SetBound(true, "", "FHasta");

            // Tipo documento
            cmbTipoD.ValidValues.Add("", "");
            cmbTipoD.ValidValues.Add("FE", "Facturas");
            cmbTipoD.ValidValues.Add("FN", "Facturas Exentas");
            cmbTipoD.ValidValues.Add("BE", "Boletas");
            cmbTipoD.ValidValues.Add("BN", "Boletas Exentas");
            cmbTipoD.ValidValues.Add("ND", "Notas de Débito");
            cmbTipoD.ValidValues.Add("NC", "Notas de Crédito");
            cmbTipoD.ValidValues.Add("GE", "Guías (Entregas)");
            cmbTipoD.ValidValues.Add("GT", "Guías (Traslados)");
            cmbTipoD.ValidValues.Add("FEX", "Facturas Exportación");
            cmbTipoD.ValidValues.Add("NDX", "Notas de Débito Exportación");
            cmbTipoD.ValidValues.Add("NCX", "Notas de Crédito Exportación");

            oForm.DataSources.DataTables.Add("dtDocsE");
            EstructuraMatrixDocumentosEmitidos();
            oMatrixDE.SelectionMode = SAPbouiCOM.BoMatrixSelect.ms_Single;

            oForm.Visible = true;
        }

        private static void EstructuraMatrixDocumentosEmitidos()
        {
            string NombreDT = "dtDocsE";
            SAPbouiCOM.DataTable datatable = oForm.DataSources.DataTables.Item(NombreDT);
            string _query = @"
                SELECT 'N' AS ""Chk"",
                ""ObjType"" AS ""ObjType"",
                ""DocEntry"" AS ""DocEntry"",
                'FE' AS ""TipoDoc"",
                ""DocNum"" AS ""DocNum"",
                ""FolioNum"" AS ""FolioNum"",
                ""isIns"" AS ""isIns"",
                ""Indicator"" AS ""Indicator"",
                ""DocDate"" AS ""DocDate"",
                ""LicTradNum"" AS ""LicTradNum"",
                ""CardCode"" AS ""CardCode"",
                ""CardName"" AS ""CardName"",
                REPLICATE(' ', 10) AS ""Estado1"",
                REPLICATE(' ', 250) AS ""MEstado1"",
                REPLICATE(' ', 10) AS ""Estado2"",
                REPLICATE(' ', 250) AS ""MEstado2"",
                REPLICATE(' ', 10) AS ""Estado3"",
                REPLICATE(' ', 250) AS ""MEstado3"",
                REPLICATE(' ', 250) AS ""Obs"",
                0 AS ""StatusDTE"",
                0 AS ""StatusPDF""
                FROM ""OINV"" WHERE 1 = 0";
            datatable.ExecuteQuery(_query);

            SAPbouiCOM.Columns oColumns;
            oColumns = oMatrixDE.Columns;
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
            oColumn.TitleObject.Caption = "Doc";
            oColumn.TitleObject.Sortable = false;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 15;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "DocEntry");

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
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "isIns");

            oColumn = oColumns.Add("Indicator", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Indicador";
            oColumn.TitleObject.Sortable = false;
            oColumn.Editable = false;
            oColumn.Visible = false;
            oColumn.Width = 20;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "Indicator");

            oColumn = oColumns.Add("DocDate", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Fecha contable";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 80;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "DocDate");

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
            oColumn.TitleObject.Sortable = false;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 80;
            oColumn.RightJustified = false;
            oColumn.DataBind.Bind(NombreDT, "CardCode");

            oColumn = oColumns.Add("CardName", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Nombre SN";
            oColumn.TitleObject.Sortable = false;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 120;
            oColumn.RightJustified = false;
            oColumn.DataBind.Bind(NombreDT, "CardName");

            oColumn = oColumns.Add("Estado1", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Estado Doc.";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 30;
            oColumn.RightJustified = false;
            oColumn.DataBind.Bind(NombreDT, "Estado1");

            oColumn = oColumns.Add("MEstado1", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Desc. Estado Doc.";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 100;
            oColumn.RightJustified = false;
            oColumn.DataBind.Bind(NombreDT, "MEstado1");

            oColumn = oColumns.Add("Estado2", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Estado Cli.";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 30;
            oColumn.RightJustified = false;
            oColumn.DataBind.Bind(NombreDT, "Estado2");

            oColumn = oColumns.Add("MEstado2", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Desc. Estado Cli.";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 100;
            oColumn.RightJustified = false;
            oColumn.DataBind.Bind(NombreDT, "MEstado2");

            oColumn = oColumns.Add("Estado3", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Estado Extra";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 30;
            oColumn.RightJustified = false;
            oColumn.DataBind.Bind(NombreDT, "Estado3");

            oColumn = oColumns.Add("MEstado3", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Desc. Estado Extra";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 100;
            oColumn.RightJustified = false;
            oColumn.DataBind.Bind(NombreDT, "MEstado3");

            oColumn = oColumns.Add("Obs", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Observaciones";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = false;
            oColumn.Width = 200;
            oColumn.RightJustified = false;
            oColumn.DataBind.Bind(NombreDT, "Obs");

            oColumn = oColumns.Add("StatusDTE", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "StatusDTE";
            oColumn.TitleObject.Sortable = false;
            oColumn.Editable = false;
            oColumn.Visible = false;
            oColumn.Width = 20;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "StatusDTE");

            oColumn = oColumns.Add("StatusPDF", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "StatusPDF";
            oColumn.TitleObject.Sortable = false;
            oColumn.Editable = false;
            oColumn.Visible = false;
            oColumn.Width = 20;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "StatusPDF");

            oMatrixDE.Clear();
            oMatrixDE.LoadFromDataSourceEx();
        }

        private static void CargarMatrixDocumentosEmitidos()
        {
            // Filtro por Socio de Negocio
            SAPbouiCOM.EditText oRut = (SAPbouiCOM.EditText)oForm.Items.Item("txtRut").Specific;
            string FiltroSN = oRut.Value;
            FiltroSN = FiltroSN.Replace(".", string.Empty);

            if (!String.IsNullOrEmpty(FiltroSN))
            {
                FiltroSN = string.Format(@"AND ""LicTradNum"" = '{0}' ", FiltroSN);
            }
            else
            {
                FiltroSN = string.Format(@"");
            }

            // Filtro por Fechas
            SAPbouiCOM.EditText oFDesde = (SAPbouiCOM.EditText)oForm.Items.Item("txtFDesde").Specific;
            string DesdeFecha = oFDesde.Value;
            SAPbouiCOM.EditText oFHasta = (SAPbouiCOM.EditText)oForm.Items.Item("txtFHasta").Specific;
            string HastaFecha = oFHasta.Value;

            string FechaFinal = string.Empty;
            string FechaInicial = string.Empty;
            DateTime dt;
            string Mes = string.Empty;
            string Dia = string.Empty;
            // Fechas en formato AAAA-MM-DD
            if (!string.IsNullOrEmpty(DesdeFecha) && !string.IsNullOrEmpty(HastaFecha))
            {
                FechaInicial = string.Format("{0}{1}{2}", DesdeFecha.Substring(0, 4), DesdeFecha.Substring(4, 2), DesdeFecha.Substring(6, 2));
                FechaFinal = string.Format("{0}{1}{2}", HastaFecha.Substring(0, 4), HastaFecha.Substring(4, 2), HastaFecha.Substring(6, 2));
            }
            else
            {
                // Por defecto trae los últimos 1 días
                dt = new DateTime(DateTime.Now.Year, DateTime.Today.Month, DateTime.Today.Day);
                Mes = (dt.Month.ToString().Length == 1) ? dt.Month.ToString().PadLeft(2, '0') : dt.Month.ToString();
                Dia = (dt.Day.ToString().Length == 1) ? dt.Day.ToString().PadLeft(2, '0') : dt.Day.ToString();

                FechaFinal = string.Format("{0}{1}{2}", dt.Year.ToString(), Mes, Dia);

                dt = DateTime.Today.AddDays(-1);
                Mes = (dt.Month.ToString().Length == 1) ? dt.Month.ToString().PadLeft(2, '0') : dt.Month.ToString();
                Dia = (dt.Day.ToString().Length == 1) ? dt.Day.ToString().PadLeft(2, '0') : dt.Day.ToString();

                FechaInicial = string.Format("{0}{1}{2}", dt.Year.ToString(), Mes, Dia);
            }

            string FiltroFecha = string.Format(@"AND ""DocDate"" BETWEEN '{0}' AND '{1}' ", FechaInicial, FechaFinal);

            // Filtro por Tipo de Documento
            SAPbouiCOM.ComboBox oCombobox = (SAPbouiCOM.ComboBox)oForm.Items.Item("cmbTipoD").Specific;
            string FiltroTipo = oCombobox.Selected.Value;

            string _query = @"
                SELECT 'N' AS ""Chk"",
                ""ObjType"" AS ""ObjType"",
                ""DocEntry"" AS ""DocEntry"",
                'FE' AS ""TipoDoc"",
                ""DocNum"" AS ""DocNum"",
                ""FolioNum"" AS ""FolioNum"",
                ""isIns"" AS ""isIns"",
                ""Indicator"" AS ""Indicator"",
                ""DocDate"" AS ""DocDate"",
                ""LicTradNum"" AS ""LicTradNum"",
                ""CardCode"" AS ""CardCode"",
                ""CardName"" AS ""CardName"",
                REPLICATE(' ', 10) AS ""Estado1"",
                REPLICATE(' ', 250) AS ""MEstado1"",
                REPLICATE(' ', 10) AS ""Estado2"",
                REPLICATE(' ', 250) AS ""MEstado2"",
                REPLICATE(' ', 10) AS ""Estado3"",
                REPLICATE(' ', 250) AS ""MEstado3"",
                REPLICATE(' ', 250) AS ""Obs"",
                0 AS ""StatusDTE"",
                0 AS ""StatusPDF""
                FROM ""OINV"" WHERE ""DocSubType"" = '--' AND ""isIns"" = 'N' AND ISNULL(""FolioNum"", 0) <> 0 " + FiltroFecha;
            _query += FiltroSN;
            _query += @"
                UNION ALL
                SELECT 'N' AS ""Chk"",
                ""ObjType"" AS ""ObjType"",
                ""DocEntry"" AS ""DocEntry"",
                'FE' AS ""TipoDoc"",
                ""DocNum"" AS ""DocNum"",
                ""FolioNum"" AS ""FolioNum"",
                ""isIns"" AS ""isIns"",
                ""Indicator"" AS ""Indicator"",
                ""DocDate"" AS ""DocDate"",
                ""LicTradNum"" AS ""LicTradNum"",
                ""CardCode"" AS ""CardCode"",
                ""CardName"" AS ""CardName"",
                REPLICATE(' ', 10) AS ""Estado1"",
                REPLICATE(' ', 250) AS ""MEstado1"",
                REPLICATE(' ', 10) AS ""Estado2"",
                REPLICATE(' ', 250) AS ""MEstado2"",
                REPLICATE(' ', 10) AS ""Estado3"",
                REPLICATE(' ', 250) AS ""MEstado3"",
                REPLICATE(' ', 250) AS ""Obs"",
                0 AS ""StatusDTE"",
                0 AS ""StatusPDF""
                FROM ""OINV"" WHERE ""DocSubType"" = '--' AND ""isIns"" = 'Y' AND ""Indicator"" <> '39' AND ISNULL(""FolioNum"", 0) <> 0 " + FiltroFecha;
            _query += FiltroSN;
            _query += @"
                UNION ALL
                SELECT 'N' AS ""Chk"",
                ""ObjType"" AS ""ObjType"",
                ""DocEntry"" AS ""DocEntry"",
                'FN' AS ""TipoDoc"",
                ""DocNum"" AS ""DocNum"",
                ""FolioNum"" AS ""FolioNum"",
                ""isIns"" AS ""isIns"",
                ""Indicator"" AS ""Indicator"",
                ""DocDate"" AS ""DocDate"",
                ""LicTradNum"" AS ""LicTradNum"",
                ""CardCode"" AS ""CardCode"",
                ""CardName"" AS ""CardName"",
                REPLICATE(' ', 10) AS ""Estado1"",
                REPLICATE(' ', 250) AS ""MEstado1"",
                REPLICATE(' ', 10) AS ""Estado2"",
                REPLICATE(' ', 250) AS ""MEstado2"",
                REPLICATE(' ', 10) AS ""Estado3"",
                REPLICATE(' ', 250) AS ""MEstado3"",
                REPLICATE(' ', 250) AS ""Obs"",
                0 AS ""StatusDTE"",
                0 AS ""StatusPDF""
                FROM ""OINV"" WHERE ""DocSubType"" = 'IE' AND ISNULL(""FolioNum"", 0) <> 0 " + FiltroFecha;
            _query += FiltroSN;
            _query += @"
                UNION ALL
                SELECT 'N' AS ""Chk"",
                ""ObjType"" AS ""ObjType"",
                ""DocEntry"" AS ""DocEntry"",
                'BE' AS ""TipoDoc"",
                ""DocNum"" AS ""DocNum"",
                ""FolioNum"" AS ""FolioNum"",
                ""isIns"" AS ""isIns"",
                ""Indicator"" AS ""Indicator"",
                ""DocDate"" AS ""DocDate"",
                ""LicTradNum"" AS ""LicTradNum"",
                ""CardCode"" AS ""CardCode"",
                ""CardName"" AS ""CardName"",
                REPLICATE(' ', 10) AS ""Estado1"",
                REPLICATE(' ', 250) AS ""MEstado1"",
                REPLICATE(' ', 10) AS ""Estado2"",
                REPLICATE(' ', 250) AS ""MEstado2"",
                REPLICATE(' ', 10) AS ""Estado3"",
                REPLICATE(' ', 250) AS ""MEstado3"",
                REPLICATE(' ', 250) AS ""Obs"",
                0 AS ""StatusDTE"",
                0 AS ""StatusPDF""
                FROM ""OINV"" WHERE ""DocSubType"" = 'IB' AND ISNULL(""FolioNum"", 0) <> 0 " + FiltroFecha;
            _query += FiltroSN;
            _query += @"
                UNION ALL
                SELECT 'N' AS ""Chk"",
                ""ObjType"" AS ""ObjType"",
                ""DocEntry"" AS ""DocEntry"",
                'BE' AS ""TipoDoc"",
                ""DocNum"" AS ""DocNum"",
                ""FolioNum"" AS ""FolioNum"",
                ""isIns"" AS ""isIns"",
                ""Indicator"" AS ""Indicator"",
                ""DocDate"" AS ""DocDate"",
                ""LicTradNum"" AS ""LicTradNum"",
                ""CardCode"" AS ""CardCode"",
                ""CardName"" AS ""CardName"",
                REPLICATE(' ', 10) AS ""Estado1"",
                REPLICATE(' ', 250) AS ""MEstado1"",
                REPLICATE(' ', 10) AS ""Estado2"",
                REPLICATE(' ', 250) AS ""MEstado2"",
                REPLICATE(' ', 10) AS ""Estado3"",
                REPLICATE(' ', 250) AS ""MEstado3"",
                REPLICATE(' ', 250) AS ""Obs"",
                0 AS ""StatusDTE"",
                0 AS ""StatusPDF""
                FROM ""OINV"" WHERE ""DocSubType"" = '--' AND ""isIns"" = 'Y' AND ""Indicator"" = '39' AND ISNULL(""FolioNum"", 0) <> 0 " + FiltroFecha;
            _query += FiltroSN;
            _query += @"
                UNION ALL
                SELECT 'N' AS ""Chk"",
                ""ObjType"" AS ""ObjType"",
                ""DocEntry"" AS ""DocEntry"",
                'BN' AS ""TipoDoc"",
                ""DocNum"" AS ""DocNum"",
                ""FolioNum"" AS ""FolioNum"",
                ""isIns"" AS ""isIns"",
                ""Indicator"" AS ""Indicator"",
                ""DocDate"" AS ""DocDate"",
                ""LicTradNum"" AS ""LicTradNum"",
                ""CardCode"" AS ""CardCode"",
                ""CardName"" AS ""CardName"",
                REPLICATE(' ', 10) AS ""Estado1"",
                REPLICATE(' ', 250) AS ""MEstado1"",
                REPLICATE(' ', 10) AS ""Estado2"",
                REPLICATE(' ', 250) AS ""MEstado2"",
                REPLICATE(' ', 10) AS ""Estado3"",
                REPLICATE(' ', 250) AS ""MEstado3"",
                REPLICATE(' ', 250) AS ""Obs"",
                0 AS ""StatusDTE"",
                0 AS ""StatusPDF""
                FROM ""OINV"" WHERE ""DocSubType"" = 'EB' AND ISNULL(""FolioNum"", 0) <> 0 " + FiltroFecha;
            _query += FiltroSN;
            _query += @"
                UNION ALL
                SELECT 'N' AS ""Chk"",
                ""ObjType"" AS ""ObjType"",
                ""DocEntry"" AS ""DocEntry"",
                'ND' AS ""TipoDoc"",
                ""DocNum"" AS ""DocNum"",
                ""FolioNum"" AS ""FolioNum"",
                ""isIns"" AS ""isIns"",
                ""Indicator"" AS ""Indicator"",
                ""DocDate"" AS ""DocDate"",
                ""LicTradNum"" AS ""LicTradNum"",
                ""CardCode"" AS ""CardCode"",
                ""CardName"" AS ""CardName"",
                REPLICATE(' ', 10) AS ""Estado1"",
                REPLICATE(' ', 250) AS ""MEstado1"",
                REPLICATE(' ', 10) AS ""Estado2"",
                REPLICATE(' ', 250) AS ""MEstado2"",
                REPLICATE(' ', 10) AS ""Estado3"",
                REPLICATE(' ', 250) AS ""MEstado3"",
                REPLICATE(' ', 250) AS ""Obs"",
                0 AS ""StatusDTE"",
                0 AS ""StatusPDF""
                FROM ""OINV"" WHERE ""DocSubType"" = 'DN' AND ""Indicator"" <> '11' AND ISNULL(""FolioNum"", 0) <> 0 " + FiltroFecha;
            _query += FiltroSN;
            _query += @"
                UNION ALL
                SELECT 'N' AS ""Chk"",
                ""ObjType"" AS ""ObjType"",
                ""DocEntry"" AS ""DocEntry"",
                'NC' AS ""TipoDoc"",
                ""DocNum"" AS ""DocNum"",
                ""FolioNum"" AS ""FolioNum"",
                '' AS ""isIns"",
                '' AS ""Indicator"",
                ""DocDate"" AS ""DocDate"",
                ""LicTradNum"" AS ""LicTradNum"",
                ""CardCode"" AS ""CardCode"",
                ""CardName"" AS ""CardName"",
                REPLICATE(' ', 10) AS ""Estado1"",
                REPLICATE(' ', 250) AS ""MEstado1"",
                REPLICATE(' ', 10) AS ""Estado2"",
                REPLICATE(' ', 250) AS ""MEstado2"",
                REPLICATE(' ', 10) AS ""Estado3"",
                REPLICATE(' ', 250) AS ""MEstado3"",
                REPLICATE(' ', 250) AS ""Obs"",
                0 AS ""StatusDTE"",
                0 AS ""StatusPDF""
                FROM ""ORIN"" WHERE ""Indicator"" <> '12' AND ISNULL(""FolioNum"", 0) <> 0 " + FiltroFecha;
            _query += FiltroSN;
            _query += @"
                UNION ALL
                SELECT 'N' AS ""Chk"",
                ""ObjType"" AS ""ObjType"",
                ""DocEntry"" AS ""DocEntry"",
                'GE' AS ""TipoDoc"",
                ""DocNum"" AS ""DocNum"",
                ""FolioNum"" AS ""FolioNum"",
                '' AS ""isIns"",
                '' AS ""Indicator"",
                ""DocDate"" AS ""DocDate"",
                ""LicTradNum"" AS ""LicTradNum"",
                ""CardCode"" AS ""CardCode"",
                ""CardName"" AS ""CardName"",
                REPLICATE(' ', 10) AS ""Estado1"",
                REPLICATE(' ', 250) AS ""MEstado1"",
                REPLICATE(' ', 10) AS ""Estado2"",
                REPLICATE(' ', 250) AS ""MEstado2"",
                REPLICATE(' ', 10) AS ""Estado3"",
                REPLICATE(' ', 250) AS ""MEstado3"",
                REPLICATE(' ', 250) AS ""Obs"",
                0 AS ""StatusDTE"",
                0 AS ""StatusPDF""
                FROM ""ODLN"" WHERE ISNULL(""FolioNum"", 0) <> 0 " + FiltroFecha;
            _query += FiltroSN;
            _query += @"
                UNION ALL
                SELECT 'N' AS ""Chk"",
                ""ObjType"" AS ""ObjType"",
                ""DocEntry"" AS ""DocEntry"",
                'GT' AS ""TipoDoc"",
                ""DocNum"" AS ""DocNum"",
                ""FolioNum"" AS ""FolioNum"",
                '' AS ""isIns"",
                '' AS ""Indicator"",
                ""DocDate"" AS ""DocDate"",
                ""LicTradNum"" AS ""LicTradNum"",
                ""CardCode"" AS ""CardCode"",
                ""CardName"" AS ""CardName"",
                REPLICATE(' ', 10) AS ""Estado1"",
                REPLICATE(' ', 250) AS ""MEstado1"",
                REPLICATE(' ', 10) AS ""Estado2"",
                REPLICATE(' ', 250) AS ""MEstado2"",
                REPLICATE(' ', 10) AS ""Estado3"",
                REPLICATE(' ', 250) AS ""MEstado3"",
                REPLICATE(' ', 250) AS ""Obs"",
                0 AS ""StatusDTE"",
                0 AS ""StatusPDF""
                FROM ""OWTR"" WHERE ISNULL(""FolioNum"", 0) <> 0 " + FiltroFecha;
            _query += FiltroSN;
            _query += @"
                UNION ALL
                SELECT 'N' AS ""Chk"",
                ""ObjType"" AS ""ObjType"",
                ""DocEntry"" AS ""DocEntry"",
                'FEX' AS ""TipoDoc"",
                ""DocNum"" AS ""DocNum"",
                ""FolioNum"" AS ""FolioNum"",
                ""isIns"" AS ""isIns"",
                ""Indicator"" AS ""Indicator"",
                ""DocDate"" AS ""DocDate"",
                ""LicTradNum"" AS ""LicTradNum"",
                ""CardCode"" AS ""CardCode"",
                ""CardName"" AS ""CardName"",
                REPLICATE(' ', 10) AS ""Estado1"",
                REPLICATE(' ', 250) AS ""MEstado1"",
                REPLICATE(' ', 10) AS ""Estado2"",
                REPLICATE(' ', 250) AS ""MEstado2"",
                REPLICATE(' ', 10) AS ""Estado3"",
                REPLICATE(' ', 250) AS ""MEstado3"",
                REPLICATE(' ', 250) AS ""Obs"",
                0 AS ""StatusDTE"",
                0 AS ""StatusPDF""
                FROM ""OINV"" WHERE ""DocSubType"" = 'IX' AND ISNULL(""FolioNum"", 0) <> 0 " + FiltroFecha;
            _query += FiltroSN;
            _query += @"
                UNION ALL
                SELECT 'N' AS ""Chk"",
                ""ObjType"" AS ""ObjType"",
                ""DocEntry"" AS ""DocEntry"",
                'NDX' AS ""TipoDoc"",
                ""DocNum"" AS ""DocNum"",
                ""FolioNum"" AS ""FolioNum"",
                ""isIns"" AS ""isIns"",
                ""Indicator"" AS ""Indicator"",
                ""DocDate"" AS ""DocDate"",
                ""LicTradNum"" AS ""LicTradNum"",
                ""CardCode"" AS ""CardCode"",
                ""CardName"" AS ""CardName"",
                REPLICATE(' ', 10) AS ""Estado1"",
                REPLICATE(' ', 250) AS ""MEstado1"",
                REPLICATE(' ', 10) AS ""Estado2"",
                REPLICATE(' ', 250) AS ""MEstado2"",
                REPLICATE(' ', 10) AS ""Estado3"",
                REPLICATE(' ', 250) AS ""MEstado3"",
                REPLICATE(' ', 250) AS ""Obs"",
                0 AS ""StatusDTE"",
                0 AS ""StatusPDF""
                FROM ""OINV"" WHERE ""DocSubType"" = 'DN' AND ""Indicator"" = '11' AND ISNULL(""FolioNum"", 0) <> 0 " + FiltroFecha;
            _query += FiltroSN;
            _query += @"
                UNION ALL
                SELECT 'N' AS ""Chk"",
                ""ObjType"" AS ""ObjType"",
                ""DocEntry"" AS ""DocEntry"",
                'NCX' AS ""TipoDoc"",
                ""DocNum"" AS ""DocNum"",
                ""FolioNum"" AS ""FolioNum"",
                '' AS ""isIns"",
                '' AS ""Indicator"",
                ""DocDate"" AS ""DocDate"",
                ""LicTradNum"" AS ""LicTradNum"",
                ""CardCode"" AS ""CardCode"",
                ""CardName"" AS ""CardName"",
                REPLICATE(' ', 10) AS ""Estado1"",
                REPLICATE(' ', 250) AS ""MEstado1"",
                REPLICATE(' ', 10) AS ""Estado2"",
                REPLICATE(' ', 250) AS ""MEstado2"",
                REPLICATE(' ', 10) AS ""Estado3"",
                REPLICATE(' ', 250) AS ""MEstado3"",
                REPLICATE(' ', 250) AS ""Obs"",
                0 AS ""StatusDTE"",
                0 AS ""StatusPDF""
                FROM ""ORIN"" WHERE ""Indicator"" = '12' AND ISNULL(""FolioNum"", 0) <> 0 " + FiltroFecha;
            _query += FiltroSN;

            if (!String.IsNullOrEmpty(FiltroTipo))
            {
                switch (FiltroTipo)
                {
                    case "FE":
                        _query = @"
                            SELECT 'N' AS ""Chk"",
                            ""ObjType"" AS ""ObjType"",
                            ""DocEntry"" AS ""DocEntry"",
                            'FE' AS ""TipoDoc"",
                            ""DocNum"" AS ""DocNum"",
                            ""FolioNum"" AS ""FolioNum"",
                            ""isIns"" AS ""isIns"",
                            ""Indicator"" AS ""Indicator"",
                            ""DocDate"" AS ""DocDate"",
                            ""LicTradNum"" AS ""LicTradNum"",
                            ""CardCode"" AS ""CardCode"",
                            ""CardName"" AS ""CardName"",
                            REPLICATE(' ', 10) AS ""Estado1"",
                            REPLICATE(' ', 250) AS ""MEstado1"",
                            REPLICATE(' ', 10) AS ""Estado2"",
                            REPLICATE(' ', 250) AS ""MEstado2"",
                            REPLICATE(' ', 10) AS ""Estado3"",
                            REPLICATE(' ', 250) AS ""MEstado3"",
                            REPLICATE(' ', 250) AS ""Obs"",
                            0 AS ""StatusDTE"",
                            0 AS ""StatusPDF""
                            FROM ""OINV"" WHERE ""DocSubType"" = '--' AND ""Indicator"" <> '39' AND ISNULL(""FolioNum"", 0) <> 0 " + FiltroFecha;
                        _query += FiltroSN;
                        break;
                    case "FN":
                        _query = @"
                            SELECT 'N' AS ""Chk"",
                            ""ObjType"" AS ""ObjType"",
                            ""DocEntry"" AS ""DocEntry"",
                            'FN' AS ""TipoDoc"",
                            ""DocNum"" AS ""DocNum"",
                            ""FolioNum"" AS ""FolioNum"",
                            ""isIns"" AS ""isIns"",
                            ""Indicator"" AS ""Indicator"",
                            ""DocDate"" AS ""DocDate"",
                            ""LicTradNum"" AS ""LicTradNum"",
                            ""CardCode"" AS ""CardCode"",
                            ""CardName"" AS ""CardName"",
                            REPLICATE(' ', 10) AS ""Estado1"",
                            REPLICATE(' ', 250) AS ""MEstado1"",
                            REPLICATE(' ', 10) AS ""Estado2"",
                            REPLICATE(' ', 250) AS ""MEstado2"",
                            REPLICATE(' ', 10) AS ""Estado3"",
                            REPLICATE(' ', 250) AS ""MEstado3"",
                            REPLICATE(' ', 250) AS ""Obs"",
                            0 AS ""StatusDTE"",
                            0 AS ""StatusPDF""
                            FROM ""OINV"" WHERE ""DocSubType"" = 'IE' AND ISNULL(""FolioNum"", 0) <> 0 " + FiltroFecha;
                        _query += FiltroSN;
                        break;
                    case "BE":
                        _query = @"
                            SELECT 'N' AS ""Chk"",
                            ""ObjType"" AS ""ObjType"",
                            ""DocEntry"" AS ""DocEntry"",
                            'BE' AS ""TipoDoc"",
                            ""DocNum"" AS ""DocNum"",
                            ""FolioNum"" AS ""FolioNum"",
                            ""isIns"" AS ""isIns"",
                            ""Indicator"" AS ""Indicator"",
                            ""DocDate"" AS ""DocDate"",
                            ""LicTradNum"" AS ""LicTradNum"",
                            ""CardCode"" AS ""CardCode"",
                            ""CardName"" AS ""CardName"",
                            REPLICATE(' ', 10) AS ""Estado1"",
                            REPLICATE(' ', 250) AS ""MEstado1"",
                            REPLICATE(' ', 10) AS ""Estado2"",
                            REPLICATE(' ', 250) AS ""MEstado2"",
                            REPLICATE(' ', 10) AS ""Estado3"",
                            REPLICATE(' ', 250) AS ""MEstado3"",
                            REPLICATE(' ', 250) AS ""Obs"",
                            0 AS ""StatusDTE"",
                            0 AS ""StatusPDF""
                            FROM ""OINV"" WHERE ""DocSubType"" = 'IB' AND ISNULL(""FolioNum"", 0) <> 0 " + FiltroFecha;
                        _query += FiltroSN;
                        _query += @"
                            UNION ALL
                            SELECT 'N' AS ""Chk"",
                            ""ObjType"" AS ""ObjType"",
                            ""DocEntry"" AS ""DocEntry"",
                            'BE' AS ""TipoDoc"",
                            ""DocNum"" AS ""DocNum"",
                            ""FolioNum"" AS ""FolioNum"",
                            ""isIns"" AS ""isIns"",
                            ""Indicator"" AS ""Indicator"",
                            ""DocDate"" AS ""DocDate"",
                            ""LicTradNum"" AS ""LicTradNum"",
                            ""CardCode"" AS ""CardCode"",
                            ""CardName"" AS ""CardName"",
                            REPLICATE(' ', 10) AS ""Estado1"",
                            REPLICATE(' ', 250) AS ""MEstado1"",
                            REPLICATE(' ', 10) AS ""Estado2"",
                            REPLICATE(' ', 250) AS ""MEstado2"",
                            REPLICATE(' ', 10) AS ""Estado3"",
                            REPLICATE(' ', 250) AS ""MEstado3"",
                            REPLICATE(' ', 250) AS ""Obs"",
                            0 AS ""StatusDTE"",
                            0 AS ""StatusPDF""
                            FROM ""OINV"" WHERE ""DocSubType"" = '--' AND ""isIns"" = 'Y' AND ""Indicator"" = '39' AND ISNULL(""FolioNum"", 0) <> 0 " + FiltroFecha;
                        _query += FiltroSN;
                        break;
                    case "BN":
                        _query = @"
                            SELECT 'N' AS ""Chk"",
                            ""ObjType"" AS ""ObjType"",
                            ""DocEntry"" AS ""DocEntry"",
                            'BN' AS ""TipoDoc"",
                            ""DocNum"" AS ""DocNum"",
                            ""FolioNum"" AS ""FolioNum"",
                            ""isIns"" AS ""isIns"",
                            ""Indicator"" AS ""Indicator"",
                            ""DocDate"" AS ""DocDate"",
                            ""LicTradNum"" AS ""LicTradNum"",
                            ""CardCode"" AS ""CardCode"",
                            ""CardName"" AS ""CardName"",
                            REPLICATE(' ', 10) AS ""Estado1"",
                            REPLICATE(' ', 250) AS ""MEstado1"",
                            REPLICATE(' ', 10) AS ""Estado2"",
                            REPLICATE(' ', 250) AS ""MEstado2"",
                            REPLICATE(' ', 10) AS ""Estado3"",
                            REPLICATE(' ', 250) AS ""MEstado3"",
                            REPLICATE(' ', 250) AS ""Obs"",
                            0 AS ""StatusDTE"",
                            0 AS ""StatusPDF""
                            FROM ""OINV"" WHERE ""DocSubType"" = 'EB' AND ISNULL(""FolioNum"", 0) <> 0 " + FiltroFecha;
                        _query += FiltroSN;
                        break;
                    case "ND":
                        _query = @"
                            SELECT 'N' AS ""Chk"",
                            ""ObjType"" AS ""ObjType"",
                            ""DocEntry"" AS ""DocEntry"",
                            'ND' AS ""TipoDoc"",
                            ""DocNum"" AS ""DocNum"",
                            ""FolioNum"" AS ""FolioNum"",
                            ""isIns"" AS ""isIns"",
                            ""Indicator"" AS ""Indicator"",
                            ""DocDate"" AS ""DocDate"",
                            ""LicTradNum"" AS ""LicTradNum"",
                            ""CardCode"" AS ""CardCode"",
                            ""CardName"" AS ""CardName"",
                            REPLICATE(' ', 10) AS ""Estado1"",
                            REPLICATE(' ', 250) AS ""MEstado1"",
                            REPLICATE(' ', 10) AS ""Estado2"",
                            REPLICATE(' ', 250) AS ""MEstado2"",
                            REPLICATE(' ', 10) AS ""Estado3"",
                            REPLICATE(' ', 250) AS ""MEstado3"",
                            REPLICATE(' ', 250) AS ""Obs"",
                            0 AS ""StatusDTE"",
                            0 AS ""StatusPDF""
                            FROM ""OINV"" WHERE ""DocSubType"" = 'DN' AND ""Indicator"" <> '11' AND ISNULL(""FolioNum"", 0) <> 0 " + FiltroFecha;
                        _query += FiltroSN;
                        break;
                    case "NC":
                        _query = @"
                            SELECT 'N' AS ""Chk"",
                            ""ObjType"" AS ""ObjType"",
                            ""DocEntry"" AS ""DocEntry"",
                            'NC' AS ""TipoDoc"",
                            ""DocNum"" AS ""DocNum"",
                            ""FolioNum"" AS ""FolioNum"",
                            '' AS ""isIns"",
                            '' AS ""Indicator"",
                            ""DocDate"" AS ""DocDate"",
                            ""LicTradNum"" AS ""LicTradNum"",
                            ""CardCode"" AS ""CardCode"",
                            ""CardName"" AS ""CardName"",
                            REPLICATE(' ', 10) AS ""Estado1"",
                            REPLICATE(' ', 250) AS ""MEstado1"",
                            REPLICATE(' ', 10) AS ""Estado2"",
                            REPLICATE(' ', 250) AS ""MEstado2"",
                            REPLICATE(' ', 10) AS ""Estado3"",
                            REPLICATE(' ', 250) AS ""MEstado3"",
                            REPLICATE(' ', 250) AS ""Obs"",
                            0 AS ""StatusDTE"",
                            0 AS ""StatusPDF""
                            FROM ""ORIN"" WHERE ""Indicator"" <> '12' AND ISNULL(""FolioNum"", 0) <> 0 " + FiltroFecha;
                        _query += FiltroSN;
                        break;
                    case "GE":
                        _query = @"
                            SELECT 'N' AS ""Chk"",
                            ""ObjType"" AS ""ObjType"",
                            ""DocEntry"" AS ""DocEntry"",
                            'GE' AS ""TipoDoc"",
                            ""DocNum"" AS ""DocNum"",
                            ""FolioNum"" AS ""FolioNum"",
                            '' AS ""isIns"",
                            '' AS ""Indicator"",
                            ""DocDate"" AS ""DocDate"",
                            ""LicTradNum"" AS ""LicTradNum"",
                            ""CardCode"" AS ""CardCode"",
                            ""CardName"" AS ""CardName"",
                            REPLICATE(' ', 10) AS ""Estado1"",
                            REPLICATE(' ', 250) AS ""MEstado1"",
                            REPLICATE(' ', 10) AS ""Estado2"",
                            REPLICATE(' ', 250) AS ""MEstado2"",
                            REPLICATE(' ', 10) AS ""Estado3"",
                            REPLICATE(' ', 250) AS ""MEstado3"",
                            REPLICATE(' ', 250) AS ""Obs"",
                            0 AS ""StatusDTE"",
                            0 AS ""StatusPDF""
                            FROM ""ODLN"" WHERE ISNULL(""FolioNum"", 0) <> 0 " + FiltroFecha;
                        _query += FiltroSN;
                        break;
                    case "GT":
                        _query = @"
                            SELECT 'N' AS ""Chk"",
                            ""ObjType"" AS ""ObjType"",
                            ""DocEntry"" AS ""DocEntry"",
                            'GT' AS ""TipoDoc"",
                            ""DocNum"" AS ""DocNum"",
                            ""FolioNum"" AS ""FolioNum"",
                            '' AS ""isIns"",
                            '' AS ""Indicator"",
                            ""DocDate"" AS ""DocDate"",
                            ""LicTradNum"" AS ""LicTradNum"",
                            ""CardCode"" AS ""CardCode"",
                            ""CardName"" AS ""CardName"",
                            REPLICATE(' ', 10) AS ""Estado1"",
                            REPLICATE(' ', 250) AS ""MEstado1"",
                            REPLICATE(' ', 10) AS ""Estado2"",
                            REPLICATE(' ', 250) AS ""MEstado2"",
                            REPLICATE(' ', 10) AS ""Estado3"",
                            REPLICATE(' ', 250) AS ""MEstado3"",
                            REPLICATE(' ', 250) AS ""Obs"",
                            0 AS ""StatusDTE"",
                            0 AS ""StatusPDF""
                            FROM ""OWTR"" WHERE ISNULL(""FolioNum"", 0) <> 0 " + FiltroFecha;
                        _query += FiltroSN;
                        break;
                    case "FEX":
                        _query = @"
                            SELECT 'N' AS ""Chk"",
                            ""ObjType"" AS ""ObjType"",
                            ""DocEntry"" AS ""DocEntry"",
                            'FEX' AS ""TipoDoc"",
                            ""DocNum"" AS ""DocNum"",
                            ""FolioNum"" AS ""FolioNum"",
                            ""isIns"" AS ""isIns"",
                            ""Indicator"" AS ""Indicator"",
                            ""DocDate"" AS ""DocDate"",
                            ""LicTradNum"" AS ""LicTradNum"",
                            ""CardCode"" AS ""CardCode"",
                            ""CardName"" AS ""CardName"",
                            REPLICATE(' ', 10) AS ""Estado1"",
                            REPLICATE(' ', 250) AS ""MEstado1"",
                            REPLICATE(' ', 10) AS ""Estado2"",
                            REPLICATE(' ', 250) AS ""MEstado2"",
                            REPLICATE(' ', 10) AS ""Estado3"",
                            REPLICATE(' ', 250) AS ""MEstado3"",
                            REPLICATE(' ', 250) AS ""Obs"",
                            0 AS ""StatusDTE"",
                            0 AS ""StatusPDF""
                            FROM ""OINV"" WHERE ""DocSubType"" = 'IX' AND ISNULL(""FolioNum"", 0) <> 0 " + FiltroFecha;
                        _query += FiltroSN;
                        break;
                    case "NDX":
                        _query = @"
                            SELECT 'N' AS ""Chk"",
                            ""ObjType"" AS ""ObjType"",
                            ""DocEntry"" AS ""DocEntry"",
                            'NDX' AS ""TipoDoc"",
                            ""DocNum"" AS ""DocNum"",
                            ""FolioNum"" AS ""FolioNum"",
                            ""isIns"" AS ""isIns"",
                            ""Indicator"" AS ""Indicator"",
                            ""DocDate"" AS ""DocDate"",
                            ""LicTradNum"" AS ""LicTradNum"",
                            ""CardCode"" AS ""CardCode"",
                            ""CardName"" AS ""CardName"",
                            REPLICATE(' ', 10) AS ""Estado1"",
                            REPLICATE(' ', 250) AS ""MEstado1"",
                            REPLICATE(' ', 10) AS ""Estado2"",
                            REPLICATE(' ', 250) AS ""MEstado2"",
                            REPLICATE(' ', 10) AS ""Estado3"",
                            REPLICATE(' ', 250) AS ""MEstado3"",
                            REPLICATE(' ', 250) AS ""Obs"",
                            0 AS ""StatusDTE"",
                            0 AS ""StatusPDF""
                            FROM ""OINV"" WHERE ""DocSubType"" = 'DN' AND ""Indicator"" = '11' AND ISNULL(""FolioNum"", 0) <> 0 " + FiltroFecha;
                        _query += FiltroSN;
                        break;
                    case "NCX":
                        _query = @"
                            SELECT 'N' AS ""Chk"",
                            ""ObjType"" AS ""ObjType"",
                            ""DocEntry"" AS ""DocEntry"",
                            'NCX' AS ""TipoDoc"",
                            ""DocNum"" AS ""DocNum"",
                            ""FolioNum"" AS ""FolioNum"",
                            '' AS ""isIns"",
                            '' AS ""Indicator"",
                            ""DocDate"" AS ""DocDate"",
                            ""LicTradNum"" AS ""LicTradNum"",
                            ""CardCode"" AS ""CardCode"",
                            ""CardName"" AS ""CardName"",
                            REPLICATE(' ', 10) AS ""Estado1"",
                            REPLICATE(' ', 250) AS ""MEstado1"",
                            REPLICATE(' ', 10) AS ""Estado2"",
                            REPLICATE(' ', 250) AS ""MEstado2"",
                            REPLICATE(' ', 10) AS ""Estado3"",
                            REPLICATE(' ', 250) AS ""MEstado3"",
                            REPLICATE(' ', 250) AS ""Obs"",
                            0 AS ""StatusDTE"",
                            0 AS ""StatusPDF""
                            FROM ""ORIN"" WHERE ""Indicator"" = '12' AND ISNULL(""FolioNum"", 0) <> 0 " + FiltroFecha;
                        _query += FiltroSN;
                        break;
                    default:
                        break;
                }
            }

            string NombreDT = "dtDocsE";
            SAPbouiCOM.DataTable datatable = oForm.DataSources.DataTables.Item(NombreDT);

            _query += @"ORDER BY ""DocDate"" DESC";
            datatable.ExecuteQuery(_query);
            oForm.Freeze(true);
            oMatrixDE.Clear();
            oMatrixDE.LoadFromDataSourceEx();
            oForm.Freeze(false);
        }

        /// <summary>
        /// Crea la estructura común de los datatables usados para cada matrix
        /// </summary>
        /// <param name="NombreDT"></param>
        private void CreaEstructuraComunDataTable(string NombreDT)
        {
            oForm.DataSources.DataTables.Add(NombreDT);
            SAPbouiCOM.DataTable dt = oForm.DataSources.DataTables.Item(NombreDT);

            dt.Columns.Add("DocEntry", SAPbouiCOM.BoFieldsType.ft_Integer);
            dt.Columns.Add("co_num", SAPbouiCOM.BoFieldsType.ft_Integer);
            switch (NombreDT)
            {
                case "DOCUMENTOSDE":
                    dt.Columns.Add("co_ChkDE", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
                    break;
            }
            dt.Columns.Add("co_pdf", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dt.Columns.Add("co_febosId", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dt.Columns.Add("co_tipoDoc", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dt.Columns.Add("co_folio", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dt.Columns.Add("co_fechaEm", SAPbouiCOM.BoFieldsType.ft_Date);
            dt.Columns.Add("co_formaPa", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dt.Columns.Add("co_iDeTras", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dt.Columns.Add("co_rutEmis", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dt.Columns.Add("co_razSocE", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dt.Columns.Add("co_rutRece", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dt.Columns.Add("co_razSocR", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dt.Columns.Add("co_contact", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dt.Columns.Add("co_iva", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dt.Columns.Add("co_montoTo", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dt.Columns.Add("co_estSii", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dt.Columns.Add("co_estCome", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dt.Columns.Add("co_fechaRe", SAPbouiCOM.BoFieldsType.ft_Date);
            dt.Columns.Add("co_tipo", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dt.Columns.Add("co_codSii", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dt.Columns.Add("co_tieneNc", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dt.Columns.Add("co_tieneNd", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dt.Columns.Add("co_docBase", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dt.Columns.Add("co_tipoAso", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dt.Columns.Add("co_docAso", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dt.Columns.Add("co_razonRe", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dt.Columns.Add("co_estado", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dt.Columns.Add("co_obs", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dt.Columns.Add("co_prelim", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
        }

        /// <summary>
        /// Genera estructura de Matrix documentos descargados
        /// </summary>
        private void EstructuraMatrixDescarga()
        {
            string NombreDT = "DOCUMENTOSDE";
            oForm.DataSources.DataTables.Add(NombreDT);
            //SAPbouiCOM.DataTable datatable = oForm.DataSources.DataTables.Item(NombreDT);
            //string _query = ObtenerQueryConFiltros("99");
            //datatable.ExecuteQuery(_query);

            SAPbouiCOM.Columns oColumns;
            oColumns = oMatrixDE.Columns;
            SAPbouiCOM.Column oColumn;

            oColumn = oColumns.Add("DocEntry", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "DocEntry";
            oColumn.Editable = false;
            oColumn.Visible = false;
            oColumn.DataBind.Bind(NombreDT, "DocEntry");

            oColumn = oColumns.Add("co_num", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "#";
            oColumn.Editable = false;
            oColumn.Width = 20;
            oColumn.DataBind.Bind(NombreDT, "NUM");

            oColumn = oColumns.Add("co_ChkDL", SAPbouiCOM.BoFormItemTypes.it_CHECK_BOX);
            oColumn.TitleObject.Caption = String.Empty;
            oColumn.Editable = true;
            oColumn.Width = 15;
            oColumn.ValOn = "Y";
            oColumn.ValOff = "N";
            oColumn.DataBind.Bind(NombreDT, "CHK");

            oColumn = oColumns.Add("co_pdf", SAPbouiCOM.BoFormItemTypes.it_PICTURE);
            oColumn.TitleObject.Caption = "Pdf";
            oColumn.Editable = false;
            oColumn.Width = 30;
            oColumn.DataBind.Bind(NombreDT, "PDF");

            oColumn = oColumns.Add("co_plazo", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Plazo";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 40;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "U_PLAZO");

            // Columna que guarda pero no muestra el DTE ID, que se usa para dar respuesta comercial
            oColumn = oColumns.Add("co_docId", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.Visible = false;
            oColumn.DataBind.Bind(NombreDT, "U_DOCID");

            oColumn = oColumns.Add("co_tipoDoc", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Tipo Doc.";
            oColumn.Editable = false;
            oColumn.Width = 60;
            oColumn.DataBind.Bind(NombreDT, "U_TIPODOC");

            oColumn = oColumns.Add("co_folio", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Folio";
            oColumn.Editable = false;
            oColumn.Width = 60;
            oColumn.DataBind.Bind(NombreDT, "U_FOLIO");

            oColumn = oColumns.Add("co_fechaEm", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Fecha";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Width = 60;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "U_FECHAEM");

            oColumn = oColumns.Add("co_formaPa", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Forma de pago";
            oColumn.Editable = false;
            oColumn.Visible = false;
            oColumn.Width = 60;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "U_FORMAPA");

            oColumn = oColumns.Add("co_iDeTras", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Indicador de traslado";
            oColumn.Editable = false;
            oColumn.Visible = false;
            oColumn.Width = 60;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "U_IDETRAS");

            oColumn = oColumns.Add("co_rutEmis", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Rut Emisor";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Width = 70;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "U_RUTEMIS");

            oColumn = oColumns.Add("co_razSocE", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Razón Social";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Width = 200;
            oColumn.DataBind.Bind(NombreDT, "U_RAZSOCE");

            oColumn = oColumns.Add("co_rutRece", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Rut Receptor";
            oColumn.Editable = false;
            oColumn.Visible = false;
            oColumn.Width = 60;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "U_RUTRECE");

            oColumn = oColumns.Add("co_razSocR", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Razón Social";
            oColumn.Editable = false;
            oColumn.Visible = false;
            oColumn.Width = 60;
            oColumn.DataBind.Bind(NombreDT, "U_RAZSOCR");

            oColumn = oColumns.Add("co_contact", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Contacto";
            oColumn.Editable = false;
            oColumn.Visible = false;
            oColumn.Width = 60;
            oColumn.DataBind.Bind(NombreDT, "U_CONTACT");

            oColumn = oColumns.Add("co_iva", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "IVA";
            //oColumn.ExpandType=
            oColumn.Editable = false;
            oColumn.Visible = false;
            oColumn.Width = 60;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "U_IVA");

            oColumn = oColumns.Add("co_montoTo", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Monto Total";
            oColumn.TitleObject.Sortable = true;
            //oColumn.ExpandType=
            oColumn.Editable = false;
            oColumn.Width = 80;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "U_MONTOTO");

            oColumn = oColumns.Add("co_docBase", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "OC";
            oColumn.Editable = false;
            oColumn.Visible = false;
            oColumn.Width = 60;
            oColumn.DataBind.Bind(NombreDT, "U_DOCBASE");

            oColumn = oColumns.Add("co_docAso", SAPbouiCOM.BoFormItemTypes.it_LINKED_BUTTON);
            oColumn.TitleObject.Caption = "Link OC";
            oColumn.Editable = false;
            oColumn.Visible = false;
            oColumn.Width = 60;
            oColumn.DataBind.Bind(NombreDT, "U_DOCASO");

            oColumn = oColumns.Add("co_estSii", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Estado SII";
            oColumn.Editable = false;
            oColumn.Width = 160;
            oColumn.DataBind.Bind(NombreDT, "U_ESTSII");

            oColumn = oColumns.Add("co_estCome", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Estado Comercial";
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 160;
            oColumn.DataBind.Bind(NombreDT, "U_ESTCOME");

            oColumn = oColumns.Add("co_fechaRe", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Fecha";
            oColumn.Editable = false;
            oColumn.Visible = false;
            oColumn.Width = 60;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "U_FECHARE");

            oColumn = oColumns.Add("co_tipo", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Tipo";
            oColumn.Editable = false;
            oColumn.Visible = false;
            oColumn.Width = 60;
            oColumn.DataBind.Bind(NombreDT, "U_TIPO");

            oColumn = oColumns.Add("co_codSii", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Código SII";
            oColumn.Editable = false;
            oColumn.Visible = false;
            oColumn.Width = 60;
            oColumn.DataBind.Bind(NombreDT, "U_CODSII");

            oColumn = oColumns.Add("co_tieneNc", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Tiene NC";
            oColumn.Editable = false;
            oColumn.Visible = false;
            oColumn.Width = 60;
            oColumn.DataBind.Bind(NombreDT, "U_TIENENC");

            oColumn = oColumns.Add("co_tieneNd", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Tiene ND";
            oColumn.Editable = false;
            oColumn.Visible = false;
            oColumn.Width = 60;
            oColumn.DataBind.Bind(NombreDT, "U_TIENEND");

            oColumn = oColumns.Add("co_tipoAso", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Tipo documento asociado";
            oColumn.Editable = false;
            oColumn.Visible = false;
            oColumn.Width = 60;
            oColumn.DataBind.Bind(NombreDT, "U_TIPOASO");

            oColumn = oColumns.Add("co_razonRe", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Razón reparo o rechazo";
            oColumn.Editable = false;
            oColumn.Visible = false;
            oColumn.Width = 250;
            oColumn.DataBind.Bind(NombreDT, "U_RAZONRE");

            oColumn = oColumns.Add("co_estado", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Estado";
            oColumn.Editable = false;
            oColumn.Visible = false;
            oColumn.Width = 60;
            oColumn.DataBind.Bind(NombreDT, "U_ESTADO");

            oColumn = oColumns.Add("co_obs", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Observaciones";
            oColumn.Editable = false;
            oColumn.Visible = false;
            oColumn.Width = 250;
            oColumn.DataBind.Bind(NombreDT, "U_OBS");

            oColumn = oColumns.Add("co_prelim", SAPbouiCOM.BoFormItemTypes.it_LINKED_BUTTON);
            oColumn.TitleObject.Caption = "Preliminar";
            oColumn.Editable = false;
            oColumn.Visible = false;
            oColumn.Width = 60;
            oColumn.DataBind.Bind(NombreDT, "U_PRELIM");
        }

        /// <summary>
        /// Carga documentos en una Matrix determinada desde API proveedor
        /// </summary>
        /// <param name="oMatrix"></param>
        /// <param name="oForm"></param>
        private static void CargarDocumentosDesdeAPI(SAPbouiCOM.Matrix oMatrix, SAPbouiCOM.Form oForm)
        {
            Application.SBO_Application.StatusBar.SetText("Descargando información. Espere unos momentos.", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);
            oForm.Freeze(true);

            // Datos para api  
            Int64?[] ArraysFolio = new Int64?[] { };
            String FechaFinal = String.Empty;
            String FechaInicial = String.Empty;
            String RutEmisorDTE = String.Empty;
            String TipoDte = String.Empty;
            String FolioDte = String.Empty;
            String FechaEmisDte = String.Empty;
            Int64 lMontoTotalDte = 0;
            String MontoTotalDte = String.Empty;
            String RazonSocialDTE = String.Empty;
            String Estado = String.Empty;
            Int32 NumeroPaginas = 0;
            Int32 NumeroDocumentos = 0;
            Int32 IndexMatrix = 0;
            String DteID = String.Empty;
            String OC = String.Empty;

            // Filtro por Tipo de Documento
            SAPbouiCOM.ComboBox oCombobox = (SAPbouiCOM.ComboBox)oForm.Items.Item("cmbTipoD").Specific;
            String FiltroTipo = oCombobox.Selected.Value;

            // Filtro por Socio de Negocio
            SAPbouiCOM.EditText oRut = (SAPbouiCOM.EditText)oForm.Items.Item("txtRut").Specific;
            String FiltroSN = oRut.Value;

            //RutEmisorDTE = SBO.ConsultasSBO.ObtenerRutEmpresaEmisora();
            RutEmisorDTE = ExtConf.Parametros.Rut_Emisor;

            // Filtro por Fechas
            SAPbouiCOM.EditText oFDesde = (SAPbouiCOM.EditText)oForm.Items.Item("txtFDesde").Specific;
            String DesdeFecha = oFDesde.Value;
            SAPbouiCOM.EditText oFHasta = (SAPbouiCOM.EditText)oForm.Items.Item("txtFHasta").Specific;
            String HastaFecha = oFHasta.Value;

            SAPbouiCOM.DataTable dtDoc = oForm.DataSources.DataTables.Item("DOCUMENTOSDE");
            // Limpiar Matrix
            dtDoc.Rows.Clear();
            oMatrix.Clear();

            System.Globalization.CultureInfo pesosChilenos2decimales = System.Globalization.CultureInfo.CreateSpecificCulture("es-CL");
            pesosChilenos2decimales.NumberFormat.CurrencyDecimalDigits = 2;
            pesosChilenos2decimales.NumberFormat.CurrencySymbol = "$";

            ProveedorDTE proveedorDTE = new ProveedorDTE();

            string[] parametros = new string[] { FiltroTipo, RutEmisorDTE, FiltroSN, DesdeFecha, HastaFecha, "1" };

            var provResult = proveedorDTE.ObtenerDocumentosEmitidos(parametros);

            if (provResult.Success)
            {
                var _Datos = proveedorDTE.DTEResponse;

                foreach (var item in _Datos.Documentos)
                {
                    AgregarRegistroADataTableDesdeObjeto(dtDoc, ref IndexMatrix, item);
                    IndexMatrix++;
                }

                for (int i = 2; i <= _Datos.TotalPaginas; i++)
                {
                    parametros = new string[] { FiltroTipo, RutEmisorDTE, FiltroSN, DesdeFecha, HastaFecha, i.ToString() };
                    provResult = proveedorDTE.ObtenerDocumentosEmitidos(parametros);

                    if (provResult.Success)
                    {
                        _Datos = proveedorDTE.DTEResponse;

                        foreach (var item in _Datos.Documentos)
                        {
                            AgregarRegistroADataTableDesdeObjeto(dtDoc, ref IndexMatrix, item);
                            IndexMatrix++;
                        }
                    }
                }
                oMatrix.LoadFromDataSource();
                Application.SBO_Application.StatusBar.SetText("Información descargada correctamente.", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success);
            }
            else
            {
                Application.SBO_Application.StatusBar.SetText(provResult.Mensaje, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);
            }
            oForm.Freeze(false);
        }

        /// <summary>
        /// Agrega registros a un DataTable desde una clase Documento
        /// </summary>
        /// <param name="dtDoc"></param>
        /// <param name="IndexMatrix"></param>
        /// <param name="item"></param>
        private static void AgregarRegistroADataTableDesdeObjeto(SAPbouiCOM.DataTable dtDoc, ref Int32 IndexMatrix, ClasesDTE.DocuDTE item)
        {
            try
            {
                string sPath = null;
                sPath = System.Windows.Forms.Application.StartupPath.ToString();
                sPath += "\\";
                sPath = sPath + "pdf.jpg";

                // validación si documento existe
                string DocId = string.Empty;
                DocId = item.DocId;
                SAPbobsCOM.Recordset oRecord = (SAPbobsCOM.Recordset)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                string _query = @"SELECT COUNT(1) AS ""RESP"" FROM ""@SO_MONITOR"" WHERE ""U_DOCID"" = '" + DocId + "'";
                oRecord.DoQuery(_query);
                if (!oRecord.EoF)
                {
                    if (oRecord.Fields.Item("RESP").Value.Equals(0))
                    {
                        dtDoc.Rows.Add();
                        //dtDoc.SetValue("DocEntry", IndexMatrix, IndexMatrix + 1);
                        dtDoc.SetValue("co_num", IndexMatrix, IndexMatrix + 1);
                        dtDoc.SetValue("co_pdf", IndexMatrix, sPath);
                        dtDoc.SetValue("co_febosId", IndexMatrix, item.DocId);
                        dtDoc.SetValue("co_tipoDoc", IndexMatrix, item.TipoDocumento);
                        dtDoc.SetValue("co_folio", IndexMatrix, item.Folio);
                        dtDoc.SetValue("co_fechaEm", IndexMatrix, item.FechaEmision);
                        dtDoc.SetValue("co_formaPa", IndexMatrix, string.IsNullOrEmpty(item.FormaDePago.ToString()) ? 0 : item.FormaDePago);
                        dtDoc.SetValue("co_iDeTras", IndexMatrix, string.IsNullOrEmpty(item.IndicadorDeTraslado.ToString()) ? 0 : item.IndicadorDeTraslado);
                        dtDoc.SetValue("co_rutEmis", IndexMatrix, item.RutEmisor);
                        dtDoc.SetValue("co_razSocE", IndexMatrix, item.RazonSocialEmisor);
                        dtDoc.SetValue("co_rutRece", IndexMatrix, item.RutReceptor);
                        dtDoc.SetValue("co_razSocR", IndexMatrix, item.RazonSocialReceptor);
                        dtDoc.SetValue("co_contact", IndexMatrix, string.IsNullOrEmpty(item.Contacto) ? "" : item.Contacto);
                        var iva = string.IsNullOrEmpty(item.Iva.ToString()) ? 0 : item.Iva;
                        dtDoc.SetValue("co_iva", IndexMatrix, String.Format(System.Globalization.CultureInfo.GetCultureInfo("es-CL"), "{0:C2}", iva));
                        //dtDoc.SetValue("co_montoTo", IndexMatrix, item.MontoTotal.ToString("C2", pesosChilenos2decimales));
                        var montototal = string.IsNullOrEmpty(item.MontoTotal.ToString()) ? 0 : item.MontoTotal;
                        dtDoc.SetValue("co_montoTo", IndexMatrix, String.Format(System.Globalization.CultureInfo.GetCultureInfo("es-CL"), "{0:C2}", montototal));
                        dtDoc.SetValue("co_estSii", IndexMatrix, Comun.Enumeradores.GetEstadoSii((Comun.Enumeradores.EstadosSii)item.EstadoSii));
                        dtDoc.SetValue("co_estCome", IndexMatrix, Comun.Enumeradores.GetEstadoComercial((Comun.Enumeradores.EstadosComerciales)item.EstadoComercial));
                        dtDoc.SetValue("co_fechaRe", IndexMatrix, item.FechaRecepcion);
                        dtDoc.SetValue("co_tipo", IndexMatrix, item.Tipo);
                        dtDoc.SetValue("co_codSii", IndexMatrix, string.IsNullOrEmpty(item.CodigoSii) ? "" : item.CodigoSii);
                        dtDoc.SetValue("co_tieneNc", IndexMatrix, item.TieneNc);
                        dtDoc.SetValue("co_tieneNd", IndexMatrix, item.TieneNd);
                        dtDoc.SetValue("co_docBase", IndexMatrix, "");
                        dtDoc.SetValue("co_tipoAso", IndexMatrix, "");
                        dtDoc.SetValue("co_docAso", IndexMatrix, "");
                        dtDoc.SetValue("co_razonRe", IndexMatrix, "");
                        dtDoc.SetValue("co_estado", IndexMatrix, "0");
                        dtDoc.SetValue("co_prelim", IndexMatrix, "");
                    }
                    else
                    {
                        IndexMatrix--;
                    }
                }
                else
                {
                    IndexMatrix--;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(IndexMatrix.ToString());
            }
        }

        /// <summary>
        /// Marca o desmarca los registros de una Matrix determinada
        /// </summary>
        /// <param name="oMatrix"></param>
        /// <param name="dtDoc"></param>
        /// <param name="columna"></param>
        private static void MarcarRegistrosMatrix(SAPbouiCOM.Matrix oMatrix, SAPbouiCOM.DataTable dtDoc, string columna)
        {
            if (oMatrix.VisualRowCount > 0)
            {
                oForm.Freeze(true);
                string accion = "Y";
                for (int i = 0; i < dtDoc.Rows.Count; i++)
                {
                    if (i == 0 && dtDoc.GetValue(columna, i).ToString().Equals("Y"))
                    {
                        accion = "N";
                    }
                    dtDoc.SetValue(columna, i, accion);
                }
                oMatrix.LoadFromDataSource();
                oForm.Freeze(false);
            }
        }

        /// <summary>
        /// Visualiza PDF de documento de la matrix correspondiente
        /// </summary>
        /// <param name="dtDoc"></param>
        /// <param name="Row"></param>
        private static void VisualizarPdf(SAPbouiCOM.DataTable dtDoc, int Row)
        {
            Application.SBO_Application.StatusBar.SetText("Importando Pdf del documento. Espere unos momentos.", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);

            try
            {
                // Obtener datos necesarios de DataTable
                string docId = dtDoc.GetValue("co_docId", Row).ToString();

                ProveedorDTE proveedorDTE = new ProveedorDTE();

                string[] parametros = new string[] { docId };

                //////var provResult = proveedorDTE.ObtenerPDFDocumento(parametros);
                //////if (provResult.Success)
                //////{
                //////    var _Datos = proveedorDTE.DetalleDocuDTE;
                //////    String link = _Datos.ImagenLink.ToString();
                //////    SAPbobsCOM.CompanyService com_service = SBO.ConexionSBO.oCompany.GetCompanyService();
                //////    SAPbobsCOM.PathAdmin oPathAdmin = com_service.GetPathAdmin();
                //////    String Path = string.Empty;
                //////    Path = oPathAdmin.AttachmentsFolderPath;
                //////    link = link.Replace("AttachmentsFolderPath:/", Path);

                //////    // Visualizar con chrome
                //////    //System.Diagnostics.Process.Start("chrome.exe", link);

                //////    // Visualizar con navegador por defecto
                //////    System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo();
                //////    info.FileName = link;
                //////    info.Verb = "open";
                //////    info.WindowStyle = System.Diagnostics.ProcessWindowStyle.Maximized;
                //////    System.Diagnostics.Process.Start(info);
                //////}
                //////// Validación obtención de documento
                //////else
                //////{
                //////    Application.SBO_Application.StatusBar.SetText(provResult.Mensaje, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);
                //////}
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void ActualizarEstados(SAPbouiCOM.Matrix oMatrix, SAPbouiCOM.DataTable dtDoc, string columna)
        {
            if (oMatrix.VisualRowCount > 0)
            {
                oForm.Freeze(true);
                oMatrix.FlushToDataSource();
                for (int i = 0; i < dtDoc.Rows.Count; i++)
                {
                    string check = dtDoc.GetValue(columna, i).ToString();
                    if (check.Equals("Y"))
                    {
                        string tipodoc = dtDoc.GetValue("TipoDoc", i).ToString();
                        string folio = dtDoc.GetValue("FolioNum", i).ToString();
                        ClasesDTE.EstadosEmision estado = new ClasesDTE.EstadosEmision();
                        switch (tipodoc)
                        {
                            case "FE":
                                estado = ObtenerEstado("33", folio);
                                break;
                            case "FN":
                                estado = ObtenerEstado("34", folio);
                                break;
                            case "BE":
                                estado = ObtenerEstado("39", folio);
                                break;
                            case "BN":
                                estado = ObtenerEstado("41", folio);
                                break;
                            case "ND":
                                estado = ObtenerEstado("56", folio);
                                break;
                            case "NC":
                                estado = ObtenerEstado("61", folio);
                                break;
                            case "GE":
                                estado = ObtenerEstado("52", folio);
                                break;
                            case "GT":
                                estado = ObtenerEstado("52", folio);
                                break;
                            case "FEX":
                                estado = ObtenerEstado("110", folio);
                                break;
                            case "NDX":
                                estado = ObtenerEstado("111", folio);
                                break;
                            case "NCX":
                                estado = ObtenerEstado("112", folio);
                                break;
                            default:
                                break;
                        }
                        dtDoc.SetValue("Estado1", i, estado.Estado1);
                        dtDoc.SetValue("MEstado1", i, estado.MsgEstado1);
                        dtDoc.SetValue("Estado2", i, estado.Estado2);
                        dtDoc.SetValue("MEstado2", i, estado.MsgEstado2);
                        dtDoc.SetValue("Estado3", i, estado.Estado3);
                        dtDoc.SetValue("MEstado3", i, estado.MsgEstado3);
                    }
                }

                oMatrix.LoadFromDataSource();
                oForm.Freeze(false);
            }
        }

        private static ClasesDTE.EstadosEmision ObtenerEstado(string tipodoc, string folio)
        {
            ClasesDTE.EstadosEmision resp = new Clases.DTE.EstadosEmision();
            ProveedorDTE proveedorDTE = new ProveedorDTE();
            string[] parametros = new string[] { tipodoc, folio };
            var result = proveedorDTE.ObtenerEstado(parametros);
            if (result.Success)
            {
                var respConsultaEstado = proveedorDTE.EstadosEmision;
                resp = respConsultaEstado;
            }
            else
            {
                resp.Estado1 = "ERROR";
                resp.MsgEstado1 = result.Mensaje;
            }
            return resp;
        }
    }
}
