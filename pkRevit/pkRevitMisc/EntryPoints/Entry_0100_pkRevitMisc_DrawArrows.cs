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
using _952_PRLoogleClassLibrary;
using Binding = Autodesk.Revit.DB.Binding;
using View = Autodesk.Revit.DB.View;
using System.IO;
using System.Diagnostics;
using Autodesk.Revit.DB.ExtensibleStorage;
using System.Globalization;
using System.Windows;
using MessageBox = System.Windows.Forms.MessageBox;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.UI.Selection;


namespace pkRevitMisc.EntryPoints  //Entry_0010_pkRevitDatasheets
{
    public enum ARROW_TYPE_CONVERT
    {
        LOOP = 12,
        ELEVATION_TARGET = 11,
        BOX = 10,
        DATUM_TRIANGLE = 9,
        ARROW = 8,
        HEAVY_END_TICK_MARK = 7,
        DOT = 3,
        DIAGONAL = 0
    }

    public enum FILL
    {
        FILLED = 1,
        UNFILLED = 0
    }

    public enum CLOS
    {
        CLOSED = 1,
        UNCLOSED = 0
    }


    public struct ANGLES
    {
        public const double ANG_90 = 1.5708;
        public const double ANG_30 = 0.523599;
        public const double ANG_20 = 0.349066;
        public const double ANG_15 = 0.261799;
    }


    public struct SingleArrow
    {

        //public static string Name = "Arrow 30 Degree";
        //public static double ARROW_SIZE = (3.0) * (1 / 304.8);
        //public static string ARROW_TYPE = "Arrow";
        //public static int ARROW_CLOSED = 0;
        //public static double LEADER_ARROW_WIDTH = Math.PI / 8;
        //public static int ARROW_FILLED = 1;

        public string Name { get; set; }
        public double ARROW_SIZE { get; set; }
        public int ARROW_TYPE { get; set; }
        public int ARROW_CLOSED { get; set; }
        public double LEADER_ARROW_WIDTH { get; set; }
        public int ARROW_FILLED { get; set; }

        public List<SingleArrow> goProps { get; set; }
    }

    public partial class Entry_0100_pkRevitMisc
    {
        public List<SingleArrow> list_SingleArrow { get; set; }

        public void addSingleArrow(string Name, double ARROW_SIZE, int ARROW_TYPE, int ARROW_CLOSED, double LEADER_ARROW_WIDTH, int ARROW_FILLED)
        {
            if (list_SingleArrow == null) list_SingleArrow = new List<SingleArrow>();
            list_SingleArrow.Add(new SingleArrow() { Name = Name, ARROW_SIZE = ARROW_SIZE, ARROW_TYPE = ARROW_TYPE, ARROW_CLOSED = ARROW_CLOSED, LEADER_ARROW_WIDTH = LEADER_ARROW_WIDTH, ARROW_FILLED = ARROW_FILLED });
        }

        //public double ANG_90 { get; set; } = 1.5708;
        //public double ANG_30 { get; set; } = 0.523599;
        //public double ANG_20 { get; set; } = 0.349066;
        //public double ANG_15 { get; set; } = 0.261799;


        public class FamilyOptionOverWrite : IFamilyLoadOptions
        {
            public bool OnFamilyFound(bool familyInUse, out bool overwriteParameterValues)
            {
                overwriteParameterValues = true;
                return true;
            }
            public bool OnSharedFamilyFound(Family sharedFamily, bool familyInUse, out FamilySource source, out bool overwriteParameterValues)
            {
                source = FamilySource.Family;
                overwriteParameterValues = true;
                return true;
            }
        }

