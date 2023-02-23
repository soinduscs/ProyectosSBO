using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM.Framework;

namespace Soindus.AddOnFactoringCob.Formularios
{
    [FormAttribute("Soindus.AddOnFactoringCob.Formularios.frmFacList", "Formularios/frmFacList.b1f")]
    class frmFacList : UserFormBase
    {
        // Objetos de formulario
        #region Objetos de formulario
        private static SAPbouiCOM.Form oForm;
        private static SAPbouiCOM.EditText txtEntidad;
        private static SAPbouiCOM.EditText txtMoneda;
        private static SAPbouiCOM.EditText txtFechaD;
        private static SAPbouiCOM.EditText txtFechaH;
        private static SAPbouiCOM.EditText txtNumOper;
        private static SAPbouiCOM.ComboBox cbxTipoRes;
        private static SAPbouiCOM.EditText txtId;
        private static SAPbouiCOM.ComboBox cbxEstado;
        private static SAPbouiCOM.Button btnFiltrar;
        private static SAPbouiCOM.Button btnAbrir;
        private static SAPbouiCOM.Folder tab01;
        private static SAPbouiCOM.Folder tab02;
        private static SAPbouiCOM.Matrix mtxDocs;
        private static int globalId;
        #endregion

        public frmFacList()
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

                }
                else
                {
                    if (pVal.EventType.Equals(SAPbouiCOM.BoEventTypes.et_DOUBLE_CLICK))
                    {
                        if (pVal.ItemUID.Equals("mtxDocs"))
                        {
                            // Abrir documento seleccionado
                            if (mtxDocs.VisualRowCount > 0)
                            {
                                for (int i = 0; i < mtxDocs.RowCount; i++)
                                {
                                    if (mtxDocs.IsRowSelected(i + 1))
                                    {
                                        string NombreDT = "dtFact";
                                        SAPbouiCOM.DataTable datatable = oForm.DataSources.DataTables.Item(NombreDT);
                                        int Id = (int)datatable.GetValue("U_ID", i);
                                        Comun.FuncionesComunes.LiberarObjetoGenerico(datatable);
                                        try
                                        {
                                            Application.SBO_Application.Forms.Item("frmFactoring").Close();
                                        }
                                        catch
                                        {
                                        }
                                        Formularios.frmFactoring activeForm = new Formularios.frmFactoring(Id);
                                        activeForm.Show();
                                    }
                                }
                            }
                        }
                    }

                    if (pVal.EventType.Equals(SAPbouiCOM.BoEventTypes.et_CLICK))
                    {
                        // Boton filtrar
                        #region filtrar
                        if (pVal.ItemUID.Equals("btnFiltrar"))
                        {
                            // Documentos en factoring
                            CargarMatrixConFiltros();
                            Application.SBO_Application.StatusBar.SetText("Documentos cargados correctamente.", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success);
                        }
                        #endregion
                        // Boton abrir
                        #region abrir
                        if (pVal.ItemUID.Equals("btnAbrir"))
                        {
                            // Abrir documento seleccionado
                            if (mtxDocs.VisualRowCount > 0)
                            {
                                for (int i = 0; i < mtxDocs.RowCount; i++)
                                {
                                    if (mtxDocs.IsRowSelected(i + 1))
                                    {
                                        string NombreDT = "dtFact";
                                        SAPbouiCOM.DataTable datatable = oForm.DataSources.DataTables.Item(NombreDT);
                                        int Id = (int)datatable.GetValue("U_ID", i);
                                        Comun.FuncionesComunes.LiberarObjetoGenerico(datatable);
                                        try
                                        {
                                            Application.SBO_Application.Forms.Item("frmFactoring").Close();
                                        }
                                        catch
                                        {
                                        }
                                        Formularios.frmFactoring activeForm = new Formularios.frmFactoring(Id);
                                        activeForm.Show();
                                    }
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

                        if (pVal.ItemUID.Equals("Id"))
                        {
                            if (oDataTable != null)
                            {
                                oForm.DataSources.UserDataSources.Item("Id").Value = string.Empty;
                                string _Value = string.Empty;
                                _Value = oDataTable.GetValue(0, 0).ToString();
                                oForm.DataSources.UserDataSources.Item("Id").Value = _Value;
                            }
                        }
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
            oForm = ((SAPbouiCOM.Form)(Application.SBO_Application.Forms.Item("frmFacList")));
            txtEntidad = ((SAPbouiCOM.EditText)(GetItem("Entidad").Specific));
            txtMoneda = ((SAPbouiCOM.EditText)(GetItem("Moneda").Specific));
            txtFechaD = ((SAPbouiCOM.EditText)(GetItem("FechaD").Specific));
            txtFechaH = ((SAPbouiCOM.EditText)(GetItem("FechaH").Specific));
            txtNumOper = ((SAPbouiCOM.EditText)(GetItem("NumOper").Specific));
            cbxTipoRes = ((SAPbouiCOM.ComboBox)(GetItem("TipoRes").Specific));
            txtId = ((SAPbouiCOM.EditText)(GetItem("Id").Specific));
            cbxEstado = ((SAPbouiCOM.ComboBox)(GetItem("Estado").Specific));
            btnFiltrar = ((SAPbouiCOM.Button)(GetItem("btnFiltrar").Specific));
            btnAbrir = ((SAPbouiCOM.Button)(GetItem("btnAbrir").Specific));
            tab01 = ((SAPbouiCOM.Folder)(GetItem("tab01").Specific));
            tab02 = ((SAPbouiCOM.Folder)(GetItem("tab02").Specific));
            tab01.Select();
            mtxDocs = ((SAPbouiCOM.Matrix)(GetItem("mtxDocs").Specific));
            //btnMarcar = ((SAPbouiCOM.Button)(GetItem("btnMarcar").Specific));
            //btnAsign = ((SAPbouiCOM.Button)(GetItem("btnAsign").Specific));
            //btnDesas = ((SAPbouiCOM.Button)(GetItem("btnDesas").Specific));
            //btnPrelim = ((SAPbouiCOM.Button)(GetItem("btnPrelim").Specific));
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
            // Vencimiento
            oForm.DataSources.UserDataSources.Add("FechaD", SAPbouiCOM.BoDataType.dt_DATE);
            txtFechaD.DataBind.SetBound(true, "", "FechaD");
            oForm.DataSources.UserDataSources.Add("FechaH", SAPbouiCOM.BoDataType.dt_DATE);
            txtFechaH.DataBind.SetBound(true, "", "FechaH");
            // Número operación
            oForm.DataSources.UserDataSources.Add("NumOper", SAPbouiCOM.BoDataType.dt_SHORT_TEXT);
            txtNumOper.DataBind.SetBound(true, "", "NumOper");
            // Tipo responsabilidad
            oForm.DataSources.UserDataSources.Add("TipoRes", SAPbouiCOM.BoDataType.dt_SHORT_TEXT);
            cbxTipoRes.DataBind.SetBound(true, "", "TipoRes");
            cbxTipoRes.ValidValues.Add("", "");
            cbxTipoRes.ValidValues.Add("CR", "Con responsabilidad");
            cbxTipoRes.ValidValues.Add("SR", "Sin responsabilidad");
            cbxTipoRes.ValidValues.Add("FN", "Financiero");
            //cbxTipoRes.ExpandType = SAPbouiCOM.BoExpandType.et_DescriptionOnly;
            // Id
            oForm.DataSources.UserDataSources.Add("Id", SAPbouiCOM.BoDataType.dt_SHORT_TEXT);
            txtId.DataBind.SetBound(true, "", "Id");
            oCFLs = oForm.ChooseFromLists;
            oCFLCreationParams = ((SAPbouiCOM.ChooseFromListCreationParams)(Application.SBO_Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_ChooseFromListCreationParams)));
            oCFLCreationParams.MultiSelection = false;
            oCFLCreationParams.ObjectType = "SO_FACTORING";
            oCFLCreationParams.UniqueID = "cflFact";
            oCFL = oCFLs.Add(oCFLCreationParams);
            txtId.ChooseFromListUID = "cflFact";
            txtId.ChooseFromListAlias = "DocEntry";
            // Estado
            oForm.DataSources.UserDataSources.Add("Estado", SAPbouiCOM.BoDataType.dt_SHORT_TEXT);
            cbxEstado.DataBind.SetBound(true, "", "Estado");
            cbxEstado.ValidValues.Add("", "");
            cbxEstado.ValidValues.Add("P", "Preliminar");
            cbxEstado.ValidValues.Add("D", "Definitivo");

            oForm.DataSources.DataTables.Add("dtFact");

            EstructuraMatrixDocumentos();
            CargarMatrixConFiltros();

            mtxDocs.SelectionMode = SAPbouiCOM.BoMatrixSelect.ms_Single;
            oForm.Visible = true;
        }

        private static void EstructuraMatrixDocumentos()
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
                'Con responsabilidad' AS ""U_TIPORES"",
                'Preliminar' AS ""U_ESTADO""
                FROM ""@SO_FCTRNG""
                WHERE 1 = 0";
            datatable.ExecuteQuery(_query);

            SAPbouiCOM.Columns oColumns;
            oColumns = mtxDocs.Columns;
            SAPbouiCOM.Column oColumn;

            oColumn = oColumns.Add("Id", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Id Factoring";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 60;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "U_ID");

            oColumn = oColumns.Add("Entidad", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Entidad";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 80;
            oColumn.RightJustified = false;
            oColumn.DataBind.Bind(NombreDT, "U_ENTIDAD");

            oColumn = oColumns.Add("Moneda", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Moneda";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 60;
            oColumn.RightJustified = false;
            oColumn.DataBind.Bind(NombreDT, "U_MONEDA");

            oColumn = oColumns.Add("Valor", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Valor";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 60;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "U_VALOR");

            oColumn = oColumns.Add("Fecha", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Otorgamiento";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 80;
            oColumn.RightJustified = false;
            oColumn.DataBind.Bind(NombreDT, "U_FECHA");

            oColumn = oColumns.Add("NumOper", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "N° Operación";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 80;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "U_NUMOPER");

            oColumn = oColumns.Add("TipoRes", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Tipo responsabilidad";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 80;
            oColumn.RightJustified = false;
            oColumn.DataBind.Bind(NombreDT, "U_TIPORES");

            oColumn = oColumns.Add("Estado", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Estado";
            oColumn.TitleObject.Sortable = true;
            oColumn.Editable = false;
            oColumn.Visible = true;
            oColumn.Width = 80;
            oColumn.RightJustified = false;
            oColumn.DataBind.Bind(NombreDT, "U_ESTADO");

            mtxDocs.Clear();
            mtxDocs.LoadFromDataSource();
            Comun.FuncionesComunes.LiberarObjetoGenerico(datatable);
        }

        private static void CargarMatrixConFiltros()
        {
            // Filtro por Id
            string filtroId = txtId.Value;
            if (!string.IsNullOrEmpty(filtroId))
            {
                filtroId = @" AND ""U_ID"" = '" + filtroId + "'";
            }
            else
            {
                filtroId = string.Empty;
            }
            // Filtro por Estado
            string filtroEstado = cbxEstado.Value;
            if (!string.IsNullOrEmpty(filtroEstado))
            {
                filtroEstado = @" AND ""U_ESTADO"" = '" + filtroEstado + "'";
            }
            else
            {
                filtroEstado = string.Empty;
            }
            // Filtro por Entidad
            string filtroEntidad = txtEntidad.Value;
            if (!string.IsNullOrEmpty(filtroEntidad))
            {
                filtroEntidad = @" AND ""U_ENTIDAD"" = '" + filtroEntidad + "'";
            }
            else
            {
                filtroEntidad = string.Empty;
            }
            // Filtro por Moneda
            string filtroMoneda = txtMoneda.Value;
            if (!string.IsNullOrEmpty(filtroMoneda))
            {
                filtroMoneda = @" AND ""U_MONEDA"" = '" + filtroMoneda + "'";
            }
            else
            {
                filtroMoneda = string.Empty;
            }
            // Filtro por fecha de vencimiento
            string FechaInicial = string.Empty;
            string FechaFinal = string.Empty;
            DateTime dt;
            string Mes = string.Empty;
            string Dia = string.Empty;
            string DesdeFecha = txtFechaD.Value;
            string HastaFecha = txtFechaH.Value;
            string filtroFecha = string.Empty;
            // Fechas en formato AAAA-MM-DD
            if (!string.IsNullOrEmpty(DesdeFecha) && !string.IsNullOrEmpty(HastaFecha))
            {
                FechaInicial = string.Format("{0}{1}{2}", DesdeFecha.Substring(0, 4), DesdeFecha.Substring(4, 2), DesdeFecha.Substring(6, 2));
                FechaFinal = string.Format("{0}{1}{2}", HastaFecha.Substring(0, 4), HastaFecha.Substring(4, 2), HastaFecha.Substring(6, 2));
                filtroFecha = string.Format(@" AND ""U_FECHA"" BETWEEN '{0}' AND '{1}'", FechaInicial, FechaFinal);
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
                filtroFecha = string.Format(@" AND ""U_FECHA"" BETWEEN '{0}' AND '{1}'", FechaInicial, FechaFinal);
            }
            // Filtro por Número operación
            string filtroNumOper = txtNumOper.Value;
            if (!string.IsNullOrEmpty(filtroNumOper))
            {
                filtroNumOper = @" AND ""U_NUMOPER"" = '" + filtroNumOper + "'";
            }
            else
            {
                filtroNumOper = string.Empty;
            }
            // Filtro por Tipo responsabilidad
            string filtroTipoRes = cbxTipoRes.Value;
            if (!string.IsNullOrEmpty(filtroTipoRes))
            {
                filtroTipoRes = @" AND ""U_NUMOPER"" = '" + filtroTipoRes + "'";
            }
            else
            {
                filtroTipoRes = string.Empty;
            }

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
                CASE ""U_TIPORES"" WHEN 'CR' THEN 'Con responsabilidad' WHEN 'SR' THEN 'Sin responsabilidad' WHEN 'FN' THEN 'Financiero' ELSE '' END AS ""U_TIPORES"",
                CASE ""U_ESTADO"" WHEN 'P' THEN 'Preliminar' WHEN 'D' THEN 'Definitivo' ELSE '' END AS ""U_ESTADO""
                FROM ""@SO_FCTRNG""
                WHERE 1 = 1";
            if (string.IsNullOrEmpty(filtroId))
            {
                _query += filtroId + filtroEstado + filtroEntidad + filtroMoneda + filtroFecha + filtroNumOper + filtroTipoRes;
            }
            else
            {
                _query += filtroId;
            }
            datatable.ExecuteQuery(_query);
            oForm.Freeze(true);
            mtxDocs.Clear();
            mtxDocs.LoadFromDataSource();
            Comun.FuncionesComunes.LiberarObjetoGenerico(datatable);
            oForm.Freeze(false);
        }
    }
}
