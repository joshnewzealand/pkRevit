


//using Autodesk = global2019.Autodesk;
//using _934_PRLoogle_Command06;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
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



    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class _937_PRLoogle_Command02_EE04_PutFittingInCentreOfScreen : IExternalEventHandler  //this is the last when one making a checklist change, EE4 must be just for when an element is new
    {
        //public bool myRadioButtonModeDunedin { get; set; }
        public TextNoteType myTextType { get; set; }
        //public XYZ myXYZ { get; set; }
        public FamilySymbol myFamilySymbol { get; set; }

        public void Execute(UIApplication uiapp)
        {
            int eL = -1;

            try
            {
                PlaceInCentreOfScreen( myTextType , myFamilySymbol, uiapp);
                //////UIDocument uidoc = uiapp.ActiveUIDocument;
                //////Document doc = uidoc.Document; // myListView_ALL_Fam_Master.Items.Add(doc.GetElement(uidoc.Selection.GetElementIds().First()).Name);

                //////using (Transaction tx = new Transaction(doc))
                //////{
                //////    tx.Start("Fitting To Centre Screen");

                //////    myFamilySymbol.Activate();
                //////    FamilyInstance myFamilyInstance = doc.Create.NewFamilyInstance(myXYZ, myFamilySymbol, uidoc.ActiveView);

                //////    TextNoteOptions opts = new TextNoteOptions() { HorizontalAlignment = HorizontalTextAlignment.Center, TypeId = myTextType.Id };
                //////    int myBoolAlwaysVertical = 1;
                //////    myBoolAlwaysVertical = myFamilySymbol.Family.get_Parameter(BuiltInParameter.FAMILY_ALWAYS_VERTICAL).AsInteger();
                //////    TextNote note = TextNote.Create(doc, uidoc.ActiveView.Id, myXYZ, myFamilySymbol.FamilyName + Environment.NewLine + "AV = " + myBoolAlwaysVertical, opts);

                //////    uidoc.Selection.SetElementIds(new List<ElementId> { myFamilyInstance.Id });

                //////    if (uidoc.ActiveView.GetCategoryHidden(myFamilySymbol.Category.Id)) uidoc.ActiveView.SetCategoryHidden(myFamilySymbol.Category.Id, false);
                //////    tx.Commit();
                //////}
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

        public static void PlaceInCentreOfScreen(TextNoteType myTextType, FamilySymbol myFamilySymbol, UIApplication uiapp)
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

                    //FamilyInstance myFamilyInstance = doc.Create.NewFamilyInstance(centre3, myFamilySymbol, uidoc.ActiveView);
                    ///if this is view based toggle that works then the one under !Yes must be updated as well
                    //////if this is view based toggle that works then the one under !Yes must be updated as well
                    //////if this is view based toggle that works then the one under !Yes must be updated as well
                    //////if this is view based toggle that works then the one under !Yes must be updated as well  <=== read
                    //////if this is view based toggle that works then the one under !Yes must be updated as well
                    //////if this is view based toggle that works then the one under !Yes must be updated as well

                 
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
                        //MessageBox.Show("OneLevelBased");
                        myFamilyInstance = doc.Create.NewFamilyInstance(centre3, myFamilySymbol, _937_PRLoogle_Command02_createArcs.myReference_fromLevel(uidoc), Autodesk.Revit.DB.Structure.StructuralType.NonStructural);
                    }
                    else
                    {
                        //MessageBox.Show("Final");
                        myFamilyInstance = doc.Create.NewFamilyInstance(_937_PRLoogle_Command02_createArcs.myReference_fromLevel(uidoc).GetPlaneReference(), centre3/* + new XYZ(0, 0, myDouble_Offset)*/, new XYZ(1, 0, 0/*myDouble_Offset*/), myFamilySymbol);  //link 460
                    }
                    //doc.Regenerate();

                   
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

                //if (uidoc.ActiveView.ViewType == ViewType.CeilingPlan)
                //{
                //    using (Transaction tx = new Transaction(doc))
                //    {
                //        tx.Start("Delete Level");
                //       // doc.Delete(new List<ElementId>() { level.Id });
                //        tx.Commit();
                //    }
                //}
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


        public string GetName()
        {
            return "External Event Example";
        }
    }

    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class _937_PRLoogle_Command02_EE01 : IExternalEventHandler  //this is the last when one making a checklist change, EE4 must be just for when an element is new
    {
        public string myStringCategory { get; set; }
        public Window01_ChooseYourCategory myWindow01_ChooseYourCategory { get; set; }

        public void Execute(UIApplication uiapp)
        {
            int eL = -1;

            try
            {
                UIDocument uidoc = uiapp.ActiveUIDocument;
                Document doc = uidoc.Document;

                string myString_0909_VersionNumber = "2019";// uidoc.Application.Application.VersionNumber;
                string myString_2044 = "PRL-Base Circle_" + myString_0909_VersionNumber + "_" + myStringCategory;

                var pluralizationService = System.Data.Entity.Design.PluralizationServices.PluralizationService.CreateService(System.Globalization.CultureInfo.GetCultureInfo("en-us"));
                Categories categories = doc.Settings.Categories;

               // Array bics = Enum.GetValues(typeof(BuiltInCategory));
                //TaskDialog.Show("ME",  "before or after");
                BuiltInCategory myBuiltInCategory = BuiltInCategory.OST_AdaptivePoints; // <-- ignore this is has to be set to something
                BuiltInCategory myBuiltInCategoryTags = BuiltInCategory.OST_AdaptivePoints; // <-- ignore this is has to be set to something
                foreach (Category bic in doc.Settings.Categories)
                {
                    try
                    {
                        // TaskDialog.Show("ME", bics.Length + " sdfa");
                        if (bic.Name == myStringCategory)
                        {
                            myBuiltInCategory = (BuiltInCategory)bic.Id.IntegerValue;
                            //TaskDialog.Show("ME", "before or after");
                        }

                        string myString_2208 = "";
                        string myString_2204 = myStringCategory.Split(' ').Last();
                        if (pluralizationService.IsPlural(myString_2204))
                        {
                            myString_2208 = _937_PRLoogle_Command02_createArcs.ReplaceLastOccurrence(myStringCategory, myString_2204, pluralizationService.Singularize(myString_2204)) + " Tags";

                            // myString_2208 = myStringCategory.Substring(0, myStringCategory.Length - 1) + " Tags");
                        }
                        else
                        {
                            myString_2208 = myStringCategory + " Tags";
                        }

                        if (bic.Name == myString_2208)
                        {
                            myBuiltInCategoryTags = (BuiltInCategory)bic.Id.IntegerValue;
                            //TaskDialog.Show("ME", "before or after");
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }

                eL = 335;

                bool IsDetailItems = false;
                if (myBuiltInCategory == BuiltInCategory.OST_DetailComponents) IsDetailItems = true;
                if (myBuiltInCategory == BuiltInCategory.OST_GenericAnnotation) IsDetailItems = true;

               IsDetailItems = true;

                ////////////////////if (!IsDetailItems)
                ////////////////////{
                ////////////////////    if (myBuiltInCategory == BuiltInCategory.OST_AdaptivePoints)
                ////////////////////    {
                ////////////////////        TaskDialog.Show("Error", "Category not found");
                ////////////////////        return;
                ////////////////////    }
                ////////////////////    if (myBuiltInCategoryTags == BuiltInCategory.OST_AdaptivePoints)
                ////////////////////    {
                ////////////////////        TaskDialog.Show("Error", "Tag Category not found");
                ////////////////////        return;
                ////////////////////    }
                ////////////////////}

                FilteredElementCollector a = new FilteredElementCollector(doc).OfClass(typeof(Family));
                Family myFamily_1815 = a.FirstOrDefault<Element>(e => e.Name.Equals(myString_2044)) as Family; bool mybool_EFBaseCircle_LoadAnyWay = false; //0003.5
                                                                                                                                                            //FilteredElementCollector b = new FilteredElementCollector(doc).OfClass(typeof(FamilySymbol)).OfCategory(BuiltInCategory.OST_ElectricalFixtureTags); //0004
                //FilteredElementCollector b = new FilteredElementCollector(doc).OfClass(typeof(FamilySymbol)).OfCategory(myBuiltInCategoryTags);
                //List<Element> myListElementEF_Tag = b.Where(e => e.Category.Equals(BuiltInCategory.OST_ElectricalFixtureTags)).ToList();

                //FilteredElementCollector myFilteredElementCollector = ThisApplication.GetConnectorElements(doc, false);
                Dictionary<string, List<FamilySymbol>> myDictionary = pkRevitCircleOutFamilies.EntryPoints.Entry_0110_pkRevitCircleOutFamilies.FindFamilyTypes(doc, myBuiltInCategory, IsDetailItems); //0001
                                                                                                                              //Dictionary<string, List<FamilySymbol>> myDictionary = FindFamilyTypes(doc, BuiltInCategory.OST_ElectricalFixtures); //0001

                using (Transaction y = new Transaction(doc, "Fan out"))
                {
                    y.Start();

                    UIView myUIView_1957 = _937_PRLoogle_Command02_createArcs.myUIView(uidoc);
                    IList<XYZ> myXYZ_Corners_1957 = _937_PRLoogle_Command02_createArcs.myXYZ_Corners(myUIView_1957);
                    XYZ centre3 = myXYZ_Corners_1957[0] + ((myXYZ_Corners_1957[1] - myXYZ_Corners_1957[0]) / 2);

                    FamilyInstance myFamilyInstance_2033 = null;

                    if (!IsDetailItems)
                    {
                        ////////////////////if (myFamily_1815 == null | mybool_EFBaseCircle_LoadAnyWay)
                        ////////////////////{
                        ////////////////////    //  Family myFamily1646 = null;
                        ////////////////////    string[] myStringArray = myWindow01_ChooseYourCategory.myThisApplication.messageConst.Split('|')[1].Split(':');

                        ////////////////////    string myString1754 = myStringArray[0] + ":" + myStringArray[1] + "\\Families " + myString_0909_VersionNumber + "\\" + myString_2044 + ".rfa";  //00006

                        ////////////////////    doc.LoadFamily(myString1754, new _947_PRLoogle_Command01.Class3_20190327.SampleFamilyLoadOptions(), out myFamily_1815);

                        ////////////////////    myFamily_1815 = a.FirstOrDefault<Element>(e => e.Name.Equals(myString_2044)) as Family; //00002  
                        ////////////////////}

                        ////////////////////FamilySymbol myFamilySymbol_1937 = (FamilySymbol)doc.GetElement(myFamily_1815.GetFamilySymbolIds().First());  //and goto 220
                        ////////////////////myFamilySymbol_1937.Activate();

                        //////////////////////myFamilyInstance_2033 = doc.Create.NewFamilyInstance(_937_PRLoogle_Command02_createArcs.myReference_fromLevel(uidoc).GetPlaneReference(), centre3, new XYZ(1, 0, 0), myFamilySymbol_1937);

                        //////////////////////foreach (Parameter para in myFamilyInstance_2033.Parameters)
                        //////////////////////{
                        //////////////////////    if (!para.IsReadOnly & para.IsShared)
                        //////////////////////    {
                        //////////////////////        if (para.StorageType.Equals(StorageType.Integer)) para.Set(0);
                        //////////////////////        if (para.StorageType.Equals(StorageType.Double)) para.Set(100);
                        //////////////////////    }
                        //////////////////////    if (!para.IsReadOnly) if (para.StorageType.Equals(StorageType.String)) para.Set(para.Definition.Name);
                        //////////////////////}

                        //////////////////////foreach (Parameter para in doc.GetElement(myFamilyInstance_2033.GetTypeId()).Parameters)
                        //////////////////////{
                        //////////////////////    if (!para.IsReadOnly)
                        //////////////////////    {
                        //////////////////////        if (para.StorageType.Equals(StorageType.Integer)) para.Set(0);
                        //////////////////////        if (para.StorageType.Equals(StorageType.Double)) para.Set(100);
                        //////////////////////        if (para.StorageType.Equals(StorageType.String)) para.Set(para.Definition.Name);
                        //////////////////////    }
                        //////////////////////}
                    }
                   //////////////////////////////rerereregenerate//////////////////////////////// if (IsDetailItems)  doc.Regenerate();

                    doc.Create.NewDetailCurve(doc.ActiveView, _937_PRLoogle_Command02_createArcs.ArcsSeveral(uidoc).arc);
                    doc.Create.NewDetailCurve(doc.ActiveView, _937_PRLoogle_Command02_createArcs.ArcsSeveral(uidoc).arc2);

                    //factory.NewModelText("Joe", pMT, uidoc.ActiveView.SketchPlane, new XYZ(0.0, 0.0, 0.0), HorizontalAlign.Left, 5.0);

                    foreach (KeyValuePair<string, List<FamilySymbol>> entry in myDictionary)
                    {
                    }

                    Category myCategory = Category.GetCategory(doc, myBuiltInCategory); //0003 we missed one
                                                                                        //Category myCategory = Category.GetCategory(doc, BuiltInCategory.OST_ElectricalFixtures); //0003 we missed one
                    foreach (Category cc in myCategory.SubCategories)
                    {
                        if (cc.Name == "Geometry")
                        {
                            doc.ActiveView.SetCategoryHidden(cc.Id, false);
                        }
                    }

                    //MessageBox.Show("progress " + myDictionary.Count);
                    //alright we need to keep track of what is workplane based
                    //alright keep track of what is workplane based
                   
                    int myInt = 0;
                    int myIntAlwaysVertal = 0;
                    TagMode tagMode = TagMode.TM_ADDBY_CATEGORY;
                    TagOrientation tagorn = TagOrientation.Horizontal;

                    //TaskDialog.Show("Me", "We're on the outside of the second to last for loop");

                    IList<XYZ> myIListXYZ_1829 = _947_PRLoogle_Command01.Class3_20190327.myTesselation(_937_PRLoogle_Command02_createArcs.ArcsSeveral(uidoc).arc, myDictionary.Count);//
                    eL = 396;
                    for (int index = 0; index < myDictionary.Count; index++)  //this one fans out the families, and i don't think  we ever get there.
                    {
                        //var item = myDictionary.ElementAt(index);
                        //var itemKey = item.Key;
                        FamilySymbol itemValue = myDictionary.ElementAt(index).Value[0];

                        // do something with entry.Value or entry.Key

                        //TaskDialog.Show("Me", "We're on the inside " + itemValue.FamilyName);
                        eL = 406;
                        itemValue.Activate();

                        if (!IsDetailItems)
                        {
                            eL = 411;
                           // MessageBox.Show("hello world are we hitting here");
                            doc.Create.NewFamilyInstance(_937_PRLoogle_Command02_createArcs.myReference_fromLevel(uidoc).GetPlaneReference(), myIListXYZ_1829[myInt] + new XYZ(0, 0, 0), new XYZ(1, 0, 0), itemValue);  //link 201
                            if (itemValue.Family.get_Parameter(BuiltInParameter.FAMILY_ALWAYS_VERTICAL).AsInteger() != 0) myIntAlwaysVertal++;
                        }
                        else
                        {
                            eL = 418;
                           
                            //doc.Create.NewFamilyInstance(myIListXYZ_1829[myInt], itemValue, uidoc.ActiveView);
                           FamilyInstance famInt =  doc.Create.NewFamilyInstance(myIListXYZ_1829[myInt], itemValue, _937_PRLoogle_Command02_createArcs.myReference_fromLevel(uidoc),Autodesk.Revit.DB.Structure.StructuralType.NonStructural);
                            //MessageBox.Show("hello world are we hitting here");
                           // if(famInt.)
                             if (famInt.Host == null)
                            {
                                doc.Delete(famInt.Id);
                                //MessageBox.Show("hello world are we hitting here2");

                                eL = 428;
                                FilteredElementCollector myFEC_WallTypes = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Walls).WhereElementIsElementType();
                                eL = 430;
                                double myX = 0;
                                Element myLevel = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Levels).WhereElementIsNotElementType().First();
                                eL = 433;
                                Wall myWall = null;
                                eL = 435;
                                foreach (ElementId myElementID in myFEC_WallTypes.ToElementIds())
                                {
                                    // Creates a geometry line in Revit application
                                    XYZ startPoint = new XYZ(myIListXYZ_1829[myInt].X, myIListXYZ_1829[myInt].Y + 5, 0);
                                    XYZ endPoint = new XYZ(myIListXYZ_1829[myInt].X, myIListXYZ_1829[myInt].Y - 5, 0);
                                    Line geomLine = Line.CreateBound(startPoint, endPoint);

                                    myWall = Wall.Create(doc, geomLine, myElementID, myLevel.Id, 8, 0, false, false);
                                    myWall.get_Parameter(BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS).Set("Example 2 Walls");

                                    myX = myX + 3;
                                    break;
                                }
                                doc.Regenerate();
                                eL = 449;
                                //IList<Reference> sideFaces = HostObjectUtils.GetSideFaces(myWall, ShellLayerType.Interior);

                                doc.Create.NewFamilyInstance(myIListXYZ_1829[myInt], itemValue, myWall, _937_PRLoogle_Command02_createArcs.myReference_fromLevel(uidoc), StructuralType.NonStructural);

                                // access the side face
                                //  Face face = uidoc.Document.GetElement(sideFaces[0]).GetGeometryObjectFromReference(sideFaces[0]) as Face;
                                eL = 455;
                              //  doc.Create.NewFamilyInstance(myWall.ref, myIListXYZ_1829[myInt], XYZ.BasisY, itemValue);
                               //doc.Create.NewFamilyInstance(myIListXYZ_1829[myInt], itemValue, myPlanarFace.Reference, Autodesk.Revit.DB.Structure.StructuralType.NonStructural);
                            }

                           eL = 420;
                        }

                        myInt++;
                    }
                    IsDetailItems = false;

                    eL = 422;
                    if (!IsDetailItems) //this is the one that fans out the tags
                    {
                        FilteredElementCollector b = new FilteredElementCollector(doc).OfClass(typeof(FamilySymbol)).OfCategory(myBuiltInCategoryTags);
                        IList<XYZ> myIListXYZ_1930 = _947_PRLoogle_Command01.Class3_20190327.myTesselation(_937_PRLoogle_Command02_createArcs.ArcsSeveral(uidoc).arc2, b.Count());//

                        if (b.Count() < myIListXYZ_1930.Count())
                        {
                            int myInt2 = 0;
                            foreach (ElementType myIndependentTag_1815 in b)
                            {
                             //////////////////////////////////  IndependentTag newTag = IndependentTag.Create(doc, doc.ActiveView.Id, new Reference(myFamilyInstance_2033), false, tagMode, tagorn, myIListXYZ_1930[myInt2]);// link 206

                             FamilyInstance newTag =  doc.Create.NewFamilyInstance(myIListXYZ_1930[myInt2], (FamilySymbol)myIndependentTag_1815, uidoc.ActiveView);


                                myInt2++;                                ////////////////////////if (null == newTag) throw new Exception("Create IndependentTag Failed.");
                                ////////////////////////newTag.ChangeTypeId(myIndependentTag_1815.Id);
                            }

                            TextNoteOptions opts = new TextNoteOptions() { HorizontalAlignment = HorizontalTextAlignment.Center, TypeId = myWindow01_ChooseYourCategory.myThisApplication.myTextNoteType.Id };
                            //TextNote note = TextNote.Create(doc, uidoc.ActiveView.Id, centre3, "**y no floorbox, Always Vertical = " + myIntAlwaysVertal, opts);
                            TextNote note = TextNote.Create(doc, uidoc.ActiveView.Id, centre3, "Inner circle are FamilyInstances, count=" + myDictionary.Count() + "." + Environment.NewLine + "Outer circle are Independant Tags, count=" + (myIListXYZ_1930.Count() - 1) + ".", opts);

                            y.Commit();
                        }
                        else
                        {
                            TaskDialog.Show("Too many tags", "Sorry you will need to pan back, there are too many annotations (" + b.Count() + ") to fit in the outer circle.");
                            y.RollBack();
                        }
                    } else
                    {
                        y.Commit();//////////////////////////////////
                    }
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
       


    
    class FamilyOption : IFamilyLoadOptions
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


}

