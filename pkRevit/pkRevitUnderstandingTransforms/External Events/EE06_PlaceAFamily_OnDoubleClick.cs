using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Autodesk.Revit.DB.Events;
using System.Runtime.InteropServices;
using System.Windows.Threading;

namespace pkRevitUnderstandingTransforms
{

    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class EE06_PlaceAFamily_OnDoubleClick : IExternalEventHandler  //this is the last when one making a checklist change, EE4 must be just for when an element is new
    {
        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

        public Window_RotatePlatform myWindow1 { get; set; }

        public FamilySymbol myFamilySymbol { get; set; }

        void OnDocumentChanged(object sender, DocumentChangedEventArgs e)
        {
            try
            {
                UIDocument uidoc = uiapp.ActiveUIDocument;
                Document doc = uidoc.Document;

                if (e.GetAddedElementIds().Count == 0 & e.GetModifiedElementIds().Count == 0) return;

                if (e.GetAddedElementIds().Count > 0)
                {
                    Element myElement = doc.GetElement(e.GetAddedElementIds().First());

                    if (myElement.GetType().Name != "FamilyInstance") return;

                    //uidoc.Application.Application.DocumentChanged -= new EventHandler<DocumentChangedEventArgs>(OnDocumentChanged);
                }

                if (e.GetModifiedElementIds().Count > 0)
                {
                    Element myElement = doc.GetElement(e.GetModifiedElementIds().First());

                    if (myElement.GetType().Name != "FamilyInstance") return;

                    ICollection<ElementId> icollection = e.GetModifiedElementIds();

                    //
                    Dispatcher.CurrentDispatcher.Invoke(async () =>
                    //                    myWindow1.Dispatcher.Invoke(async () =>
                    {
                        SetForegroundWindow(uidoc.Application.MainWindowHandle); //this is an excape event
                        keybd_event(0x1B, 0, 0, 0);
                        keybd_event(0x1B, 0, 2, 0);
                        keybd_event(0x1B, 0, 0, 0);
                        keybd_event(0x1B, 0, 2, 0);

                        await Task.Delay(1000);

                      //  ReferencePoint myRefPoint = doc.GetElement(new ElementId(myIntUpDown_Middle2.Value.Value)) as ReferencePoint;

                        //MessageBox.Show("Hello world");
                        if(myWindow1 != null)
                        {
                            uidoc.Selection.SetElementIds(new List<ElementId>() { icollection.ElementAt(0) });
                            myWindow1.updateMiddle2_UpdateUI();
                        } else
                        {
                            //if (uidoc.Selection.GetElementIds().Count != 1) return Result.Succeeded;
                            Element myElement2 = doc.GetElement(icollection.ElementAt(0)) as Element;
                            //if (myElement2.GetType() != typeof(FamilyInstance)) return Result.Succeeded;
                            FamilyInstance myFamilyInstance = myElement as FamilyInstance;

                            IList<ElementId> placePointIds_1338 = AdaptiveComponentInstanceUtils.GetInstancePointElementRefIds(myFamilyInstance);
                            ReferencePoint myReferencePoint_Centre = uidoc.Document.GetElement(placePointIds_1338.First()) as ReferencePoint;

                            uidoc.Selection.SetElementIds(new List<ElementId>() { myReferencePoint_Centre.Id });
                        }
                    });

                    //uidoc.Selection.SetElementIds(new List<ElementId>() { doc.GetElement(icollection.ElementAt(2)).Id });
                }


                uidoc.Application.Application.DocumentChanged -= new EventHandler<DocumentChangedEventArgs>(OnDocumentChanged);

                ////SetForegroundWindow(uidoc.Application.MainWindowHandle); //this is an excape event
                ////keybd_event(0x1B, 0, 0, 0);
                ////keybd_event(0x1B, 0, 2, 0);
                ////keybd_event(0x1B, 0, 0, 0);
                ////keybd_event(0x1B, 0, 2, 0);

            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("OnDocumentChanged" + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        UIApplication uiapp;
        bool myBool_PassThrough = true;

        public void Execute(UIApplication uiappp)
        {
            uiapp = uiappp;

            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            ///                TECHNIQUE 06 OF 19 (EE06_PlaceAFamily_OnDoubleClick.cs)
            ///↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓ PLACING A FAMILY THEN RELEASING THE COMMMAND ↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓
            ///
            /// Interfaces and ENUM's:
            ///     BuiltInParameter.FAMILY_WORK_PLANE_BASED
            ///     using System.Runtime.InteropServices (namespace)
            /// 
            /// Demonstrates classes:
            ///     DocumentChangedEventArgs*
            ///     PromptForFamilyInstancePlacementOptions
            ///     SketchPlane
            /// 
            /// 
            /// Key methods:
            ///     SketchPlane.Create(doc, myLevel.GetPlaneReference());
            ///     uidoc.PromptForFamilyInstancePlacement(myFamilySymbol);
            ///	    SetForegroundWindow(
            ///     keybd_event(
            ///
            ///
            /// * class is actually part of the .NET framework (not Revit API)
			///	
			///	
			///	
			///	https://github.com/joshnewzealand/Revit-API-Playpen-CSharp
            ///	


            uidoc.Application.Application.DocumentChanged += new EventHandler<DocumentChangedEventArgs>(OnDocumentChanged);

            PromptForFamilyInstancePlacementOptions myPromptForFamilyInstancePlacementOptions = new PromptForFamilyInstancePlacementOptions();
            try
            {


                SetForegroundWindow(uidoc.Application.MainWindowHandle); //this is an excape event

                ViewPlan myViewPlan = doc.ActiveView as ViewPlan;

                if (doc.ActiveView.GetType() ==   typeof(ViewPlan))
                {


                    ////////    if (uidoc.ActiveView.SketchPlane.Name != "Level 1")
                    ////////{
                    ///    Level myLevel = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Levels).WhereElementIsNotElementType().First() as Level;
                    ////////    using (Transaction y = new Transaction(doc, "Place a Family"))
                    ////////    {
                    ////////        y.Start();
                    ////////        uidoc.ActiveView.SketchPlane = SketchPlane.Create(doc, myLevel.GetPlaneReference());
                    ////////        y.Commit();
                    ////////    }
                    ////////}

                    myPromptForFamilyInstancePlacementOptions.FaceBasedPlacementType = FaceBasedPlacementType.PlaceOnWorkPlane;
                    uidoc.PromptForFamilyInstancePlacement(myFamilySymbol, myPromptForFamilyInstancePlacementOptions);  //<-- decided not to use this late in the project

                }
                else
                {
                    myPromptForFamilyInstancePlacementOptions.FaceBasedPlacementType = FaceBasedPlacementType.PlaceOnFace;
                    uidoc.PromptForFamilyInstancePlacement(myFamilySymbol, myPromptForFamilyInstancePlacementOptions);
                }
            }


            #region catch and finally
            catch (Exception ex)
            {
                if (ex.Message != "The user aborted the pick operation.")
                {
                    _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("EE01_Part1_PlaceAFamily" + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);

                }
                else
                {
                    uidoc.Application.Application.DocumentChanged -= new EventHandler<DocumentChangedEventArgs>(OnDocumentChanged);
                }
            }
            finally
            {
            }
            #endregion
        }

        public string GetName()
        {
            return "External Event Example";
        }
    }
}
