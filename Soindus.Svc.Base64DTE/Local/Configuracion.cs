using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Newtonsoft.Json;

namespace Soindus.Svc.Base64DTE.Local
{
    public class Configuracion
    {
        public ConfiguracionParams Parametros { get; set; }

        public Configuracion()
        {
            Parametros = new ConfiguracionParams();
            try
            {
                Parametros = ObtenerParametrosToken();
            }
            catch (Exception)
            {
                Parametros.Proveedor_FE = string.Empty;
                Parametros.Rut_Emisor = string.Empty;
                Parametros.Url_WS_DTE = string.Empty;
                Parametros.User_WS = string.Empty;
                Parametros.Pass_WS = string.Empty;
            }
        }

        private ConfiguracionParams ObtenerParametrosToken()
        {
            ConfiguracionParams result = new ConfiguracionParams();
            string Token = string.Empty;
            //Token = ConfigurationManager.AppSettings["Token"].ToString();
            Token = SBO.ConsultasSBO.ObtenerTokenConexionFE();
            // Decodificar el string base 64
            string origen = Token;
            Byte[] datos = Convert.FromBase64String(origen);
            origen = Encoding.UTF8.GetString(datos);
            result = JsonConvert.DeserializeObject<ConfiguracionParams>(origen);
            result.Rut_Emisor = SBO.ConsultasSBO.ObtenerRutEmpresaEmisora();
            return result;
        }
    }

    public class ConfiguracionParams
    {
        [JsonProperty("PROVEEDOR_FE")]
        public string Proveedor_FE { get; set; }
        [JsonProperty("URL_WS_DTE")]
        public string Url_WS_DTE { get; set; }
        [JsonProperty("USER_WS")]
        public string User_WS { get; set; }
        [JsonProperty("PWD_WS")]
        public string Pass_WS { get; set; }
        [JsonProperty("RUT_EMISOR")]
        public string Rut_Emisor { get; set; }
    }
}
