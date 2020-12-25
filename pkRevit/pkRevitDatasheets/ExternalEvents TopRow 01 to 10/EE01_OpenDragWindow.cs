﻿using System;
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
using Autodesk.Revit.DB.Events;
using System.Runtime.InteropServices;
using _952_PRLoogleClassLibrary;
using Binding = Autodesk.Revit.DB.Binding;
using View = Autodesk.Revit.DB.View;
using System.IO;
using System.Diagnostics;
using Autodesk.Revit.DB.ExtensibleStorage;
using System.Globalization;
using System.Windows;
using MessageBox = System.Windows.Forms.MessageBox;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.UI.Selection;


namespace pkRevitDatasheets
{
    public partial class ThisApplication
    {
        #region external event declarations 

        public EE01_OpenDragWindow myEE01_SetAFL { get; set; }
        public ExternalEvent myExternalEvent_EE01_SetAFL { get; set; }

        #endregion

        public Result StartMethod_01(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            #region initiating_ExternalEvents
            myEE01_SetAFL = new EE01_OpenDragWindow();
            myExternalEvent_EE01_SetAFL = ExternalEvent.Create(myEE01_SetAFL);
            #endregion

            ////myMainWindow.Topmost = true;
            ////myMainWindow.commandData = commandData;
            /////////////////////////////////////////// myMainWindow.Show();
            ///
            myExternalEvent_EE01_SetAFL.Raise();
            return Result.Succeeded;
        }
    }


    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class EE01_OpenDragWindow : IExternalEventHandler  //this is the last when one making a checklist change, EE4 must be just for when an element is new
    {
        public MainWindow myWindow1 { get; set; }

        public void Execute(UIApplication uiapp)
        {
            try
            {
                MessageBox.Show("Hello world3");
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("EE01_OpenDragWindow" + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        public string GetName()
        {
            return "EE01_OpenDragWindow";
        }
    }
}
