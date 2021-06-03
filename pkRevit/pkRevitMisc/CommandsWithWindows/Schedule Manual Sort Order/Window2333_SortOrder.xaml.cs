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
using elementWithSortOrder = pkRevitMisc.CommandsWithWindows.Schedule_Manual_Sort_Order.EE2333_AddSortIndextoSchedule.elementWithSortOrder;
using System.Windows;
using MessageBox = System.Windows.Forms.MessageBox;
using System.Windows.Data;

namespace pkRevitMisc.CommandsWithWindows.Schedule_Manual_Sort_Order
{
    public class StringToBoolConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string str_typeComment = ((Element)value).get_Parameter(BuiltInParameter.ALL_MODEL_TYPE_COMMENTS)?.AsString();
            string str_FamilyInstance = ((Element)value).get_Parameter(BuiltInParameter.ELEM_FAMILY_PARAM)?.AsValueString();
            string str_FamilySymbol = ((Element)value).get_Parameter(BuiltInParameter.ALL_MODEL_FAMILY_NAME)?.AsString();

            return "(" + str_typeComment + ") " + ((Element)value).Name + ", " + str_FamilyInstance + str_FamilySymbol;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window2333_SortOrder : Window
    {
        public ToAvoidLoadingRevitDLLs toavoidloadingrevitdlls { get; set; }
        //////public Entry_0170_pkRevitMisc myWindow1 { get; set; }


        public Window2333_SortOrder_ExternalEvent_Up myWindow2333_SortOrder_ExternalEvent_Up { get; set; }
        public ExternalEvent myExternalEvent_Up { get; set; }

        public Window2333_SortOrder_ExternalEvent_Down myWindow2333_SortOrder_ExternalEvent_Down { get; set; }
        public ExternalEvent myExternalEvent_Down { get; set; }

        
        public Window2333_SortOrder()
        {
            InitializeComponent();
        }
        

        public string str_NewParameter { get; set; }
        public UIApplication uiapp { get; set; }

        public ViewSchedule viewschedule { get; set; }

        int eL = -1;

        private void reFetchList(Document doc)
        {
            Window2333_SortOrder window2333 = new Window2333_SortOrder();
            List<elementWithSortOrder> list_elementWithSortOrder = new List<elementWithSortOrder>();

            if (true) //candidate for methodisation 202010261332
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

                window2333.Topmost = true;

                list_elementWithSortOrder = list_Element.Select(x => new elementWithSortOrder() { element = x, sortorder = x.LookupParameter(str_NewParameter).AsInteger() }).ToList();
            }

            lv_ReorderThis.ItemsSource = list_elementWithSortOrder;
        }

        private bool MovingDBInteration(int intDestinationRow, int EffectiveSelectiveIndex)
        {

            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document; // myListView_ALL_Fam_Master.Items.Add(doc.GetElement(uidoc.Selection.GetElementIds().First()).Name);

            elementWithSortOrder classCurrent = lv_ReorderThis.Items[EffectiveSelectiveIndex] as elementWithSortOrder;
            elementWithSortOrder classOther = lv_ReorderThis.Items[intDestinationRow] as elementWithSortOrder;

            using (Transaction y = new Transaction(doc, "MovingDBInteration"))
            {
                y.Start();
                classCurrent.element.LookupParameter(str_NewParameter).Set(classOther.sortorder);
                classOther.element.LookupParameter(str_NewParameter).Set(classCurrent.sortorder);
                y.Commit();
            }
            reFetchList(doc);

            return true;
        }


        private void MovingDBInterationMultiple(List<int> ListIntSelectedItems, int EffectiveSelectiveIndex, int PlusOrNegative)
        {
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document; // myListView_ALL_Fam_Master.Items.Add(doc.GetElement(uidoc.Selection.GetElementIds().First()).Name);

            using (Transaction y = new Transaction(doc, "MovingDBInterationMultiple"))
            {
                y.Start();

                elementWithSortOrder StoreRangeTop = lv_ReorderThis.Items[ListIntSelectedItems.First()] as elementWithSortOrder;
                elementWithSortOrder StoreRangeBottom = lv_ReorderThis.Items[ListIntSelectedItems.Last()] as elementWithSortOrder;
                elementWithSortOrder classOther = lv_ReorderThis.Items[EffectiveSelectiveIndex + (PlusOrNegative * -1)] as elementWithSortOrder;

                classOther.element.LookupParameter(str_NewParameter).Set(2147483647);

                int lowerNumber = 0;
                int upperNumber = 0;
                if (StoreRangeTop.sortorder > StoreRangeBottom.sortorder)  //this is probably alwasy true or false
                {
                    lowerNumber = StoreRangeBottom.sortorder;
                    upperNumber = StoreRangeTop.sortorder;
                }
                else
                {
                    lowerNumber = StoreRangeTop.sortorder;
                    upperNumber = StoreRangeBottom.sortorder;
                }

                for (int i = lowerNumber; i <= upperNumber; i++)
                {
                    elementWithSortOrder loopAround = lv_ReorderThis.Items[i] as elementWithSortOrder;

                    loopAround.element.LookupParameter(str_NewParameter).Set(-PlusOrNegative + ((elementWithSortOrder)lv_ReorderThis.Items[i]).sortorder);
                }

                Int64 Goto_TopNegative_OR_BottomPositive = (PlusOrNegative == -1) ? StoreRangeTop.sortorder : StoreRangeBottom.sortorder;
                classOther.element.LookupParameter(str_NewParameter).Set(Goto_TopNegative_OR_BottomPositive);

                y.Commit();
            }

            reFetchList(doc);

            for (int i = 0; i < ListIntSelectedItems.Count; ++i)
            {
                ListIntSelectedItems[i] = ListIntSelectedItems[i] + (PlusOrNegative * -1); //here is a variant
            }

            if (ListIntSelectedItems != null)
            {
                for (int i = 0; i < ListIntSelectedItems.Count(); i++)
                {
                    lv_ReorderThis.SelectedItems.Add(lv_ReorderThis.Items[ListIntSelectedItems[i]]);
                }
            }
            lv_ReorderThis.Focus();
        }

