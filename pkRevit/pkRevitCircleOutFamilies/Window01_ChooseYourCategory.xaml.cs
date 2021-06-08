
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
using System.Collections.ObjectModel;
using System.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using Autodesk.Revit.DB;
#pragma warning restore CS0246 // The type or namespace name 'Autodesk' could not be found (are you missing a using directive or an assembly reference?)

using Autodesk.Revit.DB.Electrical;
#pragma warning restore CS0246 // The type or namespace name 'Autodesk' could not be found (are you missing a using directive or an assembly reference?)

using Autodesk.Revit.DB.Mechanical; //global2019.
#pragma warning restore CS0246 // The type or namespace name 'Autodesk' could not be found (are you missing a using directive or an assembly reference?)

using Autodesk.Revit.DB.Plumbing;
#pragma warning restore CS0246 // The type or namespace name 'Autodesk' could not be found (are you missing a using directive or an assembly reference?)

using Autodesk.Revit.UI;
#pragma warning restore CS0246 // The type or namespace name 'Autodesk' could not be found (are you missing a using directive or an assembly reference?)

using Autodesk.Revit.UI.Selection;
#pragma warning restore CS0246 // The type or namespace name 'Autodesk' could not be found (are you missing a using directive or an assembly reference?)

namespace _937_PRLoogle_Command02
{
    /// <summary>
    /// Interaction logic for Window01_ChooseYourCategory.xaml
    /// </summary>
    /// 




    public enum FAMILY_PLACEMENT_TYPE
    {
#pragma warning disable CS0103 // The name 'FamilyPlacementType' does not exist in the current context
        Adaptive = FamilyPlacementType.Adaptive,
#pragma warning restore CS0103 // The name 'FamilyPlacementType' does not exist in the current context
#pragma warning disable CS0103 // The name 'FamilyPlacementType' does not exist in the current context
        CurveBased = FamilyPlacementType.CurveBased,
#pragma warning restore CS0103 // The name 'FamilyPlacementType' does not exist in the current context
#pragma warning disable CS0103 // The name 'FamilyPlacementType' does not exist in the current context
        CurveBasedDetail = FamilyPlacementType.CurveBasedDetail,
#pragma warning restore CS0103 // The name 'FamilyPlacementType' does not exist in the current context
#pragma warning disable CS0103 // The name 'FamilyPlacementType' does not exist in the current context
        CurveDrivenStructural = FamilyPlacementType.CurveDrivenStructural,
#pragma warning restore CS0103 // The name 'FamilyPlacementType' does not exist in the current context
#pragma warning disable CS0103 // The name 'FamilyPlacementType' does not exist in the current context
        Invalid = FamilyPlacementType.Invalid,
#pragma warning restore CS0103 // The name 'FamilyPlacementType' does not exist in the current context
#pragma warning disable CS0103 // The name 'FamilyPlacementType' does not exist in the current context
        OneLevelBased = FamilyPlacementType.OneLevelBased,
#pragma warning restore CS0103 // The name 'FamilyPlacementType' does not exist in the current context
#pragma warning disable CS0103 // The name 'FamilyPlacementType' does not exist in the current context
        OneLevelBasedHosted = FamilyPlacementType.OneLevelBasedHosted,
#pragma warning restore CS0103 // The name 'FamilyPlacementType' does not exist in the current context
#pragma warning disable CS0103 // The name 'FamilyPlacementType' does not exist in the current context
        TwoLevelsBased = FamilyPlacementType.TwoLevelsBased, //these are for columns
#pragma warning restore CS0103 // The name 'FamilyPlacementType' does not exist in the current context
#pragma warning disable CS0103 // The name 'FamilyPlacementType' does not exist in the current context
        ViewBased = FamilyPlacementType.ViewBased,
#pragma warning restore CS0103 // The name 'FamilyPlacementType' does not exist in the current context
#pragma warning disable CS0103 // The name 'FamilyPlacementType' does not exist in the current context
        WorkPlaneBased = FamilyPlacementType.WorkPlaneBased
#pragma warning restore CS0103 // The name 'FamilyPlacementType' does not exist in the current context
    }

