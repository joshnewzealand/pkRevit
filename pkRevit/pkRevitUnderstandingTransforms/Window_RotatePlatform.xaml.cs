/*
 * Created by SharpDevelop.
 * User: Joshua.Lumley
 * Date: 28/04/2019
 * Time: 5:59 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using System.Linq;

using Numerics = System.Numerics;
using Transform = Autodesk.Revit.DB.Transform;
using Binding = Autodesk.Revit.DB.Binding;
using RevitTransformSliders;
using pkRevitUnderstandingTransforms.External_Events;
using System.Threading.Tasks;

namespace pkRevitUnderstandingTransforms
{
    /// <summary>
    /// Interaction logic for Window_RotatePlatform.xaml
    /// </summary>
    public partial class Window_RotatePlatform : Window
    {
        ////public Xceed.Wpf.Toolkit.IntegerUpDown myToolKit_IntUpDown { get; set; }
        ////public Slider mySlider { get; set; }

        //public bool mySlideInProgress { get; set; } = false;
        public bool myBool_Rezero { get; set; } = false;

        public EE03_RotateAroundBasis myEE03_RotateAroundBasis { get; set; }
        public ExternalEvent myExternalEvent_EE03_RotateAroundBasis { get; set; }

        public EE04_ResetPosition myEE04_ResetPosition { get; set; }
        public ExternalEvent myExternalEvent_EE04_ResetPosition { get; set; }

        public EE05_Move myEE05_Move { get; set; }
        public ExternalEvent myExternalEvent_EE05_Move { get; set; }

        public EE06_PlaceAFamily_OnDoubleClick myEE06_PlaceAFamily_OnDoubleClick { get; set; }
        public ExternalEvent myExternalEvent_EE06_PlaceAFamily_OnDoubleClick { get; set; }

        public EE06_PlaceAFamily_CentreScreen myEE06_PlaceAFamily_CentreScreen { get; set; }
        public ExternalEvent myExternalEvent_EE06_PlaceAFamily_CentreScreen { get; set; }

        public EE07_Unhost myEE07_Unhost { get; set; }
        public ExternalEvent myExternalEvent_EE07_Unhost { get; set; }

        public EE08_LoadFamily myEE08_LoadFamily { get; set; }
        public ExternalEvent myExternalEvent_EE08_LoadFamily { get; set; }

        public EE09_RotateBy90 myEE09_RotateBy90 { get; set; }
        public ExternalEvent myExternalEvent_EE09_RotateBy90  { get; set; }
        

        ////////public string messageConst { get; set; }
        ////////public ExternalCommandData commandData { get; set; }

        public ToAvoidLoadingRevitDLLs toavoidloadingrevitdlls { get; set; }

        public SetSlider setSliderClassInstance { get; set; }

        public ReferencePoint myReferencePoint_Window { get; set; }

        private FamilySymbol returnSymbol_workswhen_TypeAnd_FamilyNameAreSame(string FamilyAndTypeName)
        {
            UIDocument uidoc = toavoidloadingrevitdlls.commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            IEnumerable<Element> myIEnumerableElement = new FilteredElementCollector(doc).WherePasses(new ElementClassFilter(typeof(Family))).Where(x => x.Name == FamilyAndTypeName);

            if (myIEnumerableElement.Count() == 0) return null;
            return doc.GetElement(((Family)myIEnumerableElement.First()).GetFamilySymbolIds().First()) as FamilySymbol;
        }

        private void button_NewOnWorkplane_Click(object sender, RoutedEventArgs e)
        {
            try
            {

            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary2.DatabaseMethods.writeDebug("button_NewOnWorkplane_Click" + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        private void button_NewCenter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                myExternalEvent_EE06_PlaceAFamily_CentreScreen.Raise();

                ////////string str_getOrLoadThis_Family = "PRL-GM-2020 Adaptive Carrier";

                ////////FamilySymbol myFamilySymbol_Carrier = returnSymbol_workswhen_TypeAnd_FamilyNameAreSame(str_getOrLoadThis_Family);


                ////////if (myFamilySymbol_Carrier == null)
                ////////{
                ////////    myEE08_LoadFamily.str_getOrLoadThis_Family = str_getOrLoadThis_Family;
                ////////    myEE08_LoadFamily.myWindow1 = this;

                ////////    int myInt = 0;

                ////////    Dispatcher.Invoke(async () =>
                ////////    {
                ////////        myExternalEvent_EE08_LoadFamily.Raise();

                ////////        myEE08_LoadFamily.bool_Loop_UntilFinished = true;

                ////////        while (myEE08_LoadFamily.bool_Loop_UntilFinished)
                ////////        {
                ////////            await Task.Delay(100);
                ////////        }

                ////////        myFamilySymbol_Carrier = returnSymbol_workswhen_TypeAnd_FamilyNameAreSame(str_getOrLoadThis_Family);

                ////////        myEE06_PlaceAFamily_OnDoubleClick.myFamilySymbol = myFamilySymbol_Carrier;
                ////////        myExternalEvent_EE06_PlaceAFamily_OnDoubleClick.Raise();
                ////////    });
                ////////}
                ////////else
                ////////{
                ////////    myEE06_PlaceAFamily_OnDoubleClick.myFamilySymbol = myFamilySymbol_Carrier;
                ////////    myExternalEvent_EE06_PlaceAFamily_OnDoubleClick.Raise();
                ////////}
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary2.DatabaseMethods.writeDebug("button_NewCenter_Click" + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        private void button_NewOnFace_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string str_getOrLoadThis_Family = "PRL-GM-2020 Adaptive Carrier";

                FamilySymbol myFamilySymbol_Carrier = returnSymbol_workswhen_TypeAnd_FamilyNameAreSame(str_getOrLoadThis_Family);

               
                if(myFamilySymbol_Carrier == null)
                {
                    myEE08_LoadFamily.str_getOrLoadThis_Family = str_getOrLoadThis_Family;
                    myEE08_LoadFamily.toavoidloadingrevitdlls = toavoidloadingrevitdlls;

                    Dispatcher.Invoke(async () =>
                    {
                        myExternalEvent_EE08_LoadFamily.Raise();

                        myEE08_LoadFamily.bool_Loop_UntilFinished = true;

                        while (myEE08_LoadFamily.bool_Loop_UntilFinished)
                        {
                            await Task.Delay(100);
                        }

                        myFamilySymbol_Carrier = returnSymbol_workswhen_TypeAnd_FamilyNameAreSame(str_getOrLoadThis_Family);
                        myEE06_PlaceAFamily_OnDoubleClick.myFamilySymbol = myFamilySymbol_Carrier;
                        myExternalEvent_EE06_PlaceAFamily_OnDoubleClick.Raise();
                    });
                } else
                {
                    myEE06_PlaceAFamily_OnDoubleClick.myFamilySymbol = myFamilySymbol_Carrier;
                    myExternalEvent_EE06_PlaceAFamily_OnDoubleClick.Raise();
                }
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary2.DatabaseMethods.writeDebug("button_NewOnFace_Click" + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        public class FamilyOptionOverWrite : IFamilyLoadOptions
        {
            public bool OnFamilyFound(bool familyInUse, out bool overwriteParameterValues)
            {
                overwriteParameterValues = true;
                return true;
            }
            public bool OnSharedFamilyFound(Family sharedFamily, bool familyInUse, out FamilySource source, out bool overwriteParameterValues)
            {
                source = FamilySource.Family;
                overwriteParameterValues = true;
                return true;
            }
        }

        private void button_Unhost_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                UIDocument uidoc = toavoidloadingrevitdlls.commandData.Application.ActiveUIDocument;
                Document doc = uidoc.Document;

                myReferencePoint_Window = doc.GetElement(new ElementId(myIntUpDown_Middle2.Value.Value)) as ReferencePoint;

                if (myReferencePoint_Window == null)
                {
                    myIntUpDown_Middle2.Value = -1;
                    myBool_Rezero = true;
                    MessageBox.Show("Please 'pick' or 'place' platform 'AGM'");
                    return;
                }
                myExternalEvent_EE07_Unhost.Raise();
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary2.DatabaseMethods.writeDebug("button_Unhost_Click" + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        public void updateMiddle2_UpdateUI()
        {
            int eL = -1;
            try
            {
                UIDocument uidoc = toavoidloadingrevitdlls.commandData.Application.ActiveUIDocument;
                Document doc = uidoc.Document;

                if (!SelectMethod.mySelectMethod(uidoc, myIntUpDown_Middle2)) return;

                ReferencePoint myRefPoint = doc.GetElement(new ElementId(myIntUpDown_Middle2.Value.Value)) as ReferencePoint;

                if (myRefPoint.get_Parameter(BuiltInParameter.POINT_ELEMENT_DRIVEN).AsInteger() == 1)
                {
                    myButton_HomeY.IsEnabled = false;
                    myButton_NinetyZ.IsEnabled = false;
                    myButton_NinetyZ_Reverse.IsEnabled = false;
                    myButton_HomeZ.IsEnabled = false;
                    myButton_NinetyY.IsEnabled = false;
                    myButton_NinetyY_Reverse.IsEnabled = false;

                    mySlider_Rotate_BasisX.IsEnabled = false;
                    mySlider_Rotate_BasisY.IsEnabled = false;
                    button_Unhost.Background = Brushes.Yellow;
                    button_Unhost.IsEnabled = true;
                }
                else
                {
                    myButton_HomeY.IsEnabled = true;
                    myButton_NinetyZ.IsEnabled = true;
                    myButton_NinetyZ_Reverse.IsEnabled = true;
                    myButton_HomeZ.IsEnabled = true;
                    myButton_NinetyY.IsEnabled = true;
                    myButton_NinetyY_Reverse.IsEnabled = true;

                    mySlider_Rotate_BasisX.IsEnabled = true;
                    mySlider_Rotate_BasisY.IsEnabled = true;
                    button_Unhost.Background = null;
                    button_Unhost.IsEnabled = false;
                }

                setSliderClassInstance.setSlider(myRefPoint, mySlider_Rotate_BasisZ, false);
                setSliderClassInstance.setSlider(myRefPoint, mySlider_Rotate_BasisX, false);
                setSliderClassInstance.setSlider(myRefPoint, mySlider_Rotate_BasisY, true);
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("updateMiddle2_UpdateUI, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        private void button_RepickfromModel_Click(object sender, RoutedEventArgs e)
        {
            updateMiddle2_UpdateUI();
        }

        public Window_RotatePlatform(ToAvoidLoadingRevitDLLs TALDLL, int int_SelectedRefPoint)
        {

            toavoidloadingrevitdlls = TALDLL;
            //myThisApplication = tA;
            ////messageConst = message;
            ////commandData = cD;

            InitializeComponent();

            myIntUpDown_Middle2.Value = int_SelectedRefPoint;

            setSliderClassInstance = new pkRevitUnderstandingTransforms.External_Events.SetSlider();
            setSliderClassInstance.commandData = toavoidloadingrevitdlls.commandData;
            setSliderClassInstance.myLabel_TransformOrigin = myLabel_TransformOrigin;
            setSliderClassInstance.myLabel_TransformXBasis = myLabel_TransformXBasis;
            setSliderClassInstance.myLabel_TransformYBasis = myLabel_TransformYBasis;
            setSliderClassInstance.myLabel_TransformZBasis = myLabel_TransformZBasis;

            if(myIntUpDown_Middle2.Value != -1)
            {
                UIDocument uidoc = toavoidloadingrevitdlls.commandData.Application.ActiveUIDocument;
                Document doc = uidoc.Document;

                ReferencePoint myRefPoint = doc.GetElement(new ElementId(myIntUpDown_Middle2.Value.Value)) as ReferencePoint;

                ////if(myRefPoint == null)
                ////{
                ////    myIntUpDown_Middle2.Value = -1;
                ////}

                if (myRefPoint.get_Parameter(BuiltInParameter.POINT_ELEMENT_DRIVEN).AsInteger() == 1)
                {
                    myButton_HomeY.IsEnabled = false;
                    myButton_NinetyZ.IsEnabled = false;
                    myButton_NinetyZ_Reverse.IsEnabled = false;
                    myButton_HomeZ.IsEnabled = false;
                    myButton_NinetyY.IsEnabled = false;
                    myButton_NinetyY_Reverse.IsEnabled = false;

                    mySlider_Rotate_BasisX.IsEnabled = false;
                    mySlider_Rotate_BasisY.IsEnabled = false;
                    button_Unhost.Background = Brushes.Yellow;
                    button_Unhost.IsEnabled = true;
                }
                else
                {
                    myButton_HomeY.IsEnabled = true;
                    myButton_NinetyZ.IsEnabled = true;
                    myButton_NinetyZ_Reverse.IsEnabled = true;
                    myButton_HomeZ.IsEnabled = true;
                    myButton_NinetyY.IsEnabled = true;
                    myButton_NinetyY_Reverse.IsEnabled = true;

                    mySlider_Rotate_BasisX.IsEnabled = true;
                    mySlider_Rotate_BasisY.IsEnabled = true;
                    button_Unhost.Background = null;
                    button_Unhost.IsEnabled = false;
                }

                setSliderClassInstance.setSlider(myRefPoint, mySlider_Rotate_BasisZ, false);
                setSliderClassInstance.setSlider(myRefPoint, mySlider_Rotate_BasisX, false);
                setSliderClassInstance.setSlider(myRefPoint, mySlider_Rotate_BasisY, true);
            }

            myEE03_RotateAroundBasis = new EE03_RotateAroundBasis();
            myEE03_RotateAroundBasis.myIntUpDown_A = null;
            myEE03_RotateAroundBasis.myIntUpDown_B = null;
            myEE03_RotateAroundBasis.mySlider_Interpolate = null;
            myEE03_RotateAroundBasis.myIntUpDown_Middle2 = myIntUpDown_Middle2;
            myEE03_RotateAroundBasis.setSliderClassInstance = setSliderClassInstance;
            myEE03_RotateAroundBasis.myLabel_ChangeCount = myLabel_ChangeCount;
            myEE03_RotateAroundBasis.myLabel_Setting = myLabel_Setting;
            myEE03_RotateAroundBasis.mySlider_Rotate_BasisZ = mySlider_Rotate_BasisZ;
            myEE03_RotateAroundBasis.mySlider_Rotate_BasisX = mySlider_Rotate_BasisX;
            myEE03_RotateAroundBasis.mySlider_Rotate_BasisY = mySlider_Rotate_BasisY;
            myExternalEvent_EE03_RotateAroundBasis = ExternalEvent.Create(myEE03_RotateAroundBasis);


            myEE04_ResetPosition = new EE04_ResetPosition();
            myEE04_ResetPosition.mySlider_Rotate_BasisZ = mySlider_Rotate_BasisZ;
            myEE04_ResetPosition.mySlider_Rotate_BasisX = mySlider_Rotate_BasisX;
            myEE04_ResetPosition.mySlider_Rotate_BasisY = mySlider_Rotate_BasisY;
            myEE04_ResetPosition.setSliderClassInstance = setSliderClassInstance;
            myExternalEvent_EE04_ResetPosition = ExternalEvent.Create(myEE04_ResetPosition);

            myEE05_Move = new EE05_Move();
            myEE05_Move.setSliderClassInstance = setSliderClassInstance;
            myEE05_Move.myLabel_ChangeCount = myLabel_ChangeCount;
            myEE05_Move.myLabel_Setting = myLabel_Setting;
            myEE05_Move.mySlider_Rotate_BasisZ = mySlider_Rotate_BasisZ;
            myEE05_Move.mySlider_Rotate_BasisX = mySlider_Rotate_BasisX;
            myEE05_Move.mySlider_Rotate_BasisY = mySlider_Rotate_BasisY;
            myExternalEvent_EE05_Move = ExternalEvent.Create(myEE05_Move);

            myEE06_PlaceAFamily_OnDoubleClick = new EE06_PlaceAFamily_OnDoubleClick();
            myEE06_PlaceAFamily_OnDoubleClick.myWindow1 = this;
            myExternalEvent_EE06_PlaceAFamily_OnDoubleClick = ExternalEvent.Create(myEE06_PlaceAFamily_OnDoubleClick);

            myEE06_PlaceAFamily_CentreScreen = new EE06_PlaceAFamily_CentreScreen();
            myEE06_PlaceAFamily_CentreScreen.myWindow1 = this;
            myExternalEvent_EE06_PlaceAFamily_CentreScreen = ExternalEvent.Create(myEE06_PlaceAFamily_CentreScreen);

            myEE07_Unhost = new EE07_Unhost();
            myEE07_Unhost.myWindow1 = this;
            myExternalEvent_EE07_Unhost = ExternalEvent.Create(myEE07_Unhost);

            myEE08_LoadFamily = new EE08_LoadFamily();
            myEE08_LoadFamily.toavoidloadingrevitdlls = toavoidloadingrevitdlls;
            myExternalEvent_EE08_LoadFamily = ExternalEvent.Create(myEE08_LoadFamily);

            myEE09_RotateBy90 = new EE09_RotateBy90();
            myEE09_RotateBy90.myWindow1 = this;
            myExternalEvent_EE09_RotateBy90 = ExternalEvent.Create(myEE09_RotateBy90);

        }

        private void MyButton_HomeX_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                XYZ BasisX = new XYZ(1.0, 0.0, 0.0);
                XYZ BasisY = new XYZ(0.0, 1.0, 0.0);
                XYZ BasisZ = new XYZ(0.0, 0.0, 1.0);

                homingTheChair(BasisX, BasisY, BasisZ);
            }
            #region catch and finally

            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary2.DatabaseMethods.writeDebug("MyButton_HomeX_Click" + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        private void homingTheChair(XYZ BasisX, XYZ BasisY, XYZ BasisZ)
        {
            try
            {
                UIDocument uidoc = toavoidloadingrevitdlls.commandData.Application.ActiveUIDocument;
                Document doc = uidoc.Document;

                if (!SelectMethod.mySelectMethod(uidoc, myIntUpDown_Middle2)) return;


                ReferencePoint myReferencePoint_Centre = doc.GetElement(new ElementId(myIntUpDown_Middle2.Value.Value)) as ReferencePoint;
                Transform myTransform = myReferencePoint_Centre.GetCoordinateSystem();


                myEE04_ResetPosition.myTransform_Temp = Transform.Identity;
                myEE04_ResetPosition.myTransform_Temp.Origin = myTransform.Origin;


                myEE04_ResetPosition.myTransform_Temp.BasisX = BasisX;
                myEE04_ResetPosition.myTransform_Temp.BasisY = BasisY;
                myEE04_ResetPosition.myTransform_Temp.BasisZ = BasisZ;

                myEE04_ResetPosition.myToolKit_IntUpDown = myIntUpDown_Middle2;
                myExternalEvent_EE04_ResetPosition.Raise();
            }
            #region catch and finally

            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary2.DatabaseMethods.writeDebug("MyButton_HomeX_Click" + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        private void MyButton_HomeY_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                XYZ BasisX = new XYZ(1.0, 0.0, 0.0);
                XYZ BasisY = new XYZ(0.0, 0.0, 1.0);
                XYZ BasisZ = new XYZ(0.0, -1.0, 0.0);

                homingTheChair(BasisX, BasisY, BasisZ);
            }
            #region catch and finally

            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary2.DatabaseMethods.writeDebug("MyButton_HomeY_Click" + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        private void MyButton_HomeZ_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                XYZ BasisX = new XYZ(0.0, 1.0, 0.0);
                XYZ BasisY = new XYZ(0.0, 0.0, 1.0);
                XYZ BasisZ = new XYZ(1.0, 0.0, 0.0);

                homingTheChair(BasisX, BasisY, BasisZ);
            }
            #region catch and finally

            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary2.DatabaseMethods.writeDebug("MyButton_HomeZ_Click" + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        private void MyButton_NinetyZ_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                myEE09_RotateBy90.bool_Clockwise = true;
                myEE09_RotateBy90.xyz_Basis = (int)xyzThree.XYZ_X;
                myExternalEvent_EE09_RotateBy90.Raise();
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary2.DatabaseMethods.writeDebug("MyButton_NinetyZ_Click" + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        private void MyButton_NinetyZ_Reverse_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                myEE09_RotateBy90.bool_Clockwise = false;
                myEE09_RotateBy90.xyz_Basis = (int)xyzThree.XYZ_X;
                myExternalEvent_EE09_RotateBy90.Raise();
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary2.DatabaseMethods.writeDebug("MyButton_NinetyZ_Reverse_Click" + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        private void MyButton_NinetyY_Reverse_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                myEE09_RotateBy90.bool_Clockwise = true;
                myEE09_RotateBy90.xyz_Basis = (int)xyzThree.XYZ_Y;
                myExternalEvent_EE09_RotateBy90.Raise();
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary2.DatabaseMethods.writeDebug("MyButton_NinetyY_Reverse_Click" + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        private void MyButton_NinetyY_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                myEE09_RotateBy90.bool_Clockwise = false;
                myEE09_RotateBy90.xyz_Basis = (int)xyzThree.XYZ_Y;
                myExternalEvent_EE09_RotateBy90.Raise();
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary2.DatabaseMethods.writeDebug("MyButton_NinetyY_Click" + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }
        private void MyButton_NinetyX_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                myEE09_RotateBy90.bool_Clockwise = true;
                myEE09_RotateBy90.xyz_Basis = (int)xyzThree.XYZ_Z;
                myExternalEvent_EE09_RotateBy90.Raise();
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary2.DatabaseMethods.writeDebug("MyButton_NinetyX_Click" + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }
        private void MyButton_NinetyX_Reverse_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                myEE09_RotateBy90.bool_Clockwise = false;
                myEE09_RotateBy90.xyz_Basis = (int)xyzThree.XYZ_Z;
                myExternalEvent_EE09_RotateBy90.Raise();
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary2.DatabaseMethods.writeDebug("MyButton_NinetyX_Reverse_Click" + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }
        private void mySlider_Move_X_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            try
            {
                myLabel_Progress.Content = "Not in Progress";
                myEE05_Move.mySlideInProgress = false;
                if (myBool_Rezero) { mySlider_Move_X.Value = 0; myBool_Rezero = false; }
            }
            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary2.DatabaseMethods.writeDebug("mySlider_Move_X_DragCompleted" + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        private void mySlider_Move_Y_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            try
            {
                myLabel_Progress.Content = "Not in Progress";
                myEE05_Move.mySlideInProgress = false;
                if (myBool_Rezero) { mySlider_Move_Y.Value = 0; myBool_Rezero = false; }
            }
            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary2.DatabaseMethods.writeDebug("mySlider_Move_Y_DragCompleted" + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        private void mySlider_Move_Z_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            try
            {
                myLabel_Progress.Content = "Not in Progress";
                myEE05_Move.mySlideInProgress = false;
                if (myBool_Rezero) { mySlider_Move_Z.Value = 0; myBool_Rezero = false; }
            }
            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary2.DatabaseMethods.writeDebug("mySlider_Move_Z_DragCompleted" + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        private void mySlider_DragStartedCommon(Slider currentSlider)
        {
            try
            {
                UIDocument uidoc = toavoidloadingrevitdlls.commandData.Application.ActiveUIDocument;
                Document doc = uidoc.Document;

                ReferencePoint myReferencePoint_middle = doc.GetElement(new ElementId(myIntUpDown_Middle2.Value.Value)) as ReferencePoint;

                if (myReferencePoint_middle == null)
                {
                    myBool_Rezero = true;
                    myIntUpDown_Middle2.Value = -1;
                    MessageBox.Show("Please 'pick' or 'place' platform 'AGM'");
                    return;
                }

                myLabel_Progress.Content = "In Progress";

                myEE05_Move.mySlideInProgress = true;
                myEE05_Move.mySlider = currentSlider;
                myEE05_Move.myToolKit_IntUpDown = myIntUpDown_Middle2;
                myExternalEvent_EE05_Move.Raise();

            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary2.DatabaseMethods.writeDebug("mySlider_DragStartedCommon" + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        private void mySlider_Move_X_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            mySlider_DragStartedCommon(mySlider_Move_X);
        }

        private void mySlider_Move_Y_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            mySlider_DragStartedCommon(mySlider_Move_Y);
        }

        private void mySlider_Move_Z_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            mySlider_DragStartedCommon(mySlider_Move_Z);
        }

        private void mySlider_RotatedStartedCommon(Slider currentSlider, bool xoRy_Basis)
        {
            try
            {
                UIDocument uidoc = toavoidloadingrevitdlls.commandData.Application.ActiveUIDocument;
                Document doc = uidoc.Document;

                ReferencePoint myReferencePoint_middle = doc.GetElement(new ElementId(myIntUpDown_Middle2.Value.Value)) as ReferencePoint;

                if (myReferencePoint_middle == null)
                {
                    myIntUpDown_Middle2.Value = -1;
                    myBool_Rezero = true;
                    MessageBox.Show("Please create a new 'Platform'.");
                    return;
                }

                if(xoRy_Basis)
                {
                    if (myReferencePoint_middle.get_Parameter(BuiltInParameter.POINT_ELEMENT_DRIVEN).AsInteger() == 1)
                    {
                        myButton_HomeY.IsEnabled = false;
                        myButton_NinetyZ.IsEnabled = false;
                        myButton_NinetyZ_Reverse.IsEnabled = false;
                        myButton_HomeZ.IsEnabled = false;
                        myButton_NinetyY.IsEnabled = false;
                        myButton_NinetyY_Reverse.IsEnabled = false;

                        mySlider_Rotate_BasisX.IsEnabled = false;
                        mySlider_Rotate_BasisY.IsEnabled = false;
                        button_Unhost.Background = Brushes.Yellow;
                        button_Unhost.IsEnabled = true;
                    }
                    else
                    {
                        myButton_HomeY.IsEnabled = true;
                        myButton_NinetyZ.IsEnabled = true;
                        myButton_NinetyZ_Reverse.IsEnabled = true;
                        myButton_HomeZ.IsEnabled = true;
                        myButton_NinetyY.IsEnabled = true;
                        myButton_NinetyY_Reverse.IsEnabled = true;

                        mySlider_Rotate_BasisX.IsEnabled = true;
                        mySlider_Rotate_BasisY.IsEnabled = true;
                        button_Unhost.Background = null;
                        button_Unhost.IsEnabled = false;
                    }
                }

                myLabel_Progress.Content = "In Progress";

                myEE03_RotateAroundBasis.myBool_InterpolateMiddle_WhenEitherA_or_B = false;
                myEE03_RotateAroundBasis.mySlideInProgress = true;
                myEE03_RotateAroundBasis.mySlider = currentSlider;
                myEE03_RotateAroundBasis.myToolKit_IntUpDown = myIntUpDown_Middle2;
                myExternalEvent_EE03_RotateAroundBasis.Raise();
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary2.DatabaseMethods.writeDebug("mySlider_RotatedStartedCommon" + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        private void mySlider_Rotate_BasisX_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            try
            {
                mySlider_RotatedStartedCommon(mySlider_Rotate_BasisX, true);
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary2.DatabaseMethods.writeDebug("mySlider_Move_X_DragStarted" + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        private void mySlider_Rotate_BasisY_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            try
            {
                mySlider_RotatedStartedCommon(mySlider_Rotate_BasisY, true);
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary2.DatabaseMethods.writeDebug("mySlider_Rotate_BasisY_DragStarted" + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        private void mySlider_Rotate_BasisZ_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            try
            {
                mySlider_RotatedStartedCommon(mySlider_Rotate_BasisZ, false);
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary2.DatabaseMethods.writeDebug("mySlider_Rotate_BasisZ_DragStarted" + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        private void mySlider_Rotate_BasisZ_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            try
            {
                myLabel_Progress.Content = "Not in Progress";
                myEE03_RotateAroundBasis.mySlideInProgress = false;
                if (myBool_Rezero) { mySlider_Rotate_BasisZ.Value = 12; myBool_Rezero = false; }
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary2.DatabaseMethods.writeDebug("mySlider_Rotate_BasisZ_DragCompleted" + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        private void mySlider_Rotate_BasisY_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            try
            {
                myLabel_Progress.Content = "Not in Progress";
                myEE03_RotateAroundBasis.mySlideInProgress = false;
                if (myBool_Rezero) { mySlider_Rotate_BasisY.Value = 12; myBool_Rezero = false; }
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary2.DatabaseMethods.writeDebug("mySlider_Rotate_BasisY_DragCompleted" + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        private void mySlider_Rotate_BasisX_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            try
            {
                myLabel_Progress.Content = "Not in Progress";
                myEE03_RotateAroundBasis.mySlideInProgress = false;
                if (myBool_Rezero) { mySlider_Rotate_BasisX.Value = 12; myBool_Rezero = false; }
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary2.DatabaseMethods.writeDebug("mySlider_Rotate_BasisX_DragCompleted" + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        private void myWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                pkRevitUnderstandingTransforms.Properties.Settings.Default.LastSelectedID = myIntUpDown_Middle2.Value.Value;
                pkRevitUnderstandingTransforms.Properties.Settings.Default.Save();
                pkRevitUnderstandingTransforms.Properties.Settings.Default.Reload();
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary2.DatabaseMethods.writeDebug("myWindow_Closing" + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

    }
}
