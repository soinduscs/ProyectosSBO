using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAPbouiCOM.Framework;
using SAPbobsCOM;

namespace Soindus.AddOnMonitorProveedores.SBO
{

    public class FuncionesDIAPI
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
                CreaTablaMD("SO_MONITOR", "Monitor documentos proveedores", SAPbobsCOM.BoUTBTableType.bott_Document);
                CreaTablaMD("SO_MONCONF", "Config. Monitor proveedores", SAPbobsCOM.BoUTBTableType.bott_MasterData);
                CreaTablaMD("SO_ENCABEZADOSDTE", "Validac. de encabezados DTE", SAPbobsCOM.BoUTBTableType.bott_NoObjectAutoIncrement);
                CreaTablaMD("SO_USERMX", "Aut. Usuario DTE Moneda Ext.", SAPbobsCOM.BoUTBTableType.bott_NoObjectAutoIncrement);
                CreaTablaMD("SO_USERSINOC", "Aut. Usuario DTE Sin OC", SAPbobsCOM.BoUTBTableType.bott_NoObjectAutoIncrement);
                CreaTablaMD("SO_RELACMONTC", "Rel. Monedas y TC", SAPbobsCOM.BoUTBTableType.bott_NoObjectAutoIncrement);
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
            ////Campos de usuario para tablas SAP
            Local.ValorValido[] valoresSINO = new Local.ValorValido[]
                { new Local.ValorValido("Y", "Si"),
                  new Local.ValorValido("N", "No")};
            CreaCampoMD("OCRD", "SO_PROVOC", "Proveedor orden de compra", 30, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, "Y", valoresSINO, null);
            CreaCampoMD("OCRD", "SO_RESP", "Empleado responsable", 10, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null, SAPbobsCOM.UDFLinkedSystemObjectTypesEnum.ulEmployeesInfo);

