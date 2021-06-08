


//using Autodesk = global2019.Autodesk;
//using _934_PRLoogle_Command06;
#pragma warning disable CS0246 // The type or namespace name 'Autodesk' could not be found (are you missing a using directive or an assembly reference?)
using Autodesk.Revit.DB;
#pragma warning restore CS0246 // The type or namespace name 'Autodesk' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'Autodesk' could not be found (are you missing a using directive or an assembly reference?)
using Autodesk.Revit.DB.Structure;
#pragma warning restore CS0246 // The type or namespace name 'Autodesk' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'Autodesk' could not be found (are you missing a using directive or an assembly reference?)
using Autodesk.Revit.UI;
#pragma warning restore CS0246 // The type or namespace name 'Autodesk' could not be found (are you missing a using directive or an assembly reference?)
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using System.Windows;
//using ListOfPatternsParent_ALL_Families_Master_List = _947_PRLoogle_Command01.XML_Classes.ListOfPatternsParent_ALL_Families_Master_List;


namespace _937_PRLoogle_Command02
{



#pragma warning disable CS0103 // The name 'Autodesk' does not exist in the current context
#pragma warning disable CS0246 // The type or namespace name 'Autodesk' could not be found (are you missing a using directive or an assembly reference?)
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
#pragma warning restore CS0246 // The type or namespace name 'Autodesk' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0103 // The name 'Autodesk' does not exist in the current context
#pragma warning disable CS0246 // The type or namespace name 'IExternalEventHandler' could not be found (are you missing a using directive or an assembly reference?)
    public class _937_PRLoogle_Command02_EE04_PutFittingInCentreOfScreen : IExternalEventHandler  //this is the last when one making a checklist change, EE4 must be just for when an element is new
#pragma warning restore CS0246 // The type or namespace name 'IExternalEventHandler' could not be found (are you missing a using directive or an assembly reference?)
    {
        //public bool myRadioButtonModeDunedin { get; set; }
#pragma warning disable CS0246 // The type or namespace name 'TextNoteType' could not be found (are you missing a using directive or an assembly reference?)
        public TextNoteType myTextType { get; set; }
#pragma warning restore CS0246 // The type or namespace name 'TextNoteType' could not be found (are you missing a using directive or an assembly reference?)
        //public XYZ myXYZ { get; set; }
#pragma warning disable CS0246 // The type or namespace name 'FamilySymbol' could not be found (are you missing a using directive or an assembly reference?)
        public FamilySymbol myFamilySymbol { get; set; }
#pragma warning restore CS0246 // The type or namespace name 'FamilySymbol' could not be found (are you missing a using directive or an assembly reference?)

#pragma warning disable CS0246 // The type or namespace name 'UIApplication' could not be found (are you missing a using directive or an assembly reference?)
        public void Execute(UIApplication uiapp)
#pragma warning restore CS0246 // The type or namespace name 'UIApplication' could not be found (are you missing a using directive or an assembly reference?)
        {
            int eL = -1;

            try
            {
                PlaceInCentreOfScreen( myTextType , myFamilySymbol, uiapp);

            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("WindowLoaded, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

#pragma warning disable CS0246 // The type or namespace name 'FamilySymbol' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'TextNoteType' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'UIApplication' could not be found (are you missing a using directive or an assembly reference?)
        public static void PlaceInCentreOfScreen(TextNoteType myTextType, FamilySymbol myFamilySymbol, UIApplication uiapp)
#pragma warning restore CS0246 // The type or namespace name 'UIApplication' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'TextNoteType' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'FamilySymbol' could not be found (are you missing a using directive or an assembly reference?)
        {
            try
            {

                UIDocument uidoc = uiapp.ActiveUIDocument;
                Document doc = uidoc.Document; // myListView_ALL_Fam_Master.Items.Add(doc.GetElement(uidoc.Selection.GetElementIds().First()).Name);

                if (!(uidoc.ActiveView.ViewType == ViewType.FloorPlan | uidoc.ActiveView.ViewType == ViewType.CeilingPlan))
                {
                    MessageBox.Show("ViewType must be a FloorPlan or CeilingPlan");

                    return;
                }

                UIView myUIView_1957 = myUIView(uidoc);
                IList<XYZ> myXYZ_Corners_1957 = myUIView_1957.GetZoomCorners();
                XYZ centre3 = myXYZ_Corners_1957[0] + ((myXYZ_Corners_1957[1] - myXYZ_Corners_1957[0]) / 2);
                //centre3 = centre3 + new XYZ(0.0, 0.0, 8.0);

                Level level = null;

                using (Transaction tx = new Transaction(doc))
                {
                    tx.Start("Fitting To Centre Screen");

                    PlanViewRange myPlaneViewRange = ((ViewPlan)uidoc.ActiveView).GetViewRange();

                    if (uidoc.ActiveView.ViewType == ViewType.CeilingPlan)
                    {
                        double myDouble_Offset = myPlaneViewRange.GetOffset(PlanViewPlane.CutPlane);
                        MessageBox.Show("The cut plane is " + (myDouble_Offset * 304.8) + " fittings probably will not be visible");
                    }

                    
                    level = (Level)doc.GetElement(myPlaneViewRange.GetLevelId(PlanViewPlane.BottomClipPlane));
                    ////}
                 
                    myFamilySymbol.Activate();
                    
                    FamilyInstance myFamilyInstance = null;

                 
                    if (myFamilySymbol.Family.FamilyPlacementType == FamilyPlacementType.ViewBased)
                    //                           if(myEleeee.Category == Category.GetCategory(doc, BuiltInCategory.OST_DetailComponents))
                    {
                        //MessageBox.Show("OneLevelBased");
                        myFamilyInstance = doc.Create.NewFamilyInstance(centre3, myFamilySymbol, uidoc.ActiveView);
                        //MessageBox.Show("OneLevelBased");
                        try
                        {
                            AnnotationSymbol myAnnotationSymbol = (AnnotationSymbol)myFamilyInstance;
                            myAnnotationSymbol.addLeader();
                            Leader myLeader = myAnnotationSymbol.GetLeaders()[0];
                            myLeader.End = centre3 - new XYZ(-1, 0, 0);

                            ParameterValueProvider provider = new ParameterValueProvider(new ElementId(BuiltInParameter.ALL_MODEL_FAMILY_NAME));
                            FilteredElementCollector collector = new FilteredElementCollector(doc).OfClass(typeof(ElementType)).WherePasses(new ElementParameterFilter(new FilterStringRule(provider, new FilterStringEquals(), "Arrowhead", false)));
                            Element newArrow = collector.ToElements().Cast<ElementType>().ToList().FirstOrDefault(x => x.Name.StartsWith("Arrow 20d HCG"));

                            myFamilySymbol.get_Parameter(BuiltInParameter.LEADER_ARROWHEAD).Set(newArrow.Id);
                        }
                       catch (Exception ex) { }
                   
                    }
                    else if (myFamilySymbol.Family.FamilyPlacementType == FamilyPlacementType.OneLevelBased)
                    {
                        myFamilyInstance = doc.Create.NewFamilyInstance(centre3, myFamilySymbol, _937_PRLoogle_Command02_createArcs.myReference_fromLevel(uidoc), Autodesk.Revit.DB.Structure.StructuralType.NonStructural);
                    }
                    else
                    {
                        myFamilyInstance = doc.Create.NewFamilyInstance(_937_PRLoogle_Command02_createArcs.myReference_fromLevel(uidoc).GetPlaneReference(), centre3/* + new XYZ(0, 0, myDouble_Offset)*/, new XYZ(1, 0, 0/*myDouble_Offset*/), myFamilySymbol);  //link 460
                    }

                    TextNoteOptions opts = new TextNoteOptions() { HorizontalAlignment = HorizontalTextAlignment.Center, TypeId = myTextType.Id };
                    int myBoolAlwaysVertical = 1;
                    myBoolAlwaysVertical = myFamilySymbol.Family.get_Parameter(BuiltInParameter.FAMILY_ALWAYS_VERTICAL).AsInteger();
                    TextNote note = TextNote.Create(doc, uidoc.ActiveView.Id, centre3, myFamilySymbol.FamilyName + Environment.NewLine + "AV = " + myBoolAlwaysVertical, opts);

                    uidoc.Selection.SetElementIds(new List<ElementId> { myFamilyInstance.Id });

                    if (uidoc.ActiveView.GetCategoryHidden(myFamilySymbol.Category.Id)) uidoc.ActiveView.SetCategoryHidden(myFamilySymbol.Category.Id, false);
                    if (uidoc.ActiveView.DetailLevel == ViewDetailLevel.Coarse) uidoc.ActiveView.DetailLevel = ViewDetailLevel.Fine;

                    doc.Regenerate();
                    tx.Commit();
                }


            }

            #region catch and finally
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
            finally
            {
            }
            #endregion

        }

#pragma warning disable CS0246 // The type or namespace name 'UIDocument' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'UIView' could not be found (are you missing a using directive or an assembly reference?)
        public static UIView myUIView(UIDocument uidoc)
#pragma warning restore CS0246 // The type or namespace name 'UIView' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'UIDocument' could not be found (are you missing a using directive or an assembly reference?)
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


        public string GetName()
        {
            return "External Event Example";
        }
    }

    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class _937_PRLoogle_Command02_EE01 : IExternalEventHandler  //this is the last when one making a checklist change, EE4 must be just for when an element is new
    {
        public string myStringCategory { get; set; }
        public bool bool_CircleOutTypeOption { get; set; }
        public bool bool_JustDislayInformation { get; set; }
        public Window01_ChooseYourCategory myWindow01_ChooseYourCategory { get; set; }

        public void Execute(UIApplication uiapp)
        {
            int eL = -1;

            try
            {

                UIDocument uidoc = uiapp.ActiveUIDocument;
                Document doc = uidoc.Document;

                eL = 283;

                Dictionary<string, List<FamilySymbol>> myDictionary = new Dictionary<string, List<FamilySymbol>>();

                if (!bool_CircleOutTypeOption)
                {
                    BuiltInCategory myBuiltInCategory = 0;

                    if (myStringCategory != "ALL")
                    {
                        List<Category> listCategory2 = doc.Settings.Categories.Cast<Category>().Where(x => x.Name == myStringCategory).ToList();

                        eL = 242;

                        if(listCategory2.Count == 0) return;

                        myBuiltInCategory = (BuiltInCategory)listCategory2.FirstOrDefault().Id.IntegerValue;
                    }

                    eL = 321;

                    if (myStringCategory == "ALL")
                    {
                        bool bool_Continue = true;
                        int intAppend = 0;

                        foreach (string ssss in (List<string>)myWindow01_ChooseYourCategory.listBoxFilters.ItemsSource)
                        {
                            if (ssss == "ALL") continue;

                            intAppend++;
                            eL = 331;

                            List<Category> listCategory = doc.Settings.Categories.Cast<Category>().Where(x => x.Name == ssss).ToList();

                            if (ssss == "Title Blocks") continue;

                            if (listCategory.Count == 0) continue;

                            BuiltInCategory aBic = (BuiltInCategory)listCategory.FirstOrDefault().Id.IntegerValue;
                            Dictionary<string, List<FamilySymbol>> temp = pkRevitCircleOutFamilies.EntryPoints.Entry_0110_pkRevitCircleOutFamilies.FindFamilyTypes(doc, aBic, true); //0001)
                            if (temp.Count == 0) continue;

                            if (bool_Continue)
                            {
                                foreach (var keyPair in temp) myDictionary.Add(keyPair.Key + intAppend, keyPair.Value);
                            }
                            bool_Continue = true;
                        }
                    }
                    else
                    {
                        myDictionary = pkRevitCircleOutFamilies.EntryPoints.Entry_0110_pkRevitCircleOutFamilies.FindFamilyTypes(doc, myBuiltInCategory, true); //0001
                    }
                }
                else
                {
                    //we're gonna fan out the doors

                    ICollection<ElementId> ic_EleID = uidoc.Selection.GetElementIds();

                    if (ic_EleID.Count == 0) return;

                    ElementId eleId = ic_EleID.First();

                    Element element = doc.GetElement(eleId);

                    if (typeof(FamilyInstance) != element.GetType())
                    {
                        if(!(element is AnnotationSymbol))
                        {
                            MessageBox.Show("Select element must be of type 'FamilyInstance'");
                            return;
                        }
                    }
                    FamilyInstance famInt = element as FamilyInstance;

                    FamilySymbol famSym = doc.GetElement(famInt.GetTypeId()) as FamilySymbol;

                    List<FamilySymbol> list_FamSymbol = famSym.Family.GetFamilySymbolIds().Select(x => (FamilySymbol)doc.GetElement(x)).ToList();

                    myDictionary.Clear();

                    int intAppend = 0;

                    foreach (FamilySymbol famSymmmm in list_FamSymbol)
                    {
                        intAppend++;
                        myDictionary.Add(intAppend.ToString(), new List<FamilySymbol>() { famSymmmm });
                    }


                   myWindow01_ChooseYourCategory.myTextBox_2022.Text = famSym.Family.Name + "  (" + famSym.Family.GetFamilySymbolIds().Count() + " = type count)";
                }


                if (bool_JustDislayInformation) return;

                if (myDictionary.Count() == 0) return;

                Family famFamily = myDictionary.First().Value.First().Family;

                using (TransactionGroup transGroup = new TransactionGroup(doc))
                {
                    transGroup.Start("Circle out Families");

                    using (Transaction y = new Transaction(doc, "Make Circle"))
                    {
                        y.Start();

                        FailureHandlingOptions options = y.GetFailureHandlingOptions();
                        MyPreProcessor preproccessor = new MyPreProcessor();
                        options.SetFailuresPreprocessor(preproccessor);
                        y.SetFailureHandlingOptions(options);
                        doc.Create.NewDetailCurve(doc.ActiveView, _937_PRLoogle_Command02_createArcs.ArcsSeveral(uidoc).arc);
                        y.Commit();
                    }

                    UIView myUIView_1957 = _937_PRLoogle_Command02_createArcs.myUIView(uidoc);
                    IList<XYZ> myXYZ_Corners_1957 = _937_PRLoogle_Command02_createArcs.myXYZ_Corners(myUIView_1957);
                    XYZ centre3 = myXYZ_Corners_1957[0] + ((myXYZ_Corners_1957[1] - myXYZ_Corners_1957[0]) / 2);

                    FamilyInstance myFamilyInstance_2033 = null;


                    int myInt = 0;
                    int myIntAlwaysVertal = 0;
                    TagMode tagMode = TagMode.TM_ADDBY_CATEGORY;
                    TagOrientation tagorn = TagOrientation.Horizontal;


                    IList<XYZ> myIListXYZ_1829 = _947_PRLoogle_Command01.Class3_20190327.myTesselation(_937_PRLoogle_Command02_createArcs.ArcsSeveral(uidoc).arc, myDictionary.Count);//
                    eL = 396;
                    List<string> listStringNoGoCategories = new List<string>();
                    
                    for (int index = 0; index < myDictionary.Count; index++)  //this one fans out the families, and i don't think  we ever get there.
                    {

                        using (Transaction y = new Transaction(doc, "Fan out"))
                        {
                            y.Start();

                            FailureHandlingOptions options = y.GetFailureHandlingOptions();
                            MyPreProcessor preproccessor = new MyPreProcessor();
                            options.SetFailuresPreprocessor(preproccessor);
                            y.SetFailureHandlingOptions(options);

                            eL = 459;
                            FamilySymbol itemValue = myDictionary.ElementAt(index).Value[0];

                           

                            eL = 407;
                            
                            if (!itemValue.IsActive)
                            {
                                eL = 377;
                                itemValue.Activate();
                                eL = 379;
                                doc.Regenerate();
                                eL = 380;
                            }

                            eL = 381;
                            FamilyInstance famInt = doc.Create.NewFamilyInstance(myIListXYZ_1829[myInt], itemValue, _937_PRLoogle_Command02_createArcs.myReference_fromLevel(uidoc), Autodesk.Revit.DB.Structure.StructuralType.NonStructural);

                            eL = 385;
                            ////if (!uidoc.ActiveView.CanCategoryBeHidden(famInt.Category.Id))
                            ////{
                            ////    MessageBox.Show("are we hitting here");
                            ////}
                            eL = 386;


                            if (uidoc.ActiveView.GetCategoryHidden(famInt.Category.Id))
                            {
                                if (!uidoc.ActiveView.CanCategoryBeHidden(famInt.Category.Id))  //bug fix 0002 - JL_20210608 Circle out will now skip family categories that are hidden in the view template.
                                {
                                    if(!listStringNoGoCategories.Contains(famInt.Category.Name))
                                    {
                                        MessageBox.Show("The category - '" + famInt.Category.Name + "' -  was hidden in the view template." + Environment.NewLine + Environment.NewLine + "Therefore, this family was skipped." + Environment.NewLine + Environment.NewLine + "ALTERNATIVELY: Please goto a plan view without a template applied.");
                                    }

                                    listStringNoGoCategories.Add(famInt.Category.Name);
                                    continue;
                                }

                                uidoc.ActiveView.SetCategoryHidden(famInt.Category.Id, false);
                            }
                            eL = 387;


                            if (famInt.Host == null)
                            {
                                switch (famFamily.FamilyPlacementType.ToString())
                                {
                                    case "ViewBased":
                                        doc.Delete(famInt.Id);
                                        if (true)
                                        {
                                           ////////////////////////////////////////// Element myLevel = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Levels).WhereElementIsNotElementType().First();
                                            eL = 503;

                                            try
                                            {
                                                Level myLevel = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Levels).WhereElementIsNotElementType().Where(x => x.Name == doc.ActiveView.get_Parameter(BuiltInParameter.PLAN_VIEW_LEVEL).AsString()).FirstOrDefault() as Level;
                                                XYZ xyzFamilyPosition = new XYZ(myIListXYZ_1829[myInt].X, myIListXYZ_1829[myInt].Y, myLevel.Elevation);
                                                FamilyInstance famInst_1306 = doc.Create.NewFamilyInstance(xyzFamilyPosition, itemValue, uidoc.ActiveView);
                                            }
                                            catch
                                            {

                                            }
                                            doc.Regenerate();
                                            uidoc.RefreshActiveView();

                                            eL = 508;
                                            //_952_PRLoogleClassLibrary.DatabaseMethods.writeDebug(famInst_1306.Id.IntegerValue.ToString(), true);
                                        }

                                        break;
                                    case "OneLevelBased":
                                        doc.Delete(famInt.Id);
                                        if (true)
                                        {
                                            goto cheat;  //bug fix 0001 20210608 - JL, circle out families: the case for "OneLevelBased" now jumps straight to the default - it produces a better result

                                            Level myLevel = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Levels).WhereElementIsNotElementType().Where(x => x.Name == doc.ActiveView.get_Parameter(BuiltInParameter.PLAN_VIEW_LEVEL).AsString()).FirstOrDefault() as Level;
                                            XYZ xyzFamilyPosition = new XYZ(myIListXYZ_1829[myInt].X, myIListXYZ_1829[myInt].Y, 0/*myLevel.Elevation*/);

                                            Line lineline = Line.CreateUnbound(xyzFamilyPosition, xyzFamilyPosition  + new XYZ(1, 0, 0));

                                            ///////////////////////////////////Element myLevel = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Levels).WhereElementIsNotElementType().First();



                                            eL = 465;

                                            FamilyInstance famInst_1306 = null;//= doc.Create.NewFamilyInstance(lineline, itemValue, (Level)myLevel, StructuralType.NonStructural);
                                            
                                            if(famInst_1306 == null)
                                            {
                                                //MessageBox.Show(itemValue.FamilyName);
                                                famInst_1306 = doc.Create.NewFamilyInstance(xyzFamilyPosition, itemValue, (Level)myLevel, StructuralType.NonStructural);

                                                ////////if (famInst_1306 == null)
                                                ////////{
                                                ////////    MessageBox.Show("Hello world2");
                                                ////////    famInst_1306 = doc.Create.NewFamilyInstance(xyzFamilyPosition, itemValue, (Level)myLevel, StructuralType.NonStructural);
                                                ////////}
                                            }

                                            if (famInst_1306.Host == null)
                                            {
                                                doc.Delete(famInst_1306.Id);
                                                goto cheat;
                                            }
                                                //FamilyInstance famInst_1306 = doc.Create.NewFamilyInstance()

                                                eL = 466;
                                            //doc.Regenerate();
                                            eL = 467;
                                            //_952_PRLoogleClassLibrary.DatabaseMethods.writeDebug(famInst_1306.Id.IntegerValue.ToString(), true);
                                            eL = 467;
                                            //  FamilyInstance famInst_1306 = doc.Create.NewFamilyInstance()
                                            //_952_PRLoogleClassLibrary.DatabaseMethods.writeDebug(famInst_1306.Id.IntegerValue.ToString(), true);
                                        }
                                        break;
                                    case "TwoLevelsBased":
                                        doc.Delete(famInt.Id);

                                        if (true)
                                        {
                                            /////////////////////////////////////Element myLevel = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Levels).WhereElementIsNotElementType().First();
                                            Level myLevel = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Levels).WhereElementIsNotElementType().Where(x => x.Name == doc.ActiveView.get_Parameter(BuiltInParameter.PLAN_VIEW_LEVEL).AsString()).FirstOrDefault() as Level;
                                            XYZ xyzFamilyPosition = new XYZ(myIListXYZ_1829[myInt].X, myIListXYZ_1829[myInt].Y, myLevel.Elevation);
                                            FamilyInstance famInst_1306 = doc.Create.NewFamilyInstance(xyzFamilyPosition, itemValue, (Level)myLevel, StructuralType.NonStructural);
                                            //  FamilyInstance famInst_1306 = doc.Create.NewFamilyInstance()
                                            //_952_PRLoogleClassLibrary.DatabaseMethods.writeDebug(famInst_1306.Id.IntegerValue.ToString(), true);
                                        }

                                        break;
                                    default:
                                    
                                        doc.Delete(famInt.Id);
                                    cheat:
                                        //MessageBox.Show("hello world are we hitting here2");
                                        if (true)
                                        {

                                            eL = 428;
                                            List<Element> myFEC_WallTypes = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Walls).WhereElementIsElementType().Where(x => ((WallType)x).Kind != WallKind.Curtain).ToList();
                                            eL = 430;
                                            double myX = 0;
                                            ////////////////////////////////////////////////////// Element myLevel = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Levels).WhereElementIsNotElementType().First();
                                            Level myLevel = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Levels).WhereElementIsNotElementType().Where(x => x.Name == doc.ActiveView.get_Parameter(BuiltInParameter.PLAN_VIEW_LEVEL).AsString()).FirstOrDefault() as Level;
                                            eL = 433;
                                            Wall myWall = null;
                                            eL = 435;

                                            XYZ startPoint = new XYZ(myIListXYZ_1829[myInt].X, myIListXYZ_1829[myInt].Y + 5, 0);
                                            XYZ endPoint = new XYZ(myIListXYZ_1829[myInt].X, myIListXYZ_1829[myInt].Y - 5, 0);
                                            Line geomLine = Line.CreateBound(startPoint, endPoint);

                                            myWall = Wall.Create(doc, geomLine, myFEC_WallTypes.First().Id, myLevel.Id, 8, 0, false, false);
                                            myWall.get_Parameter(BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS).Set("Example 2 Walls");

                                            myX = myX + 3;

                                            eL = 449;
                                            //IList<Reference> sideFaces = HostObjectUtils.GetSideFaces(myWall, ShellLayerType.Interior);

                                            if(true)
                                            {
                                                XYZ xyzFamilyPosition = new XYZ(myIListXYZ_1829[myInt].X, myIListXYZ_1829[myInt].Y, myLevel.Elevation);

                                                FamilyInstance famInst_1306 = doc.Create.NewFamilyInstance(xyzFamilyPosition, itemValue, myWall, _937_PRLoogle_Command02_createArcs.myReference_fromLevel(uidoc), StructuralType.NonStructural);
                                                // MessageBox.Show(famInst_1306.Id.IntegerValue.ToString());
                                                eL = 458;
                                                ////doc.Create.NewFamilyInstance(myWall.face)
                                                doc.Regenerate();
                                                GeometryElement myGeomeryElement = myWall.get_Geometry(new Options() { ComputeReferences = true });
                                                GeometryObject myGeometryObject = myGeomeryElement.Where(x => (x as Solid) != null).First();
                                                Solid solid = myGeometryObject as Solid;
                                                PlanarFace myPlanarFace = ((Solid)myGeometryObject).Faces.get_Item(0) as PlanarFace;

                                                ////_952_PRLoogleClassLibrary.DatabaseMethods.writeDebug(famInst_1306.Id.IntegerValue.ToString(), true);
                                                if (itemValue.Family.FamilyPlacementType == FamilyPlacementType.WorkPlaneBased) //famInst_1306 == null
                                                {
                                                    doc.Delete(famInst_1306.Id);
                                                    eL = 465;

                                                    XYZ xdir = myPlanarFace.Evaluate(new UV(myPlanarFace.GetBoundingBox().Max.U, 0));
                                                    Line myLine = Line.CreateBound(myPlanarFace.Origin, myPlanarFace.FaceNormal);

                                                    //amInst_1306 = doc.Create.NewFamilyInstance(myPlanarFace, myLine, itemValue);
                                                    famInst_1306 = doc.Create.NewFamilyInstance(myPlanarFace, myPlanarFace.Evaluate(myPlanarFace.GetBoundingBox().Max / 2), myPlanarFace.Origin.CrossProduct(myPlanarFace.FaceNormal), itemValue);

                                                    doc.Regenerate();

                                                    ElementTransformUtils.RotateElement(doc, famInst_1306.Id, Line.CreateUnbound(famInst_1306.GetTotalTransform().Origin, famInst_1306.GetTotalTransform().BasisZ), Math.PI / 2);
                                                }
                                                else
                                                {
                                                    PlanViewRange myPlaneViewRange = ((ViewPlan)uidoc.ActiveView).GetViewRange();

                                                    double myDouble_Offset = myPlaneViewRange.GetOffset(PlanViewPlane.CutPlane);
                                                    ElementTransformUtils.MoveElement(doc, famInst_1306.Id, new XYZ(0, 0, myDouble_Offset / 2));
                                                }
                                                eL = 478;

                                                if (famInst_1306.Host == null)
                                                {
                                                    doc.Delete(myWall.Id);
                                                    famInst_1306.get_Parameter(BuiltInParameter.INSTANCE_FREE_HOST_OFFSET_PARAM).Set(0);
                                                }
                                            }

                                        }
                                        break;
                                }
                                ////}

                                eL = 420;
                            }

                            myInt++;

                            uidoc.RefreshActiveView();

                            y.Commit();
                        }
                    }
                    transGroup.Assimilate();
                }

                //  DatabaseMethods.writeDebug(string.Join(Environment.NewLine, myListString), true);
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("_937_PRLoogle_Command02_EE01, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
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
     
        
#pragma warning disable CS0246 // The type or namespace name 'IFamilyLoadOptions' could not be found (are you missing a using directive or an assembly reference?)
    class FamilyOption : IFamilyLoadOptions
#pragma warning restore CS0246 // The type or namespace name 'IFamilyLoadOptions' could not be found (are you missing a using directive or an assembly reference?)
    {
        public bool OnFamilyFound(bool familyInUse, out bool overwriteParameterValues)
        {
            overwriteParameterValues = true;
            return true;
        }

#pragma warning disable CS0246 // The type or namespace name 'Family' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'FamilySource' could not be found (are you missing a using directive or an assembly reference?)
        public bool OnSharedFamilyFound(Family sharedFamily, bool familyInUse, out FamilySource source, out bool overwriteParameterValues)
#pragma warning restore CS0246 // The type or namespace name 'FamilySource' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'Family' could not be found (are you missing a using directive or an assembly reference?)
        {
            source = FamilySource.Family;
            overwriteParameterValues = true;
            return true;
        }
    }



#pragma warning disable CS0246 // The type or namespace name 'IFailuresPreprocessor' could not be found (are you missing a using directive or an assembly reference?)
        public class MyPreProcessor : IFailuresPreprocessor
#pragma warning restore CS0246 // The type or namespace name 'IFailuresPreprocessor' could not be found (are you missing a using directive or an assembly reference?)
    {
#pragma warning disable CS0246 // The type or namespace name 'FailureProcessingResult' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'FailuresAccessor' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'IFailuresPreprocessor' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0538 // 'IFailuresPreprocessor' in explicit interface declaration is not an interface
        FailureProcessingResult IFailuresPreprocessor.PreprocessFailures(FailuresAccessor failuresAccessor)
#pragma warning restore CS0538 // 'IFailuresPreprocessor' in explicit interface declaration is not an interface
#pragma warning restore CS0246 // The type or namespace name 'IFailuresPreprocessor' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'FailuresAccessor' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'FailureProcessingResult' could not be found (are you missing a using directive or an assembly reference?)
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
                //if (fseverity == FailureSeverity.DocumentCorruption) failuresAccessor.DeleteAllWarnings();
                if (fseverity == FailureSeverity.Error) failuresAccessor.DeleteAllWarnings();
                if (fseverity == FailureSeverity.Warning) failuresAccessor.DeleteAllWarnings();

               // MessageBox.Show("hello world");

                //failuresAccessor.ResolveFailure(fma);
                // DeleteWarning mimics clicking 'Ok' button.
                //failuresAccessor.DeleteWarning( fma );         
            }

            //return FailureProcessingResult
            //  .ProceedWithCommit;
            return FailureProcessingResult.Continue;
        }
    }


