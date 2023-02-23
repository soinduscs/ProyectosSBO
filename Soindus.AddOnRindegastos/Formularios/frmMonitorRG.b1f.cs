using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using SAPbouiCOM;
using SAPbouiCOM.Framework;
//using InterRG = Soindus.Interfaces.Rindegastos;

namespace Soindus.AddOnRindegastos.Formularios
{
    [FormAttribute("Soindus.AddOnRindegastos.Formularios.frmMonitorRG", "Formularios/frmMonitorRG.b1f")]
    class frmMonitorRG : UserFormBase
    {
        // Objetos de formulario
        #region Objetos de formulario
        private static SAPbouiCOM.Form oForm;
        private static SAPbouiCOM.Folder Folder1;
        private static SAPbouiCOM.Folder Folder2;
        private static SAPbouiCOM.EditText txtFDesde;
        private static SAPbouiCOM.EditText txtFHasta;
        private static SAPbouiCOM.Matrix mtxRendi;
        private static SAPbouiCOM.Matrix mtxGastos;
        private static SAPbouiCOM.Button btnReproc;
        private static SAPbouiCOM.Button btnGenFR;
        private static SAPbouiCOM.Button btnFilter;
        private static SAPbouiCOM.Button btnClose;
        #endregion

        public frmMonitorRG()
        {
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

        public static void Form_ItemEvent(string formUID, ref SAPbouiCOM.ItemEvent pVal, out bool bubbleEvent)
        {
            bubbleEvent = true;
            try
            {
                if (pVal.BeforeAction.Equals(true))
                {
                    if (pVal.EventType.Equals(SAPbouiCOM.BoEventTypes.et_CLICK))
                    {
                        if (pVal.ItemUID.Equals("btnTest"))
                        {
                            ProcesarRendiciones();
                        }
                        if (pVal.ItemUID.Equals("btnGenFR"))
                        {
                            ProcesarSolicitudesMarcadas();
                        }
                        if (pVal.ItemUID.Equals("btnFilter"))
                        {
                            CargarMatrixRendiciones();
                            Application.SBO_Application.StatusBar.SetText("Rendiciones cargadas correctamente.", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success);
                        }
                        if (pVal.ItemUID.Equals("btnReproc"))
                        {
                            Reprocesar();
                            CargarMatrixRendiciones();
                            Application.SBO_Application.StatusBar.SetText("Rendiciones actualizadas para reproceso.", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success);
                        }
                        if (pVal.ItemUID.Equals("mtxRendi"))
                        {
                            if (pVal.Row > 0)
                            {
                                int Row = pVal.Row;
                                mtxRendi.SelectRow(Row, true, false);
                                SAPbouiCOM.EditText EditValue = (SAPbouiCOM.EditText)mtxRendi.GetCellSpecific("U_ID", Row);
                                string strValue = EditValue.Value.ToString().Trim();
                                CargarMatrixGastos(strValue);
                            }
                        }
                        if (pVal.ItemUID.Equals("mtxGastos"))
                        {
                            if (pVal.Row > 0)
                            {
                                int Row = pVal.Row;
                                mtxGastos.SelectRow(Row, true, false);
                            }
                        }
                    }
                }
                else
                {

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
            oForm = ((SAPbouiCOM.Form)(Application.SBO_Application.Forms.Item("frmMonitorRG")));
            Folder1 = ((SAPbouiCOM.Folder)(GetItem("tab01").Specific));
            Folder2 = ((SAPbouiCOM.Folder)(GetItem("tab02").Specific));
            Folder1.Select();
            txtFDesde = ((SAPbouiCOM.EditText)(GetItem("txtDesde").Specific));
            txtFHasta = ((SAPbouiCOM.EditText)(GetItem("txtHasta").Specific));
            mtxRendi = ((SAPbouiCOM.Matrix)(GetItem("mtxRendi").Specific));
            mtxGastos = ((SAPbouiCOM.Matrix)(GetItem("mtxGastos").Specific));
            btnReproc = ((SAPbouiCOM.Button)(GetItem("btnReproc").Specific));
            btnFilter = ((SAPbouiCOM.Button)(GetItem("btnFilter").Specific));
            btnClose = ((SAPbouiCOM.Button)(GetItem("2").Specific));
        }

        private void CargarFormulario()
        {
            // Fecha desde
            oForm.DataSources.UserDataSources.Add("FDesde", SAPbouiCOM.BoDataType.dt_DATE);
            txtFDesde.DataBind.SetBound(true, "", "FDesde");

            // Fecha hasta
            oForm.DataSources.UserDataSources.Add("FHasta", SAPbouiCOM.BoDataType.dt_DATE);
            txtFHasta.DataBind.SetBound(true, "", "FHasta");

            oForm.DataSources.DataTables.Add("dtRendi");
            oForm.DataSources.DataTables.Add("dtGastos");

            EstructuraMatrixRendiciones();
            EstructuraMatrixGastos();

            CargarMatrixRendiciones();
            //CargarMatrixGastos("0");

            mtxRendi.SelectionMode = SAPbouiCOM.BoMatrixSelect.ms_Single;
            mtxGastos.SelectionMode = SAPbouiCOM.BoMatrixSelect.ms_Single;
            oForm.Visible = true;
        }

        private static void EstructuraMatrixRendiciones()
        {
            string NombreDT = "dtRendi";
            SAPbouiCOM.DataTable datatable = oForm.DataSources.DataTables.Item(NombreDT);
            string _query = @"SELECT 'N' AS ""Chk"", * FROM ""@SO_RENDI"" WHERE ""U_ID"" = 0";
            datatable.ExecuteQuery(_query);

            SAPbouiCOM.Columns oColumns;
            oColumns = mtxRendi.Columns;
            SAPbouiCOM.Column oColumn;

            oColumn = oColumns.Add("Chk", SAPbouiCOM.BoFormItemTypes.it_CHECK_BOX);
            oColumn.TitleObject.Caption = String.Empty;
            oColumn.Editable = true;
            oColumn.Width = 15;
            oColumn.ValOn = "Y";
            oColumn.ValOff = "N";
            oColumn.DataBind.Bind(NombreDT, "Chk");

            oColumn = oColumns.Add("DocEntry", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "DocEntry";
            oColumn.TitleObject.Sortable = false;
            oColumn.Editable = false;
            oColumn.Visible = false;
            oColumn.Width = 20;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "DocEntry");

            oColumn = oColumns.Add("U_GESTION", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Gestión";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 60;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "U_GESTION");

            oColumn = oColumns.Add("U_ID", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Id único";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 60;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "U_ID");

            oColumn = oColumns.Add("U_REPNUM", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "N° rendición";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 40;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "U_REPNUM");

            oColumn = oColumns.Add("U_CDATE", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Fecha de cierre";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 70;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "U_CDATE");

            oColumn = oColumns.Add("U_EMPIDE", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Rut rendidor";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 70;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "U_EMPIDE");

            oColumn = oColumns.Add("U_EMPNAME", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Nombre rendidor";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 150;
            oColumn.RightJustified = false;
            oColumn.DataBind.Bind(NombreDT, "U_EMPNAME");

            oColumn = oColumns.Add("U_REPTOT", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Importe";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 70;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "U_REPTOT");

            oColumn = oColumns.Add("U_CUR", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Moneda";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 40;
            oColumn.RightJustified = false;
            oColumn.DataBind.Bind(NombreDT, "U_CUR");

            oColumn = oColumns.Add("U_STATUS", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Estado";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 40;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "U_STATUS");

            oColumn = oColumns.Add("U_CSTATUS", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Pagado";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 60;
            oColumn.RightJustified = false;
            oColumn.DataBind.Bind(NombreDT, "U_CSTATUS");

            oColumn = oColumns.Add("U_INTEG", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Integrado";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 40;
            oColumn.RightJustified = false;
            oColumn.DataBind.Bind(NombreDT, "U_INTEG");

            oColumn = oColumns.Add("U_ESTADO", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Estado";
            oColumn.TitleObject.Sortable = false;
            oColumn.Editable = false;
            oColumn.Visible = false;
            oColumn.Width = 0;
            oColumn.RightJustified = false;
            oColumn.DataBind.Bind(NombreDT, "U_ESTADO");

            oColumn = oColumns.Add("U_OBS", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Comentario";
            oColumn.TitleObject.Sortable = false;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 200;
            oColumn.RightJustified = false;
            oColumn.DataBind.Bind(NombreDT, "U_OBS");

            mtxRendi.Clear();
            mtxRendi.LoadFromDataSourceEx();
        }

        private static void EstructuraMatrixGastos()
        {
            string NombreDT = "dtGastos";
            SAPbouiCOM.DataTable datatable = oForm.DataSources.DataTables.Item(NombreDT);
            string _query = @"SELECT 'N' AS ""Chk"", * FROM ""@SO_RENDIG"" WHERE ""U_RID"" = 0";
            datatable.ExecuteQuery(_query);

            SAPbouiCOM.Columns oColumns;
            oColumns = mtxGastos.Columns;
            SAPbouiCOM.Column oColumn;

            oColumn = oColumns.Add("Chk", SAPbouiCOM.BoFormItemTypes.it_CHECK_BOX);
            oColumn.TitleObject.Caption = String.Empty;
            oColumn.Editable = true;
            oColumn.Width = 15;
            oColumn.ValOn = "Y";
            oColumn.ValOff = "N";
            oColumn.DataBind.Bind(NombreDT, "Chk");

            oColumn = oColumns.Add("DocEntry", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "DocEntry";
            oColumn.TitleObject.Sortable = false;
            oColumn.Editable = false;
            oColumn.Visible = false;
            oColumn.Width = 20;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "DocEntry");

            oColumn = oColumns.Add("U_ID", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Id único";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 60;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "U_ID");

            oColumn = oColumns.Add("U_STATUS", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Estado";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 60;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "U_STATUS");

            oColumn = oColumns.Add("U_SUPPLIER", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Proveedor";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 150;
            oColumn.RightJustified = false;
            oColumn.DataBind.Bind(NombreDT, "U_SUPPLIER");

            oColumn = oColumns.Add("U_IDATE", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Fecha gasto";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 70;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "U_IDATE");

            oColumn = oColumns.Add("U_OAMOUNT", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Importe Or.";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 70;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "U_OAMOUNT");

            oColumn = oColumns.Add("U_OCUR", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Moneda Or.";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 40;
            oColumn.RightJustified = false;
            oColumn.DataBind.Bind(NombreDT, "U_OCUR");

            oColumn = oColumns.Add("U_NET", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Neto";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 70;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "U_NET");

            oColumn = oColumns.Add("U_TAX", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Impuesto";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 50;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "U_TAX");

            oColumn = oColumns.Add("U_TAXNAME", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Cód. Imp.";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 40;
            oColumn.RightJustified = false;
            oColumn.DataBind.Bind(NombreDT, "U_TAXNAME");

            oColumn = oColumns.Add("U_TOTAL", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Total";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 70;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "U_TOTAL");

            oColumn = oColumns.Add("U_CUR", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Moneda";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 40;
            oColumn.RightJustified = false;
            oColumn.DataBind.Bind(NombreDT, "U_CUR");

            oColumn = oColumns.Add("U_CATEGORY", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Categoría";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 150;
            oColumn.RightJustified = false;
            oColumn.DataBind.Bind(NombreDT, "U_CATEGORY");

            oColumn = oColumns.Add("U_CATCODE", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Cód. Categ.";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 60;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "U_CATCODE");

            oColumn = oColumns.Add("U_NOTE", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Comentarios";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 200;
            oColumn.RightJustified = false;
            oColumn.DataBind.Bind(NombreDT, "U_NOTE");

            //CreaCampoMD("@SO_RENDIG", "ID", "Id Gasto", 11, SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            //CreaCampoMD("@SO_RENDIG", "STATUS", "Status", 11, SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            //CreaCampoMD("@SO_RENDIG", "SUPPLIER", "Supplier", 254, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            //CreaCampoMD("@SO_RENDIG", "IDATE", "IssueDate", 10, SAPbobsCOM.BoFieldTypes.db_Date, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            //CreaCampoMD("@SO_RENDIG", "OAMOUNT", "OriginalAmount", 30, SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Price, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            //CreaCampoMD("@SO_RENDIG", "OCUR", "OriginalCurrency", 10, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            //CreaCampoMD("@SO_RENDIG", "EXRATE", "ExchangeRate", 30, SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Price, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            //CreaCampoMD("@SO_RENDIG", "NET", "Net", 30, SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Price, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            //CreaCampoMD("@SO_RENDIG", "TAX", "Tax", 30, SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Price, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            //CreaCampoMD("@SO_RENDIG", "TAXNAME", "TaxName", 50, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            //CreaCampoMD("@SO_RENDIG", "OTAX", "OtherTaxes", 30, SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Price, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            //CreaCampoMD("@SO_RENDIG", "RETNAME", "RetentionName", 254, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            //CreaCampoMD("@SO_RENDIG", "RET", "Retention", 30, SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Price, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            //CreaCampoMD("@SO_RENDIG", "TOTAL", "Total", 30, SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Price, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            //CreaCampoMD("@SO_RENDIG", "CUR", "Currency", 10, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            //CreaCampoMD("@SO_RENDIG", "REIMB", "Reimbursable", 1, SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            //CreaCampoMD("@SO_RENDIG", "CATEGORY", "Category", 254, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            //CreaCampoMD("@SO_RENDIG", "CATCODE", "CategoryCode", 254, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            //CreaCampoMD("@SO_RENDIG", "CATGRP", "CategoryGroup", 254, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            //CreaCampoMD("@SO_RENDIG", "CATGRPC", "CategoryGroupCode", 254, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            //CreaCampoMD("@SO_RENDIG", "NOTE", "Note", 254, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            //CreaCampoMD("@SO_RENDIG", "INTDATE", "IntegrationDate", 254, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            //CreaCampoMD("@SO_RENDIG", "INTECODE", "IntegrationExternalCode", 254, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            //CreaCampoMD("@SO_RENDIG", "NBRFILES", "NbrFiles", 11, SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            //CreaCampoMD("@SO_RENDIG", "RID", "ReportId", 11, SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            //CreaCampoMD("@SO_RENDIG", "EXPOLID", "ExpensePolicyId", 11, SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            //CreaCampoMD("@SO_RENDIG", "USERID", "UserId", 11, SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);

            mtxGastos.Clear();
            mtxGastos.LoadFromDataSourceEx();
        }

        private static void CargarMatrixRendiciones()
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

                //dt = DateTime.Today.AddDays(-30);
                dt = DateTime.Today.AddDays(-7);
                Mes = (dt.Month.ToString().Length == 1) ? dt.Month.ToString().PadLeft(2, '0') : dt.Month.ToString();
                Dia = (dt.Day.ToString().Length == 1) ? dt.Day.ToString().PadLeft(2, '0') : dt.Day.ToString();

                FechaInicial = string.Format("{0}{1}{2}", dt.Year.ToString(), Mes, Dia);
            }

            string FiltroFecha = string.Format(@"WHERE ""U_CDATE"" BETWEEN '{0}' AND '{1}'", FechaInicial, FechaFinal);

            string NombreDT = "dtRendi";
            SAPbouiCOM.DataTable datatable = oForm.DataSources.DataTables.Item(NombreDT);
            string _query = @"SELECT 'N' AS ""Chk"", * FROM ""@SO_RENDI"" " + FiltroFecha;
            datatable.ExecuteQuery(_query);
            oForm.Freeze(true);
            mtxRendi.Clear();
            mtxRendi.LoadFromDataSourceEx();
            oForm.Freeze(false);
        }

        private static void CargarMatrixGastos(string IDRendicion)
        {
            string NombreDT = "dtGastos";
            SAPbouiCOM.DataTable datatable = oForm.DataSources.DataTables.Item(NombreDT);
            string _query = @"SELECT 'N' AS ""Chk"", * FROM ""@SO_RENDIG"" WHERE ""U_RID"" = " + IDRendicion + " ";
            datatable.ExecuteQuery(_query);
            oForm.Freeze(true);
            mtxGastos.Clear();
            mtxGastos.LoadFromDataSourceEx();
            oForm.Freeze(false);
        }

        private static void ProcesarRendiciones()
        {
            string Since = string.Empty;
            string Until = string.Empty;
            string TypeDateFilter = string.Empty;
            string Currency = string.Empty;
            string Status = string.Empty;
            string ExpensePolicyId = string.Empty;
            string IntegrationStatus = string.Empty;
            string IntegrationCode = string.Empty;
            string IntegrationDate = string.Empty;
            string UserId = string.Empty;
            string OrderBy = string.Empty;
            string Order = string.Empty;
            string ResultsPerPage = string.Empty;
            string Page = string.Empty;

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
                FechaInicial = string.Format("{0}-{1}-{2}", DesdeFecha.Substring(0, 4), DesdeFecha.Substring(4, 2), DesdeFecha.Substring(6, 2));
                FechaFinal = string.Format("{0}-{1}-{2}", HastaFecha.Substring(0, 4), HastaFecha.Substring(4, 2), HastaFecha.Substring(6, 2));
            }
            else
            {
                // Por defecto trae los últimos 7 días
                dt = new DateTime(DateTime.Now.Year, DateTime.Today.Month, DateTime.Today.Day);
                Mes = (dt.Month.ToString().Length == 1) ? dt.Month.ToString().PadLeft(2, '0') : dt.Month.ToString();
                Dia = (dt.Day.ToString().Length == 1) ? dt.Day.ToString().PadLeft(2, '0') : dt.Day.ToString();

                FechaFinal = string.Format("{0}-{1}-{2}", dt.Year.ToString(), Mes, Dia);

                //dt = DateTime.Today.AddDays(-30);
                dt = DateTime.Today.AddDays(-7);
                Mes = (dt.Month.ToString().Length == 1) ? dt.Month.ToString().PadLeft(2, '0') : dt.Month.ToString();
                Dia = (dt.Day.ToString().Length == 1) ? dt.Day.ToString().PadLeft(2, '0') : dt.Day.ToString();

                FechaInicial = string.Format("{0}-{1}-{2}", dt.Year.ToString(), Mes, Dia);
            }

            //Since = "2020-07-01";
            //Until = "2020-07-08";
            Since = FechaInicial;
            Until = FechaFinal;

            TypeDateFilter = "1";
            Currency = string.Empty;
            Status = string.Empty;
            ExpensePolicyId = string.Empty;
            IntegrationStatus = "0";
            IntegrationCode = string.Empty;
            IntegrationDate = string.Empty;
            UserId = string.Empty;
            OrderBy = "3";
            Order = "DESC";
            ResultsPerPage = "5";
            Page = "1";

            string[] parametros = new string[] { Since, Until, TypeDateFilter,
                                    Currency, Status, ExpensePolicyId, IntegrationStatus,
                                    IntegrationCode, IntegrationDate, UserId,
                                    OrderBy, Order, ResultsPerPage, Page };

            Application.SBO_Application.StatusBar.SetText("Importando Rendiciones. Por favor espere...", SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);
            var resp = ImportarRendiciones(parametros);
            Application.SBO_Application.StatusBar.SetText("Integrando Rendiciones. Por favor espere...", SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);
            var conta = ContabilizarRendiciones();
            var conta2 = ContabilizarDevoluciones();
            CargarMatrixRendiciones();
            Application.SBO_Application.StatusBar.SetText("Proceso finalizado...", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success);
        }

        private static Clases.Message ImportarRendiciones(string[] filtros)
        {
            Clases.Message resp = new Clases.Message();
            InterfazRG interfazRG = new InterfazRG();
            try
            {
                Clases.Configuracion ExtConf = new Clases.Configuracion();
                string rut_sociedad = ExtConf.Parametros.Rut_Sociedad;

                string Since = string.Empty;
                string Until = string.Empty;
                string TypeDateFilter = string.Empty;
                string Currency = string.Empty;
                string Status = string.Empty;
                string ExpensePolicyId = string.Empty;
                string IntegrationStatus = string.Empty;
                string IntegrationCode = string.Empty;
                string IntegrationDate = string.Empty;
                string UserId = string.Empty;
                string OrderBy = string.Empty;
                string Order = string.Empty;
                string ResultsPerPage = string.Empty;
                string Page = string.Empty;

                Since = filtros[0];
                Until = filtros[1];
                TypeDateFilter = filtros[2];
                Currency = filtros[3];
                Status = filtros[4];
                ExpensePolicyId = filtros[5];
                IntegrationStatus = filtros[6];
                IntegrationCode = filtros[7];
                IntegrationDate = filtros[8];
                UserId = filtros[9];
                OrderBy = filtros[10];
                Order = filtros[11];
                ResultsPerPage = filtros[12];
                Page = filtros[13];

                string[] parametros = new string[] { Since, Until, TypeDateFilter,
                                    Currency, Status, ExpensePolicyId, IntegrationStatus,
                                    IntegrationCode, IntegrationDate, UserId,
                                    OrderBy, Order, ResultsPerPage, Page };

                // Primera página de reportes
                var rgResult = interfazRG.ObtenerRendiciones(parametros);
                if (rgResult.Success)
                {
                    long _pages = interfazRG.ResponseExpenseReports.Records.Pages;
                    var _Rendiciones = interfazRG.ResponseExpenseReports;
                    foreach (var _Reporte in _Rendiciones.ExpenseReports)
                    {
                        if (!EsRendicionSociedad(rut_sociedad, _Reporte))
                        {
                            continue;
                        }
                        string ReportId = _Reporte.Id.ToString();
                        string Category = string.Empty;
                        string gOrderBy = string.Empty;
                        string gOrder = string.Empty;
                        string gResultsPerPage = string.Empty;
                        string gPage = string.Empty;
                        gOrderBy = "1";
                        gOrder = "DESC";
                        gResultsPerPage = "500";
                        gPage = "1";

                        string[] parametrosg = new string[] { null, null, null,
                                            null, null, ReportId, null,
                                            null, null, null, null,
                                            gOrderBy, gOrder, gResultsPerPage, gPage };

                        var _Gastos = new Clases.ResponseExpenses();
                        // Primera página de gastos
                        var rgResultG = interfazRG.ObtenerGastos(parametrosg);
                        if (rgResultG.Success)
                        {
                            _Gastos = interfazRG.ResponseExpenses;
                        }
                        var guardar = SBO.ModeloSBO.AddRendiciones(_Reporte, _Gastos);

                        //// Resto de páginas de gastos
                        //if (rgResultG.Success)
                        //{
                        //    long _gpages = interfazRG.ResponseExpenses.Records.Pages;
                        //    for (long k = 0; k < _gpages - 1; k++)
                        //    {
                        //        parametrosg = new string[] { null, null, null,
                        //                    null, null, ReportId, null,
                        //                    null, null, null, null,
                        //                    gOrderBy, gOrder, gResultsPerPage, (k + 2).ToString() };

                        //        _Gastos = new Clases.ResponseExpenses();
                        //        _Gastos = interfazRG.ResponseExpenses;
                        //        guardar = SBO.ModeloSBO.RendicionesAdd(_Reporte, _Gastos);
                        //    }
                        //}
                    }

                    // Resto de páginas
                    for (long i = 0; i < _pages - 1; i++)
                    {
                        parametros = new string[] { Since, Until, TypeDateFilter,
                                    Currency, Status, ExpensePolicyId, IntegrationStatus,
                                    IntegrationCode, IntegrationDate, UserId,
                                    OrderBy, Order, ResultsPerPage, (i + 2).ToString() };
                        rgResult = interfazRG.ObtenerRendiciones(parametros);
                        if (rgResult.Success)
                        {
                            _Rendiciones = interfazRG.ResponseExpenseReports;
                            foreach (var _Reporte in _Rendiciones.ExpenseReports)
                            {
                                if (!EsRendicionSociedad(rut_sociedad, _Reporte))
                                {
                                    continue;
                                }
                                string ReportId = _Reporte.Id.ToString();
                                string Category = string.Empty;
                                string gOrderBy = string.Empty;
                                string gOrder = string.Empty;
                                string gResultsPerPage = string.Empty;
                                string gPage = string.Empty;
                                gOrderBy = "1";
                                gOrder = "DESC";
                                gResultsPerPage = "500";
                                gPage = "1";

                                string[] parametrosg = new string[] { null, null, null,
                                            null, null, ReportId, null,
                                            null, null, null, null,
                                            gOrderBy, gOrder, gResultsPerPage, gPage };

                                var _Gastos = new Clases.ResponseExpenses();
                                var rgResultG = interfazRG.ObtenerGastos(parametrosg);
                                if (rgResultG.Success)
                                {
                                    _Gastos = interfazRG.ResponseExpenses;
                                }
                                var guardar = SBO.ModeloSBO.AddRendiciones(_Reporte, _Gastos);

                                //// Resto de páginas de gastos
                                //if (rgResultG.Success)
                                //{
                                //    long _gpages = interfazRG.ResponseExpenses.Records.Pages;
                                //    for (long k = 0; k < _gpages - 1; k++)
                                //    {
                                //        parametrosg = new string[] { null, null, null,
                                //            null, null, ReportId, null,
                                //            null, null, null, null,
                                //            gOrderBy, gOrder, gResultsPerPage, (k + 2).ToString() };

                                //        _Gastos = new Clases.ResponseExpenses();
                                //        _Gastos = interfazRG.ResponseExpenses;
                                //        guardar = SBO.ModeloSBO.RendicionesAdd(_Reporte, _Gastos);
                                //    }
                                //}
                            }
                        }
                    }
                }
                resp.Success = true;
            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Mensaje = ex.Message;
            }
            return resp;
        }

        private static bool EsRendicionSociedad(string sociedad, Clases.ExpenseReport rendicion)
        {
            bool ret = false;
            foreach (var item in rendicion.ExtraFields)
            {
                if (item.Name.Equals("Empresa") && item.Code.Equals(sociedad))
                {
                    return true;
                }
            }
            return ret;
        }

        private static Clases.Message ContabilizarSolicitud(string DocEntry)
        {
            Clases.Message resp = new Clases.Message();
            try
            {
                var sbo = SBO.ModeloSBO.GetSolicitud(DocEntry);
                if (sbo.Success)
                {
                    var conta = SBO.IntegracionSBO.ContabilizarSolicitudes(sbo.Rendiciones);
                }
                else
                {
                    resp = sbo;
                }
            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Mensaje = ex.Message;
            }
            return resp;
        }

        private static Clases.Message ContabilizarRendiciones()
        {
            Clases.Message resp = new Clases.Message();
            try
            {
                var sbo = SBO.ModeloSBO.GetRendiciones("1", "Rendición");
                if (sbo.Success)
                {
                    var conta = SBO.IntegracionSBO.ContabilizarRendiciones(sbo.Rendiciones);
                }
                else
                {
                    resp = sbo;
                }
            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Mensaje = ex.Message;
            }
            return resp;
        }

        private static Clases.Message ContabilizarDevoluciones()
        {
            Clases.Message resp = new Clases.Message();
            try
            {
                var sbo = SBO.ModeloSBO.GetRendiciones("1", "Devolución");
                if (sbo.Success)
                {
                    var conta = SBO.IntegracionSBO.ContabilizarDevoluciones(sbo.Rendiciones);
                }
                else
                {
                    resp = sbo;
                }
            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Mensaje = ex.Message;
            }
            return resp;
        }

        private static void ProcesarSolicitudesMarcadas()
        {
            string NombreDT = "dtRendi";
            SAPbouiCOM.DataTable datatable = oForm.DataSources.DataTables.Item(NombreDT);
            mtxRendi.FlushToDataSource();
            for (int i = 0; i < datatable.Rows.Count; i++)
            {
                string Chk = datatable.GetValue("Chk", i).ToString();
                string Estado = datatable.GetValue("U_ESTADO", i).ToString();
                string Gestion = datatable.GetValue("U_GESTION", i).ToString();
                // sólo si check está marcado
                if (Chk.Equals("Y") && Gestion.Equals("Solicitud") && (Estado.Equals("0") || Estado.Equals("2")))
                {
                    string DocEntry = datatable.GetValue("DocEntry", i).ToString();
                    Application.SBO_Application.StatusBar.SetText("Integrando Solicitudes. Por favor espere...", SAPbouiCOM.BoMessageTime.bmt_Long, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);
                    var conta = ContabilizarSolicitud(DocEntry);
                }
            }
            CargarMatrixRendiciones();
            Application.SBO_Application.StatusBar.SetText("Proceso finalizado...", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success);
        }

        private static void Reprocesar()
        {
            string NombreDT = "dtRendi";
            SAPbouiCOM.DataTable datatable = oForm.DataSources.DataTables.Item(NombreDT);
            mtxRendi.FlushToDataSource();
            for (int i = 0; i < datatable.Rows.Count; i++)
            {
                string Chk = datatable.GetValue("Chk", i).ToString();
                string Estado = datatable.GetValue("U_ESTADO", i).ToString();
                // sólo si check está marcado
                if (Chk.Equals("Y") && Estado.Equals("9"))
                {
                    string DocEntry = datatable.GetValue("DocEntry", i).ToString();
                    SBO.ModeloSBO.UpdateRendicion(DocEntry, 2, "En reproceso...");
                }
            }
        }

        private void CodigoViejo()
        {
            InterfazRG interfazRG = new InterfazRG();
            try
            {
                string Since = string.Empty;
                string Until = string.Empty;
                string TypeDateFilter = string.Empty;
                string Currency = string.Empty;
                string Status = string.Empty;
                string ExpensePolicyId = string.Empty;
                string IntegrationStatus = string.Empty;
                string IntegrationCode = string.Empty;
                string IntegrationDate = string.Empty;
                string UserId = string.Empty;
                string OrderBy = string.Empty;
                string Order = string.Empty;
                string ResultsPerPage = string.Empty;
                string Page = string.Empty;

                Since = "2020-06-20";
                Until = "2020-06-30";
                TypeDateFilter = "1";
                Currency = string.Empty;
                Status = string.Empty;
                ExpensePolicyId = string.Empty;
                IntegrationStatus = "0";
                IntegrationCode = string.Empty;
                IntegrationDate = string.Empty;
                UserId = string.Empty;
                OrderBy = "3";
                Order = "DESC";
                ResultsPerPage = "20";
                Page = "1";

                string[] parametros = new string[] { Since, Until, TypeDateFilter,
                                    Currency, Status, ExpensePolicyId, IntegrationStatus,
                                    IntegrationCode, IntegrationDate, UserId,
                                    OrderBy, Order, ResultsPerPage, Page };

                var rgResult = interfazRG.ObtenerRendiciones(parametros);
                if (rgResult.Success)
                {
                    var _Rendiciones = interfazRG.ResponseExpenseReports;

                    foreach (var _Reporte in _Rendiciones.ExpenseReports)
                    {
                        string ReportId = _Reporte.Id.ToString();
                        string Category = string.Empty;
                        OrderBy = "1";
                        Order = "DESC";
                        ResultsPerPage = "100";
                        Page = "1";

                        //string[] parametrosg = new string[] { Since, Until, Currency,
                        //    Status, Category, ReportId, ExpensePolicyId,
                        //    IntegrationStatus, IntegrationCode, IntegrationDate, UserId,
                        //    OrderBy, Order, ResultsPerPage, Page };
                        string[] parametrosg = new string[] { null, null, null,
                                            null, null, ReportId, null,
                                            null, null, null, null,
                                            OrderBy, Order, ResultsPerPage, Page };

                        var _Gastos = new Clases.ResponseExpenses();
                        var rgResultG = interfazRG.ObtenerGastos(parametrosg);
                        if (rgResultG.Success)
                        {
                            _Gastos = interfazRG.ResponseExpenses;

                        }
                        //string Id = "6808152";
                        //parametrosg = new string[] { Id };
                        //rgResultG = interfazRG.ObtenerGasto(parametrosg);


                        var guardar = SBO.ModeloSBO.AddRendiciones(_Reporte, _Gastos);
                    }

                    //var guardar = SBO.ModeloSBO.RendicionesAdd(_Rendiciones);
                }
            }
            catch (Exception ex)
            {
            }

            //try
            //{
            //    string Id = "944693";
            //    string[] parametros = new string[] { Id };

            //    var rgResult = interfazRG.ObtenerRendicion(parametros);
            //    if (rgResult.Success)
            //    {
            //        var _Rendicion = interfazRG.ExpenseReport;
            //    }
            //}
            //catch (Exception)
            //{
            //}

            //try
            //{
            //    string Id = "944693";
            //    string IntegrationStatus = "1";
            //    string IntegrationCode = "123";
            //    string IntegrationDate = "2020-01-29 12:00:00";
            //    string[] parametros = new string[] { Id, IntegrationStatus, IntegrationCode, IntegrationDate };

            //    var rgResult = interfazRG.CambiarEstadoRendicion(parametros);
            //    if (rgResult.Success)
            //    {
            //        parametros = new string[] { Id };
            //        rgResult = interfazRG.ObtenerRendicion(parametros);
            //        if (rgResult.Success)
            //        {
            //            var _Rendicion = interfazRG.ExpenseReport;
            //        }
            //    }
            //}
            //catch (Exception)
            //{
            //}
        }

    }
}
