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
    public partial class Entry_0060_pkRevitMisc
    {
        class DetailCurveSelectionFilter : ISelectionFilter  // i would like to merge this later with the above but, i think there is efficient code somewhere
        {
            Autodesk.Revit.DB.Document doc = null;

            public DetailCurveSelectionFilter(Document document)
            {
                doc = document;
            }

            public bool AllowElement(Element e)
            {

                return true;
            }

            public bool AllowReference(Reference r, XYZ p)
            {
                return true;
            }
        }
        int eL = -1;

        public Result StartMethod_0060(ExternalCommandData cd, ref string message, ElementSet elements)
        {
            ExternalCommandData commandData = cd;
            string executionLocation = message;

            ToAvoidLoadingRevitDLLs toavoidloadingrevitdlls = new ToAvoidLoadingRevitDLLs();
            toavoidloadingrevitdlls.commandData = commandData;
            toavoidloadingrevitdlls.executionLocation = executionLocation;

            try
            {
                UIDocument uidoc = commandData.Application.ActiveUIDocument;
                Document doc = uidoc.Document;

                Element fam01_Or_Link = null;
                Reference ref01 = null;
                Document docLink = null;
                ElementType elementType = null;

                if (uidoc.Selection.GetElementIds().Count == 1)
                {
                    fam01_Or_Link = doc.GetElement(uidoc.Selection.GetElementIds().First());
                    if (fam01_Or_Link.GetType() == typeof(RevitLinkInstance))
                    {
                        docLink = ((RevitLinkInstance)fam01_Or_Link).GetLinkDocument();
                    }
                }
                eL = 80;
                if (docLink == null)
                {
                    ref01 = uidoc.Selection.PickObject(ObjectType.Element, "Please pick element. ESC for cancel.");
                    if (ref01 == null) return Result.Succeeded;

                    fam01_Or_Link = doc.GetElement(ref01.ElementId);
                    elementType = doc.GetElement(fam01_Or_Link.GetTypeId()) as ElementType;
                }
                eL = 88;

                if (fam01_Or_Link.GetType() == typeof(RevitLinkInstance))
                {
                    docLink = ((RevitLinkInstance)fam01_Or_Link).GetLinkDocument();
                    ref01 = uidoc.Selection.PickObject(ObjectType.LinkedElement, new DetailCurveSelectionFilter(doc), "Please pick element. ESC for cancel.");

                    fam01_Or_Link = docLink.GetElement(ref01.LinkedElementId);
                    elementType = docLink.GetElement(fam01_Or_Link.GetTypeId()) as ElementType;  //bug fix 0003 20210608 - @"...is not the kind with 'Types' error message repaired.
                }
                eL = 97;

                if (fam01_Or_Link == null)
                {
                    MessageBox.Show("Try again, this time selecting the linked file first.");
                    return Result.Succeeded;
                }

                eL = 107;
                if (elementType == null)
                {
                    MessageBox.Show("Selected element is not the kind with 'Types'.");
                    return Result.Succeeded;
                }
                eL = 113;
                if (elementType.Category == null)
                {
                    MessageBox.Show("Filtered can't be applied to annotative elements.");
                    return Result.Succeeded;
                }
                eL = 119;
                if (elementType.Category.IsTagCategory)
                {
                    MessageBox.Show("'" + elementType.FamilyName + "' is a Tag Category." + Environment.NewLine + Environment.NewLine + "...and filters can't be applied to tag categories." + Environment.NewLine + Environment.NewLine + "try either: Hide Element OR Hide Category.");
                    return Result.Succeeded;
                }
                eL = 125;
                bool bool_Stop = true;
                bool bool_HideJustType = false;

                if (true)
                {
                    TaskDialog mainDialog = new TaskDialog("Choose FAMILY or TYPE");
                    ////mainDialog.MainInstruction = "Hello, viewport check!";
                    ////mainDialog.MainContent =
                    ////        "Revit API doesn't automatically know if the user is in an active viewport. "
                    ////        + "Please click 'Yes' if your are, or 'No' if your not.";

                    mainDialog.AddCommandLink(TaskDialogCommandLinkId.CommandLink1, "Add hide filter for FAMILY.");
                    mainDialog.AddCommandLink(TaskDialogCommandLinkId.CommandLink2, "Add hide filter for TYPE.");
                    mainDialog.AddCommandLink(TaskDialogCommandLinkId.CommandLink3, "Cancel.");

                    mainDialog.CommonButtons = TaskDialogCommonButtons.Close;
                    mainDialog.DefaultButton = TaskDialogResult.Close;


                    TaskDialogResult tResult = mainDialog.Show();


                    if (TaskDialogResult.CommandLink1 == tResult) bool_Stop = false;

                    if (TaskDialogResult.CommandLink2 == tResult)
                    {
                        bool_HideJustType = true;
                        bool_Stop = false;
                    }
                }

                if (bool_Stop) return Result.Succeeded;


                ////MessageBoxResult result = System.Windows.MessageBox.Show("Add filter for '" + elementType.FamilyName + "'?" + Environment.NewLine + "'No' will just copy to clipboard.", "Continue", System.Windows.MessageBoxButton.YesNoCancel);

                ////eL = 118;

                ////if (result == MessageBoxResult.No)
                ////{
                ////    System.Windows.Clipboard.SetText(elementType.FamilyName);
                ////    return Result.Succeeded;
                ////}
                ////else if (result == MessageBoxResult.Cancel)
                ////{
                ////    return Result.Succeeded;
                ////}
                ////eL = 129;

                //MessageBox.Show(famSym.Category.Id.ToString());
                List<ElementId> categories = new List<ElementId>();
                categories.Add(fam01_Or_Link.Category.Id);

                List<ElementFilter> elemFilters = new List<ElementFilter>();

                using (Transaction tx = new Transaction(doc))
                {
                    tx.Start("EE23_FamilyNameFilter");

                    eL = 141;

                    if (true)
                    {
                        List<Element> listOfFilters = new FilteredElementCollector(doc).OfClass(typeof(ParameterFilterElement)).ToList().Where(x => x.Name == elementType.FamilyName).ToList();
                        ParameterFilterElement parameterFilterElement = null;
                        eL = 147;
                        if (listOfFilters.Count > 0)
                        {
                            eL = 150;
                            parameterFilterElement = listOfFilters.First() as ParameterFilterElement;
                        }
                        else
                        {
                            eL = 154;
                            // Create filter element assocated to the input categories
                            parameterFilterElement = ParameterFilterElement.Create(doc, bool_HideJustType ?  elementType.FamilyName + " " + elementType.Name : elementType.FamilyName, categories);
                            eL = 158;
                            // Criterion 1 - this could be one of many elements
                            ElementId exteriorParamId = new ElementId(BuiltInParameter.ALL_MODEL_FAMILY_NAME);
                            elemFilters.Add(new ElementParameterFilter(ParameterFilterRuleFactory.CreateEqualsRule(exteriorParamId, elementType.FamilyName, true)));
                            ElementId exteriorParamId_Type = new ElementId(BuiltInParameter.ALL_MODEL_TYPE_NAME);
                            if (bool_HideJustType) elemFilters.Add(new ElementParameterFilter(ParameterFilterRuleFactory.CreateEqualsRule(exteriorParamId_Type, elementType.Name, true)));
                            eL = 162;
                            System.Windows.Clipboard.SetText(elementType.FamilyName);
                            eL = 162;
                            LogicalAndFilter elemFilter = new LogicalAndFilter(elemFilters);
                            eL = 166;
                            parameterFilterElement.SetElementFilter(elemFilter);
                            //One of the given rules refers to a parameter that does not apply to this filter's categories.  Parameter name: elementFilter
                        }
                        eL = 170;
                        if (uidoc.ActiveView.GetFilters().Where(x => x.IntegerValue == parameterFilterElement.Id.IntegerValue).Count() == 0)
                        {
                            eL = 172;
                            uidoc.ActiveView.AddFilter(parameterFilterElement.Id);
                        }
                        eL = 175;
                        uidoc.ActiveView.SetFilterVisibility(parameterFilterElement.Id, false);
                    }
                    tx.Commit();
                }
            }

            #region catch and finally
            catch (Exception ex)
            {
                if (ex.Message == "The user aborted the pick operation.") return Result.Succeeded;

                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("StartMethod_0060, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion

            return Result.Succeeded;
        }
    }
}
