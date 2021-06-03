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

namespace pkRevitMisc.CommandsWithWindows.Schedule_Manual_Sort_Order
{

    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class EE2333_AddSortIndextoSchedule : IExternalEventHandler  //this is the last when one making a checklist change, EE4 must be just for when an element is new
    {
       // public MainWindow myWindow1 { get; set; }


        public static void RawCreateProjectParameter(Autodesk.Revit.ApplicationServices.Application app, string name, ParameterType type, bool visible, CategorySet cats, BuiltInParameterGroup group, bool inst)
        {
            string oriFile = app.SharedParametersFilename;
            string tempFile = Path.GetTempFileName() + ".txt";
            using (File.Create(tempFile)) { }
            app.SharedParametersFilename = tempFile;

            var defOptions = new ExternalDefinitionCreationOptions(name, type)
            {
                Visible = visible
            };
            ExternalDefinition def = app.OpenSharedParameterFile().Groups.Create("TemporaryDefintionGroup").Definitions.Create(defOptions) as ExternalDefinition;

            app.SharedParametersFilename = oriFile;
            File.Delete(tempFile);

            Autodesk.Revit.DB.Binding binding = app.Create.NewTypeBinding(cats);

            if (inst) binding = app.Create.NewInstanceBinding(cats);

            BindingMap map = (new UIApplication(app)).ActiveUIDocument.Document.ParameterBindings;
            if (!map.Insert(def, binding, group))
            {
                Trace.WriteLine($"Failed to create Project parameter '{name}' :(");
            }
        }

        public void Execute(UIApplication uiapp)
        {
            int eL = -1;



            try
            {

                UIDocument uidoc = uiapp.ActiveUIDocument;
                Document doc = uidoc.Document; // myListView_ALL_Fam_Master.Items.Add(doc.GetElement(uidoc.Selection.GetElementIds().First()).Name);

                Window2333_SortOrder window2333 = new Window2333_SortOrder();


                View view_ActiveView = uidoc.ActiveView;
                ViewSchedule viewschedule = null;

                if (view_ActiveView.GetType().Name != "ViewSchedule")
                {
                    if (uidoc.Selection.GetElementIds().Count != 0)
                    {
                        Element element_HopefullySchedule = doc.GetElement(uidoc.Selection.GetElementIds().First());

                        if (element_HopefullySchedule.GetType() == typeof(ScheduleSheetInstance))
                        { 
                            ScheduleSheetInstance scheduleSheetInstance = element_HopefullySchedule as ScheduleSheetInstance;

                            viewschedule = doc.GetElement(scheduleSheetInstance.ScheduleId) as ViewSchedule;
                        }
                    }
                }
                else
                {
                    viewschedule = view_ActiveView as ViewSchedule;
                }

                if (viewschedule == null)
                {
                    MessageBox.Show("The current view (or selection) is not a 'ViewSchedule'");
                    return;
                }

                ScheduleDefinition definition = viewschedule.Definition;
                ElementId catSched = viewschedule.Definition.CategoryId;

                Category c = doc.Settings.Categories.get_Item((BuiltInCategory)catSched.IntegerValue);

                CategorySet categoryset = new CategorySet();
                categoryset.Insert(c);

                string str_NewParameter = "SortOrder_" + viewschedule.Id.IntegerValue;

                TypeBinding typebinding = null;

                BindingMap map = doc.ParameterBindings;
                DefinitionBindingMapIterator it = map.ForwardIterator();
                it.Reset();
                while (it.MoveNext())
                {
                    if (it.Key.Name == str_NewParameter)
                    {
                        typebinding = it.Current as TypeBinding;
                    }
                }

                if (typebinding == null)
                {
                    using (Transaction tx = new Transaction(doc))
                    {
                        tx.Start("RawCreateProjectParameter");
                        RawCreateProjectParameter(uiapp.Application, str_NewParameter, ParameterType.Integer, true, categoryset, BuiltInParameterGroup.PG_IDENTITY_DATA, false);
                        tx.Commit();
                    }
                }

                SchedulableField schedulablefield = viewschedule.Definition.GetSchedulableFields().Where(f => f.GetName(doc) == str_NewParameter).FirstOrDefault();

                bool fieldAlreadyAdded = false;
                IList<ScheduleFieldId> ids = viewschedule.Definition.GetFieldOrder();
                foreach (ScheduleFieldId id in ids)
                {
                    if (viewschedule.Definition.GetField(id).GetSchedulableField() == schedulablefield)
                    {
                        fieldAlreadyAdded = true;
                        break;
                    }
                }
                eL = 146;

                List<elementWithSortOrder> list_elementWithSortOrder = new List<elementWithSortOrder>();

                if (true)  //candidate for methodisation 202010261332
                {
                    List<Element> list_Element = new FilteredElementCollector(doc, viewschedule.Id).Cast<Element>().Select(x => doc.GetElement(x.GetTypeId())).ToList().GroupBy(x => x.Id.IntegerValue).Select(g => g.First()).ToList();

                    if (list_Element.Count != 0)
                    {
                        if (false)//candidate exception 
                        {
                        }
                        else
                        {
                            list_Element = list_Element.OrderBy(y => y.LookupParameter(str_NewParameter).AsInteger()/*,  new SemiNumericComparer()*/).ToList();
                        }
                    }
                    eL = 164;
                    if (true) //candidate exception 
                    {
                        if (fieldAlreadyAdded == false)
                        {
                            using (Transaction tx = new Transaction(doc))
                            {
                                tx.Start("AddSortGroupField");
                                eL = 172;
                                ////if (list_Element.First().LookupParameter("Type Comments").AsString() != null)
                                ////{
                                ////    eL = 175;
                                ////    int maxlen = list_Element.Max(x => x.get_Parameter(BuiltInParameter.ALL_MODEL_TYPE_COMMENTS).AsString().Length);
                                ////    eL = 177;
                                ////    list_Element = list_Element.OrderBy(y => y.get_Parameter(BuiltInParameter.ALL_MODEL_TYPE_COMMENTS).AsString().PadLeft(maxlen, '0')/*,  new SemiNumericComparer()*/).ToList();
                                ////}
                                eL = 178;
                                ScheduleField schedulableField = viewschedule.Definition.AddField(schedulablefield);
                                viewschedule.Definition.ClearSortGroupFields();
                                ScheduleSortGroupField schsortgroupfield = new ScheduleSortGroupField();
                                eL = 182;
                                schsortgroupfield.FieldId = schedulableField.FieldId;
                                viewschedule.Definition.AddSortGroupField(schsortgroupfield);

                                int int_CountUp = 0;
                                foreach (Element ele in list_Element)
                                {
                                    ele.GetParameters(str_NewParameter)[0].Set(int_CountUp);
                                    int_CountUp++;
                                }

                                tx.Commit();
                            }
                        }
                    }
                    eL = 197;
                    if (list_Element.GroupBy(x => x.LookupParameter(str_NewParameter).AsInteger()).Where(g => g.Count() > 1).Count() != 0)
                    {
                        MessageBoxResult result = System.Windows.MessageBox.Show("There are double up, do you wish to recount?", "Warning double ups", System.Windows.MessageBoxButton.YesNoCancel);

                        if (result == MessageBoxResult.Yes)
                        {
                            using (Transaction tx = new Transaction(doc))
                            {
                                tx.Start("AddSortGroupField");

                                //int maxlen = list_Element.Max(x => x.get_Parameter(BuiltInParameter.ALL_MODEL_TYPE_COMMENTS).AsString().Length);
                                //list_Element = list_Element.OrderBy(y => y.get_Parameter(BuiltInParameter.ALL_MODEL_TYPE_COMMENTS).AsString().PadLeft(maxlen, '0')/*,  new SemiNumericComparer()*/).ToList();

                                int int_CountUp = 0;
                                foreach (Element ele in list_Element)
                                {
                                    ele.GetParameters(str_NewParameter)[0].Set(int_CountUp);
                                    int_CountUp++;
                                }

                                tx.Commit();
                            }
                        }
                    }

                    list_elementWithSortOrder = list_Element.Select(x => new elementWithSortOrder() { element = x, sortorder = x.LookupParameter(str_NewParameter).AsInteger() }).ToList();
                }
                eL = 225;

                window2333.lv_ReorderThis.ItemsSource = list_elementWithSortOrder;
                window2333.Topmost = true;
                window2333.str_NewParameter = str_NewParameter;
                window2333.viewschedule = viewschedule;
                window2333.uiapp = uiapp;


                window2333.myWindow2333_SortOrder_ExternalEvent_Up = new Window2333_SortOrder_ExternalEvent_Up();
                window2333.myWindow2333_SortOrder_ExternalEvent_Up.myWindow2 = window2333;
                window2333.myExternalEvent_Up = ExternalEvent.Create(window2333.myWindow2333_SortOrder_ExternalEvent_Up);

                window2333.myWindow2333_SortOrder_ExternalEvent_Down = new Window2333_SortOrder_ExternalEvent_Down();
                window2333.myWindow2333_SortOrder_ExternalEvent_Down.myWindow2 = window2333;
                window2333.myExternalEvent_Down = ExternalEvent.Create(window2333.myWindow2333_SortOrder_ExternalEvent_Down);

                window2333.Show();
            }

            #region catch and finally
            catch (Exception ex)
            {
                //_952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("EE20_AddSortIndextoSchedule" + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("EE20_AddSortIndextoSchedule, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
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

        public partial class elementWithSortOrder
        {
            public int sortorder { get; set; }
            public Element element { get; set; }

        }
    }
}
