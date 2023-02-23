using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProvDTE = Soindus.Interfaces.ProveedoresDTE;

namespace Soindus.Svc.Base64DTE
{
    public class ProveedorDTE
    {
        public string Proveedor { get; set; }
        public DBNet.putCustomerETDLoadResult Respuesta { get; set; }
        public DBNet.GetPdfSucursalResult RespuestaPDF { get; set; }
        private static Local.Configuracion ExtConf = new Local.Configuracion();
        private string RutSociedad = ExtConf.Parametros.Rut_Emisor;

        public ProveedorDTE()
        {
            ExtConf = new Local.Configuracion();
            switch (ExtConf.Parametros.Proveedor_FE)
            {
                case "DBN-EV":
                    Proveedor = "DBNet-EV";
                    break;
                case "DBN-SO":
                    Proveedor = "DBNet-SO";
                    break;
                default:
                    Proveedor = "DBNet-SO";
                    break;
            }
        }

        public ProvDTE.Local.Message ObtenerPDF(string[] args)
        {
            ProvDTE.Local.Message result = new ProvDTE.Local.Message();
            switch (Proveedor)
            {
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
                            RespuestaPDF = _Datos2;
                            if (string.IsNullOrEmpty(RespuestaPDF.String[1]))
                            {
                                provResultDBNet.Mensaje = "No se obtuvo el contenido del archivo PDF";
                                provResultDBNet.Success = false;
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
                            RespuestaPDF = _Datos2;
                            if (string.IsNullOrEmpty(RespuestaPDF.String[1]))
                            {
                                provResultDBNet.Mensaje = "No se obtuvo el contenido del archivo PDF";
                                provResultDBNet.Success = false;
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
