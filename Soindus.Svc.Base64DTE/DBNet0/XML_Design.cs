using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Microsoft.Web.Services3.Security.Tokens;

namespace Soindus.Svc.Base64DTE.DBNet0
{
    public class XML_Design
    {
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
            XNode Node = JsonConvert.DeserializeXNode(json, "get_pdf_sucursal");

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
            XNamespace dbn = "DBNET";
            XNamespace soapenv = "http://schemas.xmlsoap.org/soap/envelope/";
            XNamespace wsse = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd";
            XNamespace wsu = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd";


            XElement docum = XElement.Parse(Node.ToString());
            //docum.Add(new XAttribute(XNamespace.Xmlns + "cus", cus));

            foreach (XElement item in docum.DescendantsAndSelf())
            {
                item.Name = dbn + item.Name.LocalName;
            }


            XElement root = new XElement(soapenv + "Envelope",
                new XAttribute(XNamespace.Xmlns + "soapenv", "http://schemas.xmlsoap.org/soap/envelope/"),
                new XAttribute(XNamespace.Xmlns + "dbn", "DBNET"),
                new XElement(soapenv + "Header" //,
                                                //new XElement(wsse + "Security",
                                                //new XAttribute(XNamespace.Xmlns + "wsu", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd"),
                                                //new XAttribute(XNamespace.Xmlns + "wsse", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd"),
                                                //new XElement(wsse + "UsernameToken",
                                                //new XAttribute(wsu + "Id", UsernameToken),
                                                //new XElement(wsse + "Username", Username),
                                                //new XElement(wsse + "Password", Password, new XAttribute("Type", Type)),
                                                //new XElement(wsse + "Nonce", Nonce, new XAttribute("EncodingType", EncodingType)),
                                                //new XElement(wsu + "Created", Created)
                                                //)
                                                //)
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
