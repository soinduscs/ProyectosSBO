using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM.Framework;

namespace Soindus.AddOnEmisionDTE.Formularios
{
    [FormAttribute("Soindus.AddOnEmisionDTE.Formularios.frmGeneradorDTE", "Formularios/frmGeneradorDTE.b1f")]
    class frmGeneradorDTE : UserFormBase
    {
        private static Local.Configuracion ExtConf = new Local.Configuracion();
        // Objetos de formulario
        #region Objetos de formulario
        private static SAPbouiCOM.Form oForm;
        private static SAPbouiCOM.EditText txtDesde;
        private static SAPbouiCOM.EditText txtHasta;
        private static SAPbouiCOM.ComboBox cbxTipo;
        private static SAPbouiCOM.Button btnFiltro;
        private static SAPbouiCOM.Folder tab01;
        private static SAPbouiCOM.Matrix mtxDocs;
        private static SAPbouiCOM.Button btnTodos;
        private static SAPbouiCOM.Button btnDTE;
        #endregion

        public frmGeneradorDTE()
        {
            AsignarObjetos();
            CargarFormulario();
            ExtConf = new Local.Configuracion();
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
            try
            {
                if (pVal.BeforeAction.Equals(true))
                {
                    // Link documento base en matrix
                    #region link documento base en matrix
                    if (pVal.EventType.Equals(SAPbouiCOM.BoEventTypes.et_MATRIX_LINK_PRESSED))
                    {
                        // Matrix de documentos no foliados
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
                        // Boton actualizar
                        #region actualizar
                        if (pVal.ItemUID.Equals("btnFiltro"))
                        {
                            // Documentos sin folio
                            if (tab01.Selected)
                            {
                                CargarMatrixDocumentosSinFolio();
                                Application.SBO_Application.StatusBar.SetText("Documentos cargados correctamente.", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success);
                            }
                        }
                        #endregion

                        // Boton marcar
                        #region marcar
                        if (pVal.ItemUID.Equals("btnTodos"))
                        {
                            // Documentos sin folio
                            if (tab01.Selected)
                            {
                                string NombreDT = "dtDocs";
                                SAPbouiCOM.DataTable datatable = oForm.DataSources.DataTables.Item(NombreDT);
                                MarcarRegistrosMatrix(mtxDocs, datatable, "Chk");
                            }

                        }
                        #endregion

                        // Boton procesar DTE
                        #region validar
                        if (pVal.ItemUID.Equals("btnDTE"))
                        {
                            // Genera DTE de documentos seleccionados
                            if (tab01.Selected)
                            {
                                mtxDocs.FlushToDataSource();
                                string NombreDT = "dtDocs";
                                SAPbouiCOM.DataTable datatable = oForm.DataSources.DataTables.Item(NombreDT);
                                ProcesarDocumentos(mtxDocs, datatable, "Chk");
                            }
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

        private void AsignarObjetos()
        {
            oForm = ((SAPbouiCOM.Form)(Application.SBO_Application.Forms.Item("frmGeneradorDTE")));
            txtDesde = ((SAPbouiCOM.EditText)(GetItem("txtDesde").Specific));
            txtHasta = ((SAPbouiCOM.EditText)(GetItem("txtHasta").Specific));
            cbxTipo = ((SAPbouiCOM.ComboBox)(GetItem("cbxTipo").Specific));
            btnFiltro = ((SAPbouiCOM.Button)(GetItem("btnFiltro").Specific));
            tab01 = ((SAPbouiCOM.Folder)(GetItem("tab01").Specific));
            tab01.Select();
            mtxDocs = ((SAPbouiCOM.Matrix)(GetItem("mtxDocs").Specific));
            btnTodos = ((SAPbouiCOM.Button)(GetItem("btnTodos").Specific));
            btnDTE = ((SAPbouiCOM.Button)(GetItem("btnDTE").Specific));
        }

        private void CargarFormulario()
        {
            // Fecha desde
            oForm.DataSources.UserDataSources.Add("FDesde", SAPbouiCOM.BoDataType.dt_DATE);
            txtDesde.DataBind.SetBound(true, "", "FDesde");

            // Fecha hasta
            oForm.DataSources.UserDataSources.Add("FHasta", SAPbouiCOM.BoDataType.dt_DATE);
            txtHasta.DataBind.SetBound(true, "", "FHasta");

            // Tipo documento
            cbxTipo.ValidValues.Add("", "");
            cbxTipo.ValidValues.Add("FE", "Facturas");
            cbxTipo.ValidValues.Add("FN", "Facturas Exentas");
            cbxTipo.ValidValues.Add("BE", "Boletas");
            cbxTipo.ValidValues.Add("BN", "Boletas Exentas");
            cbxTipo.ValidValues.Add("ND", "Notas de Débito");
            cbxTipo.ValidValues.Add("NC", "Notas de Crédito");
            cbxTipo.ValidValues.Add("GE", "Guías (Entregas)");
            cbxTipo.ValidValues.Add("GT", "Guías (Traslados)");
            cbxTipo.ValidValues.Add("FEX", "Facturas Exportación");
            cbxTipo.ValidValues.Add("NDX", "Notas de Débito Exportación");
            cbxTipo.ValidValues.Add("NCX", "Notas de Crédito Exportación");

            oForm.DataSources.DataTables.Add("dtDocs");

            EstructuraMatrixDocumentosSinFolio();

            //CargarMatrixRendiciones();

            mtxDocs.SelectionMode = SAPbouiCOM.BoMatrixSelect.ms_Single;
            oForm.Visible = true;
        }

        private static void EstructuraMatrixDocumentosSinFolio()
        {
            string NombreDT = "dtDocs";
            SAPbouiCOM.DataTable datatable = oForm.DataSources.DataTables.Item(NombreDT);
            string _query = @"
                SELECT 'N' AS ""Chk"",
                ""ObjType"" AS ""ObjType"",
                ""DocEntry"" AS ""DocEntry"",
                'FE' AS ""TipoDoc"",
                ""DocNum"" AS ""DocNum"",
                ""isIns"" AS ""isIns"",
                ""Indicator"" AS ""Indicator"",
                ""DocDate"" AS ""DocDate"",
                ""LicTradNum"" AS ""LicTradNum"",
                ""CardCode"" AS ""CardCode"",
                ""CardName"" AS ""CardName"",
                REPLICATE(' ', 250) AS ""Obs"",
                0 AS ""StatusDTE"",
                0 AS ""StatusPDF""
                FROM ""OINV"" WHERE 1 = 0";
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
            oColumn.TitleObject.Caption = "Folio ERP";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 60;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "DocNum");

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

            oColumn = oColumns.Add("Obs", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Comentario";
            oColumn.TitleObject.Sortable = false;
            oColumn.Editable = false;
            oColumn.Visible = true;
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

            mtxDocs.Clear();
            mtxDocs.LoadFromDataSourceEx();
        }

        private static void CargarMatrixDocumentosSinFolio()
        {
            // Filtro por Fechas
            SAPbouiCOM.EditText oFDesde = (SAPbouiCOM.EditText)oForm.Items.Item("txtDesde").Specific;
            string DesdeFecha = oFDesde.Value;
            SAPbouiCOM.EditText oFHasta = (SAPbouiCOM.EditText)oForm.Items.Item("txtHasta").Specific;
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
                // Por defecto trae los últimos 7 días
                dt = new DateTime(DateTime.Now.Year, DateTime.Today.Month, DateTime.Today.Day);
                Mes = (dt.Month.ToString().Length == 1) ? dt.Month.ToString().PadLeft(2, '0') : dt.Month.ToString();
                Dia = (dt.Day.ToString().Length == 1) ? dt.Day.ToString().PadLeft(2, '0') : dt.Day.ToString();

                FechaFinal = string.Format("{0}{1}{2}", dt.Year.ToString(), Mes, Dia);

                dt = DateTime.Today.AddDays(-7);
                Mes = (dt.Month.ToString().Length == 1) ? dt.Month.ToString().PadLeft(2, '0') : dt.Month.ToString();
                Dia = (dt.Day.ToString().Length == 1) ? dt.Day.ToString().PadLeft(2, '0') : dt.Day.ToString();

                FechaInicial = string.Format("{0}{1}{2}", dt.Year.ToString(), Mes, Dia);
            }

            string FiltroFecha = string.Format(@"AND ""DocDate"" BETWEEN '{0}' AND '{1}'", FechaInicial, FechaFinal);

            // Filtro por Tipo de Documento
            SAPbouiCOM.ComboBox oCombobox = (SAPbouiCOM.ComboBox)oForm.Items.Item("cbxTipo").Specific;
            string FiltroTipo = oCombobox.Selected.Value;
            string FiltroFolio = string.Empty;
            string FiltroEmiteGuia = string.Empty;
            if (SBO.ConexionSBO.oCompany.DbServerType.Equals(SAPbobsCOM.BoDataServerTypes.dst_HANADB))
            {
                FiltroFolio = @"AND IFNULL(""FolioNum"", 0) = 0 ";
            }
            else
            {
                FiltroFolio = @"AND ISNULL(""FolioNum"", 0) = 0 ";
            }
            if (ExtConf.Parametros.Proveedor_FE.Equals("DBN-EV"))
            {
                FiltroEmiteGuia = @"""U_SO_EMITEGUIA"" = '1' ";
            }
            else
            {
                FiltroEmiteGuia = @"'1' = '1' ";
            }

            string _query = @"
                SELECT 'N' AS ""Chk"",
                ""ObjType"" AS ""ObjType"",
                ""DocEntry"" AS ""DocEntry"",
                'FE' AS ""TipoDoc"",
                ""DocNum"" AS ""DocNum"",
                ""isIns"" AS ""isIns"",
                ""Indicator"" AS ""Indicator"",
                ""DocDate"" AS ""DocDate"",
                ""LicTradNum"" AS ""LicTradNum"",
                ""CardCode"" AS ""CardCode"",
                ""CardName"" AS ""CardName"",
                REPLICATE(' ', 250) AS ""Obs"",
                0 AS ""StatusDTE"",
                0 AS ""StatusPDF""
                FROM ""OINV"" WHERE ""DocSubType"" = '--' AND ""isIns"" = 'N' " + FiltroFolio + FiltroFecha;
            _query += @"
                UNION ALL
                SELECT 'N' AS ""Chk"",
                ""ObjType"" AS ""ObjType"",
                ""DocEntry"" AS ""DocEntry"",
                'FE' AS ""TipoDoc"",
                ""DocNum"" AS ""DocNum"",
                ""isIns"" AS ""isIns"",
                ""Indicator"" AS ""Indicator"",
                ""DocDate"" AS ""DocDate"",
                ""LicTradNum"" AS ""LicTradNum"",
                ""CardCode"" AS ""CardCode"",
                ""CardName"" AS ""CardName"",
                REPLICATE(' ', 250) AS ""Obs"",
                0 AS ""StatusDTE"",
                0 AS ""StatusPDF""
                FROM ""OINV"" WHERE ""DocSubType"" = '--' AND ""isIns"" = 'Y' AND ""Indicator"" <> '39' " + FiltroFolio + FiltroFecha;
            _query += @"
                UNION ALL
                SELECT 'N' AS ""Chk"",
                ""ObjType"" AS ""ObjType"",
                ""DocEntry"" AS ""DocEntry"",
                'FN' AS ""TipoDoc"",
                ""DocNum"" AS ""DocNum"",
                ""isIns"" AS ""isIns"",
                ""Indicator"" AS ""Indicator"",
                ""DocDate"" AS ""DocDate"",
                ""LicTradNum"" AS ""LicTradNum"",
                ""CardCode"" AS ""CardCode"",
                ""CardName"" AS ""CardName"",
                REPLICATE(' ', 250) AS ""Obs"",
                0 AS ""StatusDTE"",
                0 AS ""StatusPDF""
                FROM ""OINV"" WHERE ""DocSubType"" = 'IE' " + FiltroFolio + FiltroFecha;
            _query += @"
                UNION ALL
                SELECT 'N' AS ""Chk"",
                ""ObjType"" AS ""ObjType"",
                ""DocEntry"" AS ""DocEntry"",
                'BE' AS ""TipoDoc"",
                ""DocNum"" AS ""DocNum"",
                ""isIns"" AS ""isIns"",
                ""Indicator"" AS ""Indicator"",
                ""DocDate"" AS ""DocDate"",
                ""LicTradNum"" AS ""LicTradNum"",
                ""CardCode"" AS ""CardCode"",
                ""CardName"" AS ""CardName"",
                REPLICATE(' ', 250) AS ""Obs"",
                0 AS ""StatusDTE"",
                0 AS ""StatusPDF""
                FROM ""OINV"" WHERE ""DocSubType"" = 'IB' " + FiltroFolio + FiltroFecha;
            _query += @"
                UNION ALL
                SELECT 'N' AS ""Chk"",
                ""ObjType"" AS ""ObjType"",
                ""DocEntry"" AS ""DocEntry"",
                'BE' AS ""TipoDoc"",
                ""DocNum"" AS ""DocNum"",
                ""isIns"" AS ""isIns"",
                ""Indicator"" AS ""Indicator"",
                ""DocDate"" AS ""DocDate"",
                ""LicTradNum"" AS ""LicTradNum"",
                ""CardCode"" AS ""CardCode"",
                ""CardName"" AS ""CardName"",
                REPLICATE(' ', 250) AS ""Obs"",
                0 AS ""StatusDTE"",
                0 AS ""StatusPDF""
                FROM ""OINV"" WHERE ""DocSubType"" = '--' AND ""isIns"" = 'Y' AND ""Indicator"" = '39' " + FiltroFolio + FiltroFecha;
            _query += @"
                UNION ALL
                SELECT 'N' AS ""Chk"",
                ""ObjType"" AS ""ObjType"",
                ""DocEntry"" AS ""DocEntry"",
                'BN' AS ""TipoDoc"",
                ""DocNum"" AS ""DocNum"",
                ""isIns"" AS ""isIns"",
                ""Indicator"" AS ""Indicator"",
                ""DocDate"" AS ""DocDate"",
                ""LicTradNum"" AS ""LicTradNum"",
                ""CardCode"" AS ""CardCode"",
                ""CardName"" AS ""CardName"",
                REPLICATE(' ', 250) AS ""Obs"",
                0 AS ""StatusDTE"",
                0 AS ""StatusPDF""
                FROM ""OINV"" WHERE ""DocSubType"" = 'EB' " + FiltroFolio + FiltroFecha;
            _query += @"
                UNION ALL
                SELECT 'N' AS ""Chk"",
                ""ObjType"" AS ""ObjType"",
                ""DocEntry"" AS ""DocEntry"",
                'ND' AS ""TipoDoc"",
                ""DocNum"" AS ""DocNum"",
                ""isIns"" AS ""isIns"",
                ""Indicator"" AS ""Indicator"",
                ""DocDate"" AS ""DocDate"",
                ""LicTradNum"" AS ""LicTradNum"",
                ""CardCode"" AS ""CardCode"",
                ""CardName"" AS ""CardName"",
                REPLICATE(' ', 250) AS ""Obs"",
                0 AS ""StatusDTE"",
                0 AS ""StatusPDF""
                FROM ""OINV"" WHERE ""DocSubType"" = 'DN' AND ""Indicator"" <> '11' " + FiltroFolio + FiltroFecha;
            _query += @"
                UNION ALL
                SELECT 'N' AS ""Chk"",
                ""ObjType"" AS ""ObjType"",
                ""DocEntry"" AS ""DocEntry"",
                'NC' AS ""TipoDoc"",
                ""DocNum"" AS ""DocNum"",
                '' AS ""isIns"",
                '' AS ""Indicator"",
                ""DocDate"" AS ""DocDate"",
                ""LicTradNum"" AS ""LicTradNum"",
                ""CardCode"" AS ""CardCode"",
                ""CardName"" AS ""CardName"",
                REPLICATE(' ', 250) AS ""Obs"",
                0 AS ""StatusDTE"",
                0 AS ""StatusPDF""
                FROM ""ORIN"" WHERE ""Indicator"" <> '12' " + FiltroFolio + FiltroFecha;
            _query += @"
                UNION ALL
                SELECT 'N' AS ""Chk"",
                ""ObjType"" AS ""ObjType"",
                ""DocEntry"" AS ""DocEntry"",
                'GE' AS ""TipoDoc"",
                ""DocNum"" AS ""DocNum"",
                '' AS ""isIns"",
                '' AS ""Indicator"",
                ""DocDate"" AS ""DocDate"",
                ""LicTradNum"" AS ""LicTradNum"",
                ""CardCode"" AS ""CardCode"",
                ""CardName"" AS ""CardName"",
                REPLICATE(' ', 250) AS ""Obs"",
                0 AS ""StatusDTE"",
                0 AS ""StatusPDF""
                FROM ""ODLN"" WHERE " + FiltroEmiteGuia + FiltroFolio + FiltroFecha;
            _query += @"
                UNION ALL
                SELECT 'N' AS ""Chk"",
                ""ObjType"" AS ""ObjType"",
                ""DocEntry"" AS ""DocEntry"",
                'GT' AS ""TipoDoc"",
                ""DocNum"" AS ""DocNum"",
                '' AS ""isIns"",
                '' AS ""Indicator"",
                ""DocDate"" AS ""DocDate"",
                ""LicTradNum"" AS ""LicTradNum"",
                ""CardCode"" AS ""CardCode"",
                ""CardName"" AS ""CardName"",
                REPLICATE(' ', 250) AS ""Obs"",
                0 AS ""StatusDTE"",
                0 AS ""StatusPDF""
                FROM ""OWTR"" WHERE " + FiltroEmiteGuia + FiltroFolio + FiltroFecha;
            _query += @"
                UNION ALL
                SELECT 'N' AS ""Chk"",
                ""ObjType"" AS ""ObjType"",
                ""DocEntry"" AS ""DocEntry"",
                'FEX' AS ""TipoDoc"",
                ""DocNum"" AS ""DocNum"",
                ""isIns"" AS ""isIns"",
                ""Indicator"" AS ""Indicator"",
                ""DocDate"" AS ""DocDate"",
                ""LicTradNum"" AS ""LicTradNum"",
                ""CardCode"" AS ""CardCode"",
                ""CardName"" AS ""CardName"",
                REPLICATE(' ', 250) AS ""Obs"",
                0 AS ""StatusDTE"",
                0 AS ""StatusPDF""
                FROM ""OINV"" WHERE ""DocSubType"" = 'IX' " + FiltroFolio + FiltroFecha;
            _query += @"
                UNION ALL
                SELECT 'N' AS ""Chk"",
                ""ObjType"" AS ""ObjType"",
                ""DocEntry"" AS ""DocEntry"",
                'NDX' AS ""TipoDoc"",
                ""DocNum"" AS ""DocNum"",
                ""isIns"" AS ""isIns"",
                ""Indicator"" AS ""Indicator"",
                ""DocDate"" AS ""DocDate"",
                ""LicTradNum"" AS ""LicTradNum"",
                ""CardCode"" AS ""CardCode"",
                ""CardName"" AS ""CardName"",
                REPLICATE(' ', 250) AS ""Obs"",
                0 AS ""StatusDTE"",
                0 AS ""StatusPDF""
                FROM ""OINV"" WHERE ""DocSubType"" = 'DN' AND ""Indicator"" = '11' " + FiltroFolio + FiltroFecha;
            _query += @"
                UNION ALL
                SELECT 'N' AS ""Chk"",
                ""ObjType"" AS ""ObjType"",
                ""DocEntry"" AS ""DocEntry"",
                'NCX' AS ""TipoDoc"",
                ""DocNum"" AS ""DocNum"",
                '' AS ""isIns"",
                '' AS ""Indicator"",
                ""DocDate"" AS ""DocDate"",
                ""LicTradNum"" AS ""LicTradNum"",
                ""CardCode"" AS ""CardCode"",
                ""CardName"" AS ""CardName"",
                REPLICATE(' ', 250) AS ""Obs"",
                0 AS ""StatusDTE"",
                0 AS ""StatusPDF""
                FROM ""ORIN"" WHERE ""Indicator"" = '12' " + FiltroFolio + FiltroFecha;

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
                            ""isIns"" AS ""isIns"",
                            ""Indicator"" AS ""Indicator"",
                            ""DocDate"" AS ""DocDate"",
                            ""LicTradNum"" AS ""LicTradNum"",
                            ""CardCode"" AS ""CardCode"",
                            ""CardName"" AS ""CardName"",
                            REPLICATE(' ', 250) AS ""Obs"",
                            0 AS ""StatusDTE"",
                            0 AS ""StatusPDF""
                            FROM ""OINV"" WHERE ""DocSubType"" = '--' AND ""Indicator"" <> '39' " + FiltroFolio + FiltroFecha;
                        break;
                    case "FN":
                        _query = @"
                            SELECT 'N' AS ""Chk"",
                            ""ObjType"" AS ""ObjType"",
                            ""DocEntry"" AS ""DocEntry"",
                            'FN' AS ""TipoDoc"",
                            ""DocNum"" AS ""DocNum"",
                            ""isIns"" AS ""isIns"",
                            ""Indicator"" AS ""Indicator"",
                            ""DocDate"" AS ""DocDate"",
                            ""LicTradNum"" AS ""LicTradNum"",
                            ""CardCode"" AS ""CardCode"",
                            ""CardName"" AS ""CardName"",
                            REPLICATE(' ', 250) AS ""Obs"",
                            0 AS ""StatusDTE"",
                            0 AS ""StatusPDF""
                            FROM ""OINV"" WHERE ""DocSubType"" = 'IE' " + FiltroFolio + FiltroFecha;
                        break;
                    case "BE":
                        _query = @"
                            SELECT 'N' AS ""Chk"",
                            ""ObjType"" AS ""ObjType"",
                            ""DocEntry"" AS ""DocEntry"",
                            'BE' AS ""TipoDoc"",
                            ""DocNum"" AS ""DocNum"",
                            ""isIns"" AS ""isIns"",
                            ""Indicator"" AS ""Indicator"",
                            ""DocDate"" AS ""DocDate"",
                            ""LicTradNum"" AS ""LicTradNum"",
                            ""CardCode"" AS ""CardCode"",
                            ""CardName"" AS ""CardName"",
                            REPLICATE(' ', 250) AS ""Obs"",
                            0 AS ""StatusDTE"",
                            0 AS ""StatusPDF""
                            FROM ""OINV"" WHERE ""DocSubType"" = 'IB' " + FiltroFolio + FiltroFecha;
                        _query += @"
                            UNION ALL
                            SELECT 'N' AS ""Chk"",
                            ""ObjType"" AS ""ObjType"",
                            ""DocEntry"" AS ""DocEntry"",
                            'BE' AS ""TipoDoc"",
                            ""DocNum"" AS ""DocNum"",
                            ""isIns"" AS ""isIns"",
                            ""Indicator"" AS ""Indicator"",
                            ""DocDate"" AS ""DocDate"",
                            ""LicTradNum"" AS ""LicTradNum"",
                            ""CardCode"" AS ""CardCode"",
                            ""CardName"" AS ""CardName"",
                            REPLICATE(' ', 250) AS ""Obs"",
                            0 AS ""StatusDTE"",
                            0 AS ""StatusPDF""
                            FROM ""OINV"" WHERE ""DocSubType"" = '--' AND ""isIns"" = 'Y' AND ""Indicator"" = '39' " + FiltroFolio + FiltroFecha;
                        break;
                    case "BN":
                        _query = @"
                            SELECT 'N' AS ""Chk"",
                            ""ObjType"" AS ""ObjType"",
                            ""DocEntry"" AS ""DocEntry"",
                            'BN' AS ""TipoDoc"",
                            ""DocNum"" AS ""DocNum"",
                            ""isIns"" AS ""isIns"",
                            ""Indicator"" AS ""Indicator"",
                            ""DocDate"" AS ""DocDate"",
                            ""LicTradNum"" AS ""LicTradNum"",
                            ""CardCode"" AS ""CardCode"",
                            ""CardName"" AS ""CardName"",
                            REPLICATE(' ', 250) AS ""Obs"",
                            0 AS ""StatusDTE"",
                            0 AS ""StatusPDF""
                            FROM ""OINV"" WHERE ""DocSubType"" = 'EB' " + FiltroFolio + FiltroFecha;
                        break;
                    case "ND":
                        _query = @"
                            SELECT 'N' AS ""Chk"",
                            ""ObjType"" AS ""ObjType"",
                            ""DocEntry"" AS ""DocEntry"",
                            'ND' AS ""TipoDoc"",
                            ""DocNum"" AS ""DocNum"",
                            ""isIns"" AS ""isIns"",
                            ""Indicator"" AS ""Indicator"",
                            ""DocDate"" AS ""DocDate"",
                            ""LicTradNum"" AS ""LicTradNum"",
                            ""CardCode"" AS ""CardCode"",
                            ""CardName"" AS ""CardName"",
                            REPLICATE(' ', 250) AS ""Obs"",
                            0 AS ""StatusDTE"",
                            0 AS ""StatusPDF""
                            FROM ""OINV"" WHERE ""DocSubType"" = 'DN' AND ""Indicator"" <> '11' " + FiltroFolio + FiltroFecha;
                        break;
                    case "NC":
                        _query = @"
                            SELECT 'N' AS ""Chk"",
                            ""ObjType"" AS ""ObjType"",
                            ""DocEntry"" AS ""DocEntry"",
                            'NC' AS ""TipoDoc"",
                            ""DocNum"" AS ""DocNum"",
                            '' AS ""isIns"",
                            '' AS ""Indicator"",
                            ""DocDate"" AS ""DocDate"",
                            ""LicTradNum"" AS ""LicTradNum"",
                            ""CardCode"" AS ""CardCode"",
                            ""CardName"" AS ""CardName"",
                            REPLICATE(' ', 250) AS ""Obs"",
                            0 AS ""StatusDTE"",
                            0 AS ""StatusPDF""
                            FROM ""ORIN"" WHERE ""Indicator"" <> '12' " + FiltroFolio + FiltroFecha;
                        break;
                    case "GE":
                        _query = @"
                            SELECT 'N' AS ""Chk"",
                            ""ObjType"" AS ""ObjType"",
                            ""DocEntry"" AS ""DocEntry"",
                            'GE' AS ""TipoDoc"",
                            ""DocNum"" AS ""DocNum"",
                            '' AS ""isIns"",
                            '' AS ""Indicator"",
                            ""DocDate"" AS ""DocDate"",
                            ""LicTradNum"" AS ""LicTradNum"",
                            ""CardCode"" AS ""CardCode"",
                            ""CardName"" AS ""CardName"",
                            REPLICATE(' ', 250) AS ""Obs"",
                            0 AS ""StatusDTE"",
                            0 AS ""StatusPDF""
                            FROM ""ODLN"" WHERE " + FiltroEmiteGuia + FiltroFolio + FiltroFecha;
                        break;
                    case "GT":
                        _query = @"
                            SELECT 'N' AS ""Chk"",
                            ""ObjType"" AS ""ObjType"",
                            ""DocEntry"" AS ""DocEntry"",
                            'GT' AS ""TipoDoc"",
                            ""DocNum"" AS ""DocNum"",
                            '' AS ""isIns"",
                            '' AS ""Indicator"",
                            ""DocDate"" AS ""DocDate"",
                            ""LicTradNum"" AS ""LicTradNum"",
                            ""CardCode"" AS ""CardCode"",
                            ""CardName"" AS ""CardName"",
                            REPLICATE(' ', 250) AS ""Obs"",
                            0 AS ""StatusDTE"",
                            0 AS ""StatusPDF""
                            FROM ""OWTR"" WHERE " + FiltroEmiteGuia + FiltroFolio + FiltroFecha;
                        break;
                    case "FEX":
                        _query = @"
                            SELECT 'N' AS ""Chk"",
                            ""ObjType"" AS ""ObjType"",
                            ""DocEntry"" AS ""DocEntry"",
                            'FEX' AS ""TipoDoc"",
                            ""DocNum"" AS ""DocNum"",
                            ""isIns"" AS ""isIns"",
                            ""Indicator"" AS ""Indicator"",
                            ""DocDate"" AS ""DocDate"",
                            ""LicTradNum"" AS ""LicTradNum"",
                            ""CardCode"" AS ""CardCode"",
                            ""CardName"" AS ""CardName"",
                            REPLICATE(' ', 250) AS ""Obs"",
                            0 AS ""StatusDTE"",
                            0 AS ""StatusPDF""
                            FROM ""OINV"" WHERE ""DocSubType"" = 'IX' " + FiltroFolio + FiltroFecha;
                        break;
                    case "NDX":
                        _query = @"
                            SELECT 'N' AS ""Chk"",
                            ""ObjType"" AS ""ObjType"",
                            ""DocEntry"" AS ""DocEntry"",
                            'NDX' AS ""TipoDoc"",
                            ""DocNum"" AS ""DocNum"",
                            ""isIns"" AS ""isIns"",
                            ""Indicator"" AS ""Indicator"",
                            ""DocDate"" AS ""DocDate"",
                            ""LicTradNum"" AS ""LicTradNum"",
                            ""CardCode"" AS ""CardCode"",
                            ""CardName"" AS ""CardName"",
                            REPLICATE(' ', 250) AS ""Obs"",
                            0 AS ""StatusDTE"",
                            0 AS ""StatusPDF""
                            FROM ""OINV"" WHERE ""DocSubType"" = 'DN' AND ""Indicator"" = '11' " + FiltroFolio + FiltroFecha;
                        break;
                    case "NCX":
                        _query = @"
                            SELECT 'N' AS ""Chk"",
                            ""ObjType"" AS ""ObjType"",
                            ""DocEntry"" AS ""DocEntry"",
                            'NCX' AS ""TipoDoc"",
                            ""DocNum"" AS ""DocNum"",
                            '' AS ""isIns"",
                            '' AS ""Indicator"",
                            ""DocDate"" AS ""DocDate"",
                            ""LicTradNum"" AS ""LicTradNum"",
                            ""CardCode"" AS ""CardCode"",
                            ""CardName"" AS ""CardName"",
                            REPLICATE(' ', 250) AS ""Obs"",
                            0 AS ""StatusDTE"",
                            0 AS ""StatusPDF""
                            FROM ""ORIN"" WHERE ""Indicator"" = '12' " + FiltroFolio + FiltroFecha;
                        break;
                    default:
                        break;
                }
            }

            string NombreDT = "dtDocs";
            SAPbouiCOM.DataTable datatable = oForm.DataSources.DataTables.Item(NombreDT);

            _query += @"ORDER BY ""DocDate"" DESC";
            datatable.ExecuteQuery(_query);
            oForm.Freeze(true);
            mtxDocs.Clear();
            mtxDocs.LoadFromDataSourceEx();
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
            oMatrix.LoadFromDataSource();
        }

        private static void ProcesarDocumentos(SAPbouiCOM.Matrix oMatrix, SAPbouiCOM.DataTable dtDoc, string columna)
        {
            Application.SBO_Application.StatusBar.SetText("Procesando documentos. Espere unos momentos.", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);

            string TipoDoc = string.Empty;
            string DocEntry = string.Empty;
            string DocNum = string.Empty;
            string Reserva = string.Empty;
            string Indicador = string.Empty;
            string folio = string.Empty;

            try
            {
                int regMarcados = 0;
                int regProcesados = 0;
                for (int i = 0; i < dtDoc.Rows.Count; i++)
                {
                    try
                    {
                        string procesado = dtDoc.GetValue("StatusDTE", i).ToString();
                        // sólo si check está marcado y no ha sido procesado
                        if (dtDoc.GetValue(columna, i).ToString().Equals("Y") && procesado.Equals("0"))
                        {
                            int ErrCode = 0;
                            string ErrMsj = string.Empty;
                            string Comentario = string.Empty;
                            SAPbobsCOM.Documents oDoc = null;
                            SAPbobsCOM.StockTransfer oST = null;

                            TipoDoc = dtDoc.GetValue("TipoDoc", i).ToString();
                            DocEntry = dtDoc.GetValue("DocEntry", i).ToString();
                            DocNum = dtDoc.GetValue("DocNum", i).ToString();
                            Reserva = dtDoc.GetValue("isIns", i).ToString();
                            Indicador = dtDoc.GetValue("Indicator", i).ToString();
                            folio = string.Empty;
                            ProveedorDTE proveedorDTE = new ProveedorDTE();
                            string[] parametros = null;
                            switch (TipoDoc)
                            {
                                case "FE":
                                    parametros = new string[] { DocNum, "33", "--" };
                                    if (Reserva.Equals("Y") && !Indicador.Equals("39"))
                                    {
                                        parametros = new string[] { DocNum, "33", "FR" };
                                    }
                                    break;
                                case "FN":
                                    parametros = new string[] { DocNum, "34", "IE" };
                                    break;
                                case "BE":
                                    parametros = new string[] { DocNum, "39", "IB" };
                                    if (Reserva.Equals("Y") && Indicador.Equals("39"))
                                    {
                                        parametros = new string[] { DocNum, "39", "BR" };
                                    }
                                    break;
                                case "BN":
                                    parametros = new string[] { DocNum, "41", "EB" };
                                    break;
                                case "ND":
                                    parametros = new string[] { DocNum, "56", "DN" };
                                    break;
                                case "NC":
                                    parametros = new string[] { DocNum, "61", "" };
                                    break;
                                case "GE":
                                    parametros = new string[] { DocNum, "52", "EM" };
                                    break;
                                case "GT":
                                    parametros = new string[] { DocNum, "52", "ST" };
                                    break;
                                case "FEX":
                                    parametros = new string[] { DocNum, "110", "IX" };
                                    break;
                                case "NDX":
                                    parametros = new string[] { DocNum, "111", "DN" };
                                    break;
                                case "NCX":
                                    parametros = new string[] { DocNum, "112", "" };
                                    break;
                                default:
                                    break;
                            }

                            var result = proveedorDTE.CrearDocumento(parametros);
                            if (result.Success)
                            {
                                var resp = proveedorDTE.Respuesta;

                                // Guardar Folio
                                int Ret = 0;
                                int Intento = 0;
                                switch (TipoDoc)
                                {
                                    case "FE":
                                    case "FN":
                                    case "BE":
                                    case "BN":
                                    case "ND":
                                    case "FEX":
                                    case "NDX":
                                        oDoc = (SAPbobsCOM.Documents)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices);
                                        oDoc.GetByKey(int.Parse(DocEntry));

                                        oDoc.FolioPrefixString = TipoDoc;
                                        oDoc.FolioNumber = int.Parse(resp.Folio);
                                        oDoc.Printed = SAPbobsCOM.PrintStatusEnum.psYes;

                                        if (ExtConf.Parametros.Proveedor_FE.Equals("DBN-EV"))
                                        {
                                            //Guardar Links
                                            var links = resp.Mensajes.Split(';');
                                            if (links.Length > 1)
                                            {
                                                oDoc.UserFields.Fields.Item("U_LinkPDF").Value = links[1];
                                            }
                                            string linkEV = SBO.ConsultasSBO.ObtenerURLPDFDTE();
                                            linkEV += string.Format("?Sociedad={0}&Tipo={1}&Folio={2}", ExtConf.Parametros.Rut_Emisor.Replace(".", ""), parametros[1], resp.Folio);
                                            oDoc.UserFields.Fields.Item("U_URL").Value = linkEV;
                                        }

                                        //Ret = oDoc.Update();
                                        Ret = 0;
                                        Intento = 0;
                                        while (Intento < 5)
                                        {
                                            Ret = oDoc.Update();
                                            if (Ret.Equals(-2038)) //Deadlocks
                                            {
                                                System.Threading.Thread.Sleep(1000);
                                                Intento++;
                                            }
                                            else
                                            {
                                                break;
                                            }
                                        }
                                        break;
                                    case "NC":
                                    case "NCX":
                                        oDoc = (SAPbobsCOM.Documents)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oCreditNotes);
                                        oDoc.GetByKey(int.Parse(DocEntry));

                                        oDoc.FolioPrefixString = TipoDoc;
                                        oDoc.FolioNumber = int.Parse(resp.Folio);
                                        oDoc.Printed = SAPbobsCOM.PrintStatusEnum.psYes;

                                        if (ExtConf.Parametros.Proveedor_FE.Equals("DBN-EV"))
                                        {
                                            //Guardar Links
                                            var links = resp.Mensajes.Split(';');
                                            if (links.Length > 1)
                                            {
                                                oDoc.UserFields.Fields.Item("U_LinkPDF").Value = links[1];
                                            }
                                            string linkEV = SBO.ConsultasSBO.ObtenerURLPDFDTE();
                                            linkEV += string.Format("?Sociedad={0}&Tipo={1}&Folio={2}", ExtConf.Parametros.Rut_Emisor.Replace(".", ""), parametros[1], resp.Folio);
                                            oDoc.UserFields.Fields.Item("U_URL").Value = linkEV;
                                        }

                                        //Ret = oDoc.Update();
                                        Ret = 0;
                                        Intento = 0;
                                        while (Intento < 5)
                                        {
                                            Ret = oDoc.Update();
                                            if (Ret.Equals(-2038)) //Deadlocks
                                            {
                                                System.Threading.Thread.Sleep(1000);
                                                Intento++;
                                            }
                                            else
                                            {
                                                break;
                                            }
                                        }
                                        break;
                                    case "GE":
                                        oDoc = (SAPbobsCOM.Documents)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oDeliveryNotes);
                                        oDoc.GetByKey(int.Parse(DocEntry));

                                        oDoc.FolioPrefixString = TipoDoc;
                                        oDoc.FolioNumber = int.Parse(resp.Folio);
                                        oDoc.Printed = SAPbobsCOM.PrintStatusEnum.psYes;

                                        if (ExtConf.Parametros.Proveedor_FE.Equals("DBN-EV"))
                                        {
                                            //Guardar Links
                                            var links = resp.Mensajes.Split(';');
                                            if (links.Length > 1)
                                            {
                                                oDoc.UserFields.Fields.Item("U_LinkPDF").Value = links[1];
                                            }
                                            string linkEV = SBO.ConsultasSBO.ObtenerURLPDFDTE();
                                            linkEV += string.Format("?Sociedad={0}&Tipo={1}&Folio={2}", ExtConf.Parametros.Rut_Emisor.Replace(".", ""), parametros[1], resp.Folio);
                                            oDoc.UserFields.Fields.Item("U_URL").Value = linkEV;
                                        }

                                        //Ret = oDoc.Update();
                                        Ret = 0;
                                        Intento = 0;
                                        while (Intento < 5)
                                        {
                                            Ret = oDoc.Update();
                                            if (Ret.Equals(-2038)) //Deadlocks
                                            {
                                                System.Threading.Thread.Sleep(1000);
                                                Intento++;
                                            }
                                            else
                                            {
                                                break;
                                            }
                                        }
                                        break;
                                    case "GT":
                                        oST = (SAPbobsCOM.StockTransfer)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oStockTransfer);
                                        oST.GetByKey(int.Parse(DocEntry));

                                        oST.FolioPrefixString = TipoDoc;
                                        oST.FolioNumber = int.Parse(resp.Folio);

                                        if (ExtConf.Parametros.Proveedor_FE.Equals("DBN-EV"))
                                        {
                                            //Guardar Links
                                            var links = resp.Mensajes.Split(';');
                                            if (links.Length > 1)
                                            {
                                                oST.UserFields.Fields.Item("U_LinkPDF").Value = links[1];
                                            }
                                            string linkEV = SBO.ConsultasSBO.ObtenerURLPDFDTE();
                                            linkEV += string.Format("?Sociedad={0}&Tipo={1}&Folio={2}", ExtConf.Parametros.Rut_Emisor.Replace(".", ""), parametros[1], resp.Folio);
                                            oST.UserFields.Fields.Item("U_URL").Value = linkEV;
                                        }

                                        //Ret = oST.Update();
                                        Ret = 0;
                                        Intento = 0;
                                        while (Intento < 5)
                                        {
                                            Ret = oST.Update();
                                            if (Ret.Equals(-2038)) //Deadlocks
                                            {
                                                System.Threading.Thread.Sleep(1000);
                                                Intento++;
                                            }
                                            else
                                            {
                                                break;
                                            }
                                        }
                                        break;
                                    default:
                                        break;
                                }
                                if (Ret != 0)
                                {
                                    SBO.ConexionSBO.oCompany.GetLastError(out ErrCode, out ErrMsj);
                                    Application.SBO_Application.StatusBar.SetText(string.Format("{0}-{1} No se logró actualizar el folio del documento: {2}-{3}", TipoDoc, DocNum, ErrCode, ErrMsj), SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
                                    Comentario = string.Format("No se logró actualizar el folio. Revise Log.");
                                    dtDoc.SetValue("Obs", i, Comentario);
                                }
                                else
                                {
                                    folio = resp.Folio;
                                    Comentario = string.Format("Folio actualizado {0}-{1}", TipoDoc, resp.Folio);
                                    dtDoc.SetValue("Obs", i, Comentario);
                                    dtDoc.SetValue("StatusDTE", i, 1);
                                }
                                regProcesados++;
                            }
                            else
                            {
                                Application.SBO_Application.StatusBar.SetText(string.Format("{0}-{1} Error al crear DTE: {2}", TipoDoc, DocNum, result.Mensaje), SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
                                Comentario = string.Format("Error al crear DTE. Revise Log.");
                                dtDoc.SetValue("Obs", i, Comentario);
                            }

                            //if (!string.IsNullOrEmpty(folio))
                            if (!string.IsNullOrEmpty(folio) && !ExtConf.Parametros.Proveedor_FE.Equals("DBN-EC"))
                            {
                                if (!ExtConf.Parametros.Proveedor_FE.Equals("DBN-EV"))
                                {
                                    switch (TipoDoc)
                                    {
                                        case "FE":
                                            parametros = new string[] { "", folio, "33", "", "", "false" };
                                            if (ExtConf.Parametros.Proveedor_FE.Equals("DBN-EC"))
                                            {
                                                parametros = new string[] { "", folio, "33", oDoc.DocTotal.ToString(), oDoc.DocDate.ToString("yyyy-MM-dd"), "false" };
                                            }
                                            break;
                                        case "FN":
                                            parametros = new string[] { "", folio, "34", "", "", "false" };
                                            if (ExtConf.Parametros.Proveedor_FE.Equals("DBN-EC"))
                                            {
                                                parametros = new string[] { "", folio, "34", oDoc.DocTotal.ToString(), oDoc.DocDate.ToString("yyyy-MM-dd"), "false" };
                                            }
                                            break;
                                        case "BE":
                                            parametros = new string[] { "", folio, "39", "", "", "false" };
                                            break;
                                        case "BN":
                                            parametros = new string[] { "", folio, "41", "", "", "false" };
                                            break;
                                        case "ND":
                                            parametros = new string[] { "", folio, "56", "", "", "false" };
                                            if (ExtConf.Parametros.Proveedor_FE.Equals("DBN-EC"))
                                            {
                                                parametros = new string[] { "", folio, "56", oDoc.DocTotal.ToString(), oDoc.DocDate.ToString("yyyy-MM-dd"), "false" };
                                            }
                                            break;
                                        case "NC":
                                            parametros = new string[] { "", folio, "61", "", "", "false" };
                                            if (ExtConf.Parametros.Proveedor_FE.Equals("DBN-EC"))
                                            {
                                                parametros = new string[] { "", folio, "61", oDoc.DocTotal.ToString(), oDoc.DocDate.ToString("yyyy-MM-dd"), "false" };
                                            }
                                            break;
                                        case "GE":
                                        case "GT":
                                            parametros = new string[] { "", folio, "52", "", "", "false" };
                                            break;
                                        case "FEX":
                                            parametros = new string[] { "", folio, "110", "", "", "false" };
                                            break;
                                        case "NDX":
                                            parametros = new string[] { "", folio, "111", "", "", "false" };
                                            break;
                                        case "NCX":
                                            parametros = new string[] { "", folio, "112", "", "", "false" };
                                            break;
                                        default:
                                            break;
                                    }
                                    var resultPDF = proveedorDTE.ObtenerPDF(parametros);
                                    if (resultPDF.Success)
                                    {
                                        var respPDF = proveedorDTE.RespuestaPDF;

                                        //Guardar PDF
                                        SAPbobsCOM.CompanyService com_service = SBO.ConexionSBO.oCompany.GetCompanyService();
                                        SAPbobsCOM.PathAdmin oPathAdmin = com_service.GetPathAdmin();
                                        string Path = string.Empty;
                                        Path = oPathAdmin.AttachmentsFolderPath;
                                        int indexPath = Path.LastIndexOf("\\");
                                        if (indexPath > 0)
                                        {
                                            Path = Path.Substring(0, indexPath);
                                        }
                                        string fileName = string.Empty;
                                        string fileExt = string.Empty;

                                        string[] linkSplit = respPDF.String[0].Split('/');
                                        fileName = linkSplit[linkSplit.Length - 1];
                                        int indexExt = fileName.LastIndexOf(".");
                                        if (indexExt > 0)
                                        {
                                            fileExt = fileName.Substring(indexExt + 1);
                                            fileName = fileName.Substring(0, indexExt);
                                        }

                                        SAPbobsCOM.Attachments2 oAtt = null;
                                        oAtt = (SAPbobsCOM.Attachments2)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oAttachments2);
                                        oAtt.Lines.Add();
                                        oAtt.Lines.FileName = fileName;
                                        oAtt.Lines.FileExtension = fileExt;
                                        oAtt.Lines.SourcePath = Path;
                                        oAtt.Lines.Override = SAPbobsCOM.BoYesNoEnum.tYES;

                                        int RetAtt = oAtt.Add();
                                        int RetVal = 0;
                                        if (RetAtt != 0)
                                        {
                                            SBO.ConexionSBO.oCompany.GetLastError(out ErrCode, out ErrMsj);
                                            Application.SBO_Application.StatusBar.SetText(string.Format("{0}-{1} No se logró adjuntar PDF del documento: {2}-{3}", TipoDoc, DocNum, ErrCode, ErrMsj), SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
                                            Comentario += @" / " + string.Format("No se logró adjuntar PDF. Revise Log.");
                                            dtDoc.SetValue("Obs", i, Comentario);
                                        }
                                        else
                                        {
                                            oAtt.GetByKey(int.Parse(SBO.ConexionSBO.oCompany.GetNewObjectKey()));
                                            switch (TipoDoc)
                                            {
                                                case "FE":
                                                case "FN":
                                                case "BE":
                                                case "BN":
                                                case "ND":
                                                case "NC":
                                                case "GE":
                                                case "FEX":
                                                case "NDX":
                                                case "NCX":
                                                    oDoc.AttachmentEntry = oAtt.AbsoluteEntry;
                                                    RetVal = oDoc.Update();
                                                    break;
                                                case "GT":
                                                    oST.AttachmentEntry = oAtt.AbsoluteEntry;
                                                    RetVal = oST.Update();
                                                    break;
                                                default:
                                                    break;
                                            }
                                        }

                                        if (RetVal != 0)
                                        {
                                            SBO.ConexionSBO.oCompany.GetLastError(out ErrCode, out ErrMsj);
                                            Application.SBO_Application.StatusBar.SetText(string.Format("{0}-{1} No se logró actualizar el documento: {2}-{3}", TipoDoc, DocNum, ErrCode, ErrMsj), SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
                                            Comentario += @" / " + string.Format("No se logró actualizar el documento. Revise Log.");
                                            dtDoc.SetValue("Obs", i, Comentario);
                                        }
                                        else
                                        {
                                            Comentario += @" / " + string.Format("PDF actualizado {0}-{1}", TipoDoc, folio);
                                            dtDoc.SetValue("Obs", i, Comentario);
                                            dtDoc.SetValue("StatusPDF", i, 1);
                                        }
                                    }
                                    else
                                    {
                                        Application.SBO_Application.StatusBar.SetText(string.Format("{0}-{1} Error al recuperar PDF: {2}", TipoDoc, DocNum, resultPDF.Mensaje), SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
                                        Comentario += @" / " + string.Format("Error al recuperar PDF. Revise Log.");
                                        dtDoc.SetValue("Obs", i, Comentario);
                                    }
                                }
                            }
                            regMarcados++;
                        }
                    }
                    catch (Exception ex)
                    {
                        Application.SBO_Application.StatusBar.SetText(string.Format("{0}-{1} Error: {2}", TipoDoc, DocNum, ex.Message), SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
                        dtDoc.SetValue("Obs", i, string.Format("Error en proceso. Revise Log."));
                    }
                }
                oMatrix.LoadFromDataSource();
                Application.SBO_Application.StatusBar.SetText(string.Format("Proceso completo. Se procesaron {0} documentos. {1} documentos generados OK", regMarcados, regProcesados), SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
