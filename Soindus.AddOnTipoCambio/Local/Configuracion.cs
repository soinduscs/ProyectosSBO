using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Soindus.AddOnTipoCambio.Local
{
    public class Configuracion
    {
        public ConfiguracionParams Parametros { get; set; }

        public Configuracion()
        {
            Parametros = new ConfiguracionParams();
            try
            {
                Parametros = SBO.ConsultasSBO.ObtenerConfiguracion();
                Parametros = ObtenerParametrosToken(Parametros.TOKEN);
            }
            catch (Exception)
            {
                Parametros.TOKEN = string.Empty;
                Parametros.RUTSOC = string.Empty;
                Parametros.Portador_TC = string.Empty;
                Parametros.User_WS = string.Empty;
                Parametros.Pass_WS = string.Empty;
                Parametros.Rut_Sociedad = string.Empty;
            }
        }

        private ConfiguracionParams ObtenerParametrosToken(string Token)
        {
            ConfiguracionParams result = new ConfiguracionParams();
            // Decodificar el string base 64
            string origen = Token.Replace("dG9rZW4gZGUgc2VndXJpZGFk.", "");
            Byte[] datos = Convert.FromBase64String(origen);
            origen = Encoding.UTF8.GetString(datos);
            result = JsonConvert.DeserializeObject<ConfiguracionParams>(origen);
            result.TOKEN = Token;
            result.RUTSOC = SBO.ConsultasSBO.ObtenerRutSociedad();
            return result;
        }
    }

    public class ConfiguracionParams
    {
        public string TOKEN { get; set; }
        public string RUTSOC { get; set; }
        [JsonProperty("PORTADOR_TC")]
        public string Portador_TC { get; set; }
        [JsonProperty("USER_WS")]
        public string User_WS { get; set; }
        [JsonProperty("PWD_WS")]
        public string Pass_WS { get; set; }
        [JsonProperty("RUT_SOCIEDAD")]
        public string Rut_Sociedad { get; set; }
    }
}
