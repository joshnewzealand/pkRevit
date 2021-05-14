/*
 * Created by SharpDevelop.
 * User: Joshua.Lumley
 * Date: 28/04/2019
 * Time: 5:59 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using _952_PRLoogleClassLibrary;
using pkRevitUnderstandingTransforms;

namespace RevitTransformSliders
{
	public partial class ThisApplication_RotatePlatform
	{
        public ToAvoidLoadingRevitDLLs toavoidloadingrevitdlls { get; set; }

        public Result StartMethod_0140(ExternalCommandData cd, ref string message, ElementSet elements)
        {
            int eL = -1;
            try
            {
                ExternalCommandData commandData = cd;
                string executionLocation = message;

                toavoidloadingrevitdlls = new ToAvoidLoadingRevitDLLs();
                toavoidloadingrevitdlls.commandData = commandData;
                toavoidloadingrevitdlls.executionLocation = executionLocation;

                ////UIDocument uidoc = commandData.Application.ActiveUIDocument;
                ////Document doc = uidoc.Document;




                eL = 44;
                int int_referencePoint = pkRevitUnderstandingTransforms.pkSelectReferencePoint_Static.pkSelectRefPoint_AndReturnIDInteger(toavoidloadingrevitdlls, false);
                eL = 46;
                //if (int_referencePoint == -1) return Result.Succeeded;

                Window_RotatePlatform myWindow1 = new Window_RotatePlatform(toavoidloadingrevitdlls, int_referencePoint);
                myWindow1.Show();
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("StartMethod_0140, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion

            return Result.Succeeded;
        }
    }
}