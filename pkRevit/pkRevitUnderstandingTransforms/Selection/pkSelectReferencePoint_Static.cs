/*
 * Created by SharpDevelop.
 * User: Joshua.Lumley
 * Date: 28/04/2019
 * Time: 5:59 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using _952_PRLoogleClassLibrary;
using RevitTransformSliders;

namespace pkRevitUnderstandingTransforms
{
    public static class pkSelectReferencePoint_Static
    {
        public static int pkSelectRefPoint_AndReturnIDInteger(ToAvoidLoadingRevitDLLs toavoidloadingrevitdlls, bool bool_ShowMessageBox)
        {
            int eL = -1;

            try
            {
                UIDocument uidoc = toavoidloadingrevitdlls.commandData.Application.ActiveUIDocument;
                Document doc = uidoc.Document;

                if (uidoc.Selection.GetElementIds().Count > 1)
                {
                    foreach (ElementId eleID in uidoc.Selection.GetElementIds())
                    {
                        Element myElement = doc.GetElement(uidoc.Selection.GetElementIds().First());

                        if (myElement.GetType() == typeof(FamilyInstance))
                        {
                            IList<ElementId> placePointIds_0805 = AdaptiveComponentInstanceUtils.GetInstancePointElementRefIds((FamilyInstance)myElement);

                            if (placePointIds_0805.Count() != 0)
                            {
                                ReferencePoint myReferencePoint_0805 = doc.GetElement(placePointIds_0805.First()) as ReferencePoint;

                                uidoc.Selection.SetElementIds(new List<ElementId>() { myReferencePoint_0805.Id });

                                pkRevitUnderstandingTransforms.Properties.Settings.Default.LastSelectedID = myReferencePoint_0805.Id.IntegerValue;
                                pkRevitUnderstandingTransforms.Properties.Settings.Default.Save();
                                pkRevitUnderstandingTransforms.Properties.Settings.Default.Reload();

                                return myReferencePoint_0805.Id.IntegerValue;
                            }
                        }
                    }

                    MessageBox.Show("Your selection must contain FamilyInstance with reference point");
                    return -1;
                }

                if (uidoc.Selection.GetElementIds().Count == 0)
                {
                    int lastSelectedID = pkRevitUnderstandingTransforms.Properties.Settings.Default.LastSelectedID;

                    Element ele_TestThereAndOfRightType = doc.GetElement(new ElementId(lastSelectedID)) as Element;
                   // MessageBox.Show("it should it here");
                    if (ele_TestThereAndOfRightType != null)
                    {
                        //MessageBox.Show("it should it here");
                        if (ele_TestThereAndOfRightType.GetType() == typeof(ReferencePoint))
                        {
                            uidoc.Selection.SetElementIds(new List<ElementId>() { ele_TestThereAndOfRightType.Id });

                            //MessageBox.Show("it should it here");
                            return ele_TestThereAndOfRightType.Id.IntegerValue;
                        }
                    }

                    FilteredElementCollector fec2 = new FilteredElementCollector(doc).WhereElementIsNotElementType().OfCategory(BuiltInCategory.OST_AdaptivePoints);
                    List<Element> myListRefPoint = fec2.ToList();

                    if (myListRefPoint.Count() != 0)
                    {
                        //MessageBox.Show("we're hitting here right?");
                        ReferencePoint myReferencePoint_0805 = myListRefPoint.Last() as ReferencePoint;
                        uidoc.Selection.SetElementIds(new List<ElementId>() { new ElementId(myReferencePoint_0805.Id.IntegerValue - 1) });
                        return myReferencePoint_0805.Id.IntegerValue - 1;
                    }
                    else
                    {
                        if (bool_ShowMessageBox) MessageBox.Show("There are no reference points in the model");
                        return -1;
                    }
                }

                Element myElement_SingleSelection = doc.GetElement(uidoc.Selection.GetElementIds().First());
                eL = 100;

                //IList<ElementId> placePointIds_1338 = AdaptiveComponentInstanceUtils.GetInstancePointElementRefIds(myElement_SingleSelection as FamilyInstance);
                //ReferencePoint myReferencePoint_Centre = doc.GetElement(placePointIds_1338.First()) as ReferencePoint;

                if (myElement_SingleSelection.GetType() == typeof(ReferencePoint))
                {
                    ReferencePoint myReferencePoint = myElement_SingleSelection as ReferencePoint;
                    eL = 106;

                    return myReferencePoint.Id.IntegerValue;
                }
                eL = 113;

                if (!(myElement_SingleSelection.GetType() == typeof(FamilyInstance) | myElement_SingleSelection.GetType() == typeof(IndependentTag)))
                {
                    MessageBox.Show("Please select a family Instance.");
                    return -1;
                }

                FamilyInstance myFamilyInstance = null;
                eL = 122;
                if (myElement_SingleSelection.get_Parameter(BuiltInParameter.ELEM_FAMILY_PARAM).AsValueString() != "PRL-GM-2020 Adaptive Carrier")
                {
                    FamilyInstance myElementFitting = null;

                    if (myElement_SingleSelection.GetType() == typeof(IndependentTag))
                    {
                        IndependentTag myIndependentTag_1355 = myElement_SingleSelection as IndependentTag;
                        if (myIndependentTag_1355.TaggedLocalElementId != null)
                        {
                            myElementFitting = doc.GetElement(myIndependentTag_1355.TaggedLocalElementId) as FamilyInstance;
                        }
                        else
                        {
                            return -1;
                        }
                    }
                    else
                    {
                        myElementFitting = myElement_SingleSelection as FamilyInstance;
                    }

                    eL = 148;
                    if (myElementFitting.Host != null)
                    {
                        List<Element> myListElement = new FilteredElementCollector(doc).OfClass(typeof(FamilyInstance)).OfCategory(BuiltInCategory.OST_GenericModel).Where(x => x.Id == myElementFitting.Host.Id).ToList();
                        if (myListElement.Count == 1)
                        {
                            myFamilyInstance = myListElement.First() as FamilyInstance;

                            string str_FamilyName_2148 = myFamilyInstance.get_Parameter(BuiltInParameter.ELEM_FAMILY_PARAM).AsValueString();

                            if (str_FamilyName_2148 != "PRL-GM Adaptive Carrier Youtube" & str_FamilyName_2148 != "PRL-GM-2020 Adaptive Carrier")
                            {
                                MessageBox.Show("Name of family instance is not 'PRL-GM-2020 Adaptive Carrier'");
                                return -1;
                            }
                        }
                        else
                        {
                            return -1;
                        }
                    } else
                    {
                        myFamilyInstance = myElement_SingleSelection as FamilyInstance;

                        string str_FamilyName_2148 = myFamilyInstance.get_Parameter(BuiltInParameter.ELEM_FAMILY_PARAM).AsValueString();

                        if (str_FamilyName_2148 != "PRL-GM Adaptive Carrier Youtube" & str_FamilyName_2148 != "PRL-GM-2020 Adaptive Carrier")
                        {
                            MessageBox.Show("Name of family instance is not 'PRL-GM-2020 Adaptive Carrier'");
                            return -1;
                        }
                    }
                }
                else
                {
                    myFamilyInstance = myElement_SingleSelection as FamilyInstance;
                }
                eL = 172;
                if (true)
                {
                    eL = 178;
                    IList<ElementId> placePointIds_1337 = AdaptiveComponentInstanceUtils.GetInstancePointElementRefIds(myFamilyInstance);
                    eL = 179;
                    ReferencePoint myReferencePoint = doc.GetElement(placePointIds_1337.First()) as ReferencePoint;
                    eL = 180;
                    uidoc.Selection.SetElementIds(new List<ElementId>() { myReferencePoint.Id });
                    eL = 182;
                    pkRevitUnderstandingTransforms.Properties.Settings.Default.LastSelectedID = myReferencePoint.Id.IntegerValue;
                    pkRevitUnderstandingTransforms.Properties.Settings.Default.Save();
                    pkRevitUnderstandingTransforms.Properties.Settings.Default.Reload();

                    return myReferencePoint.Id.IntegerValue;
                }
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("pkSelectRefPoint_AndReturnIDInteger, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion

            return -1;
        }
    }
}
