/*
 * Created by SharpDevelop.
 * User: Joshua
 * Date: 22/12/2016
 * Time: 8:30 p.m.
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Configuration;
using System.Reflection;
using System.Linq;
using System.Diagnostics;

//using System.Configuration.dll;
//using System.Configuration.ConfigurationFileMap;
//using System.Management;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using Form = System.Windows.Forms.Form;

namespace pkRevitMisc.Size_and_Position_Viewport
{
    public partial class Window1548_SizePositionViewport : Form
    {
        ExternalEvent exEvent;
        ExternalEvent exEvent2;
        UIApplication uiapp;
        //static readonly Configuration Config = LoadConfig();
        //Viewport acquiredViewport;
        //Autodesk.Revit.DB.View subjectView;
        MyClass newMyClass = new MyClass();

        public Window1548_SizePositionViewport(UIApplication that)
        {

            //TaskDialog.Show("progress", "we got this far");

            uiapp = that;
            newMyClass.myApplication = that;

            IExternalEventHandler handler_event = new ExternalEventMy();
            exEvent = ExternalEvent.Create(handler_event);

            IExternalEventHandler handler_event2 = new ExternalEventMy2() { newMyClass = newMyClass };
            exEvent2 = ExternalEvent.Create(handler_event2);

            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            InitializeComponent();

            //
            // TODO: Add constructor code after the InitializeComponent() call.
            //
        }



        void Button2Click(object sender, EventArgs e)
        {
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;
            //acquiredViewport = null;	
            newMyClass.myViewport = null;

            int selectionCount = uidoc.Selection.GetElementIds().Count;
            if (selectionCount == 0)
            {
                listBox1.Items.Add("Nothing has been selected");
                listBox1.TopIndex = listBox1.Items.Count - 1;
                return;
            }
            if (selectionCount > 1)
            {
                listBox1.Items.Add("Please only select one item");
                listBox1.TopIndex = listBox1.Items.Count - 1;
                return;
            }

            Element copyViewportHopeful = doc.GetElement(uidoc.Selection.GetElementIds().ToList()[0]);
            if (copyViewportHopeful.Category.Name != "Viewports")
            {
                listBox1.Items.Add("selected entity must be a 'viewport'");
                listBox1.TopIndex = listBox1.Items.Count - 1;
                return;
            }

            listBox1.Items.Add("Success! we have aquired the viewport");
            listBox1.TopIndex = listBox1.Items.Count - 1;
            //	acquiredViewport = copyViewportHopeful as Viewport;

            newMyClass.myViewport = copyViewportHopeful as Viewport;

        }


        void Button1Click(object sender, EventArgs e)
        {
            listBox1.Items.Add("redundant code (afraid to delete the below");
            listBox1.TopIndex = listBox1.Items.Count - 1;
            return;

            /*	UIDocument uidoc = uiapp.ActiveUIDocument;
                Document doc = uidoc.Document;

                Element elementOne = doc.GetElement(new ElementId(1298701));

                FilteredElementCollector col = new FilteredElementCollector(doc)
                    .WhereElementIsNotElementType()
                    .WherePasses(new ElementCategoryFilter(BuiltInCategory.OST_Views))
                    ;

                foreach (Element myElement in col) {
                    dataGridView1.Rows.Add(myElement.Name);
                }


                elementOne.GetParameters("Comments")[0].Set("Hello World2");		

                exEvent.Raise();*/

        }


        void Button3Click(object sender, EventArgs e)
        {
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;
            newMyClass.myView = null;

            int selectionCount = uidoc.Selection.GetElementIds().Count;
            if (selectionCount == 0)
            {
                listBox1.Items.Add("Nothing has been selected");
                listBox1.TopIndex = listBox1.Items.Count - 1;
                return;
            }
            if (selectionCount > 1)
            {
                listBox1.Items.Add("Please only select one item");
                listBox1.TopIndex = listBox1.Items.Count - 1;
                return;
            }

            Element copyViewHopeful = doc.GetElement(uidoc.Selection.GetElementIds().ToList()[0]);
            if (copyViewHopeful.Category.Name != "Views")
            {
                listBox1.Items.Add("selected entity must be a 'view'");
                listBox1.TopIndex = listBox1.Items.Count - 1;
                return;
            }



            //subjectView = copyViewHopeful as Autodesk.Revit.DB.View;
            newMyClass.myView = copyViewHopeful as Autodesk.Revit.DB.View;


            if (newMyClass.myView.GetPrimaryViewId().IntegerValue == -1)
            {
                listBox1.Items.Add("this is not a dependant view");
                listBox1.TopIndex = listBox1.Items.Count - 1;
                newMyClass.myView = null;

                return;
            }



            String sheetnumber = newMyClass.myView.GetParameters("Sheet Number")[0].AsString();

            if (sheetnumber != "---")
            {
                listBox1.Items.Add("view has already been placed on sheet " + sheetnumber);
                listBox1.TopIndex = listBox1.Items.Count - 1;
                newMyClass.myView = null;
                return;
            }



            //subjectView.

            listBox1.Items.Add("Success! we have acquired the view");
            listBox1.TopIndex = listBox1.Items.Count - 1;

        }

        void Button4Click(object sender, EventArgs e)
        {
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;


            if (null == uidoc)
            {
                return; // no document, nothing to do
            }
            if (newMyClass.myViewport == null)
            {
                listBox1.Items.Add("viewport not acquired");
                listBox1.TopIndex = listBox1.Items.Count - 1;
                return;

            }
            if (newMyClass.myView == null)
            {
                listBox1.Items.Add("subject view not acquired");
                listBox1.TopIndex = listBox1.Items.Count - 1;
                return;

            }

            String sheetnumber = newMyClass.myView.GetParameters("Sheet Number")[0].AsString();

            if (sheetnumber != "---")
            {
                listBox1.Items.Add("view has already been placed on sheet " + sheetnumber);
                listBox1.TopIndex = listBox1.Items.Count - 1;
                return;
            }


            if (uidoc.ActiveView.ViewType.ToString() != "DrawingSheet")
            {
                listBox1.Items.Add("please click or pan in a drawing sheet");
                listBox1.TopIndex = listBox1.Items.Count - 1;
                return;
            }

            exEvent2.Raise();
        }


        void Button6Click(object sender, System.EventArgs e)
        {
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;


            if (newMyClass.myView == null)
            {
                listBox1.Items.Add("subject view not acquired");
                listBox1.TopIndex = listBox1.Items.Count - 1;
                return;

            }
            String sheetnumber = newMyClass.myView.GetParameters("Sheet Number")[0].AsString();
            listBox1.Items.Add("the sheet number is " + sheetnumber);
            listBox1.TopIndex = listBox1.Items.Count - 1;

            if (sheetnumber == "---")
            {
                listBox1.Items.Add("we are good to go this can be placed");
                listBox1.TopIndex = listBox1.Items.Count - 1;

            }
        }

    }


        public class MyClass
        {

            public Autodesk.Revit.DB.View myView { get; set; }
            public Viewport myViewport { get; set; }
            public UIApplication myApplication { get; set; }

        }


        public partial class ExternalEventMy : IExternalEventHandler
        {

            public void Execute(UIApplication uiapp)
            {
                UIDocument uidoc = uiapp.ActiveUIDocument;
                Document doc = uidoc.Document;
                if (null == uidoc)
                {
                    return; // no document, nothing to do
                }
                using (Transaction tx = new Transaction(doc))
                {
                    tx.Start("MyEvent");

                    // Action within valid Revit API context thread

                    tx.Commit();
                }
            }

            public string GetName()
            {
                return "External Event Example";
            }
        }


        public partial class ExternalEventMy2 : IExternalEventHandler
        {
            public MyClass newMyClass { get; set; }
            public Window1548_SizePositionViewport newMyForm { get; set; }

            public void Execute(UIApplication uiapp)
            {

                Debug.WriteLine("We got this as far as line 250.");

                UIDocument uidoc = uiapp.ActiveUIDocument;
                Document doc = uidoc.Document;

                Viewport viewportOne = newMyClass.myViewport; //= elementOne as Viewport;
                Autodesk.Revit.DB.View viewOne = doc.GetElement(viewportOne.ViewId) as Autodesk.Revit.DB.View;

                Autodesk.Revit.DB.View viewTwo = newMyClass.myView; //elementTwo as View;

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

                    //TaskDialog.Show("Revit",viewOne.CropBox.Max.X.ToString() + " x offset, offset y " + viewTwo.CropBox.Max.X.ToString());

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

                XYZ ViewportOne_XYZ = viewportOne.GetBoxCenter();

                using (Transaction t = new Transaction(doc))
                {
                    t.Start("Creating the new viewport");
                    //TaskDialog.Show("Revit","we got this far");				

                    Viewport vvp = Viewport.Create(doc, doc.ActiveView.Id, viewTwo.Id, ViewportOne_XYZ);
                    vvp.ChangeTypeId(viewportOne.GetTypeId());



                    //viewOne.CropBoxActive = true;
                    viewOne.AreAnnotationCategoriesHidden = false;
                    ////////viewOne.AreImportCategoriesHidden = false;
                    ////////viewOne.AreAnalyticalModelCategoriesHidden = false;
                    ////////viewOne.AreModelCategoriesHidden = false;
                    ////////viewOne.ArePointCloudsHidden = false;

                    //viewTwo.CropBoxActive = true;
                    viewTwo.AreAnnotationCategoriesHidden = false;
                    //////////viewTwo.AreImportCategoriesHidden = false;
                    //////////viewTwo.AreAnalyticalModelCategoriesHidden = false;
                    //////////viewTwo.AreModelCategoriesHidden = false;
                    //////////viewTwo.ArePointCloudsHidden = false;
                    t.Commit();
                }

                using (Transaction t = new Transaction(doc))
                {
                    t.Start("Add annotations");
                    viewOne.CropBoxActive = true;
                    viewTwo.CropBoxActive = true;

                    t.Commit();
                }
            }

            public string GetName()
            {
                return "External Event Example";
            }
        
    }
}
