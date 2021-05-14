using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pkRevitMisc
{
    public class ToAvoidLoadingRevitDLLs
    {
        public ExternalCommandData commandData { get; set; }
        public string executionLocation { get; set; }
    }
}
