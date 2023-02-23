using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Globalization;

namespace Soindus.Interfaces.ProveedoresDTE
{
    public class Azurian
    {
        public Azurian()
        {

        }

        #region RECEPCION

        public Local.Message ObtenerDocumentos(string xml, string url)
        {
            Local.Message result = new Local.Message();

            try
            {
                string url_ws = url + @"/dte-ws-reception/services/ServicioRecepcion?wsdl";
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url_ws);
                webRequest.Headers.Add("SOAPAction", @"");
                webRequest.ContentType = "text/xml;charset=\"utf-8\"";
                webRequest.Accept = "text/xml";
                webRequest.Method = "POST";

                string strEnvelop = xml;

                XmlDocument soapEnvelop = new XmlDocument();
                soapEnvelop.LoadXml(strEnvelop);

                using (Stream stream = webRequest.GetRequestStream())
                {
                    soapEnvelop.Save(stream);
                }

                // begin async call to web request.
                IAsyncResult asyncResult = webRequest.BeginGetResponse(null, null);

                // suspend this thread until call is complete. You might want to
                // do something usefull here like update your UI.
                asyncResult.AsyncWaitHandle.WaitOne();

                string soapResult = string.Empty;
                using (WebResponse webResponse = webRequest.EndGetResponse(asyncResult))
                {
                    if (((HttpWebResponse)webResponse).StatusCode == HttpStatusCode.OK)
                    {
                        using (StreamReader rd = new StreamReader(webResponse.GetResponseStream()))
                        {
                            //string str = rd.ReadToEnd();

                            XDocument xDocument = XDocument.Load(rd);
                            XNamespace aw = "http://controller.ws.dte.azurian.com";
                            XElement newTree = new XElement("root",
                                from el in xDocument.Descendants()
                                where (el.Name == aw + "transferenciaDocumentosReturn")
                                select el
                            );

                            string json = JsonConvert.SerializeXNode(newTree);
                            soapResult = json;
                        }
                        result.Content = soapResult;
                        result.Success = true;
                    }
                    else
                    {
                        result.Mensaje = string.Format("{0}. Verifique conexión a internet e información de sesión.", ((HttpWebResponse)webResponse).StatusDescription);
                        result.Success = false;
                    }
                    return result;
                }
            }
            catch (Exception ex)
            {
                result.Mensaje = ex.Message;
                result.Success = false;
                return result;
            }
        }

