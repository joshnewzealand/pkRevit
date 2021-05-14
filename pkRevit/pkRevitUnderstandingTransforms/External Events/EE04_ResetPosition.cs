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
    public class EE04_Template : IExternalEventHandler  //this is the last when one making a checklist change, EE4 must be just for when an element is new
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
                    tx.Start("EE04_Template_Template");

                    tx.Commit();
                }
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary2.DatabaseMethods.writeDebug("EE04_Template_Template" + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }
        public string GetName()
        {
            return "EE04_Template_Template";
        }
    }


    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class EE04_ResetPosition : IExternalEventHandler  //this is the last when one making a checklist change, EE4 must be just for when an element is new
    {
        //public Window_UnderstandingTransforms myWindow1 { get; set; }
        public Xceed.Wpf.Toolkit.IntegerUpDown myToolKit_IntUpDown { get; set; }

        public pkRevitUnderstandingTransforms.External_Events.SetSlider setSliderClassInstance { get; set; }

        public System.Windows.Controls.Slider mySlider_Rotate_BasisZ { get; set; }
        public System.Windows.Controls.Slider mySlider_Rotate_BasisX { get; set; }
        public System.Windows.Controls.Slider mySlider_Rotate_BasisY { get; set; }

        public Transform myTransform_Temp { get; set; }

        public void Execute(UIApplication uiapp)
        {
            int eL = -1;

            try
            {
                UIDocument uidoc = uiapp.ActiveUIDocument;
                Document doc = uidoc.Document; 

                ReferencePoint myReferencePoint_Centre = doc.GetElement(new ElementId(myToolKit_IntUpDown.Value.Value)) as ReferencePoint;

                Transform myTransform = myReferencePoint_Centre.GetCoordinateSystem();

                using (Transaction tx = new Transaction(doc))
                {
                    tx.Start("EE04_ResetPosition");

                    eL = 86;

                    myReferencePoint_Centre.SetCoordinateSystem(myTransform_Temp);

                    tx.Commit();
                }

                setSliderClassInstance.setSlider(myReferencePoint_Centre, mySlider_Rotate_BasisZ, false);
                setSliderClassInstance.setSlider(myReferencePoint_Centre, mySlider_Rotate_BasisX, false);
                setSliderClassInstance.setSlider(myReferencePoint_Centre, mySlider_Rotate_BasisY, true);
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary2.DatabaseMethods.writeDebug("EE04_ResetPosition" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }
        public string GetName()
        {
            return "EE04_ResetPosition";
        }
    }
}

