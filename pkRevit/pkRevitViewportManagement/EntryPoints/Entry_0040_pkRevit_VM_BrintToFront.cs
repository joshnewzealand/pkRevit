using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

using System.Windows.Forms;
using System;
using System.Collections.Generic;
using pkRevitViewportManagement;


namespace pkRevitViewportManagement.EntryPoints  //Entry_0010_pkRevitDatasheets
{
    public partial class Entry_0040_pkRevit_VM
    {
        public Result StartMethod_0040(ExternalCommandData cd, ref string message, ElementSet elements)
        {
            int eL = -1;

            try
            {
                ToAvoidLoadingRevitDLLs toavoidloadingrevitdlls = new ToAvoidLoadingRevitDLLs();
                toavoidloadingrevitdlls.commandData = cd;
                toavoidloadingrevitdlls.executionLocation = message;

                if (toavoidloadingrevitdlls.commandData.Application.ActiveUIDocument.ActiveView.ViewType != ViewType.DrawingSheet)
                {
                    MessageBox.Show("Please goto a 'Sheet' type view.");
                    return Result.Succeeded;
                }

                Window_BringToFront bringWindowToFront = new Window_BringToFront(toavoidloadingrevitdlls);
                bringWindowToFront.Show();

                //we need to lock in the current view unless the user clicks refresh.
                //we need to lock in the current view unless the user clicks refresh.
                //we need to lock in the current view unless the user clicks refresh.

                ////////////////////Bring_to_Front(commandData.Application.ActiveUIDocument);
            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("StartMethod_0040, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
            return Result.Succeeded;
        }

        //////void clear_and_update_the_listview()
        //////{
        //////    listBox1.Items.Clear();
        //////    ViewportList1.Clear();

        //////    foreach (ElementId ViewSheet1_Viewports in ViewSheet1.GetAllViewports())
        //////    {
        //////        Viewport Viewport1 = doc.GetElement(ViewSheet1_Viewports) as Viewport;
        //////        ViewportList1.Add(Viewport1);

        //////        Autodesk.Revit.DB.View ViewportList1View = doc.GetElement(Viewport1.ViewId) as Autodesk.Revit.DB.View;

        //////        listBox1.Items.Add(ViewportList1View.get_Parameter(BuiltInParameter.VIEW_NAME).AsString());
        //////    }

        //////}

        ////////////////////public void Bring_to_Front(UIDocument uid)
        ////////////////////{
        ////////////////////    UIDocument uidoc = uid;
        ////////////////////    Document doc = uidoc.Document;

        ////////////////////    try
        ////////////////////    {
        ////////////////////        if(uid.ActiveView.ViewType != ViewType.DrawingSheet)
        ////////////////////        {
        ////////////////////            MessageBox.Show("Please goto a 'Sheet' type view.");
        ////////////////////            return;
        ////////////////////        }

        ////////////////////        // Create a new instance of the form.
        ////////////////////        System.Windows.Forms.Form myForm = new Form1(doc);
        ////////////////////        myForm.Text = "Send selected viewport to front";
        ////////////////////        myForm.ShowDialog();
        ////////////////////    }// end try
        ////////////////////    catch
        ////////////////////    {
        ////////////////////        TaskDialog.Show("Revit C# Error", "Error");

        ////////////////////    }
        ////////////////////}

        ////////////////////public partial class Form1 : System.Windows.Forms.Form
        ////////////////////{


        ////////////////////    List<Viewport> ViewportList1 = new List<Viewport>();
        ////////////////////    Document doc = null;
        ////////////////////    ViewSheet ViewSheet1 = null;

        ////////////////////    public Form1(Document doc2)
        ////////////////////    {
        ////////////////////        //
        ////////////////////        // The InitializeComponent() call is required for Windows Forms designer support.
        ////////////////////        //
        ////////////////////        InitializeComponent();
        ////////////////////        doc = doc2;
        ////////////////////        //
        ////////////////////        // TODO: Add constructor code after the InitializeComponent() call.
        ////////////////////        //
        ////////////////////    }

        ////////////////////    void Form1Load(object sender, EventArgs e)
        ////////////////////    {

        ////////////////////    }

        ////////////////////    void Button1Click(object sender, EventArgs e)
        ////////////////////    {

        ////////////////////        ViewSheet1 = doc.ActiveView as ViewSheet;
        ////////////////////        clear_and_update_the_listview();
        ////////////////////    }



        ////////////////////    void ViewportBringToFront(ViewSheet sheet, Viewport viewport)
        ////////////////////    {
        ////////////////////        Document doc = sheet.Document;
        ////////////////////        ElementId viewId = viewport.ViewId;
        ////////////////////        XYZ boxCenter = viewport.GetBoxCenter();
        ////////////////////        ElementId typeId = viewport.GetTypeId();
        ////////////////////        //View view = doc.ActiveView;
        ////////////////////        using (Transaction t = new Transaction(doc))
        ////////////////////        {
        ////////////////////            t.Start("Delete and Recreate Viewport");
        ////////////////////            sheet.DeleteViewport(viewport);
        ////////////////////            Viewport vvp = Viewport.Create(doc, sheet.Id, viewId, boxCenter);
        ////////////////////            vvp.ChangeTypeId(typeId);
        ////////////////////            t.Commit();
        ////////////////////        }
        ////////////////////    }




        ////////////////////    void Button2Click(object sender, EventArgs e)
        ////////////////////    {
        ////////////////////        if (listBox1.SelectedItem == null)
        ////////////////////        {
        ////////////////////            TaskDialog.Show("List in Item", "Please select an item in the list. Or Cancel.");
        ////////////////////            return;
        ////////////////////        }
        ////////////////////        if (ViewSheet1 == null)
        ////////////////////        {
        ////////////////////            TaskDialog.Show("List in Item", "Please click List button first. Or Cancel.");
        ////////////////////            return;
        ////////////////////        }

        ////////////////////        //	ElementId ViewElementID = ViewportList1[listBox1.SelectedIndex].ViewId;			
        ////////////////////        //	XYZ storetheposition = ViewportList1[listBox1.SelectedIndex].GetBoxCenter();
        ////////////////////        //	ElementId storetypetypeID = ViewportList1[listBox1.SelectedIndex].GetTypeId();			

        ////////////////////        ViewportBringToFront(ViewSheet1, ViewportList1[listBox1.SelectedIndex]);

        ////////////////////        //	Autodesk.Revit.DB.View pView = doc.ActiveView;Autodesk.Revit.DB.Transaction
        ////////////////////        //   t = new Autodesk.Revit.DB.Transaction(doc, "Form_2");
        ////////////////////        //    t.Start();			
        ////////////////////        //	ViewSheet1.DeleteViewport(ViewportList1[listBox1.SelectedIndex]);			
        ////////////////////        //	Viewport vvp = Viewport.Create(doc, ViewSheet1.Id, ViewElementID, storetheposition);
        ////////////////////        //   t.Commit();



        ////////////////////        clear_and_update_the_listview();

        ////////////////////        //	TaskDialog.Show("List in the item", SelectedViewportView.get_Parameter(BuiltInParameter.VIEW_NAME).AsString());
        ////////////////////    }

        ////////////////////    void Button3Click(object sender, EventArgs e)
        ////////////////////    {
        ////////////////////        this.Close();
        ////////////////////    }
        ////////////////////}


        ////////////////////partial class Form1
        ////////////////////{

        ////////////////////    private System.ComponentModel.IContainer components = null;

        ////////////////////    protected override void Dispose(bool disposing)
        ////////////////////    {
        ////////////////////        if (disposing)
        ////////////////////        {
        ////////////////////            if (components != null)
        ////////////////////            {
        ////////////////////                components.Dispose();
        ////////////////////            }
        ////////////////////        }
        ////////////////////        base.Dispose(disposing);
        ////////////////////    }


        ////////////////////    private void InitializeComponent()
        ////////////////////    {
        ////////////////////        this.button1 = new System.Windows.Forms.Button();
        ////////////////////        this.button2 = new System.Windows.Forms.Button();
        ////////////////////        this.button3 = new System.Windows.Forms.Button();
        ////////////////////        //this.SuspendLayout();
        ////////////////////        this.listBox1 = new System.Windows.Forms.ListBox();
        ////////////////////        this.SuspendLayout();
        ////////////////////        // 
        ////////////////////        // listBox1
        ////////////////////        // 
        ////////////////////        this.listBox1.FormattingEnabled = true;
        ////////////////////        this.listBox1.Location = new System.Drawing.Point(5, 5);
        ////////////////////        this.listBox1.Name = "listBox1";
        ////////////////////        this.listBox1.Size = new System.Drawing.Size(500, 150);
        ////////////////////        this.listBox1.TabIndex = 0;

        ////////////////////        // 
        ////////////////////        // button1
        ////////////////////        // 
        ////////////////////        this.button1.Location = new System.Drawing.Point(50, 160);
        ////////////////////        this.button1.Name = "button1";
        ////////////////////        this.button1.Size = new System.Drawing.Size(200, 23);
        ////////////////////        this.button1.TabIndex = 0;
        ////////////////////        this.button1.Text = "List the Viewport in this View";
        ////////////////////        this.button1.UseCompatibleTextRendering = true;
        ////////////////////        this.button1.UseVisualStyleBackColor = true;
        ////////////////////        this.button1.Click += new System.EventHandler(this.Button1Click);
        ////////////////////        // 
        ////////////////////        // button2
        ////////////////////        // 
        ////////////////////        this.button2.Location = new System.Drawing.Point(50, 190);
        ////////////////////        this.button2.Name = "Bring the selected viewport to the front";
        ////////////////////        this.button2.Size = new System.Drawing.Size(200, 23);
        ////////////////////        this.button2.TabIndex = 1;
        ////////////////////        this.button2.Text = "Bring the selected viewport to front";
        ////////////////////        this.button2.UseCompatibleTextRendering = true;
        ////////////////////        this.button2.UseVisualStyleBackColor = true;
        ////////////////////        this.button2.Click += new System.EventHandler(this.Button2Click);
        ////////////////////        // 
        ////////////////////        // button3
        ////////////////////        // 
        ////////////////////        this.button3.Location = new System.Drawing.Point(50, 220);
        ////////////////////        this.button3.Name = "button3";
        ////////////////////        this.button3.Size = new System.Drawing.Size(200, 23);
        ////////////////////        this.button3.TabIndex = 2;
        ////////////////////        this.button3.Text = "Cancel";
        ////////////////////        this.button3.UseCompatibleTextRendering = true;
        ////////////////////        this.button3.UseVisualStyleBackColor = true;
        ////////////////////        this.button3.Click += new System.EventHandler(this.Button3Click);
        ////////////////////        // 
        ////////////////////        // Form1
        ////////////////////        // 
        ////////////////////        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        ////////////////////        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        ////////////////////        this.ClientSize = new System.Drawing.Size(510, 300);
        ////////////////////        this.Controls.Add(this.listBox1);
        ////////////////////        this.Controls.Add(this.button3);
        ////////////////////        this.Controls.Add(this.button2);
        ////////////////////        this.Controls.Add(this.button1);
        ////////////////////        this.Name = "Form1";
        ////////////////////        this.Text = "Form1";
        ////////////////////        this.Load += new System.EventHandler(this.Form1Load);
        ////////////////////        this.ResumeLayout(false);
        ////////////////////    }
        ////////////////////    private System.Windows.Forms.ListBox listBox1;
        ////////////////////    private System.Windows.Forms.Button button3;
        ////////////////////    private System.Windows.Forms.Button button2;
        ////////////////////    private System.Windows.Forms.Button button1;
        ////////////////////}

    }
}
