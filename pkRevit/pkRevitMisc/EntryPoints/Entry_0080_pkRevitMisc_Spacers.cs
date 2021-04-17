using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

using View = Autodesk.Revit.DB.View;

using System.Windows.Forms;
using System;
using System.Collections.Generic;


using Autodesk.Revit.UI.Selection;
using System.Linq;
using System.Diagnostics;

using System.IO;
using System.Reflection;


namespace pkRevitMisc.EntryPoints  //Entry_0010_pkRevitDatasheets
{
    public partial class Entry_0080_pkRevitMisc
    {
        string fileName;
        const double MeterToFeet = 3.2808399;
        const string FamilyTypeName = "PRL-DI Spacer Vertical";
        const string FamilyTypeNameHorizontal = "PRL-DI Spacer Horizontal";
        string PRL_Parameters = System.Environment.GetEnvironmentVariable("UserProfile") + @"\Google Drive\` d OldDropBox\For Joshua Lumley-2\20170123 second week back\99 shared links\PRL_Parameters-2.txt";


        public Result StartMethod_0080(ExternalCommandData cd, ref string message, ElementSet elements)
        {
            ExternalCommandData commandData = cd;
            string executionLocation = message;

            ToAvoidLoadingRevitDLLs toavoidloadingrevitdlls = new ToAvoidLoadingRevitDLLs();
            toavoidloadingrevitdlls.commandData = commandData;
            toavoidloadingrevitdlls.executionLocation = executionLocation;

            //MessageBox.Show("0080_Spacers-2");


            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            View myAGV2 = uidoc.ActiveView as View;

            ElementCategoryFilter filter2 = new ElementCategoryFilter(BuiltInCategory.OST_Viewports);

            FilteredElementCollector collector2 = new FilteredElementCollector(doc);

            ICollection<Element> AllViewports2 = collector2.WherePasses(filter2).ToElements();

            Category myCategory = Category.GetCategory(doc, BuiltInCategory.OST_Viewports);


            if (myAGV2.Category.Id == myCategory.Id)
            {

                TaskDialog.Show("No viewport", "Macro doesn't work in active viewport." + Environment.NewLine + "Please open view from project browser");

                return Result.Succeeded;
            }

            TaskDialog mainDialog = new TaskDialog("Hello, viewport check!");
            mainDialog.MainInstruction = "Hello, viewport check!";
            mainDialog.MainContent =
                    "Revit API doesn't automatically know if the user is in an active viewport. "
                    + "Please click 'Yes' if your are, or 'No' if your not.";


            mainDialog.AddCommandLink(TaskDialogCommandLinkId.CommandLink1, "Yes, I am just in an ordinary view.");
            mainDialog.AddCommandLink(TaskDialogCommandLinkId.CommandLink2, "No, I am in an active viewport on a sheet.");

            mainDialog.CommonButtons = TaskDialogCommonButtons.Close;
            mainDialog.DefaultButton = TaskDialogResult.Close;


            TaskDialogResult tResult = mainDialog.Show();

            bool YesOrNo = true;

            if (TaskDialogResult.CommandLink2 == tResult)
            {
                YesOrNo = true;

                TaskDialog.Show("Programming stopping", "Please goto an ordinary view.");
                return Result.Succeeded;
            }

            if (TaskDialogResult.CommandLink1 != tResult)
            {
                return Result.Succeeded;
            }
            YesOrNo = false;

            ////////else if (TaskDialogResult.CommandLink2 == tResult)
            ////////{
            ////////    YesOrNo = false;
            ////////}
            ////////else
            ////////{
            ////////    return Result.Succeeded;
            ////////}

            string myString_TempPath = "";

            if (message.Split('|')[0] == "Release")  //constructs a path for release directory (in program files)
            {
                myString_TempPath = message.Split('|')[1] + "//Families";
            }
            if (message.Split('|')[0] == "Dev") //constructs a path for development directory
            {
                myString_TempPath = message.Split('|')[1] + "//Families";
            }


            Form_2D_Spacers form_2D_Spacers = new Form_2D_Spacers(myString_TempPath, uidoc, YesOrNo);

            form_2D_Spacers.ShowDialog();


            return Result.Succeeded;
        }


