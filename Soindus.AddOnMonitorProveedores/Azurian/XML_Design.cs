using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Soindus.AddOnMonitorProveedores.Azurian
{
    public class XML_Design
    {
        public string Proveedor_FE = string.Empty;
        public string Url_WS_DTE = string.Empty;
        public string Url_WS_PDF = string.Empty;
        public string Url_WS_ECOM = string.Empty;
        public string User_WS = string.Empty;
        public string Pass_WS = string.Empty;
        public string User_WS_ECOM = string.Empty;
        public string Pass_WS_ECOM = string.Empty;
        public string Rut_Receptor = string.Empty;
        public string Rut_Emisor = string.Empty;
        public string DV = string.Empty;
        public string Tipo = string.Empty;
        public string Folio = string.Empty;
        public string AmbienteProduccion = string.Empty;
        public string Cedible = string.Empty;
        public string Accion = string.Empty;
        public string Ambiente_Prod = string.Empty;

        public XML_Design()
        {
            ObtenerParametrosToken();
        }

        public string DesignTransferenciaDocumentos(string FechaIni, string FechaFin)
        {
            string retorno = string.Empty;

            get_transferenciaDocumentos transferenciadocumentos = null;
            XML_transferenciaDocumentos xml_transferenciadocumentos = new XML_transferenciaDocumentos();
            transferenciadocumentos = xml_transferenciadocumentos.Generar(FechaIni, FechaFin);

            var root = CreateSchemaTransferenciaDocumentos(transferenciadocumentos);
            retorno = root.ToString();

            return retorno;
        }

        public XElement CreateSchemaTransferenciaDocumentos(get_transferenciaDocumentos transferenciadocumentos)
        {
            transferenciadocumentos.request.wsApiKey = Pass_WS;
            transferenciadocumentos.request.nroResolucion = Ambiente_Prod;
            var rut = Rut_Receptor.Split('-');
            string rutreceptor = rut[0];
            transferenciadocumentos.request.rutEmpresa = rutreceptor;
            var json = JsonConvert.SerializeObject(transferenciadocumentos);
            json = json.Replace(";", " ");
            XNode Node = JsonConvert.DeserializeXNode(json, "transferenciaDocumentos");

            XNamespace con = "http://controller.ws.dte.azurian.com";
            XNamespace req = "http://request.ws.dte.azurian.com";
            XNamespace soapenv = "http://schemas.xmlsoap.org/soap/envelope/";

            XElement docum = XElement.Parse(Node.ToString());

            foreach (XElement item in docum.DescendantsAndSelf())
            {
                if (item.Name.LocalName.Equals("transferenciaDocumentos") || item.Name.LocalName.Equals("request"))
                {
                    item.Name = con + item.Name.LocalName;
                }
                else
                {
                    item.Name = req + item.Name.LocalName;
                }
            }

            XElement root = new XElement(soapenv + "Envelope",
                new XAttribute(XNamespace.Xmlns + "soapenv", "http://schemas.xmlsoap.org/soap/envelope/"),
                new XAttribute(XNamespace.Xmlns + "con", "http://controller.ws.dte.azurian.com"),
                new XAttribute(XNamespace.Xmlns + "req", "http://request.ws.dte.azurian.com"),
                new XElement(soapenv + "Body",
                //new XElement(con + "transferenciaDocumentos",
                //new XElement(con + "request",
                new XElement(docum
                ))
            );

            root.Descendants()
                       .Where(n => (n.IsEmpty || String.IsNullOrWhiteSpace(n.Value)) && n.Name != soapenv + "Header")
                       .Remove();

            return root;
        }

        public string DesignDescargarArchivoDTEXML()
        {
            string retorno = string.Empty;

            get_descargarArchivo descargararchivo = null;
            XML_descargarArchivo xml_descargararchivo = new XML_descargarArchivo();
            descargararchivo = xml_descargararchivo.Generar(Rut_Emisor, Tipo, Folio);

            var root = CreateSchemaDescargarArchivoDTEXML(descargararchivo);
            retorno = root.ToString();

            return retorno;
        }

        public XElement CreateSchemaDescargarArchivoDTEXML(get_descargarArchivo descargararchivo)
        {
            descargararchivo.request.wsApiKey = Pass_WS;
            descargararchivo.request.nroResolucion = Ambiente_Prod;
            var rut = Rut_Receptor.Split('-');
            string rutreceptor = rut[0];
            descargararchivo.request.rutEmpresa = rutreceptor;
            descargararchivo.request.tipoArchivo = "2";
            descargararchivo.request.tipoEntrega = "1";
            var json = JsonConvert.SerializeObject(descargararchivo);
            json = json.Replace(";", " ");
            XNode Node = JsonConvert.DeserializeXNode(json, "descargarArchivo");

            XNamespace con = "http://controller.ws.dte.azurian.com";
            XNamespace req = "http://request.ws.dte.azurian.com";
            XNamespace soapenv = "http://schemas.xmlsoap.org/soap/envelope/";

            XElement docum = XElement.Parse(Node.ToString());

            foreach (XElement item in docum.DescendantsAndSelf())
            {
                if (item.Name.LocalName.Equals("descargarArchivo") || item.Name.LocalName.Equals("request"))
                {
                    item.Name = con + item.Name.LocalName;
                }
                else
                {
                    item.Name = req + item.Name.LocalName;
                }
            }

            XElement root = new XElement(soapenv + "Envelope",
                new XAttribute(XNamespace.Xmlns + "soapenv", "http://schemas.xmlsoap.org/soap/envelope/"),
                new XAttribute(XNamespace.Xmlns + "con", "http://controller.ws.dte.azurian.com"),
                new XAttribute(XNamespace.Xmlns + "req", "http://request.ws.dte.azurian.com"),
                new XElement(soapenv + "Body",
                //new XElement(con + "transferenciaDocumentos",
                //new XElement(con + "request",
                new XElement(docum
                ))
            );

            root.Descendants()
                       .Where(n => (n.IsEmpty || String.IsNullOrWhiteSpace(n.Value)) && n.Name != soapenv + "Header")
                       .Remove();

            return root;
        }

        public string DesignDescargarArchivoPDF()
        {
            string retorno = string.Empty;

            get_descargarArchivo descargararchivo = null;
            XML_descargarArchivo xml_descargararchivo = new XML_descargarArchivo();
            descargararchivo = xml_descargararchivo.Generar(Rut_Emisor, Tipo, Folio);

            var root = CreateSchemaDescargarArchivoPDF(descargararchivo);
            retorno = root.ToString();

            return retorno;
        }

        public XElement CreateSchemaDescargarArchivoPDF(get_descargarArchivo descargararchivo)
        {
            descargararchivo.request.wsApiKey = Pass_WS;
            descargararchivo.request.nroResolucion = Ambiente_Prod;
            var rut = Rut_Receptor.Split('-');
            string rutreceptor = rut[0];
            descargararchivo.request.rutEmpresa = rutreceptor;
            descargararchivo.request.tipoArchivo = "1";
            descargararchivo.request.tipoEntrega = "1";
            var json = JsonConvert.SerializeObject(descargararchivo);
            json = json.Replace(";", " ");
            XNode Node = JsonConvert.DeserializeXNode(json, "descargarArchivo");

            XNamespace con = "http://controller.ws.dte.azurian.com";
            XNamespace req = "http://request.ws.dte.azurian.com";
            XNamespace soapenv = "http://schemas.xmlsoap.org/soap/envelope/";

            XElement docum = XElement.Parse(Node.ToString());

            foreach (XElement item in docum.DescendantsAndSelf())
            {
                if (item.Name.LocalName.Equals("descargarArchivo") || item.Name.LocalName.Equals("request"))
                {
                    item.Name = con + item.Name.LocalName;
                }
                else
                {
                    item.Name = req + item.Name.LocalName;
                }
            }

            XElement root = new XElement(soapenv + "Envelope",
                new XAttribute(XNamespace.Xmlns + "soapenv", "http://schemas.xmlsoap.org/soap/envelope/"),
                new XAttribute(XNamespace.Xmlns + "con", "http://controller.ws.dte.azurian.com"),
                new XAttribute(XNamespace.Xmlns + "req", "http://request.ws.dte.azurian.com"),
                new XElement(soapenv + "Body",
                //new XElement(con + "transferenciaDocumentos",
                //new XElement(con + "request",
                new XElement(docum
                ))
            );

            root.Descendants()
                       .Where(n => (n.IsEmpty || String.IsNullOrWhiteSpace(n.Value)) && n.Name != soapenv + "Header")
                       .Remove();

            return root;
        }

        private void ObtenerParametrosToken()
        {
            Local.Configuracion ExtConf = new Local.Configuracion();
            ConfiguracionParams result = new ConfiguracionParams();
            string Token = ExtConf.Parametros.Token;
            // Decodificar el string base 64
            string origen = Token;
            Byte[] datos = Convert.FromBase64String(origen);
            origen = Encoding.UTF8.GetString(datos);
            result = JsonConvert.DeserializeObject<ConfiguracionParams>(origen);
            Proveedor_FE = result.Proveedor_FE;
            Url_WS_DTE = result.Url_WS_DTE;
            Url_WS_PDF = result.Url_WS_PDF;
            Url_WS_ECOM = result.Url_WS_ECOM;
            User_WS = result.User_WS;
            Pass_WS = result.Pass_WS;
            User_WS_ECOM = result.User_WS_ECOM;
            Pass_WS_ECOM = result.Pass_WS_ECOM;
            Rut_Receptor = result.Rut_Receptor;
            Ambiente_Prod = result.Ambiente_Prod;
        }

        public class ConfiguracionParams
        {
            [JsonProperty("PROVEEDOR_FE")]
            public string Proveedor_FE { get; set; }
            [JsonProperty("URL_WS_DTE")]
            public string Url_WS_DTE { get; set; }
            [JsonProperty("URL_WS_PDF")]
            public string Url_WS_PDF { get; set; }
            [JsonProperty("URL_WS_ECOM")]
            public string Url_WS_ECOM { get; set; }
            [JsonProperty("USER_WS")]
            public string User_WS { get; set; }
            [JsonProperty("PWD_WS")]
            public string Pass_WS { get; set; }
            [JsonProperty("USER_WS_ECOM")]
            public string User_WS_ECOM { get; set; }
            [JsonProperty("PWD_WS_ECOM")]
            public string Pass_WS_ECOM { get; set; }
            [JsonProperty("RUT_RECEPTOR")]
            public string Rut_Receptor { get; set; }
            [JsonProperty("AMBIENTE_PROD")]
            public string Ambiente_Prod { get; set; }
        }
    }
}
