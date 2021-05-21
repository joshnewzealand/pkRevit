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
using pkRevitUnderstandingTransforms.External_Events;

namespace RevitTransformSliders
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    /// 
    class FamilyOption : IFamilyLoadOptions
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

    public partial class Window_UnderstandingTransforms : Window
    {

        public Xceed.Wpf.Toolkit.IntegerUpDown myToolKit_IntUpDown { get; set; }
        public Slider mySlider { get; set; }

        // public ReferencePoint myReferencePoint1 { get; set; }
        public List<Transform> myListTransform { get; set; } = new List<Transform>();
        
        public bool mySlideInProgress = false;

        public EE01_Interpolate myEE01_Part1_Interpolate { get; set; }
        public ExternalEvent myExternalEvent_EE01_Part1_Interpolate { get; set; }

        public EE05_Move myEE05_Move { get; set; }
        public ExternalEvent myExternalEvent_EE05_Move { get; set; }

        public EE03_RotateAroundBasis myEE03_RotateAroundBasis { get; set; }
        public ExternalEvent myExternalEvent_EE03_RotateAroundBasis { get; set; }

        public EE04_ResetPosition myEE04_ResetPosition { get; set; }
        public ExternalEvent myExternalEvent_EE04_ResetPosition { get; set; }

        public EE06_LoadFamily myEE06_LoadFamily { get; set; }
        public ExternalEvent myExternalEvent_EE06_LoadFamily { get; set; }

        public EE06_PlaceFamily myEE06_PlaceFamily { get; set; }
        public ExternalEvent myExternalEvent_EE06_PlaceFamily { get; set; }

        ////myEE06_PlaceFamily = new EE06_PlaceFamily();
        ////myEE06_PlaceFamily.myWindow1 = this;
        ////    myEE06_PlaceFamily.mySlider_Rotate_BasisZ = mySlider_Rotate_BasisZ;
        ////    myEE06_PlaceFamily.mySlider_Rotate_BasisX = mySlider_Rotate_BasisX;
        ////    myEE06_PlaceFamily.mySlider_Rotate_BasisY = mySlider_Rotate_BasisY;
        ////    myEE06_PlaceFamily.setSliderClassInstance = setSliderClassInstance;
        ////    myExternalEvent_EE06_PlaceFamily = ExternalEvent.Create(myEE06_PlaceFamily);

        public string messageConst { get; set; }
        public ExternalCommandData commandData { get; set; }

        public pkRevitUnderstandingTransforms.External_Events.SetSlider setSliderClassInstance { get; set; }

        public Window_UnderstandingTransforms(ExternalCommandData cD, string message)
        {
            //myThisApplication = tA;
            messageConst = message;
            commandData = cD;

            InitializeComponent();

            setSliderClassInstance = new pkRevitUnderstandingTransforms.External_Events.SetSlider();
            setSliderClassInstance.commandData = commandData;
            setSliderClassInstance.myLabel_TransformOrigin = myLabel_TransformOrigin;
            setSliderClassInstance.myLabel_TransformXBasis = myLabel_TransformXBasis;
            setSliderClassInstance.myLabel_TransformYBasis = myLabel_TransformYBasis;
            setSliderClassInstance.myLabel_TransformZBasis = myLabel_TransformZBasis;


            // add 'UIDocument uid' as a parameter above, because this is the way it is called form the external event, please see youve 5 Secrets of Revit API Coding for an explaination on this

            this.Top = pkRevitUnderstandingTransforms.Properties.Settings.Default.Top;
            this.Left = pkRevitUnderstandingTransforms.Properties.Settings.Default.Left;
            this.Height = pkRevitUnderstandingTransforms.Properties.Settings.Default.Height;
            this.Width = pkRevitUnderstandingTransforms.Properties.Settings.Default.Width;
            myIntUpDown_Middle2.Value = pkRevitUnderstandingTransforms.Properties.Settings.Default.LastMiddle;
            myIntUpDown_A.Value = pkRevitUnderstandingTransforms.Properties.Settings.Default.LastA;
            myIntUpDown_B.Value = pkRevitUnderstandingTransforms.Properties.Settings.Default.LastB;
            //myIntUpDown_Middle2.Value = Properties.Settings.Default.LastRotate;
            //myIntUpDown_Middle2.Value = Properties.Settings.Default.LastMove;

            //setSlider();

            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            myEE01_Part1_Interpolate = new EE01_Interpolate();
            myEE01_Part1_Interpolate.myUpDown_CycleNumber = myUpDown_CycleNumber;
            myEE01_Part1_Interpolate.myIntUpDown_A = myIntUpDown_A;
            myEE01_Part1_Interpolate.myIntUpDown_B = myIntUpDown_B;
            //myEE01_Part1_Interpolate.mySlider = mySlider_Interpolate;
            myEE01_Part1_Interpolate.mySlider_Interpolate = mySlider_Interpolate;
            myEE01_Part1_Interpolate.myIntUpDown_Middle2 = myIntUpDown_Middle2;
            myEE01_Part1_Interpolate.setSliderClassInstance = setSliderClassInstance;
            myEE01_Part1_Interpolate.myLabel_ChangeCount = myLabel_ChangeCount;
            myEE01_Part1_Interpolate.myLabel_Setting = myLabel_Setting;
            myEE01_Part1_Interpolate.mySlider_Rotate_BasisZ = mySlider_Rotate_BasisZ;
            myEE01_Part1_Interpolate.mySlider_Rotate_BasisX = mySlider_Rotate_BasisX;
            myEE01_Part1_Interpolate.mySlider_Rotate_BasisY = mySlider_Rotate_BasisY;
            myExternalEvent_EE01_Part1_Interpolate = ExternalEvent.Create(myEE01_Part1_Interpolate);

            myEE05_Move = new EE05_Move();
            myEE05_Move.setSliderClassInstance = setSliderClassInstance;
            myEE05_Move.myLabel_ChangeCount = myLabel_ChangeCount;
            myEE05_Move.myLabel_Setting = myLabel_Setting;
            myEE05_Move.mySlider_Rotate_BasisZ = mySlider_Rotate_BasisZ;
            myEE05_Move.mySlider_Rotate_BasisX = mySlider_Rotate_BasisX;
            myEE05_Move.mySlider_Rotate_BasisY = mySlider_Rotate_BasisY;
            myExternalEvent_EE05_Move = ExternalEvent.Create(myEE05_Move);

            myEE03_RotateAroundBasis = new EE03_RotateAroundBasis();
            myEE03_RotateAroundBasis.myIntUpDown_A = myIntUpDown_A;
            myEE03_RotateAroundBasis.myIntUpDown_B = myIntUpDown_B;
            myEE03_RotateAroundBasis.mySlider_Interpolate = mySlider_Interpolate;
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

            myEE06_LoadFamily = new EE06_LoadFamily();
            myEE06_LoadFamily.messageConst = messageConst;
            myExternalEvent_EE06_LoadFamily = ExternalEvent.Create(myEE06_LoadFamily);

            myEE06_PlaceFamily = new EE06_PlaceFamily();
            myEE06_PlaceFamily.myWindow1 = this;
            myEE06_PlaceFamily.mySlider_Rotate_BasisZ = mySlider_Rotate_BasisZ;
            myEE06_PlaceFamily.mySlider_Rotate_BasisX = mySlider_Rotate_BasisX;
            myEE06_PlaceFamily.mySlider_Rotate_BasisY = mySlider_Rotate_BasisY;
            myEE06_PlaceFamily.setSliderClassInstance = setSliderClassInstance;
            myExternalEvent_EE06_PlaceFamily = ExternalEvent.Create(myEE06_PlaceFamily);
        }


        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            pkRevitUnderstandingTransforms.Properties.Settings.Default.Top = this.Top;
            pkRevitUnderstandingTransforms.Properties.Settings.Default.Left = this.Left;
            pkRevitUnderstandingTransforms.Properties.Settings.Default.Height = this.Height;
            pkRevitUnderstandingTransforms.Properties.Settings.Default.Width = this.Width;
            pkRevitUnderstandingTransforms.Properties.Settings.Default.LastMiddle = myIntUpDown_Middle2.Value.Value;
            pkRevitUnderstandingTransforms.Properties.Settings.Default.LastA = myIntUpDown_A.Value.Value;
            pkRevitUnderstandingTransforms.Properties.Settings.Default.LastB = myIntUpDown_B.Value.Value;
            //Properties.Settings.Default.LastRotate = myIntUpDown_Middle2.Value.Value;
            //Properties.Settings.Default.LastMove = myIntUpDown_Middle2.Value.Value;
            pkRevitUnderstandingTransforms.Properties.Settings.Default.Save();
        }



        
        /// TECHNIQUE 19 OF 19 (Window1.xaml.cs) (scroll down to project 'RevitTransformSliders')
        ///↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓ INTERPOLATION ↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓
        ///
        /// Interfaces and ENUM's:
        /// 
        /// 
        /// Demonstrates classes:
        ///     Numerics.Vector3
        /// 
        /// 
        /// Key methods:
        ///     Numerics.Vector3.Lerp(
        ///     
        ///     
        ///     
        /// Illustrates that it is possible to do animations in revit 3D view
        /// Usually you need an application like navisworks or 3DS max
        ///
        ///
        ///	https://github.com/joshnewzealand/Revit-API-Playpen-CSharp



        public static List<Transform> myMethod_whichTook_120Hours_OfCoding(Document doc, Xceed.Wpf.Toolkit.IntegerUpDown myIntUpDown_A, Xceed.Wpf.Toolkit.IntegerUpDown myIntUpDown_B)
        {
            List<Transform> myListTransform_Interpolate = new List<Transform>();

            ////UIDocument uidoc = commandData.Application.ActiveUIDocument;
            ////Document doc = uidoc.Document;

            FamilyInstance myFamilyInstance_1533 = doc.GetElement(new ElementId(myIntUpDown_A.Value.Value)) as FamilyInstance;
            FamilyInstance myFamilyInstance_2252 = doc.GetElement(new ElementId(myIntUpDown_B.Value.Value)) as FamilyInstance;

            ReferencePoint myReferencePoint2 = doc.GetElement(new ElementId(myIntUpDown_A.Value.Value)) as ReferencePoint;
            ReferencePoint myReferencePoint3 = doc.GetElement(new ElementId(myIntUpDown_B.Value.Value)) as ReferencePoint;

            if (myReferencePoint2 == null) myIntUpDown_A.Value = -1;
            if (myReferencePoint3 == null) myIntUpDown_B.Value = -1;
            if (myReferencePoint2 == null | myReferencePoint3 == null) return null;

            ////ReferencePoint myReferencePoint2 = doc.GetElement(AdaptiveComponentInstanceUtils.GetInstancePointElementRefIds(myFamilyInstance_1533).First()) as ReferencePoint;
            ////ReferencePoint myReferencePoint3 = doc.GetElement(AdaptiveComponentInstanceUtils.GetInstancePointElementRefIds(myFamilyInstance_2252).First()) as ReferencePoint;

            Transform myTransform_FakeBasis44444 = myReferencePoint2.GetCoordinateSystem();
            Transform myTransform_FakeBasis33333 = myReferencePoint3.GetCoordinateSystem();

            double myDouble_OneDivideTwelve = 1.0 / 24.0;

            List<float> myListfloat = new List<float>();

            myListfloat.Add((float)(myDouble_OneDivideTwelve * 1)); //13
            for (int i = 1; i <= 23; i++)
            {
                myListfloat.Add((float)(myDouble_OneDivideTwelve * i)); //12
            }
            myListfloat.Add((float)(myDouble_OneDivideTwelve * 23)); //13

            myTransform_FakeBasis44444.Origin = XYZ.Zero;
            myTransform_FakeBasis33333.Origin = XYZ.Zero;

            Transform myTransformFromQuatX;
            double aaMega_QuatX = XYZ.BasisZ.AngleTo(myTransform_FakeBasis33333.BasisZ);  //remember this is about vector not basis

            if (IsZero(aaMega_QuatX)) myTransformFromQuatX = Transform.Identity;
            else
            {
                XYZ axis = myTransform_FakeBasis33333.BasisZ.CrossProduct(-XYZ.BasisZ);
                myTransformFromQuatX = Transform.CreateRotationAtPoint(axis, aaMega_QuatX, XYZ.Zero); //step 1, extration
            }

            Transform myTransformFromQuatXX;
            double aaMega_QuatXX = XYZ.BasisZ.AngleTo(myTransform_FakeBasis44444.BasisZ);  //remember this is about vector not basis

            if (IsZero(aaMega_QuatXX)) myTransformFromQuatXX = Transform.Identity;
            else
            {
                XYZ axis = myTransform_FakeBasis44444.BasisZ.CrossProduct(-XYZ.BasisZ);
                myTransformFromQuatXX = Transform.CreateRotationAtPoint(axis, aaMega_QuatXX, XYZ.Zero); //step 1, extration
            }

            Transform myEndPointTransform = Transform.Identity;
            myEndPointTransform.BasisX = myTransformFromQuatXX.Inverse.OfVector(myTransform_FakeBasis44444.BasisX); //step 2, extracting rotation
            myEndPointTransform.BasisY = myTransformFromQuatXX.Inverse.OfVector(myTransform_FakeBasis44444.BasisY); //step 2, extracting rotation
            myEndPointTransform.BasisZ = myTransformFromQuatXX.Inverse.OfVector(myTransform_FakeBasis44444.BasisZ); //step 2, extracting rotation

            Transform myEndPointTransformFor333 = Transform.Identity;
            myEndPointTransformFor333.BasisX = myTransformFromQuatX.Inverse.OfVector(myTransform_FakeBasis33333.BasisX); //step 2, extracting rotation
            myEndPointTransformFor333.BasisY = myTransformFromQuatX.Inverse.OfVector(myTransform_FakeBasis33333.BasisY); //step 2, extracting rotation
            myEndPointTransformFor333.BasisZ = myTransformFromQuatX.Inverse.OfVector(myTransform_FakeBasis33333.BasisZ); //step 2, extracting rotation

            double angle = myEndPointTransform.BasisX.AngleOnPlaneTo(XYZ.BasisX, -XYZ.BasisZ); //step 3, extracting rotation turning into angle
            double d1_Final = myEndPointTransformFor333.BasisX.AngleOnPlaneTo(XYZ.BasisX, -XYZ.BasisZ); //step 3, extracting rotation turning into angle


            Numerics.Vector3 myQuat = new Numerics.Vector3();
            myQuat.X = (float)myTransform_FakeBasis33333.BasisZ.X; //step 3, combining z basis 
            myQuat.Y = (float)myTransform_FakeBasis33333.BasisZ.Y; //step 3, combining z basis 
            myQuat.Z = (float)myTransform_FakeBasis33333.BasisZ.Z; //step 3, combining z basis 

            Numerics.Vector3 myQuat2 = new Numerics.Vector3();
            myQuat2.X = (float)myTransform_FakeBasis44444.BasisZ.X; //step 3, combining z basis 
            myQuat2.Y = (float)myTransform_FakeBasis44444.BasisZ.Y; //step 3, combining z basis 
            myQuat2.Z = (float)myTransform_FakeBasis44444.BasisZ.Z; //step 3, combining z basis 

            foreach (float myFloat in myListfloat)
            {
                double myDoubleAlmostThere = (angle * (1 - myFloat)) + (d1_Final * (myFloat)); //step 2, z normal combining rotation

                Numerics.Vector3 myQuat3 = Numerics.Vector3.Lerp(myQuat2, myQuat, myFloat); //step 3, combining z basis 

                Transform myTransformFromQuat_Final = Transform.Identity;
                if (true)
                {
                    double aaMega_Quat = XYZ.BasisZ.AngleTo(new XYZ(myQuat3.X, myQuat3.Y, myQuat3.Z));  //step 3, turning it into a Transform

                    if (IsZero(aaMega_Quat)) myTransformFromQuat_Final = Transform.Identity;
                    else
                    {
                        XYZ axis = new XYZ(myQuat3.X, myQuat3.Y, myQuat3.Z).CrossProduct(-XYZ.BasisZ);
                        myTransformFromQuat_Final = Transform.CreateRotationAtPoint(axis, aaMega_Quat, XYZ.Zero);
                    }
                }

                Transform myEndPointTransform_JustOnceSide = Transform.CreateRotationAtPoint(myTransformFromQuat_Final.BasisZ, myDoubleAlmostThere, XYZ.Zero); //step 4, bringing it all together 

                Transform myTransformFromQuat_AllNewFinal = Transform.Identity;
                myTransformFromQuat_AllNewFinal.BasisX = myEndPointTransform_JustOnceSide.OfVector(myTransformFromQuat_Final.BasisX); //step 4, bringing it all together 
                myTransformFromQuat_AllNewFinal.BasisY = myEndPointTransform_JustOnceSide.OfVector(myTransformFromQuat_Final.BasisY); //step 4, bringing it all together
                myTransformFromQuat_AllNewFinal.BasisZ = myEndPointTransform_JustOnceSide.OfVector(myTransformFromQuat_Final.BasisZ); //step 4, bringing it all together

                Transform t = myTransformFromQuat_AllNewFinal;

                t.Origin = myReferencePoint3.Position + ((myReferencePoint2.Position - myReferencePoint3.Position) * (1 - myFloat));

                myListTransform_Interpolate.Add(t);
            }

            return myListTransform_Interpolate;
        }

        public static bool IsZero(double a, double tolerance)
        {
            return tolerance > Math.Abs(a);
        }
        const double _eps = 1.0e-9;
        public static bool IsZero(double a)
        {
            return IsZero(a, _eps);
        }


        private void mySliderRotate_AGM_B_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            try
            {

                myLabel_Progress.Content = "Not in Progress";

                mySlideInProgress = false;
                myEE03_RotateAroundBasis.mySlideInProgress = false;

                if (myBool_Rezero) { mySliderRotate_AGM_B.Value = 12; myBool_Rezero = false; }
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary2.DatabaseMethods.writeDebug("mySlider_Rotate_B_DragCompleted" + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }
        private void mySliderRotate_AGM_A_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            try
            {
                myLabel_Progress.Content = "Not in Progress";

                mySlideInProgress = false;
                myEE03_RotateAroundBasis.mySlideInProgress = false;

                if (myBool_Rezero) { mySliderRotate_AGM_A.Value = 12; myBool_Rezero = false; }
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary2.DatabaseMethods.writeDebug("MySlider_DragCompleted_RotateX" + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion 
        }

        private void mySliderRotate_AGM_B_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            try
            {
                mySlider = mySliderRotate_AGM_B;
                myToolKit_IntUpDown = myIntUpDown_B;

                if (true) //candidate for methodisation 202005232228
                {
                    if (myToolKit_IntUpDown.Value.Value == 0) return;

                    UIDocument uidoc = commandData.Application.ActiveUIDocument;
                    Document doc = uidoc.Document;

                    if (doc.GetElement(new ElementId(myToolKit_IntUpDown.Value.Value)) == null)
                    {
                        myBool_Rezero = true;
                        return;
                    }

                    ////////myLabel_Progress.Content = "In Progress";

                    ////////mySlideInProgress = true;

                    ////////myEE03_RotateAroundBasis.myBool_InterpolateMiddle_WhenEitherA_or_B = true;
                    ////////myExternalEvent_EE03_RotateAroundBasis.Raise();

                    ReferencePoint myReferencePoint3 = doc.GetElement(new ElementId(myIntUpDown_A.Value.Value)) as ReferencePoint;

                    if (myReferencePoint3 == null)
                    {
                        myIntUpDown_A.Value = -1;
                        MessageBox.Show("Please place platform A");
                        return;
                    }

                    ReferencePoint myReferencePoint_middle = doc.GetElement(new ElementId(myIntUpDown_Middle2.Value.Value)) as ReferencePoint;

                    if (myReferencePoint_middle == null)
                    {
                        myIntUpDown_Middle2.Value = -1;
                        MessageBox.Show("Please 'pick' or 'place' platform 'AGM'");
                        return;
                    }

                    myLabel_Progress.Content = "In Progress";

                    mySlideInProgress = true;

                    myEE03_RotateAroundBasis.myBool_InterpolateMiddle_WhenEitherA_or_B = true;
                    myEE03_RotateAroundBasis.mySlideInProgress = true;
                    myEE03_RotateAroundBasis.mySlider = mySliderRotate_AGM_B;
                    myEE03_RotateAroundBasis.myToolKit_IntUpDown = myToolKit_IntUpDown;
                    myExternalEvent_EE03_RotateAroundBasis.Raise();
                }
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary2.DatabaseMethods.writeDebug("mySlider_Rotate_B_DragStarted" + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion   
        }

        private void mySliderRotate_AGM_A_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            try
            {
                mySlider = mySliderRotate_AGM_A; ////////////////tabletable//////////////////
                myToolKit_IntUpDown = myIntUpDown_A; ////////////////////tabletable//////////////

                if (true) //candidate for methodisation 202005232228
                {
                    if (myToolKit_IntUpDown.Value.Value == 0) return;

                    UIDocument uidoc = commandData.Application.ActiveUIDocument;
                    Document doc = uidoc.Document;

                    if (doc.GetElement(new ElementId(myToolKit_IntUpDown.Value.Value)) == null)
                    {
                        myBool_Rezero = true;
                        return;
                    }


                    ReferencePoint myReferencePoint3 = doc.GetElement(new ElementId(myIntUpDown_B.Value.Value)) as ReferencePoint;

                    if (myReferencePoint3 == null)
                    {
                        myIntUpDown_B.Value = -1;
                        MessageBox.Show("Please place platform B");
                        return;
                    }

                    ReferencePoint myReferencePoint_middle = doc.GetElement(new ElementId(myIntUpDown_Middle2.Value.Value)) as ReferencePoint;

                    if (myReferencePoint_middle == null)
                    {
                        myIntUpDown_Middle2.Value = -1;
                        MessageBox.Show("Please 'pick' or 'place' platform 'AGM'");
                        return;
                    }


                    myLabel_Progress.Content = "In Progress";

                    mySlideInProgress = true;

                    myEE03_RotateAroundBasis.myBool_InterpolateMiddle_WhenEitherA_or_B = true;
                    myEE03_RotateAroundBasis.mySlideInProgress = true;
                    myEE03_RotateAroundBasis.mySlider = mySliderRotate_AGM_A;
                    myEE03_RotateAroundBasis.myToolKit_IntUpDown = myToolKit_IntUpDown;
                    myExternalEvent_EE03_RotateAroundBasis.Raise();
                }
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary2.DatabaseMethods.writeDebug("MySlider_DragStarted_RotateX" + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion   
        }


        bool myBool_Rezero = false;

        private void mySlider_Rotate_BasisY_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            try
            {
                mySlider = mySlider_Rotate_BasisY;
                myToolKit_IntUpDown = myIntUpDown_Middle2;

                if (true) //candidate for methodisation 202005232228
                {
                    UIDocument uidoc = commandData.Application.ActiveUIDocument;
                    Document doc = uidoc.Document;

                    ReferencePoint myReferencePoint_middle = doc.GetElement(new ElementId(myIntUpDown_Middle2.Value.Value)) as ReferencePoint;

                    if (myReferencePoint_middle == null)
                    {
                        myIntUpDown_Middle2.Value = -1;
                        MessageBox.Show("Please 'pick' or 'place' platform 'AGM'");
                        return;
                    }

                    if (doc.GetElement(new ElementId(myToolKit_IntUpDown.Value.Value)) == null)
                    {
                        myBool_Rezero = true;
                        return;
                    }

                    myLabel_Progress.Content = "In Progress";

                    mySlideInProgress = true;

                    myEE03_RotateAroundBasis.myBool_InterpolateMiddle_WhenEitherA_or_B = false;
                    myEE03_RotateAroundBasis.mySlideInProgress = true;
                    myEE03_RotateAroundBasis.mySlider = mySlider_Rotate_BasisY;
                    myEE03_RotateAroundBasis.myToolKit_IntUpDown = myToolKit_IntUpDown;
                    myExternalEvent_EE03_RotateAroundBasis.Raise();
                }
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

        private void mySlider_Rotate_BasisX_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            try
            {
                mySlider = mySlider_Rotate_BasisX;
                myToolKit_IntUpDown = myIntUpDown_Middle2;

                if (true) //candidate for methodisation 202005232228
                {
                    UIDocument uidoc = commandData.Application.ActiveUIDocument;
                    Document doc = uidoc.Document;

                    ReferencePoint myReferencePoint_middle = doc.GetElement(new ElementId(myIntUpDown_Middle2.Value.Value)) as ReferencePoint;

                    if (myReferencePoint_middle == null)
                    {
                        myIntUpDown_Middle2.Value = -1;
                        MessageBox.Show("Please 'pick' or 'place' platform 'AGM'");
                        return;
                    }

                    if (doc.GetElement(new ElementId(myToolKit_IntUpDown.Value.Value)) == null)
                    {
                        myBool_Rezero = true;
                        return;
                    }

                    myLabel_Progress.Content = "In Progress";

                    mySlideInProgress = true;

                    myEE03_RotateAroundBasis.myBool_InterpolateMiddle_WhenEitherA_or_B = false;
                    myEE03_RotateAroundBasis.mySlideInProgress = true;
                    myEE03_RotateAroundBasis.mySlider = mySlider_Rotate_BasisX;
                    myEE03_RotateAroundBasis.myToolKit_IntUpDown = myToolKit_IntUpDown;
                    myExternalEvent_EE03_RotateAroundBasis.Raise();
                }
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary2.DatabaseMethods.writeDebug("mySlider_Rotate_BasisX_DragStarted" + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
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
                mySlider = mySlider_Rotate_BasisZ;
                myToolKit_IntUpDown = myIntUpDown_Middle2;

                if (true) //candidate for methodisation 202005232228
                {
                    UIDocument uidoc = commandData.Application.ActiveUIDocument;
                    Document doc = uidoc.Document;

                    ReferencePoint myReferencePoint_middle = doc.GetElement(new ElementId(myIntUpDown_Middle2.Value.Value)) as ReferencePoint;

                    if (myReferencePoint_middle == null)
                    {
                        myIntUpDown_Middle2.Value = -1;
                        MessageBox.Show("Please 'pick' or 'place' platform 'AGM'");
                        return;
                    }

                    if (doc.GetElement(new ElementId(myToolKit_IntUpDown.Value.Value)) == null)
                    {
                        myBool_Rezero = true;
                        return;
                    }

                    myLabel_Progress.Content = "In Progress";

                    mySlideInProgress = true;


                    myEE03_RotateAroundBasis.myBool_InterpolateMiddle_WhenEitherA_or_B = false;
                    myEE03_RotateAroundBasis.mySlideInProgress = true;
                    myEE03_RotateAroundBasis.mySlider = mySlider_Rotate_BasisZ;
                    myEE03_RotateAroundBasis.myToolKit_IntUpDown = myToolKit_IntUpDown;
                    myExternalEvent_EE03_RotateAroundBasis.Raise();
                }
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


        private void mySlider_Interpolate_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            try
            {
                if (true)  //candidate for methodisataion 202006061646
                {

                    if (myIntUpDown_Middle2.Value.Value == -1)
                    {
                        MessageBox.Show("Set or Place AGM first please.");
                        return;
                    }

                    if (myIntUpDown_A.Value.Value == -1)
                    {
                        MessageBox.Show("Set or Place AGM-A first please.");
                        return;
                    }

                    if (myIntUpDown_B.Value.Value == -1)
                    {
                        MessageBox.Show("Set or Place AGM-B first please.");
                        return;
                    }

                    UIDocument uidoc = commandData.Application.ActiveUIDocument;
                    Document doc = uidoc.Document;

                    if (doc.GetElement(new ElementId(myIntUpDown_Middle2.Value.Value)) == null)
                    {
                        myBool_Rezero = true;
                        return;
                    }

                    //myMethod_whichTook_120Hours_OfCoding();

                    myLabel_Progress.Content = "In Progress";
                    mySlideInProgress = true;
                    myEE01_Part1_Interpolate.myBool_Cycle = false;
                    myEE01_Part1_Interpolate.mySlideInProgress = true;
                    myExternalEvent_EE01_Part1_Interpolate.Raise();

                }
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary2.DatabaseMethods.writeDebug("mySlider_Interpolate_DragStarted" + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        private void mySlider_Interpolate_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            try
            {
                if (true) //candidate for methodisation 202006061716
                {
                    myLabel_Progress.Content = "Not in Progress";

                    mySlideInProgress = false;
                    myEE01_Part1_Interpolate.mySlideInProgress = false;

                    if (myBool_Rezero) { mySlider_Interpolate.Value = 12; myBool_Rezero = false; }
                }
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary2.DatabaseMethods.writeDebug("mySlider_Interpolate_DragCompleted" + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }


        private void myButton_PickMiddle_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                UIDocument uidoc = commandData.Application.ActiveUIDocument;
                Document doc = uidoc.Document;

                if (!SelectMethod.mySelectMethod(uidoc, myIntUpDown_Middle2)) return;

                ReferencePoint myRefPoint = doc.GetElement(new ElementId(myIntUpDown_Middle2.Value.Value)) as ReferencePoint;
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary2.DatabaseMethods.writeDebug("myButton_PickMiddle_Click" + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        private void myButton_PickA_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                UIDocument uidoc = commandData.Application.ActiveUIDocument;
                Document doc = uidoc.Document;

                if (!SelectMethod.mySelectMethod(uidoc, myIntUpDown_A)) return;


                ReferencePoint myRefPoint = (ReferencePoint)doc.GetElement(new ElementId(myIntUpDown_A.Value.Value));
                setSliderClassInstance.setSlider(myRefPoint, mySliderRotate_AGM_A, false);
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary2.DatabaseMethods.writeDebug("myButton_PickA_Click" + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        private void myButton_PickB_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                UIDocument uidoc = commandData.Application.ActiveUIDocument;
                Document doc = uidoc.Document;


                if (!SelectMethod.mySelectMethod(uidoc, myIntUpDown_B)) return;


                ReferencePoint myRefPoint = (ReferencePoint)doc.GetElement(new ElementId(myIntUpDown_B.Value.Value));
                setSliderClassInstance.setSlider(myRefPoint, mySliderRotate_AGM_B, false);
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary2.DatabaseMethods.writeDebug("myButton_PickB_Click" + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        private ReferencePoint returnNullIfBad(Document doc, int anInteger)
        {
            if (anInteger == -1) return null;

            ElementId elementID = new ElementId(anInteger);

            Element element = doc.GetElement(elementID);

            if (element == null) return null;

            if (element.GetType() != typeof(ReferencePoint)) return null;

            return element as ReferencePoint;
        }


        private void myWindow_Loaded(object sender, RoutedEventArgs e)
        {
            int eL = -1;
            try
            {
                UIDocument uidoc = commandData.Application.ActiveUIDocument;
                Document doc = uidoc.Document;

                bool myBool_RunFamilyLoadEvent = false;

                string myString_TempPath = "";


                if (true)
                {
                    string stringAGM_FileName = "PRL-GM Adaptive Carrier Youtube";
                    List<Element> myListFamilySymbol_1738 = new FilteredElementCollector(doc).WherePasses(new ElementClassFilter(typeof(Family))).Where(x => x.Name == stringAGM_FileName).ToList();
                    if (myListFamilySymbol_1738.Count != 1) myBool_RunFamilyLoadEvent = true;
                }

                if (true)
                {
                    string stringChair_FileName = "PRL-GM Chair with always vertical OFF";
                    List<Element> myListFamilySymbol_1738 = new FilteredElementCollector(doc).WherePasses(new ElementClassFilter(typeof(Family))).Where(x => x.Name == stringChair_FileName).ToList();
                    if (myListFamilySymbol_1738.Count != 1) myBool_RunFamilyLoadEvent = true;
                }

                eL = 855;

                if (myBool_RunFamilyLoadEvent) myExternalEvent_EE06_LoadFamily.Raise();

                eL = 859;
                ///the below is bad for reasons, 

                //myRadioButton_PlatformMiddle.IsChecked = true;
                if (returnNullIfBad(doc, myIntUpDown_A.Value.Value) != null)
                {
                    setSliderClassInstance.setSlider((ReferencePoint)doc.GetElement(new ElementId(myIntUpDown_A.Value.Value)), mySliderRotate_AGM_A, false);
                }
                else myIntUpDown_A.Value = -1;

                if (returnNullIfBad(doc, myIntUpDown_Middle2.Value.Value) != null)
                {
                    setSliderClassInstance.setSlider((ReferencePoint)doc.GetElement(new ElementId(myIntUpDown_Middle2.Value.Value)), mySlider_Interpolate, false);
                }
                else myIntUpDown_Middle2.Value = -1;

                if (returnNullIfBad(doc, myIntUpDown_B.Value.Value) != null)
                {
                    setSliderClassInstance.setSlider((ReferencePoint)doc.GetElement(new ElementId(myIntUpDown_B.Value.Value)), mySliderRotate_AGM_B, false);
                }
                else myIntUpDown_B.Value = -1;


                eL = 867;

                if (returnNullIfBad(doc, myIntUpDown_Middle2.Value.Value) != null)
                {
                    setSliderClassInstance.setSlider((ReferencePoint)doc.GetElement(new ElementId(myIntUpDown_Middle2.Value.Value)), mySlider_Rotate_BasisZ, false);
                    setSliderClassInstance.setSlider((ReferencePoint)doc.GetElement(new ElementId(myIntUpDown_Middle2.Value.Value)), mySlider_Rotate_BasisX, false);
                    setSliderClassInstance.setSlider((ReferencePoint)doc.GetElement(new ElementId(myIntUpDown_Middle2.Value.Value)), mySlider_Rotate_BasisY, true);
                } 

                //myMethod_whichTook_120Hours_OfCoding();

            }

            #region catch and finally
            catch (Exception ex)
            {

                _952_PRLoogleClassLibrary2.DatabaseMethods.writeDebug("myWindow_Loaded   " + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);

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

                mySlideInProgress = false;
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

                mySlideInProgress = false;
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
                mySlideInProgress = false;

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


        private void mySlider_Move_X_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            try
            {
                mySlider = mySlider_Move_X; /////////////tabletable///////////// i think one of these is an unnecessary equivalence (unnecessarily complex code)
                myToolKit_IntUpDown = myIntUpDown_Middle2; ///////////////tabletable/////////////

                if (true) //candidate for methodisation 202005232228
                {
                    UIDocument uidoc = commandData.Application.ActiveUIDocument;
                    Document doc = uidoc.Document;

                    ReferencePoint myReferencePoint_middle = doc.GetElement(new ElementId(myIntUpDown_Middle2.Value.Value)) as ReferencePoint;

                    if (myReferencePoint_middle == null)
                    {
                        myIntUpDown_Middle2.Value = -1;
                        MessageBox.Show("Please 'pick' or 'place' platform 'AGM'");
                        return;
                    }

                    if (doc.GetElement(new ElementId(myToolKit_IntUpDown.Value.Value)) == null)
                    {
                        myBool_Rezero = true;
                        return;
                    }

                    myLabel_Progress.Content = "In Progress";

                    myEE05_Move.mySlideInProgress = true;
                    myEE05_Move.mySlider = mySlider_Move_X;
                    myEE05_Move.myToolKit_IntUpDown = myToolKit_IntUpDown;
                    myExternalEvent_EE05_Move.Raise();
                }
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

        private void mySlider_Move_Y_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            try
            {
                mySlider = mySlider_Move_Y;
                myToolKit_IntUpDown = myIntUpDown_Middle2;

                if (true) //candidate for methodisation 202005232228
                {
                    UIDocument uidoc = commandData.Application.ActiveUIDocument;
                    Document doc = uidoc.Document;

                    ReferencePoint myReferencePoint_middle = doc.GetElement(new ElementId(myIntUpDown_Middle2.Value.Value)) as ReferencePoint;

                    if (myReferencePoint_middle == null)
                    {
                        myIntUpDown_Middle2.Value = -1;
                        MessageBox.Show("Please 'pick' or 'place' platform 'AGM'");
                        return;
                    }

                    if (doc.GetElement(new ElementId(myToolKit_IntUpDown.Value.Value)) == null)
                    {
                        myBool_Rezero = true;
                        return;
                    }

                    myLabel_Progress.Content = "In Progress";

                    myEE05_Move.mySlideInProgress = true;
                    myEE05_Move.mySlider = mySlider_Move_Y;
                    myEE05_Move.myToolKit_IntUpDown = myToolKit_IntUpDown;
                    myExternalEvent_EE05_Move.Raise();
                }
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary2.DatabaseMethods.writeDebug("mySlider_Move_Y_DragStarted" + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        private void mySlider_Move_Z_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            try
            {
                mySlider = mySlider_Move_Z;
                myToolKit_IntUpDown = myIntUpDown_Middle2;

                if (true) //candidate for methodisation 202005232228
                {
                    UIDocument uidoc = commandData.Application.ActiveUIDocument;
                    Document doc = uidoc.Document;

                    ReferencePoint myReferencePoint_middle = doc.GetElement(new ElementId(myIntUpDown_Middle2.Value.Value)) as ReferencePoint;

                    if (myReferencePoint_middle == null)
                    {
                        myIntUpDown_Middle2.Value = -1;
                        MessageBox.Show("Please 'pick' or 'place' platform 'AGM'");
                        return;
                    }

                    if (doc.GetElement(new ElementId(myToolKit_IntUpDown.Value.Value)) == null)
                    {
                        myBool_Rezero = true;
                        return;
                    }

                    myLabel_Progress.Content = "In Progress";

                    myEE05_Move.mySlideInProgress = true;
                    myEE05_Move.mySlider = mySlider_Move_Z;
                    myEE05_Move.myToolKit_IntUpDown = myIntUpDown_Middle2;
                    myExternalEvent_EE05_Move.Raise();
                }
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary2.DatabaseMethods.writeDebug("mySlider_Move_Z_DragStarted" + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
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

                //if (!myBool_Rezero) myExternalEvent_EE05_Move.Raise();

                myLabel_Progress.Content = "Not in Progress";

                mySlideInProgress = false;
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

                mySlideInProgress = false;
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

                mySlideInProgress = false;
                myEE05_Move.mySlideInProgress = false;

                if (myBool_Rezero) { mySlider_Move_Z.Value = 0; myBool_Rezero = false; }
                //mySlider_Move_Z.Value = 0;
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

        private void myButton_Reset_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                UIDocument uidoc = commandData.Application.ActiveUIDocument;
                Document doc = uidoc.Document;

                if (!SelectMethod.mySelectMethod(uidoc, myIntUpDown_Middle2)) return;

                myToolKit_IntUpDown = myIntUpDown_Middle2;

                myEE04_ResetPosition.myToolKit_IntUpDown = myIntUpDown_Middle2;
                myExternalEvent_EE04_ResetPosition.Raise();
            }
            #region catch and finally

            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary2.DatabaseMethods.writeDebug("myButton_Reset_Click" + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        private void myButton_PlaceCenterAGM_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                myEE06_PlaceFamily.myString_Family_Platform = "PRL-GM Adaptive Carrier Youtube";
                myEE06_PlaceFamily.myString_Type_Platform = "PRL-GM-2020 Adaptive Carrier Rope White";
                myEE06_PlaceFamily.myString_Family_Chair = "PRL-GM Chair with always vertical OFF";
                myEE06_PlaceFamily.myString_Type_Chair = "565 x 565 mm";
                myEE06_PlaceFamily.myBool_AlsoPlaceAChair = true;
                myEE06_PlaceFamily.myIntUPDown = myIntUpDown_Middle2;

                myExternalEvent_EE06_PlaceFamily.Raise();
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary2.DatabaseMethods.writeDebug("myButton_PlaceCenterAGM_Click" + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        private void myButton_PlaceAGM_A_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                try
                {


                    myEE06_PlaceFamily.myString_Family_Platform = "PRL-GM Adaptive Carrier Youtube";
                    myEE06_PlaceFamily.myString_Type_Platform = "PRL-GM-2020 Adaptive Carrier A";
                    myEE06_PlaceFamily.myBool_AlsoPlaceAChair = false;
                    myEE06_PlaceFamily.myIntUPDown = myIntUpDown_A;

                    myExternalEvent_EE06_PlaceFamily.Raise();
                }

                #region catch and finally
                catch (Exception ex)
                {
                    _952_PRLoogleClassLibrary2.DatabaseMethods.writeDebug("myButton_PlaceCenterAGM_Click" + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
                }
                finally
                {
                }
                #endregion
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary2.DatabaseMethods.writeDebug("myButton_PlaceAGM_A_Click" + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        private void myButton_PlaceAGM_B_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                try
                {
                    myEE06_PlaceFamily.myString_Family_Platform = "PRL-GM Adaptive Carrier Youtube";
                    myEE06_PlaceFamily.myString_Type_Platform = "PRL-GM-2020 Adaptive Carrier B";
                    myEE06_PlaceFamily.myBool_AlsoPlaceAChair = false;
                    myEE06_PlaceFamily.myIntUPDown = myIntUpDown_B;

                    myExternalEvent_EE06_PlaceFamily.Raise();
                }

                #region catch and finally
                catch (Exception ex)
                {
                    _952_PRLoogleClassLibrary2.DatabaseMethods.writeDebug("myButton_PlaceCenterAGM_Click" + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
                }
                finally
                {
                }
                #endregion
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary2.DatabaseMethods.writeDebug("myButton_PlaceAGM_B_Click" + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        private void MyButton_Cycle_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (true)  //candidate for methodisataion 202006061646
                {

                    if (myIntUpDown_Middle2.Value.Value == 0)
                    {
                        MessageBox.Show("Set or Place AGM first please.");
                        return;
                    }

                    if (myIntUpDown_A.Value.Value == 0)
                    {
                        MessageBox.Show("Set or Place AGM-A first please.");
                        return;
                    }

                    if (myIntUpDown_B.Value.Value == 0)
                    {
                        MessageBox.Show("Set or Place AGM-B first please.");
                        return;
                    }

                    UIDocument uidoc = commandData.Application.ActiveUIDocument;
                    Document doc = uidoc.Document;

                    if (doc.GetElement(new ElementId(myIntUpDown_Middle2.Value.Value)) == null)
                    {
                        myBool_Rezero = true;
                        return;
                    }

                   // myMethod_whichTook_120Hours_OfCoding();

                    myLabel_Progress.Content = "In Progress";
                    mySlideInProgress = true;
                    myEE01_Part1_Interpolate.myBool_Cycle = true;
                    myExternalEvent_EE01_Part1_Interpolate.Raise();
                }
            }
            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary2.DatabaseMethods.writeDebug("MyButton_Cycle_Click" + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }
    }
}
