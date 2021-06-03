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
using pkRevitMisc.EntryPoints;

namespace pkRevitMisc.CommandsWithWindows.Add_Edit_Parameters
{

    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class EE16_AddSharedParameters_InVariousWays : IExternalEventHandler  //this is the last when one making a checklist change, EE4 must be just for when an element is new
    {
        //public Entry_0170_pkRevitMisc myWindow1 { get; set; }
        public Window1617_AddEditParameters myWindow2 { get; set; }
        public bool myBool_AddToProject { get; set; } = true;
        public int myInt_LoadType { get; set; }

        public void Execute(UIApplication uiapp)
        {
            try
            {
                UIDocument uidoc = myWindow2.toavoidloadingrevitdlls.commandData.Application.ActiveUIDocument;
                Document doc = uidoc.Document;

                myWindow2.myTextBoxPrevious.Text = "";
                myWindow2.myTextBoxNew.Text = "";
                myWindow2.myListBoxInstanceParameters.SelectedIndex = -1;
                myWindow2.myListBoxTypeParameters.SelectedIndex = -1;


                string myStringCollectup = myMethod_BindParameters();

                if (myStringCollectup != "") MessageBox.Show(myStringCollectup);
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("EE16_AddSharedParameters_InVariousWays" + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        public string GetName()
        {
            return "EE07_Part1_Binding";
        }

        public string myMethod_BindParameters()
        {
            UIDocument uidoc = myWindow2.toavoidloadingrevitdlls.commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            string myStringSharedParameterFileName = "";

            if (uidoc.Application.Application.SharedParametersFilename != null)
            {
                myStringSharedParameterFileName = uidoc.Application.Application.SharedParametersFilename; //Q:\Revit Revit Revit\Template 2018\PRL_Parameters.txt
            }

            int eL = -1;
            try
            {
                string path = "";
                bool IsTypeParameter = false;
                bool IsFamily = false;

                string pathSharedParameterFiles = (System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\pk Revit\\Shared Parameter Files");
                string Instance_Specific = "\\CategoryInstanceSpecific.txt";
                string Instance_ALL = "\\CategoryInstanceALL.txt";
                string Type_Specific = "\\CategoryTypeSpecific.txt";
                string Type_ALL = "\\CategoryTypeALL.txt";
                string Type_Family = "\\CategoryTypeFamily.txt";

                CategorySet catSet = null;

                if(myInt_LoadType == (int)PARAMETER_LOAD.Instance_Specific | myInt_LoadType == (int)PARAMETER_LOAD.Type_Specific | myInt_LoadType == (int)PARAMETER_LOAD.Type_Family)
                {
                    if (doc.GetElement(new ElementId(myWindow2.myIntegerUpDown.Value.Value)) == null)
                    {
                        return "Please use 'Acquire Selected' button.";
                    }
                }

                eL = 98;
                switch (myInt_LoadType)
                {
                    case (int)PARAMETER_LOAD.Instance_Specific:
                        path = pathSharedParameterFiles + Instance_Specific;
                        IsTypeParameter = false;
                        catSet = uidoc.Application.Application.Create.NewCategorySet();

                        Element aElement = doc.GetElement(new ElementId(myWindow2.myIntegerUpDown.Value.Value));
                        //catSet.Insert(doc.Settings.Categories.Cast<Category>().Where(x => x.Name == aElement.Category.Name).Last());
                        catSet.Insert(aElement.Category);
                        //MessageBox.Show("Instance_Specific3");
                        break;
                    case (int)PARAMETER_LOAD.Instance_ALL:
                        path = pathSharedParameterFiles + Instance_ALL;
                        IsTypeParameter = false;
                        catSet = uidoc.Application.Application.Create.NewCategorySet();
                        foreach (Category myCatttt in doc.Settings.Categories) if (myCatttt.AllowsBoundParameters) catSet.Insert(myCatttt);
                        //MessageBox.Show("Instance_ALL");
                        break;
                    case (int)PARAMETER_LOAD.Type_Specific:
                        path = pathSharedParameterFiles + Type_Specific;
                        IsTypeParameter = true;
                        catSet = uidoc.Application.Application.Create.NewCategorySet();
                        catSet.Insert(doc.GetElement(new ElementId(myWindow2.myIntegerUpDown.Value.Value)).Category);
                        //MessageBox.Show("Type_Specific");
                        break;
                    case (int)PARAMETER_LOAD.Type_ALL:
                        path = pathSharedParameterFiles + Type_ALL;
                        IsTypeParameter = true;
                        catSet = uidoc.Application.Application.Create.NewCategorySet();
                        foreach (Category myCatttt in doc.Settings.Categories) if (myCatttt.AllowsBoundParameters) catSet.Insert(myCatttt);
                        //MessageBox.Show("Type_ALL");
                        break;
                    case (int)PARAMETER_LOAD.Type_Family:
                        path = pathSharedParameterFiles + Type_Family;
                        IsTypeParameter = true;
                        IsFamily = true;
                        catSet = uidoc.Application.Application.Create.NewCategorySet();
                        catSet.Insert(doc.GetElement(new ElementId(myWindow2.myIntegerUpDown.Value.Value)).Category);
                        break;
                }
                eL = 137;
                ///////////////////////////path = pathSharedParameterFiles + Instance_Specific;

                if (catSet == null)
                {
                    if (myStringSharedParameterFileName != "")
                    {
                        uidoc.Application.Application.SharedParametersFilename = myStringSharedParameterFileName;
                    }
                    return "";
                }

                eL = 148;

                ///               TECHNIQUE 16 OF 19 (EE16_AddSharedParameters_InVariousWays.cs)
                ///↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓ ADDING SHARED PARAMETERS IN VARIOUS WAYS ↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓
                ///
                /// Interfaces and ENUM's:
                /// 
                /// 
                /// Demonstrates classes::
                ///     DefinitionFile
                ///     CategorySet
                ///     DefinitionGroup
                ///     Definition
                ///     Binding
                /// 
                /// 
                /// Key methods:
                ///     uidoc.Application.Application.OpenSharedParameterFile();
                ///     catSet.Insert(
                ///     myDefinitionFile.Groups.get_Item(
                ///     uidoc.Application.Application.Create.NewTypeBinding(
                ///     uidoc.Application.Application.Create.NewInstanceBinding(
                ///     doc.ParameterBindings.Insert(
                ///     
                ///     famDoc.FamilyManager.AddParameter(
                /// 
                ///
                ///
                ///	https://github.com/joshnewzealand/Revit-API-Playpen-CSharp


                // throw new InvalidOperationException("Testing that it resets");
                string myStringCollectup = "";

                uidoc.Application.Application.SharedParametersFilename = path;
                eL = 182;
                DefinitionFile myDefinitionFile = uidoc.Application.Application.OpenSharedParameterFile();
                eL = 184;
                if (myDefinitionFile == null)
                {
                    DatabaseMethods.writeDebug(path + Environment.NewLine + Environment.NewLine + "File does not exist OR cannot be opened.", true);
                    return "";
                }
                DefinitionGroup group = myDefinitionFile.Groups.get_Item("Default");
                eL = 189;

                if (!IsFamily)
                {
                    using (Transaction tx = new Transaction(doc))
                    {
                        tx.Start("Shared parameters to Project");

                        foreach (Definition myDefinition in group.Definitions)
                        {
                            myStringCollectup = myStringCollectup + " • " + myDefinition.Name + Environment.NewLine;

                            eL = 201;

                            bool bool_Reinserted = false;

                            BindingMap bindingMap = doc.ParameterBindings;
                            DefinitionBindingMapIterator it = bindingMap.ForwardIterator();
                            it.Reset();
                            while (it.MoveNext())
                            {
                                if (it.Key.Name == myDefinition.Name)
                                {

                                    eL = 213;

                                    if (it.Current is InstanceBinding)
                                    {
                                        foreach (Category cat in catSet)
                                        {
                                            if (!((InstanceBinding)it.Current).Categories.Contains(cat))
                                            {
                                                ((InstanceBinding)it.Current).Categories.Insert(cat);
                                                bool success = bindingMap.ReInsert(myDefinition, (InstanceBinding)it.Current, BuiltInParameterGroup.PG_IDENTITY_DATA);
                                            }
                                        }
                                    }
                                    if (it.Current is TypeBinding)
                                    {
                                        foreach (Category cat in catSet)
                                        {
                                            if (!((TypeBinding)it.Current).Categories.Contains(cat))
                                            {
                                                ((TypeBinding)it.Current).Categories.Insert(cat);
                                                bool success = bindingMap.ReInsert(myDefinition, (TypeBinding)it.Current, BuiltInParameterGroup.PG_IDENTITY_DATA);
                                            }
                                        }
                                    }
                                    bool_Reinserted = true;
                                    break;
                                }
                            }

                            if (!bool_Reinserted)
                            {
                                Binding binding = IsTypeParameter ? uidoc.Application.Application.Create.NewTypeBinding(catSet) as Binding : uidoc.Application.Application.Create.NewInstanceBinding(catSet) as Binding;

                                doc.ParameterBindings.Insert(myDefinition, binding, BuiltInParameterGroup.PG_IDENTITY_DATA);
                            }

                            //MessageBox.Show("hello world");
                        }
                        tx.Commit();
                    }
                    string string_CategoryOrAllCategories = "ALL categories";

                    if (catSet.Size == 1) string_CategoryOrAllCategories = "'" + catSet.Cast<Category>().First().Name + "' category";

                    myStringCollectup = (IsTypeParameter ? "Type" : "Instance") + " parameters added to Project (" + string_CategoryOrAllCategories + "):" + Environment.NewLine + myStringCollectup;
                } else
                {
                    Element myElement = doc.GetElement(new ElementId(myWindow2.myIntegerUpDown.Value.Value)) as Element;

                    if (myElement.GetType() == typeof(FamilyInstance))
                    {
                        eL = 194;

                        FamilyInstance myFamilyInstance = myElement as FamilyInstance;

                        Family myFamily = ((FamilySymbol)doc.GetElement(myFamilyInstance.GetTypeId())).Family;

                        if (myFamily.IsEditable)
                        {
                            Document famDoc = null;
                            famDoc = doc.EditFamily(myFamily);

                            bool myBool_GoForthAndAddParameters = false;

                            foreach (Definition myDefinition in group.Definitions)
                            {
                                myStringCollectup = myStringCollectup + " • " + myDefinition.Name + Environment.NewLine;
                                if (famDoc.FamilyManager.Parameters.Cast<FamilyParameter>().Where(x => x.Definition.Name == myDefinition.Name).Count() == 0)
                                {
                                    myBool_GoForthAndAddParameters = true;
                                }
                            }

                            if (myBool_GoForthAndAddParameters)
                            {
                                using (Transaction tx = new Transaction(famDoc))
                                {
                                    tx.Start("Shared parameters to Family");

                                    foreach (ExternalDefinition eD in group.Definitions)
                                    {
                                        if (famDoc.FamilyManager.Parameters.Cast<FamilyParameter>().Where(x => x.Definition.Name == eD.Name).Count() == 0)
                                        {
                                            famDoc.FamilyManager.AddParameter(eD, BuiltInParameterGroup.PG_TEXT, false);
                                        }
                                    }
                                    tx.Commit();
                                }
                                myFamily = famDoc.LoadFamily(doc, new FamilyOption());
                            }
                            famDoc.Close(false);
                        }
                        myStringCollectup = (IsTypeParameter ? "Type" : "Instance") + " parameters added to Family:" + Environment.NewLine + myStringCollectup;
                    } else
                    {
                        MessageBox.Show(myElement.get_Parameter(BuiltInParameter.ELEM_FAMILY_PARAM).AsValueString() + Environment.NewLine + Environment.NewLine + "...is a system family and is not *.rfa available.");
                    }
                }


                if (myStringSharedParameterFileName != "")
                {
                    uidoc.Application.Application.SharedParametersFilename = myStringSharedParameterFileName;
                }
                return (myStringCollectup);
            }

            #region catch and finally
            catch (Exception ex)
            {
                uidoc.Application.Application.SharedParametersFilename = myStringSharedParameterFileName;
                uidoc.Application.Application.OpenSharedParameterFile();
                //uidoc.Application.Application.SharedParametersFilename = "";

                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("EE17_AddSharedParameters_InVariousWays, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);

                return "";
            }
            finally
            {
            }
            #endregion

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
