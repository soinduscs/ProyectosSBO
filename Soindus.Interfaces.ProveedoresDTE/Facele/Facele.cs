using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Newtonsoft.Json;


namespace Soindus.Interfaces.ProveedoresDTE
{
    public class Facele
    {
        public string Token { get; set; }
        public string RutEmpresa { get; set; }
        private string User;
        private string Pass;
        private string RutEmpresaDecoding;
        private string FiltroTipo;
        private string FiltroSN;
        private string FechaIni;
        private string FechaFin;
        private string FiltroFecha;
        private string Filtros;

        public Facele()
        {
        }

        private void DescomponerToken()
        {
            // Decodificar el string base 64
            string origen = Token;
            Byte[] datos = Convert.FromBase64String(origen);
            origen = Encoding.UTF8.GetString(datos);
            Byte[] datos2 = Convert.FromBase64String(origen);
            origen = Encoding.UTF8.GetString(datos2);
            Byte[] datos3 = Convert.FromBase64String(origen);
            origen = Encoding.UTF8.GetString(datos3);
            var data = origen.Split(';');
            User = data[0];
            Pass = data[1];
            RutEmpresaDecoding = data[2];
        }

        public Local.Message ObtenerDocumentos(string TipoDocumento = "", string RutReceptor = "", string RutEmisor = "", string FechaInicial = "", string FechaFinal = "", string Pagina = "0")
        {
            // Filtro por Tipo de Documento
            if (!String.IsNullOrEmpty(TipoDocumento))
            {
                FiltroTipo = TipoDocumento;
            }
            else
            {
                FiltroTipo = string.Empty;
            }

            // Filtro por Socio de Negocio
            if (!String.IsNullOrEmpty(RutEmisor))
            {
                FiltroSN = RutReceptor;
            }
            else
            {
                FiltroSN = string.Empty;
            }
            FiltroSN = FiltroSN.Replace(".", string.Empty);

            // Filtro por Fechas
            DateTime dt;
            String Mes = String.Empty;
            String Dia = String.Empty;
            // Fechas en formato AAAA-MM-DD
            if (!String.IsNullOrEmpty(FechaInicial) && !String.IsNullOrEmpty(FechaFinal))
            {
                FechaIni = String.Format("{0}-{1}-{2}", FechaInicial.Substring(0, 4), FechaInicial.Substring(4, 2), FechaInicial.Substring(6, 2));
                FechaFin = String.Format("{0}-{1}-{2}", FechaFinal.Substring(0, 4), FechaFinal.Substring(4, 2), FechaFinal.Substring(6, 2));
            }
            else
            {
                // Por defecto trae los últimos 30 días
                dt = new DateTime(DateTime.Now.Year, DateTime.Today.Month, DateTime.Today.Day);
                Mes = (dt.Month.ToString().Length == 1) ? dt.Month.ToString().PadLeft(2, '0') : dt.Month.ToString();
                Dia = (dt.Day.ToString().Length == 1) ? dt.Day.ToString().PadLeft(2, '0') : dt.Day.ToString();

                FechaFin = String.Format("{0}-{1}-{2}", dt.Year.ToString(), Mes, Dia);

                //dt = DateTime.Today.AddDays(-30);
                dt = DateTime.Today.AddDays(-1);
                Mes = (dt.Month.ToString().Length == 1) ? dt.Month.ToString().PadLeft(2, '0') : dt.Month.ToString();
                Dia = (dt.Day.ToString().Length == 1) ? dt.Day.ToString().PadLeft(2, '0') : dt.Day.ToString();

                FechaIni = String.Format("{0}-{1}-{2}", dt.Year.ToString(), Mes, Dia);
            }
            //FiltroFecha = string.Format("fechaEmision:{0}--{1}|", FechaIni, FechaFin);

            //Filtros = string.Format("{0}{1}{2}estadoSii:0,1,2,3,4,5,6,7,8,9|incompleto:N", FiltroSN, FiltroFecha, FiltroTipo);

            Local.Message result = new Local.Message();
            DescomponerToken();
            if (RutEmpresa != RutEmpresaDecoding)
            {
                result.Mensaje = "Rut de empresa receptora no válido.";
                result.Success = false;
                return result;
            }
            try
            {
                //HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(@"https://caoba.docele.cl:443/DoceleOL-202001/DTEService?wsdl");
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(@"https://canelo.docele.cl/DoceleOL_Auth/DTEService?wsdl");
                webRequest.Headers.Add("SOAPAction", @"http://facele.cl/docele/servicios/DTE/consultar");
                webRequest.Headers.Add("facele.user", User);
                webRequest.Headers.Add("facele.pass", Pass);
                webRequest.ContentType = "text/xml;charset=\"utf-8\"";
                webRequest.Accept = "text/xml";
                webRequest.Method = "POST";

                string strEnvelop = @"";
                strEnvelop += @"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" ";
                strEnvelop += @"xmlns:dte=""http://facele.cl/docele/servicios/DTE/"">";
                strEnvelop += @" <soapenv:Header/>";
                strEnvelop += @" <soapenv:Body>";
                strEnvelop += @" <dte:consultar>";
                strEnvelop += @"  <rutAbonado>" + RutEmpresa + @"</rutAbonado>";
                //strEnvelop += @"  <rutAbonado>77673760-7</rutAbonado>";
                //strEnvelop += @"  <userMail>" + Mail + @"</userMail>";
                strEnvelop += @"  <userMail>testing@recepcioncaleta.cl</userMail>";
                if (!string.IsNullOrEmpty(RutEmisor))
                {
                    strEnvelop += @"  <rutContraparte>" + RutEmisor + @"</rutContraparte>";
                }
                //strEnvelop += @"  <rutContraparte>77973320-3</rutContraparte>";
                if (!string.IsNullOrEmpty(FiltroTipo))
                {
                    strEnvelop += @"  <tipoDTE>" + FiltroTipo + @"</tipoDTE>";
                }
                strEnvelop += @"  <fechaEmision>" + FechaIni + @"</fechaEmision>";
                strEnvelop += @"  <fechaEmisionHasta> " + FechaFin + @"</fechaEmisionHasta>";
                strEnvelop += @"  <operacion>RECEPCION</operacion>";
                strEnvelop += @"  <offset>" + Pagina + @"</offset>";
                strEnvelop += @" </dte:consultar>";
                strEnvelop += @" </soapenv:Body>";
                strEnvelop += @"</soapenv:Envelope>";

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

                string soapResult;
                using (WebResponse webResponse = webRequest.EndGetResponse(asyncResult))
                {
                    if (((HttpWebResponse)webResponse).StatusCode == HttpStatusCode.OK)
                    {
                        using (StreamReader rd = new StreamReader(webResponse.GetResponseStream()))
                        {
                            XDocument xDocument = XDocument.Load(rd);
                            XNamespace ns = "http://schemas.xmlsoap.org/soap/envelope";
                            var query = from c in xDocument.Descendants(ns + "consultarResponse") select c;
                            //var query = from c in xDocument.Descendants("registros") select c;

                            XNamespace aw = "http://facele.cl/docele/servicios/DTE/";
                            //XNamespace aw = "http://schemas.xmlsoap.org/soap/envelope/";
                            var query2 = from xx in xDocument.Descendants() where xx.Name.Namespace == aw select xx;

                            XElement newTree = new XElement("root",
                                from el in xDocument.Descendants()
                                where el.Name.Namespace == "" && (el.Name == "registros" || el.Name == "respuesta" || el.Name == "totalDocumentos")
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

        public Local.Message ObtenerDocumento(string DocId = "")
        {
            string RutEmisor = (Convert.ToInt32(DocId.Substring(0, 8))).ToString();
            RutEmisor += DocId.Substring(8, 2);
            string TipoDocumento = (Convert.ToInt32(DocId.Substring(10, 3))).ToString();
            string Folio = (Convert.ToInt32(DocId.Substring(13, 15))).ToString();

            Local.Message result = new Local.Message();
            DescomponerToken();
            if (RutEmpresa != RutEmpresaDecoding)
            {
                result.Mensaje = "Rut de empresa receptora no válido.";
                result.Success = false;
                return result;
            }
            try
            {
                //HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(@"https://caoba.docele.cl:443/DoceleOL-202001/DocumentosRecibidosService");
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(@"https://canelo.docele.cl/DoceleOL_Auth/DocumentosRecibidosService?wsdl");
                webRequest.Headers.Add("SOAPAction", @"http://ws.docele.cl/DocumentosRecibidos/Obtener");
                webRequest.Headers.Add("facele.user", User);
                webRequest.Headers.Add("facele.pass", Pass);
                webRequest.ContentType = "text/xml;charset=\"utf-8\"";
                webRequest.Accept = "text/xml";
                webRequest.Method = "POST";

                string strEnvelop = @"";
                strEnvelop += @"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" ";
                strEnvelop += @"xmlns:doc=""http://ws.docele.cl/DocumentosRecibidos/"">";
                strEnvelop += @" <soapenv:Header/>";
                strEnvelop += @" <soapenv:Body>";
                strEnvelop += @" <doc:Obtener>";
                strEnvelop += @"  <rutContribuyente>" + RutEmpresa + @"</rutContribuyente>";
                strEnvelop += @"  <rutEmisor>" + RutEmisor + @"</rutEmisor>";
                strEnvelop += @"  <tipoDTE>" + TipoDocumento + @"</tipoDTE>";
                strEnvelop += @"  <folioDTE>" + Folio + @"</folioDTE>";
                strEnvelop += @"  <formato>XML</formato>";
                strEnvelop += @" </doc:Obtener>";
                strEnvelop += @" </soapenv:Body>";
                strEnvelop += @"</soapenv:Envelope>";

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

                string soapResult;
                using (WebResponse webResponse = webRequest.EndGetResponse(asyncResult))
                {
                    if (((HttpWebResponse)webResponse).StatusCode == HttpStatusCode.OK)
                    {
                        using (StreamReader rd = new StreamReader(webResponse.GetResponseStream()))
                        {
                            XDocument xDocument = XDocument.Load(rd);
                            XNamespace ns = "http://schemas.xmlsoap.org/soap/envelope";
                            var query = from c in xDocument.Descendants(ns + "ObtenerResponse") select c;
                            //var query = from c in xDocument.Descendants("registros") select c;

                            XNamespace aw = "http://ws.docele.cl/DocumentosRecibidos/";
                            //XNamespace aw = "http://schemas.xmlsoap.org/soap/envelope/";
                            var query2 = from xx in xDocument.Descendants() where xx.Name.Namespace == aw select xx;

                            XElement newTree = new XElement("root",
                                from el in xDocument.Descendants()
                                where el.Name.Namespace == "" && (el.Name == "respuestaOperacion" || el.Name == "XML")
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

        public Local.Message ObtenerPDFDocumento(string DocId = "")
        {
            string RutEmisor = (Convert.ToInt32(DocId.Substring(0, 8))).ToString();
            RutEmisor += DocId.Substring(8, 2);
            string TipoDocumento = (Convert.ToInt32(DocId.Substring(10, 3))).ToString();
            string Folio = (Convert.ToInt32(DocId.Substring(13, 15))).ToString();

            Local.Message result = new Local.Message();
            DescomponerToken();
            if (RutEmpresa != RutEmpresaDecoding)
            {
                result.Mensaje = "Rut de empresa receptora no válido.";
                result.Success = false;
                return result;
            }
            try
            {
                //HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(@"https://caoba.docele.cl:443/DoceleOL-202001/DocumentosRecibidosService");
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(@"https://canelo.docele.cl/DoceleOL_Auth/DocumentosRecibidosService?wsdl");
                webRequest.Headers.Add("SOAPAction", @"http://ws.docele.cl/DocumentosRecibidos/Obtener");
                webRequest.Headers.Add("facele.user", User);
                webRequest.Headers.Add("facele.pass", Pass);
                webRequest.ContentType = "text/xml;charset=\"utf-8\"";
                webRequest.Accept = "text/xml";
                webRequest.Method = "POST";

                string strEnvelop = @"";
                strEnvelop += @"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" ";
                strEnvelop += @"xmlns:doc=""http://ws.docele.cl/DocumentosRecibidos/"">";
                strEnvelop += @" <soapenv:Header/>";
                strEnvelop += @" <soapenv:Body>";
                strEnvelop += @" <doc:Obtener>";
                strEnvelop += @"  <rutContribuyente>" + RutEmpresa + @"</rutContribuyente>";
                strEnvelop += @"  <rutEmisor>" + RutEmisor + @"</rutEmisor>";
                strEnvelop += @"  <tipoDTE>" + TipoDocumento + @"</tipoDTE>";
                strEnvelop += @"  <folioDTE>" + Folio + @"</folioDTE>";
                strEnvelop += @"  <formato>PDF</formato>";
                strEnvelop += @" </doc:Obtener>";
                strEnvelop += @" </soapenv:Body>";
                strEnvelop += @"</soapenv:Envelope>";

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

                string soapResult;
                using (WebResponse webResponse = webRequest.EndGetResponse(asyncResult))
                {
                    if (((HttpWebResponse)webResponse).StatusCode == HttpStatusCode.OK)
                    {
                        using (StreamReader rd = new StreamReader(webResponse.GetResponseStream()))
                        {
                            XDocument xDocument = XDocument.Load(rd);
                            XNamespace ns = "http://schemas.xmlsoap.org/soap/envelope";
                            var query = from c in xDocument.Descendants(ns + "ObtenerResponse") select c;
                            //var query = from c in xDocument.Descendants("registros") select c;

                            XNamespace aw = "http://ws.docele.cl/DocumentosRecibidos/";
                            //XNamespace aw = "http://schemas.xmlsoap.org/soap/envelope/";
                            var query2 = from xx in xDocument.Descendants() where xx.Name.Namespace == aw select xx;

                            XElement newTree = new XElement("root",
                                from el in xDocument.Descendants()
                                where el.Name.Namespace == "" && (el.Name == "respuestaOperacion" || el.Name == "PDF")
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

        public Local.Message CambiarEstadoComercial(string DocId = "", EstadosComerciales Estado = EstadosComerciales.Acuse_Recibo, string Recinto = "Casa Matriz", string Motivo = "")
        {
            string RutEmisor = (Convert.ToInt32(DocId.Substring(0, 8))).ToString();
            RutEmisor += DocId.Substring(8, 2);
            string TipoDocumento = (Convert.ToInt32(DocId.Substring(10, 3))).ToString();
            string Folio = (Convert.ToInt32(DocId.Substring(13, 15))).ToString();

            string accion = string.Empty;
            switch (Estado)
            {
                case EstadosComerciales.Acuse_Recibo:
                    accion = "Por_Aceptar";
                    break;
                case EstadosComerciales.Aceptacion_Comercial:
                    accion = "Acepta_Contenido_Documento";
                    //request.AddQueryParameter("recinto", Recinto);
                    //request.AddQueryParameter("motivo", Motivo);
                    break;
                case EstadosComerciales.Rechazo_Comercial:
                    accion = "Reclamo_Contenido_Documento";
                    //request.AddQueryParameter("recinto", Recinto);
                    //request.AddQueryParameter("motivo", Motivo);
                    break;
                case EstadosComerciales.Aceptacion_Recibo_Mercaderias:
                    accion = "Otorga_Recibo_Mercaderias";
                    //request.AddQueryParameter("recinto", Recinto);
                    //request.AddQueryParameter("motivo", Motivo);
                    break;
                case EstadosComerciales.Reclamo_Parcial:
                    accion = "Reclamo_Falta_Parcial_Mercaderias";
                    break;
                case EstadosComerciales.Reclamo_Total:
                    accion = "Reclamo_Falta_Total_Mercaderias";
                    break;
            }

            Local.Message result = new Local.Message();
            DescomponerToken();
            if (RutEmpresa != RutEmpresaDecoding)
            {
                result.Mensaje = "Rut de empresa receptora no válido.";
                result.Success = false;
                return result;
            }
            try
            {
                //HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(@"https://caoba.docele.cl:443/DoceleOL-202001/DTEService?wsdl");
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(@"https://canelo.docele.cl/DoceleOL_Auth/DTEService?wsdl");
                webRequest.Headers.Add("SOAPAction", @"http://facele.cl/docele/servicios/DTE/aprobarDTERecibidos");
                webRequest.Headers.Add("facele.user", User);
                webRequest.Headers.Add("facele.pass", Pass);
                webRequest.ContentType = "text/xml;charset=\"utf-8\"";
                webRequest.Accept = "text/xml";
                webRequest.Method = "POST";

                string strEnvelop = @"";
                strEnvelop += @"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" ";
                strEnvelop += @"xmlns:dte=""http://facele.cl/docele/servicios/DTE/"">";
                strEnvelop += @" <soapenv:Header/>";
                strEnvelop += @" <soapenv:Body>";
                strEnvelop += @" <dte:aprobarDTERecibidos>";
                strEnvelop += @"  <rutAbonado>" + RutEmpresa + @"</rutAbonado>";
                //strEnvelop += @"  <rutAbonado>77673760-7</rutAbonado>";
                //strEnvelop += @"  <userMail>" + Mail + @"</userMail>";
                strEnvelop += @"  <userMail>testing@recepcioncaleta.cl</userMail>";
                strEnvelop += @"  <aprobacion>";
                strEnvelop += @"   <rutEmisor>" + RutEmisor + @"</rutEmisor>";
                strEnvelop += @"   <tipoDTE>" + TipoDocumento + @"</tipoDTE>";
                strEnvelop += @"   <folio>" + Folio + @"</folio>";
                strEnvelop += @"   <accion>" + accion + @"</accion>";
                strEnvelop += @"  </aprobacion>";
                strEnvelop += @" </dte:aprobarDTERecibidos>";
                strEnvelop += @" </soapenv:Body>";
                strEnvelop += @"</soapenv:Envelope>";

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

                string soapResult;
                using (WebResponse webResponse = webRequest.EndGetResponse(asyncResult))
                {
                    if (((HttpWebResponse)webResponse).StatusCode == HttpStatusCode.OK)
                    {
                        using (StreamReader rd = new StreamReader(webResponse.GetResponseStream()))
                        {
                            XDocument xDocument = XDocument.Load(rd);
                            XNamespace ns = "http://schemas.xmlsoap.org/soap/envelope";
                            var query = from c in xDocument.Descendants(ns + "aprobarDTERecibidosResponse") select c;
                            //var query = from c in xDocument.Descendants("registros") select c;

                            XNamespace aw = "http://facele.cl/docele/servicios/DTE/";
                            //XNamespace aw = "http://schemas.xmlsoap.org/soap/envelope/";

                            var query2 = from xx in xDocument.Descendants() where xx.Name.Namespace == aw select xx;

                            XElement newTree = new XElement("root",
                                from el in xDocument.Descendants()
                                where el.Name.Namespace == "" && (el.Name == "respuesta")
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
    }
}
