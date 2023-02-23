using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
//comentar//
using SAPbouiCOM.Framework;

namespace Soindus.AddOnRindegastos.SBO
{
    public class ModeloSBO
    {
        public ModeloSBO()
        {
        }

        public static Clases.Message AddRendiciones(Clases.ExpenseReport RgExpenseReport, Clases.ResponseExpenses RgExpenses)
        {
            Clases.Message resp = new Clases.Message();

            SAPbobsCOM.GeneralService oGeneralService = null;
            SAPbobsCOM.GeneralData oGeneralData = null;
            SAPbobsCOM.GeneralData oChild = null;
            SAPbobsCOM.GeneralDataCollection oChildren = null;
            SAPbobsCOM.GeneralDataParams oGeneralParams = null;
            SAPbobsCOM.CompanyService oCompanyService = null;
            try
            {
                //comentar
                Application.SBO_Application.StatusBar.SetText(string.Format("Se guardará la rendición {0} con {1} gastos.", RgExpenseReport.Id, RgExpenses.Expenses.Count()), SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);

                oCompanyService = SBO.ConexionSBO.oCompany.GetCompanyService();
                var item = RgExpenseReport;
                var Id = item.Id;
                SAPbobsCOM.Recordset oRecord = (SAPbobsCOM.Recordset)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                string _query = @"SELECT COUNT(1) AS ""RESP"" FROM ""@SO_RENDI"" WHERE ""U_ID"" = " + Id + "";
                oRecord.DoQuery(_query);
                if (!oRecord.EoF)
                {
                    if (oRecord.Fields.Item("RESP").Value.Equals(0))
                    {
                        oGeneralService = oCompanyService.GetGeneralService("SO_RENDICION");
                        oGeneralData = ((SAPbobsCOM.GeneralData)(oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralData)));
                        oGeneralData.SetProperty("U_ID", Convert.ToInt32(item.Id));
                        oGeneralData.SetProperty("U_TITLE", item.Title);
                        oGeneralData.SetProperty("U_REPNUM", Convert.ToInt32(item.ReportNumber));
                        oGeneralData.SetProperty("U_SDATE", new DateTime(item.SendDate.Year, item.SendDate.Month, item.SendDate.Day));
                        oGeneralData.SetProperty("U_CDATE", new DateTime(item.CloseDate.Year, item.CloseDate.Month, item.CloseDate.Day));
                        oGeneralData.SetProperty("U_EMPID", Convert.ToInt32(item.EmployeeId));
                        oGeneralData.SetProperty("U_EMPNAME", item.EmployeeName);
                        oGeneralData.SetProperty("U_EMPIDE", item.EmployeeIdentification);
                        oGeneralData.SetProperty("U_APPID", Convert.ToInt32(item.ApproverId));
                        oGeneralData.SetProperty("U_APPNAME", item.ApproverName);
                        oGeneralData.SetProperty("U_POLID", Convert.ToInt32(item.PolicyId));
                        oGeneralData.SetProperty("U_POLNAME", item.PolicyName);
                        oGeneralData.SetProperty("U_STATUS", Convert.ToInt32(item.Status));
                        oGeneralData.SetProperty("U_CSTATUS", item.CustomStatus);
                        oGeneralData.SetProperty("U_FUNDID", Convert.ToInt32(item.FundId));
                        oGeneralData.SetProperty("U_FUNDNAME", item.FundName);
                        oGeneralData.SetProperty("U_REPTOT", item.ReportTotal);
                        oGeneralData.SetProperty("U_REPTOTA", item.ReportTotalApproved);
                        oGeneralData.SetProperty("U_CUR", item.Currency);
                        oGeneralData.SetProperty("U_NOTE", item.Note.PadRight(250).Substring(0, 250).Trim());
                        oGeneralData.SetProperty("U_INTEG", item.Integrated);
                        oGeneralData.SetProperty("U_INTDATE", item.IntegrationDate);
                        oGeneralData.SetProperty("U_INTECODE", item.IntegrationExternalCode);
                        oGeneralData.SetProperty("U_INTICODE", item.IntegrationInternalCode);
                        oGeneralData.SetProperty("U_NBREXP", Convert.ToInt32(item.NbrExpenses));
                        oGeneralData.SetProperty("U_NBRAEXP", Convert.ToInt32(item.NbrApprovedExpenses));
                        oGeneralData.SetProperty("U_NBRREXP", Convert.ToInt32(item.NbrRejectedExpenses));
                        oGeneralData.SetProperty("U_ESTADO", 0);
                        oGeneralData.SetProperty("U_OBS", string.Empty);
                        string Gestion = string.Empty;
                        oChildren = oGeneralData.Child("SO_RENDICE");
                        foreach (var child in item.ExtraFields)
                        {
                            oChild = oChildren.Add();
                            oChild.SetProperty("U_NAME", child.Name);
                            oChild.SetProperty("U_VALUE", child.Value);
                            oChild.SetProperty("U_CODE", child.Code);
                            if (child.Name.Equals("Tipo de Gestión") && child.Value.Equals("Solicitud"))
                            {
                                Gestion = "Solicitud";
                            }
                            else if (child.Name.Equals("Tipo de Gestión") && child.Value.Equals("Rendición"))
                            {
                                Gestion = "Rendición";
                            }
                            else if (child.Name.Equals("Tipo de Gestión") && child.Value.Equals("Devolución"))
                            {
                                Gestion = "Devolución";
                            }
                        }
                        oGeneralData.SetProperty("U_GESTION", Gestion);
                        oGeneralParams = oGeneralService.Add(oGeneralData);
                        oChild = null;
                        oChildren = null;
                        oGeneralData = null;

                        oGeneralService = oCompanyService.GetGeneralService("SO_RENDIGTO");
                        foreach (var gasto in RgExpenses.Expenses)
                        {
                            oGeneralData = ((SAPbobsCOM.GeneralData)(oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralData)));
                            oGeneralData.SetProperty("U_ID", Convert.ToInt32(gasto.Id));
                            oGeneralData.SetProperty("U_STATUS", Convert.ToInt32(gasto.Status));
                            oGeneralData.SetProperty("U_SUPPLIER", gasto.Supplier);
                            oGeneralData.SetProperty("U_IDATE", new DateTime(gasto.IssueDate.Year, gasto.IssueDate.Month, gasto.IssueDate.Day));
                            oGeneralData.SetProperty("U_OAMOUNT", gasto.OriginalAmount);
                            oGeneralData.SetProperty("U_OCUR", gasto.OriginalCurrency);
                            oGeneralData.SetProperty("U_EXRATE", gasto.ExchangeRate);
                            oGeneralData.SetProperty("U_NET", gasto.Net);
                            oGeneralData.SetProperty("U_TAX", gasto.Tax);
                            oGeneralData.SetProperty("U_TAXNAME", gasto.TaxName);
                            oGeneralData.SetProperty("U_OTAX", gasto.OtherTaxes);
                            oGeneralData.SetProperty("U_RETNAME", gasto.RetentionName);
                            oGeneralData.SetProperty("U_RET", gasto.Retention);
                            oGeneralData.SetProperty("U_TOTAL", gasto.Total);
                            oGeneralData.SetProperty("U_CUR", gasto.Currency);
                            oGeneralData.SetProperty("U_REIMB", gasto.Reimbursable == true ? 1 : 0);
                            oGeneralData.SetProperty("U_CATEGORY", gasto.Category);
                            oGeneralData.SetProperty("U_CATCODE", gasto.CategoryCode);
                            oGeneralData.SetProperty("U_CATGRP", gasto.CategoryGroup);
                            oGeneralData.SetProperty("U_CATGRPC", gasto.CategoryGroupCode);
                            oGeneralData.SetProperty("U_NOTE", gasto.Note.PadRight(250).Substring(0, 250).Trim());
                            oGeneralData.SetProperty("U_INTDATE", gasto.IntegrationDate);
                            oGeneralData.SetProperty("U_INTECODE", gasto.IntegrationExternalCode);
                            oGeneralData.SetProperty("U_NBRFILES", Convert.ToInt32(gasto.NbrFiles));
                            oGeneralData.SetProperty("U_RID", Convert.ToInt32(item.Id));
                            oGeneralData.SetProperty("U_EXPOLID", Convert.ToInt32(gasto.ExpensePolicyId));
                            oGeneralData.SetProperty("U_USERID", Convert.ToInt32(gasto.UserId));
                            oChildren = oGeneralData.Child("SO_RENDIGCE");
                            foreach (var child in gasto.ExtraFields)
                            {
                                oChild = oChildren.Add();
                                oChild.SetProperty("U_NAME", child.Name);
                                oChild.SetProperty("U_VALUE", child.Value);
                                oChild.SetProperty("U_CODE", child.Code);
                            }
                            oGeneralParams = oGeneralService.Add(oGeneralData);
                            oChild = null;
                            oChildren = null;
                            oGeneralData = null;
                        }
                    }
                }
                Comun.FuncionesComunes.LiberarObjetoGenerico(oRecord);
                resp.Success = true;
            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Mensaje = ex.Message;
                //comentar
                Application.SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
            }
            return resp;
        }

        public static Clases.Message GetRendiciones(string Estado = null, string Gestion = null)
        {
            Clases.Message resp = new Clases.Message();
            Clases.Rendiciones rendiciones = new Clases.Rendiciones();

            SAPbobsCOM.GeneralService oGeneralService = null;
            SAPbobsCOM.GeneralData oGeneralData = null;
            SAPbobsCOM.GeneralData oChild = null;
            SAPbobsCOM.GeneralDataCollection oChildren = null;
            SAPbobsCOM.GeneralDataParams oGeneralParams = null;
            SAPbobsCOM.CompanyService oCompanyService = null;
            try
            {
                oCompanyService = SBO.ConexionSBO.oCompany.GetCompanyService();

                SAPbobsCOM.Recordset oRecord = (SAPbobsCOM.Recordset)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                string _query = @"SELECT ""DocEntry"" " +
                        @"FROM ""@SO_RENDI"" ""RENDI"" " +
                        @"WHERE ""RENDI"".""U_ESTADO"" IN (0, 2) "; //0=no procesados y 2=reprocesar
                if (!string.IsNullOrEmpty(Estado))
                {
                    _query += @"AND ""RENDI"".""U_STATUS"" = " + Estado + " ";
                }
                if (!string.IsNullOrEmpty(Gestion))
                {
                    _query += @"AND ""RENDI"".""U_GESTION"" = '" + Gestion + "' ";
                }
                oRecord.DoQuery(_query);
                if (!oRecord.EoF)
                {
                    while (!oRecord.EoF)
                    {
                        int? _DocEntry = (int?)oRecord.Fields.Item("DocEntry").Value;
                        oGeneralService = oCompanyService.GetGeneralService("SO_RENDICION");
                        oGeneralParams = ((SAPbobsCOM.GeneralDataParams)(oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)));
                        oGeneralParams.SetProperty("DocEntry", _DocEntry);
                        oGeneralData = oGeneralService.GetByParams(oGeneralParams);
                        var xml = oGeneralData.ToXMLString();
                        xml = xml.Replace("Rendiciones RG>", "RendicionesRG>");
                        System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                        doc.LoadXml(xml);

                        string json = JsonConvert.SerializeXmlNode(doc);
                        var _rendicion = Newtonsoft.Json.JsonConvert.DeserializeObject<Clases.Rendicion>(json);

                        SAPbobsCOM.Recordset oRecordG = (SAPbobsCOM.Recordset)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                        _query = @"SELECT ""DocEntry"" " +
                                @"FROM ""@SO_RENDIG"" ""RENDI"" " +
                                @"WHERE ""RENDI"".""U_RID"" = " + _rendicion.Detalle.UId + " ";
                        if (!string.IsNullOrEmpty(Estado))
                        {
                            _query += @"AND ""RENDI"".""U_STATUS"" = " + Estado + " ";
                        }
                        oRecordG.DoQuery(_query);
                        if (!oRecordG.EoF)
                        {
                            Clases.Gastos gastos = new Clases.Gastos();
                            while (!oRecordG.EoF)
                            {
                                _DocEntry = (int?)oRecordG.Fields.Item("DocEntry").Value;
                                oGeneralService = oCompanyService.GetGeneralService("SO_RENDIGTO");
                                oGeneralParams = ((SAPbobsCOM.GeneralDataParams)(oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)));
                                oGeneralParams.SetProperty("DocEntry", _DocEntry);
                                oGeneralData = oGeneralService.GetByParams(oGeneralParams);
                                xml = oGeneralData.ToXMLString();
                                xml = xml.Replace("Gastos Rendiciones RG>", "GastosRendicionesRG>");
                                doc = new System.Xml.XmlDocument();
                                doc.LoadXml(xml);

                                json = JsonConvert.SerializeXmlNode(doc);
                                var _gasto = Newtonsoft.Json.JsonConvert.DeserializeObject<Clases.Gasto>(json);
                                gastos.Items.Add(_gasto);

                                oRecordG.MoveNext();
                            }
                            _rendicion.Detalle.Gastos = gastos;
                        }
                        Comun.FuncionesComunes.LiberarObjetoGenerico(oRecordG);

                        rendiciones.Items.Add(_rendicion);
                        oRecord.MoveNext();
                    }
                }
                resp.Success = true;
                resp.Rendiciones = rendiciones;
                Comun.FuncionesComunes.LiberarObjetoGenerico(oRecord);
            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Mensaje = ex.Message;
            }
            return resp;
        }

        public static Clases.Message GetSolicitud(string DocEntry)
        {
            Clases.Message resp = new Clases.Message();
            Clases.Rendiciones rendiciones = new Clases.Rendiciones();

            SAPbobsCOM.GeneralService oGeneralService = null;
            SAPbobsCOM.GeneralData oGeneralData = null;
            SAPbobsCOM.GeneralData oChild = null;
            SAPbobsCOM.GeneralDataCollection oChildren = null;
            SAPbobsCOM.GeneralDataParams oGeneralParams = null;
            SAPbobsCOM.CompanyService oCompanyService = null;
            try
            {
                oCompanyService = SBO.ConexionSBO.oCompany.GetCompanyService();

                SAPbobsCOM.Recordset oRecord = (SAPbobsCOM.Recordset)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                string _query = @"SELECT ""DocEntry"" " +
                        @"FROM ""@SO_RENDI"" ""RENDI"" " +
                        @"WHERE ""RENDI"".""DocEntry"" = " + DocEntry + " ";
                oRecord.DoQuery(_query);
                if (!oRecord.EoF)
                {
                    while (!oRecord.EoF)
                    {
                        int? _DocEntry = Convert.ToInt32(DocEntry);
                        oGeneralService = oCompanyService.GetGeneralService("SO_RENDICION");
                        oGeneralParams = ((SAPbobsCOM.GeneralDataParams)(oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)));
                        oGeneralParams.SetProperty("DocEntry", _DocEntry);
                        oGeneralData = oGeneralService.GetByParams(oGeneralParams);
                        var xml = oGeneralData.ToXMLString();
                        xml = xml.Replace("Rendiciones RG>", "RendicionesRG>");
                        System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                        doc.LoadXml(xml);

                        string json = JsonConvert.SerializeXmlNode(doc);
                        var _rendicion = Newtonsoft.Json.JsonConvert.DeserializeObject<Clases.Rendicion>(json);

                        SAPbobsCOM.Recordset oRecordG = (SAPbobsCOM.Recordset)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                        _query = @"SELECT ""DocEntry"" " +
                                @"FROM ""@SO_RENDIG"" ""RENDI"" " +
                                @"WHERE ""RENDI"".""U_RID"" = " + _rendicion.Detalle.UId + " ";
                        oRecordG.DoQuery(_query);
                        if (!oRecordG.EoF)
                        {
                            Clases.Gastos gastos = new Clases.Gastos();
                            while (!oRecordG.EoF)
                            {
                                _DocEntry = (int?)oRecordG.Fields.Item("DocEntry").Value;
                                oGeneralService = oCompanyService.GetGeneralService("SO_RENDIGTO");
                                oGeneralParams = ((SAPbobsCOM.GeneralDataParams)(oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)));
                                oGeneralParams.SetProperty("DocEntry", _DocEntry);
                                oGeneralData = oGeneralService.GetByParams(oGeneralParams);
                                xml = oGeneralData.ToXMLString();
                                xml = xml.Replace("Gastos Rendiciones RG>", "GastosRendicionesRG>");
                                doc = new System.Xml.XmlDocument();
                                doc.LoadXml(xml);

                                json = JsonConvert.SerializeXmlNode(doc);
                                var _gasto = Newtonsoft.Json.JsonConvert.DeserializeObject<Clases.Gasto>(json);
                                gastos.Items.Add(_gasto);

                                oRecordG.MoveNext();
                            }
                            _rendicion.Detalle.Gastos = gastos;
                        }
                        Comun.FuncionesComunes.LiberarObjetoGenerico(oRecordG);

                        rendiciones.Items.Add(_rendicion);
                        oRecord.MoveNext();
                    }
                }
                resp.Success = true;
                resp.Rendiciones = rendiciones;
                Comun.FuncionesComunes.LiberarObjetoGenerico(oRecord);
            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Mensaje = ex.Message;
            }
            return resp;
        }

        public static Clases.Message UpdateRendicion(string DocEntry, int Estado, string Observacion)
        {
            Clases.Message resp = new Clases.Message();

            SAPbobsCOM.GeneralService oGeneralService = null;
            SAPbobsCOM.GeneralData oGeneralData = null;
            SAPbobsCOM.GeneralData oChild = null;
            SAPbobsCOM.GeneralDataCollection oChildren = null;
            SAPbobsCOM.GeneralDataParams oGeneralParams = null;
            SAPbobsCOM.CompanyService oCompanyService = null;
            try
            {
                oCompanyService = SBO.ConexionSBO.oCompany.GetCompanyService();

                int? _DocEntry = Convert.ToInt32(DocEntry);
                oGeneralService = oCompanyService.GetGeneralService("SO_RENDICION");
                oGeneralParams = ((SAPbobsCOM.GeneralDataParams)(oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)));
                oGeneralParams.SetProperty("DocEntry", _DocEntry);
                oGeneralData = oGeneralService.GetByParams(oGeneralParams);
                oGeneralData.SetProperty("U_ESTADO", Estado);
                oGeneralData.SetProperty("U_OBS", Observacion);
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
    }
}
