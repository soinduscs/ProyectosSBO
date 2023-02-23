using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAPbouiCOM.Framework;

namespace Soindus.AddOnEmisionDTE.SBO
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
            switch (BusinessObjectInfo.FormTypeEx)
            {
                // Factura
                case "133":
                    Formularios.F133.Form_DataEvent(ref BusinessObjectInfo, out BubbleEvent);
                    break;
                // Entrega
                case "140":
                    Formularios.F140.Form_DataEvent(ref BusinessObjectInfo, out BubbleEvent);
                    break;
                // Nota de Credito
                case "179":
                    Formularios.F179.Form_DataEvent(ref BusinessObjectInfo, out BubbleEvent);
                    break;
                // Traslado
                case "940":
                    Formularios.F940.Form_DataEvent(ref BusinessObjectInfo, out BubbleEvent);
                    break;
                // Factura / Boleta de Reserva
                case "60091":
                    Formularios.F60091.Form_DataEvent(ref BusinessObjectInfo, out BubbleEvent);
                    break;
                // Factura Exenta
                case "65302":
                    Formularios.F65302.Form_DataEvent(ref BusinessObjectInfo, out BubbleEvent);
                    break;
                // Nota de Debito
                case "65303":
                    Formularios.F65303.Form_DataEvent(ref BusinessObjectInfo, out BubbleEvent);
                    break;
                // Boleta
                case "65304":
                    Formularios.F65304.Form_DataEvent(ref BusinessObjectInfo, out BubbleEvent);
                    break;
                // Boleta Exenta
                case "65305":
                    Formularios.F65305.Form_DataEvent(ref BusinessObjectInfo, out BubbleEvent);
                    break;
                // Factura de Exportación
                case "65307":
                    Formularios.F65307.Form_DataEvent(ref BusinessObjectInfo, out BubbleEvent);
                    break;
                default:
                    break;
            }
        }

        private void SBO_Application_ItemEvent(string FormUID, ref SAPbouiCOM.ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            switch (FormUID)
            {
                case "Form1":
                    //Formularios.Form1.Form_ItemEvent(FormUID, ref pVal, out BubbleEvent);
                    break;
                case "frmGeneradorDTE":
                    Formularios.frmGeneradorDTE.Form_ItemEvent(FormUID, ref pVal, out BubbleEvent);
                    break;
            }

            switch (pVal.FormType)
            {
                case 133:
                    Formularios.F133.Form_ItemEvent(FormUID, ref pVal, out BubbleEvent);
                    break;
                case 140:
                    Formularios.F140.Form_ItemEvent(FormUID, ref pVal, out BubbleEvent);
                    break;
                case 179:
                    Formularios.F179.Form_ItemEvent(FormUID, ref pVal, out BubbleEvent);
                    break;
                case 940:
                    Formularios.F940.Form_ItemEvent(FormUID, ref pVal, out BubbleEvent);
                    break;
                case 60091:
                    Formularios.F60091.Form_ItemEvent(FormUID, ref pVal, out BubbleEvent);
                    break;
                case 65302:
                    Formularios.F65302.Form_ItemEvent(FormUID, ref pVal, out BubbleEvent);
                    break;
                case 65303:
                    Formularios.F65303.Form_ItemEvent(FormUID, ref pVal, out BubbleEvent);
                    break;
                case 65304:
                    Formularios.F65304.Form_ItemEvent(FormUID, ref pVal, out BubbleEvent);
                    break;
                case 65305:
                    Formularios.F65305.Form_ItemEvent(FormUID, ref pVal, out BubbleEvent);
                    break;
                case 65307:
                    Formularios.F65307.Form_ItemEvent(FormUID, ref pVal, out BubbleEvent);
                    break;
                default:
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

                        if (oMenus.Exists("AddOnEmisionDTE.Menu"))
                        {
                            oMenus.RemoveEx("AddOnEmisionDTE.Menu");
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

                        if (oMenus.Exists("AddOnEmisionDTE.Menu"))
                        {
                            oMenus.RemoveEx("AddOnEmisionDTE.Menu");
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

                        if (oMenus.Exists("AddOnEmisionDTE.Menu"))
                        {
                            oMenus.RemoveEx("AddOnEmisionDTE.Menu");
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
