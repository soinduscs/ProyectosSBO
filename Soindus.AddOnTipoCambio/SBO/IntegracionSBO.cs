using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using portTC = Soindus.Interfaces.PortadoresTC;

namespace Soindus.AddOnTipoCambio.SBO
{
    public class IntegracionSBO
    {
        public void ObtenerIndicadores(string FechaInicial = "", string FechaFinal = "")
        {
            FechaInicial = string.IsNullOrEmpty(FechaInicial) ? DateTime.Now.ToString("yyyyMMdd") : FechaInicial;
            FechaFinal = string.IsNullOrEmpty(FechaFinal) ? DateTime.Now.ToString("yyyyMMdd") : FechaFinal;
            string FechaIni = string.Format("{0}-{1}-{2}", FechaInicial.Substring(0, 4), FechaInicial.Substring(4, 2), FechaInicial.Substring(6, 2));
            string FechaFin = string.Format("{0}-{1}-{2}", FechaFinal.Substring(0, 4), FechaFinal.Substring(4, 2), FechaFinal.Substring(6, 2));
            DateTime FecIni = Convert.ToDateTime(FechaIni);
            DateTime FecFin = Convert.ToDateTime(FechaFin);
            if (FecIni > FecFin)
            {
                string FechaAux = FechaInicial;
                FechaInicial = FechaFinal;
                FechaFinal = FechaAux;
                DateTime FecAux = FecIni;
                FecIni = FecFin;
                FecFin = FecAux;
            }
            int TotDias = (FecFin - FecIni).Days + 1;

            Clases.ListaTipoCambiario _registros = new Clases.ListaTipoCambiario();
            Clases.TipoCambiario tipoCambiario;

            NumberFormatInfo _provider = new NumberFormatInfo();
            _provider.NumberDecimalSeparator = ".";
            _provider.NumberGroupSeparator = ",";

            //Obtener desde portador
            //
            SAPbobsCOM.SBObob oSBObob = (SAPbobsCOM.SBObob)SBO.ConexionSBO.oCompany.GetBusinessObject(BoObjectTypes.BoBridge);
            SAPbobsCOM.Recordset oRecord = (SAPbobsCOM.Recordset)SBO.ConexionSBO.oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);
            string _query = @"SELECT DISTINCT ""Name"", ""U_CODIGO"", ""U_ESFIJA"", ""U_ESFACTOR"", ""U_CODIGOFACTOR"" FROM ""@SO_TIPOCAMBIO"" WHERE ""U_PORTADOR"" = '" + @"BCC" + "'";
            oRecord.DoQuery(_query);
            if (!oRecord.EoF)
            {
                _registros = new Clases.ListaTipoCambiario();
                while (!oRecord.EoF)
                {
                    PortadorTC portadorTC = new PortadorTC();
                    PortadorTC portadorTCAux = new PortadorTC();
                    double valorTC = 0;
                    double valorFactor = 0;
                    DateTime nuevaFecIni = FecIni;
                    DateTime nuevaFecFin = FecFin;
                    string[] parametros;

                    parametros = new string[] { FechaInicial, FechaFinal, oRecord.Fields.Item("U_CODIGO").Value.ToString() };
                    var portResult = portadorTC.ObtenerTipoCambiario(parametros);
                    if (portResult.Success)
                    {
                        if (portadorTC.Respuesta.Series.FameSeries.Obs == null || !portadorTC.Respuesta.Series.FameSeries.Obs.Count().Equals(TotDias))
                        {
                            int dias = 0;
                            bool salida = false;
                            while (salida.Equals(false) && dias > -10)
                            {
                                parametros = new string[] { FecIni.AddDays(dias).ToString("yyyyMMdd"), FecIni.AddDays(dias).ToString("yyyyMMdd"), oRecord.Fields.Item("U_CODIGO").Value.ToString() };
                                portResult = portadorTC.ObtenerTipoCambiario(parametros);
                                if (portResult.Success)
                                {
                                    if (portadorTC.Respuesta.Series.FameSeries.Obs != null)
                                    {
                                        nuevaFecIni = FecIni.AddDays(dias);
                                        salida = true;
                                    }
                                }
                                dias--;
                            }
                            if (portadorTC.Respuesta.Series.FameSeries.Obs != null)
                            {
                                dias = 0;
                                salida = false;
                                while (salida.Equals(false) && dias < 10)
                                {
                                    parametros = new string[] { FecFin.AddDays(dias).ToString("yyyyMMdd"), FecFin.AddDays(dias).ToString("yyyyMMdd"), oRecord.Fields.Item("U_CODIGO").Value.ToString() };
                                    portResult = portadorTC.ObtenerTipoCambiario(parametros);
                                    if (portResult.Success)
                                    {
                                        if (portadorTC.Respuesta.Series.FameSeries.Obs != null)
                                        {
                                            nuevaFecFin = FecFin.AddDays(dias);
                                            salida = true;
                                        }
                                    }
                                    dias++;
                                }
                            }
                            parametros = new string[] { nuevaFecIni.ToString("yyyyMMdd"), nuevaFecFin.ToString("yyyyMMdd"), oRecord.Fields.Item("U_CODIGO").Value.ToString() };
                            portResult = portadorTC.ObtenerTipoCambiario(parametros);
                            if (portResult.Success)
                            {
                                if (portadorTC.Respuesta.Series.FameSeries.Obs != null)
                                {
                                    portadorTC.Respuesta.Series.FameSeries.Obs = portadorTC.Respuesta.Series.FameSeries.Obs.Where(val => val.Fecha >= FecIni && val.Fecha <= FecFin).ToArray();
                                }
                            }
                        }
                        if (portadorTC.Respuesta.Series.FameSeries.Obs != null)
                        {
                            foreach (var item in portadorTC.Respuesta.Series.FameSeries.Obs)
                            {
                                tipoCambiario = new Clases.TipoCambiario();
                                tipoCambiario.Fecha = item.Fecha;
                                tipoCambiario.Indicador = oRecord.Fields.Item("Name").Value.ToString();
                                valorTC = Convert.ToDouble(item.Value, _provider);
                                if (valorTC.Equals(double.NaN) || valorTC.Equals(0))
                                {
                                    int dias = -1;
                                    while ((valorTC.Equals(double.NaN) || valorTC.Equals(0)) && dias > -10)
                                    {
                                        parametros = new string[] { item.Fecha.AddDays(dias).ToString("yyyyMMdd"), item.Fecha.AddDays(dias).ToString("yyyyMMdd"), oRecord.Fields.Item("U_CODIGO").Value.ToString() };
                                        var portResultAux = portadorTCAux.ObtenerTipoCambiario(parametros);
                                        if (portResultAux.Success)
                                        {
                                            if (portadorTCAux.Respuesta.Series.FameSeries.Obs != null)
                                            {
                                                foreach (var itemAux in portadorTCAux.Respuesta.Series.FameSeries.Obs)
                                                {
                                                    valorTC = Convert.ToDouble(itemAux.Value, _provider);
                                                }
                                            }
                                        }
                                        dias--;
                                    }
                                }
                                if (oRecord.Fields.Item("U_ESFACTOR").Value.ToString().Equals("Y") && !string.IsNullOrEmpty(oRecord.Fields.Item("U_CODIGOFACTOR").Value.ToString()))
                                {
                                    valorFactor = 0;
                                    int dias = 0;
                                    while ((valorFactor.Equals(double.NaN) || valorFactor.Equals(0)) && dias > -10)
                                    {
                                        parametros = new string[] { item.Fecha.AddDays(dias).ToString("yyyyMMdd"), item.Fecha.AddDays(dias).ToString("yyyyMMdd"), oRecord.Fields.Item("U_CODIGOFACTOR").Value.ToString() };
                                        var portResultAux = portadorTCAux.ObtenerTipoCambiario(parametros);
                                        if (portResultAux.Success)
                                        {
                                            if (portadorTCAux.Respuesta.Series.FameSeries.Obs != null)
                                            {
                                                foreach (var itemAux2 in portadorTCAux.Respuesta.Series.FameSeries.Obs)
                                                {
                                                    valorFactor = Convert.ToDouble(itemAux2.Value, _provider);
                                                }
                                            }
                                        }
                                        dias--;
                                    }
                                    if (oRecord.Fields.Item("U_ESFIJA").Value.ToString().Equals("Y"))
                                    {
                                        valorTC = 1 / valorFactor;
                                    }
                                    else
                                    {
                                        valorTC = valorTC / valorFactor;
                                    }
                                }
                                tipoCambiario.Valor = valorTC;
                                _registros.TipoCambiario.Add(tipoCambiario);
                            }
                        }
                    }
                    oRecord.MoveNext();
                }
            }
            Local.FuncionesComunes.LiberarObjetoGenerico(oRecord);

