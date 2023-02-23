using SAPbouiCOM.Framework;

namespace Soindus.AddOnFactoringCob.SBO
{
    public class EventosSBO
    {
        public EventosSBO()
        {
            // Creación de Menu
            Menu MyMenu = new Menu();
            MyMenu.AddMenuItems();

            // Eventos de Menu
            ConexionSBO.oApp.RegisterMenuEventHandler(MyMenu.SBO_Application_MenuEvent);

            // Eventos SBO
            Application.SBO_Application.AppEvent += new SAPbouiCOM._IApplicationEvents_AppEventEventHandler(SBO_Application_AppEvent);
            Application.SBO_Application.ItemEvent += new SAPbouiCOM._IApplicationEvents_ItemEventEventHandler(SBO_Application_ItemEvent);
            Application.SBO_Application.FormDataEvent += SBO_Application_FormDataEvent;

            // Iniciar aplicacion SBO
            ConexionSBO.oApp.Run();
        }

        private void SBO_Application_FormDataEvent(ref SAPbouiCOM.BusinessObjectInfo BusinessObjectInfo, out bool BubbleEvent)
        {
            BubbleEvent = true;
        }

        private void SBO_Application_ItemEvent(string FormUID, ref SAPbouiCOM.ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            switch (FormUID)
            {
                case "Form1":
                    //Formularios.Form1.Form_ItemEvent(FormUID, ref pVal, out BubbleEvent);
                    break;
                case "frmFacSelect":
                    Formularios.frmFacSelect.Form_ItemEvent(FormUID, ref pVal, out BubbleEvent);
                    break;
                case "frmFactoring":
                    Formularios.frmFactoring.Form_ItemEvent(FormUID, ref pVal, out BubbleEvent);
                    break;
                case "frmFacList":
                    Formularios.frmFacList.Form_ItemEvent(FormUID, ref pVal, out BubbleEvent);
                    break;
                case "frmFacCuentaSN":
                    Formularios.frmFacCuentaSN.Form_ItemEvent(FormUID, ref pVal, out BubbleEvent);
                    break;
                case "frmCobGestion":
                    Formularios.frmCobGestion.Form_ItemEvent(FormUID, ref pVal, out BubbleEvent);
                    break;
                case "frmCobSelect":
                    Formularios.frmCobSelect.Form_ItemEvent(FormUID, ref pVal, out BubbleEvent);
                    break;
                case "frmCobLog":
                    Formularios.frmCobLog.Form_ItemEvent(FormUID, ref pVal, out BubbleEvent);
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

                        if (oMenus.Exists("AddOnFactoringCob.Menu"))
                        {
                            oMenus.RemoveEx("AddOnFactoringCob.Menu");
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

                        if (oMenus.Exists("AddOnFactoringCob.Menu"))
                        {
                            oMenus.RemoveEx("AddOnFactoringCob.Menu");
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

                        if (oMenus.Exists("AddOnFactoringCob.Menu"))
                        {
                            oMenus.RemoveEx("AddOnFactoringCob.Menu");
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
