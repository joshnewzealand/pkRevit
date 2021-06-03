using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _952_PRLoogleClassLibrary;
using pkRevitDatasheets.BuildingCoderClasses;
using System.IO;
using System.Runtime.Serialization;
using System.Windows.Forms;

namespace pkRevitDatasheets.EntryPoints  //Entry_0010_pkRevitDatasheets
{
    public partial class Entry_0010_pkRevitDatasheets
    {
        public ExternalEvents.EE00_CopyTemplate myEE00_CopyTemplate { get; set; }
        public ExternalEvent myExternalEvent_EE00_CopyTemplate { get; set; }

        ////public ExternalCommandData commandData { get; set; }
        ////public string executionLocation { get; set; }

        public Result StartMethod_01(ExternalCommandData cd, ref string message, ElementSet elements)
        {
            

            ExternalCommandData commandData = cd;
            string executionLocation = message;
            //////////////////////MainWindow mainWindow = new MainWindow();
            //////////////////////mainWindow.Topmost = true;
            //////////////////////mainWindow.Show();

            Document doc = commandData.Application.ActiveUIDocument.Document;

            string name = "TrackChanges_project_identifier";
            Guid named_guid;

            bool rc = JtNamedGuidStorage.Get(doc, name, out named_guid, false);

            if (!rc)
            {
                rc = JtNamedGuidStorage.Get(doc, name, out named_guid, true);

                if (rc)
                {
                    DatabaseMethods.InfoMsg(string.Format("Created a new project identifier " + "for this document: {0} = {1}", name, named_guid.ToString()));
                }
                else
                {
                    DatabaseMethods.ErrorMsg("Something went wrong");
                }
            }
            ToAvoidLoadingRevitDLLs toavoidloadingrevitdlls = new ToAvoidLoadingRevitDLLs();
            toavoidloadingrevitdlls.commandData = commandData;
            toavoidloadingrevitdlls.executionLocation = executionLocation;

            MainWindow mainWindow = new MainWindow(named_guid, toavoidloadingrevitdlls);

            if(mainWindow.bool_ContinueOpening_DatabaseChecksOut)
            {
                //////////////mainWindow.method_LoadUpMasterList();
                //////////////mainWindow.ExtractEntireSchedule();

                mainWindow.Topmost = true;
                mainWindow.Show();
            }
            GC.Collect();

            if (false)
            {
                myEE00_CopyTemplate = new ExternalEvents.EE00_CopyTemplate();
                myExternalEvent_EE00_CopyTemplate = ExternalEvent.Create(myEE00_CopyTemplate);
                ////myMainWindow.Topmost = true;
                ////myMainWindow.commandData = commandData;
                //////////////.//////////////////////////// myMainWindow.Show();
                ///
                myExternalEvent_EE00_CopyTemplate.Raise();
            }

            return Result.Succeeded;
        }
    }
}
