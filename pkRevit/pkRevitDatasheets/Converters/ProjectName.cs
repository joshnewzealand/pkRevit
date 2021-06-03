using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using pkRevitDatasheets.BuildingCoderClasses;
using QuickZip.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
//using System.Data.SQLite;
using System.Data.SQLite;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using DataFormats = System.Windows.DataFormats;
using DataObject = System.Windows.DataObject;
using DragDropEffects = System.Windows.DragDropEffects;
using DragEventArgs = System.Windows.DragEventArgs;
using ListBox = System.Windows.Controls.ListBox;
using MessageBox = System.Windows.MessageBox;
using MouseEventArgs = System.Windows.Input.MouseEventArgs;
using View = Autodesk.Revit.DB.View;

namespace pkRevitDatasheets
{

    [ValueConversion(typeof(string), typeof(string))]
    public class ProjectNameConverter : IValueConverter
    {
         public MainWindow window_main { get; set; }

        #region IValueConverter Members
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
           // window_main = value[0] as MainWindow;

            string fileName = window_main.dict_GuidToAlias[new Guid(   ((DataRowView)value)["ProjectGUID"] .ToString())];  //
            string scheduleName = "↳" + ((DataRowView)value)["ScheduleName"].ToString();  //
            return fileName + Environment.NewLine + scheduleName;
        }

        public object ConvertBack(object value, System.Type targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

}
