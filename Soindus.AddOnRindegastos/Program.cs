using System;
using System.Collections.Generic;
using SAPbouiCOM.Framework;

namespace Soindus.AddOnRindegastos
{
    class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                SBO.ConexionSBO ConexionSBO = new SBO.ConexionSBO(args);

                //SAPbobsCOM.Users oUser = (SAPbobsCOM.Users)SBO.ConexionSBO.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUsers);
                //oUser.GetByKey(1);
                //var brnch = oUser.UserBranchAssignment;

                SBO.EventosSBO EventosSBO = new SBO.EventosSBO();

                System.Windows.Forms.Application.Run();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }
    }
}
