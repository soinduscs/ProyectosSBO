using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soindus.Svc.Base64DTE.Local
{
    public class ParametrosSvc
    {
        public ConexionDIAPI Conexion { get; set; }
        public double TimerMN { get; set; }

        public ParametrosSvc()
        {
            SetParametros();
        }

        private void SetParametros()
        {
            Conexion = new ConexionDIAPI();
            Conexion.DbServerType = ConfigurationManager.AppSettings["DbServerType"].ToString();
            Conexion.Server = ConfigurationManager.AppSettings["Server"].ToString();
            Conexion.LicenseServer = ConfigurationManager.AppSettings["LicenseServer"].ToString();
            Conexion.CompanyDB = ConfigurationManager.AppSettings["CompanyDB"].ToString();
            Conexion.UserName = ConfigurationManager.AppSettings["UserName"].ToString();
            Conexion.Password = ConfigurationManager.AppSettings["Password"].ToString();
            Conexion.DbUserName = ConfigurationManager.AppSettings["DbUserName"].ToString();
            Conexion.DbPassword = ConfigurationManager.AppSettings["DbPassword"].ToString();
            TimerMN = Convert.ToDouble(ConfigurationManager.AppSettings["TimerMN"].ToString());
        }
    }

    public class ConexionDIAPI
    {
        public string DbServerType { get; set; }
        public string Server { get; set; }
        public string LicenseServer { get; set; }
        public string CompanyDB { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string DbUserName { get; set; }
        public string DbPassword { get; set; }
    }
}
