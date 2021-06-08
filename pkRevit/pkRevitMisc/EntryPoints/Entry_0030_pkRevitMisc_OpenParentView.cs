using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using View = Autodesk.Revit.DB.View;
using System.Windows.Forms;

 
namespace pkRevitMisc.EntryPoints  //Entry_0010_pkRevitDatasheets
{
    public partial class Entry_0030_pkRevitMisc
    {
        public Result StartMethod_0030(ExternalCommandData cd, ref string message, ElementSet elements)
        {
            ExternalCommandData commandData = cd;
            string executionLocation = message;

            ToAvoidLoadingRevitDLLs toavoidloadingrevitdlls = new ToAvoidLoadingRevitDLLs();
            toavoidloadingrevitdlls.commandData = commandData;
            toavoidloadingrevitdlls.executionLocation = executionLocation;


            UIDocument uidoc = commandData.Application.ActiveUIDocument;

            Document doc = uidoc.Document;
            View myView = uidoc.ActiveView;


            View nextView = doc.GetElement(myView.GetPrimaryViewId()) as View;


            if (nextView == null)
            {
                TaskDialog.Show("Cannot proceed", "View is 'Independant' and does not have a parent");

            }
            else
            {
                uidoc.ActiveView = nextView;
            }

            return Result.Succeeded;
        }
    }
}
