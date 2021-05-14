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
using RevitTransformSliders;
using static pkRevitUnderstandingTransforms.EE06_PlaceAFamily_CentreScreen;

namespace pkRevitUnderstandingTransforms.External_Events
{
    public static class Centre_of_Screen_Static
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int GetWindowTextLength(IntPtr hWnd);


        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int GetWindowText(IntPtr hWnd, System.Text.StringBuilder lpString, int nMaxCount);


        private static XYZ myXYZ_KnownViewport(XYZ myXYZ_CenterOfScreen, Autodesk.Revit.DB.View myViewOnSheet, UIDocument uidoc)
        {
            Document doc = uidoc.Document;

            //ViewSheet myViewSheet = doc.GetElement(myViewOnSheet.OwnerViewId) as ViewSheet;

            Viewport myViewPort = null;
            foreach (ViewSheet vs in new FilteredElementCollector(doc).OfClass(typeof(ViewSheet)).Cast<ViewSheet>())
            {
                foreach (ElementId eid in vs.GetAllViewports())
                {
                    Viewport ev = doc.GetElement(eid) as Viewport;
                    if (ev.ViewId == myViewOnSheet.Id)
                    {
                        myViewPort = ev;
                        break;
                    }
                }
            }

            if (myViewPort == null) return null;

            XYZ myXYZ_CenterOfViewPort = myViewPort.GetBoxCenter();

            XYZ myXYZ_SheetDelta = myXYZ_CenterOfViewPort * myViewOnSheet.Scale; ;

            XYZ myXYZ_CenterInModelSpace = doc.ActiveView.get_BoundingBox(doc.ActiveView).Min + ((doc.ActiveView.get_BoundingBox(doc.ActiveView).Max - doc.ActiveView.get_BoundingBox(doc.ActiveView).Min) / 2);

            XYZ myXYZ_SheetDelta_Fixed = myXYZ_CenterOfScreen - myXYZ_SheetDelta;

            return myXYZ_CenterInModelSpace + myXYZ_SheetDelta_Fixed;
        }

        private static XYZ myPrivateXYZ_CentreWithIntersectHeight(ToAvoidLoadingRevitDLLs toavoidloadingrevitdlls)
        {
            UIDocument uidoc = toavoidloadingrevitdlls.commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            UIView myUIView_1957 = uidoc.GetOpenUIViews().Where(x => x.ViewId == doc.ActiveView.Id).First();

            IList<XYZ> myXYZ_Corners_1431 = myUIView_1957.GetZoomCorners();
            XYZ centre3 = myXYZ_Corners_1431[0] + ((myXYZ_Corners_1431[1] - myXYZ_Corners_1431[0]) / 2);

            BoundingBoxXYZ myBoundingBoxXYZ = doc.ActiveView.get_BoundingBox(doc.ActiveView);

            if (myBoundingBoxXYZ != null)
            {
                int length = GetWindowTextLength(uidoc.Application.MainWindowHandle);
                System.Text.StringBuilder sb = new System.Text.StringBuilder(length + 1);
                GetWindowText(uidoc.Application.MainWindowHandle, sb, sb.Capacity);
                string[] myStringArray = sb.ToString().Split(':');

                string myString_Last5Characters = myStringArray.First().Substring(myStringArray.First().Length - 5);

                if (myString_Last5Characters == "Sheet")
                {
                    centre3 = myXYZ_KnownViewport(centre3, doc.ActiveView, uidoc);
                }
            }

            //get the centre point of the bounding box view becuase this is what is shown and that is fine 
            //but firtst how can we tlel the bounding box is not null AND the word before the word is sheet

            ////////////////////if (doc.ActiveView.ViewType == ViewType.CeilingPlan)
            ////////////////////{
            ////////////////////    PlanViewRange myPlanViewRange = ((ViewPlan)doc.ActiveView).GetViewRange();

            ////////////////////    double myBottomClipPlane_Arch_Strut = myPlanViewRange.GetOffset(PlanViewPlane.CutPlane);  //viewPlan.GetViewRange();

            ////////////////////    if (myBottomClipPlane_Arch_Strut > MmToFoot(1000)) MessageBox.Show("Check your cutplane");
            ////////////////////}
            PlanViewRange myPlanViewRange = ((ViewPlan)doc.ActiveView).GetViewRange();
            //return new XYZ(centre3.X, centre3.Y, /*uidoc.ActiveView.Origin.Z +*/ MmToFoot(1000));
            return new XYZ(centre3.X, centre3.Y, myPlanViewRange.GetOffset(PlanViewPlane.CutPlane));
            //return centre3 + new XYZ(0, 0, uidoc.ActiveView.Origin.Z + Util.MmToFoot(500));
            //return centre3 + new XYZ(0, 0, Util.MmToFoot(myIntegerUpDown_IntersectHeight.Value.Value));
        }

