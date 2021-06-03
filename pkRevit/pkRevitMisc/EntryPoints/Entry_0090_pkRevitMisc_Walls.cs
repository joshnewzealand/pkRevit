using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

using System.Linq;
using System.Collections.Generic;
using System;

namespace pkRevitMisc.EntryPoints  //Entry_0010_pkRevitDatasheets
{
    public partial class Entry_0090_pkRevitMisc
    {
                

        public Result StartMethod_0090_Walls(ExternalCommandData cd, ref string message, ElementSet elements)
        {
            UIDocument uidoc = cd.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            using (Transaction y = new Transaction(doc, "Foreach on each wall type."))
            {
                FailureHandlingOptions options = y.GetFailureHandlingOptions();
                MyPreProcessor preproccessor = new MyPreProcessor();
                options.SetFailuresPreprocessor(preproccessor);
                y.SetFailureHandlingOptions(options);

                y.Start();
                List<ElementId> myFEC_Walls = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Walls).WhereElementIsNotElementType().Where(x => x.get_Parameter(BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS).AsString() == "Example 2 Walls").Select(x => x.Id).ToList();
                ////if (myFEC_Walls.Count != 0)
                ////{
                ////    doc.Delete(myFEC_Walls);
                ////}
                ////else
                ////{
                XYZ centre3 = null;

                if (doc.ActiveView.ViewType == ViewType.FloorPlan | doc.ActiveView.ViewType == ViewType.EngineeringPlan | doc.ActiveView.ViewType == ViewType.CeilingPlan)
                {
                    IList<XYZ> myXYZ_Corners_1957 = myUIView(uidoc).GetZoomCorners();
                    centre3 = new XYZ(myXYZ_Corners_1957[0].X, myXYZ_Corners_1957[0].Y + ((myXYZ_Corners_1957[1] - myXYZ_Corners_1957[0]) / 2).Y,0);
                }
                else
                {
                    centre3 = XYZ.Zero;
                }

                FilteredElementCollector myFEC_WallTypes = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Walls).WhereElementIsElementType();

                double myX = 0;
                Element myLevel = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Levels).WhereElementIsNotElementType().First();

                foreach (ElementId myElementID in myFEC_WallTypes.ToElementIds())
                {
                    // Creates a geometry line in Revit application
                    XYZ startPoint = new XYZ(centre3.X, centre3.Y + 5, 0);
                    XYZ endPoint = new XYZ(centre3.X, centre3.Y - 5, 0);
                    Line geomLine = Line.CreateBound(startPoint, endPoint);

                    Wall myWall = Wall.Create(doc, geomLine, myElementID, myLevel.Id, 10, 0, false, false);
                    myWall.get_Parameter(BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS).Set("Example 2 Walls");

                    centre3 = new XYZ(centre3.X + 3, centre3.Y, centre3.Z);
                }
                y.Commit();
            }

            return Result.Succeeded;
        }

        public static UIView myUIView(UIDocument uidoc)
        {
            View view = uidoc.Document.ActiveView;

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

        public class MyPreProcessor : IFailuresPreprocessor
        {
            FailureProcessingResult IFailuresPreprocessor.PreprocessFailures(FailuresAccessor failuresAccessor)
            {
                string transactionName = failuresAccessor.GetTransactionName();

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
