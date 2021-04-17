using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace pkRevitRibbon
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.DB.Macros.AddInId("099c0ec3-6fbc-4016-9c2d-5e60fc2a9995")]
    public class SettingUpRibbon : IExternalApplication
    {
        public string dllName { get; set; } = "pkRevitRibbon";
        public string Button_02_Uninstall { get; set; } = "Uninstall";
        public string path { get; set; } = Assembly.GetExecutingAssembly().Location;

        public string TabName { get; set; } = "pkRevit";

        public string PanelName_AddinControl { get; set; } = "Addin Control";
        public string PanelName_ScheduleToExternalDatabase { get; set; } = "Schedule to External Database";
        public string PanelName_Viewport { get; set; } = "Managing Viewports";
        public string PanelName_2DHelpers { get; set; } = "2D Helpers";
        public string PanelName_3DSurface { get; set; } = "Create & Control a Platform";
        public string PanelName_ScheduleHelpers { get; set; } = "Schedule Helpers";
        public string PanelName_FunStuff { get; set; } = "Fun Stuff";

        RibbonPanel RibbonPanelCurrent_AddinControl { get; set; }
        RibbonPanel RibbonPanelCurrent_ScheduleToExternalDatabase { get; set; }
        RibbonPanel RibbonPanelCurrent_Viewport { get; set; }
        RibbonPanel RibbonPanelCurrent_2DHelpers { get; set; }
        RibbonPanel RibbonPanelCurrent_3DSurface { get; set; }
        RibbonPanel RibbonPanelCurrent_ScheduleHelpers { get; set; }
        RibbonPanel RibbonPanelCurrent_FunStuff { get; set; }

        public Result OnShutdown(UIControlledApplication a) { return Result.Succeeded; }

        public Result OnStartup(UIControlledApplication a)
        {
            Properties.Settings.Default.AssemblyNeedLoading = true;
            //Properties.Settings.Default.pkRevitDatasheets_DevLocation = "";
            Properties.Settings.Default.Save();
            Properties.Settings.Default.Reload();

            RibbonSupportMethods myRibbonSupportMethods = new RibbonSupportMethods();
            myRibbonSupportMethods.mySettingUpRibbon = this;

            String exeConfigPath = Path.GetDirectoryName(path) + "\\" + dllName + ".dll";
            string stringCommand01Button = "Set Development Path Root";
            PushButtonData myPushButtonData01 = new PushButtonData(stringCommand01Button, stringCommand01Button, exeConfigPath, dllName + ".InvokeSetDevelopmentPath");

            a.CreateRibbonTab(TabName);
            RibbonPanelCurrent_AddinControl = a.CreateRibbonPanel(TabName, PanelName_AddinControl);
            RibbonPanelCurrent_Viewport = a.CreateRibbonPanel(TabName, PanelName_Viewport);
            RibbonPanelCurrent_2DHelpers = a.CreateRibbonPanel(TabName, PanelName_2DHelpers);
            RibbonPanelCurrent_3DSurface = a.CreateRibbonPanel(TabName, PanelName_3DSurface);
            RibbonPanelCurrent_ScheduleHelpers = a.CreateRibbonPanel(TabName, PanelName_ScheduleHelpers);
            RibbonPanelCurrent_ScheduleToExternalDatabase = a.CreateRibbonPanel(TabName, PanelName_ScheduleToExternalDatabase);
            RibbonPanelCurrent_FunStuff = a.CreateRibbonPanel(TabName, PanelName_FunStuff);


            ComboBoxData cbData = new ComboBoxData("DeveloperSwitch") { ToolTip = "Select an Option", LongDescription = "Select a number or letter" };
            ComboBox ComboBox01 = RibbonPanelCurrent_AddinControl.AddStackedItems(cbData, myPushButtonData01)[0] as ComboBox; //Set Development Path Root

            string stringProductVersion = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\\Pedersen Read Limited\\pkRevit joshnewzealand").GetValue("ProductVersion").ToString();
            ComboBox01.AddItem(new ComboBoxMemberData("Release", "Release: " + stringProductVersion));
            ComboBox01.AddItem(new ComboBoxMemberData("Development", "C# Developer Mode"));
            ComboBox01.Current = ComboBox01.GetItems()[0];
            ComboBox01.CurrentChanged += new EventHandler<Autodesk.Revit.UI.Events.ComboBoxCurrentChangedEventArgs>(SwitchBetweenDeveloperAndRelease);

            RibbonPanelCurrent_AddinControl.AddSeparator();

            #region buttonNames
            //Schedule to External Database
            String ApiButtonName0010 = "0010_pkRevitDatasheets_OpenWindow";
            String ApiButtonText0010 = "Open.\nDB Window";//Store.\nDatasheet            //<-- two places
            //
            String ApiButtonName0020 = "0020_pkRevitDatasheets_WholeSchedule";
            String ApiButtonText0020 = "Externalize\nSelected Schedule";//Store.\nWhole Schedule            //<-- two places
            //
            String ApiButtonName0030 = "0030_OpenParentView";
            String ApiButtonText0030 = "Open\nParent View";//Open.\nParentView            //<-- two places
            //
            String ApiButtonName0040 = "0040_BrintToFront";
            String ApiButtonText0040 = "Bring Viewport\nto Front";//Bring.\nToFront            //<-- two places
            //
            String ApiButtonName0050 = "0050_SizePositionViewport";
            String ApiButtonText0050 = "Size and.\nPosition Viewport";//Size and.\nPosition Viewport            //<-- two places
            //
            String ApiButtonName0060 = "0060_Filters";
            String ApiButtonText0060 = "Filter out.\nspecific family";//Filter out.\nspecific family            //<-- two places
            //
            String ApiButtonName0080 = "0080_Spacers";
            String ApiButtonText0080 = "Half-One-Half\nSpacers";//Half-One-Half.\nSpacers            //<-- two places
            //
            String ApiButtonName0090 = "0090_Lines";
            String ApiButtonText0090 = "Draw\nLines Styles";//Draw.\nLine Styles            //<-- two places
            //
            String ApiButtonName0100 = "0100_DrawArrows";
            String ApiButtonText0100 = "Draw\nArrow Types";//Draw.\nArrow Types            //<-- two places
            //
            String ApiButtonName0110 = "0110_TypesAndTags";
            String ApiButtonText0110 = "Circle out.\nTags or Types";//Circle out.\nTags or Types            //<-- two places
            //
            String ApiButtonName0120 = "0120_MakeAPlatform";
            String ApiButtonText0120 = "Create\nPlatform";//Create Surface.\nwith Reference Point            //<-- two places
            //
            String ApiButtonName0130 = "0130_SelectReferencePoint";
            String ApiButtonText0130 = "Select.\nRef. Point";//Select.\nRef. Point            //<-- two places
            //
            String ApiButtonName0140 = "0140_RotatePlatform";
            String ApiButtonText0140 = "Rotate.\nPlatform";//Rotate.\nSurface            //<-- two places
            //
            String ApiButtonName0150 = "0150_SortOrder";
            String ApiButtonText0150 = "Schedule Manual\nSort Order";//Schedule Manual.\nSort Order            //<-- two places
            //
            String ApiButtonName0160 = "0160_EditSchedule";
            String ApiButtonText0160 = "Edit.\nSchedule";//Edit.\nSchedule            //<-- two places
            //
            String ApiButtonName0170 = "0170_AddFields";
            String ApiButtonText0170 = "Add Parameter\nTo Family";//Add.\nParameters            //<-- two places
            //
            String ApiButtonName0180 = "0180_SmileyFace";
            String ApiButtonText0180 = "Draw 3D\nSmiley Face";//3D.\nSmiley Face            //<-- two places
            //
            String ApiButtonName0190 = "0190_UnderStandingTransforms";
            String ApiButtonText0190 = "Understanding\nTransforms";//Understanding.\nTransform            //<-- two places
            //
            String ApiButtonName0200 = "0200_NurfGun";
            String ApiButtonText0200 = "Nurf Gun\nIntersector Demo";//Nurf.\nGun            //<-- two places
            #endregion

            RibbonPanelCurrent_AddinControl.AddItem(myRibbonSupportMethods.Button02_Uninstall("Button_02_Uninstall", Button_02_Uninstall, path));

            RibbonPanelCurrent_ScheduleToExternalDatabase.AddItem(myRibbonSupportMethods.Button0010_pkRevitDatasheets(ApiButtonName0010, ApiButtonText0010, path));
            RibbonPanelCurrent_ScheduleToExternalDatabase.AddItem(myRibbonSupportMethods.Button0020_pkRevitDatasheets_WholeSchedule(ApiButtonName0020, ApiButtonText0020, path));

            RibbonPanelCurrent_Viewport.AddItem(myRibbonSupportMethods.Button0030_OpenParentView(ApiButtonName0030, ApiButtonText0030, path));
            //RibbonPanelCurrent_Viewport.AddSeparator();
            RibbonPanelCurrent_Viewport.AddItem(myRibbonSupportMethods.Button0040_BringToFront(ApiButtonName0040, ApiButtonText0040, path));
            RibbonPanelCurrent_Viewport.AddItem(myRibbonSupportMethods.Button0050_SizePositionViewport(ApiButtonName0050, ApiButtonText0050, path));
            //RibbonPanelCurrent_Viewport.AddSeparator();
            RibbonPanelCurrent_Viewport.AddItem(myRibbonSupportMethods.Button0060_Filters(ApiButtonName0060, ApiButtonText0060, path));
            //RibbonPanelCurrent.AddItem(myRibbonSupportMethods.Button0070_pkRevitDatasheets_WholeSchedule(ApiButtonName0070, ApiButtonText07, path));

            RibbonPanelCurrent_2DHelpers.AddItem(myRibbonSupportMethods.Button0080_Spacers(ApiButtonName0080, ApiButtonText0080, path));
            RibbonPanelCurrent_2DHelpers.AddItem(myRibbonSupportMethods.Button0090_Lines(ApiButtonName0090, ApiButtonText0090, path));
            RibbonPanelCurrent_2DHelpers.AddItem(myRibbonSupportMethods.Button0100_DrawArrows(ApiButtonName0100, ApiButtonText0100, path));
            //RibbonPanelCurrent_2DHelpers.AddSeparator();
            RibbonPanelCurrent_2DHelpers.AddItem(myRibbonSupportMethods.Button0110_TypesAndTags(ApiButtonName0110, ApiButtonText0110, path));

            RibbonPanelCurrent_3DSurface.AddItem(myRibbonSupportMethods.Button0120_MakeAPlatform(ApiButtonName0120, ApiButtonText0120, path));
            RibbonPanelCurrent_3DSurface.AddItem(myRibbonSupportMethods.Button0130_SelectReferencePoint(ApiButtonName0130, ApiButtonText0130, path));
            RibbonPanelCurrent_3DSurface.AddItem(myRibbonSupportMethods.Button0140_RotatePlatform(ApiButtonName0140, ApiButtonText0140, path));

            RibbonPanelCurrent_ScheduleHelpers.AddItem(myRibbonSupportMethods.Button0150_SortOrder(ApiButtonName0150, ApiButtonText0150, path));
            RibbonPanelCurrent_ScheduleHelpers.AddItem(myRibbonSupportMethods.Button0160_EditSchedule(ApiButtonName0160, ApiButtonText0160, path));
            //RibbonPanelCurrent_ScheduleHelpers.AddSeparator();
            RibbonPanelCurrent_ScheduleHelpers.AddItem(myRibbonSupportMethods.Button0170_AddFields(ApiButtonName0170, ApiButtonText0170, path));

            RibbonPanelCurrent_FunStuff.AddItem(myRibbonSupportMethods.Button0180_SmileyFace(ApiButtonName0180, ApiButtonText0180, path));
            RibbonPanelCurrent_FunStuff.AddItem(myRibbonSupportMethods.Button0190_UnderStandingTransforms(ApiButtonName0190, ApiButtonText0190, path));
            RibbonPanelCurrent_FunStuff.AddItem(myRibbonSupportMethods.Button0200_NurfGun(ApiButtonName0200, ApiButtonText0200, path));

            return Result.Succeeded;
        }


        public void SwitchBetweenDeveloperAndRelease(object sender, Autodesk.Revit.UI.Events.ComboBoxCurrentChangedEventArgs e)
        {
            try
            {
                ComboBox cBox = sender as ComboBox;

                PushButton pushbutton_0010 = RibbonPanelCurrent_ScheduleToExternalDatabase.GetItems().Where(x => x.Name == "0010_pkRevitDatasheets_OpenWindow").First() as PushButton;
                if (cBox.Current.Name == "Development") pushbutton_0010.ClassName = dllName + ".DevInvoke_0010_pkRevitDatasheets";  
                if (cBox.Current.Name == "Release") pushbutton_0010.ClassName = dllName + ".Invoke_0010_pkRevitDatasheets";  

                PushButton pushbutton_0020 = RibbonPanelCurrent_ScheduleToExternalDatabase.GetItems().Where(x => x.Name == "0020_pkRevitDatasheets_WholeSchedule").First() as PushButton;
                if (cBox.Current.Name == "Development") pushbutton_0020.ClassName = dllName + ".DevInvoke_0020_pkRevitDatasheets_WholeSchedule"; 
                if (cBox.Current.Name == "Release") pushbutton_0020.ClassName = dllName + ".Invoke_0020_pkRevitDatasheets_WholeSchedule"; 

                PushButton pushbutton_0030 = RibbonPanelCurrent_Viewport.GetItems().Where(x => x.Name == "0030_OpenParentView").First() as PushButton; 
                if (cBox.Current.Name == "Development") pushbutton_0030.ClassName = dllName + ".DevInvoke_0030_pkRevitMisc_OpenParentView";  
                if (cBox.Current.Name == "Release") pushbutton_0030.ClassName = dllName + ".Invoke_0030_pkRevitMisc_OpenParentView"; 

                PushButton pushbutton_0040 = RibbonPanelCurrent_Viewport.GetItems().Where(x => x.Name == "0040_BrintToFront").First() as PushButton;  
                if (cBox.Current.Name == "Development") pushbutton_0040.ClassName = dllName + ".DevInvoke_0040_pkRevitMisc_BrintToFront"; 
                if (cBox.Current.Name == "Release") pushbutton_0040.ClassName = dllName + ".Invoke_0040_pkRevitMisc_BrintToFront";

                PushButton pushbutton_0050 = RibbonPanelCurrent_Viewport.GetItems().Where(x => x.Name == "0050_SizePositionViewport").First() as PushButton;
                if (cBox.Current.Name == "Development") pushbutton_0050.ClassName = dllName + ".DevInvoke_0050_pkRevitMisc_SizePositionViewport";
                if (cBox.Current.Name == "Release") pushbutton_0050.ClassName = dllName + ".Invoke_0050_pkRevitMisc_SizePositionViewport";

                PushButton pushbutton_0060 = RibbonPanelCurrent_Viewport.GetItems().Where(x => x.Name == "0060_Filters").First() as PushButton;
                if (cBox.Current.Name == "Development") pushbutton_0060.ClassName = dllName + ".DevInvoke_0060_pkRevitMisc_Filters";
                if (cBox.Current.Name == "Release") pushbutton_0060.ClassName = dllName + ".Invoke_0060_pkRevitMisc_Filters";

                PushButton pushbutton_0080 = RibbonPanelCurrent_2DHelpers.GetItems().Where(x => x.Name == "0080_Spacers").First() as PushButton;
                if (cBox.Current.Name == "Development") pushbutton_0080.ClassName = dllName + ".DevInvoke_0080_pkRevitMisc_Spacers";
                if (cBox.Current.Name == "Release") pushbutton_0080.ClassName = dllName + ".Invoke_0080_pkRevitMisc_Spacers";

                PushButton pushbutton_0090 = RibbonPanelCurrent_2DHelpers.GetItems().Where(x => x.Name == "0090_Lines").First() as PushButton;
                if (cBox.Current.Name == "Development") pushbutton_0090.ClassName = dllName + ".DevInvoke_0090_pkRevitMisc_Lines";
                if (cBox.Current.Name == "Release") pushbutton_0090.ClassName = dllName + ".Invoke_0090_pkRevitMisc_Lines";

                PushButton pushbutton_0100 = RibbonPanelCurrent_2DHelpers.GetItems().Where(x => x.Name == "0100_DrawArrows").First() as PushButton;
                if (cBox.Current.Name == "Development") pushbutton_0100.ClassName = dllName + ".DevInvoke_0100_pkRevitMisc_DrawArrows";
                if (cBox.Current.Name == "Release") pushbutton_0100.ClassName = dllName + ".Invoke_0100_pkRevitMisc_DrawArrows";

                PushButton pushbutton_0110 = RibbonPanelCurrent_2DHelpers.GetItems().Where(x => x.Name == "0110_TypesAndTags").First() as PushButton;
                if (cBox.Current.Name == "Development") pushbutton_0110.ClassName = dllName + ".DevInvoke_0110_pkRevitMisc_TypesAndTags";
                if (cBox.Current.Name == "Release") pushbutton_0110.ClassName = dllName + ".Invoke_0110_pkRevitMisc_TypesAndTags";

                PushButton pushbutton_0120 = RibbonPanelCurrent_3DSurface.GetItems().Where(x => x.Name == "0120_MakeAPlatform").First() as PushButton;
                if (cBox.Current.Name == "Development") pushbutton_0120.ClassName = dllName + ".DevInvoke_0120_pkRevitMisc_MakeAPlatform";
                if (cBox.Current.Name == "Release") pushbutton_0120.ClassName = dllName + ".Invoke_0120_pkRevitMisc_MakeAPlatform";

                PushButton pushbutton_0130 = RibbonPanelCurrent_3DSurface.GetItems().Where(x => x.Name == "0130_SelectReferencePoint").First() as PushButton;
                if (cBox.Current.Name == "Development") pushbutton_0130.ClassName = dllName + ".DevInvoke_0130_pkRevitMisc_SelectReferencePoint";
                if (cBox.Current.Name == "Release") pushbutton_0130.ClassName = dllName + ".Invoke_0130_pkRevitMisc_SelectReferencePoint";

                PushButton pushbutton_0140 = RibbonPanelCurrent_3DSurface.GetItems().Where(x => x.Name == "0140_RotatePlatform").First() as PushButton;
                if (cBox.Current.Name == "Development") pushbutton_0140.ClassName = dllName + ".DevInvoke_0140_pkRevitMisc_RotatePlatform";
                if (cBox.Current.Name == "Release") pushbutton_0140.ClassName = dllName + ".Invoke_0140_pkRevitMisc_RotatePlatform";

                PushButton pushbutton_0150 = RibbonPanelCurrent_ScheduleHelpers.GetItems().Where(x => x.Name == "0150_SortOrder").First() as PushButton;
                if (cBox.Current.Name == "Development") pushbutton_0150.ClassName = dllName + ".DevInvoke_0150_pkRevitMisc_SortOrder";
                if (cBox.Current.Name == "Release") pushbutton_0150.ClassName = dllName + ".Invoke_0150_pkRevitMisc_SortOrder";

                PushButton pushbutton_0160 = RibbonPanelCurrent_ScheduleHelpers.GetItems().Where(x => x.Name == "0160_EditSchedule").First() as PushButton;
                if (cBox.Current.Name == "Development") pushbutton_0160.ClassName = dllName + ".DevInvoke_0160_pkRevitMisc_EditSchedule";
                if (cBox.Current.Name == "Release") pushbutton_0160.ClassName = dllName + ".Invoke_0160_pkRevitMisc_EditSchedule";

                PushButton pushbutton_0170 = RibbonPanelCurrent_ScheduleHelpers.GetItems().Where(x => x.Name == "0170_AddFields").First() as PushButton;
                if (cBox.Current.Name == "Development") pushbutton_0170.ClassName = dllName + ".DevInvoke_0170_pkRevitMisc_AddFields";
                if (cBox.Current.Name == "Release") pushbutton_0170.ClassName = dllName + ".Invoke_0170_pkRevitMisc_AddFields";

                PushButton pushbutton_0180 = RibbonPanelCurrent_FunStuff.GetItems().Where(x => x.Name == "0180_SmileyFace").First() as PushButton;
                if (cBox.Current.Name == "Development") pushbutton_0180.ClassName = dllName + ".DevInvoke_0180_pkRevitMisc_SmileyFace";
                if (cBox.Current.Name == "Release") pushbutton_0180.ClassName = dllName + ".Invoke_0180_pkRevitMisc_SmileyFace";

                PushButton pushbutton_0190 = RibbonPanelCurrent_FunStuff.GetItems().Where(x => x.Name == "0190_UnderStandingTransforms").First() as PushButton;
                if (cBox.Current.Name == "Development") pushbutton_0190.ClassName = dllName + ".DevInvoke_0190_pkRevitMisc_UnderStandingTransforms";
                if (cBox.Current.Name == "Release") pushbutton_0190.ClassName = dllName + ".Invoke_0190_pkRevitMisc_UnderStandingTransforms";

                PushButton pushbutton_0200 = RibbonPanelCurrent_FunStuff.GetItems().Where(x => x.Name == "0200_NurfGun").First() as PushButton;
                if (cBox.Current.Name == "Development") pushbutton_0200.ClassName = dllName + ".DevInvoke_0200_pkRevitMisc_NurfGun";
                if (cBox.Current.Name == "Release") pushbutton_0200.ClassName = dllName + ".Invoke_0200_pkRevitMisc_NurfGun";


            }

            #region catch and finally
            catch (Exception ex)
            {
                RibbonSupportMethods.writeDebug("SwitchBetweenDeveloperAndRelease" + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }

    }
}