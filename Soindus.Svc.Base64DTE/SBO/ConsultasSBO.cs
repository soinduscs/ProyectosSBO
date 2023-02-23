using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soindus.Svc.Base64DTE.SBO
{
    public class ConsultasSBO
    {
        /// <summary>
        /// Función que retorna el Token de conexión FE
        /// </summary>
        /// <returns></returns>
        public static string ObtenerTokenConexionFE()
        {
            try
            {
                string token = string.Empty;
                SAPbobsCOM.Recordset oRecord = (SAPbobsCOM.Recordset)SBO.ConexionDIAPI.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                string _query = String.Empty;

                _query = @"SELECT ""U_Descripcion"" FROM ""@SO_PARAMSFE"" WHERE ""Code"" = 'TOKEN'";

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
        /// Función que retorna la URL de visualización del PDF del DTE
        /// </summary>
        /// <returns></returns>
        public static string ObtenerURLPDFDTE()
        {
            try
            {
                string url = string.Empty;
                SAPbobsCOM.Recordset oRecord = (SAPbobsCOM.Recordset)SBO.ConexionDIAPI.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                string _query = String.Empty;

                _query = @"SELECT ""U_Descripcion"" FROM ""@SO_PARAMSFE"" WHERE ""Code"" = 'URLDTE_LOCAL'";

                oRecord.DoQuery(_query);

                // Si no hay datos
                if (oRecord.EoF)
                {
                    url = string.Empty;
                }
                // si hay datos
                else
                {
                    url = oRecord.Fields.Item(0).Value.ToString();
                }
                Local.FuncionesComunes.LiberarObjetoGenerico(oRecord);
                return url;
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