        public static Result CentreOfScreen(ToAvoidLoadingRevitDLLs toavoidloadingrevitdlls)
        {
            int eL = -1;

            try
            {
                UIDocument uidoc = toavoidloadingrevitdlls.commandData.Application.ActiveUIDocument;
                Document doc = uidoc.Document;

                if (doc.ActiveView.ViewType != ViewType.FloorPlan & doc.ActiveView.ViewType != ViewType.EngineeringPlan & doc.ActiveView.ViewType != ViewType.CeilingPlan & doc.ActiveView.ViewType != ViewType.ThreeD)
                {
                    MessageBox.Show("View must be 3D or a 2D plan.");
                    return Result.Succeeded;
                }

                string myStringCarrierAdaptive = "PRL-GM-2020 Adaptive Carrier";

                IEnumerable<Element> myIEnumerableElement = new FilteredElementCollector(doc).WherePasses(new ElementClassFilter(typeof(Family))).Where(x => x.Name == myStringCarrierAdaptive);

                FamilySymbol myFamilySymbol_Carrier = null;

                if (myIEnumerableElement.Count() == 0)
                {
                    string myString_TempPath = "";

                    if (toavoidloadingrevitdlls.executionLocation.Split('|')[0] == "Release")  //constructs a path for release directory (in program files)
                    {
                        myString_TempPath = toavoidloadingrevitdlls.executionLocation.Split('|')[1] + "//Platforms";
                    }
                    if (toavoidloadingrevitdlls.executionLocation.Split('|')[0] == "Dev") //constructs a path for development directory
                    {
                        myString_TempPath = toavoidloadingrevitdlls.executionLocation.Split('|')[1] + "//Platforms";
                    }

                    string stringAGM_FileName = "\\PRL-GM-2020 Adaptive Carrier.rfa";


                    using (Transaction tx = new Transaction(doc, "Load platform."))
                    {
                        tx.Start();
                        doc.LoadFamily(myString_TempPath + stringAGM_FileName);
                        tx.Commit();
                    }
                    myIEnumerableElement = new FilteredElementCollector(doc).WherePasses(new ElementClassFilter(typeof(Family))).Where(x => x.Name == myStringCarrierAdaptive);
                }

                myFamilySymbol_Carrier = doc.GetElement(((Family)myIEnumerableElement.First()).GetFamilySymbolIds().First()) as FamilySymbol;

                if (doc.ActiveView.ViewType == ViewType.ThreeD)//candidate for methodisation 202001072525
                {
                    uidoc.Selection.SetElementIds(new List<ElementId>());

                    View3D myView3D = (View3D)doc.ActiveView;
                    if (myView3D == null || !myView3D.IsSectionBoxActive)
                    {
                        TaskDialog.Show("Plugin stopped", "This works only in 3D views with activated section box..." + Environment.NewLine + Environment.NewLine + "....because platform is placed in the exact centre of section box.");
                        return Result.Succeeded;
                    }

                    BoundingBoxXYZ myBoundingBoxXYZ = myView3D.GetSectionBox();
                    XYZ myXYZ_SectBox_MIN = myBoundingBoxXYZ.Transform.OfPoint(myBoundingBoxXYZ.Min);
                    XYZ myXYZ_SectBox_MAX = myBoundingBoxXYZ.Transform.OfPoint(myBoundingBoxXYZ.Max);

                    Transform myTransform_1205 = Transform.Identity;
                    myTransform_1205.Origin = myXYZ_SectBox_MIN + ((myXYZ_SectBox_MAX - myXYZ_SectBox_MIN) / 2);


                    using (Transaction tx = new Transaction(doc, "Creating platform in 3D View"))
                    {
                        tx.Start();

                        FailureHandlingOptions options = tx.GetFailureHandlingOptions();
                        MyPreProcessor preproccessor = new MyPreProcessor();
                        options.SetFailuresPreprocessor(preproccessor);
                        tx.SetFailureHandlingOptions(options);

                        if (!myFamilySymbol_Carrier.IsActive) myFamilySymbol_Carrier.Activate();

                        FamilyInstance myCarrierCarrier = doc.Create.NewFamilyInstance(myTransform_1205.Origin, myFamilySymbol_Carrier, Autodesk.Revit.DB.Structure.StructuralType.NonStructural);

                        myCarrierCarrier.GetParameters("PRL_WIDTH")[0].Set(3);
                        myCarrierCarrier.GetParameters("PRL_DEPTH")[0].Set(3);

                        uidoc.Selection.SetElementIds(new List<ElementId>() { myCarrierCarrier.Id });
                        tx.Commit();
                    }
                }
                else
                {
                    Reference point_in_3d_01_Reference = null;

                    if (doc.ActiveView.SketchPlane == null)  //PLAN_VIEW_LEVEL
                    {
                        using (Transaction y = new Transaction(doc, "SetSketchPlane"))
                        {
                            y.Start();

                            Plane myPlane = Plane.CreateByNormalAndOrigin(doc.ActiveView.ViewDirection, doc.ActiveView.Origin);
                            doc.ActiveView.SketchPlane = SketchPlane.Create(doc, myPlane);

                            //  List<Category> myListCategory = doc.Settings.Categories.Cast<Level>().Where(x => x.Name == "myString_AssociatedLevel").ToList();

                            ////Plane myPlane = Plane.CreateByNormalAndOrigin(myActiveView.ViewDirection, myActiveView.Origin);
                            ////myActiveView.SketchPlane = SketchPlane.Create(doc, myPlane);
                            //MessageBox.Show("progress");
                            //myActiveView.SketchPlane = myLevel_FromActiveView.GetPlaneReference().;

                            y.Commit();
                        }
                    }

                    Reference myReference_SketchPlane = doc.ActiveView.SketchPlane.GetPlaneReference();  //this is not actually used but needs to be there to avoid the null
                    point_in_3d_01_Reference = myReference_SketchPlane;

                    if (point_in_3d_01_Reference != null)
                    {
                        using (Transaction tx = new Transaction(doc, "Creating platform in 2D View"))
                        {
                            tx.Start();

                            FailureHandlingOptions options = tx.GetFailureHandlingOptions();
                            MyPreProcessor preproccessor = new MyPreProcessor();
                            options.SetFailuresPreprocessor(preproccessor);
                            tx.SetFailureHandlingOptions(options);

                            if (!myFamilySymbol_Carrier.IsActive) myFamilySymbol_Carrier.Activate();

                            //doc.Create.NewFamilyInstance(myTransform_1205.Origin, myFamilySymbol_1808, Autodesk.Revit.DB.Structure.StructuralType.NonStructural);
                            FamilyInstance myCarrierCarrier = AdaptiveComponentInstanceUtils.CreateAdaptiveComponentInstance(doc, myFamilySymbol_Carrier);

                            IList<ElementId> placePointIds_1337 = AdaptiveComponentInstanceUtils.GetInstancePointElementRefIds(myCarrierCarrier);
                            ReferencePoint myReferencePoint = doc.GetElement(placePointIds_1337.First()) as ReferencePoint;

                            XYZ myXYZ_InlieuofReferencePoint_OrCenterOfFaceOtions = myPrivateXYZ_CentreWithIntersectHeight(toavoidloadingrevitdlls);

                            ////point_in_3d_01_Reference = myDirectReference;
                            UV point_in_3d_UV = new UV(myXYZ_InlieuofReferencePoint_OrCenterOfFaceOtions.X, myXYZ_InlieuofReferencePoint_OrCenterOfFaceOtions.Y);

                            myCarrierCarrier.GetParameters("PRL_WIDTH")[0].Set(3);
                            myCarrierCarrier.GetParameters("PRL_DEPTH")[0].Set(3);
                            uidoc.Selection.SetElementIds(new List<ElementId>() { myCarrierCarrier.Id });

                            double myDouble_1130 = ((SketchPlane)doc.GetElement(point_in_3d_01_Reference.ElementId)).GetPlane().XVec.AngleOnPlaneTo(XYZ.BasisX, XYZ.BasisZ);
                            Transform myTransform_1130 = Transform.CreateRotation(uidoc.ActiveView.SketchPlane.GetPlane().Normal, myDouble_1130);
                            XYZ myXYZ_Rotated = myTransform_1130.OfPoint(XYZ.BasisX);
                            UV myUV_1130 = new UV(myXYZ_Rotated.X, myXYZ_Rotated.Y);

                            PointOnPlane myPointOnPlane = toavoidloadingrevitdlls.commandData.Application.Application.Create.NewPointOnPlane(point_in_3d_01_Reference, point_in_3d_UV, myUV_1130, myXYZ_InlieuofReferencePoint_OrCenterOfFaceOtions.Z);
                            eL = 503;
                            // MessageBox.Show("homing here2, why doesn't this work");
                            myReferencePoint.SetPointElementReference(myPointOnPlane);

                            tx.Commit();
                        }
                    }
                }
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("CentreOfScreen, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion

            return Result.Succeeded;
        }



        public class MyPreProcessor : IFailuresPreprocessor
        {
            FailureProcessingResult IFailuresPreprocessor.PreprocessFailures(FailuresAccessor failuresAccessor)
            {
                String transactionName = failuresAccessor.GetTransactionName();

                IList<FailureMessageAccessor> fmas = failuresAccessor.GetFailureMessages();

                if (fmas.Count == 0) return FailureProcessingResult.Continue;

                // We already know the transaction name.

                foreach (FailureMessageAccessor fma in fmas)
                {
                    FailureSeverity fseverity = fma.GetSeverity();

                    // ResolveFailure mimics clicking 
                    // 'Remove Link' button             .
                    if (fseverity == FailureSeverity.Warning) failuresAccessor.DeleteWarning(fma);

                    //failuresAccessor.ResolveFailure(fma);
                    // DeleteWarning mimics clicking 'Ok' button.
                    //failuresAccessor.DeleteWarning( fma );         
                }

                //return FailureProcessingResult
                //  .ProceedWithCommit;
                return FailureProcessingResult.Continue;
            }
        }

    }
}
