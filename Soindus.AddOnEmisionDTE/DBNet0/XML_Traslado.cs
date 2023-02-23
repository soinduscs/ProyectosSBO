using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Soindus.AddOnEmisionDTE.DBNet0
{
    public class XML_Traslado
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
                            ""DocRate"", ""DocDueDate"", ""SlpName"", ""GroupNum"", ""ToWhsCode"", ""Comments"",
                            ""U_SO_FOLIODB"", ""U_SO_FECHADB"", ""U_SO_TDBSII"", ""U_SO_NOTAVTA"", ""U_SO_BULTOS"", ""U_SO_CODREFDB"",
                            ""U_SO_DESTENVIO"", ""U_SO_PATENTE"", ""U_SO_NOMTRANS"", ""U_SO_NPALLETS"", ""U_SO_NGPALLETS"",
                            ""U_SO_TIPODESP"", ""U_SO_TIPOTRAS"",
                            ""FolioNum"", ""DocSubType"", ""CardCode"",
                            case when DocRate <= 1 then Doctotal else DoctotalSy end,
                            case when ""DocRate"" <= 1 then 0 else ""DocTotal"" end as ""Exento"",
                            case when ""DocRate"" <= 1 then ""VatSum"" else ""VatSumSy"" end as ""Iva"",
                            Docentry, 
                            case when ""DocRate"" <= 1 then (ROUND(""DocTotal"", 0) - ROUND(""VatSum"", 0)) else (ROUND(""DocTotalSy"", 0) - ROUND(""VatSumSy"", 0)) end as ""Neto"", 
                            case when ""DocRate"" <= 1 then ""DiscSum"" else ""DiscSumSy"" end as ""Descuento"", 
                            doctype, PaytoCode, ShiptoCode, Header, SysRate,
                            case when DocRate <= 1 then 'Y' else 'N' end,
                            DocCur
                            
                            from ""OWTR"" INNER JOIN ""OSLP"" ON ""OWTR"".""SlpCode"" = ""OSLP"".""SlpCode""
                            where ""DocEntry"" = '" + DocEntry + "'";

            oRec.DoQuery(query);

            if (!oRec.EoF)
            {
                emision = new DBNet0.putCustomerETDLoad();
                query = @"Select * from ""OUSR"" where ""USER_CODE"" = '" + SBO.ConexionSBO.oCompany.UserName + "'";
                oRecG.DoQuery(query);
                emision.Extras.ValoresLibres.ImprDestino = oRecG.Fields.Item("U_SO_PRINTER").Value.ToString();
                query = @"Select * from ""@SO_PARAMSFE"" where ""Code"" = 'GIRO'";
                oRecG.DoQuery(query);
                emision.Extras.ValoresLibres.ValorLibre2 = oRecG.Fields.Item("U_DESCRIPCION").Value.ToString().PadLeft(80, ' ').Substring(0, 80).Trim();
                emision.Extras.ValoresLibres.ValorLibre3 = oRec2.Fields.Item("E_Mail").Value.ToString();
                emision.Extras.ValoresLibres.ValorLibre4 = oRec.Fields.Item("U_SO_DESTENVIO").Value.ToString();
                string valorlibre5 = string.Format("{0}@{1}@{2}", oRec.Fields.Item("U_SO_NPALLETS").Value.ToString(), oRec.Fields.Item("U_SO_NGPALLETS").Value.ToString(), oRec.Fields.Item("Comments").Value.ToString());
                emision.Extras.ValoresLibres.ValorLibre5 = valorlibre5;
                emision.Encabezado.camposEncabezado.Version = "1.0";
                emision.Encabezado.camposEncabezado.TipoDTE = "52";
                emision.Encabezado.camposEncabezado.Folio = oRec.Fields.Item("DocNum").Value.ToString();
                emision.Encabezado.camposEncabezado.FchEmis = DateTime.Parse(oRec.Fields.Item("DocDate").Value.ToString()).ToString("yyyy-MM-dd");
                emision.Encabezado.camposEncabezado.FchVenc = DateTime.Parse(oRec.Fields.Item("DocDueDate").Value.ToString()).ToString("yyyy-MM-dd");
                emision.Encabezado.camposEncabezado.TipoDespacho = oRec.Fields.Item("U_SO_TIPODESP").Value.ToString();
                emision.Encabezado.camposEncabezado.IndTraslado = oRec.Fields.Item("U_SO_TIPOTRAS").Value.ToString();
                //query = @"Select * from ""@SO_FORMAPAGO"" where ""Code"" = '" + oRec.Fields.Item("GroupNum").Value.ToString() + "'";
                //oRecG.DoQuery(query);
                //emision.Encabezado.camposEncabezado.FmaPago = oRecG.Fields.Item("U_FMAPAGO").Value.ToString();
                emision.Encabezado.camposEncabezado.FmaPago = "1";
                emision.Encabezado.camposEncabezado.RUTEmisor = oRec2.Fields.Item("TaxIdNum").Value.ToString();
                emision.Encabezado.camposEncabezado.RznSoc = oRec2.Fields.Item("CompnyName").Value.ToString().PadLeft(100, ' ').Substring(0, 100).Trim();
                query = @"Select * from ""@SO_PARAMSFE"" where ""Code"" = 'GIRO'";
                oRecG.DoQuery(query);
                emision.Encabezado.camposEncabezado.GiroEmis = oRecG.Fields.Item("U_DESCRIPCION").Value.ToString().PadLeft(80, ' ').Substring(0, 80).Trim();
                emision.Encabezado.camposEncabezado.DirOrigen = string.Format("{0} {1}", oRec2.Fields.Item("Street").Value.ToString(), oRec2.Fields.Item("StreetNo").Value.ToString()).PadLeft(60, ' ').Substring(0, 60).Trim();
                emision.Encabezado.camposEncabezado.CmnaOrigen = oRec2.Fields.Item("County").Value.ToString().PadLeft(20, ' ').Substring(0, 20).Trim();
                emision.Encabezado.camposEncabezado.CiudadOrigen = oRec2.Fields.Item("City").Value.ToString().PadLeft(20, ' ').Substring(0, 20).Trim();
                emision.Encabezado.camposEncabezado.Telefono = oRec2.Fields.Item("Phone1").Value.ToString();
                emision.Encabezado.camposEncabezado.CdgVendedor = oRec.Fields.Item("SlpName").Value.ToString().PadLeft(60, ' ').Substring(0, 60).Trim();
                query = @"Select * from ""OCRD"" where ""CardCode"" = '" + oRec.Fields.Item("CardCode").Value.ToString() + "'";
                oRecG.DoQuery(query);
                string impCatalogoSN = oRecG.Fields.Item("U_SO_IMPCATALOGO").Value.ToString();
                string impCodigoSN = oRecG.Fields.Item("U_SO_IMPCODIGO2").Value.ToString();
                string impDescriSN = oRecG.Fields.Item("U_SO_IMPDESCRIPCION2").Value.ToString();
                emision.Encabezado.camposEncabezado.RUTRecep = oRecG.Fields.Item("LicTradNum").Value.ToString();
                emision.Encabezado.camposEncabezado.CdgIntRecep = oRec.Fields.Item("CardCode").Value.ToString();
                emision.Encabezado.camposEncabezado.RznSocRecep = oRecG.Fields.Item("CardName").Value.ToString().PadLeft(100, ' ').Substring(0, 100).Trim();
                emision.Encabezado.camposEncabezado.GiroRecep = oRecG.Fields.Item("Notes").Value.ToString().PadLeft(40, ' ').Substring(0, 40).Trim();
                query = @"Select * from ""OWHS"" where ""WhsCode"" = '" + oRec.Fields.Item("ToWhsCode").Value.ToString() + "'";
                oRecG.DoQuery(query);
                emision.Encabezado.camposEncabezado.DirRecep = oRecG.Fields.Item("Street").Value.ToString().PadLeft(70, ' ').Substring(0, 70).Trim();
                emision.Encabezado.camposEncabezado.CmnaRecep = oRecG.Fields.Item("County").Value.ToString().PadLeft(20, ' ').Substring(0, 20).Trim();
                emision.Encabezado.camposEncabezado.CiudadRecep = oRecG.Fields.Item("City").Value.ToString().PadLeft(20, ' ').Substring(0, 20).Trim();
                emision.Encabezado.camposEncabezado.DirPostal = oRecG.Fields.Item("Street").Value.ToString().PadLeft(70, ' ').Substring(0, 70).Trim();
                emision.Encabezado.camposEncabezado.CmnaPostal = oRecG.Fields.Item("County").Value.ToString().PadLeft(20, ' ').Substring(0, 20).Trim();
                emision.Encabezado.camposEncabezado.CiudadPostal = oRecG.Fields.Item("City").Value.ToString().PadLeft(20, ' ').Substring(0, 20).Trim();
                query = @"Select * from ""CRD1"" where ""Address"" = '" + oRec.Fields.Item("ShipToCode").Value.ToString() + @"' and ""CardCode"" = '" + oRec.Fields.Item("CardCode").Value.ToString() + "'";
                oRecG.DoQuery(query);
                emision.Encabezado.camposEncabezado.DirDest = oRecG.Fields.Item("Street").Value.ToString().PadLeft(60, ' ').Substring(0, 60).Trim();
                emision.Encabezado.camposEncabezado.CmnaDest = oRecG.Fields.Item("County").Value.ToString().PadLeft(20, ' ').Substring(0, 20).Trim();
                emision.Encabezado.camposEncabezado.CiudadDest = oRecG.Fields.Item("City").Value.ToString().PadLeft(20, ' ').Substring(0, 20).Trim();

                emision.Encabezado.camposEncabezado.Patente = oRec.Fields.Item("U_SO_PATENTE").Value.ToString();
                emision.Encabezado.camposEncabezado.MntNeto = oRec.Fields.Item("Neto").Value.ToString().Replace(",", ".");
                emision.Encabezado.camposEncabezado.MntExe = oRec.Fields.Item("Exento").Value.ToString().Replace(",", ".");
                emision.Encabezado.camposEncabezado.IVA = oRec.Fields.Item("Iva").Value.ToString().Replace(",", ".");

                query = @"Select top 1 * from ""WTR1"" where ""DocEntry"" = " + DocEntry + "";
                oRecG.DoQuery(query);
                query = @"Select * from ""OSTA"" where ""Code"" = '" + oRecG.Fields.Item("TaxCode").Value.ToString() + "'";
                oRecG2.DoQuery(query);
                emision.Encabezado.camposEncabezado.TasaIVA = oRecG2.Fields.Item("Rate").Value.ToString().Replace(",", ".");
                emision.Encabezado.camposEncabezado.MntTotal = oRec.Fields.Item("DocTotal").Value.ToString().Replace(",", ".");
                query = @"Select * from ""OCTG"" where ""GroupNum"" = '" + oRec.Fields.Item("GroupNum").Value.ToString() + "'";
                oRecG.DoQuery(query);
                emision.Encabezado.camposEncabezado.GlosaPagos = oRecG.Fields.Item("PymntGroup").Value.ToString().PadLeft(100, ' ').Substring(0, 100).Trim();
                emision.Encabezado.camposEncabezado.NomCiaTransp = oRec.Fields.Item("U_SO_NOMTRANS").Value.ToString().PadLeft(30, ' ').Substring(0, 30).Trim();
                emision.Encabezado.camposEncabezado.TotBultos = oRec.Fields.Item("U_SO_BULTOS").Value.ToString();

                // Actividad económica
                ActivEcon actividades = new DBNet0.ActivEcon();
                query = @"Select * from ""@SO_ACTECONFE""";
                oRecG.DoQuery(query);
                while (!oRecG.EoF)
                {
                    actividades.ActividadEconomica.Add(new DBNet0.ActividadEconomica() { ActivEcon = oRecG.Fields.Item("Code").Value.ToString() });
                    oRecG.MoveNext();
                }
                emision.Encabezado.ActivEcon = actividades;

                //Detalle
                query = @"Select * from ""@SO_PARAMSFE"" where ""Code"" = 'MARCA'";
                oRecG.DoQuery(query);
                string impMarca = oRecG.Fields.Item("U_Descripcion").Value.ToString();

                if (impMarca.Equals("SI"))
                {
                    query = @"Select
                            ""WTR1"".""ItemCode"" as ""ItemCode"",
                            ""WTR1"".""Dscription"" as ""Dscription"",
                            ""WTR1"".""Quantity"" as ""Quantity"",
                            ""WTR1"".""Price"" as ""Price"",
                            ""WTR1"".""DiscPrcnt"" as ""DiscPrcnt"",
                            (""WTR1"".""DiscPrcnt"" * ""WTR1"".""PriceBefDi"" * ""WTR1"".""Quantity"" /100) as ""MtoDescuento"",
                            ""WTR1"".""LineTotal"" as ""LineTotal"",
                            ""WTR1"".""PackQty"" as ""PackQty"",
                            ""WTR1"".""SubCatNum"" as ""SubCatNum"",
                            ""OMRC"".""FirmName"" as ""FirmName""
                            from ""WTR1""
                            left outer join ""OITM""
                            on ""WTR1"".""ItemCode"" = ""OITM"".""ItemCode""
                            left outer join ""OMRC""
                            on ""OITM"".""FirmCode"" = ""OMRC"".""FirmCode""
                            where ""WTR1"".""DocEntry"" = " + DocEntry + "";
                }
                else
                {
                    query = @"Select *, (""DiscPrcnt"" * ""PriceBefDi"" * ""Quantity"" /100) as ""MtoDescuento"" 
                            from ""WTR1"" where ""DocEntry"" = " + DocEntry + "";
                }
                oRecG.DoQuery(query);
                int count = 1;
                while (!oRecG.EoF)
                {

                    Detalle detalle = new Detalle();
                    detalle.Detalles.NroLinDet = count.ToString();
                    detalle.Detalles.NmbItem = oRecG.Fields.Item("ItemCode").Value.ToString();
                    detalle.Detalles.DscItem = oRecG.Fields.Item("Dscription").Value.ToString();
                    detalle.Detalles.QtyItem = oRecG.Fields.Item("Quantity").Value.ToString().Replace(",", ".");
                    detalle.Detalles.PrcItem = oRecG.Fields.Item("Price").Value.ToString().Replace(",", ".");
                    detalle.Detalles.DescuentoPct = oRecG.Fields.Item("DiscPrcnt").Value.ToString().Replace(",", ".");
                    detalle.Detalles.DescuentoMonto = oRecG.Fields.Item("MtoDescuento").Value.ToString().Replace(",", ".");
                    detalle.Detalles.MontoItem = oRecG.Fields.Item("LineTotal").Value.ToString().Replace(",", ".");
                    detalle.Detalles.QtyRef = oRecG.Fields.Item("PackQty").Value.ToString().Replace(",", ".");

                    CodItems codItems = null;
                    CodItemsb codItemsb = null;
                    codItemsb = new CodItemsb();
                    query = @"Select ""Substitute"", ""U_SO_CODIGO2"", ""U_SO_DESCRIPCION2"" 
                        from ""OSCN""
                        where ""CardCode"" = '" + oRec.Fields.Item("CardCode").Value.ToString() + @"' and ""Substitute"" = '" + oRecG.Fields.Item("SubCatNum").Value.ToString() + "'";
                    if (impMarca.Equals("SI"))
                    {
                        codItems = new CodItems();
                        codItems.TpoCodigo = "MRK";
                        codItems.CodItem = oRecG.Fields.Item("FirmName").Value.ToString().PadLeft(35, ' ').Substring(0, 35).Trim();
                        //codItemsb = new CodItemsb();
                        codItemsb.CodItems.Add(codItems);
                        //detalle.CodItems.Add(codItemsb);
                    }
                    if (impCatalogoSN.Equals("Y"))
                    {
                        oRecG2.DoQuery(query);
                        codItems = new CodItems();
                        codItems.TpoCodigo = "CBAR";
                        codItems.CodItem = oRecG2.Fields.Item("Substitute").Value.ToString().PadLeft(35, ' ').Substring(0, 35).Trim();
                        //codItemsb = new CodItemsb();
                        codItemsb.CodItems.Add(codItems);
                        //detalle.CodItems.Add(codItemsb);
                    }
                    if (impCodigoSN.Equals("Y"))
                    {
                        oRecG2.DoQuery(query);
                        codItems = new CodItems();
                        codItems.TpoCodigo = "COD2";
                        codItems.CodItem = oRecG2.Fields.Item("U_SO_CODIGO2").Value.ToString().PadLeft(35, ' ').Substring(0, 35).Trim();
                        //codItemsb = new CodItemsb();
                        codItemsb.CodItems.Add(codItems);
                        //detalle.CodItems.Add(codItemsb);
                    }
                    if (impDescriSN.Equals("Y"))
                    {
                        oRecG2.DoQuery(query);
                        codItems = new CodItems();
                        codItems.TpoCodigo = "DES2";
                        codItems.CodItem = oRecG2.Fields.Item("U_SO_DESCRIPCION2").Value.ToString().PadLeft(35, ' ').Substring(0, 35).Trim();
                        //codItemsb = new CodItemsb();
                        codItemsb.CodItems.Add(codItems);
                        //detalle.CodItems.Add(codItemsb);
                    }
                    detalle.CodItems.Add(codItemsb);

                    emision.Detalles.Detalle.Add(detalle);
                    count++;
                    oRecG.MoveNext();
                }

                // Referencias OC
                Referencias referencias = null;
                int countref = 0;
                //if (!string.IsNullOrEmpty(oRec.Fields.Item("NumAtCard").Value.ToString()))
                //{
                //    string _referencia = oRec.Fields.Item("NumAtCard").Value.ToString().Replace(".", "").Replace(" ", "");
                //    var _arrRefers = _referencia.Split(';');
                //    var _listRefers = _arrRefers.Distinct().ToList();
                //    foreach (var item in _listRefers)
                //    {
                //        if (!string.IsNullOrEmpty(item))
                //        {
                //            countref++;
                //            referencias = new DBNet0.Referencias();
                //            referencias.NroLinRef = countref.ToString();
                //            referencias.TpoDocRef = "801";
                //            referencias.FolioRef = item.ToString();
                //            referencias.FchRef = DateTime.Parse(oRec.Fields.Item("DocDate").Value.ToString()).ToString("yyyy-MM-dd");
                //            emision.DescuentosRecargosyOtros.Referencias.Referencias.Add(referencias);
                //        }
                //    }
                //}

                // Otras Referencias Campos de usuario
                if (!string.IsNullOrEmpty(oRec.Fields.Item("U_SO_TDBSII").Value.ToString()))
                {
                    countref++;
                    referencias = new DBNet0.Referencias();
                    referencias.NroLinRef = countref.ToString();
                    referencias.TpoDocRef = oRec.Fields.Item("U_SO_TDBSII").Value.ToString();
                    referencias.FolioRef = oRec.Fields.Item("U_SO_FOLIODB").Value.ToString();
                    referencias.FchRef = DateTime.Parse(oRec.Fields.Item("U_SO_FECHADB").Value.ToString()).ToString("yyyy-MM-dd");
                    referencias.CodRef = oRec.Fields.Item("U_SO_CODREFDB").Value.ToString();
                    //referencias.RazonRef = "NA";
                    emision.DescuentosRecargosyOtros.Referencias.Referencias.Add(referencias);
                }

                // Referencias Nota de Venta
                if (!string.IsNullOrEmpty(oRec.Fields.Item("U_SO_NOTAVTA").Value.ToString()))
                {
                    countref++;
                    referencias = new DBNet0.Referencias();
                    referencias.NroLinRef = countref.ToString();
                    referencias.TpoDocRef = "NV";
                    referencias.FolioRef = oRec.Fields.Item("U_SO_NOTAVTA").Value.ToString();
                    referencias.FchRef = DateTime.Now.Date.ToString("yyyy-MM-dd");
                    //referencias.CodRef = "0";
                    //referencias.RazonRef = "NA";
                    emision.DescuentosRecargosyOtros.Referencias.Referencias.Add(referencias);
                }

                // Descuento general
                Descuentos descuentos = null;
                string descuento = oRec.Fields.Item("Descuento").Value.ToString().Replace(",", ".");
                if (!string.IsNullOrEmpty(descuento) && !descuento.Equals("0"))
                {
                    descuentos = new DBNet0.Descuentos();
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
