//extern alias global2017;


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Autodesk.Revit.UI;



using Autodesk.Revit.DB;  //

namespace _937_PRLoogle_Command02
{
    class _937_PRLoogle_Command02_createArcs
    {
        public partial class ArcsSeveralClass
        {

            public Arc arc;
            public Arc arc2;

        }

        public static string ReplaceLastOccurrence(string Source, string Find, string Replace)
        {
            int place = Source.LastIndexOf(Find);

            if (place == -1)
                return Source;

            string result = Source.Remove(place, Find.Length).Insert(place, Replace);
            return result;
        }

        public static class ThisApplicationConstants
        {
            public const double startAngle = 0;
            public const double endAngle = 2 * Math.PI;
        }



        public static TextNoteType myTextNoteType_2031(UIDocument uidoc, string myTextType)


        {
            FilteredElementCollector collectorUsed = new FilteredElementCollector(uidoc.Document);

            List<Element> listElement = collectorUsed.OfClass(typeof(TextNoteType)).Where(x => x.get_Parameter(BuiltInParameter.ALL_MODEL_TYPE_NAME).AsString() == myTextType).ToList();

            if(listElement.Count == 0)
            {
                listElement = collectorUsed.OfClass(typeof(TextNoteType)).ToList();
            }

            return listElement.FirstOrDefault() as TextNoteType;
        }


