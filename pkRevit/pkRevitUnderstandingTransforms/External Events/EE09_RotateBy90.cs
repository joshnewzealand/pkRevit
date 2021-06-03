using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;


namespace pkRevitUnderstandingTransforms
{

    public enum xyzThree
    {
        XYZ_X = 0,
        XYZ_Y = 1,
        XYZ_Z = 2
    }
    //public const double 

    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class EE09_RotateBy90 : IExternalEventHandler  //this is the last when one making a checklist change, EE4 must be just for when an element is new
    {
        public Window_RotatePlatform myWindow1 { get; set; }

        public bool bool_Clockwise { get; set; } = true;

        public int xyz_Basis { get; set; } = 2;

        public XYZ public_XYZ(Transform myTransform, int myOriginalXYZ)
        {
            switch (myOriginalXYZ)
            {
                case (int)xyzThree.XYZ_X:
                    return myTransform.BasisX;
                case (int)xyzThree.XYZ_Y:
                    return myTransform.BasisY;
                case (int)xyzThree.XYZ_Z:
                    return myTransform.BasisZ;
            }

            return XYZ.BasisZ;
        }

        public void Execute(UIApplication uiapp)
        {
            int eL = -1;

            try
            {
                UIDocument uidoc = uiapp.ActiveUIDocument;
                Document doc = uidoc.Document;

                eL = 55;

                ReferencePoint myReferencePoint_Centre = doc.GetElement(new ElementId(myWindow1.myIntUpDown_Middle2.Value.Value)) as ReferencePoint;

                Transform myTransform = myReferencePoint_Centre.GetCoordinateSystem();

                using (Transaction tx = new Transaction(doc))
                {
                    tx.Start("Rotate by 45°");

                    FailureHandlingOptions options = tx.GetFailureHandlingOptions();
                    MyPreProcessor preproccessor = new MyPreProcessor();
                    options.SetFailuresPreprocessor(preproccessor);
                    tx.SetFailureHandlingOptions(options);

                    Line myLine1205 = Line.CreateUnbound(myTransform.Origin, public_XYZ(myTransform, xyz_Basis));
                    ElementTransformUtils.RotateElement(doc, myReferencePoint_Centre.Id, myLine1205, Math.PI / 4 * (bool_Clockwise ? 1 : -1));
                    tx.Commit();
                }
            }


            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("EE09_RotateBy90, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
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

        ////FailureHandlingOptions options = tx.GetFailureHandlingOptions();
        ////MyPreProcessor preproccessor = new MyPreProcessor();
        ////options.SetFailuresPreprocessor(preproccessor);
        ////            tx.SetFailureHandlingOptions(options);

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


