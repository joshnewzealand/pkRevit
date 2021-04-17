using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using pkRevitMisc.Size_and_Position_Viewport;
using System;
using System.Windows.Forms;


namespace pkRevitMisc.EntryPoints  //Entry_0010_pkRevitDatasheets
{
    public partial class Entry_0050_pkRevitMisc
    {

        int eL = -1;

        public Result StartMethod_0050(ExternalCommandData cd, ref string message, ElementSet elements)
        {
            ExternalCommandData commandData = cd;
            string executionLocation = message;

            ToAvoidLoadingRevitDLLs toavoidloadingrevitdlls = new ToAvoidLoadingRevitDLLs();
            toavoidloadingrevitdlls.commandData = commandData;
            toavoidloadingrevitdlls.executionLocation = executionLocation;

            try
            {
                Window1548_SizePositionViewport window1548_SizePositionViewport = new Window1548_SizePositionViewport(commandData.Application);
                window1548_SizePositionViewport.Show();
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("Entry_0050_pkRevitMisc, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion

            return Result.Succeeded;
        }
    }
}
