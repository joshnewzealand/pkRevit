using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using System.Linq;


namespace pkRevitViewportManagement
{
    /// <summary>
    /// Interaction logic for Window_SizePositionViewport.xaml
    /// </summary>
    public partial class Window_SizePositionViewport : Window
    {
        ToAvoidLoadingRevitDLLs toavoidloadingrevitdlls;
        public View view_2011 { get; set; }
        public Viewport view_2011_viewport { get; set; }

        public int myInt { get; set; }  = 0;

        ExternalEvent ee_CreateViewport;
        EEH_CreateViewport eeh_CreateViewport;

        public Window_SizePositionViewport(ToAvoidLoadingRevitDLLs tALRdll)
        {

            int eL = -1;

            try
            {
                toavoidloadingrevitdlls = tALRdll;

                eeh_CreateViewport = new EEH_CreateViewport() { myWindow1 = this };
                ee_CreateViewport = ExternalEvent.Create(eeh_CreateViewport);

                InitializeComponent();

                this.Top = Properties.Settings.Default.SizeAndPosition_Top;
                this.Left = Properties.Settings.Default.SizeAndPosition_Left;
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("Window_SizePositionViewport, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }
        private void button_ExistingViewport_Click(object sender, RoutedEventArgs e)
        {
            int eL = -1;

            try
            {
                UIDocument uidoc = toavoidloadingrevitdlls.commandData.Application.ActiveUIDocument;
                Document doc = uidoc.Document;
                //acquiredViewport = null;	
                view_2011_viewport = null;

                int selectionCount = uidoc.Selection.GetElementIds().Count;
                if (selectionCount == 0)
                {
                    lb_status.Items.Add(myInt++ + ". Nothing has been selected.");
                    lb_status.SelectedIndex = lb_status.Items.Count - 1;
                    lb_status.ScrollIntoView(lb_status.SelectedItem);
                    return;
                }
                if (selectionCount > 1)
                {
                    lb_status.Items.Add(myInt++ + ". Please only select one item.");
                    lb_status.SelectedIndex = lb_status.Items.Count - 1;
                    lb_status.ScrollIntoView(lb_status.SelectedItem);
                    return;
                }

                Element copyViewportHopeful = doc.GetElement(uidoc.Selection.GetElementIds().ToList()[0]);
                if (copyViewportHopeful.Category.Name != "Viewports")
                {
                    lb_status.Items.Add(myInt++ + ". Selected entity must be a 'viewport'.");
                    lb_status.SelectedIndex = lb_status.Items.Count - 1;
                    lb_status.ScrollIntoView(lb_status.SelectedItem);
                    return;
                }

                lb_status.Items.Add(myInt++ + ". Success! we have the EXISTING viewport, now select a 'Duplicate as a Dependent' view.");
                lb_status.SelectedIndex = lb_status.Items.Count - 1;
                lb_status.ScrollIntoView(lb_status.SelectedItem);
                //	acquiredViewport = copyViewportHopeful as Viewport;

                view_2011_viewport = copyViewportHopeful as Viewport;
            }
            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("button_ExistingViewport_Click, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        private void button_AcquireNewPlan_Click(object sender, RoutedEventArgs e)
        {
            int eL = -1;

            try
            {

                UIDocument uidoc = toavoidloadingrevitdlls.commandData.Application.ActiveUIDocument;
                Document doc = uidoc.Document;

                //////View view_2011 = toavoidloadingrevitdlls.commandData.Application.ActiveUIDocument.ActiveView;

                //////if (view_2011.ViewType != ViewType.DrawingSheet)
                //////{
                //////    MessageBox.Show("Please goto a 'Sheet' type view.");
                //////    return;
                //////}
                ///
                if (view_2011_viewport == null)
                {
                    lb_status.Items.Add(myInt++ + ". Viewport (step 1) not acquired.");
                    lb_status.SelectedIndex = lb_status.Items.Count - 1;
                    lb_status.ScrollIntoView(lb_status.SelectedItem);
                    return;

                }

                view_2011 = null;

                int selectionCount = uidoc.Selection.GetElementIds().Count;
                if (selectionCount == 0)
                {
                    lb_status.Items.Add(myInt++ + ". Nothing has been selected.");
                    lb_status.SelectedIndex = lb_status.Items.Count - 1;
                    lb_status.ScrollIntoView(lb_status.SelectedItem);
                    return;
                }
                if (selectionCount > 1)
                {
                    lb_status.Items.Add(myInt++ + ". Please only select one item.");
                    lb_status.SelectedIndex = lb_status.Items.Count - 1;
                    lb_status.ScrollIntoView(lb_status.SelectedItem);
                    return;
                }

                Element copyViewHopeful = doc.GetElement(uidoc.Selection.GetElementIds().ToList()[0]);
                if (copyViewHopeful.Category.Name != "Views")
                {
                    lb_status.Items.Add(myInt++ + ". Selected entity must be a 'Duplicate as a Dependent' view.");
                    lb_status.SelectedIndex = lb_status.Items.Count - 1;
                    lb_status.ScrollIntoView(lb_status.SelectedItem);
                    return;
                }

                //subjectView = copyViewHopeful as Autodesk.Revit.DB.View;
                view_2011 = copyViewHopeful as Autodesk.Revit.DB.View;


                if (view_2011.GetPrimaryViewId().IntegerValue == -1)
                {
                    lb_status.Items.Add(myInt++ + ". View is not a 'Duplicate as Dependent' view.");
                    lb_status.SelectedIndex = lb_status.Items.Count - 1;
                    lb_status.ScrollIntoView(lb_status.SelectedItem);
                    view_2011 = null;

                    return;
                }


                String sheetnumber = view_2011.GetParameters("Sheet Number")[0].AsString();

                if (sheetnumber != "---")
                {
                    lb_status.Items.Add(myInt++ + ". View has already been placed on sheet " + sheetnumber);
                    lb_status.SelectedIndex = lb_status.Items.Count - 1;
                    lb_status.ScrollIntoView(lb_status.SelectedItem);
                    view_2011 = null;
                    return;
                }

                lb_status.Items.Add(myInt++ + ". Success! we have the 'Duplicate as a Dependent' view.");
                lb_status.SelectedIndex = lb_status.Items.Count - 1;
                lb_status.ScrollIntoView(lb_status.SelectedItem);
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("button_AcquireNewPlan_Click, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        private void button_CreateNewViewport_Click(object sender, RoutedEventArgs e)
        {
            int eL = -1;

            try
            {

                UIDocument uidoc = toavoidloadingrevitdlls.commandData.Application.ActiveUIDocument;
                Document doc = uidoc.Document;


                if (null == uidoc)
                {
                    return; // no document, nothing to do
                }

                if (true) //candidate for methodisation 20210518
                {
                    if (view_2011_viewport == null)
                    {
                        lb_status.Items.Add(myInt++ + ". Viewport view (step 1) not acquired.");
                        lb_status.SelectedIndex = lb_status.Items.Count - 1;
                        lb_status.ScrollIntoView(lb_status.SelectedItem);
                        return;

                    }
                    if (view_2011 == null)
                    {
                        lb_status.Items.Add(myInt++ + ". 'Duplicate as a Dependent' view (step 2) not acquired.");
                        lb_status.SelectedIndex = lb_status.Items.Count - 1;
                        lb_status.ScrollIntoView(lb_status.SelectedItem);
                        return;
                    }
                }

                if (!view_2011_viewport.IsValidObject) view_2011_viewport = null;
                if (!view_2011.IsValidObject) view_2011 = null;

                if(true) //candidate for methodisation 20210518
                {
                    if (view_2011_viewport == null)
                    {
                        lb_status.Items.Add(myInt++ + ". Viewport view (step 1) not acquired.");
                        lb_status.SelectedIndex = lb_status.Items.Count - 1;
                        lb_status.ScrollIntoView(lb_status.SelectedItem);
                        return;

                    }
                    if (view_2011 == null)
                    {
                        lb_status.Items.Add(myInt++ + ". 'Duplicate as a Dependent' view (step 2) not acquired.");
                        lb_status.SelectedIndex = lb_status.Items.Count - 1;
                        lb_status.ScrollIntoView(lb_status.SelectedItem);
                        return;
                    }
                }


                String sheetnumber = view_2011.GetParameters("Sheet Number")[0].AsString();

                if (sheetnumber != "---")
                {
                    lb_status.Items.Add(myInt++ + ". View has already been placed on sheet " + sheetnumber);
                    lb_status.SelectedIndex = lb_status.Items.Count - 1;
                    lb_status.ScrollIntoView(lb_status.SelectedItem);
                    return;
                }


                if (uidoc.ActiveView.ViewType.ToString() != "DrawingSheet")
                {
                    myInt++;
                    lb_status.Items.Add(myInt + ". Please click or pan in a SHEET, (to make it the 'Active' view)");
                    lb_status.SelectedIndex = lb_status.Items.Count - 1;
                    lb_status.ScrollIntoView(lb_status.SelectedItem);
                    return;
                }

                ee_CreateViewport.Raise();
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("button_CreateNewViewport_Click, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        private void lb_status_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var newSelectedItem = e.AddedItems[0];
            if (newSelectedItem != null)
            {
                (sender as ListBox).ScrollIntoView(newSelectedItem);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            int eL = -1;

            try
            {
                Properties.Settings.Default.SizeAndPosition_Top = this.Top;
                Properties.Settings.Default.SizeAndPosition_Left = this.Left;
                Properties.Settings.Default.Save();
                Properties.Settings.Default.Reload();
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("Window_Closing, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }
    }


    public partial class EEH_CreateViewport : IExternalEventHandler
    {
        public Window_SizePositionViewport myWindow1 { get; set; }

        public void Execute(UIApplication uiapp)
        {
            int eL = -1;

            try
            {
                UIDocument uidoc = uiapp.ActiveUIDocument;
                Document doc = uidoc.Document;

                View viewOne = doc.GetElement(myWindow1.view_2011_viewport.ViewId) as View;
                View viewTwo = myWindow1.view_2011;

                using (Transaction y = new Transaction(doc))
                {
                    y.Start("Remove Annotations");

                    viewOne.AreAnnotationCategoriesHidden = true;
                    viewTwo.AreAnnotationCategoriesHidden = true;

                    y.Commit();
                }

                using (Transaction y = new Transaction(doc))
                {
                    y.Start("Adjusting the cropbox according to origin offset");

                    BoundingBoxXYZ newBox = new BoundingBoxXYZ();
                    newBox.set_MinEnabled(0, true);
                    newBox.set_MinEnabled(1, true);
                    newBox.set_MinEnabled(2, true);
                    newBox.Min = new XYZ(viewOne.CropBox.Min.X + viewOne.Origin.X, viewOne.CropBox.Min.Y + viewOne.Origin.Y, 0);
                    newBox.set_MaxEnabled(0, true);
                    newBox.set_MaxEnabled(1, true);
                    newBox.set_MaxEnabled(2, true);
                    newBox.Max = new XYZ(viewOne.CropBox.Max.X + viewOne.Origin.X, viewOne.CropBox.Max.Y + viewOne.Origin.Y, 0);
                    viewTwo.CropBox = newBox;

                    y.Commit();
                }

                XYZ ViewportOne_XYZ = myWindow1.view_2011_viewport.GetBoxCenter();

                using (Transaction t = new Transaction(doc))
                {
                    t.Start("Creating the new viewport");
                    //TaskDialog.Show("Revit","we got this far");				

                    Viewport vvp = Viewport.Create(doc, doc.ActiveView.Id, viewTwo.Id, ViewportOne_XYZ);
                    vvp.ChangeTypeId(myWindow1.view_2011_viewport.GetTypeId());

                    //viewOne.CropBoxActive = true;
                    viewOne.AreAnnotationCategoriesHidden = false;
                    viewTwo.AreAnnotationCategoriesHidden = false;
                    t.Commit();
                }

                using (Transaction t = new Transaction(doc))
                {
                    t.Start("Add annotations");
                    viewOne.CropBoxActive = true;
                    viewTwo.CropBoxActive = true;

                    t.Commit();
                }

                //myWindow1.view_2011 = null;
                myWindow1.lb_status.Items.Add(myWindow1.myInt++ + ". All done...");
                myWindow1.lb_status.Items.Add(myWindow1.myInt++ + ". All done...");
                myWindow1.lb_status.Items.Add(myWindow1.myInt++ + ". All done...");
                myWindow1.lb_status.SelectedIndex = myWindow1.lb_status.Items.Count - 1;
                myWindow1.lb_status.ScrollIntoView(myWindow1.lb_status.SelectedItem);
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("EEH_CreateViewport, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
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
}
