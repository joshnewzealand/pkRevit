using System;
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
using System.Windows.Threading;
using RevitTransformSliders;
using pkRevitUnderstandingTransforms.External_Events;

namespace pkRevitUnderstandingTransforms
{

    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class EE06_PlaceAFamily_CentreScreen : IExternalEventHandler  //this is the last when one making a checklist change, EE4 must be just for when an element is new
    {
        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);



      /////////////////  public bool myBool_MessageBoxAboutFamilyRevisionSystem = true;
                   
                                 

        public Window_RotatePlatform myWindow1 { get; set; }

        void OnDocumentChanged(object sender, DocumentChangedEventArgs e)
        {
            try
            {
                UIDocument uidoc = uiapp.ActiveUIDocument;
                Document doc = uidoc.Document;

                if (e.GetAddedElementIds().Count == 0 & e.GetModifiedElementIds().Count == 0) return;

                if (e.GetAddedElementIds().Count > 0)
                {
                    Element myElement = doc.GetElement(e.GetAddedElementIds().First());

                    if (myElement.GetType().Name != "FamilyInstance") return;

                    //uidoc.Application.Application.DocumentChanged -= new EventHandler<DocumentChangedEventArgs>(OnDocumentChanged);
                }

                if (e.GetModifiedElementIds().Count > 0)
                {
                    Element myElement = doc.GetElement(e.GetModifiedElementIds().First());

                    if (myElement.GetType().Name != "FamilyInstance") return;

                    ICollection<ElementId> icollection = e.GetModifiedElementIds();

                    myWindow1.Dispatcher.Invoke(async () =>
                    {
                        SetForegroundWindow(uidoc.Application.MainWindowHandle); //this is an excape event
                        keybd_event(0x1B, 0, 0, 0);
                        keybd_event(0x1B, 0, 2, 0);
                        keybd_event(0x1B, 0, 0, 0);
                        keybd_event(0x1B, 0, 2, 0);

                        await Task.Delay(1000);

                      //  ReferencePoint myRefPoint = doc.GetElement(new ElementId(myIntUpDown_Middle2.Value.Value)) as ReferencePoint;

                        uidoc.Selection.SetElementIds(new List<ElementId>() { icollection.ElementAt(0) });

                        //MessageBox.Show("Hello world");

                        myWindow1.updateMiddle2_UpdateUI();
                    });

                    //uidoc.Selection.SetElementIds(new List<ElementId>() { doc.GetElement(icollection.ElementAt(2)).Id });
                }


                uidoc.Application.Application.DocumentChanged -= new EventHandler<DocumentChangedEventArgs>(OnDocumentChanged);

                ////SetForegroundWindow(uidoc.Application.MainWindowHandle); //this is an excape event
                ////keybd_event(0x1B, 0, 0, 0);
                ////keybd_event(0x1B, 0, 2, 0);
                ////keybd_event(0x1B, 0, 0, 0);
                ////keybd_event(0x1B, 0, 2, 0);

            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("OnDocumentChanged" + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

        UIApplication uiapp;


        public void Execute(UIApplication uiappp)
        {
            uiapp = uiappp;

            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;


            //////////////uidoc.Application.Application.DocumentChanged += new EventHandler<DocumentChangedEventArgs>(OnDocumentChanged);

            PromptForFamilyInstancePlacementOptions myPromptForFamilyInstancePlacementOptions = new PromptForFamilyInstancePlacementOptions();
            try
            {
                
                Centre_of_Screen_Static.CentreOfScreen(myWindow1.toavoidloadingrevitdlls);

                myWindow1.updateMiddle2_UpdateUI();
            }


            #region catch and finally
            catch (Exception ex)
            {
                if (ex.Message != "The user aborted the pick operation.")
                {
                    _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("EE06_PlaceAFamily_CentreScreen" + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);

                }
                else
                {
                    uidoc.Application.Application.DocumentChanged -= new EventHandler<DocumentChangedEventArgs>(OnDocumentChanged);
                }
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
