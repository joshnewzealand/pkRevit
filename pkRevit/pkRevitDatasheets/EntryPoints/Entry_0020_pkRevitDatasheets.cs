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

namespace pkRevitDatasheets.EntryPoints  //Entry_0010_pkRevitDatasheets
{
    public partial class Entry_0020_pkRevitDatasheets
    {
        public ExternalEvents.EE00_CopyTemplate myEE00_CopyTemplate { get; set; }
        public ExternalEvent myExternalEvent_EE00_CopyTemplate { get; set; }

        ////public ExternalCommandData commandData { get; set; }
        ////public string executionLocation { get; set; }

        public Result StartMethod_02(ExternalCommandData cd, ref string message, ElementSet elements)
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

            bool myBool_ShowMessageBox = true;

            if (mainWindow.bool_ContinueOpening_DatabaseChecksOut)
            {
                mainWindow.method_LoadUpMasterList();
                myBool_ShowMessageBox = mainWindow.ExtractEntireSchedule();

                ////////mainWindow.Topmost = true;
                ////////mainWindow.Show();
            }
            GC.Collect();

            string str_CR = Environment.NewLine + "Please click but 'Refresh' the in the STORE ATTACHMENTS App." + Environment.NewLine + Environment.NewLine + "A shortcut is on your desktop.";
            str_CR = str_CR;// + Environment.NewLine + Environment.NewLine + "(It is the little green button in the bottom right corner of the main listview).";

            if (myBool_ShowMessageBox) TaskDialog.Show("Complete", "Schedule has been exported." + Environment.NewLine + str_CR);

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
