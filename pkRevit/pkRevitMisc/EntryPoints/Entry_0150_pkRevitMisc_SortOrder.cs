using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Windows.Forms;


namespace pkRevitMisc.EntryPoints  //Entry_0010_pkRevitDatasheets
{
    public partial class Entry_0150_pkRevitMisc
    {
        public CommandsWithWindows.Schedule_Manual_Sort_Order.EE2333_AddSortIndextoSchedule myEE2333_AddSortIndextoSchedule { get; set; }
        public ExternalEvent myExternalEvent_EE2333_AddSortIndextoSchedule { get; set; }

        int eL = -1;

        public Result StartMethod_0150(ExternalCommandData cd, ref string message, ElementSet elements)
        {
            ExternalCommandData commandData = cd;
            string executionLocation = message;

            ToAvoidLoadingRevitDLLs toavoidloadingrevitdlls = new ToAvoidLoadingRevitDLLs();
            toavoidloadingrevitdlls.commandData = commandData;
            toavoidloadingrevitdlls.executionLocation = executionLocation;

            try
            {
                myEE2333_AddSortIndextoSchedule = new CommandsWithWindows.Schedule_Manual_Sort_Order.EE2333_AddSortIndextoSchedule();
                myExternalEvent_EE2333_AddSortIndextoSchedule = ExternalEvent.Create(myEE2333_AddSortIndextoSchedule);
                // window2333_SortOrder = new pkRevitMisc.Schedule_Manual_Sort_Order.Window2333_SortOrder(commandData, this, true, 0);

                myExternalEvent_EE2333_AddSortIndextoSchedule.Raise();
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("StartMethod_0150, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion

            return Result.Succeeded;
        }
    }
}
