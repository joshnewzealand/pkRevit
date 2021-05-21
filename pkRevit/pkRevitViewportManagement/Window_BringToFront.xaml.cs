using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using pkRevitViewportManagement;

using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Globalization;

namespace pkRevitViewportManagement
{
    public class StringToBoolConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string str_typeComment = ((Element)value).get_Parameter(BuiltInParameter.VIEW_NAME)?.AsString();
            //string str_FamilyInstance = ((Element)value).get_Parameter(BuiltInParameter.ELEM_FAMILY_PARAM)?.AsValueString();
            //string str_FamilySymbol = ((Element)value).get_Parameter(BuiltInParameter.ALL_MODEL_FAMILY_NAME)?.AsString();

            // List<Parameter> pset = ((Element)value).Parameters.Cast<Parameter>().ToList();

            return str_typeComment;// "(" + str_typeComment + ") " + ((Element)value).Name + ", " + str_FamilyInstance + str_FamilySymbol;

            ////return ((ParameterSet)value).Cast<Parameter>().Where(x => x.Definition.Name == "Type Comments").First().AsString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class EE01_BringToFront : IExternalEventHandler  //this is the last when one making a checklist change, EE4 must be just for when an element is new
    {
        public Window_BringToFront myWindow1 { get; set; }

        public void Execute(UIApplication uiapp)
        {
            try
            {
                UIDocument uidoc = myWindow1.toavoidloadingrevitdlls.commandData.Application.ActiveUIDocument;
                Document doc = uidoc.Document;

                ElementId viewId = myWindow1.viewport.ViewId;
                XYZ boxCenter = myWindow1.viewport.GetBoxCenter();
                ElementId typeId = myWindow1.viewport.GetTypeId();
                //View view = doc.ActiveView;
                using (Transaction t = new Transaction(doc))
                {
                    t.Start("Delete and Recreate Viewport");
                    myWindow1.viewsheet.DeleteViewport(myWindow1.viewport);
                    Viewport vvp = Viewport.Create(doc, myWindow1.viewsheet.Id, viewId, boxCenter);
                    vvp.ChangeTypeId(typeId);
                    t.Commit();
                }

                Window_BringToFront.loadup_list(myWindow1, myWindow1.viewsheet);
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("EE01_BringToFront" + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }
        public string GetName()
        {
            return "EE01_Part1_Template";
        }
    }

        /// <summary>
    /// Interaction logic for Window_BringToFront.xaml
    /// </summary>
    public partial class Window_BringToFront : Window
    {
        public ToAvoidLoadingRevitDLLs toavoidloadingrevitdlls { get; set; }

        public List<Viewport> list_Viewports { get; set; }

        public Viewport viewport { get; set; }
        public ViewSheet viewsheet { get; set; }

        public EE01_BringToFront myEE01_BringToFront { get; set; }
        public ExternalEvent myExternalEvent_EE01_BringToFront { get; set; }


        public Window_BringToFront(ToAvoidLoadingRevitDLLs tolr)
        {
            int eL = -1;

            try
            {
                myEE01_BringToFront = new EE01_BringToFront();
                myEE01_BringToFront.myWindow1 = this;
                myExternalEvent_EE01_BringToFront = ExternalEvent.Create(myEE01_BringToFront);


                toavoidloadingrevitdlls = tolr;

                InitializeComponent();

                this.Top = Properties.Settings.Default.BringToFront_Top;
                this.Left = Properties.Settings.Default.BringToFront_Left;


                loadup_list(this, null);
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("Window_BringToFront, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        public static void loadup_list(Window_BringToFront w1, ViewSheet vs)
        {
            int eL = -1;

            try
            {
                UIDocument uidoc = w1.toavoidloadingrevitdlls.commandData.Application.ActiveUIDocument;
                Document doc = uidoc.Document;

                eL = 48;

                if (vs == null)
                {
                    if (doc.ActiveView.ViewType != ViewType.DrawingSheet)
                    {
                        MessageBox.Show("Please goto a 'Sheet' type view.");
                        return;
                    }

                    w1.viewsheet = (ViewSheet)uidoc.ActiveView;
                }


                w1.list_Viewports = new List<Viewport>();

                foreach (ElementId ViewSheet1_Viewports in w1.viewsheet.GetAllViewports())
                {
                    Viewport Viewport1 = doc.GetElement(ViewSheet1_Viewports) as Viewport;
                    w1.list_Viewports.Add(Viewport1);
                }
                eL = 57;
                w1.listview_ListofViewportOnSheet.ItemsSource = w1.list_Viewports;
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("loadup_list, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }


        private void button_Refresh_Click(object sender, RoutedEventArgs e)
        {
            int eL = -1;

            try
            {
                loadup_list(this, null);
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("button_Refresh_Click, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        private void button_BringSelectedViewportToFront_Click(object sender, RoutedEventArgs e)
        {
            int eL = -1;

            try
            {
                if (listview_ListofViewportOnSheet.SelectedItem == null)
                {
                    TaskDialog.Show("List in Item", "Please select an item in the list. Or Cancel.");
                    return;
                }

                viewport = (Viewport)listview_ListofViewportOnSheet.SelectedItem;
                myExternalEvent_EE01_BringToFront.Raise();


                //  ViewportBringToFront(viewsheet, (Viewport)listview_ListofViewportOnSheet.SelectedItem);
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("button_BringSelectedViewportToFront_Click, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }




        //////////void Button2Click(object sender, EventArgs e)
        //////////{
        //////////    if (listBox1.SelectedItem == null)
        //////////    {
        //////////        TaskDialog.Show("List in Item", "Please select an item in the list. Or Cancel.");
        //////////        return;
        //////////    }
        //////////    if (ViewSheet1 == null)
        //////////    {
        //////////        TaskDialog.Show("List in Item", "Please click List button first. Or Cancel.");
        //////////        return;
        //////////    }

        //////////    //	ElementId ViewElementID = ViewportList1[listBox1.SelectedIndex].ViewId;			
        //////////    //	XYZ storetheposition = ViewportList1[listBox1.SelectedIndex].GetBoxCenter();
        //////////    //	ElementId storetypetypeID = ViewportList1[listBox1.SelectedIndex].GetTypeId();			

        //////////    ViewportBringToFront(ViewSheet1, ViewportList1[listBox1.SelectedIndex]);

        //////////    //	Autodesk.Revit.DB.View pView = doc.ActiveView;Autodesk.Revit.DB.Transaction
        //////////    //   t = new Autodesk.Revit.DB.Transaction(doc, "Form_2");
        //////////    //    t.Start();			
        //////////    //	ViewSheet1.DeleteViewport(ViewportList1[listBox1.SelectedIndex]);			
        //////////    //	Viewport vvp = Viewport.Create(doc, ViewSheet1.Id, ViewElementID, storetheposition);
        //////////    //   t.Commit();



        //////////    clear_and_update_the_listview();

        //////////    //	TaskDialog.Show("List in the item", SelectedViewportView.get_Parameter(BuiltInParameter.VIEW_NAME).AsString());
        //////////}

        void Button3Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            int eL = -1;

            try
            {
                Properties.Settings.Default.BringToFront_Top = this.Top;
                Properties.Settings.Default.BringToFront_Left = this.Left;
                Properties.Settings.Default.Save();
                Properties.Settings.Default.Reload();
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("Window_Closing, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }
    }

}
