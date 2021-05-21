
using Autodesk.Revit.UI;
#pragma warning restore CS0246 // The type or namespace name 'Autodesk' could not be found (are you missing a using directive or an assembly reference?)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pkRevitCircleOutFamilies
{
    public class ToAvoidLoadingRevitDLLs
    {
#pragma warning disable CS0246 // The type or namespace name 'ExternalCommandData' could not be found (are you missing a using directive or an assembly reference?)
        public ExternalCommandData commandData { get; set; }
#pragma warning restore CS0246 // The type or namespace name 'ExternalCommandData' could not be found (are you missing a using directive or an assembly reference?)
        public string executionLocation { get; set; }
    }
}
