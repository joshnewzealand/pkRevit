using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

using View = Autodesk.Revit.DB.View;

using System.Windows.Forms;
using System;
using System.Collections.Generic;

namespace pkRevitMisc.EntryPoints  //Entry_0010_pkRevitDatasheets
{
    public partial class Entry_0180_pkRevitMisc
    {
        public class MyPreProcessor : IFailuresPreprocessor
        {
            FailureProcessingResult IFailuresPreprocessor.PreprocessFailures(FailuresAccessor failuresAccessor)
            {
                String transactionName = failuresAccessor.GetTransactionName();
                IList<FailureMessageAccessor> fmas = failuresAccessor.GetFailureMessages();
                if (fmas.Count == 0) return FailureProcessingResult.Continue;
                foreach (FailureMessageAccessor fma in fmas)
                {
                    FailureSeverity fseverity = fma.GetSeverity();
                    if (fseverity == FailureSeverity.Warning) failuresAccessor.DeleteWarning(fma);
                }
                return FailureProcessingResult.Continue;
            }
        }

        public class mouth
        {
            public static double mouthEdgeY = -0.9;  //make -.9  or -1.3

            public static XYZ mouthEdge1 = new XYZ(-.7, mouthEdgeY, 0);
            public static XYZ mouthEdge2 = new XYZ(.7, mouthEdgeY, 0);
            public static XYZ mouthCentre = new XYZ(0, -1.25, 0);   //make -1.25 or -.75
        }