    static class aclass
    {
#pragma warning disable CS0246 // The type or namespace name 'Element' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'View' could not be found (are you missing a using directive or an assembly reference?)
        public static bool IsElementVisibleInView(this View view, Element el)
#pragma warning restore CS0246 // The type or namespace name 'View' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'Element' could not be found (are you missing a using directive or an assembly reference?)
        {
            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }

            if (el == null)
            {
                throw new ArgumentNullException(nameof(el));
            }

            // Obtain the element's document.

            Document doc = el.Document;

            ElementId elId = el.Id;

            // Create a FilterRule that searches 
            // for an element matching the given Id.

            FilterRule idRule = ParameterFilterRuleFactory
              .CreateEqualsRule(
                new ElementId(BuiltInParameter.ID_PARAM),
                elId);

            var idFilter = new ElementParameterFilter(idRule);

            // Use an ElementCategoryFilter to speed up the 
            // search, as ElementParameterFilter is a slow filter.

            Category cat = el.Category;
            var catFilter = new ElementCategoryFilter(cat.Id);

            // Use the constructor of FilteredElementCollector 
            // that accepts a view id as a parameter to only 
            // search that view.
            // Also use the WhereElementIsNotElementType filter 
            // to eliminate element types.

            FilteredElementCollector collector =
                new FilteredElementCollector(doc, view.Id)
                  .WhereElementIsNotElementType()
                  .WherePasses(catFilter)
                  .WherePasses(idFilter);

            // If the collector contains any items, then 
            // we know that the element is visible in the
            // given view.

            return collector.Any();
        }
    }




}