        public void Up()
        {
            if (lv_ReorderThis.SelectedItems.Count > 1)
            {
                List<int> ListIntSelectedItems = new List<int>();
                foreach (var lvi in lv_ReorderThis.SelectedItems) ListIntSelectedItems.Add(lv_ReorderThis.Items.IndexOf(lvi));
                ListIntSelectedItems = ListIntSelectedItems.OrderBy(x => x).ToList();

                int EffectiveSelectiveIndex = ListIntSelectedItems.First();  //here is a variant
                if (EffectiveSelectiveIndex == 0) return;  //here is an variant

                MovingDBInterationMultiple(ListIntSelectedItems, EffectiveSelectiveIndex, 1);
            }
            else  //remember this has to happen in two places
            {
                if (lv_ReorderThis.SelectedIndex == 0) return;
                int intDestinationRow = lv_ReorderThis.SelectedIndex - 1;
                if (MovingDBInteration(intDestinationRow, lv_ReorderThis.SelectedIndex))
                {
                    lv_ReorderThis.SelectedIndex = intDestinationRow;
                }
            }
        }

        private void buttonMoveUp_Click(object sender, System.Windows.RoutedEventArgs e)  //Goes to Bottom, positive everything else.
        {
            try
            {
                myExternalEvent_Up.Raise();
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("buttonMoveUp_Click, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        public void Down()
        {
            if (lv_ReorderThis.SelectedItems.Count > 1)
            {
                List<int> ListIntSelectedItems = new List<int>();
                foreach (var lvi in lv_ReorderThis.SelectedItems) ListIntSelectedItems.Add(lv_ReorderThis.Items.IndexOf(lvi));
                ListIntSelectedItems = ListIntSelectedItems.OrderBy(x => x).ToList();

                int EffectiveSelectiveIndex = ListIntSelectedItems.Last();
                if (EffectiveSelectiveIndex == lv_ReorderThis.Items.Count - 1) return;  //here is an variant

                MovingDBInterationMultiple(ListIntSelectedItems, EffectiveSelectiveIndex, -1);
            }
            else
            {
                if (lv_ReorderThis.SelectedIndex == lv_ReorderThis.Items.Count - 1) return;
                int intDestinationRow = lv_ReorderThis.SelectedIndex + 1;

                if (MovingDBInteration(intDestinationRow, lv_ReorderThis.SelectedIndex))
                {
                    lv_ReorderThis.SelectedIndex = intDestinationRow;
                }
            }
        }


        private void buttonMoveDown_Click(object sender, System.Windows.RoutedEventArgs e)  //Goes to Top, negative everything else.
        {
            try
            {
                myExternalEvent_Down.Raise();
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("buttonMoveDown_Click, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }
    }



    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class Window2333_SortOrder_ExternalEvent_Up : IExternalEventHandler  //this is the last when one making a checklist change, EE4 must be just for when an element is new
    {
        public Window2333_SortOrder myWindow2 { get; set; }

        public void Execute(UIApplication uiapp)
        {
            try
            {
                myWindow2.Up();
            }
            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("Window2333_SortOrder_ExternalEvent_Up" + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }
        public string GetName()
        {
            return "Window2333_SortOrder_ExternalEvent_Up";
        }
    }


    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class Window2333_SortOrder_ExternalEvent_Down : IExternalEventHandler  //this is the last when one making a checklist change, EE4 must be just for when an element is new
    {
        public Window2333_SortOrder myWindow2 { get; set; }

        public void Execute(UIApplication uiapp)
        {
            try
            {
                myWindow2.Down();
            }
            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("Window2333_SortOrder_ExternalEvent_Down" + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }
        public string GetName()
        {
            return "Window2333_SortOrder_ExternalEvent_Down";
        }
    }
}
