using System;
using SAPbouiCOM.Framework;

namespace Soindus.AddOnMonitorProveedores.SBO
{
    class Menu
    {
        public void AddMenuItems()
        {
            SAPbouiCOM.Menus oMenus = null;
            SAPbouiCOM.MenuItem oMenuItem = null;

            oMenus = Application.SBO_Application.Menus;

            if (oMenus.Exists("Soindus.Consulting"))
            {
                //oMenus.RemoveEx("Soindus.Consulting");
            }

            SAPbouiCOM.MenuCreationParams oCreationPackage = null;
            oCreationPackage = ((SAPbouiCOM.MenuCreationParams)(Application.SBO_Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_MenuCreationParams)));
            oMenuItem = Application.SBO_Application.Menus.Item("43520"); // moudles'

            oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_POPUP;
            oCreationPackage.UniqueID = "Soindus.Consulting";
            oCreationPackage.String = "Soindus Consulting";
            oCreationPackage.Enabled = true;
            oCreationPackage.Position = -1;

            string sPath = null;

            sPath = System.Windows.Forms.Application.StartupPath.ToString();
            sPath += "\\";
            sPath = sPath + "soindus.jpg";
            oCreationPackage.Image = sPath;

            oMenus = oMenuItem.SubMenus;

            try
            {
                //  If the manu already exists this code will fail
                oMenus.AddEx(oCreationPackage);
            }
            catch (Exception e)
            {

            }

            try
            {
                // Get the menu collection of the newly added pop-up item
                oMenuItem = Application.SBO_Application.Menus.Item("Soindus.Consulting");
                oMenus = oMenuItem.SubMenus;

                if (oMenus.Exists("AddOnMonitorProveedores.Menu"))
                {
                    oMenus.RemoveEx("AddOnMonitorProveedores.Menu");
                }

                // Create s sub menu
                oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_POPUP;
                oCreationPackage.UniqueID = "AddOnMonitorProveedores.Menu";
                oCreationPackage.String = "Monitor de proveedores";
                oCreationPackage.Image = "";
                oMenus.AddEx(oCreationPackage);
               
            }
            catch (Exception er)
            { //  Menu already exists
                Application.SBO_Application.SetStatusBarMessage("Menu Already Exists", SAPbouiCOM.BoMessageTime.bmt_Short, true);
            }

            try
            {
                // Get the menu collection of the newly added pop-up item
                oMenuItem = Application.SBO_Application.Menus.Item("AddOnMonitorProveedores.Menu");
                oMenus = oMenuItem.SubMenus;

                if (oMenus.Exists("Soindus.Formularios.frmMonitor"))
                {
                    oMenus.RemoveEx("Soindus.Formularios.frmMonitor");
                }

                // Create s sub menu
                oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_STRING;
                oCreationPackage.UniqueID = "Soindus.Formularios.frmMonitor";
                oCreationPackage.String = "Documentos de proveedores";
                oCreationPackage.Image = "";
                oMenus.AddEx(oCreationPackage);
            }
            catch (Exception er)
            { //  Menu already exists
                Application.SBO_Application.SetStatusBarMessage("Menu Already Exists", SAPbouiCOM.BoMessageTime.bmt_Short, true);
            }

            try
            {
                // Get the menu collection of the newly added pop-up item
                oMenuItem = Application.SBO_Application.Menus.Item("AddOnMonitorProveedores.Menu");
                oMenus = oMenuItem.SubMenus;

                if (oMenus.Exists("Soindus.Formularios.frmMonConf"))
                {
                    oMenus.RemoveEx("Soindus.Formularios.frmMonConf");
                }

                // Create s sub menu
                oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_STRING;
                oCreationPackage.UniqueID = "Soindus.Formularios.frmMonConf";
                oCreationPackage.String = "Configuración";
                oCreationPackage.Image = "";
                oMenus.AddEx(oCreationPackage);
            }
            catch (Exception er)
            { //  Menu already exists
                Application.SBO_Application.SetStatusBarMessage("Menu Already Exists", SAPbouiCOM.BoMessageTime.bmt_Short, true);
            }
        }

        public void SBO_Application_MenuEvent(ref SAPbouiCOM.MenuEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            try
            {
                if (pVal.BeforeAction && pVal.MenuUID == "Soindus.Formularios.frmMonitor")
                {
                    try
                    {
                        Application.SBO_Application.Forms.Item("frmMonitor").Select();
                    }
                    catch
                    {
                        Formularios.frmMonitor activeForm = new Formularios.frmMonitor();
                        activeForm.Show();
                    }
                }
                if (pVal.BeforeAction && pVal.MenuUID == "Soindus.Formularios.frmMonConf")
                {
                    try
                    {
                        Application.SBO_Application.Forms.Item("frmMonConf").Select();
                    }
                    catch
                    {
                        Formularios.frmMonConf activeForm = new Formularios.frmMonConf();
                        activeForm.Show();
                    }
                }
            }
            catch (Exception ex)
            {
                Application.SBO_Application.MessageBox(ex.ToString(), 1, "Ok", "", "");
            }
        }

    }
}
