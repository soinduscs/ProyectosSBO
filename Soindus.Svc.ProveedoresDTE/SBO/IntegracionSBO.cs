using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Xml;
using ProvDTE = Soindus.Interfaces.ProveedoresDTE;
using ClasesDTE = Soindus.Clases.DTE;
using Comun = Soindus.Clases.Comun;

namespace Soindus.Svc.ProveedoresDTE
{
    public class IntegracionSBO
    {
        private static Local.Configuracion ExtConf;
        private static ClasesDTE.Documento objDTE = null;
        private static String TipoRef = string.Empty;
        private static String Folio = string.Empty;
        private static String CardCode = string.Empty;
        private static String DocEntryBase = string.Empty;
        private static String DocTypeBase = string.Empty;
        private static String BaseType = string.Empty;
        private static Int32 DocEntryUDO = 0;
        private static String DocId = string.Empty;
        private static String MonedaLocal = string.Empty;
        private static String MonedaDocumento = string.Empty;
        private static String MonedaConta = string.Empty;
        private static bool EsMonExtranjera = false;
        private static string SepDecimal;
        private static string SepMiles;
        private static double TipoCambio = 0;
        private static DateTime FechaEmision;

        public IntegracionSBO()
        {
            ExtConf = new Local.Configuracion();
        }

        public void CargarDocumentosDesdeAPI()
        {
            //Application.SBO_Application.StatusBar.SetText("Descargando información. Espere unos momentos.", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);

            // Datos para api  
            Int64?[] ArraysFolio = new Int64?[] { };
            String FechaFinal = String.Empty;
            String FechaInicial = String.Empty;
            String RutReceptorDTE = String.Empty;
            String TipoDte = String.Empty;
            String FolioDte = String.Empty;
            String FechaEmisDte = String.Empty;
            Int64 lMontoTotalDte = 0;
            String MontoTotalDte = String.Empty;
            String RazonSocialDTE = String.Empty;
            String Estado = String.Empty;
            Int32 NumeroPaginas = 0;
            Int32 NumeroDocumentos = 0;
            Int32 IndexMatrix = 0;
            String DteID = String.Empty;
            String OC = String.Empty;

            // Filtro por Tipo de Documento
            String FiltroTipo = string.Empty;

            // Filtro por Socio de Negocio
            String FiltroSN = string.Empty;

            //RutReceptorDTE = SBO.ConsultasSBO.ObtenerRutEmpresaReceptora();
            RutReceptorDTE = ExtConf.Parametros.Rut_Receptor;

            // Filtro por Fechas
            String DesdeFecha = string.Empty;
            String HastaFecha = string.Empty;

            DateTime dt;
            String Mes = String.Empty;
            String Dia = String.Empty;
            // Por defecto trae los últimos 7 días
            dt = new DateTime(DateTime.Now.Year, DateTime.Today.Month, DateTime.Today.Day);
            Mes = (dt.Month.ToString().Length == 1) ? dt.Month.ToString().PadLeft(2, '0') : dt.Month.ToString();
            Dia = (dt.Day.ToString().Length == 1) ? dt.Day.ToString().PadLeft(2, '0') : dt.Day.ToString();

            HastaFecha = String.Format("{0}{1}{2}", dt.Year.ToString(), Mes, Dia);

            dt = DateTime.Today.AddDays(-7);
            Mes = (dt.Month.ToString().Length == 1) ? dt.Month.ToString().PadLeft(2, '0') : dt.Month.ToString();
            Dia = (dt.Day.ToString().Length == 1) ? dt.Day.ToString().PadLeft(2, '0') : dt.Day.ToString();

            DesdeFecha = String.Format("{0}{1}{2}", dt.Year.ToString(), Mes, Dia);

            System.Globalization.CultureInfo pesosChilenos2decimales = System.Globalization.CultureInfo.CreateSpecificCulture("es-CL");
            pesosChilenos2decimales.NumberFormat.CurrencyDecimalDigits = 2;
            pesosChilenos2decimales.NumberFormat.CurrencySymbol = "$";

            ProveedorDTE proveedorDTE = new ProveedorDTE();

            string[] parametros = new string[] { FiltroTipo, RutReceptorDTE, FiltroSN, DesdeFecha, HastaFecha, "1" };

            var provResult = proveedorDTE.ObtenerDocumentos(parametros);

            if (provResult.Success)
            {
                var _Datos = proveedorDTE.DTEResponse;

                if (_Datos != null && _Datos.Documentos != null)
                {
                    foreach (var item in _Datos.Documentos)
                    {
                        AgregarRegistrosUDOFromObject(item);
                    }

                    for (int i = 2; i <= _Datos.TotalPaginas; i++)
                    {
                        parametros = new string[] { FiltroTipo, RutReceptorDTE, FiltroSN, DesdeFecha, HastaFecha, i.ToString() };
                        provResult = proveedorDTE.ObtenerDocumentos(parametros);

                        if (provResult.Success)
                        {
                            _Datos = proveedorDTE.DTEResponse;

                            if (_Datos != null && _Datos.Documentos != null)
                            {
                                foreach (var item in _Datos.Documentos)
                                {
                                    AgregarRegistrosUDOFromObject(item);
                                }
                            }
                        }
                    }
                }
                //Application.SBO_Application.StatusBar.SetText("Información descargada correctamente.", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success);
            }
            else
            {
                //Application.SBO_Application.StatusBar.SetText(provResult.Mensaje, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);
            }
        }

