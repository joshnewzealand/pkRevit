using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Input;
using System.Xml.Serialization;
using static System.Windows.Forms.ListView;
using DataFormats = System.Windows.DataFormats;
using DataObject = System.Windows.DataObject;
using DragDropEffects = System.Windows.DragDropEffects;
using DragEventArgs = System.Windows.DragEventArgs;
using MessageBox = System.Windows.MessageBox;
using MouseEventArgs = System.Windows.Input.MouseEventArgs;
using pkRevitDatasheets.BuildingCoderClasses;
using System.ComponentModel;
using QuickZip.Tools;
using System.Windows.Controls;
using System.Drawing;

namespace pkRevitDatasheets
{
    //////////public class StringToBoolConverter : IValueConverter
    //////////{

    //////////    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    //////////    {
    //////////        Parameter parameter2 = (Parameter)value;

    //////////        string s_returnValue = "";

    //////////        switch (parameter2.StorageType)
    //////////        {
    //////////            case StorageType.String:
    //////////                s_returnValue = parameter2.AsString();
    //////////                break;
    //////////            case StorageType.Integer:
    //////////                s_returnValue = parameter2.AsInteger().ToString();
    //////////                break;
    //////////            case StorageType.None:
    //////////                break;
    //////////            case StorageType.ElementId:
    //////////                //return parameter2.AsString();
    //////////                s_returnValue = parameter2.AsElementId().IntegerValue.ToString();
    //////////                //MessageBox.Show(parameterImageType.AsElementId().IntegerValue.ToString());
    //////////                break;

    //////////            case StorageType.Double:
    //////////                s_returnValue = parameter2.AsDouble().ToString();
    //////////                //MessageBox.Show(parameterImageType.AsDouble().ToString());
    //////////                break;

    //////////            default:
    //////////                break;
    //////////        }

    //////////        return s_returnValue;
    //////////        //return parameter2.Definition.Name /*+ Environment.NewLine + s_returnValue*/;
    //////////        //return ((ParameterSet)value).Cast<Parameter>().Where(x => x.Definition.Name == "Type Comments").First().AsString();
    //////////    }

    //////////    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    //////////    {
    //////////        throw new NotImplementedException();
    //////////    }
    //////////}

    class ElementID_Double_Integer_String
    {
        //we could make a sublist of classes here if you wished
        //we could make a sublist of classes here if you wished
        //we could make a sublist of classes here if you wished

        //we could have one ingeter that tells it what it is
        //a straight dictionary
    }

    public class Model : INotifyPropertyChanged
    {
        string _path;
        string[] _files;
        bool _showFiles = true, _showFolders = true;
        public string Path { get { return _path; } set { _path = value; OnPropertyChanged("Path"); } }
        public string[] Files { get { return _files; } set { _files = value; OnPropertyChanged("Files"); } }
        public bool ShowFiles { get { return _showFiles; } set { _showFiles = value; OnPropertyChanged("ShowFiles"); } }
        public bool ShowFolders { get { return _showFolders; } set { _showFolders = value; OnPropertyChanged("ShowFolders"); } }

        public event PropertyChangedEventHandler PropertyChanged;
        MainWindow _view;

        public Model(MainWindow view)
        {
            _view = view;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(
                    this,
                    new PropertyChangedEventArgs(propertyName)
                    );
            }
             
