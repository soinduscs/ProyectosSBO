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

namespace Soindus.Interfaces.ProveedoresDTE
{
    public class SoindusFE
    {
        public SoindusFE()
        {

        }
        #region RECEPCION

        public Local.Message ObtenerDocumentos(string xml, string url)
        {
            Local.Message result = new Local.Message();

            try
            {
                string url_ws = url + @"/WSDocProv/WSDocProv.asmx?WSDL";
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url_ws);
                webRequest.Headers.Add("SOAPAction", @"http://tempuri.org/ListaResumenDocumento");
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
                                where (el.Name == aw + "ListaResumenDocumentoResult")
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
                string url_ws = url + @"/WSDocProv/WSDocProv.asmx?WSDL";
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url_ws);
                webRequest.Headers.Add("SOAPAction", @"http://tempuri.org/TraerDocumentoXML");
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
                                where (el.Name == aw + "TraerDocumentoXMLResult")
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
                string url_ws = url + @"/WSFactElect/FactElect.asmx?WSDL";
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url_ws);
                webRequest.Headers.Add("SOAPAction", @"http://tempuri.org/RecuperarPDF");
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
                                where (el.Name == aw + "RecuperarPDFResult")
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

        #region EMISION

        public Local.Message CrearDocumento(string xml, string url, string version = "")
        {
            Local.Message result = new Local.Message();

            try
            {
                string url_ws = string.Empty;
                if (version.Equals("39") || version.Equals("41"))
                {
                    url_ws = url + @"/WSBolElect/BolElect.asmx?WSDL";
                    HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url_ws);
                    webRequest.Headers.Add("SOAPAction", @"http://tempuri.org/GenerarBoletaElectronica");
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
                                    where (el.Name == aw + "GenerarBoletaElectronicaResult")
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
                else
                {
                    url_ws = url + @"/WSFactElect/FactElect.asmx?WSDL";
                    HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url_ws);
                    webRequest.Headers.Add("SOAPAction", @"http://tempuri.org/GenerarDocumentoElectronico");
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
                                    where (el.Name == aw + "GenerarDocumentoElectronicoResult")
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

            }
            catch (Exception ex)
            {
                result.Mensaje = ex.Message;
                result.Success = false;
                return result;
            }
        }

        public Local.Message ObtenerPDF(string xml, string url, string version = "")
        {
            Local.Message result = new Local.Message();

            try
            {
                string url_ws = url + @"/WSFactElect/FactElect.asmx?WSDL";
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url_ws);
                webRequest.Headers.Add("SOAPAction", @"http://tempuri.org/RecuperarPDF");
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
                                where (el.Name == aw + "RecuperarPDFResult")
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

        public Local.Message ObtenerEstado(string xml, string url)
        {
            Local.Message result = new Local.Message();

            try
            {
                string url_ws = url + @"/WSFactElect/FactElect.asmx?WSDL";
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url_ws);
                webRequest.Headers.Add("SOAPAction", @"http://tempuri.org/ConsultarEstadoDocumento");
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
                                where (el.Name == aw + "ConsultarEstadoDocumentoResult")
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

        #endregion
    }
}
