using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM.Framework;
using ProvDTE = Soindus.Interfaces.ProveedoresDTE;
using Newtonsoft.Json;

namespace Soindus.AddOnEmisionDTE
{
    public class ProveedorDTE
    {
        public string Proveedor { get; set; }
        public Local.Respuesta Respuesta { get; set; }
        public Local.RespuestaPDF RespuestaPDF { get; set; }
        public Local.RespuestaPDFCloud RespuestaPDFCloud { get; set; }
        private static Local.Configuracion ExtConf = new Local.Configuracion();
        private string RutSociedad = ExtConf.Parametros.Rut_Emisor;

        public ProveedorDTE()
        {
            ExtConf = new Local.Configuracion();
            Respuesta = new Local.Respuesta();
            RespuestaPDF = new Local.RespuestaPDF();
            RespuestaPDFCloud = new Local.RespuestaPDFCloud();
            switch (ExtConf.Parametros.Proveedor_FE)
            {
                case "SO":
                    Proveedor = "SoindusFE";
                    break;
                case "FEB":
                    Proveedor = "Febos";
                    break;
                case "DBN-EV":
                    Proveedor = "DBNet-EV";
                    break;
                case "DBN-SO":
                    Proveedor = "DBNet-SO";
                    break;
                case "DBN-EC":
                    Proveedor = "DBNet-EC";
                    break;
                case "FAC":
                    Proveedor = "Facele";
                    break;
                case "AZU":
                    Proveedor = "Azurian";
                    break;
                case "SID":
                    Proveedor = "Sidge";
                    break;
                default:
                    Proveedor = "SoindusFE";
                    break;
            }
        }

