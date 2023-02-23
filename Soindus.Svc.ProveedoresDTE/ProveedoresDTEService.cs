using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Comun = Soindus.Clases.Comun;

namespace Soindus.Svc.ProveedoresDTE
{
    public partial class ProveedoresDTEService : ServiceBase
    {
        private Local.ParametrosSvc Parametros;
        private string txtLog;
        private Timer timer0;

        public ProveedoresDTEService()
        {
            InitializeComponent();
            eventLog1 = new System.Diagnostics.EventLog();
            if (!System.Diagnostics.EventLog.SourceExists("Svc.ProveedoresDTE"))
            {
                System.Diagnostics.EventLog.CreateEventSource(
                    "Svc.ProveedoresDTE", "Soindus");
            }
            eventLog1.Source = "Svc.ProveedoresDTE";
            eventLog1.Log = "Soindus";
        }

        protected override void OnStart(string[] args)
        {
            eventLog1.WriteEntry("Servicio Svc.ProveedoresDTE Iniciando...", EventLogEntryType.Information, Convert.ToInt32(Comun.Enumeradores.ProcesosSvcDTE.Inicio));
            try
            {
                new SBO.ConexionDIAPI();
            }
            catch (Exception ex)
            {
                AlertOnProcess("(Conexión DIAPI...) " + ex.Message, Convert.ToInt32(Comun.Enumeradores.ProcesosSvcDTE.Conexion));
                StopService();
            }
            Parametros = new Local.ParametrosSvc();
            Timer timer = new Timer();
            //timer.Interval = 60000; // 60 seconds
            //timer.Interval = TimeSpan.FromSeconds(60).TotalMilliseconds; // 60 seconds
            timer.Interval = TimeSpan.FromHours(Parametros.TimerHR).TotalMilliseconds;
            timer.Elapsed += new ElapsedEventHandler(this.OnTimer);
            timer.Start();

            timer0 = new Timer();
            timer0.Interval = TimeSpan.FromSeconds(10).TotalMilliseconds;
            timer0.Elapsed += new ElapsedEventHandler(this.OnTimer0);
            timer0.Start();

            eventLog1.WriteEntry("Servicio Svc.ProveedoresDTE iniciado correctamente.", EventLogEntryType.Information, Convert.ToInt32(Comun.Enumeradores.ProcesosSvcDTE.Inicio));
            //Proceso();
        }

        protected override void OnStop()
        {
            eventLog1.WriteEntry("Servicio Svc.ProveedoresDTE detenido correctamente.", EventLogEntryType.Information, Convert.ToInt32(Comun.Enumeradores.ProcesosSvcDTE.Stop));
        }

        protected override void OnPause()
        {
            eventLog1.WriteEntry("Servicio Svc.ProveedoresDTE pausado correctamente.", EventLogEntryType.Information, Convert.ToInt32(Comun.Enumeradores.ProcesosSvcDTE.Pausa));
        }

        protected override void OnContinue()
        {
            eventLog1.WriteEntry("Servicio Svc.ProveedoresDTE re-iniciado correctamente.", EventLogEntryType.Information, Convert.ToInt32(Comun.Enumeradores.ProcesosSvcDTE.Re_Inicio));
        }


        private void AlertOnProcess(string message, int process)
        {
            eventLog1.WriteEntry(string.Format("Servicio Svc.ProveedoresDTE Alerta - {0}", message), EventLogEntryType.Warning, process);
        }

        private void StopService()
        {
            Stop();
        }

        private void Proceso()
        {
            try
            {
                txtLog = string.Empty;
                eventLog1.WriteEntry(string.Format("Servicio Svc.ProveedoresDTE procesando..."), EventLogEntryType.Information, Convert.ToInt32(Comun.Enumeradores.ProcesosSvcDTE.Proceso));
                IntegracionSBO integracionSBO = new IntegracionSBO();
                eventLog1.WriteEntry(string.Format("Servicio Svc.ProveedoresDTE cargando documentos..."), EventLogEntryType.Information, Convert.ToInt32(Comun.Enumeradores.ProcesosSvcDTE.Proceso));
                integracionSBO.CargarDocumentosDesdeAPI();
                eventLog1.WriteEntry(string.Format("Servicio Svc.ProveedoresDTE validando documentos..."), EventLogEntryType.Information, Convert.ToInt32(Comun.Enumeradores.ProcesosSvcDTE.Proceso));
                integracionSBO.ValidarDocumentos();
                eventLog1.WriteEntry(string.Format("Servicio Svc.ProveedoresDTE integrando documentos..."), EventLogEntryType.Information, Convert.ToInt32(Comun.Enumeradores.ProcesosSvcDTE.Proceso));
                integracionSBO.IntegrarDocumentos();
                eventLog1.WriteEntry(string.Format("Servicio Svc.ProveedoresDTE en espera..."), EventLogEntryType.Information, Convert.ToInt32(Comun.Enumeradores.ProcesosSvcDTE.Proceso));
            }
            catch (Exception ex)
            {
                AlertOnProcess("(Procesando...) " + ex.Message, Convert.ToInt32(Comun.Enumeradores.ProcesosSvcDTE.Proceso));
            }
        }

        public void OnTimer(object sender, ElapsedEventArgs args)
        {
            Proceso();
        }

        public void OnTimer0(object sender, ElapsedEventArgs args)
        {
            timer0.Stop();
            Proceso();
        }
    }
}
