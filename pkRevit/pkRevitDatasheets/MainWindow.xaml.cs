using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Microsoft.Win32;
using pkRevitDatasheets.BuildingCoderClasses;
using QuickZip.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
//using System.Data.SQLite;
using System.Data.SQLite;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ListViewItem = System.Windows.Controls.ListViewItem;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using DataFormats = System.Windows.DataFormats;
using DataObject = System.Windows.DataObject;
using DragDropEffects = System.Windows.DragDropEffects;
using DragEventArgs = System.Windows.DragEventArgs;
using ListBox = System.Windows.Controls.ListBox;
using MessageBox = System.Windows.MessageBox;
using MouseEventArgs = System.Windows.Input.MouseEventArgs;
using View = Autodesk.Revit.DB.View;
using Clipboard = System.Windows.Clipboard;
using System.Drawing;
using Microsoft.WindowsAPICodePack.Shell;
using static QuickZip.Tools.FileToIconConverter;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

using Com = System.Runtime.InteropServices.ComTypes;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;

namespace pkRevitDatasheets
{
    public static class ExtensionMethods

    {
        private static Action EmptyDelegate = delegate () { };

        public static void Refresh(this UIElement uiElement)

        {
            uiElement.Dispatcher.Invoke(DispatcherPriority.Render, EmptyDelegate);
        }
    }
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

    public class Model2 : INotifyPropertyChanged  ///rename these to view models
    {
        string _path;
        string[] _files;
        Dictionary<string, Bitmap> _imageDictionary;
        bool _showFiles = true, _showFolders = true;
        public string Path { get { return _path; } set { _path = value; OnPropertyChanged("Path"); } }
        public string[] Files2 { get { return _files; } set { _files = value; OnPropertyChanged("Files2"); } }
        public Dictionary<string, Bitmap> ImageDictionary { get { return _imageDictionary; } set { _imageDictionary = value; OnPropertyChanged("ImageDictionary"); } }
        public bool ShowFiles { get { return _showFiles; } set { _showFiles = value; OnPropertyChanged("ShowFiles"); } }
        public bool ShowFolders { get { return _showFolders; } set { _showFolders = value; OnPropertyChanged("ShowFolders"); } }

        public event PropertyChangedEventHandler PropertyChanged;
        public MainWindow _view;

