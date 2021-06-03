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

            if (doc.ActiveView.ViewType != ViewType.FloorPlan & doc.ActiveView.ViewType != ViewType.EngineeringPlan & doc.ActiveView.ViewType != ViewType.CeilingPlan)
            {
                TaskDialog.Show("Not the correct type of view", "The active view must be a view 'plan'");
                return Result.Succeeded;
            }

            //if (uid.ActiveView.ViewType != ViewType.DrawingSheet)
            //{
            //    MessageBox.Show("Please goto a 'Sheet' type view.");
            //    return;
            //}


            View myAGV2 = uidoc.ActiveView as View;

            ElementCategoryFilter filter2 = new ElementCategoryFilter(BuiltInCategory.OST_Viewports);

            FilteredElementCollector collector2 = new FilteredElementCollector(doc);


            Category myCategory = Category.GetCategory(doc, BuiltInCategory.OST_Viewports);


            if (myAGV2.Category.Id == myCategory.Id)
            {

                TaskDialog.Show("No viewport", "Macro doesn't work in active viewport." + Environment.NewLine + "Please open view from project browser");

                return Result.Succeeded;
            }

            TaskDialog mainDialog = new TaskDialog("Hello, viewport check!");
            mainDialog.MainInstruction = "Hello, viewport check!";
            mainDialog.MainContent =
                    "This doesn't work when editing in an active viewport on a sheet. "
                    + "If you ARE in an active viewport on a sheet...click No...then goto a normal plan view and try again.";


            mainDialog.AddCommandLink(TaskDialogCommandLinkId.CommandLink1, "Yes, proceed.");
            mainDialog.AddCommandLink(TaskDialogCommandLinkId.CommandLink2, "No, stop.");

            mainDialog.CommonButtons = TaskDialogCommonButtons.Close;
            mainDialog.DefaultButton = TaskDialogResult.Close;


            TaskDialogResult tResult = mainDialog.Show();

            bool YesOrNo = true;

            if (TaskDialogResult.CommandLink2 == tResult)
            {
                YesOrNo = true;

                //TaskDialog.Show("Programming stopping", "Please goto a plan view.");
                return Result.Succeeded;
            }

            if (TaskDialogResult.CommandLink1 != tResult)
            {
                return Result.Succeeded;
            }
            YesOrNo = false;


            string myString_TempPath = "";

            if (message.Split('|')[0] == "Release")  //constructs a path for release directory (in program files)
            {
                myString_TempPath = message.Split('|')[1] + "//Families";
            }
            if (message.Split('|')[0] == "Dev") //constructs a path for development directory
            {
                myString_TempPath = message.Split('|')[1] + "//Families";
            }


            CommandsWithWindows.Form_2D_Spacers form_2D_Spacers = new CommandsWithWindows.Form_2D_Spacers(myString_TempPath, uidoc, YesOrNo);

            form_2D_Spacers.ShowDialog();


            return Result.Succeeded;
        }


        public void _99_AutomaticLayout(int numericUpDown1, int numericUpDown2, UIDocument uid, string myAddinFolder, bool YesOrNo)
        {
            int eL = -1;

            try
            {
                UIDocument uidoc = uid;
                Document doc = uidoc.Document;
                View view = uidoc.ActiveView;
                ////if (useserver) PRL_Parameters = PRL_ParametersServer;
                ////if (useserverdunedin) PRL_Parameters = PRL_ParametersServerDunedin;


                int myNumber = numericUpDown1;
                int myNumberHorizontal = numericUpDown2;


                eL = 439;
                string path = Path.GetTempPath() + "` aa Spacer Families";
                //////////if (!System.IO.Directory.Exists(path)) System.IO.Directory.CreateDirectory(path);

                //////////string path = (System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\` aa Spacer Families");
                //////////if (!System.IO.Directory.Exists(path)) System.IO.Directory.CreateDirectory(path);

                //////////string subdirectory_reversedatetothesecond = (path + ("\\" + (DateTime.Now.ToString("yyyyMMddHHmmss"))));
                if (!System.IO.Directory.Exists(path)) System.IO.Directory.CreateDirectory(path);
           

                //////////string FILE_NAME = (subdirectory_reversedatetothesecond + "\\");
                string FILE_NAME = (path + "\\");

                string VertricalSpacer_FamilyName = FamilyTypeName + " x" + numericUpDown1.ToString() + " " + uid.Application.Application.VersionNumber;
                string HorizontalSpacer_FamilyName = FamilyTypeNameHorizontal + " x" + numericUpDown2.ToString() + " " + uid.Application.Application.VersionNumber;

                string mystringFilepath = FILE_NAME + VertricalSpacer_FamilyName + ".rfa";
                string mystringFilepathHorizontal = FILE_NAME + HorizontalSpacer_FamilyName + ".rfa";


                //ActiveUIDocument.ActiveView.ui
                #region view checks		

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


                List<Element> alreadyThereVertical = new FilteredElementCollector(doc).WherePasses(new ElementClassFilter(typeof(Family))).Where(x => x.Name == VertricalSpacer_FamilyName).ToList();
                List<Element> alreadyThereHorizontal = new FilteredElementCollector(doc).WherePasses(new ElementClassFilter(typeof(Family))).Where(x => x.Name == HorizontalSpacer_FamilyName).ToList();

                if ((alreadyThereVertical.Count == 0) | (alreadyThereHorizontal.Count == 0))
                {
                    //Document famDoc = uid.Document.Application.NewFamilyDocument(System.Environment.GetEnvironmentVariable("ProgramData") + "\\Autodesk\\RVT 2017\\Family Templates\\English\\Metric Detail Item.rft");
                    string string_MetricDetailItem = "\\Metric Detail Item 2019.rft";

                    if (uid.Application.Application.VersionNumber == "2018") string_MetricDetailItem = "\\Metric Detail Item 2018.rft";
                    if (uid.Application.Application.VersionNumber == "2019") string_MetricDetailItem = "\\Metric Detail Item 2019.rft";
                    if (uid.Application.Application.VersionNumber == "2020") string_MetricDetailItem = "\\Metric Detail Item 2020.rft";
                    if (uid.Application.Application.VersionNumber == "2021") string_MetricDetailItem = "\\Metric Detail Item 2021.rft";
                    if (uid.Application.Application.VersionNumber == "2022") string_MetricDetailItem = "\\Metric Detail Item 2022.rft";

                    Document famDoc = uid.Document.Application.NewFamilyDocument(myAddinFolder + string_MetricDetailItem);//uid.Document.Application.NewFamilyDocument(System.Environment.GetEnvironmentVariable("ProgramData") + "\\Autodesk\\RVT 2017\\Family Templates\\English\\Metric Detail Item.rft");
                    Document famDocHorizontal = uid.Document.Application.NewFamilyDocument(myAddinFolder + string_MetricDetailItem);

                    fileName = string.Empty;
                    fileName = myAddinFolder + "\\PRL_Parameters.txt";


                    FamilyManager familyManager = famDoc.FamilyManager;
                    FamilyManager familyManagerHorizontal = famDocHorizontal.FamilyManager;

                    if (File.Exists(fileName) == false)
                    {
                        TaskDialog.Show("Error", "Cannot find the shared parameters file.");
                        return;
                    }

                    string myStringSharedParameterFileName = "";

                    if (uidoc.Application.Application.SharedParametersFilename != null)
                    {
                        myStringSharedParameterFileName = uidoc.Application.Application.SharedParametersFilename; //Q:\Revit Revit Revit\Template 2018\PRL_Parameters.txt
                    }

                    eL = 202;
                    uidoc.Application.Application.SharedParametersFilename = fileName;
                    DefinitionFile defFile = uidoc.Application.Application.OpenSharedParameterFile();
                    DefinitionGroups myGroups = defFile.Groups;
                    DefinitionGroup myGroup = myGroups.get_Item("_98_PRL_Generic Dimensions");


                    Definitions myDefinitions = myGroup.Definitions;
                    ExternalDefinition eDef = myDefinitions.get_Item("PRL_WIDTH") as ExternalDefinition;


                    eL = 271;
                    FamilyParameter paramTd;
                    FamilyParameter paramTdHorizontal;

                    if (true)
                    {
                        #region hide this away for a bit
                        eL = 276;
                        using (Transaction y = new Transaction(famDoc, "Put in parameter"))
                        {
                            y.Start();
                            eL = 280;
                            if (familyManager.GetParameters().Where(x => x.Definition.Name == "PRL_WIDTH").Count() != 0)
                            {
                                paramTd = familyManager.GetParameters().Where(x => x.Definition.Name == "PRL_WIDTH").FirstOrDefault();
                            }
                            else
                            {
                                eL = 281;
                                paramTd = familyManager.AddParameter(eDef, BuiltInParameterGroup.PG_IDENTITY_DATA, true);
                            }

                            eL = 282;
                            if (familyManager.Types.Size == 0)
                                familyManager.NewType(FamilyTypeName);

                            y.Commit();
                        }
                        eL = 296;

                        using (Transaction y = new Transaction(famDocHorizontal, "Put in parameter"))
                        {
                            y.Start();
                            paramTdHorizontal = familyManagerHorizontal.AddParameter(eDef, BuiltInParameterGroup.PG_IDENTITY_DATA, true);

                            if (familyManagerHorizontal.Types.Size == 0)
                                familyManagerHorizontal.NewType(FamilyTypeNameHorizontal);

                            y.Commit();
                        }

                        #endregion
                    }

                    if (myStringSharedParameterFileName != "")
                    {
                        uidoc.Application.Application.SharedParametersFilename = myStringSharedParameterFileName;
                    }

                    eL = 312;
                    FilteredElementCollector viewCollector = new FilteredElementCollector(famDoc);
                    viewCollector.OfClass(typeof(Autodesk.Revit.DB.View));
                    eL = 315;
                    List<ReferencePlane> myListReferencePlane = new List<ReferencePlane>();
                    List<ReferencePlane> myListReferencePlaneHorizontal = new List<ReferencePlane>();

                    View myView = viewCollector.FirstOrDefault() as View;
                    eL = 320;
                    Options goption = new Options();
                    goption.ComputeReferences = true;
                    goption.IncludeNonVisibleObjects = true;
                    goption.View = myView;
                    #region hide all this for a bit

                    SaveAsOptions options = new SaveAsOptions();
                    options.OverwriteExistingFile = true;

                    if (alreadyThereVertical.Count == 0)
                    {

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
                    }

                    if (alreadyThereHorizontal.Count == 0)
                    {
                        #endregion
                        eL = 384;
                        using (Transaction y = new Transaction(famDocHorizontal, "The second one (horizontal)"))
                        {
                            double Ystat = twoxpositionsYStatic;
                            y.Start();
                            eL = 398;
                            myListReferencePlaneHorizontal.Add(famDocHorizontal.FamilyCreate.NewReferencePlane(new XYZ(0.0, 0.0, 0.0), new XYZ(00.0, 10.0, 0.0), new XYZ(0.0, 0.0, 1), myView));
                            eL = 400;
                            myListReferencePlaneHorizontal.Add(famDocHorizontal.FamilyCreate.NewReferencePlane(new XYZ(10.0, 0.0, 0.0), new XYZ(10.0, 10.0, 0.0), new XYZ(0.0, 0.0, 1), myView));
                            famDocHorizontal.FamilyCreate.NewDetailCurve(myView, Line.CreateBound(new XYZ(0, (Ystat / 2), 0.0), new XYZ(10, (Ystat / 2), 0.0)));
                            eL = 402;
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
                            eL = 419;
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
                        eL = 441;
                      }


                    if (alreadyThereVertical.Count == 0)
                    {
                        famDoc.SaveAs(mystringFilepath, options);
                        famDoc.Close();
                    }


                    if (alreadyThereHorizontal.Count == 0)
                    {

                        famDocHorizontal.SaveAs(mystringFilepathHorizontal, options);
                        famDocHorizontal.Close();

                    }
                }


                SampleFamilyLoadOptions mySampleFamilyLoadOptions = new SampleFamilyLoadOptions();

                Family family = null;
                Family familyHorizontal = null;

                eL = 465;

                ////MessageBox.Show("this is definitely the place right");
                ////System.Diagnostics.Process.Start(Path.GetDirectoryName(mystringFilepath));
                //_952_PRLoogleClassLibrary.DatabaseMethods.writeDebug(mystringFilepathHorizontal, true);

                using (Transaction y = new Transaction(doc, "Make a detail family and stick a line in"))
                {
                    y.Start();

                    List<Element> myListVertricalSpacer_FamilyName = new FilteredElementCollector(doc).WherePasses(new ElementClassFilter(typeof(Family))).Where(x => x.Name == VertricalSpacer_FamilyName).ToList();

                    if (myListVertricalSpacer_FamilyName.Count != 0)
                    {
                        family = myListVertricalSpacer_FamilyName.First() as Family;
                    }

                    List<Element> myListHorizontalSpacer_FamilyName = new FilteredElementCollector(doc).WherePasses(new ElementClassFilter(typeof(Family))).Where(x => x.Name == HorizontalSpacer_FamilyName).ToList();

                    if (myListHorizontalSpacer_FamilyName.Count != 0)
                    {
                        familyHorizontal = myListHorizontalSpacer_FamilyName.First() as Family;
                    }

                    if (family == null)
                    {
                        doc.LoadFamily(mystringFilepath, mySampleFamilyLoadOptions, out family);
                    }

                    if (familyHorizontal == null)
                    {
                        doc.LoadFamily(mystringFilepathHorizontal, mySampleFamilyLoadOptions, out familyHorizontal);
                    }
                    y.Commit();
                }
                eL = 471;
                ISet<ElementId> myListElementIDforFamilySymbols = family.GetFamilySymbolIds();
                ISet<ElementId> myListElementIDforFamilySymbolsHorizontal = familyHorizontal.GetFamilySymbolIds();
                eL = 474;
                FamilySymbol myFamilySymbol = doc.GetElement(myListElementIDforFamilySymbols.FirstOrDefault()) as FamilySymbol;
                FamilySymbol myFamilySymbolHorizontal = doc.GetElement(myListElementIDforFamilySymbolsHorizontal.FirstOrDefault()) as FamilySymbol;
                eL = 477;
                XYZ normal = XYZ.BasisZ;
                XYZ origin = XYZ.Zero;


                using (Transaction y = new Transaction(doc, "Placing the spacers"))
                {
                    y.Start();

                    View myAGV = uidoc.ActiveGraphicalView as View;

                    ElementCategoryFilter filter = new ElementCategoryFilter(BuiltInCategory.OST_Viewports);

                    FilteredElementCollector collector = new FilteredElementCollector(doc);

                    ICollection<Element> AllViewports = collector.WherePasses(filter).ToElements();
                    Viewport CorrespondingViewport = null;


                    string writethis = null;

                    foreach (Viewport vv in AllViewports)
                    {
                        writethis = writethis + Environment.NewLine + vv.Name;

                        View myView2 = doc.GetElement(vv.ViewId) as View;


                        if (myView2.Name == myAGV.Name)
                        {
                            CorrespondingViewport = vv;
                        }
                    }
                    myFamilySymbol.Activate();
                    myFamilySymbolHorizontal.Activate();

                    FamilyInstance myFamInstVertical = doc.Create.NewFamilyInstance(new XYZ(twoxpositionsX, twoxpositionsY, 0.0), myFamilySymbol, view);
                    FamilyInstance myFamInstHorizontal = doc.Create.NewFamilyInstance(new XYZ(twoxpositionsX, twoxpositionsY, 0.0), myFamilySymbolHorizontal, view);

                    myFamInstVertical.GetParameters("PRL_WIDTH")[0].Set(YLineLength);
                    myFamInstHorizontal.GetParameters("PRL_WIDTH")[0].Set(XLineLength);

                    y.Commit();
                }
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("_99_AutomaticLayout, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
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

