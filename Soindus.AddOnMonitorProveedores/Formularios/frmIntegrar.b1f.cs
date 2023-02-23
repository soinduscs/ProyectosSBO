using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using SAPbouiCOM.Framework;
using ProvDTE = Soindus.Interfaces.ProveedoresDTE;
using ClasesDTE = Soindus.Clases.DTE;

namespace Soindus.AddOnMonitorProveedores.Formularios
{
    [FormAttribute("Soindus.AddOnMonitorProveedores.Formularios.frmIntegrar", "Formularios/frmIntegrar.b1f")]
    class frmIntegrar : UserFormBase
    {
        // Objetos de formulario
        #region Objetos de formulario
        private static SAPbouiCOM.Form oForm;
        private static SAPbouiCOM.Matrix oMatrixOBJ;
        private static SAPbouiCOM.Matrix oMatrixBAS;
        private static SAPbouiCOM.Matrix oMatrixENT;
        private static SAPbouiCOM.Folder Folder1;
        private static SAPbouiCOM.Folder Folder2;
        private static SAPbouiCOM.Button btnOk;
        private static SAPbouiCOM.Button btnClose;
        private static SAPbouiCOM.StaticText lblTC;
        private static SAPbouiCOM.EditText txtTC;
        private static SAPbouiCOM.StaticText lblMC;
        private static SAPbouiCOM.ComboBox cbxMC;
        private static ClasesDTE.Documento objDTE = null;
        private static String TipoRef = string.Empty;
        private static String Folio = string.Empty;
        private static String CardCode = string.Empty;
        private static String DocEntryBase = string.Empty;
        private static String DocTypeBase = string.Empty;
        private static String BaseType = string.Empty;
        private static Int32 DocEntryUDO = 0;
        private static String DocId = string.Empty;
        private static String MonedaLocal = string.Empty;
        private static String MonedaDocumento = string.Empty;
        private static String MonedaConta = string.Empty;
        private static bool EsMonExtranjera = false;
        private static string SepDecimal;
        private static string SepMiles;
        private static double TipoCambio = 0;
        private static DateTime FechaEmision;
        #endregion

        public frmIntegrar(ClasesDTE.Documento _objDTE, String _TipoRef, String _Folio, String _CardCode, String _DocEntryBase, String _DocTypeBase, String _BaseType, Int32 _DocEntryUDO, string _DocId)
        {
            objDTE = _objDTE;
            //objDTE.Detalle = objDTE.Detalle.Where(x => x.MontoItem != 0 && x.QtyItem > 0).ToArray();
            objDTE.Detalle = objDTE.Detalle.Where(x => x.MontoItem > 0).ToArray();
            TipoRef = _TipoRef;
            Folio = _Folio;
            CardCode = _CardCode;
            DocEntryBase = _DocEntryBase;
            DocTypeBase = _DocTypeBase;
            BaseType = _BaseType;
            DocEntryUDO = _DocEntryUDO;
            DocId = _DocId;

            SepDecimal = SBO.ConsultasSBO.ObtenerSeparadorDecimal();
            SepMiles = SBO.ConsultasSBO.ObtenerSeparadorMiles();

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

        /// <summary>
        /// Ebentos SB1
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
                        // Matrix de detalle de entradas
                        if (pVal.ItemUID.Equals("mtxENT"))
                        {
                            if (pVal.ColUID.Equals("LinkEnt"))
                            {
                                string sObjectType = "20";
                                SAPbouiCOM.Column oColumn = (SAPbouiCOM.Column)oMatrixENT.Columns.Item(pVal.ColUID);
                                SAPbouiCOM.LinkedButton oLink = ((SAPbouiCOM.LinkedButton)(oColumn.ExtendedObject));
                                oLink.LinkedObjectType = sObjectType;
                            }
                        }
                    }
                    #endregion
                }
                else
                {
                    if (pVal.EventType.Equals(SAPbouiCOM.BoEventTypes.et_KEY_DOWN))
                    {
                        // Texto TC
                        #region txtTC
                        if (pVal.ItemUID.Equals("txtTC") && pVal.CharPressed.Equals(9))
                        {
                            NumberFormatInfo provider = new NumberFormatInfo();
                            provider.NumberDecimalSeparator = SepDecimal;
                            provider.NumberGroupSeparator = SepMiles;
                            string stipocambio = oForm.DataSources.UserDataSources.Item("TC").Value;
                            double dtipocambio = Convert.ToDouble(stipocambio, provider);
                            if (dtipocambio <= 0)
                            {
                                TipoCambio = 1;
                            }
                            else
                            {
                                TipoCambio = dtipocambio;
                            }
                            RecalculoMatrixDetalleProveedor();
                        }
                        #endregion
                    }

                    if (pVal.EventType.Equals(SAPbouiCOM.BoEventTypes.et_CLICK))
                    {
                        // Boton integrar
                        #region integrar
                        if (pVal.ItemUID.Equals("btnIntegra"))
                        {
                            if (Folder1.Selected)
                            {
                                var tipoDTE = int.Parse(objDTE.Encabezado.IdDoc.TipoDTE);
                                if(tipoDTE.Equals(33) || tipoDTE.Equals(34) || tipoDTE.Equals(52))
                                {
                                    if (!string.IsNullOrEmpty(TipoRef))
                                    {
                                        long open = SBO.ConsultasSBO.ObtenerReferenciaAbierta(TipoRef, Folio, CardCode, tipoDTE);
                                        if (open.Equals(0))
                                        {
                                            Application.SBO_Application.StatusBar.SetText("Documento base se encuentra cerrado. No se puede integrar", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);
                                            return;
                                        }
                                    }
                                }
                            }
                            MonedaConta = oForm.DataSources.UserDataSources.Item("MC").Value;
                            Local.Message result = new Local.Message();
                            result = ValidarTC();
                            if (result.Success)
                            {
                                oMatrixOBJ.FlushToDataSource();
                                result = ValidarLinDocBaseIngresado();
                                if (result.Success || (TipoRef == null && Folio == null && DocEntryBase == null && DocTypeBase == null && BaseType == null))
                                {
                                    result = ValidarLineNumBaseExiste();
                                    if (result.Success || (TipoRef == null && Folio == null && DocEntryBase == null && DocTypeBase == null && BaseType == null))
                                    {
                                        // Integrar
                                        oForm.Freeze(true);
                                        IntegrarDocumentoSAP();
                                        oForm.Freeze(false);
                                        oForm.Close();
                                    }
                                    else
                                    {
                                        Application.SBO_Application.StatusBar.SetText(result.Mensaje, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);
                                    }
                                }
                                else
                                {
                                    Application.SBO_Application.StatusBar.SetText(result.Mensaje, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);
                                }
                            }
                            else
                            {
                                Application.SBO_Application.StatusBar.SetText(result.Mensaje, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);
                            }
                        }
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
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
            oForm = ((SAPbouiCOM.Form)(Application.SBO_Application.Forms.Item("frmIntegra")));
            oMatrixOBJ = ((SAPbouiCOM.Matrix)(GetItem("mtxOBJ").Specific));
            oMatrixBAS = ((SAPbouiCOM.Matrix)(GetItem("mtxBAS").Specific));
            oMatrixENT = ((SAPbouiCOM.Matrix)(GetItem("mtxENT").Specific));
            Folder1 = ((SAPbouiCOM.Folder)(GetItem("tab01").Specific));
            Folder2 = ((SAPbouiCOM.Folder)(GetItem("tab02").Specific));
            Folder1.Select();
            btnOk = ((SAPbouiCOM.Button)(GetItem("btnIntegra").Specific));
            btnClose = ((SAPbouiCOM.Button)(GetItem("2").Specific));
            lblTC = ((SAPbouiCOM.StaticText)(GetItem("lblTC").Specific));
            txtTC = ((SAPbouiCOM.EditText)(GetItem("txtTC").Specific));
            lblMC = ((SAPbouiCOM.StaticText)(GetItem("lblMC").Specific));
            cbxMC = ((SAPbouiCOM.ComboBox)(GetItem("cbxMC").Specific));
        }

        /// <summary>
        /// Carga formulario
        /// </summary>
        private void CargarFormulario()
        {
            // Verificar si moneda Local es moneda extranjera
            MonedaLocal = SBO.ConsultasSBO.ObtenerMonedaLocal();
            MonedaDocumento = MonedaLocal;
            MonedaConta = MonedaLocal;
            EsMonExtranjera = SBO.ConsultasSBO.EsMonedaExtranjera(MonedaDocumento);

            oForm.DataSources.UserDataSources.Add("TC", SAPbouiCOM.BoDataType.dt_PRICE, 10);
            txtTC.DataBind.SetBound(true, "", "TC");
            oForm.DataSources.UserDataSources.Add("MC", SAPbouiCOM.BoDataType.dt_LONG_TEXT, 3);
            cbxMC.DataBind.SetBound(true, "", "MC");

            // Estrucrura de matrix detalle proveedor
            EstructuraMatrixDetalleProveedor();
            // Estrucrura de matrix detalle documento base
            EstructuraMatrixDetalleDocumentoBase();
            // Estrucrura de matrix detalle entradas
            EstructuraMatrixDetalleEntradas();

            ActivarOpcionesMX();

            if (EsMonExtranjera)
            {
                var oSBObob = (SAPbobsCOM.SBObob)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoBridge);
                var monTC = SBO.ConsultasSBO.ObtenerMonedaTC(MonedaDocumento);
                var Rec = oSBObob.GetCurrencyRate(monTC, FechaEmision);
                var _rate = Convert.ToDouble(Rec.Fields.Item(0).Value.ToString());
                Local.FuncionesComunes.LiberarObjetoGenerico(Rec);
                TipoCambio = _rate;
            }
            else
            {
                TipoCambio = 1;
            }
            //var docrate = string.IsNullOrEmpty(oRecord.Fields.Item(12).Value.ToString()) ? 0 : oRecord.Fields.Item(12).Value;
            //NumberFormatInfo provider = new NumberFormatInfo();
            //provider.NumberDecimalSeparator = SepDecimal;
            //provider.NumberGroupSeparator = SepMiles;
            //string stipocambio = docrate.ToString();
            //double dtipocambio = Convert.ToDouble(stipocambio, provider);
            //TipoCambio = dtipocambio;
            oForm.DataSources.UserDataSources.Item("TC").Value = TipoCambio.ToString();
            RecalculoMatrixDetalleProveedor();

            MonedaConta = MonedaDocumento;
            if (string.IsNullOrEmpty(DocEntryBase))
            {
                MonedaConta = MonedaLocal;
                SAPbobsCOM.Recordset oRecord = (SAPbobsCOM.Recordset)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                string _query = String.Empty;
                _query = @"select distinct(""CurrCode"") as ""Moneda"" from ""OCRN""";
                oRecord.DoQuery(_query);
                while (!oRecord.EoF)
                {
                    cbxMC.ValidValues.Add(oRecord.Fields.Item(0).Value.ToString(), oRecord.Fields.Item(0).Value.ToString());
                    oRecord.MoveNext();
                }
                Local.FuncionesComunes.LiberarObjetoGenerico(oRecord);
            }
            else
            {
                lblMC.Item.Visible = true;
                cbxMC.Item.Enabled = false;
            }
            oForm.DataSources.UserDataSources.Item("MC").Value = MonedaConta.ToString();

            oForm.Visible = true;
        }

