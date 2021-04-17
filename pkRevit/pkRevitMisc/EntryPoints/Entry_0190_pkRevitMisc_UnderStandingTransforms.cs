using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

using System.Windows.Forms;


namespace pkRevitMisc.EntryPoints  //Entry_0010_pkRevitDatasheets
{
    public partial class Entry_0190_pkRevitMisc
    {
        public Result StartMethod_0190(ExternalCommandData cd, ref string message, ElementSet elements)
        {
            ExternalCommandData commandData = cd;
            string executionLocation = message;

            ToAvoidLoadingRevitDLLs toavoidloadingrevitdlls = new ToAvoidLoadingRevitDLLs();
            toavoidloadingrevitdlls.commandData = commandData;
            toavoidloadingrevitdlls.executionLocation = executionLocation;

            MessageBox.Show("0190_UnderStandingTransforms-2");

            return Result.Succeeded;
        }
    }
}
