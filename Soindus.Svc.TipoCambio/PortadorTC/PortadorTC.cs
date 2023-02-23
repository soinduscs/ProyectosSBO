using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PortTC = Soindus.Interfaces.PortadoresTC;

namespace Soindus.Svc.TipoCambio
{
    public class PortadorTC
    {
        public string Portador { get; set; }
        public BCC.GetSeriesResult Respuesta { get; set; }
        private static Local.Configuracion ExtConf = new Local.Configuracion();

        public PortadorTC()
        {
            ExtConf = new Local.Configuracion();
            Portador = ExtConf.Parametros.Portador_TC;
        }

        public PortTC.Local.Message ObtenerTipoCambiario(string[] args)
        {
            string Token = string.Empty;
            string DesdeFecha = args[0];
            string HastaFecha = args[1];
            string Serie = args[2];

            PortTC.Local.Message result = new PortTC.Local.Message();
            switch (Portador)
            {
                case "BCC":
                    Token = @"" + ExtConf.Parametros.User_WS + @";" + ExtConf.Parametros.Pass_WS;
                    var portTCBCC = new PortTC.BCentral() { Token = Token };
                    PortTC.Local.Message portResultBCC = new PortTC.Local.Message();

                    portResultBCC = portTCBCC.ObtenerTipoCambiario(DesdeFecha, HastaFecha, Serie);
                    if (portResultBCC.Success)
                    {
                        var _Datos = Newtonsoft.Json.JsonConvert.DeserializeObject<BCC.GetSeriesResponse>(portResultBCC.Content);
                        var _Datos2 = _Datos.Root.GetSeriesResult;
                        Respuesta = _Datos2;
                        if (!Respuesta.Codigo.Equals(0))
                        {
                            portResultBCC.Mensaje = string.Format("Obtener TC - Código de respuesta: {0}-{1}", Respuesta.Codigo, Respuesta.Descripcion);
                            portResultBCC.Success = false;
                        }
                    }
                    result = portResultBCC;
                    break;
                default:
                    break;
            }
            return result;
        }
    }
}
