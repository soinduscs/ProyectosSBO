using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soindus.AddOnFactoringCob.SBO
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
                CreaTablaMD("SO_FCTRNG", "SO Factoring", SAPbobsCOM.BoUTBTableType.bott_Document);
                CreaTablaMD("SO_FCTRNGL", "SO Factoring Lines", SAPbobsCOM.BoUTBTableType.bott_DocumentLines);
                CreaTablaMD("SO_FCTRNGCF", "SO Factoring Config", SAPbobsCOM.BoUTBTableType.bott_NoObjectAutoIncrement);
                CreaTablaMD("SO_FCTRNGBK", "SO Factoring Banks", SAPbobsCOM.BoUTBTableType.bott_NoObjectAutoIncrement);
                CreaTablaMD("SO_FCTRNGLOG", "SO Factoring Logs", SAPbobsCOM.BoUTBTableType.bott_Document);
                //CreaTablaMD("SO_RENDICF", "Rendiciones RG Config.", SAPbobsCOM.BoUTBTableType.bott_MasterData);
            }
            catch (Exception e)
            {
                ConexionSBO.oCompany.GetLastError(out ret, out errMsg);
                Comun.Mensajes.Errores(8, "Funciones DIAPI - Cargar Tablas - " + errMsg);
            }
        }

        /// <summary>
        /// Funcion que permite cargar los campos necesarios en sus tablas respectivas
        /// </summary>
        private static void CargarCampos()
        {
            ////Campos de usuario para tablas SAP
            //CreaCampoMD("OADM", "SO_TOKEN", "Token de conexión Febos", 100, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            //CreaCampoMD("OADM", "SO_RRECEP", "Rut del receptor de DTE", 15, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            //CreaCampoMD("OADM", "SO_RECINTO", "Recinto para rechazo de DTE", 30, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            //Clases.ValorValido[] valoresSINO = new Clases.ValorValido[]
            //    { new Clases.ValorValido("Y", "Si"),
            //      new Clases.ValorValido("N", "No")};
            //CreaCampoMD("OCRD", "SO_PROVOC", "Proveedor orden de compra", 30, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, "Y", valoresSINO, null);
            //CreaCampoMD("OCRD", "SO_RESP", "Empleado responsable", 10, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null, SAPbobsCOM.UDFLinkedSystemObjectTypesEnum.ulEmployeesInfo);

            ////Campos de usuario para tabla factoring
            CreaCampoMD("@SO_FCTRNG", "ID", "Id Factoring", 11, SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_FCTRNG", "ENTIDAD", "Entidad", 254, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_FCTRNG", "MONEDA", "Moneda", 10, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_FCTRNG", "FECHA", "Otorgamiento", 10, SAPbobsCOM.BoFieldTypes.db_Date, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_FCTRNG", "NUMOPER", "Número operación", 100, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_FCTRNG", "VALOR", "Valor", 30, SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Price, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            Clases.ValorValido[] valoresTIPORES = new Clases.ValorValido[]
                { new Clases.ValorValido("", ""),
                  new Clases.ValorValido("CR", "Con responsabilidad"),
                  new Clases.ValorValido("SR", "Sin responsabilidad"),
                  new Clases.ValorValido("FN", "Financiero") };
            CreaCampoMD("@SO_FCTRNG", "TIPORES", "Tipo de responsabilidad", 3, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, valoresTIPORES, null);
            Clases.ValorValido[] valoresESTADO = new Clases.ValorValido[]
                { new Clases.ValorValido("", ""),
                  new Clases.ValorValido("P", "Preliminar"),
                  new Clases.ValorValido("D", "Definitivo")};
            CreaCampoMD("@SO_FCTRNG", "ESTADO", "Estado", 3, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, valoresESTADO, null);

            ////Campos de usuario para tabla factoring lineas
            CreaCampoMD("@SO_FCTRNGL", "OBJTYPE", "ObjType", 20, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_FCTRNGL", "DOCENTRY", "DocEntry", 11, SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_FCTRNGL", "BASEENTRY", "BaseEntry", 11, SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_FCTRNGL", "BASEREF", "BaseRef", 16, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_FCTRNGL", "TIPODOC", "TipoDoc", 2, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_FCTRNGL", "DOCNUM", "DocNum", 11, SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_FCTRNGL", "FOLIONUM", "FolioNum", 11, SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_FCTRNGL", "ISINS", "isIns", 1, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_FCTRNGL", "INDICATOR", "Indicator", 2, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_FCTRNGL", "DOCDATE", "DocDate", 10, SAPbobsCOM.BoFieldTypes.db_Date, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_FCTRNGL", "DOCDUEDATE", "DocDueDate", 10, SAPbobsCOM.BoFieldTypes.db_Date, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_FCTRNGL", "TAXDATE", "TaxDate", 10, SAPbobsCOM.BoFieldTypes.db_Date, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_FCTRNGL", "DOCCUR", "DocCur", 3, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_FCTRNGL", "DOCTOTAL", "DocTotal", 30, SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Price, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_FCTRNGL", "DOCTOTALSY", "DocTotalSy", 30, SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Price, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_FCTRNGL", "LICTRADNUM", "LicTradNum", 32, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_FCTRNGL", "CARDCODE", "CardCode", 15, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_FCTRNGL", "CARDNAME", "CardName", 100, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);

            ////Campos de usuario para tabla de configuración
            CreaCampoMD("@SO_FCTRNGCF", "VALUE", "Valor de Parámetro", 254, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);

            ////Campos de usuario para tabla de bancos relacionados
            CreaCampoMD("@SO_FCTRNGBK", "BANKCODE", "Código bancario SAP", 3, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_FCTRNGBK", "BANKALIAS", "Alias banco", 20, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_FCTRNGBK", "BANKACC", "Cuenta contable", 15, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);

            ////Campos de usuario para tabla factoring logs
            //CreaCampoMD("@SO_FCTRNGLOG", "OBJTYPE", "ObjType", 20, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            //CreaCampoMD("@SO_FCTRNGLOG", "DOCENTRY", "DocEntry", 11, SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            //CreaCampoMD("@SO_FCTRNGLOG", "TIPODOC", "TipoDoc", 2, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            //CreaCampoMD("@SO_FCTRNGLOG", "DOCNUM", "DocNum", 11, SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            //CreaCampoMD("@SO_FCTRNGLOG", "FOLIONUM", "FolioNum", 11, SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            //CreaCampoMD("@SO_FCTRNGLOG", "DOCDATE", "DocDate", 10, SAPbobsCOM.BoFieldTypes.db_Date, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            //CreaCampoMD("@SO_FCTRNGLOG", "DOCDUEDATE", "DocDueDate", 10, SAPbobsCOM.BoFieldTypes.db_Date, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            //CreaCampoMD("@SO_FCTRNGLOG", "TAXDATE", "TaxDate", 10, SAPbobsCOM.BoFieldTypes.db_Date, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            //CreaCampoMD("@SO_FCTRNGLOG", "DOCCUR", "DocCur", 3, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            //CreaCampoMD("@SO_FCTRNGLOG", "DOCTOTAL", "DocTotal", 30, SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Price, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_FCTRNGLOG", "CARDCODE", "CardCode", 15, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_FCTRNGLOG", "CARDNAME", "CardName", 100, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_FCTRNGLOG", "EMAIL", "EMail", 100, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_FCTRNGLOG", "EMAILTYPE", "EmailType", 20, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_FCTRNGLOG", "EMAILLANG", "EmailLang", 20, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_FCTRNGLOG", "DOCS", "Docs", 1, SAPbobsCOM.BoFieldTypes.db_Memo, SAPbobsCOM.BoFldSubTypes.st_Link, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_FCTRNGLOG", "FOLIOS", "Folios", 1, SAPbobsCOM.BoFieldTypes.db_Memo, SAPbobsCOM.BoFldSubTypes.st_Link, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_FCTRNGLOG", "OBS", "Obs", 1, SAPbobsCOM.BoFieldTypes.db_Memo, SAPbobsCOM.BoFldSubTypes.st_Link, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_FCTRNGLOG", "SENDDATE", "SendDate", 10, SAPbobsCOM.BoFieldTypes.db_Date, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_FCTRNGLOG", "SENDTIME", "SendTime", 30, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);

        }

        /// <summary>
        /// Funcion que permite cargar el UDO necesario para el funcionamiento del AddOn
        /// </summary>
        private static void CargarUDO()
        {
            try
            {
                CreaUDOMD("SO_FCTRNG", "SO_FCTRNGL", "SO_FACTORING", "SO Factoring", SAPbobsCOM.BoUDOObjType.boud_Document);
                CreaUDOMD("SO_FCTRNGLOG", "", "SO_FACTORINGLOG", "SO Factoring Log", SAPbobsCOM.BoUDOObjType.boud_Document);
                //CreaUDOMD("SO_RENDIG", "SO_RENDIGCE", "SO_RENDIGTO", "Gastos Rendiciones RG", SAPbobsCOM.BoUDOObjType.boud_Document);
                //CreaUDOMD("SO_RENDICF", "", "SO_RENDICF", "Conf. Monitor Rindegastos Soindus", SAPbobsCOM.BoUDOObjType.boud_MasterData);
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
                        Comun.Mensajes.Errores(8, "Funciones DIAPI - CreaTablaMD - " + errMsg);
                    }

                }
                Comun.FuncionesComunes.LiberarObjetoGenerico(oUserTablesMD);
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
        public static void CreaCampoMD(String nombretabla, String nombrecampo, String descripcion, int longitud, SAPbobsCOM.BoFieldTypes tipo, SAPbobsCOM.BoFldSubTypes subtipo, SAPbobsCOM.BoYesNoEnum mandatory, String defaultValue, Clases.ValorValido[] valores, String linkTable, Object linkObject = null)
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
                    foreach (Clases.ValorValido vv in valores)
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
                    //oUserFieldsMD.LinkedSystemObject = (SAPbobsCOM.UDFLinkedSystemObjectTypesEnum)linkObject;
                }

                ret = oUserFieldsMD.Add();//Se agrega el campo de usuario

                if (ret != 0 && ret != -2035 && ret != -5002)
                {
                    ConexionSBO.oCompany.GetLastError(out ret, out errMsg);
                    Comun.Mensajes.Errores(8, "Funciones DIAPI - CreaCampoMD - " + errMsg);
                }

                Comun.FuncionesComunes.LiberarObjetoGenerico(oUserFieldsMD);
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
                oUserObjectMD.CanCancel = SAPbobsCOM.BoYesNoEnum.tYES;
                oUserObjectMD.CanClose = SAPbobsCOM.BoYesNoEnum.tYES;
                oUserObjectMD.CanCreateDefaultForm = SAPbobsCOM.BoYesNoEnum.tYES;
                oUserObjectMD.CanDelete = SAPbobsCOM.BoYesNoEnum.tYES;
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
                    Comun.Mensajes.Errores(19, "Funciones DIAPI - CreaUDOMD - " + errMsg);
                }
                else
                {
                    Comun.Mensajes.Exitos(4, "Funciones DIAPI - CreaUDOMD - ");
                }

                Comun.FuncionesComunes.LiberarObjetoGenerico(oUserObjectMD);
                oUserObjectMD = null;
                GC.Collect();
            }
            catch
            {

            }
        }
    }
}
