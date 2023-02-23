using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.Web.Services3.Security.Tokens;

namespace Soindus.AddOnMonitorProveedores.DBNet
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
        public string Tipo = string.Empty;
        public string Folio = string.Empty;

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

        public string DesignListadoDTE()
        {
            string retorno = string.Empty;

            get_RescataListadoDTE rescatalistadodte = null;
            XML_RescataListadoDTE xml_rescatalistadodte = new XML_RescataListadoDTE();
            var rut = Rut_Receptor.Split('-');
            string rutreceptor = rut[0];
            rescatalistadodte = xml_rescatalistadodte.Generar(rutreceptor);

            var root = CreateSchemaRescataListadoDTE(rescatalistadodte);
            retorno = root.ToString();

            return retorno;
        }

        public XElement CreateSchemaRescataListadoDTE(get_RescataListadoDTE rescatalistadodte)
        {
            var json = JsonConvert.SerializeObject(rescatalistadodte);
            json = json.Replace(";", " ");
            XNode Node = JsonConvert.DeserializeXNode(json, "RescataListadoDTE");

            UsernameToken t = new UsernameToken(User_WS, Pass_WS, PasswordOption.SendHashed);
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
            XNamespace wss = "http://www.dbnet.cl/wssSupplierETD";
            XNamespace soapenv = "http://schemas.xmlsoap.org/soap/envelope/";
            XNamespace wsse = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd";
            XNamespace wsu = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd";

            XElement docum = XElement.Parse(Node.ToString());
            //docum.Add(new XAttribute(XNamespace.Xmlns + "cus", cus));

            foreach (XElement item in docum.DescendantsAndSelf())
            {
                item.Name = wss + item.Name.LocalName;
            }

            XElement root = new XElement(soapenv + "Envelope",
                new XAttribute(XNamespace.Xmlns + "soapenv", "http://schemas.xmlsoap.org/soap/envelope/"),
                new XAttribute(XNamespace.Xmlns + "wss", "http://www.dbnet.cl/wssSupplierETD"),
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

        public string DesignXMLDTE()
        {
            string retorno = string.Empty;

            get_RescataXMLDTE rescataxmldte = null;
            XML_RescataXMLDTE xml_rescataxmldte = new XML_RescataXMLDTE();
            var rut = Rut_Receptor.Split('-');
            string rutreceptor = rut[0];
            var rut2 = Rut_Emisor.Split('-');
            string rutemisor = rut2[0];
            rescataxmldte = xml_rescataxmldte.Generar(rutreceptor, rutemisor, Tipo, Folio);

            var root = CreateSchemaRescataXMLDTE(rescataxmldte);
            retorno = root.ToString();

            return retorno;
        }

        public XElement CreateSchemaRescataXMLDTE(get_RescataXMLDTE rescataxmldte)
        {
            var json = JsonConvert.SerializeObject(rescataxmldte);
            json = json.Replace(";", " ");
            XNode Node = JsonConvert.DeserializeXNode(json, "RescataXMLDTE");

            UsernameToken t = new UsernameToken(User_WS, Pass_WS, PasswordOption.SendHashed);
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
            XNamespace wss = "http://www.dbnet.cl/wssSupplierETD";
            XNamespace soapenv = "http://schemas.xmlsoap.org/soap/envelope/";
            XNamespace wsse = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd";
            XNamespace wsu = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd";

            XElement docum = XElement.Parse(Node.ToString());
            //docum.Add(new XAttribute(XNamespace.Xmlns + "cus", cus));

            foreach (XElement item in docum.DescendantsAndSelf())
            {
                item.Name = wss + item.Name.LocalName;
            }

            XElement root = new XElement(soapenv + "Envelope",
                new XAttribute(XNamespace.Xmlns + "soapenv", "http://schemas.xmlsoap.org/soap/envelope/"),
                new XAttribute(XNamespace.Xmlns + "wss", "http://www.dbnet.cl/wssSupplierETD"),
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

        public string DesignObtenerPDF()
        {
            string retorno = string.Empty;

            get_ObtenerPDF obtenerpdf = null;
            XML_ObtenerPDF xml_obtenerpdf = new XML_ObtenerPDF();
            var rut = Rut_Receptor.Split('-');
            string rutreceptor = rut[0];
            var rut2 = Rut_Emisor.Split('-');
            string rutemisor = rut2[0];
            obtenerpdf = xml_obtenerpdf.Generar(rutreceptor, rutemisor, Tipo, Folio);

            var root = CreateSchemaObtenerPDF(obtenerpdf);
            retorno = root.ToString();

            return retorno;
        }

        public XElement CreateSchemaObtenerPDF(get_ObtenerPDF obtenerpdf)
        {
            var json = JsonConvert.SerializeObject(obtenerpdf);
            json = json.Replace(";", " ");
            XNode Node = JsonConvert.DeserializeXNode(json, "ObtenerPDF");

            UsernameToken t = new UsernameToken(User_WS, Pass_WS, PasswordOption.SendHashed);
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
            XNamespace wss = "http://www.dbnet.cl/wssSupplierETDPDF";
            XNamespace soapenv = "http://schemas.xmlsoap.org/soap/envelope/";
            XNamespace wsse = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd";
            XNamespace wsu = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd";

            XElement docum = XElement.Parse(Node.ToString());
            //docum.Add(new XAttribute(XNamespace.Xmlns + "cus", cus));

            foreach (XElement item in docum.DescendantsAndSelf())
            {
                item.Name = wss + item.Name.LocalName;
            }

            XElement root = new XElement(soapenv + "Envelope",
                new XAttribute(XNamespace.Xmlns + "soapenv", "http://schemas.xmlsoap.org/soap/envelope/"),
                new XAttribute(XNamespace.Xmlns + "wss", "http://www.dbnet.cl/wssSupplierETDPDF"),
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

        public string DesignCambiarEstadoComercial(string estado, string motivo)
        {
            string retorno = string.Empty;

            get_CambiarEstadoComercial cambiarestadocomercial = null;
            XML_CambiarEstadoComercial xml_cambiarestadocomercial = new XML_CambiarEstadoComercial();
            var rut = Rut_Receptor.Split('-');
            string rutreceptor = rut[0];
            var rut2 = Rut_Emisor.Split('-');
            string rutemisor = rut2[0];
            cambiarestadocomercial = xml_cambiarestadocomercial.Generar(rutreceptor, rutemisor, Tipo, Folio, estado, motivo);

            var root = CreateSchemaCambiarEstadoComercial(cambiarestadocomercial);
            retorno = root.ToString();

            return retorno;
        }

        public XElement CreateSchemaCambiarEstadoComercial(get_CambiarEstadoComercial cambiarestadocomercial)
        {
            var json = JsonConvert.SerializeObject(cambiarestadocomercial);
            json = json.Replace(";", " ");
            XNode Node = JsonConvert.DeserializeXNode(json, "setSupplierETDBusinessState");

            UsernameToken t = new UsernameToken(User_WS_ECOM, Pass_WS_ECOM, PasswordOption.SendHashed);
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
            XNamespace sup = "http://www.dbnet.cl/SupplierETDBusinessStateASP";
            XNamespace soapenv = "http://schemas.xmlsoap.org/soap/envelope/";
            XNamespace wsse = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd";
            XNamespace wsu = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd";

            XElement docum = XElement.Parse(Node.ToString());
            //docum.Add(new XAttribute(XNamespace.Xmlns + "cus", cus));

            foreach (XElement item in docum.DescendantsAndSelf())
            {
                item.Name = sup + item.Name.LocalName;
            }

            XElement root = new XElement(soapenv + "Envelope",
                new XAttribute(XNamespace.Xmlns + "soapenv", "http://schemas.xmlsoap.org/soap/envelope/"),
                new XAttribute(XNamespace.Xmlns + "sup", "http://www.dbnet.cl/SupplierETDBusinessStateASP"),
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

        public string DesignCambiarEstadoSII(string estado)
        {
            string retorno = string.Empty;

            get_CambiarEstadoSII cambiarestadosii = null;
            XML_CambiarEstadoSII xml_cambiarestadosii = new XML_CambiarEstadoSII();
            //var rut = Rut_Receptor.Split('-');
            //string rutreceptor = rut[0];
            string rutreceptor = Rut_Receptor;
            //var rut2 = Rut_Emisor.Split('-');
            //string rutemisor = rut2[0];
            string rutemisor = Rut_Emisor;
            cambiarestadosii = xml_cambiarestadosii.Generar(rutreceptor, rutemisor, Tipo, Folio, estado);

            var root = CreateSchemaCambiarEstadoSII(cambiarestadosii);
            retorno = root.ToString();

            return retorno;
        }

        public XElement CreateSchemaCambiarEstadoSII(get_CambiarEstadoSII cambiarestadosii)
        {
            var json = JsonConvert.SerializeObject(cambiarestadosii);
            json = json.Replace(";", " ");
            XNode Node = JsonConvert.DeserializeXNode(json, "setRejection");

            UsernameToken t = new UsernameToken(User_WS_ECOM, Pass_WS_ECOM, PasswordOption.SendHashed);
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
            XNamespace sup = "http://www.dbnet.cl/SupplierETDRejection";
            XNamespace soapenv = "http://schemas.xmlsoap.org/soap/envelope/";
            XNamespace wsse = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd";
            XNamespace wsu = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd";

            XElement docum = XElement.Parse(Node.ToString());
            //docum.Add(new XAttribute(XNamespace.Xmlns + "cus", cus));

            foreach (XElement item in docum.DescendantsAndSelf())
            {
                item.Name = sup + item.Name.LocalName;
            }

            XElement root = new XElement(soapenv + "Envelope",
                new XAttribute(XNamespace.Xmlns + "soapenv", "http://schemas.xmlsoap.org/soap/envelope/"),
                new XAttribute(XNamespace.Xmlns + "sup", "http://www.dbnet.cl/SupplierETDRejection"),
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

        public string DesignActualizaEstadoDTE()
        {
            string retorno = string.Empty;

            get_ActualizaEstadoDTE actualizaestadodte = null;
            XML_ActualizaEstadoDTE xml_actualizaestadodte = new XML_ActualizaEstadoDTE();
            var rut = Rut_Receptor.Split('-');
            string rutreceptor = rut[0];
            var rut2 = Rut_Emisor.Split('-');
            string rutemisor = rut2[0];
            actualizaestadodte = xml_actualizaestadodte.Generar(rutreceptor, rutemisor, Tipo, Folio, "TRA");

            var root = CreateSchemaActualizaEstadoDTE(actualizaestadodte);
            retorno = root.ToString();

            return retorno;
        }

        public XElement CreateSchemaActualizaEstadoDTE(get_ActualizaEstadoDTE actualizaestadodte)
        {
            var json = JsonConvert.SerializeObject(actualizaestadodte);
            json = json.Replace(";", " ");
            XNode Node = JsonConvert.DeserializeXNode(json, "ActualizaEstadoDTE");

            UsernameToken t = new UsernameToken(User_WS, Pass_WS, PasswordOption.SendHashed);
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
            XNamespace wss = "http://www.dbnet.cl/wssSupplierETD";
            XNamespace soapenv = "http://schemas.xmlsoap.org/soap/envelope/";
            XNamespace wsse = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd";
            XNamespace wsu = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd";

            XElement docum = XElement.Parse(Node.ToString());
            //docum.Add(new XAttribute(XNamespace.Xmlns + "cus", cus));

            foreach (XElement item in docum.DescendantsAndSelf())
            {
                item.Name = wss + item.Name.LocalName;
            }

            XElement root = new XElement(soapenv + "Envelope",
                new XAttribute(XNamespace.Xmlns + "soapenv", "http://schemas.xmlsoap.org/soap/envelope/"),
                new XAttribute(XNamespace.Xmlns + "wss", "http://www.dbnet.cl/wssSupplierETD"),
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
