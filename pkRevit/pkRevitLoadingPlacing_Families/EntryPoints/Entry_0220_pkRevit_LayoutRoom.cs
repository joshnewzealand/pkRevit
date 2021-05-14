using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Runtime.Serialization;
using System.Windows.Forms;

namespace pkRevitLoadingPlacing_Families.EntryPoints  //Entry_0010_pkRevitDatasheets
{


    public partial class Entry_0220_pkRevit_LayoutRoom
    {
        ////////////////////public MainWindow myWindow1 { get; set; }


        public pkRevitMisc.ToAvoidLoadingRevitDLLs toavoidloadingrevitdlls { get; set; }

        public Result StartMethod_0220(ExternalCommandData cd, ref string message, ElementSet elements)
        {
            ExternalCommandData commandData = cd;
            string executionLocation = message;

            toavoidloadingrevitdlls = new pkRevitMisc.ToAvoidLoadingRevitDLLs();
            toavoidloadingrevitdlls.commandData = commandData;
            toavoidloadingrevitdlls.executionLocation = executionLocation;

            int eL = -1;
            Window1213_ExtensibleStorage myWindow3 = new Window1213_ExtensibleStorage(commandData);

            try
            {
                ////////////////////myWindow1 = new MainWindow(commandData, this, true, 0);
                myWindow3.myThisApplication = this;
                myWindow3.Topmost = true;
                myWindow3.Show();
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("Entry_0220_pkRevit_LayoutRoom, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion

            return Result.Succeeded;
        }
    }
}
