using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soindus.AddOnMonitorEmision.SBO
{
    public class ConsultasSBO
    {
        /// <summary>
        /// Función que verifica la estructura del AddOn
        /// </summary>
        public static Boolean VerificaEstructura()
        {
            try
            {
                bool existe = false;
                SAPbobsCOM.Recordset oRecord = (SAPbobsCOM.Recordset)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                string _query = String.Empty;

                _query = @"SELECT * FROM ""OUTB"" WHERE ""TableName"" IN ('SO_XMONCONF')";

                oRecord.DoQuery(_query);

                // Si hay datos
                if (!oRecord.EoF)
                {
                    if (oRecord.RecordCount == 1)
                    {
                        existe = true;
                    }
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
        /// Función que retorna si existe el registro de configuración
        /// </summary>
        public static Boolean ExisteConfiguracion()
        {
            try
            {
                bool existe = false;
                SAPbobsCOM.Recordset oRecord = (SAPbobsCOM.Recordset)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                string _query = String.Empty;

                _query = @"SELECT * FROM ""@SO_XMONCONF"" WHERE ""Code"" = 'CONF'";

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
        /// Función que retorna el Token de conexión FE
        /// </summary>
        /// <returns></returns>
        public static string ObtenerTokenConexionFE()
        {
            try
            {
                string token = string.Empty;
                SAPbobsCOM.Recordset oRecord = (SAPbobsCOM.Recordset)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                string _query = String.Empty;

                _query = @"SELECT ""U_TOKEN"" FROM ""@SO_XMONCONF"" WHERE ""Code"" = 'CONF'";

                oRecord.DoQuery(_query);

                // Si no hay datos
                if (oRecord.EoF)
                {
                    token = string.Empty;
                }
                // si hay datos
                else
                {
                    token = oRecord.Fields.Item(0).Value.ToString();
                }
                Local.FuncionesComunes.LiberarObjetoGenerico(oRecord);
                return token;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Función que retorna el rut de la empresa emisora de DTE
        /// </summary>
        /// <returns></returns>
        public static string ObtenerRutEmpresaEmisora()
        {
            try
            {
                string rut = string.Empty;
                SAPbobsCOM.Recordset oRecord = (SAPbobsCOM.Recordset)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                string _query = String.Empty;

                _query = @"SELECT ""TaxIdNum"" FROM ""OADM""";

                oRecord.DoQuery(_query);

                // Si no hay datos
                if (oRecord.EoF)
                {
                    rut = string.Empty;
                }
                // si hay datos
                else
                {
                    rut = oRecord.Fields.Item(0).Value.ToString();
                    rut = rut.Replace(".", "");
                }
                Local.FuncionesComunes.LiberarObjetoGenerico(oRecord);
                return rut;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}
