using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soindus.AddOnMonitorProveedores.SBO
{
    public class ConsultasSBO
    {
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
                Local.FuncionesComunes.LiberarObjetoGenerico(oRecord);
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

                _query = @"SELECT ""DecSep"", ""ThousSep"" FROM ""OADM""";

                oRecord.DoQuery(_query);

                // Si hay datos
                if (!oRecord.EoF)
                {
                    separador = oRecord.Fields.Item("DecSep").Value.ToString();
                }
                Local.FuncionesComunes.LiberarObjetoGenerico(oRecord);
                return separador;
            }
            catch (Exception)
            {
                return ".";
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

                _query = @"SELECT * FROM ""@SO_MONCONF"" WHERE ""Code"" = 'CONF'";

                oRecord.DoQuery(_query);

                // Si hay datos
                if (!oRecord.EoF)
                {
                    existe = true;
                }
                Local.FuncionesComunes.LiberarObjetoGenerico(oRecord);
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
        public static Local.ConfiguracionParams ObtenerConfiguracion()
        {
            Local.ConfiguracionParams ret = new Local.ConfiguracionParams();
            try
            {
                SAPbobsCOM.Recordset oRecord = (SAPbobsCOM.Recordset)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                string _query = String.Empty;

                _query = @"SELECT * FROM ""@SO_MONCONF"" WHERE ""Code"" = 'CONF'";

                oRecord.DoQuery(_query);

                // Si no hay datos
                if (oRecord.EoF)
                {
                    ret.Proveedor_FE = string.Empty;
                    ret.Token = string.Empty;
                    ret.Rut_Receptor = string.Empty;
                    ret.Recinto = string.Empty;
                    ret.Visualiza_Responsable = false;
                    ret.Visualiza_Responsable_Doc = false;
                    ret.Visualiza_Cesion = false;
                    ret.Valida_ExisteOC = true;
                    ret.Valida_ExisteEntrada = false;
                    ret.Valida_MontoMaximo = false;
                    ret.Valida_ValorMontoMaximo = 0;
                    ret.Valida_PermiteOCManual = false;
                    ret.Valida_EncabezadosDTE = false;
                    ret.Valida_MontoOC = false;
                    ret.Valida_PermiteTolerancias = false;
                    ret.Valida_ValorRechazoMontoMenor = 0;
                    ret.Valida_ValorRechazoMontoMayor = 0;
                    ret.Valida_ValorAprobacionMontoMenor = 0;
                    ret.Valida_ValorAprobacionMontoMayor = 0;
                    ret.Integracion_SolicitaMultiBranch = false;
                }
                // si hay datos
                else
                {
                    ret.Proveedor_FE = oRecord.Fields.Item("U_PROVFE").Value.ToString();
                    ret.Token = oRecord.Fields.Item("U_TOKEN").Value.ToString();
                    ret.Rut_Receptor = oRecord.Fields.Item("U_RRECEP").Value.ToString();
                    ret.Recinto = oRecord.Fields.Item("U_RECINTO").Value.ToString();
                    ret.Visualiza_Responsable = oRecord.Fields.Item("U_G_RESP").Value.ToString() == "Y" ? true : false;
                    ret.Visualiza_Responsable_Doc = oRecord.Fields.Item("U_G_RESPD").Value.ToString() == "Y" ? true : false;
                    ret.Visualiza_Cesion = oRecord.Fields.Item("U_G_CESION").Value.ToString() == "Y" ? true : false;
                    ret.Valida_ExisteOC = oRecord.Fields.Item("U_V_OC").Value.ToString() == "Y" ? true : false;
                    ret.Valida_ExisteEntrada = oRecord.Fields.Item("U_V_ENT").Value.ToString() == "Y" ? true : false;
                    ret.Valida_MontoMaximo = oRecord.Fields.Item("U_V_MTO").Value.ToString() == "Y" ? true : false;
                    ret.Valida_ValorMontoMaximo = (double)oRecord.Fields.Item("U_V_MTOVAL").Value;
                    ret.Valida_PermiteOCManual = oRecord.Fields.Item("U_V_OCMAN").Value.ToString() == "Y" ? true : false;
                    ret.Valida_EncabezadosDTE = oRecord.Fields.Item("U_V_ENCDTE").Value.ToString() == "Y" ? true : false;
                    ret.Valida_MontoOC = oRecord.Fields.Item("U_V_MTOOC").Value.ToString() == "Y" ? true : false;
                    ret.Valida_PermiteTolerancias = oRecord.Fields.Item("U_V_TOLER").Value.ToString() == "Y" ? true : false;
                    ret.Valida_ValorRechazoMontoMenor = (double)oRecord.Fields.Item("U_V_RMEVAL").Value;
                    ret.Valida_ValorRechazoMontoMayor = (double)oRecord.Fields.Item("U_V_RMAVAL").Value;
                    ret.Valida_ValorAprobacionMontoMenor = (double)oRecord.Fields.Item("U_V_AMEVAL").Value;
                    ret.Valida_ValorAprobacionMontoMayor = (double)oRecord.Fields.Item("U_V_AMAVAL").Value;
                    ret.Integracion_SolicitaMultiBranch = oRecord.Fields.Item("U_I_MB").Value.ToString() == "Y" ? true : false;
                }
                Local.FuncionesComunes.LiberarObjetoGenerico(oRecord);
                return ret;
            }
            catch (Exception)
            {
                return ret;
            }
        }

        /// <summary>
        /// Función que retorna parametros de conexión de proveedor DTE, devuelve true si existen
        /// </summary>
        //public static Boolean ObtenerParametrosConexion(ref String Token, ref String RutEmpresa, ref String Recinto)
        //{
        //    Boolean existe = false;
        //    SAPbobsCOM.Recordset oRecord = (SAPbobsCOM.Recordset)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
        //    string _query = String.Empty;

        //    _query = @"SELECT U_SO_TOKEN, U_SO_RRECEP, U_SO_RECINTO FROM OADM";

        //    oRecord.DoQuery(_query);

        //    // Si no hay datos
        //    if (oRecord.EoF)
        //    {
        //        existe = false;
        //    }
        //    // si hay datos
        //    else
        //    {
        //        existe = true;
        //        Token = oRecord.Fields.Item(0).Value.ToString();
        //        RutEmpresa = oRecord.Fields.Item(1).Value.ToString();
        //        Recinto = oRecord.Fields.Item(2).Value.ToString();
        //    }
        //    return existe;
        //}

        /// <summary>
        /// Función que retorna el rut de la empresa receptora de DTE
        /// </summary>
        /// <returns></returns>
        //public static string ObtenerRutEmpresaReceptora()
        //{
        //    try
        //    {
        //        string rut = string.Empty;
        //        SAPbobsCOM.Recordset oRecord = (SAPbobsCOM.Recordset)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
        //        string _query = String.Empty;

        //        _query = @"SELECT U_SO_RRECEP FROM OADM";

        //        oRecord.DoQuery(_query);

        //        // Si no hay datos
        //        if (oRecord.EoF)
        //        {
        //            rut = "76035224-1";
        //        }
        //        // si hay datos
        //        else
        //        {
        //            rut = oRecord.Fields.Item(0).Value.ToString();
        //        }
        //        return rut;
        //    }
        //    catch (Exception)
        //    {
        //        return "76035224-1";
        //    }
        //}

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
            Local.FuncionesComunes.LiberarObjetoGenerico(oRecord);
            return activo;
        }

        /// <summary>
        /// Función que valida si un DTE se ha integrado antes o no.
        /// Retorna true si se puede integrar, false si ya existe y no se puede integrar
        /// </summary>
        public static Local.Message ValidacionDTEIntegrado(String Rut, Int32 Tipo, Int64 Folio)
        {
            Local.Message result = new Local.Message();
            SAPbobsCOM.Recordset oRecord = (SAPbobsCOM.Recordset)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            string _query = String.Empty;

            try
            {
                _query = @"SELECT 1 FROM ""OPCH"" ""T0"" WHERE REPLACE(""T0"".""LicTradNum"",'.','') = '" + Rut + @"' AND ""T0"".""Indicator"" = '" + Tipo + @"' AND ""T0"".""FolioNum"" = " + Folio + @"
                          UNION
                          SELECT 1 FROM ""ORPC"" ""T0"" WHERE REPLACE(""T0"".""LicTradNum"",'.','') = '" + Rut + @"' AND ""T0"".""Indicator"" = '" + Tipo + @"' AND ""T0"".""FolioNum"" = " + Folio + @"
                          UNION
                          SELECT 1 FROM ""OPDN"" ""T0"" WHERE REPLACE(""T0"".""LicTradNum"",'.','') = '" + Rut + @"' AND ""T0"".""Indicator"" = '" + Tipo + @"' AND ""T0"".""FolioNum"" = " + Folio + @"";
                oRecord.DoQuery(_query);

                // Si no hay datos
                if (oRecord.EoF)
                {
                    result.Success = true;
                }
                // si hay datos
                else
                {
                    result.Success = false;
                    result.Mensaje = String.Format("El DTE Nro {0} de Tipo {1} del proveedor {2} ya se encuentra integrado", Folio, Tipo, Rut);
                }
                Local.FuncionesComunes.LiberarObjetoGenerico(oRecord);
                return result;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Mensaje = ex.Message;
                return result;
            }
        }

        /// <summary>
        /// Función que obtiene el Cardcode del socio de negocio de la BD
        /// </summary>
        public static String ObtenerCardcode(String Rut)
        {
            try
            {
                String Cardcode = String.Empty;
                SAPbobsCOM.Recordset oRecord = (SAPbobsCOM.Recordset)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                string _query = String.Empty;

                Rut = Rut.Replace(".", String.Empty);

                _query = @"SELECT ""CardCode"" FROM ""OCRD""
                            WHERE ""CardType""='S' AND REPLACE(""LicTradNum"",'.','')='" + Rut + "'";
                oRecord.DoQuery(_query);

                if (!oRecord.EoF)
                {
                    Cardcode = oRecord.Fields.Item(0).Value.ToString();
                }
                Local.FuncionesComunes.LiberarObjetoGenerico(oRecord);
                return Cardcode;
            }
            catch (Exception ex)
            {
                Local.Mensajes.Errores(14, "ConsultasSBO_ObtenerCardcode->" + ex.Message);
                return String.Empty;
            }
        }

        /// <summary>
        /// Función que valida la existencia del documento de referencia en SAP, devuelve true si existe
        /// </summary>
        public static Boolean ExisteReferencia(String TipoRef, String Folio, String Cardcode, ref String DocEntryBase, ref String DocTypeBase, ref String ObjTypeBase, Int32 TipoDoc)
        {
            Boolean existe = false;
            Int64 lFolio = 0;
            Int64.TryParse(Folio, out lFolio);
            SAPbobsCOM.Recordset oRecord = (SAPbobsCOM.Recordset)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            string _query = String.Empty;

            if (TipoDoc.Equals(56) || TipoDoc.Equals(61))
            {
                // Factura - OPCH
                _query = @"SELECT ""DocEntry"", ""DocType"", ""ObjType"" FROM ""OPCH"" WHERE ""CardCode"" = '" + Cardcode + @"' AND ""FolioPref"" = '" + TipoRef + @"' AND ""FolioNum"" = '" + lFolio + "'";
                oRecord.DoQuery(_query);
            }
            else if (TipoDoc.Equals(33) || TipoDoc.Equals(34) || TipoDoc.Equals(52))
            {
                // Guia - OPDN
                if (TipoRef.Equals("52"))
                {
                    _query = @"SELECT ""DocEntry"", ""DocType"", ""ObjType"" FROM ""OPDN"" WHERE ""CardCode"" = '" + Cardcode + @"' AND ""FolioPref"" = '" + TipoRef + @"' AND ""FolioNum"" = '" + lFolio + "'";
                }
                // OC - OPOR
                else
                {
                    //_query = @"SELECT DocEntry FROM OPOR WHERE CardCode = '" + Cardcode + "' AND NumAtCard = '" + lFolio + "'";
                    _query = @"SELECT ""DocEntry"", ""DocType"", ""ObjType"" FROM ""OPOR"" WHERE ""CardCode"" = '" + Cardcode + @"' AND ""DocNum"" = '" + lFolio + "'";
                }
                oRecord.DoQuery(_query);
            }

            // Si no hay datos
            if (!oRecord.EoF)
            {
                existe = true;
                DocEntryBase = oRecord.Fields.Item(0).Value.ToString();
                DocTypeBase = oRecord.Fields.Item(1).Value.ToString();
                ObjTypeBase = oRecord.Fields.Item(2).Value.ToString();
            }
            // si hay datos
            else
            {
                existe = false;
            }
            Local.FuncionesComunes.LiberarObjetoGenerico(oRecord);
            return existe;
        }

        /// <summary>
        /// Función que obtiene si el documento de referencia en SAP está abierto, devuelve 0 si no existen documentos abiertos
        /// </summary>
        public static long ObtenerReferenciaAbierta(String TipoRef, String Folio, String Cardcode, Int32 TipoDoc)
        {
            long count = 0;
            Int64 lFolio = 0;
            Int64.TryParse(Folio, out lFolio);
            SAPbobsCOM.Recordset oRecord = (SAPbobsCOM.Recordset)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            string _query = String.Empty;

            if (TipoDoc.Equals(56) || TipoDoc.Equals(61))
            {
                // Factura - OPCH
                _query = @"SELECT COUNT(1) FROM ""OPCH"" WHERE ""CardCode"" = '" + Cardcode + @"' AND ""FolioPref"" = '" + TipoRef + @"' AND ""FolioNum"" = '" + lFolio + @"' AND ""DocStatus"" = 'O' ";
                oRecord.DoQuery(_query);
            }
            else if (TipoDoc.Equals(33) || TipoDoc.Equals(34) || TipoDoc.Equals(52))
            {
                // Guia - OPDN
                if (TipoRef.Equals("52"))
                {
                    _query = @"SELECT COUNT(1) FROM ""OPDN"" WHERE ""CardCode"" = '" + Cardcode + @"' AND ""FolioPref"" = '" + TipoRef + @"' AND ""FolioNum"" = '" + lFolio + @"' AND ""DocStatus"" = 'O' ";
                }
                // OC - OPOR
                else
                {
                    _query = @"SELECT COUNT(1) FROM ""OPOR"" WHERE ""CardCode"" = '" + Cardcode + @"' AND ""DocNum"" = '" + lFolio + @"' AND ""DocStatus"" = 'O' ";
                }
                oRecord.DoQuery(_query);
            }

            // Si hay datos
            if (!oRecord.EoF)
            {
                count = long.Parse(oRecord.Fields.Item(0).Value.ToString());
            }
            Local.FuncionesComunes.LiberarObjetoGenerico(oRecord);
            return count;
        }

        /// <summary>
        /// Función que valida la existencia del documento de referencia en SAP, devuelve true si existe
        /// </summary>
        public static Boolean ExisteEntrada(String DocEntryBase)
        {
            Boolean existe = false;
            SAPbobsCOM.Recordset oRecord = (SAPbobsCOM.Recordset)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            string _query = String.Empty;

            _query = @"SELECT ""T0"".""DocNum"" FROM ""OPDN"" ""T0"" INNER JOIN ""PDN1"" ""T1"" ON ""T0"".""DocEntry"" = ""T1"".""DocEntry"" WHERE ""T1"".""BaseType"" = '22' AND ""T1"".""BaseEntry"" = " + DocEntryBase + "";

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
            }
            Local.FuncionesComunes.LiberarObjetoGenerico(oRecord);
            return existe;
        }

        /// <summary>
        /// Función que obtiene si el documento de referencia en SAP tiene entradas abierta, devuelve 0 si no existen entradas abiertas
        /// </summary>
        public static long ObtenerEntradasAbiertas(String DocEntryBase)
        {
            long count = 0;
            SAPbobsCOM.Recordset oRecord = (SAPbobsCOM.Recordset)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            string _query = String.Empty;

            _query = @"SELECT COUNT(1) FROM ""OPDN"" ""T0"" INNER JOIN ""PDN1"" ""T1"" ON ""T0"".""DocEntry"" = ""T1"".""DocEntry"" WHERE ""T1"".""BaseType"" = '22' AND ""T1"".""BaseEntry"" = " + DocEntryBase + @" AND ""T0"".""DocStatus"" = 'O'";

            oRecord.DoQuery(_query);

            // Si hay datos
            if (!oRecord.EoF)
            {
                count = long.Parse(oRecord.Fields.Item(0).Value.ToString());
            }
            Local.FuncionesComunes.LiberarObjetoGenerico(oRecord);
            return count;
        }

        /// <summary>
        /// Función que obtiene si el Provedor es de tipo OC. Retorna true si es proveedor tipo OC
        /// </summary>
        public static bool EsProveedorOC(String CardCode)
        {
            try
            {
                bool resp = true;
                SAPbobsCOM.Recordset oRecord = (SAPbobsCOM.Recordset)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                string _query = String.Empty;

                if (SBO.ConexionSBO.oCompany.DbServerType.Equals(SAPbobsCOM.BoDataServerTypes.dst_HANADB))
                {
                    _query = @"SELECT IFNULL(""U_SO_PROVOC"", 'Y') FROM ""OCRD""
                            WHERE ""CardCode""='" + CardCode + "'";
                }
                else
                {
                    _query = @"SELECT ISNULL(""U_SO_PROVOC"", 'Y') FROM ""OCRD""
                            WHERE ""CardCode""='" + CardCode + "'";
                }
                oRecord.DoQuery(_query);

                if (!oRecord.EoF)
                {
                    resp = oRecord.Fields.Item(0).Value.ToString() == "Y" ? true : false;
                }
                Local.FuncionesComunes.LiberarObjetoGenerico(oRecord);
                return resp;
            }
            catch (Exception ex)
            {
                Local.Mensajes.Errores(14, "ConsultasSBO_EsProveedorOC->" + ex.Message);
                return true;
            }
        }

        /// <summary>
        /// Función que recupera el DocNum del documento
        /// </summary>
        public static string RecuperaDocNum(String TipoRef, String DocEntryBase, Int32 TipoDoc)
        {
            string DocNum = string.Empty;
            SAPbobsCOM.Recordset oRecord = (SAPbobsCOM.Recordset)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            string _query = String.Empty;

            if (TipoDoc.Equals(56) || TipoDoc.Equals(61))
            {
                // Factura - OPCH
                _query = @"SELECT ""DocNum"" FROM ""OPCH"" WHERE ""DocEntry"" = " + DocEntryBase + "";
                oRecord.DoQuery(_query);
            }
            else if (TipoDoc.Equals(33) || TipoDoc.Equals(34) || TipoDoc.Equals(52))
            {
                // Guia - OPDN
                if (TipoRef.Equals("52"))
                {
                    _query = @"SELECT ""DocNum"" FROM ""OPDN"" WHERE ""DocEntry"" = " + DocEntryBase + "";
                }
                // OC - OPOR
                else
                {
                    _query = @"SELECT ""DocNum"" FROM ""OPOR"" WHERE ""DocEntry"" = " + DocEntryBase + "";
                }
                oRecord.DoQuery(_query);
            }

            if (!oRecord.EoF)
            {
                DocNum = oRecord.Fields.Item(0).Value.ToString();
            }
            Local.FuncionesComunes.LiberarObjetoGenerico(oRecord);
            return DocNum;
        }

        /// <summary>
        /// Función que recupera el Responsable del documento
        /// </summary>
        public static string ObtenerResponsable(String TipoRef, String DocEntryBase, Int32 TipoDoc)
        {
            string SlpCode = string.Empty;
            SAPbobsCOM.Recordset oRecord = (SAPbobsCOM.Recordset)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            string _query = String.Empty;

            if (TipoDoc.Equals(56) || TipoDoc.Equals(61))
            {
                // Factura - OPCH
                _query = @"SELECT ""SlpCode"" FROM ""OPCH"" WHERE ""DocEntry"" = " + DocEntryBase + "";
            }
            else
            {
                // Guia - OPDN
                if (TipoRef.Equals("52"))
                {
                    _query = @"SELECT ""SlpCode"" FROM ""OPDN"" WHERE ""DocEntry"" = " + DocEntryBase + "";
                }
                // OC - OPOR
                else
                {
                    _query = @"SELECT ""SlpCode"" FROM ""OPOR"" WHERE ""DocEntry"" = " + DocEntryBase + "";
                }
            }

            oRecord.DoQuery(_query);

            // Si no hay datos
            if (oRecord.EoF)
            {
            }
            // si hay datos
            else
            {
                SlpCode = oRecord.Fields.Item(0).Value.ToString();
            }
            Local.FuncionesComunes.LiberarObjetoGenerico(oRecord);
            return SlpCode;
        }

        /// <summary>
        /// Función que recupera el Nombre del Responsable del documento
        /// </summary>
        public static string ObtenerNombreResponsable(String SlpCode)
        {
            string SlpName = string.Empty;
            SAPbobsCOM.Recordset oRecord = (SAPbobsCOM.Recordset)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            string _query = String.Empty;

            _query = @"SELECT ""SlpName"" FROM ""OSLP"" WHERE ""SlpCode"" = " + SlpCode + "";

            oRecord.DoQuery(_query);

            // Si no hay datos
            if (oRecord.EoF)
            {
            }
            // si hay datos
            else
            {
                SlpName = oRecord.Fields.Item(0).Value.ToString();
            }
            Local.FuncionesComunes.LiberarObjetoGenerico(oRecord);
            return SlpName;
        }

        /// <summary>
        /// Función que recupera el Empleado Responsable del SN
        /// </summary>
        public static string ObtenerResponsableSN(String CardCode)
        {
            string empID = string.Empty;
            SAPbobsCOM.Recordset oRecord = (SAPbobsCOM.Recordset)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            string _query = String.Empty;

            _query = @"SELECT ""U_SO_RESP"" FROM ""OCRD"" WHERE ""CardCode"" = '" + CardCode + "'";

            oRecord.DoQuery(_query);

            // Si no hay datos
            if (oRecord.EoF)
            {
            }
            // si hay datos
            else
            {
                empID = oRecord.Fields.Item(0).Value.ToString();
            }
            Local.FuncionesComunes.LiberarObjetoGenerico(oRecord);
            return empID;
        }

        /// <summary>
        /// Función que recupera el Nombre del Empleado
        /// </summary>
        public static string ObtenerNombreResponsableSN(String empID)
        {
            string EmpName = string.Empty;
            SAPbobsCOM.Recordset oRecord = (SAPbobsCOM.Recordset)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            string _query = String.Empty;

            _query = @"SELECT CONCAT(CONCAT(""firstName"", ' '), ""lastName"") AS ""EmpName"" FROM ""OHEM"" WHERE ""empID"" = " + empID + "";

            oRecord.DoQuery(_query);

            // Si no hay datos
            if (oRecord.EoF)
            {
            }
            // si hay datos
            else
            {
                EmpName = oRecord.Fields.Item(0).Value.ToString();
            }
            Local.FuncionesComunes.LiberarObjetoGenerico(oRecord);
            return EmpName;
        }

        /// <summary>
        /// Función que obtiene el CardName del socio de negocio de la BD
        /// </summary>
        public static String ObtenerCardName(String Rut, String CardCode)
        {
            try
            {
                String CardName = String.Empty;
                SAPbobsCOM.Recordset oRecord = (SAPbobsCOM.Recordset)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                string _query = String.Empty;


                Rut = Rut.Replace(".", String.Empty);

                _query = @"SELECT ""CardName"" FROM ""OCRD""
                            WHERE REPLACE(""LicTradNum"",'.','')='" + Rut + @"' AND ""CardCode"" = '" + CardCode + "'";
                oRecord.DoQuery(_query);

                if (!oRecord.EoF)
                {
                    CardName = oRecord.Fields.Item(0).Value.ToString();
                }
                Local.FuncionesComunes.LiberarObjetoGenerico(oRecord);
                return CardName;
            }
            catch (Exception ex)
            {
                Local.Mensajes.Errores(14, "ConsultasSBO_ObtenerCardName->" + ex.Message);
                return String.Empty;
            }
        }

        public static bool ValidaRazonSocial(string rznSocRecep)
        {
            bool resp = false;
            SAPbobsCOM.Recordset oRecord = (SAPbobsCOM.Recordset)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            string _query = String.Empty;
            try
            {
                _query = @"SELECT COUNT(""Code"") FROM ""@SO_ENCABEZADOSDTE""
                            WHERE ""Name"" = 'RAZON' AND REPLACE(""U_TEXTO"", ' ', '') = '" + rznSocRecep.ToUpper().Trim().Replace(" ","") + "'";
                oRecord.DoQuery(_query);

                if (!oRecord.EoF)
                {
                    resp = Int32.Parse(oRecord.Fields.Item(0).Value.ToString()) > 0 ? true : false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            Local.FuncionesComunes.LiberarObjetoGenerico(oRecord);
            return resp;
        }

        public static bool ValidaGiro(string giroRecep)
        {
            bool resp = false;
            SAPbobsCOM.Recordset oRecord = (SAPbobsCOM.Recordset)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            string _query = String.Empty;
            try
            {
                _query = @"SELECT COUNT(""Code"") FROM ""@SO_ENCABEZADOSDTE""
                            WHERE ""Name"" = 'GIRO' AND REPLACE(""U_TEXTO"", ' ', '') = '" + giroRecep.ToUpper().Trim().Replace(" ", "") + "'";
                oRecord.DoQuery(_query);

                if (!oRecord.EoF)
                {
                    resp = Int32.Parse(oRecord.Fields.Item(0).Value.ToString()) > 0 ? true : false;
                }
            }
            catch (Exception)
            {
                return false;
            }
            Local.FuncionesComunes.LiberarObjetoGenerico(oRecord);
            return resp;
        }

        public static bool ValidaDireccion(string dirRecep)
        {
            bool resp = false;
            SAPbobsCOM.Recordset oRecord = (SAPbobsCOM.Recordset)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            string _query = String.Empty;
            try
            {
                _query = @"SELECT COUNT(""Code"") FROM ""@SO_ENCABEZADOSDTE""
                            WHERE ""Name"" = 'DIRECCION' AND REPLACE(""U_TEXTO"", ' ', '') = '" + dirRecep.ToUpper().Trim().Replace(" ", "") + "'";
                oRecord.DoQuery(_query);

                if (!oRecord.EoF)
                {
                    resp = Int32.Parse(oRecord.Fields.Item(0).Value.ToString()) > 0 ? true : false;
                }
            }
            catch (Exception)
            {
                return false;
            }
            Local.FuncionesComunes.LiberarObjetoGenerico(oRecord);
            return resp;
        }

        public static bool ValidaComuna(string cmnaRecep)
        {
            bool resp = false;
            SAPbobsCOM.Recordset oRecord = (SAPbobsCOM.Recordset)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            string _query = String.Empty;
            try
            {
                _query = @"SELECT COUNT(""Code"") FROM ""@SO_ENCABEZADOSDTE""
                            WHERE ""Name"" = 'COMUNA' AND REPLACE(""U_TEXTO"", ' ', '') = '" + cmnaRecep.ToUpper().Trim().Replace(" ", "") + "'";
                oRecord.DoQuery(_query);

                if (!oRecord.EoF)
                {
                    resp = Int32.Parse(oRecord.Fields.Item(0).Value.ToString()) > 0 ? true : false;
                }
            }
            catch (Exception)
            {
                return false;
            }
            Local.FuncionesComunes.LiberarObjetoGenerico(oRecord);
            return resp;
        }

        public static bool ValidaMontos(double mtoDTE, string DocEntryBase, string DocTypeBase)
        {
            bool resp = false;
            SAPbobsCOM.Recordset oRecord = (SAPbobsCOM.Recordset)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            string _query = String.Empty;
            try
            {
                switch (DocTypeBase)
                {
                    case "22":
                        _query = @"SELECT ""DocTotal"" FROM ""OPOR""
                            WHERE ""DocEntry"" = " + DocEntryBase + "";
                        oRecord.DoQuery(_query);
                        break;
                    default:
                        break;
                }

                if (!oRecord.EoF)
                {
                    var montoDoc = double.Parse(oRecord.Fields.Item(0).Value.ToString());
                    resp = mtoDTE.Equals(montoDoc) ? true : false;
                }
            }
            catch (Exception)
            {
                return false;
            }
            Local.FuncionesComunes.LiberarObjetoGenerico(oRecord);
            return resp;
        }

        public static double ObtieneDiferenciaMontosDTEyOC(double mtoDTE, string DocEntryBase, string DocTypeBase)
        {
            double resp = 0;
            SAPbobsCOM.Recordset oRecord = (SAPbobsCOM.Recordset)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            string _query = String.Empty;
            try
            {
                switch (DocTypeBase)
                {
                    case "22":
                        _query = @"SELECT ""DocTotal"" FROM ""OPOR""
                            WHERE ""DocEntry"" = " + DocEntryBase + "";
                        oRecord.DoQuery(_query);
                        break;
                    case "13":
                        _query = @"SELECT ""DocTotal"" FROM ""OPCH""
                            WHERE ""DocEntry"" = " + DocEntryBase + "";
                        oRecord.DoQuery(_query);
                        break;
                    default:
                        break;
                }

                if (!oRecord.EoF)
                {
                    var montoOC = double.Parse(oRecord.Fields.Item(0).Value.ToString());
                    resp = mtoDTE - montoOC;
                }
            }
            catch (Exception)
            {
                return resp;
            }
            Local.FuncionesComunes.LiberarObjetoGenerico(oRecord);
            return resp;
        }

        public static bool EsOCMonedaExtranjera(string DocEntryBase, string DocTypeBase)
        {
            bool resp = false;
            SAPbobsCOM.Recordset oRecord = (SAPbobsCOM.Recordset)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            string _query = String.Empty;
            try
            {
                switch (DocTypeBase)
                {
                    case "22":
                        _query = @"SELECT COUNT(1) FROM ""OPOR""
                            LEFT JOIN ""OCRN"" ON ""OPOR"".""DocCur"" = ""OCRN"".""CurrCode""
                            WHERE ""OCRN"".""ISOCurrCod"" <> 'CLP'
                            AND ""OPOR"".""DocEntry"" = " + DocEntryBase + "";
                        oRecord.DoQuery(_query);
                        break;
                    case "13":
                        _query = @"SELECT COUNT(1) FROM ""OPCH""
                            LEFT JOIN ""OCRN"" ON ""OPCH"".""DocCur"" = ""OCRN"".""CurrCode""
                            WHERE ""OCRN"".""ISOCurrCod"" <> 'CLP'
                            AND ""OPCH"".""DocEntry"" = " + DocEntryBase + "";
                        oRecord.DoQuery(_query);
                        break;
                    default:
                        break;
                }

                if (!oRecord.EoF)
                {
                    var count = double.Parse(oRecord.Fields.Item(0).Value.ToString());
                    if (count > 0)
                    {
                        resp = true;
                    }
                }
            }
            catch (Exception)
            {
                return resp;
            }
            Local.FuncionesComunes.LiberarObjetoGenerico(oRecord);
            return resp;
        }

        public static bool EsUsuarioMX(string usuario)
        {
            bool resp = false;
            SAPbobsCOM.Recordset oRecord = (SAPbobsCOM.Recordset)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            string _query = String.Empty;
            try
            {
                _query = @"SELECT COUNT(1) FROM ""@SO_USERMX""
                            WHERE ""Name"" = '" + usuario + "'";
                oRecord.DoQuery(_query);

                if (!oRecord.EoF)
                {
                    var count = double.Parse(oRecord.Fields.Item(0).Value.ToString());
                    if (count > 0)
                    {
                        resp = true;
                    }
                }
            }
            catch (Exception)
            {
                return resp;
            }
            Local.FuncionesComunes.LiberarObjetoGenerico(oRecord);
            return resp;
        }

        public static bool EsUsuarioSOC(string usuario)
        {
            bool resp = false;
            SAPbobsCOM.Recordset oRecord = (SAPbobsCOM.Recordset)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            string _query = String.Empty;
            try
            {
                _query = @"SELECT COUNT(1) FROM ""@SO_USERSINOC""
                            WHERE ""Name"" = '" + usuario + "'";
                oRecord.DoQuery(_query);

                if (!oRecord.EoF)
                {
                    var count = double.Parse(oRecord.Fields.Item(0).Value.ToString());
                    if (count > 0)
                    {
                        resp = true;
                    }
                }
            }
            catch (Exception)
            {
                return resp;
            }
            Local.FuncionesComunes.LiberarObjetoGenerico(oRecord);
            return resp;
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
                Local.FuncionesComunes.LiberarObjetoGenerico(oRecord);
                return moneda;
            }
            catch (Exception)
            {
                return "CLP";
            }
        }

        public static bool EsMonedaExtranjera(string moneda)
        {
            bool resp = false;
            SAPbobsCOM.Recordset oRecord = (SAPbobsCOM.Recordset)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            string _query = String.Empty;
            try
            {
                _query = @"SELECT COUNT(1) FROM ""OCRN""
                            WHERE ""OCRN"".""ISOCurrCod"" <> 'CLP'
                            AND ""OCRN"".""CurrCode"" = " + moneda + "";
                oRecord.DoQuery(_query);

                if (!oRecord.EoF)
                {
                    var count = double.Parse(oRecord.Fields.Item(0).Value.ToString());
                    if (count > 0)
                    {
                        resp = true;
                    }
                }
            }
            catch (Exception)
            {
                return resp;
            }
            Local.FuncionesComunes.LiberarObjetoGenerico(oRecord);
            return resp;
        }

        public static string ObtenerMonedaTC(string moneda)
        {
            try
            {
                SAPbobsCOM.Recordset oRecord = (SAPbobsCOM.Recordset)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                string _query = String.Empty;

                _query = @"SELECT ""U_MONTC"" FROM ""@SO_RELACMONTC"" WHERE ""Name"" = '" + moneda + "'";

                oRecord.DoQuery(_query);

                // Si hay datos
                if (!oRecord.EoF)
                {
                    moneda = oRecord.Fields.Item("U_MONTC").Value.ToString();
                }
                Local.FuncionesComunes.LiberarObjetoGenerico(oRecord);
                return moneda;
            }
            catch (Exception)
            {
                return moneda;
            }
        }

        public static string ObtenerCodigoIVAEXE()
        {
            try
            {
                string codigo = string.Empty;
                SAPbobsCOM.Recordset oRecord = (SAPbobsCOM.Recordset)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                string _query = string.Empty;

                _query = @"SELECT TOP 1
                    CASE WHEN (SELECT ""Code"" FROM ""OSTC"" WHERE ""Code"" = 'IVA_EXE') = 'IVA_EXE' THEN 'IVA_EXE'
                    ELSE 'EXE' END AS ""RESP""
                    FROM ""OSTC"" WHERE ""Rate"" = 0";

                oRecord.DoQuery(_query);

                // Si hay datos
                if (!oRecord.EoF)
                {
                    codigo = oRecord.Fields.Item("RESP").Value.ToString();
                }
                Local.FuncionesComunes.LiberarObjetoGenerico(oRecord);
                return codigo;
            }
            catch (Exception)
            {
                return "IVA_EXE";
            }
        }
    }
}
