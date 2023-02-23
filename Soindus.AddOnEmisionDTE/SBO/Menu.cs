using System;
using SAPbouiCOM.Framework;

namespace Soindus.AddOnEmisionDTE.SBO
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

                if (oMenus.Exists("AddOnEmisionDTE.Menu"))
                {
                    oMenus.RemoveEx("AddOnEmisionDTE.Menu");
                }

                // Create s sub menu
                oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_POPUP;
                oCreationPackage.UniqueID = "AddOnEmisionDTE.Menu";
                oCreationPackage.String = "Emisión DTE";
                oCreationPackage.Image = "";
                oMenus.AddEx(oCreationPackage);

            }
            catch (Exception er)
            { //  Menu already exists
                Application.SBO_Application.SetStatusBarMessage("Menu Already Exists", SAPbouiCOM.BoMessageTime.bmt_Short, true);
            }

            //try
            //{
            //    // Get the menu collection of the newly added pop-up item
            //    oMenuItem = Application.SBO_Application.Menus.Item("AddOnEmisionDTE.Menu");
            //    oMenus = oMenuItem.SubMenus;

            //    if (oMenus.Exists("Soindus.Formularios.Form1"))
            //    {
            //        oMenus.RemoveEx("Soindus.Formularios.Form1");
            //    }

            //    // Create s sub menu
            //    oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_STRING;
            //    oCreationPackage.UniqueID = "Soindus.Formularios.Form1";
            //    oCreationPackage.String = "Emisión DTE";
            //    oCreationPackage.Image = "";
            //    oMenus.AddEx(oCreationPackage);
            //}
            //catch (Exception er)
            //{ //  Menu already exists
            //    Application.SBO_Application.SetStatusBarMessage("Menu Already Exists", SAPbouiCOM.BoMessageTime.bmt_Short, true);
            //}

            try
            {
                // Get the menu collection of the newly added pop-up item
                oMenuItem = Application.SBO_Application.Menus.Item("AddOnEmisionDTE.Menu");
                oMenus = oMenuItem.SubMenus;

                if (oMenus.Exists("Soindus.Formularios.frmGeneradorDTE"))
                {
                    oMenus.RemoveEx("Soindus.Formularios.frmGeneradorDTE");
                }

                // Create s sub menu
                oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_STRING;
                oCreationPackage.UniqueID = "Soindus.Formularios.frmGeneradorDTE";
                oCreationPackage.String = "Generador DTE";
                oCreationPackage.Image = "";
                oMenus.AddEx(oCreationPackage);
            }
            catch (Exception er)
            { //  Menu already exists
                Application.SBO_Application.SetStatusBarMessage("Menu Already Exists", SAPbouiCOM.BoMessageTime.bmt_Short, true);
            }

            //try
            //{
            //    // Get the menu collection of the newly added pop-up item
            //    oMenuItem = Application.SBO_Application.Menus.Item("AddOnEmisionDTE.Menu");
            //    oMenus = oMenuItem.SubMenus;

            //    if (oMenus.Exists("Soindus.Formularios.frmDteConf"))
            //    {
            //        oMenus.RemoveEx("Soindus.Formularios.frmDteConf");
            //    }

            //    // Create s sub menu
            //    oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_STRING;
            //    oCreationPackage.UniqueID = "Soindus.Formularios.frmDteConf";
            //    oCreationPackage.String = "Configuración";
            //    oCreationPackage.Image = "";
            //    oMenus.AddEx(oCreationPackage);
            //}
            //catch (Exception er)
            //{ //  Menu already exists
            //    Application.SBO_Application.SetStatusBarMessage("Menu Already Exists", SAPbouiCOM.BoMessageTime.bmt_Short, true);
            //}
        }

        public void SBO_Application_MenuEvent(ref SAPbouiCOM.MenuEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            try
            {
                if (pVal.BeforeAction && pVal.MenuUID == "Soindus.Formularios.Form1")
                {
                    try
                    {
                        Application.SBO_Application.Forms.Item("Form1").Select();
                    }
                    catch
                    {
                        Formularios.Form1 activeForm = new Formularios.Form1();
                        activeForm.Show();
                    }
                }

                if (pVal.BeforeAction && pVal.MenuUID == "Soindus.Formularios.frmGeneradorDTE")
                {
                    try
                    {
                        Application.SBO_Application.Forms.Item("frmGeneradorDTE").Select();
                    }
                    catch
                    {
                        Formularios.frmGeneradorDTE activeForm = new Formularios.frmGeneradorDTE();
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
