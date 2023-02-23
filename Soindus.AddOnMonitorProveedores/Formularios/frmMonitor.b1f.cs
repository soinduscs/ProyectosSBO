using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM.Framework;
using RestSharp;
using Newtonsoft.Json;
using System.Xml;
using System.Net;
using ProvDTE = Soindus.Interfaces.ProveedoresDTE;
using ClasesDTE = Soindus.Clases.DTE;
using Comun = Soindus.Clases.Comun;

namespace Soindus.AddOnMonitorProveedores.Formularios
{
    [FormAttribute("Soindus.AddOnMonitorProveedores.Formularios.frmMonitor", "Formularios/frmMonitor.b1f")]
    class frmMonitor : UserFormBase
    {
        // Objetos de formulario
        #region Objetos de formulario
        private static SAPbouiCOM.Form oForm;
        private static SAPbouiCOM.Matrix oMatrixSP;
        private static SAPbouiCOM.Matrix oMatrixVAL;
        private static SAPbouiCOM.Matrix oMatrixACE;
        private static SAPbouiCOM.Matrix oMatrixACR;
        private static SAPbouiCOM.Matrix oMatrixREC;
        private static SAPbouiCOM.Matrix oMatrixINT;
        private static SAPbouiCOM.Matrix oMatrixDL;
        private static SAPbouiCOM.Folder Folder1;
        private static SAPbouiCOM.Folder Folder2;
        private static SAPbouiCOM.Folder Folder3;
        private static SAPbouiCOM.Folder Folder4;
        private static SAPbouiCOM.Folder Folder5;
        private static SAPbouiCOM.Folder Folder6;
        private static SAPbouiCOM.Folder Folder7;
        private static SAPbouiCOM.EditText txtProv;
        private static SAPbouiCOM.EditText txtFDesde;
        private static SAPbouiCOM.EditText txtFHasta;
        private static SAPbouiCOM.ComboBox cmbTipoD;
        private static SAPbouiCOM.EditText txtRut;
        private static SAPbouiCOM.EditText txtNomProv;
        private static SAPbouiCOM.StaticText lblResp;
        private static SAPbouiCOM.EditText txtResp;
        private static SAPbouiCOM.EditText txtNResp;
        private static SAPbouiCOM.StaticText lblRespD;
        private static SAPbouiCOM.EditText txtRespD;
        private static SAPbouiCOM.EditText txtNRespD;
        private static SAPbouiCOM.Button btnFiltrar;
        private static SAPbouiCOM.Button btnMarcar;
        private static SAPbouiCOM.Button btnValidar;
        private static SAPbouiCOM.Button btnValMX;
        private static SAPbouiCOM.Button btnValSOC;
        private static SAPbouiCOM.Button btnAceptar;
        private static SAPbouiCOM.Button btnRechaza;
        private static SAPbouiCOM.Button btnIntegra;
        private static SAPbouiCOM.Button btnIntMX;
        private static SAPbouiCOM.Button btnIntSOC;
        private static SAPbouiCOM.Button btnImport;

        private static bool clickbtnImport = false;
        private static bool clickbtnValidar = true;
        private static bool clickbtnAceptar = true;
        private static bool clickbtnRechazar = true;
        private static bool clickbtnIntegrar = true;

        private static Local.Configuracion ExtConf;
        #endregion

