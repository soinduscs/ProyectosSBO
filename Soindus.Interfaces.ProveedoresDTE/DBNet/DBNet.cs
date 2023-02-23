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
    public class DBNet
    {
        public DBNet()
        {
        }

        #region RECEPCION

        public Local.Message ObtenerDocumentos(string xml, string url)
        {
            Local.Message result = new Local.Message();

            try
            {
                string url_ws = url + @"/wssSupplierETD/wssSupplierETD.asmx?WSDL";
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url_ws);
                webRequest.Headers.Add("SOAPAction", @"http://www.dbnet.cl/wssSupplierETD/RescataListadoDTE");
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
                            XNamespace aw = "http://www.dbnet.cl/wssSupplierETD";
                            XElement newTree = new XElement("root",
                                from el in xDocument.Descendants()
                                where (el.Name == aw + "RescataListadoDTEResult")
                                select el
                            );

                            //foreach (var node in newTree.DescendantsAndSelf())
                            //{
                            //    node.Name = node.Name.LocalName;
                            //}

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
                string url_ws = url + @"/wssSupplierETD/wssSupplierETD.asmx?WSDL";
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url_ws);
                webRequest.Headers.Add("SOAPAction", @"http://www.dbnet.cl/wssSupplierETD/RescataXMLDTE");
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
                            XNamespace aw = "http://www.dbnet.cl/wssSupplierETD";
                            XElement newTree = new XElement("root",
                                from el in xDocument.Descendants()
                                where (el.Name == aw + "RescataXMLDTEResult")
                                select el
                            );

                            //foreach (var node in newTree.DescendantsAndSelf())
                            //{
                            //    node.Name = node.Name.LocalName;
                            //}

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
                string url_ws = url + @"/wssSupplierETDPdf/wsssupplierETDPdf.asmx?WSDL";
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url_ws);
                webRequest.Headers.Add("SOAPAction", @"http://www.dbnet.cl/wssSupplierETDPDF/ObtenerPDF");
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
                            XNamespace aw = "http://www.dbnet.cl/wssSupplierETDPDF";
                            XElement newTree = new XElement("root",
                                from el in xDocument.Descendants()
                                where (el.Name == aw + "ObtenerPDFResult")
                                select el
                            );

                            //foreach (var node in newTree.DescendantsAndSelf())
                            //{
                            //    node.Name = node.Name.LocalName;
                            //}

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
                string url_ws = url + @"/wssSupplierETDBusinessStateASP/SupplierETDBusinessStateASP.asmx?WSDL";
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url_ws);
                webRequest.Headers.Add("SOAPAction", @"http://www.dbnet.cl/SupplierETDBusinessStateASP/setSupplierETDBusinessState");
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
                            XNamespace aw = "http://www.dbnet.cl/SupplierETDBusinessStateASP";
                            XElement newTree = new XElement("root",
                                from el in xDocument.Descendants()
                                where (el.Name == aw + "setSupplierETDBusinessStateResult")
                                select el
                            );

                            //foreach (var node in newTree.DescendantsAndSelf())
                            //{
                            //    node.Name = node.Name.LocalName;
                            //}

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

        public Local.Message CambiarEstadoSII(string xml, string url)
        {
            Local.Message result = new Local.Message();

            try
            {
                string url_ws = url + @"/wssSupplierETDRejection/SupplierETDRejection.asmx?WSDL";
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url_ws);
                webRequest.Headers.Add("SOAPAction", @"http://www.dbnet.cl/SupplierETDRejection/setRejection");
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
                            XNamespace aw = "http://www.dbnet.cl/SupplierETDRejection";
                            XElement newTree = new XElement("root",
                                from el in xDocument.Descendants()
                                where (el.Name == aw + "setRejectionResult")
                                select el
                            );

                            //foreach (var node in newTree.DescendantsAndSelf())
                            //{
                            //    node.Name = node.Name.LocalName;
                            //}

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

        public Local.Message ActualizarEstadoDTE(string xml, string url)
        {
            Local.Message result = new Local.Message();

            try
            {
                string url_ws = url + @"/wssSupplierETD/wssSupplierETD.asmx?WSDL";
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url_ws);
                webRequest.Headers.Add("SOAPAction", @"http://www.dbnet.cl/wssSupplierETD/ActualizaEstadoDTE");
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
                            XNamespace aw = "http://www.dbnet.cl/wssSupplierETD";
                            XElement newTree = new XElement("root",
                                from el in xDocument.Descendants()
                                where (el.Name == aw + "ActualizaEstadoDTEResult")
                                select el
                            );

                            //foreach (var node in newTree.DescendantsAndSelf())
                            //{
                            //    node.Name = node.Name.LocalName;
                            //}

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
                if (version.Equals("CLOUD"))
                {
                    url_ws = url + @"/wssCustomerETDLoadASPFolio/CustomerETDLoadASP.asmx?WSDL";
                }
                else
                {
                    url_ws = url + @"/wssCustomerETDLoadASP/CustomerETDLoadASP.asmx?WSDL";
                }
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url_ws);
                webRequest.Headers.Add("SOAPAction", @"http://www.dbnet.cl/CustomerETDLoadASP/putCustomerETDLoad");
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
                            XNamespace aw = "http://www.dbnet.cl/CustomerETDLoadASP";
                            XElement newTree = new XElement("root",
                                from el in xDocument.Descendants()
                                where (el.Name == aw + "putCustomerETDLoadResult")
                                select el
                            );

                            //foreach (var node in newTree.DescendantsAndSelf())
                            //{
                            //    node.Name = node.Name.LocalName;
                            //}

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

        public Local.Message ObtenerPDF(string xml, string url, string version = "")
        {
            Local.Message result = new Local.Message();

            try
            {
                string url_ws = string.Empty;
                if (version.Equals("CLOUD"))
                {
                    url_ws = url + @"/wssCustomerETDPDF/CustomerETDPDF.asmx?WSDL";
                }
                else
                {
                    url_ws = url + @"/wssCustomerETDPDF/getPDF64.asmx?WSDL";
                }
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url_ws);
                if (version.Equals("CLOUD"))
                {
                    webRequest.Headers.Add("SOAPAction", @"http://www.dbnet.cl/getPDF64/get_pdf");
                }
                else
                {
                    webRequest.Headers.Add("SOAPAction", @"DBNET/get_pdf_sucursal");
                }
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
                            XElement newTree;
                            if (version.Equals("CLOUD"))
                            {
                                XNamespace aw = "http://www.dbnet.cl/getPDF64";
                                newTree = new XElement("root",
                                    from el in xDocument.Descendants()
                                    where (el.Name == aw + "get_pdfResult")
                                    select el
                                );
                            }
                            else
                            {
                                XNamespace aw = "DBNET";
                                newTree = new XElement("root",
                                    from el in xDocument.Descendants()
                                    where (el.Name == aw + "get_pdf_sucursalResult")
                                    select el
                                );
                            }

                            //foreach (var node in newTree.DescendantsAndSelf())
                            //{
                            //    node.Name = node.Name.LocalName;
                            //}

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
                string url_ws = url + @"/wssConsultaEstadoASP/ConsultaEstado.asmx?WSDL";
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url_ws);
                webRequest.Headers.Add("SOAPAction", @"http://www.dbnet.cl/ConsultaEstado/ConsultaEstado");
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
                            XNamespace aw = "http://www.dbnet.cl/ConsultaEstado";
                            XElement newTree = new XElement("root",
                                from el in xDocument.Descendants()
                                where (el.Name == aw + "ConsultaEstadoResult")
                                select el
                            );

                            //foreach (var node in newTree.DescendantsAndSelf())
                            //{
                            //    node.Name = node.Name.LocalName;
                            //}

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
