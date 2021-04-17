using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

using System.Windows.Forms;


namespace pkRevitMisc.EntryPoints  //Entry_0010_pkRevitDatasheets
{
    public partial class Entry_0160_pkRevitMisc
    {
        public Result StartMethod_0160(ExternalCommandData cd, ref string message, ElementSet elements)
        {
            ExternalCommandData commandData = cd;
            string executionLocation = message;

            ToAvoidLoadingRevitDLLs toavoidloadingrevitdlls = new ToAvoidLoadingRevitDLLs();
            toavoidloadingrevitdlls.commandData = commandData;
            toavoidloadingrevitdlls.executionLocation = executionLocation;

            MessageBox.Show("0160_EditSchedule-2");

            return Result.Succeeded;
        }
    }
}
