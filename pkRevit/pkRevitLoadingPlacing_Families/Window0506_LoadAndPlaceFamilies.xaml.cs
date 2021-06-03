extern alias global3;

using global3.Autodesk.Revit.DB;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
using _952_PRLoogleClassLibrary;
//using Autodesk.Revit.DB;
using global3.Autodesk.Revit.DB.Events;
using Autodesk.Revit.UI;

namespace pkRevitLoadingPlacing_Families
{
    /// <summary>
    /// Interaction logic for Window3.xaml
    /// </summary>
    /// 

    public partial class Window0506_LoadAndPlaceFamilies : Window
    {
        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

        public EntryPoints.Entry_0210_pkRevitLoadingPlacing_Families myThisApplication { get; set; }
        public ExternalCommandData commandData { get; set; }

        public List<ListView_Class> myListClass { get; set; } = new List<ListView_Class>();

        public class ListView_Class
        {
            public string String_Name { get; set; }
            public string String_FileName { get; set; }
        }

        
        public EE05_LoadAllFamilies myEE05_LoadAllFamilies { get; set; }
        public ExternalEvent myExternalEvent_EE05_LoadAllFamilies { get; set; }

        public EE06_PlaceAFamily_OnDoubleClick myEE06_PlaceAFamily_OnDoubleClick { get; set; }
        public ExternalEvent myExternalEvent_EE06_PlaceAFamily_OnDoubleClick { get; set; }

        public Window0506_LoadAndPlaceFamilies(ExternalCommandData cD)
        {
            commandData = cD;

            int eL = -1;

            try
            {
                foreach (string myStrrr in EntryPoints.Families_ThatMustBeLoaded.ListStringMustHaveFamilies) myListClass.Add(new ListView_Class() { String_Name = myStrrr, String_FileName = "//Families//" + myStrrr + ".rfa" });

                myEE05_LoadAllFamilies = new EE05_LoadAllFamilies();
                myEE05_LoadAllFamilies.myWindow1 = this;
                myExternalEvent_EE05_LoadAllFamilies = ExternalEvent.Create(myEE05_LoadAllFamilies);

                myEE06_PlaceAFamily_OnDoubleClick = new EE06_PlaceAFamily_OnDoubleClick();
                myEE06_PlaceAFamily_OnDoubleClick.myWindow1 = this;
                myExternalEvent_EE06_PlaceAFamily_OnDoubleClick = ExternalEvent.Create(myEE06_PlaceAFamily_OnDoubleClick);

                InitializeComponent();
                this.Top = Properties.Settings.Default.Win3Top;
                this.Left = Properties.Settings.Default.Win3Top;
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("Window0506_LoadAndPlaceFamilies, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }
          
        private void ListViewItem_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            int eL = -1;

            try
            {
                UIDocument uidoc = commandData.Application.ActiveUIDocument;
                Document doc = uidoc.Document;

                ListView_Class myListView_Class = myListView.SelectedItem as ListView_Class;

                IEnumerable<Element> myIEnumerableElement = new FilteredElementCollector(doc).WherePasses(new ElementClassFilter(typeof(Family))).Where(x => x.Name == myListView_Class.String_Name);


                Dispatcher.Invoke(async () =>
                {
                    if (myIEnumerableElement.Count() == 0)
                    {
                        myEE05_LoadAllFamilies.string_JustLoadThisOne = myListView_Class.String_Name;
                        myExternalEvent_EE05_LoadAllFamilies.Raise();


                        myEE05_LoadAllFamilies.bool_Loop_UntilFinished = true;

                        while (myEE05_LoadAllFamilies.bool_Loop_UntilFinished)
                        {
                            await Task.Delay(100);
                        }

                        myIEnumerableElement = new FilteredElementCollector(doc).WherePasses(new ElementClassFilter(typeof(Family))).Where(x => x.Name == myListView_Class.String_Name);
                    }
                    FamilySymbol myFamilySymbol_Carrier = doc.GetElement(((Family)myIEnumerableElement.First()).GetFamilySymbolIds().First()) as FamilySymbol;

                    SetForegroundWindow(uidoc.Application.MainWindowHandle); //this is an excape event
                    keybd_event(0x1B, 0, 0, 0);
                    keybd_event(0x1B, 0, 2, 0);

                    myEE06_PlaceAFamily_OnDoubleClick.myFamilySymbol = myFamilySymbol_Carrier;
                    myExternalEvent_EE06_PlaceAFamily_OnDoubleClick.Raise();
                });


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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            int eL = -1;

            try
            {
                
                myListView.ItemsSource = myListClass;
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("Window_Loaded, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
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
                Properties.Settings.Default.Win3Top = this.Top;
                Properties.Settings.Default.Win3Left = this.Left;
                Properties.Settings.Default.Save();

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

        private void myButton_LoadAllFamilies_Click(object sender, RoutedEventArgs e)
        {
            int eL = -1;

            try
            {
                MessageBoxResult result = System.Windows.MessageBox.Show("Loading all families will take approx 5 min?", "Continue?", System.Windows.MessageBoxButton.YesNoCancel);

                if (result != MessageBoxResult.Yes)
                {
                    return;
                }

                //string_JustLoadThisOne

                myEE05_LoadAllFamilies.string_JustLoadThisOne = "";
                myExternalEvent_EE05_LoadAllFamilies.Raise();
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("myButton_LoadAllFamilies_Click, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }
    }
}
