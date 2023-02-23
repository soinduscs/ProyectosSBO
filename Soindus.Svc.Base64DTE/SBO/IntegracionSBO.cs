using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAPbobsCOM;
using System.Runtime.InteropServices;

namespace Soindus.Svc.Base64DTE.SBO
{
    public class IntegracionSBO : IDisposable
    {
        [DllImport("kernel32.dll", EntryPoint = "SetProcessWorkingSetSize", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int SetProcessWorkingSetSize(IntPtr process, int minimumWorkingSetSize, int maximumWorkingSetSize);
        public static void Alzheimer()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
        }

        public void ActualizarBase64()
        {
            Local.Configuracion ExtConf = new Local.Configuracion();
            string _query = string.Empty;
            string _filtrobase64 = string.Empty;
            if (ExtConf.Parametros.Proveedor_FE.Equals("DBN-EV"))
            {
                _filtrobase64 = @" AND ISNULL(""U_STTBase64"", '0') = '0' AND ISNULL(""FolioNum"", 0) <> 0 AND ""DocDate"" >= '20210830' ";
            }
            else if (ExtConf.Parametros.Proveedor_FE.Equals("DBN-SO"))
            {
                _filtrobase64 = @" AND ISNULL(""U_SO_STTBASE64"", '') = '' AND ISNULL(""FolioNum"", 0) <> 0 ";
            }
            _query += @"
                SELECT
                ""DocEntry"" AS ""DocEntry"",
                'FE' AS ""TipoDoc"",
                ""FolioNum"" AS ""FolioNum"",
                0 AS ""StatusPDF""
                FROM ""OINV"" WHERE ""DocSubType"" = '--' AND ""isIns"" = 'N'" + _filtrobase64;
            _query += @"
                UNION ALL
                SELECT
                ""DocEntry"" AS ""DocEntry"",
                'FE' AS ""TipoDoc"",
                ""FolioNum"" AS ""FolioNum"",
                0 AS ""StatusPDF""
                FROM ""OINV"" WHERE ""DocSubType"" = '--' AND ""isIns"" = 'Y' AND ""Indicator"" <> '39'" + _filtrobase64;
            _query += @"
                UNION ALL
                SELECT
                ""DocEntry"" AS ""DocEntry"",
                'FN' AS ""TipoDoc"",
                ""FolioNum"" AS ""FolioNum"",
                0 AS ""StatusPDF""
                FROM ""OINV"" WHERE ""DocSubType"" = 'IE'" + _filtrobase64;
            _query += @"
                UNION ALL
                SELECT
                ""DocEntry"" AS ""DocEntry"",
                'BE' AS ""TipoDoc"",
                ""FolioNum"" AS ""FolioNum"",
                0 AS ""StatusPDF""
                FROM ""OINV"" WHERE ""DocSubType"" = 'IB'" + _filtrobase64;
            _query += @"
                UNION ALL
                SELECT
                ""DocEntry"" AS ""DocEntry"",
                'BE' AS ""TipoDoc"",
                ""FolioNum"" AS ""FolioNum"",
                0 AS ""StatusPDF""
                FROM ""OINV"" WHERE ""DocSubType"" = '--' AND ""isIns"" = 'Y' AND ""Indicator"" = '39'" + _filtrobase64;
            _query += @"
                UNION ALL
                SELECT
                ""DocEntry"" AS ""DocEntry"",
                'BN' AS ""TipoDoc"",
                ""FolioNum"" AS ""FolioNum"",
                0 AS ""StatusPDF""
                FROM ""OINV"" WHERE ""DocSubType"" = 'EB'" + _filtrobase64;
            _query += @"
                UNION ALL
                SELECT
                ""DocEntry"" AS ""DocEntry"",
                'ND' AS ""TipoDoc"",
                ""FolioNum"" AS ""FolioNum"",
                0 AS ""StatusPDF""
                FROM ""OINV"" WHERE ""DocSubType"" = 'DN' AND ""Indicator"" <> '11'" + _filtrobase64;
            _query += @"
                UNION ALL
                SELECT
                ""DocEntry"" AS ""DocEntry"",
                'NC' AS ""TipoDoc"",
                ""FolioNum"" AS ""FolioNum"",
                0 AS ""StatusPDF""
                FROM ""ORIN"" WHERE ""Indicator"" <> '12'" + _filtrobase64;
            _query += @"
                UNION ALL
                SELECT
                ""DocEntry"" AS ""DocEntry"",
                'GE' AS ""TipoDoc"",
                ""FolioNum"" AS ""FolioNum"",
                0 AS ""StatusPDF""
                FROM ""ODLN"" WHERE ""U_SO_EMITEGUIA"" = '1'" + _filtrobase64;
            _query += @"
                UNION ALL
                SELECT
                ""DocEntry"" AS ""DocEntry"",
                'GT' AS ""TipoDoc"",
                ""FolioNum"" AS ""FolioNum"",
                0 AS ""StatusPDF""
                FROM ""OWTR"" WHERE ""U_SO_EMITEGUIA"" = '1'" + _filtrobase64;
            _query += @"
                UNION ALL
                SELECT
                ""DocEntry"" AS ""DocEntry"",
                'FEX' AS ""TipoDoc"",
                ""FolioNum"" AS ""FolioNum"",
                0 AS ""StatusPDF""
                FROM ""OINV"" WHERE ""DocSubType"" = 'IX'" + _filtrobase64;
            _query += @"
                UNION ALL
                SELECT
                ""DocEntry"" AS ""DocEntry"",
                'NDX' AS ""TipoDoc"",
                ""FolioNum"" AS ""FolioNum"",
                0 AS ""StatusPDF""
                FROM ""OINV"" WHERE ""DocSubType"" = 'DN' AND ""Indicator"" = '11'" + _filtrobase64;
            _query += @"
                UNION ALL
                SELECT
                ""DocEntry"" AS ""DocEntry"",
                'NCX' AS ""TipoDoc"",
                ""FolioNum"" AS ""FolioNum"",
                0 AS ""StatusPDF""
                FROM ""ORIN"" WHERE ""Indicator"" = '12'" + _filtrobase64;

            int ErrCode = 0;
            string ErrMsj = string.Empty;
            SAPbobsCOM.Documents oDoc = null;
            SAPbobsCOM.StockTransfer oST = null;
            SAPbobsCOM.Recordset oRecord = (SAPbobsCOM.Recordset)SBO.ConexionDIAPI.oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);
            try
            {
                oRecord.DoQuery(_query);
                while (!oRecord.EoF)
                {
                    string[] parametros = null;
                    string DocEntry = oRecord.Fields.Item("DocEntry").Value.ToString();
                    string TipoDoc = oRecord.Fields.Item("TipoDoc").Value.ToString();
                    string folio = oRecord.Fields.Item("FolioNum").Value.ToString();

                    if (!string.IsNullOrEmpty(folio))
                    {
                        switch (TipoDoc)
                        {
                            case "FE":
                                parametros = new string[] { "", folio, "33", "", "", "false" };
                                break;
                            case "FN":
                                parametros = new string[] { "", folio, "34", "", "", "false" };
                                break;
                            case "BE":
                                parametros = new string[] { "", folio, "39", "", "", "false" };
                                break;
                            case "BN":
                                parametros = new string[] { "", folio, "41", "", "", "false" };
                                break;
                            case "ND":
                                parametros = new string[] { "", folio, "56", "", "", "false" };
                                break;
                            case "NC":
                                parametros = new string[] { "", folio, "61", "", "", "false" };
                                break;
                            case "GE":
                            case "GT":
                                parametros = new string[] { "", folio, "52", "", "", "false" };
                                break;
                            case "FEX":
                                parametros = new string[] { "", folio, "110", "", "", "false" };
                                break;
                            case "NDX":
                                parametros = new string[] { "", folio, "111", "", "", "false" };
                                break;
                            case "NCX":
                                parametros = new string[] { "", folio, "112", "", "", "false" };
                                break;
                            default:
                                break;
                        }
                        ProveedorDTE proveedorDTE = new ProveedorDTE();
                        var resultPDF = proveedorDTE.ObtenerPDF(parametros);
                        if (resultPDF.Success)
                        {
                            var respPDF = proveedorDTE.RespuestaPDF;

                            // Guardar Base64
                            int Ret = 0;
                            int Intento = 0;
                            switch (TipoDoc)
                            {
                                case "FE":
                                case "FN":
                                case "BE":
                                case "BN":
                                case "ND":
                                case "FEX":
                                case "NDX":
                                    oDoc = (SAPbobsCOM.Documents)SBO.ConexionDIAPI.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices);
                                    oDoc.GetByKey(int.Parse(DocEntry));

                                    if (ExtConf.Parametros.Proveedor_FE.Equals("DBN-EV"))
                                    {
                                        oDoc.UserFields.Fields.Item("U_DTEBase64").Value = respPDF.String[1];
                                        oDoc.UserFields.Fields.Item("U_STTBase64").Value = "1";
                                    }
                                    else if (ExtConf.Parametros.Proveedor_FE.Equals("DBN-SO"))
                                    {
                                        oDoc.UserFields.Fields.Item("U_SO_DTEBASE64").Value = respPDF.String[1];
                                        oDoc.UserFields.Fields.Item("U_SO_STTBASE64").Value = "1";
                                    }

                                    //Ret = oDoc.Update();
                                    Ret = 0;
                                    Intento = 0;
                                    while (Intento < 5)
                                    {
                                        Ret = oDoc.Update();
                                        if (Ret.Equals(-2038)) //Deadlocks
                                        {
                                            System.Threading.Thread.Sleep(1000);
                                            Intento++;
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                    break;
                                case "NC":
                                case "NCX":
                                    oDoc = (SAPbobsCOM.Documents)SBO.ConexionDIAPI.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oCreditNotes);
                                    oDoc.GetByKey(int.Parse(DocEntry));

                                    if (ExtConf.Parametros.Proveedor_FE.Equals("DBN-EV"))
                                    {
                                        oDoc.UserFields.Fields.Item("U_DTEBase64").Value = respPDF.String[1];
                                        oDoc.UserFields.Fields.Item("U_STTBase64").Value = "1";
                                    }
                                    else if (ExtConf.Parametros.Proveedor_FE.Equals("DBN-SO"))
                                    {
                                        oDoc.UserFields.Fields.Item("U_SO_DTEBASE64").Value = respPDF.String[1];
                                        oDoc.UserFields.Fields.Item("U_SO_STTBASE64").Value = "1";
                                    }

                                    //Ret = oDoc.Update();
                                    Ret = 0;
                                    Intento = 0;
                                    while (Intento < 5)
                                    {
                                        Ret = oDoc.Update();
                                        if (Ret.Equals(-2038)) //Deadlocks
                                        {
                                            System.Threading.Thread.Sleep(1000);
                                            Intento++;
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                    break;
                                case "GE":
                                    oDoc = (SAPbobsCOM.Documents)SBO.ConexionDIAPI.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oDeliveryNotes);
                                    oDoc.GetByKey(int.Parse(DocEntry));

                                    if (ExtConf.Parametros.Proveedor_FE.Equals("DBN-EV"))
                                    {
                                        oDoc.UserFields.Fields.Item("U_DTEBase64").Value = respPDF.String[1];
                                        oDoc.UserFields.Fields.Item("U_STTBase64").Value = "1";
                                    }
                                    else if (ExtConf.Parametros.Proveedor_FE.Equals("DBN-SO"))
                                    {
                                        oDoc.UserFields.Fields.Item("U_SO_DTEBASE64").Value = respPDF.String[1];
                                        oDoc.UserFields.Fields.Item("U_SO_STTBASE64").Value = "1";
                                    }

                                    //Ret = oDoc.Update();
                                    Ret = 0;
                                    Intento = 0;
                                    while (Intento < 5)
                                    {
                                        Ret = oDoc.Update();
                                        if (Ret.Equals(-2038)) //Deadlocks
                                        {
                                            System.Threading.Thread.Sleep(1000);
                                            Intento++;
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                    break;
                                case "GT":
                                    oST = (SAPbobsCOM.StockTransfer)SBO.ConexionDIAPI.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oStockTransfer);
                                    oST.GetByKey(int.Parse(DocEntry));

                                    if (ExtConf.Parametros.Proveedor_FE.Equals("DBN-EV"))
                                    {
                                        oDoc.UserFields.Fields.Item("U_DTEBase64").Value = respPDF.String[1];
                                        oDoc.UserFields.Fields.Item("U_STTBase64").Value = "1";
                                    }
                                    else if (ExtConf.Parametros.Proveedor_FE.Equals("DBN-SO"))
                                    {
                                        oDoc.UserFields.Fields.Item("U_SO_DTEBASE64").Value = respPDF.String[1];
                                        oDoc.UserFields.Fields.Item("U_SO_STTBASE64").Value = "1";
                                    }

                                    //Ret = oST.Update();
                                    Ret = 0;
                                    Intento = 0;
                                    while (Intento < 5)
                                    {
                                        Ret = oST.Update();
                                        if (Ret.Equals(-2038)) //Deadlocks
                                        {
                                            System.Threading.Thread.Sleep(1000);
                                            Intento++;
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                    break;
                                default:
                                    break;
                            }
                            if (Ret != 0)
                            {
                                SBO.ConexionDIAPI.oCompany.GetLastError(out ErrCode, out ErrMsj);
                            }
                        }
                    }
                    oRecord.MoveNext();
                }
                Local.FuncionesComunes.LiberarObjetoGenerico(oDoc);
                Local.FuncionesComunes.LiberarObjetoGenerico(oST);
                Local.FuncionesComunes.LiberarObjetoGenerico(oRecord);
            }
            catch (Exception ex)
            {
                Local.FuncionesComunes.LiberarObjetoGenerico(oDoc);
                Local.FuncionesComunes.LiberarObjetoGenerico(oST);
                Local.FuncionesComunes.LiberarObjetoGenerico(oRecord);
            }
        }

        public void Dispose()
        {
            Alzheimer();
            GC.SuppressFinalize(this);
        }
    }
}
