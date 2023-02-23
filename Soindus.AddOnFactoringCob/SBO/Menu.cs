using System;
using SAPbouiCOM.Framework;

namespace Soindus.AddOnFactoringCob.SBO
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

                if (oMenus.Exists("AddOnFactoringCob.Menu"))
                {
                    oMenus.RemoveEx("AddOnFactoringCob.Menu");
                }

                // Create s sub menu
                oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_POPUP;
                oCreationPackage.UniqueID = "AddOnFactoringCob.Menu";
                oCreationPackage.String = "Factoring y Cobranza";
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
            //    oMenuItem = Application.SBO_Application.Menus.Item("AddOnFactoringCob.Menu");
            //    oMenus = oMenuItem.SubMenus;

            //    if (oMenus.Exists("Soindus.Formularios.Form1"))
            //    {
            //        oMenus.RemoveEx("Soindus.Formularios.Form1");
            //    }

            //    // Create s sub menu
            //    oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_STRING;
            //    oCreationPackage.UniqueID = "Soindus.Formularios.Form1";
            //    oCreationPackage.String = "Form1";
            //    oMenus.AddEx(oCreationPackage);
            //}
            //catch (Exception er)
            //{ //  Menu already exists
            //    Application.SBO_Application.SetStatusBarMessage("Menu Already Exists", SAPbouiCOM.BoMessageTime.bmt_Short, true);
            //}

            try
            {
                // Get the menu collection of the newly added pop-up item
                oMenuItem = Application.SBO_Application.Menus.Item("AddOnFactoringCob.Menu");
                oMenus = oMenuItem.SubMenus;

                if (oMenus.Exists("Soindus.Formularios.frmFacSelect"))
                {
                    oMenus.RemoveEx("Soindus.Formularios.frmFacSelect");
                }

                // Create s sub menu
                oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_STRING;
                oCreationPackage.UniqueID = "Soindus.Formularios.frmFacSelect";
                oCreationPackage.String = "Selector de documentos";
                oMenus.AddEx(oCreationPackage);
            }
            catch (Exception er)
            { //  Menu already exists
                Application.SBO_Application.SetStatusBarMessage("Menu Already Exists", SAPbouiCOM.BoMessageTime.bmt_Short, true);
            }

            try
            {
                // Get the menu collection of the newly added pop-up item
                oMenuItem = Application.SBO_Application.Menus.Item("AddOnFactoringCob.Menu");
                oMenus = oMenuItem.SubMenus;

                if (oMenus.Exists("Soindus.Formularios.frmFacList"))
                {
                    oMenus.RemoveEx("Soindus.Formularios.frmFacList");
                }

                // Create s sub menu
                oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_STRING;
                oCreationPackage.UniqueID = "Soindus.Formularios.frmFacList";
                oCreationPackage.String = "Documentos en factoring";
                oMenus.AddEx(oCreationPackage);
            }
            catch (Exception er)
            { //  Menu already exists
                Application.SBO_Application.SetStatusBarMessage("Menu Already Exists", SAPbouiCOM.BoMessageTime.bmt_Short, true);
            }

            try
            {
                // Get the menu collection of the newly added pop-up item
                oMenuItem = Application.SBO_Application.Menus.Item("AddOnFactoringCob.Menu");
                oMenus = oMenuItem.SubMenus;

                if (oMenus.Exists("Soindus.Formularios.frmFacCuentaSN"))
                {
                    oMenus.RemoveEx("Soindus.Formularios.frmFacCuentaSN");
                }

                // Create s sub menu
                oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_STRING;
                oCreationPackage.UniqueID = "Soindus.Formularios.frmFacCuentaSN";
                oCreationPackage.String = "Saldo de cuenta socio de negocios";
                oMenus.AddEx(oCreationPackage);
            }
            catch (Exception er)
            { //  Menu already exists
                Application.SBO_Application.SetStatusBarMessage("Menu Already Exists", SAPbouiCOM.BoMessageTime.bmt_Short, true);
            }

            try
            {
                // Get the menu collection of the newly added pop-up item
                oMenuItem = Application.SBO_Application.Menus.Item("AddOnFactoringCob.Menu");
                oMenus = oMenuItem.SubMenus;

                if (oMenus.Exists("Soindus.Formularios.frmCobGestion"))
                {
                    oMenus.RemoveEx("Soindus.Formularios.frmCobGestion");
                }

                // Create s sub menu
                oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_STRING;
                oCreationPackage.UniqueID = "Soindus.Formularios.frmCobGestion";
                oCreationPackage.String = "Gestión de cobranza";
                oMenus.AddEx(oCreationPackage);
            }
            catch (Exception er)
            { //  Menu already exists
                Application.SBO_Application.SetStatusBarMessage("Menu Already Exists", SAPbouiCOM.BoMessageTime.bmt_Short, true);
            }

            try
            {
                // Get the menu collection of the newly added pop-up item
                oMenuItem = Application.SBO_Application.Menus.Item("AddOnFactoringCob.Menu");
                oMenus = oMenuItem.SubMenus;

                if (oMenus.Exists("Soindus.Formularios.frmCobLog"))
                {
                    oMenus.RemoveEx("Soindus.Formularios.frmCobLog");
                }

                // Create s sub menu
                oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_STRING;
                oCreationPackage.UniqueID = "Soindus.Formularios.frmCobLog";
                oCreationPackage.String = "Registro de cobranza";
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

                if (pVal.BeforeAction && pVal.MenuUID == "Soindus.Formularios.frmFacSelect")
                {
                    try
                    {
                        Application.SBO_Application.Forms.Item("frmFacSelect").Select();
                    }
                    catch
                    {
                        Formularios.frmFacSelect activeForm = new Formularios.frmFacSelect();
                        activeForm.Show();
                    }
                }

                if (pVal.BeforeAction && pVal.MenuUID == "Soindus.Formularios.frmFactoring")
                {
                    try
                    {
                        Application.SBO_Application.Forms.Item("frmFactoring").Select();
                    }
                    catch
                    {
                        Formularios.frmFactoring activeForm = new Formularios.frmFactoring();
                        activeForm.Show();
                    }
                }

                if (pVal.BeforeAction && pVal.MenuUID == "Soindus.Formularios.frmFacList")
                {
                    try
                    {
                        Application.SBO_Application.Forms.Item("frmFacList").Select();
                    }
                    catch
                    {
                        Formularios.frmFacList activeForm = new Formularios.frmFacList();
                        activeForm.Show();
                    }
                }

                if (pVal.BeforeAction && pVal.MenuUID == "Soindus.Formularios.frmFacCuentaSN")
                {
                    try
                    {
                        Application.SBO_Application.Forms.Item("frmFacCuentaSN").Select();
                    }
                    catch
                    {
                        Formularios.frmFacCuentaSN activeForm = new Formularios.frmFacCuentaSN();
                        activeForm.Show();
                    }
                }

                if (pVal.BeforeAction && pVal.MenuUID == "Soindus.Formularios.frmCobGestion")
                {
                    try
                    {
                        Application.SBO_Application.Forms.Item("frmCobGestion").Select();
                    }
                    catch
                    {
                        Formularios.frmCobGestion activeForm = new Formularios.frmCobGestion();
                        activeForm.Show();
                    }
                }

                if (pVal.BeforeAction && pVal.MenuUID == "Soindus.Formularios.frmCobLog")
                {
                    try
                    {
                        Application.SBO_Application.Forms.Item("frmCobLog").Select();
                    }
                    catch
                    {
                        Formularios.frmCobLog activeForm = new Formularios.frmCobLog();
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