        public frmMonitor()
        {
            ExtConf = new Local.Configuracion();
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
            //Application.SBO_Application.ItemEvent += SBO_Application_ItemEvent;
            //Application.SBO_Application.ItemEvent += new SAPbouiCOM._IApplicationEvents_ItemEventEventHandler(SBO_Application_ItemEvent);
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
                        // Matrix de documentos validados
                        if (pVal.ItemUID.Equals("mtxVAL"))
                        {
                            if (pVal.ColUID.Equals("co_docAso"))
                            {
                                string sObjectType =
                                    ((SAPbouiCOM.EditText)(oMatrixVAL.Columns.Item("co_tipoAso").Cells.Item(pVal.Row).Specific)).String;
                                SAPbouiCOM.Column oColumn = (SAPbouiCOM.Column)oMatrixVAL.Columns.Item(pVal.ColUID);
                                SAPbouiCOM.LinkedButton oLink = ((SAPbouiCOM.LinkedButton)(oColumn.ExtendedObject));
                                oLink.LinkedObjectType = sObjectType;
                            }
                        }
                        // Matrix de documentos aceptados
                        if (pVal.ItemUID.Equals("mtxACE"))
                        {
                            if (pVal.ColUID.Equals("co_docAso"))
                            {
                                string sObjectType =
                                    ((SAPbouiCOM.EditText)(oMatrixACE.Columns.Item("co_tipoAso").Cells.Item(pVal.Row).Specific)).String;
                                SAPbouiCOM.Column oColumn = (SAPbouiCOM.Column)oMatrixACE.Columns.Item(pVal.ColUID);
                                SAPbouiCOM.LinkedButton oLink = ((SAPbouiCOM.LinkedButton)(oColumn.ExtendedObject));
                                oLink.LinkedObjectType = sObjectType;
                            }
                        }
                        // Matrix de documentos aceptados con reparo
                        if (pVal.ItemUID.Equals("mtxACR"))
                        {
                            if (pVal.ColUID.Equals("co_docAso"))
                            {
                                string sObjectType =
                                    ((SAPbouiCOM.EditText)(oMatrixACR.Columns.Item("co_tipoAso").Cells.Item(pVal.Row).Specific)).String;
                                SAPbouiCOM.Column oColumn = (SAPbouiCOM.Column)oMatrixACR.Columns.Item(pVal.ColUID);
                                SAPbouiCOM.LinkedButton oLink = ((SAPbouiCOM.LinkedButton)(oColumn.ExtendedObject));
                                oLink.LinkedObjectType = sObjectType;
                            }
                        }
                        // Matrix de documentos rechazados
                        if (pVal.ItemUID.Equals("mtxREC"))
                        {
                            if (pVal.ColUID.Equals("co_docAso"))
                            {
                                string sObjectType =
                                    ((SAPbouiCOM.EditText)(oMatrixREC.Columns.Item("co_tipoAso").Cells.Item(pVal.Row).Specific)).String;
                                SAPbouiCOM.Column oColumn = (SAPbouiCOM.Column)oMatrixREC.Columns.Item(pVal.ColUID);
                                SAPbouiCOM.LinkedButton oLink = ((SAPbouiCOM.LinkedButton)(oColumn.ExtendedObject));
                                oLink.LinkedObjectType = sObjectType;
                            }
                        }
                        // Matrix de documentos integrados
                        if (pVal.ItemUID.Equals("mtxINT"))
                        {
                            if (pVal.ColUID.Equals("co_docAso"))
                            {
                                string sObjectType =
                                    ((SAPbouiCOM.EditText)(oMatrixINT.Columns.Item("co_tipoAso").Cells.Item(pVal.Row).Specific)).String;
                                SAPbouiCOM.Column oColumn = (SAPbouiCOM.Column)oMatrixINT.Columns.Item(pVal.ColUID);
                                SAPbouiCOM.LinkedButton oLink = ((SAPbouiCOM.LinkedButton)(oColumn.ExtendedObject));
                                oLink.LinkedObjectType = sObjectType;
                            }
                            if (pVal.ColUID.Equals("co_prelim"))
                            {
                                string sObjectType = "112";
                                SAPbouiCOM.Column oColumn = (SAPbouiCOM.Column)oMatrixINT.Columns.Item(pVal.ColUID);
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
                        if (pVal.FormUID.Equals("frmMonitor"))
                        {
                            // Texto SN
                            #region txtProv
                            if (pVal.ItemUID.Equals("txtProv"))
                            {
                                SAPbouiCOM.EditText oProv = (SAPbouiCOM.EditText)oForm.Items.Item("txtProv").Specific;
                                String Prov = oProv.Value;

                                if (String.IsNullOrEmpty(Prov))
                                {
                                    SAPbouiCOM.EditText oRut = (SAPbouiCOM.EditText)oForm.Items.Item("txtRut").Specific;
                                    SAPbouiCOM.EditText oNomProv = (SAPbouiCOM.EditText)oForm.Items.Item("txtNomProv").Specific;
                                    oRut.Value = string.Empty;
                                    oNomProv.Value = string.Empty;
                                    //oForm.DataSources.UserDataSources.Item("Prov").Value = string.Empty;
                                    //oForm.DataSources.UserDataSources.Item("Rut").Value = string.Empty;
                                    //oForm.DataSources.UserDataSources.Item("NomProv").Value = string.Empty;
                                }
                            }
                            #endregion

                            // Texto Responsable
                            #region txtResp
                            if (pVal.ItemUID.Equals("txtResp"))
                            {
                                SAPbouiCOM.EditText oResp = (SAPbouiCOM.EditText)oForm.Items.Item("txtResp").Specific;
                                String Resp = oResp.Value;

                                if (String.IsNullOrEmpty(Resp))
                                {
                                    SAPbouiCOM.EditText oNResp = (SAPbouiCOM.EditText)oForm.Items.Item("txtNResp").Specific;
                                    oNResp.Value = string.Empty;
                                }
                            }
                            #endregion

                            // Texto Responsable Documento
                            #region txtRespD
                            if (pVal.ItemUID.Equals("txtRespD"))
                            {
                                SAPbouiCOM.EditText oRespD = (SAPbouiCOM.EditText)oForm.Items.Item("txtRespD").Specific;
                                String RespD = oRespD.Value;

                                if (String.IsNullOrEmpty(RespD))
                                {
                                    SAPbouiCOM.EditText oNRespD = (SAPbouiCOM.EditText)oForm.Items.Item("txtNRespD").Specific;
                                    oNRespD.Value = string.Empty;
                                }
                            }
                            #endregion
                        }
                    }

                    if (pVal.EventType.Equals(SAPbouiCOM.BoEventTypes.et_CLICK))
                    {
                        // Imagen PDF
                        #region Imagen PDF
                        if (pVal.FormUID.Equals("frmMonitor"))
                        {
                            if (pVal.ItemUID.Contains("mtx"))
                            {
                                if (pVal.ColUID.Equals("co_pdf"))
                                {
                                    if(pVal.Row > 0)
                                    {
                                        SAPbouiCOM.DataTable dtDoc = null;
                                        switch (pVal.ItemUID)
                                        {
                                            case "mtxSP":
                                                dtDoc = oForm.DataSources.DataTables.Item("DOCUMENTOS");
                                                break;
                                            case "mtxVAL":
                                                dtDoc = oForm.DataSources.DataTables.Item("DOCUMENTOSVAL");
                                                break;
                                            case "mtxACE":
                                                dtDoc = oForm.DataSources.DataTables.Item("DOCUMENTOSACE");
                                                break;
                                            case "mtxACR":
                                                dtDoc = oForm.DataSources.DataTables.Item("DOCUMENTOSACR");
                                                break;
                                            case "mtxREC":
                                                dtDoc = oForm.DataSources.DataTables.Item("DOCUMENTOSREC");
                                                break;
                                            case "mtxINT":
                                                dtDoc = oForm.DataSources.DataTables.Item("DOCUMENTOSINT");
                                                break;
                                            case "mtxDL":
                                                dtDoc = oForm.DataSources.DataTables.Item("DOCUMENTOSDL");
                                                break;
                                        }
                                        VisualizarPdf(dtDoc, pVal.Row - 1);
                                    }
                                }
                            }
                        }
                        #endregion

                        // Boton actualizar
                        #region actualizar
                        if (pVal.ItemUID.Equals("btnFiltrar"))
                        {
                            // Documentos sin procesar
                            if (Folder1.Selected)
                            {
                                CargarDocumentosDesdeUDO(oMatrixSP, oForm, "0", "DOCUMENTOS");
                            }

                            // Documentos validados
                            if (Folder2.Selected)
                            {
                                CargarDocumentosDesdeUDO(oMatrixVAL, oForm, "1", "DOCUMENTOSVAL");
                            }

                            // Documentos aceptados
                            if (Folder3.Selected)
                            {
                                CargarDocumentosDesdeUDO(oMatrixACE, oForm, "2", "DOCUMENTOSACE");
                            }

                            // Documentos aceptados con reparo
                            if (Folder4.Selected)
                            {
                                CargarDocumentosDesdeUDO(oMatrixACR, oForm, "3", "DOCUMENTOSACR");
                            }

                            // Documentos rechazados
                            if (Folder5.Selected)
                            {
                                CargarDocumentosDesdeUDO(oMatrixREC, oForm, "4", "DOCUMENTOSREC");
                            }

                            // Documentos integrados
                            if (Folder6.Selected)
                            {
                                CargarDocumentosDesdeUDO(oMatrixINT, oForm, "5", "DOCUMENTOSINT");
                            }

                            // Descarga nuevos documentos
                            if (Folder7.Selected)
                            {
                                CargarDocumentosDesdeAPI(oMatrixDL, oForm);
                            }
                        }
                        #endregion

                        // Boton marcar
                        #region marcar
                        if (pVal.ItemUID.Equals("btnMarcar"))
                        {
                            // Documentos sin procesar
                            if (Folder1.Selected)
                            {
                                SAPbouiCOM.DataTable dtDoc = oForm.DataSources.DataTables.Item("DOCUMENTOS");
                                MarcarRegistrosMatrix(oMatrixSP, dtDoc, "CHK");
                            }

                            // Documentos validados
                            if (Folder2.Selected)
                            {
                                SAPbouiCOM.DataTable dtDoc = oForm.DataSources.DataTables.Item("DOCUMENTOSVAL");
                                MarcarRegistrosMatrix(oMatrixVAL, dtDoc, "CHK");
                            }

                            // Documentos aceptados
                            if (Folder3.Selected)
                            {
                                SAPbouiCOM.DataTable dtDoc = oForm.DataSources.DataTables.Item("DOCUMENTOSACE");
                                MarcarRegistrosMatrix(oMatrixACE, dtDoc, "CHK");
                            }

                            // Documentos aceptados con reparo
                            if (Folder4.Selected)
                            {
                                SAPbouiCOM.DataTable dtDoc = oForm.DataSources.DataTables.Item("DOCUMENTOSACR");
                                MarcarRegistrosMatrix(oMatrixACR, dtDoc, "CHK");
                            }

                            // Documentos rechazados
                            if (Folder5.Selected)
                            {
                                SAPbouiCOM.DataTable dtDoc = oForm.DataSources.DataTables.Item("DOCUMENTOSREC");
                                MarcarRegistrosMatrix(oMatrixREC, dtDoc, "CHK");
                            }

                            // Documentos integrados
                            if (Folder6.Selected)
                            {
                                SAPbouiCOM.DataTable dtDoc = oForm.DataSources.DataTables.Item("DOCUMENTOSINT");
                                MarcarRegistrosMatrix(oMatrixINT, dtDoc, "CHK");
                            }

                            // Descarga nuevos documentos
                            if (Folder7.Selected)
                            {
                                SAPbouiCOM.DataTable dtDoc = oForm.DataSources.DataTables.Item("DOCUMENTOSDL");
                                MarcarRegistrosMatrix(oMatrixDL, dtDoc, "CHK");
                            }
                        }
                        #endregion

                        // Boton validar
                        #region validar
                        if (pVal.ItemUID.Equals("btnValidar"))
                        {
                            // Valida documentos no procesados
                            if (Folder1.Selected)
                            {
                                oMatrixSP.FlushToDataSource();
                                SAPbouiCOM.DataTable dtDoc = oForm.DataSources.DataTables.Item("DOCUMENTOS");
                                ValidarDocumentos(oMatrixSP, dtDoc, "CHK");
                                clickbtnValidar = true;
                            }
                        }
                        #endregion

                        // Boton validar MX
                        #region validar MX
                        if (pVal.ItemUID.Equals("btnValMX"))
                        {
                            if (PermiteOpcionesMX())
                            {
                                // Valida documentos no procesados con documento base en moneda extranjera
                                if (Folder1.Selected)
                                {
                                    oMatrixSP.FlushToDataSource();
                                    SAPbouiCOM.DataTable dtDoc = oForm.DataSources.DataTables.Item("DOCUMENTOS");
                                    ValidarDocumentosMX(oMatrixSP, dtDoc, "CHK");
                                    clickbtnValidar = true;
                                }
                            }
                            else
                            {
                                Application.SBO_Application.MessageBox("Operación no permitida para el perfil del usuario");
                            }
                        }
                        #endregion

                        // Boton validar sin OC
                        #region validar sin OC
                        if (pVal.ItemUID.Equals("btnValSOC"))
                        {
                            if (PermiteOpcionesSOC())
                            {
                                // Valida documentos no procesados con documento base en moneda extranjera
                                if (Folder1.Selected)
                                {
                                    oMatrixSP.FlushToDataSource();
                                    SAPbouiCOM.DataTable dtDoc = oForm.DataSources.DataTables.Item("DOCUMENTOS");
                                    ValidarDocumentosSinOC(oMatrixSP, dtDoc, "CHK");
                                    clickbtnValidar = true;
                                }
                            }
                            else
                            {
                                Application.SBO_Application.MessageBox("Operación no permitida para el perfil del usuario");
                            }
                        }
                        #endregion

                        // Boton aceptar
                        #region aceptar
                        if (pVal.ItemUID.Equals("btnAceptar"))
                        {
                            // Acepta documentos validados
                            if (Folder2.Selected)
                            {
                                oMatrixVAL.FlushToDataSource();
                                SAPbouiCOM.DataTable dtDoc = oForm.DataSources.DataTables.Item("DOCUMENTOSVAL");
                                AceptarDocumentos(oMatrixVAL, dtDoc, "CHK");
                                clickbtnAceptar = true;
                            }
                        }
                        #endregion

                        // Boton rechazar
                        #region rechazar
                        if (pVal.ItemUID.Equals("btnRechaza"))
                        {
                            // Rechaza documentos sin procesar
                            if (Folder1.Selected)
                            {
                                oMatrixSP.FlushToDataSource();
                                SAPbouiCOM.DataTable dtDoc = oForm.DataSources.DataTables.Item("DOCUMENTOS");
                                RechazarDocumentos(oMatrixSP, dtDoc, "CHK");
                                clickbtnRechazar = true;
                            }

                            // Rechaza documentos validados
                            if (Folder2.Selected)
                            {
                                oMatrixVAL.FlushToDataSource();
                                SAPbouiCOM.DataTable dtDoc = oForm.DataSources.DataTables.Item("DOCUMENTOSVAL");
                                RechazarDocumentos(oMatrixVAL, dtDoc, "CHK");
                                clickbtnRechazar = true;
                            }
                        }
                        #endregion

                        // Boton integrar
                        #region integrar
                        if (pVal.ItemUID.Equals("btnIntegra"))
                        {
                            // Integra documentos aceptados
                            if (Folder3.Selected)
                            {
                                oMatrixACE.FlushToDataSource();
                                SAPbouiCOM.DataTable dtDoc = oForm.DataSources.DataTables.Item("DOCUMENTOSACE");
                                IntegrarDocumentos(oMatrixACE, dtDoc, "CHK");
                                clickbtnIntegrar = true;
                            }

                            // Integra documentos aceptados con reparo
                            if (Folder4.Selected)
                            {
                                oMatrixACR.FlushToDataSource();
                                SAPbouiCOM.DataTable dtDoc = oForm.DataSources.DataTables.Item("DOCUMENTOSACR");
                                IntegrarDocumentos(oMatrixACR, dtDoc, "CHK");
                                clickbtnIntegrar = true;
                            }
                        }
                        #endregion

                        // Boton integrar MX
                        #region integrar MX
                        if (pVal.ItemUID.Equals("btnIntMX"))
                        {
                            if (PermiteOpcionesMX())
                            {
                                // Integra documentos aceptados
                                if (Folder3.Selected)
                                {
                                    oMatrixACE.FlushToDataSource();
                                    SAPbouiCOM.DataTable dtDoc = oForm.DataSources.DataTables.Item("DOCUMENTOSACE");
                                    IntegrarDocumentosMX(oMatrixACE, dtDoc, "CHK");
                                    clickbtnIntegrar = true;
                                }

                                // Integra documentos aceptados con reparo
                                if (Folder4.Selected)
                                {
                                    oMatrixACR.FlushToDataSource();
                                    SAPbouiCOM.DataTable dtDoc = oForm.DataSources.DataTables.Item("DOCUMENTOSACR");
                                    IntegrarDocumentosMX(oMatrixACR, dtDoc, "CHK");
                                    clickbtnIntegrar = true;
                                }
                            }
                            else
                            {
                                Application.SBO_Application.MessageBox("Operación no permitida para el perfil del usuario");
                            }
                        }
                        #endregion

                        // Boton integrar sin OC
                        #region integrar sin OC
                        if (pVal.ItemUID.Equals("btnIntSOC"))
                        {
                            if (PermiteOpcionesSOC())
                            {
                                // Integra documentos aceptados
                                if (Folder3.Selected)
                                {
                                    oMatrixACE.FlushToDataSource();
                                    SAPbouiCOM.DataTable dtDoc = oForm.DataSources.DataTables.Item("DOCUMENTOSACE");
                                    IntegrarDocumentosSinOC(oMatrixACE, dtDoc, "CHK");
                                    clickbtnIntegrar = true;
                                }

                                // Integra documentos aceptados con reparo
                                if (Folder4.Selected)
                                {
                                    oMatrixACR.FlushToDataSource();
                                    SAPbouiCOM.DataTable dtDoc = oForm.DataSources.DataTables.Item("DOCUMENTOSACR");
                                    IntegrarDocumentosSinOC(oMatrixACR, dtDoc, "CHK");
                                    clickbtnIntegrar = true;
                                }
                            }
                            else
                            {
                                Application.SBO_Application.MessageBox("Operación no permitida para el perfil del usuario");
                            }
                        }
                        #endregion

                        // Boton importar a DB
                        #region importar a BD
                        if (pVal.ItemUID.Equals("btnImport"))
                        {
                            // Descarga nuevos documentos
                            if (Folder7.Selected)
                            {
                                oMatrixDL.FlushToDataSource();
                                SAPbouiCOM.DataTable dtDoc = oForm.DataSources.DataTables.Item("DOCUMENTOSDL");
                                AgregarRegistrosUDOFromDT(dtDoc, "CHK");
                                clickbtnImport = true;
                            }
                        }
                        #endregion

                        // Panel documentos no procesados
                        #region click en panel documentos no procesados
                        if (pVal.ItemUID.Equals("tab01"))
                        {
                            if (clickbtnImport)
                            {
                                CargarDocumentosDesdeUDO(oMatrixSP, oForm, "0", "DOCUMENTOS");
                                clickbtnImport = false;
                            }
                        }
                        #endregion

                        // Panel documentos validados
                        #region click en panel documentos validados
                        if (pVal.ItemUID.Equals("tab02"))
                        {
                            if (clickbtnValidar)
                            {
                                CargarDocumentosDesdeUDO(oMatrixVAL, oForm, "1", "DOCUMENTOSVAL");
                                clickbtnValidar = false;
                            }
                        }
                        #endregion

                        // Panel documentos aceptados
                        #region click en panel documentos aceptados
                        if (pVal.ItemUID.Equals("tab03"))
                        {
                            if (clickbtnAceptar)
                            {
                                CargarDocumentosDesdeUDO(oMatrixACE, oForm, "2", "DOCUMENTOSACE");
                                CargarDocumentosDesdeUDO(oMatrixACR, oForm, "3", "DOCUMENTOSACR");
                                clickbtnAceptar = false;
                            }
                        }
                        #endregion
                        
                        // Panel documentos aceptados con reparo
                        #region click en panel documentos aceptados con reparo
                        if (pVal.ItemUID.Equals("tab04"))
                        {
                            if (clickbtnAceptar)
                            {
                                CargarDocumentosDesdeUDO(oMatrixACE, oForm, "2", "DOCUMENTOSACE");
                                CargarDocumentosDesdeUDO(oMatrixACR, oForm, "3", "DOCUMENTOSACR");
                                clickbtnAceptar = false;
                            }
                        }
                        #endregion

                        // Panel documentos rechazados
                        #region click en panel documentos rechazados
                        if (pVal.ItemUID.Equals("tab05"))
                        {
                            //if (clickbtnIntegrar)
                            //{
                                CargarDocumentosDesdeUDO(oMatrixREC, oForm, "4", "DOCUMENTOSREC");
                                //clickbtnIntegrar = false;
                            //}
                        }
                        #endregion

                        // Panel documentos integrados
                        #region click en panel documentos integrados
                        if (pVal.ItemUID.Equals("tab06"))
                        {
                            //if (clickbtnIntegrar)
                            //{
                            CargarDocumentosDesdeUDO(oMatrixINT, oForm, "5", "DOCUMENTOSINT");
                            //clickbtnIntegrar = false;
                            //}
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

                        if (pVal.FormUID.Equals("frmMonitor"))
                        {
                            if (pVal.ItemUID.Equals("txtProv"))
                            {
                                if (oDataTable != null)
                                {
                                    oForm.DataSources.UserDataSources.Item("Prov").Value = string.Empty;
                                    oForm.DataSources.UserDataSources.Item("Rut").Value = string.Empty;
                                    oForm.DataSources.UserDataSources.Item("NomProv").Value = string.Empty;

                                    string CardCode = string.Empty;
                                    string LicTradNum = string.Empty;
                                    string CardName = string.Empty;
                                    CardCode = oDataTable.GetValue(0, 0).ToString();
                                    LicTradNum = oDataTable.GetValue(23, 0).ToString();
                                    CardName = oDataTable.GetValue("CardName", 0).ToString();

                                    oForm.DataSources.UserDataSources.Item("Prov").Value = CardCode;
                                    oForm.DataSources.UserDataSources.Item("Rut").Value = LicTradNum;
                                    oForm.DataSources.UserDataSources.Item("NomProv").Value = CardName;

                                    //for (Int32 Row = 0; Row <= (oDataTable.Rows.Count - 1); Row++)
                                    //{
                                    //    String LicTradNum = String.Empty;
                                    //    LicTradNum = oDataTable.GetValue(23, Row).ToString();
                                    //    oForm.DataSources.UserDataSources.Item("Rut").Value += LicTradNum;
                                    //    if (Row < (oDataTable.Rows.Count - 1))
                                    //    {
                                    //        oForm.DataSources.UserDataSources.Item("Rut").Value += ";";
                                    //    }
                                    //}
                                }
                            }
                            if (pVal.ItemUID.Equals("txtResp"))
                            {
                                if (oDataTable != null)
                                {
                                    oForm.DataSources.UserDataSources.Item("Resp").Value = string.Empty;

                                    string empID = string.Empty;
                                    string empName = string.Empty;
                                    empID = oDataTable.GetValue(0, 0).ToString();
                                    empName = oDataTable.GetValue("firstName", 0).ToString() + " " + oDataTable.GetValue("lastName", 0).ToString();

                                    oForm.DataSources.UserDataSources.Item("Resp").Value = empID;
                                    oForm.DataSources.UserDataSources.Item("NResp").Value = empName;
                                }
                            }
                            if (pVal.ItemUID.Equals("txtRespD"))
                            {
                                if (oDataTable != null)
                                {
                                    oForm.DataSources.UserDataSources.Item("RespD").Value = string.Empty;

                                    string SlpCode = string.Empty;
                                    string SlpName = string.Empty;
                                    SlpCode = oDataTable.GetValue(0, 0).ToString();
                                    SlpName = oDataTable.GetValue("SlpName", 0).ToString();

                                    oForm.DataSources.UserDataSources.Item("RespD").Value = SlpCode;
                                    oForm.DataSources.UserDataSources.Item("NRespD").Value = SlpName;
                                }
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

        /// <summary>
        /// Asignación de objetos de formulario SB1
        /// </summary>
        private void AsignarObjetos()
        {
            oForm = ((SAPbouiCOM.Form)(Application.SBO_Application.Forms.Item("frmMonitor")));
            oMatrixSP = ((SAPbouiCOM.Matrix)(GetItem("mtxSP").Specific));
            oMatrixVAL = ((SAPbouiCOM.Matrix)(GetItem("mtxVAL").Specific));
            oMatrixACE = ((SAPbouiCOM.Matrix)(GetItem("mtxACE").Specific));
            oMatrixACR = ((SAPbouiCOM.Matrix)(GetItem("mtxACR").Specific));
            oMatrixREC = ((SAPbouiCOM.Matrix)(GetItem("mtxREC").Specific));
            oMatrixINT = ((SAPbouiCOM.Matrix)(GetItem("mtxINT").Specific));
            oMatrixDL = ((SAPbouiCOM.Matrix)(GetItem("mtxDL").Specific));
            Folder1 = ((SAPbouiCOM.Folder)(GetItem("tab01").Specific));
            Folder2 = ((SAPbouiCOM.Folder)(GetItem("tab02").Specific));
            Folder3 = ((SAPbouiCOM.Folder)(GetItem("tab03").Specific));
            Folder4 = ((SAPbouiCOM.Folder)(GetItem("tab04").Specific));
            Folder5 = ((SAPbouiCOM.Folder)(GetItem("tab05").Specific));
            Folder6 = ((SAPbouiCOM.Folder)(GetItem("tab06").Specific));
            Folder7 = ((SAPbouiCOM.Folder)(GetItem("tab07").Specific));
            Folder1.Select();
            txtProv = ((SAPbouiCOM.EditText)(GetItem("txtProv").Specific));
            txtFDesde = ((SAPbouiCOM.EditText)(GetItem("txtFDesde").Specific));
            txtFHasta = ((SAPbouiCOM.EditText)(GetItem("txtFHasta").Specific));
            cmbTipoD = ((SAPbouiCOM.ComboBox)(GetItem("cmbTipoD").Specific));
            txtRut = ((SAPbouiCOM.EditText)(GetItem("txtRut").Specific));
            txtNomProv = ((SAPbouiCOM.EditText)(GetItem("txtNomProv").Specific));
            lblResp = ((SAPbouiCOM.StaticText)(GetItem("lblResp").Specific));
            txtResp = ((SAPbouiCOM.EditText)(GetItem("txtResp").Specific));
            txtNResp = ((SAPbouiCOM.EditText)(GetItem("txtNResp").Specific));
            lblRespD = ((SAPbouiCOM.StaticText)(GetItem("lblRespD").Specific));
            txtRespD = ((SAPbouiCOM.EditText)(GetItem("txtRespD").Specific));
            txtNRespD = ((SAPbouiCOM.EditText)(GetItem("txtNRespD").Specific));
            btnFiltrar = ((SAPbouiCOM.Button)(GetItem("btnFiltrar").Specific));
            btnMarcar = ((SAPbouiCOM.Button)(GetItem("btnMarcar").Specific));
            btnValidar = ((SAPbouiCOM.Button)(GetItem("btnValidar").Specific));
            btnValMX = ((SAPbouiCOM.Button)(GetItem("btnValMX").Specific));
            btnValSOC = ((SAPbouiCOM.Button)(GetItem("btnValSOC").Specific));
            btnAceptar = ((SAPbouiCOM.Button)(GetItem("btnAceptar").Specific));
            btnRechaza = ((SAPbouiCOM.Button)(GetItem("btnRechaza").Specific));
            btnIntegra = ((SAPbouiCOM.Button)(GetItem("btnIntegra").Specific));
            btnIntMX = ((SAPbouiCOM.Button)(GetItem("btnIntMX").Specific));
            btnIntSOC = ((SAPbouiCOM.Button)(GetItem("btnIntSOC").Specific));
            btnImport = ((SAPbouiCOM.Button)(GetItem("btnImport").Specific));
        }

        /// <summary>
        /// Carga formulario
        /// </summary>
        private void CargarFormulario()
        {
            // Propiedades del formulario

            // Choose from list socio de negocio
            SAPbouiCOM.ChooseFromListCollection oCFLs = null;
            SAPbouiCOM.ChooseFromList oCFL = null;
            SAPbouiCOM.ChooseFromListCreationParams oCFLCreationParams = null;
            SAPbouiCOM.Conditions oCons = null;

            // Blindear edittext Socio de negocio
            oForm.DataSources.UserDataSources.Add("Prov", SAPbouiCOM.BoDataType.dt_SHORT_TEXT, 200);
            oForm.DataSources.UserDataSources.Add("Rut", SAPbouiCOM.BoDataType.dt_SHORT_TEXT, 200);
            oForm.DataSources.UserDataSources.Add("NomProv", SAPbouiCOM.BoDataType.dt_SHORT_TEXT, 200);
            oForm.DataSources.UserDataSources.Add("Resp", SAPbouiCOM.BoDataType.dt_SHORT_TEXT, 200);
            oForm.DataSources.UserDataSources.Add("NResp", SAPbouiCOM.BoDataType.dt_SHORT_TEXT, 200);
            oForm.DataSources.UserDataSources.Add("RespD", SAPbouiCOM.BoDataType.dt_SHORT_TEXT, 200);
            oForm.DataSources.UserDataSources.Add("NRespD", SAPbouiCOM.BoDataType.dt_SHORT_TEXT, 200);
            txtProv.DataBind.SetBound(true, "", "Prov");
            txtRut.DataBind.SetBound(true, "", "Rut");
            txtNomProv.DataBind.SetBound(true, "", "NomProv");
            txtResp.DataBind.SetBound(true, "", "Resp");
            txtNResp.DataBind.SetBound(true, "", "NResp");
            txtRespD.DataBind.SetBound(true, "", "RespD");
            txtNRespD.DataBind.SetBound(true, "", "NRespD");

            oCFLs = oForm.ChooseFromLists;
            oCFLCreationParams = ((SAPbouiCOM.ChooseFromListCreationParams)(Application.SBO_Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_ChooseFromListCreationParams)));
            oCFLCreationParams.MultiSelection = false;
            oCFLCreationParams.ObjectType = "2";
            oCFLCreationParams.UniqueID = "cflProv";

            oCFL = oCFLs.Add(oCFLCreationParams);

            oCons = new SAPbouiCOM.Conditions();
            //Dar condiciones al ChooseFromList
            oCons = oCFL.GetConditions();

            SAPbouiCOM.Condition oCon = oCons.Add();
            oCon.Alias = "CardType";
            oCon.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
            oCon.CondVal = "S";

            oCFL.SetConditions(oCons);

            //Asignamos el ChoosefromList al campo de texto
            txtProv.ChooseFromListUID = "cflProv";
            txtProv.ChooseFromListAlias = "CardCode";

            // Fecha desde
            oForm.DataSources.UserDataSources.Add("FDesde", SAPbouiCOM.BoDataType.dt_DATE);
            txtFDesde.DataBind.SetBound(true, "", "FDesde");

            // Fecha hasta
            oForm.DataSources.UserDataSources.Add("FHasta", SAPbouiCOM.BoDataType.dt_DATE);
            txtFHasta.DataBind.SetBound(true, "", "FHasta");

            // Tipo documento
            cmbTipoD.ValidValues.Add("", "");
            cmbTipoD.ValidValues.Add("33", "Factura Electrónica");
            cmbTipoD.ValidValues.Add("34", "Factura Exenta de IVA Electrónica");
            //cmbTipoD.ValidValues.Add("43", "Liquidación Factura Electrónica");
            //cmbTipoD.ValidValues.Add("46", "Factura de Compra Electrónica");
            cmbTipoD.ValidValues.Add("52", "Guía de Despacho Electrónica");
            cmbTipoD.ValidValues.Add("56", "Nota de Débito Electrónica");
            cmbTipoD.ValidValues.Add("61", "Nota de Crédito Electrónica");

            oCFLs = oForm.ChooseFromLists;
            oCFLCreationParams = ((SAPbouiCOM.ChooseFromListCreationParams)(Application.SBO_Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_ChooseFromListCreationParams)));
            oCFLCreationParams.MultiSelection = false;
            oCFLCreationParams.ObjectType = "171";
            oCFLCreationParams.UniqueID = "cflResp";

            oCFL = oCFLs.Add(oCFLCreationParams);

            //Asignamos el ChoosefromList al campo de texto
            txtResp.ChooseFromListUID = "cflResp";
            txtResp.ChooseFromListAlias = "empID";

            oCFLs = oForm.ChooseFromLists;
            oCFLCreationParams = ((SAPbouiCOM.ChooseFromListCreationParams)(Application.SBO_Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_ChooseFromListCreationParams)));
            oCFLCreationParams.MultiSelection = false;
            oCFLCreationParams.ObjectType = "53";
            oCFLCreationParams.UniqueID = "cflRespD";

            oCFL = oCFLs.Add(oCFLCreationParams);

            //Asignamos el ChoosefromList al campo de texto
            txtRespD.ChooseFromListUID = "cflRespD";
            txtRespD.ChooseFromListAlias = "SlpCode";

            lblResp.Item.Visible = ExtConf.Parametros.Visualiza_Responsable;
            txtResp.Item.Visible = ExtConf.Parametros.Visualiza_Responsable;
            txtNResp.Item.Visible = ExtConf.Parametros.Visualiza_Responsable;

            lblRespD.Item.Visible = ExtConf.Parametros.Visualiza_Responsable_Doc;
            txtRespD.Item.Visible = ExtConf.Parametros.Visualiza_Responsable_Doc;
            txtNRespD.Item.Visible = ExtConf.Parametros.Visualiza_Responsable_Doc;

            if (!ExtConf.Parametros.Visualiza_Responsable)
            {
                lblRespD.Item.Top = lblResp.Item.Top;
                txtRespD.Item.Top = txtResp.Item.Top;
                txtNRespD.Item.Top = txtNResp.Item.Top;
            }

            // Estrucrura de matrix documentos sin procesar
            EstructuraMatrixSinProcesar();
            // Estructura de matrix documentos validados
            EstructuraMatrixValidados();
            // Estrucrura de matrix descarga nuevos documentos
            EstructuraMatrixAceptados();
            // Estrucrura de matrix descarga nuevos documentos
            EstructuraMatrixAceptadosConReparo();
            // Estrucrura de matrix descarga nuevos documentos
            EstructuraMatrixRechazados();
            // Estrucrura de matrix descarga nuevos documentos
            EstructuraMatrixIntegrados();
            // Estrucrura de matrix descarga nuevos documentos
            EstructuraMatrixDescarga();

            oForm.Visible = true;
            //oForm.Freeze(false);
            //CargarDocumentosDesdeAPI(oMatrixSP, oForm);
            CargarDocumentosDesdeUDO(oMatrixSP, oForm, "0", "DOCUMENTOS");
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
                case "DOCUMENTOS":
                    dt.Columns.Add("co_Chk", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
                    break;
                case "DOCUMENTOSVAL":
                    dt.Columns.Add("co_ChkVAL", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
                    break;
                case "DOCUMENTOSACE":
                    dt.Columns.Add("co_ChkACE", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
                    break;
                case "DOCUMENTOSACR":
                    dt.Columns.Add("co_ChkACR", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
                    break;
                case "DOCUMENTOSREC":
                    dt.Columns.Add("co_ChkREC", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
                    break;
                case "DOCUMENTOSINT":
                    dt.Columns.Add("co_ChkINT", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
                    break;
                case "DOCUMENTOSDL":
                    dt.Columns.Add("co_ChkDL", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
                    break;
            }
            dt.Columns.Add("co_pdf", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dt.Columns.Add("co_plazo", SAPbouiCOM.BoFieldsType.ft_Integer);
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
            dt.Columns.Add("co_rutCes", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dt.Columns.Add("co_razSocC", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dt.Columns.Add("co_fechaCe", SAPbouiCOM.BoFieldsType.ft_Date);
            dt.Columns.Add("co_codResp", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dt.Columns.Add("co_nomResp", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dt.Columns.Add("co_codResD", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dt.Columns.Add("co_nomResD", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
        }

        /// <summary>
        /// Genera estructura de Matrix documentos no procesados
        /// </summary>
        private void EstructuraMatrixSinProcesar()
        {
            string NombreDT = "DOCUMENTOS";
            oForm.DataSources.DataTables.Add(NombreDT);
            SAPbouiCOM.DataTable datatable = oForm.DataSources.DataTables.Item(NombreDT);
            string _query = ObtenerQueryConFiltros("0");
            datatable.ExecuteQuery(_query);

            SAPbouiCOM.Columns oColumns;
            oColumns = oMatrixSP.Columns;
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

            oColumn = oColumns.Add("co_Chk", SAPbouiCOM.BoFormItemTypes.it_CHECK_BOX);
            oColumn.TitleObject.Caption = String.Empty;
            oColumn.Editable = true;
            oColumn.Width = 15;
            oColumn.ValOn = "Y";
            oColumn.ValOff = "N";
            //oForm.DataSources.UserDataSources.Add("co_Chk", SAPbouiCOM.BoDataType.dt_SHORT_TEXT, 1);
            //oColumn.DataBind.SetBound(true, "", "co_Chk");
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
            oColumn.Visible = false;
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
            oColumn.Editable = ExtConf.Parametros.Valida_PermiteOCManual;
            oColumn.Visible = ExtConf.Parametros.Valida_PermiteOCManual;
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

            //oColumn = oColumns.Add("co_docAso", SAPbouiCOM.BoFormItemTypes.it_LINKED_BUTTON);
            //oColumn.TitleObject.Caption = "Link OC";
            //oColumn.Editable = false;
            //oColumn.Width = 60;
            //oColumn.DataBind.Bind(NombreDT, "co_docAso");

            oColumn = oColumns.Add("co_razonRe", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Razón de rechazo";
            oColumn.Editable = true;
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

            oColumn = oColumns.Add("co_rutCes", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Rut Cesionario";
            oColumn.Editable = false;
            oColumn.Visible = ExtConf.Parametros.Visualiza_Cesion;
            oColumn.Width = 70;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "U_RUTCES");

            oColumn = oColumns.Add("co_razSocC", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Razón Social Cesionario";
            oColumn.Editable = false;
            oColumn.Visible = ExtConf.Parametros.Visualiza_Cesion;
            oColumn.Width = 200;
            oColumn.DataBind.Bind(NombreDT, "U_RAZSOCC");

            oColumn = oColumns.Add("co_fechaCe", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Fecha Cesión";
            oColumn.Editable = false;
            oColumn.Visible = ExtConf.Parametros.Visualiza_Cesion;
            oColumn.Width = 60;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "U_FECHACE");

            oColumn = oColumns.Add("co_codResp", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Código Resposable";
            oColumn.Editable = false;
            oColumn.Visible = false;
            oColumn.Width = 60;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "U_CODRESP");

            oColumn = oColumns.Add("co_nomResp", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Nombre Responsable";
            oColumn.Editable = false;
            oColumn.Visible = false;
            oColumn.Width = 60;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "U_NOMRESP");

            oColumn = oColumns.Add("co_codResD", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Código Resp. Doc.";
            oColumn.Editable = false;
            oColumn.Visible = false;
            oColumn.Width = 60;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "U_CODRESPD");

            oColumn = oColumns.Add("co_nomResD", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Nombre Resp. Doc.";
            oColumn.Editable = false;
            oColumn.Visible = false;
            oColumn.Width = 60;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "U_NOMRESPD");
        }

        /// <summary>
        /// Genera estructura de Matrix documentos validados
        /// </summary>
        private void EstructuraMatrixValidados()
        {
            string NombreDT = "DOCUMENTOSVAL";
            oForm.DataSources.DataTables.Add(NombreDT);
            SAPbouiCOM.DataTable datatable = oForm.DataSources.DataTables.Item(NombreDT);
            string _query = ObtenerQueryConFiltros("1");
            datatable.ExecuteQuery(_query);

            SAPbouiCOM.Columns oColumns;
            oColumns = oMatrixVAL.Columns;
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

            oColumn = oColumns.Add("co_ChkVAL", SAPbouiCOM.BoFormItemTypes.it_CHECK_BOX);
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
            oColumn.Visible = false;
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
            oColumn.Visible = true;
            oColumn.Width = 60;
            oColumn.DataBind.Bind(NombreDT, "U_DOCBASE");

            oColumn = oColumns.Add("co_docAso", SAPbouiCOM.BoFormItemTypes.it_LINKED_BUTTON);
            oColumn.TitleObject.Caption = "Link OC";
            oColumn.Editable = false;
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

            //oColumn = oColumns.Add("co_docAso", SAPbouiCOM.BoFormItemTypes.it_LINKED_BUTTON);
            //oColumn.TitleObject.Caption = "Link OC";
            //oColumn.Editable = false;
            //oColumn.Width = 60;
            //oColumn.DataBind.Bind(NombreDT, "co_docAso");

            oColumn = oColumns.Add("co_razonRe", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Razón reparo o rechazo";
            oColumn.Editable = true;
            //oColumn.Visible = false;
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

            oColumn = oColumns.Add("co_rutCes", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Rut Cesionario";
            oColumn.Editable = false;
            oColumn.Visible = ExtConf.Parametros.Visualiza_Cesion;
            oColumn.Width = 70;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "U_RUTCES");

            oColumn = oColumns.Add("co_razSocC", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Razón Social Cesionario";
            oColumn.Editable = false;
            oColumn.Visible = ExtConf.Parametros.Visualiza_Cesion;
            oColumn.Width = 200;
            oColumn.DataBind.Bind(NombreDT, "U_RAZSOCC");

            oColumn = oColumns.Add("co_fechaCe", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Fecha Cesión";
            oColumn.Editable = false;
            oColumn.Visible = ExtConf.Parametros.Visualiza_Cesion;
            oColumn.Width = 60;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "U_FECHACE");

            oColumn = oColumns.Add("co_codResp", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Código Resposable";
            oColumn.Editable = false;
            oColumn.Visible = false;
            oColumn.Width = 60;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "U_CODRESP");

            oColumn = oColumns.Add("co_nomResp", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Nombre Responsable";
            oColumn.Editable = false;
            oColumn.Visible = false;
            oColumn.Width = 60;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "U_NOMRESP");

            oColumn = oColumns.Add("co_codResD", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Código Resp. Doc.";
            oColumn.Editable = false;
            oColumn.Visible = false;
            oColumn.Width = 60;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "U_CODRESPD");

            oColumn = oColumns.Add("co_nomResD", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Nombre Resp. Doc.";
            oColumn.Editable = false;
            oColumn.Visible = false;
            oColumn.Width = 60;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "U_NOMRESPD");
        }

        /// <summary>
        /// Genera estructura de Matrix documentos aceptados
        /// </summary>
        private void EstructuraMatrixAceptados()
        {
            string NombreDT = "DOCUMENTOSACE";
            oForm.DataSources.DataTables.Add(NombreDT);
            SAPbouiCOM.DataTable datatable = oForm.DataSources.DataTables.Item(NombreDT);
            string _query = ObtenerQueryConFiltros("2");
            datatable.ExecuteQuery(_query);

            SAPbouiCOM.Columns oColumns;
            oColumns = oMatrixACE.Columns;
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

            oColumn = oColumns.Add("co_ChkACE", SAPbouiCOM.BoFormItemTypes.it_CHECK_BOX);
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
            oColumn.Visible = false;
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
            oColumn.Visible = true;
            oColumn.Width = 60;
            oColumn.DataBind.Bind(NombreDT, "U_DOCBASE");

            oColumn = oColumns.Add("co_docAso", SAPbouiCOM.BoFormItemTypes.it_LINKED_BUTTON);
            oColumn.TitleObject.Caption = "Link OC";
            oColumn.Editable = false;
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
            oColumn.Visible = false;
            oColumn.Width = 60;
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

            //oColumn = oColumns.Add("co_docAso", SAPbouiCOM.BoFormItemTypes.it_LINKED_BUTTON);
            //oColumn.TitleObject.Caption = "Link OC";
            //oColumn.Editable = false;
            //oColumn.Width = 60;
            //oColumn.DataBind.Bind(NombreDT, "co_docAso");

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

            oColumn = oColumns.Add("co_rutCes", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Rut Cesionario";
            oColumn.Editable = false;
            oColumn.Visible = ExtConf.Parametros.Visualiza_Cesion;
            oColumn.Width = 70;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "U_RUTCES");

            oColumn = oColumns.Add("co_razSocC", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Razón Social Cesionario";
            oColumn.Editable = false;
            oColumn.Visible = ExtConf.Parametros.Visualiza_Cesion;
            oColumn.Width = 200;
            oColumn.DataBind.Bind(NombreDT, "U_RAZSOCC");

            oColumn = oColumns.Add("co_fechaCe", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Fecha Cesión";
            oColumn.Editable = false;
            oColumn.Visible = ExtConf.Parametros.Visualiza_Cesion;
            oColumn.Width = 60;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "U_FECHACE");

            oColumn = oColumns.Add("co_codResp", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Código Resposable";
            oColumn.Editable = false;
            oColumn.Visible = false;
            oColumn.Width = 60;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "U_CODRESP");

            oColumn = oColumns.Add("co_nomResp", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Nombre Responsable";
            oColumn.Editable = false;
            oColumn.Visible = false;
            oColumn.Width = 60;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "U_NOMRESP");

            oColumn = oColumns.Add("co_codResD", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Código Resp. Doc.";
            oColumn.Editable = false;
            oColumn.Visible = false;
            oColumn.Width = 60;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "U_CODRESPD");

            oColumn = oColumns.Add("co_nomResD", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Nombre Resp. Doc.";
            oColumn.Editable = false;
            oColumn.Visible = false;
            oColumn.Width = 60;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "U_NOMRESPD");
        }

        /// <summary>
        /// Genera estructura de Matrix documentos aceptados
        /// </summary>
        private void EstructuraMatrixAceptadosConReparo()
        {
            string NombreDT = "DOCUMENTOSACR";
            oForm.DataSources.DataTables.Add(NombreDT);
            SAPbouiCOM.DataTable datatable = oForm.DataSources.DataTables.Item(NombreDT);
            string _query = ObtenerQueryConFiltros("3");
            datatable.ExecuteQuery(_query);

            SAPbouiCOM.Columns oColumns;
            oColumns = oMatrixACR.Columns;
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

            oColumn = oColumns.Add("co_ChkACR", SAPbouiCOM.BoFormItemTypes.it_CHECK_BOX);
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
            oColumn.Visible = false;
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
            oColumn.Visible = true;
            oColumn.Width = 60;
            oColumn.DataBind.Bind(NombreDT, "U_DOCBASE");

            oColumn = oColumns.Add("co_docAso", SAPbouiCOM.BoFormItemTypes.it_LINKED_BUTTON);
            oColumn.TitleObject.Caption = "Link OC";
            oColumn.Editable = false;
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
            oColumn.Visible = false;
            oColumn.Width = 60;
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

            //oColumn = oColumns.Add("co_docAso", SAPbouiCOM.BoFormItemTypes.it_LINKED_BUTTON);
            //oColumn.TitleObject.Caption = "Link OC";
            //oColumn.Editable = false;
            //oColumn.Width = 60;
            //oColumn.DataBind.Bind(NombreDT, "co_docAso");

            oColumn = oColumns.Add("co_razonRe", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Razón reparo";
            oColumn.Editable = false;
            oColumn.Visible = true;
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

            oColumn = oColumns.Add("co_rutCes", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Rut Cesionario";
            oColumn.Editable = false;
            oColumn.Visible = ExtConf.Parametros.Visualiza_Cesion;
            oColumn.Width = 70;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "U_RUTCES");

            oColumn = oColumns.Add("co_razSocC", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Razón Social Cesionario";
            oColumn.Editable = false;
            oColumn.Visible = ExtConf.Parametros.Visualiza_Cesion;
            oColumn.Width = 200;
            oColumn.DataBind.Bind(NombreDT, "U_RAZSOCC");

            oColumn = oColumns.Add("co_fechaCe", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Fecha Cesión";
            oColumn.Editable = false;
            oColumn.Visible = ExtConf.Parametros.Visualiza_Cesion;
            oColumn.Width = 60;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "U_FECHACE");

            oColumn = oColumns.Add("co_codResp", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Código Resposable";
            oColumn.Editable = false;
            oColumn.Visible = false;
            oColumn.Width = 60;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "U_CODRESP");

            oColumn = oColumns.Add("co_nomResp", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Nombre Responsable";
            oColumn.Editable = false;
            oColumn.Visible = false;
            oColumn.Width = 60;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "U_NOMRESP");

            oColumn = oColumns.Add("co_codResD", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Código Resp. Doc.";
            oColumn.Editable = false;
            oColumn.Visible = false;
            oColumn.Width = 60;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "U_CODRESPD");

            oColumn = oColumns.Add("co_nomResD", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Nombre Resp. Doc.";
            oColumn.Editable = false;
            oColumn.Visible = false;
            oColumn.Width = 60;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "U_NOMRESPD");
        }

        /// <summary>
        /// Genera estructura de Matrix documentos rechazados
        /// </summary>
        private void EstructuraMatrixRechazados()
        {
            string NombreDT = "DOCUMENTOSREC";
            oForm.DataSources.DataTables.Add(NombreDT);
            SAPbouiCOM.DataTable datatable = oForm.DataSources.DataTables.Item(NombreDT);
            string _query = ObtenerQueryConFiltros("4");
            datatable.ExecuteQuery(_query);

            SAPbouiCOM.Columns oColumns;
            oColumns = oMatrixREC.Columns;
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

            oColumn = oColumns.Add("co_ChkREC", SAPbouiCOM.BoFormItemTypes.it_CHECK_BOX);
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
            oColumn.Visible = false;
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
            oColumn.Visible = true;
            oColumn.Width = 60;
            oColumn.DataBind.Bind(NombreDT, "U_DOCBASE");

            oColumn = oColumns.Add("co_docAso", SAPbouiCOM.BoFormItemTypes.it_LINKED_BUTTON);
            oColumn.TitleObject.Caption = "Link OC";
            oColumn.Editable = false;
            oColumn.Width = 60;
            oColumn.DataBind.Bind(NombreDT, "U_DOCASO");

            oColumn = oColumns.Add("co_estSii", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Estado SII";
            oColumn.Editable = false;
            oColumn.Visible = false;
            oColumn.Width = 160;
            oColumn.DataBind.Bind(NombreDT, "U_ESTSII");

            oColumn = oColumns.Add("co_estCome", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Estado Comercial";
            oColumn.Editable = false;
            oColumn.Visible = false;
            oColumn.Width = 60;
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

            //oColumn = oColumns.Add("co_docAso", SAPbouiCOM.BoFormItemTypes.it_LINKED_BUTTON);
            //oColumn.TitleObject.Caption = "Link OC";
            //oColumn.Editable = false;
            //oColumn.Width = 60;
            //oColumn.DataBind.Bind(NombreDT, "co_docAso");

            oColumn = oColumns.Add("co_razonRe", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Razón de rechazo";
            oColumn.Editable = false;
            //oColumn.Visible = false;
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

            oColumn = oColumns.Add("co_rutCes", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Rut Cesionario";
            oColumn.Editable = false;
            oColumn.Visible = ExtConf.Parametros.Visualiza_Cesion;
            oColumn.Width = 70;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "U_RUTCES");

            oColumn = oColumns.Add("co_razSocC", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Razón Social Cesionario";
            oColumn.Editable = false;
            oColumn.Visible = ExtConf.Parametros.Visualiza_Cesion;
            oColumn.Width = 200;
            oColumn.DataBind.Bind(NombreDT, "U_RAZSOCC");

            oColumn = oColumns.Add("co_fechaCe", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Fecha Cesión";
            oColumn.Editable = false;
            oColumn.Visible = ExtConf.Parametros.Visualiza_Cesion;
            oColumn.Width = 60;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "U_FECHACE");

            oColumn = oColumns.Add("co_codResp", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Código Resposable";
            oColumn.Editable = false;
            oColumn.Visible = false;
            oColumn.Width = 60;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "U_CODRESP");

            oColumn = oColumns.Add("co_nomResp", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Nombre Responsable";
            oColumn.Editable = false;
            oColumn.Visible = false;
            oColumn.Width = 60;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "U_NOMRESP");

            oColumn = oColumns.Add("co_codResD", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Código Resp. Doc.";
            oColumn.Editable = false;
            oColumn.Visible = false;
            oColumn.Width = 60;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "U_CODRESPD");

            oColumn = oColumns.Add("co_nomResD", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Nombre Resp. Doc.";
            oColumn.Editable = false;
            oColumn.Visible = false;
            oColumn.Width = 60;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "U_NOMRESPD");
        }

        /// <summary>
        /// Genera estructura de Matrix documentos rechazados
        /// </summary>
        private void EstructuraMatrixIntegrados()
        {
            string NombreDT = "DOCUMENTOSINT";
            oForm.DataSources.DataTables.Add(NombreDT);
            SAPbouiCOM.DataTable datatable = oForm.DataSources.DataTables.Item(NombreDT);
            string _query = ObtenerQueryConFiltros("5");
            datatable.ExecuteQuery(_query);

            SAPbouiCOM.Columns oColumns;
            oColumns = oMatrixINT.Columns;
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

            oColumn = oColumns.Add("co_ChkINT", SAPbouiCOM.BoFormItemTypes.it_CHECK_BOX);
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
            oColumn.Visible = false;
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
            oColumn.Visible = true;
            oColumn.Width = 60;
            oColumn.DataBind.Bind(NombreDT, "U_DOCBASE");

            oColumn = oColumns.Add("co_docAso", SAPbouiCOM.BoFormItemTypes.it_LINKED_BUTTON);
            oColumn.TitleObject.Caption = "Link OC";
            oColumn.Editable = false;
            oColumn.Width = 60;
            oColumn.DataBind.Bind(NombreDT, "U_DOCASO");

            oColumn = oColumns.Add("co_estSii", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Estado SII";
            oColumn.Editable = false;
            oColumn.Visible = false;
            oColumn.Width = 160;
            oColumn.DataBind.Bind(NombreDT, "U_ESTSII");

            oColumn = oColumns.Add("co_estCome", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Estado Comercial";
            oColumn.Editable = false;
            oColumn.Visible = false;
            oColumn.Width = 60;
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

            //oColumn = oColumns.Add("co_docAso", SAPbouiCOM.BoFormItemTypes.it_LINKED_BUTTON);
            //oColumn.TitleObject.Caption = "Link OC";
            //oColumn.Editable = false;
            //oColumn.Width = 60;
            //oColumn.DataBind.Bind(NombreDT, "co_docAso");

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
            oColumn.Width = 60;
            oColumn.DataBind.Bind(NombreDT, "U_PRELIM");

            oColumn = oColumns.Add("co_rutCes", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Rut Cesionario";
            oColumn.Editable = false;
            oColumn.Visible = ExtConf.Parametros.Visualiza_Cesion;
            oColumn.Width = 70;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "U_RUTCES");

            oColumn = oColumns.Add("co_razSocC", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Razón Social Cesionario";
            oColumn.Editable = false;
            oColumn.Visible = ExtConf.Parametros.Visualiza_Cesion;
            oColumn.Width = 200;
            oColumn.DataBind.Bind(NombreDT, "U_RAZSOCC");

            oColumn = oColumns.Add("co_fechaCe", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Fecha Cesión";
            oColumn.Editable = false;
            oColumn.Visible = ExtConf.Parametros.Visualiza_Cesion;
            oColumn.Width = 60;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "U_FECHACE");

            oColumn = oColumns.Add("co_codResp", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Código Resposable";
            oColumn.Editable = false;
            oColumn.Visible = false;
            oColumn.Width = 60;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "U_CODRESP");

            oColumn = oColumns.Add("co_nomResp", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Nombre Responsable";
            oColumn.Editable = false;
            oColumn.Visible = false;
            oColumn.Width = 60;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "U_NOMRESP");

            oColumn = oColumns.Add("co_codResD", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Código Resp. Doc.";
            oColumn.Editable = false;
            oColumn.Visible = false;
            oColumn.Width = 60;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "U_CODRESPD");

            oColumn = oColumns.Add("co_nomResD", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Nombre Resp. Doc.";
            oColumn.Editable = false;
            oColumn.Visible = false;
            oColumn.Width = 60;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "U_NOMRESPD");
        }

        /// <summary>
        /// Genera estructura de Matrix documentos descargados
        /// </summary>
        private void EstructuraMatrixDescarga()
        {
            string NombreDT = "DOCUMENTOSDL";
            oForm.DataSources.DataTables.Add(NombreDT);
            SAPbouiCOM.DataTable datatable = oForm.DataSources.DataTables.Item(NombreDT);
            string _query = ObtenerQueryConFiltros("99");
            datatable.ExecuteQuery(_query);

            SAPbouiCOM.Columns oColumns;
            oColumns = oMatrixDL.Columns;
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

            oColumn = oColumns.Add("co_rutCes", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Rut Cesionario";
            oColumn.Editable = false;
            oColumn.Visible = ExtConf.Parametros.Visualiza_Cesion;
            oColumn.Width = 70;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "U_RUTCES");

            oColumn = oColumns.Add("co_razSocC", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Razón Social Cesionario";
            oColumn.Editable = false;
            oColumn.Visible = ExtConf.Parametros.Visualiza_Cesion;
            oColumn.Width = 200;
            oColumn.DataBind.Bind(NombreDT, "U_RAZSOCC");

            oColumn = oColumns.Add("co_fechaCe", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Fecha Cesión";
            oColumn.Editable = false;
            oColumn.Visible = ExtConf.Parametros.Visualiza_Cesion;
            oColumn.Width = 60;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "U_FECHACE");

            oColumn = oColumns.Add("co_codResp", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Código Resposable";
            oColumn.Editable = false;
            oColumn.Visible = false;
            oColumn.Width = 60;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "U_CODRESP");

            oColumn = oColumns.Add("co_nomResp", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Nombre Responsable";
            oColumn.Editable = false;
            oColumn.Visible = false;
            oColumn.Width = 60;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "U_NOMRESP");

            oColumn = oColumns.Add("co_codResD", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Código Resp. Doc.";
            oColumn.Editable = false;
            oColumn.Visible = false;
            oColumn.Width = 60;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "U_CODRESPD");

            oColumn = oColumns.Add("co_nomResD", SAPbouiCOM.BoFormItemTypes.it_EDIT);
            oColumn.TitleObject.Caption = "Nombre Resp. Doc.";
            oColumn.Editable = false;
            oColumn.Visible = false;
            oColumn.Width = 60;
            oColumn.RightJustified = true;
            oColumn.DataBind.Bind(NombreDT, "U_NOMRESPD");
        }

        private static string ObtenerQueryConFiltros(string estado)
        {
            String FechaFinal = String.Empty;
            String FechaInicial = String.Empty;
            String RutReceptorDTE = String.Empty;

            // Filtro por Tipo de Documento
            SAPbouiCOM.ComboBox oCombobox = (SAPbouiCOM.ComboBox)oForm.Items.Item("cmbTipoD").Specific;
            String FiltroTipo = oCombobox.Selected.Value;

            if (!String.IsNullOrEmpty(FiltroTipo))
            {
                FiltroTipo = string.Format(@" AND ""U_TIPODOC"" = '{0}'", FiltroTipo);
            }
            else
            {
                FiltroTipo = string.Format(@" AND ""U_TIPODOC"" IN ('33','34','52','56','61')");
            }

            // Filtro por Socio de Negocio
            SAPbouiCOM.EditText oRut = (SAPbouiCOM.EditText)oForm.Items.Item("txtRut").Specific;
            String FiltroSN = oRut.Value;
            FiltroSN = FiltroSN.Replace(".", string.Empty);

            if (!String.IsNullOrEmpty(FiltroSN))
            {
                //if (ExtConf.Parametros.Proveedor_FE.Equals("DBN"))
                //{
                //    var _rutreceptor = FiltroSN.Split('-');
                //    FiltroSN = _rutreceptor[0];
                //}
                FiltroSN = string.Format(@" AND ""U_RUTEMIS"" = '{0}'", FiltroSN);
            }
            else
            {
                RutReceptorDTE = ExtConf.Parametros.Rut_Receptor;
                //if (ExtConf.Parametros.Proveedor_FE.Equals("DBN"))
                //{
                //    var _rutreceptor = RutReceptorDTE.Split('-');
                //    RutReceptorDTE = _rutreceptor[0];
                //}
                FiltroSN = string.Format(@" AND ""U_RUTRECE"" = '{0}'", RutReceptorDTE);
            }

            // Filtro por Fechas
            SAPbouiCOM.EditText oFDesde = (SAPbouiCOM.EditText)oForm.Items.Item("txtFDesde").Specific;
            String DesdeFecha = oFDesde.Value;
            SAPbouiCOM.EditText oFHasta = (SAPbouiCOM.EditText)oForm.Items.Item("txtFHasta").Specific;
            String HastaFecha = oFHasta.Value;

            DateTime dt;
            String Mes = String.Empty;
            String Dia = String.Empty;
            // Fechas en formato AAAA-MM-DD
            if (!String.IsNullOrEmpty(DesdeFecha) && !String.IsNullOrEmpty(HastaFecha))
            {
                FechaInicial = String.Format("{0}{1}{2}", DesdeFecha.Substring(0, 4), DesdeFecha.Substring(4, 2), DesdeFecha.Substring(6, 2));
                FechaFinal = String.Format("{0}{1}{2}", HastaFecha.Substring(0, 4), HastaFecha.Substring(4, 2), HastaFecha.Substring(6, 2));
            }
            else
            {
                // Por defecto trae los últimos 1 días
                dt = new DateTime(DateTime.Now.Year, DateTime.Today.Month, DateTime.Today.Day);
                Mes = (dt.Month.ToString().Length == 1) ? dt.Month.ToString().PadLeft(2, '0') : dt.Month.ToString();
                Dia = (dt.Day.ToString().Length == 1) ? dt.Day.ToString().PadLeft(2, '0') : dt.Day.ToString();

                FechaFinal = String.Format("{0}{1}{2}", dt.Year.ToString(), Mes, Dia);

                //dt = DateTime.Today.AddDays(-30);
                dt = DateTime.Today.AddDays(-1);
                Mes = (dt.Month.ToString().Length == 1) ? dt.Month.ToString().PadLeft(2, '0') : dt.Month.ToString();
                Dia = (dt.Day.ToString().Length == 1) ? dt.Day.ToString().PadLeft(2, '0') : dt.Day.ToString();

                FechaInicial = String.Format("{0}{1}{2}", dt.Year.ToString(), Mes, Dia);
            }

            string FiltroFecha = string.Format(@" AND ""U_FECHAEM"" BETWEEN '{0}' AND '{1}'", FechaInicial, FechaFinal);

            // Filtro por Responsable
            SAPbouiCOM.EditText oResp = (SAPbouiCOM.EditText)oForm.Items.Item("txtResp").Specific;
            String FiltroResp = oResp.Value;

            if (!String.IsNullOrEmpty(FiltroResp))
            {
                FiltroResp = string.Format(@" AND ""U_CODRESP"" = '{0}'", FiltroResp);
            }
            else
            {
                FiltroResp = string.Empty;
            }

            // Filtro por Responsable Documento
            SAPbouiCOM.EditText oRespD = (SAPbouiCOM.EditText)oForm.Items.Item("txtRespD").Specific;
            String FiltroRespD = oRespD.Value;

            if (!String.IsNullOrEmpty(FiltroRespD))
            {
                FiltroRespD = string.Format(@" AND ""U_CODRESPD"" = '{0}'", FiltroRespD);
            }
            else
            {
                FiltroRespD = string.Empty;
            }

            string Filtros = string.Format(@" WHERE ""U_ESTADO"" = '{0}'{1}{2}{3}{4}{5}", estado, FiltroSN, FiltroFecha, FiltroTipo, FiltroResp, FiltroRespD);

            string _query = @"SELECT 0 AS ""NUM"", 'N' AS ""CHK"", REPLICATE(' ', 254) AS ""PDF"", * FROM ""@SO_MONITOR"" " + Filtros;
            return _query;
        }

        /// <summary>
        /// Carga documentos a una Matrix determinada desde UDO @SO_MONITOR
        /// </summary>
        /// <param name="oMatrix"></param>
        /// <param name="oForm"></param>
        /// <param name="estado"></param>
        private static void CargarDocumentosDesdeUDO(SAPbouiCOM.Matrix oMatrix, SAPbouiCOM.Form oForm, string estado, string datatable)
        {
            Application.SBO_Application.StatusBar.SetText("Cargando información. Espere unos momentos.", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);
            oForm.Freeze(true);

            string sPath = null;
            sPath = System.Windows.Forms.Application.StartupPath.ToString();
            sPath += "\\";
            sPath = sPath + "pdf.jpg";

            SAPbouiCOM.DataTable dtDoc = oForm.DataSources.DataTables.Item(datatable);

            // Limpiar Matrix
            SAPbouiCOM.Columns oColumns;
            oColumns = oMatrix.Columns;
            oColumns.Item("co_obs").Visible = false;
            dtDoc.Clear();
            dtDoc.Rows.Clear();
            oMatrix.Clear();
            oMatrix.LoadFromDataSource();

            // usando datatable
            string _query = ObtenerQueryConFiltros(estado);
            dtDoc.ExecuteQuery(_query);

            //for (int i = 0; i < dtDoc.Rows.Count; i++)
            //{
            //    dtDoc.SetValue("NUM", i, i + 1);
            //    dtDoc.SetValue("PDF", i, sPath);
            //}
            oMatrix.LoadFromDataSource();
            for (int i = 0; i < oMatrix.VisualRowCount; i++)
            {
                dtDoc.SetValue("NUM", i, i + 1);
                dtDoc.SetValue("PDF", i, sPath);
            }
            oMatrix.LoadFromDataSource();

            oForm.Freeze(false);
            Application.SBO_Application.StatusBar.SetText("Información cargada correctamente.", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success);
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
            String RutReceptorDTE = String.Empty;
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

            //RutReceptorDTE = SBO.ConsultasSBO.ObtenerRutEmpresaReceptora();
            RutReceptorDTE = ExtConf.Parametros.Rut_Receptor;

            // Filtro por Fechas
            SAPbouiCOM.EditText oFDesde = (SAPbouiCOM.EditText)oForm.Items.Item("txtFDesde").Specific;
            String DesdeFecha = oFDesde.Value;
            SAPbouiCOM.EditText oFHasta = (SAPbouiCOM.EditText)oForm.Items.Item("txtFHasta").Specific;
            String HastaFecha = oFHasta.Value;

            SAPbouiCOM.DataTable dtDoc = oForm.DataSources.DataTables.Item("DOCUMENTOSDL");
            // Limpiar Matrix
            dtDoc.Rows.Clear();
            oMatrix.Clear();

            System.Globalization.CultureInfo pesosChilenos2decimales = System.Globalization.CultureInfo.CreateSpecificCulture("es-CL");
            pesosChilenos2decimales.NumberFormat.CurrencyDecimalDigits = 2;
            pesosChilenos2decimales.NumberFormat.CurrencySymbol = "$";

            ProveedorDTE proveedorDTE = new ProveedorDTE();

            string[] parametros = new string[] { FiltroTipo, RutReceptorDTE, FiltroSN, DesdeFecha, HastaFecha, "1" };

            var provResult = proveedorDTE.ObtenerDocumentos(parametros);

            if (provResult.Success)
            {
                var _Datos = proveedorDTE.DTEResponse;

                if (_Datos != null && _Datos.Documentos != null)
                {
                    foreach (var item in _Datos.Documentos)
                    {
                        AgregarRegistroADataTableDesdeObjeto(dtDoc, ref IndexMatrix, item);
                        IndexMatrix++;
                    }

                    for (int i = 2; i <= _Datos.TotalPaginas; i++)
                    {
                        parametros = new string[] { FiltroTipo, RutReceptorDTE, FiltroSN, DesdeFecha, HastaFecha, i.ToString() };
                        provResult = proveedorDTE.ObtenerDocumentos(parametros);

                        if (provResult.Success)
                        {
                            _Datos = proveedorDTE.DTEResponse;

                            if (_Datos != null && _Datos.Documentos != null)
                            {
                                foreach (var item in _Datos.Documentos)
                                {
                                    AgregarRegistroADataTableDesdeObjeto(dtDoc, ref IndexMatrix, item);
                                    IndexMatrix++;
                                }
                            }
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
                        dtDoc.SetValue("NUM", IndexMatrix, IndexMatrix + 1);
                        dtDoc.SetValue("PDF", IndexMatrix, sPath);
                        dtDoc.SetValue("U_PLAZO", IndexMatrix, string.IsNullOrEmpty(item.Plazo.ToString()) ? 0 : item.Plazo);
                        dtDoc.SetValue("U_DOCID", IndexMatrix, item.DocId);
                        dtDoc.SetValue("U_TIPODOC", IndexMatrix, item.TipoDocumento);
                        dtDoc.SetValue("U_FOLIO", IndexMatrix, item.Folio.ToString());
                        dtDoc.SetValue("U_FECHAEM", IndexMatrix, item.FechaEmision);
                        dtDoc.SetValue("U_FORMAPA", IndexMatrix, string.IsNullOrEmpty(item.FormaDePago.ToString()) ? 0 : item.FormaDePago);
                        dtDoc.SetValue("U_IDETRAS", IndexMatrix, string.IsNullOrEmpty(item.IndicadorDeTraslado.ToString()) ? 0 : item.IndicadorDeTraslado);
                        dtDoc.SetValue("U_RUTEMIS", IndexMatrix, Local.Rut.ObtenerRutConGuion(item.RutEmisor));
                        dtDoc.SetValue("U_RAZSOCE", IndexMatrix, string.IsNullOrEmpty(item.RazonSocialEmisor) ? "" : item.RazonSocialEmisor);
                        dtDoc.SetValue("U_RUTRECE", IndexMatrix, Local.Rut.ObtenerRutConGuion(item.RutReceptor));
                        dtDoc.SetValue("U_RAZSOCR", IndexMatrix, string.IsNullOrEmpty(item.RazonSocialReceptor) ? "" : item.RazonSocialReceptor);
                        dtDoc.SetValue("U_CONTACT", IndexMatrix, string.IsNullOrEmpty(item.Contacto) ? "" : item.Contacto);
                        var iva = string.IsNullOrEmpty(item.Iva.ToString()) ? 0 : item.Iva;
                        //dtDoc.SetValue("U_IVA", IndexMatrix, String.Format(System.Globalization.CultureInfo.GetCultureInfo("es-CL"), "{0:C2}", iva));
                        //dtDoc.SetValue("U_IVA", IndexMatrix, Convert.ToDouble(string.Format(System.Globalization.CultureInfo.GetCultureInfo("es-CL"), "{0:C2}", iva)));
                        dtDoc.SetValue("U_IVA", IndexMatrix, iva);
                        //dtDoc.SetValue("U_MONTOTO", IndexMatrix, item.MontoTotal.ToString("C2", pesosChilenos2decimales));
                        var montototal = string.IsNullOrEmpty(item.MontoTotal.ToString()) ? 0 : item.MontoTotal;
                        //dtDoc.SetValue("U_MONTOTO", IndexMatrix, String.Format(System.Globalization.CultureInfo.GetCultureInfo("es-CL"), "{0:C2}", montototal));
                        //dtDoc.SetValue("U_MONTOTO", IndexMatrix, Convert.ToDouble(string.Format(System.Globalization.CultureInfo.GetCultureInfo("es-CL"), "{0:C2}", montototal)));
                        dtDoc.SetValue("U_MONTOTO", IndexMatrix, montototal);
                        var estadosii = string.IsNullOrEmpty(item.EstadoSii.ToString()) ? 0 : item.EstadoSii;
                        dtDoc.SetValue("U_ESTSII", IndexMatrix, Comun.Enumeradores.GetEstadoSii((Comun.Enumeradores.EstadosSii)estadosii));
                        var estadocomercial = string.IsNullOrEmpty(item.EstadoComercial.ToString()) ? 0 : item.EstadoComercial;
                        dtDoc.SetValue("U_ESTCOME", IndexMatrix, Comun.Enumeradores.GetEstadoComercial((Comun.Enumeradores.EstadosComerciales)estadocomercial));
                        dtDoc.SetValue("U_FECHARE", IndexMatrix, item.FechaRecepcion != null ? item.FechaRecepcion : item.FechaEmision);
                        dtDoc.SetValue("U_TIPO", IndexMatrix, item.Tipo);
                        dtDoc.SetValue("U_CODSII", IndexMatrix, string.IsNullOrEmpty(item.CodigoSii) ? "" : item.CodigoSii);
                        dtDoc.SetValue("U_TIENENC", IndexMatrix, string.IsNullOrEmpty(item.TieneNc) ? "" : item.TieneNc);
                        dtDoc.SetValue("U_TIENEND", IndexMatrix, string.IsNullOrEmpty(item.TieneNd) ? "" : item.TieneNd);
                        dtDoc.SetValue("U_DOCBASE", IndexMatrix, "");
                        dtDoc.SetValue("U_TIPOASO", IndexMatrix, "");
                        dtDoc.SetValue("U_DOCASO", IndexMatrix, "");
                        dtDoc.SetValue("U_RAZONRE", IndexMatrix, "");
                        dtDoc.SetValue("U_ESTADO", IndexMatrix, "0");
                        dtDoc.SetValue("U_PRELIM", IndexMatrix, "");
                        dtDoc.SetValue("U_RUTCES", IndexMatrix, item.RutCesionario);
                        dtDoc.SetValue("U_RAZSOCC", IndexMatrix, item.RazonSocialCesionario);
                        dtDoc.SetValue("U_FECHACE", IndexMatrix, item.FechaCesion);
                        dtDoc.SetValue("U_CODRESP", IndexMatrix, "0");
                        dtDoc.SetValue("U_NOMRESP", IndexMatrix, "");
                        dtDoc.SetValue("U_CODRESPD", IndexMatrix, "0");
                        dtDoc.SetValue("U_NOMRESPD", IndexMatrix, "");
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
        /// Agrega registros al UDO @SO_MONITOR desde un DataTable
        /// </summary>
        /// <param name="dtDoc"></param>
        private static void AgregarRegistrosUDOFromDT(SAPbouiCOM.DataTable dtDoc, string columna)
        {
            SAPbobsCOM.GeneralService oGeneralService = null;
            SAPbobsCOM.GeneralData oGeneralData = null;
            SAPbobsCOM.GeneralData oChild = null;
            SAPbobsCOM.GeneralDataCollection oChildren = null;
            SAPbobsCOM.GeneralDataParams oGeneralParams = null;
            SAPbobsCOM.CompanyService oCompanyService = null;

            Application.SBO_Application.StatusBar.SetText("Importando documentos. Espere unos momentos.", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);

            try
            {
                oCompanyService = SBO.ConexionSBO.oCompany.GetCompanyService();
                // Get GeneralService (oCmpSrv is the CompanyService)
                oGeneralService = oCompanyService.GetGeneralService("SO_MONITOR");
                // Create data for new row in main UDO
                oGeneralData = ((SAPbobsCOM.GeneralData)(oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralData)));

                int regImportados = 0;
                for (int i = 0; i < dtDoc.Rows.Count; i++)
                {
                    try
                    {
                        // sólo si check está marcado
                        if (dtDoc.GetValue(columna, i).ToString().Equals("Y"))
                        {
                            // validación si documento existe
                            string docid = string.Empty;
                            docid = dtDoc.GetValue("U_DOCID", i).ToString();
                            SAPbobsCOM.Recordset oRecord = (SAPbobsCOM.Recordset)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                            string _query = @"SELECT COUNT(1) AS ""RESP"" FROM ""@SO_MONITOR"" WHERE ""U_DOCID"" = '" + docid + "'";
                            oRecord.DoQuery(_query);
                            if (!oRecord.EoF)
                            {
                                if (oRecord.Fields.Item("RESP").Value.Equals(0))
                                {
                                    oGeneralData.SetProperty("U_PLAZO", dtDoc.GetValue("U_PLAZO", i));
                                    oGeneralData.SetProperty("U_DOCID", dtDoc.GetValue("U_DOCID", i));
                                    oGeneralData.SetProperty("U_TIPODOC", dtDoc.GetValue("U_TIPODOC", i));
                                    oGeneralData.SetProperty("U_FOLIO", dtDoc.GetValue("U_FOLIO", i));
                                    oGeneralData.SetProperty("U_FECHAEM", dtDoc.GetValue("U_FECHAEM", i));
                                    oGeneralData.SetProperty("U_FORMAPA", dtDoc.GetValue("U_FORMAPA", i));
                                    oGeneralData.SetProperty("U_IDETRAS", dtDoc.GetValue("U_IDETRAS", i));
                                    oGeneralData.SetProperty("U_RUTEMIS", dtDoc.GetValue("U_RUTEMIS", i));
                                    oGeneralData.SetProperty("U_RAZSOCE", dtDoc.GetValue("U_RAZSOCE", i));
                                    oGeneralData.SetProperty("U_RUTRECE", dtDoc.GetValue("U_RUTRECE", i));
                                    oGeneralData.SetProperty("U_RAZSOCR", dtDoc.GetValue("U_RAZSOCR", i));
                                    oGeneralData.SetProperty("U_CONTACT", dtDoc.GetValue("U_CONTACT", i));
                                    oGeneralData.SetProperty("U_IVA", dtDoc.GetValue("U_IVA", i));
                                    oGeneralData.SetProperty("U_MONTOTO", dtDoc.GetValue("U_MONTOTO", i));
                                    //var iva = Double.Parse(dtDoc.GetValue("co_iva", i).ToString(), System.Globalization.NumberStyles.Currency);
                                    //oGeneralData.SetProperty("U_IVA", iva);
                                    //var montoto = Double.Parse(dtDoc.GetValue("co_montoTo", i).ToString(), System.Globalization.NumberStyles.Currency);
                                    //oGeneralData.SetProperty("U_MONTOTO", montoto);
                                    //oGeneralData.SetProperty("U_ESTSII", dtDoc.GetValue("co_estSii", i));
                                    //var estadosii = Comun.Enumeradores.GetEstadoSii(dtDoc.GetValue("co_estSii", i).ToString());
                                    //oGeneralData.SetProperty("U_ESTSII", estadosii.ToString());
                                    //var estadocomercial = Comun.Enumeradores.GetEstadoComercial(dtDoc.GetValue("co_estCome", i).ToString());
                                    //oGeneralData.SetProperty("U_ESTCOME", estadocomercial.ToString());
                                    oGeneralData.SetProperty("U_ESTSII", dtDoc.GetValue("U_ESTSII", i));
                                    oGeneralData.SetProperty("U_ESTCOME", dtDoc.GetValue("U_ESTCOME", i));
                                    oGeneralData.SetProperty("U_FECHARE", dtDoc.GetValue("U_FECHARE", i));
                                    oGeneralData.SetProperty("U_TIPO", dtDoc.GetValue("U_TIPO", i));
                                    oGeneralData.SetProperty("U_CODSII", dtDoc.GetValue("U_CODSII", i));
                                    oGeneralData.SetProperty("U_TIENENC", dtDoc.GetValue("U_TIENENC", i));
                                    oGeneralData.SetProperty("U_TIENEND", dtDoc.GetValue("U_TIENEND", i));
                                    oGeneralData.SetProperty("U_DOCBASE", dtDoc.GetValue("U_DOCBASE", i));
                                    oGeneralData.SetProperty("U_TIPOASO", dtDoc.GetValue("U_TIPOASO", i));
                                    oGeneralData.SetProperty("U_DOCASO", dtDoc.GetValue("U_DOCASO", i));
                                    oGeneralData.SetProperty("U_RAZONRE", dtDoc.GetValue("U_RAZONRE", i));
                                    string estado = dtDoc.GetValue("U_ESTADO", i).ToString();
                                    if (string.IsNullOrEmpty(estado))
                                    {
                                        estado = "0";
                                    }
                                    oGeneralData.SetProperty("U_ESTADO", estado);
                                    oGeneralData.SetProperty("U_PRELIM", dtDoc.GetValue("U_PRELIM", i));
                                    oGeneralData.SetProperty("U_RUTCES", dtDoc.GetValue("U_RUTCES", i));
                                    oGeneralData.SetProperty("U_RAZSOCC", dtDoc.GetValue("U_RAZSOCC", i));
                                    if (dtDoc.GetValue("U_FECHACE", i) != null)
                                    {
                                        oGeneralData.SetProperty("U_FECHACE", dtDoc.GetValue("U_FECHACE", i));
                                    }
                                    string Rut = dtDoc.GetValue("U_RUTEMIS", i).ToString();
                                    string empID = string.Empty;
                                    string empName = string.Empty;
                                    if (!string.IsNullOrEmpty(Rut))
                                    {
                                        string CardCode = SBO.ConsultasSBO.ObtenerCardcode(Rut);
                                        if (!string.IsNullOrEmpty(CardCode))
                                        {
                                            empID = SBO.ConsultasSBO.ObtenerResponsableSN(CardCode);
                                            if (!string.IsNullOrEmpty(empID))
                                            {
                                                empName = SBO.ConsultasSBO.ObtenerNombreResponsableSN(empID);
                                                oGeneralData.SetProperty("U_CODRESP", empID);
                                                oGeneralData.SetProperty("U_NOMRESP", empName);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        oGeneralData.SetProperty("U_CODRESP", dtDoc.GetValue("U_CODRESP", i));
                                        oGeneralData.SetProperty("U_NOMRESP", dtDoc.GetValue("U_NOMRESP", i));
                                    }
                                    oGeneralData.SetProperty("U_CODRESPD", dtDoc.GetValue("U_CODRESPD", i));
                                    oGeneralData.SetProperty("U_NOMRESPD", dtDoc.GetValue("U_NOMRESPD", i));

                                    // Add the new row, including children, to database
                                    oGeneralParams = oGeneralService.Add(oGeneralData);
                                    //txtCode.Text = System.Convert.ToString(oGeneralParams.GetProperty("DocEntry"));
                                    //Interaction.MsgBox("Record added", (Microsoft.VisualBasic.MsgBoxStyle)(0), null);
                                    regImportados++;
                                    dtDoc.Rows.Remove(i);
                                    i--;

                                    ProveedorDTE proveedorDTE = new ProveedorDTE();
                                    string[] parametros = new string[] { docid };
                                    var provResult = proveedorDTE.ActualizaEstadoDTE(parametros);
                                }
                            }
                            Local.FuncionesComunes.LiberarObjetoGenerico(oRecord);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(string.Format("Reg: {0} - {1}", i, ex.Message));
                        Application.SBO_Application.StatusBar.SetText(string.Format("Reg: {0} - {1}", i, ex.Message), SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
                    }
                }
                ActualizarColumnaNumMatrix(oMatrixDL, dtDoc, "NUM");
                Application.SBO_Application.StatusBar.SetText(string.Format("Importación completa. Se importaron {0} nuevos documentos.", regImportados), SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //Interaction.MsgBox(ex.Message, (Microsoft.VisualBasic.MsgBoxStyle)(0), null);
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

        /// <summary>
        /// Valida que solo se haya seleccionado un registro en la matrix determinada
        /// </summary>
        /// <param name="oMatrix"></param>
        /// <param name="dtDoc"></param>
        /// <param name="columna"></param>
        private static Int32 CantidadRegistrosMarcados(SAPbouiCOM.Matrix oMatrix, SAPbouiCOM.DataTable dtDoc, string columna)
        {
            Int32 Cant = 0;
            for (int i = 0; i < dtDoc.Rows.Count; i++)
            {
                if (dtDoc.GetValue(columna, i).ToString().Equals("Y"))
                {
                    Cant++;
                }
            }
            return Cant;
        }

        /// <summary>
        /// Actualiza correlativo de la columna y matrix correspondiente
        /// </summary>
        /// <param name="oMatrix"></param>
        /// <param name="dtDoc"></param>
        /// <param name="column"></param>
        private static void ActualizarColumnaNumMatrix(SAPbouiCOM.Matrix oMatrix, SAPbouiCOM.DataTable dtDoc, string column)
        {
            for (int i = 0; i < dtDoc.Rows.Count; i++)
            {
                dtDoc.SetValue(column, i, i + 1);
            }
            oMatrix.LoadFromDataSource();
        }

        /// <summary>
        /// Valida registros marcados en la matrix correspondiente
        /// </summary>
        /// <param name="dtDoc"></param>
        /// <param name="columna"></param>
        private static void ValidarDocumentos(SAPbouiCOM.Matrix oMatrix, SAPbouiCOM.DataTable dtDoc, string columna)
        {
            SAPbobsCOM.GeneralService oGeneralService = null;
            SAPbobsCOM.GeneralData oGeneralData = null;
            SAPbobsCOM.GeneralData oChild = null;
            SAPbobsCOM.GeneralDataCollection oChildren = null;
            SAPbobsCOM.GeneralDataParams oGeneralParams = null;
            SAPbobsCOM.CompanyService oCompanyService = null;

            Application.SBO_Application.StatusBar.SetText("Validando documentos. Espere unos momentos.", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);

            String Rut = String.Empty;
            String BaseType = String.Empty;
            Int32 Tipo = 0;
            Int64 Folio = 0;
            Int32 DocEntryUDO;
            String OCManual = String.Empty;

            try
            {
                SAPbouiCOM.Columns oColumns;
                oColumns = oMatrix.Columns;
                oColumns.Item("co_obs").Visible = true;

                int regMarcados = 0;
                int regValidados = 0;
                for (int i = 0; i < dtDoc.Rows.Count; i++)
                {
                    DocEntryUDO = Convert.ToInt32(dtDoc.GetValue("DocEntry", i));
                    try
                    {
                        // sólo si check está marcado
                        if (dtDoc.GetValue(columna, i).ToString().Equals("Y"))
                        {
                            // Obtener datos necesarios de DataTable
                            string docId = dtDoc.GetValue("U_DOCID", i).ToString();
                            Rut = dtDoc.GetValue("U_RUTEMIS", i).ToString();
                            Tipo = Convert.ToInt16(dtDoc.GetValue("U_TIPODOC", i).ToString());
                            Folio = Convert.ToInt64(dtDoc.GetValue("U_FOLIO", i).ToString());
                            OCManual = dtDoc.GetValue("U_DOCBASE", i).ToString();

                            var validacion = ProcesaValidacionGeneral(docId, Rut, Tipo, Folio, OCManual);
                            if (validacion.Success)
                            {
                                Application.SBO_Application.StatusBar.SetText(String.Format("El DTE Nro {0} de Tipo {1} del proveedor {2} se puede integrar. Validaciones OK", Folio, Tipo, Rut), SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);

                                string docnum = SBO.ConsultasSBO.RecuperaDocNum(validacion.TpoDocRef, validacion.DocEntryBase, Tipo);
                                BaseType = String.Empty;

                                string empID = string.Empty;
                                string empName = string.Empty;
                                string slpcode = string.Empty;
                                string slpname = string.Empty;

                                if (Tipo.Equals(33) || Tipo.Equals(34) || Tipo.Equals(52) || Tipo.Equals(56) || Tipo.Equals(61))
                                {
                                    if (validacion.TpoDocRef.Equals("801"))
                                    {
                                        BaseType = "22";
                                    }
                                    else if (validacion.TpoDocRef.Equals("33"))
                                    {
                                        BaseType = "13";
                                    }
                                    else if (validacion.TpoDocRef.Equals(""))
                                    {
                                        BaseType = "";
                                    }
                                    else
                                    {
                                        BaseType = "20";
                                    }

                                    empID = SBO.ConsultasSBO.ObtenerResponsableSN(validacion.CardCode);
                                    if (!string.IsNullOrEmpty(empID) && !empID.Equals("0"))
                                    {
                                        empName = SBO.ConsultasSBO.ObtenerNombreResponsableSN(empID);
                                    }

                                    slpcode = SBO.ConsultasSBO.ObtenerResponsable(validacion.TpoDocRef, validacion.DocEntryBase, Tipo);
                                    if (!string.IsNullOrEmpty(slpcode))
                                    {
                                        slpname = SBO.ConsultasSBO.ObtenerNombreResponsable(slpcode);
                                    }
                                }

                                // Cambiar estado a validado
                                try
                                {
                                    oCompanyService = SBO.ConexionSBO.oCompany.GetCompanyService();
                                    // Get GeneralService (oCmpSrv is the CompanyService)
                                    oGeneralService = oCompanyService.GetGeneralService("SO_MONITOR");
                                    // Create data for new row in main UDO
                                    oGeneralParams = ((SAPbobsCOM.GeneralDataParams)(oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)));
                                    //oGeneralData = ((SAPbobsCOM.GeneralData)(oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralData)));
                                    oGeneralParams.SetProperty("DocEntry", DocEntryUDO);
                                    oGeneralData = oGeneralService.GetByParams(oGeneralParams);
                                    oGeneralData.SetProperty("U_DOCBASE", docnum);
                                    oGeneralData.SetProperty("U_TIPOASO", BaseType);
                                    oGeneralData.SetProperty("U_DOCASO", validacion.DocEntryBase == "0" ? "" : validacion.DocEntryBase);
                                    oGeneralData.SetProperty("U_ESTADO", "1");
                                    if (!empID.Equals("0") && !string.IsNullOrEmpty(empID))
                                    {
                                        oGeneralData.SetProperty("U_CODRESP", empID);
                                        oGeneralData.SetProperty("U_NOMRESP", empName);
                                    }
                                    else
                                    {
                                        oGeneralData.SetProperty("U_CODRESP", 0);
                                        oGeneralData.SetProperty("U_NOMRESP", "");
                                    }
                                    if (!slpcode.Equals("-1") && !string.IsNullOrEmpty(slpcode))
                                    {
                                        oGeneralData.SetProperty("U_CODRESPD", slpcode);
                                        oGeneralData.SetProperty("U_NOMRESPD", slpname);
                                    }
                                    else
                                    {
                                        oGeneralData.SetProperty("U_CODRESPD", -1);
                                        oGeneralData.SetProperty("U_NOMRESPD", "");
                                    }
                                    oGeneralService.Update(oGeneralData);
                                    dtDoc.SetValue("U_DOCBASE", i, docnum);
                                    dtDoc.SetValue("U_TIPOASO", i, BaseType);
                                    dtDoc.SetValue("U_DOCASO", i, validacion.DocEntryBase == "0" ? "" : validacion.DocEntryBase);
                                    dtDoc.SetValue("U_OBS", i, "OK");
                                    if (!empID.Equals("0") && !string.IsNullOrEmpty(empID))
                                    {
                                        dtDoc.SetValue("U_CODRESP", i, empID);
                                        dtDoc.SetValue("U_NOMRESP", i, empName);
                                    }
                                    else
                                    {
                                        dtDoc.SetValue("U_CODRESP", i, 0);
                                        dtDoc.SetValue("U_NOMRESP", i, "");
                                    }
                                    if (!slpcode.Equals("-1") && !string.IsNullOrEmpty(slpcode))
                                    {
                                        dtDoc.SetValue("U_CODRESPD", i, slpcode);
                                        dtDoc.SetValue("U_NOMRESPD", i, slpname);
                                    }
                                    else
                                    {
                                        dtDoc.SetValue("U_CODRESPD", i, -1);
                                        dtDoc.SetValue("U_NOMRESPD", i, "");
                                    }
                                    regValidados++;
                                    dtDoc.Rows.Remove(i);
                                    i--;
                                }
                                catch (Exception ex)
                                {
                                    dtDoc.SetValue("U_OBS", i, ex.Message);
                                }

                            }
                            else
                            {
                                dtDoc.SetValue("U_OBS", i, validacion.Mensaje);
                                Application.SBO_Application.StatusBar.SetText(validacion.Mensaje, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);
                            }
                            regMarcados++;
                        }
                        else
                        {
                            dtDoc.SetValue("U_OBS", i, string.Empty);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(string.Format("Reg: {0} - {1}", i, ex.Message));
                    }
                }
                ActualizarColumnaNumMatrix(oMatrix, dtDoc, "NUM");
                Application.SBO_Application.StatusBar.SetText(string.Format("Validación completa. Se procesaron {0} documentos. {1} documentos validados OK", regMarcados, regValidados), SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void ValidarDocumentosMX(SAPbouiCOM.Matrix oMatrix, SAPbouiCOM.DataTable dtDoc, string columna)
        {
            SAPbobsCOM.GeneralService oGeneralService = null;
            SAPbobsCOM.GeneralData oGeneralData = null;
            SAPbobsCOM.GeneralData oChild = null;
            SAPbobsCOM.GeneralDataCollection oChildren = null;
            SAPbobsCOM.GeneralDataParams oGeneralParams = null;
            SAPbobsCOM.CompanyService oCompanyService = null;

            Application.SBO_Application.StatusBar.SetText("Validando documentos. Espere unos momentos.", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);

            String Rut = String.Empty;
            String BaseType = String.Empty;
            Int32 Tipo = 0;
            Int64 Folio = 0;
            Int32 DocEntryUDO;
            String OCManual = String.Empty;

            try
            {
                SAPbouiCOM.Columns oColumns;
                oColumns = oMatrix.Columns;
                oColumns.Item("co_obs").Visible = true;

                int regMarcados = 0;
                int regValidados = 0;
                for (int i = 0; i < dtDoc.Rows.Count; i++)
                {
                    DocEntryUDO = Convert.ToInt32(dtDoc.GetValue("DocEntry", i));
                    try
                    {
                        // sólo si check está marcado
                        if (dtDoc.GetValue(columna, i).ToString().Equals("Y"))
                        {
                            // Obtener datos necesarios de DataTable
                            string docId = dtDoc.GetValue("U_DOCID", i).ToString();
                            Rut = dtDoc.GetValue("U_RUTEMIS", i).ToString();
                            Tipo = Convert.ToInt16(dtDoc.GetValue("U_TIPODOC", i).ToString());
                            Folio = Convert.ToInt64(dtDoc.GetValue("U_FOLIO", i).ToString());
                            OCManual = dtDoc.GetValue("U_DOCBASE", i).ToString();

                            var validacion = ProcesaValidacionMX(docId, Rut, Tipo, Folio, OCManual);
                            if (validacion.Success)
                            {
                                Application.SBO_Application.StatusBar.SetText(String.Format("El DTE Nro {0} de Tipo {1} del proveedor {2} se puede integrar. Validaciones OK", Folio, Tipo, Rut), SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);

                                string docnum = SBO.ConsultasSBO.RecuperaDocNum(validacion.TpoDocRef, validacion.DocEntryBase, Tipo);
                                BaseType = String.Empty;

                                string empID = string.Empty;
                                string empName = string.Empty;
                                string slpcode = string.Empty;
                                string slpname = string.Empty;

                                if (Tipo.Equals(33) || Tipo.Equals(34) || Tipo.Equals(52) || Tipo.Equals(56) || Tipo.Equals(61))
                                {
                                    if (validacion.TpoDocRef.Equals("801"))
                                    {
                                        BaseType = "22";
                                    }
                                    else if (validacion.TpoDocRef.Equals("33"))
                                    {
                                        BaseType = "13";
                                    }
                                    else if (validacion.TpoDocRef.Equals(""))
                                    {
                                        BaseType = "";
                                    }
                                    else
                                    {
                                        BaseType = "20";
                                    }

                                    empID = SBO.ConsultasSBO.ObtenerResponsableSN(validacion.CardCode);
                                    if (!string.IsNullOrEmpty(empID) && !empID.Equals("0"))
                                    {
                                        empName = SBO.ConsultasSBO.ObtenerNombreResponsableSN(empID);
                                    }

                                    slpcode = SBO.ConsultasSBO.ObtenerResponsable(validacion.TpoDocRef, validacion.DocEntryBase, Tipo);
                                    if (!string.IsNullOrEmpty(slpcode))
                                    {
                                        slpname = SBO.ConsultasSBO.ObtenerNombreResponsable(slpcode);
                                    }
                                }

                                // Cambiar estado a validado
                                try
                                {
                                    oCompanyService = SBO.ConexionSBO.oCompany.GetCompanyService();
                                    // Get GeneralService (oCmpSrv is the CompanyService)
                                    oGeneralService = oCompanyService.GetGeneralService("SO_MONITOR");
                                    // Create data for new row in main UDO
                                    oGeneralParams = ((SAPbobsCOM.GeneralDataParams)(oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)));
                                    //oGeneralData = ((SAPbobsCOM.GeneralData)(oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralData)));
                                    oGeneralParams.SetProperty("DocEntry", DocEntryUDO);
                                    oGeneralData = oGeneralService.GetByParams(oGeneralParams);
                                    oGeneralData.SetProperty("U_DOCBASE", docnum);
                                    oGeneralData.SetProperty("U_TIPOASO", BaseType);
                                    oGeneralData.SetProperty("U_DOCASO", validacion.DocEntryBase == "0" ? "" : validacion.DocEntryBase);
                                    oGeneralData.SetProperty("U_ESTADO", "1");
                                    if (!empID.Equals("0") && !string.IsNullOrEmpty(empID))
                                    {
                                        oGeneralData.SetProperty("U_CODRESP", empID);
                                        oGeneralData.SetProperty("U_NOMRESP", empName);
                                    }
                                    else
                                    {
                                        oGeneralData.SetProperty("U_CODRESP", 0);
                                        oGeneralData.SetProperty("U_NOMRESP", "");
                                    }
                                    if (!slpcode.Equals("-1") && !string.IsNullOrEmpty(slpcode))
                                    {
                                        oGeneralData.SetProperty("U_CODRESPD", slpcode);
                                        oGeneralData.SetProperty("U_NOMRESPD", slpname);
                                    }
                                    else
                                    {
                                        oGeneralData.SetProperty("U_CODRESPD", -1);
                                        oGeneralData.SetProperty("U_NOMRESPD", "");
                                    }
                                    oGeneralService.Update(oGeneralData);
                                    dtDoc.SetValue("U_DOCBASE", i, docnum);
                                    dtDoc.SetValue("U_TIPOASO", i, BaseType);
                                    dtDoc.SetValue("U_DOCASO", i, validacion.DocEntryBase == "0" ? "" : validacion.DocEntryBase);
                                    dtDoc.SetValue("U_OBS", i, "OK");
                                    if (!empID.Equals("0") && !string.IsNullOrEmpty(empID))
                                    {
                                        dtDoc.SetValue("U_CODRESP", i, empID);
                                        dtDoc.SetValue("U_NOMRESP", i, empName);
                                    }
                                    else
                                    {
                                        dtDoc.SetValue("U_CODRESP", i, 0);
                                        dtDoc.SetValue("U_NOMRESP", i, "");
                                    }
                                    if (!slpcode.Equals("-1") && !string.IsNullOrEmpty(slpcode))
                                    {
                                        dtDoc.SetValue("U_CODRESPD", i, slpcode);
                                        dtDoc.SetValue("U_NOMRESPD", i, slpname);
                                    }
                                    else
                                    {
                                        dtDoc.SetValue("U_CODRESPD", i, -1);
                                        dtDoc.SetValue("U_NOMRESPD", i, "");
                                    }
                                    regValidados++;
                                    dtDoc.Rows.Remove(i);
                                    i--;
                                }
                                catch (Exception ex)
                                {
                                    dtDoc.SetValue("U_OBS", i, ex.Message);
                                }

                            }
                            else
                            {
                                dtDoc.SetValue("U_OBS", i, validacion.Mensaje);
                                Application.SBO_Application.StatusBar.SetText(validacion.Mensaje, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);
                            }
                            regMarcados++;
                        }
                        else
                        {
                            dtDoc.SetValue("U_OBS", i, string.Empty);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(string.Format("Reg: {0} - {1}", i, ex.Message));
                    }
                }
                ActualizarColumnaNumMatrix(oMatrix, dtDoc, "NUM");
                Application.SBO_Application.StatusBar.SetText(string.Format("Validación completa. Se procesaron {0} documentos. {1} documentos validados OK", regMarcados, regValidados), SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void ValidarDocumentosSinOC(SAPbouiCOM.Matrix oMatrix, SAPbouiCOM.DataTable dtDoc, string columna)
        {
            SAPbobsCOM.GeneralService oGeneralService = null;
            SAPbobsCOM.GeneralData oGeneralData = null;
            SAPbobsCOM.GeneralData oChild = null;
            SAPbobsCOM.GeneralDataCollection oChildren = null;
            SAPbobsCOM.GeneralDataParams oGeneralParams = null;
            SAPbobsCOM.CompanyService oCompanyService = null;

            Application.SBO_Application.StatusBar.SetText("Validando documentos. Espere unos momentos.", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);

            String Rut = String.Empty;
            String BaseType = String.Empty;
            Int32 Tipo = 0;
            Int64 Folio = 0;
            Int32 DocEntryUDO;
            String OCManual = String.Empty;

            try
            {
                SAPbouiCOM.Columns oColumns;
                oColumns = oMatrix.Columns;
                oColumns.Item("co_obs").Visible = true;

                int regMarcados = 0;
                int regValidados = 0;
                for (int i = 0; i < dtDoc.Rows.Count; i++)
                {
                    DocEntryUDO = Convert.ToInt32(dtDoc.GetValue("DocEntry", i));
                    try
                    {
                        // sólo si check está marcado
                        if (dtDoc.GetValue(columna, i).ToString().Equals("Y"))
                        {
                            // Obtener datos necesarios de DataTable
                            string docId = dtDoc.GetValue("U_DOCID", i).ToString();
                            Rut = dtDoc.GetValue("U_RUTEMIS", i).ToString();
                            Tipo = Convert.ToInt16(dtDoc.GetValue("U_TIPODOC", i).ToString());
                            Folio = Convert.ToInt64(dtDoc.GetValue("U_FOLIO", i).ToString());
                            OCManual = dtDoc.GetValue("U_DOCBASE", i).ToString();

                            var validacion = ProcesaValidacionSinOC(docId, Rut, Tipo, Folio, OCManual);
                            if (validacion.Success)
                            {
                                Application.SBO_Application.StatusBar.SetText(String.Format("El DTE Nro {0} de Tipo {1} del proveedor {2} se puede integrar. Validaciones OK", Folio, Tipo, Rut), SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);

                                string docnum = SBO.ConsultasSBO.RecuperaDocNum(validacion.TpoDocRef, validacion.DocEntryBase, Tipo);
                                BaseType = String.Empty;

                                string empID = string.Empty;
                                string empName = string.Empty;
                                string slpcode = string.Empty;
                                string slpname = string.Empty;

                                if (Tipo.Equals(33) || Tipo.Equals(34) || Tipo.Equals(52) || Tipo.Equals(56) || Tipo.Equals(61))
                                {
                                    if (validacion.TpoDocRef.Equals("801"))
                                    {
                                        BaseType = "22";
                                    }
                                    else if (validacion.TpoDocRef.Equals("33"))
                                    {
                                        BaseType = "13";
                                    }
                                    else if (validacion.TpoDocRef.Equals(""))
                                    {
                                        BaseType = "";
                                    }
                                    else
                                    {
                                        BaseType = "20";
                                    }

                                    empID = SBO.ConsultasSBO.ObtenerResponsableSN(validacion.CardCode);
                                    if (!string.IsNullOrEmpty(empID) && !empID.Equals("0"))
                                    {
                                        empName = SBO.ConsultasSBO.ObtenerNombreResponsableSN(empID);
                                    }

                                    slpcode = SBO.ConsultasSBO.ObtenerResponsable(validacion.TpoDocRef, validacion.DocEntryBase, Tipo);
                                    if (!string.IsNullOrEmpty(slpcode))
                                    {
                                        slpname = SBO.ConsultasSBO.ObtenerNombreResponsable(slpcode);
                                    }
                                }

                                // Cambiar estado a validado
                                try
                                {
                                    oCompanyService = SBO.ConexionSBO.oCompany.GetCompanyService();
                                    // Get GeneralService (oCmpSrv is the CompanyService)
                                    oGeneralService = oCompanyService.GetGeneralService("SO_MONITOR");
                                    // Create data for new row in main UDO
                                    oGeneralParams = ((SAPbobsCOM.GeneralDataParams)(oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)));
                                    //oGeneralData = ((SAPbobsCOM.GeneralData)(oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralData)));
                                    oGeneralParams.SetProperty("DocEntry", DocEntryUDO);
                                    oGeneralData = oGeneralService.GetByParams(oGeneralParams);
                                    oGeneralData.SetProperty("U_DOCBASE", docnum);
                                    oGeneralData.SetProperty("U_TIPOASO", BaseType);
                                    oGeneralData.SetProperty("U_DOCASO", validacion.DocEntryBase == "0" ? "" : validacion.DocEntryBase);
                                    oGeneralData.SetProperty("U_ESTADO", "1");
                                    if (!empID.Equals("0") && !string.IsNullOrEmpty(empID))
                                    {
                                        oGeneralData.SetProperty("U_CODRESP", empID);
                                        oGeneralData.SetProperty("U_NOMRESP", empName);
                                    }
                                    else
                                    {
                                        oGeneralData.SetProperty("U_CODRESP", 0);
                                        oGeneralData.SetProperty("U_NOMRESP", "");
                                    }
                                    if (!slpcode.Equals("-1") && !string.IsNullOrEmpty(slpcode))
                                    {
                                        oGeneralData.SetProperty("U_CODRESPD", slpcode);
                                        oGeneralData.SetProperty("U_NOMRESPD", slpname);
                                    }
                                    else
                                    {
                                        oGeneralData.SetProperty("U_CODRESPD", -1);
                                        oGeneralData.SetProperty("U_NOMRESPD", "");
                                    }
                                    oGeneralService.Update(oGeneralData);
                                    dtDoc.SetValue("U_DOCBASE", i, docnum);
                                    dtDoc.SetValue("U_TIPOASO", i, BaseType);
                                    dtDoc.SetValue("U_DOCASO", i, validacion.DocEntryBase == "0" ? "" : validacion.DocEntryBase);
                                    dtDoc.SetValue("U_OBS", i, "OK");
                                    if (!empID.Equals("0") && !string.IsNullOrEmpty(empID))
                                    {
                                        dtDoc.SetValue("U_CODRESP", i, empID);
                                        dtDoc.SetValue("U_NOMRESP", i, empName);
                                    }
                                    else
                                    {
                                        dtDoc.SetValue("U_CODRESP", i, 0);
                                        dtDoc.SetValue("U_NOMRESP", i, "");
                                    }
                                    if (!slpcode.Equals("-1") && !string.IsNullOrEmpty(slpcode))
                                    {
                                        dtDoc.SetValue("U_CODRESPD", i, slpcode);
                                        dtDoc.SetValue("U_NOMRESPD", i, slpname);
                                    }
                                    else
                                    {
                                        dtDoc.SetValue("U_CODRESPD", i, -1);
                                        dtDoc.SetValue("U_NOMRESPD", i, "");
                                    }
                                    regValidados++;
                                    dtDoc.Rows.Remove(i);
                                    i--;
                                }
                                catch (Exception ex)
                                {
                                    dtDoc.SetValue("U_OBS", i, ex.Message);
                                }

                            }
                            else
                            {
                                dtDoc.SetValue("U_OBS", i, validacion.Mensaje);
                                Application.SBO_Application.StatusBar.SetText(validacion.Mensaje, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);
                            }
                            regMarcados++;
                        }
                        else
                        {
                            dtDoc.SetValue("U_OBS", i, string.Empty);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(string.Format("Reg: {0} - {1}", i, ex.Message));
                    }
                }
                ActualizarColumnaNumMatrix(oMatrix, dtDoc, "NUM");
                Application.SBO_Application.StatusBar.SetText(string.Format("Validación completa. Se procesaron {0} documentos. {1} documentos validados OK", regMarcados, regValidados), SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static Local.Message ProcesaValidacionGeneral(string docId, string Rut, int Tipo, Int64 Folio, string OCManual)
        {
            Local.Message retorno = new Local.Message();
            Local.Configuracion ExtConf = new Local.Configuracion();
            Local.Message result = new Local.Message();
            string CardCode = string.Empty;
            bool EsProveedorOC = true;
            string DecodeString = string.Empty;
            bool existeOC = false;
            bool existeEM = false;
            bool existeFC = false;
            string DocEntryBase = string.Empty;
            string DocTypeBase = string.Empty;
            string ObjTypeBase = string.Empty;

            try
            {
                // Validación de DTE ya integrado por RUT - TIPO - FOLIO
                result = SBO.ConsultasSBO.ValidacionDTEIntegrado(Rut, Tipo, Folio);
                if (!result.Success)
                {
                    retorno.Success = result.Success;
                    retorno.Mensaje = result.Mensaje;
                    return retorno;
                }

                // Validar existencia de socio de negocios
                CardCode = SBO.ConsultasSBO.ObtenerCardcode(Rut);
                if (string.IsNullOrEmpty(CardCode))
                {
                    retorno.Success = false;
                    retorno.Mensaje = string.Format("El DTE Nro {0} de Tipo {1} del proveedor {2} no tiene Socio de Negocio", Folio, Tipo, Rut);
                    return retorno;
                }

                // Obtener documento DTE
                ProveedorDTE proveedorDTE = new ProveedorDTE();
                string[] parametros = new string[] { docId };
                var provResult = proveedorDTE.ObtenerDocumento(parametros);
                if (!provResult.Success)
                {
                    retorno.Success = provResult.Success;
                    retorno.Mensaje = provResult.Mensaje;
                    return retorno;
                }

                var _Datos = proveedorDTE.DetalleDocuDTE;

                DecodeString = _Datos.XmlData;
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(DecodeString);

                //var newjson = Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc, Newtonsoft.Json.Formatting.None, false);
                var newjson = Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc);

                var settings = new JsonSerializerSettings();
                settings.NullValueHandling = NullValueHandling.Ignore;

                var _Documento = Newtonsoft.Json.JsonConvert.DeserializeObject<ClasesDTE.JsonDTE>(newjson, settings);

                var objDTE = _Documento.DTE.Documento;
                retorno.objDTE = objDTE;

                // Sólo buscar referencias para Facturas Afectas, Exentas y Guias
                if (Tipo.Equals(33) || Tipo.Equals(34) || Tipo.Equals(52))
                {
                    // Buscar referencias en DTE
                    // si no existen referencias en DTE
                    if (objDTE.Referencia == null)
                    {
                        retorno.Success = false;
                        retorno.Mensaje = string.Format("El DTE Nro {0} de Tipo {1} del proveedor {2} no tiene referencias", Folio, Tipo, Rut);
                    }
                    // Si existen referencias en DTE
                    if (objDTE.Referencia != null && string.IsNullOrEmpty(OCManual))
                    {
                        // Validacion de Referencia a OC
                        foreach (ClasesDTE.Referencia docRef in objDTE.Referencia)
                        {
                            if (docRef.TpoDocRef.Equals("801"))
                            {
                                // Validar que referencia exista en SAP
                                existeOC = SBO.ConsultasSBO.ExisteReferencia(docRef.TpoDocRef, docRef.FolioRef, CardCode, ref DocEntryBase, ref DocTypeBase, ref ObjTypeBase, Tipo);
                                retorno.TpoDocRef = docRef.TpoDocRef;
                                retorno.FolioRef = docRef.FolioRef;
                                retorno.DocEntryBase = DocEntryBase;
                                retorno.DocTypeBase = DocTypeBase;
                                retorno.ObjTypeBase = ObjTypeBase;
                                retorno.CardCode = CardCode;
                                break;
                            }
                        }
                        if (!existeOC)
                        {
                            retorno.Success = false;
                            retorno.Mensaje = string.Format("El DTE Nro {0} de Tipo {1} del proveedor {2} no tiene referencia de OC existente en SAP BO", Folio, Tipo, Rut);
                        }
                    }
                    // Si no existe OC y si tiene OC Manual
                    if (!existeOC && !string.IsNullOrEmpty(OCManual))
                    {
                        // Validar que referencia exista en SAP
                        existeOC = SBO.ConsultasSBO.ExisteReferencia("801", OCManual, CardCode, ref DocEntryBase, ref DocTypeBase, ref ObjTypeBase, Tipo);
                        retorno.TpoDocRef = "801";
                        retorno.FolioRef = OCManual;
                        retorno.DocEntryBase = DocEntryBase;
                        retorno.DocTypeBase = DocTypeBase;
                        retorno.ObjTypeBase = ObjTypeBase;
                        retorno.CardCode = CardCode;
                        if (!existeOC)
                        {
                            retorno.Success = false;
                            retorno.Mensaje = string.Format("El DTE Nro {0} de Tipo {1} del proveedor {2} no tiene referencia de OC existente en SAP BO", Folio, Tipo, Rut);
                        }
                    }

                    // Consultar si Proveedor requiere OC
                    EsProveedorOC = true;
                    EsProveedorOC = SBO.ConsultasSBO.EsProveedorOC(CardCode);

                    // Si es Proveedor OC
                    if (EsProveedorOC)
                    {
                        // Si se exige OC
                        if (ExtConf.Parametros.Valida_ExisteOC)
                        {
                            if (existeOC)
                            {
                                // Si se exige EM
                                if (ExtConf.Parametros.Valida_ExisteEntrada)
                                {
                                    if (!Tipo.Equals(52))
                                    {
                                        existeEM = SBO.ConsultasSBO.ExisteEntrada(DocEntryBase);
                                        if (!existeEM)
                                        {
                                            retorno.Success = false;
                                            retorno.Mensaje = string.Format("El DTE Nro {0} de Tipo {1} del proveedor {2} no tiene entradas registradas en SAP BO", Folio, Tipo, Rut);
                                            return retorno;
                                        }
                                    }
                                }
                                retorno.Mensaje = string.Format("El DTE Nro {0} de Tipo {1} del proveedor {2} se puede integrar. Validaciones OK", Folio, Tipo, Rut);

                                // Validacion montos DTE y OC
                                result = ValidaMontosDTEyOC(objDTE, Rut, Tipo, Folio, OCManual, existeOC, existeEM, DocEntryBase, ObjTypeBase);
                                if (!result.Success)
                                {
                                    retorno.Success = result.Success;
                                    retorno.Mensaje = result.Mensaje;
                                    retorno.Content = result.Content;
                                    return retorno;
                                }
                            }
                            else
                            {
                                retorno.Success = false;
                                return retorno;
                            }
                        }
                    }
                    // Proveedor no OC
                    else
                    {
                        // Si se exige monto máximo SN no OC
                        if (ExtConf.Parametros.Valida_MontoMaximo)
                        {
                            if (objDTE.Encabezado.Totales.MntTotal > ExtConf.Parametros.Valida_ValorMontoMaximo)
                            {
                                retorno.Success = false;
                                retorno.Mensaje = string.Format("El DTE Nro {0} de Tipo {1} del proveedor {2} excede el monto máximo para SN sin OC", Folio, Tipo, Rut);
                                return retorno;
                            }
                        }
                        if (!existeOC)
                        {
                            retorno.TpoDocRef = "";
                            retorno.FolioRef = "";
                            retorno.DocEntryBase = "0";
                            retorno.DocTypeBase = "";
                            retorno.ObjTypeBase = "";
                            retorno.CardCode = CardCode;
                        }
                        retorno.Mensaje = string.Format("El DTE Nro {0} de Tipo {1} del proveedor {2} se puede integrar. Validaciones OK", Folio, Tipo, Rut);
                    }
                    if(existeOC)
                    {
                        long open = 0;
                        open = SBO.ConsultasSBO.ObtenerReferenciaAbierta(retorno.TpoDocRef, retorno.FolioRef, CardCode, Tipo);
                        if (existeEM)
                        {
                            open += SBO.ConsultasSBO.ObtenerEntradasAbiertas(DocEntryBase);
                        }
                        if (open.Equals(0))
                        {
                            retorno.Success = false;
                            retorno.Mensaje = string.Format("El DTE Nro {0} de Tipo {1} del proveedor {2} no tiene OC ni entradas abiertas en SAP BO", Folio, Tipo, Rut);
                            return retorno;
                        }
                    }
                }
                // Sólo buscar referencias para Notas de Débito y Crédito
                else if (Tipo.Equals(56) || Tipo.Equals(61))
                {
                    // Buscar referencias en DTE
                    // si no existen referencias en DTE
                    if (objDTE.Referencia == null)
                    {
                        retorno.Success = false;
                        retorno.Mensaje = string.Format("El DTE Nro {0} de Tipo {1} del proveedor {2} no tiene referencias", Folio, Tipo, Rut);
                    }
                    // Si existen referencias en DTE
                    if (objDTE.Referencia != null && string.IsNullOrEmpty(OCManual))
                    {
                        // Validacion de Referencia a OC
                        foreach (ClasesDTE.Referencia docRef in objDTE.Referencia)
                        {
                            if (docRef.TpoDocRef.Equals("33"))
                            {
                                // Validar que referencia exista en SAP
                                existeFC = SBO.ConsultasSBO.ExisteReferencia(docRef.TpoDocRef, docRef.FolioRef, CardCode, ref DocEntryBase, ref DocTypeBase, ref ObjTypeBase, Tipo);
                                retorno.TpoDocRef = docRef.TpoDocRef;
                                retorno.FolioRef = docRef.FolioRef;
                                retorno.DocEntryBase = DocEntryBase;
                                retorno.DocTypeBase = DocTypeBase;
                                retorno.ObjTypeBase = ObjTypeBase;
                                retorno.CardCode = CardCode;
                                break;
                            }
                        }
                        if (!existeFC)
                        {
                            retorno.Success = false;
                            retorno.Mensaje = string.Format("El DTE Nro {0} de Tipo {1} del proveedor {2} no tiene referencia de FC existente en SAP BO", Folio, Tipo, Rut);
                        }
                    }
                    // Si no existe OC y si tiene OC Manual
                    if (!existeFC && !string.IsNullOrEmpty(OCManual))
                    {
                        // Validar que referencia exista en SAP
                        existeFC = SBO.ConsultasSBO.ExisteReferencia("33", OCManual, CardCode, ref DocEntryBase, ref DocTypeBase, ref ObjTypeBase, Tipo);
                        retorno.TpoDocRef = "33";
                        retorno.FolioRef = OCManual;
                        retorno.DocEntryBase = DocEntryBase;
                        retorno.DocTypeBase = DocTypeBase;
                        retorno.ObjTypeBase = ObjTypeBase;
                        retorno.CardCode = CardCode;
                        if (!existeFC)
                        {
                            retorno.Success = false;
                            retorno.Mensaje = string.Format("El DTE Nro {0} de Tipo {1} del proveedor {2} no tiene referencia de FC existente en SAP BO", Folio, Tipo, Rut);
                        }
                    }

                    if (existeFC)
                    {
                        retorno.Mensaje = string.Format("El DTE Nro {0} de Tipo {1} del proveedor {2} se puede integrar. Validaciones OK", Folio, Tipo, Rut);

                        // Validacion montos DTE y OC
                        result = ValidaMontosDTEyOC(objDTE, Rut, Tipo, Folio, OCManual, existeOC, existeEM, DocEntryBase, ObjTypeBase);
                        if (!result.Success)
                        {
                            retorno.Success = result.Success;
                            retorno.Mensaje = result.Mensaje;
                            retorno.Content = result.Content;
                            return retorno;
                        }
                    }
                    else
                    {
                        retorno.Success = false;
                        return retorno;
                    }
                }
                else
                {
                    retorno.TpoDocRef = "";
                    retorno.FolioRef = "";
                    retorno.DocEntryBase = "0";
                    retorno.DocTypeBase = "";
                    retorno.ObjTypeBase = "";
                    retorno.CardCode = CardCode;
                }

                // Validacion encabezados DTE
                result = ValidaEncabezadoDTE(objDTE, Rut, Tipo, Folio, OCManual);
                if (!result.Success)
                {
                    retorno.Success = result.Success;
                    retorno.Mensaje = result.Mensaje;
                    return retorno;
                }

                // Cumple todas las validaciones
                retorno.Success = true;
            }
            catch (Exception ex)
            {
                retorno.Success = false;
                retorno.Mensaje = ex.Message;
            }
            return retorno;
        }

        private static Local.Message ProcesaValidacionMX(string docId, string Rut, int Tipo, Int64 Folio, string OCManual)
        {
            Local.Message retorno = new Local.Message();
            Local.Configuracion ExtConf = new Local.Configuracion();
            Local.Message result = new Local.Message();
            string CardCode = string.Empty;
            bool EsProveedorOC = true;
            string DecodeString = string.Empty;
            bool existeOC = false;
            bool existeEM = false;
            bool existeFC = false;
            string DocEntryBase = string.Empty;
            string DocTypeBase = string.Empty;
            string ObjTypeBase = string.Empty;

            try
            {
                // Validación de DTE ya integrado por RUT - TIPO - FOLIO
                result = SBO.ConsultasSBO.ValidacionDTEIntegrado(Rut, Tipo, Folio);
                if (!result.Success)
                {
                    retorno.Success = result.Success;
                    retorno.Mensaje = result.Mensaje;
                    return retorno;
                }

                // Validar existencia de socio de negocios
                CardCode = SBO.ConsultasSBO.ObtenerCardcode(Rut);
                if (string.IsNullOrEmpty(CardCode))
                {
                    retorno.Success = false;
                    retorno.Mensaje = string.Format("El DTE Nro {0} de Tipo {1} del proveedor {2} no tiene Socio de Negocio", Folio, Tipo, Rut);
                    return retorno;
                }

                // Obtener documento DTE
                ProveedorDTE proveedorDTE = new ProveedorDTE();
                string[] parametros = new string[] { docId };
                var provResult = proveedorDTE.ObtenerDocumento(parametros);
                if (!provResult.Success)
                {
                    retorno.Success = provResult.Success;
                    retorno.Mensaje = provResult.Mensaje;
                    return retorno;
                }

                var _Datos = proveedorDTE.DetalleDocuDTE;

                DecodeString = _Datos.XmlData;
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(DecodeString);

                //var newjson = Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc, Newtonsoft.Json.Formatting.None, false);
                var newjson = Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc);

                var settings = new JsonSerializerSettings();
                settings.NullValueHandling = NullValueHandling.Ignore;

                var _Documento = Newtonsoft.Json.JsonConvert.DeserializeObject<ClasesDTE.JsonDTE>(newjson, settings);

                var objDTE = _Documento.DTE.Documento;
                retorno.objDTE = objDTE;

                // Sólo buscar referencias para Facturas Afectas, Exentas y Guias
                if (Tipo.Equals(33) || Tipo.Equals(34) || Tipo.Equals(52))
                {
                    // Buscar referencias en DTE
                    // si no existen referencias en DTE
                    if (objDTE.Referencia == null)
                    {
                        retorno.Success = false;
                        retorno.Mensaje = string.Format("El DTE Nro {0} de Tipo {1} del proveedor {2} no tiene referencias", Folio, Tipo, Rut);
                    }
                    // Si existen referencias en DTE
                    if (objDTE.Referencia != null && string.IsNullOrEmpty(OCManual))
                    {
                        // Validacion de Referencia a OC
                        foreach (ClasesDTE.Referencia docRef in objDTE.Referencia)
                        {
                            if (docRef.TpoDocRef.Equals("801"))
                            {
                                // Validar que referencia exista en SAP
                                existeOC = SBO.ConsultasSBO.ExisteReferencia(docRef.TpoDocRef, docRef.FolioRef, CardCode, ref DocEntryBase, ref DocTypeBase, ref ObjTypeBase, Tipo);
                                retorno.TpoDocRef = docRef.TpoDocRef;
                                retorno.FolioRef = docRef.FolioRef;
                                retorno.DocEntryBase = DocEntryBase;
                                retorno.DocTypeBase = DocTypeBase;
                                retorno.ObjTypeBase = ObjTypeBase;
                                retorno.CardCode = CardCode;
                                break;
                            }
                        }
                        if (!existeOC)
                        {
                            retorno.Success = false;
                            retorno.Mensaje = string.Format("El DTE Nro {0} de Tipo {1} del proveedor {2} no tiene referencia de OC existente en SAP BO", Folio, Tipo, Rut);
                        }
                    }
                    // Si no existe OC y si tiene OC Manual
                    if (!existeOC && !string.IsNullOrEmpty(OCManual))
                    {
                        // Validar que referencia exista en SAP
                        existeOC = SBO.ConsultasSBO.ExisteReferencia("801", OCManual, CardCode, ref DocEntryBase, ref DocTypeBase, ref ObjTypeBase, Tipo);
                        retorno.TpoDocRef = "801";
                        retorno.FolioRef = OCManual;
                        retorno.DocEntryBase = DocEntryBase;
                        retorno.DocTypeBase = DocTypeBase;
                        retorno.ObjTypeBase = ObjTypeBase;
                        retorno.CardCode = CardCode;
                        if (!existeOC)
                        {
                            retorno.Success = false;
                            retorno.Mensaje = string.Format("El DTE Nro {0} de Tipo {1} del proveedor {2} no tiene referencia de OC existente en SAP BO", Folio, Tipo, Rut);
                        }
                    }

                    // Consultar si Proveedor requiere OC
                    EsProveedorOC = true;
                    EsProveedorOC = SBO.ConsultasSBO.EsProveedorOC(CardCode);

                    // Si es Proveedor OC
                    if (EsProveedorOC)
                    {
                        // Si se exige OC
                        if (ExtConf.Parametros.Valida_ExisteOC)
                        {
                            if (existeOC)
                            {
                                // Si se exige EM
                                if (ExtConf.Parametros.Valida_ExisteEntrada)
                                {
                                    if (!Tipo.Equals(52))
                                    {
                                        existeEM = SBO.ConsultasSBO.ExisteEntrada(DocEntryBase);
                                        if (!existeEM)
                                        {
                                            retorno.Success = false;
                                            retorno.Mensaje = string.Format("El DTE Nro {0} de Tipo {1} del proveedor {2} no tiene entradas registradas en SAP BO", Folio, Tipo, Rut);
                                            return retorno;
                                        }
                                    }
                                }
                                retorno.Mensaje = string.Format("El DTE Nro {0} de Tipo {1} del proveedor {2} se puede integrar. Validaciones OK", Folio, Tipo, Rut);

                                //// Validacion montos DTE y OC
                                //result = ValidaMontosDTEyOC(objDTE, Rut, Tipo, Folio, OCManual, existeOC, existeEM, DocEntryBase, ObjTypeBase);
                                //if (!result.Success)
                                //{
                                //    retorno.Success = result.Success;
                                //    retorno.Mensaje = result.Mensaje;
                                //    retorno.Content = result.Content;
                                //    return retorno;
                                //}

                                var EsMonExtranjera = SBO.ConsultasSBO.EsOCMonedaExtranjera(DocEntryBase, ObjTypeBase);
                                if(!EsMonExtranjera)
                                {
                                    retorno.Success = false;
                                    retorno.Mensaje = string.Format("El DTE Nro {0} de Tipo {1} del proveedor {2} no posee documento base en moneda extranjera", Folio, Tipo, Rut);
                                    return retorno;
                                }
                            }
                            else
                            {
                                retorno.Success = false;
                                return retorno;
                            }
                        }
                    }
                    // Proveedor no OC
                    else
                    {
                        // Si se exige monto máximo SN no OC
                        if (ExtConf.Parametros.Valida_MontoMaximo)
                        {
                            if (objDTE.Encabezado.Totales.MntTotal > ExtConf.Parametros.Valida_ValorMontoMaximo)
                            {
                                retorno.Success = false;
                                retorno.Mensaje = string.Format("El DTE Nro {0} de Tipo {1} del proveedor {2} excede el monto máximo para SN sin OC", Folio, Tipo, Rut);
                                return retorno;
                            }
                        }
                        if (!existeOC)
                        {
                            retorno.TpoDocRef = "";
                            retorno.FolioRef = "";
                            retorno.DocEntryBase = "0";
                            retorno.DocTypeBase = "";
                            retorno.ObjTypeBase = "";
                            retorno.CardCode = CardCode;
                        }
                        retorno.Mensaje = string.Format("El DTE Nro {0} de Tipo {1} del proveedor {2} se puede integrar. Validaciones OK", Folio, Tipo, Rut);
                    }
                    if (existeOC)
                    {
                        long open = 0;
                        open = SBO.ConsultasSBO.ObtenerReferenciaAbierta(retorno.TpoDocRef, retorno.FolioRef, CardCode, Tipo);
                        if (existeEM)
                        {
                            open += SBO.ConsultasSBO.ObtenerEntradasAbiertas(DocEntryBase);
                        }
                        if (open.Equals(0))
                        {
                            retorno.Success = false;
                            retorno.Mensaje = string.Format("El DTE Nro {0} de Tipo {1} del proveedor {2} no tiene OC ni entradas abiertas en SAP BO", Folio, Tipo, Rut);
                            return retorno;
                        }
                    }
                }
                // Sólo buscar referencias para Notas de Débito y Crédito
                else if (Tipo.Equals(56) || Tipo.Equals(61))
                {
                    // Buscar referencias en DTE
                    // si no existen referencias en DTE
                    if (objDTE.Referencia == null)
                    {
                        retorno.Success = false;
                        retorno.Mensaje = string.Format("El DTE Nro {0} de Tipo {1} del proveedor {2} no tiene referencias", Folio, Tipo, Rut);
                    }
                    // Si existen referencias en DTE
                    if (objDTE.Referencia != null && string.IsNullOrEmpty(OCManual))
                    {
                        // Validacion de Referencia a OC
                        foreach (ClasesDTE.Referencia docRef in objDTE.Referencia)
                        {
                            if (docRef.TpoDocRef.Equals("33"))
                            {
                                // Validar que referencia exista en SAP
                                existeFC = SBO.ConsultasSBO.ExisteReferencia(docRef.TpoDocRef, docRef.FolioRef, CardCode, ref DocEntryBase, ref DocTypeBase, ref ObjTypeBase, Tipo);
                                retorno.TpoDocRef = docRef.TpoDocRef;
                                retorno.FolioRef = docRef.FolioRef;
                                retorno.DocEntryBase = DocEntryBase;
                                retorno.DocTypeBase = DocTypeBase;
                                retorno.ObjTypeBase = ObjTypeBase;
                                retorno.CardCode = CardCode;
                                break;
                            }
                        }
                        if (!existeFC)
                        {
                            retorno.Success = false;
                            retorno.Mensaje = string.Format("El DTE Nro {0} de Tipo {1} del proveedor {2} no tiene referencia de FC existente en SAP BO", Folio, Tipo, Rut);
                        }
                    }
                    // Si no existe OC y si tiene OC Manual
                    if (!existeFC && !string.IsNullOrEmpty(OCManual))
                    {
                        // Validar que referencia exista en SAP
                        existeFC = SBO.ConsultasSBO.ExisteReferencia("33", OCManual, CardCode, ref DocEntryBase, ref DocTypeBase, ref ObjTypeBase, Tipo);
                        retorno.TpoDocRef = "33";
                        retorno.FolioRef = OCManual;
                        retorno.DocEntryBase = DocEntryBase;
                        retorno.DocTypeBase = DocTypeBase;
                        retorno.ObjTypeBase = ObjTypeBase;
                        retorno.CardCode = CardCode;
                        if (!existeFC)
                        {
                            retorno.Success = false;
                            retorno.Mensaje = string.Format("El DTE Nro {0} de Tipo {1} del proveedor {2} no tiene referencia de FC existente en SAP BO", Folio, Tipo, Rut);
                        }
                    }

                    if (existeFC)
                    {
                        retorno.Mensaje = string.Format("El DTE Nro {0} de Tipo {1} del proveedor {2} se puede integrar. Validaciones OK", Folio, Tipo, Rut);

                        //// Validacion montos DTE y OC
                        //result = ValidaMontosDTEyOC(objDTE, Rut, Tipo, Folio, OCManual, existeOC, existeEM, DocEntryBase, ObjTypeBase);
                        //if (!result.Success)
                        //{
                        //    retorno.Success = result.Success;
                        //    retorno.Mensaje = result.Mensaje;
                        //    retorno.Content = result.Content;
                        //    return retorno;
                        //}

                        var EsMonExtranjera = SBO.ConsultasSBO.EsOCMonedaExtranjera(DocEntryBase, ObjTypeBase);
                        if (!EsMonExtranjera)
                        {
                            retorno.Success = false;
                            retorno.Mensaje = string.Format("El DTE Nro {0} de Tipo {1} del proveedor {2} no posee documento base en moneda extranjera", Folio, Tipo, Rut);
                            return retorno;
                        }
                    }
                    else
                    {
                        retorno.Success = false;
                        return retorno;
                    }
                }
                else
                {
                    retorno.TpoDocRef = "";
                    retorno.FolioRef = "";
                    retorno.DocEntryBase = "0";
                    retorno.DocTypeBase = "";
                    retorno.ObjTypeBase = "";
                    retorno.CardCode = CardCode;
                }

                // Validacion encabezados DTE
                result = ValidaEncabezadoDTE(objDTE, Rut, Tipo, Folio, OCManual);
                if (!result.Success)
                {
                    retorno.Success = result.Success;
                    retorno.Mensaje = result.Mensaje;
                    return retorno;
                }

                // Cumple todas las validaciones
                retorno.Success = true;
            }
            catch (Exception ex)
            {
                retorno.Success = false;
                retorno.Mensaje = ex.Message;
            }
            return retorno;
        }

        private static Local.Message ProcesaValidacionSinOC(string docId, string Rut, int Tipo, Int64 Folio, string OCManual)
        {
            Local.Message retorno = new Local.Message();
            Local.Configuracion ExtConf = new Local.Configuracion();
            Local.Message result = new Local.Message();
            string CardCode = string.Empty;
            bool EsProveedorOC = true;
            string DecodeString = string.Empty;
            bool existeOC = false;
            bool existeEM = false;
            bool existeFC = false;
            string DocEntryBase = string.Empty;
            string DocTypeBase = string.Empty;
            string ObjTypeBase = string.Empty;

            try
            {
                // Validación de DTE ya integrado por RUT - TIPO - FOLIO
                result = SBO.ConsultasSBO.ValidacionDTEIntegrado(Rut, Tipo, Folio);
                if (!result.Success)
                {
                    retorno.Success = result.Success;
                    retorno.Mensaje = result.Mensaje;
                    return retorno;
                }

                // Validar existencia de socio de negocios
                CardCode = SBO.ConsultasSBO.ObtenerCardcode(Rut);
                if (string.IsNullOrEmpty(CardCode))
                {
                    retorno.Success = false;
                    retorno.Mensaje = string.Format("El DTE Nro {0} de Tipo {1} del proveedor {2} no tiene Socio de Negocio", Folio, Tipo, Rut);
                    return retorno;
                }

                // Obtener documento DTE
                ProveedorDTE proveedorDTE = new ProveedorDTE();
                string[] parametros = new string[] { docId };
                var provResult = proveedorDTE.ObtenerDocumento(parametros);
                if (!provResult.Success)
                {
                    retorno.Success = provResult.Success;
                    retorno.Mensaje = provResult.Mensaje;
                    return retorno;
                }

                var _Datos = proveedorDTE.DetalleDocuDTE;

                DecodeString = _Datos.XmlData;
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(DecodeString);

                //var newjson = Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc, Newtonsoft.Json.Formatting.None, false);
                var newjson = Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc);

                var settings = new JsonSerializerSettings();
                settings.NullValueHandling = NullValueHandling.Ignore;

                var _Documento = Newtonsoft.Json.JsonConvert.DeserializeObject<ClasesDTE.JsonDTE>(newjson, settings);

                var objDTE = _Documento.DTE.Documento;
                retorno.objDTE = objDTE;

                // Sólo buscar referencias para Facturas Afectas, Exentas y Guias
                if (Tipo.Equals(33) || Tipo.Equals(34) || Tipo.Equals(52))
                {
                    // Buscar referencias en DTE
                    // si no existen referencias en DTE
                    if (objDTE.Referencia == null)
                    {
                        retorno.Success = false;
                        retorno.Mensaje = string.Format("El DTE Nro {0} de Tipo {1} del proveedor {2} no tiene referencias", Folio, Tipo, Rut);
                    }
                    // Si existen referencias en DTE
                    if (objDTE.Referencia != null && string.IsNullOrEmpty(OCManual))
                    {
                        // Validacion de Referencia a OC
                        foreach (ClasesDTE.Referencia docRef in objDTE.Referencia)
                        {
                            if (docRef.TpoDocRef.Equals("801"))
                            {
                                // Validar que referencia exista en SAP
                                existeOC = SBO.ConsultasSBO.ExisteReferencia(docRef.TpoDocRef, docRef.FolioRef, CardCode, ref DocEntryBase, ref DocTypeBase, ref ObjTypeBase, Tipo);
                                retorno.TpoDocRef = docRef.TpoDocRef;
                                retorno.FolioRef = docRef.FolioRef;
                                retorno.DocEntryBase = DocEntryBase;
                                retorno.DocTypeBase = DocTypeBase;
                                retorno.ObjTypeBase = ObjTypeBase;
                                retorno.CardCode = CardCode;
                                break;
                            }
                        }
                        if (!existeOC)
                        {
                            retorno.Success = false;
                            retorno.Mensaje = string.Format("El DTE Nro {0} de Tipo {1} del proveedor {2} no tiene referencia de OC existente en SAP BO", Folio, Tipo, Rut);
                        }
                    }
                    // Si no existe OC y si tiene OC Manual
                    if (!existeOC && !string.IsNullOrEmpty(OCManual))
                    {
                        // Validar que referencia exista en SAP
                        existeOC = SBO.ConsultasSBO.ExisteReferencia("801", OCManual, CardCode, ref DocEntryBase, ref DocTypeBase, ref ObjTypeBase, Tipo);
                        retorno.TpoDocRef = "801";
                        retorno.FolioRef = OCManual;
                        retorno.DocEntryBase = DocEntryBase;
                        retorno.DocTypeBase = DocTypeBase;
                        retorno.ObjTypeBase = ObjTypeBase;
                        retorno.CardCode = CardCode;
                        if (!existeOC)
                        {
                            retorno.Success = false;
                            retorno.Mensaje = string.Format("El DTE Nro {0} de Tipo {1} del proveedor {2} no tiene referencia de OC existente en SAP BO", Folio, Tipo, Rut);
                        }
                    }

                    // Si existe OC no puede continuar con esta opción
                    if (existeOC)
                    {
                        retorno.Success = false;
                        retorno.Mensaje = string.Format("El DTE Nro {0} de Tipo {1} del proveedor {2} tiene referencia de OC existente en SAP BO", Folio, Tipo, Rut);
                        return retorno;
                    }

                    // Si finalmente no existe OC y no tiene OC Manual
                    if (!existeOC)
                    {
                        retorno.TpoDocRef = "";
                        retorno.FolioRef = "";
                        retorno.DocEntryBase = "0";
                        retorno.DocTypeBase = "";
                        retorno.ObjTypeBase = "";
                        retorno.CardCode = CardCode;
                    }
                    retorno.Mensaje = string.Format("El DTE Nro {0} de Tipo {1} del proveedor {2} se puede integrar. Validaciones OK", Folio, Tipo, Rut);

                }
                // Sólo buscar referencias para Notas de Débito y Crédito
                else if (Tipo.Equals(56) || Tipo.Equals(61))
                {
                    // Buscar referencias en DTE
                    // si no existen referencias en DTE
                    if (objDTE.Referencia == null)
                    {
                        retorno.Success = false;
                        retorno.Mensaje = string.Format("El DTE Nro {0} de Tipo {1} del proveedor {2} no tiene referencias", Folio, Tipo, Rut);
                    }
                    // Si existen referencias en DTE
                    if (objDTE.Referencia != null && string.IsNullOrEmpty(OCManual))
                    {
                        // Validacion de Referencia a OC
                        foreach (ClasesDTE.Referencia docRef in objDTE.Referencia)
                        {
                            if (docRef.TpoDocRef.Equals("33"))
                            {
                                // Validar que referencia exista en SAP
                                existeFC = SBO.ConsultasSBO.ExisteReferencia(docRef.TpoDocRef, docRef.FolioRef, CardCode, ref DocEntryBase, ref DocTypeBase, ref ObjTypeBase, Tipo);
                                retorno.TpoDocRef = docRef.TpoDocRef;
                                retorno.FolioRef = docRef.FolioRef;
                                retorno.DocEntryBase = DocEntryBase;
                                retorno.DocTypeBase = DocTypeBase;
                                retorno.ObjTypeBase = ObjTypeBase;
                                retorno.CardCode = CardCode;
                                break;
                            }
                        }
                        if (!existeFC)
                        {
                            retorno.Success = false;
                            retorno.Mensaje = string.Format("El DTE Nro {0} de Tipo {1} del proveedor {2} no tiene referencia de FC existente en SAP BO", Folio, Tipo, Rut);
                        }
                    }
                    // Si no existe OC y si tiene OC Manual
                    if (!existeFC && !string.IsNullOrEmpty(OCManual))
                    {
                        // Validar que referencia exista en SAP
                        existeFC = SBO.ConsultasSBO.ExisteReferencia("33", OCManual, CardCode, ref DocEntryBase, ref DocTypeBase, ref ObjTypeBase, Tipo);
                        retorno.TpoDocRef = "33";
                        retorno.FolioRef = OCManual;
                        retorno.DocEntryBase = DocEntryBase;
                        retorno.DocTypeBase = DocTypeBase;
                        retorno.ObjTypeBase = ObjTypeBase;
                        retorno.CardCode = CardCode;
                        if (!existeFC)
                        {
                            retorno.Success = false;
                            retorno.Mensaje = string.Format("El DTE Nro {0} de Tipo {1} del proveedor {2} no tiene referencia de FC existente en SAP BO", Folio, Tipo, Rut);
                        }
                    }

                    if (existeFC)
                    {
                        retorno.Mensaje = string.Format("El DTE Nro {0} de Tipo {1} del proveedor {2} se puede integrar. Validaciones OK", Folio, Tipo, Rut);

                        // Validacion montos DTE y OC
                        result = ValidaMontosDTEyOC(objDTE, Rut, Tipo, Folio, OCManual, existeOC, existeEM, DocEntryBase, ObjTypeBase);
                        if (!result.Success)
                        {
                            retorno.Success = result.Success;
                            retorno.Mensaje = result.Mensaje;
                            retorno.Content = result.Content;
                            return retorno;
                        }
                    }
                    else
                    {
                        retorno.Success = false;
                        return retorno;
                    }
                }
                else
                {
                    retorno.TpoDocRef = "";
                    retorno.FolioRef = "";
                    retorno.DocEntryBase = "0";
                    retorno.DocTypeBase = "";
                    retorno.ObjTypeBase = "";
                    retorno.CardCode = CardCode;
                }

                // Validacion encabezados DTE
                result = ValidaEncabezadoDTE(objDTE, Rut, Tipo, Folio, OCManual);
                if (!result.Success)
                {
                    retorno.Success = result.Success;
                    retorno.Mensaje = result.Mensaje;
                    return retorno;
                }

                // Cumple todas las validaciones
                retorno.Success = true;
            }
            catch (Exception ex)
            {
                retorno.Success = false;
                retorno.Mensaje = ex.Message;
            }
            return retorno;
        }

        private static Local.Message ValidaMontosDTE(ClasesDTE.Documento objDTE, string Rut, int Tipo, Int64 Folio, string OCManual, bool existeOC, bool existeEM, string DocEntryBase, string DocTypeBase)
        {
            Local.Message retorno = new Local.Message();
            //Local.Configuracion ExtConf = new Local.Configuracion();
            //Local.Message result = new Local.Message();
            retorno.Success = true;

            try
            {
                if (true)
                {
                    if (existeOC)
                    {
                        //Validar Montos OC y Tolerancias
                        if (!SBO.ConsultasSBO.ValidaMontos(objDTE.Encabezado.Totales.MntTotal, DocEntryBase, DocTypeBase))
                        {
                            retorno.Success = false;
                            retorno.Mensaje = string.Format("El DTE Nro {0} de Tipo {1} del proveedor {2} no conincide con los montos y tolerancias de la OC", Folio, Tipo, Rut);
                            return retorno;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                retorno.Success = false;
                retorno.Mensaje = ex.Message;
            }
            return retorno;
        }
        private static Local.Message ValidaMontosDTEyOC(ClasesDTE.Documento objDTE, string Rut, int Tipo, Int64 Folio, string OCManual, bool existeOC, bool existeEM, string DocEntryBase, string DocTypeBase)
        {
            Local.Message retorno = new Local.Message();
            Local.Configuracion ExtConf = new Local.Configuracion();
            //Local.Message result = new Local.Message();
            retorno.Success = true;

            try
            {
                if (ExtConf.Parametros.Valida_MontoOC)
                {
                    if (existeOC)
                    {
                        //Diferencia Montos DTE y OC
                        var diferencia = SBO.ConsultasSBO.ObtieneDiferenciaMontosDTEyOC(objDTE.Encabezado.Totales.MntTotal, DocEntryBase, DocTypeBase);

                        if (ExtConf.Parametros.Valida_PermiteTolerancias)
                        {
                            if (diferencia < ExtConf.Parametros.Valida_ValorRechazoMontoMenor || diferencia > ExtConf.Parametros.Valida_ValorRechazoMontoMayor)
                            {
                                var EsMonExtranjera = SBO.ConsultasSBO.EsOCMonedaExtranjera(DocEntryBase, DocTypeBase);
                                if (EsMonExtranjera)
                                {
                                    retorno.Success = false;
                                    retorno.Mensaje = string.Format("El DTE Nro {0} de Tipo {1} del proveedor {2} excede los montos y/o tolerancias de la OC. Documento base en moneda extranjera", Folio, Tipo, Rut);
                                    retorno.Content = "INVALIDO";
                                }
                                else
                                {
                                    retorno.Success = false;
                                    retorno.Mensaje = string.Format("El DTE Nro {0} de Tipo {1} del proveedor {2} excede los montos y/o tolerancias de la OC", Folio, Tipo, Rut);
                                    retorno.Content = "RECHAZAR";
                                }
                                return retorno;
                            }
                            else if (diferencia < ExtConf.Parametros.Valida_ValorAprobacionMontoMenor && diferencia > ExtConf.Parametros.Valida_ValorAprobacionMontoMayor)
                            {
                                retorno.Success = true;
                                retorno.Mensaje = string.Format("El DTE Nro {0} de Tipo {1} del proveedor {2} conincide con los montos y/o tolerancias de la OC", Folio, Tipo, Rut);
                                retorno.Content = "APROBAR";
                            }
                            else if (diferencia.Equals(0))
                            {
                                retorno.Success = true;
                                retorno.Mensaje = string.Format("El DTE Nro {0} de Tipo {1} del proveedor {2} conincide con los montos y/o tolerancias de la OC", Folio, Tipo, Rut);
                                retorno.Content = "APROBAR";
                            }
                            else
                            {
                                retorno.Success = false;
                                retorno.Mensaje = string.Format("El DTE Nro {0} de Tipo {1} del proveedor {2} no coincide con los montos y/o tolerancias de la OC", Folio, Tipo, Rut);
                                retorno.Content = "INVALIDO";
                                return retorno;
                            }
                        }
                        else
                        {
                            if (diferencia.Equals(0))
                            {
                                retorno.Success = true;
                                retorno.Mensaje = string.Format("El DTE Nro {0} de Tipo {1} del proveedor {2} conincide con los montos de la OC", Folio, Tipo, Rut);
                                retorno.Content = "APROBAR";
                            }
                            else
                            {
                                retorno.Success = false;
                                retorno.Mensaje = string.Format("El DTE Nro {0} de Tipo {1} del proveedor {2} no coincide con los montos de la OC", Folio, Tipo, Rut);
                                retorno.Content = "INVALIDO";
                                return retorno;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                retorno.Success = false;
                retorno.Mensaje = ex.Message;
            }
            return retorno;
        }

        private static Local.Message ValidaEncabezadoDTE(ClasesDTE.Documento objDTE, string Rut, int Tipo, Int64 Folio, string OCManual)
        {
            Local.Message retorno = new Local.Message();
            Local.Configuracion ExtConf = new Local.Configuracion();
            //Local.Message result = new Local.Message();
            retorno.Success = true;

            try
            {
                if (ExtConf.Parametros.Valida_EncabezadosDTE)
                {
                    //Validar Razon Social
                    if (!SBO.ConsultasSBO.ValidaRazonSocial(objDTE.Encabezado.Receptor.RznSocRecep))
                    {
                        retorno.Success = false;
                        retorno.Mensaje = string.Format("El DTE Nro {0} de Tipo {1} del proveedor {2} no contiene una Razón Social de receptor válida", Folio, Tipo, Rut);
                        retorno.Content = "RECHAZAR";
                        return retorno;
                    }
                    //Validar Giro
                    if (!SBO.ConsultasSBO.ValidaGiro(objDTE.Encabezado.Receptor.GiroRecep))
                    {
                        retorno.Success = false;
                        retorno.Mensaje = string.Format("El DTE Nro {0} de Tipo {1} del proveedor {2} no contiene un Giro de receptor válido", Folio, Tipo, Rut);
                        retorno.Content = "RECHAZAR";
                        return retorno;
                    }
                    //Validar Dirección
                    if (!SBO.ConsultasSBO.ValidaDireccion(objDTE.Encabezado.Receptor.DirRecep))
                    {
                        retorno.Success = false;
                        retorno.Mensaje = string.Format("El DTE Nro {0} de Tipo {1} del proveedor {2} no contiene una Dirección de receptor válida", Folio, Tipo, Rut);
                        retorno.Content = "RECHAZAR";
                        return retorno;
                    }
                    //Validar Comuna
                    if (!SBO.ConsultasSBO.ValidaComuna(objDTE.Encabezado.Receptor.CmnaRecep))
                    {
                        retorno.Success = false;
                        retorno.Mensaje = string.Format("El DTE Nro {0} de Tipo {1} del proveedor {2} no contiene una Comuna de receptor válida", Folio, Tipo, Rut);
                        retorno.Content = "RECHAZAR";
                        return retorno;
                    }
                }
            }
            catch (Exception ex)
            {
                retorno.Success = false;
                retorno.Mensaje = ex.Message;
            }
            return retorno;
        }

        /// <summary>
        /// Integra registros marcados en la matrix correspondiente
        /// </summary>
        /// <param name="oMatrix"></param>
        /// <param name="dtDoc"></param>
        /// <param name="columna"></param>
        private static void IntegrarDocumentos(SAPbouiCOM.Matrix oMatrix, SAPbouiCOM.DataTable dtDoc, string columna)
        {
            Application.SBO_Application.StatusBar.SetText("Validando documentos. Espere unos momentos.", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);

            String Rut = String.Empty;
            String BaseType = String.Empty;
            Int32 Tipo = 0;
            Int64 Folio = 0;
            Int32 DocEntryUDO;
            String OCManual = String.Empty;

            try
            {
                Int32 regmarcados = CantidadRegistrosMarcados(oMatrix, dtDoc, columna);
                if (regmarcados.Equals(1))
                {
                    SAPbouiCOM.Columns oColumns;
                    oColumns = oMatrix.Columns;
                    oColumns.Item("co_obs").Visible = true;

                    int regMarcados = 0;
                    int regIntegrados = 0;
                    for (int i = 0; i < dtDoc.Rows.Count; i++)
                    {
                        DocEntryUDO = Convert.ToInt32(dtDoc.GetValue("DocEntry", i));
                        try
                        {
                            // sólo si check está marcado
                            if (dtDoc.GetValue(columna, i).ToString().Equals("Y"))
                            {
                                // Obtener datos necesarios de DataTable
                                string docId = dtDoc.GetValue("U_DOCID", i).ToString();
                                Rut = dtDoc.GetValue("U_RUTEMIS", i).ToString();
                                Tipo = Convert.ToInt16(dtDoc.GetValue("U_TIPODOC", i).ToString());
                                Folio = Convert.ToInt64(dtDoc.GetValue("U_FOLIO", i).ToString());
                                OCManual = dtDoc.GetValue("U_DOCBASE", i).ToString();

                                var validacion = ProcesaValidacionGeneral(docId, Rut, Tipo, Folio, OCManual);
                                if (validacion.Success)
                                {
                                    Application.SBO_Application.StatusBar.SetText(String.Format("El DTE Nro {0} de Tipo {1} del proveedor {2} se puede integrar. Validaciones OK", Folio, Tipo, Rut), SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);

                                    BaseType = String.Empty;
                                    if (validacion.TpoDocRef.Equals("801"))
                                    {
                                        BaseType = "22";
                                    }
                                    else if (validacion.TpoDocRef.Equals(""))
                                    {
                                        BaseType = "";
                                    }
                                    else
                                    {
                                        BaseType = "20";
                                    }

                                    // Integrar documento
                                    try
                                    {
                                        string TpoDocRef = validacion.TpoDocRef == "" ? null : validacion.TpoDocRef;
                                        string FolioRef = validacion.FolioRef == "" ? null : validacion.FolioRef;
                                        string DocEntryBase = validacion.DocEntryBase == "0" ? null : validacion.DocEntryBase;
                                        string DocTypeBase = validacion.DocTypeBase == "" ? null : validacion.DocTypeBase;
                                        BaseType = BaseType == "" ? null : BaseType;
                                        IntegrarDTE(validacion.objDTE, TpoDocRef, FolioRef, validacion.CardCode, DocEntryBase, DocTypeBase, BaseType, DocEntryUDO, docId);
                                        regIntegrados++;
                                        dtDoc.Rows.Remove(i);
                                        i--;
                                    }
                                    catch (Exception ex)
                                    {
                                        dtDoc.SetValue("U_OBS", i, ex.Message);
                                    }

                                }
                                else
                                {
                                    dtDoc.SetValue("U_OBS", i, validacion.Mensaje);
                                    Application.SBO_Application.StatusBar.SetText(validacion.Mensaje, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);
                                }
                                regMarcados++;
                            }
                            else
                            {
                                dtDoc.SetValue("U_OBS", i, string.Empty);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(string.Format("Reg: {0} - {1}", i, ex.Message));
                        }
                    }
                    ActualizarColumnaNumMatrix(oMatrix, dtDoc, "NUM");
                    Application.SBO_Application.StatusBar.SetText(string.Format("Integración completa. Se validaron {0} documentos. {1} documentos serán integrados.", regMarcados, regIntegrados), SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success);
                }
                else
                {
                    Application.SBO_Application.StatusBar.SetText(string.Format("Sólo puede seleccionar un documento a la vez."), SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Integra registros marcados en la matrix correspondiente
        /// </summary>
        /// <param name="oMatrix"></param>
        /// <param name="dtDoc"></param>
        /// <param name="columna"></param>
        private static void IntegrarDocumentosMX(SAPbouiCOM.Matrix oMatrix, SAPbouiCOM.DataTable dtDoc, string columna)
        {
            Application.SBO_Application.StatusBar.SetText("Validando documentos. Espere unos momentos.", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);

            String Rut = String.Empty;
            String BaseType = String.Empty;
            Int32 Tipo = 0;
            Int64 Folio = 0;
            Int32 DocEntryUDO;
            String OCManual = String.Empty;

            try
            {
                Int32 regmarcados = CantidadRegistrosMarcados(oMatrix, dtDoc, columna);
                if (regmarcados.Equals(1))
                {
                    SAPbouiCOM.Columns oColumns;
                    oColumns = oMatrix.Columns;
                    oColumns.Item("co_obs").Visible = true;

                    int regMarcados = 0;
                    int regIntegrados = 0;
                    for (int i = 0; i < dtDoc.Rows.Count; i++)
                    {
                        DocEntryUDO = Convert.ToInt32(dtDoc.GetValue("DocEntry", i));
                        try
                        {
                            // sólo si check está marcado
                            if (dtDoc.GetValue(columna, i).ToString().Equals("Y"))
                            {
                                // Obtener datos necesarios de DataTable
                                string docId = dtDoc.GetValue("U_DOCID", i).ToString();
                                Rut = dtDoc.GetValue("U_RUTEMIS", i).ToString();
                                Tipo = Convert.ToInt16(dtDoc.GetValue("U_TIPODOC", i).ToString());
                                Folio = Convert.ToInt64(dtDoc.GetValue("U_FOLIO", i).ToString());
                                OCManual = dtDoc.GetValue("U_DOCBASE", i).ToString();

                                var validacion = ProcesaValidacionMX(docId, Rut, Tipo, Folio, OCManual);
                                if (validacion.Success)
                                {
                                    Application.SBO_Application.StatusBar.SetText(String.Format("El DTE Nro {0} de Tipo {1} del proveedor {2} se puede integrar. Validaciones OK", Folio, Tipo, Rut), SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);

                                    BaseType = String.Empty;
                                    if (validacion.TpoDocRef.Equals("801"))
                                    {
                                        BaseType = "22";
                                    }
                                    else if (validacion.TpoDocRef.Equals(""))
                                    {
                                        BaseType = "";
                                    }
                                    else
                                    {
                                        BaseType = "20";
                                    }

                                    // Integrar documento
                                    try
                                    {
                                        string TpoDocRef = validacion.TpoDocRef == "" ? null : validacion.TpoDocRef;
                                        string FolioRef = validacion.FolioRef == "" ? null : validacion.FolioRef;
                                        string DocEntryBase = validacion.DocEntryBase == "0" ? null : validacion.DocEntryBase;
                                        string DocTypeBase = validacion.DocTypeBase == "" ? null : validacion.DocTypeBase;
                                        BaseType = BaseType == "" ? null : BaseType;
                                        IntegrarDTE(validacion.objDTE, TpoDocRef, FolioRef, validacion.CardCode, DocEntryBase, DocTypeBase, BaseType, DocEntryUDO, docId);
                                        regIntegrados++;
                                        dtDoc.Rows.Remove(i);
                                        i--;
                                    }
                                    catch (Exception ex)
                                    {
                                        dtDoc.SetValue("U_OBS", i, ex.Message);
                                    }

                                }
                                else
                                {
                                    dtDoc.SetValue("U_OBS", i, validacion.Mensaje);
                                    Application.SBO_Application.StatusBar.SetText(validacion.Mensaje, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);
                                }
                                regMarcados++;
                            }
                            else
                            {
                                dtDoc.SetValue("U_OBS", i, string.Empty);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(string.Format("Reg: {0} - {1}", i, ex.Message));
                        }
                    }
                    ActualizarColumnaNumMatrix(oMatrix, dtDoc, "NUM");
                    Application.SBO_Application.StatusBar.SetText(string.Format("Integración completa. Se validaron {0} documentos. {1} documentos serán integrados.", regMarcados, regIntegrados), SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success);
                }
                else
                {
                    Application.SBO_Application.StatusBar.SetText(string.Format("Sólo puede seleccionar un documento a la vez."), SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void IntegrarDocumentosSinOC(SAPbouiCOM.Matrix oMatrix, SAPbouiCOM.DataTable dtDoc, string columna)
        {
            Application.SBO_Application.StatusBar.SetText("Validando documentos. Espere unos momentos.", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);

            String Rut = String.Empty;
            String BaseType = String.Empty;
            Int32 Tipo = 0;
            Int64 Folio = 0;
            Int32 DocEntryUDO;
            String OCManual = String.Empty;

            try
            {
                Int32 regmarcados = CantidadRegistrosMarcados(oMatrix, dtDoc, columna);
                if (regmarcados.Equals(1))
                {
                    SAPbouiCOM.Columns oColumns;
                    oColumns = oMatrix.Columns;
                    oColumns.Item("co_obs").Visible = true;

                    int regMarcados = 0;
                    int regIntegrados = 0;
                    for (int i = 0; i < dtDoc.Rows.Count; i++)
                    {
                        DocEntryUDO = Convert.ToInt32(dtDoc.GetValue("DocEntry", i));
                        try
                        {
                            // sólo si check está marcado
                            if (dtDoc.GetValue(columna, i).ToString().Equals("Y"))
                            {
                                // Obtener datos necesarios de DataTable
                                string docId = dtDoc.GetValue("U_DOCID", i).ToString();
                                Rut = dtDoc.GetValue("U_RUTEMIS", i).ToString();
                                Tipo = Convert.ToInt16(dtDoc.GetValue("U_TIPODOC", i).ToString());
                                Folio = Convert.ToInt64(dtDoc.GetValue("U_FOLIO", i).ToString());
                                OCManual = dtDoc.GetValue("U_DOCBASE", i).ToString();

                                var validacion = ProcesaValidacionSinOC(docId, Rut, Tipo, Folio, OCManual);
                                if (validacion.Success)
                                {
                                    Application.SBO_Application.StatusBar.SetText(String.Format("El DTE Nro {0} de Tipo {1} del proveedor {2} se puede integrar. Validaciones OK", Folio, Tipo, Rut), SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);

                                    BaseType = String.Empty;
                                    if (validacion.TpoDocRef.Equals("801"))
                                    {
                                        BaseType = "22";
                                    }
                                    else if (validacion.TpoDocRef.Equals(""))
                                    {
                                        BaseType = "";
                                    }
                                    else
                                    {
                                        BaseType = "20";
                                    }

                                    // Integrar documento
                                    try
                                    {
                                        string TpoDocRef = validacion.TpoDocRef == "" ? null : validacion.TpoDocRef;
                                        string FolioRef = validacion.FolioRef == "" ? null : validacion.FolioRef;
                                        string DocEntryBase = validacion.DocEntryBase == "0" ? null : validacion.DocEntryBase;
                                        string DocTypeBase = validacion.DocTypeBase == "" ? null : validacion.DocTypeBase;
                                        BaseType = BaseType == "" ? null : BaseType;
                                        IntegrarDTE(validacion.objDTE, TpoDocRef, FolioRef, validacion.CardCode, DocEntryBase, DocTypeBase, BaseType, DocEntryUDO, docId);
                                        regIntegrados++;
                                        dtDoc.Rows.Remove(i);
                                        i--;
                                    }
                                    catch (Exception ex)
                                    {
                                        dtDoc.SetValue("U_OBS", i, ex.Message);
                                    }

                                }
                                else
                                {
                                    dtDoc.SetValue("U_OBS", i, validacion.Mensaje);
                                    Application.SBO_Application.StatusBar.SetText(validacion.Mensaje, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);
                                }
                                regMarcados++;
                            }
                            else
                            {
                                dtDoc.SetValue("U_OBS", i, string.Empty);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(string.Format("Reg: {0} - {1}", i, ex.Message));
                        }
                    }
                    ActualizarColumnaNumMatrix(oMatrix, dtDoc, "NUM");
                    Application.SBO_Application.StatusBar.SetText(string.Format("Integración completa. Se validaron {0} documentos. {1} documentos serán integrados.", regMarcados, regIntegrados), SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success);
                }
                else
                {
                    Application.SBO_Application.StatusBar.SetText(string.Format("Sólo puede seleccionar un documento a la vez."), SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Integra documentos en SBO
        /// </summary>
        /// <param name="objDTE"></param>
        /// <param name="TipoRef"></param>
        /// <param name="Folio"></param>
        /// <param name="CardCode"></param>
        /// <param name="DocEntryBase"></param>
        /// <param name="BaseType"></param>
        /// <param name="DocEntryUDO"></param>
        private static void IntegrarDTE(ClasesDTE.Documento objDTE, String TipoRef, String Folio, String CardCode, String DocEntryBase, String DocTypeBase, String BaseType, Int32 DocEntryUDO, string DocId)
        {
            try
            {
                Application.SBO_Application.Forms.Item("frmIntegra").Close();
            }
            catch
            {

            }
            Formularios.frmIntegrar activeForm = new Formularios.frmIntegrar(objDTE, TipoRef, Folio, CardCode, DocEntryBase, DocTypeBase, BaseType, DocEntryUDO, DocId);
            activeForm.Show();
        }

        /// <summary>
        /// Acepta registros marcados en la matrix correspondiente
        /// </summary>
        /// <param name="oMatrix"></param>
        /// <param name="dtDoc"></param>
        /// <param name="columna"></param>
        private static void AceptarDocumentos(SAPbouiCOM.Matrix oMatrix, SAPbouiCOM.DataTable dtDoc, string columna)
        {
            Application.SBO_Application.StatusBar.SetText("Aceptando documentos. Espere unos momentos.", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);

            SAPbobsCOM.GeneralService oGeneralService = null;
            SAPbobsCOM.GeneralData oGeneralData = null;
            SAPbobsCOM.GeneralData oChild = null;
            SAPbobsCOM.GeneralDataCollection oChildren = null;
            SAPbobsCOM.GeneralDataParams oGeneralParams = null;
            SAPbobsCOM.CompanyService oCompanyService = null;

            Boolean TieneReferencia = false;
            String TipoReferencia = String.Empty;
            String RutEmisor = String.Empty;
            String Uid = String.Empty;
            String Rut = String.Empty;
            String DecodeString = String.Empty;
            String CardCode = String.Empty;
            String DocEntryBase = String.Empty;
            String BaseType = String.Empty;
            Int32 Tipo = 0;
            Int32 Index;
            Int64 Folio = 0;
            String UsuarioIntegrador = String.Empty;
            String PasswordIntegrador = String.Empty;
            Int32 DocEntryUDO;
            String NuevoDocumento = string.Empty;

            try
            {
                SAPbouiCOM.Columns oColumns;
                oColumns = oMatrix.Columns;
                oColumns.Item("co_obs").Visible = true;

                int regMarcados = 0;
                int regAceptados = 0;
                int regAceptadosReparo = 0;
                for (int i = 0; i < dtDoc.Rows.Count; i++)
                {

                    DocEntryUDO = Convert.ToInt32(dtDoc.GetValue("DocEntry", i));
                    try
                    {
                        // sólo si check está marcado
                        if (dtDoc.GetValue(columna, i).ToString().Equals("Y"))
                        {
                            // Obtener datos necesarios de DataTable
                            string docId = dtDoc.GetValue("U_DOCID", i).ToString();
                            Rut = dtDoc.GetValue("U_RUTEMIS", i).ToString();
                            Tipo = Convert.ToInt16(dtDoc.GetValue("U_TIPODOC", i).ToString());
                            Folio = Convert.ToInt64(dtDoc.GetValue("U_FOLIO", i).ToString());
                            string motivo = dtDoc.GetValue("U_RAZONRE", i).ToString();

                            // Aceptar documento
                            try
                            {
                                ProveedorDTE proveedorDTE = new ProveedorDTE();

                                string[] parametros = new string[] { docId, Comun.Enumeradores.EstadosComercialesComunes.Aceptacion_Recibo_Mercaderias.ToString(), motivo };

                                var provResult = proveedorDTE.CambiarEstadoComercial(parametros);
                                //ProvDTE.Local.Message provResult = new ProvDTE.Local.Message();
                                //provResult.Success = true;
                                if (provResult.Success)
                                {
                                    // Cambiar estado a aceptado
                                    try
                                    {
                                        oCompanyService = SBO.ConexionSBO.oCompany.GetCompanyService();
                                        // Get GeneralService (oCmpSrv is the CompanyService)
                                        oGeneralService = oCompanyService.GetGeneralService("SO_MONITOR");
                                        // Create data for new row in main UDO
                                        oGeneralParams = ((SAPbobsCOM.GeneralDataParams)(oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)));
                                        oGeneralParams.SetProperty("DocEntry", DocEntryUDO);
                                        oGeneralData = oGeneralService.GetByParams(oGeneralParams);
                                        if (string.IsNullOrEmpty(motivo))
                                        {
                                            //oGeneralData.SetProperty("U_ESTSII", ((int)Comun.Enumeradores.EstadosSii.aceptado).ToString());
                                            oGeneralData.SetProperty("U_ESTSII", Comun.Enumeradores.GetEstadoSii(Comun.Enumeradores.EstadosSii.aceptado));
                                            oGeneralData.SetProperty("U_ESTADO", "2");
                                        }
                                        else
                                        {
                                            //oGeneralData.SetProperty("U_ESTSII", ((int)Comun.Enumeradores.EstadosSii.aceptadoreparo).ToString());
                                            oGeneralData.SetProperty("U_ESTSII", Comun.Enumeradores.GetEstadoSii(Comun.Enumeradores.EstadosSii.aceptadoreparo));
                                            oGeneralData.SetProperty("U_ESTADO", "3");
                                            oGeneralData.SetProperty("U_RAZONRE", motivo);
                                        }
                                        oGeneralService.Update(oGeneralData);
                                    }
                                    catch (Exception ex)
                                    {
                                        dtDoc.SetValue("U_OBS", i, String.Format("Aceptado OK. No actualizado: {0}.", ex.Message));
                                        Application.SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);
                                    }
                                    if (string.IsNullOrEmpty(motivo))
                                    {
                                        dtDoc.SetValue("U_OBS", i, String.Format("Aceptado OK"));
                                        regAceptados++;
                                    }
                                    else
                                    {
                                        dtDoc.SetValue("U_OBS", i, String.Format("Aceptado con reparos OK"));
                                        regAceptadosReparo++;
                                    }
                                    dtDoc.Rows.Remove(i);
                                    i--;
                                }
                                // Validación cambio de estado de documento
                                else
                                {
                                    Application.SBO_Application.StatusBar.SetText(provResult.Mensaje, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);
                                    dtDoc.SetValue("U_OBS", i, provResult.Mensaje);
                                }
                            }
                            catch (Exception ex)
                            {
                                dtDoc.SetValue("U_OBS", i, ex.Message);
                            }
                            regMarcados++;
                        }
                        else
                        {
                            dtDoc.SetValue("U_OBS", i, string.Empty);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(string.Format("Reg: {0} - {1}", i, ex.Message));
                    }
                }
                ActualizarColumnaNumMatrix(oMatrix, dtDoc, "NUM");
                Application.SBO_Application.StatusBar.SetText(string.Format("Aceptación completa. Se procesaron {0} documentos. {1} documentos aceptados. {2} documentos con reparos.", regMarcados, regAceptados, regAceptadosReparo), SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Rechaza registros marcados en la matrix correspondiente
        /// </summary>
        /// <param name="oMatrix"></param>
        /// <param name="dtDoc"></param>
        /// <param name="columna"></param>
        private static void RechazarDocumentos(SAPbouiCOM.Matrix oMatrix, SAPbouiCOM.DataTable dtDoc, string columna)
        {
            Application.SBO_Application.StatusBar.SetText("Rechazando documentos. Espere unos momentos.", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);

            SAPbobsCOM.GeneralService oGeneralService = null;
            SAPbobsCOM.GeneralData oGeneralData = null;
            SAPbobsCOM.GeneralData oChild = null;
            SAPbobsCOM.GeneralDataCollection oChildren = null;
            SAPbobsCOM.GeneralDataParams oGeneralParams = null;
            SAPbobsCOM.CompanyService oCompanyService = null;

            Boolean TieneReferencia = false;
            String TipoReferencia = String.Empty;
            String RutEmisor = String.Empty;
            String Uid = String.Empty;
            String Rut = String.Empty;
            String DecodeString = String.Empty;
            String CardCode = String.Empty;
            String DocEntryBase = String.Empty;
            String BaseType = String.Empty;
            Int32 Tipo = 0;
            Int32 Index;
            Int64 Folio = 0;
            String UsuarioIntegrador = String.Empty;
            String PasswordIntegrador = String.Empty;
            Int32 DocEntryUDO;
            String NuevoDocumento = string.Empty;

            try
            {
                SAPbouiCOM.Columns oColumns;
                oColumns = oMatrix.Columns;
                oColumns.Item("co_obs").Visible = true;

                int regMarcados = 0;
                int regRechazados = 0;
                for (int i = 0; i < dtDoc.Rows.Count; i++)
                {

                    DocEntryUDO = Convert.ToInt32(dtDoc.GetValue("DocEntry", i));
                    try
                    {
                        // sólo si check está marcado
                        if (dtDoc.GetValue(columna, i).ToString().Equals("Y"))
                        {
                            // Obtener datos necesarios de DataTable
                            string docId = dtDoc.GetValue("U_DOCID", i).ToString();
                            Rut = dtDoc.GetValue("U_RUTEMIS", i).ToString();
                            Tipo = Convert.ToInt16(dtDoc.GetValue("U_TIPODOC", i).ToString());
                            Folio = Convert.ToInt64(dtDoc.GetValue("U_FOLIO", i).ToString());
                            string motivo = dtDoc.GetValue("U_RAZONRE", i).ToString();
                            string formapago = dtDoc.GetValue("U_FORMAPA", i).ToString();

                            // Validar motivo de rechazo
                            if (!string.IsNullOrEmpty(motivo))
                            {
                                // Validar que forma de pago sea crédito
                                if (formapago.Equals("2"))
                                {
                                    // Rechazar documento
                                    try
                                    {
                                        ProveedorDTE proveedorDTE = new ProveedorDTE();

                                        string[] parametros = new string[] { docId, Comun.Enumeradores.EstadosComercialesComunes.Rechazo_Comercial.ToString(), motivo };

                                        //var provResult = proveedorDTE.CambiarEstadoComercial(parametros);
                                        ProvDTE.Local.Message provResult = new ProvDTE.Local.Message();
                                        provResult.Success = true;
                                        if (provResult.Success)
                                        {
                                            // Cambiar estado a rechazado
                                            try
                                            {
                                                oCompanyService = SBO.ConexionSBO.oCompany.GetCompanyService();
                                                // Get GeneralService (oCmpSrv is the CompanyService)
                                                oGeneralService = oCompanyService.GetGeneralService("SO_MONITOR");
                                                // Create data for new row in main UDO
                                                oGeneralParams = ((SAPbobsCOM.GeneralDataParams)(oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)));
                                                oGeneralParams.SetProperty("DocEntry", DocEntryUDO);
                                                oGeneralData = oGeneralService.GetByParams(oGeneralParams);
                                                //oGeneralData.SetProperty("U_ESTSII", ((int)Comun.Enumeradores.EstadosSii.rechazado).ToString());
                                                oGeneralData.SetProperty("U_ESTSII", Comun.Enumeradores.GetEstadoSii(Comun.Enumeradores.EstadosSii.rechazado));
                                                oGeneralData.SetProperty("U_RAZONRE", motivo);
                                                oGeneralData.SetProperty("U_ESTADO", "4");
                                                oGeneralService.Update(oGeneralData);

                                            }
                                            catch (Exception ex)
                                            {
                                                dtDoc.SetValue("U_OBS", i, String.Format("Rechazado OK. No actualizado: {0}.", ex.Message));
                                                Application.SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);
                                            }
                                            dtDoc.SetValue("U_OBS", i, String.Format("Rechazado OK"));
                                            regRechazados++;
                                            dtDoc.Rows.Remove(i);
                                            i--;
                                        }
                                        // Validación cambio de estado de documento
                                        else
                                        {
                                            Application.SBO_Application.StatusBar.SetText(provResult.Mensaje, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);
                                            dtDoc.SetValue("U_OBS", i, provResult.Mensaje);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        dtDoc.SetValue("U_OBS", i, ex.Message);
                                    }
                                }
                                // Validación forma de pago no sea contado
                                else
                                {
                                    Application.SBO_Application.StatusBar.SetText(string.Format("El DTE Nro {0} de Tipo {1} del proveedor {2} no se puede rechazar por que tiene forma de pago contado.", Folio, Tipo, Rut), SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);
                                    dtDoc.SetValue("U_OBS", i, "El DTE tiene forma de pago distinta de crédito");
                                }
                            }
                            // Validación motivo de rechazo
                            else
                            {
                                Application.SBO_Application.StatusBar.SetText(string.Format("El DTE Nro {0} de Tipo {1} del proveedor {2} no tiene Razón de rechazo.", Folio, Tipo, Rut), SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);
                                dtDoc.SetValue("U_OBS", i, "El DTE no tiene Razón de rechazo");
                            }
                             regMarcados++;
                        }
                        else
                        {
                            dtDoc.SetValue("U_OBS", i, string.Empty);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(string.Format("Reg: {0} - {1}", i, ex.Message));
                    }
                }
                ActualizarColumnaNumMatrix(oMatrix, dtDoc, "NUM");
                Application.SBO_Application.StatusBar.SetText(string.Format("Rechazo completo. Se procesaron {0} documentos. {1} documentos rechazados.", regMarcados, regRechazados), SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
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
                string docId = dtDoc.GetValue("U_DOCID", Row).ToString();

                ProveedorDTE proveedorDTE = new ProveedorDTE();

                string[] parametros = new string[] { docId };

                var provResult = proveedorDTE.ObtenerPDFDocumento(parametros);
                if (provResult.Success)
                {
                    var _Datos = proveedorDTE.DetalleDocuDTE;
                    String link = _Datos.ImagenLink.ToString();
                    SAPbobsCOM.CompanyService com_service = SBO.ConexionSBO.oCompany.GetCompanyService();
                    SAPbobsCOM.PathAdmin oPathAdmin = com_service.GetPathAdmin();
                    String Path = string.Empty;
                    Path = oPathAdmin.AttachmentsFolderPath;
                    link = link.Replace("AttachmentsFolderPath:/", Path);

                    // Visualizar con chrome
                    //System.Diagnostics.Process.Start("chrome.exe", link);

                    // Visualizar con navegador por defecto
                    System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo();
                    info.FileName = link;
                    info.Verb = "open";
                    info.WindowStyle = System.Diagnostics.ProcessWindowStyle.Maximized;
                    System.Diagnostics.Process.Start(info);
                }
                // Validación obtención de documento
                else
                {
                    Application.SBO_Application.StatusBar.SetText(provResult.Mensaje, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static bool PermiteOpcionesMX()
        {
            var usuario = SBO.ConexionSBO.oCompany.UserName;
            var esusuariomx = SBO.ConsultasSBO.EsUsuarioMX(usuario);
            if (esusuariomx)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static bool PermiteOpcionesSOC()
        {
            var usuario = SBO.ConexionSBO.oCompany.UserName;
            var esusuariomx = SBO.ConsultasSBO.EsUsuarioSOC(usuario);
            if (esusuariomx)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
