extern alias global3;

using global3.Autodesk.Revit.DB;
using global3.Autodesk.Revit.DB.ExtensibleStorage;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;


namespace pkRevitLoadingPlacing_Families
{
    [global3.Autodesk.Revit.Attributes.Transaction(global3.Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class EE05_LoadAllFamilies : IExternalEventHandler  //this is the last when one making a checklist change, EE4 must be just for when an element is new
    {
        public Window0506_LoadAndPlaceFamilies myWindow1 { get; set; }
        public string string_JustLoadThisOne  { get; set; }

        public bool bool_Loop_UntilFinished { get; set; } = false;
        

        public void Execute(UIApplication uiapp)
        {

            int eL = -1;

            try
            {
                UIDocument uidoc = uiapp.ActiveUIDocument;
                Document doc = uidoc.Document;

                string myStringMessageBox = "";

                int myInt = 0;


                ///                             TECHNIQUE 5 OF 19 (EE05_LoadAllFamilies.cs)
                ///↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓ LOADING ALL THE FAMILIES FROM A LISTVIEW ↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓
                ///
                /// Interfaces and ENUM's:
                ///     IFamilyLoadOptions
                /// 
                /// Demonstrates classes:
                ///     Window0506_LoadAndPlaceFamilies.ListView_Class*
                ///     
                /// 
                /// Key methods:
                ///     doc.LoadFamily(
                ///
                /// 
                /// * class is part of C# playpen (not Revit API)
                ///	
                ///	
                ///	
                ///	https://github.com/joshnewzealand/Revit-API-Playpen-CSharp
                eL = 55;

                foreach (Window0506_LoadAndPlaceFamilies.ListView_Class myListView_Class in myWindow1.myListClass)
                {
                    if (string_JustLoadThisOne != "") if (string_JustLoadThisOne != myListView_Class.String_Name) continue;

                    eL = 59;
                    List<Element> myListFamily = new FilteredElementCollector(doc).WherePasses(new ElementClassFilter(typeof(Family))).Where(x => x.Name == myListView_Class.String_Name).ToList();

                    if (myListFamily.Count == 0)
                    {
                        string myString_TempPath = "";

                        eL = 65;
                        if (myWindow1.myThisApplication.toavoidloadingrevitdlls.executionLocation.Split('|')[0] == "Release")  //constructs a path for release directory (in program files)
                        {
                            myString_TempPath = myWindow1.myThisApplication.toavoidloadingrevitdlls.executionLocation.Split('|')[1] + myListView_Class.String_FileName;
                        }
                        if (myWindow1.myThisApplication.toavoidloadingrevitdlls.executionLocation.Split('|')[0] == "Dev") //constructs a path for development directory
                        {
                            myString_TempPath = myWindow1.myThisApplication.toavoidloadingrevitdlls.executionLocation.Split('|')[1] + myListView_Class.String_FileName;
                        }
                        using (Transaction tx = new Transaction(doc))
                        {
                            tx.Start("Load a " + myListView_Class.String_Name);
                            doc.LoadFamily(myString_TempPath, new FamilyOptionOverWrite(), out Family myFamily);
                            tx.Commit();
                        }

                        myStringMessageBox = myStringMessageBox + Environment.NewLine + myListView_Class.String_Name;
                        myInt++;
                    }
                }

                string myStringStart = myInt.ToString() + " families have been loaded: " + Environment.NewLine + Environment.NewLine;

                if (string_JustLoadThisOne == "") MessageBox.Show(myStringStart + myStringMessageBox + Environment.NewLine + Environment.NewLine + "This only happens once per project.");

                bool_Loop_UntilFinished  = false;
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("EE05_LoadAllFamilies, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
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
    }

}