        public Result StartMethod_0180(ExternalCommandData cd, ref string message, ElementSet elements)
        {
            ExternalCommandData commandData = cd;
            string executionLocation = message;

            ToAvoidLoadingRevitDLLs toavoidloadingrevitdlls = new ToAvoidLoadingRevitDLLs();
            toavoidloadingrevitdlls.commandData = commandData;
            toavoidloadingrevitdlls.executionLocation = executionLocation;

            //MessageBox.Show("0180_SmileyFace-2");

            ///is the current view a 3d type view
            ///
            View view = commandData.Application.ActiveUIDocument.ActiveView;

            if(view.ViewType == ViewType.ThreeD | view.ViewType == ViewType.Walkthrough)
            {
                try
                {
                    UIDocument uidoc = commandData.Application.ActiveUIDocument;
                    Document doc = uidoc.Document; // myListView_ALL_Fam_Master.Items.Add(doc.GetElement(uidoc.Selection.GetElementIds().First()).Name);

                    do
                    {
                        ///                TECHNIQUE 09 OF 19 (EE09_Draw3D_ModelLines.cs)
                        ///↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓ DRAWING 3D MODEL LINES (A SIMILY FACE) ↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓
                        ///
                        /// Interfaces and ENUM's:
                        ///     FailureHandlingOptions
                        ///     
                        /// 
                        /// Demonstrates classes:
                        ///     Arc
                        ///     Line
                        ///     Curve
                        ///     Ellipse
                        /// 
                        /// 
                        /// Key methods:
                        ///     Arc.Create(end02, end12, pointOnCurve2);
                        ///     Arc.Create(myTransform.OfPoint(new XYZ(-.6, 0.5, 0)), radius, startAngle, endAngle, xVec, yVec);
                        ///     Ellipse.CreateCurve(pickedRef.GlobalPoint, radiusEllipse, radiusEllipse2, xVec, yVec, param0, param1);
                        ///     doc.Create.NewModelCurve(
                        /// 
                        ///
                        ///
                        /// * class is actually part of the .NET framework (not Revit API)
                        ///	https://github.com/joshnewzealand/Revit-API-Playpen-CSharp

                        Reference pickedRef = null;
                        try
                        {
                            pickedRef = uidoc.Selection.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Face, "Please select a Face");
                        }

                        #region catch and finally
                        catch (Exception ex)
                        {
                        }
                        finally
                        {
                        }
                        #endregion

                        if (pickedRef == null) break;

                        Element myElement = doc.GetElement(pickedRef.ElementId);
                        Face myFace = myElement.GetGeometryObjectFromReference(pickedRef) as Face;

                        if (myFace == null) return Result.Succeeded;

                        Transform myXYZ_FamilyTransform = Transform.Identity;

                        if (pickedRef.ConvertToStableRepresentation(doc).Contains("INSTANCE"))
                        {
                            myXYZ_FamilyTransform = (myElement as FamilyInstance).GetTotalTransform();
                        }

                        Transform myTransform = Transform.Identity;

                        if (myFace.GetType() != typeof(PlanarFace)) continue;

                        PlanarFace myPlanarFace = myFace as PlanarFace;

                        myTransform.Origin = pickedRef.GlobalPoint;
                        myTransform.BasisX = myXYZ_FamilyTransform.OfVector(myPlanarFace.XVector);
                        myTransform.BasisY = myXYZ_FamilyTransform.OfVector(myPlanarFace.YVector);
                        myTransform.BasisZ = myXYZ_FamilyTransform.OfVector(myPlanarFace.FaceNormal);

                        // Create a geometry line
                        XYZ startPoint = new XYZ(0, 0, 0);
                        XYZ endPoint = new XYZ(10, 10, 0);

                        // Create a arc for the mouth
                        XYZ origin = myTransform.OfPoint(new XYZ(0, 0, 0));
                        XYZ normal = myTransform.OfPoint(new XYZ(1, 1, 0));
                        XYZ end02 = myTransform.OfPoint(mouth.mouthEdge1);  //make -.9
                        XYZ end12 = myTransform.OfPoint(mouth.mouthEdge2);  //make -.9
                        XYZ pointOnCurve2 = myTransform.OfPoint(mouth.mouthCentre); //make -0.75
                        Arc geomArc2 = Arc.Create(end02, end12, pointOnCurve2);

                        // Create a geometry circle in Revit application
                        XYZ xVec = myTransform.OfVector(XYZ.BasisX);
                        XYZ yVec = myTransform.OfVector(XYZ.BasisY);
                        double startAngle = 0;
                        double endAngle = 2 * Math.PI;
                        double radius = .3;
                        Arc geomPlane2 = Arc.Create(myTransform.OfPoint(new XYZ(-.6, 0.5, 0)), radius, startAngle, endAngle, xVec, yVec);
                        Arc geomPlane3 = Arc.Create(myTransform.OfPoint(new XYZ(.6, 0.5, 0)), radius, startAngle, endAngle, xVec, yVec);

                        ////////////////////// straight lines
                        Line L2 = Line.CreateBound(myTransform.OfPoint(new XYZ(-.2, -.5, 0)), myTransform.OfPoint(new XYZ(.2, -.5, 0)));
                        Line L3 = Line.CreateBound(myTransform.OfPoint(new XYZ(.2, -.5, 0)), myTransform.OfPoint(new XYZ(0, .1, 0)));

                        // Create a ellipse
                        double param0 = 0.0;
                        double param1 = 2 * Math.PI;
                        double radiusEllipse = 1.4;
                        double radiusEllipse2 = 1.4 * 1.2;
                        Curve myCurve_Ellipse = Ellipse.CreateCurve(pickedRef.GlobalPoint, radiusEllipse, radiusEllipse2, xVec, yVec, param0, param1);

                        using (Transaction y = new Transaction(doc, "Simily Face"))
                        {
                            y.Start();

                            FailureHandlingOptions options = y.GetFailureHandlingOptions();
                            options.SetFailuresPreprocessor(new MyPreProcessor());
                            y.SetFailureHandlingOptions(options);

                            SketchPlane sketch = SketchPlane.Create(doc, pickedRef);

                            try
                            {
                                doc.Create.NewModelCurve(geomArc2, sketch); //mouth
                                doc.Create.NewModelCurve(myCurve_Ellipse, sketch); //head
                                doc.Create.NewModelCurve(geomPlane2, sketch); //eye
                                doc.Create.NewModelCurve(geomPlane3, sketch); //eye
                                doc.Create.NewModelCurve(L2, sketch); //nose
                                doc.Create.NewModelCurve(L3, sketch); //nose
                            }

                            #region catch and finally
                            catch (Exception ex)
                            {
                                if (ex.Message != "Curve must be in the plane\r\nParameter name: pCurveCopy")
                                {
                                    _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("EE01_Part1" + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
                                }
                            }
                            finally
                            {
                            }
                            #endregion

                            y.Commit();
                        }

                        break;

                    } while (true);

                }

                #region catch and finally
                catch (Exception ex)
                {
                    if (ex.Message != "The user aborted the pick operation.")
                    {
                        _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("EE09_Draw3D_ModelLines" + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
                    }
                    else
                    {
                    }
                }
                finally
                {
                }
                #endregion
            }
            else
            {
                try
                {
                    UIDocument uidoc =  commandData.Application.ActiveUIDocument;
                    Document doc = uidoc.Document; // myListView_ALL_Fam_Master.Items.Add(doc.GetElement(uidoc.Selection.GetElementIds().First()).Name);


                    if (doc.ActiveView.ViewType != ViewType.AreaPlan & doc.ActiveView.ViewType != ViewType.FloorPlan & doc.ActiveView.ViewType != ViewType.CeilingPlan & doc.ActiveView.ViewType != ViewType.DraftingView & doc.ActiveView.ViewType != ViewType.Detail & doc.ActiveView.ViewType != ViewType.DrawingSheet & doc.ActiveView.ViewType != ViewType.EngineeringPlan & doc.ActiveView.ViewType != ViewType.Legend)
                    {
                        TaskDialog.Show("Plugin stopped", "Active view is not 2D and will not receive Detail Lines");
                        return Result.Succeeded;
                    }


                    UIView uiview = null;

                    IList<UIView> uiviews = uidoc.GetOpenUIViews();

                    foreach (UIView uv in uiviews)
                    {
                        if (uv.ViewId.Equals(doc.ActiveView.Id))
                        {
                            uiview = uv;
                            break;
                        }
                    }

                    ///                TECHNIQUE 10 OF 19 (EE10_Draw2D_DetailLines.cs)
                    ///↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓ DRAWING 2D DETAIL LINES (A SIMILY FACE) ↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓
                    ///
                    /// Interfaces and ENUM's:
                    ///     
                    /// 
                    /// Demonstrates classes:
                    ///     UIDocument
                    /// 
                    /// 
                    /// Key methods:
                    ///     uiview.GetZoomCorners(
                    ///     doc.Create.NewDetailCurve(
                    /// 
                    ///
                    ///
                    /// * class is actually part of the .NET framework (not Revit API)
                    ///	https://github.com/joshnewzealand/Revit-API-Playpen-CSharp



                    using (Transaction tx = new Transaction(doc))
                    {
                        tx.Start("Drawing Detail Lines");

                        XYZ myXYZ_Corner1 = uiview.GetZoomCorners()[0];
                        XYZ myXYZ_Corner2 = uiview.GetZoomCorners()[1];

                        XYZ myXYZ_Centre = uiview.GetZoomCorners()[0] + ((uiview.GetZoomCorners()[1] - uiview.GetZoomCorners()[0]) / 2);


                        // Create a geometry line
                        XYZ startPoint = new XYZ(0, 0, 0);
                        XYZ endPoint = new XYZ(10, 10, 0);

                        // Create a ellipse
                        XYZ origin = new XYZ(0, 0, 0);
                        XYZ normal = new XYZ(1, 1, 0);

                        XYZ end02 = myXYZ_Centre + mouth.mouthEdge1;
                        XYZ end12 = myXYZ_Centre + mouth.mouthEdge2;
                        XYZ pointOnCurve2 = myXYZ_Centre + mouth.mouthCentre;
                        Arc geomArc2 = Arc.Create(end02, end12, pointOnCurve2);

                        // Create a geometry circle in Revit application
                        XYZ xVec = XYZ.BasisX;
                        XYZ yVec = XYZ.BasisY;
                        double startAngle = 0;
                        double endAngle = 2 * Math.PI;
                        double radius = .3;
                        Arc geomPlane2 = Arc.Create(myXYZ_Centre + new XYZ(-.6, 0.5, 0), radius, startAngle, endAngle, xVec, yVec);
                        Arc geomPlane3 = Arc.Create(myXYZ_Centre + new XYZ(.6, 0.5, 0), radius, startAngle, endAngle, xVec, yVec);

                        // stright lines
                        Line L2 = Line.CreateBound(myXYZ_Centre + new XYZ(-.2, -.5, 0), myXYZ_Centre + new XYZ(.2, -.5, 0));
                        Line L3 = Line.CreateBound(myXYZ_Centre + new XYZ(.2, -.5, 0), myXYZ_Centre + new XYZ(0, .1, 0));

                        double param0 = 0.0;
                        double param1 = 2 * Math.PI;
                        double radiusEllipse = 1.4;
                        double radiusEllipse2 = 1.4 * 1.2;
                        Curve myCurve_Ellipse = Ellipse.CreateCurve(myXYZ_Centre, radiusEllipse, radiusEllipse2, xVec, yVec, param0, param1);

                        doc.Create.NewDetailCurve(doc.ActiveView, geomArc2);
                        doc.Create.NewDetailCurve(doc.ActiveView, myCurve_Ellipse);
                        doc.Create.NewDetailCurve(doc.ActiveView, geomPlane2);
                        doc.Create.NewDetailCurve(doc.ActiveView, geomPlane3);
                        doc.Create.NewDetailCurve(doc.ActiveView, L2);
                        doc.Create.NewDetailCurve(doc.ActiveView, L3);

                        tx.Commit();
                    }
                }

                #region catch and finally
                catch (Exception ex)
                {
                    _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("EE10_Draw2D_DetailLines" + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
                }
                finally
                {
                }
                #endregion
            }

            return Result.Succeeded;
        }
    }
}