        private static void AgregarRegistrosUDOFromObject(Clases.DTE.DocuDTE item)
        {
            SAPbobsCOM.GeneralService oGeneralService = null;
            SAPbobsCOM.GeneralData oGeneralData = null;
            SAPbobsCOM.GeneralData oChild = null;
            SAPbobsCOM.GeneralDataCollection oChildren = null;
            SAPbobsCOM.GeneralDataParams oGeneralParams = null;
            SAPbobsCOM.CompanyService oCompanyService = null;

            //Application.SBO_Application.StatusBar.SetText("Importando documentos. Espere unos momentos.", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);

            try
            {
                oCompanyService = SBO.ConexionDIAPI.oCompany.GetCompanyService();
                // Get GeneralService (oCmpSrv is the CompanyService)
                oGeneralService = oCompanyService.GetGeneralService("SO_MONITOR");
                // Create data for new row in main UDO
                oGeneralData = ((SAPbobsCOM.GeneralData)(oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralData)));

                // validación si documento existe
                string docid = string.Empty;
                docid = item.DocId;
                SAPbobsCOM.Recordset oRecord = (SAPbobsCOM.Recordset)SBO.ConexionDIAPI.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                string _query = @"SELECT COUNT(1) AS ""RESP"" FROM ""@SO_MONITOR"" WHERE ""U_DOCID"" = '" + docid + "'";
                oRecord.DoQuery(_query);
                if (!oRecord.EoF)
                {
                    if (oRecord.Fields.Item("RESP").Value.Equals(0))
                    {
                        oGeneralData.SetProperty("U_PLAZO", string.IsNullOrEmpty(item.Plazo.ToString()) ? 0 : item.Plazo);
                        oGeneralData.SetProperty("U_DOCID", item.DocId);
                        oGeneralData.SetProperty("U_TIPODOC", item.TipoDocumento.ToString());
                        oGeneralData.SetProperty("U_FOLIO", item.Folio.ToString());
                        oGeneralData.SetProperty("U_FECHAEM", item.FechaEmision);
                        oGeneralData.SetProperty("U_FORMAPA", string.IsNullOrEmpty(item.FormaDePago.ToString()) ? "0" : item.FormaDePago.ToString());
                        oGeneralData.SetProperty("U_IDETRAS", string.IsNullOrEmpty(item.IndicadorDeTraslado.ToString()) ? "0" : item.IndicadorDeTraslado.ToString());
                        oGeneralData.SetProperty("U_RUTEMIS", Local.Rut.ObtenerRutConGuion(item.RutEmisor));
                        oGeneralData.SetProperty("U_RAZSOCE", item.RazonSocialEmisor);
                        oGeneralData.SetProperty("U_RUTRECE", Local.Rut.ObtenerRutConGuion(item.RutReceptor));
                        oGeneralData.SetProperty("U_RAZSOCR", item.RazonSocialReceptor);
                        oGeneralData.SetProperty("U_CONTACT", string.IsNullOrEmpty(item.Contacto) ? "" : item.Contacto);
                        oGeneralData.SetProperty("U_IVA", string.IsNullOrEmpty(item.Iva.ToString()) ? 0 : item.Iva);
                        oGeneralData.SetProperty("U_MONTOTO", string.IsNullOrEmpty(item.MontoTotal.ToString()) ? 0 : item.MontoTotal);
                        var estadosii = string.IsNullOrEmpty(item.EstadoSii.ToString()) ? 0 : item.EstadoSii;
                        oGeneralData.SetProperty("U_ESTSII", Comun.Enumeradores.GetEstadoSii((Comun.Enumeradores.EstadosSii)estadosii));
                        var estadocomercial = string.IsNullOrEmpty(item.EstadoComercial.ToString()) ? 0 : item.EstadoComercial;
                        oGeneralData.SetProperty("U_ESTCOME", Comun.Enumeradores.GetEstadoComercial((Comun.Enumeradores.EstadosComerciales)estadocomercial));
                        oGeneralData.SetProperty("U_FECHARE", item.FechaRecepcion != null ? item.FechaRecepcion : item.FechaEmision);
                        oGeneralData.SetProperty("U_TIPO", item.Tipo.ToString());
                        oGeneralData.SetProperty("U_CODSII", string.IsNullOrEmpty(item.CodigoSii) ? "" : item.CodigoSii);
                        oGeneralData.SetProperty("U_TIENENC", string.IsNullOrEmpty(item.TieneNc) ? "" : item.TieneNc);
                        oGeneralData.SetProperty("U_TIENEND", string.IsNullOrEmpty(item.TieneNd) ? "" : item.TieneNd);
                        oGeneralData.SetProperty("U_DOCBASE", "");
                        oGeneralData.SetProperty("U_TIPOASO", "");
                        oGeneralData.SetProperty("U_DOCASO", "");
                        oGeneralData.SetProperty("U_RAZONRE", "");
                        oGeneralData.SetProperty("U_ESTADO", "0");
                        oGeneralData.SetProperty("U_PRELIM", "");
                        oGeneralData.SetProperty("U_RUTCES", string.IsNullOrEmpty(item.RutCesionario) ? "" : item.RutCesionario);
                        oGeneralData.SetProperty("U_RAZSOCC", string.IsNullOrEmpty(item.RazonSocialCesionario) ? "" : item.RazonSocialCesionario);
                        if (item.FechaCesion != null)
                        {
                            oGeneralData.SetProperty("U_FECHACE", item.FechaCesion);
                        }
                        string Rut = item.RutEmisor;
                        string empID = string.Empty;
                        string empName = string.Empty;
                        if (!string.IsNullOrEmpty(Rut))
                        {
                            string CardCode = SBO.ConsultasSBO.ObtenerCardcode(Rut);
                            if (!string.IsNullOrEmpty(CardCode))
                            {
                                empID = SBO.ConsultasSBO.ObtenerResponsableSN(CardCode);
                                if (!string.IsNullOrEmpty(empID))
                                {
                                    empName = SBO.ConsultasSBO.ObtenerNombreResponsableSN(empID);
                                    oGeneralData.SetProperty("U_CODRESP", empID);
                                    oGeneralData.SetProperty("U_NOMRESP", empName);
                                }
                            }
                        }
                        else
                        {
                            oGeneralData.SetProperty("U_CODRESP", "");
                            oGeneralData.SetProperty("U_NOMRESP", "");
                        }
                        oGeneralData.SetProperty("U_CODRESPD", "");
                        oGeneralData.SetProperty("U_NOMRESPD", "");

                        // Add the new row, including children, to database
                        oGeneralParams = oGeneralService.Add(oGeneralData);

                        ProveedorDTE proveedorDTE = new ProveedorDTE();
                        string[] parametros = new string[] { docid };
                        var provResult = proveedorDTE.ActualizaEstadoDTE(parametros);
                    }
                }
                Local.FuncionesComunes.LiberarObjetoGenerico(oRecord);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void ValidarDocumentos()
        {
            try
            {
                SAPbobsCOM.GeneralService oGeneralService = null;
                SAPbobsCOM.GeneralData oGeneralData = null;
                SAPbobsCOM.GeneralData oChild = null;
                SAPbobsCOM.GeneralDataCollection oChildren = null;
                SAPbobsCOM.GeneralDataParams oGeneralParams = null;
                SAPbobsCOM.CompanyService oCompanyService = null;

                String Rut = String.Empty;
                String BaseType = String.Empty;
                Int32 Tipo = 0;
                Int64 Folio = 0;
                Int32 DocEntryUDO;
                String OCManual = String.Empty;

                // Filtro por Fechas
                String DesdeFecha = string.Empty;
                String HastaFecha = string.Empty;

                DateTime dt;
                String Mes = String.Empty;
                String Dia = String.Empty;
                // Por defecto trae los últimos 7 días
                dt = new DateTime(DateTime.Now.Year, DateTime.Today.Month, DateTime.Today.Day);
                Mes = (dt.Month.ToString().Length == 1) ? dt.Month.ToString().PadLeft(2, '0') : dt.Month.ToString();
                Dia = (dt.Day.ToString().Length == 1) ? dt.Day.ToString().PadLeft(2, '0') : dt.Day.ToString();

                HastaFecha = String.Format("{0}{1}{2}", dt.Year.ToString(), Mes, Dia);

                dt = DateTime.Today.AddDays(-7);
                Mes = (dt.Month.ToString().Length == 1) ? dt.Month.ToString().PadLeft(2, '0') : dt.Month.ToString();
                Dia = (dt.Day.ToString().Length == 1) ? dt.Day.ToString().PadLeft(2, '0') : dt.Day.ToString();

                DesdeFecha = String.Format("{0}{1}{2}", dt.Year.ToString(), Mes, Dia);

                SAPbobsCOM.Recordset oRecord = (SAPbobsCOM.Recordset)SBO.ConexionDIAPI.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                string _query = @"SELECT * FROM ""@SO_MONITOR"" WHERE ""U_ESTADO"" IN ('0')";
                _query += string.Format(@" AND ""U_FECHAEM"" BETWEEN '{0}' AND '{1}'", DesdeFecha, HastaFecha);
                oRecord.DoQuery(_query);

                while (!oRecord.EoF)
                {
                    DocEntryUDO = Convert.ToInt32(oRecord.Fields.Item("DocEntry").Value);
                    string docId = oRecord.Fields.Item("U_DOCID").Value.ToString();
                    Rut = oRecord.Fields.Item("U_RUTEMIS").Value.ToString();
                    Tipo = Convert.ToInt16(oRecord.Fields.Item("U_TIPODOC").Value.ToString());
                    Folio = Convert.ToInt64(oRecord.Fields.Item("U_FOLIO").Value.ToString());
                    OCManual = oRecord.Fields.Item("U_DOCBASE").Value.ToString();

                    var validacion = ProcesaValidacionGeneral(docId, Rut, Tipo, Folio, OCManual);
                    if (validacion.Success)
                    {
                        string docnum = SBO.ConsultasSBO.RecuperaDocNum(validacion.TpoDocRef, validacion.DocEntryBase, Tipo);
                        BaseType = String.Empty;

                        string empID = string.Empty;
                        string empName = string.Empty;
                        string slpcode = string.Empty;
                        string slpname = string.Empty;

                        if (Tipo.Equals(33) || Tipo.Equals(34) || Tipo.Equals(52) || Tipo.Equals(56) || Tipo.Equals(61))
                        {
                            if (validacion.TpoDocRef.Equals("801"))
                            {
                                BaseType = "22";
                            }
                            else if (validacion.TpoDocRef.Equals("33"))
                            {
                                BaseType = "13";
                            }
                            else if (validacion.TpoDocRef.Equals(""))
                            {
                                BaseType = "";
                            }
                            else
                            {
                                BaseType = "20";
                            }

                            empID = SBO.ConsultasSBO.ObtenerResponsableSN(validacion.CardCode);
                            if (!string.IsNullOrEmpty(empID) && !empID.Equals("0"))
                            {
                                empName = SBO.ConsultasSBO.ObtenerNombreResponsableSN(empID);
                            }

                            slpcode = SBO.ConsultasSBO.ObtenerResponsable(validacion.TpoDocRef, validacion.DocEntryBase, Tipo);
                            if (!string.IsNullOrEmpty(slpcode))
                            {
                                slpname = SBO.ConsultasSBO.ObtenerNombreResponsable(slpcode);
                            }
                        }

                        // Cambiar estado a validado
                        try
                        {
                            oCompanyService = SBO.ConexionDIAPI.oCompany.GetCompanyService();
                            // Get GeneralService (oCmpSrv is the CompanyService)
                            oGeneralService = oCompanyService.GetGeneralService("SO_MONITOR");
                            // Create data for new row in main UDO
                            oGeneralParams = ((SAPbobsCOM.GeneralDataParams)(oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)));
                            //oGeneralData = ((SAPbobsCOM.GeneralData)(oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralData)));
                            oGeneralParams.SetProperty("DocEntry", DocEntryUDO);
                            oGeneralData = oGeneralService.GetByParams(oGeneralParams);
                            oGeneralData.SetProperty("U_DOCBASE", docnum);
                            oGeneralData.SetProperty("U_TIPOASO", BaseType);
                            oGeneralData.SetProperty("U_DOCASO", validacion.DocEntryBase == "0" ? "" : validacion.DocEntryBase);
                            oGeneralData.SetProperty("U_ESTADO", "1");
                            if (!empID.Equals("0") && !string.IsNullOrEmpty(empID))
                            {
                                oGeneralData.SetProperty("U_CODRESP", empID);
                                oGeneralData.SetProperty("U_NOMRESP", empName);
                            }
                            else
                            {
                                oGeneralData.SetProperty("U_CODRESP", 0);
                                oGeneralData.SetProperty("U_NOMRESP", "");
                            }
                            if (!slpcode.Equals("-1") && !string.IsNullOrEmpty(slpcode))
                            {
                                oGeneralData.SetProperty("U_CODRESPD", slpcode);
                                oGeneralData.SetProperty("U_NOMRESPD", slpname);
                            }
                            else
                            {
                                oGeneralData.SetProperty("U_CODRESPD", -1);
                                oGeneralData.SetProperty("U_NOMRESPD", "");
                            }
                            oGeneralService.Update(oGeneralData);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        AceptarDocumentos(docId, "", DocEntryUDO);
                    }
                    else
                    {
                        if (validacion.Content!=null && validacion.Content.Equals("RECHAZAR"))
                        {
                            string docnum = SBO.ConsultasSBO.RecuperaDocNum(validacion.TpoDocRef, validacion.DocEntryBase, Tipo);
                            BaseType = String.Empty;

                            string empID = string.Empty;
                            string empName = string.Empty;
                            string slpcode = string.Empty;
                            string slpname = string.Empty;

                            if (Tipo.Equals(33) || Tipo.Equals(34) || Tipo.Equals(52) || Tipo.Equals(56) || Tipo.Equals(61))
                            {
                                if (validacion.TpoDocRef.Equals("801"))
                                {
                                    BaseType = "22";
                                }
                                else if (validacion.TpoDocRef.Equals("33"))
                                {
                                    BaseType = "13";
                                }
                                else if (validacion.TpoDocRef.Equals(""))
                                {
                                    BaseType = "";
                                }
                                else
                                {
                                    BaseType = "20";
                                }

                                empID = SBO.ConsultasSBO.ObtenerResponsableSN(validacion.CardCode);
                                if (!string.IsNullOrEmpty(empID) && !empID.Equals("0"))
                                {
                                    empName = SBO.ConsultasSBO.ObtenerNombreResponsableSN(empID);
                                }

                                slpcode = SBO.ConsultasSBO.ObtenerResponsable(validacion.TpoDocRef, validacion.DocEntryBase, Tipo);
                                if (!string.IsNullOrEmpty(slpcode))
                                {
                                    slpname = SBO.ConsultasSBO.ObtenerNombreResponsable(slpcode);
                                }
                            }
                            RechazarDocumentos(docId, validacion.Mensaje, DocEntryUDO, docnum, BaseType, validacion.DocEntryBase, empID, empName, slpcode, slpname);
                        }
                    }
                    oRecord.MoveNext();
                }
                Local.FuncionesComunes.LiberarObjetoGenerico(oRecord);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static Local.Message ProcesaValidacionGeneral(string docId, string Rut, int Tipo, Int64 Folio, string OCManual)
        {
            Local.Message retorno = new Local.Message();
            Local.Configuracion ExtConf = new Local.Configuracion();
            Local.Message result = new Local.Message();
            string CardCode = string.Empty;
            bool EsProveedorOC = true;
            string DecodeString = string.Empty;
            bool existeOC = false;
            bool existeEM = false;
            bool existeFC = false;
            string DocEntryBase = string.Empty;
            string DocTypeBase = string.Empty;
            string ObjTypeBase = string.Empty;

            try
            {
                // Validación de DTE ya integrado por RUT - TIPO - FOLIO
                result = SBO.ConsultasSBO.ValidacionDTEIntegrado(Rut, Tipo, Folio);
                if (!result.Success)
                {
                    retorno.Success = result.Success;
                    retorno.Mensaje = result.Mensaje;
                    return retorno;
                }

                // Validar existencia de socio de negocios
                CardCode = SBO.ConsultasSBO.ObtenerCardcode(Rut);
                if (string.IsNullOrEmpty(CardCode))
                {
                    retorno.Success = false;
                    retorno.Mensaje = string.Format("El DTE Nro {0} de Tipo {1} del proveedor {2} no tiene Socio de Negocio", Folio, Tipo, Rut);
                    return retorno;
                }

                // Obtener documento DTE
                ProveedorDTE proveedorDTE = new ProveedorDTE();
                string[] parametros = new string[] { docId };
                var provResult = proveedorDTE.ObtenerDocumento(parametros);
                if (!provResult.Success)
                {
                    retorno.Success = provResult.Success;
                    retorno.Mensaje = provResult.Mensaje;
                    return retorno;
                }

                var _Datos = proveedorDTE.DetalleDocuDTE;

                DecodeString = _Datos.XmlData;
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(DecodeString);

                //var newjson = Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc, Newtonsoft.Json.Formatting.None, false);
                var newjson = Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc);

                var settings = new JsonSerializerSettings();
                settings.NullValueHandling = NullValueHandling.Ignore;

                var _Documento = Newtonsoft.Json.JsonConvert.DeserializeObject<ClasesDTE.JsonDTE>(newjson, settings);

                var objDTE = _Documento.DTE.Documento;
                retorno.objDTE = objDTE;

                // Sólo buscar referencias para Facturas Afectas, Exentas y Guias
                if (Tipo.Equals(33) || Tipo.Equals(34) || Tipo.Equals(52))
                {
                    // Buscar referencias en DTE
                    // si no existen referencias en DTE
                    if (objDTE.Referencia == null)
                    {
                        retorno.Success = false;
                        retorno.Mensaje = string.Format("El DTE Nro {0} de Tipo {1} del proveedor {2} no tiene referencias", Folio, Tipo, Rut);
                    }
                    // Si existen referencias en DTE
                    if (objDTE.Referencia != null && string.IsNullOrEmpty(OCManual))
                    {
                        // Validacion de Referencia a OC
                        foreach (ClasesDTE.Referencia docRef in objDTE.Referencia)
                        {
                            if (docRef.TpoDocRef.Equals("801"))
                            {
                                // Validar que referencia exista en SAP
                                existeOC = SBO.ConsultasSBO.ExisteReferencia(docRef.TpoDocRef, docRef.FolioRef, CardCode, ref DocEntryBase, ref DocTypeBase, ref ObjTypeBase, Tipo);
                                retorno.TpoDocRef = docRef.TpoDocRef;
                                retorno.FolioRef = docRef.FolioRef;
                                retorno.DocEntryBase = DocEntryBase;
                                retorno.DocTypeBase = DocTypeBase;
                                retorno.ObjTypeBase = ObjTypeBase;
                                retorno.CardCode = CardCode;
                                break;
                            }
                        }
                        if (!existeOC)
                        {
                            retorno.Success = false;
                            retorno.Mensaje = string.Format("El DTE Nro {0} de Tipo {1} del proveedor {2} no tiene referencia de OC existente en SAP BO", Folio, Tipo, Rut);
                        }
                    }
                    // Si no existe OC y si tiene OC Manual
                    if (!existeOC && !string.IsNullOrEmpty(OCManual))
                    {
                        // Validar que referencia exista en SAP
                        existeOC = SBO.ConsultasSBO.ExisteReferencia("801", OCManual, CardCode, ref DocEntryBase, ref DocTypeBase, ref ObjTypeBase, Tipo);
                        retorno.TpoDocRef = "801";
                        retorno.FolioRef = OCManual;
                        retorno.DocEntryBase = DocEntryBase;
                        retorno.DocTypeBase = DocTypeBase;
                        retorno.ObjTypeBase = ObjTypeBase;
                        retorno.CardCode = CardCode;
                        if (!existeOC)
                        {
                            retorno.Success = false;
                            retorno.Mensaje = string.Format("El DTE Nro {0} de Tipo {1} del proveedor {2} no tiene referencia de OC existente en SAP BO", Folio, Tipo, Rut);
                        }
                    }

                    // Consultar si Proveedor requiere OC
                    EsProveedorOC = true;
                    EsProveedorOC = SBO.ConsultasSBO.EsProveedorOC(CardCode);

                    // Si es Proveedor OC
                    if (EsProveedorOC)
                    {
                        // Si se exige OC
                        if (ExtConf.Parametros.Valida_ExisteOC)
                        {
                            if (existeOC)
                            {
                                // Si se exige EM
                                if (ExtConf.Parametros.Valida_ExisteEntrada)
                                {
                                    if (!Tipo.Equals(52))
                                    {
                                        existeEM = SBO.ConsultasSBO.ExisteEntrada(DocEntryBase);
                                        if (!existeEM)
                                        {
                                            retorno.Success = false;
                                            retorno.Mensaje = string.Format("El DTE Nro {0} de Tipo {1} del proveedor {2} no tiene entradas registradas en SAP BO", Folio, Tipo, Rut);
                                            return retorno;
                                        }
                                    }
                                }
                                retorno.Mensaje = string.Format("El DTE Nro {0} de Tipo {1} del proveedor {2} se puede integrar. Validaciones OK", Folio, Tipo, Rut);

                                // Validacion montos DTE y OC
                                result = ValidaMontosDTEyOC(objDTE, Rut, Tipo, Folio, OCManual, existeOC, existeEM, DocEntryBase, ObjTypeBase);
                                if (!result.Success)
                                {
                                    retorno.Success = result.Success;
                                    retorno.Mensaje = result.Mensaje;
                                    retorno.Content = result.Content;
                                    return retorno;
                                }
                            }
                            else
                            {
                                retorno.Success = false;
                                return retorno;
                            }
                        }
                    }
                    // Proveedor no OC
                    else
                    {
                        // Si se exige monto máximo SN no OC
                        if (ExtConf.Parametros.Valida_MontoMaximo)
                        {
                            if (objDTE.Encabezado.Totales.MntTotal > ExtConf.Parametros.Valida_ValorMontoMaximo)
                            {
                                retorno.Success = false;
                                retorno.Mensaje = string.Format("El DTE Nro {0} de Tipo {1} del proveedor {2} excede el monto máximo para SN sin OC", Folio, Tipo, Rut);
                                return retorno;
                            }
                        }
                        if (!existeOC)
                        {
                            retorno.TpoDocRef = "";
                            retorno.FolioRef = "";
                            retorno.DocEntryBase = "0";
                            retorno.DocTypeBase = "";
                            retorno.ObjTypeBase = "";
                            retorno.CardCode = CardCode;
                        }
                        retorno.Mensaje = string.Format("El DTE Nro {0} de Tipo {1} del proveedor {2} se puede integrar. Validaciones OK", Folio, Tipo, Rut);
                    }
                    if (existeOC)
                    {
                        long open = 0;
                        open = SBO.ConsultasSBO.ObtenerReferenciaAbierta(retorno.TpoDocRef, retorno.FolioRef, CardCode, Tipo);
                        //if (existeEM)
                        //{
                        //    open += SBO.ConsultasSBO.ObtenerEntradasAbiertas(DocEntryBase);
                        //}
                        if (open.Equals(0))
                        {
                            retorno.Success = false;
                            retorno.Mensaje = string.Format("El DTE Nro {0} de Tipo {1} del proveedor {2} no tiene OC ni entradas abiertas en SAP BO", Folio, Tipo, Rut);
                            return retorno;
                        }
                    }
                }
                // Sólo buscar referencias para Notas de Débito y Crédito
                else if (Tipo.Equals(56) || Tipo.Equals(61))
                {
                    // Buscar referencias en DTE
                    // si no existen referencias en DTE
                    if (objDTE.Referencia == null)
                    {
                        retorno.Success = false;
                        retorno.Mensaje = string.Format("El DTE Nro {0} de Tipo {1} del proveedor {2} no tiene referencias", Folio, Tipo, Rut);
                    }
                    // Si existen referencias en DTE
                    if (objDTE.Referencia != null && string.IsNullOrEmpty(OCManual))
                    {
                        // Validacion de Referencia a OC
                        foreach (ClasesDTE.Referencia docRef in objDTE.Referencia)
                        {
                            if (docRef.TpoDocRef.Equals("33"))
                            {
                                // Validar que referencia exista en SAP
                                existeFC = SBO.ConsultasSBO.ExisteReferencia(docRef.TpoDocRef, docRef.FolioRef, CardCode, ref DocEntryBase, ref DocTypeBase, ref ObjTypeBase, Tipo);
                                retorno.TpoDocRef = docRef.TpoDocRef;
                                retorno.FolioRef = docRef.FolioRef;
                                retorno.DocEntryBase = DocEntryBase;
                                retorno.DocTypeBase = DocTypeBase;
                                retorno.ObjTypeBase = ObjTypeBase;
                                retorno.CardCode = CardCode;
                                break;
                            }
                        }
                        if (!existeFC)
                        {
                            retorno.Success = false;
                            retorno.Mensaje = string.Format("El DTE Nro {0} de Tipo {1} del proveedor {2} no tiene referencia de FC existente en SAP BO", Folio, Tipo, Rut);
                        }
                    }
                    // Si no existe OC y si tiene OC Manual
                    if (!existeFC && !string.IsNullOrEmpty(OCManual))
                    {
                        // Validar que referencia exista en SAP
                        existeFC = SBO.ConsultasSBO.ExisteReferencia("33", OCManual, CardCode, ref DocEntryBase, ref DocTypeBase, ref ObjTypeBase, Tipo);
                        retorno.TpoDocRef = "33";
                        retorno.FolioRef = OCManual;
                        retorno.DocEntryBase = DocEntryBase;
                        retorno.DocTypeBase = DocTypeBase;
                        retorno.ObjTypeBase = ObjTypeBase;
                        retorno.CardCode = CardCode;
                        if (!existeFC)
                        {
                            retorno.Success = false;
                            retorno.Mensaje = string.Format("El DTE Nro {0} de Tipo {1} del proveedor {2} no tiene referencia de FC existente en SAP BO", Folio, Tipo, Rut);
                        }
                    }

                    if (existeFC)
                    {
                        retorno.Mensaje = string.Format("El DTE Nro {0} de Tipo {1} del proveedor {2} se puede integrar. Validaciones OK", Folio, Tipo, Rut);

                        // Validacion montos DTE y OC
                        result = ValidaMontosDTEyOC(objDTE, Rut, Tipo, Folio, OCManual, existeOC, existeEM, DocEntryBase, ObjTypeBase);
                        if (!result.Success)
                        {
                            retorno.Success = result.Success;
                            retorno.Mensaje = result.Mensaje;
                            retorno.Content = result.Content;
                            return retorno;
                        }
                    }
                    else
                    {
                        retorno.Success = false;
                        return retorno;
                    }
                }
                else
                {
                    retorno.TpoDocRef = "";
                    retorno.FolioRef = "";
                    retorno.DocEntryBase = "0";
                    retorno.DocTypeBase = "";
                    retorno.ObjTypeBase = "";
                    retorno.CardCode = CardCode;
                }

                // Validacion encabezados DTE
                result = ValidaEncabezadoDTE(objDTE, Rut, Tipo, Folio, OCManual);
                if (!result.Success)
                {
                    retorno.Success = result.Success;
                    retorno.Mensaje = result.Mensaje;
                    return retorno;
                }

                // Cumple todas las validaciones
                retorno.Success = true;
            }
            catch (Exception ex)
            {
                retorno.Success = false;
                retorno.Mensaje = ex.Message;
            }
            return retorno;
        }

        private static Local.Message ValidaMontosDTE(ClasesDTE.Documento objDTE, string Rut, int Tipo, Int64 Folio, string OCManual, bool existeOC, bool existeEM, string DocEntryBase, string DocTypeBase)
        {
            Local.Message retorno = new Local.Message();
            //Local.Configuracion ExtConf = new Local.Configuracion();
            //Local.Message result = new Local.Message();
            retorno.Success = true;

            try
            {
                if (true)
                {
                    if (existeOC)
                    {
                        //Validar Razon Social
                        if (!SBO.ConsultasSBO.ValidaMontos(objDTE.Encabezado.Totales.MntTotal, DocEntryBase, DocTypeBase))
                        {
                            retorno.Success = false;
                            retorno.Mensaje = string.Format("El DTE Nro {0} de Tipo {1} del proveedor {2} no conincide con los montos y tolerancias de la OC", Folio, Tipo, Rut);
                            return retorno;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                retorno.Success = false;
                retorno.Mensaje = ex.Message;
            }
            return retorno;
        }
        private static Local.Message ValidaMontosDTEyOC(ClasesDTE.Documento objDTE, string Rut, int Tipo, Int64 Folio, string OCManual, bool existeOC, bool existeEM, string DocEntryBase, string DocTypeBase)
        {
            Local.Message retorno = new Local.Message();
            Local.Configuracion ExtConf = new Local.Configuracion();
            //Local.Message result = new Local.Message();
            retorno.Success = true;

            try
            {
                if (ExtConf.Parametros.Valida_MontoOC)
                {
                    if (existeOC)
                    {
                        //Diferencia Montos DTE y OC
                        var diferencia = SBO.ConsultasSBO.ObtieneDiferenciaMontosDTEyOC(objDTE.Encabezado.Totales.MntTotal, DocEntryBase, DocTypeBase);

                        if (ExtConf.Parametros.Valida_PermiteTolerancias)
                        {
                            if (diferencia < ExtConf.Parametros.Valida_ValorRechazoMontoMenor || diferencia > ExtConf.Parametros.Valida_ValorRechazoMontoMayor)
                            {
                                var EsMonExtranjera = SBO.ConsultasSBO.EsOCMonedaExtranjera(DocEntryBase, DocTypeBase);
                                if (EsMonExtranjera)
                                {
                                    retorno.Success = false;
                                    retorno.Mensaje = string.Format("El DTE Nro {0} de Tipo {1} del proveedor {2} excede los montos y/o tolerancias de la OC. Documento base en moneda extranjera", Folio, Tipo, Rut);
                                    retorno.Content = "INVALIDO";
                                }
                                else
                                {
                                    retorno.Success = false;
                                    retorno.Mensaje = string.Format("El DTE Nro {0} de Tipo {1} del proveedor {2} excede los montos y/o tolerancias de la OC", Folio, Tipo, Rut);
                                    retorno.Content = "RECHAZAR";
                                }
                                return retorno;
                            }
                            else if (diferencia < ExtConf.Parametros.Valida_ValorAprobacionMontoMenor && diferencia > ExtConf.Parametros.Valida_ValorAprobacionMontoMayor)
                            {
                                retorno.Success = true;
                                retorno.Mensaje = string.Format("El DTE Nro {0} de Tipo {1} del proveedor {2} conincide con los montos y/o tolerancias de la OC", Folio, Tipo, Rut);
                                retorno.Content = "APROBAR";
                            }
                            else if (diferencia.Equals(0))
                            {
                                retorno.Success = true;
                                retorno.Mensaje = string.Format("El DTE Nro {0} de Tipo {1} del proveedor {2} conincide con los montos y/o tolerancias de la OC", Folio, Tipo, Rut);
                                retorno.Content = "APROBAR";
                            }
                            else
                            {
                                retorno.Success = false;
                                retorno.Mensaje = string.Format("El DTE Nro {0} de Tipo {1} del proveedor {2} no coincide con los montos y/o tolerancias de la OC", Folio, Tipo, Rut);
                                retorno.Content = "INVALIDO";
                                return retorno;
                            }
                        }
                        else
                        {
                            if (diferencia.Equals(0))
                            {
                                retorno.Success = true;
                                retorno.Mensaje = string.Format("El DTE Nro {0} de Tipo {1} del proveedor {2} conincide con los montos de la OC", Folio, Tipo, Rut);
                                retorno.Content = "APROBAR";
                            }
                            else
                            {
                                retorno.Success = false;
                                retorno.Mensaje = string.Format("El DTE Nro {0} de Tipo {1} del proveedor {2} no coincide con los montos de la OC", Folio, Tipo, Rut);
                                retorno.Content = "INVALIDO";
                                return retorno;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                retorno.Success = false;
                retorno.Mensaje = ex.Message;
            }
            return retorno;
        }

        private static Local.Message ValidaEncabezadoDTE(ClasesDTE.Documento objDTE, string Rut, int Tipo, Int64 Folio, string OCManual)
        {
            Local.Message retorno = new Local.Message();
            Local.Configuracion ExtConf = new Local.Configuracion();
            //Local.Message result = new Local.Message();
            retorno.Success = true;

            try
            {
                if (ExtConf.Parametros.Valida_EncabezadosDTE)
                {
                    //Validar Razon Social
                    if (!SBO.ConsultasSBO.ValidaRazonSocial(objDTE.Encabezado.Receptor.RznSocRecep))
                    {
                        retorno.Success = false;
                        retorno.Mensaje = string.Format("El DTE Nro {0} de Tipo {1} del proveedor {2} no contiene una Razón Social de receptor válida", Folio, Tipo, Rut);
                        retorno.Content = "RECHAZAR";
                        return retorno;
                    }
                    //Validar Giro
                    if (!SBO.ConsultasSBO.ValidaGiro(objDTE.Encabezado.Receptor.GiroRecep))
                    {
                        retorno.Success = false;
                        retorno.Mensaje = string.Format("El DTE Nro {0} de Tipo {1} del proveedor {2} no contiene un Giro de receptor válido", Folio, Tipo, Rut);
                        retorno.Content = "RECHAZAR";
                        return retorno;
                    }
                    //Validar Dirección
                    if (!SBO.ConsultasSBO.ValidaDireccion(objDTE.Encabezado.Receptor.DirRecep))
                    {
                        retorno.Success = false;
                        retorno.Mensaje = string.Format("El DTE Nro {0} de Tipo {1} del proveedor {2} no contiene una Dirección de receptor válida", Folio, Tipo, Rut);
                        retorno.Content = "RECHAZAR";
                        return retorno;
                    }
                    //Validar Comuna
                    if (!SBO.ConsultasSBO.ValidaComuna(objDTE.Encabezado.Receptor.CmnaRecep))
                    {
                        retorno.Success = false;
                        retorno.Mensaje = string.Format("El DTE Nro {0} de Tipo {1} del proveedor {2} no contiene una Comuna de receptor válida", Folio, Tipo, Rut);
                        retorno.Content = "RECHAZAR";
                        return retorno;
                    }
                }
            }
            catch (Exception ex)
            {
                retorno.Success = false;
                retorno.Mensaje = ex.Message;
            }
            return retorno;
        }

        public void AceptarDocumentos(string docId, string motivo, int DocEntryUDO)
        {
            try
            {
                SAPbobsCOM.GeneralService oGeneralService = null;
                SAPbobsCOM.GeneralData oGeneralData = null;
                SAPbobsCOM.GeneralData oChild = null;
                SAPbobsCOM.GeneralDataCollection oChildren = null;
                SAPbobsCOM.GeneralDataParams oGeneralParams = null;
                SAPbobsCOM.CompanyService oCompanyService = null;

                // Aceptar documento
                try
                {
                    ProveedorDTE proveedorDTE = new ProveedorDTE();

                    string[] parametros = new string[] { docId, Comun.Enumeradores.EstadosComercialesComunes.Aceptacion_Recibo_Mercaderias.ToString(), motivo };

                    var provResult = proveedorDTE.CambiarEstadoComercial(parametros);
                    //ProvDTE.Local.Message provResult = new ProvDTE.Local.Message();
                    //provResult.Success = true;
                    if (provResult.Success)
                    {
                        // Cambiar estado a aceptado
                        try
                        {
                            oCompanyService = SBO.ConexionDIAPI.oCompany.GetCompanyService();
                            // Get GeneralService (oCmpSrv is the CompanyService)
                            oGeneralService = oCompanyService.GetGeneralService("SO_MONITOR");
                            // Create data for new row in main UDO
                            oGeneralParams = ((SAPbobsCOM.GeneralDataParams)(oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)));
                            oGeneralParams.SetProperty("DocEntry", DocEntryUDO);
                            oGeneralData = oGeneralService.GetByParams(oGeneralParams);
                            if (string.IsNullOrEmpty(motivo))
                            {
                                //oGeneralData.SetProperty("U_ESTSII", ((int)Comun.Enumeradores.EstadosSii.aceptado).ToString());
                                oGeneralData.SetProperty("U_ESTSII", Comun.Enumeradores.GetEstadoSii(Comun.Enumeradores.EstadosSii.aceptado));
                                oGeneralData.SetProperty("U_ESTADO", "2");
                            }
                            else
                            {
                                //oGeneralData.SetProperty("U_ESTSII", ((int)Comun.Enumeradores.EstadosSii.aceptadoreparo).ToString());
                                oGeneralData.SetProperty("U_ESTSII", Comun.Enumeradores.GetEstadoSii(Comun.Enumeradores.EstadosSii.aceptadoreparo));
                                oGeneralData.SetProperty("U_ESTADO", "3");
                                oGeneralData.SetProperty("U_RAZONRE", motivo);
                            }
                            oGeneralService.Update(oGeneralData);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void RechazarDocumentos(string docId, string motivo, int DocEntryUDO, string docnum, string BaseType, string DocEntryBase, string empID, string empName, string slpcode, string slpname)
        {
            try
            {
                SAPbobsCOM.GeneralService oGeneralService = null;
                SAPbobsCOM.GeneralData oGeneralData = null;
                SAPbobsCOM.GeneralData oChild = null;
                SAPbobsCOM.GeneralDataCollection oChildren = null;
                SAPbobsCOM.GeneralDataParams oGeneralParams = null;
                SAPbobsCOM.CompanyService oCompanyService = null;

                // Aceptar documento
                try
                {
                    ProveedorDTE proveedorDTE = new ProveedorDTE();

                    string[] parametros = new string[] { docId, Comun.Enumeradores.EstadosComercialesComunes.Rechazo_Comercial.ToString(), motivo };

                    var provResult = proveedorDTE.CambiarEstadoComercial(parametros);
                    //ProvDTE.Local.Message provResult = new ProvDTE.Local.Message();
                    //provResult.Success = true;
                    if (provResult.Success)
                    {
                        // Cambiar estado a rechazado
                        try
                        {
                            oCompanyService = SBO.ConexionDIAPI.oCompany.GetCompanyService();
                            // Get GeneralService (oCmpSrv is the CompanyService)
                            oGeneralService = oCompanyService.GetGeneralService("SO_MONITOR");
                            // Create data for new row in main UDO
                            oGeneralParams = ((SAPbobsCOM.GeneralDataParams)(oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)));
                            oGeneralParams.SetProperty("DocEntry", DocEntryUDO);
                            oGeneralData = oGeneralService.GetByParams(oGeneralParams);
                            oGeneralData.SetProperty("U_DOCBASE", docnum);
                            oGeneralData.SetProperty("U_TIPOASO", BaseType);
                            oGeneralData.SetProperty("U_DOCASO", DocEntryBase == "0" ? "" : DocEntryBase);
                            //oGeneralData.SetProperty("U_ESTSII", ((int)Comun.Enumeradores.EstadosSii.rechazado).ToString());
                            oGeneralData.SetProperty("U_ESTSII", Comun.Enumeradores.GetEstadoSii(Comun.Enumeradores.EstadosSii.rechazado));
                            oGeneralData.SetProperty("U_RAZONRE", motivo);
                            oGeneralData.SetProperty("U_ESTADO", "4");
                            if (!empID.Equals("0") && !string.IsNullOrEmpty(empID))
                            {
                                oGeneralData.SetProperty("U_CODRESP", empID);
                                oGeneralData.SetProperty("U_NOMRESP", empName);
                            }
                            else
                            {
                                oGeneralData.SetProperty("U_CODRESP", 0);
                                oGeneralData.SetProperty("U_NOMRESP", "");
                            }
                            if (!slpcode.Equals("-1") && !string.IsNullOrEmpty(slpcode))
                            {
                                oGeneralData.SetProperty("U_CODRESPD", slpcode);
                                oGeneralData.SetProperty("U_NOMRESPD", slpname);
                            }
                            else
                            {
                                oGeneralData.SetProperty("U_CODRESPD", -1);
                                oGeneralData.SetProperty("U_NOMRESPD", "");
                            }
                            oGeneralService.Update(oGeneralData);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void IntegrarDocumentos()
        {
            try
            {
                String Rut = String.Empty;
                String BaseType = String.Empty;
                Int32 Tipo = 0;
                Int64 Folio = 0;
                Int32 DocEntryUDO;
                String OCManual = String.Empty;

                // Filtro por Fechas
                String DesdeFecha = string.Empty;
                String HastaFecha = string.Empty;

                DateTime dt;
                String Mes = String.Empty;
                String Dia = String.Empty;
                // Por defecto trae los últimos 1 días
                dt = new DateTime(DateTime.Now.Year, DateTime.Today.Month, DateTime.Today.Day);
                Mes = (dt.Month.ToString().Length == 1) ? dt.Month.ToString().PadLeft(2, '0') : dt.Month.ToString();
                Dia = (dt.Day.ToString().Length == 1) ? dt.Day.ToString().PadLeft(2, '0') : dt.Day.ToString();

                HastaFecha = String.Format("{0}{1}{2}", dt.Year.ToString(), Mes, Dia);

                dt = DateTime.Today.AddDays(-1);
                Mes = (dt.Month.ToString().Length == 1) ? dt.Month.ToString().PadLeft(2, '0') : dt.Month.ToString();
                Dia = (dt.Day.ToString().Length == 1) ? dt.Day.ToString().PadLeft(2, '0') : dt.Day.ToString();

                DesdeFecha = String.Format("{0}{1}{2}", dt.Year.ToString(), Mes, Dia);

                SAPbobsCOM.Recordset oRecord = (SAPbobsCOM.Recordset)SBO.ConexionDIAPI.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                string _query = @"SELECT * FROM ""@SO_MONITOR"" WHERE ""U_ESTADO"" IN ('2', '3')";
                _query += string.Format(@" AND ""U_FECHAEM"" BETWEEN '{0}' AND '{1}'", DesdeFecha, HastaFecha);
                oRecord.DoQuery(_query);
                while (!oRecord.EoF)
                {
                    DocEntryUDO = Convert.ToInt32(oRecord.Fields.Item("DocEntry").Value);
                    string docId = oRecord.Fields.Item("U_DOCID").Value.ToString();
                    Rut = oRecord.Fields.Item("U_RUTEMIS").Value.ToString();
                    Tipo = Convert.ToInt16(oRecord.Fields.Item("U_TIPODOC").Value.ToString());
                    Folio = Convert.ToInt64(oRecord.Fields.Item("U_FOLIO").Value.ToString());
                    OCManual = oRecord.Fields.Item("U_DOCBASE").Value.ToString();

                    var validacion = ProcesaValidacionGeneral(docId, Rut, Tipo, Folio, OCManual);
                    if (validacion.Success)
                    {
                        BaseType = String.Empty;
                        if (validacion.TpoDocRef.Equals("801"))
                        {
                            BaseType = "22";
                        }
                        else if (validacion.TpoDocRef.Equals(""))
                        {
                            BaseType = "";
                        }
                        else
                        {
                            BaseType = "20";
                        }

                        // Integrar documento
                        try
                        {
                            string TpoDocRef = validacion.TpoDocRef == "" ? null : validacion.TpoDocRef;
                            string FolioRef = validacion.FolioRef == "" ? null : validacion.FolioRef;
                            string DocEntryBase = validacion.DocEntryBase == "0" ? null : validacion.DocEntryBase;
                            string DocTypeBase = validacion.DocTypeBase == "" ? null : validacion.DocTypeBase;
                            BaseType = BaseType == "" ? null : BaseType;
                            IntegrarDTE(validacion.objDTE, TpoDocRef, FolioRef, validacion.CardCode, DocEntryBase, DocTypeBase, BaseType, DocEntryUDO, docId);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    oRecord.MoveNext();
                }
                Local.FuncionesComunes.LiberarObjetoGenerico(oRecord);
            }
            catch (Exception ex )
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void IntegrarDTE(ClasesDTE.Documento objDTE, String TipoRef, String Folio, String CardCode, String DocEntryBase, String DocTypeBase, String BaseType, Int32 DocEntryUDO, string DocId)
        {
            objDTE.Detalle = objDTE.Detalle.Where(x => x.MontoItem > 0).ToArray();

            SAPbobsCOM.GeneralService oGeneralService = null;
            SAPbobsCOM.GeneralData oGeneralData = null;
            SAPbobsCOM.GeneralData oChild = null;
            SAPbobsCOM.GeneralDataCollection oChildren = null;
            SAPbobsCOM.GeneralDataParams oGeneralParams = null;
            SAPbobsCOM.CompanyService oCompanyService = null;

            String NuevoDocumento = string.Empty;
            String CardCodeSN = String.Empty;
            String DocEntryDocBase = String.Empty;
            String BaseTypeDocBase = String.Empty;
            string Branch = "1";
            CardCodeSN = CardCode;
            DocEntryDocBase = DocEntryBase;
            BaseTypeDocBase = BaseType;

            TipoCambio = 1;

            MonedaLocal = SBO.ConsultasSBO.ObtenerMonedaLocal();
            MonedaDocumento = MonedaLocal;
            MonedaConta = MonedaLocal;

            SAPbobsCOM.Documents oDoc = null;
            oDoc = (SAPbobsCOM.Documents)SBO.ConexionDIAPI.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oDrafts);

            // Codigo IVA Exento
            string CodigoIVAEXE = SBO.ConsultasSBO.ObtenerCodigoIVAEXE();

            // Encabezado
            oDoc.CardCode = CardCode;
            oDoc.CardName = SBO.ConsultasSBO.ObtenerCardName(objDTE.Encabezado.Emisor.RUTEmisor, CardCode);
            switch (objDTE.Encabezado.IdDoc.TipoDTE)
            {
                case "33":
                    oDoc.DocObjectCode = SAPbobsCOM.BoObjectTypes.oPurchaseInvoices;
                    oDoc.Indicator = objDTE.Encabezado.IdDoc.TipoDTE;
                    break;
                case "34":
                    oDoc.DocObjectCode = SAPbobsCOM.BoObjectTypes.oPurchaseInvoices;
                    oDoc.Indicator = objDTE.Encabezado.IdDoc.TipoDTE;
                    break;
                case "43":
                    oDoc.DocObjectCode = SAPbobsCOM.BoObjectTypes.oPurchaseInvoices;
                    oDoc.Indicator = objDTE.Encabezado.IdDoc.TipoDTE;
                    break;
                case "52":
                    oDoc.DocObjectCode = SAPbobsCOM.BoObjectTypes.oPurchaseDeliveryNotes;
                    oDoc.Indicator = objDTE.Encabezado.IdDoc.TipoDTE;
                    break;
                case "56":
                    oDoc.DocObjectCode = SAPbobsCOM.BoObjectTypes.oPurchaseInvoices;
                    oDoc.DocumentSubType = SAPbobsCOM.BoDocumentSubType.bod_PurchaseDebitMemo;
                    oDoc.Indicator = objDTE.Encabezado.IdDoc.TipoDTE;
                    break;
                case "61":
                    oDoc.DocObjectCode = SAPbobsCOM.BoObjectTypes.oPurchaseCreditNotes;
                    oDoc.Indicator = objDTE.Encabezado.IdDoc.TipoDTE;
                    break;
                default:
                    oDoc.DocObjectCode = SAPbobsCOM.BoObjectTypes.oPurchaseInvoices;
                    oDoc.Indicator = objDTE.Encabezado.IdDoc.TipoDTE;
                    break;
            }

            oDoc.DocDate = Convert.ToDateTime(objDTE.Encabezado.IdDoc.FchEmis);
            oDoc.DocDueDate = Convert.ToDateTime(objDTE.Encabezado.IdDoc.FchVenc);
            oDoc.FolioNumber = Convert.ToInt32(objDTE.Encabezado.IdDoc.Folio);
            oDoc.FolioPrefixString = objDTE.Encabezado.IdDoc.TipoDTE;
            switch (DocTypeBase)
            {
                case "I":
                    oDoc.DocType = SAPbobsCOM.BoDocumentTypes.dDocument_Items;
                    break;
                case "S":
                    oDoc.DocType = SAPbobsCOM.BoDocumentTypes.dDocument_Service;
                    break;
                default:
                    oDoc.DocType = SAPbobsCOM.BoDocumentTypes.dDocument_Service;
                    break;
            }

            // Obtener los detalles del documento base
            SAPbobsCOM.Recordset oRecord = (SAPbobsCOM.Recordset)SBO.ConexionDIAPI.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            string _query = String.Empty;

            if (TipoRef != null)
            {
                if (TipoRef.Equals("801"))
                {
                    // OC - OPOR
                    _query = @"SELECT ""T1"".""ItemCode"", ""T1"".""Dscription"", ""T1"".""Quantity"", ""T1"".""Price"",
                            ""T1"".""LineTotal"", ""T1"".""LineNum"", ""T1"".""OpenQty"",
                            ""T0"".""BPLId"", ""T0"".""DocCur"",
                            ""T1"".""Currency"", ""T1"".""Rate"", ""T1"".""TotalFrgn"", ""T0"".""DocRate"" ";
                    _query += @"FROM ""OPOR"" ""T0"" INNER JOIN ""POR1"" ""T1"" ";
                    _query += @"ON ""T0"".""DocEntry"" = ""T1"".""DocEntry"" ";
                    _query += @"WHERE ""T0"".""CardCode"" = '" + CardCode + @"' AND ""T0"".""DocNum"" = '" + Folio + "'";
                }
                else if (TipoRef.Equals("33"))
                {
                    // FC - OPCH
                    _query = @"SELECT ""T1"".""ItemCode"", ""T1"".""Dscription"", ""T1"".""Quantity"", ""T1"".""Price"",
                            ""T1"".""LineTotal"", ""T1"".""LineNum"", ""T1"".""OpenQty"",
                            ""T0"".""BPLId"", ""T0"".""DocCur"",
                            ""T1"".""Currency"", ""T1"".""Rate"", ""T1"".""TotalFrgn"", ""T0"".""DocRate"" ";
                    _query += @"FROM ""OPCH"" ""T0"" INNER JOIN ""PCH1"" ""T1"" ";
                    _query += @"ON ""T0"".""DocEntry"" = ""T1"".""DocEntry"" ";
                    _query += @"WHERE ""T0"".""CardCode"" = '" + CardCode + @"' AND ""FolioPref"" = '" + TipoRef + @"' AND ""FolioNum"" = '" + Folio + "'";
                }
                oRecord.DoQuery(_query);
            }

            // Detalle
            Int32 indexDet = 0;
            if (!oRecord.EoF)
            {
                MonedaDocumento = oRecord.Fields.Item("DocCur").Value.ToString();
                MonedaConta = MonedaDocumento;
                if (!MonedaConta.Equals(MonedaLocal))
                {
                    FechaEmision = Convert.ToDateTime(string.Format("{0:YYYY-MM-DD}", objDTE.Encabezado.IdDoc.FchEmis), CultureInfo.CurrentCulture);
                    var oSBObob = (SAPbobsCOM.SBObob)SBO.ConexionDIAPI.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoBridge);
                    var monTC = SBO.ConsultasSBO.ObtenerMonedaTC(MonedaDocumento);
                    var Rec = oSBObob.GetCurrencyRate(monTC, FechaEmision);
                    var _rate = Convert.ToDouble(Rec.Fields.Item(0).Value.ToString());
                    Local.FuncionesComunes.LiberarObjetoGenerico(Rec);
                    TipoCambio = _rate;
                }
                else
                {
                    TipoCambio = 1;
                }

                while (!oRecord.EoF)
                {
                    if (indexDet != 0)
                    {
                        oDoc.Lines.Add();
                    }

                    switch (DocTypeBase)
                    {
                        case "I":
                            oDoc.Lines.ItemCode = oRecord.Fields.Item("ItemCode").Value.ToString();
                            oDoc.Lines.ItemDescription = oRecord.Fields.Item("Dscription").Value.ToString();
                            oDoc.Lines.Quantity = (double)oRecord.Fields.Item("Quantity").Value;
                            break;
                        case "S":
                            oDoc.Lines.ItemDescription = oRecord.Fields.Item("Dscription").Value.ToString();
                            oDoc.Lines.Quantity = 1;
                            break;
                        default:
                            oDoc.Lines.ItemDescription = oRecord.Fields.Item("Dscription").Value.ToString();
                            oDoc.Lines.Quantity = 1;
                            break;
                    }
                    oDoc.Lines.LineTotal = ((double)oRecord.Fields.Item("LineTotal").Value / TipoCambio);
                    oDoc.Lines.Price = ((double)oRecord.Fields.Item("Price").Value / TipoCambio);
                    oDoc.Lines.BaseEntry = Int32.Parse(DocEntryDocBase);
                    oDoc.Lines.BaseLine = (int)oRecord.Fields.Item("LineNum").Value;
                    oDoc.Lines.BaseType = Int32.Parse(BaseTypeDocBase);
                    indexDet++;
                    oRecord.MoveNext();
                }
            }
            else
            {
                foreach (ClasesDTE.Detalle det in objDTE.Detalle)
                {
                    if (indexDet != 0)
                    {
                        oDoc.Lines.Add();
                    }
                    switch (DocTypeBase)
                    {
                        case "I":
                            oDoc.Lines.ItemCode = det.ItemCode;
                            oDoc.Lines.ItemDescription = det.NmbItem;
                            oDoc.Lines.Quantity = det.QtyItem;
                            break;
                        case "S":
                            oDoc.Lines.ItemDescription = det.NmbItem;
                            oDoc.Lines.Quantity = 1;
                            break;
                        default:
                            oDoc.Lines.ItemDescription = det.NmbItem;
                            oDoc.Lines.Quantity = 1;
                            break;
                    }
                    oDoc.Lines.Price = (det.PrcItem / TipoCambio);
                    if (!MonedaConta.Equals(MonedaLocal))
                    {
                        oDoc.Lines.Currency = MonedaConta;
                        oDoc.Lines.RowTotalFC = (det.MontoItem / TipoCambio);
                        if (!TipoCambio.Equals(1))
                        {
                            oDoc.Lines.Rate = TipoCambio;
                        }
                    }
                    else if (MonedaConta.Equals(MonedaLocal))
                    {
                        oDoc.Lines.LineTotal = (det.MontoItem / TipoCambio);
                    }
                    oDoc.Lines.TaxCode = "IVA";
                    if (objDTE.Encabezado.IdDoc.TipoDTE.Equals("34"))
                    {
                        oDoc.Lines.TaxCode = CodigoIVAEXE;
                    }
                    else if (objDTE.Encabezado.Totales.MntExe.Equals(objDTE.Encabezado.Totales.MntTotal))
                    {
                        oDoc.Lines.TaxCode = CodigoIVAEXE;
                    }
                    else if (det.IndExe.Equals(1))
                    {
                        oDoc.Lines.TaxCode = CodigoIVAEXE;
                    }
                    oDoc.Lines.DiscountPercent = det.DescuentoPct;
                    indexDet++;
                }
            }
            Local.FuncionesComunes.LiberarObjetoGenerico(oRecord);

            if (!MonedaConta.Equals(MonedaLocal))
            {
                oDoc.DocCurrency = MonedaConta;
                oDoc.DocTotalFc = (objDTE.Encabezado.Totales.MntTotal / TipoCambio);
                if (!TipoCambio.Equals(1))
                {
                    oDoc.DocRate = TipoCambio;
                }
            }
            else if (MonedaConta.Equals(MonedaLocal))
            {
                oDoc.DocTotal = (objDTE.Encabezado.Totales.MntTotal / TipoCambio);
            }

            if (SBO.ConsultasSBO.MultiBranchActivo())
            {
                oDoc.BPL_IDAssignedToInvoice = Convert.ToInt32(Branch);
            }

            SAPbobsCOM.CompanyService com_service = SBO.ConexionDIAPI.oCompany.GetCompanyService();
            SAPbobsCOM.PathAdmin oPathAdmin = com_service.GetPathAdmin();
            String Path = string.Empty;
            Path = oPathAdmin.AttachmentsFolderPath;
            int indexPath = Path.LastIndexOf("\\");
            if (indexPath > 0)
            {
                Path = Path.Substring(0, indexPath);
            }
            String fileName = string.Empty;
            String fileExt = string.Empty;

            try
            {
                ProveedorDTE proveedorDTE = new ProveedorDTE();

                string[] parametros = new string[] { DocId };

                var provResult = proveedorDTE.ObtenerPDFDocumento(parametros);

                if (provResult.Success)
                {
                    var _Datos = proveedorDTE.DetalleDocuDTE;
                    String link = _Datos.ImagenLink.ToString();

                    String[] linkSplit = link.Split('/');
                    fileName = linkSplit[linkSplit.Length - 1];
                    int indexExt = fileName.LastIndexOf(".");
                    if (indexExt > 0)
                    {
                        fileExt = fileName.Substring(indexExt + 1);
                        fileName = fileName.Substring(0, indexExt);
                    }

                    // Descargar PDF
                    try
                    {
                        if (link.LastIndexOf("://") > 0)
                        {
                            using (System.Net.WebClient client = new System.Net.WebClient())
                            {
                                client.DownloadFile(link, @"" + Path + @"\\" + fileName + @"." + fileExt);
                            }
                        }
                    }
                    // PDF en directorio
                    catch
                    {
                    }
                }
                // Validación obtención de documento
                else
                {
                    Console.WriteLine(provResult.Mensaje);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            SAPbobsCOM.Attachments2 oAtt = null;
            oAtt = (SAPbobsCOM.Attachments2)SBO.ConexionDIAPI.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oAttachments2);

            oAtt.Lines.Add();
            oAtt.Lines.FileName = fileName;
            oAtt.Lines.FileExtension = fileExt;
            oAtt.Lines.SourcePath = Path;
            oAtt.Lines.Override = SAPbobsCOM.BoYesNoEnum.tYES;

            Int32 RetAtt = oAtt.Add();
            if (RetAtt != 0)
            {
                Int32 ErrCode = 0;
                String ErrMsj = String.Empty;
                SBO.ConexionDIAPI.oCompany.GetLastError(out ErrCode, out ErrMsj);
            }
            else
            {
                oAtt.GetByKey(int.Parse(SBO.ConexionDIAPI.oCompany.GetNewObjectKey()));
                oDoc.AttachmentEntry = oAtt.AbsoluteEntry;
            }

            Int32 RetVal = oDoc.Add();
            String Mensaje = String.Empty;
            if (RetVal.Equals(0))
            {
                NuevoDocumento = SBO.ConexionDIAPI.oCompany.GetNewObjectKey();
                Mensaje = String.Format("Documento número {0} de proveedor {1} integrado correctamente como preliminar {2}.", objDTE.Encabezado.IdDoc.Folio, CardCodeSN, NuevoDocumento);
                Console.WriteLine(Mensaje);

                // Cambiar estado a integrado
                try
                {
                    oCompanyService = SBO.ConexionDIAPI.oCompany.GetCompanyService();
                    // Get GeneralService (oCmpSrv is the CompanyService)
                    oGeneralService = oCompanyService.GetGeneralService("SO_MONITOR");
                    // Create data for new row in main UDO
                    oGeneralParams = ((SAPbobsCOM.GeneralDataParams)(oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)));
                    oGeneralParams.SetProperty("DocEntry", DocEntryUDO);
                    oGeneralData = oGeneralService.GetByParams(oGeneralParams);
                    oGeneralData.SetProperty("U_PRELIM", NuevoDocumento);
                    oGeneralData.SetProperty("U_ESTADO", "5");
                    oGeneralService.Update(oGeneralData);
                    //dtDoc.SetValue("co_obs", i, String.Format("OK. Documento preliminar {0}.", NuevoDocumento));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else
            {
                Int32 ErrCode = 0;
                String ErrMsj = String.Empty;
                SBO.ConexionDIAPI.oCompany.GetLastError(out ErrCode, out ErrMsj);
                Mensaje = String.Format("Documento número {0} de proveedor {1} no integrado: {2}", objDTE.Encabezado.IdDoc.Folio, CardCodeSN, ErrMsj);
                Console.WriteLine(Mensaje);
            }
        }
    }
}
