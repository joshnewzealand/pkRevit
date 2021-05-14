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
using RevitTransformSliders;

namespace pkRevitUnderstandingTransforms
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class EE08_LoadFamily : IExternalEventHandler  //this is the last when one making a checklist change, EE4 must be just for when an element is new
    {
        public ToAvoidLoadingRevitDLLs toavoidloadingrevitdlls { get; set; }
        //public Window_RotatePlatform myWindow1 { get; set; }
        public string str_getOrLoadThis_Family  { get; set; }
        public bool bool_Loop_UntilFinished { get; set; } = false;


        public void Execute(UIApplication uiapp)
        {

            int eL = -1;

            try
            {
                UIDocument uidoc = uiapp.ActiveUIDocument;
                Document doc = uidoc.Document;



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

                string myString_TempPath = null;
                if (toavoidloadingrevitdlls.executionLocation.Split('|')[0] == "Release")  //constructs a path for release directory (in program files)
                {
                    myString_TempPath = toavoidloadingrevitdlls.executionLocation.Split('|')[1] + "\\Platforms\\" + str_getOrLoadThis_Family + ".rfa";
                }
                if (toavoidloadingrevitdlls.executionLocation.Split('|')[0] == "Dev") //constructs a path for development directory
                {
                    myString_TempPath = toavoidloadingrevitdlls.executionLocation.Split('|')[1] + "\\Platforms\\" + str_getOrLoadThis_Family + ".rfa";
                }

                using (Transaction tx = new Transaction(doc))
                {
                    tx.Start("Load a " + str_getOrLoadThis_Family);
                    doc.LoadFamily(myString_TempPath, new FamilyOptionOverWrite(), out Family myFamily);
                    tx.Commit();
                }


                bool_Loop_UntilFinished = false;

                ////string myStringStart = myInt.ToString() + " families have been loaded: " + Environment.NewLine + Environment.NewLine;

                ////MessageBox.Show(myStringStart + myStringMessageBox + Environment.NewLine + Environment.NewLine + "This only happens once per project.");

            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("EE08_LoadFamily, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
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
