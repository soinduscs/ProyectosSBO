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

namespace Soindus.Svc.TipoCambio
{
    public partial class TipoCambioService : ServiceBase
    {
        private Local.ParametrosSvc Parametros;
        private string txtLog;
        private Timer timer0;
        private static Local.Configuracion ExtConf;

        public TipoCambioService()
        {
            InitializeComponent();
            eventLog1 = new System.Diagnostics.EventLog();
            if (!System.Diagnostics.EventLog.SourceExists("Svc.TipoCambio"))
            {
                System.Diagnostics.EventLog.CreateEventSource(
                    "Svc.TipoCambio", "Soindus");
            }
            eventLog1.Source = "Svc.TipoCambio";
            eventLog1.Log = "Soindus";
        }

        protected override void OnStart(string[] args)
        {
            eventLog1.WriteEntry("Servicio Svc.TipoCambio Iniciando...", EventLogEntryType.Information, Convert.ToInt32(Local.Enumeradores.ProcesosSvcTC.Inicio));
            try
            {
                new SBO.ConexionDIAPI();
            }
            catch (Exception ex)
            {
                AlertOnProcess("(Conexión DIAPI...) " + ex.Message, Convert.ToInt32(Local.Enumeradores.ProcesosSvcTC.Conexion));
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

            eventLog1.WriteEntry("Servicio Svc.TipoCambio iniciado correctamente.", EventLogEntryType.Information, Convert.ToInt32(Local.Enumeradores.ProcesosSvcTC.Inicio));
            //Proceso();
        }

        protected override void OnStop()
        {
            eventLog1.WriteEntry("Servicio Svc.TipoCambio detenido correctamente.", EventLogEntryType.Information, Convert.ToInt32(Local.Enumeradores.ProcesosSvcTC.Stop));
        }

        protected override void OnPause()
        {
            eventLog1.WriteEntry("Servicio Svc.TipoCambio pausado correctamente.", EventLogEntryType.Information, Convert.ToInt32(Local.Enumeradores.ProcesosSvcTC.Pausa));
        }

        protected override void OnContinue()
        {
            eventLog1.WriteEntry("Servicio Svc.TipoCambio re-iniciado correctamente.", EventLogEntryType.Information, Convert.ToInt32(Local.Enumeradores.ProcesosSvcTC.Re_Inicio));
        }

        private void AlertOnProcess(string message, int process)
        {
            eventLog1.WriteEntry(string.Format("Servicio Svc.TipoCambio Alerta - {0}", message), EventLogEntryType.Warning, process);
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
                eventLog1.WriteEntry(string.Format("Servicio Svc.TipoCambio procesando..."), EventLogEntryType.Information, Convert.ToInt32(Local.Enumeradores.ProcesosSvcTC.Proceso));
                ExtConf = new Local.Configuracion();
                if (!ExtConf.Parametros.RUTSOC.Equals(ExtConf.Parametros.Rut_Sociedad))
                {
                    eventLog1.WriteEntry(string.Format("Servicio Svc.TipoCambio - La sociedad actual no se encuentra habilitada. Verifique Token..."), EventLogEntryType.Warning, Convert.ToInt32(Local.Enumeradores.ProcesosSvcTC.Error));
                }
                else
                {
                    SBO.IntegracionSBO integracionSBO = new SBO.IntegracionSBO();
                    eventLog1.WriteEntry(string.Format("Servicio Svc.TipoCambio cargando e integrando tipos de cambio..."), EventLogEntryType.Information, Convert.ToInt32(Local.Enumeradores.ProcesosSvcTC.Proceso));
                    integracionSBO.ObtenerIndicadores(DateTime.Now.ToString("yyyyMMdd"), DateTime.Now.ToString("yyyyMMdd"));
                }
                eventLog1.WriteEntry(string.Format("Servicio Svc.TipoCambio en espera..."), EventLogEntryType.Information, Convert.ToInt32(Local.Enumeradores.ProcesosSvcTC.Proceso));
            }
            catch (Exception ex)
            {
                AlertOnProcess("(Procesando...) " + ex.Message, Convert.ToInt32(Local.Enumeradores.ProcesosSvcTC.Proceso));
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
