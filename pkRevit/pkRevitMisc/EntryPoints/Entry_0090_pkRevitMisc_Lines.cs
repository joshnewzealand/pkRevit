using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

using System.Linq;
using System.Collections.Generic;


namespace pkRevitMisc.EntryPoints  //Entry_0010_pkRevitDatasheets
{
    public partial class Entry_0090_pkRevitMisc
    {
        public Result StartMethod_0090(ExternalCommandData cd, ref string message, ElementSet elements)
        {
            ExternalCommandData commandData = cd;
            string executionLocation = message;

            Document doc = commandData.Application.ActiveUIDocument.Document;

            bool modellines = false;
            bool closing = true;

            View view = doc.ActiveView;

            if (!modellines)
            {
                View3D myView3D = doc.ActiveView as View3D;

                if (myView3D != null)
                {
                    TaskDialog.Show("Not the correct type of view", "'Detail' lines can't be drawn in 3D view.");
                    return Result.Succeeded;
                }
            }

            if (modellines)
            {
                ViewPlan myViewPlan = doc.ActiveView as ViewPlan;

                if (myViewPlan == null)
                {

                    TaskDialog.Show("Not the correct type of view", "The active view must be a view 'plan'");
                    return Result.Succeeded;
                }
            }

            UIView uiview = null;

            IList<UIView> uiviews = commandData.Application.ActiveUIDocument.GetOpenUIViews();

            foreach (UIView uv in uiviews)
            {
                if (uv.ViewId.Equals(view.Id))
                {
                    uiview = uv;
                    break;
                }
            }

            View nextView = doc.GetElement(view.GetPrimaryViewId()) as View;

            if (nextView != null)
            {
                TaskDialog.Show("Cannot proceed", "Please goto the 'parent' view.");
                return Result.Succeeded;
            }

            Rectangle rect = uiview.GetWindowRectangle();
            IList<XYZ> corners = uiview.GetZoomCorners();
            XYZ pXchanges = corners[0];
            XYZ qXchanges = corners[1];

            double divideScreeenWidthBy50 = ((corners[1].X - corners[0].X) / 100);

            double halfOfX = corners[0].X;//corners[0].X + ((corners[1].X - corners[0].X)/2);
            pXchanges = new XYZ(halfOfX + (divideScreeenWidthBy50 * 10), corners[0].Y + (corners[0].Y / 10), 0);
            qXchanges = new XYZ(halfOfX + (divideScreeenWidthBy50 * 10), corners[1].Y - (corners[1].Y / 10), 0);


            XYZ[] pts = new XYZ[] {
            pXchanges,
            qXchanges,
            };

            List<Curve> profile = new List<Curve>(pts.Length); // 2013

            Category myCategory = doc.Settings.Categories.get_Item(BuiltInCategory.OST_Lines);

            CategoryNameMap subcats = myCategory.SubCategories;

            ICollection<ElementId> ls = GetLineStyles(commandData.Application, doc);

            if (ls == null)
            {
                TaskDialog.Show("Cannot proceed", "is null");
                return Result.Succeeded;
            }

            Transaction transaction = new Transaction(doc);
            transaction.Start("TestWall");

            foreach (ElementId lineStyleID in ls)
            {
                if (closing)
                {
                    for (int ii = 0; ii <= pts.Length - 2; ii++)
                    {
                        profile.Add(Line.CreateBound(pts[ii], pts[ii + 1]));
                    }
                }
                else
                {
                    XYZ q = pts[pts.Length - 1];

                    foreach (XYZ p in pts)
                    {
                        profile.Add(Line.CreateBound(q, p)); // 2014
                                                             //profile.Add( Line.CreateBound( p, q ) ); // 2014
                        q = p;
                    }
                }

                for (int ii = 0; ii <= pts.Length - 1; ii++)
                {
                    pts[ii] = new XYZ(pts[ii].X + divideScreeenWidthBy50, pts[ii].Y, pts[ii].Z);
                }

                XYZ normal = XYZ.BasisZ;    // use basis of the z-axis (0,0,1) for normal vector 
                XYZ origin = XYZ.Zero;  // origin is (0,0,0)  
                Plane geomPlane = Plane.CreateByNormalAndOrigin(normal, origin);

                foreach (Curve c in profile) // 2013
                {
                    ModelCurve myModelCurve = null;
                    DetailCurve myDetailCurve = null;

                    if (modellines) myModelCurve = doc.Create.NewModelCurve(c, doc.ActiveView.SketchPlane);
                    //doc.Create.NewDetailCurve(c, doc.ActiveView.SketchPlane );        
                    if (!modellines) myDetailCurve = doc.Create.NewDetailCurve(doc.ActiveView, c);

                    List<ElementId> lsArr = new List<ElementId>();

                    if (modellines) lsArr = myModelCurve.GetLineStyleIds().ToList();
                    if (!modellines) lsArr = myDetailCurve.GetLineStyleIds().ToList();


                    if (modellines) myModelCurve.LineStyle = doc.GetElement(lineStyleID);// as linest;
                    if (!modellines) myDetailCurve.LineStyle = doc.GetElement(lineStyleID);
                }

                profile.Clear();
            }

            transaction.Commit();

            return Result.Succeeded;
        }


        ICollection<ElementId> GetLineStyles(UIApplication m_rvtApp, Document m_rvtDoc)
        {
            Transaction tr =
                new Transaction(m_rvtDoc, "Get Line Styles");
            tr.Start();

            // Create a detail line

            View view = m_rvtDoc.ActiveView;
            XYZ pt1 = XYZ.Zero;
            XYZ pt2 = new XYZ(10.0, 0.0, 0.0);


            XYZ[] pts = new XYZ[] {
            pt1,
            pt2,

	        };

            //CurveArray profile = new CurveArray(); // 2012
            List<Curve> profile = new List<Curve>(pts.Length); // 2013

            for (int ii = 0; ii <= pts.Length - 2; ii++)
            {
                profile.Add(Line.CreateBound(pts[ii], pts[ii + 1]));
            }


            DetailCurve dc = m_rvtDoc.Create.NewDetailCurve(view, profile[0]);

            ICollection<ElementId> lineStyles = dc.GetLineStyleIds();


            tr.RollBack();

            return lineStyles;
        }
    }
}
