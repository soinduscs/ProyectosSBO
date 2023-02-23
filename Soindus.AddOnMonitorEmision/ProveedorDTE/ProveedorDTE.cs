using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProvDTE = Soindus.Interfaces.ProveedoresDTE;
using ClasesDTE = Soindus.Clases.DTE;
using Comun = Soindus.Clases.Comun;

namespace Soindus.AddOnMonitorEmision
{
    public class ProveedorDTE
    {
        public string Proveedor { get; set; }
        public ClasesDTE.DTEResponse DTEResponse { get; set; }
        public ClasesDTE.DetalleDocuDTE DetalleDocuDTE { get; set; }
        public ClasesDTE.EstadosEmision EstadosEmision { get; set; }
        private static Local.Configuracion ExtConf = new Local.Configuracion();
        private string RutSociedad = ExtConf.Parametros.Rut_Emisor;

        public ProveedorDTE()
        {
            ExtConf = new Local.Configuracion();
            switch (ExtConf.Parametros.Proveedor_FE)
            {
                case "FEB":
                    Proveedor = "Febos";
                    break;
                case "DBN":
                    Proveedor = "DBNet";
                    break;
                case "FAC":
                    Proveedor = "Facele";
                    break;
                case "AZU":
                    Proveedor = "Azurian";
                    break;
                case "SID":
                    Proveedor = "Sidge";
                    break;
                default:
                    Proveedor = "Febos";
                    break;
            }
        }

        /// <summary>
        /// Obtiene los documentos emitidos según filtros aplicados
        /// (Febos: TipoDocumento, RutEmisor, RutReceptor, FechaInicial, FechaFinal, Pagina)
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public ProvDTE.Local.Message ObtenerDocumentosEmitidos(string[] args)
        {
            string Token = string.Empty;
            string RutEmpresa = string.Empty;

            string FiltroTipo = args[0];
            string RutEmisorDTE = args[1];
            string FiltroSN = args[2];
            string DesdeFecha = args[3];
            string HastaFecha = args[4];
            string Pagina = args[5];

            ProvDTE.Local.Message result = new Interfaces.ProveedoresDTE.Local.Message();
            switch (Proveedor)
            {
                case "Febos":
                    Token = ExtConf.Parametros.TOKEN;
                    RutEmpresa = ExtConf.Parametros.Rut_Emisor;

                    var provDTEFebos = new ProvDTE.Febos() { Token = Token, RutEmpresa = RutEmpresa };
                    var provResultFebos = provDTEFebos.ObtenerDocumentosEmitidos(FiltroTipo, RutEmisorDTE, FiltroSN, DesdeFecha, HastaFecha, Pagina);
                    if (provResultFebos.Success)
                    {
                        //var _Cabecera = Newtonsoft.Json.JsonConvert.DeserializeObject<Clases.CabeceraResponse>(result.Content);
                        var _Datos = Newtonsoft.Json.JsonConvert.DeserializeObject<Febos.DTEResponse>(provResultFebos.Content);
                        var _DatosAux = Newtonsoft.Json.JsonConvert.DeserializeObject<ClasesDTE.DTEResponse>(Newtonsoft.Json.JsonConvert.SerializeObject(_Datos));
                        DTEResponse = new ClasesDTE.DTEResponse();
                        DTEResponse = _DatosAux;
                    }
                    result = provResultFebos;
                    break;
                case "DBNet":
                    result.Success = false;
                    result.Mensaje = "Proveedor DTE no implementado.";
                    break;
                default:
                    result.Success = false;
                    result.Mensaje = "Proveedor DTE no implementado.";
                    break;
            }
            return result;
        }

        public ProvDTE.Local.Message ObtenerEstado(string[] args)
        {
            ProvDTE.Local.Message result = new ProvDTE.Local.Message();
            switch (Proveedor)
            {
                case "DBNet":
                    try
                    {
                        var rut = RutSociedad.Split('-');
                        string rutemisor = rut[0];
                        DBNet.XML_Design xml_design = new DBNet.XML_Design();
                        string xml = xml_design.DesignEstado(rutemisor, args[0], args[1]);

                        var provDTEDBNet = new ProvDTE.DBNet();
                        var provResultDBNet = provDTEDBNet.ObtenerEstado(xml, ExtConf.Parametros.Url_WS_DTE);
                        if (provResultDBNet.Success)
                        {
                            var _Datos = Newtonsoft.Json.JsonConvert.DeserializeObject<DBNet.ConsultaEstadoResponse>(provResultDBNet.Content);
                            var _DatosAux = Newtonsoft.Json.JsonConvert.DeserializeObject<ClasesDTE.ConsultaEstadoResponse>(Newtonsoft.Json.JsonConvert.SerializeObject(_Datos));
                            var RespuestaConsultaEstado = _DatosAux.Root.ConsultaEstadoResult.Estado;
                            ClasesDTE.EstadosEmision estadosemision = new Clases.DTE.EstadosEmision();
                            if (string.IsNullOrEmpty(RespuestaConsultaEstado))
                            {
                                estadosemision.Estado1 = "ERROR";
                                estadosemision.MsgEstado1 = "No se obtuvo el estado del documento";
                                estadosemision.Estado2 = string.Empty;
                                estadosemision.MsgEstado2 = string.Empty;
                                estadosemision.Estado3 = string.Empty;
                                estadosemision.MsgEstado3 = string.Empty;
                                result.Mensaje = "No se obtuvo el estado del documento";
                                result.Success = false;
                            }
                            else
                            {
                                var _array = RespuestaConsultaEstado.Split(';');
                                estadosemision.Estado1 = _array[0];
                                estadosemision.MsgEstado1 = string.IsNullOrEmpty(_array[1]) ? Comun.Enumeradores.GetEstadoEmision(_array[0]) : _array[1];
                                estadosemision.Estado2 = _array[6];
                                estadosemision.MsgEstado2 = Comun.Enumeradores.GetEstadoEmision(_array[6]);
                                estadosemision.Estado3 = _array[7];
                                estadosemision.MsgEstado3 = Comun.Enumeradores.GetEstadoEmision(_array[7]);
                            }
                            EstadosEmision = estadosemision;
                        }
                        result = provResultDBNet;
                    }
                    catch (Exception ex)
                    {
                        result.Mensaje = ex.Message;
                        result.Success = false;
                    }
                    break;
                default:
                    break;
            }
            return result;
        }
    }
}