        /// <summary>
        /// Genera estructura de Matrix detalle proveedor
        /// </summary>
        private static void EstructuraMatrixDetalleProveedor()
        {
            // Columnas para Rut Emisor, Razón social emisor, Tipo Doc, folio, monto total.
            SAPbouiCOM.Columns oColumns;
            oColumns = oMatrixOBJ.Columns;
            SAPbouiCOM.Column oColumn;

            //oForm.Freeze(true);

            oForm.DataSources.DataTables.Add("DETALLES");
            SAPbouiCOM.DataTable dt = oForm.DataSources.DataTables.Item("DETALLES");

            dt.Columns.Add("co_num", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dt.Columns.Add("co_codItem", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dt.Columns.Add("co_nomItem", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dt.Columns.Add("co_desItem", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dt.Columns.Add("co_cant", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dt.Columns.Add("co_precio", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dt.Columns.Add("co_total", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dt.Columns.Add("co_totalfc", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dt.Columns.Add("co_linBase", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);

            oColumn = oColumns.Add("co_num", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "#";
            oColumn.Editable = false;
            oColumn.Width = 30;
            oColumn.DataBind.Bind("DETALLES", "co_num");

            oColumn = oColumns.Add("co_codItem", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Código item";
            oColumn.Editable = false;
            oColumn.Width = 82;
            oColumn.DataBind.Bind("DETALLES", "co_codItem");

            oColumn = oColumns.Add("co_nomItem", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Nombre item";
            oColumn.Editable = false;
            oColumn.Width = 130;
            oColumn.DataBind.Bind("DETALLES", "co_nomItem");

            oColumn = oColumns.Add("co_desItem", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Descripción item";
            oColumn.Editable = false;
            oColumn.Width = 210;
            oColumn.DataBind.Bind("DETALLES", "co_desItem");

            oColumn = oColumns.Add("co_cant", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Cantidad";
            oColumn.RightJustified = true;
            oColumn.Editable = false;
            oColumn.Width = 80;
            oColumn.DataBind.Bind("DETALLES", "co_cant");

            oColumn = oColumns.Add("co_precio", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Precio";
            oColumn.RightJustified = true;
            oColumn.Editable = false;
            oColumn.Width = 80;
            oColumn.DataBind.Bind("DETALLES", "co_precio");

            oColumn = oColumns.Add("co_total", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Total línea";
            oColumn.RightJustified = true;
            oColumn.Editable = false;
            oColumn.Width = 80;
            oColumn.DataBind.Bind("DETALLES", "co_total");

            oColumn = oColumns.Add("co_totalfc", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Total línea (ME)";
            oColumn.RightJustified = true;
            oColumn.Editable = false;
            oColumn.Width = 80;
            oColumn.DataBind.Bind("DETALLES", "co_totalfc");

            oColumn = oColumns.Add("co_linBase", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Línea documento base";
            oColumn.Editable = true;
            oColumn.Width = 80;
            oColumn.DataBind.Bind("DETALLES", "co_linBase");

            FechaEmision = Convert.ToDateTime(string.Format("{0:YYYY-MM-DD}", objDTE.Encabezado.IdDoc.FchEmis), CultureInfo.CurrentCulture);

            Int32 Index = 0;
            foreach (ClasesDTE.Detalle det in objDTE.Detalle)
            {
                dt.Rows.Add();
                dt.SetValue("co_num", Index, det.NroLinDet);
                string codigoitem = (det.CdgItem == null ? String.Empty : det.CdgItem[0].VlrCodigo);
                dt.SetValue("co_codItem", Index, codigoitem);
                dt.SetValue("co_nomItem", Index, det.NmbItem);
                dt.SetValue("co_desItem", Index, (det.DscItem == null ? String.Empty : (det.DscItem.Length > 100 ? det.DscItem.Substring(0, 100) : det.DscItem)));
                dt.SetValue("co_cant", Index, det.QtyItem.ToString());
                //dt.SetValue("co_precio", Index, det.PrcItem.ToString());
                var precio = string.IsNullOrEmpty(det.PrcItem.ToString()) ? 0 : det.PrcItem;
                dt.SetValue("co_precio", Index, String.Format(System.Globalization.CultureInfo.GetCultureInfo("es-CL"), "{0:C2}", precio));
                //dt.SetValue("co_total", Index, det.MontoItem.ToString());
                var total = string.IsNullOrEmpty(det.MontoItem.ToString()) ? 0 : det.MontoItem;
                dt.SetValue("co_total", Index, String.Format(System.Globalization.CultureInfo.GetCultureInfo("es-CL"), "{0:C2}", total));
                dt.SetValue("co_totalfc", Index, String.Format(System.Globalization.CultureInfo.GetCultureInfo("es-CL"), "{0:N2}", (total / TipoCambio)));
                Index++;
            }
            oMatrixOBJ.LoadFromDataSource();
            oMatrixOBJ.AutoResizeColumns();
        }

        private static void RecalculoMatrixDetalleProveedor()
        {
            oForm.Freeze(true);
            SAPbouiCOM.DataTable dt = oForm.DataSources.DataTables.Item("DETALLES");
            dt.Rows.Clear();
            Int32 Index = 0;
            foreach (ClasesDTE.Detalle det in objDTE.Detalle)
            {
                dt.Rows.Add();
                dt.SetValue("co_num", Index, det.NroLinDet);
                string codigoitem = (det.CdgItem == null ? String.Empty : det.CdgItem[0].VlrCodigo);
                dt.SetValue("co_codItem", Index, codigoitem);
                dt.SetValue("co_nomItem", Index, det.NmbItem);
                dt.SetValue("co_desItem", Index, (det.DscItem == null ? String.Empty : (det.DscItem.Length > 100 ? det.DscItem.Substring(0, 100) : det.DscItem)));
                dt.SetValue("co_cant", Index, det.QtyItem.ToString());
                //dt.SetValue("co_precio", Index, det.PrcItem.ToString());
                var precio = string.IsNullOrEmpty(det.PrcItem.ToString()) ? 0 : det.PrcItem;
                dt.SetValue("co_precio", Index, String.Format(System.Globalization.CultureInfo.GetCultureInfo("es-CL"), "{0:C2}", precio));
                //dt.SetValue("co_total", Index, det.MontoItem.ToString());
                var total = string.IsNullOrEmpty(det.MontoItem.ToString()) ? 0 : det.MontoItem;
                dt.SetValue("co_total", Index, String.Format(System.Globalization.CultureInfo.GetCultureInfo("es-CL"), "{0:C2}", total));
                dt.SetValue("co_totalfc", Index, String.Format(System.Globalization.CultureInfo.GetCultureInfo("es-CL"), "{0:N2}", (total / TipoCambio)));
                Index++;
            }
            oMatrixOBJ.LoadFromDataSource();
            oMatrixOBJ.AutoResizeColumns();
            oForm.Freeze(false);
        }

        /// <summary>
        /// Genera estructura de Matrix detalle documento base
        /// </summary>
        private void EstructuraMatrixDetalleDocumentoBase()
        {
            // Columnas para Rut Emisor, Razón social emisor, Tipo Doc, folio, monto total.
            SAPbouiCOM.Columns oColumns;
            oColumns = oMatrixBAS.Columns;
            SAPbouiCOM.Column oColumn;

            //oForm.Freeze(true);
            oForm.DataSources.DataTables.Add("DETALLESBAS");
            SAPbouiCOM.DataTable dtBase = oForm.DataSources.DataTables.Item("DETALLESBAS");

            dtBase.Columns.Add("co_num", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtBase.Columns.Add("co_lineNum", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtBase.Columns.Add("co_codItem", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtBase.Columns.Add("co_desItem", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtBase.Columns.Add("co_cant", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtBase.Columns.Add("co_cantP", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtBase.Columns.Add("co_curr", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtBase.Columns.Add("co_rate", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtBase.Columns.Add("co_precio", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtBase.Columns.Add("co_total", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtBase.Columns.Add("co_totalfc", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtBase.Columns.Add("co_branch", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);

            oColumn = oColumns.Add("co_num", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "#";
            oColumn.Editable = false;
            oColumn.Width = 20;
            oColumn.DataBind.Bind("DETALLESBAS", "co_num");

            oColumn = oColumns.Add("co_lineNum", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Línea documento base";
            oColumn.Editable = false;
            oColumn.Visible = false;
            oColumn.Width = 80;
            oColumn.DataBind.Bind("DETALLESBAS", "co_lineNum");

            oColumn = oColumns.Add("co_codItem", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Código item";
            oColumn.Editable = false;
            oColumn.Width = 82;
            oColumn.DataBind.Bind("DETALLESBAS", "co_codItem");

            oColumn = oColumns.Add("co_desItem", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Descripción item";
            oColumn.Editable = false;
            oColumn.Width = 210;
            oColumn.DataBind.Bind("DETALLESBAS", "co_desItem");

            oColumn = oColumns.Add("co_cant", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Cantidad";
            oColumn.RightJustified = true;
            oColumn.Editable = false;
            oColumn.Width = 80;
            oColumn.DataBind.Bind("DETALLESBAS", "co_cant");

            oColumn = oColumns.Add("co_cantP", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Cantidad abierta";
            oColumn.RightJustified = true;
            oColumn.Editable = false;
            oColumn.Width = 80;
            oColumn.DataBind.Bind("DETALLESBAS", "co_cantP");

            oColumn = oColumns.Add("co_curr", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Moneda";
            oColumn.RightJustified = true;
            oColumn.Editable = false;
            oColumn.Width = 60;
            oColumn.DataBind.Bind("DETALLESBAS", "co_curr");

            oColumn = oColumns.Add("co_rate", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "TC";
            oColumn.RightJustified = true;
            oColumn.Editable = false;
            oColumn.Width = 60;
            oColumn.DataBind.Bind("DETALLESBAS", "co_rate");

            oColumn = oColumns.Add("co_precio", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Precio";
            oColumn.RightJustified = true;
            oColumn.Editable = false;
            oColumn.Width = 80;
            oColumn.DataBind.Bind("DETALLESBAS", "co_precio");

            oColumn = oColumns.Add("co_total", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Total línea (ML)";
            oColumn.RightJustified = true;
            oColumn.Editable = false;
            oColumn.Width = 80;
            oColumn.DataBind.Bind("DETALLESBAS", "co_total");

            oColumn = oColumns.Add("co_totalfc", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Total línea (ME)";
            oColumn.RightJustified = true;
            oColumn.Editable = false;
            oColumn.Width = 80;
            oColumn.DataBind.Bind("DETALLESBAS", "co_totalfc");

            oColumn = oColumns.Add("co_branch", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Branch";
            oColumn.RightJustified = true;
            oColumn.Editable = false;
            oColumn.Visible = false;
            oColumn.Width = 10;
            oColumn.DataBind.Bind("DETALLESBAS", "co_branch");

            if (!(TipoRef == null && Folio == null && DocEntryBase == null && DocTypeBase == null && BaseType == null))
            {
                // Obtener los detalles del documento base
                SAPbobsCOM.Recordset oRecord = (SAPbobsCOM.Recordset)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                string _query = String.Empty;
                Int64 lFolio = 0;
                Int64.TryParse(Folio, out lFolio);

                if (TipoRef.Equals("801"))
                {
                    // OC - OPOR
                    _query = @"SELECT ""T1"".""ItemCode"", ""T1"".""Dscription"", ""T1"".""Quantity"", ""T1"".""Price"",
                            ""T1"".""LineTotal"", ""T1"".""LineNum"", ""T1"".""OpenQty"",
                            ""T0"".""BPLId"", ""T0"".""DocCur"",
                            ""T1"".""Currency"", ""T1"".""Rate"", ""T1"".""TotalFrgn"", ""T0"".""DocRate"" ";
                    _query += @"FROM ""OPOR"" ""T0"" INNER JOIN ""POR1"" ""T1"" ";
                    _query += @"ON ""T0"".""DocEntry"" = ""T1"".""DocEntry"" ";
                    _query += @"WHERE ""T0"".""CardCode"" = '" + CardCode + @"' AND ""T0"".""DocNum"" = '" + lFolio + "'";
                    EsMonExtranjera = SBO.ConsultasSBO.EsOCMonedaExtranjera(DocEntryBase, "22");
                }
                else if (TipoRef.Equals("33"))
                {
                    // FC - OPCH
                    _query = @"SELECT ""T1"".""ItemCode"", ""T1"".""Dscription"", ""T1"".""Quantity"", ""T1"".""Price"",
                            ""T1"".""LineTotal"", ""T1"".""LineNum"", ""T1"".""OpenQty"",
                            ""T0"".""BPLId"", ""T0"".""DocCur"",
                            ""T1"".""Currency"", ""T1"".""Rate"", ""T1"".""TotalFrgn"", ""T0"".""DocRate"" ";
                    _query += @"FROM ""OPCH"" ""T0"" INNER JOIN ""PCH1"" ""T1"" ";
                    _query += @"ON ""T0"".""DocEntry"" = ""T1"".""DocEntry"" ";
                    _query += @"WHERE ""T0"".""CardCode"" = '" + CardCode + @"' AND ""FolioPref"" = '" + TipoRef + @"' AND ""FolioNum"" = '" + lFolio + "'";
                    EsMonExtranjera = SBO.ConsultasSBO.EsOCMonedaExtranjera(DocEntryBase, "13");
                }
                oRecord.DoQuery(_query);

                Int32 Index = 0;
                while (!oRecord.EoF)
                {
                    dtBase.Rows.Add();
                    dtBase.SetValue("co_num", Index, Index + 1);
                    dtBase.SetValue("co_codItem", Index, oRecord.Fields.Item(0).Value.ToString());
                    dtBase.SetValue("co_desItem", Index, (oRecord.Fields.Item(1).Value.ToString() == null ? String.Empty : oRecord.Fields.Item(1).Value.ToString()));
                    dtBase.SetValue("co_cant", Index, oRecord.Fields.Item(2).Value.ToString());
                    //dtBase.SetValue("co_precio", Index, oRecord.Fields.Item(3).Value.ToString());
                    var precio = string.IsNullOrEmpty(oRecord.Fields.Item(3).Value.ToString()) ? 0 : oRecord.Fields.Item(3).Value;
                    //dtBase.SetValue("co_precio", Index, String.Format(System.Globalization.CultureInfo.GetCultureInfo("es-CL"), "{0:C2}", precio));
                    dtBase.SetValue("co_precio", Index, String.Format(System.Globalization.CultureInfo.GetCultureInfo("es-CL"), "{0:N2}", precio));
                    //dtBase.SetValue("co_total", Index, oRecord.Fields.Item(4).Value.ToString());
                    var total = string.IsNullOrEmpty(oRecord.Fields.Item(4).Value.ToString()) ? 0 : oRecord.Fields.Item(4).Value;
                    //dtBase.SetValue("co_total", Index, String.Format(System.Globalization.CultureInfo.GetCultureInfo("es-CL"), "{0:C2}", total));
                    dtBase.SetValue("co_total", Index, String.Format(System.Globalization.CultureInfo.GetCultureInfo("es-CL"), "{0:N2}", total));
                    dtBase.SetValue("co_lineNum", Index, oRecord.Fields.Item(5).Value.ToString());
                    dtBase.SetValue("co_cantP", Index, oRecord.Fields.Item(6).Value.ToString());
                    dtBase.SetValue("co_branch", Index, oRecord.Fields.Item(7).Value.ToString());
                    MonedaDocumento = oRecord.Fields.Item(8).Value.ToString();
                    dtBase.SetValue("co_curr", Index, oRecord.Fields.Item(9).Value.ToString());
                    //dtBase.SetValue("co_rate", Index, oRecord.Fields.Item(10).Value.ToString());
                    var rate = string.IsNullOrEmpty(oRecord.Fields.Item(10).Value.ToString()) ? 0 : oRecord.Fields.Item(10).Value;
                    dtBase.SetValue("co_rate", Index, String.Format(System.Globalization.CultureInfo.GetCultureInfo("es-CL"), "{0:N2}", rate));
                    //dtBase.SetValue("co_totalfc", Index, oRecord.Fields.Item(11).Value.ToString());
                    var totalfc = string.IsNullOrEmpty(oRecord.Fields.Item(11).Value.ToString()) ? 0 : oRecord.Fields.Item(11).Value;
                    dtBase.SetValue("co_totalfc", Index, String.Format(System.Globalization.CultureInfo.GetCultureInfo("es-CL"), "{0:N2}", totalfc));
                    Index++;
                    oRecord.MoveNext();
                }
            }
            oMatrixBAS.LoadFromDataSource();
            oMatrixBAS.AutoResizeColumns();
        }

        /// <summary>
        /// Genera estructura de Matrix detalle documento base
        /// </summary>
        private void EstructuraMatrixDetalleEntradas()
        {
            // Columnas para Rut Emisor, Razón social emisor, Tipo Doc, folio, monto total.
            SAPbouiCOM.Columns oColumns;
            oColumns = oMatrixENT.Columns;
            SAPbouiCOM.Column oColumn;

            //oForm.Freeze(true);
            oForm.DataSources.DataTables.Add("DETALLESENT");
            SAPbouiCOM.DataTable dtBase = oForm.DataSources.DataTables.Item("DETALLESENT");

            dtBase.Columns.Add("DocEntry", SAPbouiCOM.BoFieldsType.ft_Integer);
            dtBase.Columns.Add("DocNum", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtBase.Columns.Add("LinkEnt", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtBase.Columns.Add("co_num", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtBase.Columns.Add("co_lineNum", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtBase.Columns.Add("co_codItem", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtBase.Columns.Add("co_desItem", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtBase.Columns.Add("co_cant", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtBase.Columns.Add("co_cantP", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtBase.Columns.Add("co_curr", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtBase.Columns.Add("co_rate", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtBase.Columns.Add("co_precio", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtBase.Columns.Add("co_total", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtBase.Columns.Add("co_totalfc", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtBase.Columns.Add("co_branch", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);

            oColumn = oColumns.Add("DocEntry", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "DocEntry";
            oColumn.Editable = false;
            oColumn.Visible = false;
            oColumn.Width = 60;
            oColumn.DataBind.Bind("DETALLESENT", "DocEntry");

            oColumn = oColumns.Add("DocNum", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "EM";
            oColumn.Editable = false;
            oColumn.Width = 30;
            oColumn.DataBind.Bind("DETALLESENT", "DocNum");

            oColumn = oColumns.Add("LinkEnt", SAPbouiCOM.BoFormItemTypes.it_LINKED_BUTTON);
            oColumn.TitleObject.Caption = "Link EM";
            oColumn.Editable = false;
            oColumn.Width = 30;
            oColumn.DataBind.Bind("DETALLESENT", "LinkEnt");

            oColumn = oColumns.Add("co_num", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "#";
            oColumn.Editable = false;
            oColumn.Width = 20;
            oColumn.DataBind.Bind("DETALLESENT", "co_num");

            oColumn = oColumns.Add("co_lineNum", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Línea documento base";
            oColumn.Editable = false;
            oColumn.Visible = false;
            oColumn.Width = 80;
            oColumn.DataBind.Bind("DETALLESENT", "co_lineNum");

            oColumn = oColumns.Add("co_codItem", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Código item";
            oColumn.Editable = false;
            oColumn.Width = 82;
            oColumn.DataBind.Bind("DETALLESENT", "co_codItem");

            oColumn = oColumns.Add("co_desItem", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Descripción item";
            oColumn.Editable = false;
            oColumn.Width = 210;
            oColumn.DataBind.Bind("DETALLESENT", "co_desItem");

            oColumn = oColumns.Add("co_cant", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Cantidad";
            oColumn.RightJustified = true;
            oColumn.Editable = false;
            oColumn.Width = 80;
            oColumn.DataBind.Bind("DETALLESENT", "co_cant");

            oColumn = oColumns.Add("co_cantP", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Cantidad abierta";
            oColumn.RightJustified = true;
            oColumn.Editable = false;
            oColumn.Width = 80;
            oColumn.DataBind.Bind("DETALLESENT", "co_cantP");

            oColumn = oColumns.Add("co_curr", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Moneda";
            oColumn.RightJustified = true;
            oColumn.Editable = false;
            oColumn.Width = 60;
            oColumn.DataBind.Bind("DETALLESENT", "co_curr");

            oColumn = oColumns.Add("co_rate", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "TC";
            oColumn.RightJustified = true;
            oColumn.Editable = false;
            oColumn.Width = 60;
            oColumn.DataBind.Bind("DETALLESENT", "co_rate");

            oColumn = oColumns.Add("co_precio", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Precio";
            oColumn.RightJustified = true;
            oColumn.Editable = false;
            oColumn.Width = 80;
            oColumn.DataBind.Bind("DETALLESENT", "co_precio");

            oColumn = oColumns.Add("co_total", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Total línea (ML)";
            oColumn.RightJustified = true;
            oColumn.Editable = false;
            oColumn.Width = 80;
            oColumn.DataBind.Bind("DETALLESENT", "co_total");

            oColumn = oColumns.Add("co_totalfc", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Total línea (ME)";
            oColumn.RightJustified = true;
            oColumn.Editable = false;
            oColumn.Width = 80;
            oColumn.DataBind.Bind("DETALLESENT", "co_totalfc");

            oColumn = oColumns.Add("co_branch", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Branch";
            oColumn.RightJustified = true;
            oColumn.Editable = false;
            oColumn.Visible = false;
            oColumn.Width = 10;
            oColumn.DataBind.Bind("DETALLESENT", "co_branch");

            if (!(TipoRef == null && Folio == null && DocEntryBase == null && DocTypeBase == null && BaseType == null))
            {
                // Obtener los detalles del documento base
                SAPbobsCOM.Recordset oRecord = (SAPbobsCOM.Recordset)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                string _query = String.Empty;
                Int64 lFolio = 0;
                Int64.TryParse(Folio, out lFolio);

                //// Guia - OPDN
                _query = @"SELECT ""T1"".""ItemCode"", ""T1"".""Dscription"", ""T1"".""Quantity"", ""T1"".""Price"",
                        ""T1"".""LineTotal"", ""T1"".""LineNum"", ""T1"".""OpenQty"",
                        ""T0"".""DocEntry"", ""T0"".""DocNum"",
                        ""T0"".""BPLId"", ""T0"".""DocCur"",
                        ""T1"".""Currency"", ""T1"".""Rate"", ""T1"".""TotalFrgn"" ";
                _query += @"FROM ""OPDN"" ""T0"" INNER JOIN ""PDN1"" ""T1"" ";
                _query += @"ON ""T0"".""DocEntry"" = ""T1"".""DocEntry"" ";
                _query += @"WHERE ""T0"".""DocStatus""='O' AND ""T1"".""BaseType"" = '22' AND ""T1"".""BaseEntry"" = " + DocEntryBase + "";
                //EsMonExtranjera = SBO.ConsultasSBO.EsOCMonedaExtranjera(DocEntryBase, "20");
                oRecord.DoQuery(_query);

                Int32 Index = 0;
                Int32 IndexAux = 0;
                Int32 DocEntry = 0;
                while (!oRecord.EoF)
                {
                    dtBase.Rows.Add();

                    dtBase.SetValue("DocEntry", Index, oRecord.Fields.Item(7).Value);
                    if (!DocEntry.Equals(oRecord.Fields.Item(7).Value))
                    {
                        DocEntry = (Int32)oRecord.Fields.Item(7).Value;
                        dtBase.SetValue("DocNum", Index, oRecord.Fields.Item(8).Value.ToString());
                        dtBase.SetValue("LinkEnt", Index, oRecord.Fields.Item(7).Value.ToString());
                        //MonedaLocal = oRecord.Fields.Item(10).Value.ToString();
                    }
                    else
                    {
                        dtBase.SetValue("LinkEnt", Index, String.Empty);
                        dtBase.SetValue("co_codItem", Index, oRecord.Fields.Item(0).Value.ToString());
                        dtBase.SetValue("co_num", Index, IndexAux + 1);
                        dtBase.SetValue("co_codItem", Index, oRecord.Fields.Item(0).Value.ToString());
                        dtBase.SetValue("co_desItem", Index, (oRecord.Fields.Item(1).Value.ToString() == null ? String.Empty : oRecord.Fields.Item(1).Value.ToString()));
                        dtBase.SetValue("co_cant", Index, oRecord.Fields.Item(2).Value.ToString());
                        //dtBase.SetValue("co_precio", Index, oRecord.Fields.Item(3).Value.ToString());
                        var precio = string.IsNullOrEmpty(oRecord.Fields.Item(3).Value.ToString()) ? 0 : oRecord.Fields.Item(3).Value;
                        //dtBase.SetValue("co_precio", Index, String.Format(System.Globalization.CultureInfo.GetCultureInfo("es-CL"), "{0:C2}", precio));
                        dtBase.SetValue("co_precio", Index, String.Format(System.Globalization.CultureInfo.GetCultureInfo("es-CL"), "{0:N2}", precio));
                        //dtBase.SetValue("co_total", Index, oRecord.Fields.Item(4).Value.ToString());
                        var total = string.IsNullOrEmpty(oRecord.Fields.Item(4).Value.ToString()) ? 0 : oRecord.Fields.Item(4).Value;
                        //dtBase.SetValue("co_total", Index, String.Format(System.Globalization.CultureInfo.GetCultureInfo("es-CL"), "{0:C2}", total));
                        dtBase.SetValue("co_total", Index, String.Format(System.Globalization.CultureInfo.GetCultureInfo("es-CL"), "{0:N2}", total));
                        dtBase.SetValue("co_lineNum", Index, oRecord.Fields.Item(5).Value.ToString());
                        dtBase.SetValue("co_cantP", Index, oRecord.Fields.Item(6).Value.ToString());
                        dtBase.SetValue("co_branch", Index, oRecord.Fields.Item(9).Value.ToString());
                        //MonedaLocal = oRecord.Fields.Item(10).Value.ToString();
                        dtBase.SetValue("co_curr", Index, oRecord.Fields.Item(11).Value.ToString());
                        //dtBase.SetValue("co_rate", Index, oRecord.Fields.Item(12).Value.ToString());
                        var rate = string.IsNullOrEmpty(oRecord.Fields.Item(12).Value.ToString()) ? 0 : oRecord.Fields.Item(12).Value;
                        dtBase.SetValue("co_rate", Index, String.Format(System.Globalization.CultureInfo.GetCultureInfo("es-CL"), "{0:N2}", rate));
                        //dtBase.SetValue("co_totalfc", Index, oRecord.Fields.Item(13).Value.ToString());
                        var totalfc = string.IsNullOrEmpty(oRecord.Fields.Item(13).Value.ToString()) ? 0 : oRecord.Fields.Item(13).Value;
                        dtBase.SetValue("co_totalfc", Index, String.Format(System.Globalization.CultureInfo.GetCultureInfo("es-CL"), "{0:N2}", totalfc));
                        IndexAux++;
                        oRecord.MoveNext();
                    }
                    Index++;
                }
            }
            oMatrixENT.LoadFromDataSource();
            oMatrixENT.AutoResizeColumns();
        }

        /// <summary>
        /// Integra documentos en SBO
        /// </summary>
        private static void IntegrarDocumentoSAP()
        {
            SAPbobsCOM.GeneralService oGeneralService = null;
            SAPbobsCOM.GeneralData oGeneralData = null;
            SAPbobsCOM.GeneralData oChild = null;
            SAPbobsCOM.GeneralDataCollection oChildren = null;
            SAPbobsCOM.GeneralDataParams oGeneralParams = null;
            SAPbobsCOM.CompanyService oCompanyService = null;

            String NuevoDocumento = string.Empty;
            String CardCodeSN = String.Empty;
            String DocEntryDocBase = String.Empty;
            String BaseTypeDocBase = String.Empty;
            string Branch = "1";
            CardCodeSN = CardCode;
            DocEntryDocBase = DocEntryBase;
            BaseTypeDocBase = BaseType;

            TipoCambio = TipoCambio <= 0 ? 1 : TipoCambio;

            SAPbobsCOM.Documents oDoc = null;
            oDoc = (SAPbobsCOM.Documents)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oDrafts);

            // Codigo IVA Exento
            string CodigoIVAEXE = SBO.ConsultasSBO.ObtenerCodigoIVAEXE();

            // Encabezado
            oDoc.CardCode = CardCode;
            oDoc.CardName = SBO.ConsultasSBO.ObtenerCardName(objDTE.Encabezado.Emisor.RUTEmisor, CardCode);
            switch (objDTE.Encabezado.IdDoc.TipoDTE)
            {
                case "33":
                    oDoc.DocObjectCode = SAPbobsCOM.BoObjectTypes.oPurchaseInvoices;
                    oDoc.Indicator = objDTE.Encabezado.IdDoc.TipoDTE;
                    break;
                case "34":
                    oDoc.DocObjectCode = SAPbobsCOM.BoObjectTypes.oPurchaseInvoices;
                    oDoc.Indicator = objDTE.Encabezado.IdDoc.TipoDTE;
                    break;
                case "43":
                    oDoc.DocObjectCode = SAPbobsCOM.BoObjectTypes.oPurchaseInvoices;
                    oDoc.Indicator = objDTE.Encabezado.IdDoc.TipoDTE;
                    break;
                case "52":
                    oDoc.DocObjectCode = SAPbobsCOM.BoObjectTypes.oPurchaseDeliveryNotes;
                    oDoc.Indicator = objDTE.Encabezado.IdDoc.TipoDTE;
                    break;
                case "56":
                    oDoc.DocObjectCode = SAPbobsCOM.BoObjectTypes.oPurchaseInvoices;
                    oDoc.DocumentSubType = SAPbobsCOM.BoDocumentSubType.bod_PurchaseDebitMemo;
                    oDoc.Indicator = objDTE.Encabezado.IdDoc.TipoDTE;
                    break;
                case "61":
                    oDoc.DocObjectCode = SAPbobsCOM.BoObjectTypes.oPurchaseCreditNotes;
                    oDoc.Indicator = objDTE.Encabezado.IdDoc.TipoDTE;
                    break;
                default:
                    oDoc.DocObjectCode = SAPbobsCOM.BoObjectTypes.oPurchaseInvoices;
                    oDoc.Indicator = objDTE.Encabezado.IdDoc.TipoDTE;
                    break;
            }

            oDoc.DocDate = Convert.ToDateTime(objDTE.Encabezado.IdDoc.FchEmis);
            oDoc.DocDueDate = Convert.ToDateTime(objDTE.Encabezado.IdDoc.FchVenc);
            oDoc.FolioNumber = Convert.ToInt32(objDTE.Encabezado.IdDoc.Folio);
            oDoc.FolioPrefixString = objDTE.Encabezado.IdDoc.TipoDTE;
            switch (DocTypeBase)
            {
                case "I":
                    oDoc.DocType = SAPbobsCOM.BoDocumentTypes.dDocument_Items;
                    break;
                case "S":
                    oDoc.DocType = SAPbobsCOM.BoDocumentTypes.dDocument_Service;
                    break;
                default:
                    oDoc.DocType = SAPbobsCOM.BoDocumentTypes.dDocument_Service;
                    break;
            }

            // Detalle
            Int32 indexDet = 0;
            Int32 NumeroDetalle = 0;
            String LineNum = String.Empty;
            String LineNumBase = String.Empty;
            String VisualLineNum = string.Empty;

            foreach (ClasesDTE.Detalle det in objDTE.Detalle)
            {
                if (indexDet != 0)
                {
                    oDoc.Lines.Add();
                }

                if(!(TipoRef == null && Folio == null && DocEntryBase == null && DocTypeBase == null && BaseType == null))
                {
                    // Para ItemCode, primero buscar el LineNum del documento base de ese numero de linea de detalle
                    // Recorrer Matrix y validar
                    SAPbouiCOM.DataTable dtDetOBJ = oForm.DataSources.DataTables.Item("DETALLES");
                    for (Int32 index = 0; index < dtDetOBJ.Rows.Count; index++)
                    {
                        NumeroDetalle = Int32.Parse(dtDetOBJ.GetValue("co_num", index).ToString());
                        if (NumeroDetalle.Equals(det.NroLinDet))
                        {
                            LineNum = dtDetOBJ.GetValue("co_linBase", index).ToString();
                            break;
                        }
                    }

                    // Con el Line Num del documento base ya establecido, obtener el codigo de item de la matrix base
                    if (Folder1.Selected)
                    {
                        SAPbouiCOM.DataTable dtDetBAS = oForm.DataSources.DataTables.Item("DETALLESBAS");
                        for (Int32 index = 0; index < dtDetBAS.Rows.Count; index++)
                        {
                            VisualLineNum = dtDetBAS.GetValue("co_num", index).ToString();
                            Branch= dtDetBAS.GetValue("co_branch", index).ToString();
                            if (VisualLineNum.Equals(LineNum))
                            {
                                LineNumBase = dtDetBAS.GetValue("co_lineNum", index).ToString();
                                det.ItemCode = dtDetBAS.GetValue("co_codItem", index).ToString();
                                det.NmbItem = dtDetBAS.GetValue("co_desItem", index).ToString();
                                break;
                            }
                        }
                    }
                    if (Folder2.Selected)
                    {
                        SAPbouiCOM.DataTable dtDetENT = oForm.DataSources.DataTables.Item("DETALLESENT");
                        for (Int32 index = 0; index < dtDetENT.Rows.Count; index++)
                        {
                            VisualLineNum = dtDetENT.GetValue("co_num", index).ToString();
                            Branch = dtDetENT.GetValue("co_branch", index).ToString();
                            if (VisualLineNum.Equals(LineNum))
                            {
                                LineNumBase = dtDetENT.GetValue("co_lineNum", index).ToString();
                                DocEntryDocBase = dtDetENT.GetValue("DocEntry", index).ToString();
                                BaseTypeDocBase = "20";
                                det.ItemCode = dtDetENT.GetValue("co_codItem", index).ToString();
                                det.NmbItem = dtDetENT.GetValue("co_desItem", index).ToString();
                                break;
                            }
                        }
                    }
                }

                switch (DocTypeBase)
                {
                    case "I":
                        oDoc.Lines.ItemCode = det.ItemCode;
                        oDoc.Lines.ItemDescription = det.NmbItem;
                        oDoc.Lines.Quantity = det.QtyItem;
                        break;
                    case "S":
                        oDoc.Lines.ItemDescription = det.NmbItem;
                        oDoc.Lines.Quantity = 1;
                        break;
                    default:
                        oDoc.Lines.ItemDescription = det.NmbItem;
                        oDoc.Lines.Quantity = 1;
                        break;
                }
                oDoc.Lines.Price = (det.PrcItem / TipoCambio);
                if (!MonedaConta.Equals(MonedaLocal))
                {
                    oDoc.Lines.Currency = MonedaConta;
                    oDoc.Lines.RowTotalFC = (det.MontoItem / TipoCambio);
                    if (!TipoCambio.Equals(1))
                    {
                        oDoc.Lines.Rate = TipoCambio;
                    }
                }
                else if (MonedaConta.Equals(MonedaLocal))
                {
                    oDoc.Lines.LineTotal = (det.MontoItem / TipoCambio);
                }
                if (!(TipoRef == null && Folio == null && DocEntryBase == null && DocTypeBase == null && BaseType == null))
                {
                    oDoc.Lines.BaseEntry = Int32.Parse(DocEntryDocBase);
                    oDoc.Lines.BaseLine = Int32.Parse(LineNumBase);
                    oDoc.Lines.BaseType = Int32.Parse(BaseTypeDocBase);
                }
                oDoc.Lines.TaxCode = "IVA";
                if (objDTE.Encabezado.IdDoc.TipoDTE.Equals("34"))
                {
                    oDoc.Lines.TaxCode = CodigoIVAEXE;
                }
                else if (objDTE.Encabezado.Totales.MntExe.Equals(objDTE.Encabezado.Totales.MntTotal))
                {
                    oDoc.Lines.TaxCode = CodigoIVAEXE;
                }
                else if (det.IndExe.Equals(1))
                {
                    oDoc.Lines.TaxCode = CodigoIVAEXE;
                }
                oDoc.Lines.DiscountPercent = det.DescuentoPct;
                indexDet++;
            }

            if (!MonedaConta.Equals(MonedaLocal))
            {
                oDoc.DocCurrency = MonedaConta;
                oDoc.DocTotalFc = (objDTE.Encabezado.Totales.MntTotal / TipoCambio);
                if (!TipoCambio.Equals(1))
                {
                    oDoc.DocRate = TipoCambio;
                }
            }
            else if (MonedaConta.Equals(MonedaLocal))
            {
                oDoc.DocTotal = (objDTE.Encabezado.Totales.MntTotal / TipoCambio);
            }

            if (SBO.ConsultasSBO.MultiBranchActivo())
            {
                oDoc.BPL_IDAssignedToInvoice = Convert.ToInt32(Branch);
            }

            SAPbobsCOM.CompanyService com_service = SBO.ConexionSBO.oCompany.GetCompanyService();
            SAPbobsCOM.PathAdmin oPathAdmin = com_service.GetPathAdmin();
            String Path = string.Empty;
            Path = oPathAdmin.AttachmentsFolderPath;
            int indexPath = Path.LastIndexOf("\\");
            if (indexPath > 0)
            {
                Path = Path.Substring(0, indexPath);
            }
            String fileName = string.Empty;
            String fileExt = string.Empty;

            try
            {
                ProveedorDTE proveedorDTE = new ProveedorDTE();

                string[] parametros = new string[] { DocId };

                var provResult = proveedorDTE.ObtenerPDFDocumento(parametros);

                if (provResult.Success)
                {
                    var _Datos = proveedorDTE.DetalleDocuDTE;
                    String link = _Datos.ImagenLink.ToString();

                    String[] linkSplit = link.Split('/');
                    fileName = linkSplit[linkSplit.Length - 1];
                    int indexExt = fileName.LastIndexOf(".");
                    if (indexExt > 0)
                    {
                        fileExt = fileName.Substring(indexExt + 1);
                        fileName = fileName.Substring(0, indexExt);
                    }

                    // Descargar PDF
                    try
                    {
                        if (link.LastIndexOf("://") > 0)
                        {
                            using (System.Net.WebClient client = new System.Net.WebClient())
                            {
                                client.DownloadFile(link, @"" + Path + @"\\" + fileName + @"." + fileExt);
                            }
                        }
                    }
                    // PDF en directorio
                    catch
                    {
                    }
                }
                // Validación obtención de documento
                else
                {
                    Application.SBO_Application.StatusBar.SetText(provResult.Mensaje, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);
                }
            }
            catch (Exception ex)
            {
                Application.SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);
            }

            SAPbobsCOM.Attachments2 oAtt = null;
            oAtt = (SAPbobsCOM.Attachments2)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oAttachments2);

            oAtt.Lines.Add();
            oAtt.Lines.FileName = fileName;
            oAtt.Lines.FileExtension = fileExt;
            oAtt.Lines.SourcePath = Path;
            oAtt.Lines.Override = SAPbobsCOM.BoYesNoEnum.tYES;

            Int32 RetAtt = oAtt.Add();
            if (RetAtt != 0)
            {
                Int32 ErrCode = 0;
                String ErrMsj = String.Empty;
                SBO.ConexionSBO.oCompany.GetLastError(out ErrCode, out ErrMsj);
            }
            else
            {
                oAtt.GetByKey(int.Parse(SBO.ConexionSBO.oCompany.GetNewObjectKey()));
                oDoc.AttachmentEntry = oAtt.AbsoluteEntry;
            }

            // Adjuntar OC
            //SAPbobsCOM.ReportLayoutsService oReportLayoutService;
            //SAPbobsCOM.ReportLayoutPrintParams oPrintParam;
            //oReportLayoutService = (SAPbobsCOM.ReportLayoutsService)com_service.GetBusinessService(SAPbobsCOM.ServiceTypes.ReportLayoutsService);
            //oPrintParam = (SAPbobsCOM.ReportLayoutPrintParams)oReportLayoutService.GetDataInterface(SAPbobsCOM.ReportLayoutsServiceDataInterfaces.rlsdiReportLayoutPrintParams);
            //oPrintParam.LayoutCode = "POR10003";
            //oPrintParam.DocEntry = Int32.Parse(DocEntryDocBase);
            //oReportLayoutService.Print(oPrintParam);



            Int32 RetVal = oDoc.Add();
            String Mensaje = String.Empty;
            if (RetVal.Equals(0))
            {
                NuevoDocumento = SBO.ConexionSBO.oCompany.GetNewObjectKey();
                Mensaje = String.Format("Documento número {0} de proveedor {1} integrado correctamente como preliminar {2}.", objDTE.Encabezado.IdDoc.Folio, CardCodeSN, NuevoDocumento);
                Application.SBO_Application.StatusBar.SetText(Mensaje, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success);

                // Cambiar estado a integrado
                try
                {
                    oCompanyService = SBO.ConexionSBO.oCompany.GetCompanyService();
                    // Get GeneralService (oCmpSrv is the CompanyService)
                    oGeneralService = oCompanyService.GetGeneralService("SO_MONITOR");
                    // Create data for new row in main UDO
                    oGeneralParams = ((SAPbobsCOM.GeneralDataParams)(oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)));
                    oGeneralParams.SetProperty("DocEntry", DocEntryUDO);
                    oGeneralData = oGeneralService.GetByParams(oGeneralParams);
                    oGeneralData.SetProperty("U_PRELIM", NuevoDocumento);
                    oGeneralData.SetProperty("U_ESTADO", "5");
                    oGeneralService.Update(oGeneralData);
                    //dtDoc.SetValue("co_obs", i, String.Format("OK. Documento preliminar {0}.", NuevoDocumento));
                }
                catch (Exception ex)
                {
                    //dtDoc.SetValue("co_obs", i, ex.Message);
                    Application.SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);
                }
            }
            else
            {
                Int32 ErrCode = 0;
                String ErrMsj = String.Empty;
                SBO.ConexionSBO.oCompany.GetLastError(out ErrCode, out ErrMsj);
                Mensaje = String.Format("Documento número {0} de proveedor {1} no integrado: {2}", objDTE.Encabezado.IdDoc.Folio, CardCodeSN, ErrMsj);
                Application.SBO_Application.StatusBar.SetText(Mensaje, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);
            }
            //return NuevoDocumento;
        }

        /// <summary>
        /// Validar que número de línea de documento base se ingresen por pantalla
        /// </summary>
        private static Local.Message ValidarLinDocBaseIngresado()
        {
            Local.Message result = new Local.Message();
            result.Success = true;
            SAPbouiCOM.DataTable dtDetOBJ = oForm.DataSources.DataTables.Item("DETALLES");
            String LineNum = String.Empty;

            // Recorrer Matrix y validar
            for (Int32 index = 0; index < dtDetOBJ.Rows.Count; index++)
            {
                LineNum = dtDetOBJ.GetValue("co_linBase", index).ToString();
                if (String.IsNullOrEmpty(LineNum))
                {
                    result.Success = false;
                    result.Mensaje = "Número de línea de documento base es obligatorio";
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// Valida que Line Num ingresado exista en documento base
        /// </summary>
        private static Local.Message ValidarLineNumBaseExiste()
        {
            Local.Message result = new Local.Message();
            result.Success = true;
            String[] arrayLineNum = null;
            String LineNum = String.Empty;

            if (Folder1.Selected)
            {
                SAPbouiCOM.DataTable dtDetBAS = oForm.DataSources.DataTables.Item("DETALLESBAS");
                arrayLineNum = new String[dtDetBAS.Rows.Count];

                // Recorrer Matrix base y agregar LineNum
                for (Int32 index = 0; index < dtDetBAS.Rows.Count; index++)
                {
                    arrayLineNum[index] = dtDetBAS.GetValue("co_num", index).ToString();
                }
            }

            if (Folder2.Selected)
            {
                SAPbouiCOM.DataTable dtDetENT = oForm.DataSources.DataTables.Item("DETALLESENT");
                arrayLineNum = new String[dtDetENT.Rows.Count];

                // Recorrer Matrix base y agregar LineNum
                for (Int32 index = 0; index < dtDetENT.Rows.Count; index++)
                {
                    arrayLineNum[index] = dtDetENT.GetValue("co_num", index).ToString();
                }
            }

            // Validar que número de línea exista en documento base
            SAPbouiCOM.DataTable dtDetOBJ = oForm.DataSources.DataTables.Item("DETALLES");
            for (Int32 index = 0; index < dtDetOBJ.Rows.Count; index++)
            {
                LineNum = dtDetOBJ.GetValue("co_linBase", index).ToString();
                if (!arrayLineNum.Contains(LineNum))
                {
                    result.Success = false;
                    if (Folder1.Selected)
                    {
                        result.Mensaje = String.Format("Número de línea {0} no se encuentra en detalle de documento base.", LineNum);
                    }
                    if (Folder2.Selected)
                    {
                        result.Mensaje = String.Format("Número de línea {0} no se encuentra en detalle de entradas.", LineNum);
                    }
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// Validar ingreso tipo de cambio para documentos base con moneda extranjera
        /// </summary>
        private static Local.Message ValidarTC()
        {
            Local.Message result = new Local.Message();
            result.Success = true;
            NumberFormatInfo provider = new NumberFormatInfo();
            provider.NumberDecimalSeparator = SepDecimal;
            provider.NumberGroupSeparator = SepMiles;
            string stipocambio = oForm.DataSources.UserDataSources.Item("TC").Value;
            double dtipocambio = Convert.ToDouble(stipocambio, provider);

            if (EsMonExtranjera)
            {
                if (dtipocambio <= 0)
                {
                    result.Success = false;
                    result.Mensaje = "Tipo de cambio de documento base es obligatorio";
                }
                else
                {
                    TipoCambio = dtipocambio;
                }
            }
            else
            {
                if (dtipocambio <= 0)
                {
                    TipoCambio = 1;
                }
                else
                {
                    TipoCambio = dtipocambio;
                }
            }
            return result;
        }

        private static void ActivarOpcionesMX()
        {
            var usuario = SBO.ConexionSBO.oCompany.UserName;
            var esusuariomx = SBO.ConsultasSBO.EsUsuarioMX(usuario);
            if (esusuariomx || EsMonExtranjera)
            {
                lblTC.Caption = string.Format("Tipo de cambio {0}", MonedaDocumento);
                lblTC.Item.Visible = true;
                txtTC.Item.Visible = true;
            }
            else
            {
                lblTC.Item.Visible = false;
                txtTC.Item.Visible = false;
            }
        }
    }
}
