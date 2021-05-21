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
using Autodesk.Revit.DB.ExtensibleStorage;
using _952_PRLoogleClassLibrary;
using pkRevitMisc.EntryPoints;

namespace pkRevitMisc.CommandsWithWindows.Add_Edit_Parameters
{
    /// <summary>
    /// Interaction logic for Window1617_AddEditParameters.xaml
    /// </summary>
    /// 
    ////public class ToAvoidLoadingRevitDLLs  //pkRevitMisc.CommandsWithWindows.Add_Edit_Parameters
    ////{
    ////    public ExternalCommandData commandData { get; set; }
    ////    public string executionLocation { get; set; }
    ////}
    //////public ToAvoidLoadingRevitDLLs toavoidloadingrevitdlls { get; set; }

    public partial class Window1617_AddEditParameters : Window
    {
        public ToAvoidLoadingRevitDLLs toavoidloadingrevitdlls { get; set; }
        //////public Entry_0170_pkRevitMisc myWindow1 { get; set; }


        public EE16_AddSharedParameters_InVariousWays myEE16_AddSharedParameters_InVariousWays { get; set; }
        public ExternalEvent myExternalEvent_EE16_AddSharedParameters_InVariousWays { get; set; }

        public EE17_Edit_StringBasedParameters myEE17_Edit_StringBasedParameters { get; set; }
        public ExternalEvent myExternalEvent_EE17_Edit_StringBasedParameters { get; set; }




        //////////////public bool myBool_AddToProject { get; set; } = true;



        public partial class aBuiltInParameter_and_Name
        {
            public BuiltInParameter theBIP { get; set; }
            public string theParameterName { get; set; }   //Note DisplayMemberPath doesn't work unless it is an auto-implementing field (uses get;set)
            public bool theIsBuiltInParameter { get; set; }
        }

