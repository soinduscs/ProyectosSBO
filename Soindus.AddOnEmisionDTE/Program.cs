using System;
using System.Collections.Generic;
using SAPbouiCOM.Framework;

namespace Soindus.AddOnEmisionDTE
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
