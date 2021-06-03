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
using Xceed.Wpf.Toolkit;

namespace pkRevitUnderstandingTransforms.External_Events//SetSlider
{
    public static class SelectMethod//.mySelectMethod.
    {
        private static bool setAndSelect(UIDocument uidoc, IntegerUpDown myToolKit_IntUpDown,  FamilyInstance myFamilyInstance)
        {
            string str_FamilyName = myFamilyInstance.get_Parameter(BuiltInParameter.ELEM_FAMILY_PARAM).AsValueString();

            if (str_FamilyName == "PRL-GM Adaptive Carrier Youtube" | str_FamilyName == "PRL-GM-2020 Adaptive Carrier")
            {
                ////if (uidoc.Selection.GetElementIds().Count != 1) break;
                ////Element myElement = doc.GetElement(uidoc.Selection.GetElementIds().First()) as Element;
                ////if (myElement.GetType() != typeof(FamilyInstance)) break;
                ////FamilyInstance myFamilyInstance = myElement as FamilyInstance;

                IList<ElementId> placePointIds_1338 = AdaptiveComponentInstanceUtils.GetInstancePointElementRefIds(myFamilyInstance);
                ReferencePoint myReferencePoint_Centre = uidoc.Document.GetElement(placePointIds_1338.First()) as ReferencePoint;

                myToolKit_IntUpDown.Value = myReferencePoint_Centre.Id.IntegerValue;
                uidoc.Selection.SetElementIds(new List<ElementId>() { myReferencePoint_Centre.Id });

                return true;
            }

            return false;
        }


        public static bool mySelectMethod(UIDocument uidoc, Xceed.Wpf.Toolkit.IntegerUpDown myToolKit_IntUpDown) //myToolKit_IntUpDown
        {
            Document doc = uidoc.Document;

            while (true)
            {
                if (uidoc.Selection.GetElementIds().Count != 1) break;
                Element myElement = doc.GetElement(uidoc.Selection.GetElementIds().First()) as Element;
                if (myElement.GetType() != typeof(FamilyInstance)) break;
                FamilyInstance myFamilyInstance = myElement as FamilyInstance;

                if (setAndSelect(uidoc, myToolKit_IntUpDown, myFamilyInstance)) break;

                if (myFamilyInstance.Host == null) break;
                List<Element> myListElement = new FilteredElementCollector(doc).OfClass(typeof(FamilyInstance)).OfCategory(BuiltInCategory.OST_GenericModel).Where(x => x.Id == myFamilyInstance.Host.Id).ToList();
                if (myListElement.Count != 1) break;
                FamilyInstance myFamilyInstance_2148 = myListElement.First() as FamilyInstance;

                if (setAndSelect(uidoc, myToolKit_IntUpDown, myFamilyInstance_2148)) break;
            }

            //find the familyInstance from the reference

            if (myToolKit_IntUpDown.Value.Value == -1) 
            {
                FilteredElementCollector fec2 = new FilteredElementCollector(doc).WhereElementIsNotElementType().OfCategory(BuiltInCategory.OST_AdaptivePoints);
                List<Element> myListRefPoint = fec2.ToList();

                if (myListRefPoint.Count() != 0)
                {
                    ReferencePoint myReferencePoint_0805 = myListRefPoint.Last() as ReferencePoint;
                    uidoc.Selection.SetElementIds(new List<ElementId>() { new ElementId(myReferencePoint_0805.Id.IntegerValue - 1) });
                    myToolKit_IntUpDown.Value = myReferencePoint_0805.Id.IntegerValue;
                    uidoc.Selection.SetElementIds(new List<ElementId>() { myReferencePoint_0805.Id });
                }
                else
                {
                    System.Windows.MessageBox.Show("There are no reference points in the model");
                }
            }


            if (myToolKit_IntUpDown.Value == null) return false;
            if (myToolKit_IntUpDown.Value.Value == -1) return false;
            if (doc.GetElement(new ElementId(myToolKit_IntUpDown.Value.Value)) == null) return false;
            return true;
        }
    }


    public class SetSlider
    {
        public ExternalCommandData commandData { get; set; }
        public System.Windows.Controls.Label myLabel_TransformXBasis { get; set; }
        public System.Windows.Controls.Label myLabel_TransformYBasis { get; set; }
        public System.Windows.Controls.Label myLabel_TransformZBasis { get; set; }
        public System.Windows.Controls.Label myLabel_TransformOrigin { get; set; }