        public Window1617_AddEditParameters(ToAvoidLoadingRevitDLLs ecd)
        {
            toavoidloadingrevitdlls = ecd;

            createParameterFiles();

            InitializeComponent();

            this.Top = Properties.Settings.Default.WindowAddEditParameters_Top;
            this.Left = Properties.Settings.Default.WindowAddEditParameters_Left;

            myEE16_AddSharedParameters_InVariousWays = new EE16_AddSharedParameters_InVariousWays();
           // myEE16_AddSharedParameters_InVariousWays.myWindow1 = this;
            myExternalEvent_EE16_AddSharedParameters_InVariousWays = ExternalEvent.Create(myEE16_AddSharedParameters_InVariousWays);

            myEE17_Edit_StringBasedParameters = new EE17_Edit_StringBasedParameters();
            //myEE17_Edit_StringBasedParameters.myWindow1 = this;
            myExternalEvent_EE17_Edit_StringBasedParameters = ExternalEvent.Create(myEE17_Edit_StringBasedParameters);


            UIDocument uidoc = toavoidloadingrevitdlls.commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            if (uidoc.Selection.GetElementIds().Count != 0)
            {
                myIntegerUpDown.Value = uidoc.Selection.GetElementIds().First().IntegerValue;

                Element myElement = doc.GetElement(uidoc.Selection.GetElementIds().First());
                btn_CategoryInstanceSpecific_textBlock.Text = "Add to category: " + myElement.Category.Name;
                btn_CategoryTypeSpecific_textBlock.Text = "Add to category: " + myElement.Category.Name;
                btn_CategoryTypeFamily_textBlock.Text = "Add to family: " + myElement.get_Parameter(BuiltInParameter.ELEM_FAMILY_PARAM).AsValueString();


                myLabel_Type.Content = myElement.Name;
                myLabel_Family.Content = myElement.get_Parameter(BuiltInParameter.ELEM_FAMILY_PARAM).AsValueString();
            }

            if (true)
            {
                BuiltInParameter myBIP = BuiltInParameter.ALL_MODEL_TYPE_COMMENTS;
                string myParameterName = "ALL_MODEL_TYPE_COMMENTS";

                aBuiltInParameter_and_Name myBIP_and_IsStance = new aBuiltInParameter_and_Name() { theBIP = myBIP, theParameterName = myParameterName, theIsBuiltInParameter = true };
                myListBoxTypeParameters.Items.Add(myBIP_and_IsStance);
            }
            if (/*hash out x2,*HASH OUT IN *.txt* ______ x2, and this -> x 5*/true) //make false make FALSE make FALSE //make false make FALSE make FALSE //make false make FALSE make FALSE //make false make FALSE make FALSE 
            {
                BuiltInParameter myBIP = BuiltInParameter.ALL_MODEL_DESCRIPTION; //ALL_MODEL_DESCRIPTION
                string myParameterName = "ALL_MODEL_DESCRIPTION";

                aBuiltInParameter_and_Name myBIP_and_IsStance = new aBuiltInParameter_and_Name() { theBIP = myBIP, theParameterName = myParameterName, theIsBuiltInParameter = true };
                myListBoxTypeParameters.Items.Add(myBIP_and_IsStance);
            }
            if (true)
            {
                BuiltInParameter myBIP = BuiltInParameter.ALL_MODEL_MANUFACTURER;
                string myParameterName = "ALL_MODEL_MANUFACTURER";

                aBuiltInParameter_and_Name myBIP_and_IsStance = new aBuiltInParameter_and_Name() { theBIP = myBIP, theParameterName = myParameterName, theIsBuiltInParameter = true };
                myListBoxTypeParameters.Items.Add(myBIP_and_IsStance);
            }
            if (true)
            {
                string myParameterName = "Manufacturer Alternative1";

                aBuiltInParameter_and_Name myBIP_and_IsStance = new aBuiltInParameter_and_Name() { theParameterName = myParameterName, theIsBuiltInParameter = false };
                myListBoxTypeParameters.Items.Add(myBIP_and_IsStance);
            }
            if (/*hash out x2,*HASH OUT IN *.txt* ______ x2, and this -> x 5*/true) //make false make FALSE make FALSE //make false make FALSE make FALSE //make false make FALSE make FALSE //make false make FALSE make FALSE 
            {
                string myParameterName = "Manufacturer Alternative2";

                aBuiltInParameter_and_Name myBIP_and_IsStance = new aBuiltInParameter_and_Name() { theParameterName = myParameterName, theIsBuiltInParameter = false };
                myListBoxTypeParameters.Items.Add(myBIP_and_IsStance);
            }
            if (/*hash out x2,*HASH OUT IN *.txt* ______ x2, and this -> x 5*/true) //make false make FALSE make FALSE //make false make FALSE make FALSE //make false make FALSE make FALSE //make false make FALSE make FALSE 
            {
                string myParameterName = "Manufacturer Alternative3";

                aBuiltInParameter_and_Name myBIP_and_IsStance = new aBuiltInParameter_and_Name() { theParameterName = myParameterName, theIsBuiltInParameter = false };
                myListBoxTypeParameters.Items.Add(myBIP_and_IsStance);
            }

            if (true)
            {
                BuiltInParameter myBIP = BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS;
                string myParameterName = "ALL_MODEL_INSTANCE_COMMENTS";

                aBuiltInParameter_and_Name myBIP_and_IsStance = new aBuiltInParameter_and_Name() { theBIP = myBIP, theParameterName = myParameterName, theIsBuiltInParameter = true };
                myListBoxInstanceParameters.Items.Add(myBIP_and_IsStance);
            }
            if (true)
            {
                string myParameterName = "Comments1";

                aBuiltInParameter_and_Name myBIP_and_IsStance = new aBuiltInParameter_and_Name() { theParameterName = myParameterName, theIsBuiltInParameter = false };
                myListBoxInstanceParameters.Items.Add(myBIP_and_IsStance);
            }
            if (/*hash out x2,*HASH OUT IN *.txt* ______ x2, and this -> x 5*/true) //make false make FALSE make FALSE //make false make FALSE make FALSE //make false make FALSE make FALSE //make false make FALSE make FALSE 
            {
                string myParameterName = "Comments2";

                aBuiltInParameter_and_Name myBIP_and_IsStance = new aBuiltInParameter_and_Name() { theParameterName = myParameterName, theIsBuiltInParameter = false };
                myListBoxInstanceParameters.Items.Add(myBIP_and_IsStance);
            }
            if (/*hash out x2,*HASH OUT IN *.txt* ______ x2, and this -> x 5*/true) //make false make FALSE make FALSE //make false make FALSE make FALSE //make false make FALSE make FALSE //make false make FALSE make FALSE 
            {
                string myParameterName = "Comments3";

                aBuiltInParameter_and_Name myBIP_and_IsStance = new aBuiltInParameter_and_Name() { theParameterName = myParameterName, theIsBuiltInParameter = false };
                myListBoxInstanceParameters.Items.Add(myBIP_and_IsStance);
            }

            myListBoxTypeParameters.SelectedIndex = -1;
            myListBoxInstanceParameters.SelectedIndex = -1;

        }

