extern alias global3;

using global3.Autodesk.Revit.DB;
using global3.Autodesk.Revit.DB.ExtensibleStorage;

//using Autodesk.Revit.DB;
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


            int eL = -1;

            try
            {
                ExternalCommandData commandData = cd;
                string executionLocation = message;

                toavoidloadingrevitdlls = new pkRevitMisc.ToAvoidLoadingRevitDLLs();
                toavoidloadingrevitdlls.commandData = commandData;
                toavoidloadingrevitdlls.executionLocation = executionLocation;

                Window1213_ExtensibleStorage myWindow3 = new Window1213_ExtensibleStorage(commandData);


                ////////////////////myWindow1 = new MainWindow(commandData, this, true, 0);
                myWindow3.myThisApplication = this;
                myWindow3.Topmost = true;


                myWindow3.myEE13_ExtensibleStorage_NewOrSave = new EE13_ExtensibleStorage_NewOrSave();
                myWindow3.myEE13_ExtensibleStorage_NewOrSave.myWindow1 = myWindow3;
                myWindow3.myExternalEvent_EE13_ExtensibleStorage_NewOrSave = ExternalEvent.Create(myWindow3.myEE13_ExtensibleStorage_NewOrSave);

                myWindow3.myEE13_ExtensibleStorage_Rearrange = new EE13_ExtensibleStorage_Rearrange();
                myWindow3.myEE13_ExtensibleStorage_Rearrange.myWindow1 = myWindow3;
                myWindow3.myExternalEvent_EE13_ExtensibleStorage_Rearrange = ExternalEvent.Create(myWindow3.myEE13_ExtensibleStorage_Rearrange);

                myWindow3.myEE13_ExtensibleStorage_DeleteItem = new EE13_ExtensibleStorage_DeleteItem();
                myWindow3.myEE13_ExtensibleStorage_DeleteItem.myWindow1 = myWindow3;
                myWindow3.myExternalEvent_EE13_ExtensibleStorage_DeleteItem = ExternalEvent.Create(myWindow3.myEE13_ExtensibleStorage_DeleteItem);

                myWindow3.myEE13_ExtensibleStorage_DeleteAll = new EE13_ExtensibleStorage_DeleteAll();
                myWindow3.myEE13_ExtensibleStorage_DeleteAll.myWindow1 = myWindow3;
                myWindow3.myExternalEvent_EE13_ExtensibleStorage_DeleteAll = ExternalEvent.Create(myWindow3.myEE13_ExtensibleStorage_DeleteAll);

                myWindow3.myEE13_ExtensibleStorage_zRandomise = new EE13_ExtensibleStorage_zRandomise();
                myWindow3.myEE13_ExtensibleStorage_zRandomise.myWindow1 = myWindow3;
                myWindow3.myExternalEvent_EE13_ExtensibleStorage_zRandomise = ExternalEvent.Create(myWindow3.myEE13_ExtensibleStorage_zRandomise);

                myWindow3.myEE12_SetupRoom = new EE12_SetupRoom();
                myWindow3.myEE12_SetupRoom.myWindow1 = myWindow3;
                myWindow3.myExternalEvent_EE12_SetupRoom = ExternalEvent.Create(myWindow3.myEE12_SetupRoom);


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
