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

        public ExternalCommandData myCommandData { get; set; }
        public string myMessage { get; set; }
        public ElementSet myElements { get; set; }

        public DataTable myDataTable { get; set; }

        public static Dictionary<string, List<FamilySymbol>> FindFamilyTypes(Document doc, BuiltInCategory cat, bool IsDetailItem)
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




        public Result StartMethod_0110(ExternalCommandData cd, ref string message, ElementSet elements)
        {
            ExternalCommandData commandData = cd;
            string executionLocation = message;

            ToAvoidLoadingRevitDLLs toavoidloadingrevitdlls = new ToAvoidLoadingRevitDLLs();
            toavoidloadingrevitdlls.commandData = commandData;
            toavoidloadingrevitdlls.executionLocation = executionLocation;


            messageConst = message;

            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;


            myTextNoteType = _937_PRLoogle_Command02._937_PRLoogle_Command02_createArcs.myTextNoteType_2031(uidoc, messageTextType);

            List<string> cities = new FilteredElementCollector(doc).OfClass(typeof(Family)).Cast<Family>().Select(x => x.FamilyCategory.Name).Where(x => x.Substring(x.Length - 4) != "Tags").Distinct().ToList();

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