        public Model2(MainWindow view)
        {
            _view = view;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            int eL = -1;

            try
            {
                if (PropertyChanged != null)
                {
                    PropertyChanged(
                        this,
                        new PropertyChangedEventArgs(propertyName)
                        );
                }
                eL = 141;
                //////if (Path != "")
                //////{
                //Lazy =D

                if (Path != "")
                {
                    if (propertyName == "Path" || propertyName == "ShowFiles" || propertyName == "ShowFolders")
                    {
                        _view.ClearCache();

                        eL = 150;
                        Files2 = new string[] { "asdf", "asdf" };

                        Stream stream = new FileStream(Path + @"\Parameters_Thumbnails.xml", FileMode.Open, FileAccess.Read);
                        DataContractSerializer serializer = new DataContractSerializer(typeof(Dictionary<string, Bitmap>));   //one of four
                        ImageDictionary = serializer.ReadObject(stream) as Dictionary<string, Bitmap>; stream.Close();  //two of four
                        //public Dictionary<string, Bitmap> 
                    }
                } else
                {
                    if (ImageDictionary != null)
                    {
                        ImageDictionary.Clear();
                        _view.listbox_objectmodel.Items.Refresh();
                    }
                }
                ///where is the one what we need to block out, up or down?
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("OnPropertyChanged, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }
    }

    //////////////////public class Model : INotifyPropertyChanged
    //////////////////{
    //////////////////    string _path;
    //////////////////    string[] _files;
    //////////////////    bool _showFiles = true, _showFolders = true;
    //////////////////    public string Path { get { return _path; } set { _path = value; OnPropertyChanged("Path"); } }
    //////////////////    public string[] Files { get { return _files; } set { _files = value; OnPropertyChanged("Files"); } }
    //////////////////    public bool ShowFiles { get { return _showFiles; } set { _showFiles = value; OnPropertyChanged("ShowFiles"); } }
    //////////////////    public bool ShowFolders { get { return _showFolders; } set { _showFolders = value; OnPropertyChanged("ShowFolders"); } }
    //////////////////    public event PropertyChangedEventHandler PropertyChanged;
    //////////////////    MainWindow _view;

    //////////////////    public Model(MainWindow view)
    //////////////////    {
    //////////////////        _view = view;
    //////////////////    }
    //////////////////    protected virtual void OnPropertyChanged(string propertyName)
    //////////////////    {
    //////////////////        int eL = -1;

    //////////////////        try
    //////////////////        {
    //////////////////            if (_view.dataTable == null) return;

    //////////////////            if (PropertyChanged != null)
    //////////////////            {
    //////////////////                PropertyChanged(
    //////////////////                    this,
    //////////////////                    new PropertyChangedEventArgs(propertyName)
    //////////////////                    );
    //////////////////            }

    //////////////////            if (propertyName == "Path" || propertyName == "ShowFiles" || propertyName == "ShowFolders")
    //////////////////            {
    //////////////////                _view.ClearCache();
    //////////////////                List<string> folderAndFiles = new List<string>();
    //////////////////                if (ShowFolders) folderAndFiles.AddRange(Directory.GetDirectories(Path).ToArray());
    //////////////////                if (ShowFiles) folderAndFiles.AddRange(Directory.GetFiles(Path).Where(x => System.IO.Path.GetFileName(x) != "Parameters_BuiltIn.xml" & System.IO.Path.GetFileName(x) != "Parameters_Project.xml" & System.IO.Path.GetFileName(x) != "Parameters_Shared.xml").ToArray());

    //////////////////                Files = folderAndFiles.ToArray();
    //////////////////            }
    //////////////////        }

    //////////////////        #region catch and finally
    //////////////////        catch (Exception ex)
    //////////////////        {
    //////////////////            _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("OnPropertyChanged, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
    //////////////////        }
    //////////////////        finally
    //////////////////        {
    //////////////////        }
    //////////////////        #endregion
    //////////////////    }
    //////////////////}

    public class CombiningViewModel
    {
        ////////////////////public Model model { get; set; }
        public Model2 model2 { get; set; }

        public CombiningViewModel Clone()
        {
            IFormatter formatter = new BinaryFormatter();
            using (MemoryStream stream = new MemoryStream())
            {
                formatter.Serialize(stream, this);
                stream.Seek(0, SeekOrigin.Begin);
                return (CombiningViewModel)formatter.Deserialize(stream);
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

    public class ToAvoidLoadingRevitDLLs
    {
        public ExternalCommandData commandData { get; set; }
        public string executionLocation { get; set; }
    }


    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //////////ExternalCommandData externalcommanddata;
        ////////////public string string_Execution_Location { get; set; }
        /// <summary>
        /// 
        /// </summary>

        ToAvoidLoadingRevitDLLs toavoidloadingrevitdlls;

        public Window_MoreParameters win_moreParam { get; set; }

        //public string myString_ServerDirectory = @"Q:\Revit Revit Revit\~Luminaires (do not edit directly)\";

        static int MAX_FILE_PATH = 260;
        Guid named_guid;


        public MainWindow(Guid ng, ToAvoidLoadingRevitDLLs tol)
        {
            toavoidloadingrevitdlls = tol;

            named_guid = ng;

              this.Top = Properties.Settings.Default.Top;
            this.Left = Properties.Settings.Default.Left;

            InitializeComponent();

            win_moreParam = new Window_MoreParameters(this);

            ProjectNameConverter converter = (ProjectNameConverter)this.FindResource("ProjectNameConverter");

            converter.window_main = this;

            combing_models = new CombiningViewModel
            {
                ////////////////////model = new Model(this),
                model2 = new Model2(this)
            };
            //this.DataContext = this;
            DataContext = combing_models;

            //  this.DataContext = this;
            //GridViewSort.ApplySort(lv_ReorderThis.Items, "Item1");

            UIDocument uidoc = toavoidloadingrevitdlls.commandData.Application.ActiveUIDocument;
            string s_NewAliasName = uidoc.Document.PathName;
            if (uidoc.Document.IsWorkshared) s_NewAliasName = ModelPathUtils.ConvertModelPathToUserVisiblePath(uidoc.Document.GetWorksharingCentralModelPath());
            s_NewAliasName = Path.GetFileNameWithoutExtension(s_NewAliasName);

            EstablishDirectories(true, s_NewAliasName);

            this.slider.Value = Properties.Settings.Default.DoubleSlider;
            buttonNewFromRevit.IsEnabled = true;
            buttonReselect.IsEnabled = true;

            textBoxSearch.Foreground = new SolidColorBrush(Colors.Gray);
            this.textBoxSearch.Text = "Type anything here ... to search"; //in three places

            this.textBoxSearch.GotFocus += (source, e) =>
            {
                if (this.textBoxSearch.Text == "Type anything here ... to search")
                {
                    this.textBoxSearch.Text = "";   //this needs to update in two places
                }
                this.textBoxSearch.Foreground = new SolidColorBrush(Colors.Black);
            };

            this.textBoxSearch.LostFocus += (source, e) =>
            {
                if (string.IsNullOrEmpty(this.textBoxSearch.Text))
                {
                    this.textBoxSearch.Text = "Type anything here ... to search"; //in three places
                    this.textBoxSearch.Foreground = new SolidColorBrush(Colors.Gray);
                }
            };

        }

        //named_guid focus3

        /// 
        ///
        /// 
        ///the main guide is the schedule
        ///5177 Foodstuffs Hornby DC Project Nest (Bottling Plant)
        ///5236 Argus Heating
        ///5147 St Lukes Church, Chch
        ///5113 New World Ravenswood - Revisions
        ///5076 invercargill
        ///5236 clutha community hub
        ///and how far back do we go, it shouldn't be a problem if we get the automation working
        ///.
        ///5177 Foodstuffs Hornby DC Project Nest (Bottling Plant)
        ///it stores the family and the revision (where to find it via revision control)
        ///the independant backup system is nice and fast we can use that
        ///the independant backup system is nice and fast we can use that
        ///the independant backup system is nice and fast we can use that
        ///.
        ///.
        ///please see Miro, and tasks 'weekend coding'
        ///.
        ///.
        /// now we are going, to get the select text understanding tags (which it currently doesn't on use of the main button)
        /// i don't think it matters much where that code comes from, but do some minimal research into miro to find it

        public void ClearCache()
        {
            FileToIconConverter fic = this.Resources["fic"] as FileToIconConverter;
            //Clear Thumbnail only, icon is not cleared.
            fic.ClearInstanceCache();
        }

        public CombiningViewModel combing_models { get; set; }
       /// public Model2 model2 { get; set; }

        public MainWindow()
        {

            named_guid = Guid.Parse("00000000-0000-0000-0000-000000000000");

            this.Top = Properties.Settings.Default.Top;
            this.Left = Properties.Settings.Default.Left;

            ///////////////////////////this.Resources.Add("ProjectNameConverter", new help.ProjectNameConverter(this));


            InitializeComponent();

            win_moreParam = new Window_MoreParameters(this);

            ProjectNameConverter converter = (ProjectNameConverter)this.FindResource("ProjectNameConverter");

            converter.window_main = this;

            combing_models = new CombiningViewModel
            {
                ////////////////////model = new Model(this),
                model2 = new Model2(this)
            };



            //this.DataContext = this;
            DataContext = combing_models;

            EstablishDirectories(true, "");


            this.slider.Value = Properties.Settings.Default.DoubleSlider;

            textBoxSearch.Foreground = new SolidColorBrush(Colors.Gray);
            this.textBoxSearch.Text = "Type anything here ... to search"; // three places , four places now

            this.textBoxSearch.GotFocus += (source, e) =>
            {
                if (this.textBoxSearch.Text == "Type anything here ... to search")
                {
                    this.textBoxSearch.Text = "";  //this needs to update in two places
                }

                this.textBoxSearch.Foreground = new SolidColorBrush(Colors.Black);
            };

            this.textBoxSearch.LostFocus += (source, e) =>
            {
                if (string.IsNullOrEmpty(this.textBoxSearch.Text))
                {
                    this.textBoxSearch.Text = "Type anything here ... to search";
                    this.textBoxSearch.Foreground = new SolidColorBrush(Colors.Gray);
                }
            };

            buttonNewFromRevit.Click -= new RoutedEventHandler(buttonNewFromRevit_Click);
            buttonNewFromRevit.Click += new RoutedEventHandler(buttonNewFromRevit_Click_Alternative);
        }

        private void EstablishDirectories(bool bool_Ask_Questionaire, string s_NewAliasName)
        {
            dict_SortOrdered_BuiltIn.Add(0, -1010105); //type commonent
            dict_SortOrdered_BuiltIn.Add(1, -1002001); //name
            dict_SortOrdered_BuiltIn.Add(2, -1002002); //family namme
            dict_SortOrdered_Shared.Add(3, Guid.Parse("301a1881-5639-4780-bddf-4f890402a275")); //classification
            dict_SortOrdered_Shared.Add(4, Guid.Parse("48ec2a3a-ee88-4c5d-a7fc-5db0ce7d867a"));//installation 
            dict_SortOrdered_Shared.Add(5, Guid.Parse("6d8dbef1-9a33-4f9e-96a5-bb0ce3e496d1"));//LIGHTING DESCRIPTION
            dict_SortOrdered_Shared.Add(6, Guid.Parse("1f4546a4-a8b0-4909-bbaa-30679c01d56f"));//control
            dict_SortOrdered_Shared.Add(7, Guid.Parse("eb7fae7e-0c08-4c11-a6c3-f5ecb7511b6b"));//CCT
            dict_SortOrdered_Shared.Add(8, Guid.Parse("fa202e01-e9c4-4618-9b72-027c0c4dae7b"));//ACCESSORIES
            dict_SortOrdered_Shared.Add(9, Guid.Parse("3096c531-fd26-4a6a-83fd-56ed56b84d65"));//MANUFACTURER
            dict_SortOrdered_Shared.Add(10, Guid.Parse("73bd9f05-97e0-4bac-ab36-fae30fd7d07f"));//OUTPUT
            dict_SortOrdered_Shared.Add(11, Guid.Parse("a97d17c8-c358-4001-8ad1-25837a373fb8"));//LIFETIME
            dict_SortOrdered_Shared.Add(12, Guid.Parse("3ea0ea38-8f4b-47c2-9e2d-ef1766446475"));//CATNO
            dict_SortOrdered_Shared.Add(13, Guid.Parse("8b319582-a1ed-4b26-a6e4-6dcfe2f69a55"));//CONNECTIONTYPE
            dict_SortOrdered_Shared.Add(14, Guid.Parse("92fa773e-5616-4577-958c-0906d49779b7"));//CONNECTIONTYPEDEPT
            dict_SortOrdered_Shared.Add(15, Guid.Parse("8d38b3ee-ece7-496a-880b-3fdcbbb9622c"));//DESCRIPTION
            dict_SortOrdered_Project.Add(16, "Connection Type");//connection type
            dict_SortOrdered_Project.Add(17, "Connection Type Dept");//connection type dept

            if(true)
            {
                if(!System.IO.Directory.Exists((Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\pkRevit")))
                {
                    System.IO.Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\pkRevit");
                }


                string FILE_NAME = (Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\pkRevit\\DropboxOrGoogleDriveOrOnedrive_Path.txt");

                if (!System.IO.File.Exists(FILE_NAME))
                {
                    System.IO.File.Create(FILE_NAME).Dispose();

                    System.IO.StreamWriter objWriter = new System.IO.StreamWriter(FILE_NAME, true);

                    objWriter.WriteLine("");
                    objWriter.Close();
                }
                System.IO.StreamReader objReader = new System.IO.StreamReader(FILE_NAME, true);
                Properties.Settings.Default.DropboxOrGoogleDriveOrOnedrive_Path = objReader.ReadLine();
                objReader.Close();
                Properties.Settings.Default.Save();
                Properties.Settings.Default.Reload();
            }

            bool bool_passthisone = false;

            if(true)
            {
                string string_MainDirectoryLocation = Properties.Settings.Default.DropboxOrGoogleDriveOrOnedrive_Path + @"\pkRevit Storage (do not edit directly)";
                bool bool_IfDirectoryExists = System.IO.Directory.Exists(string_MainDirectoryLocation);

                string string_DirectoryToCreate = string_MainDirectoryLocation + @"\Database File";
                bool bool_IfFileExists = System.IO.File.Exists(string_DirectoryToCreate + @"\Full_Index.db");

                if (bool_IfDirectoryExists & !bool_IfFileExists) bool_passthisone = true;
            }

            if (bool_Ask_Questionaire | bool_passthisone)
            {
                //
                if (false)  //candidate for methodasation 20210602
                {
                    string FILE_NAME = (Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\pkRevit\\DropboxOrGoogleDriveOrOnedrive_Path.txt");

                    System.IO.File.Create(FILE_NAME).Dispose();

                    System.IO.StreamWriter objWriter = new System.IO.StreamWriter(FILE_NAME, true);

                    objWriter.WriteLine(Properties.Settings.Default.DropboxOrGoogleDriveOrOnedrive_Path);
                    objWriter.Close();

                    ////System.IO.StreamReader objReader = new System.IO.StreamReader(FILE_NAME, true);
                    ////objReader.Close();
                }

                if (bool_passthisone | (!System.IO.Directory.Exists(Properties.Settings.Default.DropboxOrGoogleDriveOrOnedrive_Path + @"\pkRevit Storage (do not edit directly)")) | Properties.Settings.Default.DropboxOrGoogleDriveOrOnedrive_Path == "")
                {
                    bool bool_ManuallySetInitialDirectory = true;
                   
                    string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name.Split('\\').Last();
                    string userNameToMatch = "Joshua.Lumley2";
                 
                    if (userName == userNameToMatch)
                    {
                        MessageBoxResult result = System.Windows.MessageBox.Show("First time load...use Teams (recommended)?", "Use Teams?", System.Windows.MessageBoxButton.YesNoCancel);

                        if (result == MessageBoxResult.Yes)
                        {
                            Properties.Settings.Default.DropboxOrGoogleDriveOrOnedrive_Path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\Pedersen Read\PRearch❤️ - General";//\~Datasheet❤️ Expansion";
                            Properties.Settings.Default.Save();
                            Properties.Settings.Default.Reload();


                            if (true)  //candidate for methodasation 20210602
                            {
                                string FILE_NAME = (Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\pkRevit\\DropboxOrGoogleDriveOrOnedrive_Path.txt");

                                System.IO.File.Create(FILE_NAME).Dispose();

                                System.IO.StreamWriter objWriter = new System.IO.StreamWriter(FILE_NAME, true);

                                objWriter.WriteLine(Properties.Settings.Default.DropboxOrGoogleDriveOrOnedrive_Path);
                                objWriter.Close();

                            }

                            label_DropboxGoogleDriveOnedrive.Content = Properties.Settings.Default.DropboxOrGoogleDriveOrOnedrive_Path;

                            bool_ManuallySetInitialDirectory = false;
                        }
                    }
                    else if  /*candidate for methodisation 20210123*/ (System.IO.Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\Pedersen Read\~PRearch❤️ - General"))
                    {
                       // string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name.Split('\\').Last();
                       
                        if (userName == userNameToMatch)  
                        {
                            MessageBoxResult result = System.Windows.MessageBox.Show("First time load...use Teams (recommended)?", "Use Teams?", System.Windows.MessageBoxButton.YesNoCancel);

                            if (result == MessageBoxResult.Yes)
                            {
                                Properties.Settings.Default.DropboxOrGoogleDriveOrOnedrive_Path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\Pedersen Read\~PRearch❤️ - General";//\~Datasheet❤️ Expansion";
                                Properties.Settings.Default.Save();
                                Properties.Settings.Default.Reload();


                                if (true)  //candidate for methodasation 20210602
                                {
                                    string FILE_NAME = (Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\pkRevit\\DropboxOrGoogleDriveOrOnedrive_Path.txt");

                                    System.IO.File.Create(FILE_NAME).Dispose();

                                    System.IO.StreamWriter objWriter = new System.IO.StreamWriter(FILE_NAME, true);

                                    objWriter.WriteLine(Properties.Settings.Default.DropboxOrGoogleDriveOrOnedrive_Path);
                                    objWriter.Close();

                                }

                                label_DropboxGoogleDriveOnedrive.Content = Properties.Settings.Default.DropboxOrGoogleDriveOrOnedrive_Path;

                                bool_ManuallySetInitialDirectory = false;
                            }
                        }

                    }
                  
                    if (bool_ManuallySetInitialDirectory)
                    {
                       // cheat:
                        if (System.IO.Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\Dropbox"))
                        {
                            MessageBoxResult result = System.Windows.MessageBox.Show("First time load...use Dropbox?", "Use Dropbox?", System.Windows.MessageBoxButton.YesNoCancel);

                            if (result == MessageBoxResult.Yes)
                            {
                                Properties.Settings.Default.DropboxOrGoogleDriveOrOnedrive_Path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\Dropbox";
                                Properties.Settings.Default.Save();
                                Properties.Settings.Default.Reload();


                                if (true)  //candidate for methodasation 20210602
                                {
                                    string FILE_NAME = (Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\pkRevit\\DropboxOrGoogleDriveOrOnedrive_Path.txt");

                                    System.IO.File.Create(FILE_NAME).Dispose();

                                    System.IO.StreamWriter objWriter = new System.IO.StreamWriter(FILE_NAME, true);

                                    objWriter.WriteLine(Properties.Settings.Default.DropboxOrGoogleDriveOrOnedrive_Path);
                                    objWriter.Close();

                                }

                                label_DropboxGoogleDriveOnedrive.Content = Properties.Settings.Default.DropboxOrGoogleDriveOrOnedrive_Path;

                                bool_ManuallySetInitialDirectory = false;
                            }
                        }
                    }

                    if (bool_ManuallySetInitialDirectory)
                    {

                        Properties.Settings.Default.DropboxOrGoogleDriveOrOnedrive_Path = "";
                        Properties.Settings.Default.Save();
                        Properties.Settings.Default.Reload();


                        if (true)  //candidate for methodasation 20210602
                        {
                            string FILE_NAME = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\pkRevit\\DropboxOrGoogleDriveOrOnedrive_Path.txt";

                            System.IO.File.Create(FILE_NAME).Dispose();

                            System.IO.StreamWriter objWriter = new System.IO.StreamWriter(FILE_NAME, true);

                            objWriter.WriteLine(Properties.Settings.Default.DropboxOrGoogleDriveOrOnedrive_Path);
                            objWriter.Close();

                        }

                        setStoreDirectory();
                    }
                }
            
                if (!System.IO.Directory.Exists(Properties.Settings.Default.DropboxOrGoogleDriveOrOnedrive_Path))
                {
                    MessageBox.Show("The app will be closed because a database path must be available");
                    bool_ContinueOpening_DatabaseChecksOut = false;
                    this.Close();
                    return;
                }

                label_DropboxGoogleDriveOnedrive.Content = Properties.Settings.Default.DropboxOrGoogleDriveOrOnedrive_Path;
            }

            if (true)
            {
                string string_MainDirectoryLocation = Properties.Settings.Default.DropboxOrGoogleDriveOrOnedrive_Path + @"\pkRevit Storage (do not edit directly)";
                bool bool_IfDirectoryExists = System.IO.Directory.Exists(string_MainDirectoryLocation);

                string string_DirectoryToCreate = string_MainDirectoryLocation + @"\pkRevit Storage (do not edit directly)\Database File";
                bool bool_IfFileExists = System.IO.File.Exists(string_DirectoryToCreate + @"\Full_Index.db");

                //if (bool_IfDirectoryExists & !bool_IfFileExists) 

                if (!bool_IfDirectoryExists)
                {
                    MessageBoxResult result2 = System.Windows.MessageBox.Show("Initiating a blank database file...." + Environment.NewLine + Environment.NewLine, "Are you sure?", System.Windows.MessageBoxButton.YesNoCancel);

                    if (result2 == MessageBoxResult.Yes)
                    {
                        setStoreDirectory_MakeSubDirectories();
                    }
                    else
                    {
                        MessageBox.Show("The app will be closed because a database file must be available");
                        bool_ContinueOpening_DatabaseChecksOut = false;
                        this.Close();
                        return;
                    }
                }
                else
                {
                    setStoreDirectory_MakeSubDirectories();
                }
            }


            ///stuff stuff stuff, there are differences, differences, differences
            ///

            if (true)
            {
                string string_DirectoryToCreate = label_DropboxGoogleDriveOnedrive.Content + @"\pkRevit Storage (do not edit directly)\Database File";
                string string_Default_GuidToAlias = string_DirectoryToCreate + @"\Project GUID to Alias.xml";

                if (!File.Exists(string_Default_GuidToAlias))
                {
                    dict_GuidToAlias = new Dictionary<Guid, string>();
                    dict_GuidToAlias.Add(new Guid("{00000000-0000-0000-0000-000000000000}"), "All projects");

                    Stream stream = new FileStream(string_Default_GuidToAlias, FileMode.Create, FileAccess.Write);
                    DataContractSerializer serializer = new DataContractSerializer(typeof(Dictionary<Guid, string>));  //four of four
                    serializer.WriteObject(stream, dict_GuidToAlias); stream.Close();
                }

                ////////////string string_DirectoryToCreate = label_DropboxGoogleDriveOnedrive.Content + @"\pkRevit Storage (do not edit directly)\Database File";
                ////////////string string_Default_GuidToAlias = string_DirectoryToCreate + @"\Project GUID to Alias.xml";
                 
                if (true)
                {
                    Stream stream = new FileStream(string_Default_GuidToAlias, FileMode.Open, FileAccess.Read);
                    DataContractSerializer serializer = new DataContractSerializer(typeof(Dictionary<Guid, string>));   //one of four
                    dict_GuidToAlias = serializer.ReadObject(stream) as Dictionary<Guid, string>; stream.Close();  //two of four
                }

                ////_952_PRLoogleClassLibrary.DatabaseMethods.writeDebug(named_guid.ToString(), true);
                ////MessageBox.Show("we got here");

                if(true)  //candidate for methodisation 20210516
                {

                    if (!dict_GuidToAlias.ContainsKey(named_guid))
                    {
                        this.Topmost = false;

                        if (s_NewAliasName == "")
                        {
                            s_NewAliasName = Microsoft.VisualBasic.Interaction.InputBox("Please enter a name....", "This is the first time we've saved from this Project.", s_NewAliasName, -1, -1);
                        }
                        else
                        {
                            s_NewAliasName = Microsoft.VisualBasic.Interaction.InputBox("Happy with this name?", "This is the first time we've saved from this Project.", s_NewAliasName, -1, -1);
                        }
                        this.Topmost = true;

                        if (s_NewAliasName == "") s_NewAliasName = "Revit Project Alias Name";

                        dict_GuidToAlias.Add(named_guid, s_NewAliasName);
                        Stream stream = new FileStream(string_Default_GuidToAlias, FileMode.Create, FileAccess.Write);
                        DataContractSerializer serializer = new DataContractSerializer(typeof(Dictionary<Guid, string>));  //four of four
                        serializer.WriteObject(stream, dict_GuidToAlias); stream.Close();

                        Properties.Settings.Default.str_carryOverGuid = named_guid;
                        Properties.Settings.Default.Save();
                        Properties.Settings.Default.Reload();
                    }
                }

                ComboBoxProjectFilter.ItemsSource = dict_GuidToAlias;
                ComboBoxProjectFilter.DisplayMemberPath = "Value";
                ComboBoxProjectFilter.SelectedIndex = 0;


                if (true) //candidate for methodisation 202103031123
                {
                    if (true)
                    {
                        Stream stream = new FileStream(string_Default_GuidToAlias, FileMode.Open, FileAccess.Read);
                        DataContractSerializer serializer = new DataContractSerializer(typeof(Dictionary<Guid, string>));   //one of four
                        dict_GuidToAlias = serializer.ReadObject(stream) as Dictionary<Guid, string>; stream.Close();  //two of four
                    }

                    List<KeyValuePair<Guid, string>> listKPV = dict_GuidToAlias.Where(x => x.Key != new Guid("{00000000-0000-0000-0000-000000000000}")).OrderBy(x => x.Value).ToList();
                    listKPV.Insert(0, new KeyValuePair<Guid, string>(new Guid("{00000000-0000-0000-0000-000000000000}"), dict_GuidToAlias[new Guid("{00000000-0000-0000-0000-000000000000}")]));
                    dict_GuidToAlias = listKPV.ToDictionary(x => x.Key, x => x.Value);//in two places 20210225


                    lb_Explorer.ItemsSource = dict_GuidToAlias;

                    lb_Explorer.DisplayMemberPath = "Value";

                    foreach (KeyValuePair<Guid, string> t_gs in dict_GuidToAlias)
                    {
                        if (t_gs.Key == str_carryOverGuid)
                        {
                            lb_Explorer.SelectedItem = t_gs;
                        }
                    }

                    if (lb_Explorer.SelectedIndex == -1) lb_Explorer.SelectedIndex = 0;
                }
            }
            // ComboBoxProjectFilter.UpdateLayout();
            // ComboBoxProjectFilter.InvalidateVisual();
        }


        public Dictionary<Guid, string> dict_GuidToAlias = null;
        public bool bool_ContinueOpening_DatabaseChecksOut { get; set; } = true;
        private System.Windows.Documents.Hyperlink return_Hyperlink(string s_root, string s_showText)
        {
            var h1 = new System.Windows.Documents.Hyperlink();
            h1.NavigateUri = new Uri("http://" + s_root);
            h1.RequestNavigate += new System.Windows.Navigation.RequestNavigateEventHandler(Hyperlink_RequestNavigate);
            h1.Inlines.Add(s_showText);
            h1.Foreground = System.Windows.Media.Brushes.Green;
            return h1;
        }

        string str_CurrentModeOfExplorer = "ModeProject";

        private void RequestNavigate_AndStart()
        {
            ///this lists up but only actually only ModeSchedule does a custom thing, but I expect in soon 
            ///

            ///if we're going to do string work should it be before or after the relisting stuff

            List<DataRow> listDataRow = dataTable.Rows.Cast<DataRow>().ToList();

            switch (str_CurrentModeOfExplorer) /// str_CurrentModeOfExplorer = "ModeCategory"
            {
                case "ModeProject":
                    //lb_Explorer.ItemsSource = dict_GuidToAlias.Values;

                    if (true)
                    {
                        string string_DirectoryToCreate = label_DropboxGoogleDriveOnedrive.Content + @"\pkRevit Storage (do not edit directly)\Database File";
                        string string_Default_GuidToAlias = string_DirectoryToCreate + @"\Project GUID to Alias.xml";

                        Stream stream = new FileStream(string_Default_GuidToAlias, FileMode.Open, FileAccess.Read);
                        DataContractSerializer serializer = new DataContractSerializer(typeof(Dictionary<Guid, string>));   //one of four
                        dict_GuidToAlias = serializer.ReadObject(stream) as Dictionary<Guid, string>; stream.Close();  //two of four
                    }

                    List<KeyValuePair<Guid, string>> listKPV = dict_GuidToAlias.Where(x => x.Key != new Guid("{00000000-0000-0000-0000-000000000000}")).OrderBy(x => x.Value).ToList();
                    listKPV.Insert(0, new KeyValuePair<Guid, string>(new Guid("{00000000-0000-0000-0000-000000000000}"), dict_GuidToAlias[new Guid("{00000000-0000-0000-0000-000000000000}")]));
                    dict_GuidToAlias = listKPV.ToDictionary(x => x.Key, x => x.Value);//in two places 20210225

                    lb_Explorer.ItemsSource = dict_GuidToAlias;

                    lb_Explorer.DisplayMemberPath = "Value";

                    foreach (KeyValuePair<Guid, string> t_gs in dict_GuidToAlias)
                    {
                        if (t_gs.Key == str_carryOverGuid)
                        {
                            lb_Explorer.SelectedItem = t_gs;
                        }
                    }

                    if (lb_Explorer.SelectedIndex == -1) lb_Explorer.SelectedIndex = 0;

                    ///////////////rowFilter(bool_IsolationIsOn, false);
                    //MessageBox.Show("DID IT UPDATE");

                    ModeProjectHyperLinks_AllSchedules(true);
                    ///more of this more of this

                    // lb_Explorer.SelectedItem = dict_GuidToAlias.ToList().Where(x => x.Key == str_carryOverGuid).First(); //  new Tuple<Guid, string>(str_carryOverGuid, dict_GuidToAlias[str_carryOverGuid]);

                    break;

                case "ModeCategory":

                    List<string> listString = new List<string>() { "ALL Categoreis", "Lighting Fixtures", "Lighting Devices", "Electrical Fixtures", "Electrical Equipment", "Communication Devices", "Data Devices", "Security Devices", "Cable Trays" };

                    lb_Explorer.ItemsSource = listString;
                    lb_Explorer.DisplayMemberPath = "";

                    string string_SaveCategory = "All categories";
                    if (RadioButton_CD.IsChecked.Value) string_SaveCategory = "Communication Devices";  //in three places search for turnoffevents category
                    if (RadioButton_DD.IsChecked.Value) string_SaveCategory = "Data Devices";
                    if (RadioButton_EE.IsChecked.Value) string_SaveCategory = "Electrical Equipment";
                    if (RadioButton_EF.IsChecked.Value) string_SaveCategory = "Electrical Fixtures";
                    if (RadioButton_LD.IsChecked.Value) string_SaveCategory = "Lighting Devices";
                    if (RadioButton_LF.IsChecked.Value) string_SaveCategory = "Lighting Fixtures";
                    if (RadioButton_CT.IsChecked.Value) string_SaveCategory = "Cable Trays";
                    if (RadioButton_SD.IsChecked.Value) string_SaveCategory = "Security Devices";
                    if (RadioButton_ALL.IsChecked.Value) string_SaveCategory = "All categories";


                    foreach (string s in listString)
                    {
                        if (s == string_SaveCategory)
                        {
                            lb_Explorer.SelectedItem = s;
                        }
                    }

                    if (lb_Explorer.SelectedIndex == -1) lb_Explorer.SelectedIndex = 0;

                    break;

                case "ModeSchedule":

                    ///

                    if (str_carryOverGuid == new Guid("{00000000-0000-0000-0000-000000000000}"))
                    {
                        listDataRow = listDataRow.GroupBy(x => x["ScheduleID"]).Select(x => x.First()).ToList();
                    }
                    else
                    {
                        listDataRow = listDataRow.Where(x => (string)x["ProjectGUID"] == str_carryOverGuid.ToString()).GroupBy(x => x["ScheduleID"]).Select(x => x.First()).ToList();
                    }

                    ///this is loading up the explorer view well done
                    ///we now need to navigate to the part where we load up the masterlist with the values it should know to be when set at ALL Schedule

                    List<Tuple<Int64, string>> listTuple_String = listDataRow.Select(x => new Tuple<Int64, string>((Int64)x["ScheduleID"], (string)x["ScheduleName"])).OrderBy(x => x.Item2).ToList();

                    listTuple_String.Insert(0, new Tuple<Int64, string>((Int64)0, "ALL Schedules"));

                    lb_Explorer.ItemsSource = listTuple_String;

                    //   ((List<Tuple<Int64, string>>)lb_Explorer.ItemsSource).Insert(0, new Tuple<Int64, string>((Int64)0, "ALL Schedules"));

                    lb_Explorer.DisplayMemberPath = "Item2";

                    foreach (Tuple<Int64, string> t_is in lb_Explorer.ItemsSource)
                    {
                        if (t_is.Item1 == str_carryOverScheduleId)
                        {
                            lb_Explorer.SelectedItem = t_is;
                        }
                    }

                    if (lb_Explorer.SelectedIndex == -1) lb_Explorer.SelectedIndex = 0;

                    break;
            }
        }

        private void btn_delete_explorer_Click(object sender, RoutedEventArgs e)
        {
            int eL = -1;

            try
            {
                if (lb_Explorer.SelectedItems.Count == 0) return;
                if (lb_Explorer.SelectedIndex == 0) return;
                if (str_CurrentModeOfExplorer != "ModeProject") return;

                string string_More = ((KeyValuePair<Guid, string>)lb_Explorer.SelectedItems[0]).Key.ToString();

                List<DataRow> listDataRow = dataTable.Rows.Cast<DataRow>().ToList();

                int myIntCount = listDataRow.Where(x => x["ProjectGUID"].ToString() == string_More).Count();

                if (myIntCount != 0)
                {
                    MessageBox.Show("Please remove '" + myIntCount + "' rows before deleting project.");
                    return;
                }

                string string_DirectoryToCreate = label_DropboxGoogleDriveOnedrive.Content + @"\pkRevit Storage (do not edit directly)\Database File";
                string string_Default_GuidToAlias = string_DirectoryToCreate + @"\Project GUID to Alias.xml";

                dict_GuidToAlias.Remove(((KeyValuePair<Guid, string>)lb_Explorer.SelectedItems[0]).Key);
                Stream stream = new FileStream(string_Default_GuidToAlias, FileMode.Create, FileAccess.Write);
                DataContractSerializer serializer = new DataContractSerializer(typeof(Dictionary<Guid, string>));  //four of four
                serializer.WriteObject(stream, dict_GuidToAlias); stream.Close();

                textBoxSearch.Text = "";
                RemoveFilters_UsedOnUnisolate_And_RemoveFilterButton();
                textBoxSearch.Focus();
                textBoxSearch.SelectAll();

            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("btn_delete_explorer_Click, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        private void btn_rename_explorer_Click(object sender, RoutedEventArgs e)
        {
            int eL = -1;

            try
            {
                ///in what way is the below 

                if (lb_Explorer.SelectedItems.Count == 0) return;
                if (str_CurrentModeOfExplorer != "ModeProject") return;

                ///string s_NewAliasName = ((KeyValuePair<Guid, string>)lb_Explorer.SelectedItems[0]).Key;
                string s_NewAliasName = ((KeyValuePair<Guid, string>)lb_Explorer.SelectedItems[0]).Value;

                string string_DirectoryToCreate = label_DropboxGoogleDriveOnedrive.Content + @"\pkRevit Storage (do not edit directly)\Database File";
                string string_Default_GuidToAlias = string_DirectoryToCreate + @"\Project GUID to Alias.xml";

                ////////MessageBox.Show(dict_GuidToAlias[((KeyValuePair<Guid, string>)lb_Explorer.SelectedItems[0]).Key]);

                string string_CheckingThereWasInFactTheSame = s_NewAliasName;

                
                bool bool_ResetTopMost = false;
                if(this.Topmost)
                {
                    this.Topmost = false;
                    bool_ResetTopMost = true;
                }

                s_NewAliasName = Microsoft.VisualBasic.Interaction.InputBox("Please rename.", "Renaming", s_NewAliasName, -1, -1);
                if(bool_ResetTopMost) this.Topmost = true;
                   

                if (s_NewAliasName == "") return;
                if (s_NewAliasName == string_CheckingThereWasInFactTheSame) return;
              

                if (true)
                {
                    Guid guid_RenamingThisOne = ((KeyValuePair<Guid, string>)lb_Explorer.SelectedItems[0]).Key;
                    dict_GuidToAlias[guid_RenamingThisOne] = s_NewAliasName;

                    str_carryOverGuid = guid_RenamingThisOne;

                    //in two places start
                    List<KeyValuePair<Guid, string>> listKPV = dict_GuidToAlias.Where(x => x.Key != new Guid("{00000000-0000-0000-0000-000000000000}")).OrderBy(x => x.Value).ToList();
                    listKPV.Insert(0, new KeyValuePair<Guid, string>(new Guid("{00000000-0000-0000-0000-000000000000}"), dict_GuidToAlias[new Guid("{00000000-0000-0000-0000-000000000000}")]));
                    dict_GuidToAlias = listKPV.ToDictionary(x => x.Key, x => x.Value);//in two places 20210225
                    //in two places finish

                    Stream stream = new FileStream(string_Default_GuidToAlias, FileMode.Create, FileAccess.Write);
                    DataContractSerializer serializer = new DataContractSerializer(typeof(Dictionary<Guid, string>));  //four of four
                    serializer.WriteObject(stream, dict_GuidToAlias); stream.Close();
                }


                if(true) //candidate for methodisation 202103031123
                {
                    if (true)
                    {
                        Stream stream = new FileStream(string_Default_GuidToAlias, FileMode.Open, FileAccess.Read);
                        DataContractSerializer serializer = new DataContractSerializer(typeof(Dictionary<Guid, string>));   //one of four
                        dict_GuidToAlias = serializer.ReadObject(stream) as Dictionary<Guid, string>; stream.Close();  //two of four
                    }


                    lb_Explorer.ItemsSource = dict_GuidToAlias;

                    lb_Explorer.DisplayMemberPath = "Value";

                    foreach (KeyValuePair<Guid, string> t_gs in dict_GuidToAlias)
                    {
                        if (t_gs.Key == str_carryOverGuid)
                        {
                            lb_Explorer.SelectedItem = t_gs;
                        }
                    }

                    if (lb_Explorer.SelectedIndex == -1) lb_Explorer.SelectedIndex = 0;
                }

                lv_MasterList.Items.Refresh();


            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("btn_rename_explorer_Click, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            str_CurrentModeOfExplorer = e.Uri.OriginalString.Substring(7);
            //str_CurrentModeOfExplorer = string_Level;

            RequestNavigate_AndStart();

            e.Handled = true;
        }

        private string[] stringArray2()
        {
            Drawing textBlockDrawing = VisualTreeHelper.GetDrawing(tb_tree);
            var sb = new StringBuilder();
            WalkDrawingForText(sb, textBlockDrawing);

            return sb.ToString().Split(new string[] { " , " }, StringSplitOptions.None);
        }

        private void buttonRemoveFilters_Click(object sender, RoutedEventArgs e)
        {
            int eL = -1;

            try
            {

                RemoveFilters_UsedOnUnisolate_And_RemoveFilterButton();
                textBoxSearch.Focus();
                textBoxSearch.SelectAll();
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("buttonRemoveFilters_Click, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        private void buttonHome_Click(object sender, RoutedEventArgs e)
        {
            int eL = -1;

            try
            {
                textBoxSearch.Text = "";
                RemoveFilters_UsedOnUnisolate_And_RemoveFilterButton();
                textBoxSearch.Focus();
                textBoxSearch.SelectAll();
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("buttonRemoveFilters_Click, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        private void RemoveFilters_UsedOnUnisolate_And_RemoveFilterButton()
        {
            str_CurrentModeOfExplorer = "ModeProject";
            str_carryOverGuid = Guid.Parse("00000000-0000-0000-0000-000000000000");
            //lbl_ScheduleName_Project.Content = "All projects";
            lbl_ScheduleName.Content = "ALL Schedules";
            str_carryOverScheduleId = 0;
            str_carryOverScheduleId_string = "ALL Schedules"; //in two places

            /////////lb_Explorer.SelectedIndex = -1;

            RequestNavigate_AndStart();

            tb_tree.Text = "";

            ModeProjectHyperLinks_AllSchedules(true);


        }

        private void ModeProjectHyperLinks_AllSchedules(bool bool_ModeViaExplorer)
        {
            ///str_carryOverGuid = ((KeyValuePair<Guid, string>)lb_Explorer.SelectedItems[0]).Key;
            ///lbl_ScheduleName_Project.Content = ((KeyValuePair<Guid, string>)lb_Explorer.SelectedItems[0]).Value;

            foreach (KeyValuePair<Guid, string> kpv in dict_GuidToAlias)
            {
                if (kpv.Key == str_carryOverGuid)
                {
                    ComboBoxProjectFilter.SelectedItem = kpv;
                }
            }

            tb_tree.Inlines.Add(return_Hyperlink("ModeProject", dict_GuidToAlias[str_carryOverGuid]));  //stringArray[0].Trim()
            tb_tree.Inlines.Add(" , ");

            //////do not delete//////////tb_tree.Inlines.Add(return_Hyperlink("ModeCategory", "ALL Categories"));
            //////do not delete//////////tb_tree.Inlines.Add(" , ");

            tb_tree.Inlines.Add(return_Hyperlink("ModeSchedule", "ALL Schedules"));

            //str_carryOverGuid = new Guid("{00000000-0000-0000-0000-000000000000}");
            RadioButton_ALL.Checked -= RadioButton_ALL_Checked;
            RadioButton_ALL.IsChecked = true;
            RadioButton_ALL.Checked += RadioButton_ALL_Checked;

            string_checkedRadioButton = "";

            str_carryOverScheduleId = 0;
            str_carryOverScheduleId_string = "ALL Schedules";

            rowFilter(bool_IsolationIsOn, bool_ModeViaExplorer);
        }
        private void rowFilter(bool UseScheduleIsolation, bool UseScheduleIsolation_viaExplorer)
        {
            string stringFilter = "";

            if (((KeyValuePair<Guid, string>)ComboBoxProjectFilter.SelectedItem).Key != new Guid("{00000000-0000-0000-0000-000000000000}"))
            {
                stringFilter = "ProjectGUID LIKE '%" + ((KeyValuePair<Guid, string>)ComboBoxProjectFilter.SelectedItem).Key.ToString() + "%'";
            }
            // if (lv_MasterList.SelectedIndex == -1 & lv_MasterList.Items.Count > 0) lv_MasterList.SelectedIndex = 0;

            UseScheduleIsolation = true;

            if (UseScheduleIsolation)
            {
                //string guid = label_ProjectGUID.Content.ToString();

                if (UseScheduleIsolation_viaExplorer)
                {
                    /// clicking in it brings the one level up to All and selects it in the explorer

                    ////////if (str_CurrentModeOfExplorer != "ModeCategory")
                    ////////{
                    ////////    str_carryOverScheduleId = (Int64)((Tuple<Int64, string>)lb_Explorer.SelectedItem).Item1;
                    ////////    str_carryOverScheduleId_string = (string)((Tuple<Int64, string>)lb_Explorer.SelectedItem).Item2;

                    ////////    lbl_ScheduleName.Content = str_carryOverScheduleId_string;
                    ////////}

                    ///
                    if (str_carryOverScheduleId != 0)
                    {
                        if (stringFilter != "") stringFilter = stringFilter + " AND ";
                        stringFilter = stringFilter + "ScheduleID = " + str_carryOverScheduleId;
                    }
                }
                else
                {
                    if (lv_MasterList.SelectedIndex != -1)
                    {
                        str_carryOverScheduleId = (Int64)((DataRowView)lv_MasterList.SelectedItem)["ScheduleID"];
                        str_carryOverGuid = Guid.Parse(((DataRowView)lv_MasterList.SelectedItem)["ProjectGUID"].ToString());
                        str_carryOverScheduleId_string = ((DataRowView)lv_MasterList.SelectedItem)["ScheduleName"].ToString();

                        lbl_ScheduleName.Content = str_carryOverScheduleId_string;
                        //lbl_ScheduleName_Project.Content = dict_GuidToAlias[str_carryOverGuid];
                    }

                    if (str_carryOverScheduleId != 0)
                    {
                        if (stringFilter != "") stringFilter = stringFilter + " AND ";
                        stringFilter = stringFilter + "ScheduleID = " + str_carryOverScheduleId;
                    }

                    //////if (stringFilter != "") stringFilter = stringFilter + " AND ";
                    //////stringFilter = stringFilter + "ProjectGUID LIKE '%" + str_carryOverGuid + "%'";
                }
            }

            if (textBoxSearch.Text != "" & textBoxSearch.Text != "Type anything here ... to search") //in three places
            {
                if (stringFilter != "") stringFilter = stringFilter + " AND ";

                string[] stringArray = textBoxSearch.Text.Split(' ');


                //string stringFilter = "SearchTerms LIKE '%" + stringArray[0].Replace("'", "''") + "%'";  //here is a like arrow that is not implemented on our search string
                stringFilter = stringFilter + "SearchTerms LIKE '%" + stringArray[0].Replace("'", "''") + "%'";  //here is a like arrow that is not implemented on our search string
                                                                                                                 //we can check that it gets to here
                foreach (string str in textBoxSearch.Text.Split(' ').Skip(1))
                {
                    if (str.Trim() != "") stringFilter = stringFilter + " AND SearchTerms LIKE '%" + str + "%'";
                }

                //MessageBox.Show("we're hitting here right");
            }

            if (string_checkedRadioButton != "")
            {
                if (stringFilter != "") stringFilter = stringFilter + " AND ";
                stringFilter = stringFilter + "Category = '" + string_checkedRadioButton + "'";
            }

            dataTable.DefaultView.RowFilter = stringFilter;//implication of one text filter

            if (lv_MasterList.Items.Count == 0)
            {
                textBlock_NoResults.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                textBlock_NoResults.Visibility = System.Windows.Visibility.Hidden;
            }

            if (lv_MasterList.SelectedItem != null)
            {
                lv_MasterList.ScrollIntoView(lv_MasterList.SelectedItem);
            } else
            {
                //combing_models.model.Path = "";
                combing_models.model2.Path = "";
            }

            bool_IsolationIsOn = false;

            // if (lv_MasterList.SelectedIndex == -1 & lv_MasterList.Items.Count > 0) lv_MasterList.SelectedIndex = 0;
        }

        private void IsolateButton_Or_DoubleClick()
        {
            if (lv_MasterList.SelectedIndex != -1)
            {

                ///////////////////////////textBoxSearch.Text = "";

                str_CurrentModeOfExplorer = "ModeSchedule";
                str_carryOverScheduleId = (Int64)((DataRowView)lv_MasterList.SelectedItem)["ScheduleID"];
                str_carryOverGuid = Guid.Parse(((DataRowView)lv_MasterList.SelectedItem)["ProjectGUID"].ToString());
                str_carryOverScheduleId_string = ((DataRowView)lv_MasterList.SelectedItem)["ScheduleName"].ToString();


                lbl_ScheduleName.Content = str_carryOverScheduleId_string;
                //lbl_ScheduleName_Project.Content = dict_GuidToAlias[str_carryOverGuid];

                RequestNavigate_AndStart();

                tb_tree.Text = "";

                bool_IsolationIsOn = true;
                ModeProjectHyperLinks_IndividualSchedule();
            }
            else
            {
                MessageBox.Show("Please select an item first.");
            }
        }

        private void btn_Isolate_Item_Click(object sender, RoutedEventArgs e)
        {
            int eL = -1;

            try
            {
                IsolateButton_Or_DoubleClick();
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("btn_Isolate_Item_Click, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }
        private void ModeProjectHyperLinks_IndividualSchedule()
        {

            //////////////string[] stringArray = stringArray2();

            //////////////lbl_ScheduleName.Content = ((Tuple<Int64, string>)lb_Explorer.SelectedItems[0]).Item2;  // "ALL Schedules";

            tb_tree.Inlines.Add(return_Hyperlink("ModeProject", dict_GuidToAlias[str_carryOverGuid]));  //stringArray[0].Trim()
            tb_tree.Inlines.Add(" , ");

            ////////////tb_tree.Inlines.Add(return_Hyperlink("ModeCategory", "ALL Categories"));
            ////////////tb_tree.Inlines.Add(" , ");

            RadioButton_ALL.Checked -= RadioButton_ALL_Checked;
            RadioButton_ALL.IsChecked = true;
            RadioButton_ALL.Checked += RadioButton_ALL_Checked;
            string_checkedRadioButton = "";

            tb_tree.Inlines.Add(return_Hyperlink("ModeSchedule", str_carryOverScheduleId_string));  //stringArray[2].Trim()

           // bool_IsolationIsOn = true;
            rowFilter(bool_IsolationIsOn, true);
        }
        private void ListBoxItemExplorer_PreviewMouseUp(object sender, MouseButtonEventArgs e)  ///i say it should be selected first that way this will work
        {
            int eL = -1;

            try
            {
                if (lb_Explorer.SelectedItems.Count == 0) return;

                tb_tree.Text = "";

                switch (str_CurrentModeOfExplorer)
                {
                    case "ModeProject":  //the easiest thing to do is to manually select the first time then run this

                        str_carryOverGuid = ((KeyValuePair<Guid, string>)lb_Explorer.SelectedItems[0]).Key;
                        //lbl_ScheduleName_Project.Content = ((KeyValuePair<Guid, string>)lb_Explorer.SelectedItems[0]).Value;
                        lbl_ScheduleName.Content = "ALL Schedules";
                        str_carryOverScheduleId = 0;
                        str_carryOverScheduleId_string = "ALL Schedules";  //in two places

                        ModeProjectHyperLinks_AllSchedules(true);
                        //////////ModeProjectHyperLinks_IndividualSchedule();
                        break;

                    case "ModeCategory":

                        #region 
                        string[] stringArray = stringArray2();

                        string string_ChangingForThis = lb_Explorer.SelectedItems[0].ToString();

                        switch (string_ChangingForThis)  //happens in two places the other place is in closing
                        {
                            case "Communication Devices": RadioButton_CD.IsChecked = true; break;
                            case "Data Devices": RadioButton_DD.IsChecked = true; break;
                            case "Electrical Equipment": RadioButton_EE.IsChecked = true; break;
                            case "Electrical Fixtures": RadioButton_EF.IsChecked = true; break;
                            case "Lighting Devices": RadioButton_LD.IsChecked = true; break;
                            case "Lighting Fixtures": RadioButton_LF.IsChecked = true; break;
                            case "Cable Trays": RadioButton_CT.IsChecked = true; break;
                            case "Security Devices": RadioButton_SD.IsChecked = true; break;
                            default: RadioButton_ALL.IsChecked = true; break;
                        }

                        tb_tree.Inlines.Add(return_Hyperlink("ModeProject", stringArray[0].Trim()));  //stringArray[0].Trim()
                        tb_tree.Inlines.Add(" , ");

                        tb_tree.Inlines.Add(return_Hyperlink("ModeCategory", string_ChangingForThis));  // stringArray[1].Trim()
                        tb_tree.Inlines.Add(" , ");

                        //////////tb_tree.Inlines.Add(return_Hyperlink("ModeSchedule", stringArray[2].Trim()));  //stringArray[2].Trim()

                        //////tb_tree.Inlines.Add(return_Hyperlink("ModeCategory", "ALL Categories"));
                        //////tb_tree.Inlines.Add(" , ");

                        tb_tree.Inlines.Add(return_Hyperlink("ModeSchedule", "ALL Schedules"));


                        //str_carryOverGuid = new Guid("{00000000-0000-0000-0000-000000000000}");
                        //////RadioButton_ALL.Checked -= RadioButton_ALL_Checked;
                        //////RadioButton_ALL.IsChecked = true;
                        //////RadioButton_ALL.Checked += RadioButton_ALL_Checked;

                        str_carryOverScheduleId = 0;
                        str_carryOverScheduleId_string = "ALL Schedules";

                        bool_IsolationIsOn = true;
                        rowFilter(bool_IsolationIsOn, true);
                        #endregion not used at all
                        break;

                    case "ModeSchedule":

                        str_carryOverScheduleId_string = (string)((Tuple<Int64, string>)lb_Explorer.SelectedItem).Item2;
                        str_carryOverScheduleId = ((Tuple<Int64, string>)lb_Explorer.SelectedItems[0]).Item1;
                        bool_IsolationIsOn = true;

                        if(str_carryOverScheduleId != 0)
                        {
                            ModeProjectHyperLinks_IndividualSchedule();
                        } else
                        {
                            ModeProjectHyperLinks_AllSchedules(true);
                        }

                        break;
                }
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("ListBoxItemExplorer_PreviewMouseUp, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }
        private static void WalkDrawingForText(StringBuilder sb, Drawing d)
        {
            var glyphs = d as GlyphRunDrawing;
            if (glyphs != null)
            {
                sb.Append(glyphs.GlyphRun.Characters.ToArray());
            }
            else
            {
                var g = d as DrawingGroup;
                if (g != null)
                {
                    foreach (Drawing child in g.Children)
                    {
                        WalkDrawingForText(sb, child);
                    }
                }
            }
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
        //////private void list_MouseDown(object sender, MouseButtonEventArgs e)
        //////{
        //////    int eL = -1;

        //////    try
        //////    {
        //////        if (e.ClickCount == 2 && e.LeftButton == MouseButtonState.Pressed)
        //////        {
        //////            if (lb_listBox.SelectedValue is string)
        //////            {
        //////                string dir = lb_listBox.SelectedValue as string;
        //////                if (Directory.Exists(dir))
        //////                {
        //////                    model.Path = dir;
        //////                    btn_OneLevelUp.IsEnabled = false;
        //////                    e.Handled = true;
        //////                }
        //////            }
        //////        }
        //////    }
        //////    #region catch and finally
        //////    catch (Exception ex)
        //////    {
        //////        _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("list_MouseDown, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
        //////    }
        //////    finally
        //////    {
        //////    }
        //////    #endregion
        //////}

        private void Open_Click_New(object sender, RoutedEventArgs e)
        {
            int eL = -1;

            if (lv_MasterList.SelectedItems.Count == 0) return;

            try
            {
                string string_DirectoryToCreate_Storage = method_Storage_DBFile_Directory() + @"\Admin Storage";
                string myString_DataStore = string_DirectoryToCreate_Storage + "\\" + ((DataRowView)lv_MasterList.SelectedItem)["FolderName"];

                string path = ((KeyValuePair<string, Bitmap>)listbox_objectmodel.SelectedItem).Key;
                path = myString_DataStore + "\\" + path;

                System.Diagnostics.Process.Start(path);
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("Open_Click_New, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        ////////////////////private void Open_Click(object sender, RoutedEventArgs e)
        ////////////////////{
        ////////////////////    int eL = -1;

        ////////////////////    try
        ////////////////////    {
        ////////////////////        System.Diagnostics.Process.Start((string)lb_listBox.SelectedItem);
        ////////////////////    }

        ////////////////////    #region catch and finally
        ////////////////////    catch (Exception ex)
        ////////////////////    {
        ////////////////////        _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("Open_Click, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
        ////////////////////    }
        ////////////////////    finally
        ////////////////////    {
        ////////////////////    }
        ////////////////////    #endregion
        ////////////////////}

        private void Openwith_Click_New(object sender, RoutedEventArgs e)
        {
            int eL = -1;
            if (lv_MasterList.SelectedItems.Count == 0) return;
            try
            {
                string string_DirectoryToCreate_Storage = method_Storage_DBFile_Directory() + @"\Admin Storage";
                string myString_DataStore = string_DirectoryToCreate_Storage + "\\" + ((DataRowView)lv_MasterList.SelectedItem)["FolderName"];

                string path = ((KeyValuePair<string, Bitmap>)listbox_objectmodel.SelectedItem).Key;
                path = myString_DataStore + "\\" + path;

                System.Diagnostics.Process.Start("rundll32.exe", "shell32.dll, OpenAs_RunDLL " + path);
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

        ////////////////////private void Openwith_Click(object sender, RoutedEventArgs e)
        ////////////////////{
        ////////////////////    int eL = -1;

        ////////////////////    try
        ////////////////////    {
        ////////////////////        System.Diagnostics.Process.Start("rundll32.exe", "shell32.dll, OpenAs_RunDLL " + (string)lb_listBox.SelectedItem);
        ////////////////////    }

        ////////////////////    #region catch and finally
        ////////////////////    catch (Exception ex)
        ////////////////////    {
        ////////////////////        _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("Openwith_Click, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
        ////////////////////    }
        ////////////////////    finally
        ////////////////////    {
        ////////////////////    }
        ////////////////////    #endregion
        ////////////////////}

        private void Open_Containing_Folder_New(object sender, RoutedEventArgs e)
        {
            int eL = -1;
            if (lv_MasterList.SelectedItems.Count == 0) return;
            try
            {
                ////////////////////string string_DirectoryToCreate_Storage = method_Storage_DBFile_Directory() + @"\Admin Storage";
                ////////////////////string myString_DataStore = string_DirectoryToCreate_Storage + "\\" + ((DataRowView)lv_MasterList.SelectedItem)["FolderName"];

                ////////////////////string path = ((KeyValuePair<string, Bitmap>)listbox_objectmodel.SelectedItem).Key;
                ////////////////////path = myString_DataStore + "\\" + path;

                ////////////////////path = Path.GetDirectoryName(path);

                ////////////////////System.Diagnostics.Process.Start(path);

                #region catch and finally
            }
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("Open_Containing_Folder_New, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        ////////////////////private void Open_Containing_Folder(object sender, RoutedEventArgs e)
        ////////////////////{
        ////////////////////    int eL = -1;

        ////////////////////    try
        ////////////////////    {
        ////////////////////        string path = Path.GetDirectoryName((string)lb_listBox.SelectedItem);

        ////////////////////        System.Diagnostics.Process.Start(path);

        ////////////////////        #region catch and finally
        ////////////////////    }
        ////////////////////    catch (Exception ex)
        ////////////////////    {
        ////////////////////        _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("Open_Containing_Folder, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
        ////////////////////    }
        ////////////////////    finally
        ////////////////////    {
        ////////////////////    }
        ////////////////////    #endregion
        ////////////////////}



        private void Delete_Click_New(object sender, RoutedEventArgs e)
        {
            int eL = -1;
            if (lv_MasterList.SelectedItems.Count == 0) return;

            try
            {
                string string_DirectoryToCreate_Storage = method_Storage_DBFile_Directory() + @"\Admin Storage";
                string myString_DataStore = string_DirectoryToCreate_Storage + "\\" + ((DataRowView)lv_MasterList.SelectedItem)["FolderName"];

                string str_FileName = ((KeyValuePair<string, Bitmap>)listbox_objectmodel.SelectedItem).Key;

                MessageBoxResult result = System.Windows.MessageBox.Show("Delete '" + str_FileName + "'?", "Warning", System.Windows.MessageBoxButton.YesNoCancel);

                if (result == MessageBoxResult.Yes)
                {
                    string str_FileName_Full = myString_DataStore + "\\" + str_FileName;

                    if (System.IO.Directory.Exists(str_FileName_Full))
                    {
                        System.IO.Directory.Delete(str_FileName_Full, true);
                    }
                    else File.Delete(str_FileName_Full);

                    Dictionary<string, Bitmap> imageDictionary = new Dictionary<string, Bitmap>();

                    if (true)
                    {
                        Stream stream = new FileStream(myString_DataStore + @"\Parameters_Thumbnails.xml", FileMode.Open, FileAccess.Read);
                        DataContractSerializer serializer = new DataContractSerializer(typeof(Dictionary<string, Bitmap>));   //one of four
                        imageDictionary = serializer.ReadObject(stream) as Dictionary<string, Bitmap>; stream.Close();  //two of four
                    }

                    if (imageDictionary.ContainsKey(System.IO.Path.GetFileName(str_FileName))) imageDictionary.Remove(str_FileName);

                    if (true)
                    {
                        Stream stream = new FileStream(myString_DataStore + @"\Parameters_Thumbnails.xml", FileMode.Create, FileAccess.Write);  //of course this is bad because of c:
                        DataContractSerializer serializer = new DataContractSerializer(typeof(Dictionary<string, Bitmap>));
                        serializer.WriteObject(stream, imageDictionary); stream.Close();
                    }

                    methodRefresh_LoadUp_lvDragDirectory(LoadDirectoryOrNot.LoadIconDirectoryOnly);
                }
            }
            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("Delete_Click_New, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        ////////////////////private void Delete_Click(object sender, RoutedEventArgs e)
        ////////////////////{
        ////////////////////    int eL = -1;

        ////////////////////    try
        ////////////////////    {
        ////////////////////        MessageBoxResult result = System.Windows.MessageBox.Show("Delete '" + Path.GetFileNameWithoutExtension((string)lb_listBox.SelectedItem) + "'?", "Warning", System.Windows.MessageBoxButton.YesNoCancel);

        ////////////////////        if (result == MessageBoxResult.Yes)
        ////////////////////        {
        ////////////////////            if (System.IO.Directory.Exists((string)lb_listBox.SelectedItem))
        ////////////////////            {
        ////////////////////                System.IO.Directory.Delete((string)lb_listBox.SelectedItem, true);
        ////////////////////            }
        ////////////////////            else
        ////////////////////            {
        ////////////////////                File.Delete((string)lb_listBox.SelectedItem);
        ////////////////////            }

        ////////////////////            methodRefresh_LoadUp_lvDragDirectory(LoadDirectoryOrNot.LoadIconDirectoryOnly);
        ////////////////////        }
        ////////////////////        else if (result == MessageBoxResult.No)
        ////////////////////        {
        ////////////////////            //code for No
        ////////////////////        }
        ////////////////////        else if (result == MessageBoxResult.Cancel)
        ////////////////////        {
        ////////////////////            //code for Cancel
        ////////////////////        }
        ////////////////////    }
        ////////////////////    #region catch and finally
        ////////////////////    catch (Exception ex)
        ////////////////////    {
        ////////////////////        _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("Delete_Click, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
        ////////////////////    }
        ////////////////////    finally
        ////////////////////    {
        ////////////////////    }
        ////////////////////    #endregion
        ////////////////////}
        private void buttonSetStoreFolder_Click(object sender, RoutedEventArgs e) //focus2 keep this one here to copy the try statemen3
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
            //  System.Windows.MessageBox.Show("Please select store directory" + Environment.NewLine + Environment.NewLine + "Recommend: Dropbox OR GoogleDrive OR OneDrive");

            MessageBoxResult result2 = System.Windows.MessageBox.Show("Careful: This will change the database file...." + Environment.NewLine + Environment.NewLine + "We recommend: Dropbox OR GoogleDrive OR OneDrive" + Environment.NewLine + Environment.NewLine + "Click 'Cancel' if not sure.", "Careful! click 'Cancel' if not sure.", System.Windows.MessageBoxButton.OKCancel);

            if (result2 != MessageBoxResult.OK) return;

            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                   // string[] files = Directory.GetFiles(fbd.SelectedPath);

                    Properties.Settings.Default.DropboxOrGoogleDriveOrOnedrive_Path = fbd.SelectedPath;
                    Properties.Settings.Default.Save();
                    Properties.Settings.Default.Reload();


                    if (true)  //candidate for methodasation 20210602
                    {
                        string FILE_NAME = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\pkRevit\\DropboxOrGoogleDriveOrOnedrive_Path.txt";

                        System.IO.File.Create(FILE_NAME).Dispose();

                        System.IO.StreamWriter objWriter = new System.IO.StreamWriter(FILE_NAME, true);

                        objWriter.WriteLine(Properties.Settings.Default.DropboxOrGoogleDriveOrOnedrive_Path);
                        objWriter.Close();

                    }

                    label_DropboxGoogleDriveOnedrive.Content = Properties.Settings.Default.DropboxOrGoogleDriveOrOnedrive_Path;
                }
            }


            dict_SortOrdered_BuiltIn.Clear();
            dict_SortOrdered_Shared.Clear();
            dict_SortOrdered_Project.Clear();

            EstablishDirectories(false, "");

            onDirectorySwitch_OrFirstRender();
        }
        private void setStoreDirectory_MakeSubDirectories()
        {
            string string_DirectoryToCreate = label_DropboxGoogleDriveOnedrive.Content + @"\pkRevit Storage (do not edit directly)\Database File";
            string string_DirectoryToCreate_Storage = string_DirectoryToCreate + @"\Admin Storage";

            if (!System.IO.Directory.Exists(string_DirectoryToCreate_Storage))
            {
                System.IO.Directory.CreateDirectory(string_DirectoryToCreate_Storage);

                Properties.Settings.Default.lvMasterListSelectedIndex_WillNotWorkWhenFilterActive = -1;
                Properties.Settings.Default.Save();
                Properties.Settings.Default.Reload();
            }

            string_DatabaseLocation = string_DirectoryToCreate + @"\Full_Index.db";

            if (!System.IO.File.Exists(string_DatabaseLocation))
            {
                string string_Execution = "";

                if (toavoidloadingrevitdlls != null)
                {
                    string_Execution = toavoidloadingrevitdlls.executionLocation;
                }
                else
                {
                    string_Execution = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                }

                string string_DevOrRelease = string_Execution.Split('|')[0];

                if (string_DevOrRelease == "Release" | string_DevOrRelease == "Dev")  //constructs a path for release directory (in program files)
                {
                    //myString_TempPath = message.Split('|')[1] + "\\Families";
                    System.IO.File.Copy(string_Execution.Split('|')[1] + @"\StartDB.db", string_DatabaseLocation);
                }
                else
                {
                    System.IO.File.Copy(string_Execution + @"\StartDB.db", string_DatabaseLocation);
                }

                Properties.Settings.Default.lvMasterListSelectedIndex_WillNotWorkWhenFilterActive = -1;
                Properties.Settings.Default.Save();
                Properties.Settings.Default.Reload();
            }


            string_Default_Parameters_BuiltIn = string_DirectoryToCreate + @"\Default_Param_Builtin.db";
            if (!System.IO.File.Exists(string_Default_Parameters_BuiltIn))
            {
                Stream stream = new FileStream(string_Default_Parameters_BuiltIn, FileMode.Create, FileAccess.Write);
                DataContractSerializer serializer = new DataContractSerializer(typeof(Dictionary<int, int>));  //four of four
                serializer.WriteObject(stream, dict_SortOrdered_BuiltIn); stream.Close();
            }

            //label_DropboxGoogleDriveOnedrive.Content = Properties.Settings.Default.DropboxOrGoogleDriveOrOnedrive_Path;
            //string string_DirectoryToCreate = label_DropboxGoogleDriveOnedrive.Content + @"\pkRevit Storage (do not edit directly)\Database File";
            //string string_DirectoryToCreate_Storage = string_DirectoryToCreate + @"\Admin Storage";

            string_Default_Parameters_Shared = string_DirectoryToCreate + @"\Default_Param_Shared.db";
            if (!System.IO.File.Exists(string_Default_Parameters_Shared))
            {
                Stream stream = new FileStream(string_Default_Parameters_Shared, FileMode.Create, FileAccess.Write);
                DataContractSerializer serializer = new DataContractSerializer(typeof(Dictionary<int, Guid>));  //four of four
                serializer.WriteObject(stream, dict_SortOrdered_Shared); stream.Close();
            }

            string_Default_Parameters_Project = string_DirectoryToCreate + @"\Default_Param_Project.db";
            if (!System.IO.File.Exists(string_Default_Parameters_Project))
            {
                Stream stream = new FileStream(string_Default_Parameters_Project, FileMode.Create, FileAccess.Write);
                DataContractSerializer serializer = new DataContractSerializer(typeof(Dictionary<int, string>));  //four of four
                serializer.WriteObject(stream, dict_SortOrdered_Project); stream.Close();
            }

        }

        public SQLiteConnection OleDbConnection_ButtonOK { get; set; }
        public string string_DatabaseLocation { get; set; }
        public string string_Default_Parameters_BuiltIn { get; set; }
        public string string_Default_Parameters_Shared { get; set; }
        public string string_Default_Parameters_Project { get; set; }

        public DataTable dataTable { get; set; } = new DataTable();
        public SQLiteConnection connRLPrivateFlexible { get; set; }

        public Dictionary<int, NameAndValue> dictParameters_BuiltInt { get; set; } = new Dictionary<int, NameAndValue>();  //three of four
        public Dictionary<Guid, NameAndValue> dictParameters_Shared { get; set; } = new Dictionary<Guid, NameAndValue>();  //three of four
        public Dictionary<string, NameAndValue> dictParameters_Project { get; set; } = new Dictionary<string, NameAndValue>();  //three of four

        public Dictionary<int, int> dict_SortOrdered_BuiltIn { get; set; } = new Dictionary<int, int>();  //three of four
        public Dictionary<int, Guid> dict_SortOrdered_Shared { get; set; } = new Dictionary<int, Guid>();  //three of four
        public Dictionary<int, string> dict_SortOrdered_Project { get; set; } = new Dictionary<int, string>();  //three of four

        //public Dictionary<Guid, string> dict_GuidToAlias { get; set; } = new Dictionary<Guid, string>();  

        Func<Task> testFunc = async () =>
        {
            await Task.Delay(100);
        };

        private void Window_Loaded(object sender, RoutedEventArgs e)  //Window_Loaded  RoutedEventArgs
        {
            int eL = -1;

            try
            {
                using (var hklm = Microsoft.Win32.RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
                using (var key = hklm.OpenSubKey("SOFTWARE\\Josh New Zealand\\pkRevit"))
                {
                    //string stringProductVersion = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\\Pedersen Read Limited\\PRearch 2019").GetValue("ProductVersion").ToString();
                    //string stringProductVersion = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\\Pedersen Read Limited\\pkRevit joshnewzealand").GetValue("ProductVersion").ToString();
                    labelBuild.Content = "Build: " + key.GetValue("Version").ToString();
                }

                textBoxSearch.Focus();
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

        private void onDirectorySwitch_OrFirstRender()
        {
            int eL = -1;

            try
            {
                ///////////////////////////////////////////////////////////////////////////this.Refresh();


                //////combing_models = new CombiningViewModel
                //////{
                //////    model = new Model(this),
                //////    model2 = new Model2(this)
                //////};
                ////////this.DataContext = this;
                //////DataContext = combing_models;
                eL = 836;

                if (dataTable.Rows.Count != 0)
                {
                    string string_DirectoryToCreate = label_DropboxGoogleDriveOnedrive.Content + @"\pkRevit Storage (do not edit directly)\Database File";
                    string string_DirectoryToCreate_Storage = string_DirectoryToCreate + @"\Admin Storage\";

                    /////////////////////////////model.Path = string_DirectoryToCreate_Storage + dataTable.DefaultView[0]["FolderName"];
                    ///model.Path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                    FileToIconConverter fic = this.Resources["fic"] as FileToIconConverter;
                    fic.DefaultSize = 200;
                    //////////////////////////////////////lb_listBox.AddHandler(System.Windows.Controls.ListViewItem.PreviewMouseDownEvent, new MouseButtonEventHandler(list_MouseDown));
                }
                //DropboxOrGoogleDriveOrOnedrive_Path
                eL = 850;

                ///onDirectorySwitch_OrFirstRender
                ///
                foreach (KeyValuePair<Guid, string> kpv in dict_GuidToAlias)
                {
                    if (kpv.Key == Properties.Settings.Default.ComboBoxProjectFilter)
                    {
                        ComboBoxProjectFilter.SelectedItem = kpv;
                    }
                }

                eL = 1901;

                bool_IsolationIsOn = Properties.Settings.Default.bool_IsolationIsOn;

                //////////////if (Properties.Settings.Default.SearchStringRemember != "Type anything here ... to search")
                //////////////{
                //////////////    textBoxSearch.Text = Properties.Settings.Default.SearchStringRemember;
                //////////////}


                str_carryOverScheduleId = Properties.Settings.Default.str_carryOverScheduleId;
                str_carryOverGuid = Properties.Settings.Default.str_carryOverGuid;
                str_carryOverScheduleId_string = Properties.Settings.Default.str_carryOverScheduleId_string;
                //str_carryOverCategory_string = Properties.Settings.Default.str_carryOverCategory;

                eL = 1916;
                str_CurrentModeOfExplorer = Properties.Settings.Default.str_CurrentModeOfExplorer;
                eL = 1918;
                lbl_ScheduleName.Content = str_carryOverScheduleId_string;
                eL = 1920;
                //lbl_ScheduleName_Project.Content = dict_GuidToAlias[str_carryOverGuid];

                //_952_PRLoogleClassLibrary.DatabaseMethods.writeDebug(str_carryOverGuid.ToString(), true);

                if (!dict_GuidToAlias.ContainsKey(str_carryOverGuid))
                {
                    str_carryOverGuid = dict_GuidToAlias.Last().Key;
                }

                    eL = 1923;
                method_LoadUpMasterList();
                //  InvalidateVisual();
                eL = 343;
                ////////////////////////////////////////////////this.Refresh();
                // MessageBox.Show(Properties.Settings.Default.lvMasterListSelectedIndex_WilListBoxItemExplorer_PreviewMouseUplNotWorkWhenFilterActive.ToString());

                if (true)
                {
                    tb_tree.Text = "";

                    tb_tree.Inlines.Add(return_Hyperlink("ModeProject", dict_GuidToAlias[str_carryOverGuid]));
                    tb_tree.Inlines.Add(" , ");

                    ////////////tb_tree.Inlines.Add(return_Hyperlink("ModeCategory", Properties.Settings.Default.str_carryOverCategory));
                    ////////////tb_tree.Inlines.Add(" , ");

                    tb_tree.Inlines.Add(return_Hyperlink("ModeSchedule", str_carryOverScheduleId_string));
                }

                RequestNavigate_AndStart();
                changing_RadioButtons_Codally(Properties.Settings.Default.str_carryOverCategory, true);//<-- the row filter is triggered on this event

                if (Properties.Settings.Default.lvMasterListSelectedIndex_WillNotWorkWhenFilterActive != -1)
                {
                    eL = 614;
                    if (Properties.Settings.Default.lvMasterListSelectedIndex_WillNotWorkWhenFilterActive < dataTable.Rows.Count)
                    {
                        eL = 617;
                        lv_MasterList.SelectedIndex = Properties.Settings.Default.lvMasterListSelectedIndex_WillNotWorkWhenFilterActive;
                        lv_MasterList.ScrollIntoView(lv_MasterList.SelectedItem);
                        methodRefresh_LoadUp_lvDragDirectory(LoadDirectoryOrNot.LoadEverything);
                    }
                    else
                    {
                        eL = 622;
                        lv_MasterList.SelectedIndex = 0;
                        methodRefresh_LoadUp_lvDragDirectory(LoadDirectoryOrNot.LoadEverything);
                    }
                }
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("onDirectorySwitch_OrFirstRender, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            int eL = -1;

            try
            {
                onDirectorySwitch_OrFirstRender();

            }
            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("Window_ContentRendered, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion

        }

        bool bool_NoFilterTheFirstTime = false;

        public void method_LoadUpMasterList()
        {
            int eL = -1;

            try
            {
                eL = 100;

                dataTable.Clear();
                eL = 110;
                dataTable = GetDataTable("SELECT * FROM [pkRevitMasterTable] order by [SortOrder] desc ");
                eL = 120;

                if (lv_MasterList == null) MessageBox.Show("the itemsource is null");
                if (dataTable == null)
                {
                    MessageBox.Show("The dataTable is null" + Environment.NewLine + "Please check 'Set Storage Parent Folder' the button is top left.");
                }
                else
                {

                    lv_MasterList.ItemsSource = dataTable.DefaultView;
                    eL = 130;
                    if (bool_NoFilterTheFirstTime)
                    {
                        eL = 140;
                        rowFilter(bool_IsolationIsOn, false);
                        eL = 1858;
                        //MessageBox.Show("shoudl not be hitting this");
                    }
                    else
                    {
                        eL = 140;
                        bool_NoFilterTheFirstTime = true;
                        bool_IsolationIsOn = true;
                        eL = 1866;
                        rowFilter(bool_IsolationIsOn, false);  //we reinstated at this point
                        //MessageBox.Show("shoudl not be hitting this");
                    }
                }

            }
            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("method_LoadUpMasterList, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        public DataTable GetDataTable(string sql)
        {
            int eL = -1;

            try
            {
                DataTable dt = new DataTable();

                OleDbConnection_ButtonOK = reconnectPrivateFlexible(false, string_DatabaseLocation, connRLPrivateFlexible);
                using (SQLiteCommand command = ConnectionCommandType(OleDbConnection_ButtonOK))
                {
                    // SQLitePCL.Batteries.Init();

                    //SQLitePCL.raw.SetProvider(new SQLitePCL.SQLite3Provider_dynamic_cdecl());
                    eL = 994;
                    command.CommandText = sql;// ORDER BY [TargetSort] DESC";

                    eL = 994;
                    OleDbConnection_ButtonOK.Open();

                    SQLiteDataReader dataReader = command.ExecuteReader();

                    dt.Load(dataReader);

                    dataReader.Close();
                    OleDbConnection_ButtonOK.Close();
                    OleDbConnection_ButtonOK.Dispose();
                    GC.Collect();
                }

                return dt;
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("GetDataTable, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion

            return null;
        }

        public static SQLiteConnection reconnectPrivateFlexible(bool azure, string path000000, SQLiteConnection connRLPrivateFlexible) //new methods here need to be updated in 1 2 3 4 5 places and one by the directives
        {
            int eL = -1;

            try
            {
                eL = 990;
                string myConnectionString = "";
                eL = 1000;
                myConnectionString = "Data Source=" + path000000 + "; Version=3";
                eL = 1010;
                connRLPrivateFlexible = new SQLiteConnection(myConnectionString);
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("reconnectPrivateFlexible, error line:" + eL + Environment.NewLine + path000000 + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
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

        public string method_Storage_DBFile_Directory()
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
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("lv_MasterList_MouseUp" + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
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

        private void lv_MasterList_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                int eL = -1;

                try
                {
                    IsolateButton_Or_DoubleClick();
                }

                #region catch and finally
                catch (Exception ex)
                {
                    _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("btn_Isolate_Item_Click, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
                }
                finally
                {
                }
                #endregion

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

                method_ReadIntoResult(true);
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

        private string method_XML_Parameters_BuiltIn(string s)
        {
            string string_DirectoryToCreate_Storage = method_Storage_DBFile_Directory() + @"\Admin Storage";
            string DirPath_Date_Dir = string_DirectoryToCreate_Storage + "\\" + (s == "" ? ((DataRowView)lv_MasterList.SelectedItem)["FolderName"] : s) + @"\\";

            string myXmlFilePath = DirPath_Date_Dir + "Parameters_BuiltIn.xml";

            return myXmlFilePath;
        }

        private string method_XML_Parameters_Shared(string s)
        {
            string string_DirectoryToCreate_Storage = method_Storage_DBFile_Directory() + @"\Admin Storage";
            string DirPath_Date_Dir = string_DirectoryToCreate_Storage + "\\" + (s == "" ? ((DataRowView)lv_MasterList.SelectedItem)["FolderName"] : s) + @"\\";

            string myXmlFilePath = DirPath_Date_Dir + "Parameters_Shared.xml";

            return myXmlFilePath;
        }

        private string method_XML_Parameters_Project(string s)
        {
            string string_DirectoryToCreate_Storage = method_Storage_DBFile_Directory() + @"\Admin Storage";
            string DirPath_Date_Dir = string_DirectoryToCreate_Storage + "\\" + (s == "" ? ((DataRowView)lv_MasterList.SelectedItem)["FolderName"] : s) + @"\\";

            string myXmlFilePath = DirPath_Date_Dir + "Parameters_Project.xml";

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

        public enum LoadDirectoryOrNot
        {
            LoadEverything, LoadIconDirectoryOnly
        }

        private void buttonExportRange_Click(object sender, RoutedEventArgs e)
        {
            int eL = -1;

            try
            {
                if (lv_MasterList.SelectedItem == null)
                {
                    MessageBox.Show("Please select an item(s).");
                    return;
                }

                string path = (System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\~~Schedule Lookup~~");
                if (!System.IO.Directory.Exists(path)) System.IO.Directory.CreateDirectory(path);

                path = path + "\\" + DateTime.Now.ToString("yyyy-MM-dd HHmm.ss.ff");
                if (!System.IO.Directory.Exists(path)) System.IO.Directory.CreateDirectory(path);

                int my_Int = 1;

                foreach (DataRowView drv in lv_MasterList.SelectedItems.Cast<DataRowView>().OrderByDescending(x => x["SortOrder"]))
                {
                    if(true)
                    {
                        

                        string string_DirectoryToCreate_Storage = method_Storage_DBFile_Directory() + @"\Admin Storage";
                        string myString_DataStore = string_DirectoryToCreate_Storage + "\\" + drv["FolderName"];

                        if (!System.IO.Directory.Exists(myString_DataStore)) continue;

                        string str_Cleaned = drv["SearchTerms"].ToString();

                        if (true)
                        {
                            string regexSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
                            Regex r = new Regex(string.Format("[{0}]", Regex.Escape(regexSearch)));
                            str_Cleaned = r.Replace(str_Cleaned, "");
                        }

                        str_Cleaned = str_Cleaned.Replace(',', ' ');
                        str_Cleaned = str_Cleaned.Replace("  ", " ");
                        str_Cleaned = str_Cleaned.Substring(0, Math.Min(str_Cleaned.Length, 60));
                        str_Cleaned = str_Cleaned.Trim();
                        str_Cleaned = my_Int + "_" +  str_Cleaned;
                        ///more 

                        ///more on this, more on this, more on this, more on this, more on this, more on this, more on this, 
                        ///we are duplicating somewhere

                        string str_NewFolder = path + "\\" + str_Cleaned;
                        System.IO.Directory.CreateDirectory(str_NewFolder);
                        my_Int++;
                        if (true)
                        {
                            Stream stream = new FileStream(myString_DataStore + @"\Parameters_Thumbnails.xml", FileMode.Open, FileAccess.Read);
                            DataContractSerializer serializer = new DataContractSerializer(typeof(Dictionary<string, Bitmap>));
                            Dictionary<string, Bitmap> ImageDictionary = serializer.ReadObject(stream) as Dictionary<string, Bitmap>; stream.Close();

                            foreach (KeyValuePair<string, Bitmap> kpv in ImageDictionary)
                            {
                                if(System.IO.Directory.Exists(myString_DataStore + "\\" + kpv.Key))
                                {
                                    DirectoryCopy(myString_DataStore + "\\" + kpv.Key, str_NewFolder + "\\" + kpv.Key, true);
                                } else
                                {
                                    System.IO.File.Copy(myString_DataStore + "\\" + kpv.Key, str_NewFolder + "\\" + kpv.Key);
                                }

                            }
                        }
                    }
                }
                System.Diagnostics.Process.Start(path);
               //MessageBox.Show("You now have a 'local' copy of what you selected." + Environment.NewLine + Environment.NewLine + "These files are unassociated with the database and are free to use as you wish.");
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("buttonExportRange_Click, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);
            DirectoryInfo[] dirs = dir.GetDirectories();

            // If the source directory does not exist, throw an exception.
            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException("Source directory does not exist or could not be found: " + sourceDirName);
            }

            // If the destination directory does not exist, create it.
            if (!Directory.Exists(destDirName)) Directory.CreateDirectory(destDirName);

            // Get the file contents of the directory to copy.
            FileInfo[] files = dir.GetFiles();

            foreach (FileInfo file in files)
            {
                // Create the path to the new copy of the file.
                string temppath = Path.Combine(destDirName, file.Name);

                // Copy the file.
                file.CopyTo(temppath, false);
            }

            // If copySubDirs is true, copy the subdirectories.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    // Create the subdirectory.
                    string temppath = Path.Combine(destDirName, subdir.Name);

                    // Copy the subdirectories.
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }

        public bool methodRefresh_LoadUp_lvDragDirectory(LoadDirectoryOrNot whatToLoad)
        {
            if (lv_MasterList.SelectedItem == null) return false;

            string string_DirectoryToCreate_Storage = method_Storage_DBFile_Directory() + @"\Admin Storage";
            string myString_DataStore = string_DirectoryToCreate_Storage + "\\" + ((DataRowView)lv_MasterList.SelectedItem)["FolderName"];

            if (!System.IO.Directory.Exists(myString_DataStore))
            {
                MessageBox.Show("Directory has been deleted.");
                return false;
            }

            if (whatToLoad == LoadDirectoryOrNot.LoadEverything)
            {
                Properties.Settings.Default.lvMasterListSelectedIndex_WillNotWorkWhenFilterActive = lv_MasterList.SelectedIndex;
                Properties.Settings.Default.Save();
                Properties.Settings.Default.Reload();

                myLabel_Directory.Content = ((DataRowView)lv_MasterList.SelectedItem)["FolderName"];
                label_ProjectGUID.Content = ((DataRowView)lv_MasterList.SelectedItem)["ProjectGUID"];
            } //just directory refresh

            if (whatToLoad == LoadDirectoryOrNot.LoadEverything) method_ReadIntoResult(true);

            string string_DirectoryToCreate = label_DropboxGoogleDriveOnedrive.Content + @"\pkRevit Storage (do not edit directly)\Database File";
            string string_Default_GuidToAlias = string_DirectoryToCreate + @"\Project GUID to Alias.xml";

            if (true)
            {
                Stream stream = new FileStream(string_Default_GuidToAlias, FileMode.Open, FileAccess.Read);
                DataContractSerializer serializer = new DataContractSerializer(typeof(Dictionary<Guid, string>));   //one of four
                dict_GuidToAlias = serializer.ReadObject(stream) as Dictionary<Guid, string>; stream.Close();  //two of four

                List<KeyValuePair<Guid, string>> listKPV = dict_GuidToAlias.OrderBy(x => x.Value).ToList();
                listKPV.Insert(0, listKPV.Last());
                listKPV.RemoveAt(listKPV.Count - 1);
                dict_GuidToAlias = listKPV.ToDictionary(x => x.Key, x => x.Value);  //in two places 20210225, and i don't think this place is required
            }

            Guid aguid = new Guid("{" + ((DataRowView)lv_MasterList.SelectedItem)["ProjectGuid"] + "}");

            lbl_ProjectOfSelection.Content = dict_GuidToAlias[aguid];
            lbl_Date.Content = DateTime.FromOADate((double)((DataRowView)lv_MasterList.SelectedItem)["Date"] - 2415018.5).ToString("U", CultureInfo.CreateSpecificCulture("en-US"));

            lv_ReorderThis.Items.Refresh();

            combing_models.model2.Path = myString_DataStore;
            btn_OneLevelUp.IsEnabled = false;

            return true;
        }

        private void lv_Explorer_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            int eL = -1;

            try
            {
                if (str_CurrentModeOfExplorer == "ModeProject")
                {
                    str_CurrentModeOfExplorer = "ModeSchedule";
                    RequestNavigate_AndStart();
                }
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("lv_Explorer_PreviewMouseDoubleClick, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        private void lv_Ordered_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)  //this is removing 
        {
            int eL = -1;

            //i think this is the issue

            try
            {
                MessageBoxResult result = System.Windows.MessageBox.Show("This will remove the parameter." + Environment.NewLine + Environment.NewLine + "Are you really sure?", "", System.Windows.MessageBoxButton.YesNoCancel);

                if (result != MessageBoxResult.Yes) return;


                Tuple<int, string, string, bool, bool> tuple = (Tuple<int, string, string, bool, bool>)lv_ReorderThis.SelectedItem;

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
                    if (!tuple.Item5)
                    {
                        dict_SortOrdered_Shared.Remove(tuple.Item1);

                        Stream stream = new FileStream(string_Default_Parameters_Shared, FileMode.Create, FileAccess.Write);
                        DataContractSerializer serializer = new DataContractSerializer(typeof(Dictionary<int, Guid>));  //four of four
                        serializer.WriteObject(stream, dict_SortOrdered_Shared); stream.Close();
                    }
                    else
                    {
                        dict_SortOrdered_Project.Remove(tuple.Item1);

                        Stream stream = new FileStream(string_Default_Parameters_Project, FileMode.Create, FileAccess.Write);
                        DataContractSerializer serializer = new DataContractSerializer(typeof(Dictionary<int, string>));  //four of four
                        serializer.WriteObject(stream, dict_SortOrdered_Project); stream.Close();
                    }
                }


                //lv_MasterList.Items.Refresh();
                //method_UpdateEntry((Int64)myLabel_Directory.Content, string_SearchStringAccumlation());
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
        private void but_reorderParameters_Down_Click(object sender, RoutedEventArgs e)
        {
            int eL = -1;

            try
            {
                if (lv_ReorderThis.SelectedItems.Count == 0) return;

                #region common region
                eL = 1596;
                List<Tuple<int, int, int, Guid, string>> listTuple = dict_SortOrdered_BuiltIn.Select(x => new Tuple<int, int, int, Guid, string>(x.Key, 0, x.Value, new Guid(), null)).ToList();
                listTuple.AddRange(dict_SortOrdered_Shared.Select(x => new Tuple<int, int, int, Guid, string>(x.Key, 1, 0, x.Value, null)).ToList());
                listTuple.AddRange(dict_SortOrdered_Project.Select(x => new Tuple<int, int, int, Guid, string>(x.Key, 2, 0, new Guid(), x.Value)).ToList());

                eL = 1604;

                List<int> list_Int64 = new List<int>();
                foreach (Tuple<int, string, string, bool, bool> drb in lv_ReorderThis.SelectedItems.Cast<Tuple<int, string, string, bool, bool>>()) list_Int64.Add(lv_ReorderThis.Items.IndexOf(drb));

                #endregion

                eL = 1613;

                List<Tuple<int, string, string, bool, bool>> IOrderedEnumerableRowView = lv_ReorderThis.SelectedItems.Cast<Tuple<int, string, string, bool, bool>>().OrderBy(x => x.Item1).ToList();
                Tuple<int, string, string, bool, bool> tupleWhatToFind = lv_ReorderThis.Items.Cast<Tuple<int, string, string, bool, bool>>().Where(x => x.Item1 == IOrderedEnumerableRowView.Last().Item1).FirstOrDefault();
                int myInt_ListIndexBefore = lv_ReorderThis.Items.IndexOf(tupleWhatToFind) + 1;
                if (myInt_ListIndexBefore > lv_ReorderThis.Items.Count - 1) return;

                IOrderedEnumerableRowView.Add((Tuple<int, string, string, bool, bool>)lv_ReorderThis.Items[myInt_ListIndexBefore]);

                eL = 1617;

                int int_IntergerFirst = IOrderedEnumerableRowView.First().Item1;
                int int_IntergerLast = IOrderedEnumerableRowView.Last().Item1;

                eL = 1621;

                Tuple<int, int, int, Guid, string> myTuple_OneUp = listTuple.Where(x => x.Item1 == int_IntergerLast).FirstOrDefault();
                listTuple[listTuple.IndexOf(myTuple_OneUp)] = Tuple.Create(-1, myTuple_OneUp.Item2, myTuple_OneUp.Item3, myTuple_OneUp.Item4, myTuple_OneUp.Item5);
                eL = 1641;

                for (int i = int_IntergerFirst; i < int_IntergerLast; i++)
                {
                    if (listTuple.Where(x => x.Item1 == i).Count() != 0)
                    {
                        Tuple<int, int, int, Guid, string> myTuple = listTuple.Where(x => x.Item1 == i).FirstOrDefault();
                        listTuple[listTuple.IndexOf(myTuple)] = Tuple.Create(myTuple.Item1 + 1, myTuple.Item2, myTuple.Item3, myTuple.Item4, myTuple.Item5);
                    }
                }

                eL = 1648;

                myTuple_OneUp = listTuple.Where(x => x.Item1 == -1).FirstOrDefault();

                eL = 1649;
                listTuple[listTuple.IndexOf(myTuple_OneUp)] = Tuple.Create(int_IntergerFirst, myTuple_OneUp.Item2, myTuple_OneUp.Item3, myTuple_OneUp.Item4, myTuple_OneUp.Item5);


                saving_SortList(listTuple);
                foreach (int theInt in list_Int64) lv_ReorderThis.SelectedItems.Add(lv_ReorderThis.Items[theInt + 1]);
                lv_ReorderThis.ScrollIntoView(lv_ReorderThis.SelectedItem);
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("but_reorderParameters_Down_Click, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }
        private void but_reorderParameters_Click(object sender, RoutedEventArgs e)
        {
            int eL = -1;

            try
            {
                if (lv_ReorderThis.SelectedItems.Count == 0) return;


                #region common region
                eL = 1596;
                List<Tuple<int, int, int, Guid, string>> listTuple = dict_SortOrdered_BuiltIn.Select(x => new Tuple<int, int, int, Guid, string>(x.Key, 0, x.Value, new Guid(), null)).ToList();
                listTuple.AddRange(dict_SortOrdered_Shared.Select(x => new Tuple<int, int, int, Guid, string>(x.Key, 1, 0, x.Value, null)).ToList());
                listTuple.AddRange(dict_SortOrdered_Project.Select(x => new Tuple<int, int, int, Guid, string>(x.Key, 2, 0, new Guid(), x.Value)).ToList());

                eL = 1604;

                List<int> list_Int64 = new List<int>();
                foreach (Tuple<int, string, string, bool, bool> drb in lv_ReorderThis.SelectedItems.Cast<Tuple<int, string, string, bool, bool>>()) list_Int64.Add(lv_ReorderThis.Items.IndexOf(drb));
                #endregion


                //what does this do...collection?
                eL = 1613;
                List<Tuple<int, string, string, bool, bool>> IOrderedEnumerableRowView = lv_ReorderThis.SelectedItems.Cast<Tuple<int, string, string, bool, bool>>().OrderBy(x => x.Item1).ToList();
                Tuple<int, string, string, bool, bool> tupleWhatToFind = lv_ReorderThis.Items.Cast<Tuple<int, string, string, bool, bool>>().Where(x => x.Item1 == IOrderedEnumerableRowView.First().Item1).FirstOrDefault();
                int myInt_ListIndexBefore = lv_ReorderThis.Items.IndexOf(tupleWhatToFind) - 1;
                if (myInt_ListIndexBefore < 0) return;


                //what does this do...collection?
                IOrderedEnumerableRowView.Insert(0, (Tuple<int, string, string, bool, bool>)lv_ReorderThis.Items[myInt_ListIndexBefore]);


                //what does this do...get the 'index'? of the first and last item, from the selection, but now from list view...actually from table derived from the list view selection
                eL = 1617;
                int int_IntergerFirst = IOrderedEnumerableRowView.First().Item1;
                int int_IntergerLast = IOrderedEnumerableRowView.Last().Item1;


                //what does this do...this one sets it to zero it finds the first one
                eL = 1621;
                Tuple<int, int, int, Guid, string> myTuple_OneUp = listTuple.Where(x => x.Item1 == int_IntergerFirst).FirstOrDefault();
                listTuple[listTuple.IndexOf(myTuple_OneUp)] = Tuple.Create(-1, myTuple_OneUp.Item2, myTuple_OneUp.Item3, myTuple_OneUp.Item4, myTuple_OneUp.Item5);  
                eL = 1641;


                //loops around by the values found at 1617
                for (int i = int_IntergerFirst; i < int_IntergerLast + 1; i++)
                {
                    if (listTuple.Where(x => x.Item1 == i).Count() != 0)
                    {
                        Tuple<int, int, int, Guid, string> myTuple = listTuple.Where(x => x.Item1 == i).FirstOrDefault();
                        listTuple[listTuple.IndexOf(myTuple)] = Tuple.Create(myTuple.Item1 - 1, myTuple.Item2, myTuple.Item3, myTuple.Item4, myTuple.Item5);
                    }
                }

                //gets the null, the null is because it sticks one in a holding pattern, but in this case it is retrieving one from a holding pattern
                eL = 1648;
                myTuple_OneUp = listTuple.Where(x => x.Item1 == -1).FirstOrDefault();

                ///this one below sure didn't beat any fruit, the dictionary int must be the short order stuff
                ///we found it, 16 did not become 17, 16 dids not become 17, 16 did not become 17, 16 did not become 17 therrefore
                ///add add add add add add

                eL = 1649;
                listTuple[listTuple.IndexOf(myTuple_OneUp)] = Tuple.Create(int_IntergerLast, myTuple_OneUp.Item2, myTuple_OneUp.Item3, myTuple_OneUp.Item4, myTuple_OneUp.Item5);





                //what does this do...collection?
                eL = 2643;
                saving_SortList(listTuple);
                foreach (int theInt in list_Int64) lv_ReorderThis.SelectedItems.Add(lv_ReorderThis.Items[theInt - 1]);
                lv_ReorderThis.ScrollIntoView(lv_ReorderThis.SelectedItem);
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("but_reorderParameters_Click, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }


        private void saving_SortList(List<Tuple<int, int, int, Guid, string>> listTuple)
        {
            int eL = -1;

            try
            {
                string str_BackupLocationBuiltIn = System.IO.Path.GetTempPath() + "` aa pkEdit\\" + System.IO.Path.GetFileName(string_Default_Parameters_BuiltIn);
                string str_BackupLocationShared = System.IO.Path.GetTempPath() + "` aa pkEdit\\" + System.IO.Path.GetFileName(string_Default_Parameters_Shared);
                string str_BackupLocationProject = System.IO.Path.GetTempPath() + "` aa pkEdit\\" + System.IO.Path.GetFileName(string_Default_Parameters_Project);


                if(!System.IO.Directory.Exists(System.IO.Path.GetTempPath() + "` aa pkEdit\\"))
                {
                    System.IO.Directory.CreateDirectory(System.IO.Path.GetTempPath() + "` aa pkEdit\\");
                }
                
                File.Copy(string_Default_Parameters_BuiltIn, str_BackupLocationBuiltIn, true);
                File.Copy(string_Default_Parameters_Shared, str_BackupLocationShared, true);
                File.Copy(string_Default_Parameters_Project, str_BackupLocationProject, true);

                if (true)
                    {
                        Stream stream = new FileStream(string_Default_Parameters_BuiltIn, FileMode.Create, FileAccess.Write);
                        DataContractSerializer serializer = new DataContractSerializer(typeof(Dictionary<int, int>));
                        serializer.WriteObject(stream, listTuple.Where(x => x.Item2 == 0).ToDictionary(x => x.Item1, x => x.Item3)); stream.Close();
                    }
                    if (true)
                    {
                        Stream stream = new FileStream(string_Default_Parameters_Shared, FileMode.Create, FileAccess.Write);
                        DataContractSerializer serializer = new DataContractSerializer(typeof(Dictionary<int, Guid>));
                        serializer.WriteObject(stream, listTuple.Where(x => x.Item2 == 1).ToDictionary(x => x.Item1, x => x.Item4)); stream.Close();
                    }

                    if (true)
                    {
                        Stream stream = new FileStream(string_Default_Parameters_Project, FileMode.Create, FileAccess.Write);
                        DataContractSerializer serializer = new DataContractSerializer(typeof(Dictionary<int, string>));
                        serializer.WriteObject(stream, listTuple.Where(x => x.Item2 == 2).ToDictionary(x => x.Item1, x => x.Item5)); stream.Close();  //<-- this is the last one
                    }
          
                method_ReadIntoResult(false);
                lv_ReorderThis.Items.Refresh();
            }

            #region catch and finally
            catch (Exception ex)
            {
                string path = (System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\` aa pkEdit" + "\\" + DateTime.Now.ToString("yyyyMMdd HHmm ss ff"));
                if (!System.IO.Directory.Exists(path)) System.IO.Directory.CreateDirectory(path);


                string str_BackupLocationBuiltIn = System.IO.Path.GetTempPath() + "` aa pkEdit\\" + System.IO.Path.GetFileName(string_Default_Parameters_BuiltIn);
                string str_BackupLocationShared = System.IO.Path.GetTempPath() + "` aa pkEdit\\" + System.IO.Path.GetFileName(string_Default_Parameters_Shared);
                string str_BackupLocationProject = System.IO.Path.GetTempPath() + "` aa pkEdit\\" + System.IO.Path.GetFileName(string_Default_Parameters_Project);


                string str_BackupLocationBuiltIn_Final = path + "\\" + System.IO.Path.GetFileName(string_Default_Parameters_BuiltIn);
                string str_BackupLocationShared_Final = path + "\\" + System.IO.Path.GetFileName(string_Default_Parameters_Shared);
                string str_BackupLocationProject_Final = path + "\\" + System.IO.Path.GetFileName(string_Default_Parameters_Project);


                File.Copy(str_BackupLocationBuiltIn, str_BackupLocationBuiltIn_Final, true);
                File.Copy(str_BackupLocationShared, str_BackupLocationShared_Final, true);
                File.Copy(str_BackupLocationProject, str_BackupLocationProject_Final, true);

                string FILE_NAME = (path + "\\README.txt");

                System.IO.File.Create(FILE_NAME).Dispose();

                System.IO.StreamWriter objWriter = new System.IO.StreamWriter(FILE_NAME, true);
                objWriter.WriteLine("Hi, the files in this directory are a critical backup. The software may have crashed at a place where restoring these files to the 'Set Storage Parent Folder' is the only way to fix it." + Environment.NewLine + Environment.NewLine + "Try a directory one level up if this doesn't work.");
                objWriter.Close();


                System.Diagnostics.Process.Start(path);



                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("saving_SortList, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion

        }

        private void method_ReadIntoResult(bool bool_Sort)
        {
            if (true)
            {
                string fileString = method_XML_Parameters_BuiltIn("");

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
                string fileString = method_XML_Parameters_Shared("");

                if (System.IO.File.Exists(fileString))
                {
                    Stream stream = new FileStream(fileString, FileMode.Open, FileAccess.Read);
                    DataContractSerializer serializer = new DataContractSerializer(typeof(Dictionary<Guid, NameAndValue>));   //one of four
                    dictParameters_Shared = serializer.ReadObject(stream) as Dictionary<Guid, NameAndValue>; stream.Close();  //two of four
                    win_moreParam.lv_Result_Shared.ItemsSource = dictParameters_Shared;
                }
                else
                {
                    win_moreParam.lv_Result_Shared.ItemsSource = null;
                }
            }

            if (true)
            {
                string fileString = method_XML_Parameters_Project("");

                if (System.IO.File.Exists(fileString))
                {
                    Stream stream = new FileStream(fileString, FileMode.Open, FileAccess.Read);
                    DataContractSerializer serializer = new DataContractSerializer(typeof(Dictionary<string, NameAndValue>));   //one of four
                    dictParameters_Project = serializer.ReadObject(stream) as Dictionary<string, NameAndValue>; stream.Close();  //two of four
                    win_moreParam.lv_Result_Project.ItemsSource = dictParameters_Project;
                }
                else
                {
                    win_moreParam.lv_Result_Project.ItemsSource = null;
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
                    if (dictParameters_BuiltInt.ContainsKey(kpv.Value))
                    {
                        lv_ReorderThis.Items.Add(new Tuple<int, string, string, bool, bool>(kpv.Key, dictParameters_BuiltInt[kpv.Value].sName, dictParameters_BuiltInt[kpv.Value].sValue, true, false));
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
                        lv_ReorderThis.Items.Add(new Tuple<int, string, string, bool, bool>(kpv.Key, dictParameters_Shared[kpv.Value].sName, dictParameters_Shared[kpv.Value].sValue, false, false));
                    }
                }
            }

            if (true)
            {
                Stream stream = new FileStream(string_Default_Parameters_Project, FileMode.Open, FileAccess.Read);
                DataContractSerializer serializer = new DataContractSerializer(typeof(Dictionary<int, string>));   //one of four
                dict_SortOrdered_Project = serializer.ReadObject(stream) as Dictionary<int, string>; stream.Close();  //two of four

                foreach (KeyValuePair<int, string> kpv in dict_SortOrdered_Project)
                {
                    if (dictParameters_Project.ContainsKey(kpv.Value))
                    {
                        lv_ReorderThis.Items.Add(new Tuple<int, string, string, bool, bool>(kpv.Key, dictParameters_Project[kpv.Value].sName, dictParameters_Project[kpv.Value].sValue, false, true));
                    }
                }
            }

            if (bool_Sort) GridViewSort.ApplySort(lv_ReorderThis.Items, "Item1");

        }


        private void buttonReselect_Click(object sender, RoutedEventArgs e)
        {
            int eL = -1;

            try
            {
                UIDocument uidoc = toavoidloadingrevitdlls.commandData.Application.ActiveUIDocument;
                if (uidoc.Selection.GetElementIds().Count == 0)
                {
                    MessageBox.Show("Please select an element in Revit.");

                    return;
                }

                ///when it is not there it releasees the filter
                ///when it is not there it releases the filter
                ///when it is not there it releases the filter

                Tuple<Element, Element> couldBeNull = method_butshouldworkonWallTypes(true, uidoc);
                if (couldBeNull == null) return;

                Element elementType = couldBeNull.Item2;

                eL = 708;


                EnumerableRowCollection results = does_it_already_exist_in_dataTable(uidoc, elementType);

                eL = 1832;
                List<DataRow> listDataRow = results.Cast<DataRow>().ToList();

                ///above this point, were looking for something that frees it

                string myString_Date = "";

                if (listDataRow.Count() == 0)
                {
                    MessageBox.Show("Item has not been saved to the database yet.");
                    return;
                }
                else
                {
                    myString_Date = retrive_myString_Date(results);
                }

                ///above this oint

                ///here we ask if it is in the items, and if it is not then we relese the filter propertly
                lv_MasterList.SelectedIndex = Properties.Settings.Default.lvMasterListSelectedIndex_WillNotWorkWhenFilterActive;

                ///this is the problem


                ///above this point

                if (lv_MasterList.SelectedItem != null)
                {
                    ///scroll into view bets the filter...this has been encountered before
                    ///the solution can only be to apply the filter again
                    lv_MasterList.ScrollIntoView(lv_MasterList.SelectedItem);
                }
                //MessageBox.Show(dataTable.DefaultView.RowFilter);

                methodRefresh_LoadUp_lvDragDirectory(LoadDirectoryOrNot.LoadEverything);
                //lv_MasterList.ScrollIntoView(lv_MasterList.SelectedItem);
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("buttonReselect_Click, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }
        //////////////List<DataRow> listDataRow = results.Cast<DataRow>().ToList();
        private EnumerableRowCollection does_it_already_exist_in_dataTable(UIDocument uidoc, Element elementType)
        {
            bool rc = JtNamedGuidStorage.Get(uidoc.Document, "TrackChanges_project_identifier", out named_guid, false);

            EnumerableRowCollection results = from myRow in dataTable.AsEnumerable()
                                              where myRow.Field<Int64>("ElementTypeID") == elementType.Id.IntegerValue
                                              select myRow;

            results = from myRow in results.Cast<DataRow>()
                      where myRow.Field<string>("ProjectGUID") == named_guid.ToString()
                      select myRow;

            return results;
        }

        //group differences, group differences, group differences, group differences
        private string retrive_myString_Date(EnumerableRowCollection results)
        {
            DataRow[] datarowArray;
            DataTable cut_dt;
            if (true)
            {
                cut_dt = ((DataView)lv_MasterList.ItemsSource).ToTable();
                string str_SearchExpresssion = "ID = " + results.Cast<DataRow>().First()["ID"];
                datarowArray = cut_dt.Select(str_SearchExpresssion);
            }


            if (datarowArray.Count() == 0)
            {

                ///////RemoveFilters_UsedOnUnisolate_And_RemoveFilterButton();
                //////UnIsolate_Commons();
                //////dataTable.DefaultView.RowFilter = "";
                textBlock_NoResults.Visibility = System.Windows.Visibility.Hidden;

                changing_RadioButtons_Codally(results.Cast<DataRow>().First()["Category"].ToString(), true);


                textBoxSearch.Text = "";
                cut_dt = dataTable;
                //cut_dt = ((DataView)lv_MasterList.ItemsSource).ToTable();
                string str_SearchExpresssion = "ID = " + results.Cast<DataRow>().First()["ID"];
                datarowArray = cut_dt.Select(str_SearchExpresssion);
            }


            int intint = cut_dt.Rows.IndexOf(datarowArray.First());
            Properties.Settings.Default.lvMasterListSelectedIndex_WillNotWorkWhenFilterActive = intint;
            Properties.Settings.Default.Save();
            Properties.Settings.Default.Reload();

            return results.Cast<DataRow>().First()["FolderName"].ToString();
        }
        private void changing_RadioButtons_Codally(string string_ChangingForThis, bool bool_TurnOffEvents)
        {

            if (bool_TurnOffEvents)
            {
                RadioButton_CD.Checked -= RadioButton_Checked_4;
                RadioButton_DD.Checked -= RadioButton_Checked_5;
                RadioButton_EE.Checked -= RadioButton_Checked_3;
                RadioButton_EF.Checked -= RadioButton_Checked_2;
                RadioButton_LD.Checked -= RadioButton_Checked_1;
                RadioButton_LF.Checked -= RadioButton_Checked;
                RadioButton_CT.Checked -= RadioButton_Checked_7;
                RadioButton_SD.Checked -= RadioButton_Checked_6;
                RadioButton_ALL.Checked -= RadioButton_ALL_Checked;
            }



            switch (string_ChangingForThis)  //happens in two places the other place is in closing
            {
                case "Communication Devices": RadioButton_CD.IsChecked = true; break;
                case "Data Devices": RadioButton_DD.IsChecked = true; break;
                case "Electrical Equipment": RadioButton_EE.IsChecked = true; break;
                case "Electrical Fixtures": RadioButton_EF.IsChecked = true; break;
                case "Lighting Devices": RadioButton_LD.IsChecked = true; break;
                case "Lighting Fixtures": RadioButton_LF.IsChecked = true; break;
                case "Cable Trays": RadioButton_CT.IsChecked = true; break;
                case "Security Devices": RadioButton_SD.IsChecked = true; break;
                default: RadioButton_ALL.IsChecked = true; break;
            }

            if (bool_TurnOffEvents)
            {
                RadioButton_CD.Checked += RadioButton_Checked_4;
                RadioButton_DD.Checked += RadioButton_Checked_5;
                RadioButton_EE.Checked += RadioButton_Checked_3;
                RadioButton_EF.Checked += RadioButton_Checked_2;
                RadioButton_LD.Checked += RadioButton_Checked_1;
                RadioButton_LF.Checked += RadioButton_Checked;
                RadioButton_CT.Checked += RadioButton_Checked_7;
                RadioButton_SD.Checked += RadioButton_Checked_6;
                RadioButton_ALL.Checked += RadioButton_ALL_Checked;
            }


        }
        private void extract_And_Insert_Update(bool bool_RevitGeometrySelected,  UIDocument uidoc, Tuple<Element, Element> couldBeNull, bool bool_NOT_EQUALS_ExtractEntireSchedule)
        {
            EnumerableRowCollection results = does_it_already_exist_in_dataTable(uidoc, couldBeNull.Item2);
            List<DataRow> listDataRow = results.Cast<DataRow>().ToList();

            ViewSchedule viewschedule = null;

            ///right there is bug here and it broke it so i will probably need to redo it, 
            ///there must be a way i can figure out if an item has appears on a schedule before
            ///we come back and and start by reversing our selectin

            //the 

            Document doc = uidoc.Document; // myListView_ALL_Fam_Master.Items.Add(doc.GetElement(uidoc.Selection.GetElementIds().First()).Name);
            View view_ActiveView = uidoc.ActiveView;

            if (bool_RevitGeometrySelected)
            {
                if (listDataRow.Count() == 0)
                {
                    viewschedule = fromSelection(view_ActiveView, doc, uidoc, !bool_NOT_EQUALS_ExtractEntireSchedule);

                    if (viewschedule == null)
                    {
                        MessageBox.Show("Because this is the first time this fitting has been saved, please do it from a 'Schedule' type view so that it can be isolated with its friends on the same schedule.");
                        return;
                    }
                }
            }
            else
            {
                viewschedule = fromSelection(view_ActiveView, doc, uidoc, !bool_NOT_EQUALS_ExtractEntireSchedule);

                if (viewschedule == null)
                {
                    MessageBox.Show("Because this is the first time this fitting has been saved, please do it from a 'Schedule' type view so that it can be isolated with its friends on the same schedule.");
                    return;
                }
            }

        ////if (viewschedule == null)
        ////{
        ////    MessageBox.Show("it is null");
        ////    return;
        ////}

        ////} else
        ////{
        ////    //the user must be told it must be saved from a schedule
        ////    if (listDataRow.Count() == 0)
        ////    {
        ////        Document doc = uidoc.Document; // myListView_ALL_Fam_Master.Items.Add(doc.GetElement(uidoc.Selection.GetElementIds().First()).Name);

        ////        View view_ActiveView = uidoc.ActiveView;

        ////        viewschedule = fromSelection(view_ActiveView, doc, uidoc, true);

        ////        if (viewschedule == null)
        ////        {
        ////            MessageBox.Show("Because this is the first time this fitting has been saved, please do it from a 'Schedule' type view so that it can be isolated with its friends on the same schedule.");
        ////            return;
        ////        }
        ////    }
        ////}

        ///muddled now lets see if we can break out of it
        ///we can break out of it by, is not coming from a schedule
        ///the problme is that we need the same thing to happen here in the case it indivudually come in from the schedules
        ///we're coming in indivitually from a schedules

        string myString_Date = "";

            if (true)  //these are actions on listDataRow
            {
                if (listDataRow.Count() != 0)
                {
                    myString_Date = retrive_myString_Date(results);
                }
                else
                {
                    string string_DirectoryToCreate_Storage = method_Storage_DBFile_Directory() + @"\Admin Storage";

                    myString_Date = DateTime.Now.ToString("yyyyMMdd HHmm ss ff");
                    string myString_DataStore = string_DirectoryToCreate_Storage + "\\" + myString_Date;

                    if (System.IO.Directory.Exists(myString_DataStore))
                    {
                        MessageBox.Show("Please allow 1 second between clicks to avoid duplicates.");
                        return;
                    }
                    System.IO.Directory.CreateDirectory(myString_DataStore);
                }
            } //these are actions on listDataRow

            List<Parameter> listParameters = couldBeNull.Item2.Parameters.Cast<Parameter>().OrderBy(x => x.Definition.Name).ToList();

            dictParameters_BuiltInt.Clear();  //three of four
            dictParameters_Shared.Clear();  //three of four
            dictParameters_Project.Clear();  //three of four


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

                if (parameter2.IsShared)
                {
                    dictParameters_Shared.Add(parameter2.GUID, new NameAndValue() { sName = parameter2.Definition.Name, sValue = s_returnValue });
                }
                else
                {
                    if (parameter2.Id.IntegerValue > 0)
                    {
                        if (!dictParameters_Project.ContainsKey(parameter2.Definition.Name))
                        {
                            dictParameters_Project.Add(parameter2.Definition.Name, new NameAndValue() { sName = parameter2.Definition.Name, sValue = s_returnValue });
                        }
                    }
                }
            }


             Dictionary<string, Bitmap> imageDictionary = new Dictionary<string, Bitmap>();

            if (true)
            {
                string string_DirectoryToCreate_Storage = method_Storage_DBFile_Directory() + @"\Admin Storage";
                string myString_DataStore = string_DirectoryToCreate_Storage + "\\" + myString_Date;

                if(System.IO.File.Exists(myString_DataStore + @"\Parameters_Thumbnails.xml"))
                {
                    Stream stream = new FileStream(myString_DataStore + @"\Parameters_Thumbnails.xml", FileMode.Open, FileAccess.Read);
                    DataContractSerializer serializer = new DataContractSerializer(typeof(Dictionary<string, Bitmap>));   //one of four
                    imageDictionary = serializer.ReadObject(stream) as Dictionary<string, Bitmap>; stream.Close();  //two of four
                }
            }


            if (couldBeNull.Item2.LookupParameter("Photometric Web File") != null)
            {
                string myString_Filename = couldBeNull.Item2.get_Parameter(BuiltInParameter.FBX_LIGHT_PHOTOMETRIC_FILE).AsString();

                if (myString_Filename != "")
                {
                    string myString_IESCache = couldBeNull.Item2.get_Parameter(BuiltInParameter.FBX_LIGHT_PHOTOMETRIC_FILE_CACHE).AsString();

                    byte[] data = System.Text.UnicodeEncoding.Unicode.GetBytes(myString_IESCache);

                    string string_DirectoryToCreate_Storage = method_Storage_DBFile_Directory() + @"\Admin Storage";
                    string DirPath_Date_Dir = string_DirectoryToCreate_Storage + "\\" + myString_Date + "\\";

                    string myString_FullFilePath = DirPath_Date_Dir + myString_Filename;

                    File.WriteAllBytes(myString_FullFilePath, data);

                    imageDictionary = overIfThere_AndDarkOnIES(myString_FullFilePath, imageDictionary);
                }
            }

            if (couldBeNull.Item2.LookupParameter("PRL_LuminaireSchedule_ImageSymbolic") != null)
            {
                Parameter parameterImageSymbolic = couldBeNull.Item2.LookupParameter("PRL_LuminaireSchedule_ImageSymbolic");
                string myString_Filename = parameterImageSymbolic.AsValueString();
                // string myString_IESCache = elementType.get_Parameter(BuiltInParameter.image).AsString();

                string string_DirectoryToCreate_Storage = method_Storage_DBFile_Directory() + @"\Admin Storage";
                string DirPath_Date_Dir = string_DirectoryToCreate_Storage + "\\" + myString_Date + "\\";

                string myString_FullFilePath = DirPath_Date_Dir + myString_Filename;

                ImageType imagetype = uidoc.Document.GetElement(parameterImageSymbolic.AsElementId()) as ImageType;

                if (imagetype != null)
                {
                    imagetype.GetImage().Save(myString_FullFilePath);
                    imageDictionary = overIfThere_AndDarkOnIES(myString_FullFilePath, imageDictionary);
                }
            }

            if (couldBeNull.Item2.LookupParameter("PRL_LuminaireSchedule_Image") != null)
            {
                Parameter parameterImageSymbolic = couldBeNull.Item2.LookupParameter("PRL_LuminaireSchedule_Image");

                string myString_Filename = parameterImageSymbolic.AsValueString();
                // string myString_IESCache = elementType.get_Parameter(BuiltInParameter.image).AsString();

                string string_DirectoryToCreate_Storage = method_Storage_DBFile_Directory() + @"\Admin Storage";
                string DirPath_Date_Dir = string_DirectoryToCreate_Storage + "\\" + myString_Date + "\\";

                string myString_FullFilePath = DirPath_Date_Dir + myString_Filename;

                ImageType imagetype = uidoc.Document.GetElement(parameterImageSymbolic.AsElementId()) as ImageType;

                if (imagetype != null)
                {
                    imagetype.GetImage().Save(myString_FullFilePath);
                    imageDictionary = overIfThere_AndDarkOnIES(myString_FullFilePath, imageDictionary);
                }
            }

            if (couldBeNull.Item2.LookupParameter("PRL_LuminaireSchedule_ImageLegend") != null)
            {
                Parameter parameterImageSymbolic = couldBeNull.Item2.LookupParameter("PRL_LuminaireSchedule_ImageLegend");

                string myString_Filename = parameterImageSymbolic.AsValueString();
                // string myString_IESCache = elementType.get_Parameter(BuiltInParameter.image).AsString();

                string string_DirectoryToCreate_Storage = method_Storage_DBFile_Directory() + @"\Admin Storage";
                string DirPath_Date_Dir = string_DirectoryToCreate_Storage + "\\" + myString_Date + "\\";

                string myString_FullFilePath = DirPath_Date_Dir + myString_Filename;

                ImageType imagetype = uidoc.Document.GetElement(parameterImageSymbolic.AsElementId()) as ImageType;

                if (imagetype != null)
                {
                    imagetype.GetImage().Save(myString_FullFilePath);
                    imageDictionary = overIfThere_AndDarkOnIES(myString_FullFilePath, imageDictionary);
                }
            }

            Parameter parameter = couldBeNull.Item2.get_Parameter(BuiltInParameter.ALL_MODEL_TYPE_IMAGE);

            if (parameter != null)
            {
                if (parameter.HasValue)
                {
                    string myString_Filename = parameter.AsValueString();
                    // string myString_IESCache = elementType.get_Parameter(BuiltInParameter.image).AsString();
                    string string_DirectoryToCreate_Storage = method_Storage_DBFile_Directory() + @"\Admin Storage";
                    string DirPath_Date_Dir = string_DirectoryToCreate_Storage + "\\" + myString_Date + "\\";
                    string myString_FullFilePath = DirPath_Date_Dir + myString_Filename;
                    ImageType imagetype = uidoc.Document.GetElement(parameter.AsElementId()) as ImageType;
                    if (imagetype != null)
                    {
                        imagetype.GetImage().Save(myString_FullFilePath);
                        imageDictionary = overIfThere_AndDarkOnIES(myString_FullFilePath, imageDictionary);
                    }
                }
            }

            if (couldBeNull.Item1 != null)
            {
                Parameter parameter2 = couldBeNull.Item1.get_Parameter(BuiltInParameter.ALL_MODEL_IMAGE);
                if (parameter2 != null)
                {
                    if (parameter2.HasValue)
                    {
                        string myString_Filename = parameter2.AsValueString();
                        // string myString_IESCache = elementType.get_Parameter(BuiltInParameter.image).AsString();
                        string string_DirectoryToCreate_Storage = method_Storage_DBFile_Directory() + @"\Admin Storage";
                        string DirPath_Date_Dir = string_DirectoryToCreate_Storage + "\\" + myString_Date + "\\";
                        string myString_FullFilePath = DirPath_Date_Dir + myString_Filename;
                        ImageType imagetype = uidoc.Document.GetElement(parameter2.AsElementId()) as ImageType;
                        imagetype.GetImage().Save(myString_FullFilePath);
                        imageDictionary = overIfThere_AndDarkOnIES(myString_FullFilePath, imageDictionary);
                    }
                }
            }

            /// don't know if to look up or down, the only thing we know is that....
            /// the thumbnail file was not created
            if (true)
            {
                string string_DirectoryToCreate_Storage = method_Storage_DBFile_Directory() + @"\Admin Storage";
                string myString_DataStore = string_DirectoryToCreate_Storage + "\\" + myString_Date;

                Stream stream = new FileStream(myString_DataStore + @"\Parameters_Thumbnails.xml", FileMode.Create, FileAccess.Write);  //of course this is bad because of c:
                DataContractSerializer serializer = new DataContractSerializer(typeof(Dictionary<string, Bitmap>));
                serializer.WriteObject(stream, imageDictionary); stream.Close();
            }

            if (true)
            {
                Stream stream = new FileStream(method_XML_Parameters_BuiltIn(myString_Date), FileMode.Create, FileAccess.Write);
                DataContractSerializer serializer = new DataContractSerializer(typeof(Dictionary<int, NameAndValue>));  //four of four
                serializer.WriteObject(stream, dictParameters_BuiltInt); stream.Close();
                //new FileInfo(method_XML_Parameters_BuiltIn(myString_Date)).Attributes |= FileAttributes.Hidden;
            }
            if (true)
            {
                Stream stream = new FileStream(method_XML_Parameters_Shared(myString_Date), FileMode.Create, FileAccess.Write);
                DataContractSerializer serializer = new DataContractSerializer(typeof(Dictionary<Guid, NameAndValue>));  //four of four
                serializer.WriteObject(stream, dictParameters_Shared); stream.Close();
                //new FileInfo(method_XML_Parameters_Shared(myString_Date)).Attributes |= FileAttributes.Hidden;
            }
            if (true)
            {
                Stream stream = new FileStream(method_XML_Parameters_Project(myString_Date), FileMode.Create, FileAccess.Write);
                DataContractSerializer serializer = new DataContractSerializer(typeof(Dictionary<string, NameAndValue>));  //four of four
                serializer.WriteObject(stream, dictParameters_Project); stream.Close();
            }

            if (results.Cast<DataRow>().Count() != 0)
            {
                method_UpdateEntry(viewschedule.Id.IntegerValue, viewschedule.Name, (Int64)results.Cast<DataRow>().ToList()[0][0], string_SearchStringAccumlation());
            }
            else
            {
                //MessageBox.Show("this is hit right-");

                method_NewEntry(viewschedule, couldBeNull.Item2.Id.IntegerValue, myString_Date, string_SearchStringAccumlation());

                textBoxSearch.Text = "";
                Properties.Settings.Default.lvMasterListSelectedIndex_WillNotWorkWhenFilterActive = 0;
                Properties.Settings.Default.Save();
                Properties.Settings.Default.Reload();
            }
        }
        private void finalSteps_ToShow()
        {
            method_LoadUpMasterList();
            lv_MasterList.SelectedIndex = Properties.Settings.Default.lvMasterListSelectedIndex_WillNotWorkWhenFilterActive;

            if (lv_MasterList.SelectedItem != null)
            {
                lv_MasterList.ScrollIntoView(lv_MasterList.SelectedItem);
            }

            methodRefresh_LoadUp_lvDragDirectory(LoadDirectoryOrNot.LoadEverything);
        }
        private void buttonNewFromRevit_Click(object sender, RoutedEventArgs e)
        {
            int eL = -1;
            try
            {
                UIDocument uidoc = toavoidloadingrevitdlls.commandData.Application.ActiveUIDocument;
                if (uidoc.Selection.GetElementIds().Count == 0)
                {
                    MessageBox.Show("Please select an element in Revit.");

                    return;
                }
                eL = 3551;
                Tuple<Element, Element> couldBeNull = method_butshouldworkonWallTypes(true, uidoc);
                eL = 3553;
                if (couldBeNull == null) return;

                // Element elementType = couldBeNull.Item2;

                eL = 709;

                extract_And_Insert_Update(true, uidoc, couldBeNull, true); //  Tuple<Element, Element> couldBeNull

                finalSteps_ToShow();
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("buttonNewFromRevit_Click2  happens on WriteObject  try closing the stream    , error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }
        private ViewSchedule fromSelection(View view_ActiveView, Document doc, UIDocument uidoc, bool return_TryForSheetSelect)
        {

            if (view_ActiveView.GetType().Name != "ViewSchedule")
            {
                if (return_TryForSheetSelect)
                {
                    if (uidoc.Selection.GetElementIds().Count != 0)
                    {
                        Element element_HopefullySchedule = doc.GetElement(uidoc.Selection.GetElementIds().First());

                        if (element_HopefullySchedule.GetType() == typeof(ScheduleSheetInstance))
                        {
                            ScheduleSheetInstance scheduleSheetInstance = element_HopefullySchedule as ScheduleSheetInstance;

                            return doc.GetElement(scheduleSheetInstance.ScheduleId) as ViewSchedule;
                        }
                    }
                }
            }
            else
            {
                return view_ActiveView as ViewSchedule;
            }

            return null;
        }
        public bool ExtractEntireSchedule()
        {
            int eL = -1;

            try
            {

                ///if we make it past here we then ask the question if you want just this one or the entire schedule
                ///come of josh think, think think think think, should this be another button alltogether
                ///maybe but we should start from this button
                ///
                UIDocument uidoc = toavoidloadingrevitdlls.commandData.Application.ActiveUIDocument;

                Document doc = uidoc.Document; // myListView_ALL_Fam_Master.Items.Add(doc.GetElement(uidoc.Selection.GetElementIds().First()).Name);

                View view_ActiveView = uidoc.ActiveView;

                ViewSchedule viewschedule = fromSelection(view_ActiveView, doc, uidoc, true);

                if (viewschedule == null)
                {
                    MessageBox.Show("The current view (or selection) is not a 'ViewSchedule'");
                    return false;
                }
                eL = 3267;
                ////MessageBoxResult result = System.Windows.MessageBox.Show("Extract the entire schedule?", "Warning", System.Windows.MessageBoxButton.YesNoCancel);

                ////if (result != MessageBoxResult.Yes) return false;

                ///get it to be a button and not a window
                ///after this we need to get the thing ordered better on the schedule edit

                eL = 3275;

                List<Element> listElement = null;

                if(viewschedule.Definition.GetSortGroupFieldCount() > 0)
                {
                    ScheduleSortGroupField scgf = viewschedule.Definition.GetSortGroupField(0);

                    ScheduleField sf = viewschedule.Definition.GetField(scgf.FieldId);

                    if(sf.GetName().StartsWith("SortOrder_"))
                    {
                        listElement = new FilteredElementCollector(doc, viewschedule.Id).GroupBy(x => x.GetTypeId().IntegerValue).Select(x => x.First()).OrderByDescending(x => doc.GetElement(x.GetTypeId()).get_Parameter((BuiltInParameter)sf.ParameterId.IntegerValue).AsInteger()).ToList();
                    } else
                    {
                        listElement = new FilteredElementCollector(doc, viewschedule.Id).GroupBy(x => x.GetTypeId().IntegerValue).Select(x => x.First()).ToList();
                    }
                } else
                {
                    listElement = new FilteredElementCollector(doc, viewschedule.Id).GroupBy(x => x.GetTypeId().IntegerValue).Select(x => x.First()).ToList();
                }


                eL = 3646;
                //                foreach (Element element in new FilteredElementCollector(doc, viewschedule.Id).GroupBy(x => x.GetTypeId().IntegerValue).Select(x => x.First()).Reverse())
                //                    foreach (Element element in new FilteredElementCollector(doc, viewschedule.Id).GroupBy(x => x.GetTypeId().IntegerValue).Select(x => x.First()).OrderByDescending(x => doc.GetElement(x.GetTypeId()).get_Parameter(BuiltInParameter.ALL_MODEL_TYPE_COMMENTS).AsString()))
                foreach (Element element in listElement)
                {
                    extract_And_Insert_Update(false, uidoc, new Tuple<Element, Element>(element, doc.GetElement(element.GetTypeId())), false); //  Tuple<Element, Element> couldBeNull
                }

                return true;
                ///////////////////////////////////////////////////////////////////// finalSteps_ToShow();

            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("ExtractEntireSchedule, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
                return false ;
            }
            finally
            {
            }
            #endregion

        }
        public static byte[] GuidToByteArray(string guidData)
        {
            return Encoding.ASCII.GetBytes(guidData.ToString());
        }
        string stringFolderStore = "Folder ";
        private void btn_NewFolder_Click(object sender, RoutedEventArgs e)
        {
            int eL = -1;
             
            ///non/select return belongs here

            try
            {
                if (dataTable == null) return;

                string string_DirectoryToCreate_Storage = method_Storage_DBFile_Directory() + @"\Admin Storage";
                string myString_DataStore = string_DirectoryToCreate_Storage + "\\" + ((DataRowView)lv_MasterList.SelectedItem)["FolderName"];
                string s_NewFolderName = stringFolderStore + "01";

                string new_path = "";

                int int_NextImageNumber = 1;
                do
                {
                    s_NewFolderName = s_NewFolderName.Substring(0, s_NewFolderName.Length - 2) + int_NextImageNumber.ToString().PadLeft(2, '0');

                    new_path = myString_DataStore + @"\" + s_NewFolderName;
                    int_NextImageNumber++;

                } while (Directory.Exists(new_path));  //candidate for methodisation 20210107

                string string_DoesTheUserDoSomethingDifferent = s_NewFolderName;

                s_NewFolderName = Microsoft.VisualBasic.Interaction.InputBox("Name of new folder ...", "Please provide name of new folder", s_NewFolderName, -1, -1);

                if (s_NewFolderName == "") return;

                if (string_DoesTheUserDoSomethingDifferent != s_NewFolderName)
                {
                    stringFolderStore = s_NewFolderName + " ";
                    s_NewFolderName = stringFolderStore + "01";

                    int_NextImageNumber = 1;
                    do
                    {
                        s_NewFolderName = s_NewFolderName.Substring(0, s_NewFolderName.Length - 2) + int_NextImageNumber.ToString().PadLeft(2, '0');

                        new_path = myString_DataStore + @"\" + s_NewFolderName;
                        int_NextImageNumber++;

                    } while (Directory.Exists(new_path)); //candidate for methodisation 20210107

                }

                string[] fileEntries = Directory.GetFiles(myString_DataStore);

                if (Directory.Exists(new_path))
                {
                    System.Windows.Forms.MessageBox.Show(new System.Windows.Forms.Form() { TopMost = true }, "Directory '" + s_NewFolderName + "' is already there.");
                }
                else
                {
                    Directory.CreateDirectory(new_path);



                    if (true) //candidate for methodisation 20210222
                    {
                        Dictionary<string, Bitmap> imageDictionary = new Dictionary<string, Bitmap>();

                        if (true)
                        {
                            Stream stream = new FileStream(myString_DataStore + @"\Parameters_Thumbnails.xml", FileMode.Open, FileAccess.Read);
                            DataContractSerializer serializer = new DataContractSerializer(typeof(Dictionary<string, Bitmap>));   //one of four
                            imageDictionary = serializer.ReadObject(stream) as Dictionary<string, Bitmap>; stream.Close();  //two of four
                        }

                        imageDictionary = overIfThere_AndDarkOnIES(new_path, imageDictionary);

                        if (true)
                        {
                            Stream stream = new FileStream(myString_DataStore + @"\Parameters_Thumbnails.xml", FileMode.Create, FileAccess.Write);  //of course this is bad because of c:
                            DataContractSerializer serializer = new DataContractSerializer(typeof(Dictionary<string, Bitmap>));
                            serializer.WriteObject(stream, imageDictionary); stream.Close();
                        }
                    } //candidate for methodisation 20210222




                    methodRefresh_LoadUp_lvDragDirectory(LoadDirectoryOrNot.LoadIconDirectoryOnly);
                    //

                    DragDropHelper.SetIsDragOver((DependencyObject)sender, false);
                }


            }
            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("btn_NewFolder_Click, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }


        internal static Icon GetFileIcon(string fileName, IconSize size)
        {
            SHFILEINFO shinfo = new SHFILEINFO();

            uint flags = SHGFI_SYSICONINDEX;
            if (fileName.IndexOf(":") == -1)
                flags = flags | SHGFI_USEFILEATTRIBUTES;
            if (size == IconSize.small)
                flags = flags | SHGFI_ICON | SHGFI_SMALLICON;
            else flags = flags | SHGFI_ICON;

            
            SHGetFileInfo(fileName, 0, ref shinfo, (uint)Marshal.SizeOf(shinfo), flags);
            return System.Drawing.Icon.FromHandle(shinfo.hIcon);
        }


        public void extractZipFiles(string targetFileDirectory, string zipFileDirectory, string Number)
        {
            Directory.GetFiles(zipFileDirectory, "*.zip", SearchOption.AllDirectories).ToList()
            .ForEach(zipFilePath => {
                var test = Number + "_" + Path.GetFileNameWithoutExtension(zipFilePath);
                var extractPathForCurrentZip = Path.Combine(targetFileDirectory, test);
                if (!Directory.Exists(extractPathForCurrentZip))
                {
                    Directory.CreateDirectory(extractPathForCurrentZip);
                }
                ZipFile.ExtractToDirectory(zipFilePath, extractPathForCurrentZip);
                extractZipFiles(targetFileDirectory, extractPathForCurrentZip, Number);
                
            });
        }

        //the above 

        [DllImport("user32.dll", SetLastError = true)]
        static extern uint RegisterClipboardFormat(string lpszFormat);

        private void lv_DragDirectory_Drop_New(object sender, DragEventArgs e)
        {

            if (lv_MasterList.SelectedItem == null)
            {
                MessageBox.Show("Please select an item in Masterlist.");
                DragDropHelper.SetIsDragOver(listbox_objectmodel, false);
                return;
            }


            try
            {
                string[] file_pathes = (string[])e.Data.GetData(DataFormats.FileDrop);  /*file_pathes contains pathes of dragged selected files*/

                string str_DeleteMe = "";


                if (file_pathes == null)
                {
                    using (Stream theStream = (Stream)e.Data.GetData("FileGroupDescriptorW", true))
                    {
                        byte[] fileContents = new byte[512];

                        theStream.Read(fileContents, 0, 512);

                        StringBuilder fileName = new StringBuilder("");

                        for (int i = 76; fileContents[i] != 0; i += 2) fileName.Append(Convert.ToChar(fileContents[i]));

                        var dataObject = (Com.IDataObject)e.Data;
                        var formatetc = new Com.FORMATETC();
                        Com.STGMEDIUM stgmedium;
                        formatetc.cfFormat = (short)RegisterClipboardFormat("FileContents");
                        formatetc.tymed = Com.TYMED.TYMED_ISTREAM;
                        formatetc.lindex = 0;
                        try
                        {
                            dataObject.GetData(ref formatetc, out stgmedium);
                        } catch
                        {
                            MessageBox.Show("This item will not copy. There are 'zip' file limitations." + Environment.NewLine + Environment.NewLine + "TRY.. just dragging one file at a time (and no folders).");

                            DragDropHelper.SetIsDragOver((DependencyObject)sender, false);
                            return;
                        }
                        Com.IStream istream = (Com.IStream)Marshal.GetTypedObjectForIUnknown(stgmedium.unionmember, typeof(Com.IStream));
                        Com.STATSTG stat;
                        istream.Stat(out stat, 1);
                        byte[] temp = new byte[stat.cbSize];
                        istream.Read(temp, temp.Length, IntPtr.Zero);

                        str_DeleteMe = System.IO.Path.GetTempPath() + "\\" + fileName;

                        File.WriteAllBytes(str_DeleteMe, temp);  //this just needs to go into temp

                        file_pathes = new List<string>() { str_DeleteMe }.ToArray();
                    }
                }
                adding_NewFiles(file_pathes);

                DragDropHelper.SetIsDragOver((DependencyObject)sender, false);
                if (str_DeleteMe != "") System.IO.File.Delete(str_DeleteMe);

            }
            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("lv_DragDirectory_Drop_New" + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        public void adding_NewFiles(string[] file_pathes)
        {
            string string_DirectoryToCreate_Storage = method_Storage_DBFile_Directory() + @"\Admin Storage";
            string myString_DataStore = string_DirectoryToCreate_Storage + "\\" + ((DataRowView)lv_MasterList.SelectedItem)["FolderName"];
            //string new_path = myString_DataStore + @"\" + System.IO.Path.GetFileName(path); //New file path

            string[] fileEntries = Directory.GetFiles(myString_DataStore);

            foreach (var path in file_pathes)
            {
                string new_path = myString_DataStore + @"\" + System.IO.Path.GetFileName(path); //New file path

                if (!System.IO.Directory.Exists(path))
                {
                    if (fileEntries.Contains(new_path))
                    {
                        MessageBoxResult result = System.Windows.MessageBox.Show("File " + System.IO.Path.GetFileName(path) + " is already there...overright?", "Warning", System.Windows.MessageBoxButton.YesNoCancel);

                        if (result != MessageBoxResult.Yes) continue;

                    }  //if file is stil there and user does not wish to overwrite it 'continues'

                    if (new_path.Length >= MAX_FILE_PATH)
                    {
                        System.Windows.Forms.MessageBox.Show(new System.Windows.Forms.Form() { TopMost = true }, "File path is too long.");
                        continue;
                    }

                    File.Copy(path, new_path, true);
                }
                else
                {
                    if (true)
                    {
                        if (new_path.Length >= MAX_FILE_PATH)
                        {
                            System.Windows.Forms.MessageBox.Show(new System.Windows.Forms.Form() { TopMost = true }, "File path is too long.");
                            continue;
                        }

                        FileAttributes attr = File.GetAttributes(path);

                        //detect whether its a directory or file
                        if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                        {
                            Directory.CreateDirectory(new_path);
                        }

                        //Now Create all of the directories
                        foreach (string dirPath in Directory.GetDirectories(path, "*", SearchOption.AllDirectories))
                        {
                            Directory.CreateDirectory(dirPath.Replace(path, new_path));
                        }

                        //Copy all the files & Replaces any files with the same name
                        foreach (string newPath in Directory.GetFiles(path, "*.*", SearchOption.AllDirectories))
                        {
                            if (new_path.Length >= MAX_FILE_PATH)
                            {
                                System.Windows.Forms.MessageBox.Show(new System.Windows.Forms.Form() { TopMost = true }, "File path is too long.");
                                continue;
                            }
                            File.Copy(newPath, newPath.Replace(path, new_path), true);
                        }
                    }
                }
                if (true) //candidate for methodisation 20210222
                {
                    Dictionary<string, Bitmap> imageDictionary = new Dictionary<string, Bitmap>();

                    if (true)
                    {
                        Stream stream = new FileStream(myString_DataStore + @"\Parameters_Thumbnails.xml", FileMode.Open, FileAccess.Read);
                        DataContractSerializer serializer = new DataContractSerializer(typeof(Dictionary<string, Bitmap>));   //one of four
                        imageDictionary = serializer.ReadObject(stream) as Dictionary<string, Bitmap>; stream.Close();  //two of four
                    }

                    imageDictionary = overIfThere_AndDarkOnIES(new_path, imageDictionary);

                    if (true)
                    {
                        Stream stream = new FileStream(myString_DataStore + @"\Parameters_Thumbnails.xml", FileMode.Create, FileAccess.Write);  //of course this is bad because of c:
                        DataContractSerializer serializer = new DataContractSerializer(typeof(Dictionary<string, Bitmap>));
                        serializer.WriteObject(stream, imageDictionary); stream.Close();
                    }
                } //candidate for methodisation 20210222
            }
            methodRefresh_LoadUp_lvDragDirectory(LoadDirectoryOrNot.LoadIconDirectoryOnly);

        }




        private Dictionary<string, Bitmap> overIfThere_AndDarkOnIES(string new_path, Dictionary<string, Bitmap> image_dict)
        {
            int eL = -1;

            try
            {
                FileAttributes attr = File.GetAttributes(new_path);

                string tempPath = new_path;

                bool bool_tempPath_DeleteAfterUse = false;

                //detect whether its a directory or file
                if ((attr & FileAttributes.Directory) != FileAttributes.Directory)
                {
                    tempPath = System.IO.Path.GetTempPath() + System.IO.Path.GetFileName(new_path);
                    bool_tempPath_DeleteAfterUse = true;
                    System.IO.File.Copy(new_path, tempPath, true);
                }

                eL = 3549;
                ShellObject shellobject = ShellFile.FromParsingName(tempPath);
                eL = 3551;
                ShellThumbnail thumbnail = shellobject.Thumbnail;

                thumbnail.FormatOption = ShellThumbnailFormatOption.ThumbnailOnly;

                Bitmap bm;
                eL = 3557;
                try
                {
                    bm = thumbnail.LargeBitmap;
                }
                catch // errors can occur with windows api calls so just skip
                {
                    bm = null;
                }
                if (bm == null)
                {
                    try
                    {
                        thumbnail.FormatOption = ShellThumbnailFormatOption.ThumbnailOnly;
                        bm = thumbnail.LargeBitmap;

                        //////MessageBox.Show(thumbnail.CurrentSize.ToString());


                        //////StringBuilder strB = new StringBuilder(260); // Allocate MAX_PATH chars
                        //////strB.Append(tempPath);

                        //////ushort uicon;
                        //////IntPtr handle = ExtractAssociatedIcon(IntPtr.Zero, strB, out uicon);
                        //////Icon ico = System.Drawing.Icon.FromHandle(handle);

                        //////bm = ico.ToBitmap();

                        bm.MakeTransparent(System.Drawing.Color.Black);
                    }
                    catch // errors can occur with windows api calls so just skip
                    {
          
                        thumbnail.FormatOption = ShellThumbnailFormatOption.IconOnly;
                        bm = thumbnail.LargeBitmap;
                        bm.MakeTransparent(System.Drawing.Color.Black);
                    }

                }

                if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                {
                    bm.MakeTransparent(System.Drawing.Color.Black);
                }


                string str_Last4Characters = tempPath.Substring(Math.Max(0, tempPath.Length - 4)).ToLower();

                if (str_Last4Characters == ".ies" | str_Last4Characters == ".ldt")
                {
                    //Bitmap bitmap2 = GetFileIcon(new_path, IconSize.large).ToBitmap();
                    Bitmap bitmap = FileToIconConverter.resizeImage(bm, new System.Drawing.Size(256, 256), 0, true);

                    if (image_dict.ContainsKey(System.IO.Path.GetFileName(tempPath)))
                    {
                        image_dict[System.IO.Path.GetFileName(tempPath)] = bitmap;
                    }
                    else
                    {
                        image_dict.Add(System.IO.Path.GetFileName(tempPath), bitmap);
                    }
                }
                else
                {
                    //Bitmap bitmap2 = GetFileIcon(new_path, IconSize.large).ToBitmap();
                    Bitmap bitmap = FileToIconConverter.resizeImage(bm, new System.Drawing.Size(256, 256), 0, false);
                    if (image_dict.ContainsKey(System.IO.Path.GetFileName(tempPath)))
                    {
                        image_dict[System.IO.Path.GetFileName(tempPath)] = bitmap;
                    }
                    else
                    {
                        image_dict.Add(System.IO.Path.GetFileName(tempPath), bitmap);
                    }
                }
                if (bool_tempPath_DeleteAfterUse)
                {
                    System.IO.File.Delete(tempPath);
                }
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("overIfThere_AndDarkOnIES, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion



            return image_dict;
        }




        ////public static System.Drawing.Icon GetFileIcon(string name, IconSize size,
        ////                                              bool linkOverlay)
        ////{
        ////    SHFILEINFO shfi = new SHFILEINFO();
        ////    uint flags = SHGFI_ICON | SHGFI_USEFILEATTRIBUTES;

        ////    if (true == linkOverlay) flags += SHGFI_LINKOVERLAY;


        ////    /* Check the size specified for return. */
        ////    if (IconSize.small == size)
        ////    {
        ////        flags += SHGFI_SMALLICON; // include the small icon flag
        ////    }
        ////    else
        ////    {
        ////        flags += SHGFI_LARGEICON;  // include the large icon flag
        ////    }

        ////    SHGetFileInfo(name,
        ////        FILE_ATTRIBUTE_NORMAL,
        ////        ref shfi,
        ////        (uint)System.Runtime.InteropServices.Marshal.SizeOf(shfi),
        ////        flags);


        ////    // Copy (clone) the returned icon to a new object, thus allowing us 
        ////    // to call DestroyIcon immediately
        ////    System.Drawing.Icon icon = (System.Drawing.Icon)
        ////                         System.Drawing.Icon.FromHandle(shfi.hIcon).Clone();
        ////    User32.DestroyIcon(shfi.hIcon); // Cleanup
        ////    return icon;
        ////}



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
                    if (!System.IO.Directory.Exists(path))
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
                        string new_path = myString_DataStore + @"\" + s_FileName; //New file path

                        if (fileEntries.Contains(new_path))
                        {
                            System.Windows.Forms.MessageBox.Show(new System.Windows.Forms.Form() { TopMost = true }, "File " + s_FileName + " is already there.");
                            continue;
                        }

                        if (new_path.Length >= MAX_FILE_PATH)
                        {
                            System.Windows.Forms.MessageBox.Show(new System.Windows.Forms.Form() { TopMost = true }, "File path is too long.");
                            continue;
                        }

                        File.Copy(path, new_path);

                        if (true)
                        {
                            ShellObject shellobject = ShellFile.FromParsingName(new_path);

                            Bitmap bitmap = FileToIconConverter.resizeImage(shellobject.Thumbnail.Bitmap, new System.Drawing.Size(256, 256), 0, false);

                            Dictionary<string, Bitmap> imageDictionary = new Dictionary<string, Bitmap>();
                            imageDictionary.Add(System.IO.Path.GetFileName(new_path), bitmap);
                            imageDictionary.Add(System.IO.Path.GetFileName(new_path) + 2, bitmap);
                            Stream stream = new FileStream(@"C:\Users\Joshua.Lumley\Desktop\delete me\sfd2.xml", FileMode.Create, FileAccess.Write);  //of course this is bad because of c:
                            DataContractSerializer serializer = new DataContractSerializer(typeof(Dictionary<string, Bitmap>));
                            serializer.WriteObject(stream, imageDictionary); stream.Close();
                        }
                    }
                    else
                    {
                        if (true)
                        {
                            string new_path = myString_DataStore + @"\" + System.IO.Path.GetFileName(path); //New file path

                            if (new_path.Length >= MAX_FILE_PATH)
                            {
                                System.Windows.Forms.MessageBox.Show(new System.Windows.Forms.Form() { TopMost = true }, "File path is too long.");
                                continue;
                            }

                            FileAttributes attr = File.GetAttributes(path);

                            //detect whether its a directory or file
                            if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                            {
                                Directory.CreateDirectory(new_path);
                            }

                            //Now Create all of the directories
                            foreach (string dirPath in Directory.GetDirectories(path, "*", SearchOption.AllDirectories))
                            {
                                Directory.CreateDirectory(dirPath.Replace(path, new_path));
                            }

                            //Copy all the files & Replaces any files with the same name
                            foreach (string newPath in Directory.GetFiles(path, "*.*", SearchOption.AllDirectories))
                            {
                                if (new_path.Length >= MAX_FILE_PATH)
                                {
                                    System.Windows.Forms.MessageBox.Show(new System.Windows.Forms.Form() { TopMost = true }, "File path is too long.");
                                    continue;
                                }

                                File.Copy(newPath, newPath.Replace(path, new_path), true);
                            }
                        }
                    }
                }

                methodRefresh_LoadUp_lvDragDirectory(LoadDirectoryOrNot.LoadIconDirectoryOnly);

                DragDropHelper.SetIsDragOver((DependencyObject)sender, false);
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
        public Tuple<Element, Element> method_butshouldworkonWallTypes(bool myBoolPleaseSelectCheck, UIDocument uidoc) //store this in thumbs
        {
            Document doc = uidoc.Document;

            Element firstElement = null;

            if (uidoc.Selection.GetElementIds().Count != 1)
            {
                if (uidoc.Selection.GetElementIds().Select(x => doc.GetElement(x).GetTypeId()).GroupBy(x => x.IntegerValue).Count() != 1)
                {
                    if (myBoolPleaseSelectCheck) MessageBox.Show("Please select only one element.");
                    return null;
                }
            }
            firstElement = doc.GetElement(uidoc.Selection.GetElementIds().Last());

            if (firstElement.GetType() == typeof(IndependentTag))
            {
                IndependentTag myIndependentTag_1355 = firstElement as IndependentTag;
                if (myIndependentTag_1355.TaggedLocalElementId == null) return null;

                firstElement = doc.GetElement(myIndependentTag_1355.TaggedLocalElementId);
            }

            if (true)  //testing the type has a type comment
            {
                if (firstElement.GetTypeId() != null)
                {

                } else
                {
                    MessageBox.Show("Please select an element in Revit - that has GEOMETRY.");
                    return null;
                }
            }



            if (true)  //testing the type has a type comment
            {
                try
                {
                    Element ele = doc.GetElement(firstElement.GetTypeId());
                    if(ele == null)
                    {
                        MessageBox.Show("Please select an element in Revit - that has GEOMETRY.");
                        return null;
                    }
                }
                catch
                {
                    MessageBox.Show("Please select an element in Revit - that has GEOMETRY.");
                    return null;
                }
            }


            //////List<ElementId> listElementID_ThatAppearInModel = new FilteredElementCollector(doc).WhereElementIsNotElementType().Where(x => x.GetTypeId() != null).Select(x => x.GetTypeId()).Distinct().ToList();
            //////List<Element> listElement_OfAParticular_Family = listElementID_ThatAppearInModel.Select(x => doc.GetElement(x)).Where(x => x != null).Where(x => x.Category != null).Where(x => x.Category.Name == myComboBox.SelectedValue.ToString()).Where(x => x.LookupParameter("Type Comments") != null).ToList();



            //////if (firstElement.get_Geometry(new Options()) == null)
            //////{

            //////}



            return new Tuple<Element, Element>(firstElement, doc.GetElement(firstElement.GetTypeId()));
        }
        private void butIMAGE_Click(object sender, RoutedEventArgs e)
        {
            int eL = -1;

            try
            {

                if (dataTable == null) return;


                if (!System.Windows.Clipboard.ContainsImage())
                {
                    MessageBoxResult result = System.Windows.MessageBox.Show("Clipboard does not contain image...open SnippingTool?", "Warning", System.Windows.MessageBoxButton.YesNoCancel);

                    if (result == MessageBoxResult.Yes)
                    {
                        System.Diagnostics.Process snippingToolProcess = new System.Diagnostics.Process();
                        snippingToolProcess.EnableRaisingEvents = true;
                        if (!Environment.Is64BitProcess)
                        {
                            snippingToolProcess.StartInfo.FileName = "C:\\Windows\\sysnative\\SnippingTool.exe";
                            snippingToolProcess.Start();
                        }
                        else
                        {
                            snippingToolProcess.StartInfo.FileName = "C:\\Windows\\system32\\SnippingTool.exe";
                            snippingToolProcess.Start();
                        }
                    }

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

                } while (File.Exists(s_fileName));


                var img = System.Windows.Forms.Clipboard.GetImage();
                img.Save(s_fileName, System.Drawing.Imaging.ImageFormat.Png);


                if (true) //candidate for methodisation 20210222
                {
                    Dictionary<string, Bitmap> imageDictionary = new Dictionary<string, Bitmap>();

                    if (true)
                    {
                        Stream stream = new FileStream(myString_DataStore + @"\Parameters_Thumbnails.xml", FileMode.Open, FileAccess.Read);
                        DataContractSerializer serializer = new DataContractSerializer(typeof(Dictionary<string, Bitmap>));   //one of four
                        imageDictionary = serializer.ReadObject(stream) as Dictionary<string, Bitmap>; stream.Close();  //two of four
                    }

                    imageDictionary = overIfThere_AndDarkOnIES(s_fileName, imageDictionary);

                    if (true)
                    {
                        Stream stream = new FileStream(myString_DataStore + @"\Parameters_Thumbnails.xml", FileMode.Create, FileAccess.Write);  //of course this is bad because of c:
                        DataContractSerializer serializer = new DataContractSerializer(typeof(Dictionary<string, Bitmap>));
                        serializer.WriteObject(stream, imageDictionary); stream.Close();
                    }
                } //candidate for methodisation 20210222


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


            /*
             * 
                         if (!System.IO.Directory.Exists(myString_DataStore))
            {
                MessageBox.Show("Directory has been deleted.");
                return false;
            }
             * */

            try
            {
                if (lv_MasterList.SelectedIndex == -1)
                {
                    MessageBox.Show("Please select an item in the main list.");
                    return;
                }

                ////MessageBox.Show(((DataRowView)lv_MasterList.SelectedItem)["SortOrder"].ToString());

                ////return;

                MessageBoxResult result = System.Windows.MessageBox.Show("Deleting the selected item." + Environment.NewLine + Environment.NewLine + "Are you really sure?", "", System.Windows.MessageBoxButton.YesNoCancel);

                if (result != MessageBoxResult.Yes)
                {
                    return;
                }

                foreach (var lvi in lv_MasterList.SelectedItems)
                {
                    OleDbConnection_ButtonOK = reconnectPrivateFlexible(false, string_DatabaseLocation, connRLPrivateFlexible);
                    using (SQLiteCommand cmdInsert = ConnectionCommandType(OleDbConnection_ButtonOK))
                    {
                        cmdInsert.CommandText = @"delete from pkRevitMasterTable where FolderName = '" + ((DataRowView)lvi)["FolderName"] + "';";
                        OleDbConnection_ButtonOK.Open();
                        //   if (!myMainWindow.boolDeveloperMode) cmdInsert.ExecuteNonQueryAsync();
                        cmdInsert.ExecuteNonQuery();
                        OleDbConnection_ButtonOK.Close();
                        OleDbConnection_ButtonOK.Dispose();
                        GC.Collect();
                    }
                    if (((DataRowView)lvi)["FolderName"] == myLabel_Directory.Content)
                    {
                        //model2 = new Model2(this)
                        ////////////////combing_models.model.Files = new List<string>().ToArray();
                        myLabel_Directory.Content = "yyyyMMdd HHmm ss";
                        ////////////////////lb_listBox.IsEnabled = false;
                    }
                    string string_DirectoryToCreate_Storage = method_Storage_DBFile_Directory() + @"\Admin Storage";
                    string DirPath_Date_Dir = string_DirectoryToCreate_Storage + "\\" + ((DataRowView)lvi)["FolderName"];

                    if (System.IO.Directory.Exists(DirPath_Date_Dir))
                    {
                        System.IO.Directory.Delete(DirPath_Date_Dir, true);
                    }
                }


                if (true)  //this is a candidate for methodisation, 20210221 will need to hunt and find the rest
                {
                    List<Int64> list_Int64 = new List<Int64>();

                    if (lv_MasterList.SelectedIndex != -1)
                    {
                        foreach (DataRowView drb in lv_MasterList.SelectedItems.Cast<DataRowView>())
                        {
                            list_Int64.Add((Int64)drb["ID"]);
                        }
                    }

                    method_LoadUpMasterList();

                    rowFilter(true, false);  ///becuase this works it would suggest the complexity is not required in other places
                    eL = 3897;

                        foreach (Int64 theInt in list_Int64)
                        {
                            if(lv_MasterList.Items.Cast<DataRowView>().Where(x => (Int64)x["ID"] == theInt).Count() != 0)
                            {
                                int indexOf = lv_MasterList.Items.Cast<DataRowView>().ToList().IndexOf(lv_MasterList.Items.Cast<DataRowView>().Where(x => (Int64)x["ID"] == theInt).First());

                                lv_MasterList.SelectedItems.Add(lv_MasterList.Items[indexOf]);
                            }
                        }
                        eL = 3907;

                        lv_MasterList.ScrollIntoView(lv_MasterList.SelectedItem);

                } //this is a candidate for methodisation, 20210221 will need to hunt and find the rest
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
            int eL = -1;

            try
            {
                if (dataTable == null) return;
                rowFilter(bool_IsolationIsOn, true);
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("textBoxSearch_KeyUp, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion

            //////// myDataTableNotAGrid.DefaultView.RowFilter = filterAccumlation;//implication of one text filter
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
            int eL = -1;

            try
            {
                if (dataTable == null) return;

                textBoxSearch.Text = "";
                rowFilter(bool_IsolationIsOn, true);
                //dataTable.DefaultView.RowFilter = "" ;//implication of one text filter
                textBoxSearch.Focus();
                textBoxSearch.SelectAll();
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("buttonClearSearchField_Click, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            int eL = -1;

            try
            {
                //ComboBoxProjectFilter
                if(ComboBoxProjectFilter.SelectedItem != null)  Properties.Settings.Default.ComboBoxProjectFilter = ((KeyValuePair<Guid, string>)ComboBoxProjectFilter.SelectedItem).Key;

                Properties.Settings.Default.SearchStringRemember = textBoxSearch.Text;

                Properties.Settings.Default.lvMasterListSelectedIndex_WillNotWorkWhenFilterActive = lv_MasterList.SelectedIndex;

                Properties.Settings.Default.bool_IsolationIsOn = bool_IsolationIsOn;

                Properties.Settings.Default.str_carryOverScheduleId = str_carryOverScheduleId;
                Properties.Settings.Default.str_carryOverGuid = str_carryOverGuid;
                Properties.Settings.Default.str_carryOverScheduleId_string = str_carryOverScheduleId_string;
                Properties.Settings.Default.str_CurrentModeOfExplorer = str_CurrentModeOfExplorer;

              //  Properties.Settings.Default.DropboxOrGoogleDriveOrOnedrive_Path = "";

                //////////ModeProject  str_CurrentModeOfExplorer

                string string_SaveCategory = "All categories";
                if (RadioButton_CD.IsChecked.Value) string_SaveCategory = "Communication Devices";  //in three places search for turnoffevents category
                if (RadioButton_DD.IsChecked.Value) string_SaveCategory = "Data Devices";
                if (RadioButton_EE.IsChecked.Value) string_SaveCategory = "Electrical Equipment";
                if (RadioButton_EF.IsChecked.Value) string_SaveCategory = "Electrical Fixtures";
                if (RadioButton_LD.IsChecked.Value) string_SaveCategory = "Lighting Devices";
                if (RadioButton_LF.IsChecked.Value) string_SaveCategory = "Lighting Fixtures";
                if (RadioButton_CT.IsChecked.Value) string_SaveCategory = "Cable Trays";
                if (RadioButton_SD.IsChecked.Value) string_SaveCategory = "Security Devices";
                if (RadioButton_ALL.IsChecked.Value) string_SaveCategory = "All categories";

                Properties.Settings.Default.str_carryOverCategory = string_SaveCategory;

                Properties.Settings.Default.Top = this.Top;
                Properties.Settings.Default.Left = this.Left;
                Properties.Settings.Default.Height = this.Height;
                Properties.Settings.Default.Width = this.Width;
                Properties.Settings.Default.DoubleSlider = this.slider.Value;
                Properties.Settings.Default.Save();

                win_moreParam.closeSpecial();
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
        ////////////////////private void btn_OneLevelUp_Click(object sender, RoutedEventArgs e) //delete this?
        ////////////////////{
        ////////////////////    int eL = -1;

        ////////////////////    try
        ////////////////////    {
        ////////////////////        string string_DirectoryToCreate_Storage = method_Storage_DBFile_Directory() + @"\Admin Storage";
        ////////////////////        string DirPath_Date_Dir = string_DirectoryToCreate_Storage + "\\" + ((DataRowView)lv_MasterList.SelectedItem)["FolderName"];

        ////////////////////        combing_models.model.Path = Directory.GetParent(combing_models.model.Path).FullName;

        ////////////////////        if (combing_models.model.Path == DirPath_Date_Dir)
        ////////////////////        {
        ////////////////////            btn_OneLevelUp.IsEnabled = false;
        ////////////////////        }
        ////////////////////    }

        ////////////////////    #region catch and finally
        ////////////////////    catch (Exception ex)
        ////////////////////    {
        ////////////////////        _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("btn_OneLevelUp_Click, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
        ////////////////////    }
        ////////////////////    finally
        ////////////////////    {
        ////////////////////    }
        ////////////////////    #endregion
        ////////////////////}
        private void ListBoxItem_PreviewMouseDoubleClick_New(object sender, MouseButtonEventArgs e)
        {
            int eL = -1;
            if (lv_MasterList.SelectedItems.Count == 0) return;
            ///non/select return belongs here
            try
            {
                //MessageBox.Show("hello");
                eL = 4603;
                string string_DirectoryToCreate_Storage = method_Storage_DBFile_Directory() + @"\Admin Storage";
                eL = 4605;
                string DirPath_Date_Dir = string_DirectoryToCreate_Storage + "\\" + ((DataRowView)lv_MasterList.SelectedItem)["FolderName"];
                eL = 4607;

                ////ListBox parent = (ListBox)sender;
                ////object data = GetDataFromListBox(parent, e.GetPosition(parent));

                //ListBoxItem
                string str_FileName = ((KeyValuePair<string, Bitmap>)((ListBoxItem)sender).DataContext).Key;
                eL = 4609;
                str_FileName = DirPath_Date_Dir + "\\" + str_FileName;
                eL = 4611;
                if (listbox_objectmodel.SelectedItems.Count == 0) return;
                eL = 4613; 

                System.Diagnostics.Process.Start(str_FileName);


                ////MessageBoxResult result = System.Windows.MessageBox.Show("Open?  " + str_FileName, "", System.Windows.MessageBoxButton.YesNoCancel);

                ////if (result == MessageBoxResult.Yes)
                ////{
                ////    System.Diagnostics.Process.Start(str_FileName);
                ////}
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("ListBoxItem_PreviewMouseDoubleClick_New, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }


        ////////////////////private void ListBoxItem_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        ////////////////////{
        ////////////////////    int eL = -1;

        ////////////////////    try
        ////////////////////    {
        ////////////////////        //  string str_FileName = ((KeyValuePair<string, Bitmap>)listbox_objectmodel.SelectedItem).Key;
        ////////////////////        if (lb_listBox.SelectedItems.Count == 0) return;

        ////////////////////        if (System.IO.Directory.Exists((string)lb_listBox.SelectedItem))
        ////////////////////        {
        ////////////////////            combing_models.model.Path = (string)lb_listBox.SelectedItem;
        ////////////////////            btn_OneLevelUp.IsEnabled = true;
        ////////////////////            return;
        ////////////////////        }

        ////////////////////        MessageBoxResult result = System.Windows.MessageBox.Show("Open?  " + Path.GetFileNameWithoutExtension((string)lb_listBox.SelectedItem), "", System.Windows.MessageBoxButton.YesNoCancel);

        ////////////////////        if (result == MessageBoxResult.Yes)
        ////////////////////        {
        ////////////////////            System.Diagnostics.Process.Start((string)lb_listBox.SelectedItem);
        ////////////////////        }
        ////////////////////    }

        ////////////////////    #region catch and finally
        ////////////////////    catch (Exception ex)
        ////////////////////    {
        ////////////////////        _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("ListBoxItem_PreviewMouseDoubleClick, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
        ////////////////////    }
        ////////////////////    finally
        ////////////////////    {
        ////////////////////    }
        ////////////////////    #endregion
        ////////////////////}
        private void method_UpdateEntry(int viewscheduleIdIntegerValue, string viewscheduleName, Int64 index, string str_SearchTerms)
        {
            int eL = -1;
            try
            {
                //UIDocument uidoc = toavoidloadingrevitdlls.commandData.Application.ActiveUIDocument;
                OleDbConnection_ButtonOK = reconnectPrivateFlexible(false, string_DatabaseLocation, connRLPrivateFlexible);
                using (SQLiteCommand cmdInsert = ConnectionCommandType(OleDbConnection_ButtonOK))
                {
                    //here it is please add a new field here, and to the start DB
                    eL = 2953;
                    if(viewscheduleName == "")
                    {
                        cmdInsert.CommandText = @"update [pkRevitMasterTable] SET [SearchTerms] = @b, [Date] = julianday('now') WHERE ID = @a";
                        cmdInsert.Parameters.Clear();

                    } else
                    {
                        cmdInsert.CommandText = @"update [pkRevitMasterTable] SET [SearchTerms] = @b,  [ScheduleID] = @c,  [ScheduleName] = @d, [Date] = julianday('now') WHERE ID = @a";
                        cmdInsert.Parameters.Clear();
                        cmdInsert.Parameters.AddWithValue("@c", viewscheduleIdIntegerValue);
                        cmdInsert.Parameters.AddWithValue("@d", viewscheduleName);
                    }

                    //cmdInsert.CommandText = @"update [pkRevitMasterTable] SET [Category] = @c, [SearchTerms] = @b, [Date] = julianday('now') WHERE ID = @a";
                    eL = 2954;

                    eL = 2955;
                    cmdInsert.Parameters.AddWithValue("@a", index);
                    eL = 2959;
                    cmdInsert.Parameters.AddWithValue("@b", str_SearchTerms);

                    eL = 2961;
                    //////////    removing becasue it only needed to be used once      ////////////////cmdInsert.Parameters.AddWithValue("@c", uidoc.Document.Settings.Categories.get_Item((BuiltInCategory)int.Parse(dictParameters_BuiltInt[-1140362].sValue)).Name);
                    eL = 2963;
                    ////cmdInsert.Parameters.AddWithValue("@c", uidoc.ActiveView.Id.IntegerValue);
                    ////cmdInsert.Parameters.AddWithValue("@d", uidoc.ActiveView.Name);
                    OleDbConnection_ButtonOK.Open();
                    cmdInsert.ExecuteNonQuery();
                    OleDbConnection_ButtonOK.Close();
                    OleDbConnection_ButtonOK.Dispose();
                    GC.Collect();
                    // MessageBox.Show(  doc.Settings.Categories.get_Item((BuiltInCategory) int.Parse(dictParameters_BuiltInt[-1140362].sValue)).Name );
                }
            }
            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("method_UpdateEntry, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }
        private void method_NewEntry(ViewSchedule viewschedule, Int64 ElementTypeID, string str_String_Date, string str_SearchTerms)
        {
            UIDocument uidoc = toavoidloadingrevitdlls.commandData.Application.ActiveUIDocument;  //uidoc.ActiveView.Id.IntegerValue);
            Int64 Int64_LastMaxCount = methodNewScalar("pkRevitMasterTable");

            OleDbConnection_ButtonOK = reconnectPrivateFlexible(false, string_DatabaseLocation, connRLPrivateFlexible);
            using (SQLiteCommand cmdInsert = ConnectionCommandType(OleDbConnection_ButtonOK))
            {
                cmdInsert.CommandText = @"INSERT INTO [pkRevitMasterTable] ([Date],[FolderName],[SortOrder],[SearchTerms],[ProjectGUID],[ElementTypeID],[ScheduleID],[ScheduleName]) values ( julianday('now'),@a,@b,@c,@d,@e,@f,@g)";
                cmdInsert.Parameters.Clear();
                cmdInsert.Parameters.AddWithValue("@a", str_String_Date);
                cmdInsert.Parameters.AddWithValue("@b", Int64_LastMaxCount);
                cmdInsert.Parameters.AddWithValue("@c", str_SearchTerms);
                cmdInsert.Parameters.AddWithValue("@d", named_guid.ToString());
                cmdInsert.Parameters.AddWithValue("@e", ElementTypeID);
                cmdInsert.Parameters.AddWithValue("@f", viewschedule.Id.IntegerValue);
                cmdInsert.Parameters.AddWithValue("@g", viewschedule.Name);
                // cmdInsert.Parameters.AddWithValue("@h", uidoc.Document.Settings.Categories.get_Item((BuiltInCategory)int.Parse(dictParameters_BuiltInt[-1140362].sValue)).Name);

                OleDbConnection_ButtonOK.Open();
                cmdInsert.ExecuteNonQuery();
                OleDbConnection_ButtonOK.Close();
                OleDbConnection_ButtonOK.Dispose();
                GC.Collect();
            }
        }
        private string string_SearchStringAccumlation()  //this is wrong because it needs to be reordered completely 
        {
            int eL = -1;
            string str_SearchStringAccumlation = "";
            try
            {
                eL = 3003;
                List<Tuple<int, string>> listTupleIntString = new List<Tuple<int, string>>();
                if (true)
                {
                    Stream stream = new FileStream(string_Default_Parameters_BuiltIn, FileMode.Open, FileAccess.Read);
                    DataContractSerializer serializer = new DataContractSerializer(typeof(Dictionary<int, int>));   //one of four
                    dict_SortOrdered_BuiltIn = serializer.ReadObject(stream) as Dictionary<int, int>; stream.Close();  //two of four
                    foreach (KeyValuePair<int, int> kpv in dict_SortOrdered_BuiltIn)
                    {
                        if (dictParameters_BuiltInt.ContainsKey(kpv.Value))
                        {
                            listTupleIntString.Add(new Tuple<int, string>(kpv.Key, dictParameters_BuiltInt[kpv.Value].sValue));
                            //str_SearchStringAccumlation = str_SearchStringAccumlation + "," + dictParameters_BuiltInt[kpv.Value].sValue;
                        }
                    }
                }
                eL = 3021;
                //getting the category, the easist thing is to simply use the work "Category" which isn't going to work.
                if (true)
                {
                    Stream stream = new FileStream(string_Default_Parameters_Shared, FileMode.Open, FileAccess.Read);
                    DataContractSerializer serializer = new DataContractSerializer(typeof(Dictionary<int, Guid>));   //one of four
                    dict_SortOrdered_Shared = serializer.ReadObject(stream) as Dictionary<int, Guid>; stream.Close();  //two of four
                    foreach (KeyValuePair<int, Guid> kpv in dict_SortOrdered_Shared)
                    {
                        if (dictParameters_Shared.ContainsKey(kpv.Value))
                        {
                            listTupleIntString.Add(new Tuple<int, string>(kpv.Key, dictParameters_Shared[kpv.Value].sValue));
                            //str_SearchStringAccumlation = str_SearchStringAccumlation + "," + dictParameters_Shared[kpv.Value].sValue;
                        }
                    }
                }
                eL = 3039;
                if (true)
                {
                    Stream stream = new FileStream(string_Default_Parameters_Project, FileMode.Open, FileAccess.Read);
                    DataContractSerializer serializer = new DataContractSerializer(typeof(Dictionary<int, string>));   //one of four
                    dict_SortOrdered_Project = serializer.ReadObject(stream) as Dictionary<int, string>; stream.Close();  //two of four
                    foreach (KeyValuePair<int, string> kpv in dict_SortOrdered_Project)
                    {
                        if (dictParameters_Project.ContainsKey(kpv.Value))
                        {
                            listTupleIntString.Add(new Tuple<int, string>(kpv.Key, dictParameters_Project[kpv.Value].sValue));
                            //str_SearchStringAccumlation = str_SearchStringAccumlation + "," + dictParameters_Shared[kpv.Value].sValue;
                        }
                    }
                }
                eL = 3055;
                listTupleIntString = listTupleIntString.OrderBy(x => x.Item1).ToList();
                //listTupleIntString.Sort(Key = lam tup: tup[1]);
                for (int a = 0; a < listTupleIntString.Count(); a++)
                {
                    str_SearchStringAccumlation = str_SearchStringAccumlation + "," + listTupleIntString[a].Item2;
                }
                str_SearchStringAccumlation = str_SearchStringAccumlation.Substring(1);
                return str_SearchStringAccumlation.Replace("\n", " ").Replace("\r", " ");
            }
            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("string_SearchStringAccumlation, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
                return str_SearchStringAccumlation.Replace("\n", " ").Replace("\r", " ");
            }
            finally
            {
            }
            #endregion

        }
        private void btn_SearchTermLoad_Click(object sender, RoutedEventArgs e)
        {
            int eL = -1;

            try
            {
                MessageBox.Show(string_SearchStringAccumlation());
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("btn_SearchTermLoad_Click, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }
        private void btn_UpdatedToMatchChosenParameters_Click(object sender, RoutedEventArgs e)
        {
            int eL = -1;

            try
            {
                if (lv_MasterList.SelectedItems.Count == 0)
                {
                    MessageBox.Show("Please select something in the masterlist.");
                    return;
                }

                MessageBoxResult result = System.Windows.MessageBox.Show("Be careful, if the list is not 'isolated' this will take a long time to complete...." + Environment.NewLine + Environment.NewLine + "Continue?", "Warning", System.Windows.MessageBoxButton.YesNoCancel);

                if (result != MessageBoxResult.Yes) return;

                foreach (DataRowView dr in lv_MasterList.Items)
                {
                    if (true)
                    {
                        string fileString = method_XML_Parameters_BuiltIn(dr["FolderName"].ToString());

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
                        string fileString = method_XML_Parameters_Shared(dr["FolderName"].ToString());

                        if (System.IO.File.Exists(fileString))
                        {
                            Stream stream = new FileStream(fileString, FileMode.Open, FileAccess.Read);
                            DataContractSerializer serializer = new DataContractSerializer(typeof(Dictionary<Guid, NameAndValue>));   //one of four
                            dictParameters_Shared = serializer.ReadObject(stream) as Dictionary<Guid, NameAndValue>; stream.Close();  //two of four
                            win_moreParam.lv_Result_Shared.ItemsSource = dictParameters_Shared;
                        }
                        else
                        {
                            win_moreParam.lv_Result_Shared.ItemsSource = null;
                        }
                    }

                    if (true)
                    {
                        string fileString = method_XML_Parameters_Project(dr["FolderName"].ToString());

                        if (System.IO.File.Exists(fileString))
                        {
                            Stream stream = new FileStream(fileString, FileMode.Open, FileAccess.Read);
                            DataContractSerializer serializer = new DataContractSerializer(typeof(Dictionary<string, NameAndValue>));   //one of four
                            dictParameters_Project = serializer.ReadObject(stream) as Dictionary<string, NameAndValue>; stream.Close();  //two of four
                            win_moreParam.lv_Result_Project.ItemsSource = dictParameters_Project;
                        }
                        else
                        {
                            win_moreParam.lv_Result_Project.ItemsSource = null;
                        }
                    }
                    method_UpdateEntry(-1,"", (Int64)dr[0], string_SearchStringAccumlation());
                }


                //////Properties.Settings.Default.lvMasterListSelectedIndex_WillNotWorkWhenFilterActive = lv_MasterList.SelectedIndex;
                //////Properties.Settings.Default.Save();
                //////Properties.Settings.Default.Reload();

                //////method_LoadUpMasterList();
                //////lv_MasterList.SelectedIndex = Properties.Settings.Default.lvMasterListSelectedIndex_WillNotWorkWhenFilterActive;

                //////methodRefresh_LoadUp_lvDragDirectory(LoadDirectoryOrNot.LoadEverything);

                if(true)  //this is a candidate for methodisation, 20210221 will need to hunt and find the rest
                {
                    List<Int64> list_Int64 = new List<Int64>();

                    if (lv_MasterList.SelectedIndex != -1)
                    {
                        foreach (DataRowView drb in lv_MasterList.SelectedItems.Cast<DataRowView>())
                        {
                            list_Int64.Add((Int64)drb["ID"]);
                        }
                    }

                    method_LoadUpMasterList();

                    rowFilter(true, false);  ///becuase this works it would suggest the complexity is not required in other places

                    foreach (Int64 theInt in list_Int64)
                    {
                        if (lv_MasterList.Items.Cast<DataRowView>().Where(x => (Int64)x["ID"] == theInt).Count() != 0)
                        {
                            int indexOf = lv_MasterList.Items.Cast<DataRowView>().ToList().IndexOf(lv_MasterList.Items.Cast<DataRowView>().Where(x => (Int64)x["ID"] == theInt).First());

                            lv_MasterList.SelectedItems.Add(lv_MasterList.Items[indexOf]);
                        }
                    }
                    eL = 3907;

                    lv_MasterList.ScrollIntoView(lv_MasterList.SelectedItem);

                } //this is a candidate for methodisation, 20210221 will need to hunt and find the rest
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("btn_UpdatedToMatchChosenParameters_Click, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }
        private void lv_MasterList_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            try
            {
                if (dataTable == null) return;

                methodRefresh_LoadUp_lvDragDirectory(LoadDirectoryOrNot.LoadEverything);  //this returns a false if the directoy is not there
            }
            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("lv_MasterList_KeyUp" + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }
        private void ComboBoxProjectFilter_DropDownClosed(object sender, EventArgs e)
        {
            int eL = -1;

            try
            {

                if (dataTable == null) return;
                rowFilter(bool_IsolationIsOn, false);
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("ComboBoxProjectFilter_DropDownClosed, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        Guid str_carryOverGuid = new Guid("{00000000-0000-0000-0000-000000000000}");
        Int64 str_carryOverScheduleId = 0;
        string str_carryOverScheduleId_string = "";
        //string str_carryOverCategory_string = "Lighting Fixtures";



        bool bool_IsolationIsOn = false;
        string string_checkedRadioButton = "";



        //////////private void UnIsolate_Commons()
        //////////{
        //////////    lbl_ScheduleName.Content = "";
        //////////    lbl_ScheduleName_Project.Content = "All projects";

        //////////    ComboBoxProjectFilter.SelectedIndex = 0;

        //////////    str_carryOverGuid = Guid.Parse("00000000-0000-0000-0000-000000000000"); 
        //////////    str_carryOverScheduleId = 0;
        //////////    str_carryOverScheduleId_string = "";

        //////////    bool_IsolationIsOn = false;
        //////////}

        private void buttonUnisolate_Click(object sender, RoutedEventArgs e)
        {
            int eL = -1;

            try
            {
                if (lv_MasterList.SelectedIndex != -1)
                {


                 //   textBoxSearch.Text = "";

                    RemoveFilters_UsedOnUnisolate_And_RemoveFilterButton();

                    //////////UnIsolate_Commons();
                    //////////rowFilter(bool_IsolationIsOn, false);

                    // rowFilter(bool_IsolationIsOn);

                }
                else
                {
                    MessageBox.Show("Please select an item first.");
                }

            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("buttonUnisolate_Click, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }
        private void Copy_Text(object sender, RoutedEventArgs e)
        {
            int eL = -1;

            try
            {
                var menuItem = sender as System.Windows.Controls.MenuItem;
                var contextMenu = menuItem.Parent as System.Windows.Controls.ContextMenu;
                var textBlock = contextMenu.PlacementTarget as TextBlock;
                System.Windows.Clipboard.SetText(textBlock.Text.Replace(System.Environment.NewLine, " "));
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("Copy_Text, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        /*

UPDATE pkRevitMasterTable SET SortOrder = -1 WHERE SortOrder = 15;
UPDATE pkRevitMasterTable SET SortOrder = SortOrder + 1 WHERE SortOrder between 12 and 14;
UPDATE pkRevitMasterTable SET SortOrder = 12 WHERE SortOrder = -1;
SELECT * FROM [pkRevitMasterTable] order by [SortOrder];

UPDATE pkRevitMasterTable SET SortOrder = -1 WHERE SortOrder = 12;
UPDATE pkRevitMasterTable SET SortOrder = SortOrder - 1 WHERE SortOrder between 13 and 15;
UPDATE pkRevitMasterTable SET SortOrder = 15 WHERE SortOrder = -1;
SELECT * FROM [pkRevitMasterTable] order by [SortOrder];

*/
        bool triggerTheFirstTime = true;
        private void buttonMoveUp_Click(object sender, RoutedEventArgs e)
        {
            int eL = -1;
            ///we're getting closer well done, when this is done we can have lunch and pound two sports drinks if you want
            try
            {


                if (lv_MasterList.SelectedIndex == -1)
                {
                    MessageBox.Show("Please select an item in the main list.");
                    return;
                }

                if (textBoxSearch.Text != "" & textBoxSearch.Text != "Type anything here ... to search" & triggerTheFirstTime)
                {
                    MessageBox.Show("Please clear search text first!...this will continue anyway if you click the arrow again...but it will make the unfiltered list appear jumbled.");
                    triggerTheFirstTime = false;
                    return;
                }

                triggerTheFirstTime = true;

                List<Int64> list_Int64 = new List<Int64>();

                foreach (DataRowView drb in lv_MasterList.SelectedItems.Cast<DataRowView>())
                {
                    list_Int64.Add((Int64)drb["ID"]);
                }

                IEnumerable<DataRowView> IOrderedEnumerableRowView = lv_MasterList.SelectedItems.Cast<DataRowView>().OrderBy(x => x["SortOrder"]).ToList();

                Int64 int_IntergerFirst = (Int64)IOrderedEnumerableRowView.First()["SortOrder"];
                Int64 int_IntergerLast = (Int64)IOrderedEnumerableRowView.Last()["SortOrder"];

                //because that is not the correct range

                int int_LowestIndex_WhichIs_TheHighestSortOrder = lv_MasterList.Items.Count - -1;

                foreach (DataRowView lvi in lv_MasterList.SelectedItems)
                {
                    int int_IndexOf = lv_MasterList.Items.IndexOf(lvi);

                    if (int_IndexOf < int_LowestIndex_WhichIs_TheHighestSortOrder) int_LowestIndex_WhichIs_TheHighestSortOrder = int_IndexOf;

                    if (int_IndexOf == 0) return;
                }
                int_LowestIndex_WhichIs_TheHighestSortOrder = int_LowestIndex_WhichIs_TheHighestSortOrder - 1;


                OleDbConnection_ButtonOK = reconnectPrivateFlexible(false, string_DatabaseLocation, connRLPrivateFlexible);
                using (SQLiteCommand cmdInsert = ConnectionCommandType(OleDbConnection_ButtonOK))
                {
                    cmdInsert.CommandText = @"
                UPDATE pkRevitMasterTable SET SortOrder = -1 WHERE SortOrder = @highestNumber;
                UPDATE pkRevitMasterTable SET SortOrder = SortOrder + 1 WHERE SortOrder between @lowNumber and @highNumber;
                UPDATE pkRevitMasterTable SET SortOrder = @lowNumber WHERE SortOrder = -1;";
                    cmdInsert.Parameters.Clear();
                    cmdInsert.Parameters.AddWithValue("@highestNumber", ((DataRowView)lv_MasterList.Items[int_LowestIndex_WhichIs_TheHighestSortOrder])["SortOrder"]);
                    cmdInsert.Parameters.AddWithValue("@lowNumber", int_IntergerFirst);
                    cmdInsert.Parameters.AddWithValue("@highNumber", ((Int64)((DataRowView)lv_MasterList.Items[int_LowestIndex_WhichIs_TheHighestSortOrder])["SortOrder"] - 1));
                    ////cmdInsert.Parameters.AddWithValue("@c", uidoc.ActiveView.Id.IntegerValue);
                    ////cmdInsert.Parameters.AddWithValue("@d", uidoc.ActiveView.Name);

                    OleDbConnection_ButtonOK.Open();
                    cmdInsert.ExecuteNonQuery();
                    OleDbConnection_ButtonOK.Close();
                    OleDbConnection_ButtonOK.Dispose();
                    GC.Collect();
                }

                if(true) //candidate for methodisation  20210305, this can be collected up in several further places
                {
                    method_LoadUpMasterList();

                    rowFilter(bool_IsolationIsOn, false);

                    foreach (Int64 theInt in list_Int64)
                    {
                        int indexOf = lv_MasterList.Items.Cast<DataRowView>().ToList().IndexOf(lv_MasterList.Items.Cast<DataRowView>().Where(x => (Int64)x["ID"] == theInt).First());
                        lv_MasterList.SelectedItems.Add(lv_MasterList.Items[indexOf]);
                    }

                    lv_MasterList.ScrollIntoView(lv_MasterList.SelectedItem);
                }

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

        bool triggerTheFirstTimeTwo = true;
        private void buttonMoveDown_Click(object sender, RoutedEventArgs e)
        {
            int eL = -1;
            ///we're getting closer well done, when this is done we can have lunch and pound two sports drinks if you want
            try
            {
                if (lv_MasterList.SelectedIndex == -1)
                {
                    MessageBox.Show("Please select an item in the main list.");
                    return;
                }

                if (lv_MasterList.SelectedIndex == lv_MasterList.Items.Count - 1)
                {
                    return;
                }


                if (textBoxSearch.Text != "" & textBoxSearch.Text != "Type anything here ... to search" & triggerTheFirstTimeTwo)
                {
                    MessageBox.Show("Please clear search text first!...this will continue anyway if you click the arrow again...but it will make the unfiltered list appear jumbled.");
                    triggerTheFirstTimeTwo = false;
                    return;
                }

                triggerTheFirstTimeTwo = true;

                List<Int64> list_Int64 = new List<Int64>();

                foreach (DataRowView drb in lv_MasterList.SelectedItems.Cast<DataRowView>())
                {
                    list_Int64.Add((Int64)drb["ID"]);
                }

                IEnumerable<DataRowView> IOrderedEnumerableRowView = lv_MasterList.SelectedItems.Cast<DataRowView>().OrderBy(x => x["SortOrder"]).ToList();

                Int64 int_IntergerFirst = (Int64)IOrderedEnumerableRowView.First()["SortOrder"];
                Int64 int_IntergerLast = (Int64)IOrderedEnumerableRowView.Last()["SortOrder"];


                int int_HighestIndex_WhichIs_TheLowestSortOrder = 0;

                foreach (DataRowView lvi in lv_MasterList.SelectedItems)
                {
                    int int_IndexOf = lv_MasterList.Items.IndexOf(lvi);

                    if (int_IndexOf > int_HighestIndex_WhichIs_TheLowestSortOrder) int_HighestIndex_WhichIs_TheLowestSortOrder = int_IndexOf;

                    if (int_IndexOf == lv_MasterList.Items.Count - 1)
                    {
                        return;
                    }
                }
                int_HighestIndex_WhichIs_TheLowestSortOrder = int_HighestIndex_WhichIs_TheLowestSortOrder + 1;


                //UPDATE pkRevitMasterTable SET SortOrder = -1 WHERE SortOrder = 12;
                //UPDATE pkRevitMasterTable SET SortOrder = SortOrder - 1 WHERE SortOrder between 13 and 15;
                //UPDATE pkRevitMasterTable SET SortOrder = 15 WHERE SortOrder = -1;

                OleDbConnection_ButtonOK = reconnectPrivateFlexible(false, string_DatabaseLocation, connRLPrivateFlexible);
                using (SQLiteCommand cmdInsert = ConnectionCommandType(OleDbConnection_ButtonOK))
                {
                    cmdInsert.CommandText = @"
                UPDATE pkRevitMasterTable SET SortOrder = -1 WHERE SortOrder = @lowestNumber;
                UPDATE pkRevitMasterTable SET SortOrder = SortOrder - 1 WHERE SortOrder between @lowNumber and @highNumber;
                UPDATE pkRevitMasterTable SET SortOrder = @highNumber WHERE SortOrder = -1;";
                    cmdInsert.Parameters.Clear();
                    cmdInsert.Parameters.AddWithValue("@lowestNumber", ((DataRowView)lv_MasterList.Items[int_HighestIndex_WhichIs_TheLowestSortOrder])["SortOrder"]);
                    cmdInsert.Parameters.AddWithValue("@lowNumber", ((Int64)((DataRowView)lv_MasterList.Items[int_HighestIndex_WhichIs_TheLowestSortOrder])["SortOrder"] + 1));
                    cmdInsert.Parameters.AddWithValue("@highNumber", int_IntergerLast);
                    ////cmdInsert.Parameters.AddWithValue("@c", uidoc.ActiveView.Id.IntegerValue);
                    ////cmdInsert.Parameters.AddWithValue("@d", uidoc.ActiveView.Name);

                    OleDbConnection_ButtonOK.Open();
                    cmdInsert.ExecuteNonQuery();
                    OleDbConnection_ButtonOK.Close();
                    OleDbConnection_ButtonOK.Dispose();
                    GC.Collect();
                }
                method_LoadUpMasterList();

                rowFilter(bool_IsolationIsOn, false);

                foreach (Int64 theInt in list_Int64)
                {
                    int indexOf = lv_MasterList.Items.Cast<DataRowView>().ToList().IndexOf(lv_MasterList.Items.Cast<DataRowView>().Where(x => (Int64)x["ID"] == theInt).First());
                    //MessageBox.Show(indexOf.ToString());
                    lv_MasterList.SelectedItems.Add(lv_MasterList.Items[indexOf]);
                }

                lv_MasterList.ScrollIntoView(lv_MasterList.SelectedItem);
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
        private void btn_refresh_Click(object sender, RoutedEventArgs e)
        {
            int eL = -1;

            try
            {
                if (dataTable == null) return;

                List<Int64> list_Int64 = new List<Int64>();
                if (lv_MasterList.SelectedIndex != -1)
                {
                    foreach (DataRowView drb in lv_MasterList.SelectedItems.Cast<DataRowView>()) list_Int64.Add((Int64)drb["ID"]);
                }


                if (true) //candidate for methodisation 20210305, this can be collected up in several further places
                {
                    method_LoadUpMasterList();
                    rowFilter(bool_IsolationIsOn, false);

                    foreach (Int64 theInt in list_Int64)
                    {
                        int indexOf = lv_MasterList.Items.Cast<DataRowView>().ToList().IndexOf(lv_MasterList.Items.Cast<DataRowView>().Where(x => (Int64)x["ID"] == theInt).First());
                        lv_MasterList.SelectedItems.Add(lv_MasterList.Items[indexOf]);
                    }

                    lv_MasterList.ScrollIntoView(lv_MasterList.SelectedItem);
                }

                //lb_Explorer.Items.Refresh();

                RequestNavigate_AndStart();

                //textBoxSearch.Text = "";
                //RemoveFilters_UsedOnUnisolate_And_RemoveFilterButton();
                //textBoxSearch.Focus();
                //textBoxSearch.SelectAll();
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("btn_refresh_Click, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        ListBox dragSource = null;

        private void lb_listBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            int eL = -1;

            if (lv_MasterList.SelectedItem == null)
            {
                MessageBox.Show("Please select an item in Masterlist.");
                DragDropHelper.SetIsDragOver(listbox_objectmodel, false);
                return;
            }


            try
            {
                if (true)
                {
                   //string[] file_pathes = (string[])e.Data.GetData(DataFormats.FileDrop);  /*file_pathes contains pathes of dragged selected files*/

                    string string_DirectoryToCreate_Storage = method_Storage_DBFile_Directory() + @"\Admin Storage";
                    string myString_DataStore = string_DirectoryToCreate_Storage + "\\" + ((DataRowView)lv_MasterList.SelectedItem)["FolderName"];


                    listbox_objectmodel.Drop -= lv_DragDirectory_Drop_New;
                    listbox_objectmodel.DragEnter -= DropBorder_OnDragEnter;
                    listbox_objectmodel.DragLeave -= DropBorder_OnPreviewDragLeave;

                    ListBox parent = (ListBox)sender;
                    string[] files = null;

                    dragSource = parent;
                    object data = GetDataFromListBox(dragSource, e.GetPosition(parent));

                    if (data == null)
                    {
                        listbox_objectmodel.Drop += lv_DragDirectory_Drop_New;
                        listbox_objectmodel.DragEnter += DropBorder_OnDragEnter;
                        listbox_objectmodel.DragLeave += DropBorder_OnPreviewDragLeave;
                        return;
                    }

                    string string_Path = myString_DataStore + "\\" + ((KeyValuePair<string, Bitmap>)data).Key;

                    ////lb_listBox.Drop -= lv_DragDirectory_Drop;
                    files = new string[] { string_Path };
                    ////}
                    if (true)
                    {
                        //object data = GetDataFromListBox(dragSource, e.GetPosition(parent));

                        if (data != null)
                        {
                            foreach (var item in listbox_objectmodel.Items)
                            {
                                if (item.ToString() == string_Path)
                                {
                                    listbox_objectmodel.SelectedItem = item;
                                    listbox_objectmodel.Refresh();
                                }
                            }
                        }
                    }
                    DragDrop.DoDragDrop(this, new DataObject(DataFormats.FileDrop, files), DragDropEffects.Copy);
                    listbox_objectmodel.Drop += lv_DragDirectory_Drop_New;
                    listbox_objectmodel.DragEnter += DropBorder_OnDragEnter;
                    listbox_objectmodel.DragLeave += DropBorder_OnPreviewDragLeave;
                }
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("lb_listBox_MouseDown, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        #region GetDataFromListBox(ListBox,Point)
        private static object GetDataFromListBox(ListBox source, System.Windows.Point point)
        {
            UIElement element = source.InputHitTest(point) as UIElement;
            if (element != null)
            {
                object data = DependencyProperty.UnsetValue;
                while (data == DependencyProperty.UnsetValue)
                {
                    data = source.ItemContainerGenerator.ItemFromContainer(element);

                    if (data == DependencyProperty.UnsetValue)
                    {
                        element = VisualTreeHelper.GetParent(element) as UIElement;
                    }

                    if (element == source)
                    {
                        return null;
                    }
                }

                if (data != DependencyProperty.UnsetValue)
                {

                    return data;

                }
            }

            return null;
        }
        #endregion

        private void RadioButton_Checked(object sender, RoutedEventArgs e) //RadioButton_LF
        {
            int eL = -1;

            try
            {
                ////bool bool_returnNow = false;
                ////if (string_checkedRadioButton == "") bool_returnNow = true;

                string_checkedRadioButton = RadioButton_LF.Content.ToString();

                ////if (bool_returnNow) return;

                if (dataTable == null) return;
                rowFilter(bool_IsolationIsOn, false);

            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("RadioButton_Checked, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        private void RadioButton_Checked_1(object sender, RoutedEventArgs e) //RadioButton_LD
        {
            int eL = -1;

            try
            {

                string_checkedRadioButton = RadioButton_LD.Content.ToString();


                if (dataTable == null) return;
                rowFilter(bool_IsolationIsOn, false);

            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("RadioButton_Checked_1, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        private void RadioButton_Checked_2(object sender, RoutedEventArgs e) //RadioButton_EF
        {
            int eL = -1;

            try
            {


                string_checkedRadioButton = RadioButton_EF.Content.ToString();


                if (dataTable == null) return;
                rowFilter(bool_IsolationIsOn, false);

            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("RadioButton_Checked_2, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        private void RadioButton_Checked_3(object sender, RoutedEventArgs e) //RadioButton_EE
        {
            int eL = -1;

            try
            {


                string_checkedRadioButton = RadioButton_EE.Content.ToString();


                if (dataTable == null) return;
                rowFilter(bool_IsolationIsOn, false);

            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("RadioButton_Checked_3, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        private void RadioButton_Checked_4(object sender, RoutedEventArgs e) //RadioButton_CD
        {
            int eL = -1;

            try
            {
                string_checkedRadioButton = RadioButton_CD.Content.ToString();


                if (dataTable == null) return;
                rowFilter(bool_IsolationIsOn, false);
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("RadioButton_Checked_4, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        private void RadioButton_Checked_5(object sender, RoutedEventArgs e) //RadioButton_DD
        {
            int eL = -1;

            try
            {

                string_checkedRadioButton = RadioButton_DD.Content.ToString();


                if (dataTable == null) return;
                rowFilter(bool_IsolationIsOn, false);

            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("RadioButton_Checked_5, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        private void RadioButton_Checked_6(object sender, RoutedEventArgs e) //RadioButton_SD
        {
            int eL = -1;

            try
            {
                string_checkedRadioButton = RadioButton_SD.Content.ToString();

                if (dataTable == null) return;
                rowFilter(bool_IsolationIsOn, false);
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("RadioButton_Checked_6, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        private void RadioButton_Checked_7(object sender, RoutedEventArgs e) //RadioButton_NCD
        {
            int eL = -1;

            try
            {

                string_checkedRadioButton = RadioButton_CT.Content.ToString();

                if (dataTable == null) return;
                rowFilter(bool_IsolationIsOn, false);
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("RadioButton_Checked_7, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        private void RadioButton_ALL_Checked(object sender, RoutedEventArgs e)
        {
            int eL = -1;

            try
            {

                string_checkedRadioButton = "";

                if (dataTable == null) return;
                rowFilter(bool_IsolationIsOn, false);

            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("RadioButton_ALL_Checked, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        private void TxtMsg_OnPaste(object sender, ExecutedRoutedEventArgs e)
        {
            int eL = -1;

            try
            {
                //  MessageBox.Show("hello world");

                textBoxSearch.Text = Clipboard.GetText();

                if (dataTable == null) return;
                rowFilter(bool_IsolationIsOn, false);



               // MessageBox.Show("hello world");
                //e.Handled = false;
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("TxtMsg_OnPaste, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        private void lv_MasterList_MouseWheel(object sender, MouseWheelEventArgs e)
        {
    
        }

        private void ListBox_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
                     
          //  lv_MasterList.CanContentScroll = true;
            //e.Handled = true;
        }

        private void btn_TestOne_Click(object sender, RoutedEventArgs e)
        {
            int eL = -1;

            ///the lot is done!
            ///.
            ///now we need to make the top list act like the old list
            ///simple remove the old list, but i think we will approach this differently
            ///.
            ///so the first thing we need to progresss is getting the darker things copied across....done!
            ///next are context menu, or the dragging
            ///the bitmap you have represent the files, 
            ///we're going exactly the same thing as we did before except when we delete we delete it also from the xml
            ///and when we drag in we drag in from the xaml.
            ///but simply by copying it over it will break it, we need the template to work with
            ///but the act of breaking it will tell us what needs to work (create an error list)
            ///therefore therefore, how do we partially break the first one
            ///...
            ///this is the best idea we will partially break the first one
            ///now i suddely disagree..what i think we should do is get the delte to work as per what we discussed above, this is the simpliest example, getting this to work will create a new 
            ///new easiest one, and then it's done already
            ///get the delete to work 
            ///but in order to get delete to work we need a file to delete, therefore we need to get the drag copy to work
            ///get the drag copy to worki



            MessageBox.Show("we've finished with this subroutine");
            return;
            try
            {
                if (lv_MasterList.SelectedIndex == -1)
                {
                    MessageBox.Show("Please select an item in the main list");
                    return;
                }

string string_DirectoryToCreate_Storage = method_Storage_DBFile_Directory() + @"\Admin Storage";

                foreach (DataRow dr in dataTable.DefaultView.ToTable().Rows)
                {

                    string str_Path = string_DirectoryToCreate_Storage + "\\" + dr["FolderName"];

                    List<string> folderAndFiles = new List<string>();
                    folderAndFiles.AddRange(Directory.GetFiles(str_Path).Where(x => System.IO.Path.GetFileName(x) != "Parameters_BuiltIn.xml" & System.IO.Path.GetFileName(x) != "Parameters_Project.xml" & System.IO.Path.GetFileName(x) != "Parameters_Shared.xml" & System.IO.Path.GetFileName(x) != "Parameters_Thumbnails.xml").ToArray());

                    Dictionary<string, Bitmap> imageDictionary = new Dictionary<string, Bitmap>();

                    int int_Count = 0;

                    foreach (string fileName in folderAndFiles)
                    {
                        int_Count++;
                        string fileName_Destination = @"C:\temp2\" + System.IO.Path.GetFileName(fileName);

                        System.IO.File.Copy(fileName, fileName_Destination, true);

                        ShellObject shellobject = ShellFile.FromParsingName(fileName_Destination);

                        string str_Last4Characters = fileName_Destination.Substring(Math.Max(0, fileName_Destination.Length - 4)).ToLower();

                        ////if(int_Count < 10)
                        ////{
                        ////    MessageBox.Show(int_Count.ToString());
                        ////}

                        if (str_Last4Characters == ".ies" | str_Last4Characters == ".ldt")
                        {
                            Bitmap bitmap = FileToIconConverter.resizeImage(shellobject.Thumbnail.Bitmap, new System.Drawing.Size(256, 256), 0, true);
                            imageDictionary.Add(System.IO.Path.GetFileName(fileName_Destination), bitmap);
                        } else
                        {
                            Bitmap bitmap = FileToIconConverter.resizeImage(shellobject.Thumbnail.Bitmap, new System.Drawing.Size(256, 256), 0, false);
                            imageDictionary.Add(System.IO.Path.GetFileName(fileName_Destination), bitmap);
                        }
                    }

                    if (true)
                    {
                        Stream stream = new FileStream(str_Path + @"\Parameters_Thumbnails.xml", FileMode.Create, FileAccess.Write);  //of course this is bad because of c:
                        DataContractSerializer serializer = new DataContractSerializer(typeof(Dictionary<string, Bitmap>));
                        serializer.WriteObject(stream, imageDictionary); stream.Close();
                    }
                }
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("btn_TestOne_Click, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        private void btnTemp_RemoveTheWrongImageAllThoseTimes_Click(object sender, RoutedEventArgs e)
        {
            int eL = -1;
            string eL_String = "";

            MessageBox.Show("we've finished with this subroutine");
            return;

            try
            {

                string string_DirectoryToCreate_Storage = method_Storage_DBFile_Directory() + @"\Admin Storage";

                foreach (DataRow dr in dataTable.DefaultView.ToTable().Rows)
                {
                    eL = 5423;
                    string str_Path = string_DirectoryToCreate_Storage + "\\" + dr["FolderName"];
                    eL = 5426;
                    List<string> folderAndFiles = new List<string>();
                    eL = 5428;

                    eL_String = str_Path;

                    try
                    {
                        folderAndFiles.AddRange(Directory.GetFiles(str_Path).Where(x => System.IO.Path.GetFileName(x) != "Parameters_BuiltIn.xml" & System.IO.Path.GetFileName(x) != "Parameters_Project.xml" & System.IO.Path.GetFileName(x) != "Parameters_Shared.xml" & System.IO.Path.GetFileName(x) != "Parameters_Thumbnails.xml").ToArray());
                    }
                    catch
                    {

                        string fileName = Environment.NewLine + dict_GuidToAlias[new Guid(dr["ProjectGUID"].ToString())];  //
                        string scheduleName = Environment.NewLine + dr["ScheduleName"].ToString();  //
                        string SearchTerms = Environment.NewLine + dr["SearchTerms"].ToString();  //

                        _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug(fileName + scheduleName + SearchTerms, true);
                        //dr["FolderName"]


                        continue;
                    }


                    eL = 5430;

                    Dictionary<string, Bitmap> imageDictionary = null;

                    if (true)
                    {
                        eL = 5434;
                        ////Stream stream = new FileStream(str_Path + @"\Parameters_Thumbnails.xml", FileMode.Create, FileAccess.Write);  //of course this is bad because of c:
                        ////DataContractSerializer serializer = new DataContractSerializer(typeof(Dictionary<string, Bitmap>));
                        ////ImageDictionary = serializer.WriteObject(stream, imageDictionary); stream.Close();

                        Stream stream = new FileStream(str_Path + @"\Parameters_Thumbnails.xml", FileMode.Open, FileAccess.Read);
                        DataContractSerializer serializer = new DataContractSerializer(typeof(Dictionary<string, Bitmap>));   //one of four
                        imageDictionary = serializer.ReadObject(stream) as Dictionary<string, Bitmap>; stream.Close();  //two of four
                    }
                    eL = 5443;

                    bool bool_Save = false;

                    /// int int_Count = 0;

                    foreach (string fileName in folderAndFiles)
                    {
                        if((str_Path + @"\Single Phase IP56 Protected.png") == fileName)
                        {
                            bool_Save = true;

                            ///  int_Count++;
                            //////string fileName_Destination = @"C:\temp2\" + System.IO.Path.GetFileName(fileName);

                            System.IO.File.Delete(str_Path + "\\Single Phase IP56 Protected.png");

                            ///ShellObject shellobject = ShellFile.FromParsingName(fileName_Destination);

                            ////////////  string str_Last4Characters = fileName_Destination.Substring(Math.Max(0, fileName_Destination.Length - 4)).ToLower();

                            ///if this equals something else, then do this

                            imageDictionary.Remove("Single Phase IP56 Protected.png");

                            ////////////if (str_Last4Characters == ".ies" | str_Last4Characters == ".ldt")
                            ////////////{
                            ////////////    imageDictionary.Remove("ASD");
                            ////////////}
                        }


                    }
                    eL = 5477;
                    if (bool_Save)
                    {
                        Stream stream = new FileStream(str_Path + @"\Parameters_Thumbnails.xml", FileMode.Create, FileAccess.Write);  //of course this is bad because of c:
                        DataContractSerializer serializer = new DataContractSerializer(typeof(Dictionary<string, Bitmap>));
                        serializer.WriteObject(stream, imageDictionary); stream.Close();
                    }
                }
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("btnTemp_RemoveTheWrongImageAllThoseTimes_Click, error line:" + eL + " " + eL_String + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        private void but_DuplicateAttachments_Click(object sender, RoutedEventArgs e)
        {
            int eL = -1;

            try
            {

                if (lv_MasterList.SelectedItems.Count == 0) return;

                    string string_DirectoryToCreate_Storage = method_Storage_DBFile_Directory() + @"\Admin Storage";
                    string myString_DataStore = string_DirectoryToCreate_Storage + "\\" + ((DataRowView)lv_MasterList.SelectedItem)["FolderName"];

                    //////string path = ((KeyValuePair<string, Bitmap>)listbox_objectmodel.SelectedItem).Key;
                    //////path = myString_DataStore + "\\" + path;

                    //////System.Diagnostics.Process.Start(path);

                DuplicateStage1 ds1 = new DuplicateStage1();

                ds1.combing_models = new CombiningViewModel
                {
                    ////////////////////model = new Model(this),
                    model2 = new Model2(this)
                };
                ds1.DataContext = ds1.combing_models;

                // ds1.DataContext = ds1.combing_models;
                ds1.myLabel_Directory.Content = myLabel_Directory.Content;

                ds1.combing_models.model2.Path = myString_DataStore;

                ds1.Show();
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("but_DuplicateAttachments_Click, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        private void buttonCopyText_Click(object sender, RoutedEventArgs e)
        {
            int eL = -1;

            try
            {
                if (lv_MasterList.SelectedItem == null)
                {
                    MessageBox.Show("Please select an item in Masterlist.");
                    return;
                }

                string str_Cleaned = "";

                foreach (DataRowView drv in lv_MasterList.SelectedItems)
                {
                    str_Cleaned = drv["SearchTerms"].ToString();

                    if (true)
                    {
                        string regexSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
                        Regex r = new Regex(string.Format("[{0}]", Regex.Escape(regexSearch)));
                        str_Cleaned = r.Replace(str_Cleaned, "");
                    }

                    str_Cleaned = str_Cleaned.Replace(",", Environment.NewLine);
                    ////str_Cleaned = str_Cleaned.Replace("  ", " ");


                    //////////str_Cleaned = str_Cleaned.Substring(0, Math.Min(str_Cleaned.Length, 60));
                    //////////str_Cleaned = str_Cleaned.Trim();
                    //////////str_Cleaned = my_Int + "_" + str_Cleaned;
                    ///more 

                    break;
                }

                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug(str_Cleaned, true);
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("buttonCopyText_Click, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        private void but_OpenContainingFolder_Click(object sender, RoutedEventArgs e)
        {
            int eL = -1;
            if (lv_MasterList.SelectedItems.Count == 0) return;
            try
            {
                string string_DirectoryToCreate_Storage = method_Storage_DBFile_Directory() + @"\Admin Storage";
                string myString_DataStore = string_DirectoryToCreate_Storage + "\\" + ((DataRowView)lv_MasterList.SelectedItem)["FolderName"];

                ////string path = ((KeyValuePair<string, Bitmap>)listbox_objectmodel.SelectedItem).Key;
                ////path = myString_DataStore + "\\" + path;

                //////path = Path.GetDirectoryName(path);

                System.Diagnostics.Process.Start(myString_DataStore);

                #region catch and finally
            }
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("but_OpenContainingFolder_Click, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }
    }
}