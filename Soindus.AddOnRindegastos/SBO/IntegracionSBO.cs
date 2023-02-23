using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAPbobsCOM;

namespace Soindus.AddOnRindegastos.SBO
{
    public class IntegracionSBO
    {
        public IntegracionSBO()
        {
        }

        public static Clases.Message ContabilizarSolicitudes(Clases.Rendiciones Rendiciones)
        {
            Clases.Configuracion ExtConf = new Clases.Configuracion();
            //Parametrizables
            string rut_sociedad = ExtConf.Parametros.Rut_Sociedad;
            string localizacion = ExtConf.Parametros.Localizacion;
            string cta_ant_proveedor = ExtConf.Parametros.Cuenta_Anticipo_Proveedores; //"1-1-100-20-000";
            string cta_serv_proveedor = ExtConf.Parametros.Cuenta_Servicios_Proveedores; //"6-1-010-40-000";
            string cta_compra_materiales = ExtConf.Parametros.Cuenta_Compra_Materiales; //"1-1-100-20-000";
            string cta_compensacion_facturas = ExtConf.Parametros.Cuenta_Compensacion_Facturas; //"1-1-100-20-000";
            string cta_pago_reembolsos = ExtConf.Parametros.Cuenta_Pago_Reembolsos; //"1-1-100-20-000";
            string campo_extra_centro_costo_nombre = ExtConf.Parametros.Campo_Centro_Costo; //"Centro de Costo";
            string campo_extra_centro_costo_valor = ExtConf.Parametros.Campo_Centro_Costo_Valor; //"CODE";
            string campo_extra_tipo_documento_nombre = ExtConf.Parametros.Campo_Tipo_Documento; //"Tipo de Documento";
            string campo_extra_tipo_documento_valor = ExtConf.Parametros.Campo_Tipo_Documento_Valor; //"CODE";
            string codigo_factura = ExtConf.Parametros.Codigo_Factura_Afecta; //"01";
            string codigo_factura_ex = ExtConf.Parametros.Codigo_Factura_Exenta; //"02";
            string codigo_factura_mat = ExtConf.Parametros.Codigo_Factura_Materiales; //"04";
            string campo_extra_rut_proveedor_nombre = ExtConf.Parametros.Campo_Rut_Proveedor; //"RUC Proveedor";
            string campo_extra_rut_proveedor_valor = ExtConf.Parametros.Campo_Rut_Proveedor_Valor; //"VALUE";
            string campo_extra_numero_documento_nombre = ExtConf.Parametros.Campo_Numero_Documento; //"Número de Documento";
            string campo_extra_numero_documento_valor = ExtConf.Parametros.Campo_Numero_Documento_Valor; //"VALUE";
            //Fin Parametrizables
            string cta_gasto = "6-1-020-10-000"; // Sólo Pruebas
            string tipo_gasto = "Gasto";
            string rut_proveedor = string.Empty;
            string centro_costo = string.Empty;
            string dimension = string.Empty;
            string numero_documento = string.Empty;
            string Observaciones = string.Empty;
            string nombre_fondo = string.Empty;
            string ctacontable_fondo = string.Empty;
            string proyecto = string.Empty;
            string ctacontable_pago_fondo = string.Empty;
            //Logicos Localización
            bool borrador = false;
            bool borrador_1er_asiento = false;
            bool borrador_factura = false;
            bool borrador_compensa_factura_pagoef = false;
            bool borrador_compensa_factura_asiento = false;
            bool borrador_pago_a_favor = false;
            bool cuenta_en_gastos = false;
            bool registrar_campos_soindus = false;
            bool compensar_factura_con_pago = false;
            bool registrar_folio = false;
            bool registrar_indicador = false;
            bool pago_con_cta_reembolso = false;
            bool cerrar_fondo_compensado = false;
            switch (localizacion)
            {
                case "SOINDUS":
                    borrador = false;
                    borrador_1er_asiento = false;
                    borrador_factura = false;
                    borrador_compensa_factura_pagoef = false;
                    borrador_compensa_factura_asiento = false;
                    borrador_pago_a_favor = false;
                    cuenta_en_gastos = true;
                    registrar_campos_soindus = true;
                    compensar_factura_con_pago = true;
                    registrar_folio = true;
                    registrar_indicador = true;
                    pago_con_cta_reembolso = true;
                    cerrar_fondo_compensado = true;
                    break;
                case "FLESAN1":
                    borrador = true;
                    borrador_1er_asiento = true;
                    borrador_factura = true;
                    borrador_compensa_factura_pagoef = true;
                    borrador_compensa_factura_asiento = true;
                    borrador_pago_a_favor = true;
                    cuenta_en_gastos = true;
                    registrar_campos_soindus = false;
                    compensar_factura_con_pago = false;
                    registrar_folio = false;
                    registrar_indicador = false;
                    pago_con_cta_reembolso = false;
                    cerrar_fondo_compensado = false;
                    break;
                case "ICLINICS":
                    borrador = false;
                    borrador_1er_asiento = false;
                    borrador_factura = false;
                    borrador_compensa_factura_pagoef = false;
                    borrador_compensa_factura_asiento = false;
                    borrador_pago_a_favor = false;
                    cuenta_en_gastos = true;
                    registrar_campos_soindus = true;
                    compensar_factura_con_pago = true;
                    registrar_folio = true;
                    registrar_indicador = true;
                    pago_con_cta_reembolso = true;
                    cerrar_fondo_compensado = false;
                    break;
                case "PCLINICS":
                    borrador = false;
                    borrador_1er_asiento = false;
                    borrador_factura = false;
                    borrador_compensa_factura_pagoef = false;
                    borrador_compensa_factura_asiento = false;
                    borrador_pago_a_favor = false;
                    cuenta_en_gastos = true;
                    registrar_campos_soindus = true;
                    compensar_factura_con_pago = true;
                    registrar_folio = true;
                    registrar_indicador = true;
                    pago_con_cta_reembolso = true;
                    cerrar_fondo_compensado = false;
                    break;
                case "LATINEQ":
                    borrador = false;
                    borrador_1er_asiento = false;
                    borrador_factura = false;
                    borrador_compensa_factura_pagoef = false;
                    borrador_compensa_factura_asiento = false;
                    borrador_pago_a_favor = false;
                    cuenta_en_gastos = true;
                    registrar_campos_soindus = true;
                    compensar_factura_con_pago = true;
                    registrar_folio = true;
                    registrar_indicador = true;
                    pago_con_cta_reembolso = true;
                    cerrar_fondo_compensado = true;
                    break;
                case "LATINEUY":
                    borrador = false;
                    borrador_1er_asiento = false;
                    borrador_factura = false;
                    borrador_compensa_factura_pagoef = false;
                    borrador_compensa_factura_asiento = false;
                    borrador_pago_a_favor = false;
                    cuenta_en_gastos = true;
                    registrar_campos_soindus = true;
                    compensar_factura_con_pago = true;
                    registrar_folio = true;
                    registrar_indicador = true;
                    pago_con_cta_reembolso = true;
                    cerrar_fondo_compensado = true;
                    break;
                case "SUATRANS":
                    borrador = false;
                    borrador_1er_asiento = false;
                    borrador_factura = false;
                    borrador_compensa_factura_pagoef = false;
                    borrador_compensa_factura_asiento = false;
                    borrador_pago_a_favor = false;
                    cuenta_en_gastos = true;
                    registrar_campos_soindus = true;
                    compensar_factura_con_pago = true;
                    registrar_folio = true;
                    registrar_indicador = true;
                    pago_con_cta_reembolso = true;
                    cerrar_fondo_compensado = false;
                    break;
                case "SUATRANSTR":
                    borrador = false;
                    borrador_1er_asiento = false;
                    borrador_factura = false;
                    borrador_compensa_factura_pagoef = false;
                    borrador_compensa_factura_asiento = false;
                    borrador_pago_a_favor = false;
                    cuenta_en_gastos = true;
                    registrar_campos_soindus = true;
                    compensar_factura_con_pago = true;
                    registrar_folio = true;
                    registrar_indicador = true;
                    pago_con_cta_reembolso = true;
                    cerrar_fondo_compensado = false;
                    break;
                case "SUATRANSPE":
                    borrador = false;
                    borrador_1er_asiento = false;
                    borrador_factura = false;
                    borrador_compensa_factura_pagoef = false;
                    borrador_compensa_factura_asiento = false;
                    borrador_pago_a_favor = false;
                    cuenta_en_gastos = true;
                    registrar_campos_soindus = true;
                    compensar_factura_con_pago = true;
                    registrar_folio = true;
                    registrar_indicador = true;
                    pago_con_cta_reembolso = true;
                    cerrar_fondo_compensado = true;
                    break;
                default:
                    borrador = true;
                    borrador_1er_asiento = true;
                    borrador_factura = true;
                    borrador_compensa_factura_pagoef = true;
                    borrador_compensa_factura_asiento = true;
                    borrador_pago_a_favor = true;
                    cuenta_en_gastos = true;
                    registrar_campos_soindus = true;
                    compensar_factura_con_pago = true;
                    registrar_folio = true;
                    registrar_indicador = true;
                    pago_con_cta_reembolso = true;
                    cerrar_fondo_compensado = true;
                    break;
            }

            int errCode = 0;
            string errMsg = string.Empty;
            Clases.Message resp = new Clases.Message();
            Clases.Message update = new Clases.Message();
            string monedalocal = SBO.ConsultasSBO.ObtenerMonedaLocal();

            string memo = string.Empty;
            double totGastos = 0;
            int linea = 0;
            string _CardCodeRendidor = string.Empty;

            InterfazRG interfazRG = new InterfazRG();

            long gReportNum = 0;
            long gDocEntry = 0;
            try
            {
                foreach (var rendicion in Rendiciones.Items)
                {
                    bool algunError = false;
                    gReportNum = rendicion.Detalle.URepnum;
                    gDocEntry = rendicion.Detalle.DocEntry;
                    if (EsRendicionSociedad(rut_sociedad, rendicion))
                    {
                        switch (TipoGestion(rendicion))
                        {
                            case "SOLICITUD":
                                Observaciones = string.Empty;
                                if (localizacion.Equals("ICLINICS"))
                                {
                                    _CardCodeRendidor = SBO.ConsultasSBO.ObtenerCardCodeClienteConLetra(rendicion.Detalle.UEmpide, "R");
                                }
                                else if (localizacion.Equals("PCLINICS"))
                                {
                                    _CardCodeRendidor = SBO.ConsultasSBO.ObtenerCardCodeClienteConLetra(rendicion.Detalle.UEmpide, "R");
                                }
                                else if (localizacion.Equals("LATINEQ"))
                                {
                                    _CardCodeRendidor = SBO.ConsultasSBO.ObtenerCardCodeProveedor(rendicion.Detalle.UEmpide);
                                }
                                else if (localizacion.Equals("LATINEUY"))
                                {
                                    _CardCodeRendidor = SBO.ConsultasSBO.ObtenerCardCodeProveedor(rendicion.Detalle.UEmpide);
                                }
                                else if (localizacion.Equals("SUATRANS"))
                                {
                                    _CardCodeRendidor = SBO.ConsultasSBO.ObtenerCardCodeProveedor(rendicion.Detalle.UEmpide);
                                }
                                else if (localizacion.Equals("SUATRANSTR"))
                                {
                                    _CardCodeRendidor = SBO.ConsultasSBO.ObtenerCardCodeCliente(rendicion.Detalle.UEmpide);
                                }
                                else if (localizacion.Equals("SUATRANSPE"))
                                {
                                    _CardCodeRendidor = SBO.ConsultasSBO.ObtenerCardCodeProveedorConLetra(rendicion.Detalle.UEmpide, "E");
                                }
                                else
                                {
                                    _CardCodeRendidor = SBO.ConsultasSBO.ObtenerCardCode(rendicion.Detalle.UEmpide);
                                }
                                // Validar Existencia del Detalle de Gastos
                                if (rendicion.Detalle.Gastos == null || rendicion.Detalle.Gastos.Items.Count() <= 0)
                                {
                                    if (SBO.ConexionSBO.oCompany.InTransaction)
                                    {
                                        SBO.ConexionSBO.oCompany.EndTransaction(BoWfTransOpt.wf_RollBack);
                                    }
                                    Observaciones = string.Format("Solicitud: {0} - No existe detalle de gastos.", rendicion.Detalle.URepnum);
                                    update = SBO.ModeloSBO.UpdateRendicion(rendicion.Detalle.DocEntry.ToString(), 9, Observaciones.PadRight(250).Substring(0, 250).Trim());
                                    algunError = true;
                                    continue;
                                }
                                // Validar SN Empleado Rendidor
                                if (string.IsNullOrEmpty(_CardCodeRendidor))
                                {
                                    if (SBO.ConexionSBO.oCompany.InTransaction)
                                    {
                                        SBO.ConexionSBO.oCompany.EndTransaction(BoWfTransOpt.wf_RollBack);
                                    }
                                    Observaciones = string.Format("Pago Efectuado Solicitud: {0} - No existe socio de negocios {1}.", rendicion.Detalle.URepnum, rendicion.Detalle.UEmpide);
                                    update = SBO.ModeloSBO.UpdateRendicion(rendicion.Detalle.DocEntry.ToString(), 9, Observaciones.PadRight(250).Substring(0, 250).Trim());
                                    algunError = true;
                                    continue;
                                }
                                else
                                {
                                    if (localizacion.Equals("SUATRANS") || localizacion.Equals("SUATRANSTR"))
                                    {
                                        proyecto = string.Empty;
                                        foreach (var extrarend in rendicion.Detalle.CamposExtra.Detalle)
                                        {
                                            if (extrarend.UName.Equals("Proyecto"))
                                            {
                                                proyecto = extrarend.UValue;
                                            }
                                        }
                                    }
                                    if (localizacion.Equals("SUATRANSPE"))
                                    {
                                        ctacontable_pago_fondo = string.Empty;
                                        foreach (var extrarend in rendicion.Detalle.CamposExtra.Detalle)
                                        {
                                            if (extrarend.UName.Equals("Cuenta Fondo Solicitud"))
                                            {
                                                ctacontable_pago_fondo = extrarend.UCode;
                                            }
                                        }
                                    }

                                    //***PAGO EFECTUADO FONDO A RENDIR***//
                                    #region PAGO EFECTUADO
                                    SAPbobsCOM.Payments oPayment;
                                    if (borrador)
                                    {
                                        //preliminar
                                        oPayment = (SAPbobsCOM.Payments)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPaymentsDrafts);
                                    }
                                    else
                                    {
                                        //final
                                        oPayment = (SAPbobsCOM.Payments)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oVendorPayments);
                                    }
                                    oPayment.DocObjectCode = SAPbobsCOM.BoPaymentsObjectType.bopot_OutgoingPayments;
                                    if (localizacion.Equals("ICLINICS"))
                                    {
                                        oPayment.DocType = SAPbobsCOM.BoRcptTypes.rCustomer;
                                    }
                                    else if (localizacion.Equals("PCLINICS"))
                                    {
                                        oPayment.DocType = SAPbobsCOM.BoRcptTypes.rCustomer;
                                    }
                                    else if (localizacion.Equals("LATINEQ"))
                                    {
                                        oPayment.DocType = SAPbobsCOM.BoRcptTypes.rSupplier;
                                    }
                                    else if (localizacion.Equals("LATINEUY"))
                                    {
                                        oPayment.DocType = SAPbobsCOM.BoRcptTypes.rSupplier;
                                    }
                                    else if (localizacion.Equals("SUATRANS"))
                                    {
                                        oPayment.DocType = SAPbobsCOM.BoRcptTypes.rSupplier;
                                    }
                                    else if (localizacion.Equals("SUATRANSTR"))
                                    {
                                        oPayment.DocType = SAPbobsCOM.BoRcptTypes.rCustomer;
                                    }
                                    else if (localizacion.Equals("SUATRANSPE"))
                                    {
                                        oPayment.DocType = SAPbobsCOM.BoRcptTypes.rSupplier;
                                    }
                                    else
                                    {
                                        oPayment.DocType = SAPbobsCOM.BoRcptTypes.rCustomer;
                                    }
                                    memo = string.Empty;
                                    memo = "Fondo a Rendir N°" + rendicion.Detalle.URepnum.ToString() + " " + rendicion.Detalle.UEmpname;
                                    memo = memo.PadRight(250).Substring(0, 250).Trim();
                                    oPayment.Remarks = memo;
                                    if (localizacion.Equals("LATINEQ") || localizacion.Equals("LATINEUY"))
                                    {
                                        memo = memo.PadRight(50).Substring(0, 50).Trim();
                                        oPayment.JournalRemarks = memo;
                                    }
                                    oPayment.CardCode = _CardCodeRendidor;
                                    //oPayment.DocDate = Convert.ToDateTime(rendicion.Detalle.UCdate.ToString("yyyy-MM-dd"));
                                    oPayment.DocDate = DateTime.Now;
                                    oPayment.TaxDate = Convert.ToDateTime(rendicion.Detalle.UCdate.ToString("yyyy-MM-dd"));
                                    string monedaFR = SBO.ConsultasSBO.ObtenerCodigoMonedaISO(rendicion.Detalle.UCur);
                                    oPayment.LocalCurrency = SAPbobsCOM.BoYesNoEnum.tNO;
                                    oPayment.DocCurrency = monedaFR;
                                    oPayment.CashSum = rendicion.Detalle.UReptota;
                                    if (pago_con_cta_reembolso)
                                    {
                                        oPayment.CashAccount = CuentaContable(cta_pago_reembolsos);
                                        if (localizacion.Equals("SUATRANSPE"))
                                        {
                                            oPayment.CashAccount = CuentaContable(ctacontable_pago_fondo);
                                        }
                                    }
                                    foreach (var gasto in rendicion.Detalle.Gastos.Items)
                                    {
                                        ctacontable_fondo = gasto.Detalle.UCatcode;
                                        nombre_fondo = gasto.Detalle.UCategory;
                                        break;
                                    }
                                    oPayment.ControlAccount = CuentaContable(ctacontable_fondo);

                                    string numER = DateTime.Now.Ticks.ToString();
                                    if (localizacion.Equals("FLESAN1"))
                                    {
                                        //Tipo operación de pago
                                        oPayment.UserFields.Fields.Item("U_SYP_TPOOPER").Value = "99";
                                        //Numero ER
                                        oPayment.UserFields.Fields.Item("U_SYP_NUMER").Value = string.Format("ER{0}", numER);
                                        //Estado cajachica/ER
                                        oPayment.UserFields.Fields.Item("U_SYP_CCERSTATUS").Value = "O";
                                        //Serie ER
                                        oPayment.UserFields.Fields.Item("U_SYP_SERER").Value = numER;
                                    }
                                    if (registrar_campos_soindus)
                                    {
                                        //Numero ER (entrega a rendir)
                                        oPayment.UserFields.Fields.Item("U_SO_NUMER").Value = string.Format("ER{0}", numER);
                                    }
                                    if (localizacion.Equals("SUATRANS") || localizacion.Equals("SUATRANSTR"))
                                    {
                                        oPayment.ProjectCode = proyecto;
                                    }

                                    errCode = 0;
                                    errMsg = "";
                                    SBO.ConexionSBO.oCompany.StartTransaction();
                                    int ret = oPayment.Add();

                                    #endregion PAGO EFECTUADO

                                    if (ret != 0)
                                    {
                                        SBO.ConexionSBO.oCompany.GetLastError(out errCode, out errMsg);
                                        if (SBO.ConexionSBO.oCompany.InTransaction)
                                        {
                                            SBO.ConexionSBO.oCompany.EndTransaction(BoWfTransOpt.wf_RollBack);
                                        }
                                        Observaciones = string.Format("Pago Efectuado Solicitud: {0} - {1}", rendicion.Detalle.URepnum, errMsg);
                                        update = SBO.ModeloSBO.UpdateRendicion(rendicion.Detalle.DocEntry.ToString(), 9, Observaciones.PadRight(250).Substring(0, 250).Trim());
                                        algunError = true;
                                        break;
                                    }
                                    else
                                    {
                                        string pagoefectuado = SBO.ConexionSBO.oCompany.GetNewObjectKey();
                                        Observaciones += string.Format("Pago Efectuado: {0}", pagoefectuado);

                                        ///******CREAR FONDO A RENDIR EN RG*******///
                                        algunError = false;

                                        string IdEmployee = rendicion.Detalle.UEmpid.ToString();
                                        string IdAdmin = rendicion.Detalle.UAppid.ToString();
                                        string FundName = string.Format("{0} {1}", nombre_fondo, rendicion.Detalle.UEmpname);
                                        if (!localizacion.Equals("FLESAN1"))
                                        {
                                            FundName = string.Format("{0} {1} {2}", rendicion.Detalle.URepnum.ToString(), nombre_fondo, rendicion.Detalle.UEmpname);
                                        }
                                        string FundCurrency = rendicion.Detalle.UCur;
                                        string FundCode = string.Format("ER{0};{1};{2}", numER, ctacontable_fondo, rendicion.Detalle.UReptota.ToString());
                                        string FundAmount = rendicion.Detalle.UReptota.ToString();
                                        string FundComment = string.Format("ER{0};{1};{2}", numER, ctacontable_fondo, rendicion.Detalle.UReptota.ToString());
                                        string FundFlexibility = "false";
                                        string FundAutoDeposit = "false";
                                        string FundAutoBlock = "false";
                                        string FundExpiration = "true";
                                        string FundExpirationDate = DateTime.Now.AddMonths(1).ToString("yyyy-MM-dd");
                                        if (localizacion.Equals("FLESAN1"))
                                        {
                                            FundFlexibility = "true";
                                            if(nombre_fondo.Contains("Fondo Fijo"))
                                            {
                                                FundAutoDeposit = "true";
                                            }
                                            FundExpiration = "false";
                                            FundExpirationDate = string.Format("{0}-{1}-{2}", DateTime.Now.Year, "12", "31");
                                        }
                                        else if (localizacion.Equals("SOINDUS"))
                                        {
                                            FundFlexibility = "true";
                                            if (nombre_fondo.Contains("Fondo Fijo"))
                                            {
                                                FundAutoDeposit = "true";
                                            }
                                            FundExpiration = "false";
                                            FundExpirationDate = string.Format("{0}-{1}-{2}", DateTime.Now.Year, "12", "31");
                                        }
                                        else if (localizacion.Equals("ICLINICS"))
                                        {
                                            FundFlexibility = "true";
                                            if (nombre_fondo.Contains("Fondo Fijo"))
                                            {
                                                FundAutoDeposit = "true";
                                            }
                                            FundExpiration = "false";
                                            FundExpirationDate = string.Format("{0}-{1}-{2}", DateTime.Now.Year, "12", "31");
                                        }
                                        else if (localizacion.Equals("PCLINICS"))
                                        {
                                            FundFlexibility = "true";
                                            if (nombre_fondo.Contains("Fondo Fijo"))
                                            {
                                                FundAutoDeposit = "true";
                                            }
                                            FundExpiration = "false";
                                            FundExpirationDate = string.Format("{0}-{1}-{2}", DateTime.Now.Year, "12", "31");
                                        }
                                        else if (localizacion.Equals("LATINEQ"))
                                        {
                                            FundFlexibility = "true";
                                            if (nombre_fondo.Contains("Fondo Fijo"))
                                            {
                                                FundAutoDeposit = "true";
                                            }
                                            FundExpiration = "false";
                                            FundExpirationDate = string.Format("{0}-{1}-{2}", DateTime.Now.Year, "12", "31");
                                        }
                                        else if (localizacion.Equals("LATINEUY"))
                                        {
                                            FundFlexibility = "true";
                                            if (nombre_fondo.Contains("Fondo Fijo"))
                                            {
                                                FundAutoDeposit = "true";
                                            }
                                            FundExpiration = "false";
                                            FundExpirationDate = string.Format("{0}-{1}-{2}", DateTime.Now.Year, "12", "31");
                                        }
                                        else if (localizacion.Equals("SUATRANS") || localizacion.Equals("SUATRANSTR") || localizacion.Equals("SUATRANSPE"))
                                        {
                                            FundFlexibility = "true";
                                            if (nombre_fondo.Contains("Fondo Fijo"))
                                            {
                                                FundAutoDeposit = "true";
                                            }
                                            FundExpiration = "false";
                                            FundExpirationDate = string.Format("{0}-{1}-{2}", DateTime.Now.Year, "12", "31");
                                        }

                                        string[] _args = new string[]
                                        {
                                            IdEmployee, IdAdmin, FundName,
                                            FundCurrency, FundCode, FundAmount, FundComment,
                                            FundFlexibility, FundAutoDeposit, FundAutoBlock,
                                            FundExpiration, FundExpirationDate
                                        };

                                        var respF = interfazRG.CrearFondo(_args);
                                        if (respF.Success)
                                        {
                                            string fondo_rendir = string.Empty;
                                            var fondo = interfazRG.Fund;
                                            fondo_rendir = fondo.Code;
                                            if (string.IsNullOrEmpty(fondo_rendir))
                                            {
                                                if (SBO.ConexionSBO.oCompany.InTransaction)
                                                {
                                                    SBO.ConexionSBO.oCompany.EndTransaction(BoWfTransOpt.wf_RollBack);
                                                }
                                                Observaciones = string.Format("Solicitud {0} - No se pudo validar la creación del fondo en Rindegastos.", rendicion.Detalle.URepnum.ToString());
                                                update = SBO.ModeloSBO.UpdateRendicion(rendicion.Detalle.DocEntry.ToString(), 9, Observaciones.PadRight(250).Substring(0, 250).Trim());
                                                algunError = true;
                                                break;
                                            }
                                            else
                                            {
                                                algunError = false;
                                                Observaciones += string.Format(" - Fondo a rendir creado en Rindegastos");
                                            }
                                        }
                                        else
                                        {
                                            if (SBO.ConexionSBO.oCompany.InTransaction)
                                            {
                                                SBO.ConexionSBO.oCompany.EndTransaction(BoWfTransOpt.wf_RollBack);
                                            }
                                            Observaciones = string.Format("Solicitud: {0} - No se pudo crear el fondo en Rindegastos.", rendicion.Detalle.URepnum.ToString());
                                            update = SBO.ModeloSBO.UpdateRendicion(rendicion.Detalle.DocEntry.ToString(), 9, Observaciones.PadRight(250).Substring(0, 250).Trim());
                                            algunError = true;
                                            break;
                                        }
                                        ///******MARCAR SOLICITUD COMO INTEGRADA EN RG*******///
                                        algunError = false;

                                        string Id = rendicion.Detalle.UId.ToString();
                                        string IntegrationStatus = "1";
                                        string IntegrationCode = string.Format("PE-{0}", pagoefectuado);
                                        string IntegrationDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                                        string[] _args2 = new string[]
                                        {
                                            Id, IntegrationStatus, IntegrationCode, IntegrationDate
                                        };

                                        var respInt = interfazRG.CambiarEstadoRendicion(_args2);
                                        if (respInt.Success)
                                        {
                                            algunError = false;
                                            Observaciones += string.Format(" - Integrada en Rindegastos");
                                        }
                                        else
                                        {
                                            if (SBO.ConexionSBO.oCompany.InTransaction)
                                            {
                                                SBO.ConexionSBO.oCompany.EndTransaction(BoWfTransOpt.wf_RollBack);
                                            }
                                            Observaciones = string.Format("Solicitud: {0} - No se pudo cambiar el estado integrado en Rindegastos.", rendicion.Detalle.URepnum.ToString());
                                            update = SBO.ModeloSBO.UpdateRendicion(rendicion.Detalle.DocEntry.ToString(), 9, Observaciones.PadRight(250).Substring(0, 250).Trim());
                                            algunError = true;
                                            break;
                                        }
                                        ///******ESTADO PERSONALIZADO EN RG*******///
                                        algunError = false;

                                        string CustomStatus = "CONTABILIZADO";
                                        string CustomMessage = string.Format("Fondo a Rendir contabilizado en PE-{0}", pagoefectuado);

                                        string[] _args3 = new string[]
                                        {
                                            Id, IdAdmin, CustomStatus, CustomMessage
                                        };

                                        var respEP = interfazRG.CambiarEstadoPersonalizado(_args3);
                                        if (respEP.Success)
                                        {
                                            algunError = false;
                                            Observaciones += string.Format(" - Contabilizada en Rindegastos");
                                        }
                                        else
                                        {
                                            if (SBO.ConexionSBO.oCompany.InTransaction)
                                            {
                                                SBO.ConexionSBO.oCompany.EndTransaction(BoWfTransOpt.wf_RollBack);
                                            }
                                            Observaciones = string.Format("Solicitud: {0} - No se pudo cambiar el estado contabilizado en Rindegastos.", rendicion.Detalle.URepnum.ToString());
                                            update = SBO.ModeloSBO.UpdateRendicion(rendicion.Detalle.DocEntry.ToString(), 9, Observaciones.PadRight(250).Substring(0, 250).Trim());
                                            algunError = true;
                                            break;
                                        }
                                    }
                                }
                                break;
                            default:
                                break;
                        }
                        // Fin Switch Tipo de Gestión 
                    }
                    // Fin Validar Es Rendicion Sociedad
                    if (!algunError)
                    {
                        if (SBO.ConexionSBO.oCompany.InTransaction)
                        {
                            SBO.ConexionSBO.oCompany.EndTransaction(BoWfTransOpt.wf_Commit);
                        }
                        update = SBO.ModeloSBO.UpdateRendicion(rendicion.Detalle.DocEntry.ToString(), 1, Observaciones.PadRight(250).Substring(0, 250).Trim());
                    }
                }
                // Fin Foreach Rendiciones
            }
            catch (Exception ex)
            {
                if (SBO.ConexionSBO.oCompany.InTransaction)
                {
                    SBO.ConexionSBO.oCompany.EndTransaction(BoWfTransOpt.wf_RollBack);
                }
                resp.Success = false;
                resp.Mensaje = ex.Message;
                Observaciones = string.Format("Solicitud N°{0} - ERROR: {1} - Vuelva a procesar las solicitudes restantes.", gReportNum.ToString(), ex.Message);
                update = SBO.ModeloSBO.UpdateRendicion(gDocEntry.ToString(), 9, Observaciones.PadRight(250).Substring(0, 250).Trim());
            }
            return resp;
        }

        public static Clases.Message ContabilizarRendiciones(Clases.Rendiciones Rendiciones)
        {
            Clases.Configuracion ExtConf = new Clases.Configuracion();
            //Parametrizables
            string rut_sociedad = ExtConf.Parametros.Rut_Sociedad;
            string localizacion = ExtConf.Parametros.Localizacion;
            string cta_ant_proveedor = ExtConf.Parametros.Cuenta_Anticipo_Proveedores; //"1-1-100-20-000";
            string cta_serv_proveedor = ExtConf.Parametros.Cuenta_Servicios_Proveedores; //"6-1-010-40-000";
            string cta_compra_materiales = ExtConf.Parametros.Cuenta_Compra_Materiales; //"1-1-100-20-000";
            string cta_compensacion_facturas = ExtConf.Parametros.Cuenta_Compensacion_Facturas; //"1-1-100-20-000";
            string cta_pago_reembolsos = ExtConf.Parametros.Cuenta_Pago_Reembolsos; //"1-1-100-20-000";
            string campo_extra_centro_costo_nombre = ExtConf.Parametros.Campo_Centro_Costo; //"Centro de Costo";
            string campo_extra_centro_costo_valor = ExtConf.Parametros.Campo_Centro_Costo_Valor; //"CODE";
            string campo_extra_centro_costo_2_nombre = "Sucursal";
            string campo_extra_centro_costo_2_valor = "CODE";
            string campo_extra_centro_costo_dim4_nombre = "Dimensión - Fase";
            string campo_extra_centro_costo_dim4_valor = "VALUE";
            string campo_extra_centro_costo_dim5_nombre = "Dimensión - Subfase";
            string campo_extra_centro_costo_dim5_valor = "VALUE";
            string campo_extra_tipo_documento_nombre = ExtConf.Parametros.Campo_Tipo_Documento; //"Tipo de Documento";
            string campo_extra_tipo_documento_valor = ExtConf.Parametros.Campo_Tipo_Documento_Valor; //"CODE";
            string codigo_factura = ExtConf.Parametros.Codigo_Factura_Afecta; //"01";
            string codigo_factura_ex = ExtConf.Parametros.Codigo_Factura_Exenta; //"02";
            string codigo_factura_mat = ExtConf.Parametros.Codigo_Factura_Materiales; //"04";
            string codigo_factura_honorarios = "02"; //Válido para SuatransPE
            string campo_extra_rut_proveedor_nombre = ExtConf.Parametros.Campo_Rut_Proveedor; //"RUC Proveedor";
            string campo_extra_rut_proveedor_valor = ExtConf.Parametros.Campo_Rut_Proveedor_Valor; //"VALUE";
            string campo_extra_numero_documento_nombre = ExtConf.Parametros.Campo_Numero_Documento; //"Número de Documento";
            string campo_extra_numero_documento_valor = ExtConf.Parametros.Campo_Numero_Documento_Valor; //"VALUE";
            //Fin Parametrizables
            string cta_gasto = "6-1-020-10-000"; // Sólo Pruebas
            string tipo_gasto = "Gasto";
            string rut_proveedor = string.Empty;
            string centro_costo = string.Empty;
            string centro_costo_2 = string.Empty;
            string dimension = string.Empty;
            string centro_costo_dim1 = string.Empty;
            string centro_costo_dim3 = string.Empty;
            string centro_costo_dim4 = string.Empty;
            string centro_costo_dim5 = string.Empty;
            string numero_documento = string.Empty;
            string Observaciones = string.Empty;
            string nombre_fondo = string.Empty;
            string ctacontable_fondo = string.Empty;
            string fondo_rendir = string.Empty;
            double monto_tot_fondo = 0;
            double monto_saldo_fondo = 0;
            double monto_renov_fondo = 0;
            string tipo_fondo_pago = string.Empty;
            string accion_fondo_rendir = string.Empty;
            double monto_reembolso = 0;
            string asiento_integracion = string.Empty;
            string pago_rendicion = string.Empty;
            string forma_pago = string.Empty;
            string ctacontable_tarjetas = string.Empty;
            string nro_llamada = string.Empty;
            string cat1 = string.Empty;
            string cat2 = string.Empty;
            string cat3 = string.Empty;
            string cat4 = string.Empty;
            string cat5 = string.Empty;
            string ctactbgasto = string.Empty;
            string prioridad = string.Empty;
            string maquina_referencia = string.Empty;
            string sucursales = string.Empty;
            string tipo_rend = string.Empty;
            string proyecto = string.Empty;
            string codigo_sap = string.Empty;
            string detalle_factura = string.Empty;
            string ctacontable_asoc_sn = string.Empty;
            string codigoIVA22 = string.Empty;
            string codigoIVA10 = string.Empty;
            string codigoIVA0 = string.Empty;
            string codigoIVAPropina = string.Empty;
            double montoIVA22 = 0;
            double montoIVA10 = 0;
            double montoIVA0 = 0;
            double montoPropina = 0;

            //Logicos Localización
            bool borrador = false;
            bool borrador_1er_asiento = false;
            bool borrador_factura = false;
            bool borrador_compensa_factura_pagoef = false;
            bool borrador_compensa_factura_asiento = false;
            bool borrador_pago_a_favor = false;
            bool cuenta_en_gastos = false;
            bool registrar_campos_soindus = false;
            bool compensar_factura_con_pago = false;
            bool registrar_folio = false;
            bool registrar_indicador = false;
            bool pago_con_cta_reembolso = false;
            bool cerrar_fondo_compensado = false;
            switch (localizacion)
            {
                case "SOINDUS":
                    borrador = false;
                    borrador_1er_asiento = false;
                    borrador_factura = false;
                    borrador_compensa_factura_pagoef = false;
                    borrador_compensa_factura_asiento = false;
                    borrador_pago_a_favor = false;
                    cuenta_en_gastos = true;
                    registrar_campos_soindus = true;
                    compensar_factura_con_pago = true;
                    registrar_folio = true;
                    registrar_indicador = true;
                    pago_con_cta_reembolso = true;
                    cerrar_fondo_compensado = true;
                    break;
                case "FLESAN1":
                    borrador = true;
                    borrador_1er_asiento = true;
                    borrador_factura = true;
                    borrador_compensa_factura_pagoef = true;
                    borrador_compensa_factura_asiento = true;
                    borrador_pago_a_favor = true;
                    cuenta_en_gastos = true;
                    registrar_campos_soindus = false;
                    compensar_factura_con_pago = false;
                    registrar_folio = false;
                    registrar_indicador = false;
                    pago_con_cta_reembolso = false;
                    cerrar_fondo_compensado = false;
                    break;
                case "ICLINICS":
                    borrador = false;
                    borrador_1er_asiento = false;
                    borrador_factura = false;
                    borrador_compensa_factura_pagoef = false;
                    borrador_compensa_factura_asiento = false;
                    borrador_pago_a_favor = false;
                    cuenta_en_gastos = true;
                    registrar_campos_soindus = true;
                    compensar_factura_con_pago = true;
                    registrar_folio = true;
                    registrar_indicador = true;
                    pago_con_cta_reembolso = true;
                    cerrar_fondo_compensado = false;
                    break;
                case "PCLINICS":
                    borrador = false;
                    borrador_1er_asiento = false;
                    borrador_factura = false;
                    borrador_compensa_factura_pagoef = false;
                    borrador_compensa_factura_asiento = false;
                    borrador_pago_a_favor = false;
                    cuenta_en_gastos = true;
                    registrar_campos_soindus = true;
                    compensar_factura_con_pago = true;
                    registrar_folio = true;
                    registrar_indicador = true;
                    pago_con_cta_reembolso = true;
                    cerrar_fondo_compensado = false;
                    break;
                case "LATINEQ":
                    borrador = false;
                    borrador_1er_asiento = false;
                    borrador_factura = false;
                    borrador_compensa_factura_pagoef = false;
                    borrador_compensa_factura_asiento = false;
                    borrador_pago_a_favor = false;
                    cuenta_en_gastos = true;
                    registrar_campos_soindus = true;
                    compensar_factura_con_pago = true;
                    registrar_folio = true;
                    registrar_indicador = true;
                    pago_con_cta_reembolso = true;
                    cerrar_fondo_compensado = true;
                    break;
                case "LATINEUY":
                    borrador = false;
                    borrador_1er_asiento = false;
                    borrador_factura = false;
                    borrador_compensa_factura_pagoef = false;
                    borrador_compensa_factura_asiento = false;
                    borrador_pago_a_favor = false;
                    cuenta_en_gastos = true;
                    registrar_campos_soindus = true;
                    compensar_factura_con_pago = true;
                    registrar_folio = true;
                    registrar_indicador = true;
                    pago_con_cta_reembolso = true;
                    cerrar_fondo_compensado = true;
                    break;
                case "SUATRANS":
                    borrador = false;
                    borrador_1er_asiento = false;
                    borrador_factura = false;
                    borrador_compensa_factura_pagoef = false;
                    borrador_compensa_factura_asiento = false;
                    borrador_pago_a_favor = false;
                    cuenta_en_gastos = true;
                    registrar_campos_soindus = true;
                    compensar_factura_con_pago = true;
                    registrar_folio = true;
                    registrar_indicador = true;
                    pago_con_cta_reembolso = true;
                    cerrar_fondo_compensado = false;
                    break;
                case "SUATRANSTR":
                    borrador = false;
                    borrador_1er_asiento = false;
                    borrador_factura = false;
                    borrador_compensa_factura_pagoef = false;
                    borrador_compensa_factura_asiento = false;
                    borrador_pago_a_favor = false;
                    cuenta_en_gastos = true;
                    registrar_campos_soindus = true;
                    compensar_factura_con_pago = true;
                    registrar_folio = true;
                    registrar_indicador = true;
                    pago_con_cta_reembolso = true;
                    cerrar_fondo_compensado = false;
                    break;
                case "SUATRANSPE":
                    borrador = false;
                    borrador_1er_asiento = false;
                    borrador_factura = false;
                    borrador_compensa_factura_pagoef = false;
                    borrador_compensa_factura_asiento = false;
                    borrador_pago_a_favor = false;
                    cuenta_en_gastos = true;
                    registrar_campos_soindus = true;
                    compensar_factura_con_pago = true;
                    registrar_folio = true;
                    registrar_indicador = true;
                    pago_con_cta_reembolso = true;
                    cerrar_fondo_compensado = true;
                    break;
                default:
                    borrador = true;
                    borrador_1er_asiento = true;
                    borrador_factura = true;
                    borrador_compensa_factura_pagoef = true;
                    borrador_compensa_factura_asiento = true;
                    borrador_pago_a_favor = true;
                    cuenta_en_gastos = true;
                    registrar_campos_soindus = true;
                    compensar_factura_con_pago = true;
                    registrar_folio = true;
                    registrar_indicador = true;
                    pago_con_cta_reembolso = true;
                    cerrar_fondo_compensado = true;
                    break;
            }

            int errCode = 0;
            string errMsg = string.Empty;
            int ret = 0;
            int retDoc = 0;
            Clases.Message resp = new Clases.Message();
            Clases.Message update = new Clases.Message();
            string monedalocal = SBO.ConsultasSBO.ObtenerMonedaLocal();

            string memo = string.Empty;
            double totGastos = 0;
            int linea = 0;
            string _CardCodeRendidor = string.Empty;

            InterfazRG interfazRG = new InterfazRG();

            long gReportNum = 0;
            long gDocEntry = 0;
            try
            {
                foreach (var rendicion in Rendiciones.Items)
                {
                    gReportNum = rendicion.Detalle.URepnum;
                    gDocEntry = rendicion.Detalle.DocEntry;
                    bool algunError = false;
                    if (EsRendicionSociedad(rut_sociedad, rendicion) && rendicion.Detalle.UReptota > 0)
                    {
                        if (localizacion.Equals("ICLINICS"))
                        {
                            _CardCodeRendidor = SBO.ConsultasSBO.ObtenerCardCodeClienteConLetra(rendicion.Detalle.UEmpide, "R");
                        }
                        else if (localizacion.Equals("PCLINICS"))
                        {
                            _CardCodeRendidor = SBO.ConsultasSBO.ObtenerCardCodeClienteConLetra(rendicion.Detalle.UEmpide, "R");
                        }
                        else if (localizacion.Equals("LATINEQ"))
                        {
                            _CardCodeRendidor = SBO.ConsultasSBO.ObtenerCardCodeProveedor(rendicion.Detalle.UEmpide);
                        }
                        else if (localizacion.Equals("LATINEUY"))
                        {
                            _CardCodeRendidor = SBO.ConsultasSBO.ObtenerCardCodeProveedor(rendicion.Detalle.UEmpide);
                        }
                        else if (localizacion.Equals("SUATRANS"))
                        {
                            _CardCodeRendidor = SBO.ConsultasSBO.ObtenerCardCodeProveedor(rendicion.Detalle.UEmpide);
                        }
                        else if (localizacion.Equals("SUATRANSTR"))
                        {
                            _CardCodeRendidor = SBO.ConsultasSBO.ObtenerCardCodeCliente(rendicion.Detalle.UEmpide);
                        }
                        else if (localizacion.Equals("SUATRANSPE"))
                        {
                            _CardCodeRendidor = SBO.ConsultasSBO.ObtenerCardCodeProveedorConLetra(rendicion.Detalle.UEmpide, "E");
                        }
                        else
                        {
                            _CardCodeRendidor = SBO.ConsultasSBO.ObtenerCardCode(rendicion.Detalle.UEmpide);
                        }
                        // Validar Existencia del Detalle de Gastos
                        if (rendicion.Detalle.Gastos == null || rendicion.Detalle.Gastos.Items.Count() <= 0)
                        {
                            if (SBO.ConexionSBO.oCompany.InTransaction)
                            {
                                SBO.ConexionSBO.oCompany.EndTransaction(BoWfTransOpt.wf_RollBack);
                            }
                            Observaciones = string.Format("Rendición: {0} - No existe detalle de gastos. Realice proceso manual.", rendicion.Detalle.URepnum);
                            update = SBO.ModeloSBO.UpdateRendicion(rendicion.Detalle.DocEntry.ToString(), 9, Observaciones.PadRight(250).Substring(0, 250).Trim());
                            algunError = true;
                            continue;
                        }
                        // Validar SN Empleado Rendidor
                        if (string.IsNullOrEmpty(_CardCodeRendidor))
                        {
                            if (SBO.ConexionSBO.oCompany.InTransaction)
                            {
                                SBO.ConexionSBO.oCompany.EndTransaction(BoWfTransOpt.wf_RollBack);
                            }
                            Observaciones = string.Format("Asiento Rendición: {0} - No existe socio de negocios {1}.", rendicion.Detalle.URepnum, rendicion.Detalle.UEmpide);
                            update = SBO.ModeloSBO.UpdateRendicion(rendicion.Detalle.DocEntry.ToString(), 9, Observaciones.PadRight(250).Substring(0, 250).Trim());
                            algunError = true;
                            continue;
                        }
                        else
                        {
                            switch (TipoGestion(rendicion))
                            {
                                case "RENDICIÓN":
                                    Observaciones = string.Empty;
                                    //VALIDACION FONDO A RENDIR
                                    fondo_rendir = string.Empty;
                                    monto_tot_fondo = 0;
                                    monto_saldo_fondo = 0;
                                    monto_renov_fondo = 0;
                                    ctacontable_fondo = string.Empty;
                                    tipo_fondo_pago = string.Empty;
                                    monto_reembolso = 0;
                                    asiento_integracion = string.Empty;
                                    pago_rendicion = string.Empty;
                                    forma_pago = string.Empty;
                                    ctacontable_tarjetas = string.Empty;
                                    ctacontable_tarjetas = CuentaTarjetaCredito(rendicion);
                                    if (!string.IsNullOrEmpty(ctacontable_tarjetas))
                                    {
                                        forma_pago = "TARJETACREDITO";
                                    }
                                    if (!rendicion.Detalle.UFundid.ToString().Equals("0"))
                                    {
                                        tipo_fondo_pago = "FONDORENDIR";
                                        accion_fondo_rendir = "MANTENER";
                                        string[] _args = new string[] { rendicion.Detalle.UFundid.ToString() };
                                        var respF = interfazRG.ObtenerFondo(_args);
                                        if (respF.Success)
                                        {
                                            var fondo = interfazRG.Fund;
                                            try
                                            {
                                                var fondocode = fondo.Code.Split(';');
                                                fondo_rendir = fondocode[0];
                                                ctacontable_fondo = fondocode[1];
                                                monto_tot_fondo = double.Parse(fondocode[2]);
                                            }
                                            catch
                                            {
                                            }
                                            nombre_fondo = fondo.Title;
                                            monto_renov_fondo = monto_tot_fondo;
                                            monto_saldo_fondo = fondo.Balance;
                                            if (string.IsNullOrEmpty(fondo_rendir))
                                            {
                                                if (SBO.ConexionSBO.oCompany.InTransaction)
                                                {
                                                    SBO.ConexionSBO.oCompany.EndTransaction(BoWfTransOpt.wf_RollBack);
                                                }
                                                Observaciones = string.Format("Rendición N°{0} - No posee un Fondo a Rendir válido.", rendicion.Detalle.URepnum.ToString());
                                                update = SBO.ModeloSBO.UpdateRendicion(rendicion.Detalle.DocEntry.ToString(), 9, Observaciones.PadRight(250).Substring(0, 250).Trim());
                                                algunError = true;
                                                break;
                                            }
                                            if (rendicion.Detalle.UReptota < fondo.Charges && monto_saldo_fondo < 0)
                                            {
                                                if (rendicion.Detalle.UReptota <= (monto_saldo_fondo * -1))
                                                {
                                                    monto_reembolso = rendicion.Detalle.UReptota;
                                                    tipo_fondo_pago = "EXCESO";
                                                }
                                                else
                                                {
                                                    monto_reembolso = (monto_saldo_fondo * -1);
                                                    tipo_fondo_pago = "EXCESO";
                                                }
                                            }
                                            else if (rendicion.Detalle.UReptota == fondo.Charges && monto_saldo_fondo < 0)
                                            {
                                                monto_reembolso = (monto_saldo_fondo * -1);
                                                tipo_fondo_pago = "EXCESO";
                                            }
                                            else if (monto_tot_fondo == fondo.Deposits && rendicion.Detalle.UReptota > monto_tot_fondo)
                                            {
                                                monto_reembolso = rendicion.Detalle.UReptota - monto_tot_fondo;
                                                tipo_fondo_pago = "EXCESO";
                                            }
                                            else
                                            {
                                                monto_renov_fondo = rendicion.Detalle.UReptota;
                                            }
                                            if (monto_saldo_fondo.Equals(0))
                                            {
                                                accion_fondo_rendir = "CERRAR";
                                            }
                                        }
                                    }
                                    else
                                    {
                                        monto_reembolso = rendicion.Detalle.UReptota;
                                        tipo_fondo_pago = "REEMBOLSO";
                                    }

                                    if (localizacion.Equals("LATINEQ") || localizacion.Equals("LATINEUY"))
                                    {
                                        nro_llamada = string.Empty;
                                        cat1 = string.Empty;
                                        cat2 = string.Empty;
                                        cat3 = string.Empty;
                                        prioridad = string.Empty;
                                        maquina_referencia = string.Empty;
                                        sucursales = string.Empty;
                                        tipo_rend = string.Empty;
                                        foreach (var extrarend in rendicion.Detalle.CamposExtra.Detalle)
                                        {
                                            if (extrarend.UName.Equals("N° Llamada de Servicio"))
                                            {
                                                nro_llamada = extrarend.UValue;
                                            }
                                            if (extrarend.UName.Equals("Tipo de Llamada"))
                                            {
                                                cat1 = extrarend.UValue;
                                            }
                                            if (extrarend.UName.Equals("Línea"))
                                            {
                                                cat2 = extrarend.UValue;
                                            }
                                            if (extrarend.UName.Equals("Cat3"))
                                            {
                                                cat3 = extrarend.UValue;
                                            }
                                            if (extrarend.UName.Equals("U_SEI_PRIORIDAD"))
                                            {
                                                prioridad = extrarend.UValue;
                                            }
                                            if (extrarend.UName.Equals("U_SEI_SUCURSALES"))
                                            {
                                                sucursales = extrarend.UValue;
                                            }
                                            if (extrarend.UName.Equals("U_SEI_MAQ_REF"))
                                            {
                                                maquina_referencia = extrarend.UValue;
                                            }
                                            if (extrarend.UName.Equals("U_SEI_TIPR"))
                                            {
                                                tipo_rend = extrarend.UValue;
                                            }
                                        }
                                    }
                                    if (localizacion.Equals("SUATRANS") || localizacion.Equals("SUATRANSTR"))
                                    {
                                        proyecto = string.Empty;
                                        foreach (var extrarend in rendicion.Detalle.CamposExtra.Detalle)
                                        {
                                            if (extrarend.UName.Equals("Proyecto"))
                                            {
                                                proyecto = extrarend.UValue;
                                            }
                                        }
                                    }
                                    if (localizacion.Equals("SUATRANSPE"))
                                    {
                                        ctacontable_asoc_sn = string.Empty;
                                        foreach (var extrarend in rendicion.Detalle.CamposExtra.Detalle)
                                        {
                                            if (extrarend.UName.Equals("Cuenta SN Rendición"))
                                            {
                                                ctacontable_asoc_sn = extrarend.UCode;
                                            }
                                        }
                                    }

                                    //***1ER ASIENTO CONTABLE***//
                                    #region 1ER ASIENTO CONTABLE
                                    if (borrador_1er_asiento)
                                    {
                                        //preliminar
                                        SAPbobsCOM.JournalVouchers oVoucher = (SAPbobsCOM.JournalVouchers)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oJournalVouchers);
                                        //fecha documento
                                        oVoucher.JournalEntries.ReferenceDate = DateTime.Now;
                                        oVoucher.JournalEntries.TaxDate = Convert.ToDateTime(rendicion.Detalle.UCdate.ToString("yyyy-MM-dd"));
                                        oVoucher.JournalEntries.DueDate = Convert.ToDateTime(rendicion.Detalle.UCdate.ToString("yyyy-MM-dd"));
                                        oVoucher.JournalEntries.Reference2 = rendicion.Detalle.URepnum.ToString();
                                        oVoucher.JournalEntries.Reference3 = fondo_rendir;
                                        memo = string.Empty;
                                        //memo = rendicion.Detalle.UTitle;
                                        memo = "Rendición N°" + rendicion.Detalle.URepnum.ToString() + " " + rendicion.Detalle.UEmpname;
                                        memo = memo.PadRight(50).Substring(0, 50).Trim();
                                        oVoucher.JournalEntries.Memo = memo;
                                        if (localizacion.Equals("LATINEQ") || localizacion.Equals("LATINEUY"))
                                        {
                                            oVoucher.JournalEntries.ProjectCode = nro_llamada;
                                        }
                                        totGastos = 0;
                                        linea = 0;
                                        oVoucher.JournalEntries.SetCurrentLine(linea);
                                        foreach (var gasto in rendicion.Detalle.Gastos.Items)
                                        {
                                            if (gasto.Detalle.UStatus.Equals(1))
                                            {
                                                if (linea > 0)
                                                {
                                                    oVoucher.JournalEntries.Lines.Add();
                                                }
                                                tipo_gasto = "Gasto";
                                                rut_proveedor = "";
                                                centro_costo = "";
                                                centro_costo_2 = "";
                                                centro_costo_dim4 = "";
                                                centro_costo_dim5 = "";

                                                foreach (var extras in gasto.Detalle.CamposExtra.Detalle)
                                                {
                                                    if (extras.UName.Equals(campo_extra_tipo_documento_nombre) && campo_extra_tipo_documento_valor.Equals("VALUE"))
                                                    {
                                                        if (extras.UValue.Equals(codigo_factura))
                                                        {
                                                            tipo_gasto = "Factura";
                                                        }
                                                        if (extras.UValue.Equals(codigo_factura_ex))
                                                        {
                                                            tipo_gasto = "FacturaEx";
                                                        }
                                                        if (extras.UValue.Equals(codigo_factura_mat))
                                                        {
                                                            tipo_gasto = "FacturaMat";
                                                        }
                                                        if (extras.UValue.Equals(codigo_factura_honorarios) && localizacion.Equals("SUATRANSPE"))
                                                        {
                                                            tipo_gasto = "FacturaHR";
                                                        }
                                                    }
                                                    if (extras.UName.Equals(campo_extra_tipo_documento_nombre) && campo_extra_tipo_documento_valor.Equals("CODE"))
                                                    {
                                                        if (extras.UCode.Equals(codigo_factura))
                                                        {
                                                            tipo_gasto = "Factura";
                                                        }
                                                        if (extras.UCode.Equals(codigo_factura_ex))
                                                        {
                                                            tipo_gasto = "FacturaEx";
                                                        }
                                                        if (extras.UCode.Equals(codigo_factura_mat))
                                                        {
                                                            tipo_gasto = "FacturaMat";
                                                        }
                                                        if (extras.UCode.Equals(codigo_factura_honorarios) && localizacion.Equals("SUATRANSPE"))
                                                        {
                                                            tipo_gasto = "FacturaHR";
                                                        }
                                                    }
                                                    if (extras.UName.Equals(campo_extra_rut_proveedor_nombre) && campo_extra_rut_proveedor_valor.Equals("VALUE"))
                                                    {
                                                        rut_proveedor = extras.UValue;
                                                    }
                                                    if (extras.UName.Equals(campo_extra_rut_proveedor_nombre) && campo_extra_rut_proveedor_valor.Equals("CODE"))
                                                    {
                                                        rut_proveedor = extras.UCode;
                                                    }
                                                    if (extras.UName.Equals(campo_extra_centro_costo_nombre) && campo_extra_centro_costo_valor.Equals("VALUE"))
                                                    {
                                                        centro_costo = extras.UValue;
                                                    }
                                                    if (extras.UName.Equals(campo_extra_centro_costo_nombre) && campo_extra_centro_costo_valor.Equals("CODE"))
                                                    {
                                                        centro_costo = extras.UCode;
                                                    }
                                                    if (extras.UName.Equals(campo_extra_centro_costo_2_nombre) && campo_extra_centro_costo_2_valor.Equals("VALUE"))
                                                    {
                                                        centro_costo_2 = extras.UValue;
                                                    }
                                                    if (extras.UName.Equals(campo_extra_centro_costo_2_nombre) && campo_extra_centro_costo_2_valor.Equals("CODE"))
                                                    {
                                                        centro_costo_2 = extras.UCode;
                                                    }
                                                    if (extras.UName.Equals(campo_extra_centro_costo_dim4_nombre) && campo_extra_centro_costo_dim4_valor.Equals("VALUE"))
                                                    {
                                                        centro_costo_dim4 = extras.UValue;
                                                    }
                                                    if (extras.UName.Equals(campo_extra_centro_costo_dim4_nombre) && campo_extra_centro_costo_dim4_valor.Equals("CODE"))
                                                    {
                                                        centro_costo_dim4 = extras.UCode;
                                                    }
                                                    if (extras.UName.Equals(campo_extra_centro_costo_dim5_nombre) && campo_extra_centro_costo_dim5_valor.Equals("VALUE"))
                                                    {
                                                        centro_costo_dim5 = extras.UValue;
                                                    }
                                                    if (extras.UName.Equals(campo_extra_centro_costo_dim5_nombre) && campo_extra_centro_costo_dim5_valor.Equals("CODE"))
                                                    {
                                                        centro_costo_dim5 = extras.UCode;
                                                    }
                                                }
                                                dimension = SBO.ConsultasSBO.ObtenerDimensionCC(centro_costo);
                                                if (tipo_gasto.Equals("Factura") || tipo_gasto.Equals("FacturaEx") || tipo_gasto.Equals("FacturaMat") || tipo_gasto.Equals("FacturaHR"))
                                                {
                                                    oVoucher.JournalEntries.Lines.AccountCode = CuentaContable(cta_ant_proveedor);
                                                    switch (dimension)
                                                    {
                                                        case "1":
                                                            oVoucher.JournalEntries.Lines.CostingCode = centro_costo;
                                                            break;
                                                        case "2":
                                                            if (localizacion.Equals("FLESAN1"))
                                                            {
                                                                centro_costo_dim1 = SBO.ConsultasSBO.ObtenerCCDimension1(centro_costo);
                                                                oVoucher.JournalEntries.Lines.CostingCode = centro_costo_dim1;
                                                                centro_costo_dim3 = SBO.ConsultasSBO.ObtenerCCDimension3(cta_ant_proveedor);
                                                                if (!string.IsNullOrEmpty(centro_costo_dim3))
                                                                {
                                                                    oVoucher.JournalEntries.Lines.CostingCode3 = centro_costo_dim3;
                                                                    //oVoucher.JournalEntries.Lines.CostingCode4 = "2000";
                                                                    //oVoucher.JournalEntries.Lines.CostingCode5 = "2007";
                                                                    oVoucher.JournalEntries.Lines.CostingCode4 = centro_costo_dim4;
                                                                    oVoucher.JournalEntries.Lines.CostingCode5 = centro_costo_dim5;
                                                                }
                                                                oVoucher.JournalEntries.Lines.ProjectCode = centro_costo;
                                                                oVoucher.JournalEntries.ProjectCode = centro_costo;
                                                            }
                                                            oVoucher.JournalEntries.Lines.CostingCode2 = centro_costo;
                                                            break;
                                                        case "3":
                                                            oVoucher.JournalEntries.Lines.CostingCode3 = centro_costo;
                                                            break;
                                                        case "4":
                                                            oVoucher.JournalEntries.Lines.CostingCode4 = centro_costo;
                                                            break;
                                                        case "5":
                                                            oVoucher.JournalEntries.Lines.CostingCode5 = centro_costo;
                                                            break;
                                                        default:
                                                            oVoucher.JournalEntries.Lines.CostingCode = centro_costo;
                                                            break;
                                                    }
                                                    if (localizacion.Equals("LATINEQ") || localizacion.Equals("LATINEUY"))
                                                    {
                                                        oVoucher.JournalEntries.Lines.CostingCode = centro_costo;
                                                        oVoucher.JournalEntries.Lines.CostingCode2 = centro_costo_2;
                                                        oVoucher.JournalEntries.Lines.ProjectCode = nro_llamada;
                                                    }
                                                    memo = gasto.Detalle.UIdate.ToString("dd-MM-yyyy") + " / " + gasto.Detalle.UCategory + " / Factura / " + rut_proveedor;
                                                    if (localizacion.Equals("LATINEQ") || localizacion.Equals("LATINEUY"))
                                                    {
                                                        memo = "RG" + rendicion.Detalle.URepnum.ToString() + "_" + gasto.Detalle.UIdate.ToString("ddMM") + "/" + gasto.Detalle.UCategory + "/Factura/" + rut_proveedor;
                                                    }
                                                    memo = memo.PadRight(100).Substring(0, 100).Trim();
                                                    oVoucher.JournalEntries.Lines.Reference2 = memo;
                                                    memo = memo.PadRight(50).Substring(0, 50).Trim();
                                                    oVoucher.JournalEntries.Lines.LineMemo = memo;
                                                }
                                                else
                                                {
                                                    //oVoucher.JournalEntries.Lines.AccountCode = cta_gasto;
                                                    oVoucher.JournalEntries.Lines.AccountCode = CuentaContable(gasto.Detalle.UCatcode);
                                                    if (localizacion.Equals("LATINEQ") || localizacion.Equals("LATINEUY"))
                                                    {
                                                        cat4 = string.Empty;
                                                        cat5 = string.Empty;
                                                        ctactbgasto = string.Empty;
                                                        foreach (var extragasto in gasto.Detalle.CamposExtra.Detalle)
                                                        {
                                                            if (extragasto.UName.Equals("Cat4"))
                                                            {
                                                                cat4 = extragasto.UValue;
                                                            }
                                                            if (extragasto.UName.Equals("Cat5"))
                                                            {
                                                                cat5 = extragasto.UValue;
                                                            }
                                                        }
                                                        cat4 = gasto.Detalle.UCategory;
                                                        ctactbgasto = SBO.ConsultasSBO.ObtenerCtaCtbSegunCategorias(cat1, cat2, cat3, cat4, cat5);
                                                        oVoucher.JournalEntries.Lines.AccountCode = ctactbgasto;
                                                    }
                                                    switch (dimension)
                                                    {
                                                        case "1":
                                                            oVoucher.JournalEntries.Lines.CostingCode = centro_costo;
                                                            break;
                                                        case "2":
                                                            if (localizacion.Equals("FLESAN1"))
                                                            {
                                                                centro_costo_dim1 = SBO.ConsultasSBO.ObtenerCCDimension1(centro_costo);
                                                                oVoucher.JournalEntries.Lines.CostingCode = centro_costo_dim1;
                                                                centro_costo_dim3 = SBO.ConsultasSBO.ObtenerCCDimension3(gasto.Detalle.UCatcode);
                                                                if (!string.IsNullOrEmpty(centro_costo_dim3))
                                                                {
                                                                    oVoucher.JournalEntries.Lines.CostingCode3 = centro_costo_dim3;
                                                                    //oVoucher.JournalEntries.Lines.CostingCode4 = "2000";
                                                                    //oVoucher.JournalEntries.Lines.CostingCode5 = "2007";
                                                                    oVoucher.JournalEntries.Lines.CostingCode4 = centro_costo_dim4;
                                                                    oVoucher.JournalEntries.Lines.CostingCode5 = centro_costo_dim5;
                                                                }
                                                                oVoucher.JournalEntries.Lines.ProjectCode = centro_costo;
                                                                oVoucher.JournalEntries.ProjectCode = centro_costo;
                                                            }
                                                            oVoucher.JournalEntries.Lines.CostingCode2 = centro_costo;
                                                            break;
                                                        case "3":
                                                            oVoucher.JournalEntries.Lines.CostingCode3 = centro_costo;
                                                            break;
                                                        case "4":
                                                            oVoucher.JournalEntries.Lines.CostingCode4 = centro_costo;
                                                            break;
                                                        case "5":
                                                            oVoucher.JournalEntries.Lines.CostingCode5 = centro_costo;
                                                            break;
                                                        default:
                                                            oVoucher.JournalEntries.Lines.CostingCode = centro_costo;
                                                            break;
                                                    }
                                                    if (localizacion.Equals("LATINEQ") || localizacion.Equals("LATINEUY"))
                                                    {
                                                        oVoucher.JournalEntries.Lines.CostingCode = centro_costo;
                                                        oVoucher.JournalEntries.Lines.CostingCode2 = centro_costo_2;
                                                        oVoucher.JournalEntries.Lines.ProjectCode = nro_llamada;
                                                    }
                                                    memo = gasto.Detalle.UIdate.ToString("dd-MM-yyyy") + " / " + gasto.Detalle.UCategory + " / Boleta";
                                                    if (localizacion.Equals("LATINEQ") || localizacion.Equals("LATINEUY"))
                                                    {
                                                        memo = "RG" + rendicion.Detalle.URepnum.ToString() + "_" + gasto.Detalle.UIdate.ToString("ddMM") + "/" + gasto.Detalle.UCategory + "/Boleta";
                                                    }
                                                    memo = memo.PadRight(100).Substring(0, 100).Trim();
                                                    oVoucher.JournalEntries.Lines.Reference2 = memo;
                                                    memo = memo.PadRight(50).Substring(0, 50).Trim();
                                                    oVoucher.JournalEntries.Lines.LineMemo = memo;
                                                }
                                                string monedaGasto = SBO.ConsultasSBO.ObtenerCodigoMonedaISO(gasto.Detalle.UOcur);
                                                if (monedalocal.Equals(monedaGasto))
                                                {
                                                    oVoucher.JournalEntries.Lines.Debit = gasto.Detalle.UTotal;
                                                }
                                                else
                                                {
                                                    oVoucher.JournalEntries.Lines.FCCurrency = monedaGasto;
                                                    oVoucher.JournalEntries.Lines.FCDebit = gasto.Detalle.UTotal;
                                                }
                                                totGastos += gasto.Detalle.UTotal;
                                                linea++;
                                            }
                                        }

                                        if (linea > 0)
                                        {
                                            oVoucher.JournalEntries.Lines.Add();
                                        }
                                        //////oVoucher.JournalEntries.SetCurrentLine(linea);
                                        //oVoucher.JournalEntries.Lines.ShortName = rendicion.Detalle.UEmpide;
                                        if (forma_pago.Equals("TARJETACREDITO"))
                                        {
                                            oVoucher.JournalEntries.Lines.AccountCode = CuentaContable(ctacontable_tarjetas);
                                        }
                                        else
                                        {
                                            oVoucher.JournalEntries.Lines.ShortName = _CardCodeRendidor;
                                            if (localizacion.Equals("SUATRANSPE"))
                                            {
                                                oVoucher.JournalEntries.Lines.ControlAccount = CuentaContable(ctacontable_asoc_sn);
                                            }
                                        }
                                        string monedaRendicion = SBO.ConsultasSBO.ObtenerCodigoMonedaISO(rendicion.Detalle.UCur);
                                        if (monedalocal.Equals(monedaRendicion))
                                        {
                                            oVoucher.JournalEntries.Lines.Credit = totGastos;
                                        }
                                        else
                                        {
                                            oVoucher.JournalEntries.Lines.FCCurrency = monedaRendicion;
                                            oVoucher.JournalEntries.Lines.FCCredit = totGastos;
                                        }
                                        if (forma_pago.Equals("TARJETACREDITO"))
                                        {
                                            memo = "Rendidor " + rendicion.Detalle.UEmpname;
                                        }
                                        else
                                        {
                                            memo = "A pagar a rendidor " + rendicion.Detalle.UEmpname;
                                            if (localizacion.Equals("LATINEQ") || localizacion.Equals("LATINEUY"))
                                            {
                                                memo = "Rendición N°" + rendicion.Detalle.URepnum.ToString() + " " + rendicion.Detalle.UEmpname;
                                            }
                                        }
                                        memo = memo.PadRight(50).Substring(0, 50).Trim();
                                        oVoucher.JournalEntries.Lines.LineMemo = memo;
                                        if (localizacion.Equals("FLESAN1"))
                                        {
                                            oVoucher.JournalEntries.Lines.ProjectCode = centro_costo;
                                        }
                                        if (localizacion.Equals("LATINEQ") || localizacion.Equals("LATINEUY"))
                                        {
                                            oVoucher.JournalEntries.Lines.ProjectCode = nro_llamada;
                                        }

                                        errCode = 0;
                                        errMsg = "";
                                        SBO.ConexionSBO.oCompany.StartTransaction();
                                        ret = oVoucher.Add();
                                    }
                                    else
                                    {
                                        //final
                                        SAPbobsCOM.JournalEntries oVoucher = (SAPbobsCOM.JournalEntries)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oJournalEntries);
                                        //fecha documento
                                        oVoucher.ReferenceDate = DateTime.Now;
                                        oVoucher.TaxDate = Convert.ToDateTime(rendicion.Detalle.UCdate.ToString("yyyy-MM-dd"));
                                        oVoucher.DueDate = Convert.ToDateTime(rendicion.Detalle.UCdate.ToString("yyyy-MM-dd"));
                                        oVoucher.Reference2 = rendicion.Detalle.URepnum.ToString();
                                        oVoucher.Reference3 = fondo_rendir;
                                        memo = string.Empty;
                                        //memo = rendicion.Detalle.UTitle;
                                        memo = "Rendición N°" + rendicion.Detalle.URepnum.ToString() + " " + rendicion.Detalle.UEmpname;
                                        memo = memo.PadRight(50).Substring(0, 50).Trim();
                                        oVoucher.Memo = memo;
                                        if (localizacion.Equals("LATINEQ") || localizacion.Equals("LATINEUY"))
                                        {
                                            oVoucher.ProjectCode = nro_llamada;
                                        }
                                        totGastos = 0;
                                        linea = 0;
                                        oVoucher.SetCurrentLine(linea);
                                        foreach (var gasto in rendicion.Detalle.Gastos.Items)
                                        {
                                            if (gasto.Detalle.UStatus.Equals(1))
                                            {
                                                if (linea > 0)
                                                {
                                                    oVoucher.Lines.Add();
                                                }
                                                tipo_gasto = "Gasto";
                                                rut_proveedor = "";
                                                centro_costo = "";
                                                centro_costo_2 = "";

                                                foreach (var extras in gasto.Detalle.CamposExtra.Detalle)
                                                {
                                                    if (extras.UName.Equals(campo_extra_tipo_documento_nombre) && campo_extra_tipo_documento_valor.Equals("VALUE"))
                                                    {
                                                        if (extras.UValue.Equals(codigo_factura))
                                                        {
                                                            tipo_gasto = "Factura";
                                                        }
                                                        if (extras.UValue.Equals(codigo_factura_ex))
                                                        {
                                                            tipo_gasto = "FacturaEx";
                                                        }
                                                        if (extras.UValue.Equals(codigo_factura_mat))
                                                        {
                                                            tipo_gasto = "FacturaMat";
                                                        }
                                                        if (extras.UValue.Equals(codigo_factura_honorarios) && localizacion.Equals("SUATRANSPE"))
                                                        {
                                                            tipo_gasto = "FacturaHR";
                                                        }
                                                    }
                                                    if (extras.UName.Equals(campo_extra_tipo_documento_nombre) && campo_extra_tipo_documento_valor.Equals("CODE"))
                                                    {
                                                        if (extras.UCode.Equals(codigo_factura))
                                                        {
                                                            tipo_gasto = "Factura";
                                                        }
                                                        if (extras.UCode.Equals(codigo_factura_ex))
                                                        {
                                                            tipo_gasto = "FacturaEx";
                                                        }
                                                        if (extras.UCode.Equals(codigo_factura_mat))
                                                        {
                                                            tipo_gasto = "FacturaMat";
                                                        }
                                                        if (extras.UCode.Equals(codigo_factura_honorarios) && localizacion.Equals("SUATRANSPE"))
                                                        {
                                                            tipo_gasto = "FacturaHR";
                                                        }
                                                    }
                                                    if (extras.UName.Equals(campo_extra_rut_proveedor_nombre) && campo_extra_rut_proveedor_valor.Equals("VALUE"))
                                                    {
                                                        rut_proveedor = extras.UValue;
                                                    }
                                                    if (extras.UName.Equals(campo_extra_rut_proveedor_nombre) && campo_extra_rut_proveedor_valor.Equals("CODE"))
                                                    {
                                                        rut_proveedor = extras.UCode;
                                                    }
                                                    if (extras.UName.Equals(campo_extra_centro_costo_nombre) && campo_extra_centro_costo_valor.Equals("VALUE"))
                                                    {
                                                        centro_costo = extras.UValue;
                                                    }
                                                    if (extras.UName.Equals(campo_extra_centro_costo_nombre) && campo_extra_centro_costo_valor.Equals("CODE"))
                                                    {
                                                        centro_costo = extras.UCode;
                                                    }
                                                    if (extras.UName.Equals(campo_extra_centro_costo_2_nombre) && campo_extra_centro_costo_2_valor.Equals("VALUE"))
                                                    {
                                                        centro_costo_2 = extras.UValue;
                                                    }
                                                    if (extras.UName.Equals(campo_extra_centro_costo_2_nombre) && campo_extra_centro_costo_2_valor.Equals("CODE"))
                                                    {
                                                        centro_costo_2 = extras.UCode;
                                                    }
                                                }
                                                dimension = SBO.ConsultasSBO.ObtenerDimensionCC(centro_costo);
                                                if (tipo_gasto.Equals("Factura") || tipo_gasto.Equals("FacturaEx") || tipo_gasto.Equals("FacturaMat") || tipo_gasto.Equals("FacturaHR"))
                                                {
                                                    oVoucher.Lines.AccountCode = CuentaContable(cta_ant_proveedor);
                                                    switch (dimension)
                                                    {
                                                        case "1":
                                                            oVoucher.Lines.CostingCode = centro_costo;
                                                            break;
                                                        case "2":
                                                            if (localizacion.Equals("FLESAN1"))
                                                            {
                                                                centro_costo_dim1 = SBO.ConsultasSBO.ObtenerCCDimension1(centro_costo);
                                                                oVoucher.Lines.CostingCode = centro_costo_dim1;
                                                                centro_costo_dim3 = SBO.ConsultasSBO.ObtenerCCDimension3(cta_ant_proveedor);
                                                                if (!string.IsNullOrEmpty(centro_costo_dim3))
                                                                {
                                                                    oVoucher.Lines.CostingCode3 = centro_costo_dim3;
                                                                    oVoucher.Lines.CostingCode4 = "2000";
                                                                    oVoucher.Lines.CostingCode5 = "2007";
                                                                }
                                                                oVoucher.Lines.ProjectCode = centro_costo;
                                                                oVoucher.ProjectCode = centro_costo;
                                                            }
                                                            oVoucher.Lines.CostingCode2 = centro_costo;
                                                            break;
                                                        case "3":
                                                            oVoucher.Lines.CostingCode3 = centro_costo;
                                                            break;
                                                        case "4":
                                                            oVoucher.Lines.CostingCode4 = centro_costo;
                                                            break;
                                                        case "5":
                                                            oVoucher.Lines.CostingCode5 = centro_costo;
                                                            break;
                                                        default:
                                                            oVoucher.Lines.CostingCode = centro_costo;
                                                            break;
                                                    }
                                                    if (localizacion.Equals("LATINEQ") || localizacion.Equals("LATINEUY"))
                                                    {
                                                        oVoucher.Lines.CostingCode = centro_costo;
                                                        oVoucher.Lines.CostingCode2 = centro_costo_2;
                                                        oVoucher.Lines.ProjectCode = nro_llamada;
                                                    }
                                                    memo = gasto.Detalle.UIdate.ToString("dd-MM-yyyy") + " / " + gasto.Detalle.UCategory + " / Factura / " + rut_proveedor;
                                                    if (localizacion.Equals("LATINEQ") || localizacion.Equals("LATINEUY"))
                                                    {
                                                        memo = "RG" + rendicion.Detalle.URepnum.ToString() + "_" + gasto.Detalle.UIdate.ToString("ddMM") + "/" + gasto.Detalle.UCategory + "/Factura/" + rut_proveedor;
                                                    }
                                                    memo = memo.PadRight(100).Substring(0, 100).Trim();
                                                    oVoucher.Lines.Reference2 = memo;
                                                    memo = memo.PadRight(50).Substring(0, 50).Trim();
                                                    oVoucher.Lines.LineMemo = memo;
                                                }
                                                else
                                                {
                                                    //oVoucher.Lines.AccountCode = cta_gasto;
                                                    oVoucher.Lines.AccountCode = CuentaContable(gasto.Detalle.UCatcode);
                                                    if (localizacion.Equals("LATINEQ") || localizacion.Equals("LATINEUY"))
                                                    {
                                                        cat4 = string.Empty;
                                                        cat5 = string.Empty;
                                                        ctactbgasto = string.Empty;
                                                        foreach (var extragasto in gasto.Detalle.CamposExtra.Detalle)
                                                        {
                                                            if (extragasto.UName.Equals("Cat4"))
                                                            {
                                                                cat4 = extragasto.UValue;
                                                            }
                                                            if (extragasto.UName.Equals("Cat5"))
                                                            {
                                                                cat5 = extragasto.UValue;
                                                            }
                                                        }
                                                        cat4 = gasto.Detalle.UCategory;
                                                        ctactbgasto = SBO.ConsultasSBO.ObtenerCtaCtbSegunCategorias(cat1, cat2, cat3, cat4, cat5);
                                                        oVoucher.Lines.AccountCode = ctactbgasto;
                                                    }
                                                    switch (dimension)
                                                    {
                                                        case "1":
                                                            oVoucher.Lines.CostingCode = centro_costo;
                                                            break;
                                                        case "2":
                                                            if (localizacion.Equals("FLESAN1"))
                                                            {
                                                                centro_costo_dim1 = SBO.ConsultasSBO.ObtenerCCDimension1(centro_costo);
                                                                oVoucher.Lines.CostingCode = centro_costo_dim1;
                                                                centro_costo_dim3 = SBO.ConsultasSBO.ObtenerCCDimension3(gasto.Detalle.UCatcode);
                                                                if (!string.IsNullOrEmpty(centro_costo_dim3))
                                                                {
                                                                    oVoucher.Lines.CostingCode3 = centro_costo_dim3;
                                                                    oVoucher.Lines.CostingCode4 = "2000";
                                                                    oVoucher.Lines.CostingCode5 = "2007";
                                                                }
                                                                oVoucher.Lines.ProjectCode = centro_costo;
                                                                oVoucher.ProjectCode = centro_costo;
                                                            }
                                                            oVoucher.Lines.CostingCode2 = centro_costo;
                                                            break;
                                                        case "3":
                                                            oVoucher.Lines.CostingCode3 = centro_costo;
                                                            break;
                                                        case "4":
                                                            oVoucher.Lines.CostingCode4 = centro_costo;
                                                            break;
                                                        case "5":
                                                            oVoucher.Lines.CostingCode5 = centro_costo;
                                                            break;
                                                        default:
                                                            oVoucher.Lines.CostingCode = centro_costo;
                                                            break;
                                                    }
                                                    if (localizacion.Equals("LATINEQ") || localizacion.Equals("LATINEUY"))
                                                    {
                                                        oVoucher.Lines.CostingCode = centro_costo;
                                                        oVoucher.Lines.CostingCode2 = centro_costo_2;
                                                        oVoucher.Lines.ProjectCode = nro_llamada;
                                                    }
                                                    memo = gasto.Detalle.UIdate.ToString("dd-MM-yyyy") + " / " + gasto.Detalle.UCategory + " / Boleta";
                                                    if (localizacion.Equals("LATINEQ") || localizacion.Equals("LATINEUY"))
                                                    {
                                                        memo = "RG" + rendicion.Detalle.URepnum.ToString() + "_" + gasto.Detalle.UIdate.ToString("ddMM") + "/" + gasto.Detalle.UCategory + "/Boleta";
                                                    }
                                                    memo = memo.PadRight(100).Substring(0, 100).Trim();
                                                    oVoucher.Lines.Reference2 = memo;
                                                    memo = memo.PadRight(50).Substring(0, 50).Trim();
                                                    oVoucher.Lines.LineMemo = memo;
                                                }
                                                string monedaGasto = SBO.ConsultasSBO.ObtenerCodigoMonedaISO(gasto.Detalle.UOcur);
                                                if (monedalocal.Equals(monedaGasto))
                                                {
                                                    oVoucher.Lines.Debit = gasto.Detalle.UTotal;
                                                }
                                                else
                                                {
                                                    oVoucher.Lines.FCCurrency = monedaGasto;
                                                    oVoucher.Lines.FCDebit = gasto.Detalle.UTotal;
                                                }
                                                totGastos += gasto.Detalle.UTotal;
                                                linea++;
                                            }
                                        }

                                        if (linea > 0)
                                        {
                                            oVoucher.Lines.Add();
                                        }
                                        //////oVoucher.SetCurrentLine(linea);
                                        //oVoucher.Lines.ShortName = rendicion.Detalle.UEmpide;
                                        if (forma_pago.Equals("TARJETACREDITO"))
                                        {
                                            oVoucher.Lines.AccountCode = CuentaContable(ctacontable_tarjetas);
                                        }
                                        else
                                        {
                                            oVoucher.Lines.ShortName = _CardCodeRendidor;
                                            if (localizacion.Equals("SUATRANSPE"))
                                            {
                                                oVoucher.Lines.ControlAccount = CuentaContable(ctacontable_asoc_sn);
                                            }
                                        }
                                        string monedaRendicion = SBO.ConsultasSBO.ObtenerCodigoMonedaISO(rendicion.Detalle.UCur);
                                        if (monedalocal.Equals(monedaRendicion))
                                        {
                                            oVoucher.Lines.Credit = totGastos;
                                        }
                                        else
                                        {
                                            oVoucher.Lines.FCCurrency = monedaRendicion;
                                            oVoucher.Lines.FCCredit = totGastos;
                                        }
                                        if (forma_pago.Equals("TARJETACREDITO"))
                                        {
                                            memo = "Rendidor " + rendicion.Detalle.UEmpname;
                                        }
                                        else
                                        {
                                            memo = "A pagar a rendidor " + rendicion.Detalle.UEmpname;
                                            if (localizacion.Equals("LATINEQ") || localizacion.Equals("LATINEUY"))
                                            {
                                                memo = "Rendición N°" + rendicion.Detalle.URepnum.ToString() + " " + rendicion.Detalle.UEmpname;
                                            }
                                        }
                                        memo = memo.PadRight(50).Substring(0, 50).Trim();
                                        oVoucher.Lines.LineMemo = memo;
                                        if (localizacion.Equals("FLESAN1"))
                                        {
                                            oVoucher.Lines.ProjectCode = centro_costo;
                                        }
                                        if (localizacion.Equals("LATINEQ") || localizacion.Equals("LATINEUY"))
                                        {
                                            oVoucher.Lines.ProjectCode = nro_llamada;
                                        }

                                        errCode = 0;
                                        errMsg = "";
                                        SBO.ConexionSBO.oCompany.StartTransaction();
                                        ret = oVoucher.Add();
                                    }
                                    #endregion 1ER ASIENTO CONTABLE

                                    if (ret != 0)
                                    {
                                        SBO.ConexionSBO.oCompany.GetLastError(out errCode, out errMsg);
                                        if (SBO.ConexionSBO.oCompany.InTransaction)
                                        {
                                            SBO.ConexionSBO.oCompany.EndTransaction(BoWfTransOpt.wf_RollBack);
                                        }
                                        Observaciones = string.Format("Asiento Rendición: {0} - {1}", rendicion.Detalle.URepnum, errMsg);
                                        update = SBO.ModeloSBO.UpdateRendicion(rendicion.Detalle.DocEntry.ToString(), 9, Observaciones.PadRight(250).Substring(0, 250).Trim());
                                        algunError = true;
                                        break;
                                    }
                                    else
                                    {
                                        string asiento1 = SBO.ConexionSBO.oCompany.GetNewObjectKey();
                                        asiento_integracion = asiento1;
                                        Observaciones += string.Format("Asiento: {0}", asiento1);

                                        //***FACTURA DE PROVEEDOR***//
                                        algunError = false;
                                        foreach (var gasto in rendicion.Detalle.Gastos.Items)
                                        {
                                            if (gasto.Detalle.UStatus.Equals(1) && !algunError)
                                            {
                                                tipo_gasto = "Gasto";
                                                rut_proveedor = "";
                                                centro_costo = "";
                                                centro_costo_2 = "";
                                                detalle_factura = "";
                                                codigoIVA22 = "";
                                                codigoIVA10 = "";
                                                codigoIVA0 = "";
                                                codigoIVAPropina = "";
                                                montoIVA22 = 0;
                                                montoIVA10 = 0;
                                                montoIVA0 = 0;
                                                montoPropina = 0;

                                                foreach (var extras in gasto.Detalle.CamposExtra.Detalle)
                                                {
                                                    if (extras.UName.Equals(campo_extra_tipo_documento_nombre) && campo_extra_tipo_documento_valor.Equals("VALUE"))
                                                    {
                                                        if (extras.UValue.Equals(codigo_factura))
                                                        {
                                                            tipo_gasto = "Factura";
                                                        }
                                                        if (extras.UValue.Equals(codigo_factura_ex))
                                                        {
                                                            tipo_gasto = "FacturaEx";
                                                        }
                                                        if (extras.UValue.Equals(codigo_factura_mat))
                                                        {
                                                            tipo_gasto = "FacturaMat";
                                                        }
                                                        if (extras.UValue.Equals(codigo_factura_honorarios) && localizacion.Equals("SUATRANSPE"))
                                                        {
                                                            tipo_gasto = "FacturaHR";
                                                        }
                                                    }
                                                    if (extras.UName.Equals(campo_extra_tipo_documento_nombre) && campo_extra_tipo_documento_valor.Equals("CODE"))
                                                    {
                                                        if (extras.UCode.Equals(codigo_factura))
                                                        {
                                                            tipo_gasto = "Factura";
                                                        }
                                                        if (extras.UCode.Equals(codigo_factura_ex))
                                                        {
                                                            tipo_gasto = "FacturaEx";
                                                        }
                                                        if (extras.UCode.Equals(codigo_factura_mat))
                                                        {
                                                            tipo_gasto = "FacturaMat";
                                                        }
                                                        if (extras.UCode.Equals(codigo_factura_honorarios) && localizacion.Equals("SUATRANSPE"))
                                                        {
                                                            tipo_gasto = "FacturaHR";
                                                        }
                                                    }
                                                    if (extras.UName.Equals(campo_extra_rut_proveedor_nombre) && campo_extra_rut_proveedor_valor.Equals("VALUE"))
                                                    {
                                                        rut_proveedor = extras.UValue;
                                                    }
                                                    if (extras.UName.Equals(campo_extra_rut_proveedor_nombre) && campo_extra_rut_proveedor_valor.Equals("CODE"))
                                                    {
                                                        rut_proveedor = extras.UCode;
                                                    }
                                                    if (extras.UName.Equals(campo_extra_centro_costo_nombre) && campo_extra_centro_costo_valor.Equals("VALUE"))
                                                    {
                                                        centro_costo = extras.UValue;
                                                    }
                                                    if (extras.UName.Equals(campo_extra_centro_costo_nombre) && campo_extra_centro_costo_valor.Equals("CODE"))
                                                    {
                                                        centro_costo = extras.UCode;
                                                    }
                                                    if (extras.UName.Equals(campo_extra_centro_costo_2_nombre) && campo_extra_centro_costo_2_valor.Equals("VALUE"))
                                                    {
                                                        centro_costo_2 = extras.UValue;
                                                    }
                                                    if (extras.UName.Equals(campo_extra_centro_costo_2_nombre) && campo_extra_centro_costo_2_valor.Equals("CODE"))
                                                    {
                                                        centro_costo_2 = extras.UCode;
                                                    }
                                                    if (extras.UName.Equals(campo_extra_centro_costo_dim4_nombre) && campo_extra_centro_costo_dim4_valor.Equals("VALUE"))
                                                    {
                                                        centro_costo_dim4 = extras.UValue;
                                                    }
                                                    if (extras.UName.Equals(campo_extra_centro_costo_dim4_nombre) && campo_extra_centro_costo_dim4_valor.Equals("CODE"))
                                                    {
                                                        centro_costo_dim4 = extras.UCode;
                                                    }
                                                    if (extras.UName.Equals(campo_extra_centro_costo_dim5_nombre) && campo_extra_centro_costo_dim5_valor.Equals("VALUE"))
                                                    {
                                                        centro_costo_dim5 = extras.UValue;
                                                    }
                                                    if (extras.UName.Equals(campo_extra_centro_costo_dim5_nombre) && campo_extra_centro_costo_dim5_valor.Equals("CODE"))
                                                    {
                                                        centro_costo_dim5 = extras.UCode;
                                                    }
                                                    if (extras.UName.Equals(campo_extra_numero_documento_nombre) && campo_extra_numero_documento_valor.Equals("VALUE"))
                                                    {
                                                        numero_documento = extras.UValue;
                                                    }
                                                    if (extras.UName.Equals(campo_extra_numero_documento_nombre) && campo_extra_numero_documento_valor.Equals("CODE"))
                                                    {
                                                        numero_documento = extras.UCode;
                                                    }
                                                    //if (localizacion.Equals("LATINEQ") || localizacion.Equals("LATINEUY"))
                                                    //{
                                                    //    if (extras.UName.Equals("Detalle Factura"))
                                                    //    {
                                                    //        detalle_factura = extras.UValue;
                                                    //        detalle_factura = detalle_factura.PadRight(254).Substring(0, 254).Trim();
                                                    //    }
                                                    //}
                                                    if (localizacion.Equals("LATINEUY"))
                                                    {
                                                        if (extras.UName.Equals("IVA_22"))
                                                        {
                                                            codigoIVA22 = extras.UValue;
                                                        }
                                                        if (extras.UName.Equals("IVA_10"))
                                                        {
                                                            codigoIVA10 = extras.UValue;
                                                        }
                                                        if (extras.UName.Equals("IVA_0"))
                                                        {
                                                            codigoIVA0 = extras.UValue;
                                                            codigoIVAPropina = extras.UValue;
                                                        }
                                                        if (extras.UName.Equals("Monto_22"))
                                                        {
                                                            montoIVA22 = Convert.ToDouble(extras.UValue);
                                                        }
                                                        if (extras.UName.Equals("Monto_10"))
                                                        {
                                                            montoIVA10 = Convert.ToDouble(extras.UValue);
                                                        }
                                                        if (extras.UName.Equals("Monto_0"))
                                                        {
                                                            montoIVA0 = Convert.ToDouble(extras.UValue);
                                                        }
                                                        if (extras.UName.Equals("Propina"))
                                                        {
                                                            montoPropina = Convert.ToDouble(extras.UValue);
                                                        }
                                                    }

                                                    if (localizacion.Equals("SUATRANS") || localizacion.Equals("SUATRANSTR"))
                                                    {
                                                        if (extras.UName.Equals("Código SAP"))
                                                        {
                                                            codigo_sap = extras.UValue;
                                                        }
                                                    }
                                                }
                                                dimension = SBO.ConsultasSBO.ObtenerDimensionCC(centro_costo);
                                                if (tipo_gasto.Equals("Factura") || tipo_gasto.Equals("FacturaEx") || tipo_gasto.Equals("FacturaMat") || tipo_gasto.Equals("FacturaHR"))
                                                {
                                                    string _CardCode = SBO.ConsultasSBO.ObtenerCardCodeProveedor(rut_proveedor);
                                                    // Validar SN Proveedor
                                                    if (string.IsNullOrEmpty(_CardCode))
                                                    {
                                                        if (SBO.ConexionSBO.oCompany.InTransaction)
                                                        {
                                                            SBO.ConexionSBO.oCompany.EndTransaction(BoWfTransOpt.wf_RollBack);
                                                        }
                                                        Observaciones = string.Format("FC Gasto ID: {0} - No existe socio de negocios {1}.", gasto.Detalle.UId, rut_proveedor);
                                                        update = SBO.ModeloSBO.UpdateRendicion(rendicion.Detalle.DocEntry.ToString(), 9, Observaciones.PadRight(250).Substring(0, 250).Trim());
                                                        algunError = true;
                                                        break;
                                                    }

                                                    #region FACTURA DE PROVEEDOR
                                                    SAPbobsCOM.Documents oDoc;
                                                    if (borrador_factura)
                                                    {
                                                        //borrador
                                                        oDoc = (SAPbobsCOM.Documents)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oDrafts);
                                                        oDoc.DocObjectCode = BoObjectTypes.oPurchaseInvoices;
                                                    }
                                                    else
                                                    {
                                                        //final
                                                        oDoc = (SAPbobsCOM.Documents)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseInvoices);
                                                    }
                                                    oDoc.TaxDate = Convert.ToDateTime(gasto.Detalle.UIdate.ToString("yyyy-MM-dd"));
                                                    oDoc.DocDueDate = Convert.ToDateTime(gasto.Detalle.UIdate.ToString("yyyy-MM-dd"));
                                                    //oDoc.TaxDate = DateTime.Now;
                                                    //oDoc.DocDueDate = DateTime.Now;
                                                    oDoc.DocDate = DateTime.Now;
                                                    oDoc.DocType = BoDocumentTypes.dDocument_Service;
                                                    oDoc.CardCode = _CardCode;
                                                    oDoc.Comments = "Rendición N°" + rendicion.Detalle.URepnum.ToString() + " " + rendicion.Detalle.UEmpname;
                                                    oDoc.NumAtCard = numero_documento;
                                                    if (localizacion.Equals("LATINEQ") || localizacion.Equals("LATINEUY"))
                                                    {
                                                        memo = "RG" + rendicion.Detalle.URepnum.ToString() + "_" + gasto.Detalle.UIdate.ToString("ddMM") + "/" + gasto.Detalle.UCategory + "/Factura";
                                                        memo = memo.PadRight(50).Substring(0, 50).Trim();
                                                        oDoc.JournalMemo = memo;
                                                    }
                                                    if (registrar_folio)
                                                    {
                                                        switch (tipo_gasto)
                                                        {
                                                            case "Factura":
                                                                oDoc.FolioPrefixString = "33";
                                                                break;
                                                            case "FacturaEx":
                                                                oDoc.FolioPrefixString = "34";
                                                                break;
                                                            default:
                                                                oDoc.FolioPrefixString = "33";
                                                                break;
                                                        }
                                                        if (localizacion.Equals("SUATRANSPE"))
                                                        {
                                                            if (tipo_gasto.Equals("FacturaHR"))
                                                            {
                                                                oDoc.FolioPrefixString = "RH";
                                                            }
                                                            else
                                                            {
                                                                oDoc.FolioPrefixString = "FT";
                                                            }
                                                            var SerieCorr = numero_documento.Split('-');
                                                            int numfolio = 0;
                                                            if (Int32.TryParse(SerieCorr[1], out numfolio))
                                                            {
                                                                oDoc.FolioNumber = numfolio;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            oDoc.FolioNumber = int.Parse(numero_documento);
                                                        }
                                                    }
                                                    if (registrar_indicador)
                                                    {
                                                        switch (tipo_gasto)
                                                        {
                                                            case "Factura":
                                                                oDoc.Indicator = "33";
                                                                break;
                                                            case "FacturaEx":
                                                                oDoc.Indicator = "34";
                                                                break;
                                                            default:
                                                                oDoc.Indicator = "33";
                                                                break;
                                                        }
                                                        if (localizacion.Equals("SUATRANSPE"))
                                                        {
                                                            if (tipo_gasto.Equals("FacturaHR"))
                                                            {
                                                                oDoc.Indicator = "02";
                                                            }
                                                            else
                                                            {
                                                                oDoc.Indicator = "01";
                                                            }
                                                        }
                                                    }
                                                    if (localizacion.Equals("LATINEQ") || localizacion.Equals("LATINEUY"))
                                                    {
                                                        oDoc.Project = nro_llamada;
                                                    }

                                                    ////////////////////////////////////
                                                    string monedaGasto = SBO.ConsultasSBO.ObtenerCodigoMonedaISO(gasto.Detalle.UOcur);
                                                    if (!localizacion.Equals("LATINEUY"))
                                                    {
                                                        oDoc.Lines.SetCurrentLine(0);
                                                        oDoc.Lines.ItemDescription = gasto.Detalle.UNote.PadRight(100).Substring(0, 100).Trim();
                                                        oDoc.Lines.Quantity = 1;
                                                        //cuenta gastos generales o compra materiales menores
                                                        if (tipo_gasto.Equals("FacturaMat"))
                                                        {
                                                            oDoc.Lines.AccountCode = CuentaContable(cta_compra_materiales);
                                                        }
                                                        else
                                                        {
                                                            if (cuenta_en_gastos)
                                                            {
                                                                oDoc.Lines.AccountCode = CuentaContable(gasto.Detalle.UCatcode);
                                                                if (localizacion.Equals("LATINEQ"))
                                                                {
                                                                    cat4 = string.Empty;
                                                                    cat5 = string.Empty;
                                                                    ctactbgasto = string.Empty;
                                                                    foreach (var extragasto in gasto.Detalle.CamposExtra.Detalle)
                                                                    {
                                                                        if (extragasto.UName.Equals("Cat4"))
                                                                        {
                                                                            cat4 = extragasto.UValue;
                                                                        }
                                                                        if (extragasto.UName.Equals("Cat5"))
                                                                        {
                                                                            cat5 = extragasto.UValue;
                                                                        }
                                                                    }
                                                                    cat4 = gasto.Detalle.UCategory;
                                                                    ctactbgasto = SBO.ConsultasSBO.ObtenerCtaCtbSegunCategorias(cat1, cat2, cat3, cat4, cat5);
                                                                    oDoc.Lines.AccountCode = ctactbgasto;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                oDoc.Lines.AccountCode = CuentaContable(cta_serv_proveedor);
                                                            }
                                                        }

                                                        switch (dimension)
                                                        {
                                                            case "1":
                                                                oDoc.Lines.CostingCode = centro_costo;
                                                                break;
                                                            case "2":
                                                                if (localizacion.Equals("FLESAN1"))
                                                                {
                                                                    centro_costo_dim1 = SBO.ConsultasSBO.ObtenerCCDimension1(centro_costo);
                                                                    oDoc.Lines.CostingCode = centro_costo_dim1;
                                                                    if (tipo_gasto.Equals("FacturaMat"))
                                                                    {
                                                                        centro_costo_dim3 = SBO.ConsultasSBO.ObtenerCCDimension3(cta_compra_materiales);
                                                                    }
                                                                    else
                                                                    {
                                                                        centro_costo_dim3 = SBO.ConsultasSBO.ObtenerCCDimension3(gasto.Detalle.UCatcode);
                                                                    }
                                                                    if (!string.IsNullOrEmpty(centro_costo_dim3))
                                                                    {
                                                                        oDoc.Lines.CostingCode3 = centro_costo_dim3;
                                                                        //oDoc.Lines.CostingCode4 = "2000";
                                                                        //oDoc.Lines.CostingCode5 = "2007";
                                                                        oDoc.Lines.CostingCode4 = centro_costo_dim4;
                                                                        oDoc.Lines.CostingCode5 = centro_costo_dim5;
                                                                    }
                                                                    oDoc.Lines.ProjectCode = centro_costo;
                                                                }
                                                                oDoc.Lines.CostingCode2 = centro_costo;
                                                                break;
                                                            case "3":
                                                                oDoc.Lines.CostingCode3 = centro_costo;
                                                                break;
                                                            case "4":
                                                                oDoc.Lines.CostingCode4 = centro_costo;
                                                                break;
                                                            case "5":
                                                                oDoc.Lines.CostingCode5 = centro_costo;
                                                                break;
                                                            default:
                                                                oDoc.Lines.CostingCode = centro_costo;
                                                                break;
                                                        }
                                                        if (localizacion.Equals("LATINEQ"))
                                                        {
                                                            oDoc.Lines.CostingCode = centro_costo;
                                                            oDoc.Lines.CostingCode2 = centro_costo_2;
                                                            oDoc.Lines.ProjectCode = nro_llamada;
                                                        }
                                                        oDoc.Lines.TaxCode = gasto.Detalle.UTaxname;

                                                        //oDoc.Lines.LineTotal = gasto.Detalle.UNet;
                                                        //oDoc.Lines.TaxTotal = gasto.Detalle.UTax;

                                                        monedaGasto = SBO.ConsultasSBO.ObtenerCodigoMonedaISO(gasto.Detalle.UOcur);
                                                        if (monedalocal.Equals(monedaGasto))
                                                        {
                                                            oDoc.Lines.LineTotal = gasto.Detalle.UNet;
                                                            oDoc.DocTotal = gasto.Detalle.UTotal;
                                                        }
                                                        else
                                                        {
                                                            oDoc.Lines.Currency = monedaGasto;
                                                            oDoc.Lines.LineTotal = gasto.Detalle.UNet;
                                                            oDoc.DocCurrency = monedaGasto;
                                                            oDoc.DocTotalFc = gasto.Detalle.UTotal;
                                                        }
                                                    }
                                                    else if (localizacion.Equals("LATINEUY"))
                                                    {
                                                        int lin = 0;
                                                        if (!string.IsNullOrEmpty(gasto.Detalle.UTaxname) && gasto.Detalle.UNet > 0)
                                                        {
                                                            oDoc.Lines.SetCurrentLine(lin);
                                                            oDoc.Lines.ItemDescription = gasto.Detalle.UNote.PadRight(100).Substring(0, 100).Trim();
                                                            oDoc.Lines.Quantity = 1;
                                                            //cuenta gastos generales o compra materiales menores
                                                            if (tipo_gasto.Equals("FacturaMat"))
                                                            {
                                                                oDoc.Lines.AccountCode = CuentaContable(cta_compra_materiales);
                                                            }
                                                            else
                                                            {
                                                                if (cuenta_en_gastos)
                                                                {
                                                                    oDoc.Lines.AccountCode = CuentaContable(gasto.Detalle.UCatcode);
                                                                    cat4 = string.Empty;
                                                                    cat5 = string.Empty;
                                                                    ctactbgasto = string.Empty;
                                                                    foreach (var extragasto in gasto.Detalle.CamposExtra.Detalle)
                                                                    {
                                                                        if (extragasto.UName.Equals("Cat4"))
                                                                        {
                                                                            cat4 = extragasto.UValue;
                                                                        }
                                                                        if (extragasto.UName.Equals("Cat5"))
                                                                        {
                                                                            cat5 = extragasto.UValue;
                                                                        }
                                                                    }
                                                                    cat4 = gasto.Detalle.UCategory;
                                                                    ctactbgasto = SBO.ConsultasSBO.ObtenerCtaCtbSegunCategorias(cat1, cat2, cat3, cat4, cat5);
                                                                    oDoc.Lines.AccountCode = ctactbgasto;
                                                                }
                                                                else
                                                                {
                                                                    oDoc.Lines.AccountCode = CuentaContable(cta_serv_proveedor);
                                                                }
                                                            }

                                                            switch (dimension)
                                                            {
                                                                case "1":
                                                                    oDoc.Lines.CostingCode = centro_costo;
                                                                    break;
                                                                case "2":
                                                                    oDoc.Lines.CostingCode2 = centro_costo;
                                                                    break;
                                                                case "3":
                                                                    oDoc.Lines.CostingCode3 = centro_costo;
                                                                    break;
                                                                case "4":
                                                                    oDoc.Lines.CostingCode4 = centro_costo;
                                                                    break;
                                                                case "5":
                                                                    oDoc.Lines.CostingCode5 = centro_costo;
                                                                    break;
                                                                default:
                                                                    oDoc.Lines.CostingCode = centro_costo;
                                                                    break;
                                                            }
                                                            oDoc.Lines.CostingCode = centro_costo;
                                                            oDoc.Lines.CostingCode2 = centro_costo_2;
                                                            oDoc.Lines.ProjectCode = nro_llamada;
                                                            oDoc.Lines.TaxCode = gasto.Detalle.UTaxname;

                                                            monedaGasto = SBO.ConsultasSBO.ObtenerCodigoMonedaISO(gasto.Detalle.UOcur);
                                                            if (monedalocal.Equals(monedaGasto))
                                                            {
                                                                oDoc.Lines.LineTotal = gasto.Detalle.UNet;
                                                                oDoc.DocTotal = gasto.Detalle.UTotal;
                                                            }
                                                            else
                                                            {
                                                                oDoc.Lines.Currency = monedaGasto;
                                                                oDoc.Lines.LineTotal = gasto.Detalle.UNet;
                                                                oDoc.DocCurrency = monedaGasto;
                                                                oDoc.DocTotalFc = gasto.Detalle.UTotal;
                                                            }
                                                            lin++;
                                                        }
                                                        if (montoIVA22 > 0)
                                                        {
                                                            if(lin > 0)
                                                            {
                                                                oDoc.Lines.Add();
                                                            }
                                                            else
                                                            {
                                                                oDoc.Lines.SetCurrentLine(lin);
                                                            }
                                                            //oDoc.Lines.SetCurrentLine(lin);
                                                            oDoc.Lines.ItemDescription = gasto.Detalle.UNote.PadRight(100).Substring(0, 100).Trim();
                                                            oDoc.Lines.Quantity = 1;
                                                            //cuenta gastos generales o compra materiales menores
                                                            if (tipo_gasto.Equals("FacturaMat"))
                                                            {
                                                                oDoc.Lines.AccountCode = CuentaContable(cta_compra_materiales);
                                                            }
                                                            else
                                                            {
                                                                if (cuenta_en_gastos)
                                                                {
                                                                    oDoc.Lines.AccountCode = CuentaContable(gasto.Detalle.UCatcode);
                                                                    cat4 = string.Empty;
                                                                    cat5 = string.Empty;
                                                                    ctactbgasto = string.Empty;
                                                                    foreach (var extragasto in gasto.Detalle.CamposExtra.Detalle)
                                                                    {
                                                                        if (extragasto.UName.Equals("Cat4"))
                                                                        {
                                                                            cat4 = extragasto.UValue;
                                                                        }
                                                                        if (extragasto.UName.Equals("Cat5"))
                                                                        {
                                                                            cat5 = extragasto.UValue;
                                                                        }
                                                                    }
                                                                    cat4 = gasto.Detalle.UCategory;
                                                                    ctactbgasto = SBO.ConsultasSBO.ObtenerCtaCtbSegunCategorias(cat1, cat2, cat3, cat4, cat5);
                                                                    oDoc.Lines.AccountCode = ctactbgasto;
                                                                }
                                                                else
                                                                {
                                                                    oDoc.Lines.AccountCode = CuentaContable(cta_serv_proveedor);
                                                                }
                                                            }

                                                            switch (dimension)
                                                            {
                                                                case "1":
                                                                    oDoc.Lines.CostingCode = centro_costo;
                                                                    break;
                                                                case "2":
                                                                    oDoc.Lines.CostingCode2 = centro_costo;
                                                                    break;
                                                                case "3":
                                                                    oDoc.Lines.CostingCode3 = centro_costo;
                                                                    break;
                                                                case "4":
                                                                    oDoc.Lines.CostingCode4 = centro_costo;
                                                                    break;
                                                                case "5":
                                                                    oDoc.Lines.CostingCode5 = centro_costo;
                                                                    break;
                                                                default:
                                                                    oDoc.Lines.CostingCode = centro_costo;
                                                                    break;
                                                            }
                                                            oDoc.Lines.CostingCode = centro_costo;
                                                            oDoc.Lines.CostingCode2 = centro_costo_2;
                                                            oDoc.Lines.ProjectCode = nro_llamada;
                                                            oDoc.Lines.TaxCode = codigoIVA22;

                                                            monedaGasto = SBO.ConsultasSBO.ObtenerCodigoMonedaISO(gasto.Detalle.UOcur);
                                                            if (monedalocal.Equals(monedaGasto))
                                                            {
                                                                oDoc.Lines.LineTotal = montoIVA22;
                                                                oDoc.DocTotal = gasto.Detalle.UTotal;
                                                            }
                                                            else
                                                            {
                                                                oDoc.Lines.Currency = monedaGasto;
                                                                oDoc.Lines.LineTotal = montoIVA22;
                                                                oDoc.DocCurrency = monedaGasto;
                                                                oDoc.DocTotalFc = gasto.Detalle.UTotal;
                                                            }
                                                            lin++;
                                                        }
                                                        if (montoIVA10 > 0)
                                                        {
                                                            if (lin > 0)
                                                            {
                                                                oDoc.Lines.Add();
                                                            }
                                                            else
                                                            {
                                                                oDoc.Lines.SetCurrentLine(lin);
                                                            }
                                                            //oDoc.Lines.SetCurrentLine(lin);
                                                            oDoc.Lines.ItemDescription = gasto.Detalle.UNote.PadRight(100).Substring(0, 100).Trim();
                                                            oDoc.Lines.Quantity = 1;
                                                            //cuenta gastos generales o compra materiales menores
                                                            if (tipo_gasto.Equals("FacturaMat"))
                                                            {
                                                                oDoc.Lines.AccountCode = CuentaContable(cta_compra_materiales);
                                                            }
                                                            else
                                                            {
                                                                if (cuenta_en_gastos)
                                                                {
                                                                    oDoc.Lines.AccountCode = CuentaContable(gasto.Detalle.UCatcode);
                                                                    cat4 = string.Empty;
                                                                    cat5 = string.Empty;
                                                                    ctactbgasto = string.Empty;
                                                                    foreach (var extragasto in gasto.Detalle.CamposExtra.Detalle)
                                                                    {
                                                                        if (extragasto.UName.Equals("Cat4"))
                                                                        {
                                                                            cat4 = extragasto.UValue;
                                                                        }
                                                                        if (extragasto.UName.Equals("Cat5"))
                                                                        {
                                                                            cat5 = extragasto.UValue;
                                                                        }
                                                                    }
                                                                    cat4 = gasto.Detalle.UCategory;
                                                                    ctactbgasto = SBO.ConsultasSBO.ObtenerCtaCtbSegunCategorias(cat1, cat2, cat3, cat4, cat5);
                                                                    oDoc.Lines.AccountCode = ctactbgasto;
                                                                }
                                                                else
                                                                {
                                                                    oDoc.Lines.AccountCode = CuentaContable(cta_serv_proveedor);
                                                                }
                                                            }

                                                            switch (dimension)
                                                            {
                                                                case "1":
                                                                    oDoc.Lines.CostingCode = centro_costo;
                                                                    break;
                                                                case "2":
                                                                    oDoc.Lines.CostingCode2 = centro_costo;
                                                                    break;
                                                                case "3":
                                                                    oDoc.Lines.CostingCode3 = centro_costo;
                                                                    break;
                                                                case "4":
                                                                    oDoc.Lines.CostingCode4 = centro_costo;
                                                                    break;
                                                                case "5":
                                                                    oDoc.Lines.CostingCode5 = centro_costo;
                                                                    break;
                                                                default:
                                                                    oDoc.Lines.CostingCode = centro_costo;
                                                                    break;
                                                            }
                                                            oDoc.Lines.CostingCode = centro_costo;
                                                            oDoc.Lines.CostingCode2 = centro_costo_2;
                                                            oDoc.Lines.ProjectCode = nro_llamada;
                                                            oDoc.Lines.TaxCode = codigoIVA10;

                                                            monedaGasto = SBO.ConsultasSBO.ObtenerCodigoMonedaISO(gasto.Detalle.UOcur);
                                                            if (monedalocal.Equals(monedaGasto))
                                                            {
                                                                oDoc.Lines.LineTotal = montoIVA10;
                                                                oDoc.DocTotal = gasto.Detalle.UTotal;
                                                            }
                                                            else
                                                            {
                                                                oDoc.Lines.Currency = monedaGasto;
                                                                oDoc.Lines.LineTotal = montoIVA10;
                                                                oDoc.DocCurrency = monedaGasto;
                                                                oDoc.DocTotalFc = gasto.Detalle.UTotal;
                                                            }
                                                            lin++;
                                                        }
                                                        if (montoIVA0 > 0)
                                                        {
                                                            if (lin > 0)
                                                            {
                                                                oDoc.Lines.Add();
                                                            }
                                                            else
                                                            {
                                                                oDoc.Lines.SetCurrentLine(lin);
                                                            }
                                                            //oDoc.Lines.SetCurrentLine(lin);
                                                            oDoc.Lines.ItemDescription = gasto.Detalle.UNote.PadRight(100).Substring(0, 100).Trim();
                                                            oDoc.Lines.Quantity = 1;
                                                            //cuenta gastos generales o compra materiales menores
                                                            if (tipo_gasto.Equals("FacturaMat"))
                                                            {
                                                                oDoc.Lines.AccountCode = CuentaContable(cta_compra_materiales);
                                                            }
                                                            else
                                                            {
                                                                if (cuenta_en_gastos)
                                                                {
                                                                    oDoc.Lines.AccountCode = CuentaContable(gasto.Detalle.UCatcode);
                                                                    cat4 = string.Empty;
                                                                    cat5 = string.Empty;
                                                                    ctactbgasto = string.Empty;
                                                                    foreach (var extragasto in gasto.Detalle.CamposExtra.Detalle)
                                                                    {
                                                                        if (extragasto.UName.Equals("Cat4"))
                                                                        {
                                                                            cat4 = extragasto.UValue;
                                                                        }
                                                                        if (extragasto.UName.Equals("Cat5"))
                                                                        {
                                                                            cat5 = extragasto.UValue;
                                                                        }
                                                                    }
                                                                    cat4 = gasto.Detalle.UCategory;
                                                                    ctactbgasto = SBO.ConsultasSBO.ObtenerCtaCtbSegunCategorias(cat1, cat2, cat3, cat4, cat5);
                                                                    oDoc.Lines.AccountCode = ctactbgasto;
                                                                }
                                                                else
                                                                {
                                                                    oDoc.Lines.AccountCode = CuentaContable(cta_serv_proveedor);
                                                                }
                                                            }

                                                            switch (dimension)
                                                            {
                                                                case "1":
                                                                    oDoc.Lines.CostingCode = centro_costo;
                                                                    break;
                                                                case "2":
                                                                    oDoc.Lines.CostingCode2 = centro_costo;
                                                                    break;
                                                                case "3":
                                                                    oDoc.Lines.CostingCode3 = centro_costo;
                                                                    break;
                                                                case "4":
                                                                    oDoc.Lines.CostingCode4 = centro_costo;
                                                                    break;
                                                                case "5":
                                                                    oDoc.Lines.CostingCode5 = centro_costo;
                                                                    break;
                                                                default:
                                                                    oDoc.Lines.CostingCode = centro_costo;
                                                                    break;
                                                            }
                                                            oDoc.Lines.CostingCode = centro_costo;
                                                            oDoc.Lines.CostingCode2 = centro_costo_2;
                                                            oDoc.Lines.ProjectCode = nro_llamada;
                                                            oDoc.Lines.TaxCode = codigoIVA0;

                                                            monedaGasto = SBO.ConsultasSBO.ObtenerCodigoMonedaISO(gasto.Detalle.UOcur);
                                                            if (monedalocal.Equals(monedaGasto))
                                                            {
                                                                oDoc.Lines.LineTotal = montoIVA0;
                                                                oDoc.DocTotal = gasto.Detalle.UTotal;
                                                            }
                                                            else
                                                            {
                                                                oDoc.Lines.Currency = monedaGasto;
                                                                oDoc.Lines.LineTotal = montoIVA0;
                                                                oDoc.DocCurrency = monedaGasto;
                                                                oDoc.DocTotalFc = gasto.Detalle.UTotal;
                                                            }
                                                            lin++;
                                                        }
                                                        if (montoPropina > 0)
                                                        {
                                                            if (lin > 0)
                                                            {
                                                                oDoc.Lines.Add();
                                                            }
                                                            else
                                                            {
                                                                oDoc.Lines.SetCurrentLine(lin);
                                                            }
                                                            //oDoc.Lines.SetCurrentLine(lin);
                                                            oDoc.Lines.ItemDescription = "Propina";
                                                            oDoc.Lines.Quantity = 1;
                                                            //cuenta gastos generales o compra materiales menores
                                                            if (tipo_gasto.Equals("FacturaMat"))
                                                            {
                                                                oDoc.Lines.AccountCode = CuentaContable(cta_compra_materiales);
                                                            }
                                                            else
                                                            {
                                                                if (cuenta_en_gastos)
                                                                {
                                                                    oDoc.Lines.AccountCode = CuentaContable(gasto.Detalle.UCatcode);
                                                                    cat4 = string.Empty;
                                                                    cat5 = string.Empty;
                                                                    ctactbgasto = string.Empty;
                                                                    foreach (var extragasto in gasto.Detalle.CamposExtra.Detalle)
                                                                    {
                                                                        if (extragasto.UName.Equals("Cat4"))
                                                                        {
                                                                            cat4 = extragasto.UValue;
                                                                        }
                                                                        if (extragasto.UName.Equals("Cat5"))
                                                                        {
                                                                            cat5 = extragasto.UValue;
                                                                        }
                                                                    }
                                                                    cat4 = gasto.Detalle.UCategory;
                                                                    ctactbgasto = SBO.ConsultasSBO.ObtenerCtaCtbSegunCategorias(cat1, cat2, cat3, cat4, cat5);
                                                                    oDoc.Lines.AccountCode = ctactbgasto;
                                                                }
                                                                else
                                                                {
                                                                    oDoc.Lines.AccountCode = CuentaContable(cta_serv_proveedor);
                                                                }
                                                            }

                                                            switch (dimension)
                                                            {
                                                                case "1":
                                                                    oDoc.Lines.CostingCode = centro_costo;
                                                                    break;
                                                                case "2":
                                                                    oDoc.Lines.CostingCode2 = centro_costo;
                                                                    break;
                                                                case "3":
                                                                    oDoc.Lines.CostingCode3 = centro_costo;
                                                                    break;
                                                                case "4":
                                                                    oDoc.Lines.CostingCode4 = centro_costo;
                                                                    break;
                                                                case "5":
                                                                    oDoc.Lines.CostingCode5 = centro_costo;
                                                                    break;
                                                                default:
                                                                    oDoc.Lines.CostingCode = centro_costo;
                                                                    break;
                                                            }
                                                            oDoc.Lines.CostingCode = centro_costo;
                                                            oDoc.Lines.CostingCode2 = centro_costo_2;
                                                            oDoc.Lines.ProjectCode = nro_llamada;
                                                            oDoc.Lines.TaxCode = codigoIVAPropina;

                                                            monedaGasto = SBO.ConsultasSBO.ObtenerCodigoMonedaISO(gasto.Detalle.UOcur);
                                                            if (monedalocal.Equals(monedaGasto))
                                                            {
                                                                oDoc.Lines.LineTotal = montoPropina;
                                                                oDoc.DocTotal = gasto.Detalle.UTotal;
                                                            }
                                                            else
                                                            {
                                                                oDoc.Lines.Currency = monedaGasto;
                                                                oDoc.Lines.LineTotal = montoPropina;
                                                                oDoc.DocCurrency = monedaGasto;
                                                                oDoc.DocTotalFc = gasto.Detalle.UTotal;
                                                            }
                                                        }
                                                    }
                                                    ///////////////////////////////////////////

                                                    if (localizacion.Equals("FLESAN1"))
                                                    {
                                                        oDoc.Project = centro_costo;
                                                        //Motivo de traslado
                                                        oDoc.UserFields.Fields.Item("U_SYP_MDMT").Value = "02";
                                                        //Tipo de documento
                                                        oDoc.UserFields.Fields.Item("U_SYP_MDTD").Value = "01";
                                                        try
                                                        {
                                                            var SerieCorr = numero_documento.Split('-');
                                                            //Serie del documento
                                                            oDoc.UserFields.Fields.Item("U_SYP_MDSD").Value = SerieCorr[0];
                                                            //Correlativo del documento
                                                            oDoc.UserFields.Fields.Item("U_SYP_MDCD").Value = int.Parse(SerieCorr[1]).ToString().PadLeft(8, '0');
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            if (SBO.ConexionSBO.oCompany.InTransaction)
                                                            {
                                                                SBO.ConexionSBO.oCompany.EndTransaction(BoWfTransOpt.wf_RollBack);
                                                            }
                                                            Observaciones = string.Format("FC Gasto ID: {0} - Error al recuperar Serie o Correlativo del documento {1}.", gasto.Detalle.UId, numero_documento);
                                                            update = SBO.ModeloSBO.UpdateRendicion(rendicion.Detalle.DocEntry.ToString(), 9, Observaciones.PadRight(250).Substring(0, 250).Trim());
                                                            algunError = true;
                                                            break;
                                                        }
                                                        //Tipo de compra
                                                        oDoc.UserFields.Fields.Item("U_SYP_TCOMPRA").Value = "03";
                                                        //Numero CC/ER
                                                        oDoc.UserFields.Fields.Item("U_SYP_CODERCC").Value = fondo_rendir;
                                                        //Bienes y Servicios adquiridos
                                                        oDoc.UserFields.Fields.Item("U_SYP_BIESRVADQ").Value = "1";
                                                    }
                                                    if (localizacion.Equals("LATINEQ") || localizacion.Equals("LATINEUY"))
                                                    {
                                                        oDoc.UserFields.Fields.Item("U_SEI_NmroLlamada").Value = string.IsNullOrEmpty(nro_llamada) ? "*" : nro_llamada;
                                                        oDoc.UserFields.Fields.Item("U_SEI_PRIORIDAD").Value = prioridad;
                                                        oDoc.UserFields.Fields.Item("U_SEI_MAQ_REF").Value = maquina_referencia;
                                                        oDoc.UserFields.Fields.Item("U_SEI_Sucursales").Value = sucursales;
                                                        oDoc.UserFields.Fields.Item("U_SEI_TIPR").Value = tipo_rend;
                                                        detalle_factura = gasto.Detalle.UNote.PadRight(254).Substring(0, 254).Trim();
                                                        oDoc.UserFields.Fields.Item("U_SEI_DetalleFactura").Value = detalle_factura;
                                                    }
                                                    if (localizacion.Equals("SUATRANS") || localizacion.Equals("SUATRANSTR"))
                                                    {
                                                        oDoc.UserFields.Fields.Item("U_IDSAP").Value = codigo_sap;
                                                        oDoc.Lines.UserFields.Fields.Item("U_IDSAP").Value = codigo_sap;
                                                    }
                                                    if (registrar_campos_soindus)
                                                    {
                                                        //Numero ER
                                                        oDoc.UserFields.Fields.Item("U_SO_NUMER").Value = fondo_rendir;
                                                    }

                                                    errCode = 0;
                                                    errMsg = "";
                                                    retDoc = oDoc.Add();

                                                    #endregion FACTURA DE PROVEEDOR

                                                    if (retDoc != 0)
                                                    {
                                                        SBO.ConexionSBO.oCompany.GetLastError(out errCode, out errMsg);
                                                        if (SBO.ConexionSBO.oCompany.InTransaction)
                                                        {
                                                            SBO.ConexionSBO.oCompany.EndTransaction(BoWfTransOpt.wf_RollBack);
                                                        }
                                                        Observaciones = string.Format("FC Gasto ID: {0} - {1}", gasto.Detalle.UId, errMsg);
                                                        update = SBO.ModeloSBO.UpdateRendicion(rendicion.Detalle.DocEntry.ToString(), 9, Observaciones.PadRight(250).Substring(0, 250).Trim());
                                                        algunError = true;
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        var nuevo_doc = SBO.ConexionSBO.oCompany.GetNewObjectKey();
                                                        string docnum = string.Empty;
                                                        try
                                                        {
                                                            docnum = SBO.ConsultasSBO.RecuperaDocNum(nuevo_doc);
                                                        }
                                                        catch (Exception)
                                                        {
                                                            docnum = nuevo_doc;
                                                        }

                                                        Observaciones += string.Format(" - Factura: {0}", docnum);

                                                        if (compensar_factura_con_pago && !borrador_factura)
                                                        {
                                                            // Compensar Factura con pago efectuado
                                                            #region PAGO EFECTUADO
                                                            SAPbobsCOM.Payments oPayment;
                                                            if (borrador_compensa_factura_pagoef)
                                                            {
                                                                //preliminar
                                                                oPayment = (SAPbobsCOM.Payments)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPaymentsDrafts);
                                                            }
                                                            else
                                                            {
                                                                //final
                                                                oPayment = (SAPbobsCOM.Payments)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oVendorPayments);
                                                            }
                                                            oPayment.DocObjectCode = SAPbobsCOM.BoPaymentsObjectType.bopot_OutgoingPayments;
                                                            oPayment.DocType = SAPbobsCOM.BoRcptTypes.rSupplier;
                                                            memo = string.Empty;
                                                            memo = "Compensa Rendición N°" + rendicion.Detalle.URepnum.ToString() + " " + rendicion.Detalle.UEmpname;
                                                            memo = memo.PadRight(250).Substring(0, 250).Trim();
                                                            oPayment.Remarks = memo;
                                                            if (localizacion.Equals("LATINEQ") || localizacion.Equals("LATINEUY"))
                                                            {
                                                                memo = memo.PadRight(50).Substring(0, 50).Trim();
                                                                oPayment.JournalRemarks = memo;
                                                            }
                                                            oPayment.CardCode = _CardCode;
                                                            //oPayment.DocDate = Convert.ToDateTime(rendicion.Detalle.UCdate.ToString("yyyy-MM-dd"));
                                                            oPayment.DocDate = DateTime.Now;
                                                            oPayment.TaxDate = Convert.ToDateTime(rendicion.Detalle.UCdate.ToString("yyyy-MM-dd"));
                                                            string monedaPago = SBO.ConsultasSBO.ObtenerCodigoMonedaISO(rendicion.Detalle.UCur);
                                                            oPayment.LocalCurrency = SAPbobsCOM.BoYesNoEnum.tNO;
                                                            if (monedalocal.Equals(monedaPago))
                                                            {
                                                                oPayment.CashSum = gasto.Detalle.UTotal;
                                                            }
                                                            else
                                                            {
                                                                oPayment.DocCurrency = monedaPago;
                                                                oPayment.CashSum = gasto.Detalle.UTotal;
                                                            }
                                                            oPayment.Invoices.InvoiceType = BoRcptInvTypes.it_PurchaseInvoice;
                                                            oPayment.Invoices.DocEntry = int.Parse(nuevo_doc);
                                                            oPayment.Invoices.SumApplied = gasto.Detalle.UTotal;
                                                            if (compensar_factura_con_pago)
                                                            {
                                                                oPayment.CashAccount = CuentaContable(cta_compensacion_facturas);
                                                            }
                                                            if (registrar_campos_soindus)
                                                            {
                                                                //Numero ER
                                                                oPayment.UserFields.Fields.Item("U_SO_NUMER").Value = fondo_rendir;
                                                            }
                                                            if (localizacion.Equals("LATINEQ") || localizacion.Equals("LATINEUY"))
                                                            {
                                                                oPayment.ProjectCode = nro_llamada;
                                                                oPayment.CounterReference = rendicion.Detalle.URepnum.ToString();
                                                            }
                                                            if (localizacion.Equals("SUATRANS") || localizacion.Equals("SUATRANSTR"))
                                                            {
                                                                oPayment.ProjectCode = proyecto;
                                                            }

                                                            errCode = 0;
                                                            errMsg = "";
                                                            int retPago = oPayment.Add();

                                                            #endregion PAGO EFECTUADO

                                                            if (retPago != 0)
                                                            {
                                                                SBO.ConexionSBO.oCompany.GetLastError(out errCode, out errMsg);
                                                                if (SBO.ConexionSBO.oCompany.InTransaction)
                                                                {
                                                                    SBO.ConexionSBO.oCompany.EndTransaction(BoWfTransOpt.wf_RollBack);
                                                                }
                                                                Observaciones = string.Format("Pago Efectuado Rendición: {0} - {1}", rendicion.Detalle.URepnum, errMsg);
                                                                update = SBO.ModeloSBO.UpdateRendicion(rendicion.Detalle.DocEntry.ToString(), 9, Observaciones.PadRight(250).Substring(0, 250).Trim());
                                                                algunError = true;
                                                                break;
                                                            }
                                                            else
                                                            {
                                                                string pagoefectuado = SBO.ConexionSBO.oCompany.GetNewObjectKey();
                                                                Observaciones += string.Format(" - Pago Efectuado: {0}", pagoefectuado);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            // Compensar Factura con asiento
                                                            //***3ER ASIENTO CONTABLE***//
                                                            #region 3ER ASIENTO
                                                            if (borrador_compensa_factura_asiento)
                                                            {
                                                                //preliminar
                                                                SAPbobsCOM.JournalVouchers oVoucherN = (SAPbobsCOM.JournalVouchers)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oJournalVouchers);
                                                                //fecha documento
                                                                oVoucherN.JournalEntries.ReferenceDate = DateTime.Now;
                                                                oVoucherN.JournalEntries.TaxDate = Convert.ToDateTime(gasto.Detalle.UIdate.ToString("yyyy-MM-dd"));
                                                                oVoucherN.JournalEntries.DueDate = Convert.ToDateTime(gasto.Detalle.UIdate.ToString("yyyy-MM-dd"));
                                                                oVoucherN.JournalEntries.Reference2 = rendicion.Detalle.URepnum.ToString();
                                                                oVoucherN.JournalEntries.Reference3 = fondo_rendir;

                                                                memo = "Compensa Rendición N°" + rendicion.Detalle.URepnum.ToString() + " " + rendicion.Detalle.UEmpname;
                                                                memo = memo.PadRight(50).Substring(0, 50).Trim();
                                                                oVoucherN.JournalEntries.Memo = memo;
                                                                if (localizacion.Equals("LATINEQ") || localizacion.Equals("LATINEUY"))
                                                                {
                                                                    oVoucherN.JournalEntries.ProjectCode = nro_llamada;
                                                                }
                                                                linea = 0;
                                                                oVoucherN.JournalEntries.SetCurrentLine(linea);

                                                                oVoucherN.JournalEntries.Lines.ShortName = _CardCode;
                                                                memo = gasto.Detalle.UIdate.ToString("dd-MM-yyyy") + " / " + gasto.Detalle.UCategory + " / Factura";
                                                                if (localizacion.Equals("LATINEQ") || localizacion.Equals("LATINEUY"))
                                                                {
                                                                    memo = "RG" + rendicion.Detalle.URepnum.ToString() + "_" + gasto.Detalle.UIdate.ToString("ddMM") + "/" + gasto.Detalle.UCategory + "/Factura";
                                                                }
                                                                memo = memo.PadRight(100).Substring(0, 100).Trim();
                                                                oVoucherN.JournalEntries.Lines.Reference2 = memo;
                                                                memo = memo.PadRight(50).Substring(0, 50).Trim();
                                                                oVoucherN.JournalEntries.Lines.LineMemo = memo;

                                                                switch (dimension)
                                                                {
                                                                    case "1":
                                                                        oVoucherN.JournalEntries.Lines.CostingCode = centro_costo;
                                                                        break;
                                                                    case "2":
                                                                        if (localizacion.Equals("FLESAN1"))
                                                                        {
                                                                            centro_costo_dim1 = SBO.ConsultasSBO.ObtenerCCDimension1(centro_costo);
                                                                            oVoucherN.JournalEntries.Lines.CostingCode = centro_costo_dim1;
                                                                            if (tipo_gasto.Equals("FacturaMat"))
                                                                            {
                                                                                centro_costo_dim3 = SBO.ConsultasSBO.ObtenerCCDimension3(cta_compra_materiales);
                                                                            }
                                                                            else
                                                                            {
                                                                                centro_costo_dim3 = SBO.ConsultasSBO.ObtenerCCDimension3(gasto.Detalle.UCatcode);
                                                                            }
                                                                            if (!string.IsNullOrEmpty(centro_costo_dim3))
                                                                            {
                                                                                oVoucherN.JournalEntries.Lines.CostingCode3 = centro_costo_dim3;
                                                                                //oVoucherN.JournalEntries.Lines.CostingCode4 = "2000";
                                                                                //oVoucherN.JournalEntries.Lines.CostingCode5 = "2007";
                                                                                oVoucherN.JournalEntries.Lines.CostingCode4 = centro_costo_dim4;
                                                                                oVoucherN.JournalEntries.Lines.CostingCode5 = centro_costo_dim5;
                                                                            }
                                                                            oVoucherN.JournalEntries.Lines.ProjectCode = centro_costo;
                                                                            oVoucherN.JournalEntries.ProjectCode = centro_costo;
                                                                        }
                                                                        oVoucherN.JournalEntries.Lines.CostingCode2 = centro_costo;
                                                                        break;
                                                                    case "3":
                                                                        oVoucherN.JournalEntries.Lines.CostingCode3 = centro_costo;
                                                                        break;
                                                                    case "4":
                                                                        oVoucherN.JournalEntries.Lines.CostingCode4 = centro_costo;
                                                                        break;
                                                                    case "5":
                                                                        oVoucherN.JournalEntries.Lines.CostingCode5 = centro_costo;
                                                                        break;
                                                                    default:
                                                                        oVoucherN.JournalEntries.Lines.CostingCode = centro_costo;
                                                                        break;
                                                                }
                                                                if (localizacion.Equals("LATINEQ") || localizacion.Equals("LATINEUY"))
                                                                {
                                                                    oVoucherN.JournalEntries.Lines.CostingCode = centro_costo;
                                                                    oVoucherN.JournalEntries.Lines.CostingCode2 = centro_costo_2;
                                                                    oVoucherN.JournalEntries.Lines.ProjectCode = nro_llamada;
                                                                }
                                                                if (monedalocal.Equals(monedaGasto))
                                                                {
                                                                    oVoucherN.JournalEntries.Lines.Debit = gasto.Detalle.UTotal;
                                                                }
                                                                else
                                                                {
                                                                    oVoucherN.JournalEntries.Lines.FCCurrency = monedaGasto;
                                                                    oVoucherN.JournalEntries.Lines.FCDebit = gasto.Detalle.UTotal;
                                                                }

                                                                oVoucherN.JournalEntries.Lines.Add();

                                                                oVoucherN.JournalEntries.Lines.AccountCode = CuentaContable(cta_ant_proveedor);
                                                                memo = gasto.Detalle.UIdate.ToString("dd-MM-yyyy") + " / " + gasto.Detalle.UCategory + " / Factura";
                                                                memo = memo.PadRight(100).Substring(0, 100).Trim();
                                                                oVoucherN.JournalEntries.Lines.Reference2 = memo;
                                                                memo = memo.PadRight(50).Substring(0, 50).Trim();
                                                                oVoucherN.JournalEntries.Lines.LineMemo = memo;
                                                                switch (dimension)
                                                                {
                                                                    case "1":
                                                                        oVoucherN.JournalEntries.Lines.CostingCode = centro_costo;
                                                                        break;
                                                                    case "2":
                                                                        if (localizacion.Equals("FLESAN1"))
                                                                        {
                                                                            centro_costo_dim1 = SBO.ConsultasSBO.ObtenerCCDimension1(centro_costo);
                                                                            oVoucherN.JournalEntries.Lines.CostingCode = centro_costo_dim1;
                                                                            centro_costo_dim3 = SBO.ConsultasSBO.ObtenerCCDimension3(cta_ant_proveedor);
                                                                            if (!string.IsNullOrEmpty(centro_costo_dim3))
                                                                            {
                                                                                oVoucherN.JournalEntries.Lines.CostingCode3 = centro_costo_dim3;
                                                                                //oVoucherN.JournalEntries.Lines.CostingCode4 = "2000";
                                                                                //oVoucherN.JournalEntries.Lines.CostingCode5 = "2007";
                                                                                oVoucherN.JournalEntries.Lines.CostingCode4 = centro_costo_dim4;
                                                                                oVoucherN.JournalEntries.Lines.CostingCode5 = centro_costo_dim5;
                                                                            }
                                                                            oVoucherN.JournalEntries.Lines.ProjectCode = centro_costo;
                                                                            oVoucherN.JournalEntries.ProjectCode = centro_costo;
                                                                        }
                                                                        oVoucherN.JournalEntries.Lines.CostingCode2 = centro_costo;
                                                                        break;
                                                                    case "3":
                                                                        oVoucherN.JournalEntries.Lines.CostingCode3 = centro_costo;
                                                                        break;
                                                                    case "4":
                                                                        oVoucherN.JournalEntries.Lines.CostingCode4 = centro_costo;
                                                                        break;
                                                                    case "5":
                                                                        oVoucherN.JournalEntries.Lines.CostingCode5 = centro_costo;
                                                                        break;
                                                                    default:
                                                                        oVoucherN.JournalEntries.Lines.CostingCode = centro_costo;
                                                                        break;
                                                                }
                                                                if (localizacion.Equals("LATINEQ") || localizacion.Equals("LATINEUY"))
                                                                {
                                                                    oVoucherN.JournalEntries.Lines.CostingCode = centro_costo;
                                                                    oVoucherN.JournalEntries.Lines.CostingCode2 = centro_costo_2;
                                                                    oVoucherN.JournalEntries.Lines.ProjectCode = nro_llamada;
                                                                }
                                                                if (monedalocal.Equals(monedaGasto))
                                                                {
                                                                    oVoucherN.JournalEntries.Lines.Credit = gasto.Detalle.UTotal;
                                                                }
                                                                else
                                                                {
                                                                    oVoucherN.JournalEntries.Lines.FCCurrency = monedaGasto;
                                                                    oVoucherN.JournalEntries.Lines.FCCredit = gasto.Detalle.UTotal;
                                                                }

                                                                errCode = 0;
                                                                errMsg = "";
                                                                ret = oVoucherN.Add();
                                                            }
                                                            else
                                                            {
                                                                //final
                                                                SAPbobsCOM.JournalEntries oVoucherN = (SAPbobsCOM.JournalEntries)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oJournalEntries);
                                                                //fecha documento
                                                                oVoucherN.ReferenceDate = DateTime.Now;
                                                                oVoucherN.TaxDate = Convert.ToDateTime(gasto.Detalle.UIdate.ToString("yyyy-MM-dd"));
                                                                oVoucherN.DueDate = Convert.ToDateTime(gasto.Detalle.UIdate.ToString("yyyy-MM-dd"));
                                                                oVoucherN.Reference2 = rendicion.Detalle.URepnum.ToString();
                                                                oVoucherN.Reference3 = fondo_rendir;

                                                                memo = "Compensa Rendición N°" + rendicion.Detalle.URepnum.ToString() + " " + rendicion.Detalle.UEmpname;
                                                                memo = memo.PadRight(50).Substring(0, 50).Trim();
                                                                oVoucherN.Memo = memo;
                                                                if (localizacion.Equals("LATINEQ") || localizacion.Equals("LATINEUY"))
                                                                {
                                                                    oVoucherN.ProjectCode = nro_llamada;
                                                                }

                                                                linea = 0;
                                                                oVoucherN.SetCurrentLine(linea);

                                                                oVoucherN.Lines.ShortName = _CardCode;
                                                                memo = gasto.Detalle.UIdate.ToString("dd-MM-yyyy") + " / " + gasto.Detalle.UCategory + " / Factura";
                                                                if (localizacion.Equals("LATINEQ") || localizacion.Equals("LATINEUY"))
                                                                {
                                                                    memo = "RG" + rendicion.Detalle.URepnum.ToString() + "_" + gasto.Detalle.UIdate.ToString("ddMM") + "/" + gasto.Detalle.UCategory + "/Factura";
                                                                }
                                                                memo = memo.PadRight(100).Substring(0, 100).Trim();
                                                                oVoucherN.Lines.Reference2 = memo;
                                                                memo = memo.PadRight(50).Substring(0, 50).Trim();
                                                                oVoucherN.Lines.LineMemo = memo;
                                                                switch (dimension)
                                                                {
                                                                    case "1":
                                                                        oVoucherN.Lines.CostingCode = centro_costo;
                                                                        break;
                                                                    case "2":
                                                                        if (localizacion.Equals("FLESAN1"))
                                                                        {
                                                                            centro_costo_dim1 = SBO.ConsultasSBO.ObtenerCCDimension1(centro_costo);
                                                                            oVoucherN.Lines.CostingCode = centro_costo_dim1;
                                                                            if (tipo_gasto.Equals("FacturaMat"))
                                                                            {
                                                                                centro_costo_dim3 = SBO.ConsultasSBO.ObtenerCCDimension3(cta_compra_materiales);
                                                                            }
                                                                            else
                                                                            {
                                                                                centro_costo_dim3 = SBO.ConsultasSBO.ObtenerCCDimension3(gasto.Detalle.UCatcode);
                                                                            }
                                                                            if (!string.IsNullOrEmpty(centro_costo_dim3))
                                                                            {
                                                                                oVoucherN.Lines.CostingCode3 = centro_costo_dim3;
                                                                                //oVoucherN.Lines.CostingCode4 = "2000";
                                                                                //oVoucherN.Lines.CostingCode5 = "2007";
                                                                                oVoucherN.Lines.CostingCode4 = centro_costo_dim4;
                                                                                oVoucherN.Lines.CostingCode5 = centro_costo_dim5;
                                                                            }
                                                                            oVoucherN.Lines.ProjectCode = centro_costo;
                                                                            oVoucherN.ProjectCode = centro_costo;
                                                                        }
                                                                        oVoucherN.Lines.CostingCode2 = centro_costo;
                                                                        break;
                                                                    case "3":
                                                                        oVoucherN.Lines.CostingCode3 = centro_costo;
                                                                        break;
                                                                    case "4":
                                                                        oVoucherN.Lines.CostingCode4 = centro_costo;
                                                                        break;
                                                                    case "5":
                                                                        oVoucherN.Lines.CostingCode5 = centro_costo;
                                                                        break;
                                                                    default:
                                                                        oVoucherN.Lines.CostingCode = centro_costo;
                                                                        break;
                                                                }
                                                                if (localizacion.Equals("LATINEQ") || localizacion.Equals("LATINEUY"))
                                                                {
                                                                    oVoucherN.Lines.CostingCode = centro_costo;
                                                                    oVoucherN.Lines.CostingCode2 = centro_costo_2;
                                                                    oVoucherN.Lines.ProjectCode = nro_llamada;
                                                                }
                                                                if (monedalocal.Equals(monedaGasto))
                                                                {
                                                                    oVoucherN.Lines.Debit = gasto.Detalle.UTotal;
                                                                }
                                                                else
                                                                {
                                                                    oVoucherN.Lines.FCCurrency = monedaGasto;
                                                                    oVoucherN.Lines.FCDebit = gasto.Detalle.UTotal;
                                                                }

                                                                oVoucherN.Lines.Add();

                                                                oVoucherN.Lines.AccountCode = CuentaContable(cta_ant_proveedor);
                                                                memo = gasto.Detalle.UIdate.ToString("dd-MM-yyyy") + " / " + gasto.Detalle.UCategory + " / Factura";
                                                                memo = memo.PadRight(100).Substring(0, 100).Trim();
                                                                oVoucherN.Lines.Reference2 = memo;
                                                                memo = memo.PadRight(50).Substring(0, 50).Trim();
                                                                oVoucherN.Lines.LineMemo = memo;
                                                                switch (dimension)
                                                                {
                                                                    case "1":
                                                                        oVoucherN.Lines.CostingCode = centro_costo;
                                                                        break;
                                                                    case "2":
                                                                        if (localizacion.Equals("FLESAN1"))
                                                                        {
                                                                            centro_costo_dim1 = SBO.ConsultasSBO.ObtenerCCDimension1(centro_costo);
                                                                            oVoucherN.Lines.CostingCode = centro_costo_dim1;
                                                                            centro_costo_dim3 = SBO.ConsultasSBO.ObtenerCCDimension3(cta_ant_proveedor);
                                                                            if (!string.IsNullOrEmpty(centro_costo_dim3))
                                                                            {
                                                                                oVoucherN.Lines.CostingCode3 = centro_costo_dim3;
                                                                                //oVoucherN.Lines.CostingCode4 = "2000";
                                                                                //oVoucherN.Lines.CostingCode5 = "2007";
                                                                                oVoucherN.Lines.CostingCode4 = centro_costo_dim4;
                                                                                oVoucherN.Lines.CostingCode5 = centro_costo_dim5;
                                                                            }
                                                                            oVoucherN.Lines.ProjectCode = centro_costo;
                                                                            oVoucherN.ProjectCode = centro_costo;
                                                                        }
                                                                        oVoucherN.Lines.CostingCode2 = centro_costo;
                                                                        break;
                                                                    case "3":
                                                                        oVoucherN.Lines.CostingCode3 = centro_costo;
                                                                        break;
                                                                    case "4":
                                                                        oVoucherN.Lines.CostingCode4 = centro_costo;
                                                                        break;
                                                                    case "5":
                                                                        oVoucherN.Lines.CostingCode5 = centro_costo;
                                                                        break;
                                                                    default:
                                                                        oVoucherN.Lines.CostingCode = centro_costo;
                                                                        break;
                                                                }
                                                                if (localizacion.Equals("LATINEQ") || localizacion.Equals("LATINEUY"))
                                                                {
                                                                    oVoucherN.Lines.CostingCode = centro_costo;
                                                                    oVoucherN.Lines.CostingCode2 = centro_costo_2;
                                                                    oVoucherN.Lines.ProjectCode = nro_llamada;
                                                                }
                                                                if (monedalocal.Equals(monedaGasto))
                                                                {
                                                                    oVoucherN.Lines.Credit = gasto.Detalle.UTotal;
                                                                }
                                                                else
                                                                {
                                                                    oVoucherN.Lines.FCCurrency = monedaGasto;
                                                                    oVoucherN.Lines.FCCredit = gasto.Detalle.UTotal;
                                                                }

                                                                errCode = 0;
                                                                errMsg = "";
                                                                ret = oVoucherN.Add();
                                                            }

                                                            #endregion 3ER ASIENTO

                                                            if (ret != 0)
                                                            {
                                                                SBO.ConexionSBO.oCompany.GetLastError(out errCode, out errMsg);
                                                                if (SBO.ConexionSBO.oCompany.InTransaction)
                                                                {
                                                                    SBO.ConexionSBO.oCompany.EndTransaction(BoWfTransOpt.wf_RollBack);
                                                                }
                                                                Observaciones = string.Format("Asiento Gasto ID: {0} - {1}", gasto.Detalle.UId, errMsg);
                                                                update = SBO.ModeloSBO.UpdateRendicion(rendicion.Detalle.DocEntry.ToString(), 9, Observaciones.PadRight(250).Substring(0, 250).Trim());
                                                                algunError = true;
                                                                break;
                                                            }
                                                            else
                                                            {
                                                                string asiento3 = SBO.ConexionSBO.oCompany.GetNewObjectKey();
                                                                Observaciones += string.Format(" - Asiento: {0}", asiento3);
                                                            }
                                                            // Fin Creación 3er asiento
                                                        }
                                                        // Fin Compensar factura
                                                    }
                                                    // Fin Creación Factura
                                                }
                                                // Fin Factura o FacturaEx
                                            }
                                            // Fin Gastos aprobados
                                        }
                                        // Fin Foreach Gastos
                                    }
                                    // Fin Creación 1er asiento
                                    if (algunError)
                                    {
                                        break;
                                    }
                                    if ((tipo_fondo_pago.Equals("REEMBOLSO") || tipo_fondo_pago.Equals("EXCESO")) && !forma_pago.Equals("TARJETACREDITO"))
                                    {
                                        //***PAGO EFECTUADO POR REEMBOLSO O EXCESO A FAVOR***//
                                        #region PAGO EFECTUADO
                                        SAPbobsCOM.Payments oPayment;
                                        if (borrador_pago_a_favor)
                                        {
                                            //preliminar
                                            oPayment = (SAPbobsCOM.Payments)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPaymentsDrafts);
                                        }
                                        else
                                        {
                                            //final
                                            oPayment = (SAPbobsCOM.Payments)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oVendorPayments);
                                        }
                                        oPayment.DocObjectCode = SAPbobsCOM.BoPaymentsObjectType.bopot_OutgoingPayments;
                                        if (localizacion.Equals("ICLINICS"))
                                        {
                                            oPayment.DocType = SAPbobsCOM.BoRcptTypes.rCustomer;
                                        }
                                        else if (localizacion.Equals("PCLINICS"))
                                        {
                                            oPayment.DocType = SAPbobsCOM.BoRcptTypes.rCustomer;
                                        }
                                        else if (localizacion.Equals("LATINEQ"))
                                        {
                                            oPayment.DocType = SAPbobsCOM.BoRcptTypes.rSupplier;
                                        }
                                        else if (localizacion.Equals("LATINEUY"))
                                        {
                                            oPayment.DocType = SAPbobsCOM.BoRcptTypes.rSupplier;
                                        }
                                        else if (localizacion.Equals("SUATRANS"))
                                        {
                                            oPayment.DocType = SAPbobsCOM.BoRcptTypes.rSupplier;
                                        }
                                        else if (localizacion.Equals("SUATRANSTR"))
                                        {
                                            oPayment.DocType = SAPbobsCOM.BoRcptTypes.rCustomer;
                                        }
                                        else if (localizacion.Equals("SUATRANSPE"))
                                        {
                                            oPayment.DocType = SAPbobsCOM.BoRcptTypes.rSupplier;
                                        }
                                        else
                                        {
                                            oPayment.DocType = SAPbobsCOM.BoRcptTypes.rCustomer;
                                        }
                                        memo = string.Empty;
                                        memo = "Reembolso rendición N°" + rendicion.Detalle.URepnum.ToString() + " " + rendicion.Detalle.UEmpname;
                                        memo = memo.PadRight(250).Substring(0, 250).Trim();
                                        oPayment.Remarks = memo;
                                        if (localizacion.Equals("LATINEQ") || localizacion.Equals("LATINEUY"))
                                        {
                                            memo = memo.PadRight(50).Substring(0, 50).Trim();
                                            oPayment.JournalRemarks = memo;
                                        }
                                        oPayment.CardCode = _CardCodeRendidor;
                                        //oPayment.DocDate = Convert.ToDateTime(rendicion.Detalle.UCdate.ToString("yyyy-MM-dd"));
                                        oPayment.DocDate = DateTime.Now;
                                        oPayment.TaxDate = Convert.ToDateTime(rendicion.Detalle.UCdate.ToString("yyyy-MM-dd"));
                                        string monedaPago = SBO.ConsultasSBO.ObtenerCodigoMonedaISO(rendicion.Detalle.UCur);
                                        oPayment.LocalCurrency = SAPbobsCOM.BoYesNoEnum.tNO;
                                        oPayment.DocCurrency = monedaPago;
                                        oPayment.CashSum = monto_reembolso;
                                        if (pago_con_cta_reembolso)
                                        {
                                            oPayment.CashAccount = CuentaContable(cta_pago_reembolsos);
                                        }
                                        if (tipo_fondo_pago.Equals("EXCESO"))
                                        {
                                            oPayment.ControlAccount = CuentaContable(ctacontable_fondo);
                                        }
                                        //else if (tipo_fondo_pago.Equals("REEMBOLSO"))
                                        //{
                                        //    if (pago_con_cta_reembolso)
                                        //    {
                                        //        oPayment.ControlAccount = cta_pago_reembolsos;
                                        //    }
                                        //}

                                        string numER = DateTime.Now.Ticks.ToString();
                                        if (localizacion.Equals("FLESAN1"))
                                        {
                                            //Tipo operación de pago
                                            oPayment.UserFields.Fields.Item("U_SYP_TPOOPER").Value = "99";
                                            //Numero ER
                                            //oPayment.UserFields.Fields.Item("U_SYP_NUMER").Value = string.Format("ER{0}", numER);
                                            if (tipo_fondo_pago.Equals("EXCESO"))
                                            {
                                                oPayment.UserFields.Fields.Item("U_SYP_NUMER").Value = fondo_rendir;
                                            }
                                            //Estado cajachica/ER
                                            oPayment.UserFields.Fields.Item("U_SYP_CCERSTATUS").Value = "O";
                                            //Serie ER
                                            //oPayment.UserFields.Fields.Item("U_SYP_SERER").Value = numER;
                                            if (tipo_fondo_pago.Equals("EXCESO"))
                                            {
                                                oPayment.UserFields.Fields.Item("U_SYP_SERER").Value = fondo_rendir.Substring(2, fondo_rendir.Length - 2);
                                            }
                                        }
                                        if (registrar_campos_soindus)
                                        {
                                            //Numero ER
                                            if (tipo_fondo_pago.Equals("EXCESO"))
                                            {
                                                oPayment.UserFields.Fields.Item("U_SO_NUMER").Value = fondo_rendir;
                                            }
                                        }
                                        if (localizacion.Equals("LATINEQ") || localizacion.Equals("LATINEUY"))
                                        {
                                            oPayment.ProjectCode = nro_llamada;
                                            oPayment.CounterReference = rendicion.Detalle.URepnum.ToString();
                                        }
                                        if (localizacion.Equals("SUATRANS") || localizacion.Equals("SUATRANSTR"))
                                        {
                                            oPayment.ProjectCode = proyecto;
                                        }

                                        errCode = 0;
                                        errMsg = "";
                                        int retPago = oPayment.Add();

                                        #endregion PAGO EFECTUADO

                                        if (retPago != 0)
                                        {
                                            SBO.ConexionSBO.oCompany.GetLastError(out errCode, out errMsg);
                                            if (SBO.ConexionSBO.oCompany.InTransaction)
                                            {
                                                SBO.ConexionSBO.oCompany.EndTransaction(BoWfTransOpt.wf_RollBack);
                                            }
                                            Observaciones = string.Format("Pago Efectuado Rendición: {0} - {1}", rendicion.Detalle.URepnum, errMsg);
                                            update = SBO.ModeloSBO.UpdateRendicion(rendicion.Detalle.DocEntry.ToString(), 9, Observaciones.PadRight(250).Substring(0, 250).Trim());
                                            algunError = true;
                                            break;
                                        }
                                        else
                                        {
                                            string pagoefectuado = SBO.ConexionSBO.oCompany.GetNewObjectKey();
                                            pago_rendicion = pagoefectuado;
                                            Observaciones += string.Format(" - Pago Efectuado: {0}", pagoefectuado);
                                        }
                                    }

                                    ///******MARCAR RENDICION COMO INTEGRADA EN RG*******///
                                    algunError = false;

                                    string Id = rendicion.Detalle.UId.ToString();
                                    string IntegrationStatus = "1";
                                    string IntegrationCode = string.Format("{0}", asiento_integracion);
                                    string IntegrationDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                                    string[] _args2 = new string[]
                                    {
                                            Id, IntegrationStatus, IntegrationCode, IntegrationDate
                                    };

                                    var respInt = interfazRG.CambiarEstadoRendicion(_args2);
                                    if (respInt.Success)
                                    {
                                        algunError = false;
                                        Observaciones += string.Format(" - Integrada en Rindegastos");
                                    }
                                    else
                                    {
                                        if (SBO.ConexionSBO.oCompany.InTransaction)
                                        {
                                            SBO.ConexionSBO.oCompany.EndTransaction(BoWfTransOpt.wf_RollBack);
                                        }
                                        Observaciones = string.Format("Rendición: {0} - No se pudo cambiar el estado integrado en Rindegastos.", rendicion.Detalle.URepnum.ToString());
                                        update = SBO.ModeloSBO.UpdateRendicion(rendicion.Detalle.DocEntry.ToString(), 9, Observaciones.PadRight(250).Substring(0, 250).Trim());
                                        algunError = true;
                                        break;
                                    }

                                    ///******ESTADO PERSONALIZADO EN RG*******///
                                    algunError = false;

                                    string IdAdmin = rendicion.Detalle.UAppid.ToString();
                                    string CustomStatus = "PAGADO";
                                    string CustomMessage = string.Empty;
                                    if (forma_pago.Equals("TARJETACREDITO"))
                                    {
                                        CustomMessage = string.Format("Rendición Pagada contra cuenta tarjetas {0}", ctacontable_tarjetas);
                                    }
                                    else if (tipo_fondo_pago.Equals("REEMBOLSO"))
                                    {
                                        CustomMessage = string.Format("Rendición Pagada sin ER en PE-{0}", pago_rendicion);
                                    }
                                    else if (tipo_fondo_pago.Equals("EXCESO"))
                                    {
                                        CustomMessage = string.Format("Rendición Pagada contra {0} y PE-{1}", fondo_rendir, pago_rendicion);
                                    }
                                    else
                                    {
                                        CustomMessage = string.Format("Rendición Pagada contra {0}", fondo_rendir);
                                    }

                                    string[] _args3 = new string[]
                                    {
                                            Id, IdAdmin, CustomStatus, CustomMessage
                                    };

                                    var respEP = interfazRG.CambiarEstadoPersonalizado(_args3);
                                    if (respEP.Success)
                                    {
                                        algunError = false;
                                        Observaciones += string.Format(" - Pagada en Rindegastos");
                                    }
                                    else
                                    {
                                        if (SBO.ConexionSBO.oCompany.InTransaction)
                                        {
                                            SBO.ConexionSBO.oCompany.EndTransaction(BoWfTransOpt.wf_RollBack);
                                        }
                                        Observaciones = string.Format("Rendición: {0} - No se pudo cambiar el estado pagado en Rindegastos.", rendicion.Detalle.URepnum.ToString());
                                        update = SBO.ModeloSBO.UpdateRendicion(rendicion.Detalle.DocEntry.ToString(), 9, Observaciones.PadRight(250).Substring(0, 250).Trim());
                                        algunError = true;
                                        break;
                                    }

                                    ///******CERRAR FONDO A RENDIR EN RG*******///
                                    if (accion_fondo_rendir.Equals("CERRAR") && cerrar_fondo_compensado)
                                    {
                                        algunError = false;

                                        string FundStatus = "2";

                                        string[] _args4 = new string[]
                                        {
                                                    rendicion.Detalle.UFundid.ToString(), IdAdmin, FundStatus
                                        };

                                        var respCF = interfazRG.CambiarEstadoFondo(_args4);
                                        if (respCF.Success)
                                        {
                                            var fondo = interfazRG.Fund;
                                            var nuevo_estado_fondo = fondo.Status;
                                            if (nuevo_estado_fondo.Equals(2))
                                            {
                                                Observaciones += string.Format(" - Fondo a rendir Cerrado en Rindegastos");
                                            }
                                        }
                                        else
                                        {
                                            if (SBO.ConexionSBO.oCompany.InTransaction)
                                            {
                                                SBO.ConexionSBO.oCompany.EndTransaction(BoWfTransOpt.wf_RollBack);
                                            }
                                            Observaciones = string.Format("Rendición: {0} - No se pudo cerrar el fondo a rendir en Rindegastos.", rendicion.Detalle.URepnum.ToString());
                                            update = SBO.ModeloSBO.UpdateRendicion(rendicion.Detalle.DocEntry.ToString(), 9, Observaciones.PadRight(250).Substring(0, 250).Trim());
                                            algunError = true;
                                            break;
                                        }
                                    }

                                    /////////******CERRAR FONDO A RENDIR EN RG*******///
                                    //////algunError = false;

                                    //////string FundStatus = "2";

                                    //////if (!tipo_fondo_pago.Equals("REEMBOLSO"))
                                    //////{
                                    //////    if (localizacion.Equals("FLESAN1"))
                                    //////    {
                                    //////        if (nombre_fondo.Contains("Fondo Fijo"))
                                    //////        {
                                    //////            FundStatus = "1"; //Mantener activo
                                    //////        }
                                    //////    }

                                    //////    if (FundStatus.Equals("2"))
                                    //////    {
                                    //////        string[] _args4 = new string[]
                                    //////        {
                                    //////            Id, IdAdmin, FundStatus
                                    //////        };

                                    //////        var respCF = interfazRG.CambiarEstadoFondo(_args4);
                                    //////        if (respCF.Success)
                                    //////        {
                                    //////            var fondo = interfazRG.Fund;
                                    //////            var nuevo_estado_fondo = fondo.Status;
                                    //////            if (nuevo_estado_fondo.Equals(2))
                                    //////            {
                                    //////                Observaciones += string.Format(" - Fondo a rendir Cerrado en Rindegastos");
                                    //////            }
                                    //////        }
                                    //////        else
                                    //////        {
                                    //////            if (SBO.ConexionSBO.oCompany.InTransaction)
                                    //////            {
                                    //////                SBO.ConexionSBO.oCompany.EndTransaction(BoWfTransOpt.wf_RollBack);
                                    //////            }
                                    //////            Observaciones = string.Format("Rendición: {0} - No se pudo cerrar el fondo a rendir en Rindegastos.", rendicion.Detalle.URepnum.ToString());
                                    //////            update = SBO.ModeloSBO.UpdateRendicion(rendicion.Detalle.DocEntry.ToString(), 9, Observaciones.PadRight(250).Substring(0, 250).Trim());
                                    //////            algunError = true;
                                    //////            break;
                                    //////        }
                                    //////    }
                                    //////}

                                    ///******RENOVAR FONDO A RENDIR EN RG*******///
                                    algunError = false;

                                    if (!tipo_fondo_pago.Equals("REEMBOLSO"))
                                    {
                                        if (localizacion.Equals("FLESAN1") || localizacion.Equals("SOINDUS"))
                                        {
                                            if (nombre_fondo.Contains("Fondo Fijo"))
                                            {
                                                //preliminar
                                                SAPbobsCOM.Payments oPayment = (SAPbobsCOM.Payments)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPaymentsDrafts);
                                                oPayment.DocObjectCode = SAPbobsCOM.BoPaymentsObjectType.bopot_OutgoingPayments;
                                                oPayment.DocType = SAPbobsCOM.BoRcptTypes.rCustomer;
                                                memo = string.Empty;
                                                memo = "Renovación Fondo a rendir rendición N°" + rendicion.Detalle.URepnum.ToString() + " " + rendicion.Detalle.UEmpname;
                                                memo = memo.PadRight(250).Substring(0, 250).Trim();
                                                oPayment.Remarks = memo;
                                                oPayment.CardCode = _CardCodeRendidor;
                                                oPayment.DocDate = Convert.ToDateTime(rendicion.Detalle.UCdate.ToString("yyyy-MM-dd"));
                                                oPayment.TaxDate = Convert.ToDateTime(rendicion.Detalle.UCdate.ToString("yyyy-MM-dd"));
                                                string monedaPago = SBO.ConsultasSBO.ObtenerCodigoMonedaISO(rendicion.Detalle.UCur);
                                                oPayment.LocalCurrency = SAPbobsCOM.BoYesNoEnum.tNO;
                                                oPayment.DocCurrency = monedaPago;
                                                oPayment.CashSum = monto_renov_fondo;
                                                oPayment.ControlAccount = ctacontable_fondo;

                                                string numER = DateTime.Now.Ticks.ToString();
                                                if (localizacion.Equals("FLESAN1"))
                                                {
                                                    //Tipo operación de pago
                                                    oPayment.UserFields.Fields.Item("U_SYP_TPOOPER").Value = "ER01";
                                                    //Numero ER
                                                    //oPayment.UserFields.Fields.Item("U_SYP_NUMER").Value = string.Format("ER{0}", numER);
                                                    oPayment.UserFields.Fields.Item("U_SYP_NUMER").Value = fondo_rendir;
                                                    //Estado cajachica/ER
                                                    oPayment.UserFields.Fields.Item("U_SYP_CCERSTATUS").Value = "O";
                                                    //Serie ER
                                                    //oPayment.UserFields.Fields.Item("U_SYP_SERER").Value = numER;
                                                    oPayment.UserFields.Fields.Item("U_SYP_SERER").Value = fondo_rendir.Substring(2, fondo_rendir.Length - 2);
                                                }
                                                if (registrar_campos_soindus)
                                                {
                                                    //Numero ER
                                                    oPayment.UserFields.Fields.Item("U_SO_NUMER").Value = fondo_rendir;
                                                }

                                                errCode = 0;
                                                errMsg = "";
                                                int retPago = oPayment.Add();

                                                if (retPago != 0)
                                                {
                                                    SBO.ConexionSBO.oCompany.GetLastError(out errCode, out errMsg);
                                                    if (SBO.ConexionSBO.oCompany.InTransaction)
                                                    {
                                                        SBO.ConexionSBO.oCompany.EndTransaction(BoWfTransOpt.wf_RollBack);
                                                    }
                                                    Observaciones = string.Format("Pago Efectuado Renovación Fondo a rendir Rendición: {0} - {1}", rendicion.Detalle.URepnum, errMsg);
                                                    update = SBO.ModeloSBO.UpdateRendicion(rendicion.Detalle.DocEntry.ToString(), 9, Observaciones.PadRight(250).Substring(0, 250).Trim());
                                                    algunError = true;
                                                    break;
                                                }
                                                else
                                                {
                                                    string pagoefectuado = SBO.ConexionSBO.oCompany.GetNewObjectKey();
                                                    Observaciones += string.Format(" - Pago Efectuado: {0}", pagoefectuado);

                                                    /////////******MODIFICAR FONDO A RENDIR EN RG*******///
                                                    //////algunError = false;

                                                    //////Id = rendicion.Detalle.UFundid.ToString();
                                                    //////IdAdmin = rendicion.Detalle.UAppid.ToString();
                                                    //////string FundName = nombre_fondo;
                                                    //////string FundCode = string.Format("ER{0};{1}", numER, ctacontable_fondo);
                                                    //////string FundComment = string.Format("ER{0};{1}", numER, ctacontable_fondo);
                                                    //////string FundFlexibility = "false";
                                                    //////string FundAutoDeposit = "false";
                                                    //////string FundAutoBlock = "false";
                                                    //////string FundExpiration = "true";
                                                    //////string FundExpirationDate = DateTime.Now.AddMonths(1).ToString("yyyy-MM-dd");
                                                    //////if (localizacion.Equals("FLESAN1"))
                                                    //////{
                                                    //////    FundFlexibility = "true";
                                                    //////    if (nombre_fondo.Contains("Fondo Fijo"))
                                                    //////    {
                                                    //////        FundAutoDeposit = "true";
                                                    //////    }
                                                    //////    FundExpiration = "false";
                                                    //////    FundExpirationDate = string.Format("{0}-{1}-{2}", DateTime.Now.Year, "12", "31");
                                                    //////}

                                                    //////string[] _args = new string[]
                                                    //////{
                                                    //////    Id, IdAdmin, FundName,
                                                    //////    FundCode, FundComment,
                                                    //////    FundFlexibility, FundAutoDeposit, FundAutoBlock,
                                                    //////    FundExpiration, FundExpirationDate
                                                    //////};

                                                    //////var respF = interfazRG.ModificarFondo(_args);
                                                    //////if (respF.Success)
                                                    //////{
                                                    //////    string nuevo_fondo_rendir = string.Empty;
                                                    //////    var fondo = interfazRG.Fund;
                                                    //////    nuevo_fondo_rendir = fondo.Code;
                                                    //////    if (string.IsNullOrEmpty(nuevo_fondo_rendir))
                                                    //////    {
                                                    //////        if (SBO.ConexionSBO.oCompany.InTransaction)
                                                    //////        {
                                                    //////            SBO.ConexionSBO.oCompany.EndTransaction(BoWfTransOpt.wf_RollBack);
                                                    //////        }
                                                    //////        Observaciones = string.Format("Renovación fondo a rendir Rendición {0} - No se pudo validar la renovación del fondo en Rindegastos.", rendicion.Detalle.URepnum.ToString());
                                                    //////        update = SBO.ModeloSBO.UpdateRendicion(rendicion.Detalle.DocEntry.ToString(), 9, Observaciones.PadRight(250).Substring(0, 250).Trim());
                                                    //////        algunError = true;
                                                    //////        break;
                                                    //////    }
                                                    //////    else
                                                    //////    {
                                                    //////        algunError = false;
                                                    //////        Observaciones += string.Format(" - Fondo a rendir actualizado en Rindegastos");
                                                    //////    }
                                                    //////}
                                                    //////else
                                                    //////{
                                                    //////    if (SBO.ConexionSBO.oCompany.InTransaction)
                                                    //////    {
                                                    //////        SBO.ConexionSBO.oCompany.EndTransaction(BoWfTransOpt.wf_RollBack);
                                                    //////    }
                                                    //////    Observaciones = string.Format("Renovación fondo a rendir Rendición: {0} - No se pudo renovar el fondo en Rindegastos.", rendicion.Detalle.URepnum.ToString());
                                                    //////    update = SBO.ModeloSBO.UpdateRendicion(rendicion.Detalle.DocEntry.ToString(), 9, Observaciones.PadRight(250).Substring(0, 250).Trim());
                                                    //////    algunError = true;
                                                    //////    break;
                                                    //////}
                                                }
                                            }
                                        }
                                    }
                                    break;
                                default:
                                    break;
                            }
                            // Fin Switch Tipo de Gestión 
                        }
                        // Fin Validar SN Empleado Rendidor
                    }
                    else
                    {
                        if (rendicion.Detalle.UReptota.Equals(0))
                        {
                            SBO.ConexionSBO.oCompany.GetLastError(out errCode, out errMsg);
                            if (SBO.ConexionSBO.oCompany.InTransaction)
                            {
                                SBO.ConexionSBO.oCompany.EndTransaction(BoWfTransOpt.wf_RollBack);
                            }
                            Observaciones = string.Format("Rendición: {0} - No posee gastos aprobados o fue rechazada.", rendicion.Detalle.URepnum);
                            update = SBO.ModeloSBO.UpdateRendicion(rendicion.Detalle.DocEntry.ToString(), 9, Observaciones.PadRight(250).Substring(0, 250).Trim());
                            algunError = true;
                            break;
                        }
                    }
                    // Fin Validar Es Rendicion Sociedad y Monto Aprobado es Mayor a Cero
                    if (!algunError)
                    {
                        if (SBO.ConexionSBO.oCompany.InTransaction)
                        {
                            SBO.ConexionSBO.oCompany.EndTransaction(BoWfTransOpt.wf_Commit);
                        }
                        update = SBO.ModeloSBO.UpdateRendicion(rendicion.Detalle.DocEntry.ToString(), 1, Observaciones.PadRight(250).Substring(0, 250).Trim());
                    }

                    ////ELIMINAR
                    //return resp;
                    ////ELIMINAR
                }
                // Fin Foreach Rendiciones
            }
            catch (Exception ex)
            {
                if (SBO.ConexionSBO.oCompany.InTransaction)
                {
                    SBO.ConexionSBO.oCompany.EndTransaction(BoWfTransOpt.wf_RollBack);
                }
                resp.Success = false;
                resp.Mensaje = ex.Message;
                Observaciones = string.Format("Rendición N°{0} - ERROR: {1} - Vuelva a procesar el período para continuar con las rendiciones restantes.", gReportNum.ToString(), ex.Message);
                update = SBO.ModeloSBO.UpdateRendicion(gDocEntry.ToString(), 9, Observaciones.PadRight(250).Substring(0, 250).Trim());
            }
            return resp;
        }

        public static Clases.Message ContabilizarDevoluciones(Clases.Rendiciones Rendiciones)
        {
            Clases.Configuracion ExtConf = new Clases.Configuracion();
            //Parametrizables
            string rut_sociedad = ExtConf.Parametros.Rut_Sociedad;
            string localizacion = ExtConf.Parametros.Localizacion;
            string cta_ant_proveedor = ExtConf.Parametros.Cuenta_Anticipo_Proveedores; //"1-1-100-20-000";
            string cta_serv_proveedor = ExtConf.Parametros.Cuenta_Servicios_Proveedores; //"6-1-010-40-000";
            string cta_compra_materiales = ExtConf.Parametros.Cuenta_Compra_Materiales; //"1-1-100-20-000";
            string cta_compensacion_facturas = ExtConf.Parametros.Cuenta_Compensacion_Facturas; //"1-1-100-20-000";
            string cta_pago_reembolsos = ExtConf.Parametros.Cuenta_Pago_Reembolsos; //"1-1-100-20-000";
            string campo_extra_centro_costo_nombre = ExtConf.Parametros.Campo_Centro_Costo; //"Centro de Costo";
            string campo_extra_centro_costo_valor = ExtConf.Parametros.Campo_Centro_Costo_Valor; //"CODE";
            string campo_extra_tipo_documento_nombre = ExtConf.Parametros.Campo_Tipo_Documento; //"Tipo de Documento";
            string campo_extra_tipo_documento_valor = ExtConf.Parametros.Campo_Tipo_Documento_Valor; //"CODE";
            string codigo_factura = ExtConf.Parametros.Codigo_Factura_Afecta; //"01";
            string codigo_factura_ex = ExtConf.Parametros.Codigo_Factura_Exenta; //"02";
            string codigo_factura_mat = ExtConf.Parametros.Codigo_Factura_Materiales; //"04";
            string campo_extra_rut_proveedor_nombre = ExtConf.Parametros.Campo_Rut_Proveedor; //"RUC Proveedor";
            string campo_extra_rut_proveedor_valor = ExtConf.Parametros.Campo_Rut_Proveedor_Valor; //"VALUE";
            string campo_extra_numero_documento_nombre = ExtConf.Parametros.Campo_Numero_Documento; //"Número de Documento";
            string campo_extra_numero_documento_valor = ExtConf.Parametros.Campo_Numero_Documento_Valor; //"VALUE";
            //Fin Parametrizables
            string cta_gasto = "6-1-020-10-000"; // Sólo Pruebas
            string tipo_gasto = "Gasto";
            string rut_proveedor = string.Empty;
            string centro_costo = string.Empty;
            string dimension = string.Empty;
            string centro_costo_dim1 = string.Empty;
            string centro_costo_dim3 = string.Empty;
            string numero_documento = string.Empty;
            string Observaciones = string.Empty;
            string nombre_fondo = string.Empty;
            string ctacontable_fondo = string.Empty;
            string fondo_rendir = string.Empty;
            double monto_tot_fondo = 0;
            double monto_saldo_fondo = 0;
            double monto_renov_fondo = 0;
            string tipo_fondo_pago = string.Empty;
            string accion_fondo_rendir = string.Empty;
            double monto_reembolso = 0;
            string asiento_integracion = string.Empty;
            string pago_rendicion = string.Empty;
            string forma_pago = string.Empty;
            string ctacontable_tarjetas = string.Empty;
            string proyecto = string.Empty;
            //Logicos Localización
            bool borrador = false;
            bool borrador_1er_asiento = false;
            bool borrador_factura = false;
            bool borrador_compensa_factura_pagoef = false;
            bool borrador_compensa_factura_asiento = false;
            bool borrador_pago_a_favor = false;
            bool cuenta_en_gastos = false;
            bool registrar_campos_soindus = false;
            bool compensar_factura_con_pago = false;
            bool registrar_folio = false;
            bool registrar_indicador = false;
            bool pago_con_cta_reembolso = false;
            bool cerrar_fondo_compensado = false;
            switch (localizacion)
            {
                case "SOINDUS":
                    borrador = false;
                    borrador_1er_asiento = false;
                    borrador_factura = false;
                    borrador_compensa_factura_pagoef = false;
                    borrador_compensa_factura_asiento = false;
                    borrador_pago_a_favor = false;
                    cuenta_en_gastos = true;
                    registrar_campos_soindus = true;
                    compensar_factura_con_pago = true;
                    registrar_folio = true;
                    registrar_indicador = true;
                    pago_con_cta_reembolso = true;
                    cerrar_fondo_compensado = true;
                    break;
                case "FLESAN1":
                    borrador = true;
                    borrador_1er_asiento = true;
                    borrador_factura = true;
                    borrador_compensa_factura_pagoef = true;
                    borrador_compensa_factura_asiento = true;
                    borrador_pago_a_favor = true;
                    cuenta_en_gastos = true;
                    registrar_campos_soindus = false;
                    compensar_factura_con_pago = false;
                    registrar_folio = false;
                    registrar_indicador = false;
                    pago_con_cta_reembolso = false;
                    cerrar_fondo_compensado = false;
                    break;
                case "ICLINICS":
                    borrador = false;
                    borrador_1er_asiento = false;
                    borrador_factura = false;
                    borrador_compensa_factura_pagoef = false;
                    borrador_compensa_factura_asiento = false;
                    borrador_pago_a_favor = false;
                    cuenta_en_gastos = true;
                    registrar_campos_soindus = true;
                    compensar_factura_con_pago = true;
                    registrar_folio = true;
                    registrar_indicador = true;
                    pago_con_cta_reembolso = true;
                    cerrar_fondo_compensado = false;
                    break;
                case "PCLINICS":
                    borrador = false;
                    borrador_1er_asiento = false;
                    borrador_factura = false;
                    borrador_compensa_factura_pagoef = false;
                    borrador_compensa_factura_asiento = false;
                    borrador_pago_a_favor = false;
                    cuenta_en_gastos = true;
                    registrar_campos_soindus = true;
                    compensar_factura_con_pago = true;
                    registrar_folio = true;
                    registrar_indicador = true;
                    pago_con_cta_reembolso = true;
                    cerrar_fondo_compensado = false;
                    break;
                case "LATINEQ":
                    borrador = false;
                    borrador_1er_asiento = false;
                    borrador_factura = false;
                    borrador_compensa_factura_pagoef = false;
                    borrador_compensa_factura_asiento = false;
                    borrador_pago_a_favor = false;
                    cuenta_en_gastos = true;
                    registrar_campos_soindus = true;
                    compensar_factura_con_pago = true;
                    registrar_folio = true;
                    registrar_indicador = true;
                    pago_con_cta_reembolso = true;
                    cerrar_fondo_compensado = true;
                    break;
                case "LATINEUY":
                    borrador = false;
                    borrador_1er_asiento = false;
                    borrador_factura = false;
                    borrador_compensa_factura_pagoef = false;
                    borrador_compensa_factura_asiento = false;
                    borrador_pago_a_favor = false;
                    cuenta_en_gastos = true;
                    registrar_campos_soindus = true;
                    compensar_factura_con_pago = true;
                    registrar_folio = true;
                    registrar_indicador = true;
                    pago_con_cta_reembolso = true;
                    cerrar_fondo_compensado = true;
                    break;
                case "SUATRANS":
                    borrador = false;
                    borrador_1er_asiento = false;
                    borrador_factura = false;
                    borrador_compensa_factura_pagoef = false;
                    borrador_compensa_factura_asiento = false;
                    borrador_pago_a_favor = false;
                    cuenta_en_gastos = true;
                    registrar_campos_soindus = true;
                    compensar_factura_con_pago = true;
                    registrar_folio = true;
                    registrar_indicador = true;
                    pago_con_cta_reembolso = true;
                    cerrar_fondo_compensado = false;
                    break;
                case "SUATRANSTR":
                    borrador = false;
                    borrador_1er_asiento = false;
                    borrador_factura = false;
                    borrador_compensa_factura_pagoef = false;
                    borrador_compensa_factura_asiento = false;
                    borrador_pago_a_favor = false;
                    cuenta_en_gastos = true;
                    registrar_campos_soindus = true;
                    compensar_factura_con_pago = true;
                    registrar_folio = true;
                    registrar_indicador = true;
                    pago_con_cta_reembolso = true;
                    cerrar_fondo_compensado = false;
                    break;
                case "SUATRANSPE":
                    borrador = false;
                    borrador_1er_asiento = false;
                    borrador_factura = false;
                    borrador_compensa_factura_pagoef = false;
                    borrador_compensa_factura_asiento = false;
                    borrador_pago_a_favor = false;
                    cuenta_en_gastos = true;
                    registrar_campos_soindus = true;
                    compensar_factura_con_pago = true;
                    registrar_folio = true;
                    registrar_indicador = true;
                    pago_con_cta_reembolso = true;
                    cerrar_fondo_compensado = false;
                    break;
                default:
                    borrador = true;
                    borrador_1er_asiento = true;
                    borrador_factura = true;
                    borrador_compensa_factura_pagoef = true;
                    borrador_compensa_factura_asiento = true;
                    borrador_pago_a_favor = true;
                    cuenta_en_gastos = true;
                    registrar_campos_soindus = true;
                    compensar_factura_con_pago = true;
                    registrar_folio = true;
                    registrar_indicador = true;
                    pago_con_cta_reembolso = true;
                    cerrar_fondo_compensado = true;
                    break;
            }

            int errCode = 0;
            string errMsg = string.Empty;
            int ret = 0;
            int retDoc = 0;
            Clases.Message resp = new Clases.Message();
            Clases.Message update = new Clases.Message();
            string monedalocal = SBO.ConsultasSBO.ObtenerMonedaLocal();

            string memo = string.Empty;
            double totGastos = 0;
            int linea = 0;
            string _CardCodeRendidor = string.Empty;

            InterfazRG interfazRG = new InterfazRG();

            long gReportNum = 0;
            long gDocEntry = 0;
            try
            {
                foreach (var rendicion in Rendiciones.Items)
                {
                    bool algunError = false;
                    gReportNum = rendicion.Detalle.URepnum;
                    gDocEntry = rendicion.Detalle.DocEntry;
                    if (EsRendicionSociedad(rut_sociedad, rendicion) && rendicion.Detalle.UReptota > 0)
                    {
                        if (localizacion.Equals("ICLINICS"))
                        {
                            _CardCodeRendidor = SBO.ConsultasSBO.ObtenerCardCodeClienteConLetra(rendicion.Detalle.UEmpide, "R");
                        }
                        else if (localizacion.Equals("PCLINICS"))
                        {
                            _CardCodeRendidor = SBO.ConsultasSBO.ObtenerCardCodeClienteConLetra(rendicion.Detalle.UEmpide, "R");
                        }
                        else if (localizacion.Equals("LATINEQ"))
                        {
                            _CardCodeRendidor = SBO.ConsultasSBO.ObtenerCardCodeProveedor(rendicion.Detalle.UEmpide);
                        }
                        else if (localizacion.Equals("LATINEUY"))
                        {
                            _CardCodeRendidor = SBO.ConsultasSBO.ObtenerCardCodeProveedor(rendicion.Detalle.UEmpide);
                        }
                        else if (localizacion.Equals("SUATRANS"))
                        {
                            _CardCodeRendidor = SBO.ConsultasSBO.ObtenerCardCodeProveedor(rendicion.Detalle.UEmpide);
                        }
                        else if (localizacion.Equals("SUATRANSTR"))
                        {
                            _CardCodeRendidor = SBO.ConsultasSBO.ObtenerCardCodeCliente(rendicion.Detalle.UEmpide);
                        }
                        else if (localizacion.Equals("SUATRANSPE"))
                        {
                            _CardCodeRendidor = SBO.ConsultasSBO.ObtenerCardCodeProveedorConLetra(rendicion.Detalle.UEmpide, "E");
                        }
                        else
                        {
                            _CardCodeRendidor = SBO.ConsultasSBO.ObtenerCardCode(rendicion.Detalle.UEmpide);
                        }
                        // Validar Existencia del Detalle de Gastos
                        if (rendicion.Detalle.Gastos == null || rendicion.Detalle.Gastos.Items.Count() <= 0)
                        {
                            if (SBO.ConexionSBO.oCompany.InTransaction)
                            {
                                SBO.ConexionSBO.oCompany.EndTransaction(BoWfTransOpt.wf_RollBack);
                            }
                            Observaciones = string.Format("Devolución: {0} - No existe detalle de gastos.", rendicion.Detalle.URepnum);
                            update = SBO.ModeloSBO.UpdateRendicion(rendicion.Detalle.DocEntry.ToString(), 9, Observaciones.PadRight(250).Substring(0, 250).Trim());
                            algunError = true;
                            continue;
                        }
                        // Validar SN Empleado Rendidor
                        if (string.IsNullOrEmpty(_CardCodeRendidor))
                        {
                            if (SBO.ConexionSBO.oCompany.InTransaction)
                            {
                                SBO.ConexionSBO.oCompany.EndTransaction(BoWfTransOpt.wf_RollBack);
                            }
                            Observaciones = string.Format("Asiento Devolución: {0} - No existe socio de negocios {1}.", rendicion.Detalle.URepnum, rendicion.Detalle.UEmpide);
                            update = SBO.ModeloSBO.UpdateRendicion(rendicion.Detalle.DocEntry.ToString(), 9, Observaciones.PadRight(250).Substring(0, 250).Trim());
                            algunError = true;
                            continue;
                        }
                        else
                        {
                            switch (TipoGestion(rendicion))
                            {
                                case "DEVOLUCIÓN":
                                    Observaciones = string.Empty;
                                    //VALIDACION FONDO A RENDIR
                                    fondo_rendir = string.Empty;
                                    monto_tot_fondo = 0;
                                    monto_saldo_fondo = 0;
                                    monto_renov_fondo = 0;
                                    ctacontable_fondo = string.Empty;
                                    tipo_fondo_pago = string.Empty;
                                    monto_reembolso = 0;
                                    asiento_integracion = string.Empty;
                                    pago_rendicion = string.Empty;
                                    forma_pago = string.Empty;
                                    ctacontable_tarjetas = string.Empty;
                                    ctacontable_tarjetas = CuentaTarjetaCredito(rendicion);
                                    if (!rendicion.Detalle.UFundid.ToString().Equals("0"))
                                    {
                                        accion_fondo_rendir = "MANTENER";
                                        string[] _args = new string[] { rendicion.Detalle.UFundid.ToString() };
                                        var respF = interfazRG.ObtenerFondo(_args);
                                        if (respF.Success)
                                        {
                                            var fondo = interfazRG.Fund;
                                            try
                                            {
                                                var fondocode = fondo.Code.Split(';');
                                                fondo_rendir = fondocode[0];
                                                ctacontable_fondo = fondocode[1];
                                                monto_tot_fondo = double.Parse(fondocode[2]);
                                            }
                                            catch
                                            {
                                            }
                                            nombre_fondo = fondo.Title;
                                            monto_renov_fondo = monto_tot_fondo;
                                            monto_saldo_fondo = fondo.Balance;
                                            if (string.IsNullOrEmpty(fondo_rendir))
                                            {
                                                if (SBO.ConexionSBO.oCompany.InTransaction)
                                                {
                                                    SBO.ConexionSBO.oCompany.EndTransaction(BoWfTransOpt.wf_RollBack);
                                                }
                                                Observaciones = string.Format("Devolución N°{0} - No posee un Fondo a Rendir válido.", rendicion.Detalle.URepnum.ToString());
                                                update = SBO.ModeloSBO.UpdateRendicion(rendicion.Detalle.DocEntry.ToString(), 9, Observaciones.PadRight(250).Substring(0, 250).Trim());
                                                algunError = true;
                                                break;
                                            }
                                            if (rendicion.Detalle.UReptota < (fondo.Deposits - fondo.Charges) && monto_saldo_fondo > 0)
                                            {
                                                    accion_fondo_rendir = "MANTENER";
                                            }
                                            else
                                            {
                                                accion_fondo_rendir = "CERRAR";
                                            }
                                        }
                                    }

                                    if (localizacion.Equals("SUATRANS") || localizacion.Equals("SUATRANSTR"))
                                    {
                                        proyecto = string.Empty;
                                        foreach (var extrarend in rendicion.Detalle.CamposExtra.Detalle)
                                        {
                                            if (extrarend.UName.Equals("Proyecto"))
                                            {
                                                proyecto = extrarend.UValue;
                                            }
                                        }
                                    }

                                    //***PAGO RECIBIDO POR DEVOLUCION***//
                                    #region PAGO RECIBIDO
                                    SAPbobsCOM.Payments oPayment;
                                    if (borrador)
                                    {
                                        //preliminar
                                        oPayment = (SAPbobsCOM.Payments)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPaymentsDrafts);
                                    }
                                    else
                                    {
                                        //final
                                        oPayment = (SAPbobsCOM.Payments)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oIncomingPayments);
                                    }
                                    oPayment.DocObjectCode = SAPbobsCOM.BoPaymentsObjectType.bopot_IncomingPayments;
                                    if (localizacion.Equals("ICLINICS"))
                                    {
                                        oPayment.DocType = SAPbobsCOM.BoRcptTypes.rCustomer;
                                    }
                                    else if (localizacion.Equals("PCLINICS"))
                                    {
                                        oPayment.DocType = SAPbobsCOM.BoRcptTypes.rCustomer;
                                    }
                                    else if (localizacion.Equals("LATINEQ"))
                                    {
                                        oPayment.DocType = SAPbobsCOM.BoRcptTypes.rSupplier;
                                    }
                                    else if (localizacion.Equals("LATINEUY"))
                                    {
                                        oPayment.DocType = SAPbobsCOM.BoRcptTypes.rSupplier;
                                    }
                                    else if (localizacion.Equals("SUATRANS"))
                                    {
                                        oPayment.DocType = SAPbobsCOM.BoRcptTypes.rSupplier;
                                    }
                                    else if (localizacion.Equals("SUATRANSTR"))
                                    {
                                        oPayment.DocType = SAPbobsCOM.BoRcptTypes.rCustomer;
                                    }
                                    else if (localizacion.Equals("SUATRANSPE"))
                                    {
                                        oPayment.DocType = SAPbobsCOM.BoRcptTypes.rSupplier;
                                    }
                                    else
                                    {
                                        oPayment.DocType = SAPbobsCOM.BoRcptTypes.rCustomer;
                                    }
                                    memo = string.Empty;
                                    memo = "Devolución Fondo a Rendir N°" + rendicion.Detalle.URepnum.ToString() + " " + rendicion.Detalle.UEmpname;
                                    memo = memo.PadRight(250).Substring(0, 250).Trim();
                                    oPayment.Remarks = memo;
                                    if (localizacion.Equals("LATINEQ") || localizacion.Equals("LATINEUY"))
                                    {
                                        memo = memo.PadRight(50).Substring(0, 50).Trim();
                                        oPayment.JournalRemarks = memo;
                                    }
                                    oPayment.CardCode = _CardCodeRendidor;
                                    //oPayment.DocDate = Convert.ToDateTime(rendicion.Detalle.UCdate.ToString("yyyy-MM-dd"));
                                    oPayment.DocDate = DateTime.Now;
                                    oPayment.TaxDate = Convert.ToDateTime(rendicion.Detalle.UCdate.ToString("yyyy-MM-dd"));
                                    string monedaFR = SBO.ConsultasSBO.ObtenerCodigoMonedaISO(rendicion.Detalle.UCur);
                                    oPayment.LocalCurrency = SAPbobsCOM.BoYesNoEnum.tNO;
                                    oPayment.DocCurrency = monedaFR;
                                    oPayment.CashSum = rendicion.Detalle.UReptota;
                                    foreach (var gasto in rendicion.Detalle.Gastos.Items)
                                    {
                                        cta_pago_reembolsos = gasto.Detalle.UCatcode;
                                        //nombre_fondo = gasto.Detalle.UCategory;
                                        break;
                                    }
                                    oPayment.CashAccount = CuentaContable(cta_pago_reembolsos);
                                    oPayment.ControlAccount = CuentaContable(ctacontable_fondo);

                                    string numER = DateTime.Now.Ticks.ToString();
                                    if (registrar_campos_soindus)
                                    {
                                        //Numero ER (entrega a rendir)
                                        oPayment.UserFields.Fields.Item("U_SO_NUMER").Value = fondo_rendir;
                                    }
                                    if (localizacion.Equals("SUATRANS") || localizacion.Equals("SUATRANSTR"))
                                    {
                                        oPayment.ProjectCode = proyecto;
                                    }

                                    errCode = 0;
                                    errMsg = "";
                                    SBO.ConexionSBO.oCompany.StartTransaction();
                                    ret = oPayment.Add();

                                    #endregion PAGO RECIBIDO

                                    if (ret != 0)
                                    {
                                        SBO.ConexionSBO.oCompany.GetLastError(out errCode, out errMsg);
                                        if (SBO.ConexionSBO.oCompany.InTransaction)
                                        {
                                            SBO.ConexionSBO.oCompany.EndTransaction(BoWfTransOpt.wf_RollBack);
                                        }
                                        Observaciones = string.Format("Pago Recibido Devolución: {0} - {1}", rendicion.Detalle.URepnum, errMsg);
                                        update = SBO.ModeloSBO.UpdateRendicion(rendicion.Detalle.DocEntry.ToString(), 9, Observaciones.PadRight(250).Substring(0, 250).Trim());
                                        algunError = true;
                                        break;
                                    }
                                    else
                                    {
                                        string pagorecibido = SBO.ConexionSBO.oCompany.GetNewObjectKey();
                                        Observaciones += string.Format("Pago Recibido: {0}", pagorecibido);

                                        ///******MARCAR DEVOLUCION COMO INTEGRADA EN RG*******///
                                        algunError = false;

                                        string Id = rendicion.Detalle.UId.ToString();
                                        string IdAdmin = rendicion.Detalle.UAppid.ToString();
                                        string IntegrationStatus = "1";
                                        string IntegrationCode = string.Format("PR-{0}", pagorecibido);
                                        string IntegrationDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                                        string[] _args2 = new string[]
                                        {
                                            Id, IntegrationStatus, IntegrationCode, IntegrationDate
                                        };

                                        var respInt = interfazRG.CambiarEstadoRendicion(_args2);
                                        if (respInt.Success)
                                        {
                                            algunError = false;
                                            Observaciones += string.Format(" - Integrada en Rindegastos");
                                        }
                                        else
                                        {
                                            if (SBO.ConexionSBO.oCompany.InTransaction)
                                            {
                                                SBO.ConexionSBO.oCompany.EndTransaction(BoWfTransOpt.wf_RollBack);
                                            }
                                            Observaciones = string.Format("Devolución: {0} - No se pudo cambiar el estado integrado en Rindegastos.", rendicion.Detalle.URepnum.ToString());
                                            update = SBO.ModeloSBO.UpdateRendicion(rendicion.Detalle.DocEntry.ToString(), 9, Observaciones.PadRight(250).Substring(0, 250).Trim());
                                            algunError = true;
                                            break;
                                        }
                                        ///******ESTADO PERSONALIZADO EN RG*******///
                                        algunError = false;

                                        string CustomStatus = "CONTABILIZADO";
                                        string CustomMessage = string.Format("Devolución Fondo a Rendir contabilizado en PE-{0}", pagorecibido);

                                        string[] _args3 = new string[]
                                        {
                                            Id, IdAdmin, CustomStatus, CustomMessage
                                        };

                                        var respEP = interfazRG.CambiarEstadoPersonalizado(_args3);
                                        if (respEP.Success)
                                        {
                                            algunError = false;
                                            Observaciones += string.Format(" - Contabilizada en Rindegastos");
                                        }
                                        else
                                        {
                                            if (SBO.ConexionSBO.oCompany.InTransaction)
                                            {
                                                SBO.ConexionSBO.oCompany.EndTransaction(BoWfTransOpt.wf_RollBack);
                                            }
                                            Observaciones = string.Format("Devolución: {0} - No se pudo cambiar el estado contabilizado en Rindegastos.", rendicion.Detalle.URepnum.ToString());
                                            update = SBO.ModeloSBO.UpdateRendicion(rendicion.Detalle.DocEntry.ToString(), 9, Observaciones.PadRight(250).Substring(0, 250).Trim());
                                            algunError = true;
                                            break;
                                        }

                                        ///******CERRAR FONDO A RENDIR EN RG*******///
                                        if (accion_fondo_rendir.Equals("CERRAR"))
                                        {
                                            algunError = false;

                                            string FundStatus = "2";

                                            string[] _args4 = new string[]
                                            {
                                                    rendicion.Detalle.UFundid.ToString(), IdAdmin, FundStatus
                                            };

                                            var respCF = interfazRG.CambiarEstadoFondo(_args4);
                                            if (respCF.Success)
                                            {
                                                var fondo = interfazRG.Fund;
                                                var nuevo_estado_fondo = fondo.Status;
                                                if (nuevo_estado_fondo.Equals(2))
                                                {
                                                    Observaciones += string.Format(" - Fondo a rendir Cerrado en Rindegastos");
                                                }
                                            }
                                            else
                                            {
                                                if (SBO.ConexionSBO.oCompany.InTransaction)
                                                {
                                                    SBO.ConexionSBO.oCompany.EndTransaction(BoWfTransOpt.wf_RollBack);
                                                }
                                                Observaciones = string.Format("Devolución: {0} - No se pudo cerrar el fondo a rendir en Rindegastos.", rendicion.Detalle.URepnum.ToString());
                                                update = SBO.ModeloSBO.UpdateRendicion(rendicion.Detalle.DocEntry.ToString(), 9, Observaciones.PadRight(250).Substring(0, 250).Trim());
                                                algunError = true;
                                                break;
                                            }
                                        }
                                    }
                                    break;
                                default:
                                    break;
                            }
                            // Fin Switch Tipo de Gestión 
                        }
                        // Fin Validar SN Empleado Rendidor
                    }
                    else
                    {
                        if (rendicion.Detalle.UReptota.Equals(0))
                        {
                            SBO.ConexionSBO.oCompany.GetLastError(out errCode, out errMsg);
                            if (SBO.ConexionSBO.oCompany.InTransaction)
                            {
                                SBO.ConexionSBO.oCompany.EndTransaction(BoWfTransOpt.wf_RollBack);
                            }
                            Observaciones = string.Format("Devolución: {0} - No posee gastos aprobados o fue rechazada.", rendicion.Detalle.URepnum);
                            update = SBO.ModeloSBO.UpdateRendicion(rendicion.Detalle.DocEntry.ToString(), 9, Observaciones.PadRight(250).Substring(0, 250).Trim());
                            algunError = true;
                            break;
                        }
                    }
                    // Fin Validar Es Rendicion Sociedad y Monto Aprobado es Mayor a Cero
                    if (!algunError)
                    {
                        if (SBO.ConexionSBO.oCompany.InTransaction)
                        {
                            SBO.ConexionSBO.oCompany.EndTransaction(BoWfTransOpt.wf_Commit);
                        }
                        update = SBO.ModeloSBO.UpdateRendicion(rendicion.Detalle.DocEntry.ToString(), 1, Observaciones.PadRight(250).Substring(0, 250).Trim());
                    }

                    ////ELIMINAR
                    //return resp;
                    ////ELIMINAR
                }
                // Fin Foreach Rendiciones
            }
            catch (Exception ex)
            {
                if (SBO.ConexionSBO.oCompany.InTransaction)
                {
                    SBO.ConexionSBO.oCompany.EndTransaction(BoWfTransOpt.wf_RollBack);
                }
                resp.Success = false;
                resp.Mensaje = ex.Message;
                Observaciones = string.Format("Devolución N°{0} - ERROR: {1} - Vuelva a procesar el período para continuar con las devoluciones restantes.", gReportNum.ToString(), ex.Message);
                update = SBO.ModeloSBO.UpdateRendicion(gDocEntry.ToString(), 9, Observaciones.PadRight(250).Substring(0, 250).Trim());
            }
            return resp;
        }

        private static bool EsRendicionSociedad(string sociedad, Clases.Rendicion rendicion)
        {
            bool ret = false;
            foreach (var item in rendicion.Detalle.CamposExtra.Detalle)
            {
                if (item.UName.Equals("Empresa") && item.UCode.Equals(sociedad))
                {
                    return true;
                }
            }
            return ret;
        }

        private static string TipoGestion(Clases.Rendicion rendicion)
        {
            string ret = string.Empty;
            if (rendicion.Detalle.UGestion.ToUpper().Equals("SOLICITUD"))
            {
                ret = "SOLICITUD";
            }
            else if (rendicion.Detalle.UGestion.ToUpper().Equals("RENDICIÓN"))
            {
                ret = "RENDICIÓN";
            }
            else if (rendicion.Detalle.UGestion.ToUpper().Equals("DEVOLUCIÓN"))
            {
                ret = "DEVOLUCIÓN";
            }
            return ret;
        }

        private static string CuentaTarjetaCredito(Clases.Rendicion rendicion)
        {
            string ret = string.Empty;
            foreach (var item in rendicion.Detalle.CamposExtra.Detalle)
            {
                if (item.UName.Equals("Forma de Pago") && item.UValue.Equals("Tarjeta de Crédito"))
                {
                    ret = item.UCode;
                    return ret;
                }
            }
            return ret;
        }

        private static string CuentaContable(string CtaContable)
        {
            bool ctassegmentadas = SBO.ConsultasSBO.PlanDeCuentasSegmentado();
            if (ctassegmentadas)
            {
                return SBO.ConsultasSBO.ObtenerCuentaSys(CtaContable);
            }
            else
            {
                return CtaContable;
            }
        }
    }
}
