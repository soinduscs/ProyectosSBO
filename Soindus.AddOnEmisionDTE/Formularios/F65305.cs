using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM.Framework;

namespace Soindus.AddOnEmisionDTE.Formularios
{
    [FormAttribute("65305")]
    public class F65305 : SystemFormBase
    {
        public static SAPbouiCOM.Form oForm;
        private static Local.Configuracion ExtConf = new Local.Configuracion();

        public F65305()
        {
            ExtConf = new Local.Configuracion();
        }

        public override void OnInitializeComponent()
        {
            //base.OnInitializeComponent();
        }

        public override void OnInitializeFormEvents()
        {
            //base.OnInitializeFormEvents();
        }

        public static void Form_DataEvent(ref SAPbouiCOM.BusinessObjectInfo BusinessObjectInfo, out bool BubbleEvent)
        {
            BubbleEvent = true;
            if (ExtConf.Parametros.Proveedor_FE.Equals("DBN-EC"))
            {
                return;
            }
            try
            {
                if (BusinessObjectInfo.BeforeAction == false)
                {
                    if (BusinessObjectInfo.ActionSuccess == true)
                    {
                        if (BusinessObjectInfo.EventType == SAPbouiCOM.BoEventTypes.et_FORM_DATA_ADD)
                        {
                            EnvioDTE("41", "BN");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Application.SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
            }
        }

        public static void Form_ItemEvent(string FormUID, ref SAPbouiCOM.ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            if (ExtConf.Parametros.Proveedor_FE.Equals("DBN-EC"))
            {
                return;
            }

            if (pVal.EventType != SAPbouiCOM.BoEventTypes.et_FORM_UNLOAD && pVal.Before_Action == true)
            {
                oForm = Application.SBO_Application.Forms.GetFormByTypeAndCount(pVal.FormType, pVal.FormTypeCount);

                if (pVal.EventType == SAPbouiCOM.BoEventTypes.et_FORM_LOAD && pVal.Before_Action == true)
                {
                    SAPbouiCOM.Item oRefItem;
                    SAPbouiCOM.Item oNewItem;

                    // add a new button item to the form
                    oRefItem = (SAPbouiCOM.Item)oForm.Items.Item("70");
                    SAPbouiCOM.Button oButton;
                    oNewItem = oForm.Items.Add("btnPDF", SAPbouiCOM.BoFormItemTypes.it_BUTTON);

                    oNewItem.Top = oRefItem.Top + 25;
                    oNewItem.Height = 18;
                    oNewItem.Width = 80;
                    oNewItem.Left = oRefItem.Left;

                    oButton = ((SAPbouiCOM.Button)(oNewItem.Specific));
                    oButton.Caption = "Obtener PDF";

                    // add a new button item to the form
                    oRefItem = oNewItem;
                    oNewItem = oForm.Items.Add("btnDTE", SAPbouiCOM.BoFormItemTypes.it_BUTTON);

                    oNewItem.Top = oRefItem.Top + 25;
                    oNewItem.Height = oRefItem.Height;
                    oNewItem.Width = oRefItem.Width;
                    oNewItem.Left = oRefItem.Left;

                    oButton = ((SAPbouiCOM.Button)(oNewItem.Specific));
                    oButton.Caption = "Enviar DTE";
                }

                if (pVal.EventType == SAPbouiCOM.BoEventTypes.et_CLICK && pVal.ItemUID.Equals("btnPDF") && pVal.Before_Action == true)
                {
                    if (pVal.FormMode == (int)SAPbouiCOM.BoFormMode.fm_OK_MODE)
                    {
                        SAPbouiCOM.EditText oItem;
                        oItem = (SAPbouiCOM.EditText)oForm.Items.Item("211").Specific;
                        if (!string.IsNullOrEmpty(oItem.Value))
                        {
                            ObtenerPDF(oItem.Value, "41", "BN", "false");
                        }
                        else
                        {
                            Application.SBO_Application.MessageBox(string.Format("Documento no registra un número de folio"));
                        }
                    }
                }

                if (pVal.EventType == SAPbouiCOM.BoEventTypes.et_CLICK && pVal.ItemUID.Equals("btnDTE") && pVal.Before_Action == true)
                {
                    if (pVal.FormMode == (int)SAPbouiCOM.BoFormMode.fm_OK_MODE)
                    {
                        SAPbouiCOM.EditText oItem;
                        oItem = (SAPbouiCOM.EditText)oForm.Items.Item("211").Specific;
                        if (string.IsNullOrEmpty(oItem.Value))
                        {
                            EnvioDTE("41", "BN");
                        }
                        else
                        {
                            Application.SBO_Application.MessageBox(string.Format("Documento ya registra un número de folio: {0}", oItem.Value));
                        }
                    }
                }
            }
        }

        public static void EnvioDTE(string tipodoc, string folioprefix)
        {
            SAPbobsCOM.Documents oDoc = null;
            oDoc = (SAPbobsCOM.Documents)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices);
            oDoc.Browser.GetByKeys(oForm.BusinessObject.Key);

            if (!string.IsNullOrEmpty(oDoc.FolioNumber.ToString()) && !oDoc.FolioNumber.Equals(0))
            {
                Application.SBO_Application.MessageBox(string.Format("Documento ya registra un número de folio: {0}. Actualice el formulario.", oDoc.FolioNumber));
                return;
            }
            Application.SBO_Application.StatusBar.SetText("Enviando DTE...", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);

            string folio = string.Empty;

            string DocNum = oDoc.DocNum.ToString();
            string DocSubType = string.Empty;
            switch (oDoc.DocumentSubType)
            {
                case SAPbobsCOM.BoDocumentSubType.bod_None:
                    DocSubType = "--";
                    break;
                case SAPbobsCOM.BoDocumentSubType.bod_InvoiceExempt:
                    DocSubType = "IE";
                    break;
                case SAPbobsCOM.BoDocumentSubType.bod_Bill:
                    DocSubType = "IB";
                    break;
                case SAPbobsCOM.BoDocumentSubType.bod_ExemptBill:
                    DocSubType = "EB";
                    break;
                case SAPbobsCOM.BoDocumentSubType.bod_ExportInvoice:
                    DocSubType = "IX";
                    break;
                default:
                    break;
            }

            ProveedorDTE proveedorDTE = new ProveedorDTE();
            string[] parametros = new string[] { DocNum, tipodoc, DocSubType };
            var result = proveedorDTE.CrearDocumento(parametros);
            if (result.Success)
            {
                var resp = proveedorDTE.Respuesta;

                // Guardar Folio
                int ErrCode = 0;
                string ErrMsj = string.Empty;
                oDoc.FolioPrefixString = folioprefix;
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
                    linkEV += string.Format("?Sociedad={0}&Tipo={1}&Folio={2}", ExtConf.Parametros.Rut_Emisor.Replace(".", ""), tipodoc, resp.Folio);
                    oDoc.UserFields.Fields.Item("U_URL").Value = linkEV;
                }

                //int Ret = oDoc.Update();
                int Ret = 0;
                int Intento = 0;
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
                if (Ret != 0)
                {
                    SBO.ConexionSBO.oCompany.GetLastError(out ErrCode, out ErrMsj);
                    Application.SBO_Application.StatusBar.SetText(string.Format("No se logró actualizar el folio del documento: {0} - {1}", ErrCode, ErrMsj), SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
                }
                else
                {
                    folio = resp.Folio;
                    Application.SBO_Application.StatusBar.SetText(string.Format("Folio {0}-{1} actualizado correctamente...", folioprefix, resp.Folio), SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success);
                }
            }
            else
            {
                Application.SBO_Application.StatusBar.SetText(string.Format("Error al crear DTE: {0}", result.Mensaje), SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
            }

            if (!string.IsNullOrEmpty(folio))
            {
                if (!ExtConf.Parametros.Proveedor_FE.Equals("DBN-EV"))
                {
                    ObtenerPDF(folio, tipodoc, folioprefix, "false");
                }
            }
        }

        public static void ObtenerPDF(string folio, string tipodoc, string folioprefix, string cedible)
        {
            Application.SBO_Application.StatusBar.SetText("Obteniendo PDF...", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);

            SAPbobsCOM.Documents oDoc = null;
            oDoc = (SAPbobsCOM.Documents)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices);
            oDoc.Browser.GetByKeys(oForm.BusinessObject.Key);

            ProveedorDTE proveedorDTE = new ProveedorDTE();
            string[] parametros = new string[] { "", folio, tipodoc, "", "", cedible };
            var result = proveedorDTE.ObtenerPDF(parametros);
            if (result.Success)
            {
                var respPDF = proveedorDTE.RespuestaPDF;

                //Guardar PDF
                int ErrCode = 0;
                string ErrMsj = string.Empty;

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
                if (RetAtt != 0)
                {
                    SBO.ConexionSBO.oCompany.GetLastError(out ErrCode, out ErrMsj);
                    Application.SBO_Application.StatusBar.SetText(string.Format("No se logró adjuntar PDF del documento: {0} - {1}", ErrCode, ErrMsj), SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
                }
                else
                {
                    oAtt.GetByKey(int.Parse(SBO.ConexionSBO.oCompany.GetNewObjectKey()));
                    oDoc.AttachmentEntry = oAtt.AbsoluteEntry;
                }

                int RetVal = oDoc.Update();
                if (RetVal != 0)
                {
                    SBO.ConexionSBO.oCompany.GetLastError(out ErrCode, out ErrMsj);
                    Application.SBO_Application.StatusBar.SetText(string.Format("No se logró actualizar el documento: {0} - {1}", ErrCode, ErrMsj), SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
                }
                else
                {
                    Application.SBO_Application.StatusBar.SetText(string.Format("Documento {0}-{1} actualizado correctamente...", folioprefix, folio), SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success);
                }
            }
            else
            {
                Application.SBO_Application.StatusBar.SetText(string.Format("Error al recuperar PDF: {0}", result.Mensaje), SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
            }
        }
    }
}
