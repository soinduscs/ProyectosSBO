using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soindus.AddOnTipoCambio.Local
{
    public class ValorValido
    {
        public String valor { get; set; }
        public String descripcion { get; set; }

        public ValorValido(string valor, string descripcion)
        {
            this.valor = valor;
            this.descripcion = descripcion;
        }
    }
}