        public Local.Message ObtenerDocumento(string xml, string url)
        {
            Local.Message result = new Local.Message();

            try
            {
                string url_ws = url + @"/dte-ws-reception/services/ServicioRecepcion?wsdl";
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url_ws);
                webRequest.Headers.Add("SOAPAction", @"");
                webRequest.ContentType = "text/xml;charset=\"utf-8\"";
                webRequest.Accept = "text/xml";
                webRequest.Method = "POST";

                string strEnvelop = xml;

                XmlDocument soapEnvelop = new XmlDocument();
                soapEnvelop.LoadXml(strEnvelop);

                using (Stream stream = webRequest.GetRequestStream())
                {
                    soapEnvelop.Save(stream);
                }

                // begin async call to web request.
                IAsyncResult asyncResult = webRequest.BeginGetResponse(null, null);

                // suspend this thread until call is complete. You might want to
                // do something usefull here like update your UI.
                asyncResult.AsyncWaitHandle.WaitOne();

                string soapResult = string.Empty;
                using (WebResponse webResponse = webRequest.EndGetResponse(asyncResult))
                {
                    if (((HttpWebResponse)webResponse).StatusCode == HttpStatusCode.OK)
                    {
                        using (StreamReader rd = new StreamReader(webResponse.GetResponseStream()))
                        {
                            XDocument xDocument = XDocument.Load(rd);
                            XNamespace aw = "http://controller.ws.dte.azurian.com";
                            XElement newTree = new XElement("root",
                                from el in xDocument.Descendants()
                                where (el.Name == aw + "descargarArchivoReturn")
                                select el
                            );

                            string json = JsonConvert.SerializeXNode(newTree);
                            soapResult = json;
                        }
                        result.Content = soapResult;
                        result.Success = true;
                    }
                    else
                    {
                        result.Mensaje = string.Format("{0}. Verifique conexión a internet e información de sesión.", ((HttpWebResponse)webResponse).StatusDescription);
                        result.Success = false;
                    }
                    return result;
                }

            }
            catch (Exception ex)
            {
                result.Mensaje = ex.Message;
                result.Success = false;
                return result;
            }
        }

        public Local.Message ObtenerPDFDocumento(string xml, string url)
        {
            Local.Message result = new Local.Message();

            try
            {
                string url_ws = url + @"/dte-ws-reception/services/ServicioRecepcion?wsdl";
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url_ws);
                webRequest.Headers.Add("SOAPAction", @"");
                webRequest.ContentType = "text/xml;charset=\"utf-8\"";
                webRequest.Accept = "text/xml";
                webRequest.Method = "POST";

                string strEnvelop = xml;

                XmlDocument soapEnvelop = new XmlDocument();
                soapEnvelop.LoadXml(strEnvelop);

                using (Stream stream = webRequest.GetRequestStream())
                {
                    soapEnvelop.Save(stream);
                }

                // begin async call to web request.
                IAsyncResult asyncResult = webRequest.BeginGetResponse(null, null);

                // suspend this thread until call is complete. You might want to
                // do something usefull here like update your UI.
                asyncResult.AsyncWaitHandle.WaitOne();

                string soapResult = string.Empty;
                using (WebResponse webResponse = webRequest.EndGetResponse(asyncResult))
                {
                    if (((HttpWebResponse)webResponse).StatusCode == HttpStatusCode.OK)
                    {
                        using (StreamReader rd = new StreamReader(webResponse.GetResponseStream()))
                        {
                            XDocument xDocument = XDocument.Load(rd);
                            XNamespace aw = "http://controller.ws.dte.azurian.com";
                            XElement newTree = new XElement("root",
                                from el in xDocument.Descendants()
                                where (el.Name == aw + "descargarArchivoReturn")
                                select el
                            );

                            string json = JsonConvert.SerializeXNode(newTree);
                            soapResult = json;
                        }
                        result.Content = soapResult;
                        result.Success = true;
                    }
                    else
                    {
                        result.Mensaje = string.Format("{0}. Verifique conexión a internet e información de sesión.", ((HttpWebResponse)webResponse).StatusDescription);
                        result.Success = false;
                    }
                    return result;
                }

            }
            catch (Exception ex)
            {
                result.Mensaje = ex.Message;
                result.Success = false;
                return result;
            }
        }

        public Local.Message CambiarEstadoComercial(string xml, string url)
        {
            Local.Message result = new Local.Message();

            try
            {
                string url_ws = url + @"/WSDocProv/WSDocProv.asmx?WSDL";
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url_ws);
                webRequest.Headers.Add("SOAPAction", @"http://tempuri.org/AceptacionRechazo");
                webRequest.ContentType = "text/xml;charset=\"utf-8\"";
                webRequest.Accept = "text/xml";
                webRequest.Method = "POST";

                string strEnvelop = xml;

                XmlDocument soapEnvelop = new XmlDocument();
                soapEnvelop.LoadXml(strEnvelop);

                using (Stream stream = webRequest.GetRequestStream())
                {
                    soapEnvelop.Save(stream);
                }

                // begin async call to web request.
                IAsyncResult asyncResult = webRequest.BeginGetResponse(null, null);

                // suspend this thread until call is complete. You might want to
                // do something usefull here like update your UI.
                asyncResult.AsyncWaitHandle.WaitOne();

                string soapResult = string.Empty;
                using (WebResponse webResponse = webRequest.EndGetResponse(asyncResult))
                {
                    if (((HttpWebResponse)webResponse).StatusCode == HttpStatusCode.OK)
                    {
                        using (StreamReader rd = new StreamReader(webResponse.GetResponseStream()))
                        {
                            XDocument xDocument = XDocument.Load(rd);
                            XNamespace aw = "http://tempuri.org/";
                            XElement newTree = new XElement("root",
                                from el in xDocument.Descendants()
                                where (el.Name == aw + "AceptacionRechazoResult")
                                select el
                            );

                            string json = JsonConvert.SerializeXNode(newTree);
                            soapResult = json;
                        }
                        result.Content = soapResult;
                        result.Success = true;
                    }
                    else
                    {
                        result.Mensaje = string.Format("{0}. Verifique conexión a internet e información de sesión.", ((HttpWebResponse)webResponse).StatusDescription);
                        result.Success = false;
                    }
                    return result;
                }

            }
            catch (Exception ex)
            {
                result.Mensaje = ex.Message;
                result.Success = false;
                return result;
            }
        }

        public enum EstadosComerciales
        {
            Acuse_Recibo = 0, //Acuse de recibo para indicar que una factura ya fue recibida
            Aceptacion_Comercial = 1, //Aceptación Comercial del documento
            Rechazo_Comercial = 2, //Rechazo comercial del documento por alguna discrepancia en la formalidad del documento
            Aceptacion_Recibo_Mercaderias = 3, //Recibo de Mercaderías o Servicios conforme
            Reclamo_Parcial = 4, //Reclamo parcial del documento por inconformidad de la entrega del servicio o mercadería
            Reclamo_Total = 5  //Reclamo total del documento por inconformidad de la entrega del servicio o mercadería
        }

        #endregion
    }
}