        public Result StartMethod_0100(ExternalCommandData cd, ref string message, ElementSet elements)
        {

            int eL = -1;

            try
            {
                ExternalCommandData commandData = cd;
                UIDocument uidoc = commandData.Application.ActiveUIDocument;
                Document doc = uidoc.Document; // myListView_ALL_Fam_Master.Items.Add(doc.GetElement(uidoc.Selection.GetElementIds().First()).Name);

                ViewPlan myViewPlan = doc.ActiveView as ViewPlan;

                if (doc.ActiveView.ViewType != ViewType.FloorPlan & doc.ActiveView.ViewType != ViewType.EngineeringPlan & doc.ActiveView.ViewType != ViewType.CeilingPlan)
                {
                    TaskDialog.Show("Not the correct type of view", "The active view must be a view 'plan'");
                    return Result.Succeeded;
                }

                eL = 134;
                using (Transaction tx = new Transaction(doc))
                {
                    tx.Start("EE22_ArrowWork");
                    {
                        string myString_FamilyName = "PRL-GA Leader";
                        //string myString_FamilyName = "PRL-GA LeaderSheet";
                        List<FamilySymbol> myListFamilySymbol_Original = null;

                        eL = 142;

                        if (true)  //candidate for methodisation 20201212, but don't forget the two if's immediately after
                        {
                            List<Element> myListFamilySymbol_1738 = new FilteredElementCollector(doc).WherePasses(new ElementClassFilter(typeof(Family))).Where(x => x.Name == myString_FamilyName).ToList();

                            eL = 146;

                            if (myListFamilySymbol_1738.Count != 1)
                            {
                                string myString_TempPath = "";

                                if (message.Split('|')[0] == "Release")  //constructs a path for release directory (in program files)
                                {
                                    myString_TempPath = message.Split('|')[1] + "\\Families";
                                }
                                if (message.Split('|')[0] == "Dev") //constructs a path for development directory
                                {
                                    myString_TempPath = message.Split('|')[1] + "\\Families";
                                }
                                eL = 158;

                                string string_MetricDetailItem = myString_FamilyName + " 2019";
                                if (commandData.Application.Application.VersionNumber == "2020") string_MetricDetailItem = myString_FamilyName + " 2020";
                                if (commandData.Application.Application.VersionNumber == "2021") string_MetricDetailItem = myString_FamilyName + " 2021";
                                if (commandData.Application.Application.VersionNumber == "2022") string_MetricDetailItem = myString_FamilyName + " 2022";

                                eL = 172;

                               /// System.IO.File.Copy(myString_TempPath + string_MetricDetailItem, System.IO.Path.GetTempPath() + "\\" + myString_FamilyName + ".rfa", true);

                                ////DatabaseMethods.writeDebug(myString_TempPath + string_MetricDetailItem, true);

                                ////Document famDoc = commandData.Application.Application.NewFamilyDocument(myString_TempPath + string_MetricDetailItem);//uid.Document.Application.NewFamilyDocument(System.Environment.GetEnvironmentVariable("ProgramData") + "\\Autodesk\\RVT 2017\\Family Templates\\English\\Metric Detail Item.rft");

                                ////eL = 176;

                                ////famDoc.SaveAs(System.IO.Path.GetTempPath() + "\\" + myString_FamilyName, new SaveAsOptions() { OverwriteExistingFile = true});
                                ////famDoc.Close();

                                eL = 175;

                                doc.LoadFamily(myString_TempPath + "\\" + string_MetricDetailItem + ".rfa", new FamilyOptionOverWrite(), out Family myFamily);
                                myListFamilySymbol_1738 = new FilteredElementCollector(doc).WherePasses(new ElementClassFilter(typeof(Family))).Where(x => x.Name == string_MetricDetailItem).ToList();

                                //MessageBox.Show(myListFamilySymbol_1738.Count().ToString());
                            }

                            eL = 163;

                            myListFamilySymbol_Original = ((Family)myListFamilySymbol_1738.First()).GetFamilySymbolIds().Select(x => doc.GetElement(x) as FamilySymbol).OrderBy(x => x.Name).Reverse().ToList();
                            eL = 163;
                        }
                        if (myListFamilySymbol_Original == null) return Result.Succeeded; 
                        if (myListFamilySymbol_Original.Count() == 0) return Result.Succeeded;

                        eL = 167;
                        if (true) //candidate for methodisation 20201213
                        {
                            List<Element> listArrowheads_notNamedArrowHead = null;

                            if (true)
                            {
                                ElementId id = new ElementId(BuiltInParameter.ALL_MODEL_FAMILY_NAME);

                                ParameterValueProvider provider = new ParameterValueProvider(id);
                                FilterStringRuleEvaluator evaluator = new FilterStringEquals();

                                FilterRule rule = new FilterStringRule(provider, evaluator, "Arrowhead", false);

                                ElementParameterFilter filter = new ElementParameterFilter(rule);
                                FilteredElementCollector collector = new FilteredElementCollector(doc).OfClass(typeof(ElementType)).WherePasses(filter);

                                listArrowheads_notNamedArrowHead = collector.Where(x => x.Name != "Arrowhead").ToList();
                            }

                            if (listArrowheads_notNamedArrowHead == null) return Result.Succeeded; ;
                            if (listArrowheads_notNamedArrowHead.Count() == 0) return Result.Succeeded; ;


                            List<FamilySymbol> myListFamilySymbol_Purity = myListFamilySymbol_Original.Where(x => listArrowheads_notNamedArrowHead.Any(y => y.Name == x.Name)).ToList();

                            foreach (FamilySymbol famSym in myListFamilySymbol_Purity)
                            {
                                if (famSym.get_Parameter(BuiltInParameter.LEADER_ARROWHEAD).AsElementId().IntegerValue == -1)
                                {
                                    famSym.get_Parameter(BuiltInParameter.LEADER_ARROWHEAD).Set(listArrowheads_notNamedArrowHead.Where(x => x.Name == famSym.Name).First().Id);
                                }
                            }

                            List<FamilySymbol> myListFamilySymbol_NotFound = myListFamilySymbol_Original.ToList();

                            foreach (FamilySymbol famSym in myListFamilySymbol_Purity)
                            {
                                myListFamilySymbol_NotFound.Remove(famSym);
                            }
                            eL = 211;

                            addSingleArrow("Arrow 15 Degree", (3.0) * (1 / 304.8), (int)ARROW_TYPE_CONVERT.ARROW, (int)CLOS.UNCLOSED, ANGLES.ANG_15, (int)FILL.UNFILLED);
                            addSingleArrow("Arrow 30 Degree", (3.0) * (1 / 304.8), (int)ARROW_TYPE_CONVERT.ARROW, (int)CLOS.UNCLOSED, ANGLES.ANG_30, (int)FILL.UNFILLED);
                            addSingleArrow("Arrow Filled 15 Degree", (3.0) * (1 / 304.8), (int)ARROW_TYPE_CONVERT.ARROW, (int)CLOS.UNCLOSED, ANGLES.ANG_15, (int)FILL.FILLED);
                            addSingleArrow("Arrow Filled 20 Degree", (3.0) * (1 / 304.8), (int)ARROW_TYPE_CONVERT.ARROW, (int)CLOS.UNCLOSED, ANGLES.ANG_20, (int)FILL.FILLED);
                            addSingleArrow("Arrow Filled 30 Degree", (3.0) * (1 / 304.8), (int)ARROW_TYPE_CONVERT.ARROW, (int)CLOS.UNCLOSED, ANGLES.ANG_30, (int)FILL.FILLED);
                            addSingleArrow("Arrow Open 90 Degree 1.25mm", (1.25) * (1 / 304.8), (int)ARROW_TYPE_CONVERT.ARROW, (int)CLOS.UNCLOSED, ANGLES.ANG_90, (int)FILL.UNFILLED);
                            addSingleArrow("Diagonal 3mm", (3.0) * (1 / 304.8), (int)ARROW_TYPE_CONVERT.DIAGONAL, (int)CLOS.UNCLOSED, ANGLES.ANG_30, (int)FILL.UNFILLED);
                            addSingleArrow("Filled Box 3mm", (3.0) * (1 / 304.8), (int)ARROW_TYPE_CONVERT.BOX, (int)CLOS.UNCLOSED, ANGLES.ANG_30, (int)FILL.FILLED);
                            addSingleArrow("Filled Dot 1.5mm", (1.5) * (1 / 304.8), (int)ARROW_TYPE_CONVERT.DOT, (int)CLOS.UNCLOSED, ANGLES.ANG_30, (int)FILL.FILLED);
                            addSingleArrow("Filled Dot 3mm", (3.0) * (1 / 304.8), (int)ARROW_TYPE_CONVERT.DOT, (int)CLOS.UNCLOSED, ANGLES.ANG_30, (int)FILL.FILLED);
                            addSingleArrow("Filled Elevation Target 4mm", (4.0) * (1 / 304.8), (int)ARROW_TYPE_CONVERT.ELEVATION_TARGET, (int)CLOS.UNCLOSED, ANGLES.ANG_30, (int)FILL.FILLED);
                            addSingleArrow("Filled Triangle 2.5mm", (2.5) * (1 / 304.8), (int)ARROW_TYPE_CONVERT.DATUM_TRIANGLE, (int)CLOS.UNCLOSED, ANGLES.ANG_30, (int)FILL.FILLED);
                            addSingleArrow("Heavy End 3mm", (3.0) * (1 / 304.8), (int)ARROW_TYPE_CONVERT.HEAVY_END_TICK_MARK, (int)CLOS.UNCLOSED, ANGLES.ANG_30, (int)FILL.FILLED);
                            addSingleArrow("Open Box 3mm", (3.0) * (1 / 304.8), (int)ARROW_TYPE_CONVERT.BOX, (int)CLOS.UNCLOSED, ANGLES.ANG_30, (int)FILL.UNFILLED);
                            addSingleArrow("Open Dot 1.5mm", (1.5) * (1 / 304.8), (int)ARROW_TYPE_CONVERT.DOT, (int)CLOS.UNCLOSED, ANGLES.ANG_30, (int)FILL.UNFILLED);
                            eL = 227;
                            if (true)
                            {
                                foreach (FamilySymbol famsym_NF in myListFamilySymbol_NotFound)
                                {
                                    ElementType arrowDefault = listArrowheads_notNamedArrowHead.FirstOrDefault() as ElementType;
                                    if (arrowDefault != null)
                                    {
                                        Element newArrow = arrowDefault.Duplicate(famsym_NF.Name);

                                        SingleArrow singlearrow = list_SingleArrow.Where(x => x.Name == famsym_NF.Name).FirstOrDefault();

                                        if (!newArrow.get_Parameter(BuiltInParameter.ARROW_TYPE).IsReadOnly) newArrow.get_Parameter(BuiltInParameter.ARROW_TYPE).Set(singlearrow.ARROW_TYPE);
                                        if (!newArrow.get_Parameter(BuiltInParameter.ARROW_SIZE).IsReadOnly) newArrow.get_Parameter(BuiltInParameter.ARROW_SIZE).Set(singlearrow.ARROW_SIZE);
                                        if (!newArrow.get_Parameter(BuiltInParameter.ARROW_CLOSED).IsReadOnly) newArrow.get_Parameter(BuiltInParameter.ARROW_CLOSED).Set(singlearrow.ARROW_CLOSED);
                                        if (!newArrow.get_Parameter(BuiltInParameter.LEADER_ARROW_WIDTH).IsReadOnly) newArrow.get_Parameter(BuiltInParameter.LEADER_ARROW_WIDTH).Set(singlearrow.LEADER_ARROW_WIDTH);
                                        if (!newArrow.get_Parameter(BuiltInParameter.ARROW_FILLED).IsReadOnly) newArrow.get_Parameter(BuiltInParameter.ARROW_FILLED).Set(singlearrow.ARROW_FILLED);

                                        famsym_NF.get_Parameter(BuiltInParameter.LEADER_ARROWHEAD).Set(newArrow.Id);
                                    }
                                }
                            }
                        }
                        eL = 281;
                        int int_i = 1;
                        XYZ xyz_separation = new XYZ(0, 1.5, 0);

                        Schema schema_ArrowWork_Index = Schema.Lookup(new Guid(myConstantStringSchema_ArrowWork));
                        if (schema_ArrowWork_Index == null) schema_ArrowWork_Index = createSchema_ArrowWork();

                        List<ElementId> myFEC_DataStorage = new FilteredElementCollector(doc).OfClass(typeof(DataStorage)).WhereElementIsNotElementType().Where(x => x.Name == "Delete Leaders").Select(x => x.Id).ToList();

                        DataStorage myDatastorage = null;

                        if (myFEC_DataStorage.Count == 0)
                        {
                            myDatastorage = DataStorage.Create(doc);
                            myDatastorage.Name = "Delete Leaders";

                            Entity newEntity = new Entity(schema_ArrowWork_Index);

                            IList<int> list_new = new List<int>();
                            newEntity.Set("ArrowWork", list_new);

                            myDatastorage.SetEntity(newEntity);
                        }
                        else
                        {
                            myDatastorage = doc.GetElement(myFEC_DataStorage.First()) as DataStorage;
                        }

                        Entity entity = myDatastorage.GetEntity(schema_ArrowWork_Index);
                        IList<int> list = entity.Get<IList<int>>("ArrowWork");

                        List<XYZ> listXYZ_Anchor = new List<XYZ>();
                        List<XYZ> listXYZ_End = new List<XYZ>();

                        foreach (int d in list)
                        {
                            if (doc.GetElement(new ElementId(d)) == null) continue;

                            AnnotationSymbol annoSymbol = doc.GetElement(new ElementId(d)) as AnnotationSymbol;

                            Leader leader = annoSymbol.GetLeaders()[0];

                            listXYZ_Anchor.Add(leader.Anchor);
                            listXYZ_End.Add(leader.End);
                        }

                        if (listXYZ_Anchor.Count > 0)
                        {
                            double a_Double_Anchor = listXYZ_Anchor.GroupBy(q => q.X).OrderByDescending(gp => gp.Count()).First().Key;
                            double a_Double_End = listXYZ_End.GroupBy(q => q.X).OrderByDescending(gp => gp.Count()).First().Key;

                            foreach (int d in list)
                            {
                                if (doc.GetElement(new ElementId(d)) == null) continue;

                                AnnotationSymbol annoSymbol = doc.GetElement(new ElementId(d)) as AnnotationSymbol;

                                Leader leader = annoSymbol.GetLeaders()[0];

                                if (leader.Anchor.X == a_Double_Anchor & leader.End.X == a_Double_End) doc.Delete(new ElementId(d));
                            }
                        }

                        list.Clear();

                        if (true)
                        {
                            XYZ xyz_CentreScreen;
                            View view = doc.ActiveView;

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

                            Rectangle rect = uiview.GetWindowRectangle();
                            IList<XYZ> corners = uiview.GetZoomCorners();
                            xyz_CentreScreen = (corners[0] + corners[1]) / 2;
                            //xyz_CentreScreen = corners[1];// + corners[1]) / 2;

                            int int_337 = 1;

                            foreach (FamilySymbol famSym in myListFamilySymbol_Original)
                            {
                                AnnotationSymbol anno_sym = doc.Create.NewFamilyInstance((xyz_separation * int_i/int_337) + xyz_CentreScreen, famSym, view) as AnnotationSymbol;
                                list.Add(anno_sym.Id.IntegerValue);

                                anno_sym.addLeader();
                                Leader leader = anno_sym.GetLeaders()[0];

                                leader.End = new XYZ(10/int_337, 0, 0) + (xyz_separation * int_i /int_337) + xyz_CentreScreen;
                                int_i++;
                            }

                            entity.Set("ArrowWork", list);
                            myDatastorage.SetEntity(entity);
                        }
                    }
                    tx.Commit();
                }
            }
            #region catch and finally
            catch (Exception ex)
            {
                //_952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("EE22_ArrowWork" + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("EE22_ArrowWork, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }

            #endregion

            return Result.Succeeded;
        }

        public const string myConstantStringSchema_ArrowWork = "d6900cd2-d464-45a1-a4af-6302ec4838be";
        public static Schema createSchema_ArrowWork()
        {
            Guid myGUID = new Guid(myConstantStringSchema_ArrowWork);
            SchemaBuilder mySchemaBuilder = new SchemaBuilder(myGUID);
            mySchemaBuilder.SetSchemaName("ArrowWork");

            mySchemaBuilder.AddArrayField("ArrowWork", typeof(int));

            return mySchemaBuilder.Finish();
        }
    }
}
