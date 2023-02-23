﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Microsoft.Web.Services3.Security.Tokens;

namespace Soindus.AddOnEmisionDTE.DBNetEC
{
    public class XML_Design
    {
        public string Design(string DocNum, string DocType, string DocSubType = "")
        {
            string retorno = string.Empty;
            string query = string.Empty;
            int DocEntry = 0;
            string Indicator = string.Empty;
            SAPbobsCOM.Recordset oRec = null;
            switch (DocType)
            {
                case "33":
                case "34":
                case "56":
                    oRec = (SAPbobsCOM.Recordset)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                    query = @"Select ""DocEntry"", ""Indicator"" from ""OINV"" where ""DocNum"" = " + DocNum + "";
                    if (!string.IsNullOrEmpty(DocSubType) && (DocSubType.Equals("FR") || DocSubType.Equals("BR")))
                    {
                        query += @" and ""DocSubType"" = '--' and ""isIns"" = 'Y'";
                    }
                    else if (!string.IsNullOrEmpty(DocSubType))
                    {
                        query += @" and ""DocSubType"" = '" + DocSubType + "'";
                    }

                    oRec.DoQuery(query);
                    if (!oRec.EoF)
                    {
                        DocEntry = (int)oRec.Fields.Item("DocEntry").Value;
                        Indicator = oRec.Fields.Item("Indicator").Value.ToString();
                        //DocSubType = oRec.Fields.Item("DocSubType").Value.ToString();
                        Local.FuncionesComunes.LiberarObjetoGenerico(oRec);

                        putCustomerETDLoad emision = null;
                        switch (DocSubType)
                        {
                            case "--":
                                XML_Factura xml_factura = new XML_Factura();
                                emision = xml_factura.Generar(DocEntry);
                                break;
                            case "IE":
                                XML_FacturaEx xml_facturaex = new XML_FacturaEx();
                                emision = xml_facturaex.Generar(DocEntry);
                                break;
                            case "DN":
                                switch (Indicator)
                                {
                                    default:
                                        XML_NotaDebito xml_notadebito = new XML_NotaDebito();
                                        emision = xml_notadebito.Generar(DocEntry);
                                        break;
                                }
                                break;
                            default:
                                break;
                        }

                        var root = CreateSchema(emision);
                        retorno = root.ToString();
                    }
                    break;
                case "61":
                    oRec = (SAPbobsCOM.Recordset)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                    query = @"Select ""DocEntry"", ""Indicator"" from ""ORIN"" where ""DocNum"" = " + DocNum + "";
                    if (!string.IsNullOrEmpty(DocSubType))
                    {
                        query += @" and ""DocSubType"" = '" + DocSubType + "'";
                    }
                    oRec.DoQuery(query);
                    if (!oRec.EoF)
                    {
                        DocEntry = (int)oRec.Fields.Item("DocEntry").Value;
                        Indicator = oRec.Fields.Item("Indicator").Value.ToString();
                        //DocSubType = oRec.Fields.Item("DocSubType").Value.ToString();
                        Local.FuncionesComunes.LiberarObjetoGenerico(oRec);

                        putCustomerETDLoad emision = null;
                        switch (DocSubType)
                        {
                            default:
                                switch (Indicator)
                                {
                                    default:
                                        XML_NotaCredito xml_notacredito = new XML_NotaCredito();
                                        emision = xml_notacredito.Generar(DocEntry);
                                        break;
                                }
                                break;
                        }

                        var root = CreateSchema(emision);
                        retorno = root.ToString();
                    }
                    break;
                default:
                    break;
            }
            return retorno;
        }

