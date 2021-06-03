using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

using System.Linq;
using System.Collections.Generic;
using System;

namespace pkRevitMisc.EntryPoints  //Entry_0010_pkRevitDatasheets
{
    public partial class Entry_0090_pkRevitMisc
    {
        public Result StartMethod_0090_FilledRegions(ExternalCommandData cd, ref string message, ElementSet elements)
        {
            UIDocument uidoc = cd.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            using (Transaction y = new Transaction(doc, "Foreach on each wall type."))
            {
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

            return Result.Succeeded;
        }

        public static IList<XYZ> myXYZ_Corners(UIView uiview)
        {
            return uiview.GetZoomCorners();
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

        private void createFilledRegions(Document doc, XYZ centre3, ElementId regionID)
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
    }
}
