using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using DataFormats = System.Windows.DataFormats;
using DataObject = System.Windows.DataObject;
using DragDropEffects = System.Windows.DragDropEffects;
using DragEventArgs = System.Windows.DragEventArgs;
using MessageBox = System.Windows.MessageBox;
using MouseEventArgs = System.Windows.Input.MouseEventArgs;

namespace pkRevitDatasheets
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public pkRevitDatasheets.EntryPoints.Entry_0010_pkRevitDatasheets classEntryPoint { get; set; }
        //public string myString_ServerDirectory = @"Q:\Revit Revit Revit\~Luminaires (do not edit directly)\";

        static int MAX_FILE_PATH = 260;
        Guid named_guid;
        public MainWindow(Guid ng)
        {
            named_guid = ng;

            InitializeComponent();
            //fe0a121e-b4e2-47c5-85a1-e8da1ccf6a87
        }

        public MainWindow()
        {
            named_guid = Guid.Parse("00000000-0000-0000-0000-000000000000");

            InitializeComponent();
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
                System.IO.File.Copy(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\StartDB.db", string_DatabaseLocation);
            }
        }

        public SQLiteConnection OleDbConnection_ButtonOK { get; set; }
        public string string_DatabaseLocation { get; set; }

        public DataTable dataTable { get; set; } = new DataTable();
        public SQLiteConnection connRLPrivateFlexible { get; set; }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            int eL = -1;

            try
            {
                if(true)
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
                            setStoreDirectory();
                        }
                    }
                    setStoreDirectory_MakeSubDirectories();
                } //directory setups please collapse

                ////OleDbConnection_ButtonOK = reconnectPrivateFlexible(false, string_DatabaseLocation, connRLPrivateFlexible);
                ////OleDbConnection_ButtonOK.Open();

                ////OleDbConnection_ButtonOK.Close();
                ////OleDbConnection_ButtonOK.Dispose();
                ////GC.Collect();


                method_LoadUpMasterList();

                if (lv_MasterList.Items.Count != 0)
                {
                    lv_MasterList.SelectedIndex = Properties.Settings.Default.lvMasterListSelectedIndex;
                    methodRefresh_lvDragDirectory();
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
                if (e.MouseDevice.LeftButton == System.Windows.Input.MouseButtonState.Pressed) //Checking that leftbutton is pressed
                {
                    var file = lv_DragDirectory.SelectedItem as FileObject; //Taking the selected object as FileObject

                    if (file != null) //Check that item is really selected. Alternatively file variable equals null

                        DragDrop.DoDragDrop(this, new DataObject(DataFormats.FileDrop, new string[] { file.FullName }), DragDropEffects.Move);/*new string[] {...} - array contains files` pathes user wants to drag from list*/

                    lv_DragDirectory.Items.Remove(file);                   //removing dropped obect from the list
                }
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

        private void lv_DragDirectory_Drop(object sender, DragEventArgs e)
        {
            try
            {
                string[] file_pathes = (string[])e.Data.GetData(DataFormats.FileDrop);  /*file_pathes contains pathes of dragged selected files*/

                //add every file to the list copping them to the application directory
                foreach (var path in file_pathes)
                {
                    if (false)
                    {
                        bool myBool_IESAreadyThere = false;

                        // string s_Equal_DotIES = path.Substring(path.Length - 4);
                        if (path.Substring(path.Length - 4).ToLower() == ".ies")
                        {
                            foreach (string s_DoesThisHaveIES in lv_DragDirectory.Items)
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

                    if (lv_DragDirectory.Items.Contains(s_FileName))
                    {
                        System.Windows.Forms.MessageBox.Show(new System.Windows.Forms.Form() { TopMost = true }, "File " + s_FileName + " is already there.");
                        // System.Windows.MessageBox.Show("File " + s_FileName + " is already there.");
                        continue;
                    }

                    string string_DirectoryToCreate_Storage = method_Storage_DBFile_Directory() + @"\Admin Storage";

                    string DirPath_DragDir = string_DirectoryToCreate_Storage + "\\" + ((DataRowView)lv_MasterList.SelectedItem)["FolderName"] + @"\";
                    string new_path = DirPath_DragDir + System.IO.Path.GetFileName(path); //New file path

                    if (new_path.Length >= MAX_FILE_PATH)
                    {
                        System.Windows.Forms.MessageBox.Show(new System.Windows.Forms.Form() { TopMost = true }, "File path is too long.");
                        continue;
                    }

                    File.Copy(path, new_path); 

                    lv_DragDirectory.Items.Add(System.IO.Path.GetFileName(path));  //adding FileObject (that stores file info) to the list
                }
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
        
        private bool methodRefresh_lvDragDirectory()
        {
            string string_DirectoryToCreate_Storage = method_Storage_DBFile_Directory() + @"\Admin Storage";
            string myString_DataStore = string_DirectoryToCreate_Storage + "\\" + ((DataRowView)lv_MasterList.SelectedItem)["FolderName"];

            if(!System.IO.Directory.Exists(myString_DataStore))
            {
                MessageBox.Show("Directory has been deleted.");
                lv_DragDirectory.Items.Clear();
                lv_DragDirectory.IsEnabled = false;
                return false;
            }

            Properties.Settings.Default.lvMasterListSelectedIndex = lv_MasterList.SelectedIndex;
            Properties.Settings.Default.Save();
            Properties.Settings.Default.Reload();

            lv_DragDirectory.IsEnabled = true;
            myLabel_Directory.Content = ((DataRowView)lv_MasterList.SelectedItem)["FolderName"];

            //Guid guid = new Guid((byte[])((DataRowView)lv_MasterList.SelectedItem)["ProjectGUID"]);
            label_ProjectGUID.Content = Guid.Parse(Encoding.ASCII.GetString((byte[])((DataRowView)lv_MasterList.SelectedItem)["ProjectGUID"])); 

            lv_DragDirectory.Items.Clear();
            string[] fileEntries = Directory.GetFiles(myString_DataStore);
            foreach (string fileName in fileEntries) lv_DragDirectory.Items.Add(System.IO.Path.GetFileName(fileName));

            return true;
        }

        private void lv_MasterList_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                methodRefresh_lvDragDirectory();  //this returns a false if the directoy is not there
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

        public static byte[] GuidToByteArray(string guidData)
        {
            return Encoding.ASCII.GetBytes(guidData.ToString());
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
        ///+2 i want the properties to show up on double click a it appears in the schedule, these form the basis of search terms
        ///keep focusing on dragging on stuff, but we need to know what is is first, therefore the properties above
        ///now i am torn between the wisdom of aiming for all parameters, i feel it could be good to start with that but
        ///...i also feel it is something i could get stuck on easily
        ///...the primatives work however so lets focus on getting that to work, then we'll go for getting the rest
        ///...after clicking across it will remain there but grey for the ones which don've have it

        private void buttonNewFromRevit_Click(object sender, RoutedEventArgs e)
        {
            int eL = -1;

            try
            {
                UIDocument uidoc = classEntryPoint.commandData.Application.ActiveUIDocument;
                Element element = method_butshouldworkonWallTypes(true, uidoc);
                method_NewEntry(element.GetTypeId().IntegerValue);
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("buttonNewFromRevit_Click, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        private void method_NewEntry(Int64 FamilySymbolID)
        {
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

            int int_PreviousSelectedIndex = lv_MasterList.SelectedIndex;
            method_LoadUpMasterList();
            lv_MasterList.SelectedIndex = int_PreviousSelectedIndex + 1;
        }


        private void buttonNew_in_Lieu_of_FromType_Click(object sender, RoutedEventArgs e)
        {
            int eL = -1;

            try
            {
                method_NewEntry(0);
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("buttonNew_in_Lieu_of_FromType_Click, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
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

            Element myElement = doc.GetElement(uidoc.Selection.GetElementIds().First()) as Element;
            if (myElement.GetType() == typeof(IndependentTag))
            {
                IndependentTag myIndependentTag_1355 = myElement as IndependentTag;
                if (myIndependentTag_1355.TaggedLocalElementId == null) return null;
                return doc.GetElement(myIndependentTag_1355.TaggedLocalElementId) as FamilyInstance;
            }

            if (myElement.GetType() != typeof(FamilyInstance)) return null;
            return myElement;
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

                string DirPath_DragDir = string_DirectoryToCreate_Storage + "\\" + ((DataRowView)lv_MasterList.SelectedItem)["FolderName"] + @"\";

                int int_NextImageNumber = 1;

                string s_fileName = "";

                do
                {
                    s_fileName = DirPath_DragDir + "\\ClipboardImage_" + int_NextImageNumber.ToString().PadLeft(3, '0') + ".png";
                    int_NextImageNumber++;

                } while (lv_DragDirectory.Items.Contains(System.IO.Path.GetFileName(s_fileName)));

                var img = System.Windows.Forms.Clipboard.GetImage();
                img.Save(s_fileName, System.Drawing.Imaging.ImageFormat.Png);

                methodRefresh_lvDragDirectory(); //this returns a false if the directoy is not there
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
                    lv_DragDirectory.Items.Clear();
                    myLabel_Directory.Content = "yyyyMMdd HHmm ss";
                    lv_DragDirectory.IsEnabled = false;
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


    }
}
