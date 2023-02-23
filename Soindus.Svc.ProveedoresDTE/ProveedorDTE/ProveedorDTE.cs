using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Threading.Tasks;
using ProvDTE = Soindus.Interfaces.ProveedoresDTE;
using ClasesDTE = Soindus.Clases.DTE;

namespace Soindus.Svc.ProveedoresDTE
{
    public class ProveedorDTE
    {
        public string Proveedor { get; set; }
        public ClasesDTE.DTEResponse DTEResponse { get; set; }
        public ClasesDTE.DetalleDocuDTE DetalleDocuDTE { get; set; }
        private static Local.Configuracion ExtConf = new Local.Configuracion();

        public ProveedorDTE()
        {
            ExtConf = new Local.Configuracion();
            switch (ExtConf.Parametros.Proveedor_FE)
            {
                case "FEB":
                    Proveedor = "Febos";
                    break;
                case "DBN":
                    Proveedor = "DBNet";
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
                    Proveedor = "Febos";
                    break;
            }
        }

        /// <summary>
        /// Obtiene los documentos según filtros aplicados
        /// (Febos: TipoDocumento, RutReceptor, RutEmisor, FechaInicial, FechaFinal, Pagina)
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public ProvDTE.Local.Message ObtenerDocumentos(string[] args)
        {
            string Token = string.Empty;
            string RutEmpresa = string.Empty;

            string FiltroTipo = args[0];
            string RutReceptorDTE = args[1];
            string FiltroSN = args[2];
            string DesdeFecha = args[3];
            string HastaFecha = args[4];
            string Pagina = args[5];

            ProvDTE.Local.Message result = new Interfaces.ProveedoresDTE.Local.Message();
            switch (Proveedor)
            {
                case "Febos":
                    Token = ExtConf.Parametros.Token;
                    RutEmpresa = ExtConf.Parametros.Rut_Receptor;

                    var provDTEFebos = new ProvDTE.Febos() { Token = Token, RutEmpresa = RutEmpresa };
                    var provResultFebos = provDTEFebos.ObtenerDocumentos(FiltroTipo, RutReceptorDTE, FiltroSN, DesdeFecha, HastaFecha, Pagina);
                    if (provResultFebos.Success)
                    {
                        //var _Cabecera = Newtonsoft.Json.JsonConvert.DeserializeObject<Clases.CabeceraResponse>(result.Content);
                        var _Datos = Newtonsoft.Json.JsonConvert.DeserializeObject<Febos.DTEResponse>(provResultFebos.Content);
                        var _DatosAux = Newtonsoft.Json.JsonConvert.DeserializeObject<ClasesDTE.DTEResponse>(Newtonsoft.Json.JsonConvert.SerializeObject(_Datos));
                        DTEResponse = new ClasesDTE.DTEResponse();
                        DTEResponse = _DatosAux;
                    }
                    result = provResultFebos;
                    break;
                case "DBNet":
                    DBNet.XML_Design xml_design = new DBNet.XML_Design();
                    string xml = xml_design.DesignListadoDTE();

                    var provDTEDBNet = new ProvDTE.DBNet();
                    var provResultDBNet = provDTEDBNet.ObtenerDocumentos(xml, xml_design.Url_WS_DTE);
                    if (provResultDBNet.Success)
                    {
                        var _Datos = Newtonsoft.Json.JsonConvert.DeserializeObject<DBNet.RootDTEResponse>(provResultDBNet.Content);
                        var _Datos2 = _Datos.Root;

                        if (_Datos2.DTEResponse.Documentos != null)
                        {
                            DTEResponse = new ClasesDTE.DTEResponse();
                            DTEResponse.Documentos = new List<ClasesDTE.DocuDTE>();
                            DTEResponse.TotalElementos = 0;
                            DTEResponse.TotalPaginas = 1;

                            foreach (var item in _Datos2.DTEResponse.Documentos.Documento)
                            {
                                //string[] valores = { item.RutEmisor, item.Tipo.ToString(), item.Folio.ToString() };
                                string[] valores = { item.DocId };
                                var provResultDBNetAux = ObtenerDocumento(valores);
                                if (provResultDBNetAux.Success)
                                {
                                    var _Datos3 = Newtonsoft.Json.JsonConvert.DeserializeObject<DBNet.RootXMLDTEResponse>(provResultDBNetAux.Content);
                                    if (_Datos3.Root.Documento != null)
                                    {
                                        var _Datos4 = _Datos3.Root.Documento;
                                        var _DatosAux = Newtonsoft.Json.JsonConvert.DeserializeObject<ClasesDTE.DocuDTE>(Newtonsoft.Json.JsonConvert.SerializeObject(_Datos4));

                                        string DecodeString = _Datos4.XmlData;
                                        XmlDocument doc = new XmlDocument();
                                        doc.LoadXml(DecodeString);
                                        var newjson = Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc);
                                        var settings = new Newtonsoft.Json.JsonSerializerSettings();
                                        settings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                                        var _Documento = Newtonsoft.Json.JsonConvert.DeserializeObject<ClasesDTE.JsonDTE>(newjson, settings);
                                        var objDTE = _Documento.DTE.Documento;
                                        _DatosAux.FormaDePago = int.Parse(string.IsNullOrEmpty(objDTE.Encabezado.IdDoc.FmaPago) ? "0" : objDTE.Encabezado.IdDoc.FmaPago);
                                        _DatosAux.RazonSocialEmisor = objDTE.Encabezado.Emisor.RznSoc;
                                        _DatosAux.RazonSocialReceptor = objDTE.Encabezado.Receptor.RznSocRecep;

                                        DTEResponse.Documentos.Add(_DatosAux);
                                        DTEResponse.TotalElementos++;
                                    }
                                }

                                //xml_design.Rut_Emisor = item.RutEmisor;
                                //xml_design.Tipo = item.Tipo.ToString();
                                //xml_design.Folio = item.Folio.ToString();

                                //xml = xml_design.DesignXMLDTE();
                                //var provResultDBNetAux = provDTEDBNet.ObtenerDocumento(xml, xml_design.Url_WS_DTE);
                                //if (provResultDBNetAux.Success)
                                //{
                                //    var _Datos3 = Newtonsoft.Json.JsonConvert.DeserializeObject<DBNet.RootXMLDTEResponse>(provResultDBNetAux.Content);
                                //    if (_Datos3.Root.Documento != null)
                                //    {
                                //        var _DatosAux = Newtonsoft.Json.JsonConvert.DeserializeObject<ClasesDTE.DocuDTE>(Newtonsoft.Json.JsonConvert.SerializeObject(_Datos3.Root.Documento));

                                //        DTEResponse.Documentos.Add(_DatosAux);
                                //        DTEResponse.TotalElementos++;
                                //    }
                                //}
                            }
                        }
                    }
                    result = provResultDBNet;
                    break;
                case "Facele":
                    //string User = "0cc713a13";
                    //string Pass = "mWUX8WQOfuexV9CLWaNFSA==";
                    Token = ExtConf.Parametros.Token;
                    RutEmpresa = ExtConf.Parametros.Rut_Receptor;

                    var provDTEFacele = new ProvDTE.Facele() { Token = Token, RutEmpresa = RutEmpresa };
                    //////var provResultFacele = provDTEFacele.ObtenerDocumentos(FiltroTipo, RutReceptorDTE, FiltroSN, DesdeFecha, HastaFecha, Pagina);
                    //////if (provResultFacele.Success)
                    //////{
                    //////    var _Datos = Newtonsoft.Json.JsonConvert.DeserializeObject<Facele.RootDTEResponse>(provResultFacele.Content);
                    //////    var _Datos2 = _Datos.Root;
                    //////    var _DatosAux = Newtonsoft.Json.JsonConvert.DeserializeObject<ClasesDTE.DTEResponse>(Newtonsoft.Json.JsonConvert.SerializeObject(_Datos2));
                    //////    DTEResponse = new ClasesDTE.DTEResponse();
                    //////    DTEResponse = _DatosAux;
                    //////}

                    ProvDTE.Local.Message provResultFacele = new ProvDTE.Local.Message();
                    DTEResponse = new ClasesDTE.DTEResponse();
                    DTEResponse.Documentos = new List<ClasesDTE.DocuDTE>();
                    DTEResponse.TotalElementos = 0;
                    DTEResponse.TotalPaginas = 1;
                    if (string.IsNullOrEmpty(FiltroTipo))
                    {
                        string stipos = "33;34;52;56;61";
                        var tipos = stipos.Split(';');
                        foreach (var tipo in tipos)
                        {
                            bool continuar = true;
                            int indice = 0;
                            while (continuar)
                            {
                                continuar = false;
                                provResultFacele = provDTEFacele.ObtenerDocumentos(tipo, RutReceptorDTE, FiltroSN, DesdeFecha, HastaFecha, indice.ToString());
                                if (provResultFacele.Success)
                                {
                                    var _Datos = Newtonsoft.Json.JsonConvert.DeserializeObject<Facele.RootDTEResponse>(provResultFacele.Content);
                                    var _Datos2 = _Datos.Root;
                                    var _DatosAux = Newtonsoft.Json.JsonConvert.DeserializeObject<ClasesDTE.DTEResponse>(Newtonsoft.Json.JsonConvert.SerializeObject(_Datos2));
                                    if (_DatosAux.Documentos != null)
                                    {
                                        DTEResponse.Documentos.AddRange(_DatosAux.Documentos);
                                        DTEResponse.TotalElementos += _DatosAux.Documentos.Count;
                                        indice += _DatosAux.Documentos.Count;
                                        if (indice <= _DatosAux.TotalElementos)
                                        {
                                            continuar = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        bool continuar = true;
                        int indice = 0;
                        while (continuar)
                        {
                            continuar = false;
                            provResultFacele = provDTEFacele.ObtenerDocumentos(FiltroTipo, RutReceptorDTE, FiltroSN, DesdeFecha, HastaFecha, indice.ToString());
                            if (provResultFacele.Success)
                            {
                                var _Datos = Newtonsoft.Json.JsonConvert.DeserializeObject<Facele.RootDTEResponse>(provResultFacele.Content);
                                var _Datos2 = _Datos.Root;
                                var _DatosAux = Newtonsoft.Json.JsonConvert.DeserializeObject<ClasesDTE.DTEResponse>(Newtonsoft.Json.JsonConvert.SerializeObject(_Datos2));
                                if (_DatosAux.Documentos != null)
                                {
                                    DTEResponse.Documentos.AddRange(_DatosAux.Documentos);
                                    DTEResponse.TotalElementos += _DatosAux.Documentos.Count;
                                    indice += _DatosAux.Documentos.Count;
                                    if (indice <= _DatosAux.TotalElementos)
                                    {
                                        continuar = true;
                                    }
                                }
                            }
                        }
                    }
                    result = provResultFacele;
                    break;
                default:
                    result.Success = false;
                    result.Mensaje = "Proveedor DTE no implementado.";
                    break;
            }
            return result;
        }

        /// <summary>
        /// Obtiene los documentos emitidos según filtros aplicados
        /// (Febos: TipoDocumento, RutEmisor, RutReceptor, FechaInicial, FechaFinal, Pagina)
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public ProvDTE.Local.Message ObtenerDocumentosEmitidos(string[] args)
        {
            ProvDTE.Local.Message result = new Interfaces.ProveedoresDTE.Local.Message();
            switch (Proveedor)
            {
                case "Febos":
                    string Token = ExtConf.Parametros.Token;
                    string RutEmpresa = ExtConf.Parametros.Rut_Receptor;

                    string FiltroTipo = args[0];
                    string RutEmisorDTE = args[1];
                    string FiltroSN = args[2];
                    string DesdeFecha = args[3];
                    string HastaFecha = args[4];
                    string Pagina = args[5];

                    var provDTE = new ProvDTE.Febos() { Token = Token, RutEmpresa = RutEmpresa };
                    var provResult = provDTE.ObtenerDocumentosEmitidos(FiltroTipo, RutEmisorDTE, FiltroSN, DesdeFecha, HastaFecha, Pagina);
                    if (provResult.Success)
                    {
                        //var _Cabecera = Newtonsoft.Json.JsonConvert.DeserializeObject<Clases.CabeceraResponse>(result.Content);
                        var _Datos = Newtonsoft.Json.JsonConvert.DeserializeObject<Febos.DTEResponse>(provResult.Content);
                        var _DatosAux = Newtonsoft.Json.JsonConvert.DeserializeObject<ClasesDTE.DTEResponse>(Newtonsoft.Json.JsonConvert.SerializeObject(_Datos));
                        DTEResponse = new ClasesDTE.DTEResponse();
                        DTEResponse = _DatosAux;
                    }
                    result = provResult;
                    break;
                case "DBNet":
                    result.Success = false;
                    result.Mensaje = "Proveedor DTE no implementado.";
                    break;
                default:
                    result.Success = false;
                    result.Mensaje = "Proveedor DTE no implementado.";
                    break;
            }
            return result;
        }

        /// <summary>
        /// Obtiene el documento específico
        /// (Febos: FebosId)
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public ProvDTE.Local.Message ObtenerDocumento(string[] args)
        {
            string Token = string.Empty;
            string RutEmpresa = string.Empty;

            string DocId = args[0];

            ProvDTE.Local.Message result = new Interfaces.ProveedoresDTE.Local.Message();
            switch (Proveedor)
            {
                case "Febos":
                    Token = ExtConf.Parametros.Token;
                    RutEmpresa = ExtConf.Parametros.Rut_Receptor;

                    var provDTEFebos = new ProvDTE.Febos() { Token = Token, RutEmpresa = RutEmpresa };
                    var provResultFebos = provDTEFebos.ObtenerDocumento(DocId);
                    if (provResultFebos.Success)
                    {
                        var _Datos = Newtonsoft.Json.JsonConvert.DeserializeObject<Febos.DetalleDocuDTE>(provResultFebos.Content);
                        var _DatosAux = Newtonsoft.Json.JsonConvert.DeserializeObject<ClasesDTE.DetalleDocuDTE>(Newtonsoft.Json.JsonConvert.SerializeObject(_Datos));
                        DetalleDocuDTE = new ClasesDTE.DetalleDocuDTE();
                        // Decodificar el string base 64
                        Byte[] datos = Convert.FromBase64String(_Datos.XmlData);
                        Encoding iso = Encoding.GetEncoding("ISO-8859-1");
                        _DatosAux.XmlData = iso.GetString(datos);
                        DetalleDocuDTE = _DatosAux;
                    }
                    result = provResultFebos;
                    break;
                case "DBNet":
                    DBNet.XML_Design xml_design = new DBNet.XML_Design();
                    xml_design.Rut_Emisor = DocId.Substring(0, 10).TrimStart('0');
                    xml_design.Tipo = DocId.Substring(10, 3).TrimStart('0');
                    xml_design.Folio = DocId.Substring(13, 15).TrimStart('0');
                    string xml = xml_design.DesignXMLDTE();

                    var provDTEDBNet = new ProvDTE.DBNet();
                    var provResultDBNet = provDTEDBNet.ObtenerDocumento(xml, xml_design.Url_WS_DTE);
                    if (provResultDBNet.Success)
                    {
                        var _Datos = Newtonsoft.Json.JsonConvert.DeserializeObject<DBNet.RootXMLDTEResponse>(provResultDBNet.Content);
                        var _Datos2 = _Datos.Root.Documento;
                        var _DatosAux = Newtonsoft.Json.JsonConvert.DeserializeObject<ClasesDTE.DetalleDocuDTE>(Newtonsoft.Json.JsonConvert.SerializeObject(_Datos2));
                        DetalleDocuDTE = new ClasesDTE.DetalleDocuDTE();
                        DetalleDocuDTE = _DatosAux;
                    }
                    result = provResultDBNet;
                    break;
                case "Facele":
                    //string User = "0cc713a13";
                    //string Pass = "mWUX8WQOfuexV9CLWaNFSA==";
                    Token = ExtConf.Parametros.Token;
                    RutEmpresa = ExtConf.Parametros.Rut_Receptor;

                    var provDTEFacele = new ProvDTE.Facele() { Token = Token, RutEmpresa = RutEmpresa };
                    var provResultFacele = provDTEFacele.ObtenerDocumento(DocId);
                    if (provResultFacele.Success)
                    {
                        var _Datos = Newtonsoft.Json.JsonConvert.DeserializeObject<Facele.RootDetalleDocuDTE>(provResultFacele.Content);
                        var _Datos2 = _Datos.Root;
                        var _DatosAux = Newtonsoft.Json.JsonConvert.DeserializeObject<ClasesDTE.DetalleDocuDTE>(Newtonsoft.Json.JsonConvert.SerializeObject(_Datos2));
                        DetalleDocuDTE = new ClasesDTE.DetalleDocuDTE();
                        DetalleDocuDTE = _DatosAux;
                    }
                    result = provResultFacele;
                    break;
                default:
                    result.Success = false;
                    result.Mensaje = "Proveedor DTE no implementado.";
                    break;
            }
            return result;
        }

        /// <summary>
        /// Obtiene y visualiza el PDF del documento correspondiente según parametros.
        /// (Febos: FebosId)
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public ProvDTE.Local.Message ObtenerPDFDocumento(string[] args)
        {
            string Token = string.Empty;
            string RutEmpresa = string.Empty;

            string DocId = args[0];

            ProvDTE.Local.Message result = new Interfaces.ProveedoresDTE.Local.Message();
            switch (Proveedor)
            {
                case "Febos":
                    Token = ExtConf.Parametros.Token;
                    RutEmpresa = ExtConf.Parametros.Rut_Receptor;

                    var provDTE = new ProvDTE.Febos() { Token = Token, RutEmpresa = RutEmpresa };
                    var provResult = provDTE.ObtenerPDFDocumento(DocId);
                    if (provResult.Success)
                    {
                        var _Datos = Newtonsoft.Json.JsonConvert.DeserializeObject<Febos.DetalleDocuDTE>(provResult.Content);
                        var _DatosAux = Newtonsoft.Json.JsonConvert.DeserializeObject<ClasesDTE.DetalleDocuDTE>(Newtonsoft.Json.JsonConvert.SerializeObject(_Datos));
                        DetalleDocuDTE = new ClasesDTE.DetalleDocuDTE();
                        DetalleDocuDTE = _DatosAux;
                    }
                    result = provResult;
                    break;
                case "DBNet":
                    DBNet.XML_Design xml_design = new DBNet.XML_Design();
                    xml_design.Rut_Emisor = DocId.Substring(0, 10).TrimStart('0');
                    xml_design.Tipo = DocId.Substring(10, 3).TrimStart('0');
                    xml_design.Folio = DocId.Substring(13, 15).TrimStart('0');
                    string xml = xml_design.DesignObtenerPDF();

                    var provDTEDBNet = new ProvDTE.DBNet();
                    var provResultDBNet = provDTEDBNet.ObtenerPDFDocumento(xml, xml_design.Url_WS_PDF);
                    if (provResultDBNet.Success)
                    {
                        var _Datos = Newtonsoft.Json.JsonConvert.DeserializeObject<DBNet.RootObtenerPDF>(provResultDBNet.Content);
                        var _Datos2 = _Datos.Root.ObtenerPDFResult;
                        var _DatosAux = Newtonsoft.Json.JsonConvert.DeserializeObject<ClasesDTE.DetalleDocuDTE>(Newtonsoft.Json.JsonConvert.SerializeObject(_Datos2));
                        _DatosAux.DocId = DocId;
                        DetalleDocuDTE = new ClasesDTE.DetalleDocuDTE();
                        // Decodificar el string base 64
                        string origen = _Datos2.ImagenLink;
                        Byte[] datos = Convert.FromBase64String(origen);
                        origen = Encoding.UTF8.GetString(datos);
                        //Byte[] datos2 = Convert.FromBase64String(origen);
                        //origen = Encoding.UTF8.GetString(datos2);
                        // Generar archivo temporal
                        SAPbobsCOM.CompanyService com_service = SBO.ConexionDIAPI.oCompany.GetCompanyService();
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
                        _DatosAux.ImagenLink = pathaux;
                        DetalleDocuDTE = _DatosAux;
                    }
                    result = provResultDBNet;
                    break;
                case "Facele":
                    //string User = "0cc713a13";
                    //string Pass = "mWUX8WQOfuexV9CLWaNFSA==";
                    Token = ExtConf.Parametros.Token;
                    RutEmpresa = ExtConf.Parametros.Rut_Receptor;

                    var provDTEFacele = new ProvDTE.Facele() { Token = Token, RutEmpresa = RutEmpresa };
                    var provResultFacele = provDTEFacele.ObtenerPDFDocumento(DocId);
                    if (provResultFacele.Success)
                    {
                        var _Datos = Newtonsoft.Json.JsonConvert.DeserializeObject<Facele.RootDetalleDocuDTE>(provResultFacele.Content);
                        var _Datos2 = _Datos.Root;
                        var _DatosAux = Newtonsoft.Json.JsonConvert.DeserializeObject<ClasesDTE.DetalleDocuDTE>(Newtonsoft.Json.JsonConvert.SerializeObject(_Datos2));
                        DetalleDocuDTE = new ClasesDTE.DetalleDocuDTE();
                        // Decodificar el string base 64
                        string origen = _Datos2.ImagenLink;
                        Byte[] datos = Convert.FromBase64String(origen);
                        origen = Encoding.UTF8.GetString(datos);
                        Byte[] datos2 = Convert.FromBase64String(origen);
                        origen = Encoding.UTF8.GetString(datos2);
                        // Generar archivo temporal
                        SAPbobsCOM.CompanyService com_service = SBO.ConexionDIAPI.oCompany.GetCompanyService();
                        SAPbobsCOM.PathAdmin oPathAdmin = com_service.GetPathAdmin();
                        String Path = string.Empty;
                        Path = oPathAdmin.AttachmentsFolderPath;
                        int indexPath = Path.LastIndexOf("\\");
                        //if (indexPath > 0)
                        //{
                        //    Path = Path.Substring(0, indexPath);
                        //}
                        Path += @"DTE" + DocId + @".pdf";
                        System.IO.File.WriteAllBytes(Path, datos2);
                        string pathaux = @"AttachmentsFolderPath:/DTE" + DocId + @".pdf";
                        _DatosAux.ImagenLink = pathaux;
                        DetalleDocuDTE = _DatosAux;
                    }
                    result = provResultFacele;
                    break;
                default:
                    result.Success = false;
                    result.Mensaje = "Proveedor DTE no implementado.";
                    break;
            }
            return result;
        }

        /// <summary>
        /// Obtiene y visualiza el PDF del documento correspondiente según parametros.
        /// (Febos: FebosId, Estado, Motivo)
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public ProvDTE.Local.Message CambiarEstadoComercial(string[] args)
        {
            string Token = string.Empty;
            string RutEmpresa = string.Empty;
            string Recinto = string.Empty;

            string DocId = args[0];
            string Estado = args[1];
            string Motivo = args[2];

            ProvDTE.Local.Message result = new Interfaces.ProveedoresDTE.Local.Message();
            switch (Proveedor)
            {
                case "Febos":
                    Token = ExtConf.Parametros.Token;
                    RutEmpresa = ExtConf.Parametros.Rut_Receptor;
                    Recinto = ExtConf.Parametros.Recinto;

                    var provDTE = new ProvDTE.Febos() { Token = Token, RutEmpresa = RutEmpresa };

                    ProvDTE.Febos.EstadosComerciales _EstadoFebos = (ProvDTE.Febos.EstadosComerciales)Enum.Parse(typeof(ProvDTE.Febos.EstadosComerciales), Estado);

                    var provResultFebos = provDTE.CambiarEstadoComercial(DocId, _EstadoFebos, Recinto, Motivo);

                    if (provResultFebos.Success)
                    {
                        var _Datos = Newtonsoft.Json.JsonConvert.DeserializeObject<Febos.DetalleDocuDTE>(provResultFebos.Content);
                        var _DatosAux = Newtonsoft.Json.JsonConvert.DeserializeObject<ClasesDTE.DetalleDocuDTE>(Newtonsoft.Json.JsonConvert.SerializeObject(_Datos));
                        DetalleDocuDTE = new ClasesDTE.DetalleDocuDTE();
                        DetalleDocuDTE = _DatosAux;
                    }
                    result = provResultFebos;
                    break;
                case "DBNet":
                    Token = ExtConf.Parametros.Token;
                    RutEmpresa = ExtConf.Parametros.Rut_Receptor;
                    Recinto = ExtConf.Parametros.Recinto;

                    DBNet.XML_Design xml_design = new DBNet.XML_Design();
                    xml_design.Rut_Receptor = RutEmpresa;
                    xml_design.Rut_Emisor = Local.Rut.ObtenerRutConGuion(DocId.Substring(0, 10).TrimStart('0'));
                    xml_design.Tipo = DocId.Substring(10, 3).TrimStart('0');
                    xml_design.Folio = DocId.Substring(13, 15).TrimStart('0');

                    ProvDTE.DBNet.EstadosComerciales _EstadoDBNet = (ProvDTE.DBNet.EstadosComerciales)Enum.Parse(typeof(ProvDTE.DBNet.EstadosComerciales), Estado);
                    string _estado = string.Empty;
                    switch (_EstadoDBNet)
                    {
                        case ProvDTE.DBNet.EstadosComerciales.Aceptacion_Recibo_Mercaderias:
                            if (string.IsNullOrEmpty(Motivo))
                            {
                                _estado = "ERM";
                            }
                            else
                            {
                                _estado = "ERM";
                            }
                            break;
                        case ProvDTE.DBNet.EstadosComerciales.Rechazo_Comercial:
                            _estado = "RCD";
                            break;
                        default:
                            _estado = "RCD";
                            break;
                    }
                    string xml = xml_design.DesignCambiarEstadoSII(_estado);

                    var provDTEDBNet = new ProvDTE.DBNet();
                    var provResultDBNet = provDTEDBNet.CambiarEstadoSII(xml, xml_design.Url_WS_ECOM);
                    if (provResultDBNet.Success)
                    {
                        var _Datos = Newtonsoft.Json.JsonConvert.DeserializeObject<DBNet.RootsetRejection>(provResultDBNet.Content);
                        if (_Datos.Root.setRejectionResult.Codigo.Equals("DOK"))
                        {
                            provResultDBNet.Mensaje = _Datos.Root.setRejectionResult.Mensaje;
                        }
                    }
                    result = provResultDBNet;
                    break;
                case "Facele":
                    //string User = "0cc713a13";
                    //string Pass = "mWUX8WQOfuexV9CLWaNFSA==";
                    Token = ExtConf.Parametros.Token;
                    RutEmpresa = ExtConf.Parametros.Rut_Receptor;
                    Recinto = ExtConf.Parametros.Recinto;

                    var provDTEFacele = new ProvDTE.Facele() { Token = Token, RutEmpresa = RutEmpresa };

                    ProvDTE.Facele.EstadosComerciales _EstadoFacele = (ProvDTE.Facele.EstadosComerciales)Enum.Parse(typeof(ProvDTE.Facele.EstadosComerciales), Estado);

                    var provResultFacele = provDTEFacele.CambiarEstadoComercial(DocId, _EstadoFacele, Recinto, Motivo); ;
                    if (provResultFacele.Success)
                    {
                        var _Datos = Newtonsoft.Json.JsonConvert.DeserializeObject<Facele.RootDetalleDocuDTE>(provResultFacele.Content);
                        var _Datos2 = _Datos.Root;
                        var _DatosAux = Newtonsoft.Json.JsonConvert.DeserializeObject<ClasesDTE.DetalleDocuDTE>(Newtonsoft.Json.JsonConvert.SerializeObject(_Datos2));
                        DetalleDocuDTE = new ClasesDTE.DetalleDocuDTE();
                        DetalleDocuDTE = _DatosAux;
                    }
                    result = provResultFacele;
                    break;
                default:
                    result.Success = false;
                    result.Mensaje = "Proveedor DTE no implementado.";
                    break;
            }
            return result;
        }

        public ProvDTE.Local.Message ActualizaEstadoDTE(string[] args)
        {
            string Token = string.Empty;
            string RutEmpresa = string.Empty;

            string DocId = args[0];

            ProvDTE.Local.Message result = new Interfaces.ProveedoresDTE.Local.Message();
            switch (Proveedor)
            {
                case "Febos":
                    result.Success = false;
                    result.Mensaje = "Proveedor DTE no implementado.";
                    break;
                case "DBNet":
                    DBNet.XML_Design xml_design = new DBNet.XML_Design();
                    xml_design.Rut_Emisor = DocId.Substring(0, 10).TrimStart('0');
                    xml_design.Tipo = DocId.Substring(10, 3).TrimStart('0');
                    xml_design.Folio = DocId.Substring(13, 15).TrimStart('0');
                    string xml = xml_design.DesignActualizaEstadoDTE();

                    var provDTEDBNet = new ProvDTE.DBNet();
                    var provResultDBNet = provDTEDBNet.ActualizarEstadoDTE(xml, xml_design.Url_WS_DTE);
                    if (provResultDBNet.Success)
                    {
                        var _Datos = Newtonsoft.Json.JsonConvert.DeserializeObject<DBNet.RootActualizaEstadoDTE>(provResultDBNet.Content);
                        if (_Datos.Root.ActualizaEstadoDTEResult.Codigo.Equals("DOK"))
                        {
                            provResultDBNet.Mensaje = _Datos.Root.ActualizaEstadoDTEResult.Mensajes;
                        }
                    }
                    result = provResultDBNet;
                    break;
                case "Facele":
                    result.Success = false;
                    result.Mensaje = "Proveedor DTE no implementado.";
                    break;
                default:
                    result.Success = false;
                    result.Mensaje = "Proveedor DTE no implementado.";
                    break;
            }
            return result;
        }
    }
}
