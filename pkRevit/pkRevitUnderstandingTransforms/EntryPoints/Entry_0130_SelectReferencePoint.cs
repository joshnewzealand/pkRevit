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
	public partial class ThisApplication_SelectReferencePoint
	{
        public ToAvoidLoadingRevitDLLs toavoidloadingrevitdlls { get; set; }


        public Result StartMethod_0130(ExternalCommandData cd, ref string message, ElementSet elements)
        {
            ExternalCommandData commandData = cd;
            string executionLocation = message;

            toavoidloadingrevitdlls = new ToAvoidLoadingRevitDLLs();
            toavoidloadingrevitdlls.commandData = commandData;
            toavoidloadingrevitdlls.executionLocation = executionLocation;

            //if the current selection is a family instance select that

            pkRevitUnderstandingTransforms.pkSelectReferencePoint_Static.pkSelectRefPoint_AndReturnIDInteger(toavoidloadingrevitdlls, true);

            return Result.Succeeded;
        }
    }
}