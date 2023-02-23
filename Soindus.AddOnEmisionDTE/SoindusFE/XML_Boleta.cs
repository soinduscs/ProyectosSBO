using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soindus.AddOnEmisionDTE.SoindusFE
{
    public class XML_Boleta
    {
        public Documento Generar(int DocEntry)
        {
            Documento emision = null;
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
                            ""U_SO_FOLIODB"", ""U_SO_FECHADB"", ""U_SO_TDBSII"", ""U_SO_NOTAVTA"", ""U_SO_BULTOS"", ""U_SO_CODREFDB"",
                            ""U_SO_DESTENVIO"", ""U_SO_PATENTE"", ""U_SO_NOMTRANS"", ""U_SO_NPALLETS"", ""U_SO_NGPALLETS"",
                            ""FolioNum"", ""DocSubType"", ""CardCode"",
                            case when ""DocRate"" <= 1 then 0 else ""DocTotal"" end as ""Exento"",
                            case when ""DocRate"" <= 1 then ""VatSum"" else ""VatSumSy"" end as ""Iva"",
                            ""DocEntry"", 
                            case when ""DocRate"" <= 1 then (ROUND(""DocTotal"", 0) - ROUND(""VatSum"", 0)) else (ROUND(""DocTotalSy"", 0) - ROUND(""VatSumSy"", 0)) end as ""Neto"", 
                            case when ""DocRate"" <= 1 then ""DiscSum"" else ""DiscSumSy"" end as ""Descuento"", 
                            ""PayToCode"", ""ShipToCode""
                            
                            from ""OINV"" INNER JOIN ""OSLP"" ON ""OINV"".""SlpCode"" = ""OSLP"".""SlpCode""
                            where ""DocEntry"" = '" + DocEntry + @"' and ""DocSubType"" = 'IB'";

            oRec.DoQuery(query);

            if (!oRec.EoF)
            {
                emision = new SoindusFE.Documento();
                ////query = @"Select * from ""OUSR"" where ""USER_CODE"" = '" + SBO.ConexionSBO.oCompany.UserName + "'";
                ////oRecG.DoQuery(query);
                ////emision.Extras.ValoresLibres.ImprDestino = oRecG.Fields.Item("U_SO_PRINTER").Value.ToString();
                ////query = @"Select * from ""@SO_PARAMSFE"" where ""Code"" = 'GIRO'";
                ////oRecG.DoQuery(query);
                ////emision.Extras.ValoresLibres.ValorLibre2 = oRecG.Fields.Item("U_DESCRIPCION").Value.ToString().PadLeft(80, ' ').Substring(0, 80).Trim();
                ////emision.Extras.ValoresLibres.ValorLibre3 = oRec2.Fields.Item("E_Mail").Value.ToString();
                ////emision.Extras.ValoresLibres.ValorLibre4 = oRec.Fields.Item("U_SO_DESTENVIO").Value.ToString();
                ////string valorlibre5 = string.Format("{0}@{1}@{2}", oRec.Fields.Item("U_SO_NPALLETS").Value.ToString(), oRec.Fields.Item("U_SO_NGPALLETS").Value.ToString(), oRec.Fields.Item("Comments").Value.ToString());
                ////emision.Extras.ValoresLibres.ValorLibre5 = valorlibre5;
                ////emision.Encabezado.camposEncabezado.Version = "1.0";
                emision.Encabezado.IdDoc.TipoDTE = "39";
                emision.Encabezado.IdDoc.IndServicio = "3";
                emision.Encabezado.IdDoc.MntBruto = "2";
                emision.Encabezado.IdDoc.Folio = oRec.Fields.Item("DocNum").Value.ToString();
                emision.Encabezado.IdDoc.FchEmis = DateTime.Parse(oRec.Fields.Item("DocDate").Value.ToString()).ToString("yyyy-MM-dd");
                emision.Encabezado.IdDoc.FchVenc = DateTime.Parse(oRec.Fields.Item("DocDueDate").Value.ToString()).ToString("yyyy-MM-dd");
                query = @"Select * from ""@SO_FORMAPAGO"" where ""Code"" = '" + oRec.Fields.Item("GroupNum").Value.ToString() + "'";
                oRecG.DoQuery(query);
                emision.Encabezado.IdDoc.FmaPago = oRecG.Fields.Item("U_FMAPAGO").Value.ToString();
                emision.Encabezado.Emisor.RUTEmisor = oRec2.Fields.Item("TaxIdNum").Value.ToString();
                emision.Encabezado.Emisor.RznSoc = oRec2.Fields.Item("CompnyName").Value.ToString().PadLeft(100, ' ').Substring(0, 100).Trim();
                query = @"Select * from ""@SO_PARAMSFE"" where ""Code"" = 'GIRO'";
                oRecG.DoQuery(query);
                emision.Encabezado.Emisor.GiroEmis = oRecG.Fields.Item("U_DESCRIPCION").Value.ToString().PadLeft(80, ' ').Substring(0, 80).Trim();
                emision.Encabezado.Emisor.DirOrigen = string.Format("{0} {1}", oRec2.Fields.Item("Street").Value.ToString(), oRec2.Fields.Item("StreetNo").Value.ToString()).PadLeft(60, ' ').Substring(0, 60).Trim();
                emision.Encabezado.Emisor.CmnaOrigen = oRec2.Fields.Item("County").Value.ToString().PadLeft(20, ' ').Substring(0, 20).Trim();
                emision.Encabezado.Emisor.CiudadOrigen = oRec2.Fields.Item("City").Value.ToString().PadLeft(20, ' ').Substring(0, 20).Trim();
                emision.Encabezado.Emisor.Telefono = new string[] { oRec2.Fields.Item("Phone1").Value.ToString() };
                ////emision.Encabezado.camposEncabezado.CdgVendedor = oRec.Fields.Item("SlpName").Value.ToString().PadLeft(60, ' ').Substring(0, 60).Trim();
                //valorlibre6 += string.Format("{0}@", oRec.Fields.Item("SlpName").Value.ToString().PadLeft(60, ' ').Substring(0, 60).Trim());
                query = @"Select * from ""OCRD"" where ""CardCode"" = '" + oRec.Fields.Item("CardCode").Value.ToString() + "'";
                oRecG.DoQuery(query);
                emision.Encabezado.Receptor.RUTRecep = oRecG.Fields.Item("LicTradNum").Value.ToString();
                emision.Encabezado.Receptor.CdgIntRecep = oRec.Fields.Item("CardCode").Value.ToString();
                emision.Encabezado.Receptor.RznSocRecep = oRecG.Fields.Item("CardName").Value.ToString().PadLeft(100, ' ').Substring(0, 100).Trim();
                emision.Encabezado.Receptor.GiroRecep = oRecG.Fields.Item("Notes").Value.ToString().PadLeft(40, ' ').Substring(0, 40).Trim();
                query = @"Select * from ""CRD1"" where ""Address"" = '" + oRec.Fields.Item("PayToCode").Value.ToString() + @"' and ""CardCode"" = '" + oRec.Fields.Item("CardCode").Value.ToString() + "'";
                oRecG.DoQuery(query);
                emision.Encabezado.Receptor.DirRecep = oRecG.Fields.Item("Street").Value.ToString().PadLeft(70, ' ').Substring(0, 70).Trim();
                emision.Encabezado.Receptor.CmnaRecep = oRecG.Fields.Item("County").Value.ToString().PadLeft(20, ' ').Substring(0, 20).Trim();
                emision.Encabezado.Receptor.CiudadRecep = oRecG.Fields.Item("City").Value.ToString().PadLeft(20, ' ').Substring(0, 20).Trim();
                emision.Encabezado.Receptor.DirPostal = oRecG.Fields.Item("Street").Value.ToString().PadLeft(70, ' ').Substring(0, 70).Trim();
                emision.Encabezado.Receptor.CmnaPostal = oRecG.Fields.Item("County").Value.ToString().PadLeft(20, ' ').Substring(0, 20).Trim();
                emision.Encabezado.Receptor.CiudadPostal = oRecG.Fields.Item("City").Value.ToString().PadLeft(20, ' ').Substring(0, 20).Trim();
                query = @"Select * from ""CRD1"" where ""Address"" = '" + oRec.Fields.Item("ShipToCode").Value.ToString() + @"' and ""CardCode"" = '" + oRec.Fields.Item("CardCode").Value.ToString() + "'";
                oRecG.DoQuery(query);
                emision.Encabezado.Transporte.DirDest = oRecG.Fields.Item("Street").Value.ToString().PadLeft(60, ' ').Substring(0, 60).Trim();
                emision.Encabezado.Transporte.CmnaDest = oRecG.Fields.Item("County").Value.ToString().PadLeft(20, ' ').Substring(0, 20).Trim();
                emision.Encabezado.Transporte.CiudadDest = oRecG.Fields.Item("City").Value.ToString().PadLeft(20, ' ').Substring(0, 20).Trim();

                emision.Encabezado.Transporte.Patente = oRec.Fields.Item("U_SO_PATENTE").Value.ToString();
                emision.Encabezado.Totales.MntNeto = oRec.Fields.Item("Neto").Value.ToString().Replace(",", ".");
                emision.Encabezado.Totales.MntExe = oRec.Fields.Item("Exento").Value.ToString().Replace(",", ".");
                emision.Encabezado.Totales.IVA = oRec.Fields.Item("Iva").Value.ToString().Replace(",", ".");

                query = @"Select top 1 * from ""INV1"" where ""DocEntry"" = " + DocEntry + "";
                oRecG.DoQuery(query);
                query = @"Select * from ""OSTA"" where ""Code"" = '" + oRecG.Fields.Item("TaxCode").Value.ToString() + "'";
                oRecG2.DoQuery(query);
                emision.Encabezado.Totales.TasaIVA = oRecG2.Fields.Item("Rate").Value.ToString().Replace(",", ".");
                emision.Encabezado.Totales.MntTotal = oRec.Fields.Item("DocTotal").Value.ToString().Replace(",", ".");
                query = @"Select * from ""OCTG"" where ""GroupNum"" = '" + oRec.Fields.Item("GroupNum").Value.ToString() + "'";
                oRecG.DoQuery(query);
                //emision.Encabezado.IdDoc.MntPagos = new MntPagos[] { new MntPagos() { GlosaPagos = oRecG.Fields.Item("PymntGroup").Value.ToString().PadLeft(100, ' ').Substring(0, 100).Trim() } };
                //valorlibre6 += string.Format("{0}@", oRecG.Fields.Item("PymntGroup").Value.ToString().PadLeft(100, ' ').Substring(0, 100).Trim());
                emision.Encabezado.Transporte.Aduana.NomCiaTransp = oRec.Fields.Item("U_SO_NOMTRANS").Value.ToString().PadLeft(30, ' ').Substring(0, 30).Trim();
                //emision.Encabezado.Transporte.Aduana.TotBultos = oRec.Fields.Item("U_SO_BULTOS").Value.ToString();
                //valorlibre6 += string.Format("{0}", oRec.Fields.Item("U_SO_BULTOS").Value.ToString());
                //emision.Extras.ValoresLibres.ValorLibre6 = valorlibre6;

                // Actividad económica
                List<string> actividades = new List<string>();
                query = @"Select * from ""@SO_ACTECONFE""";
                oRecG.DoQuery(query);
                while (!oRecG.EoF)
                {
                    actividades.Add(oRecG.Fields.Item("Code").Value.ToString());
                    oRecG.MoveNext();
                }
                emision.Encabezado.Emisor.Acteco = actividades.ToArray();

                List<Detalle> listaDetalle = new List<Detalle>();
                //Detalle
                query = @"Select *, (""DiscPrcnt"" * ""PriceBefDi"" * ""Quantity"" /100) as ""MtoDescuento"" 
                    from ""INV1"" where ""DocEntry"" = " + DocEntry + "";
                oRecG.DoQuery(query);
                int count = 1;
                while (!oRecG.EoF)
                {
                    Detalle detalle = new Detalle();
                    detalle.NroLinDet = count.ToString();
                    detalle.NmbItem = oRecG.Fields.Item("ItemCode").Value.ToString();
                    detalle.DscItem = oRecG.Fields.Item("Dscription").Value.ToString();
                    detalle.QtyItem = oRecG.Fields.Item("Quantity").Value.ToString().Replace(",", ".");
                    detalle.PrcItem = oRecG.Fields.Item("Price").Value.ToString().Replace(",", ".");
                    detalle.DescuentoPct = oRecG.Fields.Item("DiscPrcnt").Value.ToString().Replace(",", ".");
                    detalle.DescuentoMonto = oRecG.Fields.Item("MtoDescuento").Value.ToString().Replace(",", ".");
                    detalle.MontoItem = oRecG.Fields.Item("LineTotal").Value.ToString().Replace(",", ".");
                    detalle.QtyRef = oRecG.Fields.Item("PackQty").Value.ToString().Replace(",", ".");

                    listaDetalle.Add(detalle);
                    count++;
                    oRecG.MoveNext();
                }
                emision.Detalle = listaDetalle.ToArray();

                List<Referencia> listaReferencia = new List<Referencia>();
                // Referencias OC
                Referencia referencias = null;
                int countref = 0;
                if (!string.IsNullOrEmpty(oRec.Fields.Item("NumAtCard").Value.ToString()))
                {
                    string _referencia = oRec.Fields.Item("NumAtCard").Value.ToString().Replace(".", "").Replace(" ", "");
                    var _arrRefers = _referencia.Split(';');
                    var _listRefers = _arrRefers.Distinct().ToList();
                    foreach (var item in _listRefers)
                    {
                        if (!string.IsNullOrEmpty(item))
                        {
                            countref++;
                            referencias = new Referencia();
                            referencias.NroLinRef = countref.ToString();
                            referencias.TpoDocRef = "801";
                            referencias.FolioRef = item.ToString();
                            referencias.FchRef = DateTime.Parse(oRec.Fields.Item("DocDate").Value.ToString()).ToString("yyyy-MM-dd");
                            referencias.CodRef = "0";
                            referencias.RazonRef = "NA";
                            listaReferencia.Add(referencias);
                        }
                    }
                }

                // Otras Referencias Campos de usuario
                if (!string.IsNullOrEmpty(oRec.Fields.Item("U_SO_TDBSII").Value.ToString()))
                {
                    countref++;
                    referencias = new Referencia();
                    referencias.NroLinRef = countref.ToString();
                    referencias.TpoDocRef = oRec.Fields.Item("U_SO_TDBSII").Value.ToString();
                    referencias.FolioRef = oRec.Fields.Item("U_SO_FOLIODB").Value.ToString();
                    referencias.FchRef = DateTime.Parse(oRec.Fields.Item("U_SO_FECHADB").Value.ToString()).ToString("yyyy-MM-dd");
                    referencias.CodRef = "0";
                    referencias.RazonRef = "NA";
                    listaReferencia.Add(referencias);
                }

                // Referencias Entregas
                query = @"Select distinct(""BaseEntry"") 
                    from ""INV1"" 
                    where ""BaseType"" = 15 and ""DocEntry"" = " + DocEntry + "";
                oRecG.DoQuery(query);
                while (!oRecG.EoF)
                {
                    query = @"Select ""DocNum"", ""DocDate"", ""FolioNum"" 
                    from ""ODLN""
                    where ""DocEntry"" = " + oRecG.Fields.Item("BaseEntry").Value.ToString() + @" and ""FolioNum"" is not null and ""FolioNum"" <> 0 ";
                    oRecG2.DoQuery(query);
                    while (!oRecG2.EoF)
                    {
                        countref++;
                        referencias = new Referencia();
                        referencias.NroLinRef = countref.ToString();
                        referencias.TpoDocRef = "52";
                        referencias.FolioRef = oRecG2.Fields.Item("FolioNum").Value.ToString();
                        referencias.FchRef = DateTime.Parse(oRecG2.Fields.Item("DocDate").Value.ToString()).ToString("yyyy-MM-dd");
                        referencias.CodRef = "0";
                        referencias.RazonRef = "NA";
                        listaReferencia.Add(referencias);
                        oRecG2.MoveNext();
                    }
                    oRecG.MoveNext();
                }

                // Referencias Nota de Venta
                if (!string.IsNullOrEmpty(oRec.Fields.Item("U_SO_NOTAVTA").Value.ToString()))
                {
                    countref++;
                    referencias = new Referencia();
                    referencias.NroLinRef = countref.ToString();
                    referencias.TpoDocRef = "NV";
                    referencias.FolioRef = oRec.Fields.Item("U_SO_NOTAVTA").Value.ToString();
                    referencias.FchRef = DateTime.Now.Date.ToString("yyyy-MM-dd");
                    referencias.CodRef = "0";
                    referencias.RazonRef = "NA";
                    listaReferencia.Add(referencias);
                }
                emision.Referencia = listaReferencia.ToArray();

                List<DscRcgGlobal> listaDescuentos = new List<DscRcgGlobal>();
                // Descuento general
                DscRcgGlobal descuentos = null;
                string descuento = oRec.Fields.Item("Descuento").Value.ToString().Replace(",", ".");
                if (!string.IsNullOrEmpty(descuento) && !descuento.Equals("0"))
                {
                    descuentos = new DscRcgGlobal();
                    descuentos.NroLinDR = "1";
                    descuentos.TpoMov = "D";
                    descuentos.TpoValor = "$";
                    descuentos.ValorDR = descuento;
                    listaDescuentos.Add(descuentos);
                }
                emision.DscRcgGlobal = listaDescuentos.ToArray();

            }
            Local.FuncionesComunes.LiberarObjetoGenerico(oRec);
            Local.FuncionesComunes.LiberarObjetoGenerico(oRec2);
            Local.FuncionesComunes.LiberarObjetoGenerico(oRecG);
            Local.FuncionesComunes.LiberarObjetoGenerico(oRecG2);
            return emision;
        }
    }
}
