using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soindus.Svc.TipoCambio.Clases
{
    public class ListaTipoCambiario
    {
        public List<TipoCambiario> TipoCambiario { get; set; }

        public ListaTipoCambiario()
        {
            TipoCambiario = new List<Clases.TipoCambiario>();
        }
    }

    public class TipoCambiario
    {
        public int ID { get; set; }

        public DateTime? Fecha { get; set; }

        public string Indicador { get; set; }

        public string Codigo { get; set; }

        public string Nominacion { get; set; }

        public double? Valor { get; set; }

        public DateTime? FechaActualizacion { get; set; }

        public string Origen { get; set; }
    }
}
