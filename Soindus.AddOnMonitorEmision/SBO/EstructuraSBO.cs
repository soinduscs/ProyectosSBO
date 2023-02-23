using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soindus.AddOnMonitorEmision.SBO
{
    public class EstructuraSBO
    {
        public static int ret = 0;
        public static string errMsg = string.Empty;

        /// <summary>
        /// Crea estructura de datos del Addon
        /// </summary>
        public static void VerificarEstructuraMD()
        {
            CargarTablas();
            CargarCampos();
            CargarUDO();
        }

        /// <summary>
        /// Funcion que permite cargar las tablas necesarias para el funcionamiento del AddOn
        /// </summary>
        private static void CargarTablas()
        {
            try
            {
                CreaTablaMD("SO_XMONCONF", "Config. Monitor de Emisión", SAPbobsCOM.BoUTBTableType.bott_MasterData);
            }
            catch (Exception e)
            {
                ConexionSBO.oCompany.GetLastError(out ret, out errMsg);
                Local.Mensajes.Errores(8, "Funciones DIAPI - Cargar Tablas - " + errMsg);
            }
        }

        /// <summary>
        /// Funcion que permite cargar los campos necesarios en sus tablas respectivas
        /// </summary>
        private static void CargarCampos()
        {
            ////Campos de usuario para tabla configuración
            CreaCampoMD("@SO_XMONCONF", "TOKEN", "Token de Conexión Portador DTE", 254, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
        }

        /// <summary>
        /// Funcion que permite cargar el UDO necesario para el funcionamiento del AddOn
        /// </summary>
        private static void CargarUDO()
        {
            try
            {
                CreaUDOMD("SO_XMONCONF", "", "SO_XMONCONF", "Conf. Monitor de Emisión", SAPbobsCOM.BoUDOObjType.boud_MasterData);
            }
            catch (Exception e)
            {

            }
        }

        /// <summary>
        /// Funcion que permite crear las tablas de usuario
        /// </summary>
        /// <param name="sNombreTabla">Se especifica el nombre de la tabla</param>
        /// <param name="sDescripcion">Se especifica la descripción de la tabla</param>
        /// <param name="uTipo">Se especifica el tipo de objeto de la tabla</param>
        private static void CreaTablaMD(String sNombreTabla, String sDescripcion, SAPbobsCOM.BoUTBTableType uTipo)
        {
            SAPbobsCOM.UserTablesMD oUserTablesMD;
            try
            {
                //Creación del la tabla de documentos del monitor.
                oUserTablesMD = (SAPbobsCOM.UserTablesMD)ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserTables);
                if (oUserTablesMD.GetByKey(sNombreTabla) == false)
                {
                    oUserTablesMD.TableName = sNombreTabla;
                    oUserTablesMD.TableDescription = sDescripcion;
                    oUserTablesMD.TableType = uTipo;
                    ret = oUserTablesMD.Add();
                    if (ret != 0)
                    {
                        ConexionSBO.oCompany.GetLastError(out ret, out errMsg);
                        Local.Mensajes.Errores(8, "Funciones DIAPI - CreaTablaMD - " + errMsg);
                    }

                }
                Local.FuncionesComunes.LiberarObjetoGenerico(oUserTablesMD);
            }
            catch
            {

            }
        }

        /// <summary>
        /// Funcion que permite crear los campos de usuario de sus respectivas tablas
        /// </summary>
        /// <param name="nombretabla">Se especifica la tabla del campo a crear</param>
        /// <param name="nombrecampo">Se especifica el campo a crear</param>
        /// <param name="descripcion">Se especifica la descripción del campo a crear</param>
        /// <param name="longitud">Se especifica la longitud del campo a crear</param>
        /// <param name="tipo">Se especifica el tipo de dato del campo </param>
        /// <param name="subtipo">Se especifica el subtipo de dato del campo. Solo apliaca para tipo de datos Float</param>
        public static void CreaCampoMD(String nombretabla, String nombrecampo, String descripcion, int longitud, SAPbobsCOM.BoFieldTypes tipo, SAPbobsCOM.BoFldSubTypes subtipo, SAPbobsCOM.BoYesNoEnum mandatory, String defaultValue, Local.ValorValido[] valores, String linkTable, Object linkObject = null)
        {
            SAPbobsCOM.UserFieldsMD oUserFieldsMD;
            try
            {
                oUserFieldsMD = (SAPbobsCOM.UserFieldsMD)ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields);
                oUserFieldsMD.TableName = nombretabla;//Se obtiene el nombre de la tabla de usario
                oUserFieldsMD.Name = nombrecampo;//Se asigna el nombre del campo de usuario
                oUserFieldsMD.Description = descripcion;//Se asigna una descripcion al campo de usuario
                oUserFieldsMD.Mandatory = mandatory;
                if (longitud > 0)
                {
                    oUserFieldsMD.EditSize = longitud;//Se define una longitud al campo de usuario
                }
                oUserFieldsMD.Type = tipo;//Se asigna el tipo de dato al campo de usuario
                oUserFieldsMD.SubType = subtipo;

                if (valores != null && valores.Length > 0)
                {
                    foreach (Local.ValorValido vv in valores)
                    {
                        oUserFieldsMD.ValidValues.Value = vv.valor;
                        oUserFieldsMD.ValidValues.Description = vv.descripcion;
                        oUserFieldsMD.ValidValues.Add();
                    }
                }

                if (defaultValue != null) oUserFieldsMD.DefaultValue = defaultValue;

                oUserFieldsMD.LinkedTable = linkTable;

                if (linkObject != null)
                {
                    oUserFieldsMD.LinkedSystemObject = (SAPbobsCOM.UDFLinkedSystemObjectTypesEnum)linkObject;
                }

                ret = oUserFieldsMD.Add();//Se agrega el campo de usuario

                if (ret != 0 && ret != -2035 && ret != -5002)
                {
                    ConexionSBO.oCompany.GetLastError(out ret, out errMsg);
                    Local.Mensajes.Errores(8, "Funciones DIAPI - CreaCampoMD - " + errMsg);
                }

                Local.FuncionesComunes.LiberarObjetoGenerico(oUserFieldsMD);
            }
            catch
            {

            }
        }

        /// <summary>
        /// Funcion que permite crear los UDO
        /// </summary>
        /// <param name="sNombreTablaPadre">Se especifica el nombre de la tabla padre de la UDO</param>
        /// <param name="sNombreTablaHijo">Se especifica el nombre de la tabla hijo de la UDO</param>
        /// <param name="sCodigoUDO">Se especifica el Codigo de Objeto de la UDO</param>
        /// <param name="sNameUDO">Se especifica el nombre del Objeto de la UDO</param>
        /// <param name="oUDOObjType">Se especifica el tipo de Objeto del UDO</param>
        public static void CreaUDOMD(String sNombreTablaPadre, String sNombreTablaHijo, String sCodigoUDO, String sNameUDO, SAPbobsCOM.BoUDOObjType oUDOObjType)
        {
            try
            {
                SAPbobsCOM.UserObjectsMD oUserObjectMD = null;
                oUserObjectMD = ((SAPbobsCOM.UserObjectsMD)(ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserObjectsMD)));
                oUserObjectMD.CanCancel = SAPbobsCOM.BoYesNoEnum.tNO;
                oUserObjectMD.CanClose = SAPbobsCOM.BoYesNoEnum.tNO;
                oUserObjectMD.CanCreateDefaultForm = SAPbobsCOM.BoYesNoEnum.tNO;
                oUserObjectMD.CanDelete = SAPbobsCOM.BoYesNoEnum.tNO;
                oUserObjectMD.CanFind = SAPbobsCOM.BoYesNoEnum.tYES;
                //oUserObjectMD.CanYearTransfer = SAPbobsCOM.BoYesNoEnum.tYES;
                oUserObjectMD.TableName = sNombreTablaPadre;
                if (!string.IsNullOrEmpty(sNombreTablaHijo))
                {
                    oUserObjectMD.ChildTables.TableName = sNombreTablaHijo;
                }
                oUserObjectMD.Code = sCodigoUDO;
                //oUserObjectMD.ManageSeries = SAPbobsCOM.BoYesNoEnum.tYES;
                oUserObjectMD.Name = sNameUDO;
                oUserObjectMD.ObjectType = oUDOObjType;
                //oUserObjectMD.FormSRF = "GrupoActivoFijo";
                //string sPath = null;
                //sPath = System.IO.Directory.GetParent(Application.StartupPath).ToString();
                //sPath = System.IO.Directory.GetParent(sPath).ToString() + "\\";
                //oUserObjectMD.FormSRF = sPath + "Formularios\\" + "GrupoActivoFijo.srf";

                ret = oUserObjectMD.Add();

                if (ret != 0 && ret != -2035)
                {
                    ConexionSBO.oCompany.GetLastError(out ret, out errMsg);
                    Local.Mensajes.Errores(19, "Funciones DIAPI - CreaUDOMD - " + errMsg);
                }
                else
                {
                    Local.Mensajes.Exitos(4, "Funciones DIAPI - CreaUDOMD - ");
                }

                Local.FuncionesComunes.LiberarObjetoGenerico(oUserObjectMD);
                oUserObjectMD = null;
                GC.Collect();
            }
            catch
            {

            }
        }
    }
}