        private void MyButtonAcquireSelected_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                UIDocument uidoc = toavoidloadingrevitdlls.commandData.Application.ActiveUIDocument;
                Document doc = uidoc.Document;

                if (uidoc.Selection.GetElementIds().Count != 1)
                {
                    MessageBox.Show("Please select an element first.");
                    return;
                }

                Element myElement = doc.GetElement(uidoc.Selection.GetElementIds().First());
                btn_CategoryInstanceSpecific_textBlock.Text = "Add to category: " + myElement.Category.Name;
                btn_CategoryTypeSpecific_textBlock.Text = "Add to category: " + myElement.Category.Name;
                btn_CategoryTypeFamily_textBlock.Text = "Add to family: " + myElement.get_Parameter(BuiltInParameter.ELEM_FAMILY_PARAM).AsValueString();

                //btn_CategoryInstanceSpecific

                myIntegerUpDown.Value = uidoc.Selection.GetElementIds().First().IntegerValue;
                myLabel_Type.Content = myElement.Name;
                myLabel_Family.Content = myElement.get_Parameter(BuiltInParameter.ELEM_FAMILY_PARAM).AsValueString();

                myTextBoxPrevious.Text = "";
                myTextBoxNew.Text = "";
                myListBoxInstanceParameters.SelectedIndex = -1;
                myListBoxTypeParameters.SelectedIndex = -1;
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("MyButtonAcquireSelected_Click" + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        public bool myBool_StayDown { get; set; } = false;

        private void myListBoxInstanceParameters_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (!myBool_StayDown) myTextBoxNew.IsEnabled = true;
                myBool_StayDown = false;

                if (myListBoxInstanceParameters.SelectedIndex == -1) return;

                myTextBoxPrevious.Text = "";
                myTextBoxNew.Text = "";
                // myTextBoxNew.IsEnabled = true;

                myListBoxTypeParameters.SelectedIndex = -1;

                if (myListBoxInstanceParameters.SelectedItems.Count != 1) return;

                if (myIntegerUpDown.Value.Value == -1)
                {
                    myTextBoxPrevious.Text = "";
                    myTextBoxNew.Text = "";
                    myListBoxInstanceParameters.SelectedIndex = -1;
                    myListBoxTypeParameters.SelectedIndex = -1;

                    MessageBox.Show("Please select and 'Acquire' an entity.");
                    return;
                }
                //myWindow2.toavoidloadingrevitdlls.executionLocation.
                UIDocument uidoc = toavoidloadingrevitdlls.commandData.Application.ActiveUIDocument;
                Document doc = uidoc.Document;

                Element myElement = doc.GetElement(new ElementId(myIntegerUpDown.Value.Value));

                if (myElement == null)
                {
                    myIntegerUpDown.Value = -1;
                    return;
                }

                Parameter myParameter = null;

                if (((Window1617_AddEditParameters.aBuiltInParameter_and_Name)myListBoxInstanceParameters.SelectedItem).theIsBuiltInParameter)
                {
                    myParameter = myElement.get_Parameter(((Window1617_AddEditParameters.aBuiltInParameter_and_Name)myListBoxInstanceParameters.SelectedItem).theBIP);  //myListBoxTypeParameters
                }
                else
                {
                    if (myElement.LookupParameter(((Window1617_AddEditParameters.aBuiltInParameter_and_Name)myListBoxInstanceParameters.SelectedItem).theParameterName) == null) return;

                    myParameter = myElement.GetParameters(((Window1617_AddEditParameters.aBuiltInParameter_and_Name)myListBoxInstanceParameters.SelectedItem).theParameterName)[0];
                }

                if (myParameter == null) return;

                myTextBoxPrevious.Text = myParameter.AsString();
                myTextBoxNew.Text = myParameter.AsString();
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("myListBoxInstanceParameters_SelectionChanged" + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        private void myListBoxTypeParameters_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (!myBool_StayDown) myTextBoxNew.IsEnabled = true;
                myBool_StayDown = false;

                if (myListBoxTypeParameters.SelectedIndex == -1) return;


                myTextBoxPrevious.Text = "";
                myTextBoxNew.Text = "";


                myListBoxInstanceParameters.SelectedIndex = -1;

                if (myListBoxTypeParameters.SelectedItems.Count != 1) return;

                if (myIntegerUpDown.Value.Value == -1)
                {
                    myTextBoxPrevious.Text = "";
                    myTextBoxNew.Text = "";
                    myListBoxInstanceParameters.SelectedIndex = -1;
                    myListBoxTypeParameters.SelectedIndex = -1;

                    MessageBox.Show("Please select and 'Acquire' an entity.");
                    return;
                }

                UIDocument uidoc = toavoidloadingrevitdlls.commandData.Application.ActiveUIDocument;
                Document doc = uidoc.Document;

                Element myElement = doc.GetElement(new ElementId(myIntegerUpDown.Value.Value));

                if (myElement == null)
                {
                    myIntegerUpDown.Value = -1;

                    return;
                }

                myElement = doc.GetElement(myElement.GetTypeId());

                Parameter myParameter = null;

                if (((Window1617_AddEditParameters.aBuiltInParameter_and_Name)myListBoxTypeParameters.SelectedItem).theIsBuiltInParameter)
                {
                    myParameter = myElement.get_Parameter(((Window1617_AddEditParameters.aBuiltInParameter_and_Name)myListBoxTypeParameters.SelectedItem).theBIP);  //myListBoxTypeParameters
                }
                else
                {
                    if (myElement.LookupParameter(((Window1617_AddEditParameters.aBuiltInParameter_and_Name)myListBoxTypeParameters.SelectedItem).theParameterName) == null) return;

                    myParameter = myElement.GetParameters(((Window1617_AddEditParameters.aBuiltInParameter_and_Name)myListBoxTypeParameters.SelectedItem).theParameterName)[0];
                }

                if (myParameter == null) return;

                myTextBoxPrevious.Text = myParameter.AsString();
                myTextBoxNew.Text = myParameter.AsString();
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("myListBoxTypeParameters_SelectionChanged" + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        private void myButtonSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (myListBoxTypeParameters.SelectedIndex == -1 & myListBoxInstanceParameters.SelectedIndex == -1) return;

                myEE17_Edit_StringBasedParameters.myWindow2 = this;
                myExternalEvent_EE17_Edit_StringBasedParameters.Raise();
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("myButtonSave_Click" + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        private void myButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Close();
            }
            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("myButtonCancel_Click" + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        private void butView_CategoryInstanceSpecific_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string fullpath_PlayPenSharedParametersInstance = toavoidloadingrevitdlls.executionLocation.Split('|')[1] + "//CommandsWithWindows//Add Edit Parameters//CategoryInstanceSpecific.txt";
                string fullpath_copyTo = System.IO.Path.GetTempPath() + "//Copy of CategoryInstanceSpecific.txt";

                System.IO.File.Copy(fullpath_PlayPenSharedParametersInstance, fullpath_copyTo, true);

                System.Diagnostics.Process.Start(fullpath_copyTo);

            }
            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("butView_CategoryInstanceSpecific_Click" + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        private void butView_CategoryInstanceALL_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string fullpath_PlayPenSharedParametersInstance = toavoidloadingrevitdlls.executionLocation.Split('|')[1] + "//CommandsWithWindows//Add Edit Parameters//CategoryInstanceALL.txt";
                string fullpath_copyTo = System.IO.Path.GetTempPath() + "//Copy of CategoryInstanceALL.txt";

                System.IO.File.Copy(fullpath_PlayPenSharedParametersInstance, fullpath_copyTo, true);

                System.Diagnostics.Process.Start(fullpath_copyTo);

            }
            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("butView_CategoryInstanceALL_Click" + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        private void butView_CategoryTypeSpecific_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string fullpath_PlayPenSharedParametersInstance = toavoidloadingrevitdlls.executionLocation.Split('|')[1] + "//CommandsWithWindows//Add Edit Parameters//CategoryTypeSpecific.txt";
                string fullpath_copyTo = System.IO.Path.GetTempPath() + "//Copy of CategoryTypeSpecific.txt";

                System.IO.File.Copy(fullpath_PlayPenSharedParametersInstance, fullpath_copyTo, true);

                System.Diagnostics.Process.Start(fullpath_copyTo);

            }
            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("butView_CategoryTypeSpecific_Click" + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        private void butView_CategoryTypeALL_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string fullpath_PlayPenSharedParametersInstance = toavoidloadingrevitdlls.executionLocation.Split('|')[1] + "//CommandsWithWindows//Add Edit Parameters//CategoryTypeALL.txt";
                string fullpath_copyTo = System.IO.Path.GetTempPath() + "//Copy of CategoryTypeALL.txt";

                System.IO.File.Copy(fullpath_PlayPenSharedParametersInstance, fullpath_copyTo, true);

                System.Diagnostics.Process.Start(fullpath_copyTo);

            }
            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("butView_CategoryTypeALL_Click" + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        private void butView_CategoryTypeFamily_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string fullpath_PlayPenSharedParametersInstance = toavoidloadingrevitdlls.executionLocation.Split('|')[1] + "//CommandsWithWindows//Add Edit Parameters//CategoryTypeFamily.txt";
                string fullpath_copyTo = System.IO.Path.GetTempPath() + "//Copy of CategoryTypeFamily.txt";

                System.IO.File.Copy(fullpath_PlayPenSharedParametersInstance, fullpath_copyTo, true);

                System.Diagnostics.Process.Start(fullpath_copyTo);

            }
            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("butView_CategoryTypeFamily_Click" + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }



        private bool SelectedDoesNotMatchAcquired()
        {
            UIDocument uidoc = toavoidloadingrevitdlls.commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            if (doc.GetElement(new ElementId(myIntegerUpDown.Value.Value)) == null)
            {
                MessageBox.Show("Please use 'Acquire Selected' button.");
                return false;
            }

            if (uidoc.Selection.GetElementIds().Count > 0)
            {
                if (uidoc.Selection.GetElementIds().First().IntegerValue != myIntegerUpDown.Value.Value)
                {
                    MessageBoxResult result = System.Windows.MessageBox.Show("Note: The 'Selected' element does not match the 'Acquired' element." + Environment.NewLine + Environment.NewLine + "                      This will act on the 'Acquired' element." + Environment.NewLine + Environment.NewLine + "                      Proceed anyway?", "Proceed?", System.Windows.MessageBoxButton.YesNoCancel);

                    if (result == MessageBoxResult.Yes)
                    {
                        return true;
                    }

                    return false;
                }
            }
            return true;
        }

        ////private void myButtonAddParametersToProject_Click(object sender, RoutedEventArgs e)
        ////{
        ////    try
        ////    {
        ////        SelectedDoesNotMatchAcquired();

        ////        myEE16_AddSharedParameters_InVariousWays.myWindow2 = this;
        ////        myEE16_AddSharedParameters_InVariousWays.myBool_AddToProject = true;
        ////        myExternalEvent_EE16_AddSharedParameters_InVariousWays.Raise();
        ////    }
        ////    #region catch and finally
        ////    catch (Exception ex)
        ////    {
        ////        _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("myButtonAddParametersToProject_Click" + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
        ////    }
        ////    finally
        ////    {
        ////    }
        ////    #endregion
        ////}

        ////private void myButtonAddParametersToFamily_Click(object sender, RoutedEventArgs e)
        ////{
        ////    try
        ////    {
        ////        SelectedDoesNotMatchAcquired();

        ////        myEE16_AddSharedParameters_InVariousWays.myWindow2 = this;
        ////        myEE16_AddSharedParameters_InVariousWays.myBool_AddToProject = false;
        ////        myExternalEvent_EE16_AddSharedParameters_InVariousWays.Raise();
        ////    }
        ////    #region catch and finally
        ////    catch (Exception ex)
        ////    {
        ////        _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("myButtonAddParametersToFamily_Click" + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
        ////    }
        ////    finally
        ////    {
        ////    }
        ////    #endregion
        ////}

        private void btn_CategoryInstanceSpecific_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!SelectedDoesNotMatchAcquired()) return;

                myEE16_AddSharedParameters_InVariousWays.myWindow2 = this;
                myEE16_AddSharedParameters_InVariousWays.myInt_LoadType = (int)PARAMETER_LOAD.Instance_Specific;
                //myEE16_AddSharedParameters_InVariousWays.myBool_AddToProject = false;
                myExternalEvent_EE16_AddSharedParameters_InVariousWays.Raise();
            }
            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("btn_CategoryInstanceSpecific_Click" + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        private void btn_CategoryInstanceALL_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //SelectedDoesNotMatchAcquired();

                myEE16_AddSharedParameters_InVariousWays.myWindow2 = this;
                myEE16_AddSharedParameters_InVariousWays.myInt_LoadType = (int)PARAMETER_LOAD.Instance_ALL;
                //myEE16_AddSharedParameters_InVariousWays.myBool_AddToProject = false;
                myExternalEvent_EE16_AddSharedParameters_InVariousWays.Raise();
            }
            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("btn_CategoryInstanceALL_Click" + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        private void btn_CategoryTypeSpecific_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!SelectedDoesNotMatchAcquired()) return;

                myEE16_AddSharedParameters_InVariousWays.myWindow2 = this;
                myEE16_AddSharedParameters_InVariousWays.myInt_LoadType = (int)PARAMETER_LOAD.Type_Specific;
                //myEE16_AddSharedParameters_InVariousWays.myBool_AddToProject = false;
                myExternalEvent_EE16_AddSharedParameters_InVariousWays.Raise();
            }
            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("btn_CategoryTypeSpecific_Click" + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        private void btn_CategoryTypeALL_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //SelectedDoesNotMatchAcquired();

                myEE16_AddSharedParameters_InVariousWays.myWindow2 = this;
                myEE16_AddSharedParameters_InVariousWays.myInt_LoadType = (int)PARAMETER_LOAD.Type_ALL;
                //myEE16_AddSharedParameters_InVariousWays.myBool_AddToProject = false;
                myExternalEvent_EE16_AddSharedParameters_InVariousWays.Raise();
            }
            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("btn_CategoryTypeALL_Click" + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        private void btn_CategoryTypeFamily_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //SelectedDoesNotMatchAcquired();

                myEE16_AddSharedParameters_InVariousWays.myWindow2 = this;
                myEE16_AddSharedParameters_InVariousWays.myInt_LoadType = (int)PARAMETER_LOAD.Type_Family;
                //myEE16_AddSharedParameters_InVariousWays.myBool_AddToProject = false;
                myExternalEvent_EE16_AddSharedParameters_InVariousWays.Raise();
            }
            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("btn_CategoryTypeFamily_Click" + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }


