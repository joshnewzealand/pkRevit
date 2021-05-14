using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.ExtensibleStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Media.Media3D;
using Numerics = System.Numerics;
using std = System.Math;
using _952_PRLoogleClassLibrary22;
using System.Text.RegularExpressions;
using pkRevitUnderstandingTransforms;

namespace RevitTransformSliders
{

    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class EE03_Template : IExternalEventHandler  //this is the last when one making a checklist change, EE4 must be just for when an element is new
    {
        public Window_UnderstandingTransforms myWindow1 { get; set; }

        public void Execute(UIApplication uiapp)
        {
            try
            {
                UIDocument uidoc = uiapp.ActiveUIDocument;
                Document doc = uidoc.Document; // myListView_ALL_Fam_Master.Items.Add(doc.GetElement(uidoc.Selection.GetElementIds().First()).Name);

                using (Transaction tx = new Transaction(doc))
                {
                    tx.Start("EE01_Part1_Template");

                    tx.Commit();
                }
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary2.DatabaseMethods.writeDebug("EE01_Part1_Template" + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }
        public string GetName()
        {
            return "EE01_Part1_Template";
        }
    }

    
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class EE03_RotateAroundBasis : IExternalEventHandler  //this is the last when one making a checklist change, EE4 must be just for when an element is new
    {
        public static bool IsZero(double a, double tolerance)
        {
            return tolerance > Math.Abs(a);
        }
        const double _eps = 1.0e-9;
        public static bool IsZero(double a)
        {
            return IsZero(a, _eps);
        }

        public System.Windows.Controls.Slider mySlider { get; set; }
        public System.Windows.Controls.Slider mySlider_Interpolate { get; set; }
        public Xceed.Wpf.Toolkit.IntegerUpDown myToolKit_IntUpDown { get; set; }
        public Xceed.Wpf.Toolkit.IntegerUpDown myIntUpDown_Middle2 { get; set; }
        public Xceed.Wpf.Toolkit.IntegerUpDown myIntUpDown_A { get; set; }
        public Xceed.Wpf.Toolkit.IntegerUpDown myIntUpDown_B { get; set; }

        public pkRevitUnderstandingTransforms.External_Events.SetSlider setSliderClassInstance { get; set; }

        public System.Windows.Controls.Label myLabel_ChangeCount { get; set; }
        public System.Windows.Controls.Label myLabel_Setting { get; set; }


        public System.Windows.Controls.Slider mySlider_Rotate_BasisZ { get; set; }
        public System.Windows.Controls.Slider mySlider_Rotate_BasisX { get; set; }
        public System.Windows.Controls.Slider mySlider_Rotate_BasisY { get; set; }


        //myLabel_Setting
        public bool mySlideInProgress { get; set; }

        public bool myBool_InterpolateMiddle_WhenEitherA_or_B { get; set; }

        public static void wait(int milliseconds)
        {
            System.Windows.Forms.Timer timer1 = new System.Windows.Forms.Timer();
            if (milliseconds == 0 || milliseconds < 0) return;
            timer1.Interval = milliseconds;
            timer1.Enabled = true;
            timer1.Start();
            timer1.Tick += (s, e) =>
            {
                timer1.Enabled = false;
                timer1.Stop();
            };
            while (timer1.Enabled)
            {
                Application.DoEvents();
            }
        }

        public void Execute(UIApplication uiapp)
        {
            int eL = -1;

            try
            {
                UIDocument uidoc = uiapp.ActiveUIDocument;
                Document doc = uidoc.Document; // myListView_ALL_Fam_Master.Items.Add(doc.GetElement(uidoc.Selection.GetElementIds().First()).Name);

                ///             TECHNIQUE 18 OF 19 (EE03_RotateAroundBasis.cs) (scroll down to project 'RevitTransformSliders')
                ///↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓ USING SLIDERS TO ROTATE A TRANSFORM ↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓
                ///
                /// Interfaces and ENUM's:
                /// 
                /// 
                /// Demonstrates classes:
                ///     
                /// 
                /// 
                /// Key methods:
                ///     XYZ.BasisZ.AngleTo(
                ///     myTransform.BasisZ.CrossProduct(
                ///     Transform.CreateRotationAtPoint(
                ///     
                ///
                ///
				///	https://github.com/joshnewzealand/Revit-API-Playpen-CSharp
               
                eL = 145;

                ReferencePoint myReferencePoint_Middle = null;
                     if (myBool_InterpolateMiddle_WhenEitherA_or_B) myReferencePoint_Middle = doc.GetElement(new ElementId(myIntUpDown_Middle2.Value.Value)) as ReferencePoint;

                eL = 151;
                
                ReferencePoint myReferencePoint_Centre = doc.GetElement(new ElementId(myToolKit_IntUpDown.Value.Value)) as ReferencePoint;

                Transform myTransform = myReferencePoint_Centre.GetCoordinateSystem();
                eL = 165;
                Transform myTransform_ToMakeTheRotationRelative;
                if (true)
                {
                    double myDouble_AngleToBasis = XYZ.BasisZ.AngleTo(myTransform.BasisZ);  //2 places line , 3 places method

                    switch (mySlider.Name)
                    {
                        case "mySlider_Rotate_BasisY":
                            myDouble_AngleToBasis = XYZ.BasisY.AngleTo(myTransform.BasisY);  //2 places line , 3 places method
                            break;
                        case "mySlider_Rotate_BasisX":
                            myDouble_AngleToBasis = XYZ.BasisX.AngleTo(myTransform.BasisX);  //2 places line , 3 places method
                            break;
                    }
                    eL = 180;
                    if (IsZero(myDouble_AngleToBasis))
                    {
                        eL = 181;
                        myTransform_ToMakeTheRotationRelative = Transform.Identity;
                    }
                    else if (IsZero(myDouble_AngleToBasis - Math.PI))
                    {
                        eL = 182;
                        XYZ axis = -XYZ.BasisZ;


                        myTransform_ToMakeTheRotationRelative = Transform.CreateRotationAtPoint(axis, myDouble_AngleToBasis, XYZ.Zero);
                    }
                    else
                    {
                        XYZ axis = myTransform.BasisZ.CrossProduct(-XYZ.BasisZ);  //2 places line , 3 places method,  normally (z) negative
                        eL = 185;
                        switch (mySlider.Name)
                        {
                            case "mySlider_Rotate_BasisY":
                                axis = myTransform.BasisY.CrossProduct(-XYZ.BasisY); //2 places line , 3 places method,  normally (z) negative
                                break;
                            case "mySlider_Rotate_BasisX":
                                axis = myTransform.BasisX.CrossProduct(-XYZ.BasisX); //2 places line , 3 places method,  normally (z) negative
                                break;
                        }
                        eL = 195;
                        myTransform_ToMakeTheRotationRelative = Transform.CreateRotationAtPoint(axis, myDouble_AngleToBasis, XYZ.Zero);
                        eL = 197;
                    }
                }



                eL = 192;


                int myIntTimeOut = 0;
                int myInt_ChangeCount = 0;
                double myDouble_ChangePosition = -1;
                eL = 212;
                using (TransactionGroup transGroup = new TransactionGroup(doc))
                {
                    transGroup.Start("Transform animation 2");
                    eL = 216;

                    double double_Angle2 = 0;

                    while (mySlideInProgress)
                    {
                        wait(100); myIntTimeOut++;

                        if (myDouble_ChangePosition != mySlider.Value)
                        {
                            myDouble_ChangePosition = mySlider.Value;
                            myLabel_ChangeCount.Content = myInt_ChangeCount++.ToString();

                                double myDoubleRotateAngle = ((Math.PI * 2) / 24) * myDouble_ChangePosition;

                                Transform myTransform_Rotate = Transform.CreateRotationAtPoint(XYZ.BasisZ, Math.PI + -myDoubleRotateAngle, XYZ.Zero); //1 places line and DOUBLE normally (z) negative , 3 places method

                                switch (mySlider.Name)
                                {
                                    case "mySlider_Rotate_BasisY":
                                        myTransform_Rotate = Transform.CreateRotationAtPoint(XYZ.BasisY, Math.PI + -myDoubleRotateAngle, XYZ.Zero); //1 places line and DOUBLE normally (z) negative , 3 places method
                                        //myBool_Perform_Unhost_And_Rehost = false;
                                        break;
                                    case "mySlider_Rotate_BasisX":
                                        myTransform_Rotate = Transform.CreateRotationAtPoint(XYZ.BasisX, Math.PI + -myDoubleRotateAngle, XYZ.Zero); //1 places line and DOUBLE normally (z) negative , 3 places method
                                        //myBool_Perform_Unhost_And_Rehost = false;
                                        break;
                                }


                                Transform myTransform_Temp = Transform.Identity;
                                myTransform_Temp.Origin = myReferencePoint_Centre.GetCoordinateSystem().Origin;

                                myTransform_Temp.BasisX = myTransform_ToMakeTheRotationRelative.OfVector(myTransform_Rotate.BasisX);
                                myTransform_Temp.BasisY = myTransform_ToMakeTheRotationRelative.OfVector(myTransform_Rotate.BasisY);
                                myTransform_Temp.BasisZ = myTransform_ToMakeTheRotationRelative.OfVector(myTransform_Rotate.BasisZ);

                            using (Transaction y = new Transaction(doc, "a Transform"))
                            {
                                y.Start();

                                FailureHandlingOptions options = y.GetFailureHandlingOptions();
                                MyPreProcessor preproccessor = new MyPreProcessor();
                                options.SetFailuresPreprocessor(preproccessor);
                                y.SetFailureHandlingOptions(options);

                                if (myReferencePoint_Centre.get_Parameter(BuiltInParameter.POINT_ELEMENT_DRIVEN).AsInteger() == 1)
                                {
                                    double double_Angle = myTransform_Temp.Inverse.OfVector(myReferencePoint_Centre.GetCoordinateSystem().BasisX).AngleOnPlaneTo(XYZ.BasisX, XYZ.BasisZ);
                                    Line myLine1205 = Line.CreateUnbound(myTransform.Origin, myTransform.BasisZ);
                                    ElementTransformUtils.RotateElement(doc, myReferencePoint_Centre.Id, myLine1205, double_Angle);
                                } else
                                {
                                    myReferencePoint_Centre.SetCoordinateSystem(myTransform_Temp);
                                }


                                if (myBool_InterpolateMiddle_WhenEitherA_or_B)
                                {
                                    List<Transform> myListTransform_Interpolate = Window_UnderstandingTransforms.myMethod_whichTook_120Hours_OfCoding(doc, myIntUpDown_A, myIntUpDown_B);
                                    myReferencePoint_Middle.SetCoordinateSystem(myListTransform_Interpolate[(int)mySlider_Interpolate.Value]);
                                }
                                y.Commit();  //disable warnings, warnings, warnings

                                setSliderClassInstance.setSlider(myReferencePoint_Centre, mySlider_Rotate_BasisZ, false);
                                setSliderClassInstance.setSlider(myReferencePoint_Centre, mySlider_Rotate_BasisX, false);
                                setSliderClassInstance.setSlider(myReferencePoint_Centre, mySlider_Rotate_BasisY, true);
                            }
                        }

                        myLabel_Setting.Content = mySlider.Value.ToString();

                        if (myIntTimeOut == 400)
                        {
                            MessageBox.Show("Timeout");
                            break;
                        }
                    }

                    transGroup.Assimilate();
                }
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("EE03_RotateAroundBasis, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }
        public string GetName()
        {
            return "External Event Example";
        }

        public class MyPreProcessor : IFailuresPreprocessor
        {
            FailureProcessingResult IFailuresPreprocessor.PreprocessFailures(FailuresAccessor failuresAccessor)
            {
                String transactionName = failuresAccessor.GetTransactionName();

                IList<FailureMessageAccessor> fmas = failuresAccessor.GetFailureMessages();

                if (fmas.Count == 0) return FailureProcessingResult.Continue;

                // We already know the transaction name.

                foreach (FailureMessageAccessor fma in fmas)
                {
                    FailureSeverity fseverity = fma.GetSeverity();

                    // ResolveFailure mimics clicking 
                    // 'Remove Link' button             .
                    if (fseverity == FailureSeverity.Warning) failuresAccessor.DeleteWarning(fma);

                    //failuresAccessor.ResolveFailure(fma);
                    // DeleteWarning mimics clicking 'Ok' button.
                    //failuresAccessor.DeleteWarning( fma );         
                }

                //return FailureProcessingResult
                //  .ProceedWithCommit;
                return FailureProcessingResult.Continue;
            }
        }
    }
}
