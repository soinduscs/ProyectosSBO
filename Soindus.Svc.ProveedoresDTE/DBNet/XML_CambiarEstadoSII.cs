using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soindus.Svc.ProveedoresDTE.DBNet
{
    public class XML_CambiarEstadoSII
    {
        public get_CambiarEstadoSII Generar(string company = "", string companyCodeSii = "", string documentType = "", string documentNumber = "", string statusCode = "")
        {
            get_CambiarEstadoSII cambiarestadosii = new get_CambiarEstadoSII();
            cambiarestadosii.company = company;
            cambiarestadosii.companyCodeSii = companyCodeSii;
            cambiarestadosii.documentType = documentType;
            cambiarestadosii.documentNumber = documentNumber;
            cambiarestadosii.statusCode = statusCode;
            return cambiarestadosii;
        }
    }

    public class get_CambiarEstadoSII
    {
        public string company { get; set; }
        public string companyCodeSii { get; set; }
        public string documentType { get; set; }
        public string documentNumber { get; set; }
        public string statusCode { get; set; }
    }
}
