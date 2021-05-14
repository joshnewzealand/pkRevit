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


namespace pkRevitUnderstandingTransforms
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class EE07_Unhost : IExternalEventHandler  //this is the last when one making a checklist change, EE4 must be just for when an element is new
    {
        public Window_RotatePlatform myWindow1 { get; set; }

        public void Execute(UIApplication uiapp)
        {
            try
            {
                UIDocument uidoc = uiapp.ActiveUIDocument;
                Document doc = uidoc.Document;

                using (Transaction tx = new Transaction(doc))
                {
                    tx.Start("EE06_Unhost");

                    //MessageBox.Show(myWindow1.myReferencePoint_Window.Id.IntegerValue.ToString());

                    if (!myWindow1.myReferencePoint_Window.get_Parameter(BuiltInParameter.POINT_ELEMENT_DRIVEN).IsReadOnly)
                    {
                        myWindow1.myReferencePoint_Window.get_Parameter(BuiltInParameter.POINT_ELEMENT_DRIVEN).Set(0);

                        ////if (myRefPoint.get_Parameter(BuiltInParameter.POINT_ELEMENT_DRIVEN).AsInteger() == 1)
                        ////{
                        ////    myWindow1.mySlider_Rotate_BasisX.IsEnabled = false;
                        ////    myWindow1.mySlider_Rotate_BasisY.IsEnabled = false;
                        ////}
                        ////else
                        ////{
                        //myWindow1.mySlider_Rotate_BasisX.IsEnabled = true;
                        //myWindow1.mySlider_Rotate_BasisY.IsEnabled = true;
                    }
                    myWindow1.mySlider_Rotate_BasisX.IsEnabled = true;
                    myWindow1.mySlider_Rotate_BasisY.IsEnabled = true;
                    myWindow1.button_Unhost.Background = null;
                    myWindow1.button_Unhost.IsEnabled = false;

                    ////}
                    uidoc.Selection.SetElementIds(new List<ElementId>() { myWindow1.myReferencePoint_Window.Id });

                    tx.Commit();
                }
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary2.DatabaseMethods.writeDebug("EE06_Unhost" + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }
        public string GetName()
        {
            return "EE06_Unhost";
        }
    }
}