            //Integrar indicadores en SAP
            if (_registros.TipoCambiario != null && _registros.TipoCambiario.Count > 0)
            {
                _registros.TipoCambiario.ForEach(x =>
                {
                    string _currency = x.Indicador;
                    oRecord = (SAPbobsCOM.Recordset)SBO.ConexionSBO.oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);
                    _query = @"SELECT 1 AS ""Exist"" FROM ""OCRN"" WHERE ""CurrCode"" = '" + x.Indicador + "'";
                    oRecord.DoQuery(_query);
                    if (oRecord.RecordCount > 0)
                    {
                        //DateTime _date = Convert.ToDateTime(x.Fecha.ToString("YYYY-MM-DD"));
                        DateTime _date = Convert.ToDateTime(string.Format("{0:YYYY-MM-DD}", x.Fecha.ToString().Trim()), CultureInfo.CurrentCulture);

                        double _rate = Convert.ToDouble(x.Valor, _provider);
                        int dias = -1;
                        while (_rate.Equals(0))
                        {
                            try
                            {
                                var Rec = oSBObob.GetCurrencyRate(_currency, _date.AddDays(dias));
                                _rate = Convert.ToDouble(Rec.Fields.Item(0).Value.ToString());
                                Local.FuncionesComunes.LiberarObjetoGenerico(Rec);
                            }
                            catch
                            {
                                _rate = 0;
                            }
                            dias--;
                        }
                        if (!_rate.Equals(double.NaN) && !_rate.Equals(0))
                        {
                            oSBObob.SetCurrencyRate(_currency, _date, _rate, true);
                        }
                    }
                    Local.FuncionesComunes.LiberarObjetoGenerico(oRecord);
                });
            }
            Local.FuncionesComunes.LiberarObjetoGenerico(oSBObob);
        }
    }
}
