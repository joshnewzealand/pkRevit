/*
 * Created by SharpDevelop.
 * User: Joshua
 * Date: 21/07/2019
 * Time: 4:52 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Windows.Forms;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using MyAlias = pkRevitScheduleEdit._935_PRLoogle_Command05_EE01_EditTypeName;
using _952_PRLoogleClassLibrary;

using System.Reflection;

using System.Data;
using System.IO;
using System.Runtime.Serialization;

namespace pkRevitScheduleEdit
{

    public partial class ThisApplication
    {
        int eL = -1;

        public string messageConst { get; set; }

        public ScheduleEdit myWindow2 { get; set; }

        public ExternalCommandData myExternalCommandData { get; set; }
        public string myString_1400_filename = "\\ALL Parameter Groupings List.xml";
        public string myString_1400_filename_FirstHalf { get; set; }


        public class _935_PRLoogle_Command02_EE05_UpdateFamilyAndTypeName : IExternalEventHandler  //this is the last when one making a checklist change, EE4 must be just for when an element is new
        {
            public string myStringNewFamilyName { get; set; }
            public string myStringNewTypeName { get; set; }
            //public XYZ myXYZ { get; set; }
            public FamilySymbol myFamilySymbol { get; set; }
            public ScheduleEdit myWind2Revcontrol { get; set; }

            public void Execute(UIApplication uiapp)
            {
                UIDocument uidoc = uiapp.ActiveUIDocument;
                Document doc = uidoc.Document; // myListView_ALL_Fam_Master.Items.Add(doc.GetElement(uidoc.Selection.GetElementIds().First()).Name);

                using (Transaction y = new Transaction(doc, "Change Family and Type Name"))
                {
                    y.Start();
                    myFamilySymbol.Name = myStringNewTypeName;
                    myFamilySymbol.Family.Name = myStringNewFamilyName;

                    //  doc.Regenerate();

                    y.Commit();
                }

                uidoc.RefreshActiveView();
                myWind2Revcontrol.myTextBox_TypeNameOld_Category.Text = myFamilySymbol.Category.Name;
                myWind2Revcontrol.myTextBox_TypeNameOld.Text = myFamilySymbol.Name;
                myWind2Revcontrol.myTextBox_TypeNameNew.Text = myFamilySymbol.Name;

                myWind2Revcontrol.myTextBox_FamilyNameOld.Text = myFamilySymbol.FamilyName;
                myWind2Revcontrol.myTextBox_FamilyNameNew.Text = myFamilySymbol.FamilyName;
                //myWind2Revcontrol.FilterAndPassFamilyInstance();
            }

            public string GetName()
            {
                return "External Event Example";
            }
        }


        public partial class the_PPC
        {
            public string myName { get; set; }
            public string myDescription { get; set; }
            public string myConnection_Type { get; set; }
            public string myConnection_Type_Dept { get; set; }
            public string myAcceptable_Manufacturer { get; set; }
        }


        public Result _935_PRLoogle_Command05(ExternalCommandData commandData, ref string message, ElementSet elements) //pack man Type\nName Edit (edit properties via scroller)
        {
            eL = 94;

            try
            {
                //
                string FILE_NAME = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Pedersen Read Limited\\pkRevit joshnewzealand"; // cSharpPlaypen joshnewzealand
                //string FILE_NAME = System.Environment.GetEnvironmentVariable("ProgramData") + "\\Pedersen Read Limited\\pkRevit joshnewzealand"; // cSharpPlaypen joshnewzealand

                if (true) //grouping for clarity will alwasy be true
                {
                    if (!System.IO.Directory.Exists(FILE_NAME)) System.IO.Directory.CreateDirectory(FILE_NAME);
                }


                ////////string path_1303 = @"anything|" + @"Q:\Revit Revit Revit\~Parameter Groupings";  //this is direct path and it shouldn't be
                ////////string[] myStringArray_1302 = path_1303.Split('|');

                myString_1400_filename_FirstHalf = FILE_NAME;
                if (!System.IO.Directory.Exists(myString_1400_filename_FirstHalf)) { DatabaseMethods.writeDebug("Directory is not there:" + Environment.NewLine + myString_1400_filename_FirstHalf, true); return Result.Succeeded; }
                eL = 114;

                messageConst = message;
                myExternalCommandData = commandData;

                UIDocument uidoc = commandData.Application.ActiveUIDocument;
                Document doc = uidoc.Document;

                //  Element myElement = doc.GetElement(new ElementId(11241802));

                //string stringTargetOokiiVersion = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\\Default Company Name\\Revit API NuGet Example 2019").GetValue("OokiiVersion").ToString();
                //string stringTargetXceedVersion = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\\Default Company Name\\Revit API NuGet Example 2019").GetValue("XceedVersion").ToString();

                bool myBoolBecauseIDidntWantToCreate_NewAnything = false; //just remove this line to restore
                eL = 127;
                if (myBoolBecauseIDidntWantToCreate_NewAnything)  //this is an easy way of transferring values for not oftype
                {
                    FilteredElementCollector myFFC = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_ElectricalCircuit);

                    using (Transaction y = new Transaction(doc, "Trying to null"))
                    {
                        y.Start();

                        foreach (Element myEleeeee in myFFC)
                        {
                            myEleeeee.GetParameters("DLCS - Description")[0].Set(myEleeeee.GetParameters("Schedule Circuit Notes")[0].AsString());

                            //foreach (Parameter myppppp in myEleeeee.Parameters)
                            //{
                            //    //DLCS - Description  //Schedule Circuit Notes
                            //    //if (myppppp.Definition.Name.Split(' ').First() == "DLCS") if (myppppp.StorageType == StorageType.Integer) if (myppppp.AsInteger() == 0) myppppp.Set(-1);
                            //    if (myppppp.Definition.Name.Split(' ').First() == "DLCS") if (myppppp.StorageType == StorageType.Integer) if (myppppp.AsInteger() == 0) myppppp.Set(-1);
                            //}
                        }
                        y.Commit();
                    }
                }
                eL = 150;
                //////////bool myFromSelected = false;
                //////////if (uidoc.Selection.GetElementIds().Count == 1)
                //////////{
                //////////    myFromSelected = true;
                //////////} else
                //////////{
                //////////    MessageBox.Show("Please select just one element.");
                //////////    return Result.Succeeded;    
                //////////}

                ScheduleEdit myWindow3 = new ScheduleEdit(commandData, this);
                eL = 162;
                myWindow2 = myWindow3;
                eL = 164;
                myWindow3.Show();
                eL = 166;

                return Result.Succeeded;
            }

            #region catch and finally
            catch (Exception ex)
            {
               _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("_935_PRLoogle_Command05, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);


                return Result.Succeeded;
            }
            finally
            {
            }
            #endregion
        }



    }
}