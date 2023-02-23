using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAPbouiCOM.Framework;
using SAPbobsCOM;

namespace Soindus.AddOnRindegastos.SBO
{

    public class EstructuraSBO
    {
        public static int ret = 0;
        public static string errMsg = string.Empty;
        public static Clases.Configuracion ExtConf;
        public static string localizacion = string.Empty;

        /// <summary>
        /// Crea estructura de datos del Addon
        /// </summary>
        public static void VerificarEstructuraMD()
        {
            ExtConf = new Clases.Configuracion();
            localizacion = ExtConf.Parametros.Localizacion;
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
                CreaTablaMD("SO_RENDI", "Rendiciones RG", SAPbobsCOM.BoUTBTableType.bott_Document);
                CreaTablaMD("SO_RENDICE", "Rendiciones RG Extras", SAPbobsCOM.BoUTBTableType.bott_DocumentLines);
                CreaTablaMD("SO_RENDIG", "Rendiciones RG Gastos", SAPbobsCOM.BoUTBTableType.bott_Document);
                CreaTablaMD("SO_RENDIGCE", "Rendiciones RG Gastos Extras", SAPbobsCOM.BoUTBTableType.bott_DocumentLines);
                CreaTablaMD("SO_RENDICF", "Rendiciones RG Config.", SAPbobsCOM.BoUTBTableType.bott_MasterData);
                if (localizacion.Equals("LATINEQ") || localizacion.Equals("LATINEUY"))
                {
                    CreaTablaMD("SO_RENDICAT", "Rendiciones RG Categorías", SAPbobsCOM.BoUTBTableType.bott_NoObjectAutoIncrement);
                }
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

            ////Campos de usuario para tabla rendiciones
            CreaCampoMD("@SO_RENDI", "ID", "Id Rendición", 11, SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_RENDI", "TITLE", "Title", 254, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_RENDI", "REPNUM", "ReportNumber", 11, SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_RENDI", "SDATE", "SendDate", 10, SAPbobsCOM.BoFieldTypes.db_Date, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_RENDI", "CDATE", "CloseDate", 10, SAPbobsCOM.BoFieldTypes.db_Date, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_RENDI", "EMPID", "EmployeeId", 11, SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_RENDI", "EMPNAME", "EmployeeName", 254, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_RENDI", "EMPIDE", "EmployeeIdentification", 254, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_RENDI", "APPID", "ApproverId", 11, SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_RENDI", "APPNAME", "ApproverName", 254, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_RENDI", "POLID", "PolicyId", 11, SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_RENDI", "POLNAME", "PolicyName", 254, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_RENDI", "STATUS", "Status", 11, SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_RENDI", "CSTATUS", "CustomStatus", 254, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_RENDI", "FUNDID", "FundId", 11, SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_RENDI", "FUNDNAME", "FundName", 254, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_RENDI", "REPTOT", "ReportTotal", 30, SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Price, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_RENDI", "REPTOTA", "ReportTotalApproved", 30, SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Price, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_RENDI", "CUR", "Currency", 10, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_RENDI", "NOTE", "Note", 254, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_RENDI", "INTEG", "Integrated", 254, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_RENDI", "INTDATE", "IntegrationDate", 254, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_RENDI", "INTECODE", "IntegrationExternalCode", 254, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_RENDI", "INTICODE", "IntegrationInternalCode", 254, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_RENDI", "NBREXP", "NbrExpenses", 11, SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_RENDI", "NBRAEXP", "NbrApprovedExpenses", 11, SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_RENDI", "NBRREXP", "NbrRejectedExpenses", 11, SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_RENDI", "ESTADO", "SO Estado", 11, SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_RENDI", "OBS", "SO Observación", 254, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_RENDI", "GESTION", "SO Gestión", 254, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);

            ////Campos de usuario para tabla rendiciones campos extras
            CreaCampoMD("@SO_RENDICE", "NAME", "Name", 254, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_RENDICE", "VALUE", "Value", 254, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_RENDICE", "CODE", "Code", 254, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);

            ////Campos de usuario para tabla gastos de rendiciones
            CreaCampoMD("@SO_RENDIG", "ID", "Id Gasto", 11, SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_RENDIG", "STATUS", "Status", 11, SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_RENDIG", "SUPPLIER", "Supplier", 254, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_RENDIG", "IDATE", "IssueDate", 10, SAPbobsCOM.BoFieldTypes.db_Date, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_RENDIG", "OAMOUNT", "OriginalAmount", 30, SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Price, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_RENDIG", "OCUR", "OriginalCurrency", 10, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_RENDIG", "EXRATE", "ExchangeRate", 30, SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Price, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_RENDIG", "NET", "Net", 30, SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Price, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_RENDIG", "TAX", "Tax", 30, SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Price, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_RENDIG", "TAXNAME", "TaxName", 50, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_RENDIG", "OTAX", "OtherTaxes", 30, SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Price, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_RENDIG", "RETNAME", "RetentionName", 254, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_RENDIG", "RET", "Retention", 30, SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Price, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_RENDIG", "TOTAL", "Total", 30, SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Price, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_RENDIG", "CUR", "Currency", 10, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_RENDIG", "REIMB", "Reimbursable", 1, SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_RENDIG", "CATEGORY", "Category", 254, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_RENDIG", "CATCODE", "CategoryCode", 254, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_RENDIG", "CATGRP", "CategoryGroup", 254, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_RENDIG", "CATGRPC", "CategoryGroupCode", 254, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_RENDIG", "NOTE", "Note", 254, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_RENDIG", "INTDATE", "IntegrationDate", 254, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_RENDIG", "INTECODE", "IntegrationExternalCode", 254, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_RENDIG", "NBRFILES", "NbrFiles", 11, SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_RENDIG", "RID", "ReportId", 11, SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_RENDIG", "EXPOLID", "ExpensePolicyId", 11, SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_RENDIG", "USERID", "UserId", 11, SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);

            ////Campos de usuario para tabla gastos de rendiciones campos extras
            CreaCampoMD("@SO_RENDIGCE", "NAME", "Name", 254, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_RENDIGCE", "VALUE", "Value", 254, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_RENDIGCE", "CODE", "Code", 254, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);

            ////Campos de usuario para tabla de configuración monitor
            Clases.ValorValido[] valoresCamposExtras = new Clases.ValorValido[]
                { new Clases.ValorValido("VALUE", "Value"),
                  new Clases.ValorValido("CODE", "Code")};
            CreaCampoMD("@SO_RENDICF", "TOKEN", "Token Rindegastos", 254, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_RENDICF", "RUTSOC", "Rut Sociedad", 100, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_RENDICF", "LOCALIZ", "Localización", 100, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_RENDICF", "G_CCOST", "Campo Centro de Costo", 100, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_RENDICF", "G_CCOSTV", "Valor Campo Centro de Costo", 100, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, "CODE", valoresCamposExtras, null);
            CreaCampoMD("@SO_RENDICF", "G_TIPOD", "Campo Tipo Documento", 100, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_RENDICF", "G_TIPODV", "Valor Campo Tipo Documento", 100, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, "CODE", valoresCamposExtras, null);
            CreaCampoMD("@SO_RENDICF", "G_CODFA", "Código Factura Afecta", 100, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_RENDICF", "G_CODFE", "Código Factura Exenta", 100, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_RENDICF", "G_CODFM", "Código Factura Materiales", 100, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_RENDICF", "G_RUTP", "Campo Rut Proveedor", 100, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_RENDICF", "G_RUTPV", "Valor Campo Rut Proveedor", 100, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, "VALUE", valoresCamposExtras, null);
            CreaCampoMD("@SO_RENDICF", "G_NDOC", "Campo Número Documento", 100, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_RENDICF", "G_NDOCV", "Valor Campo Número Documento", 100, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, "VALUE", valoresCamposExtras, null);
            CreaCampoMD("@SO_RENDICF", "C_CTAANTP", "Cuenta Anticipo Proveedores", 100, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_RENDICF", "C_CTASERP", "Cuenta Servicios Proveedores", 100, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_RENDICF", "C_CTACMAT", "Cuenta Compra Materiales", 100, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_RENDICF", "C_CTACFAC", "Cuenta Compensación Facturas", 100, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            CreaCampoMD("@SO_RENDICF", "C_CTAPREM", "Cuenta Pago Reembolsos", 100, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);

            ////Campos de usuario para tabla de categorías contables
            if (localizacion.Equals("LATINEQ") || localizacion.Equals("LATINEUY"))
            {
                CreaCampoMD("@SO_RENDICAT", "CAT1", "Tipo Llamada", 30, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
                CreaCampoMD("@SO_RENDICAT", "CAT2", "Línea", 30, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
                CreaCampoMD("@SO_RENDICAT", "CAT3", "Categoría 3", 30, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
                CreaCampoMD("@SO_RENDICAT", "CAT4", "Categoría", 30, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
                CreaCampoMD("@SO_RENDICAT", "CAT5", "Categoría 5", 30, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
                CreaCampoMD("@SO_RENDICAT", "CTACTB", "Cuenta contable", 15, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            }

            ////Campos de usuario para tablas SAP
            if(localizacion.Equals("SOINDUS") || localizacion.Equals("ICLINICS") || localizacion.Equals("PCLINICS") || localizacion.Equals("LATINEQ") || localizacion.Equals("LATINEUY") || localizacion.Equals("SUATRANS") || localizacion.Equals("SUATRANSTR") || localizacion.Equals("SUATRANSPE"))
            {
                CreaCampoMD("OINV", "SO_NUMER", "Número Entrega a Rendir", 50, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
                CreaCampoMD("OVPM", "SO_NUMER", "Número Entrega a Rendir", 50, SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, SAPbobsCOM.BoYesNoEnum.tNO, null, null, null);
            }
        }

        /// <summary>
        /// Funcion que permite cargar el UDO necesario para el funcionamiento del AddOn
        /// </summary>
        private static void CargarUDO()
        {
            try
            {
                CreaUDOMD("SO_RENDI", "SO_RENDICE", "SO_RENDICION", "Rendiciones RG", SAPbobsCOM.BoUDOObjType.boud_Document);
                CreaUDOMD("SO_RENDIG", "SO_RENDIGCE", "SO_RENDIGTO", "Gastos Rendiciones RG", SAPbobsCOM.BoUDOObjType.boud_Document);
                CreaUDOMD("SO_RENDICF", "", "SO_RENDICF", "Conf. Monitor Rindegastos Soindus", SAPbobsCOM.BoUDOObjType.boud_MasterData);
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