        public ProvDTE.Local.Message CrearDocumento(string[] args)
        {
            ProvDTE.Local.Message result = new ProvDTE.Local.Message();
            switch (Proveedor)
            {
                case "SoindusFE":
                    try
                    {
                        SoindusFE.XML_Design xml_design = new SoindusFE.XML_Design();
                        string xml = xml_design.Design(args[0], args[1], args[2]);

                        var provSoindusFE = new ProvDTE.SoindusFE();
                        var provResultSoindusFE = provSoindusFE.CrearDocumento(xml, ExtConf.Parametros.Url_WS_DTE, args[1]);
                        if (provResultSoindusFE.Success)
                        {
                            if (args[1].Equals("39") || args[1].Equals("41"))
                            {
                                var _Datos = Newtonsoft.Json.JsonConvert.DeserializeObject<SoindusFE.RootGenerarBoletaElectronica>(provResultSoindusFE.Content);
                                var _Datos2 = _Datos.Root.GenerarBoletaElectronicaResult;
                                var SO_Respuesta = _Datos2;
                                if (SO_Respuesta.Resultado.Equals("false"))
                                {
                                    Respuesta.Codigo = SO_Respuesta.Error.Numero;
                                    Respuesta.Mensajes = SO_Respuesta.Error.Mensaje;
                                    provResultSoindusFE.Mensaje = string.Format("Crear DTE - Código de respuesta: {0}-{1}", Respuesta.Codigo, Respuesta.Mensajes);
                                    provResultSoindusFE.Success = false;
                                }
                                else
                                {
                                    //Respuesta.Folio = SO_Respuesta.Resultado;
                                    Respuesta.Folio = args[0];
                                }
                            }
                            else
                            {
                                var _Datos = Newtonsoft.Json.JsonConvert.DeserializeObject<SoindusFE.RootGenerarDocumentoElectronico>(provResultSoindusFE.Content);
                                var _Datos2 = _Datos.Root.GenerarDocumentoElectronicoResult;
                                var SO_Respuesta = _Datos2;
                                if (SO_Respuesta.Resultado.Equals("false"))
                                {
                                    Respuesta.Codigo = SO_Respuesta.Error.Numero;
                                    Respuesta.Mensajes = SO_Respuesta.Error.Mensaje;
                                    provResultSoindusFE.Mensaje = string.Format("Crear DTE - Código de respuesta: {0}-{1}", Respuesta.Codigo, Respuesta.Mensajes);
                                    provResultSoindusFE.Success = false;
                                }
                                else
                                {
                                    //Respuesta.Folio = SO_Respuesta.Resultado;
                                    Respuesta.Folio = args[0];
                                }
                            }
                        }
                        result = provResultSoindusFE;
                    }
                    catch (Exception ex)
                    {
                        result.Mensaje = ex.Message;
                        result.Success = false;
                    }
                    break;
                case "DBNet-EV":
                    try
                    {
                        DBNet.XML_Design xml_design = new DBNet.XML_Design();
                        string xml = xml_design.Design(args[0], args[1], args[2]);

                        var provDTEDBNet = new ProvDTE.DBNet();
                        var provResultDBNet = provDTEDBNet.CrearDocumento(xml, ExtConf.Parametros.Url_WS_DTE);
                        if (provResultDBNet.Success)
                        {
                            var _Datos = Newtonsoft.Json.JsonConvert.DeserializeObject<DBNet.putCustomerETDLoadResponse>(provResultDBNet.Content);
                            var _Datos2 = _Datos.Root.putCustomerETDLoadResult;
                            var DBNetRespuesta = _Datos2;
                            if (!Respuesta.Codigo.Equals("OK"))
                            {
                                Respuesta.Codigo = DBNetRespuesta.Codigo;
                                Respuesta.Mensajes = DBNetRespuesta.Mensajes;
                                provResultDBNet.Mensaje = string.Format("Crear DTE - Código de respuesta: {0}-{1}", Respuesta.Codigo, Respuesta.Mensajes);
                                provResultDBNet.Success = false;
                            }
                            else
                            {
                                Respuesta.Folio = DBNetRespuesta.Folio;
                                Respuesta.Mensajes = DBNetRespuesta.Mensajes;
                            }
                        }
                        result = provResultDBNet;
                    }
                    catch (Exception ex)
                    {
                        result.Mensaje = ex.Message;
                        result.Success = false;
                    }
                    break;
                case "DBNet-SO":
                    try
                    {
                        DBNet0.XML_Design xml_design = new DBNet0.XML_Design();
                        string xml = xml_design.Design(args[0], args[1], args[2]);

                        var provDTEDBNet = new ProvDTE.DBNet();
                        var provResultDBNet = provDTEDBNet.CrearDocumento(xml, ExtConf.Parametros.Url_WS_DTE);
                        if (provResultDBNet.Success)
                        {
                            var _Datos = Newtonsoft.Json.JsonConvert.DeserializeObject<DBNet.putCustomerETDLoadResponse>(provResultDBNet.Content);
                            var _Datos2 = _Datos.Root.putCustomerETDLoadResult;
                            var DBNetRespuesta = _Datos2;
                            if (!Respuesta.Codigo.Equals("OK"))
                            {
                                Respuesta.Codigo = DBNetRespuesta.Codigo;
                                Respuesta.Mensajes = DBNetRespuesta.Mensajes;
                                provResultDBNet.Mensaje = string.Format("Crear DTE - Código de respuesta: {0}-{1}", Respuesta.Codigo, Respuesta.Mensajes);
                                provResultDBNet.Success = false;
                            }
                            else
                            {
                                Respuesta.Folio = DBNetRespuesta.Folio;
                                Respuesta.Mensajes = DBNetRespuesta.Mensajes;
                            }
                        }
                        result = provResultDBNet;
                    }
                    catch (Exception ex)
                    {
                        result.Mensaje = ex.Message;
                        result.Success = false;
                    }
                    break;
                case "DBNet-EC":
                    try
                    {
                        DBNetEC.XML_Design xml_design = new DBNetEC.XML_Design();
                        string xml = xml_design.Design(args[0], args[1], args[2]);

                        var provDTEDBNet = new ProvDTE.DBNet();
                        var provResultDBNet = provDTEDBNet.CrearDocumento(xml, ExtConf.Parametros.Url_WS_DTE, "CLOUD");
                        if (provResultDBNet.Success)
                        {
                            var _Datos = Newtonsoft.Json.JsonConvert.DeserializeObject<DBNet.putCustomerETDLoadResponse>(provResultDBNet.Content);
                            var _Datos2 = _Datos.Root.putCustomerETDLoadResult;
                            var DBNetRespuesta = _Datos2;
                            if (string.IsNullOrEmpty(DBNetRespuesta.Folio) || DBNetRespuesta.Folio.Equals("0"))
                            {
                                Respuesta.Codigo = DBNetRespuesta.Codigo;
                                Respuesta.Mensajes = DBNetRespuesta.Mensajes;
                                provResultDBNet.Mensaje = string.Format("Crear DTE - Código de respuesta: {0}-{1}", Respuesta.Codigo, Respuesta.Mensajes);
                                provResultDBNet.Success = false;
                            }
                            else
                            {
                                Respuesta.Folio = DBNetRespuesta.Folio;
                                Respuesta.Mensajes = DBNetRespuesta.Mensajes;
                            }
                        }
                        result = provResultDBNet;
                    }
                    catch (Exception ex)
                    {
                        result.Mensaje = ex.Message;
                        result.Success = false;
                    }
                    break;
                default:
                    break;
            }
            return result;
        }