        public void _99_AutomaticLayout(int numericUpDown1, int numericUpDown2, UIDocument uid, string myAddinFolder, bool YesOrNo)
        {

            ////if (useserver) PRL_Parameters = PRL_ParametersServer;
            ////if (useserverdunedin) PRL_Parameters = PRL_ParametersServerDunedin;

            #region these are mostly notes to self
            //we need to be able to create a detail family
            //perferably without having to create the rfa file
            //place it in the model
            //during creation we make reference lines
            //and place dimensions on those reference lines
            //then this gets  placed on screen
            //_____________________________________________
            //it all starts with creating a detail family with coe
            //create one line and one dimension and that is all there
            //is too it			

            //thank you joshua this explained it well
            //the best place i can remember one is not in the rename which i originally thought but
            //the best place i can remember it is in the wfp make a new family type popup


            //do something better here

            //          FamilyInstance myFamilyInstance = myElement as FamilyInstance;
            //Family myFamily = myFamilyInstance.Symbol.Family;

            //we just need to create a new element

            //are families created just through the new menu
            #endregion

            int myNumber = numericUpDown1;
            int myNumberHorizontal = numericUpDown2;

            //Document famDoc = uid.Document.Application.NewFamilyDocument(System.Environment.GetEnvironmentVariable("ProgramData") + "\\Autodesk\\RVT 2017\\Family Templates\\English\\Metric Detail Item.rft");
            Document famDoc = uid.Document.Application.NewFamilyDocument(myAddinFolder + "\\Metric Detail Item.rft");//uid.Document.Application.NewFamilyDocument(System.Environment.GetEnvironmentVariable("ProgramData") + "\\Autodesk\\RVT 2017\\Family Templates\\English\\Metric Detail Item.rft");
            Document famDocHorizontal = uid.Document.Application.NewFamilyDocument(myAddinFolder + "\\Metric Detail Item.rft");

            fileName = string.Empty;
            fileName = myAddinFolder + "\\PRL_Parameters.txt";


            //C:\Users\Joshua\Dropbox\For Joshua Lumley-2\20170123 second week back\99 shared links

            if (File.Exists(fileName) == false)
            {
                TaskDialog.Show("Error", "Cannot find the shared parameters file.");
                return;
            }

            //string myGroupName = "PRL_SubcctSchedule";
            //string ScheduleType = "Subcct Details"; 
            //themainsubroutine(settheStringArray(ScheduleType)[0],myGroupName,famDoc);


            FamilyManager familyManager = famDoc.FamilyManager;
            FamilyManager familyManagerHorizontal = famDocHorizontal.FamilyManager;
            UIDocument uidoc = uid;
            Document doc = uidoc.Document;

            string myStringSharedParameterFileName = "";

            if (uidoc.Application.Application.SharedParametersFilename != null)
            {
                myStringSharedParameterFileName = uidoc.Application.Application.SharedParametersFilename; //Q:\Revit Revit Revit\Template 2018\PRL_Parameters.txt
            }


            uidoc.Application.Application.SharedParametersFilename = fileName;
            DefinitionFile defFile = uidoc.Application.Application.OpenSharedParameterFile();
            DefinitionGroups myGroups = defFile.Groups;
            DefinitionGroup myGroup = myGroups.get_Item("_98_PRL_Generic Dimensions");

            if (myStringSharedParameterFileName != "")
            {
                uidoc.Application.Application.SharedParametersFilename = myStringSharedParameterFileName;
            }

            Definitions myDefinitions = myGroup.Definitions;
            ExternalDefinition eDef = myDefinitions.get_Item("PRL_WIDTH") as ExternalDefinition;

            //ActiveUIDocument.ActiveView.ui
            #region view checks		
            View view = uidoc.ActiveView;

            View3D myView3D = doc.ActiveView as View3D;

            if (myView3D != null)
            {

                TaskDialog.Show("Not the correct type of view", "'Detail' lines can't be drawn in 3D view.");
                return;
            }

            ViewPlan myViewPlan = doc.ActiveView as ViewPlan;

            if (myViewPlan == null)
            {
                TaskDialog.Show("Not the correct type of view", "The active view must be a view 'plan'");
                return;
            }


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
            #endregion view checks
            Rectangle rect = uiview.GetWindowRectangle();
            IList<XYZ> corners = uiview.GetZoomCorners();
            XYZ pXchanges = corners[0];
            XYZ qXchanges = corners[1];

            double twoxpositionsXStatic = ((corners[1].X - corners[0].X) / 20);
            double twoxpositionsYStatic = ((corners[1].Y - corners[0].Y) / 20);
            double twoxpositionsX = (corners[0].X + (corners[1].X - corners[0].X) / 20);
            double twoxpositionsY = (corners[0].Y + (corners[1].Y - corners[0].Y) / 20);

            double YLineLengthStart = pXchanges.Y + twoxpositionsYStatic;
            double YLineLengthFinish = qXchanges.Y - twoxpositionsYStatic;
            double YLineLength = YLineLengthFinish - YLineLengthStart;
            if (YLineLength < 0) YLineLength = YLineLengthStart - YLineLengthFinish;

            double XLineLengthStart = pXchanges.X + twoxpositionsXStatic;
            double XLineLengthFinish = qXchanges.X - twoxpositionsXStatic;
            double XLineLength = XLineLengthFinish - XLineLengthStart;
            if (XLineLength < 0) XLineLength = XLineLengthStart - XLineLengthFinish;


            FamilyParameter paramTd;
            FamilyParameter paramTdHorizontal;

            #region hide this away for a bit

            using (Transaction y = new Transaction(famDoc, "Put in parameter"))
            {
                y.Start();
                paramTd = familyManager.AddParameter(eDef, BuiltInParameterGroup.PG_IDENTITY_DATA, true);

                if (familyManager.Types.Size == 0)
                    familyManager.NewType(FamilyTypeName);

                //FamilyType myFamilyType = familyManager.CurrentType;

                //myFamilyType.

                // famDoc.Regenerate();

                //familyManagerHorizontal.Set( paramTd, (1 * MeterToFeet));
                //familyManager.Set( paramTd, (YLineLength));

                y.Commit();
            }


            using (Transaction y = new Transaction(famDocHorizontal, "Put in parameter"))
            {
                y.Start();
                paramTdHorizontal = familyManagerHorizontal.AddParameter(eDef, BuiltInParameterGroup.PG_IDENTITY_DATA, true);

                if (familyManagerHorizontal.Types.Size == 0)
                    familyManagerHorizontal.NewType(FamilyTypeNameHorizontal);

                // familyManagerHorizontal.Set( paramTdHorizontal, (1 * MeterToFeet));

                y.Commit();
            }

            #endregion

            FilteredElementCollector viewCollector = new FilteredElementCollector(famDoc);
            viewCollector.OfClass(typeof(Autodesk.Revit.DB.View));

            List<ReferencePlane> myListReferencePlane = new List<ReferencePlane>();
            List<ReferencePlane> myListReferencePlaneHorizontal = new List<ReferencePlane>();

            View myView = viewCollector.FirstOrDefault() as View;

            Options goption = new Options();
            goption.ComputeReferences = true;
            goption.IncludeNonVisibleObjects = true;
            goption.View = myView;
            #region hide all this for a bit

            using (Transaction y = new Transaction(famDoc, "The first one"))
            {
                y.Start();

                double Xstat = twoxpositionsXStatic;

                myListReferencePlane.Add(famDoc.FamilyCreate.NewReferencePlane(new XYZ(0.0, 0.0, 0.0), new XYZ(10.0, 0.0, 0.0), new XYZ(0.0, 0.0, 1), myView));
                myListReferencePlane.Add(famDoc.FamilyCreate.NewReferencePlane(new XYZ(0.0, 10.0, 0.0), new XYZ(10.0, 10.0, 0.0), new XYZ(0.0, 0.0, 1), myView));
                famDoc.FamilyCreate.NewDetailCurve(myView, Line.CreateBound(new XYZ((Xstat / 2), 0.0, 0.0), new XYZ((Xstat / 2), 10.0, 0.0)));


                int myNumberRationlised = ((myNumber * 2) - 2);
                double myDouble = 10.0 / (myNumber * 2);
                double myDoubleAccumlation = 0; bool toggle = true;
                for (int i = 0; i <= myNumberRationlised; i++)
                {
                    myDoubleAccumlation = myDoubleAccumlation + myDouble;
                    myListReferencePlane.Add(famDoc.FamilyCreate.NewReferencePlane(new XYZ(0.0, myDoubleAccumlation, 0.0), new XYZ(Xstat, myDoubleAccumlation, 0.0), new XYZ(0.0, 0.0, 1), myView));

                    if (toggle)
                    {
                        famDoc.FamilyCreate.NewDetailCurve(myView, Line.CreateBound(new XYZ(0.0, myDoubleAccumlation, 0.0), new XYZ(Xstat, myDoubleAccumlation, 0.0)));
                        toggle = false;
                    }
                    else { toggle = true; }
                }

                Line dimlineboundx1 = Line.CreateUnbound(new XYZ(0, 0, 0), new XYZ(0, Xstat, 0));

                famDoc.Regenerate();

                #region hide this for a bit				        
                //equals

                #endregion

                ReferenceArray references = new ReferenceArray();
                foreach (ReferencePlane rrr in myListReferencePlane)
                {
                    references.Append(rrr.GetReference());
                }

                ReferenceArray references2 = new ReferenceArray();
                references2.Append(myListReferencePlane[0].GetReference());
                references2.Append(myListReferencePlane[1].GetReference());

                Dimension dimension = famDoc.FamilyCreate.NewDimension(myView, dimlineboundx1, references);
                dimension.AreSegmentsEqual = true;

                Dimension dimension2 = famDoc.FamilyCreate.NewDimension(myView, dimlineboundx1, references2);
                dimension2.FamilyLabel = paramTd;
                familyManager.Set(paramTd, (YLineLength));

                y.Commit();
            }

            #endregion

            using (Transaction y = new Transaction(famDocHorizontal, "The second one (horizontal)"))
            {
                double Ystat = twoxpositionsYStatic;
                y.Start();

                myListReferencePlaneHorizontal.Add(famDocHorizontal.FamilyCreate.NewReferencePlane(new XYZ(0.0, 0.0, 0.0), new XYZ(00.0, 10.0, 0.0), new XYZ(0.0, 0.0, 1), myView));
                myListReferencePlaneHorizontal.Add(famDocHorizontal.FamilyCreate.NewReferencePlane(new XYZ(10.0, 0.0, 0.0), new XYZ(10.0, 10.0, 0.0), new XYZ(0.0, 0.0, 1), myView));
                famDocHorizontal.FamilyCreate.NewDetailCurve(myView, Line.CreateBound(new XYZ(0, (Ystat / 2), 0.0), new XYZ(10, (Ystat / 2), 0.0)));

                int myNumberRationlisedHorizontal = ((myNumberHorizontal * 2) - 2); double myDoubleHorizontal = 10.0 / (myNumberHorizontal * 2);
                double myDoubleAccumlationHorizontal = 0; bool toggleHorizontal = true;
                for (int i = 0; i <= myNumberRationlisedHorizontal; i++)
                {
                    myDoubleAccumlationHorizontal = myDoubleAccumlationHorizontal + myDoubleHorizontal;
                    myListReferencePlaneHorizontal.Add(famDocHorizontal.FamilyCreate.NewReferencePlane(new XYZ(myDoubleAccumlationHorizontal, 0.0, 0.0), new XYZ(myDoubleAccumlationHorizontal, Ystat, 0.0), new XYZ(0.0, 0.0, 1), myView));

                    if (toggleHorizontal)
                    {
                        famDocHorizontal.FamilyCreate.NewDetailCurve(myView, Line.CreateBound(new XYZ(myDoubleAccumlationHorizontal, 0.0, 0.0), new XYZ(myDoubleAccumlationHorizontal, Ystat, 0.0)));
                        toggleHorizontal = false;
                    }
                    else { toggleHorizontal = true; }
                }

                Line dimlineboundx1Horizontal = Line.CreateUnbound(new XYZ(0, 0, 0), new XYZ(Ystat, 0, 0));
                //didn't get paid
                //get some advertisers
                //

                famDocHorizontal.Regenerate();

                ReferenceArray referencesHorizontal = new ReferenceArray();
                foreach (ReferencePlane rrr in myListReferencePlaneHorizontal)
                {
                    referencesHorizontal.Append(rrr.GetReference());
                }

                ReferenceArray references2Horizontal = new ReferenceArray();
                references2Horizontal.Append(myListReferencePlaneHorizontal[0].GetReference());
                references2Horizontal.Append(myListReferencePlaneHorizontal[1].GetReference());

                Dimension dimensionHorizontal = famDocHorizontal.FamilyCreate.NewDimension(myView, dimlineboundx1Horizontal, referencesHorizontal);
                dimensionHorizontal.AreSegmentsEqual = true;

                Dimension dimensionHorizontal2 = famDocHorizontal.FamilyCreate.NewDimension(myView, dimlineboundx1Horizontal, references2Horizontal);
                dimensionHorizontal2.FamilyLabel = paramTdHorizontal;
                familyManagerHorizontal.Set(paramTdHorizontal, (XLineLength));

                y.Commit();
            }

            SaveAsOptions options = new SaveAsOptions();
            options.OverwriteExistingFile = true;


            string path = (System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\` aa Spacer Families");
            if (!System.IO.Directory.Exists(path)) System.IO.Directory.CreateDirectory(path);

            string subdirectory_reversedatetothesecond = (path + ("\\" + (DateTime.Now.ToString("yyyyMMddHHmmss"))));
            if (!System.IO.Directory.Exists(subdirectory_reversedatetothesecond)) System.IO.Directory.CreateDirectory(subdirectory_reversedatetothesecond);

            string FILE_NAME = (subdirectory_reversedatetothesecond + "\\");

            //    System.IO.File.Create(FILE_NAME).Dispose();


            string mystringFilepath = FILE_NAME + FamilyTypeName + " x" + numericUpDown1.ToString() + ".rfa";
            string mystringFilepathHorizontal = FILE_NAME + FamilyTypeNameHorizontal + " x" + numericUpDown2.ToString() + ".rfa";

            famDoc.SaveAs(mystringFilepath, options);
            famDocHorizontal.SaveAs(mystringFilepathHorizontal, options);

            famDoc.Close();
            famDocHorizontal.Close();

            SampleFamilyLoadOptions mySampleFamilyLoadOptions = new SampleFamilyLoadOptions();

            Family family = null;
            Family familyHorizontal = null;


            using (Transaction y = new Transaction(doc, "Make a detail family and stick a line in"))
            {
                y.Start();
                doc.LoadFamily(mystringFilepath, mySampleFamilyLoadOptions, out family);
                doc.LoadFamily(mystringFilepathHorizontal, mySampleFamilyLoadOptions, out familyHorizontal);

                if (familyHorizontal == null)
                {
                    TaskDialog.Show("thisthing", "it is indeed a null");
                    return;
                }
                else
                {
                }
                y.Commit();
            }

            ISet<ElementId> myListElementIDforFamilySymbols = family.GetFamilySymbolIds();
            ISet<ElementId> myListElementIDforFamilySymbolsHorizontal = familyHorizontal.GetFamilySymbolIds();

            FamilySymbol myFamilySymbol = doc.GetElement(myListElementIDforFamilySymbols.FirstOrDefault()) as FamilySymbol;
            FamilySymbol myFamilySymbolHorizontal = doc.GetElement(myListElementIDforFamilySymbolsHorizontal.FirstOrDefault()) as FamilySymbol;

            XYZ normal = XYZ.BasisZ;
            XYZ origin = XYZ.Zero;

            // view.Origin.Y

            //View view = doc.ActiveView;	  
            //BoundingBoxXYZ newBox = new BoundingBoxXYZ();
            //newBox.set_MinEnabled( 0, true );
            //newBox.set_MinEnabled( 1, true );
            //newBox.set_MinEnabled( 2, true );
            //newBox.Min = new XYZ( viewOne.CropBox.Min.X + viewOne.Origin.X, viewOne.CropBox.Min.Y  + viewOne.Origin.Y, 0 );
            //newBox.set_MaxEnabled( 0, true );
            //newBox.set_MaxEnabled( 1, true );
            //newBox.set_MaxEnabled( 2, true );
            //newBox.Max = new XYZ( viewOne.CropBox.Max.X + viewOne.Origin.X, viewOne.CropBox.Max.Y + viewOne.Origin.Y, 0 );
            //viewTwo.CropBox = newBox ;	


            using (Transaction y = new Transaction(doc, "data data"))
            {
                y.Start();
                // view.Origin.Y

                //View view = doc.ActiveView;	  
                //BoundingBoxXYZ newBox = new BoundingBoxXYZ();
                //newBox.set_MinEnabled( 0, true );
                //newBox.set_MinEnabled( 1, true );
                //newBox.set_MinEnabled( 2, true );
                //newBox.Min = new XYZ( viewOne.CropBox.Min.X + viewOne.Origin.X, viewOne.CropBox.Min.Y  + viewOne.Origin.Y, 0 );
                //newBox.set_MaxEnabled( 0, true );
                //newBox.set_MaxEnabled( 1, true );
                //newBox.set_MaxEnabled( 2, true );
                //newBox.Max = new XYZ( viewOne.CropBox.Max.X + viewOne.Origin.X, viewOne.CropBox.Max.Y + viewOne.Origin.Y, 0 );
                //viewTwo.CropBox = newBox ;	

                //Viewport viewportOne = newMyClass.myViewport; //= elementOne as Viewport;
                //	view.
                /*			
                  // doc = this.ActiveUIDocument.Document;
                   ViewSheet vs = doc.ActiveView as ViewSheet;
                   Viewport legendVP = new FilteredElementCollector(doc).OfClass(typeof(Viewport)).Cast<Viewport>()
                       .Where(q => q.SheetId == vs.Id && ((View)doc.GetElement(q.ViewId)).ViewType == ViewType.Legend).First();

                   View legend = doc.GetElement(legendVP.ViewId) as View;

                   Outline legendOutline = legendVP.GetBoxOutline();

                   XYZ intialCenter = legendOutline.MinimumPoint.Subtract(legendOutline.MaximumPoint.Subtract(legendOutline.MinimumPoint).Divide(2));

                   */


                View myAGV = uidoc.ActiveGraphicalView as View;
                //myAGV.Origin
                //TaskDialog.Show("active graphical view", myAGV.Name);   


                ElementCategoryFilter filter = new ElementCategoryFilter(BuiltInCategory.OST_Viewports);

                FilteredElementCollector collector = new FilteredElementCollector(doc);

                ICollection<Element> AllViewports = collector.WherePasses(filter).ToElements();

                //	ElementWorksetFilter elementWorksetFilter = new ElementWorksetFilter(workset.Id, false);
                //ICollection<Element> worksetElemsfounds = myFilteredElementCollector.WherePasses(elementWorksetFilter).ToElements();
                Viewport CorrespondingViewport = null;

                //TaskDialog.Show("progress", "line 607");
                //bool progress = true;

                string writethis = null;

                foreach (Viewport vv in AllViewports)
                {
                    //  if( vv.Category.Name == "Viewports" )
                    // {
                    //String ViewName = vv.GetParameters("View Name").FirstOrDefault().AsString();
                    writethis = writethis + Environment.NewLine + vv.Name;

                    //if(progress)	TaskDialog.Show("progress", "line 617");
                    //progress = false;

                    //Viewport myViewport = vv as Viewport;
                    //View myAGV = uidoc.ActiveGraphicalView as View;
                    View myView2 = doc.GetElement(vv.ViewId) as View;


                    if (myView2.Name == myAGV.Name)
                    {
                        //TaskDialog.Show("progress", "line 624");
                        CorrespondingViewport = vv;
                    }
                }

                if (YesOrNo == false)
                {
                    myFamilySymbol.Activate();
                    myFamilySymbolHorizontal.Activate();

                    doc.Create.NewFamilyInstance(new XYZ(twoxpositionsX, twoxpositionsY, 0.0), myFamilySymbol, view);
                    doc.Create.NewFamilyInstance(new XYZ(twoxpositionsX, twoxpositionsY, 0.0), myFamilySymbolHorizontal, view);

                    //TaskDialog.Show("correct one", "this one is not");					

                }
                else
                {
                    //TaskDialog.Show("correct one", "this is the one we should be running");

                    //	TaskDialog.Show("", "Macro doesn't work in active viewport."+ Environment.NewLine +"please open view.");

                    //return;
                    XYZ myGetBoxCentre = CorrespondingViewport.GetBoxCenter();

                    myFamilySymbol.Activate();
                    myFamilySymbolHorizontal.Activate();


                    double XX;
                    double YY;
                    //CorrespondingViewport.GetBoxOutline().

                    XX = 0;
                    YY = 0;

                    // XX = twoxpositionsXStatic;
                    //  YY = twoxpositionsYStatic;


                    //XX = CorrespondingViewport.GetBoxOutline().MinimumPoint.X	;
                    //YY = CorrespondingViewport.GetBoxOutline().MinimumPoint.Y;
                    XX = XX - (XX * 2.0);
                    YY = YY - (YY * 2.0);
                    /*  TaskDialog.Show("add or subtract", 				               
                                                      + XX + "," + YY
                                                 );*/


                    //doc.Create.NewFamilyInstance(new XYZ(twoxpositionsX + XX, twoxpositionsY + YY, 0.0), myFamilySymbol, view);
                    //doc.Create.NewFamilyInstance(new XYZ(twoxpositionsX + XX, twoxpositionsY + YY, 0.0), myFamilySymbolHorizontal, view);

                    //doc.Create.NewFamilyInstance(new XYZ(XX, YY, 0.0), myFamilySymbol, view);
                    //doc.Create.NewFamilyInstance(new XYZ(XX, YY, 0.0), myFamilySymbolHorizontal, view);

                    //doc.Create.NewFamilyInstance(new XYZ(twoxpositionsXStatic + XX*Convert.ToDouble(myAGV.Scale), twoxpositionsYStatic + YY*Convert.ToDouble(myAGV.Scale), 0.0), myFamilySymbol, view);
                    //doc.Create.NewFamilyInstance(new XYZ(twoxpositionsXStatic + XX*Convert.ToDouble(myAGV.Scale), twoxpositionsYStatic + YY*Convert.ToDouble(myAGV.Scale), 0.0), myFamilySymbolHorizontal, view);

                    doc.Create.NewFamilyInstance(new XYZ(twoxpositionsX + XX * Convert.ToDouble(myAGV.Scale), twoxpositionsY + YY * Convert.ToDouble(myAGV.Scale), 0.0), myFamilySymbol, view);
                    doc.Create.NewFamilyInstance(new XYZ(twoxpositionsX + XX * Convert.ToDouble(myAGV.Scale), twoxpositionsY + YY * Convert.ToDouble(myAGV.Scale), 0.0), myFamilySymbolHorizontal, view);

                    //doc.Create.NewFamilyInstance(new XYZ(twoxpositionsX, twoxpositionsY, 0.0), myFamilySymbol, view);
                    //doc.Create.NewFamilyInstance(new XYZ(twoxpositionsX, twoxpositionsY, 0.0), myFamilySymbolHorizontal, view);

                    //doc.Create.NewFamilyInstance(new XYZ(twoxpositionsX - myGetBoxCentre.X*view.Scale, twoxpositionsY - myGetBoxCentre.Y*view.Scale, 0.0), myFamilySymbol, view);
                    //doc.Create.NewFamilyInstance(new XYZ(twoxpositionsX - myGetBoxCentre.X*view.Scale, twoxpositionsY - myGetBoxCentre.Y*view.Scale, 0.0), myFamilySymbolHorizontal, view);

                }

                //doc.Create.NewFamilyInstance(new XYZ(twoxpositionsX + intialCenter.X, twoxpositionsY + intialCenter.Y, 0.0), myFamilySymbol, view);
                //doc.Create.NewFamilyInstance(new XYZ(twoxpositionsX + intialCenter.X, twoxpositionsY + intialCenter.Y, 0.0), myFamilySymbolHorizontal, view);

                //doc.Create.NewFamilyInstance(new XYZ(twoxpositionsX + myOutline.X, twoxpositionsY + myOutline.Y, 0.0), myFamilySymbol, view);
                //doc.Create.NewFamilyInstance(new XYZ(twoxpositionsX + myOutline.X, twoxpositionsY + myOutline.Y, 0.0), myFamilySymbolHorizontal, view);


                //doc.Create.NewFamilyInstance(new XYZ(twoxpositionsX + myAGV.Origin.X, twoxpositionsY + myAGV.Origin.Y, 0.0), myFamilySymbol, view);
                //doc.Create.NewFamilyInstance(new XYZ(twoxpositionsX + myAGV.Origin.X, twoxpositionsY + myAGV.Origin.Y, 0.0), myFamilySymbolHorizontal, view);

                y.Commit();
            }
        }

        public class SampleFamilyLoadOptions : IFamilyLoadOptions
        {
            public bool OnFamilyFound(bool familyInUse, out bool overwriteParameterValues)
            {
                overwriteParameterValues = true;
                return true;
            }

            public bool OnSharedFamilyFound(Family sharedFamily, bool familyInUse, out FamilySource source, out bool overwriteParameterValues)
            {
                if (!familyInUse)
                {
                    TaskDialog.Show("SampleFamilyLoadOptions", "The shared family has not been in use and will keep loading.");

                    source = FamilySource.Family;
                    overwriteParameterValues = true;
                    return true;
                }
                else
                {
                    TaskDialog.Show("SampleFamilyLoadOptions", "The shared family has been in use but will still be loaded from the FamilySource with existing parameters overwritten.");

                    source = FamilySource.Family;
                    overwriteParameterValues = true;
                    return true;
                }
            }

        }
    }
}
