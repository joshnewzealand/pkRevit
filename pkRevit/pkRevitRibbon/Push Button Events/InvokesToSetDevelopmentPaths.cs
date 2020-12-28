using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using System.Reflection;
using System.IO;


using TaskDialog = Autodesk.Revit.UI.TaskDialog;

namespace pkRevitRibbon
{

    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class Invoke02_Uninstall : IExternalCommand
    {
        //public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            if (Properties.Settings.Default.MakeTheNextOneDevelopment)
            {
                Properties.Settings.Default.MakeTheNextOneDevelopment = false;
                Properties.Settings.Default.Save();
                Properties.Settings.Default.Reload();

                TaskDialog.Show("Me", "The uninstall button doesn't have a development path...please choose any to the right of this button.");

                return Result.Succeeded;
            }

            TaskDialog.Show("Me", "Please use Add Remove Programs List from Control Panel." + Environment.NewLine + Environment.NewLine + "The name of the application is: 'pkRevit joshnewzealand'." + Environment.NewLine + Environment.NewLine + "Tip: Sort by 'Installed On' date.");
            ////Manually Remove Programs from the Add Remove Programs List

            return Result.Succeeded;
        }
    }

    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]  //Set Families Path Root
    public class InvokeSetDevelopmentPath : IExternalCommand
    {
        //public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                Properties.Settings.Default.MakeTheNextOneDevelopment = true;
                Properties.Settings.Default.Save();
                Properties.Settings.Default.Reload();

                Autodesk.Revit.UI.TaskDialog.Show("Next Click", "Now please click the desired ribbon button, to set the development path.");
            }

            #region catch and finally
            catch (Exception ex)
            {
                RibbonSupportMethods.writeDebug("InvokeSetDevelopmentPath" + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion

            return Result.Succeeded;
        }
    }
}
