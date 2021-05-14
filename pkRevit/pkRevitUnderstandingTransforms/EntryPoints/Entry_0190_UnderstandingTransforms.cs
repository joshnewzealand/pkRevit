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

namespace RevitTransformSliders
{
    public partial class ThisApplication_UnderstandingSliders
    {
        public ToAvoidLoadingRevitDLLs toavoidloadingrevitdlls { get; set; }

        public Result StartMethod_0190(ExternalCommandData cd, ref string message, ElementSet elements)
        {
            ExternalCommandData commandData = cd;
            string executionLocation = message;

            toavoidloadingrevitdlls = new ToAvoidLoadingRevitDLLs();
            toavoidloadingrevitdlls.commandData = commandData;
            toavoidloadingrevitdlls.executionLocation = executionLocation;

            int eL = -1;

            try
            {
                Window_UnderstandingTransforms myWindow1 = new Window_UnderstandingTransforms(commandData, message);
                myWindow1.Show();
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("StartMethod_0190, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion

            return Result.Succeeded;
        }
    }
}