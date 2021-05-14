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


namespace RevitTransformSliders
{

    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class EE01_Template : IExternalEventHandler  //this is the last when one making a checklist change, EE4 must be just for when an element is new
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
    public class EE01_Interpolate : IExternalEventHandler  //this is the last when one making a checklist change, EE4 must be just for when an element is new
    {
        public System.Windows.Controls.Slider mySlider { get; set; }
        public System.Windows.Controls.Slider mySlider_Interpolate { get; set; }
        ////public Xceed.Wpf.Toolkit.IntegerUpDown myToolKit_IntUpDown { get; set; }
        public Xceed.Wpf.Toolkit.IntegerUpDown myIntUpDown_Middle2 { get; set; }
        public Xceed.Wpf.Toolkit.IntegerUpDown myUpDown_CycleNumber { get; set; }
        public Xceed.Wpf.Toolkit.IntegerUpDown myIntUpDown_A { get; set; }
        public Xceed.Wpf.Toolkit.IntegerUpDown myIntUpDown_B { get; set; }
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

        public bool myBool_RunOnce { get; set; }
        public bool myBool_Cycle { get; set; }
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

                List<Transform> myListTransform_Interpolate = Window_UnderstandingTransforms.myMethod_whichTook_120Hours_OfCoding(doc, myIntUpDown_A, myIntUpDown_B);

                ////FamilyInstance myFamilyInstance_Departure = doc.GetElement(new ElementId(myIntUpDown_Middle2.Value.Value)) as FamilyInstance;
                ////FamilySymbol myFamilySymbol = doc.GetElement(myFamilyInstance_Departure.GetTypeId()) as FamilySymbol;

                ////IList<ElementId> placePointIds_1338 = AdaptiveComponentInstanceUtils.GetInstancePointElementRefIds(myFamilyInstance_Departure);
                ReferencePoint myReferencePoint_Departure = doc.GetElement(new ElementId(myIntUpDown_Middle2.Value.Value)) as ReferencePoint;


                Transform myTransform = myReferencePoint_Departure.GetCoordinateSystem();

                int myIntTimeOut = 0;
                int myInt_ChangeCount = 0;
                double myDouble_ChangePosition = -1;

                ///                  TECHNIQUE 6 OF 19
                ///↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓ SET DEFAULT TYPE ↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓
                ///Demonstrates: 
                ///SketchPlane
                ///PromptForFamilyInstancePlacement with the PromptForFamilyInstancePlacementOptions class
                ///DocumentChanged event that cancels the command and focuses the window

                eL = 135;

                using (TransactionGroup transGroup = new TransactionGroup(doc))
                {
                    transGroup.Start("Transform animation");

                    if (myBool_Cycle)
                    {
                        for (int i = 1; i <= myUpDown_CycleNumber.Value; i++)
                        {
                            for (int ii = 0; ii <= 24; ii++)
                            {
                               
                                wait(100);
                                if (true) //candidate for methodisation 202006141254
                                {
                                    if (myDouble_ChangePosition != ii)
                                    {
                                        myDouble_ChangePosition = ii;
                                        myLabel_ChangeCount.Content = myInt_ChangeCount++.ToString();
                                        using (Transaction y = new Transaction(doc, "a Transform"))
                                        {
                                            y.Start();

                                            myReferencePoint_Departure.SetCoordinateSystem(myListTransform_Interpolate[(int)myDouble_ChangePosition]);

                                            y.Commit();
                                        }

                                        ReferencePoint myRefPoint = doc.GetElement(new ElementId(myIntUpDown_Middle2.Value.Value)) as ReferencePoint;
                                        setSliderClassInstance.setSlider(myRefPoint, mySlider_Rotate_BasisZ, false);
                                        setSliderClassInstance.setSlider(myRefPoint, mySlider_Rotate_BasisX, false);
                                        setSliderClassInstance.setSlider(myRefPoint, mySlider_Rotate_BasisY, true);
                                    }
                                }
                                myLabel_Setting.Content = ii.ToString();
                            }
                        }
                    }
                    eL = 173;

                    if (!myBool_Cycle)
                    {

                        while (mySlideInProgress | myBool_RunOnce == true)
                        {
                            //MessageBox.Show("do we ever hit here");
                            myBool_RunOnce = false;
                            wait(100); myIntTimeOut++;

                            eL = 186;

                            if (true) //candidate for methodisation 202006141254
                            {
                                if (myDouble_ChangePosition != mySlider_Interpolate.Value)
                                {
                                    eL = 192;
                                    myDouble_ChangePosition = mySlider_Interpolate.Value;
                                    myLabel_ChangeCount.Content = myInt_ChangeCount++.ToString();
                                    using (Transaction y = new Transaction(doc, "a Transform"))
                                    {
                                        y.Start();
                                        eL = 198;
                                        myReferencePoint_Departure.SetCoordinateSystem(myListTransform_Interpolate[(int)myDouble_ChangePosition]);

                                        y.Commit();
                                    }
                                    eL = 202;
                                    ReferencePoint myRefPoint = doc.GetElement(new ElementId(myIntUpDown_Middle2.Value.Value)) as ReferencePoint;
                                    setSliderClassInstance.setSlider(myRefPoint, mySlider_Rotate_BasisZ, false);
                                    setSliderClassInstance.setSlider(myRefPoint, mySlider_Rotate_BasisX, false);
                                    setSliderClassInstance.setSlider(myRefPoint, mySlider_Rotate_BasisY, true);
                                }
                            }
                            eL = 210;
                            myLabel_Setting.Content = mySlider_Interpolate.Value.ToString();

                            if (myIntTimeOut == 400)
                            {
                                MessageBox.Show("Timeout");
                                break;
                            }
                        }
                    }
                    transGroup.Assimilate();
                }
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary2.DatabaseMethods.writeDebug("EE01_Part1_Interpolate  " + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
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
    }
}
