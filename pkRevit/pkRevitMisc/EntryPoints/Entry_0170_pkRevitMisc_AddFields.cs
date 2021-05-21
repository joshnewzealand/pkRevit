using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using pkRevitMisc.CommandsWithWindows.Add_Edit_Parameters;
using System;
using System.Windows.Forms;



namespace pkRevitMisc.EntryPoints  //Entry_0010_pkRevitDatasheets
{
    public partial class Entry_0170_pkRevitMisc
    {
        ////////////public CommandsWithWindows.Add_Edit_Parameters.EE17_Edit_StringBasedParameters myEE16_AddSharedParameters_InVariousWays { get; set; }
        ////////////public ExternalEvent myExternalEvent_EE16_AddSharedParameters_InVariousWays { get; set; }

        ////////////public bool myBool_AddToProject { get; set; } = true;

        int eL = -1;

        public Result StartMethod_0170(ExternalCommandData cd, ref string message, ElementSet elements)
        {
            ExternalCommandData commandData = cd;
            string executionLocation = message;

            ToAvoidLoadingRevitDLLs toavoidloadingrevitdlls = new ToAvoidLoadingRevitDLLs();
            toavoidloadingrevitdlls.commandData = commandData;
            toavoidloadingrevitdlls.executionLocation = executionLocation;

            Window1617_AddEditParameters myWindowWindow1617 = new Window1617_AddEditParameters(toavoidloadingrevitdlls);

            try
            {

               // myWindowWindow1617.myWindow2 = this;
                myWindowWindow1617.Topmost = true;
                myWindowWindow1617.Show();

                //////////////myEE16_AddSharedParameters_InVariousWays = new CommandsWithWindows.Add_Edit_Parameters.EE17_Edit_StringBasedParameters();
                //////////////myEE16_AddSharedParameters_InVariousWays.myWindow1 = this;
                //////////////myExternalEvent_EE16_AddSharedParameters_InVariousWays = ExternalEvent.Create(myEE16_AddSharedParameters_InVariousWays);

                //////////////myExternalEvent_EE16_AddSharedParameters_InVariousWays.Raise();
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("StartMethod_0170, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion


            return Result.Succeeded;
        }
    }
}
