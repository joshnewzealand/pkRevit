/*
 * Created by SharpDevelop.
 * User: Joshua.Lumley
 * Date: 28/04/2019
 * Time: 5:59 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using _952_PRLoogleClassLibrary;
using pkRevitUnderstandingTransforms.External_Events;
using pkRevitUnderstandingTransforms;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace RevitTransformSliders
{
	public partial class ThisApplication_MakingPlatform
	{
        public ToAvoidLoadingRevitDLLs toavoidloadingrevitdlls { get; set; }

        public EE06_PlaceAFamily_OnDoubleClick myEE06_PlaceAFamily_OnDoubleClick { get; set; }
        public ExternalEvent myExternalEvent_EE06_PlaceAFamily_OnDoubleClick { get; set; }


        public EE08_LoadFamily myEE08_LoadFamily { get; set; }
        public ExternalEvent myExternalEvent_EE08_LoadFamily { get; set; }

        private FamilySymbol returnSymbol_workswhen_TypeAnd_FamilyNameAreSame(string FamilyAndTypeName)
        {
            UIDocument uidoc = toavoidloadingrevitdlls.commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            IEnumerable<Element> myIEnumerableElement = new FilteredElementCollector(doc).WherePasses(new ElementClassFilter(typeof(Family))).Where(x => x.Name == FamilyAndTypeName);

            if (myIEnumerableElement.Count() == 0) return null;
            return doc.GetElement(((Family)myIEnumerableElement.First()).GetFamilySymbolIds().First()) as FamilySymbol;
        }

        public Result StartMethod_0120(ExternalCommandData cd, ref string message, ElementSet elements)
        {
            ExternalCommandData commandData = cd;
            string executionLocation = message;

            toavoidloadingrevitdlls = new ToAvoidLoadingRevitDLLs();
            toavoidloadingrevitdlls.commandData = commandData;
            toavoidloadingrevitdlls.executionLocation = executionLocation;



            bool bool_FaceWorkPlane = false;
            bool bool_CentreScreenBox = false;

            if (true)
            {
                TaskDialog mainDialog = new TaskDialog("Choose HOSTED or UNHOSTED");
                ////mainDialog.MainInstruction = "Hello, viewport check!";
                ////mainDialog.MainContent =
                ////        "Revit API doesn't automatically know if the user is in an active viewport. "
                ////        + "Please click 'Yes' if your are, or 'No' if your not.";


                mainDialog.AddCommandLink(TaskDialogCommandLinkId.CommandLink1, "HOSTED on Face/Workplane.");
                mainDialog.AddCommandLink(TaskDialogCommandLinkId.CommandLink2, "UNHOSTED centre of Screen/Section Box.");
                mainDialog.AddCommandLink(TaskDialogCommandLinkId.CommandLink3, "Cancel.");

                mainDialog.CommonButtons = TaskDialogCommonButtons.Close;
                mainDialog.DefaultButton = TaskDialogResult.Close;


                TaskDialogResult tResult = mainDialog.Show();


                if (TaskDialogResult.CommandLink1 == tResult) bool_FaceWorkPlane = true;
                if (TaskDialogResult.CommandLink2 == tResult) bool_CentreScreenBox = true;
            }

            if (!bool_FaceWorkPlane & !bool_CentreScreenBox) return Result.Succeeded;


            if(bool_FaceWorkPlane)
            {
                myEE06_PlaceAFamily_OnDoubleClick = new EE06_PlaceAFamily_OnDoubleClick();
                myEE06_PlaceAFamily_OnDoubleClick.myWindow1 = null;
                myExternalEvent_EE06_PlaceAFamily_OnDoubleClick = ExternalEvent.Create(myEE06_PlaceAFamily_OnDoubleClick);

                myEE08_LoadFamily = new EE08_LoadFamily();
                myEE08_LoadFamily.toavoidloadingrevitdlls = toavoidloadingrevitdlls;
                myExternalEvent_EE08_LoadFamily = ExternalEvent.Create(myEE08_LoadFamily);


                string str_getOrLoadThis_Family = "PRL-GM-2020 Adaptive Carrier";

                FamilySymbol myFamilySymbol_Carrier = returnSymbol_workswhen_TypeAnd_FamilyNameAreSame(str_getOrLoadThis_Family);


                if (myFamilySymbol_Carrier == null)
                {
                    myEE08_LoadFamily.str_getOrLoadThis_Family = str_getOrLoadThis_Family;
                    myEE08_LoadFamily.toavoidloadingrevitdlls = toavoidloadingrevitdlls;

                    Dispatcher.CurrentDispatcher.Invoke(async () =>
                    {
                        myExternalEvent_EE08_LoadFamily.Raise();

                        myEE08_LoadFamily.bool_Loop_UntilFinished = true;

                        while (myEE08_LoadFamily.bool_Loop_UntilFinished)
                        {
                            await Task.Delay(100);
                        }

                        myFamilySymbol_Carrier = returnSymbol_workswhen_TypeAnd_FamilyNameAreSame(str_getOrLoadThis_Family);
                        myEE06_PlaceAFamily_OnDoubleClick.myFamilySymbol = myFamilySymbol_Carrier;
                        myExternalEvent_EE06_PlaceAFamily_OnDoubleClick.Raise();
                    });
                }
                else
                {
                    myEE06_PlaceAFamily_OnDoubleClick.myFamilySymbol = myFamilySymbol_Carrier;
                    myExternalEvent_EE06_PlaceAFamily_OnDoubleClick.Raise();
                }
            }

            if(bool_CentreScreenBox)
            {
                Centre_of_Screen_Static.CentreOfScreen(toavoidloadingrevitdlls);

                UIDocument uidoc = toavoidloadingrevitdlls.commandData.Application.ActiveUIDocument;
                Document doc = uidoc.Document;

                if (uidoc.Selection.GetElementIds().Count != 1) return Result.Succeeded;
                Element myElement = doc.GetElement(uidoc.Selection.GetElementIds().First()) as Element;
                if (myElement.GetType() != typeof(FamilyInstance)) return Result.Succeeded;
                FamilyInstance myFamilyInstance = myElement as FamilyInstance;

                IList<ElementId> placePointIds_1338 = AdaptiveComponentInstanceUtils.GetInstancePointElementRefIds(myFamilyInstance);
                ReferencePoint myReferencePoint_Centre = uidoc.Document.GetElement(placePointIds_1338.First()) as ReferencePoint;

                uidoc.Selection.SetElementIds(new List<ElementId>() { myReferencePoint_Centre.Id });

                return Result.Succeeded;
            }
            return Result.Succeeded;

        }

    }
}