using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAPbouiCOM.Framework;

namespace Soindus.AddOnMonitorEmision.SBO
{
    public class EventosSBO
    {
        public EventosSBO()
        {
            // Creación de Menu
            Application.SBO_Application.StatusBar.SetText("Cargando menú de la solución...", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);
            Menu MyMenu = new Menu();
            MyMenu.AddMenuItems();

            // Eventos de Menu
            ConexionSBO.oApp.RegisterMenuEventHandler(MyMenu.SBO_Application_MenuEvent);

            // Eventos SBO
            Application.SBO_Application.AppEvent += new SAPbouiCOM._IApplicationEvents_AppEventEventHandler(SBO_Application_AppEvent);
            Application.SBO_Application.ItemEvent += new SAPbouiCOM._IApplicationEvents_ItemEventEventHandler(SBO_Application_ItemEvent);

            // Iniciar aplicacion SBO
            ConexionSBO.oApp.Run();
        }

        private void SBO_Application_ItemEvent(string FormUID, ref SAPbouiCOM.ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            switch (FormUID)
            {
                case "frmXMonitor":
                    Formularios.frmXMonitor.Form_ItemEvent(FormUID, ref pVal, out BubbleEvent);
                    break;
                case "frmXMonConf":
                    Formularios.frmXMonConf.Form_ItemEvent(FormUID, ref pVal, out BubbleEvent);
                    break;
            }
        }

        static void SBO_Application_AppEvent(SAPbouiCOM.BoAppEventTypes EventType)
        {
            switch (EventType)
            {
                case SAPbouiCOM.BoAppEventTypes.aet_ShutDown:
                    //Exit Add-On
                    //System.Windows.Forms.Application.Exit();
                    try
                    {
                        SAPbouiCOM.Menus oMenus = null;
                        oMenus = Application.SBO_Application.Menus;

                        if (oMenus.Exists("AddOnMonitorEmision.Menu"))
                        {
                            oMenus.RemoveEx("SAddOnMonitorEmision.Menu");
                        }
                        SBO.ConexionSBO.oCompany.Disconnect();
                        SBO.ConexionSBO.oCompany = null;
                    }
                    catch
                    {
                    }
                    System.Environment.Exit(0);
                    break;
                case SAPbouiCOM.BoAppEventTypes.aet_CompanyChanged:
                    try
                    {
                        SAPbouiCOM.Menus oMenus = null;
                        oMenus = Application.SBO_Application.Menus;

                        if (oMenus.Exists("AddOnMonitorEmision.Menu"))
                        {
                            oMenus.RemoveEx("AddOnMonitorEmision.Menu");
                        }
                        SBO.ConexionSBO.oCompany.Disconnect();
                        SBO.ConexionSBO.oCompany = null;
                    }
                    catch
                    {
                    }
                    System.Environment.Exit(0);
                    break;
                case SAPbouiCOM.BoAppEventTypes.aet_FontChanged:
                    break;
                case SAPbouiCOM.BoAppEventTypes.aet_LanguageChanged:
                    break;
                case SAPbouiCOM.BoAppEventTypes.aet_ServerTerminition:
                    try
                    {
                        SAPbouiCOM.Menus oMenus = null;
                        oMenus = Application.SBO_Application.Menus;

                        if (oMenus.Exists("AddOnMonitorEmision.Menu"))
                        {
                            oMenus.RemoveEx("AddOnMonitorEmision.Menu");
                        }
                        SBO.ConexionSBO.oCompany.Disconnect();
                        SBO.ConexionSBO.oCompany = null;
                    }
                    catch
                    {
                    }
                    System.Environment.Exit(0);
                    break;
                default:
                    break;
            }
        }
    }
}
