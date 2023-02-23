using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soindus.AddOnMonitorProveedores.Local
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
            }
            catch (Exception)
            {
                Parametros.Proveedor_FE = string.Empty;
                Parametros.Token = string.Empty;
                Parametros.Rut_Receptor = string.Empty;
                Parametros.Recinto = string.Empty;
                Parametros.Visualiza_Responsable = false;
                Parametros.Visualiza_Responsable_Doc = false;
                Parametros.Visualiza_Cesion = false;
                Parametros.Valida_ExisteOC = true;
                Parametros.Valida_ExisteEntrada = false;
                Parametros.Valida_MontoMaximo = false;
                Parametros.Valida_ValorMontoMaximo = 0;
                Parametros.Valida_PermiteOCManual = false;
                Parametros.Valida_EncabezadosDTE = false;
                Parametros.Valida_MontoOC = false;
                Parametros.Valida_PermiteTolerancias = false;
                Parametros.Valida_ValorRechazoMontoMenor = 0;
                Parametros.Valida_ValorRechazoMontoMayor = 0;
                Parametros.Valida_ValorAprobacionMontoMenor = 0;
                Parametros.Valida_ValorAprobacionMontoMayor = 0;
                Parametros.Integracion_SolicitaMultiBranch = false;
            }
        }
    }

    public class ConfiguracionParams
    {
        public string Proveedor_FE { get; set; }
        public string Token { get; set; }
        public string Rut_Receptor { get; set; }
        public string Recinto { get; set; }
        public bool Visualiza_Responsable { get; set; }
        public bool Visualiza_Responsable_Doc { get; set; }
        public bool Visualiza_Cesion { get; set; }
        public bool Valida_ExisteOC { get; set; }
        public bool Valida_ExisteEntrada { get; set; }
        public bool Valida_MontoMaximo { get; set; }
        public double Valida_ValorMontoMaximo { get; set; }
        public bool Valida_PermiteOCManual { get; set; }
        public bool Valida_EncabezadosDTE { get; set; }
        public bool Valida_MontoOC { get; set; }
        public bool Valida_PermiteTolerancias { get; set; }
        public double Valida_ValorRechazoMontoMenor { get; set; }
        public double Valida_ValorRechazoMontoMayor { get; set; }
        public double Valida_ValorAprobacionMontoMenor { get; set; }
        public double Valida_ValorAprobacionMontoMayor { get; set; }
        public bool Integracion_SolicitaMultiBranch { get; set; }
    }
}
