using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Soindus.AddOnRindegastos.SBO
{
    public class ConsultasSBO
    {
        public static bool PlanDeCuentasSegmentado()
        {
            try
            {
                bool segmentado = false;
                SAPbobsCOM.Recordset oRecord = (SAPbobsCOM.Recordset)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                string _query = String.Empty;

                _query = @"SELECT ""EnbSgmnAct"" FROM ""CINF""";

                oRecord.DoQuery(_query);

                // Si hay datos
                if (!oRecord.EoF)
                {
                    segmentado = oRecord.Fields.Item("EnbSgmnAct").Value.ToString().Equals("N") ? false : true;
                }
                Comun.FuncionesComunes.LiberarObjetoGenerico(oRecord);
                return segmentado;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static string ObtenerFormatoCuentasSegmentado()
        {
            try
            {
                string formato = "";
                SAPbobsCOM.Recordset oRecord = (SAPbobsCOM.Recordset)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                string _query = String.Empty;

                _query = @"SELECT * FROM ""OASG"" ORDER BY ""AbsId""";

                oRecord.DoQuery(_query);

                // Si hay datos
                if (!oRecord.EoF)
                {
                    while (!oRecord.EoF)
                    {
                        formato += string.IsNullOrEmpty(formato) ? string.Concat(Enumerable.Repeat("#", (int)oRecord.Fields.Item("Size").Value)) : @"-" + string.Concat(Enumerable.Repeat("#", (int)oRecord.Fields.Item("Size").Value));
                        oRecord.MoveNext();
                    }
                }
                Comun.FuncionesComunes.LiberarObjetoGenerico(oRecord);
                return formato;
            }
            catch (Exception)
            {
                return "";
            }
        }

        public static string ObtenerCuentaSys(string FormatCode)
        {
            try
            {
                string CtaSys = string.Empty;
                SAPbobsCOM.Recordset oRecord = (SAPbobsCOM.Recordset)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                string _query = String.Empty;

                FormatCode = FormatCode.Replace(".", string.Empty);
                FormatCode = FormatCode.Replace("-", string.Empty);

                _query = @"SELECT ""AcctCode"" FROM ""OACT""
                            WHERE REPLACE(REPLACE(""FormatCode"",'.',''),'-','') = '" + FormatCode + "'";
                oRecord.DoQuery(_query);

                if (!oRecord.EoF)
                {
                    CtaSys = oRecord.Fields.Item(0).Value.ToString();
                }
                Comun.FuncionesComunes.LiberarObjetoGenerico(oRecord);
                return CtaSys;
            }
            catch (Exception ex)
            {
                Comun.Mensajes.Errores(14, "ConsultasSBO_ObtenerCuentaSys->" + ex.Message);
                return string.Empty;
            }
        }

        public static string ObtenerSeparadorMiles()
        {
            try
            {
                string separador = ",";
                SAPbobsCOM.Recordset oRecord = (SAPbobsCOM.Recordset)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                string _query = String.Empty;

                _query = @"SELECT ""DecSep"", ""ThousSep"" FROM ""OADM""";

                oRecord.DoQuery(_query);

                // Si hay datos
                if (!oRecord.EoF)
                {
                    separador = oRecord.Fields.Item("ThousSep").Value.ToString();
                }
                Comun.FuncionesComunes.LiberarObjetoGenerico(oRecord);
                return separador;
            }
            catch (Exception)
            {
                return ",";
            }
        }

        public static string ObtenerSeparadorDecimal()
        {
            try
            {
                string separador = ".";
                SAPbobsCOM.Recordset oRecord = (SAPbobsCOM.Recordset)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                string _query = String.Empty;

                _query = @"SELECT DecSep, ThousSep FROM OADM";

                oRecord.DoQuery(_query);

                // Si hay datos
                if (!oRecord.EoF)
                {
                    separador = oRecord.Fields.Item("DecSep").Value.ToString();
                }
                Comun.FuncionesComunes.LiberarObjetoGenerico(oRecord);
                return separador;
            }
            catch (Exception)
            {
                return ".";
            }
        }

        public static string ObtenerMonedaLocal()
        {
            try
            {
                string moneda = "CLP";
                SAPbobsCOM.Recordset oRecord = (SAPbobsCOM.Recordset)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                string _query = String.Empty;

                _query = @"SELECT ""MainCurncy"" FROM ""OADM""";

                oRecord.DoQuery(_query);

                // Si hay datos
                if (!oRecord.EoF)
                {
                    moneda = oRecord.Fields.Item("MainCurncy").Value.ToString();
                }
                Comun.FuncionesComunes.LiberarObjetoGenerico(oRecord);
                return moneda;
            }
            catch (Exception)
            {
                return "CLP";
            }
        }

        public static string ObtenerMonedaSistema()
        {
            try
            {
                string moneda = "CLP";
                SAPbobsCOM.Recordset oRecord = (SAPbobsCOM.Recordset)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                string _query = String.Empty;

                _query = @"SELECT ""SysCurrncy"" FROM ""OADM""";

                oRecord.DoQuery(_query);

                // Si hay datos
                if (!oRecord.EoF)
                {
                    moneda = oRecord.Fields.Item("SysCurrncy").Value.ToString();
                }
                Comun.FuncionesComunes.LiberarObjetoGenerico(oRecord);
                return moneda;
            }
            catch (Exception)
            {
                return "CLP";
            }
        }

        public static string ObtenerCodigoMonedaISO(string ISOCurrCod)
        {
            try
            {
                string moneda = "CLP";
                SAPbobsCOM.Recordset oRecord = (SAPbobsCOM.Recordset)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                string _query = String.Empty;

                _query = @"SELECT ""CurrCode"" FROM ""OCRN"" WHERE ""ISOCurrCod"" = '" + ISOCurrCod + "'";

                oRecord.DoQuery(_query);

                // Si hay datos
                if (!oRecord.EoF)
                {
                    moneda = oRecord.Fields.Item("CurrCode").Value.ToString();
                }
                Comun.FuncionesComunes.LiberarObjetoGenerico(oRecord);
                return moneda;
            }
            catch (Exception)
            {
                return "CLP";
            }
        }

        /// <summary>
        /// Función que verifica la estructura del AddOn
        /// </summary>
        public static Boolean VerificaEstructura()
        {
            try
            {
                Clases.Configuracion ExtConf = new Clases.Configuracion();
                string localizacion = string.Empty;
                localizacion = ExtConf.Parametros.Localizacion;

                bool existe = false;
                SAPbobsCOM.Recordset oRecord = (SAPbobsCOM.Recordset)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                string _query = String.Empty;

                _query = @"SELECT * FROM ""OUDO"" WHERE ""Code"" IN ('SO_RENDICION','SO_RENDIGTO','SO_RENDICF')";
                oRecord.DoQuery(_query);
                // Si hay datos
                if (!oRecord.EoF)
                {
                    if (oRecord.RecordCount == 3)
                    {
                        existe = true;
                        _query = @"SELECT * FROM ""CUFD"" WHERE ""TableID"" = '@SO_RENDICF' AND ""AliasID"" IN ('LOCALIZ','C_CTACFAC','C_CTAPREM')";
                        oRecord.DoQuery(_query);
                        // Si hay datos
                        if (!oRecord.EoF)
                        {
                            if (oRecord.RecordCount == 3)
                            {
                                existe = true;
                                if (localizacion.Equals("SOINDUS") || localizacion.Equals("ICLINICS") || localizacion.Equals("PCLINICS") || localizacion.Equals("LATINEQ") || localizacion.Equals("LATINEUY") || localizacion.Equals("SUATRANS") || localizacion.Equals("SUATRANSTR") || localizacion.Equals("SUATRANSPE"))
                                {
                                    _query = @"SELECT * FROM ""CUFD"" WHERE ""TableID"" = 'OINV' AND ""AliasID"" IN ('SO_NUMER')";
                                    oRecord.DoQuery(_query);
                                    // Si hay datos
                                    if (!oRecord.EoF)
                                    {
                                        if (oRecord.RecordCount > 0)
                                        {
                                            existe = true;
                                            if (localizacion.Equals("LATINEQ") || localizacion.Equals("LATINEUY"))
                                            {
                                                _query = @"SELECT * FROM ""CUFD"" WHERE ""TableID"" = '@SO_RENDICAT' AND ""AliasID"" IN ('CAT1','CAT2','CAT3','CAT4','CAT5','CTACTB')";
                                                oRecord.DoQuery(_query);
                                                // Si hay datos
                                                if (!oRecord.EoF)
                                                {
                                                    if (oRecord.RecordCount == 6)
                                                    {
                                                        existe = true;
                                                    }
                                                    else
                                                    {
                                                        existe = false;
                                                    }
                                                }
                                                else
                                                {
                                                    existe = false;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            existe = false;
                                        }
                                    }
                                    else
                                    {
                                        existe = false;
                                    }
                                }
                            }
                            else
                            {
                                existe = false;
                            }
                        }
                        else
                        {
                            existe = false;
                        }
                    }
                }
                else
                {
                    existe = false;
                }
                Comun.FuncionesComunes.LiberarObjetoGenerico(oRecord);
                return existe;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Función que retorna si existe el registro de configuración
        /// </summary>
        public static Boolean ExisteConfiguracion()
        {
            try
            {
                bool existe = false;
                SAPbobsCOM.Recordset oRecord = (SAPbobsCOM.Recordset)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                string _query = String.Empty;

                _query = @"SELECT * FROM ""@SO_RENDICF"" WHERE ""Code"" = 'CONF'";

                oRecord.DoQuery(_query);

                // Si hay datos
                if (!oRecord.EoF)
                {
                    existe = true;
                }
                Comun.FuncionesComunes.LiberarObjetoGenerico(oRecord);
                return existe;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Función que retorna los parámetros de configuración
        /// </summary>
        /// <returns></returns>
        public static Clases.ConfiguracionParams ObtenerConfiguracion()
        {
            Clases.ConfiguracionParams ret = new Clases.ConfiguracionParams();
            ret.Token = string.Empty;
            ret.Rut_Sociedad = string.Empty;
            ret.Localizacion = string.Empty;
            ret.Campo_Centro_Costo = "Centro de Costo";
            ret.Campo_Centro_Costo_Valor = "CODE";
            ret.Campo_Tipo_Documento = "Tipo de Documento";
            ret.Campo_Tipo_Documento_Valor = "CODE";
            ret.Codigo_Factura_Afecta = "01";
            ret.Codigo_Factura_Exenta = "02";
            ret.Codigo_Factura_Materiales = "04";
            ret.Campo_Rut_Proveedor = "RUT";
            ret.Campo_Rut_Proveedor_Valor = "VALUE";
            ret.Campo_Numero_Documento = "Número de Documento";
            ret.Campo_Numero_Documento_Valor = "VALUE";
            ret.Cuenta_Anticipo_Proveedores = string.Empty;
            ret.Cuenta_Servicios_Proveedores = string.Empty;
            ret.Cuenta_Compra_Materiales = string.Empty;
            ret.Cuenta_Compensacion_Facturas = string.Empty;
            ret.Cuenta_Pago_Reembolsos = string.Empty;

            SAPbobsCOM.Recordset oRecord = (SAPbobsCOM.Recordset)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            string _query = String.Empty;

            try
            {
                _query = @"SELECT * FROM ""@SO_RENDICF"" WHERE ""Code"" = 'CONF'";
                oRecord.DoQuery(_query);

                // Si no hay datos
                if (!oRecord.EoF)
                {
                    ret.Token = oRecord.Fields.Item("U_TOKEN").Value.ToString();
                    ret.Rut_Sociedad = oRecord.Fields.Item("U_RUTSOC").Value.ToString();
                    ret.Localizacion = oRecord.Fields.Item("U_LOCALIZ").Value.ToString();
                    ret.Campo_Centro_Costo = oRecord.Fields.Item("U_G_CCOST").Value.ToString();
                    ret.Campo_Centro_Costo_Valor = oRecord.Fields.Item("U_G_CCOSTV").Value.ToString();
                    ret.Campo_Tipo_Documento = oRecord.Fields.Item("U_G_TIPOD").Value.ToString();
                    ret.Campo_Tipo_Documento_Valor = oRecord.Fields.Item("U_G_TIPODV").Value.ToString();
                    ret.Codigo_Factura_Afecta = oRecord.Fields.Item("U_G_CODFA").Value.ToString();
                    ret.Codigo_Factura_Exenta = oRecord.Fields.Item("U_G_CODFE").Value.ToString();
                    ret.Codigo_Factura_Materiales = oRecord.Fields.Item("U_G_CODFM").Value.ToString();
                    ret.Campo_Rut_Proveedor = oRecord.Fields.Item("U_G_RUTP").Value.ToString();
                    ret.Campo_Rut_Proveedor_Valor = oRecord.Fields.Item("U_G_RUTPV").Value.ToString();
                    ret.Campo_Numero_Documento = oRecord.Fields.Item("U_G_NDOC").Value.ToString();
                    ret.Campo_Numero_Documento_Valor = oRecord.Fields.Item("U_G_NDOCV").Value.ToString();
                    ret.Cuenta_Anticipo_Proveedores = oRecord.Fields.Item("U_C_CTAANTP").Value.ToString();
                    ret.Cuenta_Servicios_Proveedores = oRecord.Fields.Item("U_C_CTASERP").Value.ToString();
                    ret.Cuenta_Compra_Materiales = oRecord.Fields.Item("U_C_CTACMAT").Value.ToString();
                    ret.Cuenta_Compensacion_Facturas = oRecord.Fields.Item("U_C_CTACFAC").Value.ToString();
                    ret.Cuenta_Pago_Reembolsos = oRecord.Fields.Item("U_C_CTAPREM").Value.ToString();
                }
            }
            catch (Exception ex)
            {
            }
            Comun.FuncionesComunes.LiberarObjetoGenerico(oRecord);
            return ret;
        }

        /// <summary>
        /// Función que retorna parametros de conexión de proveedor DTE, devuelve true si existen
        /// </summary>
        public static Boolean ObtenerParametrosConexion(ref String Token, ref String RutEmpresa, ref String Recinto)
        {
            Boolean existe = false;
            SAPbobsCOM.Recordset oRecord = (SAPbobsCOM.Recordset)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            string _query = String.Empty;

            _query = @"SELECT U_SO_TOKEN, U_SO_RRECEP, U_SO_RECINTO FROM OADM";

            oRecord.DoQuery(_query);

            // Si no hay datos
            if (oRecord.EoF)
            {
                existe = false;
            }
            // si hay datos
            else
            {
                existe = true;
                Token = oRecord.Fields.Item(0).Value.ToString();
                RutEmpresa = oRecord.Fields.Item(1).Value.ToString();
                Recinto = oRecord.Fields.Item(2).Value.ToString();
            }
            Comun.FuncionesComunes.LiberarObjetoGenerico(oRecord);
            return existe;
        }

        /// <summary>
        /// Función que retorna el rut de la empresa receptora de DTE
        /// </summary>
        /// <returns></returns>
        public static string ObtenerRutEmpresaReceptora()
        {
            try
            {
                string rut = string.Empty;
                SAPbobsCOM.Recordset oRecord = (SAPbobsCOM.Recordset)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                string _query = String.Empty;

                _query = @"SELECT U_SO_RRECEP FROM OADM";

                oRecord.DoQuery(_query);

                // Si no hay datos
                if (oRecord.EoF)
                {
                    rut = "76035224-1";
                }
                // si hay datos
                else
                {
                    rut = oRecord.Fields.Item(0).Value.ToString();
                }
                Comun.FuncionesComunes.LiberarObjetoGenerico(oRecord);
                return rut;
            }
            catch (Exception)
            {
                return "76035224-1";
            }
        }

        /// <summary>
        /// Función que retorna la activación de Multi Branch, devuelve true si está activo
        /// </summary>
        /// <returns></returns>
        public static Boolean MultiBranchActivo()
        {
            Boolean activo = false;
            SAPbobsCOM.Recordset oRecord = (SAPbobsCOM.Recordset)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            string _query = String.Empty;

            _query = @"SELECT ""MltpBrnchs"" FROM ""OADM""";

            oRecord.DoQuery(_query);

            // Si no hay datos
            if (oRecord.EoF)
            {
                activo = false;
            }
            // si hay datos
            else
            {
                string MltpBrnchs = oRecord.Fields.Item(0).Value.ToString();
                if (MltpBrnchs.Equals("Y"))
                {
                    activo = true;
                }
            }
            Comun.FuncionesComunes.LiberarObjetoGenerico(oRecord);
            return activo;
        }

        /// <summary>
        /// Función que obtiene el Cardcode del socio de negocio de la BD
        /// </summary>
        public static String ObtenerCardCode(String Rut)
        {
            try
            {
                String Cardcode = String.Empty;
                SAPbobsCOM.Recordset oRecord = (SAPbobsCOM.Recordset)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                string _query = String.Empty;

                Rut = Rut.Replace(".", String.Empty);
                Rut = Rut.Replace("-", String.Empty);

                _query = @"SELECT ""CardCode"" FROM ""OCRD""
                            WHERE REPLACE(REPLACE(""LicTradNum"",'.',''),'-','') = '" + Rut + "'";
                oRecord.DoQuery(_query);

                if (!oRecord.EoF)
                {
                    Cardcode = oRecord.Fields.Item(0).Value.ToString();
                }
                Comun.FuncionesComunes.LiberarObjetoGenerico(oRecord);
                return Cardcode;
            }
            catch (Exception ex)
            {
                Comun.Mensajes.Errores(14, "ConsultasSBO_ObtenerCardCode->" + ex.Message);
                return String.Empty;
            }
        }

        /// <summary>
        /// Función que obtiene el Cardcode del socio de negocio de la BD
        /// </summary>
        public static String ObtenerCardCodeCliente(String Rut)
        {
            try
            {
                String Cardcode = String.Empty;
                SAPbobsCOM.Recordset oRecord = (SAPbobsCOM.Recordset)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                string _query = String.Empty;

                Rut = Rut.Replace(".", String.Empty);
                Rut = Rut.Replace("-", String.Empty);

                _query = @"SELECT ""CardCode"" FROM ""OCRD""
                            WHERE REPLACE(REPLACE(""LicTradNum"",'.',''),'-','') = '" + Rut + @"' AND ""CardType"" = 'C' ";
                oRecord.DoQuery(_query);

                if (!oRecord.EoF)
                {
                    Cardcode = oRecord.Fields.Item(0).Value.ToString();
                }
                Comun.FuncionesComunes.LiberarObjetoGenerico(oRecord);
                return Cardcode;
            }
            catch (Exception ex)
            {
                Comun.Mensajes.Errores(14, "ConsultasSBO_ObtenerCardCodeCliente->" + ex.Message);
                return String.Empty;
            }
        }

        /// <summary>
        /// Función que obtiene el Cardcode del socio de negocio de la BD
        /// </summary>
        public static String ObtenerCardCodeClienteConLetra(String Rut, String Letra)
        {
            try
            {
                String Cardcode = String.Empty;
                SAPbobsCOM.Recordset oRecord = (SAPbobsCOM.Recordset)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                string _query = String.Empty;

                Rut = Rut.Replace(".", String.Empty);
                Rut = Rut.Replace("-", String.Empty);

                _query = @"SELECT ""CardCode"" FROM ""OCRD""
                            WHERE ""CardType"" = 'C' AND ""CardCode"" LIKE '" + Letra + @"%' AND REPLACE(REPLACE(""LicTradNum"",'.',''),'-','') = '" + Rut + "'";
                oRecord.DoQuery(_query);

                if (!oRecord.EoF)
                {
                    Cardcode = oRecord.Fields.Item(0).Value.ToString();
                }
                Comun.FuncionesComunes.LiberarObjetoGenerico(oRecord);
                return Cardcode;
            }
            catch (Exception ex)
            {
                Comun.Mensajes.Errores(14, "ConsultasSBO_ObtenerCardCodeClienteConLetra->" + ex.Message);
                return String.Empty;
            }
        }

        /// <summary>
        /// Función que obtiene el Cardcode del socio de negocio de la BD
        /// </summary>
        public static String ObtenerCardCodeProveedor(String Rut)
        {
            try
            {
                String Cardcode = String.Empty;
                SAPbobsCOM.Recordset oRecord = (SAPbobsCOM.Recordset)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                string _query = String.Empty;

                Rut = Rut.Replace(".", String.Empty);
                Rut = Rut.Replace("-", String.Empty);

                _query = @"SELECT ""CardCode"" FROM ""OCRD""
                            WHERE REPLACE(REPLACE(""LicTradNum"",'.',''),'-','') = '" + Rut + @"' AND ""CardType"" = 'S' ";
                oRecord.DoQuery(_query);

                if (!oRecord.EoF)
                {
                    Cardcode = oRecord.Fields.Item(0).Value.ToString();
                }
                Comun.FuncionesComunes.LiberarObjetoGenerico(oRecord);
                return Cardcode;
            }
            catch (Exception ex)
            {
                Comun.Mensajes.Errores(14, "ConsultasSBO_ObtenerCardCodeProveedor->" + ex.Message);
                return String.Empty;
            }
        }

        /// <summary>
        /// Función que obtiene el Cardcode del socio de negocio de la BD
        /// </summary>
        public static String ObtenerCardCodeProveedorConLetra(String Rut, String Letra)
        {
            try
            {
                String Cardcode = String.Empty;
                SAPbobsCOM.Recordset oRecord = (SAPbobsCOM.Recordset)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                string _query = String.Empty;

                Rut = Rut.Replace(".", String.Empty);
                Rut = Rut.Replace("-", String.Empty);

                _query = @"SELECT ""CardCode"" FROM ""OCRD""
                            WHERE ""CardType"" = 'S' AND ""CardCode"" LIKE '" + Letra + @"%' AND REPLACE(REPLACE(""LicTradNum"",'.',''),'-','') = '" + Rut + "'";
                oRecord.DoQuery(_query);

                if (!oRecord.EoF)
                {
                    Cardcode = oRecord.Fields.Item(0).Value.ToString();
                }
                Comun.FuncionesComunes.LiberarObjetoGenerico(oRecord);
                return Cardcode;
            }
            catch (Exception ex)
            {
                Comun.Mensajes.Errores(14, "ConsultasSBO_ObtenerCardCodeProveedorConLetra->" + ex.Message);
                return String.Empty;
            }
        }

        /// <summary>
        /// Función que recupera el DocNum del documento
        /// </summary>
        public static string RecuperaDocNum(String DocEntry)
        {
            string DocNum = string.Empty;
            SAPbobsCOM.Recordset oRecord = (SAPbobsCOM.Recordset)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            string _query = String.Empty;

            _query = @"SELECT ""DocNum"" FROM ""OPCH"" WHERE ""DocEntry"" = " + DocEntry + "";
            oRecord.DoQuery(_query);

            // Si no hay datos
            if (oRecord.EoF)
            {
            }
            // si hay datos
            else
            {
                DocNum = oRecord.Fields.Item(0).Value.ToString();
            }
            Comun.FuncionesComunes.LiberarObjetoGenerico(oRecord);
            return DocNum;
        }

        /// <summary>
        /// Función que obtiene la Dimension del Centro de Costo
        /// </summary>
        public static string ObtenerDimensionCC(String CentroCosto)
        {
            try
            {
                String DimCode = String.Empty;
                SAPbobsCOM.Recordset oRecord = (SAPbobsCOM.Recordset)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                string _query = String.Empty;

                _query = @"SELECT ""DimCode"" FROM ""OPRC""
                            WHERE ""PrcCode"" = '" + CentroCosto + "'";
                oRecord.DoQuery(_query);

                if (!oRecord.EoF)
                {
                    DimCode = oRecord.Fields.Item(0).Value.ToString();
                }
                Comun.FuncionesComunes.LiberarObjetoGenerico(oRecord);
                return DimCode;
            }
            catch (Exception ex)
            {
                Comun.Mensajes.Errores(14, "ConsultasSBO_ObtenerDimensionCC->" + ex.Message);
                return String.Empty;
            }
        }

        /// <summary>
        /// Función que obtiene el Centro de Costo de la Dimension 1 según el Centro de Costo
        /// </summary>
        public static string ObtenerCCDimension1(String CentroCosto)
        {
            try
            {
                String CCTypeCode = String.Empty;
                SAPbobsCOM.Recordset oRecord = (SAPbobsCOM.Recordset)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                string _query = String.Empty;

                _query = @"SELECT ""CCTypeCode"" FROM ""OPRC""
                            WHERE ""PrcCode"" = '" + CentroCosto + "'";
                oRecord.DoQuery(_query);

                if (!oRecord.EoF)
                {
                    CCTypeCode = oRecord.Fields.Item(0).Value.ToString();
                }
                Comun.FuncionesComunes.LiberarObjetoGenerico(oRecord);
                return CCTypeCode;
            }
            catch (Exception ex)
            {
                Comun.Mensajes.Errores(14, "ConsultasSBO_ObtenerCCTypeCode->" + ex.Message);
                return String.Empty;
            }
        }

        /// <summary>
        /// Función que obtiene el Centro de Costo de la Dimension 3 según la Cuenta Contable
        /// </summary>
        public static string ObtenerCCDimension3(String CtaContable)
        {
            try
            {
                String Naturaleza = String.Empty;
                SAPbobsCOM.Recordset oRecord = (SAPbobsCOM.Recordset)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                string _query = String.Empty;

                _query = @"SELECT ""U_SYP_NATURALEZA"" FROM ""OACT""
                            WHERE ""AcctCode"" = '" + CtaContable + "'";
                oRecord.DoQuery(_query);

                if (!oRecord.EoF)
                {
                    Naturaleza = oRecord.Fields.Item(0).Value.ToString();
                }
                Comun.FuncionesComunes.LiberarObjetoGenerico(oRecord);
                return Naturaleza;
            }
            catch (Exception ex)
            {
                Comun.Mensajes.Errores(14, "ConsultasSBO_ObtenerCCTypeCode->" + ex.Message);
                return String.Empty;
            }
        }

        /// <summary>
        /// Función que obtiene la Cta Contable del Gasto según Categorías
        /// </summary>
        public static string ObtenerCtaCtbSegunCategorias(string cat1 = "", string cat2 = "", string cat3 = "", string cat4 = "", string cat5 = "")
        {
            try
            {
                String CtaCtb = String.Empty;
                SAPbobsCOM.Recordset oRecord = (SAPbobsCOM.Recordset)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                string _query = String.Empty;

                _query = @"SELECT ""U_CTACTB"" FROM ""@SO_RENDICAT""
                            WHERE 1 = 1";
                if (!string.IsNullOrEmpty(cat1))
                {
                    _query += @" AND ""U_CAT1"" = '" + cat1 + "'";
                }
                if (!string.IsNullOrEmpty(cat2))
                {
                    _query += @" AND ""U_CAT2"" = '" + cat2 + "'";
                }
                if (!string.IsNullOrEmpty(cat3))
                {
                    _query += @" AND ""U_CAT3"" = '" + cat3 + "'";
                }
                if (!string.IsNullOrEmpty(cat4))
                {
                    _query += @" AND ""U_CAT4"" = '" + cat4 + "'";
                }
                if (!string.IsNullOrEmpty(cat5))
                {
                    _query += @" AND ""U_CAT5"" = '" + cat5 + "'";
                }
                oRecord.DoQuery(_query);

                if (!oRecord.EoF)
                {
                    CtaCtb = oRecord.Fields.Item(0).Value.ToString();
                }
                Comun.FuncionesComunes.LiberarObjetoGenerico(oRecord);
                return CtaCtb;
            }
            catch (Exception ex)
            {
                Comun.Mensajes.Errores(14, "ConsultasSBO_ObtenerCtaCtbSegunCategorias->" + ex.Message);
                return String.Empty;
            }
        }
    }
}
