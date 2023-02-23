using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soindus.Interfaces.Rindegastos.Clases
{
    public class EstadoIntegracion
    {
        public int? Id { get; set; }
        public int? IntegrationStatus { get; set; }
        public string IntegrationCode { get; set; }
        public string IntegrationDate { get; set; }
    }

    public class EstadoPersonalizado
    {
        public int? Id { get; set; }
        public int? IdAdmin { get; set; }
        public string CustomStatus { get; set; }
        public string CustomMessage { get; set; }
    }

    public class CreaFondo
    {
        public int? IdEmployee { get; set; }
        public int? IdAdmin { get; set; }
        public string FundName { get; set; }
        public string FundCurrency { get; set; }
        public string FundCode { get; set; }
        public double? FundAmount { get; set; }
        public string FundComment { get; set; }
        public bool FundFlexibility { get; set; }
        public bool FundAutoDeposit { get; set; }
        public bool FundAutoBlock { get; set; }
        public bool FundExpiration { get; set; }
        public string FundExpirationDate { get; set; }
    }

    public class ModificaFondo
    {
        public int? Id { get; set; }
        public int? IdAdmin { get; set; }
        public string FundName { get; set; }
        public string FundCode { get; set; }
        public string FundComment { get; set; }
        public bool FundFlexibility { get; set; }
        public bool FundAutoDeposit { get; set; }
        public bool FundAutoBlock { get; set; }
        public bool FundExpiration { get; set; }
        public string FundExpirationDate { get; set; }
    }

    public class DepositaFondo
    {
        public int? Id { get; set; }
        public int? IdAdmin { get; set; }
        public double? DepositAmount { get; set; }
    }

    public class EstadoFondo
    {
        public int? Id { get; set; }
        public int? IdAdmin { get; set; }
        public int? FundStatus { get; set; }
    }
}
