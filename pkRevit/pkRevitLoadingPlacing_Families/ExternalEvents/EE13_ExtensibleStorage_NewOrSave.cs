
//using Class1V2 = GridV2::Namespace.Class1;
extern alias global3;


using db3 = global3.Autodesk.Revit.DB;

using global3.Autodesk.Revit.DB;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using global3.Autodesk.Revit.DB.Events;
using System.Runtime.InteropServices;
using _952_PRLoogleClassLibrary;
using global3.Autodesk.Revit.DB.ExtensibleStorage;

//using Class1V2 = global2::Namespace.Class1;

namespace pkRevitLoadingPlacing_Families
{
    [global3.Autodesk.Revit.Attributes.Transaction(global3.Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class EE13_ExtensibleStorage_NewOrSave : IExternalEventHandler  //this is the last when one making a checklist change, EE4 must be just for when an element is new
    {
        public Window1213_ExtensibleStorage myWindow1 { get; set; }
        public bool myBool_New { get; set; }


        //saving now saving now, saving now saving now , saving now 
        public void Execute(UIApplication uiapp)
        {
            int version = int.Parse(uiapp.Application.VersionNumber);
            //ent_Child.Set("FurnLocations", dict_Child, (version < 2021) ? global4.Autodesk.Revit.DB.DisplayUnitType.DUT_MILLIMETERS : new ForgeTypeId(UnitTypeId.Millimeters.TypeId));

            if (version < 2021)
            {
                MessageBox.Show("Due to some dramatic changes to the API in 2021, extensible storage is currently not working on versions 2020 and earlier." + Environment.NewLine + Environment.NewLine + "We're really sorry...we're working on it.");
            }

            try
            {

                ///            TECHNIQUE 13 OF 19 (EE13_ExtensibleStorage_NewOrSave.cs)
                ///↓↓↓↓↓↓↓↓↓↓↓↓↓↓ SCHEMA DATA STRUCTURES (ENTITIES) TO THE ELEMENTS ↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓
                ///
                /// Interfaces and ENUM's:
                ///     
                /// 
                /// Demonstrates classes:
                ///     KeyValuePair*
                ///     DataStorage
                ///     Schema
                ///     Entity
                ///     IDictionary*
                ///     DisplayUnitType
                /// 
                /// 
                /// Key methods:
                ///     ((FamilyInstance)myEle).GetTransform().BasisX.AngleOnPlaneTo(XYZ.BasisY, XYZ.BasisZ);
                ///
                ///     ent_Parent = myDatastorage.GetEntity(schema_FurnLocations_Index);
                ///     dict_Parent = ent_Parent.Get<IDictionary<string, Entity>>(
                ///     dict_Parent.Add(
                ///     ent_Parent.Set(dict_Parent
                ///     myDatastorage.SetEntity(ent_Parent);
                ///
                ///
                /// * class is actually part of the .NET framework (not Revit API)
                /// 
                /// Schema allows data to be structured more like database tables, (lists within lists)
                /// Entity can exist in standalone DataStorage OR any other element.
                /// Unwrap to use and wrap up again to store...like a Christmas present
                ///
                ///
				///	https://github.com/joshnewzealand/Revit-API-Playpen-CSharp


                //if it is new then this value gets stored
                KeyValuePair<string, Entity> myKeyValuePair = myBool_New ? new KeyValuePair<string, Entity>() : (KeyValuePair<string, Entity>)myWindow1.myListViewEE.SelectedItem;

                UIDocument uidoc = uiapp.ActiveUIDocument;
                Document doc = uidoc.Document; // myListView_ALL_Fam_Master.Items.Add(doc.GetElement(uidoc.Selection.GetElementIds().First()).Name);

                List<ElementId> myFEC_DataStorage = new FilteredElementCollector(doc).OfClass(typeof(DataStorage)).WhereElementIsNotElementType().Where(x => x.Name == "Room Setup Entities").Select(x => x.Id).ToList();

                if (myFEC_DataStorage.Count == 0)
                {
                    MessageBox.Show("Please click the 'Place room' button before Saving");
                    return;
                }

                DataStorage myDatastorage = doc.GetElement(myFEC_DataStorage.First()) as DataStorage;
                List<Element> myFEC_RoomSetupEntities = new FilteredElementCollector(doc).WhereElementIsNotElementType().Where(x => x.LookupParameter("Comments") != null).Where(x => x.get_Parameter(BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS).AsString() == "Room Setup Entities").ToList();

                Schema schema_FurnLocations = Schema.Lookup(new Guid(Schema_FurnLocations.myConstantStringSchema_FurnLocations));
                if (schema_FurnLocations == null) schema_FurnLocations = Schema_FurnLocations.createSchema_FurnLocations();

                Entity ent_Child = myBool_New ? new Entity(schema_FurnLocations) : myKeyValuePair.Value;
                IDictionary<ElementId, XYZ> dict_Child = new Dictionary<ElementId, XYZ>();
                IDictionary<ElementId, double> dict_Child_Angle = new Dictionary<ElementId, double>();


                IDictionary<ElementId, ElementId> dict_Child_PatternID = new Dictionary<ElementId, ElementId>();
                IDictionary<ElementId, int> dict_Child_ColourRed = new Dictionary<ElementId, int>();
                IDictionary<ElementId, int> dict_Child_ColourGreen = new Dictionary<ElementId, int>();
                IDictionary<ElementId, int> dict_Child_ColourBlue = new Dictionary<ElementId, int>();


                foreach (Element myEle in myFEC_RoomSetupEntities)
                {
                    if (myEle.Location.GetType() == typeof(LocationPoint))
                    {
                        dict_Child.Add(myEle.Id, ((LocationPoint)myEle.Location).Point);

                        double myDouble = ((FamilyInstance)myEle).GetTransform().BasisX.AngleOnPlaneTo(XYZ.BasisY, XYZ.BasisZ);
                        dict_Child_Angle.Add(myEle.Id, myDouble);


                        OverrideGraphicSettings ogs = doc.ActiveView.GetElementOverrides(myEle.Id);
                        dict_Child_PatternID.Add(myEle.Id, ogs.SurfaceForegroundPatternId);

                        if (ogs.ProjectionLineColor.IsValid)
                        {
                            dict_Child_ColourRed.Add(myEle.Id, Convert.ToInt32(ogs.ProjectionLineColor.Red));
                            dict_Child_ColourGreen.Add(myEle.Id, Convert.ToInt32(ogs.ProjectionLineColor.Green));
                            dict_Child_ColourBlue.Add(myEle.Id, Convert.ToInt32(ogs.ProjectionLineColor.Blue));
                        }
                        else
                        {
                            dict_Child_ColourRed.Add(myEle.Id, 0);
                            dict_Child_ColourGreen.Add(myEle.Id, 0);
                            dict_Child_ColourBlue.Add(myEle.Id, 0);
                        }
                    }
                }

                
                


                ent_Child.Set("FurnLocations", dict_Child, new ForgeTypeId(UnitTypeId.Millimeters.TypeId));
                ent_Child.Set("FurnLocations_Angle", dict_Child_Angle, new ForgeTypeId(UnitTypeId.Millimeters.TypeId));

                ent_Child.Set("FurnLocations_Pattern", dict_Child_PatternID, new ForgeTypeId(UnitTypeId.Millimeters.TypeId));
                ent_Child.Set("FurnLocations_ColorRed", dict_Child_ColourRed, new ForgeTypeId(UnitTypeId.Millimeters.TypeId));
                ent_Child.Set("FurnLocations_ColorGreen", dict_Child_ColourGreen, new ForgeTypeId(UnitTypeId.Millimeters.TypeId));
                ent_Child.Set("FurnLocations_ColorBlue", dict_Child_ColourBlue, new ForgeTypeId(UnitTypeId.Millimeters.TypeId));

                //global2.Autodesk.Revit.DB.
                //ent_Child.Set("FurnLocations", dict_Child, new ForgeTypeId(UnitTypeId.Millimeters.TypeId));
                //ent_Child.Set("FurnLocations_Angle", dict_Child_Angle, new ForgeTypeId(UnitTypeId.Millimeters.TypeId));

                //ent_Child.Set("FurnLocations_Pattern", dict_Child_PatternID, new ForgeTypeId(UnitTypeId.Millimeters.TypeId));
                //ent_Child.Set("FurnLocations_ColorRed", dict_Child_ColourRed, new ForgeTypeId(UnitTypeId.Millimeters.TypeId));
                //ent_Child.Set("FurnLocations_ColorGreen", dict_Child_ColourGreen, new ForgeTypeId(UnitTypeId.Millimeters.TypeId));
                //ent_Child.Set("FurnLocations_ColorBlue", dict_Child_ColourBlue, new ForgeTypeId(UnitTypeId.Millimeters.TypeId));


                Schema schema_FurnLocations_Index = Schema.Lookup(new Guid(Schema_FurnLocations.myConstantStringSchema_FurnLocations_Index));
                if (schema_FurnLocations_Index == null) schema_FurnLocations_Index = Schema_FurnLocations.createSchema_FurnLocations_Index();

                Entity ent_Parent = myDatastorage.GetEntity(schema_FurnLocations_Index);
                IDictionary<string, Entity> dict_Parent = null; // new IDictionary<int, Entity>();

                if (ent_Parent.IsValid())
                {
                    dict_Parent = ent_Parent.Get<IDictionary<string, Entity>>("FurnLocations_Index", new ForgeTypeId(UnitTypeId.Millimeters.TypeId));
                }
                else
                {
                    ent_Parent = new Entity(schema_FurnLocations_Index);
                    dict_Parent = new Dictionary<string, Entity>();
                }



                if (myBool_New)
                {
                    dict_Parent.Add(DateTime.Now.ToString("yyyyMMdd HHmm ss"), ent_Child);
                }
                else
                {
                    dict_Parent[myKeyValuePair.Key] = ent_Child;
                }


                ent_Parent.Set("FurnLocations_Index", dict_Parent, new ForgeTypeId(UnitTypeId.Millimeters.TypeId));


                using (Transaction y = new Transaction(doc, "New Furniture Arrangement"))
                {
                    y.Start();

                    myDatastorage.SetEntity(ent_Parent);

                    y.Commit();
                }

                int int_SelectedIndex = myWindow1.myListViewEE.SelectedIndex;
                myWindow1.myListViewEE.ItemsSource = dict_Parent;

                if (myBool_New)
                {
                    myWindow1.myListViewEE.SelectedIndex = dict_Parent.Count - 1;
                }
                else
                {
                    myWindow1.myListViewEE.SelectedIndex = int_SelectedIndex;
                }

                Properties.Settings.Default.ExtensibleStor_LastSelectedIndex = myWindow1.myListViewEE.SelectedIndex;
                Properties.Settings.Default.Save();
                Properties.Settings.Default.Reload();
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("EE03_Part1_New" + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
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
    }
}
