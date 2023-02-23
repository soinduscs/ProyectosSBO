using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soindus.AddOnEmisionDTE.SBO
{
    public class EstructuraSBO
    {
        public static int ret = 0;
        public static string errMsg = string.Empty;
        private static Local.Configuracion ExtConf = new Local.Configuracion();

        /// <summary>
        /// Crea estructura de datos del Addon
        /// </summary>
        public static void VerificarEstructuraMD()
        {
            ExtConf = new Local.Configuracion();
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
                ////Configuraciones
                CreaTablaMD("SO_PARAMSFE", "SO Parámetros Extras FE", SAPbobsCOM.BoUTBTableType.bott_NoObject);
                if (ExtConf.Parametros != null)
                {
                    if (ExtConf.Parametros.Proveedor_FE.Equals("DBN-SO") || ExtConf.Parametros.Proveedor_FE.Equals("DBN-EC"))
                    {
                        ////Catalogos
                        CreaTablaMD("SO_ACTECONFE", "SO Actividades económicas FE", SAPbobsCOM.BoUTBTableType.bott_NoObject);
                        CreaTablaMD("SO_FORMAPAGO", "SO Formas de Pago FE", SAPbobsCOM.BoUTBTableType.bott_NoObject);
                        if (ExtConf.Parametros.Proveedor_FE.Equals("DBN-SO"))
                        {
                            CreaTablaMD("SO_TIPOVTA", "SO Tipos de Venta", SAPbobsCOM.BoUTBTableType.bott_NoObject);
                            CreaTablaMD("SO_TIPODESP", "SO Tipos de Despacho", SAPbobsCOM.BoUTBTableType.bott_NoObject);
                            CreaTablaMD("SO_TIPOTRAS", "SO Tipos de Traslado", SAPbobsCOM.BoUTBTableType.bott_NoObject);
                            CreaTablaMD("SO_TDBSII", "SO Tipos Doc. Base SII", SAPbobsCOM.BoUTBTableType.bott_NoObject);
                            CreaTablaMD("SO_CODREFDB", "SO Códigos Ref. Doc. Base", SAPbobsCOM.BoUTBTableType.bott_NoObject);
                            CreaTablaMD("SO_EX_MV", "SO EX Mod. de Venta Export.", SAPbobsCOM.BoUTBTableType.bott_NoObject);
                            CreaTablaMD("SO_EX_CV", "SO EX Claus. de Venta Export.", SAPbobsCOM.BoUTBTableType.bott_NoObject);
                            CreaTablaMD("SO_EX_FP", "SO EX Formas de Pago Export.", SAPbobsCOM.BoUTBTableType.bott_NoObject);
                            CreaTablaMD("SO_EX_PTOS", "SO EX Puertos Export.", SAPbobsCOM.BoUTBTableType.bott_NoObject);
                            CreaTablaMD("SO_EX_PAISES", "SO EX Países Export.", SAPbobsCOM.BoUTBTableType.bott_NoObject);
                            CreaTablaMD("SO_EX_VTRANSP", "SO EX Vías de Transp. Export.", SAPbobsCOM.BoUTBTableType.bott_NoObject);
                            CreaTablaMD("SO_EX_TBULTOS", "SO EX Tipos de Bultos Export.", SAPbobsCOM.BoUTBTableType.bott_NoObject);
                        }
                    }
                }
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
            ////Campos de configuración
            CreaCampoMD("@SO_PARAMSFE", "DESCRIPCION", "Descripción", 1, SAPbobsCOM.BoFieldTypes.db_Memo, SAPbobsCOM.BoFldSubTypes.st_Link, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null, null);
            if (ExtConf.Parametros != null)
            {
                if (ExtConf.Parametros.Proveedor_FE.Equals("DBN-SO") || ExtConf.Parametros.Proveedor_FE.Equals("DBN-EC"))
                {
                    ////Campos de catálogos
                    CreaCampoMD("@SO_FORMAPAGO", "FMAPAGO", "SO Forma de pago FE", 10, SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null, null);
                    if (ExtConf.Parametros.Proveedor_FE.Equals("DBN-SO"))
                    {
                        ////Campos maestros
                        ////Usuarios Impresión
                        CreaCampoMD("OUSR", "SO_PRINTER", "SO Impresora FE", 100, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null, null);
                        ////Socios de Negocios
                        Local.ValorValido[] valoresSiNo = new Local.ValorValido[]
                            { new Local.ValorValido("Y", "Si"),
                         new Local.ValorValido("N", "No") };
                        CreaCampoMD("OCRD", "SO_IMPCATALOGO", "SO Imprime Num. Catálogo", 1, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, "N", valoresSiNo, null, null);
                        CreaCampoMD("OCRD", "SO_IMPCODIGO2", "SO Imprime Codigo2", 1, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, "N", valoresSiNo, null, null);
                        CreaCampoMD("OCRD", "SO_IMPDESCRIPCION2", "SO Imprime Descripción2", 1, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, "N", valoresSiNo, null, null);
                        ////Número de Catálogo Cliente Proveedor
                        CreaCampoMD("OSCN", "SO_CODIGO2", "SO Código2", 50, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null, null);
                        CreaCampoMD("OSCN", "SO_DESCRIPCION2", "SO Descripción2", 50, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null, null);
                        ////Campos documentos de marketing
                        ////Venta y Guias
                        Local.ValorValido[] valoresTipoFact = new Local.ValorValido[]
                            { new Local.ValorValido("0", "Normal"),
                        new Local.ValorValido("1", "Resumida") };
                        CreaCampoMD("OINV", "SO_TIPOFACT", "SO Tipo de Factura Elect.", 1, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, "0", valoresTipoFact, null, null);
                        Local.ValorValido[] valoresEmiteGuia = new Local.ValorValido[]
                            { new Local.ValorValido("0", "No"),
                        new Local.ValorValido("1", "Si") };
                        CreaCampoMD("OINV", "SO_EMITEGUIA", "SO Emite Guía Elect.", 1, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, "1", valoresEmiteGuia, null, null);
                        CreaCampoMD("OINV", "SO_NOTAVTA", "SO Nota de venta", 15, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null, null);
                        CreaCampoMD("OINV", "SO_GUIADESP", "SO Guía de despacho", 15, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null, null);
                        CreaCampoMD("OINV", "SO_TIPOVTA", "SO Tipo de venta", 50, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, "SO_TIPOVTA", null);
                        CreaCampoMD("OINV", "SO_TIPODESP", "SO Tipo de despacho", 50, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, "SO_TIPODESP", null);
                        CreaCampoMD("OINV", "SO_TIPOTRAS", "SO Tipo de traslado", 50, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, "SO_TIPOTRAS", null);
                        ////ND NC
                        CreaCampoMD("OINV", "SO_TDBSII", "SO Tipo Doc. Base SII", 50, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, "SO_TDBSII", null);
                        CreaCampoMD("OINV", "SO_CODREFDB", "SO Cod. Ref. Doc. Base", 50, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, "SO_CODREFDB", null);
                        CreaCampoMD("OINV", "SO_FOLIODB", "SO Folio Doc. Base", 11, SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null, null);
                        CreaCampoMD("OINV", "SO_FECHADB", "SO Fecha Doc. Base", 10, SAPbobsCOM.BoFieldTypes.db_Date, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null, null);
                        ////Transporte
                        CreaCampoMD("OINV", "SO_RUTTRANS", "SO Rut Transportista", 30, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null, null);
                        CreaCampoMD("OINV", "SO_NOMTRANS", "SO Nombre Transportista", 30, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null, null);
                        CreaCampoMD("OINV", "SO_DESTENVIO", "SO Lugar de Envío", 50, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null, null);
                        CreaCampoMD("OINV", "SO_PATENTE", "SO Patente", 10, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null, null);
                        CreaCampoMD("OINV", "SO_BULTOS", "SO Cant. de Bultos", 10, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null, null);
                        CreaCampoMD("OINV", "SO_NPALLETS", "SO Cant. de Pallets", 10, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null, null);
                        CreaCampoMD("OINV", "SO_NGPALLETS", "SO Nro. Guía Pallets", 10, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null, null);
                        ////Exportación
                        CreaCampoMD("OINV", "SO_EX_MV", "SO EX Modalidad de venta", 50, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, "SO_EX_MV", null);
                        CreaCampoMD("OINV", "SO_EX_CV", "SO EX Cláusula de venta", 50, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, "SO_EX_CV", null);
                        CreaCampoMD("OINV", "SO_EX_FP", "SO EX Forma de pago", 50, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, "SO_EX_FP", null);
                        CreaCampoMD("OINV", "SO_EX_PTOEMB", "SO EX Puerto de embarque", 50, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, "SO_EX_PTOS", null);
                        CreaCampoMD("OINV", "SO_EX_PTODES", "SO EX Puerto de desembarque", 50, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, "SO_EX_PTOS", null);
                        CreaCampoMD("OINV", "SO_EX_PAIS", "SO EX País de destino", 50, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, "SO_EX_PAISES", null);
                        CreaCampoMD("OINV", "SO_EX_VTRANSP", "SO EX Vía de Transporte", 50, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, "SO_EX_VTRANSP", null);
                        CreaCampoMD("OINV", "SO_EX_TBULTOS", "SO EX Tipo de Bultos", 50, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, "SO_EX_TBULTOS", null);
                        CreaCampoMD("OINV", "SO_EX_NBULTOS", "SO EX Cant. de Bultos", 11, SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null, null);

                        ////Campos de usuario para tabla configuración
                        //CreaCampoMD("@SO_XMONCONF", "TOKEN", "Token de Conexión Portador DTE", 254, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
                    }
                }
            }
        }

        /// <summary>
        /// Funcion que permite cargar el UDO necesario para el funcionamiento del AddOn
        /// </summary>
        private static void CargarUDO()
        {
            try
            {
                //CreaUDOMD("SO_XMONCONF", "", "SO_XMONCONF", "Conf. Monitor de Emisión", SAPbobsCOM.BoUDOObjType.boud_MasterData);
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