            ////Campos de usuario para tabla monitor  
            CreaCampoMD("@SO_MONITOR", "PLAZO", "Plazo DTE", 10, SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_MONITOR", "DOCID", "Identificador documento", 80, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_MONITOR", "TIPODOC", "Tipo de documento", 8, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_MONITOR", "FOLIO", "Folio", 30, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_MONITOR", "FECHAEM", "Fecha de emisión", 10, SAPbobsCOM.BoFieldTypes.db_Date, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_MONITOR", "FORMAPA", "Forma de pago", 2, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_MONITOR", "IDETRAS", "Identificador traslado", 8, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_MONITOR", "RUTEMIS", "Rut del emisor", 15, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_MONITOR", "RAZSOCE", "Razón social del emisor", 100, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_MONITOR", "RUTRECE", "Rut del receptor", 15, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_MONITOR", "RAZSOCR", "Razón social del receptor", 100, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_MONITOR", "CONTACT", "Contacto", 100, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_MONITOR", "IVA", "IVA", 30, SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Price, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_MONITOR", "MONTOTO", "Monto total", 30, SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Price, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_MONITOR", "ESTSII", "Estado SII", 80, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_MONITOR", "ESTCOME", "Estado comercial", 80, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_MONITOR", "FECHARE", "Fecha de recepción", 10, SAPbobsCOM.BoFieldTypes.db_Date, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_MONITOR", "TIPO", "Tipo", 2, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_MONITOR", "CODSII", "Código SII", 80, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_MONITOR", "TIENENC", "Tiene nota de crédito", 80, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_MONITOR", "TIENEND", "Tiene nota de débito", 80, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_MONITOR", "DOCBASE", "Documento base", 80, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_MONITOR", "TIPOASO", "Tipo de documento asociado", 2, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_MONITOR", "DOCASO", "Documento asociado", 80, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_MONITOR", "RAZONRE", "Razón de rechazo", 100, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            Local.ValorValido[] valoresINTR = new Local.ValorValido[]
                { new Local.ValorValido("0", "Sin procesar"),
                  new Local.ValorValido("1", "Validado"),
                  new Local.ValorValido("2", "Aceptado"),
                  new Local.ValorValido("3", "Rechazado"),
                  new Local.ValorValido("4", "Aceptado con reparos"),
                  new Local.ValorValido("5", "Integrado")};
            CreaCampoMD("@SO_MONITOR", "ESTADO", "Estado", 2, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, "0", valoresINTR, null);
            CreaCampoMD("@SO_MONITOR", "PRELIM", "Documento preliminar", 80, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_MONITOR", "RUTCES", "Rut del cesionario", 15, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_MONITOR", "RAZSOCC", "Razón social del cesionario", 100, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_MONITOR", "FECHACE", "Fecha de cesión", 10, SAPbobsCOM.BoFieldTypes.db_Date, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_MONITOR", "CODRESP", "Código empleado responsable", 10, SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_MONITOR", "NOMRESP", "Nombre empleado responsable", 100, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_MONITOR", "CODRESPD", "Código responsable documento", 10, SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_MONITOR", "NOMRESPD", "Nombre responsable documento", 100, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_MONITOR", "OBS", "Observaciones", 254, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);

            ////Campos de usuario para tabla de configuración monitor
            Local.ValorValido[] valoresPFE = new Local.ValorValido[]
                { new Local.ValorValido("SO", "SoindusFE"),
                  new Local.ValorValido("FEB", "Febos"),
                  new Local.ValorValido("DBN", "DBNet"),
                  new Local.ValorValido("FAC", "Facele"),
                  new Local.ValorValido("AZU", "Azurian"),
                  new Local.ValorValido("SID", "Sidge")};
            CreaCampoMD("@SO_MONCONF", "PROVFE", "Proveedor FE", 30, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, "FEB", valoresPFE, null);
            CreaCampoMD("@SO_MONCONF", "TOKEN", "Token de conexión proveedor FE", 1, SAPbobsCOM.BoFieldTypes.db_Memo, SAPbobsCOM.BoFldSubTypes.st_Link, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_MONCONF", "RRECEP", "Rut del receptor de DTE", 15, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_MONCONF", "RECINTO", "Recinto para rechazo de DTE", 30, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_MONCONF", "G_RESP", "Visualizar responsable", 1, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, "Y", valoresSINO, null);
            CreaCampoMD("@SO_MONCONF", "G_RESPD", "Visualizar responsable documento", 1, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, "Y", valoresSINO, null);
            CreaCampoMD("@SO_MONCONF", "G_CESION", "Visualizar campos cesión", 1, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, "Y", valoresSINO, null);
            CreaCampoMD("@SO_MONCONF", "V_OC", "Validar existencia OC", 1, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, "Y", valoresSINO, null);
            CreaCampoMD("@SO_MONCONF", "V_ENT", "Validar existencia entradas", 1, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, "Y", valoresSINO, null);
            CreaCampoMD("@SO_MONCONF", "V_MTO", "Validar monto máximo", 1, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, "Y", valoresSINO, null);
            CreaCampoMD("@SO_MONCONF", "V_MTOVAL", "Monto máx. permitido", 30, SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Price, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_MONCONF", "V_OCMAN", "Permitir ingreso OC manual", 1, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, "Y", valoresSINO, null);
            CreaCampoMD("@SO_MONCONF", "V_ENCDTE", "Validar encabezados DTE", 1, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, "Y", valoresSINO, null);
            CreaCampoMD("@SO_MONCONF", "V_MTOOC", "Validar monto OC", 1, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, "Y", valoresSINO, null);
            CreaCampoMD("@SO_MONCONF", "V_TOLER", "Permitir tolerancias OC", 1, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, "Y", valoresSINO, null);
            CreaCampoMD("@SO_MONCONF", "V_RMEVAL", "Monto rechazo menor a", 30, SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Price, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_MONCONF", "V_RMAVAL", "Monto rechazo mayor a", 30, SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Price, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_MONCONF", "V_AMEVAL", "Monto aprobación menor a", 30, SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Price, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_MONCONF", "V_AMAVAL", "Monto aprobación mayor a", 30, SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Price, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_MONCONF", "I_MB", "Solicitar Multibranch", 1, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, "Y", valoresSINO, null);

            ////Campos de usuario para tabla de validación de encabezados DTE
            CreaCampoMD("@SO_ENCABEZADOSDTE", "TEXTO", "Texto válido", 254, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);

            ////Campos de usuario para tabla de relación monedas y tipos de cambio
            CreaCampoMD("@SO_RELACMONTC", "MONTC", "Moneda TC", 3, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
        }

        /// <summary>
        /// Funcion que permite cargar el UDO necesario para el funcionamiento del AddOn
        /// </summary>
        private static void CargarUDO()
        {
            try
            {
                CreaUDOMD("SO_MONITOR", "", "SO_MONITOR", "Monitor de Proveedores Soindus", SAPbobsCOM.BoUDOObjType.boud_Document);
                CreaUDOMD("SO_MONCONF", "", "SO_MONCONF", "Conf. Monitor de Proveedores Soindus", SAPbobsCOM.BoUDOObjType.boud_MasterData);
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
        public static void CreaUDOMD(string sNombreTablaPadre, String sNombreTablaHijo, String sCodigoUDO, String sNameUDO, SAPbobsCOM.BoUDOObjType oUDOObjType)
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
                //oUserObjectMD.ChildTables.TableName = sNombreTablaHijo;
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