    public partial class Window01_ChooseYourCategory : Window
    {
        public DataTable myCategoriesToListView { get; set; }
        public pkRevitCircleOutFamilies.EntryPoints.Entry_0110_pkRevitCircleOutFamilies myThisApplication { get; set; }

        _937_PRLoogle_Command02_EE01 my_937_PRLoogle_Command02_EE01;
        ExternalEvent my_937_PRLoogle_Command02_EE01_Action;



        public Window01_ChooseYourCategory()
        {
            InitializeComponent();

            try
            {
                this.Top = pkRevitCircleOutFamilies.Properties.Settings.Default.Top;
                this.Left = pkRevitCircleOutFamilies.Properties.Settings.Default.Left;
                //pkRevitCircleOutFamilies.Properties.Settings.Default.Top = this.Top;

                my_937_PRLoogle_Command02_EE01 = new _937_PRLoogle_Command02_EE01();
                my_937_PRLoogle_Command02_EE01.myWindow01_ChooseYourCategory = this;
                my_937_PRLoogle_Command02_EE01.bool_CircleOutTypeOption = false;
                my_937_PRLoogle_Command02_EE01.bool_JustDislayInformation = false;
                my_937_PRLoogle_Command02_EE01_Action = ExternalEvent.Create(my_937_PRLoogle_Command02_EE01);
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug(ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        private void ListViewItem_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)  //this is where we are injecting of course, this is fine
        {
            int eL = -1;

            try
            {
                if (myListView_2017.SelectedItems.Count != 1) return;

                eL = 68;
                string myString_1809 = (string)((DataRowView)myListView_2017.SelectedItem)["Name"];

                eL = 74;

                if(pkRevitCircleOutFamilies.Properties.Settings.Default.myStringCollection == null)
                {
                    pkRevitCircleOutFamilies.Properties.Settings.Default.myStringCollection = new System.Collections.Specialized.StringCollection();
                }

                pkRevitCircleOutFamilies.Properties.Settings.Default.myStringCollection.Add(myString_1809);
                eL = 77;
                pkRevitCircleOutFamilies.Properties.Settings.Default.Save();
                eL = 79;
                myListView_1811.Items.Refresh();

                eL = 80;

                //e.Handled = true;
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("ListViewItem_PreviewMouseDoubleClick, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion

        }

        private void ListViewItem_PreviewMouseDoubleClick_1811(object sender, MouseButtonEventArgs e)  //this is where we are injecting of course, this is fine
        {
            try
            {
                if (myListView_1811.SelectedItems.Count != 1) return;
                
                string myString_1811 = ((string)myListView_1811.SelectedItem);

                // MessageBox.Show(myString_1811); //myTextbox_1903 //myListView_1647

                pkRevitCircleOutFamilies.Properties.Settings.Default.myStringCollection.RemoveAt(myListView_1811.SelectedIndex);
                pkRevitCircleOutFamilies.Properties.Settings.Default.Save();
                myListView_1811.Items.Refresh();

                myListView_1647.Items.Add(myString_1811);

                e.Handled = true;
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug(ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        private void ListViewItem_MouseDown_1811(object sender, MouseButtonEventArgs e)  //this is where we are injecting of course, this is fine
        {
            try
            {
                string myString_1811 = ((string)myListView_1647.SelectedItem);

                //myListView_1647.Items.RemoveAt(myListView_1647.SelectedIndex);


                //Properties.Settings.Default.myStringCollection.Add(myString_1647);
                //Properties.Settings.Default.Save();
                //Properties.Settings.Default.Reload();
                //myListView_1811.Items.Refresh();
                

                myTextBox_2007.Text = (string)((ListViewItem)sender).Content;
                //MessageBox.Show("_1811"); //myTextbox_1903 //myListView_1647

               // e.Handled = true;
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug(ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }
        
        private void ListViewItem_PreviewMouseDoubleClick_1647(object sender, MouseButtonEventArgs e)  //this is where we are injecting of course, this is fine
        {
            try
            {
                string myString_1647 = ((string)myListView_1647.SelectedItem);

                myListView_1647.Items.RemoveAt(myListView_1647.SelectedIndex);

                pkRevitCircleOutFamilies.Properties.Settings.Default.myStringCollection.Add(myString_1647);
                pkRevitCircleOutFamilies.Properties.Settings.Default.Save();
                //Properties.Settings.Default.Reload();
                myListView_1811.ItemsSource = pkRevitCircleOutFamilies.Properties.Settings.Default.myStringCollection;
                myListView_1811.Items.Refresh();


                // MessageBox.Show("_1647"); //myTextbox_1903 //myListView_1647

                e.Handled = true;
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug(ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                string filterAccumlation = "";
                // myCategoriesToListView
                //myCategoriesToListView.DefaultView.RowFilter = 

                if (myTextbox_1903.Text != "")
                {
                    string[] stringArray = myTextbox_1903.Text.Split(' ');
                    filterAccumlation = "Name LIKE '%" + stringArray[0].Replace("'", "''") + "%'";  //here is a like arrow that is not implemented on our search string
                    //we can check that it gets to here

                    foreach (string str in myTextbox_1903.Text.Split(' ').Skip(1))
                    {
                        if (str.Trim() != "") filterAccumlation = filterAccumlation + " AND Name LIKE '%" + str + "%'";
                    }

                 //   filterAccumlation = "(" + filterAccumlation + ") " + filterAccumlation;
                } else
                {
                   /////////////////////// myCategoriesToListView.DefaultView.RowFilter = "ID = -1";
                }

                myCategoriesToListView.DefaultView.RowFilter = filterAccumlation;
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug(ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                myListView_2017.ItemsSource = myCategoriesToListView.DefaultView;
                myListView_1811.ItemsSource = pkRevitCircleOutFamilies.Properties.Settings.Default.myStringCollection;
                //  Properties.Settings.Default.myStringCollection.Add(myString_1809);


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

                UIDocument uidoc = myThisApplication.myCommandData.Application.ActiveUIDocument;
                Document doc = uidoc.Document;

                listBox.Items.Add("Adaptive (" + new FilteredElementCollector(doc).OfClass(typeof(Family)).Cast<Family>().Where(x => x.FamilyPlacementType == (FamilyPlacementType)GetEnumValue<FAMILY_PLACEMENT_TYPE>("Adaptive")).Count() + ")");
                listBox.Items.Add("CurveBased (" + new FilteredElementCollector(doc).OfClass(typeof(Family)).Cast<Family>().Where(x => x.FamilyPlacementType == (FamilyPlacementType)GetEnumValue<FAMILY_PLACEMENT_TYPE>("CurveBased")).Count() + ")");
                listBox.Items.Add("CurveBasedDetail (" + new FilteredElementCollector(doc).OfClass(typeof(Family)).Cast<Family>().Where(x => x.FamilyPlacementType == (FamilyPlacementType)GetEnumValue<FAMILY_PLACEMENT_TYPE>("CurveBasedDetail")).Count() + ")");
                listBox.Items.Add("CurveDrivenStructural (" + new FilteredElementCollector(doc).OfClass(typeof(Family)).Cast<Family>().Where(x => x.FamilyPlacementType == (FamilyPlacementType)GetEnumValue<FAMILY_PLACEMENT_TYPE>("CurveDrivenStructural")).Count() + ")");
                listBox.Items.Add("Invalid (" + new FilteredElementCollector(doc).OfClass(typeof(Family)).Cast<Family>().Where(x => x.FamilyPlacementType == (FamilyPlacementType)GetEnumValue<FAMILY_PLACEMENT_TYPE>("Invalid")).Count() + ")");
                listBox.Items.Add("OneLevelBased (" + new FilteredElementCollector(doc).OfClass(typeof(Family)).Cast<Family>().Where(x => x.FamilyPlacementType == (FamilyPlacementType)GetEnumValue<FAMILY_PLACEMENT_TYPE>("OneLevelBased")).Count() + ")");
                listBox.Items.Add("OneLevelBasedHosted (" + new FilteredElementCollector(doc).OfClass(typeof(Family)).Cast<Family>().Where(x => x.FamilyPlacementType == (FamilyPlacementType)GetEnumValue<FAMILY_PLACEMENT_TYPE>("OneLevelBasedHosted")).Count() + ")");
                listBox.Items.Add("TwoLevelsBased (" + new FilteredElementCollector(doc).OfClass(typeof(Family)).Cast<Family>().Where(x => x.FamilyPlacementType == (FamilyPlacementType)GetEnumValue<FAMILY_PLACEMENT_TYPE>("TwoLevelsBased")).Count() + ")");
                listBox.Items.Add("ViewBased (" + new FilteredElementCollector(doc).OfClass(typeof(Family)).Cast<Family>().Where(x => x.FamilyPlacementType == (FamilyPlacementType)GetEnumValue<FAMILY_PLACEMENT_TYPE>("ViewBased")).Count() + ")");
                listBox.Items.Add("WorkPlaneBased (" + new FilteredElementCollector(doc).OfClass(typeof(Family)).Cast<Family>().Where(x => x.FamilyPlacementType == (FamilyPlacementType)GetEnumValue<FAMILY_PLACEMENT_TYPE>("WorkPlaneBased")).Count() + ")");

                FocusManager.SetFocusedElement(this, myTextbox_1903);
                myTextbox_1903.SelectAll();
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug(ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }
        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                myCategoriesToListView.DefaultView.RowFilter = "";
                myTextbox_1903.Text = "";
                myTextbox_1903.Focus();
                ///////////////////////////////////myTextbox_1903.SelectAll();
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug(ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                UIDocument uidoc = myThisApplication.myCommandData.Application.ActiveUIDocument;
                Document doc = uidoc.Document;

                if(uidoc.Selection.GetElementIds().Count == 0)
                {
                    myTextBox_2022.Text = "";
                }

                my_937_PRLoogle_Command02_EE01.myStringCategory = myTextBox_2007.Text;
                my_937_PRLoogle_Command02_EE01.bool_CircleOutTypeOption = true;
                my_937_PRLoogle_Command02_EE01.bool_JustDislayInformation = true;
                my_937_PRLoogle_Command02_EE01_Action.Raise();
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug(ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }


        private void MyMakeCircles_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                UIDocument uidoc = myThisApplication.myCommandData.Application.ActiveUIDocument;
                Document doc = uidoc.Document;


                if (doc.ActiveView.ViewType != ViewType.FloorPlan & doc.ActiveView.ViewType != ViewType.EngineeringPlan & doc.ActiveView.ViewType != ViewType.CeilingPlan )
                {
                    TaskDialog.Show("Not the correct type of view", "The active view must be a view 'plan' type view");
                    return ;
                }

                if (myTextBox_2007.Text == "")
                {
                    MessageBox.Show("Please select, an item from the middle list.");

                    return;
                }
                my_937_PRLoogle_Command02_EE01.myStringCategory = myTextBox_2007.Text;
                my_937_PRLoogle_Command02_EE01.bool_CircleOutTypeOption = false;
                my_937_PRLoogle_Command02_EE01.bool_JustDislayInformation = false;
                my_937_PRLoogle_Command02_EE01_Action.Raise();
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug(ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        private void myMakeCirclesTypes_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                UIDocument uidoc = myThisApplication.myCommandData.Application.ActiveUIDocument;
                Document doc = uidoc.Document;


                if (doc.ActiveView.ViewType != ViewType.FloorPlan & doc.ActiveView.ViewType != ViewType.EngineeringPlan & doc.ActiveView.ViewType != ViewType.CeilingPlan)
                {
                    TaskDialog.Show("Not the correct type of view", "The active view must be a view 'plan' type view");
                    return;
                }

                my_937_PRLoogle_Command02_EE01.myStringCategory = myTextBox_2007.Text;
                my_937_PRLoogle_Command02_EE01.bool_CircleOutTypeOption = true;
                my_937_PRLoogle_Command02_EE01.bool_JustDislayInformation = false;
                my_937_PRLoogle_Command02_EE01_Action.Raise();
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug(ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        private void MyMakeCirclesAndClose_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                my_937_PRLoogle_Command02_EE01.myStringCategory = myTextBox_2007.Text;
                my_937_PRLoogle_Command02_EE01_Action.Raise();
                this.Close();
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug(ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }


        public T GetEnumValue<T>(string str) where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
            {
                throw new Exception("T must be an Enumeration type.");
            }
            T val = ((T[])Enum.GetValues(typeof(T)))[0];
            if (!string.IsNullOrEmpty(str))
            {
                foreach (T enumValue in (T[])Enum.GetValues(typeof(T)))
                {
                    if (enumValue.ToString().ToUpper().Equals(str.ToUpper()))
                    {
                        val = enumValue;
                        break;
                    }
                }
            }

            return val;
        }

        public string string_left { get; set; }

        private void ListBoxItem_MouseDown_1811(object sender, MouseButtonEventArgs e)  //this is where we are injecting of course, this is fine
        {
            try
            {
                string myString_1811 = ((string)listBox.SelectedItem);


                UIDocument uidoc = myThisApplication.myCommandData.Application.ActiveUIDocument;
                Document doc = uidoc.Document;

                string senderString = (string)((ListBoxItem)sender).Content;
                string_left = senderString.Substring(0, senderString.IndexOf('(') - 1);

                //myTextBox_2007.Text = string_left;
                myTextBox_2007.Text = "";
                myTextBox_2022.Text = "";

                List<string> cities = new FilteredElementCollector(doc).OfClass(typeof(Family)).Cast<Family>().Where(x => x.FamilyPlacementType == (FamilyPlacementType)GetEnumValue<FAMILY_PLACEMENT_TYPE>(string_left)  ).Select(x => x.FamilyCategory.Name).Distinct().OrderBy(x => x).ToList();

                //MessageBox.Show(cities.Count().ToString());

                if(cities.Count() != 0)
                {
                    cities.Insert(0, "ALL");
                    listBoxFilters.ItemsSource = cities;
                    listBoxFilters.SelectedIndex = 0;
                    myTextBox_2007.Text = "ALL";
                } else
                {
                    listBoxFilters.ItemsSource = null;
                }

            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug(ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }



        private void ListBoxItem_MouseDown_1812(object sender, MouseButtonEventArgs e)  //this is where we are injecting of course, this is fine
        {
            try
            {
                string myString_1811 = ((string)listBox.SelectedItem);

                UIDocument uidoc = myThisApplication.myCommandData.Application.ActiveUIDocument;
                Document doc = uidoc.Document;

                string senderString = (string)((ListBoxItem)sender).Content;

                myTextBox_2007.Text = senderString;
                //myTextBox_2022.Text = "";
                myTextBox_2022.Text = "";// famSym.Family.Name + "  (" + famSym.Family.GetFamilySymbolIds().Count() + " = type count)";
                //  MessageBox.Show("do something if it is diffferent?");
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug(ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }


        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            int eL = -1;

            try
            {
                pkRevitCircleOutFamilies.Properties.Settings.Default.Top = this.Top;
                pkRevitCircleOutFamilies.Properties.Settings.Default.Left = this.Left;
                pkRevitCircleOutFamilies.Properties.Settings.Default.Save();
                pkRevitCircleOutFamilies.Properties.Settings.Default.Reload();
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
