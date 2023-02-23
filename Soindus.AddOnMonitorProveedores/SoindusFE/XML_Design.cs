using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Soindus.AddOnMonitorProveedores.SoindusFE
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

        public XML_Design()
        {
            ObtenerParametrosToken();
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
        }

        public string DesignListaResumenDocumento(string FechaIni, string FechaFin)
        {
            string retorno = string.Empty;

            get_ListaResumenDocumento listaresumendocumento = null;
            XML_ListaResumenDocumento xml_listaresumendocumento = new XML_ListaResumenDocumento();
            //var rut = Rut_Receptor.Split('-');
            //string rutreceptor = rut[0];
            listaresumendocumento = xml_listaresumendocumento.Generar(FechaIni, FechaFin);

            var root = CreateSchemaListaResumenDocumento(listaresumendocumento);
            retorno = root.ToString();

            return retorno;
        }

        public XElement CreateSchemaListaResumenDocumento(get_ListaResumenDocumento listaresumendocumento)
        {
            listaresumendocumento.intCodigo = User_WS;
            listaresumendocumento.strClave = Pass_WS;
            var json = JsonConvert.SerializeObject(listaresumendocumento);
            json = json.Replace(";", " ");
            XNode Node = JsonConvert.DeserializeXNode(json, "ListaResumenDocumento");

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

        public string DesignTraerDocumentoXML()
        {
            string retorno = string.Empty;

            get_TraerDocumentoXML traerdocumentoxml = null;
            XML_TraerDocumentoXML xml_traerdocumentoxml = new XML_TraerDocumentoXML();
            traerdocumentoxml = xml_traerdocumentoxml.Generar(Rut_Emisor, Tipo, Folio);

            var root = CreateSchemaTraerDocumentoXML(traerdocumentoxml);
            retorno = root.ToString();

            return retorno;
        }

        public XElement CreateSchemaTraerDocumentoXML(get_TraerDocumentoXML traerdocumentoxml)
        {
            traerdocumentoxml.intCodigo = User_WS;
            traerdocumentoxml.strClave = Pass_WS;
            var json = JsonConvert.SerializeObject(traerdocumentoxml);
            json = json.Replace(";", " ");
            XNode Node = JsonConvert.DeserializeXNode(json, "TraerDocumentoXML");

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

        public string DesignRecuperarPDF()
        {
            string retorno = string.Empty;

            get_RecuperarPDF  recuperarpdf = null;
            XML_RecuperarPDF xml_recuperarpdf = new XML_RecuperarPDF();
            recuperarpdf = xml_recuperarpdf.Generar(Rut_Emisor, Tipo, Folio, AmbienteProduccion, Cedible);

            var root = CreateSchemaRecuperarPDF(recuperarpdf);
            retorno = root.ToString();

            return retorno;
        }

        public XElement CreateSchemaRecuperarPDF(get_RecuperarPDF recuperarpdf)
        {
            recuperarpdf.intCodigo = User_WS;
            recuperarpdf.strClave = Pass_WS;
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

        public string DesignAceptacionRechazo()
        {
            string retorno = string.Empty;

            get_AceptacionRechazo aceptacionrechazo = null;
            XML_AceptacionRechazo xml_aceptacionrechazo = new XML_AceptacionRechazo();
            aceptacionrechazo = xml_aceptacionrechazo.Generar(Rut_Emisor, DV, Tipo, Folio, Accion);

            var root = CreateSchemaAceptacionRechazo(aceptacionrechazo);
            retorno = root.ToString();

            return retorno;
        }

        public XElement CreateSchemaAceptacionRechazo(get_AceptacionRechazo aceptacionrechazo)
        {
            aceptacionrechazo.intCodigo = User_WS;
            aceptacionrechazo.strClave = Pass_WS;
            var json = JsonConvert.SerializeObject(aceptacionrechazo);
            json = json.Replace(";", " ");
            XNode Node = JsonConvert.DeserializeXNode(json, "AceptacionRechazo");

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
    }
}
