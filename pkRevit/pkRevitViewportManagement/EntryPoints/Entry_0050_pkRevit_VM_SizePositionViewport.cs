using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using pkRevitViewportManagement;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using System.Windows.Forms;


namespace pkRevitMisc.EntryPoints  //Entry_0010_pkRevitDatasheets
{
    public partial class Entry_0050_pkRevit_VM
    {

        int eL = -1;

        public Result StartMethod_0050(ExternalCommandData cd, ref string message, ElementSet elements)
        {
            ExternalCommandData commandData = cd;
            string executionLocation = message;

            ToAvoidLoadingRevitDLLs toavoidloadingrevitdlls = new ToAvoidLoadingRevitDLLs();
            toavoidloadingrevitdlls.commandData = commandData;
            toavoidloadingrevitdlls.executionLocation = executionLocation;

            try
            {
                if(true)
                {
                    Window_SizePositionViewport window1548_SizePositionViewport = new Window_SizePositionViewport(toavoidloadingrevitdlls);
                    window1548_SizePositionViewport.Show();
                }

                if (false)
                {
                    UIDocument uidoc = commandData.Application.ActiveUIDocument;
                    Document doc = uidoc.Document;

                    using (Transaction trans = new Transaction(doc, "Create View Name"))
                    {
                        trans.Start();

                        FilteredElementCollector fillRegionTypes = new FilteredElementCollector(doc).OfClass(typeof(FilledRegionType));

                        FilteredElementCollector fillPatternsDrafting = new FilteredElementCollector(doc).OfClass(typeof(FillPatternElement));

                        UIView myUIView_1957 = myUIView(uidoc);
                        IList<XYZ> myXYZ_Corners_1957 = myXYZ_Corners(myUIView_1957);
                        XYZ centre3 = myXYZ_Corners_1957[0] + ((myXYZ_Corners_1957[1] - myXYZ_Corners_1957[0]) / 2);

                        Color myColor_Random01 = new Autodesk.Revit.DB.Color(System.Drawing.Color.LightPink.R, System.Drawing.Color.LightPink.G, System.Drawing.Color.LightPink.B);
                        colour_AndPosition(doc, centre3, "Solid Light Pink", myColor_Random01, fillRegionTypes, fillPatternsDrafting);

                        Color myColor_Random02 = new Autodesk.Revit.DB.Color(System.Drawing.Color.LightGreen.R, System.Drawing.Color.LightGreen.G, System.Drawing.Color.LightGreen.B);
                        colour_AndPosition(doc, centre3 + new XYZ(6, 0, 0), "Light Green", myColor_Random02, fillRegionTypes, fillPatternsDrafting);

                        Color myColor_Random03 = new Autodesk.Revit.DB.Color(System.Drawing.Color.LightBlue.R, System.Drawing.Color.LightBlue.G, System.Drawing.Color.LightBlue.B);
                        colour_AndPosition(doc, centre3 + new XYZ(-6, 0, 0), "Light Blue", myColor_Random03, fillRegionTypes, fillPatternsDrafting);

                        trans.Commit();
                    }
                }
                if(false)
                {

                    Window_SizePositionViewport window1548_SizePositionViewport = new Window_SizePositionViewport(toavoidloadingrevitdlls);
                    window1548_SizePositionViewport.Show();
                    ///

                    UIDocument uidoc = commandData.Application.ActiveUIDocument;
                    Document doc = uidoc.Document;


                    Autodesk.Revit.DB.View currentView = doc.ActiveView;



                    BoundingBoxXYZ CurrentViewBox = currentView.CropBox;


                    Transform myTransform = Transform.Identity;

                    myTransform.Origin = CurrentViewBox.Transform.Origin + new XYZ(currentView.get_Parameter(BuiltInParameter.VIEWER_BOUND_OFFSET_FAR).AsDouble(), 0, 0);
                    myTransform.BasisY = CurrentViewBox.Transform.BasisY;
                    myTransform.BasisZ = new XYZ(-CurrentViewBox.Transform.BasisZ.X, -CurrentViewBox.Transform.BasisZ.Y, -CurrentViewBox.Transform.BasisZ.Z);
                    myTransform.BasisX = new XYZ(-CurrentViewBox.Transform.BasisX.X, -CurrentViewBox.Transform.BasisX.Y, -CurrentViewBox.Transform.BasisX.Z);

                    // BoundingBoxXYZ myBB = new BoundingBoxXYZ();


                    BoundingBoxXYZ sectionBox = CurrentViewBox;

                    sectionBox.Transform = myTransform;

                    //  Transform mirror = Transform.CreateRotation(sectionBox.Transform.Origin, Math.PI);

                    // sectionBox.Transform = sectionBox.Transform.Multiply(mirror);

                    sectionBox.set_MinEnabled(0, true);
                    sectionBox.set_MinEnabled(1, true);
                    sectionBox.set_MinEnabled(2, true);

                    sectionBox.set_MaxEnabled(0, true);
                    sectionBox.set_MaxEnabled(1, true);
                    sectionBox.set_MaxEnabled(2, true);



                    #region getting the type



                    IEnumerable<ViewFamilyType> viewFamilyTypes = from elem in new FilteredElementCollector(doc).OfClass(typeof(ViewFamilyType))
                                                                  let type = elem as ViewFamilyType
                                                                  where type.ViewFamily == ViewFamily.Section
                                                                  select type;

                    #endregion

                    using (Transaction trans = new Transaction(doc, "Create View Name"))
                    {
                        trans.Start();



                        sectionBox.Transform.Origin = doc.ActiveView.Origin;

                        ViewSection elevView = ViewSection.CreateSection(doc, viewFamilyTypes.First().Id, sectionBox);

                        doc.Regenerate();

                        MessageBox.Show(elevView.Origin + "  " + doc.ActiveView.Origin);
                        //elevView.get_BoundingBox().Transform.Origin = m_doc.ActiveView.Origin;

                        elevView.Name = "test view";
                        trans.Commit();
                    }
                    return Result.Succeeded;
                }

                return Result.Succeeded;
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("Entry_0050_pkRevitMisc, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion

            return Result.Succeeded;
        }

        private void colour_AndPosition(Document doc, XYZ centre3, string nameOfColour, Color myColor_Random01, FilteredElementCollector fillRegionTypes, FilteredElementCollector fillPatternsDrafting) //"Solid Light Pink"
        {
            IEnumerable<FilledRegionType> myPatterns =
                from pattern in fillRegionTypes.Cast<FilledRegionType>()
                where pattern.Name.Equals(nameOfColour)
                select pattern;

            if (myPatterns.Count() == 0)
            {
                ElementType regionType = fillRegionTypes.First() as ElementType;
                FilledRegionType regionCyan = regionType.Duplicate(nameOfColour) as FilledRegionType;
                regionCyan.ForegroundPatternId = fillPatternsDrafting.First().Id;
                regionCyan.ForegroundPatternColor = myColor_Random01;

                createFilledRegions(doc, centre3, regionCyan.Id);
            }
            else createFilledRegions(doc, centre3, myPatterns.First().Id);
        }


        private void createFilledRegions(Document doc,  XYZ centre3, ElementId regionID)
        {
            List<CurveLoop> profileloops = new List<CurveLoop>();

            XYZ[] points = new XYZ[5];
            points[0] = new XYZ(0.0, 0.0, 0.0) + centre3;
            points[1] = new XYZ(5.0, 0.0, 0.0) + centre3;
            points[2] = new XYZ(5.0, 5.0, 0.0) + centre3;
            points[3] = new XYZ(0.0, 5.0, 0.0) + centre3;
            points[4] = new XYZ(0.0, 0.0, 0.0) + centre3;

            CurveLoop profileloop = new CurveLoop();

            for (int i = 0; i < 4; i++)
            {
                Line line = Line.CreateBound(points[i], points[i + 1]);
                profileloop.Append(line);
            }
            profileloops.Add(profileloop);

            ElementId activeViewId = doc.ActiveView.Id;

            FilledRegion filledRegion = FilledRegion.Create(doc, regionID, activeViewId, profileloops);
        }


        public static IList<XYZ> myXYZ_Corners(UIView uiview)
        {
            return uiview.GetZoomCorners();
        }

        public static UIView myUIView(UIDocument uidoc)
        {
            Autodesk.Revit.DB.View view = uidoc.Document.ActiveView;

            UIView uiview = null;
            IList<UIView> uiviews = uidoc.GetOpenUIViews();

            foreach (UIView uv in uiviews)
            {
                if (uv.ViewId.Equals(view.Id))
                {
                    uiview = uv;
                    break;
                }
            }
            return uiview;
        }
    }
}
