using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soindus.AddOnFactoringCob.SBO
{
    public class ConsultasSBO
    {
        public static string ObtenerSeparadorMiles()
        {
            try
            {
                string separador = ",";
                SAPbobsCOM.Recordset oRecord = (SAPbobsCOM.Recordset)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                string _query = string.Empty;

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
                string _query = string.Empty;

                _query = @"SELECT ""DecSep"", ""ThousSep"" FROM ""OADM""";

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

        public static string ObtenerNombreBanco(string BankCode)
        {
            string nombanco = string.Empty;
            try
            {
                SAPbobsCOM.Recordset oRecord = (SAPbobsCOM.Recordset)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                string _query = string.Empty;

                _query = @"SELECT ""BankName"" FROM ""ODSC"" WHERE ""BankCode"" = '" + BankCode + "'";

                oRecord.DoQuery(_query);

                // Si hay datos
                if (!oRecord.EoF)
                {
                    nombanco = oRecord.Fields.Item("BankName").Value.ToString();
                }
                Comun.FuncionesComunes.LiberarObjetoGenerico(oRecord);
            }
            catch (Exception)
            {
                nombanco = string.Empty;
            }
            return nombanco;
        }

        public static string ObtenerAliasBanco(string BankCode)
        {
            string aliasbanco = string.Empty;
            try
            {
                SAPbobsCOM.Recordset oRecord = (SAPbobsCOM.Recordset)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                string _query = string.Empty;

                _query = @"SELECT * FROM ""@SO_FCTRNGBK"" WHERE ""U_BANKCODE"" = '" + BankCode + "'";

                oRecord.DoQuery(_query);

                // Si hay datos
                if (!oRecord.EoF)
                {
                    aliasbanco = oRecord.Fields.Item("U_BANKALIAS").Value.ToString();
                }
                Comun.FuncionesComunes.LiberarObjetoGenerico(oRecord);
            }
            catch (Exception)
            {
                aliasbanco = string.Empty;
            }
            return aliasbanco;
        }

        public static string ObtenerCuentaContableBanco(string BankCode)
        {
            string ccbanco = string.Empty;
            try
            {
                SAPbobsCOM.Recordset oRecord = (SAPbobsCOM.Recordset)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                string _query = string.Empty;

                _query = @"SELECT * FROM ""@SO_FCTRNGBK"" WHERE ""U_BANKCODE"" = '" + BankCode + "'";

                oRecord.DoQuery(_query);

                // Si hay datos
                if (!oRecord.EoF)
                {
                    ccbanco = oRecord.Fields.Item("U_BANKACC").Value.ToString();
                }
                Comun.FuncionesComunes.LiberarObjetoGenerico(oRecord);
            }
            catch (Exception)
            {
                ccbanco = string.Empty;
            }
            return ccbanco;
        }

        public static string ObtenerMonedaLocal()
        {
            string moneda = string.Empty;
            try
            {
                SAPbobsCOM.Recordset oRecord = (SAPbobsCOM.Recordset)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                string _query = string.Empty;
                _query = @"SELECT ""MainCurncy"" FROM ""OADM""";
                oRecord.DoQuery(_query);

                // Si hay datos
                if (!oRecord.EoF)
                {
                    moneda = oRecord.Fields.Item("MainCurncy").Value.ToString();
                }
                Comun.FuncionesComunes.LiberarObjetoGenerico(oRecord);
            }
            catch (Exception)
            {
                moneda = string.Empty;
            }
            return moneda;
        }

        public static string ObtenerMonedaSistema()
        {
            string moneda = string.Empty;
            try
            {
                SAPbobsCOM.Recordset oRecord = (SAPbobsCOM.Recordset)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                string _query = string.Empty;
                _query = @"SELECT ""SysCurrncy"" FROM ""OADM""";
                oRecord.DoQuery(_query);

                // Si hay datos
                if (!oRecord.EoF)
                {
                    moneda = oRecord.Fields.Item("SysCurrncy").Value.ToString();
                }
                Comun.FuncionesComunes.LiberarObjetoGenerico(oRecord);
            }
            catch (Exception)
            {
                moneda = string.Empty;
            }
            return moneda;
        }

        public static string ObtenerParametro(string Parametro)
        {
            string valor = string.Empty;
            try
            {
                SAPbobsCOM.Recordset oRecord = (SAPbobsCOM.Recordset)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                string _query = string.Empty;

                _query = @"SELECT ""U_VALUE"" FROM ""@SO_FCTRNGCF"" WHERE ""Name"" = '" + Parametro + "'";

                oRecord.DoQuery(_query);

                // Si hay datos
                if (!oRecord.EoF)
                {
                    valor = oRecord.Fields.Item("U_VALUE").Value.ToString();
                }
                Comun.FuncionesComunes.LiberarObjetoGenerico(oRecord);
            }
            catch (Exception)
            {
                valor = string.Empty;
            }
            return valor;
        }

        public static string ObtenerNombreSN(string CardCode)
        {
            string nombre = string.Empty;
            try
            {
                SAPbobsCOM.Recordset oRecord = (SAPbobsCOM.Recordset)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                string _query = string.Empty;

                _query = @"SELECT ""CardName"" FROM ""OCRD"" WHERE ""CardCode"" = '" + CardCode + "'";

                oRecord.DoQuery(_query);

                // Si hay datos
                if (!oRecord.EoF)
                {
                    nombre = oRecord.Fields.Item("CardName").Value.ToString();
                }
                Comun.FuncionesComunes.LiberarObjetoGenerico(oRecord);
            }
            catch (Exception)
            {
                nombre = string.Empty;
            }
            return nombre;
        }

        public static string ObtenerMailSN(string CardCode)
        {
            string mail = string.Empty;
            try
            {
                SAPbobsCOM.Recordset oRecord = (SAPbobsCOM.Recordset)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                string _query = string.Empty;

                _query = @"SELECT ""E_Mail"" FROM ""OCRD"" WHERE ""CardCode"" = '" + CardCode + "'";

                oRecord.DoQuery(_query);

                // Si hay datos
                if (!oRecord.EoF)
                {
                    mail = oRecord.Fields.Item("E_Mail").Value.ToString();
                }
                Comun.FuncionesComunes.LiberarObjetoGenerico(oRecord);
            }
            catch (Exception)
            {
                mail = string.Empty;
            }
            return mail;
        }

        public static string ObtenerMailConcatenadoSN(string CardCode)
        {
            string mail = string.Empty;
            try
            {
                SAPbobsCOM.Recordset oRecord = (SAPbobsCOM.Recordset)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                string _query = string.Empty;

                _query = @"SELECT ""E_Mail"" + ';' + ISNULL(""U_EMAIL_SOCIO"", '') + ';' + ISNULL(""U_EMAIL_AGROTOP"", '') AS ""CORREOS"" FROM ""OCRD"" WHERE ""CardCode"" = '" + CardCode + "'";

                oRecord.DoQuery(_query);

                // Si hay datos
                if (!oRecord.EoF)
                {
                    mail = oRecord.Fields.Item("CORREOS").Value.ToString();
                }
                Comun.FuncionesComunes.LiberarObjetoGenerico(oRecord);
            }
            catch (Exception)
            {
                mail = string.Empty;
            }
            return mail;
        }

        public static List<string> ObtenerGroupCodesSN()
        {
            List<string> lista = new List<string>();
            try
            {
                SAPbobsCOM.Recordset oRecord = (SAPbobsCOM.Recordset)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                string _query = string.Empty;

                _query = @"SELECT ""GroupCode"" FROM ""OCRG"" WHERE ""GroupName"" IN ('Clientes', 'Clientes Extranjeros') AND ""GroupType"" = 'C'";

                oRecord.DoQuery(_query);

                // Si hay datos
                if (!oRecord.EoF)
                {
                    while (!oRecord.EoF)
                    {
                        lista.Add(oRecord.Fields.Item("GroupCode").Value.ToString());
                        oRecord.MoveNext();
                    }
                }
                Comun.FuncionesComunes.LiberarObjetoGenerico(oRecord);
            }
            catch (Exception)
            {
                lista.Clear();
            }
            return lista;
        }
    }
}
