
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
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.DB.Mechanical; //global2019.
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

namespace _937_PRLoogle_Command02
{
    /// <summary>
    /// Interaction logic for Window01_ChooseYourCategory.xaml
    /// </summary>
    /// 
    public partial class Window01_ChooseYourCategory : Window
    {
        public DataTable myCategoriesToListView { get; set; }
        public pkRevitCircleOutFamilies.EntryPoints.Entry_0110_pkRevitCircleOutFamilies myThisApplication { get; set; }

        _937_PRLoogle_Command02_EE01 my_937_PRLoogle_Command02_EE01;
        ExternalEvent my_937_PRLoogle_Command02_EE01_Action;

        public Window01_ChooseYourCategory()
        {
            try
            {
                InitializeComponent();

                my_937_PRLoogle_Command02_EE01 = new _937_PRLoogle_Command02_EE01();
                my_937_PRLoogle_Command02_EE01.myWindow01_ChooseYourCategory = this;
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
                //string stringDatabaseName = (string)((DataRowView)myListView_2017.SelectedItem)["TargetDatabaseName"];


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

        private void MyMakeCircles_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(myTextBox_2007.Text == "")
                {
                    MessageBox.Show("Please select, an item from the middle list.");

                    return;
                }


                my_937_PRLoogle_Command02_EE01.myStringCategory = myTextBox_2007.Text;
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


    }
}