        public static IList<XYZ> myXYZ_Corners(UIView uiview)
        {
            return uiview.GetZoomCorners();
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


        public static Level myReference_fromLevel(UIDocument uidoc)
        {
            ElementId levelId = null;

            Parameter level = uidoc.Document.ActiveView.LookupParameter("Associated Level");

            FilteredElementCollector lvlCollector = new FilteredElementCollector(uidoc.Document);
            ICollection<Element> lvlCollection = lvlCollector.OfClass(typeof(Level)).ToElements();

            foreach (Element l in lvlCollection)
            {
                Level lvl = l as Level;
                if (lvl.Name == level.AsString())
                {
                    levelId = lvl.Id;
                    //TaskDialog.Show("test", lvl.Name + "\n"  + lvl.Id.ToString());
                }
            }
            Level myLevel = uidoc.Document.GetElement(levelId) as Level;
            ///////////////Reference myReference = myLevel.GetPlaneReference();

            return myLevel;
        }


        public static ArcsSeveralClass ArcsSeveral(UIDocument uidoc)
        {
            //Rectangle rect2 = uiview.GetWindowRectangle();

            UIView myUIView_1957 = myUIView(uidoc);
            IList<XYZ> myXYZ_Corners_1957 = myXYZ_Corners(myUIView_1957);
            XYZ centre3 = myXYZ_Corners_1957[0] + ((myXYZ_Corners_1957[1] - myXYZ_Corners_1957[0]) / 2);

            Plane plane = Plane.CreateByNormalAndOrigin(XYZ.BasisZ, centre3);
            Arc arc = Arc.Create(plane, myXYZ_Corners_1957[0].DistanceTo(myXYZ_Corners_1957[1]) / 6, ThisApplicationConstants.startAngle, ThisApplicationConstants.endAngle);
            Arc arc2 = Arc.Create(plane, myXYZ_Corners_1957[0].DistanceTo(myXYZ_Corners_1957[1]) / 4, ThisApplicationConstants.startAngle, ThisApplicationConstants.endAngle);

            return new ArcsSeveralClass() { arc = arc, arc2 = arc2 };
        }


        public static TextNoteType MakeArialNarrow(Document doc, string messageTextType, bool DisplayMessageBox, bool myBool_UseTransactions)
        {
            string myString20DegreeFilled = "20 Degree Filled Arrow (PRL)";

            var provider = new ParameterValueProvider(new ElementId(BuiltInParameter.ALL_MODEL_FAMILY_NAME));
            var collector = new FilteredElementCollector(doc).OfClass(typeof(ElementType)).WherePasses(new ElementParameterFilter(new FilterStringRule(provider, new FilterStringEquals(), "Arrowhead", false)));
            var elements = collector.ToElements().Cast<ElementType>().ToList();
            var newArrow = elements.FirstOrDefault(x => x.Name.StartsWith(myString20DegreeFilled));

            TextNoteType textNoteid = new FilteredElementCollector(doc).OfClass(typeof(TextNoteType)).First() as TextNoteType;

            TextNoteType textNote = doc.GetElement(textNoteid.Id) as TextNoteType;
            Transaction trans = new Transaction(doc);
            if(myBool_UseTransactions) trans.Start("Making Arrow and Arial Font");

            if (newArrow == null)
            {
                var arrowDefault = elements.First();
                if (arrowDefault != null)
                {
                    newArrow = arrowDefault.Duplicate(myString20DegreeFilled);
                    newArrow.get_Parameter(BuiltInParameter.ARROW_SIZE).Set((3.0) * (1 / 304.8));
                    newArrow.get_Parameter(BuiltInParameter.ARROW_TYPE).Set("Arrow");
                    newArrow.get_Parameter(BuiltInParameter.ARROW_CLOSED).Set(0);
                    newArrow.get_Parameter(BuiltInParameter.LEADER_ARROW_WIDTH).Set(Math.PI / 8);
                    newArrow.get_Parameter(BuiltInParameter.ARROW_FILLED).Set(1);
                    //newArrow.get_Parameter(BuiltInParameter.arr).Set(1);
                    if (DisplayMessageBox) TaskDialog.Show("LEADER_ARROWHEAD", myString20DegreeFilled);
                }
            }

            Element ele = textNote.Duplicate(messageTextType);
            TextNoteType noteType = ele as TextNoteType;

            if (null != noteType)
            {
                int txtLineWeight = 3;
                int txtBackground = 1;// 0 = Opaque	:: 1 = Transparent
                int txtShowBorder = 0; // 0 = Off :: 1 = On 
                double txtLdrBordOffset = (.25) * (1 / 304.8);
                string txtFont = "Arial Narrow";
                double txtSize = (2.0) * (1 / 304.8);
                double txtTabSize = (5.0) * (1 / 304.8);
                int txtBold = 0;
                int txtItalic = 0;
                int txtUnderline = 0;
                double txtWidth = 1;
                // create color using Color.FromArgb with RGB inputs
                System.Drawing.Color color = System.Drawing.Color.FromArgb(0, 0, 0);
                // convert color into an integer
                int colorInt = System.Drawing.ColorTranslator.ToWin32(color);

                noteType.get_Parameter(BuiltInParameter.LINE_COLOR).Set(colorInt);
                noteType.get_Parameter(BuiltInParameter.LINE_PEN).Set(txtLineWeight);
                noteType.get_Parameter(BuiltInParameter.TEXT_BACKGROUND).Set(txtBackground);
                noteType.get_Parameter(BuiltInParameter.TEXT_BOX_VISIBILITY).Set(txtShowBorder);
                noteType.get_Parameter(BuiltInParameter.LEADER_OFFSET_SHEET).Set(txtLdrBordOffset);
                noteType.get_Parameter(BuiltInParameter.TEXT_FONT).Set(txtFont);
                noteType.get_Parameter(BuiltInParameter.TEXT_SIZE).Set(txtSize);
                noteType.get_Parameter(BuiltInParameter.TEXT_TAB_SIZE).Set(txtTabSize);
                noteType.get_Parameter(BuiltInParameter.TEXT_STYLE_BOLD).Set(txtBold);
                noteType.get_Parameter(BuiltInParameter.TEXT_STYLE_ITALIC).Set(txtItalic);
                noteType.get_Parameter(BuiltInParameter.TEXT_STYLE_UNDERLINE).Set(txtUnderline);
                noteType.get_Parameter(BuiltInParameter.TEXT_WIDTH_SCALE).Set(txtWidth);
                noteType.get_Parameter(BuiltInParameter.LEADER_ARROWHEAD).Set(newArrow.Id);
            }
            if (myBool_UseTransactions) trans.Commit();

            return noteType;
        }
    }
}
