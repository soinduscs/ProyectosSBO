using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soindus.AddOnRindegastos.Clases
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
                Parametros.Token = string.Empty;
                Parametros.Rut_Sociedad = string.Empty;
                Parametros.Localizacion = string.Empty;
                Parametros.Campo_Centro_Costo = "Centro de Costo";
                Parametros.Campo_Centro_Costo_Valor = "CODE";
                Parametros.Campo_Tipo_Documento = "Tipo de Documento";
                Parametros.Campo_Tipo_Documento_Valor = "CODE";
                Parametros.Codigo_Factura_Afecta = "01";
                Parametros.Codigo_Factura_Exenta = "02";
                Parametros.Codigo_Factura_Materiales = "04";
                Parametros.Campo_Rut_Proveedor = "RUT";
                Parametros.Campo_Rut_Proveedor_Valor = "VALUE";
                Parametros.Campo_Numero_Documento = "Número de Documento";
                Parametros.Campo_Numero_Documento_Valor = "VALUE";
                Parametros.Cuenta_Anticipo_Proveedores = string.Empty;
                Parametros.Cuenta_Servicios_Proveedores = string.Empty;
                Parametros.Cuenta_Compra_Materiales = string.Empty;
                Parametros.Cuenta_Compensacion_Facturas = string.Empty;
                Parametros.Cuenta_Pago_Reembolsos = string.Empty;
            }
        }
    }

    public class ConfiguracionParams
    {
        public string Token { get; set; }
        public string Rut_Sociedad { get; set; }
        public string Localizacion { get; set; }
        public string Campo_Centro_Costo { get; set; }
        public string Campo_Centro_Costo_Valor { get; set; }
        public string Campo_Tipo_Documento { get; set; }
        public string Campo_Tipo_Documento_Valor { get; set; }
        public string Codigo_Factura_Afecta { get; set; }
        public string Codigo_Factura_Exenta { get; set; }
        public string Codigo_Factura_Materiales { get; set; }
        public string Campo_Rut_Proveedor { get; set; }
        public string Campo_Rut_Proveedor_Valor { get; set; }
        public string Campo_Numero_Documento { get; set; }
        public string Campo_Numero_Documento_Valor { get; set; }
        public string Cuenta_Anticipo_Proveedores { get; set; }
        public string Cuenta_Servicios_Proveedores { get; set; }
        public string Cuenta_Compra_Materiales { get; set; }
        public string Cuenta_Compensacion_Facturas { get; set; }
        public string Cuenta_Pago_Reembolsos { get; set; }
    }
}
