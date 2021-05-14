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

using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using pkRevitScheduleEdit;

using System.Reflection;
using System.Reflection.Emit;

using Xceed.Wpf.Toolkit.PropertyGrid;
using System.Runtime.Serialization;
using System.IO;

using System.Text.RegularExpressions;

using System.Data;
using _952_PRLoogleClassLibrary;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Collections.ObjectModel;
using System.Globalization;

using Autodesk.Revit.UI;
using Autodesk.Revit.DB;



namespace pkRevitScheduleEdit
{
    //     return ((FamilySymbol)value).get_Parameter(BuiltInParameter.ALL_MODEL_TYPE_COMMENTS).AsString();
    public class StringToBoolConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int eL = -1;

            try
            {
                string str_typeComment = ((Element)value).get_Parameter(BuiltInParameter.ALL_MODEL_TYPE_COMMENTS)?.AsString();
                string str_FamilyInstance = ((Element)value).get_Parameter(BuiltInParameter.ELEM_FAMILY_PARAM)?.AsValueString();
                string str_FamilySymbol = ((Element)value).get_Parameter(BuiltInParameter.ALL_MODEL_FAMILY_NAME)?.AsString();

                // List<Parameter> pset = ((Element)value).Parameters.Cast<Parameter>().ToList();

                return "(" + str_typeComment + ") " + ((Element)value).Name + ", " + str_FamilyInstance + str_FamilySymbol;
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("StringToBoolConverter Convert, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
            return "error";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    /// <summary>d
    /// Interaction logic for Window2.xaml
    /// </summary>
    /// 
    [DataContract(Namespace = "pkRevitScheduleEdit")]
    public partial class ListOfPatternsParent_Specific //APattern  //ISerializables
    {
        public class AFamily_Specific
        {
            public string ParameterName { get; set; }//
            //////public string IssuePerson { get; set; }//
            //////public DateTime IssueDate { get; set; }//
            public int ParameterID { get; set; }//
            //////public string IssuePathWhenPrevious { get; set; }
            //////public string IssueRevitVersionNumber { get; set; }
            //////public string IssueFromProject { get; set; }
            //////public string IssueFromProjectPath { get; set; }
        }
        [DataMember]
        public List<AFamily_Specific> the_AFamily_Specific { get; set; }
    }

    public partial class ElementAndSchedule
    {
        public Element elementType { get; set; }
        public string viewScheduleName { get; set; }
        public string string_OrderAdded { get; set; }
    }



    [DataContract(Namespace = "pkRevitScheduleEdit")]
    public partial class ListOfPatternsParent_ALL_Families_Master_List //APattern  //ISerializables
    {
        public class AFamilyRevision
        {
            public string FamilyFileName { get; set; }
            public string AdditionalInformation { get; set; }
            public string SearchString { get; set; }
            public string LastModifiedPerson { get; set; }
            public DateTime LastModified { get; set; }
            public int index { get; set; }
            public int CurrentRevision { get; set; }
            public int SortOrder { get; set; } //does this come from the count or from the maxd
        }

        [DataMember]
        public List<AFamilyRevision> the_AFamilyRevisions { get; set; }
        [DataMember]
        public string MajorClientName { get; set; }
    }


    /// <summary>
    /// Interaction logic for ScheduleEdit.xaml
    /// </summary>
    public partial class ScheduleEdit : Window
    {
        public DataTable myDataTable { get; set; }
        public ListOfPatternsParent_ALL_Families_Master_List myListOfPatternsParent_ALL_Families_Master_List { get; set; }

        public List<ElementAndSchedule> listElement_AggregatingAllSchedules { get; set; }

        public void MakeXML_Write_Command02Method() //this can never be false if myFamilyInstanceeee is null
        {
            Stream stream = new FileStream(myThisApplication.myString_1400_filename_FirstHalf + myThisApplication.myString_1400_filename, FileMode.Create, FileAccess.Write);
            DataContractSerializer serializer = new DataContractSerializer(typeof(ListOfPatternsParent_ALL_Families_Master_List));
            serializer.WriteObject(stream, myListOfPatternsParent_ALL_Families_Master_List); stream.Close();
        }
        public void PreSourceFrom_Loadand_XMLButton()  //this is for Window02
        {
            int eL = -1;

            try
            {
                eL = 131;
                Stream stream = new FileStream(myThisApplication.myString_1400_filename_FirstHalf + myThisApplication.myString_1400_filename, FileMode.Open, FileAccess.Read);
                eL = 132;
                DataContractSerializer serializer = new DataContractSerializer(typeof(ListOfPatternsParent_ALL_Families_Master_List));
                eL = 133;

                //////DatabaseMethods.writeDebug(serializer.ReadObject(stream).GetType().FullName, true);
                //////eL = 134;
                //////if (serializer.ReadObject(stream).GetType() != typeof(ListOfPatternsParent_ALL_Families_Master_List))
                //////{
                //////    MessageBox.Show("line 133, serializer readobjectType doesn't match .... are we're stopping now");
                //////    return;
                //////}

                myListOfPatternsParent_ALL_Families_Master_List = (ListOfPatternsParent_ALL_Families_Master_List)serializer.ReadObject(stream); stream.Close();

                eL = 136;

                if (myListOfPatternsParent_ALL_Families_Master_List.the_AFamilyRevisions.Count() == 0) //alright by my calculations this should never be the case
                {
                    //MessageBox.Show("Creating the first entry.");
                    //MakeXML_Write_Command02Method(false);
                    myDataTable = new DataTable();
                    myDataTable.Columns.Add("FamilyFileName", typeof(string));    //merge with below//merge with below//merge with below//merge with below//merge with below
                    myDataTable.Columns.Add("AdditionalInformation", typeof(string));//merge with below
                    myDataTable.Columns.Add("SearchString", typeof(string));//merge with below
                    myDataTable.Columns.Add("LastModifiedPerson", typeof(string));//merge with below
                    myDataTable.Columns.Add("LastModified", typeof(DateTime));//merge with below
                    myDataTable.Columns.Add("index", typeof(int));//merge with below
                    myDataTable.Columns.Add("CurrentRevision", typeof(int));//merge with below
                    myDataTable.Columns.Add("SortOrder", typeof(int));//merge with below
                    return;
                }

                //lastListOfPatternsAFamilyRevision = myListOfPatternsParent_ALL_Families_Master_List.the_AFamilyRevisions[5];
                ///////////////// lastListOfPatternsAFamilyRevision = myListOfPatternsParent_ALL_Families_Master_List.the_AFamilyRevisions[myListOfPatternsParent_ALL_Families_Master_List.the_AFamilyRevisions.Count() - 1];

                myDataTable = new DataTable();
                //myDataTable.Clear();
                myDataTable.Columns.Add("FamilyFileName", typeof(string));
                myDataTable.Columns.Add("AdditionalInformation", typeof(string));
                myDataTable.Columns.Add("SearchString", typeof(string));
                myDataTable.Columns.Add("LastModifiedPerson", typeof(string));
                myDataTable.Columns.Add("LastModified", typeof(DateTime));
                myDataTable.Columns.Add("index", typeof(int));
                myDataTable.Columns.Add("CurrentRevision", typeof(int));
                myDataTable.Columns.Add("SortOrder", typeof(int));
                foreach (ListOfPatternsParent_ALL_Families_Master_List.AFamilyRevision AFamilyReviiii in myListOfPatternsParent_ALL_Families_Master_List.the_AFamilyRevisions.AsEnumerable().Reverse())
                {
                    DataRow row = myDataTable.NewRow();
                    row["FamilyFileName"] = AFamilyReviiii.FamilyFileName;
                    row["AdditionalInformation"] = AFamilyReviiii.AdditionalInformation;
                    row["SearchString"] = AFamilyReviiii.SearchString;
                    row["LastModifiedPerson"] = AFamilyReviiii.LastModifiedPerson;
                    row["LastModified"] = AFamilyReviiii.LastModified;
                    row["index"] = AFamilyReviiii.index;
                    row["CurrentRevision"] = AFamilyReviiii.CurrentRevision;
                    row["SortOrder"] = AFamilyReviiii.SortOrder;
                    myDataTable.Rows.Add(row);
                }
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("PreSourceFrom_Loadand_XMLButton, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException + Environment.NewLine + myThisApplication.myString_1400_filename_FirstHalf + myThisApplication.myString_1400_filename, true);
            }
            finally
            {
            }
            #endregion
        }

        public ThisApplication myThisApplication { get; set; }
        public Element myElementType { get; set; }
        public ElementType myElementType2 { get; set; }

        public ObservableCollection<ThreeVariables_Internal> myOC_ThreeVariables_Internal { get; set; } /*= new ThreeVariables_Parent() ;*/

        public object myObject { get; set; }
        public ThisApplication._935_PRLoogle_Command02_EE05_UpdateFamilyAndTypeName my_935_PRLoogle_Command02_EE05_UpdateFamilyAndTypeName { get; set; }


        public _935_PRLoogle_Command05_EE03_ForEachPropertiesGrid my_935_PRLoogle_Command05_EE03_ForEachPropertiesGrid;
        public ExternalEvent myExternalEvent;
        public ExternalEvent my_935_PRLoogle_Command02_EE05_UpdateFamilyAndTypeName_Action { get; set; }

        public _935_PRLoogle_Command05_EE04_AddTypeParameter myEE04_AddTypeParameter { get; set; }
        public ExternalEvent myExternalEvent_EE04_AddTypeParameter { get; set; }
        public ExternalCommandData commandData { get; set; }

        public ScheduleEdit(ExternalCommandData cD, ThisApplication myThisApplicationnnnnnnnnnnnnnn)
        {
            int eL = -1;
            try
            {
                commandData = cD;

            myEE04_AddTypeParameter = new _935_PRLoogle_Command05_EE04_AddTypeParameter();
            myEE04_AddTypeParameter.myWindow2 = this;
            myExternalEvent_EE04_AddTypeParameter = ExternalEvent.Create(myEE04_AddTypeParameter);
            eL = 196;
            my_935_PRLoogle_Command02_EE05_UpdateFamilyAndTypeName = new ThisApplication._935_PRLoogle_Command02_EE05_UpdateFamilyAndTypeName();
            my_935_PRLoogle_Command02_EE05_UpdateFamilyAndTypeName_Action = ExternalEvent.Create(my_935_PRLoogle_Command02_EE05_UpdateFamilyAndTypeName);
            eL = 199;
            myThisApplication = myThisApplicationnnnnnnnnnnnnnn;


                eL = 204;
                if (System.IO.File.Exists(myThisApplication.myString_1400_filename_FirstHalf + myThisApplication.myString_1400_filename))
                {
                    eL = 207;
                    PreSourceFrom_Loadand_XMLButton();
                }
                else
                {
                    eL = 212;
                    MessageBox.Show("Brand new file created..." + Environment.NewLine + Environment.NewLine + "\"" + myThisApplication.myString_1400_filename.Substring(1) + "\"" + Environment.NewLine + Environment.NewLine + "Please check directory is correct..." + Environment.NewLine + Environment.NewLine + myThisApplication.myString_1400_filename_FirstHalf);

                    ListOfPatternsParent_ALL_Families_Master_List myFromBinPhoto = new ListOfPatternsParent_ALL_Families_Master_List();
                    myFromBinPhoto.MajorClientName = "PRD & PRL";
                    myFromBinPhoto.the_AFamilyRevisions = new List<ListOfPatternsParent_ALL_Families_Master_List.AFamilyRevision>() /*{ new_newAPattern }*/;

                    Stream stream = new FileStream(myThisApplication.myString_1400_filename_FirstHalf + myThisApplication.myString_1400_filename, FileMode.Create, FileAccess.Write);
                    DataContractSerializer serializer = new DataContractSerializer(typeof(ListOfPatternsParent_ALL_Families_Master_List));
                    serializer.WriteObject(stream, myFromBinPhoto); stream.Close();
                  

                    PreSourceFrom_Loadand_XMLButton();

                   // return;
                }

                eL = 223;

                my_935_PRLoogle_Command05_EE03_ForEachPropertiesGrid = new _935_PRLoogle_Command05_EE03_ForEachPropertiesGrid();
                myExternalEvent = ExternalEvent.Create(my_935_PRLoogle_Command05_EE03_ForEachPropertiesGrid);

                UIDocument uidoc = myThisApplication.myExternalCommandData.Application.ActiveUIDocument;
                Document doc = uidoc.Document;

                //MessageBox.Show(Properties.Settings.Default.Height.ToString());

                InitializeComponent();

                this.myComboBox.SelectedIndex = Properties.Settings.Default.ComboIndex;
              //  this.rb_GroupedBySchedule.IsChecked = Properties.Settings.Default.GroupedBySchedule;

                this.Top = Properties.Settings.Default.Top;
                this.Left = Properties.Settings.Default.Left;
                this.Height = Properties.Settings.Default.Height;
                this.Width = Properties.Settings.Default.Width;
                

                if (myDataTable == null)
                {
                    MessageBox.Show("line 266, myDataTable is null .... are we're stopping now");
                    return;
                }

                string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name.Split('\\').Last();

                if(userName != "Joshua.Lumley")
                {
                    myButton_AddType.Visibility = System.Windows.Visibility.Hidden;
                    myButton_AddType_Luminaires.Visibility = System.Windows.Visibility.Hidden;
                }
              

                ObservableCollection<Tuple<string, int>> cities = null;
                List<ElementId> cities2 = null;

                cities2 = new FilteredElementCollector(doc).WhereElementIsNotElementType().Where(x => x.GetTypeId() != null).Select(x => x.GetTypeId()).Distinct().ToList();
                cities = new ObservableCollection<Tuple<string, int>> (cities2.Select(x => doc.GetElement(x)).Where(x => x != null).Where(x => x.Category != null).GroupBy(x => x.Category.Name).Select(x => x.First()).Where(x => x.LookupParameter("Type Comments") != null).Select(x => new Tuple<string, int>(x.Category.Name, x.Category.Id.IntegerValue)).OrderBy(x => x.ToString()));
                eL = 295;
                // foreach (string sss in cities) myComboBox.Items.Add(sss);
                // myComboBox.SelectedValuePath = "Item1";

                cities.Insert(0,new Tuple<string, int>("ALL", -1));

                myComboBox.ItemsSource = cities;

                if(true) //candidate for methodisation 20210423
                {
                    eL = 243;
                   /// myListViewMaster.ItemsSource = myDataTable.DefaultView;  //this one is not important leave it there
                    eL = 264;
                    Element myElement = myPrivate_2020(false);
                    eL = 361;
                    if (myElement != null)
                    {
                        int myInt = 0;
                        foreach (Tuple<string, int> myCBIiiii in myComboBox.Items)
                        {
                            if (myCBIiiii.Item2 == myElement.Category.Id.IntegerValue)
                            {
                                myComboBox.SelectedIndex = myInt;
                                break;
                            }
                            myInt++;
                        }

                        eL = 319;
                        if (myComboBox.SelectedIndex != -1) rePopulate_ListView_Right(myElement.Id.IntegerValue);
                    }


                    ////foreach (Tuple<string, int> myCBI in myComboBox.Items)
                    ////{
                    ////    if (myCBI.Item2 == myElement.Category.Id.IntegerValue)
                    ////    {
                    ////        myComboBox.SelectedItem = myCBI;
                    ////        break;
                    ////    }
                    ////}



                    eL = 321;

                    if(cities.Count() != 0)  myMethod_PopulateEverything(myElement);
                    bool_ContinueWith_RadioButtonCode = true;
                }
            }
            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("ScheduleEdit(ExternalCommandData cD, ThisApplication myThisApplicationnnnnnnnnnnnnnn), error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException + Environment.NewLine + myThisApplication.myString_1400_filename_FirstHalf + myThisApplication.myString_1400_filename, true);
            }
            finally
            {
            }
            #endregion
        }

        bool bool_ContinueWith_RadioButtonCode = false;

        private void rb_GroupedBySchedule_Checked(object sender, RoutedEventArgs e)
        {
            if (myComboBox.Items.Count == 0) return;

            try
            {
                if(bool_ContinueWith_RadioButtonCode) myMethod_PopulateEverything(null);
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("rb_GroupedBySchedule_Checked" + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion

        }

        private void rb_Loose_Checked(object sender, RoutedEventArgs e)
        {
            if (myComboBox.Items.Count == 0) return;

            try
            {
                if (bool_ContinueWith_RadioButtonCode) myMethod_PopulateEverything(null);
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("rb_Loose_Checked" + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        private void ComboBox_DropDownClosed(object sender, EventArgs e)
        {
            try
            {
                //this speaks for itself when it retursn when nothing is selected, but before i get the non lighting ones working we need to make sure this adds and removes normally

                if (myComboBox.Items.Count == 0) return;

                if (myComboBox.SelectedIndex > 0)
                {
                    rePopulate_ListView_Right(((Tuple<string, int>)myComboBox.SelectedItem).Item2); 
                } else
                {
                    myListView_Right.ItemsSource = null;
                }
             
                //rePopulate_ListView_Right();
                myMethod_PopulateEverything(null);
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("ComboBox_DropDownClosed" + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        private void rePopulate_ListView_Right(int int_Category)
        {
            
            string myEE02_String_FamilyFileName = int_Category.ToString();
            //string myEE02_String_FamilyFileName = ((ComboBoxItem)myComboBox.SelectedValue).Content.ToString();
            string myString_FamilyFileDirectory = myThisApplication.myString_1400_filename_FirstHalf + "\\ALL Parameter Profiles (do not edit directly)\\";
            if (!Directory.Exists(myString_FamilyFileDirectory)) Directory.CreateDirectory(myString_FamilyFileDirectory);
            string myString_FamilyFileXAML = myString_FamilyFileDirectory + myEE02_String_FamilyFileName + ".xml";

            if (true) //candidate for methodisation 202005162058
            {
                if (File.Exists(myString_FamilyFileXAML))
                {
                    Stream stream2 = new FileStream(myString_FamilyFileXAML, FileMode.Open, FileAccess.Read);
                    DataContractSerializer serializer2 = new DataContractSerializer(typeof(ObservableCollection<ThreeVariables_Internal>));
                    myOC_ThreeVariables_Internal = (ObservableCollection<ThreeVariables_Internal>)serializer2.ReadObject(stream2);
                    stream2.Close();
                }
                else
                {
                    myOC_ThreeVariables_Internal = new ObservableCollection<ThreeVariables_Internal>();
                }

                myListView_Right.ItemsSource = myOC_ThreeVariables_Internal;
            }
        }

        private void MyButtonNewList_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (true)
                {
                    string myEE02_String_FamilyFileName = ((Tuple<string, int>)myComboBox.SelectedItem).Item1.ToString();
                    string myString_FamilyFileDirectory = myThisApplication.myString_1400_filename_FirstHalf + "\\ALL Parameter Profiles (do not edit directly)\\";
                    if (!Directory.Exists(myString_FamilyFileDirectory)) Directory.CreateDirectory(myString_FamilyFileDirectory);
                    string myString_FamilyFileXAML = myString_FamilyFileDirectory + myEE02_String_FamilyFileName + ".xml";

                    Stream stream = new FileStream(myString_FamilyFileXAML, FileMode.Create, FileAccess.Write);
                    DataContractSerializer serializer = new DataContractSerializer(typeof(ObservableCollection<ThreeVariables_Internal>));
                    serializer.WriteObject(stream, myOC_ThreeVariables_Internal.Select(x => new ThreeVariables_Internal() { myDefinitionName = x.myDefinitionName, myStorageType = x.myStorageType, myParameterType = x.myParameterType, myID = x.myID }).Cast<ThreeVariables_Internal>().ToList()); stream.Close();

                    System.Diagnostics.Process.Start(myString_FamilyFileDirectory);
                }

                return;

            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("MyButtonNewList_Click" + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }


        public void myMethod_PopulateEverything(Element myElement)
        {
            int eL = -1;
            eL = 475;
            try
            {
                UIDocument uidoc = myThisApplication.myExternalCommandData.Application.ActiveUIDocument;
                Document doc = uidoc.Document;

                /////////////////////////////////////////////////////BuiltInCategory myBuiltInCategory;
                eL = 482;
                if (myElement != null)
                {
                    //MessageBox.Show("not at this point");
                    myComboBox.SelectedIndex = -1;
                    //MessageBox.Show(myElementType.Category.Name);
                    foreach (Tuple<string, int> myCBI in myComboBox.Items)
                    {
                        if (myCBI.Item2 == myElement.Category.Id.IntegerValue)
                        {
                            myComboBox.SelectedItem = myCBI;
                            break;
                        }
                    }
                    //((Tuple<string, int>)myComboBox.SelectedItem).Item1

                    if (myComboBox.SelectedIndex == -1) return;
                    eL = 496;

                    //MessageBox.Show("has it updated at this poing");

                    rePopulate_ListView_Right(myElement.Category.Id.IntegerValue);
                    //   MessageBox.Show("asd");

                    myElementType = doc.GetElement(myElement.GetTypeId()) as ElementType;

                    if (myElementType == null) myElementType = myElement;

                    myListView_Left.ItemsSource = myElementType.Parameters.Cast<Parameter>().OrderBy(x => x.Definition.Name).Where(x => x.Definition.ParameterGroup != BuiltInParameterGroup.PG_IFC & x.Definition.ParameterType != ParameterType.Invalid & !x.Definition.Name.Contains("(Attribute)"));

                    MakePropertiesGridHappen(myOC_ThreeVariables_Internal, myPropertiesGrid, myElementType);

                    myTextBox_TypeNameOld_Category.Text = myElementType.Category.Name;
                    myTextBox_TypeNameOld.Text = myElementType.Name;
                    myTextBox_TypeNameNew.Text = myElementType.Name;

                    //////////myTextBox_FamilyNameOld.Text = myElementType.FamilyName;
                    //////////myTextBox_FamilyNameNew.Text = myElementType.FamilyName;

                    /////////////////////////////////////////////////////myBuiltInCategory = (BuiltInCategory)myElement.Category.Id.IntegerValue;

                }
                else
                {
                    uidoc.Selection.SetElementIds(new List<ElementId>());

                    myElementType = null;

                    myListView_Left.ItemsSource = null;
                    eL = 522;
                    MakePropertiesGridHappen(myOC_ThreeVariables_Internal, myPropertiesGrid, myElementType);
                    eL = 524;
                    myTextBox_TypeNameOld_Category.Text = "";
                    myTextBox_TypeNameOld.Text = "";
                    myTextBox_TypeNameNew.Text = "";

                    myTextBox_FamilyNameOld.Text = "";
                    myTextBox_FamilyNameNew.Text = "";

                    //if ((ComboBoxItem)myComboBox.SelectedItem == null) myComboBox.SelectedIndex = 0;
                    if (myComboBox.SelectedIndex == -1) myComboBox.SelectedIndex = 0;
                    eL = 534;
                   /////////////////////////////////////// myBuiltInCategory = (BuiltInCategory)doc.Settings.Categories.get_Item(myComboBox.SelectedValue.ToString()).Id.IntegerValue;
                    eL = 535;
                }

                eL = 485;
                ////string myString_AppearsInSchedule = "";
                ////switch (myBuiltInCategory)
                ////{
                ////    case BuiltInCategory.OST_LightingFixtures:
                ////        myString_AppearsInSchedule = "PRL_AppearsInSchedule";
                ////        break;
                ////    case BuiltInCategory.OST_LightingDevices:
                ////        myString_AppearsInSchedule = "PRL_AppearsInSchedule";
                ////        break;
                ////    case BuiltInCategory.OST_ElectricalFixtures:
                ////        myString_AppearsInSchedule = "PRL_AppearsInSchedule";
                ////        break;
                ////    case BuiltInCategory.OST_CommunicationDevices:
                ////        myString_AppearsInSchedule = "PRL_AppearsInSchedule";
                ////        break;
                ////    case BuiltInCategory.OST_ElectricalEquipment:
                ////        myString_AppearsInSchedule = "PRL_AppearsInSchedule";
                ////        break;
                ////    case BuiltInCategory.OST_SecurityDevices:
                ////        myString_AppearsInSchedule = "PRL_AppearsInSchedule";
                ////        break;
                ////    case BuiltInCategory.OST_DataDevices:
                ////        myString_AppearsInSchedule = "PRL_AppearsInSchedule";
                ////        break;
                ////    case BuiltInCategory.OST_NurseCallDevices:
                ////        myString_AppearsInSchedule = "PRL_AppearsInSchedule";
                ////        break;
                ////    default:
                ////        break;
                ////}

                ///if the user selects in grouped in schedule or not is fine
                ///

                List<ElementId> listElementID_ThatAppearInModel = new FilteredElementCollector(doc).WhereElementIsNotElementType().Where(x => x.GetTypeId() != null).Select(x => x.GetTypeId()).Distinct().ToList();

                List<Element> listElement_OfAParticular_Family = null;

                if (((Tuple<string, int>)myComboBox.SelectedItem).Item2 == -1)
                {
                    listElement_OfAParticular_Family = listElementID_ThatAppearInModel.Select(x => doc.GetElement(x)).Where(x => x != null).Where(x => x.Category != null).Where(x => x.LookupParameter("Type Comments") != null).ToList();
                } else
                {
                    listElement_OfAParticular_Family = listElementID_ThatAppearInModel.Select(x => doc.GetElement(x)).Where(x => x != null).Where(x => x.Category != null).Where(x => x.Category.Id.IntegerValue == ((Tuple<string, int>)myComboBox.SelectedItem).Item2).Where(x => x.LookupParameter("Type Comments") != null).ToList();
                }

                eL = 576;
                // MessageBox.Show(myComboBox.SelectedValue.ToString() + listElementID_ThatAppearInModel.Count().ToString());

                 listElement_AggregatingAllSchedules = new List<ElementAndSchedule>();




                List<ViewSchedule> listOfSchedules2 = null;

                if (((Tuple<string, int>)myComboBox.SelectedItem).Item2 == -1)
                {
                    listOfSchedules2 = listOfSchedules(doc, ((Tuple<string, int>)myComboBox.SelectedItem).Item2, true);
                }
                else
                {
                    listOfSchedules2 = listOfSchedules(doc, ((Tuple<string, int>)myComboBox.SelectedItem).Item2, false);
                }

                if (this.rb_GroupedBySchedule.IsChecked.Value)
                {
                    if (/*myString_AppearsInSchedule != "" &*/ listOfSchedules2.Count != 0)
                    {
                        // List<Element> list_Element = new FilteredElementCollector(doc, viewschedule.Id).Cast<Element>().Select(x => doc.GetElement(x.GetTypeId())).ToList().GroupBy(x => x.Id.IntegerValue).Select(g => g.First()).ToList();

                        foreach (ViewSchedule vs in listOfSchedules2)
                        {
                            //  IEnumerable<Element> fec3 = new FilteredElementCollector(doc, vs.Id).GroupBy(x => x.GetTypeId().IntegerValue).Select(x => x.First()).Select(x => (FamilySymbol)doc.GetElement(x.GetTypeId()));
                            eL = 605;

                            List<Element> ordered_element_List = new FilteredElementCollector(doc, vs.Id).Cast<Element>().ToList();


                            // List<Element> ordered_element_List = new FilteredElementCollector(doc, vs.Id).Cast<Element>().Select(x => doc.GetElement(x.GetTypeId())).ToList();
                            eL = 608;
                            if (vs.Definition.GetSortGroupFieldCount() != 0)
                            {
                                eL = 611;
                                ScheduleSortGroupField ssgf = vs.Definition.GetSortGroupField(0);

                                ScheduleField sf = vs.Definition.GetField(ssgf.FieldId);
                                eL = 614;
                                //MessageBox.Show(vs.ViewName);
                                eL = 615;
                                if(!vs.Definition.IsItemized)
                                {
                                    ordered_element_List = ordered_element_List.Select(x => doc.GetElement(x.GetTypeId())).ToList();//.Select(x => x.First()).ToList();
                                    ordered_element_List = ordered_element_List.OrderBy(x => x.get_Parameter((BuiltInParameter)sf.ParameterId.IntegerValue)?.AsInteger()).ToList(); //this was formally lookupparameter (go figure
                                    ordered_element_List = ordered_element_List.GroupBy(x => x.Id.IntegerValue).Select(g => g.First()).ToList();
                                } else
                                {
                                    ordered_element_List = ordered_element_List.OrderBy(x => x.get_Parameter((BuiltInParameter)sf.ParameterId.IntegerValue)?.AsInteger()).ToList();
                                    ordered_element_List = ordered_element_List.GroupBy(x => x.Id.IntegerValue).Select(g => g.First()).ToList();
                                }
                            }
                            else
                            {
                                eL = 622;
                                ordered_element_List = ordered_element_List.Select(x => doc.GetElement(x.GetTypeId())).ToList();//.Select(x => x.First()).ToList();
                                ordered_element_List = ordered_element_List.GroupBy(x => x.Id.IntegerValue).Select(g => g.First()).ToList();
                            }
                            eL = 624;
                            foreach (Element ele in ordered_element_List)
                            {
                                if (!vs.Definition.IsItemized)
                                {
                                    ElementType elementType = ele as ElementType;
                                    listElement_AggregatingAllSchedules.Add(new ElementAndSchedule() { viewScheduleName = vs.Name, elementType = elementType });
                                }
                                else
                                {
                                    //ElementType elementType = ele as ElementType;
                                    listElement_AggregatingAllSchedules.Add(new ElementAndSchedule() { viewScheduleName = vs.Name, elementType = ele });
                                }
                            }
                        }
                    }
                }

                eL = 635;




                if (myElement != null)
                {
                    IEnumerable<ElementAndSchedule> iEum = listElement_AggregatingAllSchedules.Where(x => x.elementType.Id.IntegerValue == myElement.GetTypeId().IntegerValue);
                    if (iEum.Count() == 0)
                    {
                        this.rb_Loose.IsChecked = true;
                        //myListViewTypes.SelectedItem = iEum.First();
                    }

                }

                eL = 581;
                if (!this.rb_GroupedBySchedule.IsChecked.Value)
                {
                    listElement_OfAParticular_Family = listElement_OfAParticular_Family.GroupBy(x => x.Id.IntegerValue).Select(g => g.First()).ToList();

                    foreach (Element ele in listElement_OfAParticular_Family)
                    {
                        ElementType elementType = ele as ElementType;

                        listElement_AggregatingAllSchedules.Add(new ElementAndSchedule() { viewScheduleName = " unscheduled", elementType = elementType });
                    }
                }

                eL = 594;

                




                myListViewTypes.ItemsSource = listElement_AggregatingAllSchedules;

                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(myListViewTypes.ItemsSource);
                PropertyGroupDescription groupDescription = new PropertyGroupDescription("viewScheduleName");
                view.GroupDescriptions.Add(groupDescription);
                eL = 660;

                //myElement


                // MessageBox.Show(myListViewTypes.Items.Count.ToString());

                if (myElement != null)
                {
                    IEnumerable<ElementAndSchedule> iEum = ((List<ElementAndSchedule>)myListViewTypes.ItemsSource).Where(x => x.elementType.Id.IntegerValue == myElement.GetTypeId().IntegerValue);
                    if(iEum.Count() > 0)
                    {
                        myListViewTypes.SelectedItem = iEum.First();
                    } else
                    {
                       // MessageBox.Show("No schedule listing...try switching to 'Loose'");
                    }
                    
                }
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("myMethod_PopulateEverything, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        private List<ViewSchedule> listOfSchedules(Document doc, int int_category, bool bool_AddAllCategories)
        {
           
            List<ViewSchedule> listSched = new List<ViewSchedule>();

            FilteredElementCollector collector = new FilteredElementCollector(doc).OfClass(typeof(ViewSchedule));

            ////Category mcat = doc.geti;
            //////MessageBox.Show("hello");
            //////foreach (Category c in doc.Settings.Categories)
            //////{
            //////    if (c.Name == string_category)
            //////    {
            //////        mcat = c;
            //////    }
            //////}
            //////MessageBox.Show("hello" + mcat.ToString());
            foreach (ViewSchedule s in collector)
            {
                ////ScheduleDefinition definition = s.Definition;
                ////ElementId catSched = s.Definition.CategoryId;

                // MySchedule mySchedule = new MySchedule();

                if(bool_AddAllCategories)
                {
                    listSched.Add(s);
                } else
                {
                    if (s.Definition.CategoryId.IntegerValue == int_category)
                    {
                        listSched.Add(s);
                    }
                }


                // mySchedule.Name = s.ViewName;
            }

            return listSched;
        }

        private void ListViewItem_PreviewMouseDoubleClick_DisplayEntityParameters(object sender, MouseButtonEventArgs e)  //display entity parameters
        {
            int eL = -1;
            try
            {
                //MessageBox.Show("when opened with nothing selected we need to make sure the left list populates on double click");
                eL = 715;
                ////ElementType myFamilySymbol = ((ElementAndSchedule)myListViewTypes.SelectedItem).elementType;
                ////eL = 717;
                ////myElementType = myFamilySymbol;
                eL = 719;
                ////UIDocument uidoc = myThisApplication.myExternalCommandData.Application.ActiveUIDocument;
                ////Document doc = uidoc.Document;

                myElementType = ((ElementAndSchedule)myListViewTypes.SelectedItem).elementType;

                if (myComboBox.SelectedIndex == 0)
                {
                    rePopulate_ListView_Right(((ElementAndSchedule)myListViewTypes.SelectedItem).elementType.Category.Id.IntegerValue);
                }
              

                if (myOC_ThreeVariables_Internal == null)
                {
                    rePopulate_ListView_Right(myElementType.Category.Id.IntegerValue);
                }

                myListView_Left.ItemsSource = myElementType.Parameters.Cast<Parameter>().OrderBy(x => x.Definition.Name).Where(x => x.Definition.ParameterGroup != BuiltInParameterGroup.PG_IFC & x.Definition.ParameterType != ParameterType.Invalid & !x.Definition.Name.Contains("(Attribute)"));
                eL = 724;
                MakePropertiesGridHappen(myOC_ThreeVariables_Internal, myPropertiesGrid, myElementType);
                eL = 726;
                myTextBox_TypeNameOld_Category.Text = myElementType.Category.Name;
                myTextBox_TypeNameOld.Text = myElementType.Name;
                myTextBox_TypeNameNew.Text = myElementType.Name;
                eL = 729;
                ////////////myTextBox_FamilyNameOld.Text = myElementType.FamilyName;
                ////////////myTextBox_FamilyNameNew.Text = myElementType.FamilyName;
                eL = 732;
                //myListView_Right.Items.Refresh();

                e.Handled = true;
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("ListViewItem_PreviewMouseDoubleClick4, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        public void MakePropertiesGridHappen(ObservableCollection<ThreeVariables_Internal> myListThreeVariables, PropertyGrid myPropGriddddddd, Element myElementtttttt)
        {
            if (myListThreeVariables == null)
            {
                return;
            }

            if (myElementtttttt == null)
            {
                myPropGriddddddd.SelectedObject = null;
                return;
            }

            UIDocument uidoc = myThisApplication.myExternalCommandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            Type myType = CompileResultType(myListThreeVariables, myElementtttttt);


            myObject = Activator.CreateInstance(myType);

            foreach (ThreeVariables_Internal myThreeVariables in myListThreeVariables)
            {
                // myObject.GetType().GetProperty(myThreeVariables.myDefinitionName).SetValue(myObject, true);
                //MessageBox.Show(myThreeVariables.myID.ToString());

                //Parameter myParameterDirect = doc.GetElement(new ElementId(myThreeVariables.myID)) as Parameter;

                //if (myElementtttttt.GetOrderedParameters().Where(x => x.Id.IntegerValue == myThreeVariables.myID).Count() == 0) continue;
                if (myElementtttttt.Parameters.Cast<Parameter>().Where(x => x.Id.IntegerValue == myThreeVariables.myID).Count() == 0) continue;

                //Parameter myParameterDirect = myElementtttttt.GetOrderedParameters().Where(x => x.Id.IntegerValue == myThreeVariables.myID).First();
                Parameter myParameterDirect = myElementtttttt.Parameters.Cast<Parameter>().Where(x => x.Definition.Name == myThreeVariables.myDefinitionName).First();

                switch (myThreeVariables.myStorageType)
                {
                    case StorageType.Double:
                        myObject.GetType().GetProperty(Regex.Replace(myThreeVariables.myDefinitionName, "[^0-9a-zA-Z]+", "")).SetValue(myObject, myParameterDirect.AsDouble());
                        break;
                    case StorageType.ElementId:
                        //if(myParameterDirect.AsElementId().IntegerValue != -1)
                        //{
                        if(myParameterDirect !=  null)
                        {
                            myObject.GetType().GetProperty(Regex.Replace(myThreeVariables.myDefinitionName, "[^0-9a-zA-Z]+", "")).SetValue(myObject, myParameterDirect.AsValueString());
                        }
                        ////}

                        //MyTypeBuilder.CreateProperty(tb, myLViiii.Definition.Name, typeof(string));
                        break;
                    case StorageType.Integer:
                        myObject.GetType().GetProperty(Regex.Replace(myThreeVariables.myDefinitionName, "[^0-9a-zA-Z]+", "")).SetValue(myObject, myParameterDirect.AsInteger());
                        break;
                    case StorageType.None:
                        //MyTypeBuilder.CreateProperty(tb, myLViiii.Definition.Name, typeof(string));
                        break;
                    case StorageType.String:
                        myObject.GetType().GetProperty(Regex.Replace(myThreeVariables.myDefinitionName, "[^0-9a-zA-Z]+", "")).SetValue(myObject, myParameterDirect.AsString());
                        break;
                }
            }

            // myPG.SelectedObject = myElement.Parameters.Cast<Parameter>().OrderBy(x => x.Definition.Name).Where(x => x.Definition.ParameterGroup != BuiltInParameterGroup.PG_IFC & x.Definition.ParameterType != ParameterType.Invalid & !x.Definition.Name.Contains("(Attribute)")); ;
            myPropGriddddddd.SelectedObject = myObject;

            EditorDefinition ed = new EditorDefinition();  //if this stops working in the future then use an earlier version of xceed extended toolkit

            PropertyDefinition pd = new PropertyDefinition();
            pd.TargetProperties = myListThreeVariables.Where(x => x.myStorageType == StorageType.String).Select(x => Regex.Replace(x.myDefinitionName, "[^0-9a-zA-Z]+", "")).ToList();
            ed.PropertiesDefinitions.Add(pd);

            FrameworkElementFactory fac = new FrameworkElementFactory(typeof(System.Windows.Controls.TextBox));
            fac.SetBinding(System.Windows.Controls.TextBox.TextProperty, new System.Windows.Data.Binding("Value"));
            fac.SetValue(System.Windows.Controls.TextBox.TextWrappingProperty, TextWrapping.Wrap);
            fac.SetValue(System.Windows.Controls.TextBox.AcceptsReturnProperty, true);
            fac.SetValue(System.Windows.Controls.TextBox.BorderThicknessProperty, new Thickness(1));
            DataTemplate dt = new DataTemplate { VisualTree = fac };

            dt.Seal();
            ed.EditorTemplate = dt;

            myPropGriddddddd.EditorDefinitions.Add(ed);

            //myListThreeVariables this will need to be an any all thing
            ///////////////////////////////////////////////////////// myPG.EditorDefinitions.Add(EditorDefinition01(myObject.GetType().GetProperties().Select(xxx => xxx.Name).ToList(), this));

            //somewhere we have put work into making give us the appropriate control for the storage type, up there it is called CompileResultType
        }

        public Type CompileResultType(ObservableCollection<ThreeVariables_Internal> myListThreeeee, Element myElementtttttt)
        {
            TypeBuilder tb = MyTypeBuilder.GetTypeBuilder();
            ConstructorBuilder constructor = tb.DefineDefaultConstructor(MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName);

          ////  IList<string> iList_ = myElementtttttt.Parameters.Cast<Parameter>().Select(x => x.Definition.Name).ToList();

            IList<Parameter> iList_Parameter = myElementtttttt.Parameters.Cast<Parameter>().ToList();
            // NOTE: assuming your list contains Field objects with fields FieldName(string) and FieldType(Type)
            // foreach (var field in yourListOfFields)
            foreach (ThreeVariables_Internal myLViiii in myListThreeeee)
            {
                if (iList_Parameter.Where(x => x.Definition.Name == myLViiii.myDefinitionName).Count() == 0) continue;
                //if (myElementtttttt.GetOrderedParameters().Where(x => x.Id.IntegerValue == myLViiii.myID).Count() == 0) continue;

                //int caseSwitch = 1;
                switch (myLViiii.myStorageType)
                {
                    case StorageType.Double:
                        MyTypeBuilder.CreateProperty(tb, Regex.Replace(myLViiii.myDefinitionName, "[^0-9a-zA-Z]+", ""), typeof(double));
                        break;
                    case StorageType.ElementId:
                        MyTypeBuilder.CreateProperty(tb, Regex.Replace(myLViiii.myDefinitionName, "[^0-9a-zA-Z]+", ""), typeof(string));
                        //MyTypeBuilder.CreateProperty(tb, myLViiii.Definition.Name, typeof(string));
                        break;
                    case StorageType.Integer:
                        if (myLViiii.myParameterType == ParameterType.YesNo)
                        {
                            MyTypeBuilder.CreateProperty(tb, Regex.Replace(myLViiii.myDefinitionName, "[^0-9a-zA-Z]+", ""), typeof(int));
                        }
                        else
                        {
                            MyTypeBuilder.CreateProperty(tb, Regex.Replace(myLViiii.myDefinitionName, "[^0-9a-zA-Z]+", ""), typeof(int));
                        }
                        break;
                    case StorageType.None:
                        //MyTypeBuilder.CreateProperty(tb, myLViiii.Definition.Name, typeof(string));
                        break;
                    case StorageType.String:
                        MyTypeBuilder.CreateProperty(tb, Regex.Replace(myLViiii.myDefinitionName, "[^0-9a-zA-Z]+", ""), typeof(string));
                        break;
                }
            }

            // MyTypeBuilder.CreateProperty(tb, "Field2", typeof(string));

            Type objectType = tb.CreateType();
            return objectType;
        }

        public partial class ThreeVariables_Internal //: INotifyPropertyChanged

        {
            //public event PropertyChangedEventHandler PropertyChanged;
            //private void NotifyPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] String propertyName = "")
            //{
            //   // MessageBox.Show("hello world");
            //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            //}


            //  private string mydefinitionname;
            public string myDefinitionName
            {
                get; //{/* return mydefinitionname;*/ }
                set;
                //{
                //    //mydefinitionname = value;

                //    //NotifyPropertyChanged();
                //}
            }


            // private StorageType mystoragetype;
            public StorageType myStorageType
            {
                get; //{ /*return mystoragetype; */}
                set;
                //{
                //    //mystoragetype = value;

                //    //NotifyPropertyChanged();
                //}
            }

            // private ParameterType myparametertype;
            public ParameterType myParameterType
            {
                get;//{ /*return myparametertype;*/ }
                set;
                //{
                //    //myparametertype = value;

                //    //NotifyPropertyChanged();
                //}
            }

            // private int myid;
            public int myID
            {
                get;//{ /*return myid;*/ }
                set;
                //{
                //    //myid = value;

                //    //NotifyPropertyChanged();
                //}
            }

        }

        private void SaveGeneral()
        {
            if (myElementType == null) return;

            // myPrivateVoid_myBool_CommitEvent();
            if (myTextBox_TypeNameOld.Text != myTextBox_TypeNameNew.Text | myTextBox_FamilyNameOld.Text != myTextBox_FamilyNameNew.Text)
            {
                my_935_PRLoogle_Command02_EE05_UpdateFamilyAndTypeName.myWind2Revcontrol = this;
                my_935_PRLoogle_Command02_EE05_UpdateFamilyAndTypeName.myFamilySymbol = (FamilySymbol)myElementType;
                my_935_PRLoogle_Command02_EE05_UpdateFamilyAndTypeName.myStringNewFamilyName = myTextBox_FamilyNameNew.Text;
                my_935_PRLoogle_Command02_EE05_UpdateFamilyAndTypeName.myStringNewTypeName = myTextBox_TypeNameNew.Text;
                my_935_PRLoogle_Command02_EE05_UpdateFamilyAndTypeName_Action.Raise();
                //MessageBox.Show("no false positives please");
            }

            my_935_PRLoogle_Command05_EE03_ForEachPropertiesGrid.myWindow2 = this;
            myExternalEvent.Raise();
        }

        private void MyButtonSaveClose_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (myComboBox.Items.Count != 0) SaveGeneral();
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

            Close();

            e.Handled = true;
        }
        private void MyButtonSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (myComboBox.Items.Count == 0) return;
                SaveGeneral();
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

            e.Handled = true;
        }

#pragma warning disable CS0618 // Type or member is obsolete
        public static EditorDefinition EditorDefinition01(List<string> ListAllStringsFields, ScheduleEdit myWindow1)
        {
            EditorDefinition ed = new EditorDefinition();  //if this stops working in the future then use an earlier version of xceed extended toolkit

            PropertyDefinition pd = new PropertyDefinition();
            pd.TargetProperties = ListAllStringsFields.Except(new List<String>() { "Sheet_to_Appear_On" }).ToList();  // new List<string>() { "Description","Make" };
            ed.PropertiesDefinitions.Add(pd);

            FrameworkElementFactory fac = new FrameworkElementFactory(typeof(System.Windows.Controls.TextBox));
            fac.SetBinding(System.Windows.Controls.TextBox.TextProperty, new System.Windows.Data.Binding("Value"));
            fac.SetValue(System.Windows.Controls.TextBox.TextWrappingProperty, TextWrapping.Wrap);
            fac.SetValue(System.Windows.Controls.TextBox.AcceptsReturnProperty, true);
            fac.SetValue(System.Windows.Controls.TextBox.BorderThicknessProperty, new Thickness(1));
            DataTemplate dt = new DataTemplate { VisualTree = fac };

            dt.Seal();
            ed.EditorTemplate = dt;
            return ed;
        }
#pragma warning restore CS0618 // Type or member is obsolete

        ////private void ListViewItem_PreviewMouseDoubleClick3(object sender, MouseButtonEventArgs e)
        ////{
        ////    MessageBox.Show("hello world");
        ////}

        private void ListViewItem_PreviewMouseDoubleClick_RemovingParameter(object sender, MouseButtonEventArgs e)  //removing removing
        {
            try
            {
                if (System.Windows.Forms.MessageBox.Show("Remove this from list..." + Environment.NewLine + Environment.NewLine + "Are you sure?", "Overwrite check", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }

                // int myInt_RemoveAt = myListView_Right.SelectedIndex;
                myOC_ThreeVariables_Internal.RemoveAt(myListView_Right.SelectedIndex);
                //myListView_Right.Items.Refresh();
                //UpdateLayout();
                ////myOC_ThreeVariables_Internal.RemoveAt(myListView_Right.SelectedIndex);
                // myOC_ThreeVariables_Internal.Remove((ThreeVariables_Internal)myListView_Right.SelectedItem);

                if (true)  //candidate for methodisation 22005161126
                {
                    string myEE02_String_FamilyFileName = ((Tuple<string, int>)myComboBox.SelectedItem).Item1.ToString();
                    string myString_FamilyFileDirectory = myThisApplication.myString_1400_filename_FirstHalf + "\\ALL Parameter Profiles (do not edit directly)\\";
                    if (!Directory.Exists(myString_FamilyFileDirectory)) Directory.CreateDirectory(myString_FamilyFileDirectory);
                    string myString_FamilyFileXAML = myString_FamilyFileDirectory + myEE02_String_FamilyFileName + ".xml";

                    Stream stream = new FileStream(myString_FamilyFileXAML, FileMode.Create, FileAccess.Write);
                    DataContractSerializer serializer = new DataContractSerializer(typeof(ObservableCollection<ThreeVariables_Internal>));
                    serializer.WriteObject(stream, myOC_ThreeVariables_Internal.Select(x => new ThreeVariables_Internal() { myDefinitionName = x.myDefinitionName, myStorageType = x.myStorageType, myParameterType = x.myParameterType, myID = x.myID }).Cast<ThreeVariables_Internal>().ToList()); stream.Close();
                    stream.Close();

                    ////Stream stream2 = new FileStream(myString_FamilyFileXAML, FileMode.Open, FileAccess.Read);
                    ////DataContractSerializer serializer2 = new DataContractSerializer(typeof(ObservableCollection<ThreeVariables_Internal>));
                    ////myOC_ThreeVariables_Internal = (ObservableCollection<ThreeVariables_Internal>)serializer2.ReadObject(stream2);
                    ////myListView_Right.ItemsSource = myOC_ThreeVariables_Internal;
                    ////stream2.Close();
                    //myListView_Right.ItemsSource = myListThreeVariables2;

                    if (myElementType != null) MakePropertiesGridHappen(myOC_ThreeVariables_Internal, myPropertiesGrid, myElementType);
                    if (myElementType2 != null) MakePropertiesGridHappen(myOC_ThreeVariables_Internal, myPropertiesGrid2, myElementType2);
                }
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
        private void ListViewItem_PreviewMouseDoubleClick_AddParameter(object sender, MouseButtonEventArgs e)  //adding adding adding adding
        {
            int eL = -1;
            try
            {
                if(myOC_ThreeVariables_Internal == null)
                {
                    rePopulate_ListView_Right(((ElementAndSchedule)myListViewTypes.SelectedItem).elementType.Category.Id.IntegerValue);
                }

                Parameter myParameter = (Parameter)myListView_Left.SelectedItem;
                eL = 1162;
                if (myOC_ThreeVariables_Internal.Where(x => x.myID == myParameter.Id.IntegerValue).Count() > 0)
                {
                    MessageBox.Show("Parameter has already been added.");
                    return;
                }
                eL = 1168;
                myOC_ThreeVariables_Internal.Add(new ThreeVariables_Internal() { myDefinitionName = myParameter.Definition.Name, myStorageType = myParameter.StorageType, myParameterType = myParameter.Definition.ParameterType, myID = myParameter.Id.IntegerValue });

                myOC_ThreeVariables_Internal = new ObservableCollection<ThreeVariables_Internal>(myOC_ThreeVariables_Internal.OrderBy(i => i.myDefinitionName));

                myListView_Right.ItemsSource = myOC_ThreeVariables_Internal;
                // myOC_ThreeVariables_Internal = myOC_ThreeVariables_Internal.OrderBy(x => x.myDefinitionName) as ObservableCollection<ThreeVariables_Internal>;

                if (myListView_Right.Items.Count == 0)
                {
                    MessageBox.Show("There must be at least one item seleced in myListView_Right");
                    return;
                }
                eL = 1176;
                if (true)  //candidate for methodisation 22005161126
                {
                    string myEE02_String_FamilyFileName = ((ElementAndSchedule)myListViewTypes.SelectedItem).elementType.Category.Id.IntegerValue.ToString();
                    string myString_FamilyFileDirectory = myThisApplication.myString_1400_filename_FirstHalf + "\\ALL Parameter Profiles (do not edit directly)\\";
                    if (!Directory.Exists(myString_FamilyFileDirectory)) Directory.CreateDirectory(myString_FamilyFileDirectory);
                    string myString_FamilyFileXAML = myString_FamilyFileDirectory + myEE02_String_FamilyFileName + ".xml";

                    Stream stream = new FileStream(myString_FamilyFileXAML, FileMode.Create, FileAccess.Write);
                    DataContractSerializer serializer = new DataContractSerializer(typeof(ObservableCollection<ThreeVariables_Internal>));
                    serializer.WriteObject(stream, myOC_ThreeVariables_Internal.Select(x => new ThreeVariables_Internal() { myDefinitionName = x.myDefinitionName, myStorageType = x.myStorageType, myParameterType = x.myParameterType, myID = x.myID }).Cast<ThreeVariables_Internal>().ToList()); stream.Close();
                    stream.Close();

                    if (myElementType != null) MakePropertiesGridHappen(myOC_ThreeVariables_Internal, myPropertiesGrid, myElementType);
                    if (myElementType2 != null) MakePropertiesGridHappen(myOC_ThreeVariables_Internal, myPropertiesGrid2, myElementType2);
                }

                
                eL = 1192;
                //myListView_Right.Items.Refresh();
                e.Handled = true;
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
        public Element myPrivate_2020(bool myBoolPleaseSelectCheck) //store this in thumbs
        {

            UIDocument uidoc = myThisApplication.myExternalCommandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;


            Element firstElement = null;

            if (uidoc.Selection.GetElementIds().Count != 1)
            {
                if (uidoc.Selection.GetElementIds().Select(x => doc.GetElement(x).GetTypeId()).GroupBy(x => x.IntegerValue).Count() != 1)
                {
                    if (myBoolPleaseSelectCheck) MessageBox.Show("Please select only one Element.");
                    return null;
                }
            }
            firstElement = doc.GetElement(uidoc.Selection.GetElementIds().Last());

            if (firstElement.GetType() == typeof(IndependentTag))
            {
                IndependentTag myIndependentTag_1355 = firstElement as IndependentTag;
                if (myIndependentTag_1355.TaggedLocalElementId == null) return null;
                return doc.GetElement(myIndependentTag_1355.TaggedLocalElementId);
            }

            //if (firstElement.GetType() != typeof(FamilyInstance)) return null;
            return firstElement;
        }
        public ListOfPatternsParent_Specific myListOfPatternsParent_Specific { get; set; }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Properties.Settings.Default.Top = this.Top;
            Properties.Settings.Default.Left = this.Left;
            Properties.Settings.Default.Height = this.Height;
            Properties.Settings.Default.Width = this.Width;
            Properties.Settings.Default.ComboIndex = this.myComboBox.SelectedIndex;
            Properties.Settings.Default.GroupedBySchedule = this.rb_GroupedBySchedule.IsChecked.Value;
            Properties.Settings.Default.Save();
            Properties.Settings.Default.Reload();

            System.GC.Collect();
            //MessageBox.Show(this.Height.ToString());
        }

        private void MyPG_KeyUp(object sender, KeyEventArgs e)
        {
            myLabel.Visibility = System.Windows.Visibility.Visible;
        }

        private void MyPG_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            myLabel.Visibility = System.Windows.Visibility.Visible;
        }

        private void MyButtonRepickOriginal_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Element myElement = myPrivate_2020(true);
                if (myElement == null) return;
                myMethod_PopulateEverything(myElement);
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("MyButtonRepickOriginal_Click" + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
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
                //myListView_Right.ItemsSource = null;
                ///myListViewMaster.ItemsSource = myDataTable.DefaultView;
                //myListView_Right.ItemsSource = myDataTable.DefaultView;

                //////FamilyInstance myFamilyInstance = myPrivate_2020(true);
                //////if (myFamilyInstance == null) return;
                //////myMethod_PopulateEverything(myFamilyInstance, true);

                //listElement_AggregatingAllSchedules

               // MessageBox.Show("hello world");
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("Window_Loaded" + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        private void MyButtonRepick_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                UIDocument uidoc = myThisApplication.myExternalCommandData.Application.ActiveUIDocument;
                Document doc = uidoc.Document;

                if (uidoc.Selection.GetElementIds().Count != 1)
                {
                    MessageBox.Show("There must just one element selected");
                    return;
                }

                Element myElement2 = doc.GetElement(uidoc.Selection.GetElementIds().First());
                myElementType2 = doc.GetElement(myElement2.GetTypeId()) as ElementType;
                if (myElementType2 != null) MakePropertiesGridHappen(myOC_ThreeVariables_Internal, myPropertiesGrid2, myElementType2);
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("MyButtonRepick_Click" + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        private void MyCopyOver_Click(object sender, RoutedEventArgs e)
        {
            myPropertiesGrid.SelectedObject = myPropertiesGrid2.SelectedObject;
        }

        private void MyButton_Left_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                LeftAndRight(true);

            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("Window Loaded" + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        private void MyButton_Right_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                LeftAndRight(false);

            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("Window Loaded" + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        private void LeftAndRight(bool Reverse)
        {
            UIDocument uidoc = myThisApplication.myExternalCommandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            List<Family> myListFamilySymbol_1650 = new FilteredElementCollector(doc).WherePasses(new ElementClassFilter(typeof(Family))).Cast<Family>().Where(x => x.Name == ((FamilySymbol)myElementType).FamilyName).ToList();

            ElementId myElementID_Previous = null;
            bool myBoolProceedToMakeChange = false;

            List<ElementId> myListFamilySymbol = Reverse ? myListFamilySymbol_1650.First().GetFamilySymbolIds().ToList() : myListFamilySymbol_1650.First().GetFamilySymbolIds().Reverse().ToList();

            foreach (ElementId myFamilySymbollll in myListFamilySymbol)
            {
                if (myFamilySymbollll.IntegerValue == myElementType.Id.IntegerValue) break;

                myElementID_Previous = myFamilySymbollll;
                myBoolProceedToMakeChange = true;
            }
            if (myBoolProceedToMakeChange)
            {
                myElementType = doc.GetElement(myElementID_Previous) as ElementType;

                if (myElementType == null) myElementType = doc.GetElement(myElementID_Previous);

                MakePropertiesGridHappen(myOC_ThreeVariables_Internal, myPropertiesGrid, myElementType);

                myTextBox_TypeNameOld_Category.Text = myElementType.Category.Name;
                myTextBox_TypeNameOld.Text = myElementType.Name;
                myTextBox_TypeNameNew.Text = myElementType.Name;

                ////////////myTextBox_FamilyNameOld.Text = myElementType.FamilyName;
                ////////////myTextBox_FamilyNameNew.Text = myElementType.FamilyName;
            }
        }

        private void MyThrow_Click(object sender, RoutedEventArgs e)
        {
            MakePropertiesGridHappen(myOC_ThreeVariables_Internal, myPropertiesGrid2, myElementType);
        }

        public bool myPublicBool_LuminaresIsNot { get; set; } = true;

        private void myButton_AddType_Luminaires_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                myPublicBool_LuminaresIsNot = false;

                myExternalEvent_EE04_AddTypeParameter.Raise();
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("myButton_AddType_Luminaires_Click" + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        private void myButton_AddType_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                myPublicBool_LuminaresIsNot = true;

                myExternalEvent_EE04_AddTypeParameter.Raise();
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("myButton_AddType_Click" + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

    }
}