        public ProvDTE.Local.Message ObtenerPDF(string[] args)
        {
            ProvDTE.Local.Message result = new ProvDTE.Local.Message();
            switch (Proveedor)
            {
                case "SoindusFE":
                    try
                    {
                        SoindusFE.XML_Design so_xml_design = new SoindusFE.XML_Design();
                        so_xml_design.Rut_Emisor = RutSociedad;
                        so_xml_design.Tipo = args[2];
                        so_xml_design.Folio = args[1];
                        so_xml_design.AmbienteProduccion = ExtConf.Parametros.Prod_Env;
                        so_xml_design.Cedible = "false";
                        string DocId = string.Format("{0}{1}{2}", RutSociedad.ToString().PadLeft(10, '0'), args[2].ToString().PadLeft(3, '0'), args[1].ToString().PadLeft(15, '0'));
                        string so_xml = so_xml_design.DesignRecuperarPDF();

                        var provDTESoindusFE = new ProvDTE.SoindusFE();
                        var provResultSoindusFE = provDTESoindusFE.ObtenerPDFDocumento(so_xml, ExtConf.Parametros.Url_WS_PDF);
                        if (provResultSoindusFE.Success)
                        {
                            var _Datos = Newtonsoft.Json.JsonConvert.DeserializeObject<SoindusFE.RootRecuperarPDF>(provResultSoindusFE.Content);
                            var _Datos2 = _Datos.Root.RecuperarPDFResult;
                            var SO_Respuesta = _Datos2;
                            if (string.IsNullOrEmpty(SO_Respuesta.Resultado) || SO_Respuesta.Resultado.Equals("false"))
                            {
                                RespuestaPDF.String = new string[] { SO_Respuesta.Error.Mensaje };
                                provResultSoindusFE.Mensaje = "No se obtuvo el contenido del archivo PDF";
                                provResultSoindusFE.Success = false;
                            }
                            else
                            {
                                RespuestaPDF.String = new string[] { @"DTE" + DocId + @".pdf", SO_Respuesta.Resultado, "DOK" };
                                // Decodificar el string base 64
                                string origen = RespuestaPDF.String[1];
                                Byte[] datos = Convert.FromBase64String(origen);
                                //origen = Encoding.UTF8.GetString(datos);
                                // Generar archivo temporal
                                SAPbobsCOM.CompanyService com_service = SBO.ConexionSBO.oCompany.GetCompanyService();
                                SAPbobsCOM.PathAdmin oPathAdmin = com_service.GetPathAdmin();
                                String Path = string.Empty;
                                Path = oPathAdmin.AttachmentsFolderPath;
                                int indexPath = Path.LastIndexOf("\\");
                                //if (indexPath > 0)
                                //{
                                //    Path = Path.Substring(0, indexPath);
                                //}
                                Path += @"DTE" + DocId + @".pdf";
                                System.IO.File.WriteAllBytes(Path, datos);
                                string pathaux = @"AttachmentsFolderPath:/DTE" + DocId + @".pdf";
                            }
                        }
                        result = provResultSoindusFE;
                    }
                    catch (Exception ex)
                    {
                        result.Mensaje = ex.Message;
                        result.Success = false;
                    }
                    break;
                case "DBNet-EV":
                    try
                    {
                        DBNet.XML_Design xml_design = new DBNet.XML_Design();
                        string xml = xml_design.DesignPDF(RutSociedad, args[1], args[2], args[3], args[4], args[5]);

                        var provDTEDBNet = new ProvDTE.DBNet();
                        var provResultDBNet = provDTEDBNet.ObtenerPDF(xml, ExtConf.Parametros.Url_WS_DTE);
                        if (provResultDBNet.Success)
                        {
                            var _Datos = Newtonsoft.Json.JsonConvert.DeserializeObject<DBNet.get_pdf_sucursalResponse>(provResultDBNet.Content);
                            var _Datos2 = _Datos.Root.GetPdfSucursalResult;
                            var DBNetRespuestaPDF = _Datos2;
                            if (string.IsNullOrEmpty(DBNetRespuestaPDF.String[1]))
                            {
                                RespuestaPDF.String = DBNetRespuestaPDF.String;
                                provResultDBNet.Mensaje = "No se obtuvo el contenido del archivo PDF";
                                provResultDBNet.Success = false;
                            }
                            else
                            {
                                RespuestaPDF.String = DBNetRespuestaPDF.String;
                                // Decodificar el string base 64
                                string origen = RespuestaPDF.String[1];
                                Byte[] datos = Convert.FromBase64String(origen);
                                //origen = Encoding.UTF8.GetString(datos);
                                //Byte[] datos2 = Convert.FromBase64String(origen);
                                //origen = Encoding.UTF8.GetString(datos2);
                                // Generar archivo temporal
                                SAPbobsCOM.CompanyService com_service = SBO.ConexionSBO.oCompany.GetCompanyService();
                                SAPbobsCOM.PathAdmin oPathAdmin = com_service.GetPathAdmin();
                                String Path = string.Empty;
                                Path = oPathAdmin.AttachmentsFolderPath;
                                int indexPath = Path.LastIndexOf("\\");
                                //if (indexPath > 0)
                                //{
                                //    Path = Path.Substring(0, indexPath);
                                //}
                                Path += @"" + RespuestaPDF.String[0];
                                System.IO.File.WriteAllBytes(Path, datos);
                                string pathaux = @"AttachmentsFolderPath:/" + RespuestaPDF.String[0];
                            }
                        }
                        result = provResultDBNet;
                    }
                    catch (Exception ex)
                    {
                        result.Mensaje = ex.Message;
                        result.Success = false;
                    }
                    break;
                case "DBNet-SO":
                    try
                    {
                        DBNet0.XML_Design xml_design = new DBNet0.XML_Design();
                        string xml = xml_design.DesignPDF(RutSociedad, args[1], args[2], args[3], args[4], args[5]);

                        var provDTEDBNet = new ProvDTE.DBNet();
                        var provResultDBNet = provDTEDBNet.ObtenerPDF(xml, ExtConf.Parametros.Url_WS_DTE);
                        if (provResultDBNet.Success)
                        {
                            var _Datos = Newtonsoft.Json.JsonConvert.DeserializeObject<DBNet.get_pdf_sucursalResponse>(provResultDBNet.Content);
                            var _Datos2 = _Datos.Root.GetPdfSucursalResult;
                            var DBNetRespuestaPDF = _Datos2;
                            if (string.IsNullOrEmpty(RespuestaPDF.String[1]))
                            {
                                RespuestaPDF.String = DBNetRespuestaPDF.String;
                                provResultDBNet.Mensaje = "No se obtuvo el contenido del archivo PDF";
                                provResultDBNet.Success = false;
                            }
                            else
                            {
                                RespuestaPDF.String = DBNetRespuestaPDF.String;
                                // Decodificar el string base 64
                                string origen = RespuestaPDF.String[1];
                                Byte[] datos = Convert.FromBase64String(origen);
                                //origen = Encoding.UTF8.GetString(datos);
                                //Byte[] datos2 = Convert.FromBase64String(origen);
                                //origen = Encoding.UTF8.GetString(datos2);
                                // Generar archivo temporal
                                SAPbobsCOM.CompanyService com_service = SBO.ConexionSBO.oCompany.GetCompanyService();
                                SAPbobsCOM.PathAdmin oPathAdmin = com_service.GetPathAdmin();
                                String Path = string.Empty;
                                Path = oPathAdmin.AttachmentsFolderPath;
                                int indexPath = Path.LastIndexOf("\\");
                                //if (indexPath > 0)
                                //{
                                //    Path = Path.Substring(0, indexPath);
                                //}
                                Path += @"" + RespuestaPDF.String[0];
                                System.IO.File.WriteAllBytes(Path, datos);
                                string pathaux = @"AttachmentsFolderPath:/" + RespuestaPDF.String[0];
                            }
                        }
                        result = provResultDBNet;
                    }
                    catch (Exception ex)
                    {
                        result.Mensaje = ex.Message;
                        result.Success = false;
                    }
                    break;
                case "DBNet-EC":
                    try
                    {
                        DBNetEC.XML_Design xml_design = new DBNetEC.XML_Design();
                        string xml = xml_design.DesignPDF(RutSociedad, args[1], args[2], args[3], args[4], args[5]);

                        var provDTEDBNet = new ProvDTE.DBNet();
                        var provResultDBNet = provDTEDBNet.ObtenerPDF(xml, ExtConf.Parametros.Url_WS_PDF, "CLOUD");
                        if (provResultDBNet.Success)
                        {
                            var _Datos = Newtonsoft.Json.JsonConvert.DeserializeObject<DBNet.get_pdfResponse>(provResultDBNet.Content);
                            var _Datos2 = _Datos.Root.GetPdfResult;
                            var DBNetRespuestaPDFCloud = _Datos2;
                            RespuestaPDF = new Local.RespuestaPDF();
                            RespuestaPDF.String = _Datos2.String;
                            if (string.IsNullOrEmpty(DBNetRespuestaPDFCloud.String[1]))
                            {
                                RespuestaPDFCloud.String = DBNetRespuestaPDFCloud.String;
                                provResultDBNet.Mensaje = "No se obtuvo el contenido del archivo PDF";
                                provResultDBNet.Success = false;
                            }
                            else
                            {
                                RespuestaPDFCloud.String = DBNetRespuestaPDFCloud.String;
                                // Decodificar el string base 64
                                string origen = RespuestaPDFCloud.String[1];
                                Byte[] datos = Convert.FromBase64String(origen);
                                //origen = Encoding.UTF8.GetString(datos);
                                //Byte[] datos2 = Convert.FromBase64String(origen);
                                //origen = Encoding.UTF8.GetString(datos2);
                                // Generar archivo temporal
                                SAPbobsCOM.CompanyService com_service = SBO.ConexionSBO.oCompany.GetCompanyService();
                                SAPbobsCOM.PathAdmin oPathAdmin = com_service.GetPathAdmin();
                                String Path = string.Empty;
                                Path = oPathAdmin.AttachmentsFolderPath;
                                int indexPath = Path.LastIndexOf("\\");
                                //if (indexPath > 0)
                                //{
                                //    Path = Path.Substring(0, indexPath);
                                //}
                                Path += @"" + RespuestaPDFCloud.String[0];
                                System.IO.File.WriteAllBytes(Path, datos);
                                string pathaux = @"AttachmentsFolderPath:/" + RespuestaPDFCloud.String[0];
                            }
                        }
                        result = provResultDBNet;
                    }
                    catch (Exception ex)
                    {
                        result.Mensaje = ex.Message;
                        result.Success = false;
                    }
                    break;
                default:
                    break;
            }
            return result;
        }
    }
}
