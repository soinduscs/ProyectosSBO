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

namespace Soindus.Svc.Base64DTE
{
    public partial class Base64DTEService : ServiceBase
    {
        private Local.ParametrosSvc Parametros;
        private string txtLog;
        private Timer timer0;
        private static Local.Configuracion ExtConf;

        public Base64DTEService()
        {
            InitializeComponent();
            eventLog1 = new System.Diagnostics.EventLog();
            if (!System.Diagnostics.EventLog.SourceExists("Svc.Base64DTE"))
            {
                System.Diagnostics.EventLog.CreateEventSource(
                    "Svc.Base64DTE", "Soindus");
            }
            eventLog1.Source = "Svc.Base64DTE";
            eventLog1.Log = "Soindus";
        }

        protected override void OnStart(string[] args)
        {
            eventLog1.WriteEntry("Servicio Svc.Base64DTE Iniciando...", EventLogEntryType.Information, Convert.ToInt32(Local.Enumeradores.ProcesosSvcBase64DTE.Inicio));
            try
            {
                new SBO.ConexionDIAPI();
            }
            catch (Exception ex)
            {
                AlertOnProcess("(Conexión DIAPI...) " + ex.Message, Convert.ToInt32(Local.Enumeradores.ProcesosSvcBase64DTE.Conexion));
                StopService();
            }
            Parametros = new Local.ParametrosSvc();
            Timer timer = new Timer();
            //timer.Interval = 60000; // 60 seconds
            //timer.Interval = TimeSpan.FromSeconds(60).TotalMilliseconds; // 60 seconds
            timer.Interval = TimeSpan.FromMinutes(Parametros.TimerMN).TotalMilliseconds;
            timer.Elapsed += new ElapsedEventHandler(this.OnTimer);
            timer.Start();

            timer0 = new Timer();
            timer0.Interval = TimeSpan.FromSeconds(10).TotalMilliseconds;
            timer0.Elapsed += new ElapsedEventHandler(this.OnTimer0);
            timer0.Start();

            eventLog1.WriteEntry("Servicio Svc.Base64DTE iniciado correctamente.", EventLogEntryType.Information, Convert.ToInt32(Local.Enumeradores.ProcesosSvcBase64DTE.Inicio));
            //Proceso();
        }

        protected override void OnStop()
        {
            eventLog1.WriteEntry("Servicio Svc.Base64DTE detenido correctamente.", EventLogEntryType.Information, Convert.ToInt32(Local.Enumeradores.ProcesosSvcBase64DTE.Stop));
        }

        protected override void OnPause()
        {
            eventLog1.WriteEntry("Servicio Svc.Base64DTE pausado correctamente.", EventLogEntryType.Information, Convert.ToInt32(Local.Enumeradores.ProcesosSvcBase64DTE.Pausa));
        }

        protected override void OnContinue()
        {
            eventLog1.WriteEntry("Servicio Svc.Base64DTE re-iniciado correctamente.", EventLogEntryType.Information, Convert.ToInt32(Local.Enumeradores.ProcesosSvcBase64DTE.Re_Inicio));
        }

        private void AlertOnProcess(string message, int process)
        {
            eventLog1.WriteEntry(string.Format("Servicio Svc.Base64DTE Alerta - {0}", message), EventLogEntryType.Warning, process);
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
                eventLog1.WriteEntry(string.Format("Servicio Svc.Base64DTE procesando..."), EventLogEntryType.Information, Convert.ToInt32(Local.Enumeradores.ProcesosSvcBase64DTE.Proceso));
                ExtConf = new Local.Configuracion();
                eventLog1.WriteEntry(string.Format("Servicio Svc.Base64DTE consultando y actualizando documentos..."), EventLogEntryType.Information, Convert.ToInt32(Local.Enumeradores.ProcesosSvcBase64DTE.Proceso));
                //SBO.IntegracionSBO integracionSBO = new SBO.IntegracionSBO();
                //integracionSBO.ActualizarBase64();
                //integracionSBO.Dispose();
                using (SBO.IntegracionSBO integracionSBO = new SBO.IntegracionSBO())
                {
                    integracionSBO.ActualizarBase64();
                    integracionSBO.Dispose();
                }
                eventLog1.WriteEntry(string.Format("Servicio Svc.Base64DTE en espera..."), EventLogEntryType.Information, Convert.ToInt32(Local.Enumeradores.ProcesosSvcBase64DTE.Proceso));
            }
            catch (Exception ex)
            {
                AlertOnProcess("(Procesando...) " + ex.Message, Convert.ToInt32(Local.Enumeradores.ProcesosSvcBase64DTE.Proceso));
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
