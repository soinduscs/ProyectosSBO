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

namespace Soindus.Interfaces.PortadoresTC
{
    public class BCentral
    {
        public string Token { get; set; }
        private string User;
        private string Pass;
        private string FechaIni;
        private string FechaFin;

        public BCentral()
        {
        }

        private void DescomponerToken()
        {
            string origen = Token;
            var data = origen.Split(';');
            User = data[0];
            Pass = data[1];
        }

        public Local.Message ObtenerTipoCambiario(string FechaInicial = "", string FechaFinal = "", string Serie = "")
        {
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
                // Por defecto trae el día actual
                dt = new DateTime(DateTime.Now.Year, DateTime.Today.Month, DateTime.Today.Day);
                Mes = (dt.Month.ToString().Length == 1) ? dt.Month.ToString().PadLeft(2, '0') : dt.Month.ToString();
                Dia = (dt.Day.ToString().Length == 1) ? dt.Day.ToString().PadLeft(2, '0') : dt.Day.ToString();

                FechaFin = String.Format("{0}-{1}-{2}", dt.Year.ToString(), Mes, Dia);
                FechaIni = String.Format("{0}-{1}-{2}", dt.Year.ToString(), Mes, Dia);
            }

            Local.Message result = new Local.Message();
            DescomponerToken();
            //User = "141362690";
            //Pass = "DAS0m5GU";

            try
            {
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(@"https://si3.bcentral.cl/SieteWS/sietews.asmx?wsdl");
                webRequest.Headers.Add("SOAPAction", @"http://bancocentral.org/GetSeries");
                webRequest.ContentType = "text/xml;charset=\"utf-8\"";
                webRequest.Accept = "text/xml";
                webRequest.Method = "POST";

                string strEnvelop = @"";
                strEnvelop += @"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" ";
                strEnvelop += @"xmlns:ban=""http://bancocentral.org/"">";
                strEnvelop += @" <soapenv:Header/>";
                strEnvelop += @" <soapenv:Body>";
                strEnvelop += @" <ban:GetSeries>";
                strEnvelop += @"  <ban:user>" + User + @"</ban:user>";
                strEnvelop += @"  <ban:password>" + Pass + @"</ban:password>";
                strEnvelop += @"  <ban:firstDate>" + FechaIni + @"</ban:firstDate>";
                strEnvelop += @"  <ban:lastDate>" + FechaFin + @"</ban:lastDate>";
                strEnvelop += @"  <ban:seriesIds>";
                strEnvelop += @"   <ban:string>" + Serie + @"</ban:string>";
                strEnvelop += @"  </ban:seriesIds>";
                strEnvelop += @" </ban:GetSeries>";
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
                            XNamespace aw = "http://bancocentral.org/";
                            XElement newTree = new XElement("root",
                                from el in xDocument.Descendants()
                                where (el.Name == aw + "GetSeriesResult")
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
    }
}
