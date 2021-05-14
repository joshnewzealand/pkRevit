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
    public class EE05_Template : IExternalEventHandler  //this is the last when one making a checklist change, EE4 must be just for when an element is new
    {
        public Window_UnderstandingTransforms myWindow1 { get; set; }

        public void Execute(UIApplication uiapp)
        {
            try
            {
                UIDocument uidoc = uiapp.ActiveUIDocument;
                Document doc = uidoc.Document;

                using (Transaction tx = new Transaction(doc))
                {
                    tx.Start("EE05_Template");


                    ReferencePoint myReferencePoint_Centre = doc.GetElement(new ElementId(myWindow1.myToolKit_IntUpDown.Value.Value)) as ReferencePoint;

                    tx.Commit();
                }
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary2.DatabaseMethods.writeDebug("EE05_Template" + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }
        public string GetName()
        {
            return "EE05_Template";
        }
    }


    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class EE05_Move : IExternalEventHandler  //this is the last when one making a checklist change, EE4 must be just for when an element is new
    {
        public System.Windows.Controls.Slider mySlider { get; set; }
        public Xceed.Wpf.Toolkit.IntegerUpDown myToolKit_IntUpDown { get; set; }
        //public Xceed.Wpf.Toolkit.IntegerUpDown myIntUpDown_Middle2 { get; set; }
        public pkRevitUnderstandingTransforms.External_Events.SetSlider setSliderClassInstance { get; set; }

         public System.Windows.Controls.Label myLabel_ChangeCount { get; set; }
        public System.Windows.Controls.Label myLabel_Setting { get; set; }


        public System.Windows.Controls.Slider mySlider_Rotate_BasisZ { get; set; }
        public System.Windows.Controls.Slider mySlider_Rotate_BasisX { get; set; }
        public System.Windows.Controls.Slider mySlider_Rotate_BasisY { get; set; }


        //myLabel_Setting
        public bool mySlideInProgress { get; set; }
        public bool myBool_Rezero { get; set; } = false;

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
                Document doc = uidoc.Document;
                eL = 91;
                ////////////FamilyInstance myFamilyInstance_Departure = doc.GetElement(new ElementId(myToolKit_IntUpDown.Value.Value)) as FamilyInstance;
                ////////////eL = 93;
                ////////////IList<ElementId> placePointIds_1338 = AdaptiveComponentInstanceUtils.GetInstancePointElementRefIds(myFamilyInstance_Departure);
                ReferencePoint myReferencePoint_Centre = doc.GetElement(new ElementId(myToolKit_IntUpDown.Value.Value)) as ReferencePoint;

                Transform myTransform = myReferencePoint_Centre.GetCoordinateSystem();

                int myIntTimeOut = 0;
                int myInt_ChangeCount = 0;
                double myDouble_ChangePosition = -1;
                eL = 119;
                using (TransactionGroup transGroup = new TransactionGroup(doc))
                {
                    transGroup.Start("EE05_Move");

                    if(!myReferencePoint_Centre.get_Parameter(BuiltInParameter.POINT_ELEMENT_DRIVEN).IsReadOnly)
                    {
                        using (Transaction y = new Transaction(doc, "Remove Hosting"))
                        {
                            y.Start();
                            myReferencePoint_Centre.get_Parameter(BuiltInParameter.POINT_ELEMENT_DRIVEN).Set(0);
                            y.Commit();
                        }
                    }

                    eL = 117;

                    while (mySlideInProgress)
                    {
                        wait(100); myIntTimeOut++;
                         
                        if (myDouble_ChangePosition != mySlider.Value)
                        {
                            myDouble_ChangePosition = mySlider.Value;
                            myLabel_ChangeCount.Content = myInt_ChangeCount++.ToString();
                            using (Transaction y = new Transaction(doc, "a Transform"))
                            {
                                y.Start();

                                double myDoubleRotateAngle = myDouble_ChangePosition;

                                Transform myTransform_Temp = Transform.Identity;

                                myTransform_Temp.BasisX = myTransform.BasisX;
                                myTransform_Temp.BasisY = myTransform.BasisY; 
                                myTransform_Temp.BasisZ = myTransform.BasisZ;

                                switch (mySlider.Name)
                                {
                                    case "mySlider_Move_X":
                                        myTransform_Temp.Origin = myTransform.Origin + new XYZ(myDoubleRotateAngle, 0, 0);
                                        break;
                                    case "mySlider_Move_Y":
                                        myTransform_Temp.Origin = myTransform.Origin + new XYZ(0, myDoubleRotateAngle, 0);
                                        break;
                                    case "mySlider_Move_Z":
                                        myTransform_Temp.Origin = myTransform.Origin + new XYZ(0, 0, myDoubleRotateAngle);
                                        break;
                                }

                                myReferencePoint_Centre.SetCoordinateSystem(myTransform_Temp);
                                eL = 171;

                                setSliderClassInstance.setSlider(myReferencePoint_Centre, mySlider_Rotate_BasisZ, false);
                                setSliderClassInstance.setSlider(myReferencePoint_Centre, mySlider_Rotate_BasisX, false);
                                setSliderClassInstance.setSlider(myReferencePoint_Centre, mySlider_Rotate_BasisY, true);

                                y.Commit();
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

                mySlider.Value = 0;
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("EE05_Move, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }
        public string GetName()
        {
            return "EE05_Move";
        }
    }
}


