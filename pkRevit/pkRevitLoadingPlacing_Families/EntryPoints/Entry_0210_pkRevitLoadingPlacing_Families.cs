extern alias global3;

using global3.Autodesk.Revit.DB;

//using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Runtime.Serialization;
using System.Windows.Forms;

namespace pkRevitLoadingPlacing_Families.EntryPoints  //Entry_0010_pkRevitDatasheets
{
    public static class Families_ThatMustBeLoaded
    {
        public static string myString00 = "Furniture Chair Executive";
        public static string myString01 = "Furniture Chair Viper";
        public static string myString02 = "Furniture Couch Viper";
        public static string myString03 = "Furniture Desk";
        public static string myString04 = "Furniture Table Dining Round w Chairs";
        public static string myString05 = "Furniture Table Night Stand";
        public static string myString06 = "Statue Virgin Mary";
        public static string myString07 = "Generic Adaptive Nerf Gun";
        public static string myString08 = "Generic Model Man Sitting Eating";
        public static string myString09 = "Generic Model Man Women Construction Worker";
        public static string myString10 = "Generic Model Tipping Hat Man";
        public static string myString11 = "Recessed Downlight Face Based";
        public static string myString12 = "Recessed Troffer Face Based";
        //public static string myString13 = "MiniDigger";

        public static List<string> ListStringMustHaveFamilies = new List<string>() { /*myString13 should be the end*/ myString00, myString01, myString02, myString03, myString04, myString05, myString06, myString07, myString08, myString09, myString10, myString11, myString12};
    }

    public partial class Entry_0210_pkRevitLoadingPlacing_Families
    {
        ////////////////////public MainWindow myWindow1 { get; set; }

        public pkRevitMisc.ToAvoidLoadingRevitDLLs toavoidloadingrevitdlls { get; set; }

        public Result StartMethod_0210(ExternalCommandData cd, ref string message, ElementSet elements)
        {
            ExternalCommandData commandData = cd;
            string executionLocation = message;

            toavoidloadingrevitdlls = new pkRevitMisc.ToAvoidLoadingRevitDLLs();
            toavoidloadingrevitdlls.commandData = commandData;
            toavoidloadingrevitdlls.executionLocation = executionLocation;

            int eL = -1;
            Window0506_LoadAndPlaceFamilies myWindow3 = new Window0506_LoadAndPlaceFamilies(commandData);

            try
            {
               ////////////////////myWindow1 = new MainWindow(commandData, this, true, 0);
                myWindow3.myThisApplication = this;
                myWindow3.Topmost = true;
                myWindow3.Show();
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("Entry_0210_pkRevitLoadingPlacing_Families, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion

            return Result.Succeeded;
        }
    }


}
