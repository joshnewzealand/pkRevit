
using Autodesk.Revit.DB;


using Autodesk.Revit.DB.Electrical;


using Autodesk.Revit.DB.Mechanical;


using Autodesk.Revit.DB.Plumbing;


using Autodesk.Revit.UI;


using Autodesk.Revit.UI.Selection;

using System;
using System.Data;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using View = Autodesk.Revit.DB.View;


using _952_PRLoogleClassLibrary;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Windows.Forms;


namespace pkRevitCircleOutFamilies.EntryPoints
{

    public partial class Entry_0110_pkRevitCircleOutFamilies
    {



        public const string myString_1400_filename = "\\ALL Families Master List.xml";
        public string myString_1400_filename_FirstHalf { get; set; }

        public const string messageTextType = "2.0mm Arial Narrow";  //should go into shared code, but there are others, candidate for methodisation 202001011854
        public string messageConst { get; set; }

        public TextNoteType myTextNoteType { get; set; }


#pragma warning disable CS0246 // The type or namespace name 'ExternalCommandData' could not be found (are you missing a using directive or an assembly reference?)
        public ExternalCommandData myCommandData { get; set; }
#pragma warning restore CS0246 // The type or namespace name 'ExternalCommandData' could not be found (are you missing a using directive or an assembly reference?)
        public string myMessage { get; set; }
#pragma warning disable CS0246 // The type or namespace name 'ElementSet' could not be found (are you missing a using directive or an assembly reference?)
        public ElementSet myElements { get; set; }
#pragma warning restore CS0246 // The type or namespace name 'ElementSet' could not be found (are you missing a using directive or an assembly reference?)

        public DataTable myDataTable { get; set; }

#pragma warning disable CS0246 // The type or namespace name 'FamilySymbol' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'BuiltInCategory' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'Document' could not be found (are you missing a using directive or an assembly reference?)
        public static Dictionary<string, List<FamilySymbol>> FindFamilyTypes(Document doc, BuiltInCategory cat, bool IsDetailItem)
#pragma warning restore CS0246 // The type or namespace name 'Document' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'BuiltInCategory' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'FamilySymbol' could not be found (are you missing a using directive or an assembly reference?)
        {

            if (!IsDetailItem)
            {
                return new FilteredElementCollector(doc)
                                .WherePasses(new ElementClassFilter(typeof(FamilySymbol)))
                                .WherePasses(new ElementCategoryFilter(cat))
                                .Cast<FamilySymbol>().Where(x => x.Family.get_Parameter(BuiltInParameter.FAMILY_WORK_PLANE_BASED).AsInteger() == 1)
                                .GroupBy(e => e.Family.Name)
                                .ToDictionary(e => e.Key, e => e.ToList());
            }
            else
            {
                return new FilteredElementCollector(doc)
                                .WherePasses(new ElementClassFilter(typeof(FamilySymbol)))
                                .WherePasses(new ElementCategoryFilter(cat))
                                .Cast<FamilySymbol>()
                                .GroupBy(e => e.Family.Name)
                                .ToDictionary(e => e.Key, e => e.ToList());
            }

        }




#pragma warning disable CS0246 // The type or namespace name 'ExternalCommandData' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'ElementSet' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning disable CS0246 // The type or namespace name 'Result' could not be found (are you missing a using directive or an assembly reference?)
        public Result StartMethod_0110(ExternalCommandData cd, ref string message, ElementSet elements)
#pragma warning restore CS0246 // The type or namespace name 'Result' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'ElementSet' could not be found (are you missing a using directive or an assembly reference?)
#pragma warning restore CS0246 // The type or namespace name 'ExternalCommandData' could not be found (are you missing a using directive or an assembly reference?)
        {
            ExternalCommandData commandData = cd;
            string executionLocation = message;

            ToAvoidLoadingRevitDLLs toavoidloadingrevitdlls = new ToAvoidLoadingRevitDLLs();
            toavoidloadingrevitdlls.commandData = commandData;
            toavoidloadingrevitdlls.executionLocation = executionLocation;

            myCommandData = commandData;
            messageConst = message;

            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;


            myTextNoteType = _937_PRLoogle_Command02._937_PRLoogle_Command02_createArcs.myTextNoteType_2031(uidoc, messageTextType);

            //List<string> cities = new FilteredElementCollector(doc).OfClass(typeof(Family)).Cast<Family>().Select(x => x.FamilyCategory.Name).Where(x => x.Substring(x.Length - 4) != "Tags").Distinct().ToList();
            //List<string> cities = new FilteredElementCollector(doc).OfClass(typeof(Family)).Cast<Family>().Where(x => x.FamilyPlacementType == FamilyPlacementType.ViewBased).Select(x => x.FamilyCategory.Name).Distinct().OrderBy(x => x).ToList();
            //List<string> cities = new FilteredElementCollector(doc).OfClass(typeof(Family)).Cast<Family>().Where(x => x.FamilyPlacementType == FamilyPlacementType.WorkPlaneBased).Select(x => x.FamilyCategory.Name).Distinct().OrderBy(x => x).ToList();
            //List<string> cities = new FilteredElementCollector(doc).OfClass(typeof(Family)).Cast<Family>().Where(x => x.FamilyPlacementType == FamilyPlacementType.OneLevelBased).Select(x => x.FamilyCategory.Name).Distinct().OrderBy(x => x).ToList();
            List<string> cities = new FilteredElementCollector(doc).OfClass(typeof(Family)).Cast<Family>().Where(x => x.FamilyPlacementType == FamilyPlacementType.Adaptive).Select(x => x.FamilyCategory.Name).Distinct().OrderBy(x => x).ToList();


            //FamilyPlacementType.Adaptive
            //FamilyPlacementType.CurveBased
            //FamilyPlacementType.CurveBasedDetail
            //FamilyPlacementType.CurveDrivenStructural
            //FamilyPlacementType.Invalid
            //FamilyPlacementType.OneLevelBased
            //FamilyPlacementType.OneLevelBasedHosted
            //FamilyPlacementType.TwoLevelsBased //these are for columns
            //FamilyPlacementType.ViewBased
            //FamilyPlacementType.WorkPlaneBased


            cities.Insert(0, Category.GetCategory(doc, BuiltInCategory.OST_DetailComponents).Name);

            DataTable table = new DataTable();
            table.Columns.Add("Name", typeof(string));
            foreach (string str in cities)
            {
                DataRow row = table.NewRow();
                row["Name"] = str;
                table.Rows.Add(row);
            }

            _937_PRLoogle_Command02.Window01_ChooseYourCategory myWindow01 = new _937_PRLoogle_Command02.Window01_ChooseYourCategory();

            myWindow01.myCategoriesToListView = table;
            myWindow01.myThisApplication = this;

            myWindow01.Show();

            return Result.Succeeded;
        }
    }
}



