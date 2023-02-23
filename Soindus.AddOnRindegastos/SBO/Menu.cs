using System;
using SAPbouiCOM.Framework;

namespace Soindus.AddOnRindegastos.SBO
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

                if (oMenus.Exists("AddOnRindegastos.Menu"))
                {
                    oMenus.RemoveEx("AddOnRindegastos.Menu");
                }

                // Create s sub menu
                oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_POPUP;
                oCreationPackage.UniqueID = "AddOnRindegastos.Menu";
                oCreationPackage.String = "Rindegastos";
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
                oMenuItem = Application.SBO_Application.Menus.Item("AddOnRindegastos.Menu");
                oMenus = oMenuItem.SubMenus;

                if (oMenus.Exists("Soindus.Formularios.frmMonitorRG"))
                {
                    oMenus.RemoveEx("Soindus.Formularios.frmMonitorRG");
                }

                // Create s sub menu
                oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_STRING;
                oCreationPackage.UniqueID = "Soindus.Formularios.frmMonitorRG";
                oCreationPackage.String = "Monitor Rendiciones";
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
                oMenuItem = Application.SBO_Application.Menus.Item("AddOnRindegastos.Menu");
                oMenus = oMenuItem.SubMenus;

                if (oMenus.Exists("Soindus.Formularios.frmMonConfRG"))
                {
                    oMenus.RemoveEx("Soindus.Formularios.frmMonConfRG");
                }

                // Create s sub menu
                oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_STRING;
                oCreationPackage.UniqueID = "Soindus.Formularios.frmMonConfRG";
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
                if (pVal.BeforeAction && pVal.MenuUID == "Soindus.Formularios.frmMonitorRG")
                {
                    try
                    {
                        Application.SBO_Application.Forms.Item("frmMonitorRG").Select();
                    }
                    catch
                    {
                        Formularios.frmMonitorRG activeForm = new Formularios.frmMonitorRG();
                        activeForm.Show();
                    }
                }
                if (pVal.BeforeAction && pVal.MenuUID == "Soindus.Formularios.frmMonConfRG")
                {
                    try
                    {
                        Application.SBO_Application.Forms.Item("frmMonConfRG").Select();
                    }
                    catch
                    {
                        Formularios.frmMonConfRG activeForm = new Formularios.frmMonConfRG();
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
