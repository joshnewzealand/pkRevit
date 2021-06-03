extern alias global3;

using global3.Autodesk.Revit.DB;

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

    public partial class Entry_0200_pkRevit_NurfGun
    {

            public EE14_Draw3D_IntersectorLines myEE14_Draw3D_IntersectorLines { get; set; }
            public ExternalEvent myExternalEvent_EE14_Draw3D_IntersectorLines { get; set; }


            public EE14_Draw3D_IntersectorLines_Delete myEE14_Draw3D_IntersectorLines_Delete { get; set; }
            public ExternalEvent myExternalEvent_EE14_Draw3D_IntersectorLines_Delete { get; set; }


            public pkRevitMisc.ToAvoidLoadingRevitDLLs toavoidloadingrevitdlls { get; set; }

            public Result StartMethod_0200(ExternalCommandData cd, ref string message, ElementSet elements)
            {
            int eL = -1;

            ExternalCommandData commandData = cd;
            string executionLocation = message;

            toavoidloadingrevitdlls = new pkRevitMisc.ToAvoidLoadingRevitDLLs();
            toavoidloadingrevitdlls.commandData = commandData;
            toavoidloadingrevitdlls.executionLocation = executionLocation;
            try
            {
              

                Window1213_ExtensibleStorage myWindow3 = new Window1213_ExtensibleStorage(commandData);

                UIDocument uidoc = commandData.Application.ActiveUIDocument;
                Document doc = uidoc.Document;

                List<Element> myListElement = new FilteredElementCollector(doc).OfClass(typeof(FamilyInstance)).Where(x => x.Name == "Nerf Gun").ToList();

                if (myListElement.Count() == 0)
                {
                    MessageBox.Show("Please place a nerf gun in the model, (previous button)");
                    return Result.Succeeded;
                }

                uidoc.Selection.SetElementIds(new List<ElementId>() { myListElement.Last().Id });

                myEE14_Draw3D_IntersectorLines = new EE14_Draw3D_IntersectorLines();
                myExternalEvent_EE14_Draw3D_IntersectorLines = ExternalEvent.Create(myEE14_Draw3D_IntersectorLines);
                // window2333_SortOrder = new pkRevitMisc.Schedule_Manual_Sort_Order.Window2333_SortOrder(commandData, this, true, 0);
               // MessageBox.Show("hello world2");
                myExternalEvent_EE14_Draw3D_IntersectorLines.Raise();
            }

            #region catch and finally
            catch (Exception ex)
                {
                    _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("Entry_0200_pkRevit_NurfGun, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
                }
                finally
                {
                }
                #endregion

                return Result.Succeeded;
            }

            public Result StartMethod_0200_DeleteLines(ExternalCommandData cd, ref string message, ElementSet elements)
            {
                ExternalCommandData commandData = cd;
                string executionLocation = message;

                toavoidloadingrevitdlls = new pkRevitMisc.ToAvoidLoadingRevitDLLs();
                toavoidloadingrevitdlls.commandData = commandData;
                toavoidloadingrevitdlls.executionLocation = executionLocation;

                int eL = -1;
                try
                {
                Window1213_ExtensibleStorage myWindow3 = new Window1213_ExtensibleStorage(commandData);

                myEE14_Draw3D_IntersectorLines_Delete = new EE14_Draw3D_IntersectorLines_Delete();
                    myExternalEvent_EE14_Draw3D_IntersectorLines_Delete = ExternalEvent.Create(myEE14_Draw3D_IntersectorLines_Delete);


                    myExternalEvent_EE14_Draw3D_IntersectorLines_Delete.Raise();
                }

                #region catch and finally
                catch (Exception ex)
                {
                    _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("StartMethod_0220_DeleteLines, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
                }
                finally
                {
                }
                #endregion

                return Result.Succeeded;
            }
        }
    }
