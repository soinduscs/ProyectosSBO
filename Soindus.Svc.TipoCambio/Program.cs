﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Soindus.Svc.TipoCambio
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            //System.Diagnostics.Debugger.Launch();
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new TipoCambioService()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
