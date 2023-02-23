using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace Soindus.AddOnFactoringCob.SBO
{
    public class ModeloSBO
    {
        private static string SepDecimal;
        private static string SepMiles;

        public ModeloSBO()
        {

        }

        public static Comun.Message AddFactoring(Clases.Factoring Factoring)
        {
            Comun.Message resp = new Comun.Message();

            SAPbobsCOM.GeneralService oGeneralService = null;
            SAPbobsCOM.GeneralData oGeneralData = null;
            SAPbobsCOM.GeneralData oChild = null;
            SAPbobsCOM.GeneralDataCollection oChildren = null;
            SAPbobsCOM.GeneralDataParams oGeneralParams = null;
            SAPbobsCOM.CompanyService oCompanyService = null;

            try
            {
                oCompanyService = SBO.ConexionSBO.oCompany.GetCompanyService();
                var item = Factoring;
                oGeneralService = oCompanyService.GetGeneralService("SO_FACTORING");
                oGeneralData = ((SAPbobsCOM.GeneralData)(oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralData)));
                oGeneralData.SetProperty("U_ID", Convert.ToInt32(item.U_ID));
                oGeneralData.SetProperty("U_ENTIDAD", string.IsNullOrEmpty(item.U_ENTIDAD) ? "" : item.U_ENTIDAD);
                oGeneralData.SetProperty("U_MONEDA", string.IsNullOrEmpty(item.U_MONEDA) ? "" : item.U_MONEDA);
                oGeneralData.SetProperty("U_FECHA", new DateTime(item.U_FECHA.Year, item.U_FECHA.Month, item.U_FECHA.Day));
                oGeneralData.SetProperty("U_NUMOPER", string.IsNullOrEmpty(item.U_NUMOPER) ? "" : item.U_NUMOPER);
                oGeneralData.SetProperty("U_VALOR", item.U_VALOR);
                oGeneralData.SetProperty("U_TIPORES", string.IsNullOrEmpty(item.U_TIPORES) ? "" : item.U_TIPORES);
                oGeneralData.SetProperty("U_ESTADO", string.IsNullOrEmpty(item.U_ESTADO) ? "" : item.U_ESTADO);
                oChildren = oGeneralData.Child("SO_FCTRNGL");
                foreach (var child in item.Documentos)
                {
                    oChild = oChildren.Add();
                    oChild.SetProperty("U_OBJTYPE", string.IsNullOrEmpty(child.U_OBJTYPE) ? "" : child.U_OBJTYPE);
                    oChild.SetProperty("U_DOCENTRY", Convert.ToInt32(child.U_DOCENTRY));
                    oChild.SetProperty("U_BASEENTRY", Convert.ToInt32(child.U_BASEENTRY));
                    oChild.SetProperty("U_BASEREF", string.IsNullOrEmpty(child.U_BASEREF) ? "" : child.U_BASEREF);
                    oChild.SetProperty("U_TIPODOC", string.IsNullOrEmpty(child.U_TIPODOC) ? "" : child.U_TIPODOC);
                    oChild.SetProperty("U_DOCNUM", Convert.ToInt32(child.U_DOCNUM));
                    oChild.SetProperty("U_FOLIONUM", Convert.ToInt32(child.U_FOLIONUM));
                    oChild.SetProperty("U_ISINS", string.IsNullOrEmpty(child.U_ISINS) ? "" : child.U_ISINS);
                    oChild.SetProperty("U_INDICATOR", string.IsNullOrEmpty(child.U_INDICATOR) ? "" : child.U_INDICATOR);
                    oChild.SetProperty("U_DOCDATE", new DateTime(child.U_DOCDATE.Year, child.U_DOCDATE.Month, child.U_DOCDATE.Day));
                    oChild.SetProperty("U_DOCDUEDATE", new DateTime(child.U_DOCDUEDATE.Year, child.U_DOCDUEDATE.Month, child.U_DOCDUEDATE.Day));
                    oChild.SetProperty("U_TAXDATE", new DateTime(child.U_TAXDATE.Year, child.U_TAXDATE.Month, child.U_TAXDATE.Day));
                    oChild.SetProperty("U_DOCCUR", string.IsNullOrEmpty(child.U_DOCCUR) ? "" : child.U_DOCCUR);
                    oChild.SetProperty("U_DOCTOTAL", child.U_DOCTOTAL);
                    oChild.SetProperty("U_DOCTOTALSY", child.U_DOCTOTALSY);
                    oChild.SetProperty("U_LICTRADNUM", string.IsNullOrEmpty(child.U_LICTRADNUM) ? "" : child.U_LICTRADNUM);
                    oChild.SetProperty("U_CARDCODE", string.IsNullOrEmpty(child.U_CARDCODE) ? "" : child.U_CARDCODE);
                    oChild.SetProperty("U_CARDNAME", string.IsNullOrEmpty(child.U_CARDNAME) ? "" : child.U_CARDNAME);
                }
                oGeneralParams = oGeneralService.Add(oGeneralData);
                var DocEntry = oGeneralParams.GetProperty("DocEntry");
                oGeneralParams.SetProperty("DocEntry", (int)DocEntry);
                oGeneralData = oGeneralService.GetByParams(oGeneralParams);
                oGeneralData.SetProperty("U_ID", (int)DocEntry);
                oGeneralService.Update(oGeneralData);
                oChild = null;
                oChildren = null;
                oGeneralData = null;
                resp.Success = true;
                resp.DocEntry = (int)DocEntry;
            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Mensaje = ex.Message;
            }
            return resp;
        }

        public static Comun.Message UpdateFactoring(int DocEntry, Clases.Factoring Factoring)
        {
            Comun.Message resp = new Comun.Message();

            SAPbobsCOM.GeneralService oGeneralService = null;
            SAPbobsCOM.GeneralData oGeneralData = null;
            SAPbobsCOM.GeneralData oChild = null;
            SAPbobsCOM.GeneralDataCollection oChildren = null;
            SAPbobsCOM.GeneralDataParams oGeneralParams = null;
            SAPbobsCOM.CompanyService oCompanyService = null;

            try
            {
                oCompanyService = SBO.ConexionSBO.oCompany.GetCompanyService();
                var item = Factoring;
                oGeneralService = oCompanyService.GetGeneralService("SO_FACTORING");
                oGeneralParams = ((SAPbobsCOM.GeneralDataParams)(oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)));
                oGeneralParams.SetProperty("DocEntry", DocEntry);
                oGeneralData = oGeneralService.GetByParams(oGeneralParams);
                oGeneralData.SetProperty("U_ID", Convert.ToInt32(item.U_ID));
                oGeneralData.SetProperty("U_ENTIDAD", string.IsNullOrEmpty(item.U_ENTIDAD) ? "" : item.U_ENTIDAD);
                oGeneralData.SetProperty("U_MONEDA", string.IsNullOrEmpty(item.U_MONEDA) ? "" : item.U_MONEDA);
                oGeneralData.SetProperty("U_FECHA", new DateTime(item.U_FECHA.Year, item.U_FECHA.Month, item.U_FECHA.Day));
                oGeneralData.SetProperty("U_NUMOPER", string.IsNullOrEmpty(item.U_NUMOPER) ? "" : item.U_NUMOPER);
                oGeneralData.SetProperty("U_VALOR", item.U_VALOR);
                oGeneralData.SetProperty("U_TIPORES", string.IsNullOrEmpty(item.U_TIPORES) ? "" : item.U_TIPORES);
                oGeneralData.SetProperty("U_ESTADO", string.IsNullOrEmpty(item.U_ESTADO) ? "" : item.U_ESTADO);
                SAPbobsCOM.Recordset oRecord = (SAPbobsCOM.Recordset)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                oChildren = oGeneralData.Child("SO_FCTRNGL");
                int totlin = oChildren.Count;
                for (int i = 0; i < totlin; i++)
                {
                    oChildren.Remove(0);
                    //oGeneralService.Update(oGeneralData);
                }
                oChildren = oGeneralData.Child("SO_FCTRNGL");
                foreach (var child in item.Documentos)
                {
                    oChild = oChildren.Add();
                    oChild.SetProperty("U_OBJTYPE", string.IsNullOrEmpty(child.U_OBJTYPE) ? "" : child.U_OBJTYPE);
                    oChild.SetProperty("U_DOCENTRY", Convert.ToInt32(child.U_DOCENTRY));
                    oChild.SetProperty("U_BASEENTRY", Convert.ToInt32(child.U_BASEENTRY));
                    oChild.SetProperty("U_BASEREF", string.IsNullOrEmpty(child.U_BASEREF) ? "" : child.U_BASEREF);
                    oChild.SetProperty("U_TIPODOC", string.IsNullOrEmpty(child.U_TIPODOC) ? "" : child.U_TIPODOC);
                    oChild.SetProperty("U_DOCNUM", Convert.ToInt32(child.U_DOCNUM));
                    oChild.SetProperty("U_FOLIONUM", Convert.ToInt32(child.U_FOLIONUM));
                    oChild.SetProperty("U_ISINS", string.IsNullOrEmpty(child.U_ISINS) ? "" : child.U_ISINS);
                    oChild.SetProperty("U_INDICATOR", string.IsNullOrEmpty(child.U_INDICATOR) ? "" : child.U_INDICATOR);
                    oChild.SetProperty("U_DOCDATE", new DateTime(child.U_DOCDATE.Year, child.U_DOCDATE.Month, child.U_DOCDATE.Day));
                    oChild.SetProperty("U_DOCDUEDATE", new DateTime(child.U_DOCDUEDATE.Year, child.U_DOCDUEDATE.Month, child.U_DOCDUEDATE.Day));
                    oChild.SetProperty("U_TAXDATE", new DateTime(child.U_TAXDATE.Year, child.U_TAXDATE.Month, child.U_TAXDATE.Day));
                    oChild.SetProperty("U_DOCCUR", string.IsNullOrEmpty(child.U_DOCCUR) ? "" : child.U_DOCCUR);
                    oChild.SetProperty("U_DOCTOTAL", child.U_DOCTOTAL);
                    oChild.SetProperty("U_DOCTOTALSY", child.U_DOCTOTALSY);
                    oChild.SetProperty("U_LICTRADNUM", string.IsNullOrEmpty(child.U_LICTRADNUM) ? "" : child.U_LICTRADNUM);
                    oChild.SetProperty("U_CARDCODE", string.IsNullOrEmpty(child.U_CARDCODE) ? "" : child.U_CARDCODE);
                    oChild.SetProperty("U_CARDNAME", string.IsNullOrEmpty(child.U_CARDNAME) ? "" : child.U_CARDNAME);
                }
                oGeneralService.Update(oGeneralData);
                oChild = null;
                oChildren = null;
                oGeneralData = null;
                resp.Success = true;
            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Mensaje = ex.Message;
            }
            return resp;
        }

        public static Comun.Message UpdateFactoringStatus(int DocEntry, string Estado)
        {
            Comun.Message resp = new Comun.Message();

            SAPbobsCOM.GeneralService oGeneralService = null;
            SAPbobsCOM.GeneralData oGeneralData = null;
            SAPbobsCOM.GeneralData oChild = null;
            SAPbobsCOM.GeneralDataCollection oChildren = null;
            SAPbobsCOM.GeneralDataParams oGeneralParams = null;
            SAPbobsCOM.CompanyService oCompanyService = null;

            try
            {
                oCompanyService = SBO.ConexionSBO.oCompany.GetCompanyService();
                oGeneralService = oCompanyService.GetGeneralService("SO_FACTORING");
                oGeneralParams = ((SAPbobsCOM.GeneralDataParams)(oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)));
                oGeneralParams.SetProperty("DocEntry", DocEntry);
                oGeneralData = oGeneralService.GetByParams(oGeneralParams);
                oGeneralData.SetProperty("U_ESTADO", Estado);
                oGeneralService.Update(oGeneralData);
                resp.Success = true;
            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Mensaje = ex.Message;
            }
            return resp;
        }

        public static Comun.Message DeleteFactoring(int DocEntry)
        {
            Comun.Message resp = new Comun.Message();

            SAPbobsCOM.GeneralService oGeneralService = null;
            SAPbobsCOM.GeneralData oGeneralData = null;
            SAPbobsCOM.GeneralData oChild = null;
            SAPbobsCOM.GeneralDataCollection oChildren = null;
            SAPbobsCOM.GeneralDataParams oGeneralParams = null;
            SAPbobsCOM.CompanyService oCompanyService = null;
            try
            {
                oCompanyService = SBO.ConexionSBO.oCompany.GetCompanyService();
                oGeneralService = oCompanyService.GetGeneralService("SO_FACTORING");
                oGeneralParams = ((SAPbobsCOM.GeneralDataParams)(oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)));
                oGeneralParams.SetProperty("DocEntry", DocEntry);
                //oGeneralData = oGeneralService.GetByParams(oGeneralParams);
                oGeneralService.Delete(oGeneralParams);
                resp.Success = true;
            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Mensaje = ex.Message;
            }
            return resp;
        }

        public static Comun.Message GetFactoring(int DocEntry)
        {
            NumberFormatInfo provider = new NumberFormatInfo();
            provider.NumberDecimalSeparator = SBO.ConsultasSBO.ObtenerSeparadorDecimal();
            provider.NumberGroupSeparator = SBO.ConsultasSBO.ObtenerSeparadorMiles();

            Comun.Message resp = new Comun.Message();

            SAPbobsCOM.GeneralService oGeneralService = null;
            SAPbobsCOM.GeneralData oGeneralData = null;
            SAPbobsCOM.GeneralData oChild = null;
            SAPbobsCOM.GeneralDataCollection oChildren = null;
            SAPbobsCOM.GeneralDataParams oGeneralParams = null;
            SAPbobsCOM.CompanyService oCompanyService = null;
            try
            {
                oCompanyService = SBO.ConexionSBO.oCompany.GetCompanyService();
                oGeneralService = oCompanyService.GetGeneralService("SO_FACTORING");
                oGeneralParams = ((SAPbobsCOM.GeneralDataParams)(oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)));
                oGeneralParams.SetProperty("DocEntry", DocEntry);
                oGeneralData = oGeneralService.GetByParams(oGeneralParams);

                Clases.Factoring factoring = new Clases.Factoring();
                factoring.DocEntry = (int)oGeneralData.GetProperty("DocEntry");
                factoring.U_ID = (int)oGeneralData.GetProperty("U_ID");
                factoring.U_ENTIDAD = oGeneralData.GetProperty("U_ENTIDAD").ToString();
                factoring.U_MONEDA = oGeneralData.GetProperty("U_MONEDA").ToString();
                factoring.U_FECHA = (DateTime)oGeneralData.GetProperty("U_FECHA");
                factoring.U_NUMOPER = oGeneralData.GetProperty("U_NUMOPER").ToString();
                factoring.U_VALOR = (double)oGeneralData.GetProperty("U_VALOR");
                factoring.U_TIPORES = oGeneralData.GetProperty("U_TIPORES").ToString();
                factoring.U_ESTADO = oGeneralData.GetProperty("U_ESTADO").ToString();
                oChildren = oGeneralData.Child("SO_FCTRNGL");
                for (int i = 0; i < oChildren.Count; i++)
                {
                    oChild = oChildren.Item(i);
                    Clases.FactoringLines documento = new Clases.FactoringLines();
                    documento.DocEntry = (int)oChild.GetProperty("DocEntry");
                    documento.LineId = (int)oChild.GetProperty("LineId");
                    documento.U_OBJTYPE = oChild.GetProperty("U_OBJTYPE").ToString();
                    documento.U_DOCENTRY = (int)oChild.GetProperty("U_DOCENTRY");
                    documento.U_BASEENTRY = (int)oChild.GetProperty("U_BASEENTRY");
                    documento.U_BASEREF = oChild.GetProperty("U_BASEREF").ToString();
                    documento.U_TIPODOC = oChild.GetProperty("U_TIPODOC").ToString();
                    documento.U_DOCNUM = (int)oChild.GetProperty("U_DOCNUM");
                    documento.U_FOLIONUM = (int)oChild.GetProperty("U_FOLIONUM");
                    documento.U_ISINS = oChild.GetProperty("U_ISINS").ToString();
                    documento.U_INDICATOR = oChild.GetProperty("U_INDICATOR").ToString();
                    documento.U_DOCDATE = (DateTime)oChild.GetProperty("U_DOCDATE");
                    documento.U_DOCDUEDATE = (DateTime)oChild.GetProperty("U_DOCDUEDATE");
                    documento.U_TAXDATE = (DateTime)oChild.GetProperty("U_TAXDATE");
                    documento.U_DOCCUR = oChild.GetProperty("U_DOCCUR").ToString();
                    documento.U_DOCTOTAL = (double)oChild.GetProperty("U_DOCTOTAL");
                    documento.U_DOCTOTALSY = (double)oChild.GetProperty("U_DOCTOTALSY");
                    documento.U_LICTRADNUM = oChild.GetProperty("U_LICTRADNUM").ToString();
                    documento.U_CARDCODE = oChild.GetProperty("U_CARDCODE").ToString();
                    documento.U_CARDNAME = oChild.GetProperty("U_CARDNAME").ToString();
                    factoring.Documentos.Add(documento);
                }
                resp.Factoring = factoring;
                resp.Success = true;
            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Mensaje = ex.Message;
            }
            return resp;
        }

        public static Comun.Message UpdateDocuments(Clases.Factoring Factoring)
        {
            Comun.Message resp = new Comun.Message();
            SAPbobsCOM.Documents oDoc = null;
            try
            {
                var listDocumentos = Factoring.Documentos;
                listDocumentos.ForEach(documento =>
                {
                    switch (documento.U_TIPODOC)
                    {
                        case "FC":
                            oDoc = (SAPbobsCOM.Documents)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices);
                            break;
                        case "NC":
                            oDoc = (SAPbobsCOM.Documents)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oCreditNotes);
                            break;
                        default:
                            oDoc = (SAPbobsCOM.Documents)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices);
                            break;
                    }
                    string key = @"<DocumentParams><DocEntry>" + documento.U_DOCENTRY + @"</DocEntry></DocumentParams>";
                    oDoc.Browser.GetByKeys(key);
                    //oDoc.GetByKey(documento.U_DOCENTRY);
                    if (oDoc.DocEntry != null && !oDoc.DocEntry.Equals(0))
                    {
                        oDoc.UserFields.Fields.Item("U_FACTORINGID").Value = Factoring.U_ID;
                        oDoc.UserFields.Fields.Item("U_INSTITU").Value = SBO.ConsultasSBO.ObtenerAliasBanco(Factoring.U_ENTIDAD);
                        //oDoc.UserFields.Fields.Item("U_FACTORINGVCTO").Value = Factoring.U_FECHA;
                        oDoc.UserFields.Fields.Item("U_NUMOPER").Value = Factoring.U_NUMOPER.ToString();
                        oDoc.UserFields.Fields.Item("U_RESFAC").Value = Factoring.U_TIPORES.ToString();
                        int errCode = 0;
                        string errMsg = string.Empty;
                        int retDoc = oDoc.Update();
                        if (retDoc != 0)
                        {
                            SBO.ConexionSBO.oCompany.GetLastError(out errCode, out errMsg);
                        }
                    }
                    oDoc = null;
                });
                resp.Success = true;
            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Mensaje = ex.Message;
            }
            return resp;
        }

        public static Comun.Message AddLogCobranza_Old(Clases.Cobranza Cobranza)
        {
            Comun.Message resp = new Comun.Message();

            SAPbobsCOM.GeneralService oGeneralService = null;
            SAPbobsCOM.GeneralData oGeneralData = null;
            SAPbobsCOM.GeneralDataParams oGeneralParams = null;
            SAPbobsCOM.CompanyService oCompanyService = null;

            try
            {
                oCompanyService = SBO.ConexionSBO.oCompany.GetCompanyService();
                oGeneralService = oCompanyService.GetGeneralService("SO_FACTORINGLOG");
                foreach (var item in Cobranza.Documentos)
                {
                    oGeneralData = ((SAPbobsCOM.GeneralData)(oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralData)));
                    oGeneralData.SetProperty("U_OBJTYPE", string.IsNullOrEmpty(item.U_OBJTYPE) ? "" : item.U_OBJTYPE);
                    oGeneralData.SetProperty("U_DOCENTRY", Convert.ToInt32(item.U_DOCENTRY));
                    oGeneralData.SetProperty("U_TIPODOC", string.IsNullOrEmpty(item.U_TIPODOC) ? "" : item.U_TIPODOC);
                    oGeneralData.SetProperty("U_DOCNUM", Convert.ToInt32(item.U_DOCNUM));
                    oGeneralData.SetProperty("U_FOLIONUM", Convert.ToInt32(item.U_FOLIONUM));
                    oGeneralData.SetProperty("U_DOCDATE", new DateTime(item.U_DOCDATE.Year, item.U_DOCDATE.Month, item.U_DOCDATE.Day));
                    oGeneralData.SetProperty("U_DOCDUEDATE", new DateTime(item.U_DOCDUEDATE.Year, item.U_DOCDUEDATE.Month, item.U_DOCDUEDATE.Day));
                    oGeneralData.SetProperty("U_TAXDATE", new DateTime(item.U_TAXDATE.Year, item.U_TAXDATE.Month, item.U_TAXDATE.Day));
                    oGeneralData.SetProperty("U_DOCCUR", string.IsNullOrEmpty(item.U_DOCCUR) ? "" : item.U_DOCCUR);
                    oGeneralData.SetProperty("U_DOCTOTAL", item.U_DOCTOTAL);
                    oGeneralData.SetProperty("U_CARDCODE", string.IsNullOrEmpty(item.U_CARDCODE) ? "" : item.U_CARDCODE);
                    oGeneralData.SetProperty("U_CARDNAME", string.IsNullOrEmpty(item.U_CARDNAME) ? "" : item.U_CARDNAME);
                    oGeneralData.SetProperty("U_EMAIL", string.IsNullOrEmpty(item.U_EMAIL) ? "" : item.U_EMAIL);
                    oGeneralData.SetProperty("U_EMAILTYPE", string.IsNullOrEmpty(item.U_EMAILTYPE) ? "" : item.U_EMAILTYPE);
                    oGeneralData.SetProperty("U_EMAILLANG", string.IsNullOrEmpty(item.U_EMAILLANG) ? "" : item.U_EMAILLANG);
                    oGeneralData.SetProperty("U_OBS", string.IsNullOrEmpty(item.U_OBS) ? "" : item.U_OBS);
                    oGeneralData.SetProperty("U_SENDDATE", DateTime.Now.Date);
                    oGeneralData.SetProperty("U_SENDTIME", DateTime.Now.ToString("s", DateTimeFormatInfo.InvariantInfo));

                    oGeneralParams = oGeneralService.Add(oGeneralData);
                    var DocEntry = oGeneralParams.GetProperty("DocEntry");
                    oGeneralData = null;
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

        public static Comun.Message AddLogCobranza(Clases.Cobranza Cobranza)
        {
            Comun.Message resp = new Comun.Message();

            SAPbobsCOM.GeneralService oGeneralService = null;
            SAPbobsCOM.GeneralData oGeneralData = null;
            SAPbobsCOM.GeneralDataParams oGeneralParams = null;
            SAPbobsCOM.CompanyService oCompanyService = null;

            try
            {
                var listClientes = Cobranza.Documentos.GroupBy(x => x.U_CARDCODE).ToList();

                listClientes.ForEach(cliente =>
                {
                    var documentos = Cobranza.Documentos.Where(z => z.U_CARDCODE == cliente.Key).ToList();
                    oCompanyService = SBO.ConexionSBO.oCompany.GetCompanyService();
                    oGeneralService = oCompanyService.GetGeneralService("SO_FACTORINGLOG");
                    oGeneralData = ((SAPbobsCOM.GeneralData)(oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralData)));
                    var item = Cobranza.Documentos.Where(z => z.U_CARDCODE == cliente.Key).FirstOrDefault();
                    oGeneralData.SetProperty("U_CARDCODE", string.IsNullOrEmpty(item.U_CARDCODE) ? "" : item.U_CARDCODE);
                    oGeneralData.SetProperty("U_CARDNAME", string.IsNullOrEmpty(item.U_CARDNAME) ? "" : item.U_CARDNAME);
                    oGeneralData.SetProperty("U_EMAIL", string.IsNullOrEmpty(item.U_EMAIL) ? "" : item.U_EMAIL);
                    oGeneralData.SetProperty("U_EMAILTYPE", string.IsNullOrEmpty(item.U_EMAILTYPE) ? "" : item.U_EMAILTYPE);
                    oGeneralData.SetProperty("U_EMAILLANG", string.IsNullOrEmpty(item.U_EMAILLANG) ? "" : item.U_EMAILLANG);
                    List<string> _docslist = new List<string>();
                    var docslist = documentos.GroupBy(x => x.U_DOCNUM.ToString()).ToList();
                    foreach (var doc in docslist)
                    {
                        _docslist.Add(string.Format("{0}", doc.Key));
                    }
                    string docs = string.Join(";", _docslist);
                    oGeneralData.SetProperty("U_DOCS", string.IsNullOrEmpty(docs) ? "" : docs);
                    _docslist = new List<string>();
                    docslist = documentos.GroupBy(x => x.U_FOLIONUM.ToString()).ToList();
                    foreach (var doc in docslist)
                    {
                        _docslist.Add(string.Format("{0}", doc.Key));
                    }
                    docs = string.Join(";", _docslist);
                    oGeneralData.SetProperty("U_FOLIOS", string.IsNullOrEmpty(docs) ? "" : docs);
                    oGeneralData.SetProperty("U_OBS", string.IsNullOrEmpty(item.U_OBS) ? "" : item.U_OBS);
                    oGeneralData.SetProperty("U_SENDDATE", DateTime.Now.Date);
                    oGeneralData.SetProperty("U_SENDTIME", DateTime.Now.ToString("s", DateTimeFormatInfo.InvariantInfo));
                    oGeneralParams = oGeneralService.Add(oGeneralData);
                    var DocEntry = oGeneralParams.GetProperty("DocEntry");
                    oGeneralData = null;
                });
                resp.Success = true;
            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Mensaje = ex.Message;
            }
            return resp;
        }
    }
}