            //Lazy =D
            if (propertyName == "Path" || propertyName == "ShowFiles" || propertyName == "ShowFolders")
            {
                _view.ClearCache();
                List<string> folderAndFiles = new List<string>();
                if (ShowFolders) folderAndFiles.AddRange(Directory.GetDirectories(Path).ToArray());
                if (ShowFiles) folderAndFiles.AddRange(Directory.GetFiles(Path).Where(x => (new FileInfo(x).Attributes & FileAttributes.Hidden) == 0).ToArray());

                Files = folderAndFiles.ToArray();
            }
        }
    }

    public class DragDropHelper
    {
        public static readonly DependencyProperty IsDragOverProperty = DependencyProperty.RegisterAttached(
            "IsDragOver", typeof(bool), typeof(DragDropHelper), new PropertyMetadata(default(bool)));

        public static void SetIsDragOver(DependencyObject element, bool value)
        {
            element.SetValue(IsDragOverProperty, value);
        }

        public static bool GetIsDragOver(DependencyObject element)
        {
            return (bool)element.GetValue(IsDragOverProperty);
        }
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public pkRevitDatasheets.EntryPoints.Entry_0010_pkRevitDatasheets classEntryPoint { get; set; }

        public Window_MoreParameters win_moreParam { get; set; }

        //public string myString_ServerDirectory = @"Q:\Revit Revit Revit\~Luminaires (do not edit directly)\";

        static int MAX_FILE_PATH = 260;
        Guid named_guid;
        public MainWindow(Guid ng)
        {
            named_guid = ng;

            //////Assembly.Load(File.ReadAllBytes(@"R:\_001_GitHubRespositories\joshnewzealand\pkRevit\pkRevit\pkRevitDatasheets\bin\Debug\Microsoft.WindowsAPICodePack.dll"));
            //////Assembly.Load(File.ReadAllBytes(@"R:\_001_GitHubRespositories\joshnewzealand\pkRevit\pkRevit\pkRevitDatasheets\bin\Debug\Microsoft.WindowsAPICodePack.Shell.dll"));

            win_moreParam = new Window_MoreParameters(this);
            InitializeComponent();

            // buttonNewFromRevit.Click += new RoutedEventHandler(buttonNewFromRevit_Click);  //buttonNewFromRevit_Click

            //fe0a121e-b4e2-47c5-85a1-e8da1ccf6a87

            buttonNewFromRevit.Click += new RoutedEventHandler(buttonNewFromRevit_Click_Alternative);

            model = new Model(this);
            DataContext = model;
            model.Path = @"C:\Users\Joshua\Dropbox\pkRevit Storage (do not edit directly)\Database File\Admin Storage\20201229 1732 57";
            ///model.Path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            FileToIconConverter fic = this.Resources["fic"] as FileToIconConverter;
            fic.DefaultSize = 200;
            lb_listBox.AddHandler(System.Windows.Controls.ListViewItem.PreviewMouseDownEvent, new MouseButtonEventHandler(list_MouseDown));

            buttonNewFromRevit.Click -= new RoutedEventHandler(buttonNewFromRevit_Click);
            buttonNewFromRevit.Click += new RoutedEventHandler(buttonNewFromRevit_Click_Alternative);
         }

        //named_guid

        ///unique id for the project please
        ///unique id for the type please, this is just element ID which is fine
        ///unique id of the type
        /// 
        ///there is a different one here, 
        ///what i would like is a different database for each having only different
        ///a different database to contain all the fields values from the type
        ///
        ///what is the next step, start storing for clutha (turn off the models)
        ///sadly clutha is in 2019, therefore our testing one needs to be 
        /// 
        ///the main guide is the schedule
        ///5168 Americold Cold Store
        ///5177 Foodstuffs Hornby DC Project Nest (Bottling Plant)
        ///5236 Argus Heating
        ///5147 St Lukes Church, Chch
        ///5113 New World Ravenswood - Revisions
        ///.
        ///.
        ///5177 Foodstuffs Hornby DC Project Nest (Bottling Plant)
        ///it stores the family and the revision (where to find it via revision control)
        ///it must store images 
        ///the independant backup system is nice and fast we can use that
        ///the independant backup system is nice and fast we can use that
        ///the independant backup system is nice and fast we can use that
        ///+2 i want the properties to show up on double click a it appears in the schedule, these form the basis of search terms, if fact it will update search terms on each click over
        ///get it to open in revit now figure out how many dlls need to be loaded
        ///.
        ///keep focusing on dragging on stuff, 
        ///keep focusing on dragging on stuff 
        ///keep focusing on dragging on stuff
        ///.
        ///get the delete button working, this shouldn't take 5 minutes with an are you sure
        ///then get it working and tested with revit, and then you can delte the 3 lines above
        ///rename the table, then get the search box working

        public void ClearCache()
        {
            FileToIconConverter fic = this.Resources["fic"] as FileToIconConverter;
            //Clear Thumbnail only, icon is not cleared.
            fic.ClearInstanceCache();
        }
        
        Model model;
        public MainWindow()
        { 
            named_guid = Guid.Parse("00000000-0000-0000-0000-000000000000");

            win_moreParam = new Window_MoreParameters(this);
            InitializeComponent();

            model = new Model(this);
            DataContext = model;
            model.Path = @"C:\Users\Joshua\Dropbox\pkRevit Storage (do not edit directly)\Database File\Admin Storage\20201229 1732 57";
            ///model.Path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            FileToIconConverter fic = this.Resources["fic"] as FileToIconConverter;
            fic.DefaultSize = 200;
            lb_listBox.AddHandler(System.Windows.Controls.ListViewItem.PreviewMouseDownEvent, new MouseButtonEventHandler(list_MouseDown));
             
            buttonNewFromRevit.Click -= new RoutedEventHandler(buttonNewFromRevit_Click);
            buttonNewFromRevit.Click += new RoutedEventHandler(buttonNewFromRevit_Click_Alternative);
        }


        

        private void DropBorder_OnDragEnter(object sender, DragEventArgs e)
        {
            int eL = -1;

            try
            {
                DragDropHelper.SetIsDragOver((DependencyObject)sender, true);
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("DropBorder_OnDragEnter, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        private void DropBorder_OnPreviewDragLeave(object sender, DragEventArgs e)
        {
            int eL = -1;

            try
            {
                DragDropHelper.SetIsDragOver((DependencyObject)sender, false);
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("DropBorder_OnPreviewDragLeave, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion

        }

        private void list_MouseDown(object sender, MouseButtonEventArgs e)
        {
            int eL = -1;

            try
            {
                if (e.ClickCount == 2 && e.LeftButton == MouseButtonState.Pressed)
                {
                    if (lb_listBox.SelectedValue is string)
                    {
                        string dir = lb_listBox.SelectedValue as string;
                        if (Directory.Exists(dir))
                        {
                            model.Path = dir;
                            e.Handled = true;
                        }
                    }
                }
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("list_MouseDown, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }


        private void Open_Click(object sender, RoutedEventArgs e)
        {
            int eL = -1;

            try
            {
                System.Diagnostics.Process.Start((string)lb_listBox.SelectedItem);
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("Open_Click, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        private void Openwith_Click(object sender, RoutedEventArgs e)
        {
            int eL = -1;

            try
            {
                System.Diagnostics.Process.Start("rundll32.exe", "shell32.dll, OpenAs_RunDLL " + (string)lb_listBox.SelectedItem);
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("Openwith_Click, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            int eL = -1;

            try
            {
                //(string)lb_listBox.SelectedItem



            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("Delete_Click, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        private void buttonSetStoreFolder_Click(object sender, RoutedEventArgs e) //keep this one here to copy the try statement
        {
            int eL = -1;

            try
            {
                setStoreDirectory();
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("buttonSetStoreFolder_Click, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        } //keep this one here to copy the try statement

        private void setStoreDirectory()
        {
            System.Windows.MessageBox.Show("Please select store directory" + Environment.NewLine + Environment.NewLine + "Recommend: Dropbox OR GoogleDrive OR OneDrive");

            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    string[] files = Directory.GetFiles(fbd.SelectedPath);

                    Properties.Settings.Default.DropboxOrGoogleDriveOrOnedrive_Path = fbd.SelectedPath;
                    Properties.Settings.Default.Save();
                    Properties.Settings.Default.Reload();

                    label_DropboxGoogleDriveOnedrive.Content = Properties.Settings.Default.DropboxOrGoogleDriveOrOnedrive_Path;
                }
            }
        }

        private void setStoreDirectory_MakeSubDirectories()
        {
            label_DropboxGoogleDriveOnedrive.Content = Properties.Settings.Default.DropboxOrGoogleDriveOrOnedrive_Path;
            string string_DirectoryToCreate = label_DropboxGoogleDriveOnedrive.Content + @"\pkRevit Storage (do not edit directly)\Database File";
            string string_DirectoryToCreate_Storage = string_DirectoryToCreate + @"\Admin Storage";

            if (!System.IO.Directory.Exists(string_DirectoryToCreate_Storage))
            {
                System.IO.Directory.CreateDirectory(string_DirectoryToCreate_Storage);
            }

            string_DatabaseLocation = string_DirectoryToCreate + @"\Full_Index.db";

            if(!System.IO.File.Exists(string_DatabaseLocation))
            {
                //MessageBox.Show(classEntryPoint.executionLocation);

                //System.Diagnostics.Process.Start(System.Reflection.Assembly.GetExecutingAssembly().Location);

                System.IO.File.Copy(classEntryPoint.executionLocation + @"\StartDB.db", string_DatabaseLocation);
            }


            string_Default_Parameters_BuiltIn = string_DirectoryToCreate + @"\Default_Param_Builtin.db";
            if (!System.IO.File.Exists(string_Default_Parameters_BuiltIn))
            {
                Stream stream = new FileStream(string_Default_Parameters_BuiltIn, FileMode.Create, FileAccess.Write);
                DataContractSerializer serializer = new DataContractSerializer(typeof(Dictionary<int, int>));  //four of four
                serializer.WriteObject(stream, dict_SortOrdered_BuiltIn); stream.Close();
            }

            string_Default_Parameters_Shared = string_DirectoryToCreate + @"\Default_Param_Shared.db";
            if (!System.IO.File.Exists(string_Default_Parameters_Shared))
            {
                Stream stream = new FileStream(string_Default_Parameters_Shared, FileMode.Create, FileAccess.Write);
                DataContractSerializer serializer = new DataContractSerializer(typeof(Dictionary<int, Guid>));  //four of four
                serializer.WriteObject(stream, dict_SortOrdered_Shared); stream.Close();
            }

        }

        public SQLiteConnection OleDbConnection_ButtonOK { get; set; }
        public string string_DatabaseLocation { get; set; }
        public string string_Default_Parameters_BuiltIn { get; set; }
        public string string_Default_Parameters_Shared { get; set; }

        public DataTable dataTable { get; set; } = new DataTable();
        public SQLiteConnection connRLPrivateFlexible { get; set; }

        public Dictionary<int, NameAndValue> dictParameters_BuiltInt { get; set; } = new Dictionary<int, NameAndValue>();  //three of four
        public Dictionary<Guid, NameAndValue> dictParameters_Shared { get; set; } = new Dictionary<Guid, NameAndValue>();  //three of four

        public Dictionary<int, int> dict_SortOrdered_BuiltIn { get; set; } = new Dictionary<int, int>();  //three of four
        public Dictionary<int, Guid> dict_SortOrdered_Shared { get; set; } = new Dictionary<int, Guid>();  //three of four

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            int eL = -1;

            try
            {
                dict_SortOrdered_BuiltIn.Add(0, -1002002);
                dict_SortOrdered_BuiltIn.Add(1, -1002001);
                dict_SortOrdered_BuiltIn.Add(2, -1010105);


                if (true)
                {
                    if (Properties.Settings.Default.DropboxOrGoogleDriveOrOnedrive_Path == "")
                    {
                        if (System.IO.Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\Dropbox"))
                        {
                            Properties.Settings.Default.DropboxOrGoogleDriveOrOnedrive_Path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\Dropbox";
                            Properties.Settings.Default.Save();
                            Properties.Settings.Default.Reload();

                            label_DropboxGoogleDriveOnedrive.Content = Properties.Settings.Default.DropboxOrGoogleDriveOrOnedrive_Path;
                        }
                        else
                        {
                            eL = 100;
                            setStoreDirectory();
                        }
                    }
                    eL = 150;
                    setStoreDirectory_MakeSubDirectories();
                } //directory setups please collapse
                eL = 239;

                method_LoadUpMasterList();

                eL = 343;

                if (lv_MasterList.Items.Count != 0)
                {
                    lv_MasterList.SelectedIndex = Properties.Settings.Default.lvMasterListSelectedIndex;
                    methodRefresh_LoadUp_lvDragDirectory(LoadDirectoryOrNot.LoadEverything);
                }

                //MessageBox.Show(dataTable.Rows.Count.ToString());

                ///////////////////////////////////////////////////////////////////////MessageBox.Show(GetDataTable("SELECT name FROM sqlite_master WHERE type = 'table' ORDER BY 1").Rows.Count.ToString());
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

        public void method_LoadUpMasterList()
        {
            dataTable.Clear();
            dataTable = GetDataTable("SELECT * FROM [JustSomeDragging] order by [SortOrder] desc ");
            lv_MasterList.ItemsSource = dataTable.DefaultView;
        }

        public DataTable GetDataTable(string sql) 
        {
            try
            {
                DataTable dt = new DataTable();

                OleDbConnection_ButtonOK = reconnectPrivateFlexible(false, string_DatabaseLocation, connRLPrivateFlexible);
                using (SQLiteCommand command = ConnectionCommandType(OleDbConnection_ButtonOK))
                {
                    OleDbConnection_ButtonOK.Open();
                    command.CommandText = sql;// ORDER BY [TargetSort] DESC";

                    SQLiteDataReader dataReader = command.ExecuteReader();

                    dt.Load(dataReader);

                    dataReader.Close();
                    OleDbConnection_ButtonOK.Close();
                    OleDbConnection_ButtonOK.Dispose();
                    GC.Collect();
                }

                return dt;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public static SQLiteConnection reconnectPrivateFlexible(bool azure, string path000000, SQLiteConnection connRLPrivateFlexible) //new methods here need to be updated in 1 2 3 4 5 places and one by the directives
        {
            int eL = -1;

            try
            {
                string myConnectionString = "";
                myConnectionString = "Data Source=" + path000000 + "; Version=3";
                connRLPrivateFlexible = new SQLiteConnection(myConnectionString, true);
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("reconnectPrivateFlexible, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion

            return connRLPrivateFlexible;
        }

        private void lv_DragDirectory_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                return;
                ////////////////if (e.MouseDevice.LeftButton == System.Windows.Input.MouseButtonState.Pressed) //Checking that leftbutton is pressed
                ////////////////{
                ////////////////    var file = lv_DragDirectory.SelectedItem as FileObject; //Taking the selected object as FileObject

                ////////////////    if (file != null) //Check that item is really selected. Alternatively file variable equals null

                ////////////////        DragDrop.DoDragDrop(this, new DataObject(DataFormats.FileDrop, new string[] { file.FullName }), DragDropEffects.Move);/*new string[] {...} - array contains files` pathes user wants to drag from list*/

                ////////////////    lv_DragDirectory.Items.Remove(file);                   //removing dropped obect from the list
                ////////////////}
            }
            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("lv_DragDirectory_MouseMove" + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        private string method_Storage_DBFile_Directory()
        {
            return label_DropboxGoogleDriveOnedrive.Content + @"\pkRevit Storage (do not edit directly)\Database File"; 
        }

        public class IES_FirstChild //delete this?
        {
            public string s_FolderName { get; set; }
            public string s_SearchTerms { get; set; }
            public int i_SortOrder { get; set; }
        } //delete this?

        class FileObject
        {
            public string Name { get; set; }

            public string FullName { get; set; }

            public FileObject(string name, string fullname)
            {
                Name = name;
                FullName = fullname;
            }

            public override string ToString()
            {
                return Name;
            }
        }
        
        private void lv_MasterList_MouseUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                methodRefresh_LoadUp_lvDragDirectory(LoadDirectoryOrNot.LoadEverything);  //this returns a false if the directoy is not there
            }
            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("lv_MasterList_PreviewMouseDoubleClick" + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        private void lv_DragDirectory_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                ////////////////if (lv_MasterList.SelectedIndex == -1) return;

                ////////////////string s_DoesThisHaveIES = lv_DragDirectory.SelectedItem.ToString();

                ////////////////if (!(s_DoesThisHaveIES.Length > 3))
                ////////////////{
                ////////////////    return;
                ////////////////}

                ////////////////if (s_DoesThisHaveIES.Substring(s_DoesThisHaveIES.Length - 4).ToLower() != ".ies")
                ////////////////{
                ////////////////    System.Windows.Forms.MessageBox.Show(new Form() { TopMost = true }, "Please double click in IES file.");
                ////////////////    return;
                ////////////////}

                //////////////////   string s_DeleteDirectory_And_Entry = ((IES_FirstChild)lv_MasterList.SelectedItem).s_FolderName;

                ////////////////myExternalEvent_EE03_ApplyIES.Raise();

            }
            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("lv_DragDirectory_PreviewMouseDoubleClick" + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        public static SQLiteCommand ConnectionCommandType(SQLiteConnection OleDbConnection_ButtonOK)
        {
            SQLiteCommand cmdInsert = new SQLiteCommand();
            cmdInsert.Connection = OleDbConnection_ButtonOK;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            return cmdInsert;
        }

        private Int64 methodNewScalar(string s_tableName)
        {
            Int64 intNewScalar = 1;

            OleDbConnection_ButtonOK = reconnectPrivateFlexible(false, string_DatabaseLocation, connRLPrivateFlexible);
            using (SQLiteCommand command = ConnectionCommandType(OleDbConnection_ButtonOK))
            {
                //command.CommandText = "SELECT [ID],[ProjectNo],[ProjectName],[NameWhereApplicable],[SearchString],[TargetDatabaseID],[TargetDatabaseName],[TargetDatabasePath],[TargetSort],[IsRevit] FROM [Search Strings]  WHERE ISDELETE = 0  order by [TargetSort] desc ";// ORDER BY [TargetSort] DESC";
                command.CommandText = "SELECT [SortOrder] FROM [" + s_tableName + "] order by [SortOrder] desc limit 1 ";// ORDER BY [TargetSort] DESC";

                OleDbConnection_ButtonOK.Open();
                SQLiteDataReader dataReader = command.ExecuteReader();

                if (dataReader.HasRows)
                {
                    DataTable dt = new DataTable();
                    dt.Load(dataReader);

                    intNewScalar = (Int64)dt.Rows[0][0] + 1;
                }

                dataReader.Close();
                OleDbConnection_ButtonOK.Close();
                OleDbConnection_ButtonOK.Dispose();
                GC.Collect();
            }

            return intNewScalar;
        }

        private void buttonNewFromRevit_Click_Alternative(object sender, RoutedEventArgs e)
        {
            int eL = -1;
            try
            {
                //string string_DirectoryToCreate_Storage = method_Storage_DBFile_Directory() + @"\Admin Storage";
                //string DirPath_DragDir = string_DirectoryToCreate_Storage + "\\" + ((DataRowView)lv_MasterList.SelectedItem)["FolderName"] + @"\\";

                //string myXmlFilePath = DirPath_DragDir + "listview.xml";

                method_ReadIntoResult();
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("buttonNewFromRevit_Click2, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        private string method_XML_Parameters_BuiltIn()
        {
            string string_DirectoryToCreate_Storage = method_Storage_DBFile_Directory() + @"\Admin Storage";
            string DirPath_Date_Dir = string_DirectoryToCreate_Storage + "\\" + ((DataRowView)lv_MasterList.SelectedItem)["FolderName"] + @"\\";

            string myXmlFilePath = DirPath_Date_Dir + "Parameters_BuiltIn.xml";

            return myXmlFilePath;
        }

        private string method_XML_Parameters_Shared()
        {
            string string_DirectoryToCreate_Storage = method_Storage_DBFile_Directory() + @"\Admin Storage";
            string DirPath_Date_Dir = string_DirectoryToCreate_Storage + "\\" + ((DataRowView)lv_MasterList.SelectedItem)["FolderName"] + @"\\";

            string myXmlFilePath = DirPath_Date_Dir + "Parameters_Shared.xml";

            return myXmlFilePath;
        }

        [DataContract]
        public class NameAndValue
        {
            [DataMember]
            public string sName { get; set; }
            [DataMember]
            public string sValue { get; set; }
        }

        private void method_ReadIntoResult()
        {
            if(true)
            {
                string fileString = method_XML_Parameters_BuiltIn();

                if (System.IO.File.Exists(fileString))
                {
                    Stream stream = new FileStream(fileString, FileMode.Open, FileAccess.Read);
                    DataContractSerializer serializer = new DataContractSerializer(typeof(Dictionary<int, NameAndValue>));   //one of four
                    dictParameters_BuiltInt = serializer.ReadObject(stream) as Dictionary<int, NameAndValue>; stream.Close();  //two of four
                    win_moreParam.lv_Result_BuiltIn.ItemsSource = dictParameters_BuiltInt;
                }
                else
                {
                    win_moreParam.lv_Result_BuiltIn.ItemsSource = null;
                }
            }

            if (true)
            {
                string fileString = method_XML_Parameters_Shared();

                if (System.IO.File.Exists(fileString))
                {
                    Stream stream = new FileStream(fileString, FileMode.Open, FileAccess.Read);
                    DataContractSerializer serializer = new DataContractSerializer(typeof(Dictionary<Guid, NameAndValue>));   //one of four
                    dictParameters_Shared = serializer.ReadObject(stream) as Dictionary<Guid, NameAndValue>; stream.Close();  //two of four
                    win_moreParam.lv_Result_Shared.ItemsSource = dictParameters_Shared;
                } else
                {
                    win_moreParam.lv_Result_Shared.ItemsSource = null;
                }
            }

            lv_ReorderThis.Items.Clear();

            if (true)
            {
                Stream stream = new FileStream(string_Default_Parameters_BuiltIn, FileMode.Open, FileAccess.Read);
                DataContractSerializer serializer = new DataContractSerializer(typeof(Dictionary<int, int>));   //one of four
                dict_SortOrdered_BuiltIn = serializer.ReadObject(stream) as Dictionary<int, int>; stream.Close();  //two of four

                foreach (KeyValuePair<int, int> kpv in dict_SortOrdered_BuiltIn)
                {
                    if(dictParameters_BuiltInt.ContainsKey(kpv.Value))
                    {
                        lv_ReorderThis.Items.Add(new Tuple<int, string, string, bool>(kpv.Key, dictParameters_BuiltInt[kpv.Value].sName, dictParameters_BuiltInt[kpv.Value].sValue, true));
                    }
                }
            }

            if (true)
            {
                Stream stream = new FileStream(string_Default_Parameters_Shared, FileMode.Open, FileAccess.Read);
                DataContractSerializer serializer = new DataContractSerializer(typeof(Dictionary<int, Guid>));   //one of four
                dict_SortOrdered_Shared = serializer.ReadObject(stream) as Dictionary<int, Guid>; stream.Close();  //two of four

                foreach (KeyValuePair<int, Guid> kpv in dict_SortOrdered_Shared)
                {
                    if (dictParameters_Shared.ContainsKey(kpv.Value))
                    {
                        lv_ReorderThis.Items.Add(new Tuple<int, string, string, bool>(kpv.Key, dictParameters_Shared[kpv.Value].sName, dictParameters_Shared[kpv.Value].sValue, false));
                    }
                }
            }
        }

        public enum LoadDirectoryOrNot
        {
            LoadEverything, LoadIconDirectoryOnly
        }

        private bool methodRefresh_LoadUp_lvDragDirectory(LoadDirectoryOrNot whatToLoad)
        {

            string string_DirectoryToCreate_Storage = method_Storage_DBFile_Directory() + @"\Admin Storage";
            string myString_DataStore = string_DirectoryToCreate_Storage + "\\" + ((DataRowView)lv_MasterList.SelectedItem)["FolderName"];

            if (!System.IO.Directory.Exists(myString_DataStore))
            {
                MessageBox.Show("Directory has been deleted.");


                lb_listBox.Items.Clear();
                lb_listBox.IsEnabled = false;
                return false;
            }

            if (whatToLoad == LoadDirectoryOrNot.LoadEverything)
            {
                Properties.Settings.Default.lvMasterListSelectedIndex = lv_MasterList.SelectedIndex;
                Properties.Settings.Default.Save();
                Properties.Settings.Default.Reload();

                lb_listBox.IsEnabled = true;
                myLabel_Directory.Content = ((DataRowView)lv_MasterList.SelectedItem)["FolderName"];

                //Guid guid = new Guid((byte[])((DataRowView)lv_MasterList.SelectedItem)["ProjectGUID"]);
                label_ProjectGUID.Content = Guid.Parse(Encoding.ASCII.GetString((byte[])((DataRowView)lv_MasterList.SelectedItem)["ProjectGUID"]));
            } //just directory refresh


            model.Path = myString_DataStore;

            if (whatToLoad == LoadDirectoryOrNot.LoadEverything)
            {
                method_ReadIntoResult();
            } //just directory refresh


            return true;
        }

        private void lv_Ordered_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            int eL = -1;

            try
            {

                Tuple<int, string, string, bool> tuple = (Tuple<int, string, string, bool>)lv_ReorderThis.SelectedItem;


                if (tuple.Item4)
                {
                    if (dict_SortOrdered_BuiltIn[tuple.Item1] == -1002002 | dict_SortOrdered_BuiltIn[tuple.Item1] == -1002001 | dict_SortOrdered_BuiltIn[tuple.Item1] == -1010105)
                    {
                        MessageBox.Show("'Family Name' and " + Environment.NewLine + "'Type Name' and " + Environment.NewLine + "'Type Comment' can't be deleted.");
                        return;
                    }
                }

                lv_ReorderThis.Items.Remove(lv_ReorderThis.SelectedItem);

                //what is happening here, there is a difference in here
                ///what happens here alight we are removing but 

                if (tuple.Item4)
                {
                    dict_SortOrdered_BuiltIn.Remove(tuple.Item1);

                    Stream stream = new FileStream(string_Default_Parameters_BuiltIn, FileMode.Create, FileAccess.Write);
                    DataContractSerializer serializer = new DataContractSerializer(typeof(Dictionary<int, int>));  //four of four
                    serializer.WriteObject(stream, dict_SortOrdered_BuiltIn); stream.Close();

                }
                else
                {
                    dict_SortOrdered_Shared.Remove(tuple.Item1);

                    Stream stream = new FileStream(string_Default_Parameters_Shared, FileMode.Create, FileAccess.Write);
                    DataContractSerializer serializer = new DataContractSerializer(typeof(Dictionary<int, Guid>));  //four of four
                    serializer.WriteObject(stream, dict_SortOrdered_Shared); stream.Close();
                }
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("lv_Ordered_PreviewMouseDoubleClick, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        private void but_moreParameters_Click(object sender, RoutedEventArgs e)
        {
            int eL = -1;

            try
            {
                win_moreParam.Show();
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("but_moreParameters_Click, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        private void buttonNewFromRevit_Click(object sender, RoutedEventArgs e)
        {
            int eL = -1;
            try
            {
                //////////////////////////method_ReadIntoResult(myXmlFilePath);
                eL = 703;

                UIDocument uidoc = classEntryPoint.commandData.Application.ActiveUIDocument;
                Element elementType = method_butshouldworkonWallTypes(true, uidoc);

                if (elementType == null) return;

                bool rc = JtNamedGuidStorage.Get(uidoc.Document, "TrackChanges_project_identifier", out named_guid, false);

                EnumerableRowCollection results = from myRow in dataTable.AsEnumerable()
                                                  where myRow.Field<Int64>("FamilySymbolID") == elementType.Id.IntegerValue
                                                  select myRow;

                results = from myRow in results.Cast<DataRow>()
                          where Guid.Parse(Encoding.ASCII.GetString(myRow.Field<byte[]>("ProjectGUID"))) == named_guid 
                                                  select myRow;


                if (results.Cast<DataRow>().Count() != 0)
                {
                    lv_MasterList.SelectedIndex = dataTable.Rows.IndexOf(results.Cast<DataRow>().First());
                }
                else
                {
                    method_NewEntry(elementType.Id.IntegerValue);
                }


                List<Parameter> listParameters = elementType.Parameters.Cast<Parameter>().OrderBy(x => x.Definition.Name).ToList();

                dictParameters_BuiltInt.Clear();  //three of four
                dictParameters_Shared.Clear();  //three of four

                eL = 715;

                foreach (Parameter parameter2 in listParameters)
                {
                    string s_returnValue = "";

                    switch (parameter2.StorageType)
                    {
                        case StorageType.String:
                            s_returnValue = parameter2.AsString();
                            break;
                        case StorageType.Integer:
                            s_returnValue = parameter2.AsInteger().ToString();
                            break;
                        case StorageType.None:
                            break;
                        case StorageType.ElementId:
                            s_returnValue = parameter2.AsElementId().IntegerValue.ToString();
                            break;
                        case StorageType.Double:
                            s_returnValue = parameter2.AsDouble().ToString();
                            break;
                        default:
                            break;
                    }
                    if (parameter2.Id.IntegerValue < 0) dictParameters_BuiltInt.Add(parameter2.Id.IntegerValue, new NameAndValue() { sName = parameter2.Definition.Name, sValue = s_returnValue });

                    eL = 742;
                    if (parameter2.IsShared) dictParameters_Shared.Add(parameter2.GUID, new NameAndValue() { sName = parameter2.Definition.Name, sValue = s_returnValue });
                }
                eL = 744;

                if (true)
                {
                    Stream stream = new FileStream(method_XML_Parameters_BuiltIn(), FileMode.Create, FileAccess.Write);
                    DataContractSerializer serializer = new DataContractSerializer(typeof(Dictionary<int, NameAndValue>));  //four of four
                    serializer.WriteObject(stream, dictParameters_BuiltInt); stream.Close();
                    new FileInfo(method_XML_Parameters_BuiltIn()).Attributes |= FileAttributes.Hidden;
                }
                eL = 889;
                if (true)
                {
                    Stream stream = new FileStream(method_XML_Parameters_Shared(), FileMode.Create, FileAccess.Write);
                    DataContractSerializer serializer = new DataContractSerializer(typeof(Dictionary<Guid, NameAndValue>));  //four of four
                    serializer.WriteObject(stream, dictParameters_Shared); stream.Close();
                    new FileInfo(method_XML_Parameters_Shared()).Attributes |= FileAttributes.Hidden;
                }
                eL = 756;

                methodRefresh_LoadUp_lvDragDirectory(LoadDirectoryOrNot.LoadEverything);

            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("buttonNewFromRevit_Click2, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        private void method_NewEntry(Int64 FamilySymbolID)
        {
            //find the family id in this project document, but is this the correct way to do it

            string string_DirectoryToCreate_Storage = method_Storage_DBFile_Directory() + @"\Admin Storage";

            string myString_Date = DateTime.Now.ToString("yyyyMMdd HHmm ss");
            string myString_DataStore = string_DirectoryToCreate_Storage + "\\" + myString_Date;

            if (System.IO.Directory.Exists(myString_DataStore))
            {
                MessageBox.Show("Please allow 1 second between clicks to avoid duplicates.");
                return;
            }

            System.IO.Directory.CreateDirectory(myString_DataStore);
            Int64 Int64_LastMaxCount = methodNewScalar("JustSomeDragging");

            OleDbConnection_ButtonOK = reconnectPrivateFlexible(false, string_DatabaseLocation, connRLPrivateFlexible);
            using (SQLiteCommand cmdInsert = ConnectionCommandType(OleDbConnection_ButtonOK))
            {
                cmdInsert.CommandText = @"INSERT INTO [JustSomeDragging] ([FolderName],[SortOrder],[SearchTerms],[ProjectGUID],[FamilySymbolID]) values (@a,@b,@c,@d,@e)";
                cmdInsert.Parameters.Clear();
                cmdInsert.Parameters.AddWithValue("@a", myString_Date);
                cmdInsert.Parameters.AddWithValue("@b", Int64_LastMaxCount);
                cmdInsert.Parameters.AddWithValue("@c", "test search term");
                cmdInsert.Parameters.AddWithValue("@d", GuidToByteArray(named_guid.ToString()));
                cmdInsert.Parameters.AddWithValue("@e", FamilySymbolID);

                OleDbConnection_ButtonOK.Open();
                cmdInsert.ExecuteNonQuery();
                OleDbConnection_ButtonOK.Close();
                OleDbConnection_ButtonOK.Dispose();
                GC.Collect();
            }

            //int int_PreviousSelectedIndex = lv_MasterList.SelectedIndex;
            method_LoadUpMasterList();
            lv_MasterList.SelectedIndex = 0;
        }

        public static byte[] GuidToByteArray(string guidData)
        {
            return Encoding.ASCII.GetBytes(guidData.ToString());
        }

        private void lv_DragDirectory_Drop(object sender, DragEventArgs e)
        {
            try
            {
                string[] file_pathes = (string[])e.Data.GetData(DataFormats.FileDrop);  /*file_pathes contains pathes of dragged selected files*/

                //add every file to the list copping them to the application directory

                string string_DirectoryToCreate_Storage = method_Storage_DBFile_Directory() + @"\Admin Storage";
                string myString_DataStore = string_DirectoryToCreate_Storage + "\\" + ((DataRowView)lv_MasterList.SelectedItem)["FolderName"];

                string[] fileEntries = Directory.GetFiles(myString_DataStore);


                foreach (var path in file_pathes)
                {
                    if (false)
                    {
                        bool myBool_IESAreadyThere = false;

                        // string s_Equal_DotIES = path.Substring(path.Length - 4);
                        if (path.Substring(path.Length - 4).ToLower() == ".ies")
                        {
                            foreach (string s_DoesThisHaveIES in fileEntries)
                            {
                                if (s_DoesThisHaveIES.Length > 3)
                                {
                                    if (s_DoesThisHaveIES.Substring(s_DoesThisHaveIES.Length - 4).ToLower() == ".ies")
                                    {
                                        System.Windows.Forms.MessageBox.Show(new System.Windows.Forms.Form() { TopMost = true }, "IES file has already been added...there can be only one.");
                                        myBool_IESAreadyThere = true;
                                    }
                                }
                            }
                        }

                        if (myBool_IESAreadyThere) continue;
                    }

                    string s_FileName = System.IO.Path.GetFileName(path);

                    if (fileEntries.Contains(s_FileName))
                    {
                        System.Windows.Forms.MessageBox.Show(new System.Windows.Forms.Form() { TopMost = true }, "File " + s_FileName + " is already there.");
                        continue;
                    }

                    string new_path = myString_DataStore + @"\" + System.IO.Path.GetFileName(path); //New file path

                    if (new_path.Length >= MAX_FILE_PATH)
                    {
                        System.Windows.Forms.MessageBox.Show(new System.Windows.Forms.Form() { TopMost = true }, "File path is too long.");
                        continue;
                    }

                    File.Copy(path, new_path);

                    //lv_DragDirectory.Items.Add(System.IO.Path.GetFileName(path));
                }

                methodRefresh_LoadUp_lvDragDirectory(LoadDirectoryOrNot.LoadIconDirectoryOnly);
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("lv_DragDirectory_Drop" + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        public Element method_butshouldworkonWallTypes(bool myBoolPleaseSelectCheck, UIDocument uidoc) //store this in thumbs
        {
            Document doc = uidoc.Document;

            if (uidoc.Selection.GetElementIds().Count != 1)
            {
                if (myBoolPleaseSelectCheck) MessageBox.Show("Please select only one FamilyInstance.");
                return null;
            }

            Element myElement = doc.GetElement(uidoc.Selection.GetElementIds().First());
            if (myElement.GetType() == typeof(IndependentTag))
            {
                IndependentTag myIndependentTag_1355 = myElement as IndependentTag;
                if (myIndependentTag_1355.TaggedLocalElementId == null) return null;
                return doc.GetElement(myIndependentTag_1355.TaggedLocalElementId);
            }

            //if (myElement.GetType() != typeof(FamilyInstance)) return null;
            return doc.GetElement(myElement.GetTypeId());
        }

        private void butIMAGE_Click(object sender, RoutedEventArgs e)
        {
            int eL = -1;

            try
            {
                if (!System.Windows.Clipboard.ContainsImage())
                {
                    MessageBox.Show("clipboard does not contain image");
                    return;
                }

                if (lv_MasterList.SelectedIndex == -1)
                {
                    MessageBox.Show("Please select item from lv_MasterList");
                    return;
                }


                string string_DirectoryToCreate_Storage = method_Storage_DBFile_Directory() + @"\Admin Storage";
                string myString_DataStore = string_DirectoryToCreate_Storage + "\\" + ((DataRowView)lv_MasterList.SelectedItem)["FolderName"];

                int int_NextImageNumber = 1;
                string s_fileName = "";

                do
                {
                    s_fileName = myString_DataStore + "\\ClipboardImage_" + int_NextImageNumber.ToString().PadLeft(3, '0') + ".png";
                    int_NextImageNumber++;

                } while (Directory.GetFiles(myString_DataStore).Contains(System.IO.Path.GetFileName(s_fileName)));

                var img = System.Windows.Forms.Clipboard.GetImage();
                img.Save(s_fileName, System.Drawing.Imaging.ImageFormat.Png);

                methodRefresh_LoadUp_lvDragDirectory(LoadDirectoryOrNot.LoadIconDirectoryOnly); //this returns a false if the directoy is not there
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("butIMAGE_Click, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        private void buttonDelete_in_Lieu_of_FromType_Click(object sender, RoutedEventArgs e)
        {
            int eL = -1;

            try
            {
                OleDbConnection_ButtonOK = reconnectPrivateFlexible(false, string_DatabaseLocation, connRLPrivateFlexible);
                using (SQLiteCommand cmdInsert = ConnectionCommandType(OleDbConnection_ButtonOK))
                {
                    cmdInsert.CommandText = @"delete from JustSomeDragging where FolderName = '" + ((DataRowView)lv_MasterList.SelectedItem)["FolderName"] + "';";

                    OleDbConnection_ButtonOK.Open();
                    //   if (!myMainWindow.boolDeveloperMode) cmdInsert.ExecuteNonQueryAsync();
                    cmdInsert.ExecuteNonQuery();
                    OleDbConnection_ButtonOK.Close();
                    OleDbConnection_ButtonOK.Dispose();
                    GC.Collect();
                }

                if (((DataRowView)lv_MasterList.SelectedItem)["FolderName"] == myLabel_Directory.Content)
                {
                    lb_listBox.Items.Clear();
                    myLabel_Directory.Content = "yyyyMMdd HHmm ss";
                    lb_listBox.IsEnabled = false;
                }
               
                method_LoadUpMasterList();
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("buttonDelete_in_Lieu_of_FromType_Click, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        private void textBoxSearch_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            //////boolIsolateMode = false;
            //////textChangeButReal();
        }

        private void textBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            //////if (textBoxSearch.Text == "")
            //////{
            //////    buttonMoveUp.IsEnabled = true;
            //////    buttonMoveDown_Copy.IsEnabled = true;
            //////}
            //////else
            //////{
            //////    buttonMoveUp.IsEnabled = false;
            //////    buttonMoveDown_Copy.IsEnabled = false;
            //////}
        }

        private void buttonClearSearchField_Click(object sender, RoutedEventArgs e)
        {
            //////Clear();

            //////boolIsolateMode = false;
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            int eL = -1;

            try
            {
                win_moreParam.Close();
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

        private void ListBoxItem_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            int eL = -1;

            try
            {
                if (lb_listBox.SelectedItems.Count == 0) return;
                //////string stringDatabasePath = (string)((DataRowView)lb_listBox.SelectedItem)["TargetDatabasePath"];
                //////string stringDatabaseName = (string)((DataRowView)lb_listBox.SelectedItem)["TargetDatabaseName"];

                //  (string)value
                
                MessageBoxResult result = System.Windows.MessageBox.Show("Open?  " + Path.GetFileNameWithoutExtension((string)lb_listBox.SelectedItem),  "", System.Windows.MessageBoxButton.YesNoCancel);


                if (result == MessageBoxResult.Yes)
                {
                    System.Diagnostics.Process.Start((string)lb_listBox.SelectedItem);
                }
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("ListBoxItem_PreviewMouseDoubleClick, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

    }
}
