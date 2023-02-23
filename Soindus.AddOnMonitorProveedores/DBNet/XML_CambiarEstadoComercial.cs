using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soindus.AddOnMonitorProveedores.DBNet
{
    public class XML_CambiarEstadoComercial
    {
        public get_CambiarEstadoComercial Generar(string company = "", string companyCodeSii = "", string documentType = "", string documentNumber = "", string statusCode = "", string reasonDesc = "")
        {
            get_CambiarEstadoComercial cambiarestadocomercial = new get_CambiarEstadoComercial();
            cambiarestadocomercial.company = company;
            cambiarestadocomercial.companyCodeSii = companyCodeSii;
            cambiarestadocomercial.documentType = documentType;
            cambiarestadocomercial.documentNumber = documentNumber;
            cambiarestadocomercial.statusCode = statusCode;
            cambiarestadocomercial.reasonDesc = reasonDesc;
            return cambiarestadocomercial;
        }
    }

    public class get_CambiarEstadoComercial
    {
        public string company { get; set; }
        public string companyCodeSii { get; set; }
        public string documentType { get; set; }
        public string documentNumber { get; set; }
        public string statusCode { get; set; }
        public string reasonDesc { get; set; }
    }
}