        public void setSlider(ReferencePoint myReferencePoint_Centre, System.Windows.Controls.Slider mySlider_temp, bool myBool_UpdateLabels)
        {
           // if (myToolKit_IntUpDown_temp.Value.Value == 0) return;

            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            ////////if (doc.GetElement(new ElementId(myToolKit_IntUpDown_temp.Value.Value)) == null) return;

            ////////FamilyInstance myFamilyInstance = doc.GetElement(new ElementId(myToolKit_IntUpDown_temp.Value.Value)) as FamilyInstance;

            ////////IList<ElementId> placePointIds_1338 = AdaptiveComponentInstanceUtils.GetInstancePointElementRefIds(myFamilyInstance);

            ////////ReferencePoint myReferencePoint_Centre = doc.GetElement(placePointIds_1338.First()) as ReferencePoint;

            Transform myTransform = myReferencePoint_Centre.GetCoordinateSystem();

            Transform myTransformFromQuatXX;
            double aaMega_QuatXX = XYZ.BasisZ.AngleTo(myTransform.BasisZ);  //2 places line , 3 places method

            switch (mySlider_temp.Name)
            {
                case "mySlider_Rotate_BasisY":
                    aaMega_QuatXX = XYZ.BasisY.AngleTo(myTransform.BasisY);  //2 places line , 3 places method
                    break;
                case "mySlider_Rotate_BasisX":
                    aaMega_QuatXX = XYZ.BasisX.AngleTo(myTransform.BasisX);  //2 places line , 3 places method
                    break;
            }

            if (IsZero(aaMega_QuatXX)) myTransformFromQuatXX = Transform.Identity;
            else
            {
                XYZ axis = myTransform.BasisZ.CrossProduct(-XYZ.BasisZ);  //2 places line , 3 places method,  normally (z) negative

                switch (mySlider_temp.Name)
                {
                    case "mySlider_Rotate_BasisY":
                        axis = myTransform.BasisY.CrossProduct(-XYZ.BasisY); //2 places line , 3 places method,  normally (z) negative
                        break;
                    case "mySlider_Rotate_BasisX":
                        axis = myTransform.BasisX.CrossProduct(-XYZ.BasisX); //2 places line , 3 places method,  normally (z) negative
                        break;
                }

                if (IsZero(axis.X) & IsZero(axis.Y) & IsZero(axis.Z))
                {
                    myTransformFromQuatXX = myTransform;

                }
                else
                {
                    myTransformFromQuatXX = Transform.CreateRotationAtPoint(axis, aaMega_QuatXX, XYZ.Zero);
                }
            }

            Transform myEndPointTransform = Transform.Identity;
            myEndPointTransform.BasisX = myTransformFromQuatXX.Inverse.OfVector(myTransform.BasisX);
            myEndPointTransform.BasisY = myTransformFromQuatXX.Inverse.OfVector(myTransform.BasisY);
            myEndPointTransform.BasisZ = myTransformFromQuatXX.Inverse.OfVector(myTransform.BasisZ);

            double myDouble_AngleToXBasis = myEndPointTransform.BasisX.AngleOnPlaneTo(-XYZ.BasisX, XYZ.BasisZ); //two places in line, 

            switch (mySlider_temp.Name)
            {
                case "mySlider_Rotate_BasisY":
                    myDouble_AngleToXBasis = myEndPointTransform.BasisZ.AngleOnPlaneTo(-XYZ.BasisZ, XYZ.BasisY); //two places in line, 
                    break;
                case "mySlider_Rotate_BasisX":
                    myDouble_AngleToXBasis = myEndPointTransform.BasisY.AngleOnPlaneTo(-XYZ.BasisY, XYZ.BasisX); //two places in line, 
                    break;
            }

            double myDouble_TurnToVector = myDouble_AngleToXBasis / (Math.PI * 2) * 24;

            mySlider_temp.Value = myDouble_TurnToVector;

            if (myBool_UpdateLabels)
            {
                myLabel_TransformXBasis.Content = myTransform.BasisX.ToString();
                myLabel_TransformYBasis.Content = myTransform.BasisY.ToString();
                myLabel_TransformZBasis.Content = myTransform.BasisZ.ToString();
                myLabel_TransformOrigin.Content = myTransform.Origin.ToString();
            }
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

    }
}
