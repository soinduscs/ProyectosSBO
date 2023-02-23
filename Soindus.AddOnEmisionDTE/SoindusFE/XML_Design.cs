using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Soindus.AddOnEmisionDTE.SoindusFE
{
    public class XML_Design
    {
        public string Rut_Emisor = string.Empty;
        public string DV = string.Empty;
        public string Tipo = string.Empty;
        public string Folio = string.Empty;
        public string AmbienteProduccion = string.Empty;
        public string Cedible = string.Empty;
        public string Accion = string.Empty;

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
                case "39":
                case "41":
                case "56":
                case "110":
                case "111":
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

                        Documento emision = null;
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
                            case "IB":
                                XML_Boleta xml_boleta = new XML_Boleta();
                                emision = xml_boleta.Generar(DocEntry);
                                break;
                            case "EB":
                                XML_BoletaEx xml_boletaex = new XML_BoletaEx();
                                emision = xml_boletaex.Generar(DocEntry);
                                break;
                            case "DN":
                                switch (Indicator)
                                {
                                    case "11":
                                        //XML_NotaDebitoExport xml_notadebitoexport = new XML_NotaDebitoExport();
                                        //emision = xml_notadebitoexport.Generar(DocEntry);
                                        break;
                                    default:
                                        XML_NotaDebito xml_notadebito = new XML_NotaDebito();
                                        emision = xml_notadebito.Generar(DocEntry);
                                        break;
                                }
                                break;
                            case "IX":
                                XML_FacturaExport xml_facturaexport = new XML_FacturaExport();
                                emision = xml_facturaexport.Generar(DocEntry);
                                break;
                            default:
                                break;
                        }

                        var root = CreateSchema(emision);
                        retorno = root.ToString();
                    }
                    break;
                case "61":
                case "112":
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

                        Documento emision = null;
                        switch (DocSubType)
                        {
                            default:
                                switch (Indicator)
                                {
                                    case "12":
                                        //XML_NotaCreditoExport xml_notacreditoexport = new XML_NotaCreditoExport();
                                        //emision = xml_notacreditoexport.Generar(DocEntry);
                                        break;
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
                case "52":
                    oRec = (SAPbobsCOM.Recordset)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                    switch (DocSubType)
                    {
                        case "EM":
                            query = @"Select ""DocEntry"" from ""ODLN"" where ""DocNum"" = " + DocNum + "";
                            break;
                        case "ST":
                            query = @"Select ""DocEntry"" from ""OWTR"" where ""DocNum"" = " + DocNum + "";
                            break;
                        default:
                            break;
                    }
                    oRec.DoQuery(query);
                    if (!oRec.EoF)
                    {
                        DocEntry = (int)oRec.Fields.Item("DocEntry").Value;
                        //DocSubType = oRec.Fields.Item("DocSubType").Value.ToString();
                        Local.FuncionesComunes.LiberarObjetoGenerico(oRec);

                        Documento emision = null;
                        switch (DocSubType)
                        {
                            case "EM":
                                XML_Entrega xml_entrega = new XML_Entrega();
                                emision = xml_entrega.Generar(DocEntry);
                                break;
                            case "ST":
                                XML_Traslado xml_traslado = new XML_Traslado();
                                emision = xml_traslado.Generar(DocEntry);
                                break;
                            default:
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

        public XElement CreateSchema(Documento emision)
        {
            Local.Configuracion ExtConf = new Local.Configuracion();

            var json = string.Empty;

            if(emision.Encabezado.IdDoc.TipoDTE.Equals("39") || emision.Encabezado.IdDoc.TipoDTE.Equals("41"))
            {
                GenerarBoletaElectronica bolElect = new GenerarBoletaElectronica();

                bolElect.intCodigo = ExtConf.Parametros.User_WS;
                bolElect.strClave = ExtConf.Parametros.Pass_WS;
                bolElect.objDTE.Boleta.Documento = emision;
                bolElect.objDTE.NroResolucion = ExtConf.Parametros.NroResolucion;
                bolElect.objDTE.FechaResolucion = ExtConf.Parametros.FechaResolucion;
                bolElect.objDTE.AmbienteProduccion = ExtConf.Parametros.Prod_Env;
                json = JsonConvert.SerializeObject(bolElect);
                json = json.Replace(";", " ");
                XNode Node = JsonConvert.DeserializeXNode(json, "GenerarBoletaElectronica");

                XNamespace tem = "http://tempuri.org/";
                XNamespace soapenv = "http://schemas.xmlsoap.org/soap/envelope/";

                XElement docum = XElement.Parse(Node.ToString());

                foreach (XElement item in docum.DescendantsAndSelf())
                {
                    item.Name = tem + item.Name.LocalName;
                }

                XElement root = new XElement(soapenv + "Envelope",
                    new XAttribute(XNamespace.Xmlns + "soapenv", "http://schemas.xmlsoap.org/soap/envelope/"),
                    new XAttribute(XNamespace.Xmlns + "tem", "http://tempuri.org/"),
                    new XElement(soapenv + "Body",
                    new XElement(docum
                    ))
                );

                root.Descendants()
                           .Where(n => (n.IsEmpty || String.IsNullOrWhiteSpace(n.Value)) && n.Name != soapenv + "Header")
                           .Remove();

                return root;
            }
            else
            {
                GenerarDocumentoElectronico docElect = new GenerarDocumentoElectronico();

                docElect.intCodigo = ExtConf.Parametros.User_WS;
                docElect.strClave = ExtConf.Parametros.Pass_WS;
                docElect.objDTE.DTE.Documento = emision;
                docElect.objDTE.NroResolucion = ExtConf.Parametros.NroResolucion;
                docElect.objDTE.FechaResolucion = ExtConf.Parametros.FechaResolucion;
                docElect.objDTE.AmbienteProduccion = ExtConf.Parametros.Prod_Env;
                json = JsonConvert.SerializeObject(docElect);
                json = json.Replace(";", " ");
                XNode Node = JsonConvert.DeserializeXNode(json, "GenerarDocumentoElectronico");

                XNamespace tem = "http://tempuri.org/";
                XNamespace soapenv = "http://schemas.xmlsoap.org/soap/envelope/";

                XElement docum = XElement.Parse(Node.ToString());

                foreach (XElement item in docum.DescendantsAndSelf())
                {
                    item.Name = tem + item.Name.LocalName;
                }

                XElement root = new XElement(soapenv + "Envelope",
                    new XAttribute(XNamespace.Xmlns + "soapenv", "http://schemas.xmlsoap.org/soap/envelope/"),
                    new XAttribute(XNamespace.Xmlns + "tem", "http://tempuri.org/"),
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

        public string DesignRecuperarPDF()
        {
            string retorno = string.Empty;

            get_RecuperarPDF recuperarpdf = null;
            XML_RecuperarPDF xml_recuperarpdf = new XML_RecuperarPDF();
            recuperarpdf = xml_recuperarpdf.Generar(Rut_Emisor, Tipo, Folio, AmbienteProduccion, Cedible);

            var root = CreateSchemaRecuperarPDF(recuperarpdf);
            retorno = root.ToString();

            return retorno;
        }

        public XElement CreateSchemaRecuperarPDF(get_RecuperarPDF recuperarpdf)
        {
            Local.Configuracion ExtConf = new Local.Configuracion();

            recuperarpdf.intCodigo = ExtConf.Parametros.User_WS;
            recuperarpdf.strClave = ExtConf.Parametros.Pass_WS;
            var json = JsonConvert.SerializeObject(recuperarpdf);
            json = json.Replace(";", " ");
            XNode Node = JsonConvert.DeserializeXNode(json, "RecuperarPDF");

            XNamespace tem = "http://tempuri.org/";
            XNamespace soapenv = "http://schemas.xmlsoap.org/soap/envelope/";

            XElement docum = XElement.Parse(Node.ToString());

            foreach (XElement item in docum.DescendantsAndSelf())
            {
                item.Name = tem + item.Name.LocalName;
            }

            XElement root = new XElement(soapenv + "Envelope",
                new XAttribute(XNamespace.Xmlns + "soapenv", "http://schemas.xmlsoap.org/soap/envelope/"),
                new XAttribute(XNamespace.Xmlns + "tem", "http://tempuri.org/"),
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
