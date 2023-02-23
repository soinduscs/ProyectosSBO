using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soindus.AddOnEmisionDTE.SBO
{
    public class ConsultasSBO
    {
        /// <summary>
        /// Función que retorna el Token de conexión FE
        /// </summary>
        /// <returns></returns>
        public static string ObtenerTokenConexionFE()
        {
            SAPbobsCOM.Recordset oRecord = (SAPbobsCOM.Recordset)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            try
            {
                string token = string.Empty;
                string _query = String.Empty;

                _query = @"SELECT ""U_DESCRIPCION"" FROM ""@SO_PARAMSFE"" WHERE ""Code"" = 'TOKEN'";

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
                Local.FuncionesComunes.LiberarObjetoGenerico(oRecord);
                return string.Empty;
            }
        }

        /// <summary>
        /// Función que retorna la URL de visualización del PDF del DTE
        /// </summary>
        /// <returns></returns>
        public static string ObtenerURLPDFDTE()
        {
            SAPbobsCOM.Recordset oRecord = (SAPbobsCOM.Recordset)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            try
            {
                string url = string.Empty;
                string _query = String.Empty;

                _query = @"SELECT ""U_DESCRIPCION"" FROM ""@SO_PARAMSFE"" WHERE ""Code"" = 'URLDTE_LOCAL'";

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
                Local.FuncionesComunes.LiberarObjetoGenerico(oRecord);
                return string.Empty;
            }
        }

        /// <summary>
        /// Función que retorna el rut de la empresa emisora de DTE
        /// </summary>
        /// <returns></returns>
        public static string ObtenerRutEmpresaEmisora()
        {
            SAPbobsCOM.Recordset oRecord = (SAPbobsCOM.Recordset)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            try
            {
                string rut = string.Empty;
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
                Local.FuncionesComunes.LiberarObjetoGenerico(oRecord);
                return string.Empty;
            }
        }

        /// <summary>
        /// Función que retorna el número de resolución de la empresa emisora de DTE
        /// </summary>
        /// <returns></returns>
        public static string ObtenerNroResolucion()
        {
            SAPbobsCOM.Recordset oRecord = (SAPbobsCOM.Recordset)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            try
            {
                string resol = string.Empty;
                string _query = String.Empty;

                _query = @"SELECT ""TaxIdNum2"" FROM ""OADM""";

                oRecord.DoQuery(_query);

                // Si no hay datos
                if (oRecord.EoF)
                {
                    resol = string.Empty;
                }
                // si hay datos
                else
                {
                    resol = oRecord.Fields.Item(0).Value.ToString();
                }
                Local.FuncionesComunes.LiberarObjetoGenerico(oRecord);
                return resol;
            }
            catch (Exception)
            {
                Local.FuncionesComunes.LiberarObjetoGenerico(oRecord);
                return string.Empty;
            }
        }

        /// <summary>
        /// Función que retorna la fecha de resolución de la empresa emisora de DTE
        /// </summary>
        /// <returns></returns>
        public static string ObtenerFechaResolucion()
        {
            SAPbobsCOM.Recordset oRecord = (SAPbobsCOM.Recordset)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            try
            {
                string fecresol = string.Empty;
                string _query = String.Empty;

                _query = @"SELECT ""TaxIdNum3"" FROM ""OADM""";

                oRecord.DoQuery(_query);

                // Si no hay datos
                if (oRecord.EoF)
                {
                    fecresol = string.Empty;
                }
                // si hay datos
                else
                {
                    fecresol = oRecord.Fields.Item(0).Value.ToString();
                }
                Local.FuncionesComunes.LiberarObjetoGenerico(oRecord);
                return fecresol;
            }
            catch (Exception)
            {
                Local.FuncionesComunes.LiberarObjetoGenerico(oRecord);
                return string.Empty;
            }
        }
    }
}
