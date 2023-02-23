using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soindus.AddOnEmisionDTE.DBNetEC
{
    public class XML_FacturaEx
    {
        public putCustomerETDLoad Generar(int DocEntry)
        {
            putCustomerETDLoad emision = null;
            string query = string.Empty;
            SAPbobsCOM.Recordset oRec = null;
            SAPbobsCOM.Recordset oRec2 = null;
            SAPbobsCOM.Recordset oRecG = null;
            SAPbobsCOM.Recordset oRecG2 = null;
            oRec = (SAPbobsCOM.Recordset)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            oRec2 = (SAPbobsCOM.Recordset)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            oRecG = (SAPbobsCOM.Recordset)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            oRecG2 = (SAPbobsCOM.Recordset)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

            query = @"Select * from ""OADM"" ""T0"" INNER JOIN ""ADM1"" ""T1"" ON 1 = 1";
            oRec2.DoQuery(query);

            query = @"Select ""Indicator"", ""DocNum"", ""DocDate"", ""TaxDate"", ""DocTotal"",
                            ""DocRate"", ""DocDueDate"", ""SlpName"", ""GroupNum"", ""NumAtCard"", ""Comments"",
                            ""U_VK_Met_Pago"", ""U_VK_OrderCompra"", ""U_VK_FechaRef"", ""U_VK_HES"", ""U_VK_NumContrato"",
                            ""FolioNum"", ""DocSubType"", ""CardCode"",
                            case when ""DocRate"" <= 1 then 0 else ""DocTotal"" end as ""Exento"",
                            case when ""DocRate"" <= 1 then ""VatSum"" else ""VatSumSy"" end as ""Iva"",
                            ""DocEntry"", 
                            case when ""DocRate"" <= 1 then (ROUND(""DocTotal"", 0) - ROUND(""VatSum"", 0)) else (ROUND(""DocTotalSy"", 0) - ROUND(""VatSumSy"", 0)) end as ""Neto"", 
                            case when ""DocRate"" <= 1 then ""DiscSum"" else ""DiscSumSy"" end as ""Descuento"", 
                            ""PayToCode"", ""ShipToCode""
                            
                            from ""OINV"" INNER JOIN ""OSLP"" ON ""OINV"".""SlpCode"" = ""OSLP"".""SlpCode""
                            where ""DocEntry"" = '" + DocEntry + @"' and ""DocSubType"" = 'IE'";

            oRec.DoQuery(query);

            if (!oRec.EoF)
            {
                emision = new DBNetEC.putCustomerETDLoad();
                //query = @"Select * from ""OUSR"" where ""USER_CODE"" = '" + SBO.ConexionSBO.oCompany.UserName + "'";
                //oRecG.DoQuery(query);
                //emision.Extras.ValoresLibres.ImprDestino = oRecG.Fields.Item("U_SO_PRINTER").Value.ToString();
                query = @"Select * from ""@SO_PARAMSFE"" where ""Code"" = 'GIRO'";
                oRecG.DoQuery(query);
                emision.Extras.ValoresLibres.ValorLibre2 = oRecG.Fields.Item("U_DESCRIPCION").Value.ToString().PadLeft(80, ' ').Substring(0, 80).Trim();
                emision.Extras.ValoresLibres.ValorLibre5 = oRec.Fields.Item("U_VK_Met_Pago").Value.ToString();
                query = @"Select * from ""OCRD"" where ""CardCode"" = '" + oRec.Fields.Item("CardCode").Value.ToString() + "'";
                oRecG.DoQuery(query);
                //emision.Extras.EnvioPdf.CamposEnvioPdf.Add(new CamposEnvioPdf() { nroLinea = "1", mailEnvio = oRecG.Fields.Item("E_Mail").Value.ToString() });
                EnvioPdf envioPDF = new EnvioPdf();
                envioPDF.CamposEnvioPdf.Add(new CamposEnvioPdf() { nroLinea = "1", mailEnvio = oRecG.Fields.Item("E_Mail").Value.ToString() });
                emision.Extras.EnvioPdf.EnvioPdf.Add(envioPDF);
                emision.Encabezado.camposEncabezado.Version = "1.0";
                emision.Encabezado.camposEncabezado.TipoDTE = "34";
                emision.Encabezado.camposEncabezado.Folio = oRec.Fields.Item("DocNum").Value.ToString();
                emision.Encabezado.camposEncabezado.FchEmis = DateTime.Parse(oRec.Fields.Item("DocDate").Value.ToString()).ToString("yyyy-MM-dd");
                emision.Encabezado.camposEncabezado.FchVenc = DateTime.Parse(oRec.Fields.Item("DocDueDate").Value.ToString()).ToString("yyyy-MM-dd");
                query = @"Select * from ""@SO_FORMAPAGO"" where ""Code"" = '" + oRec.Fields.Item("GroupNum").Value.ToString() + "'";
                oRecG.DoQuery(query);
                emision.Encabezado.camposEncabezado.FmaPago = oRecG.Fields.Item("U_FmaPago").Value.ToString();
                emision.Encabezado.camposEncabezado.RUTEmisor = oRec2.Fields.Item("TaxIdNum").Value.ToString();
                emision.Encabezado.camposEncabezado.RznSoc = oRec2.Fields.Item("CompnyName").Value.ToString().PadLeft(100, ' ').Substring(0, 100).Trim();
                query = @"Select * from ""@SO_PARAMSFE"" where ""Code"" = 'GIRO'";
                oRecG.DoQuery(query);
                emision.Encabezado.camposEncabezado.GiroEmis = oRecG.Fields.Item("U_DESCRIPCION").Value.ToString().PadLeft(80, ' ').Substring(0, 80).Trim();
                emision.Encabezado.camposEncabezado.DirOrigen = string.Format("{0} {1}", oRec2.Fields.Item("Street").Value.ToString(), oRec2.Fields.Item("StreetNo").Value.ToString()).PadLeft(60, ' ').Substring(0, 60).Trim();
                emision.Encabezado.camposEncabezado.CmnaOrigen = oRec2.Fields.Item("County").Value.ToString().PadLeft(20, ' ').Substring(0, 20).Trim();
                emision.Encabezado.camposEncabezado.CiudadOrigen = oRec2.Fields.Item("City").Value.ToString().PadLeft(20, ' ').Substring(0, 20).Trim();
                emision.Encabezado.camposEncabezado.Telefono = oRec2.Fields.Item("Phone1").Value.ToString();
                //emision.Encabezado.camposEncabezado.CdgVendedor = oRec.Fields.Item("SlpName").Value.ToString().PadLeft(60, ' ').Substring(0, 60).Trim();
                query = @"Select * from ""OCRD"" where ""CardCode"" = '" + oRec.Fields.Item("CardCode").Value.ToString() + "'";
                oRecG.DoQuery(query);
                emision.Encabezado.camposEncabezado.RUTRecep = oRecG.Fields.Item("LicTradNum").Value.ToString();
                emision.Encabezado.camposEncabezado.CdgIntRecep = oRec.Fields.Item("CardCode").Value.ToString();
                emision.Encabezado.camposEncabezado.RznSocRecep = oRecG.Fields.Item("CardName").Value.ToString().PadLeft(100, ' ').Substring(0, 100).Trim();
                emision.Encabezado.camposEncabezado.GiroRecep = oRecG.Fields.Item("Notes").Value.ToString().PadLeft(40, ' ').Substring(0, 40).Trim();
                emision.Encabezado.camposEncabezado.Telefono = oRecG.Fields.Item("Phone1").Value.ToString().PadLeft(80, ' ').Substring(0, 80).Trim();
                query = @"Select * from ""CRD1"" where ""Address"" = '" + oRec.Fields.Item("PayToCode").Value.ToString() + @"' and ""CardCode"" = '" + oRec.Fields.Item("CardCode").Value.ToString() + "'";
                oRecG.DoQuery(query);
                emision.Encabezado.camposEncabezado.DirRecep = oRecG.Fields.Item("Street").Value.ToString().PadLeft(70, ' ').Substring(0, 70).Trim();
                emision.Encabezado.camposEncabezado.CmnaRecep = oRecG.Fields.Item("County").Value.ToString().PadLeft(20, ' ').Substring(0, 20).Trim();
                emision.Encabezado.camposEncabezado.CiudadRecep = oRecG.Fields.Item("City").Value.ToString().PadLeft(20, ' ').Substring(0, 20).Trim();
                query = @"Select * from ""CRD1"" where ""Address"" = '" + oRec.Fields.Item("ShipToCode").Value.ToString() + @"' and ""CardCode"" = '" + oRec.Fields.Item("CardCode").Value.ToString() + "'";
                oRecG.DoQuery(query);
                emision.Encabezado.camposEncabezado.DirDest = oRecG.Fields.Item("Street").Value.ToString().PadLeft(60, ' ').Substring(0, 60).Trim();
                emision.Encabezado.camposEncabezado.CmnaDest = oRecG.Fields.Item("County").Value.ToString().PadLeft(20, ' ').Substring(0, 20).Trim();
                emision.Encabezado.camposEncabezado.CiudadDest = oRecG.Fields.Item("City").Value.ToString().PadLeft(20, ' ').Substring(0, 20).Trim();

                //emision.Encabezado.camposEncabezado.MntNeto = oRec.Fields.Item("Neto").Value.ToString().Replace(",", ".");
                emision.Encabezado.camposEncabezado.MntExe = oRec.Fields.Item("DocTotal").Value.ToString().Replace(",", ".");
                //emision.Encabezado.camposEncabezado.IVA = oRec.Fields.Item("Iva").Value.ToString().Replace(",", ".");

                query = @"Select top 1 * from ""INV1"" where ""DocEntry"" = " + DocEntry + "";
                oRecG.DoQuery(query);
                query = @"Select * from ""OSTA"" where ""Code"" = '" + oRecG.Fields.Item("TaxCode").Value.ToString() + "'";
                oRecG2.DoQuery(query);
                emision.Encabezado.camposEncabezado.TasaIVA = oRecG2.Fields.Item("Rate").Value.ToString().Replace(",", ".");
                emision.Encabezado.camposEncabezado.MntTotal = oRec.Fields.Item("DocTotal").Value.ToString().Replace(",", ".");
                query = @"Select * from ""OCTG"" where ""GroupNum"" = '" + oRec.Fields.Item("GroupNum").Value.ToString() + "'";
                oRecG.DoQuery(query);
                emision.Encabezado.camposEncabezado.GlosaPagos = oRecG.Fields.Item("PymntGroup").Value.ToString().PadLeft(100, ' ').Substring(0, 100).Trim();

                // Actividad económica
                ActivEcon actividades = new DBNetEC.ActivEcon();
                query = @"Select * from ""@SO_ACTECONFE""";
                oRecG.DoQuery(query);
                while (!oRecG.EoF)
                {
                    actividades.ActividadEconomica.Add(new DBNetEC.ActividadEconomica() { ActivEcon = oRecG.Fields.Item("Code").Value.ToString() });
                    oRecG.MoveNext();
                }
                emision.Encabezado.ActivEcon = actividades;

                //Detalle
                query = @"Select *, (""DiscPrcnt"" * ""PriceBefDi"" * ""Quantity"" /100) as ""MtoDescuento"" 
                                from ""INV1"" where ""DocEntry"" = " + DocEntry + "";
                oRecG.DoQuery(query);
                int count = 1;
                while (!oRecG.EoF)
                {
                    Detalle detalle = new Detalle();
                    detalle.Detalles.NroLinDet = count.ToString();
                    detalle.Detalles.NmbItem = oRecG.Fields.Item("Dscription").Value.ToString();
                    detalle.Detalles.DscItem = oRecG.Fields.Item("FreeTxt").Value.ToString();
                    detalle.Detalles.QtyItem = oRecG.Fields.Item("Quantity").Value.ToString().Replace(",", ".");
                    detalle.Detalles.PrcItem = oRecG.Fields.Item("Price").Value.ToString().Replace(",", ".");
                    detalle.Detalles.DescuentoPct = oRecG.Fields.Item("DiscPrcnt").Value.ToString().Replace(",", ".");
                    detalle.Detalles.DescuentoMonto = oRecG.Fields.Item("MtoDescuento").Value.ToString().Replace(",", ".");
                    detalle.Detalles.MontoItem = oRecG.Fields.Item("LineTotal").Value.ToString().Replace(",", ".");

                    CodItems codItems = null;
                    CodItemsb codItemsb = null;
                    codItemsb = new CodItemsb();
                    codItems = new CodItems();
                    codItems.TpoCodigo = "INT1";
                    codItems.CodItem = oRecG.Fields.Item("ItemCode").Value.ToString().PadLeft(35, ' ').Substring(0, 35).Trim();
                    codItemsb.CodItems.Add(codItems);

                    detalle.CodItems.Add(codItemsb);

                    emision.Detalles.Detalle.Add(detalle);
                    count++;
                    oRecG.MoveNext();
                }

                // Referencias OC
                Referencias referencias = null;
                int countref = 0;
                if (!string.IsNullOrEmpty(oRec.Fields.Item("U_VK_OrderCompra").Value.ToString()))
                {
                    countref++;
                    referencias = new DBNetEC.Referencias();
                    referencias.NroLinRef = countref.ToString();
                    referencias.TpoDocRef = "801";
                    referencias.FolioRef = oRec.Fields.Item("U_VK_OrderCompra").Value.ToString();
                    referencias.FchRef = DateTime.Parse(oRec.Fields.Item("U_VK_FechaRef").Value.ToString()).ToString("yyyy-MM-dd");
                    emision.DescuentosRecargosyOtros.Referencias.Referencias.Add(referencias);
                }
                // Referencias HES
                if (!string.IsNullOrEmpty(oRec.Fields.Item("U_VK_HES").Value.ToString()))
                {
                    countref++;
                    referencias = new DBNetEC.Referencias();
                    referencias.NroLinRef = countref.ToString();
                    referencias.TpoDocRef = "HES";
                    referencias.FolioRef = oRec.Fields.Item("U_VK_HES").Value.ToString();
                    referencias.FchRef = DateTime.Now.Date.ToString("yyyy-MM-dd");
                    emision.DescuentosRecargosyOtros.Referencias.Referencias.Add(referencias);
                }
                // Referencias Contrato
                if (!string.IsNullOrEmpty(oRec.Fields.Item("U_VK_NumContrato").Value.ToString()))
                {
                    countref++;
                    referencias = new DBNetEC.Referencias();
                    referencias.NroLinRef = countref.ToString();
                    referencias.TpoDocRef = "802";
                    referencias.FolioRef = oRec.Fields.Item("U_VK_NumContrato").Value.ToString();
                    referencias.FchRef = DateTime.Now.Date.ToString("yyyy-MM-dd");
                    emision.DescuentosRecargosyOtros.Referencias.Referencias.Add(referencias);
                }

                // Descuento general
                Descuentos descuentos = null;
                string descuento = oRec.Fields.Item("Descuento").Value.ToString().Replace(",", ".");
                if (!string.IsNullOrEmpty(descuento) && !descuento.Equals("0"))
                {
                    descuentos = new DBNetEC.Descuentos();
                    descuentos.NroLinDR = "1";
                    descuentos.TpoMov = "D";
                    descuentos.TpoValor = "$";
                    descuentos.ValorDR = descuento;
                    emision.DescuentosRecargosyOtros.Descuentos.Descuentos.Add(descuentos);
                }

            }
            Local.FuncionesComunes.LiberarObjetoGenerico(oRec);
            Local.FuncionesComunes.LiberarObjetoGenerico(oRec2);
            Local.FuncionesComunes.LiberarObjetoGenerico(oRecG);
            Local.FuncionesComunes.LiberarObjetoGenerico(oRecG2);
            return emision;
        }
    }
}
