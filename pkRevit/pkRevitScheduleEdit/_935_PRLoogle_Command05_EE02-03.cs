using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using System.Windows;
using System.Threading;
using System.Text.RegularExpressions;


namespace pkRevitScheduleEdit
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class _935_PRLoogle_Command05_EE0x_CopyOutLastOneWas01 : IExternalEventHandler  //this is the last when one making a checklist change, EE4 must be just for when an element is new
    {
        public void Execute(UIApplication uiapp)
        {
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document; // myListView_ALL_Fam_Master.Items.Add(doc.GetElement(uidoc.Selection.GetElementIds().First()).Name);

            using (Transaction tx = new Transaction(doc))
            {
                tx.Start("Appropriate descriptor");

                tx.Commit();
            }
        }

        public string GetName()
        {
            return "External Event Example";
        }
    }

    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class _935_PRLoogle_Command05_EE03_ForEachPropertiesGrid : IExternalEventHandler  //this is the last when one making a checklist change, EE4 must be just for when an element is new
    {
        public ScheduleEdit myWindow2 { get; set; }
        // public object myObject { get; set; }

        public void Execute(UIApplication uiapp)
        {
            try
            {
                UIDocument uidoc = uiapp.ActiveUIDocument;
                Document doc = uidoc.Document; // myListView_ALL_Fam_Master.Items.Add(doc.GetElement(uidoc.Selection.GetElementIds().First()).Name);

                bool myBoolCommittTransaction = true;
                if (myBoolCommittTransaction)
                {
                    using (Transaction tx = new Transaction(doc))
                    {
                        tx.Start("Each Properties Grid");

                        foreach (ScheduleEdit.ThreeVariables_Internal myThreeVariables in myWindow2.myOC_ThreeVariables_Internal)
                        {
                            // myObject.GetType().GetProperty(myThreeVariables.myDefinitionName).SetValue(myObject, true);

                            //myWindow2.myPropertiesGrid.SelectedObject.GetType().

                            string myString_Stripped = Regex.Replace(myThreeVariables.myDefinitionName, "[^0-9a-zA-Z]+", "");

                            if (myWindow2.myPropertiesGrid.SelectedObject.GetType().GetProperty(myString_Stripped) == null) continue;

                            object myObjectValue = myWindow2.myPropertiesGrid.SelectedObject.GetType().GetProperty(myString_Stripped).GetValue(myWindow2.myPropertiesGrid.SelectedObject);
                            //object myObjectValue = myObject.GetType().GetProperty(Regex.Replace(myThreeVariables.Definition.Name, "[^0-9a-zA-Z]+", "")).GetValue(myObject);
                            //object myObjectValue = myWindow2.myObject.GetType().GetProperty(Regex.Replace(myThreeVariables.Definition.Name, "[^0-9a-zA-Z]+", "")).GetValue(myWindow2.myObject);

                            //Parameter myParameterDirect = doc.GetElement(new ElementId(myThreeVariables.myID)) as Parameter;
                            if (myWindow2.myElementType.GetOrderedParameters().Where(x => x.Definition.Name == myThreeVariables.myDefinitionName).Count() != 0)
                            {

                                Parameter myParameterDirect = myWindow2.myElementType.GetOrderedParameters().Where(x => x.Definition.Name == myThreeVariables.myDefinitionName).First();
                                //Parameter myParameterDirect2 = myWindow2.myElementType.GetOrderedParameters().Where(x => x.Id.IntegerValue == myThreeVariables.myID).First();

                                switch (myThreeVariables.myStorageType)
                                {
                                    case StorageType.Double:
                                        if (myObjectValue != null) myParameterDirect.Set((double)myObjectValue);
                                        //myWindow2.myObject.GetType().GetProperty(myThreeVariables.Definition.Name).SetValue(myWindow2.myObject, myParameterDirect.AsDouble());
                                        break;
                                    case StorageType.ElementId:
                                        //MyTypeBuilder.CreateProperty(tb, myLViiii.Definition.Name, typeof(string));
                                        break;
                                    case StorageType.Integer:
                                        if (myObjectValue != null) myParameterDirect.Set((int)myObjectValue);
                                        //myWindow2.myObject.GetType().GetProperty(myThreeVariables.Definition.Name).SetValue(myWindow2.myObject, myParameterDirect.AsInteger());
                                        break;
                                    case StorageType.None:
                                        //MyTypeBuilder.CreateProperty(tb, myLViiii.Definition.Name, typeof(string));
                                        break;
                                    case StorageType.String:
                                        if (myObjectValue != null) myParameterDirect.Set((string)myObjectValue);
                                        //myParameterDirect.Set("anything");
                                        //myWindow2.myObject.GetType().GetProperty(myThreeVariables.Definition.Name).SetValue(myWindow2.myObject, myParameterDirect.AsString());
                                        break;
                                }
                            }

                        }
                        //MessageBox.Show(myWindow2.myObject.GetType().GetProperty(myWindow2.myListThreeVariables2[1].Definition.Name).GetValue(myWindow2.myObject).ToString());

                        //there are two different ways for this to go, we need to get the values from the properites grid 
                        //
                        tx.Commit();
                    }

                    myWindow2.myLabel.Visibility = System.Windows.Visibility.Hidden;
                }
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("_935_PRLoogle_Command05_EE03_ForEachPropertiesGrid" + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
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
    }


    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class _935_PRLoogle_Command05_EE01_EditTypeName   //this is the last when one making a checklist change, EE4 must be just for when an element is new
    {


        public class VehicleNumberComparer : IComparer<string>
        {
            public int Compare(string lhs, string rhs)
            {
                //try
                //{


                Regex digitPart = new Regex(@"^\d+", RegexOptions.Compiled);

                var numExtract = new Regex("[0-9]+");
                //int lhsNumber = int.Parse(numExtract.Match(lhs).Value);
                //int rhsNumber = int.Parse(numExtract.Match(rhs).Value);

                int lhsNumber = (Regex.Matches(lhs, @"\d+", RegexOptions.IgnoreCase).Count != 0) ? int.Parse(Regex.Matches(lhs, @"\d+", RegexOptions.IgnoreCase)[0].Value) : 0;
                int rhsNumber = (Regex.Matches(rhs, @"\d+", RegexOptions.IgnoreCase).Count != 0) ? int.Parse(Regex.Matches(rhs, @"\d+", RegexOptions.IgnoreCase)[0].Value) : 0;

                //int lhsNumber =  int.Parse(Regex.Matches(lhs, @"^\d+", RegexOptions.IgnoreCase)[0].Value) ;
                //int rhsNumber = int.Parse(Regex.Matches(rhs, @"^\d+", RegexOptions.IgnoreCase)[0].Value);
                return lhsNumber.CompareTo(rhsNumber);

                //}

                //#region catch and finally
                //catch (Exception ex)
                //{
                //    _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug(ex.Message + Environment.NewLine + ex.InnerException, true);
                //    return null;
                //}
                //finally
                //{

                //}
                //#endregion


            }
        }


        //static IEnumerable<int> GetNumbersFromString(this string input)
        //{
        //    StringBuilder number = new StringBuilder();
        //    foreach (char ch in input)
        //    {
        //        if (char.IsDigit(ch))
        //            number.Append(ch);
        //        else if (number.Length > 0)
        //        {
        //            yield return int.Parse(number.ToString());
        //            number.Clear();
        //        }
        //    }
        //    yield return int.Parse(number.ToString());
        //}

        public string GetName()
        {
            return "External Event Example";
        }
    }
}
