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
        public string PanelName { get; set; } = "Store Datasheets";

        RibbonPanel RibbonPanelCurrent { get; set; }

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
            RibbonPanelCurrent = a.CreateRibbonPanel(TabName, PanelName);


            ComboBoxData cbData = new ComboBoxData("DeveloperSwitch") { ToolTip = "Select an Option", LongDescription = "Select a number or letter" };
            ComboBox ComboBox01 = RibbonPanelCurrent.AddStackedItems(cbData, myPushButtonData01)[0] as ComboBox; //Set Development Path Root

            string stringProductVersion = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\\Pedersen Read Limited\\pkRevit joshnewzealand").GetValue("ProductVersion").ToString();
            ComboBox01.AddItem(new ComboBoxMemberData("Release", "Release: " + stringProductVersion));
            ComboBox01.AddItem(new ComboBoxMemberData("Development", "C# Developer Mode"));
            ComboBox01.CurrentChanged += new EventHandler<Autodesk.Revit.UI.Events.ComboBoxCurrentChangedEventArgs>(SwitchBetweenDeveloperAndRelease);

            RibbonPanelCurrent.AddSeparator();


            String ApiButtonName01 = "0010_pkRevitDatasheets";//Store.\nDatasheet                  //<-- two places
            String ApiButtonText01 = "Store.\nDatasheet";//Store.\nDatasheet            //<-- two places

            RibbonPanelCurrent.AddItem(myRibbonSupportMethods.Button02_Uninstall("Button_02_Uninstall", Button_02_Uninstall, path));
            RibbonPanelCurrent.AddItem(myRibbonSupportMethods.Button0010_pkRevitDatasheets(ApiButtonName01, ApiButtonText01, path));

            return Result.Succeeded;
        }


        public void SwitchBetweenDeveloperAndRelease(object sender, Autodesk.Revit.UI.Events.ComboBoxCurrentChangedEventArgs e)
        {
            try
            {
                ComboBox cBox = sender as ComboBox;

                PushButton pushbutton_0010_pkRevitDatasheets = RibbonPanelCurrent.GetItems().Where(x => x.Name == "0010_pkRevitDatasheets").First() as PushButton;  //0010_pkRevitDatasheets

                if (cBox.Current.Name == "Development") pushbutton_0010_pkRevitDatasheets.ClassName = dllName + ".DevInvoke_0010_pkRevitDatasheets";  //0010_pkRevitDatasheets

                if (cBox.Current.Name == "Release") pushbutton_0010_pkRevitDatasheets.ClassName = dllName + ".Invoke_0010_pkRevitDatasheets";  //0010_pkRevitDatasheets


                if (false) //delete all this
                {
                    string FILE_NAME = System.Environment.GetEnvironmentVariable("ProgramData") + "\\Pedersen Read Limited"; // cSharpPlaypen joshnewzealand

                    if (true) //grouping for clarity will alwasy be true
                    {
                        if (!System.IO.Directory.Exists(FILE_NAME)) System.IO.Directory.CreateDirectory(FILE_NAME);
                        FILE_NAME = FILE_NAME + "\\cSharpPlaypen joshnewzealand"; // 
                        if (!System.IO.Directory.Exists(FILE_NAME)) System.IO.Directory.CreateDirectory(FILE_NAME);
                        FILE_NAME = (FILE_NAME + "\\Location Of Shared Parameters File.txt");
                    }

                    if (true) //write line
                    {
                        string path = "";
                        if (cBox.Current.Name == "Release")
                        {
                            path = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\\Pedersen Read Limited\\pkRevit joshnewzealand").GetValue("TARGETDIR").ToString();
                        }
                        if (cBox.Current.Name == "Development")
                        {
                            string dllModuleName = "pkRevitRibbon";
                            path = Properties.Settings.Default.DevelopmentPathRoot + "\\" + dllModuleName + "\\AddIn";
                        }

                        System.IO.File.Create(FILE_NAME).Dispose();
                        System.IO.StreamWriter objWriter = new System.IO.StreamWriter(FILE_NAME, true);
                        objWriter.WriteLine(path);
                        objWriter.Close();
                    }
                } //delete all this


            }

            #region catch and finally
            catch (Exception ex)
            {
                TaskDialog.Show("Catch", "Failed due to: " + ex.Message);
            }
            finally
            {
            }
            #endregion
        }

    }
}