        public XElement CreateSchema(putCustomerETDLoad emision)
        {
            Local.Configuracion ExtConf = new Local.Configuracion();
            var json = JsonConvert.SerializeObject(emision);
            json = json.Replace(";", " ");
            XNode Node = JsonConvert.DeserializeXNode(json, "putCustomerETDLoad");

            UsernameToken t = new UsernameToken(ExtConf.Parametros.User_WS, ExtConf.Parametros.Pass_WS, PasswordOption.SendHashed);
            var id = t.Id;
            var nonce = Convert.ToBase64String(t.Nonce);
            var user = t.Username;
            var pass = t.Password;
            var created = t.Created.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
            var pd = t.PasswordDigest;

            string UsernameToken = id;
            string Username = user;
            string Password = pass;
            string Nonce = nonce;
            string Created = created;
            string Type = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordText";
            string EncodingType = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary";
            XNamespace cus = "http://www.dbnet.cl/CustomerETDLoadASP";
            XNamespace soapenv = "http://schemas.xmlsoap.org/soap/envelope/";
            XNamespace wsse = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd";
            XNamespace wsu = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd";

            XElement docum = XElement.Parse(Node.ToString());
            //docum.Add(new XAttribute(XNamespace.Xmlns + "cus", cus));

            foreach (XElement item in docum.DescendantsAndSelf())
            {
                item.Name = cus + item.Name.LocalName;
            }

            XElement root = new XElement(soapenv + "Envelope",
                new XAttribute(XNamespace.Xmlns + "soapenv", "http://schemas.xmlsoap.org/soap/envelope/"),
                new XAttribute(XNamespace.Xmlns + "cus", "http://www.dbnet.cl/CustomerETDLoadASP"),
                new XElement(soapenv + "Header",
                new XElement(wsse + "Security",
                new XAttribute(XNamespace.Xmlns + "wsu", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd"),
                new XAttribute(XNamespace.Xmlns + "wsse", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd"),
                new XElement(wsse + "UsernameToken",
                new XAttribute(wsu + "Id", UsernameToken),
                new XElement(wsse + "Username", Username),
                new XElement(wsse + "Password", Password, new XAttribute("Type", Type)),
                new XElement(wsse + "Nonce", Nonce, new XAttribute("EncodingType", EncodingType)),
                new XElement(wsu + "Created", Created)
                )
                )
                ),
                new XElement(soapenv + "Body",
                new XElement(docum
                ))
            );

            root.Descendants()
                       .Where(n => (n.IsEmpty || String.IsNullOrWhiteSpace(n.Value)) && n.Name != cus + "Extras" && n.Name != cus + "DescuentosRecargosyOtros")
                       .Remove();

            return root;
        }

        public string DesignPDF(string rutt = "", string folio = "", string doc = "", string monto = "", string fecha = "", string merito = "false")
        {
            string retorno = string.Empty;

            get_pdf pdf = null;
            XML_PDF xml_pdf = new XML_PDF();
            pdf = xml_pdf.Generar(rutt, folio, doc, monto, fecha, merito);

            var root = CreateSchemaPDF(pdf);
            retorno = root.ToString();

            return retorno;
        }

        public XElement CreateSchemaPDF(get_pdf pdf)
        {
            Local.Configuracion ExtConf = new Local.Configuracion();
            var json = JsonConvert.SerializeObject(pdf);
            XNode Node = JsonConvert.DeserializeXNode(json, "get_pdf");

            UsernameToken t = new UsernameToken(ExtConf.Parametros.User_WS, ExtConf.Parametros.Pass_WS, PasswordOption.SendHashed);
            var id = t.Id;
            var nonce = Convert.ToBase64String(t.Nonce);
            var user = t.Username;
            var pass = t.Password;
            var created = t.Created.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
            var pd = t.PasswordDigest;

            string UsernameToken = id;
            string Username = user;
            string Password = pass;
            string Nonce = nonce;
            string Created = created;
            string Type = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordText";
            string EncodingType = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary";
            XNamespace get = "http://www.dbnet.cl/getPDF64";
            XNamespace soapenv = "http://schemas.xmlsoap.org/soap/envelope/";
            XNamespace wsse = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd";
            XNamespace wsu = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd";

            XElement docum = XElement.Parse(Node.ToString());
            //docum.Add(new XAttribute(XNamespace.Xmlns + "cus", cus));

            foreach (XElement item in docum.DescendantsAndSelf())
            {
                item.Name = get + item.Name.LocalName;
            }

            XElement root = new XElement(soapenv + "Envelope",
                new XAttribute(XNamespace.Xmlns + "soapenv", "http://schemas.xmlsoap.org/soap/envelope/"),
                new XAttribute(XNamespace.Xmlns + "get", "http://www.dbnet.cl/getPDF64"),
                new XElement(soapenv + "Header",
                new XElement(wsse + "Security",
                new XAttribute(XNamespace.Xmlns + "wsu", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd"),
                new XAttribute(XNamespace.Xmlns + "wsse", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd"),
                new XElement(wsse + "UsernameToken",
                new XAttribute(wsu + "Id", UsernameToken),
                new XElement(wsse + "Username", Username),
                new XElement(wsse + "Password", Password, new XAttribute("Type", Type)),
                new XElement(wsse + "Nonce", Nonce, new XAttribute("EncodingType", EncodingType)),
                new XElement(wsu + "Created", Created)
                )
                )
                ),
                new XElement(soapenv + "Body",
                new XElement(docum
                ))
            );

            root.Descendants()
                       .Where(n => (n.IsEmpty || String.IsNullOrWhiteSpace(n.Value)) && n.Name != soapenv + "Header")
                       .Remove();

            return root;
        }
    }
}
