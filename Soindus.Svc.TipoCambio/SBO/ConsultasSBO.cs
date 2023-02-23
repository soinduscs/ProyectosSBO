using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soindus.Svc.TipoCambio.SBO
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
                SAPbobsCOM.Recordset oRecord = (SAPbobsCOM.Recordset)SBO.ConexionDIAPI.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                string _query = String.Empty;

                _query = @"SELECT * FROM ""OUTB"" WHERE ""TableName"" IN ('SO_TIPOCAMBIO', 'SO_TCCONF')";

                oRecord.DoQuery(_query);

                // Si hay datos
                if (!oRecord.EoF)
                {
                    if (oRecord.RecordCount == 2)
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
                SAPbobsCOM.Recordset oRecord = (SAPbobsCOM.Recordset)SBO.ConexionDIAPI.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                string _query = String.Empty;

                _query = @"SELECT * FROM ""@SO_TCCONF"" WHERE ""Code"" = 'CONF'";

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
                SAPbobsCOM.Recordset oRecord = (SAPbobsCOM.Recordset)SBO.ConexionDIAPI.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                string _query = String.Empty;

                _query = @"SELECT * FROM ""@SO_TCCONF"" WHERE ""Code"" = 'CONF'";

                oRecord.DoQuery(_query);

                // Si no hay datos
                if (oRecord.EoF)
                {
                    ret.TOKEN = string.Empty;
                }
                // si hay datos
                else
                {
                    ret.TOKEN = oRecord.Fields.Item("U_TOKEN").Value.ToString();
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
        /// Función que retorna el rut de la sociedad
        /// </summary>
        /// <returns></returns>
        public static string ObtenerRutSociedad()
        {
            try
            {
                string rut = string.Empty;
                SAPbobsCOM.Recordset oRecord = (SAPbobsCOM.Recordset)SBO.ConexionDIAPI.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
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