        private void createParameterFiles()
        {
            string path = (System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\pk Revit\\Shared Parameter Files");
            string pathBackup = path + "\\Original Backup";
            if (!System.IO.Directory.Exists(pathBackup)) System.IO.Directory.CreateDirectory(pathBackup);


            string Instance_Specific = "\\CategoryInstanceSpecific.txt";
            string Instance_ALL = "\\CategoryInstanceALL.txt";
            string Type_Specific = "\\CategoryTypeSpecific.txt";
            string Type_ALL = "\\CategoryTypeALL.txt";
            string Type_Family = "\\CategoryTypeFamily.txt";
            string ReadMe = "\\READ ME.txt";

            string myString_TempPath = toavoidloadingrevitdlls.executionLocation.Split('|')[1] + "//CommandsWithWindows//Add Edit Parameters";

            if (!System.IO.File.Exists(path + Instance_Specific)) System.IO.File.Copy(myString_TempPath + Instance_Specific, path + Instance_Specific, true);
            if (!System.IO.File.Exists(path + Instance_ALL)) System.IO.File.Copy(myString_TempPath + Instance_ALL, path + Instance_ALL, true);
            if (!System.IO.File.Exists(path + Type_Specific)) System.IO.File.Copy(myString_TempPath + Type_Specific, path + Type_Specific, true);
            if (!System.IO.File.Exists(path + Type_ALL)) System.IO.File.Copy(myString_TempPath + Type_ALL, path + Type_ALL, true);
            if (!System.IO.File.Exists(path + Type_Family)) System.IO.File.Copy(myString_TempPath + Type_Family, path + Type_Family, true);
            if (!System.IO.File.Exists(path + ReadMe)) System.IO.File.Copy(myString_TempPath + ReadMe, path + ReadMe, true);

            if (!System.IO.File.Exists(pathBackup + Instance_Specific)) System.IO.File.Copy(myString_TempPath + Instance_Specific, pathBackup + Instance_Specific, true);
            if (!System.IO.File.Exists(pathBackup + Instance_ALL)) System.IO.File.Copy(myString_TempPath + Instance_ALL, pathBackup + Instance_ALL, true);
            if (!System.IO.File.Exists(pathBackup + Type_Specific)) System.IO.File.Copy(myString_TempPath + Type_Specific, pathBackup + Type_Specific, true);
            if (!System.IO.File.Exists(pathBackup + Type_ALL)) System.IO.File.Copy(myString_TempPath + Type_ALL, pathBackup + Type_ALL, true);
            if (!System.IO.File.Exists(pathBackup + Type_Family)) System.IO.File.Copy(myString_TempPath + Type_Family, pathBackup + Type_Family, true);
        }

        private void btn_Open_pkRevitFolder_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string path = (System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\pk Revit\\Shared Parameter Files");
                System.Diagnostics.Process.Start(path);
            }
            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("btn_Open_pkRevitFolder_Click" + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
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
                Properties.Settings.Default.WindowAddEditParameters_Top = this.Top;
                Properties.Settings.Default.WindowAddEditParameters_Left = this.Left;
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

    public enum PARAMETER_LOAD
    {
        Instance_Specific = 1,
        Instance_ALL = 2,
        Type_Specific = 3,
        Type_ALL = 4,
        Type_Family = 5
    }

}
