
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit.UI;

using System.Reflection;
using System.IO;
using System.Windows.Media.Imaging;
//using adWin = Autodesk.Windows;

using TaskDialog = Autodesk.Revit.UI.TaskDialog;
using Autodesk.Revit.DB;
using System.Windows;

namespace pkRevitRibbon
{
    public class Availability : IExternalCommandAvailability
    {
        public bool IsCommandAvailable(UIApplication a, CategorySet b)
        {
            return true;
        }
    }

    public class RibbonSupportMethods
    {
        public SettingUpRibbon mySettingUpRibbon { get; set; }

        public static void loadPackages()
        {
            //2 August 2019: Start, The the following lines were added in Take 10 in order prevent double loading of packages.
            Microsoft.Win32.RegistryKey rkbase = null;
            rkbase = Microsoft.Win32.RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, Microsoft.Win32.RegistryView.Registry64);
            string stringTargetOokiiVersion = rkbase.OpenSubKey("SOFTWARE\\Wow6432Node\\Pedersen Read Limited\\pkRevit joshnewzealand").GetValue("OokiiVersion").ToString();
            string stringTargetXceedVersion = rkbase.OpenSubKey("SOFTWARE\\Wow6432Node\\Pedersen Read Limited\\pkRevit joshnewzealand").GetValue("XceedVersion").ToString();
            string stringTargetSQLiteVersion = rkbase.OpenSubKey("SOFTWARE\\Wow6432Node\\Pedersen Read Limited\\pkRevit joshnewzealand").GetValue("SQLite").ToString();

            string stringWindowsAPICodePack = rkbase.OpenSubKey("SOFTWARE\\Wow6432Node\\Pedersen Read Limited\\pkRevit joshnewzealand").GetValue("WindowsAPICodePack").ToString();
            string stringWindowsAPICodePackShell = rkbase.OpenSubKey("SOFTWARE\\Wow6432Node\\Pedersen Read Limited\\pkRevit joshnewzealand").GetValue("WindowsAPICodePackShell").ToString();

            string path = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\\Josh New Zealand\\pkRevit").GetValue("Path").ToString();

            if (AppDomain.CurrentDomain.GetAssemblies().Where(x => x.FullName == stringTargetOokiiVersion).Count() == 0)
            {
                //string stringTargetDirectory = rkbase.OpenSubKey("SOFTWARE\\Pedersen Read Limited\\pkRevit joshnewzealand").GetValue("TARGETDIR").ToString();
                Assembly.Load(File.ReadAllBytes(path + "\\Ookii.Dialogs.Wpf.dll"));
            }
            if (AppDomain.CurrentDomain.GetAssemblies().Where(x => x.FullName == stringTargetXceedVersion).Count() == 0)
            {
                //string stringTargetDirectory = rkbase.OpenSubKey("SOFTWARE\\Pedersen Read Limited\\pkRevit joshnewzealand").GetValue("TARGETDIR").ToString();
                Assembly.Load(File.ReadAllBytes(path + "\\Xceed.Wpf.Toolkit.dll"));
            }
            if (AppDomain.CurrentDomain.GetAssemblies().Where(x => x.FullName == stringTargetSQLiteVersion).Count() == 0)
            {
                //string stringTargetDirectory = rkbase.OpenSubKey("SOFTWARE\\Pedersen Read Limited\\pkRevit joshnewzealand").GetValue("TARGETDIR").ToString();
                Assembly.Load(File.ReadAllBytes(path + "\\System.Data.SQLite.dll"));
            }
            if (AppDomain.CurrentDomain.GetAssemblies().Where(x => x.FullName == stringWindowsAPICodePack).Count() == 0)
            {
                //string stringTargetDirectory = rkbase.OpenSubKey("SOFTWARE\\Pedersen Read Limited\\pkRevit joshnewzealand").GetValue("TARGETDIR").ToString();
                Assembly.Load(File.ReadAllBytes(path + "\\Microsoft.WindowsAPICodePack.dll"));
            }
            if (AppDomain.CurrentDomain.GetAssemblies().Where(x => x.FullName == stringWindowsAPICodePackShell).Count() == 0)
            {
               // string stringTargetDirectory = rkbase.OpenSubKey("SOFTWARE\\Pedersen Read Limited\\pkRevit joshnewzealand").GetValue("TARGETDIR").ToString();
                Assembly.Load(File.ReadAllBytes(path + "\\Microsoft.WindowsAPICodePack.Shell.dll"));
            }
            //2 August 2019: End.

            Properties.Settings.Default.AssemblyNeedLoading = false;
            Properties.Settings.Default.Save();
        }


        public static string Method_InitialDirectory_Go_NoGo(string myString_TestFileLocation)
        {
            string myString_InitialDirectory = "shell:MyComputerFolder";

            if (System.IO.File.Exists(myString_TestFileLocation))
            {
                MessageBoxResult result = System.Windows.MessageBox.Show("Please manually locate file '" + myString_TestFileLocation + "'. Do you want to continue.", "Continue...", System.Windows.MessageBoxButton.YesNoCancel);

                if (result != MessageBoxResult.Yes) return null;

                myString_InitialDirectory = System.IO.Path.GetDirectoryName(myString_TestFileLocation);
            }
            return myString_InitialDirectory;
        }

        public static Microsoft.Win32.OpenFileDialog MethodToSetDevelopmentPath_Individually(string myString_InitialDirectory, string myString_Binary)
        {
            System.Windows.MessageBox.Show(@"Please navigate to your projects 'bin\Debug' OR 'Addin' directory.");

            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter = myString_Binary + "|" + myString_Binary; //filter files by extension

            dlg.InitialDirectory = myString_InitialDirectory;

            return dlg;
        }


        public string exeConfigPath(string path)
        {
            return Path.GetDirectoryName(path) + "\\" + mySettingUpRibbon.dllName + ".dll";
        }

        public PushButtonData Button02_Uninstall(string Name, string ChecklistsNumber, string path)
        {
            PushButtonData myPushButtonData = new PushButtonData(Name, ChecklistsNumber, exeConfigPath(path), mySettingUpRibbon.dllName + ".Invoke02_Uninstall");
            myPushButtonData.AvailabilityClassName = "pkRevitRibbon.Availability";
            myPushButtonData.LargeImage = new BitmapImage(new Uri(Path.Combine(Path.GetDirectoryName(path) + "\\Images\\013 Button Image Uninstall.png"), UriKind.Absolute));
            return myPushButtonData;
        }

        public PushButtonData Button0010_pkRevitDatasheets(string Name, string ChecklistsNumber, string path)
        {
            PushButtonData myPushButtonData = new PushButtonData(Name, ChecklistsNumber, exeConfigPath(path), mySettingUpRibbon.dllName + ".Invoke_0010_pkRevitDatasheets");
            myPushButtonData.LargeImage = new BitmapImage(new Uri(Path.Combine(Path.GetDirectoryName(path) + "\\Images\\Button0020_pkRevitDatasheets_WholeSchedule.png"), UriKind.Absolute));
            return myPushButtonData;
        }

        public PushButtonData Button0020_pkRevitDatasheets_WholeSchedule(string Name, string ChecklistsNumber, string path)
        {
            PushButtonData myPushButtonData = new PushButtonData(Name, ChecklistsNumber, exeConfigPath(path), mySettingUpRibbon.dllName + ".Invoke_0020_pkRevitDatasheets_WholeSchedule");
            myPushButtonData.LargeImage = new BitmapImage(new Uri(Path.Combine(Path.GetDirectoryName(path) + "\\Images\\Button0020_pkRevitDatasheets_WholeSchedule-2.png"), UriKind.Absolute));
            return myPushButtonData;
        }

        public PushButtonData Button0030_OpenParentView(string Name, string ChecklistsNumber, string path)
        {
            PushButtonData myPushButtonData = new PushButtonData(Name, ChecklistsNumber, exeConfigPath(path), mySettingUpRibbon.dllName + ".Invoke_0030_pkRevitMisc_OpenParentView");
            myPushButtonData.LargeImage = new BitmapImage(new Uri(Path.Combine(Path.GetDirectoryName(path) + "\\Images\\Button0030_OpenParent.png"), UriKind.Absolute));
            return myPushButtonData;
        }

        public PushButtonData Button0040_BringToFront(string Name, string ChecklistsNumber, string path)
        {
            PushButtonData myPushButtonData = new PushButtonData(Name, ChecklistsNumber, exeConfigPath(path), mySettingUpRibbon.dllName + ".Invoke_0040_pkRevit_WM_BrintToFront");
            myPushButtonData.LargeImage = new BitmapImage(new Uri(Path.Combine(Path.GetDirectoryName(path) + "\\Images\\Button0040_BringToFront.png"), UriKind.Absolute));
            return myPushButtonData;
        }

        public PushButtonData Button0050_SizePositionViewport(string Name, string ChecklistsNumber, string path)
        {
            PushButtonData myPushButtonData = new PushButtonData(Name, ChecklistsNumber, exeConfigPath(path), mySettingUpRibbon.dllName + ".Invoke_0050_pkRevit_WM_SizePositionViewport");
            myPushButtonData.LargeImage = new BitmapImage(new Uri(Path.Combine(Path.GetDirectoryName(path) + "\\Images\\Button0050_SizePositionViewport.png"), UriKind.Absolute));
            return myPushButtonData;
        }

        public PushButtonData Button0060_Filters(string Name, string ChecklistsNumber, string path)
        {
            PushButtonData myPushButtonData = new PushButtonData(Name, ChecklistsNumber, exeConfigPath(path), mySettingUpRibbon.dllName + ".Invoke_0060_pkRevitMisc_Filters");
            myPushButtonData.LargeImage = new BitmapImage(new Uri(Path.Combine(Path.GetDirectoryName(path) + "\\Images\\Button0060_Filters.png"), UriKind.Absolute));
            return myPushButtonData;
        }

        public PushButtonData Button0080_Spacers(string Name, string ChecklistsNumber, string path)
        {
            PushButtonData myPushButtonData = new PushButtonData(Name, ChecklistsNumber, exeConfigPath(path), mySettingUpRibbon.dllName + ".Invoke_0080_pkRevitMisc_Spacers");
            myPushButtonData.LargeImage = new BitmapImage(new Uri(Path.Combine(Path.GetDirectoryName(path) + "\\Images\\Button0080_Spacers.png"), UriKind.Absolute));
            return myPushButtonData;
        }

        public PushButtonData Button0090_Lines(string Name, string ChecklistsNumber, string path)
        {
            PushButtonData myPushButtonData = new PushButtonData(Name, ChecklistsNumber, exeConfigPath(path), mySettingUpRibbon.dllName + ".Invoke_0090_pkRevitMisc_Lines");
            myPushButtonData.LargeImage = new BitmapImage(new Uri(Path.Combine(Path.GetDirectoryName(path) + "\\Images\\Button0090_Lines.png"), UriKind.Absolute));
            return myPushButtonData;
        }

        public PushButtonData Button0090_LinesPatterns(string Name, string ChecklistsNumber, string path)
        {
            PushButtonData myPushButtonData = new PushButtonData(Name, ChecklistsNumber, exeConfigPath(path), mySettingUpRibbon.dllName + ".Invoke_0090_pkRevitMisc_LinesPatterns");
            myPushButtonData.LargeImage = new BitmapImage(new Uri(Path.Combine(Path.GetDirectoryName(path) + "\\Images\\drawpatterns.png"), UriKind.Absolute));
            return myPushButtonData;
        }

        public PushButtonData Button0090_LinesWeights(string Name, string ChecklistsNumber, string path)
        {
            PushButtonData myPushButtonData = new PushButtonData(Name, ChecklistsNumber, exeConfigPath(path), mySettingUpRibbon.dllName + ".Invoke_0090_pkRevitMisc_LinesWeights");
            myPushButtonData.LargeImage = new BitmapImage(new Uri(Path.Combine(Path.GetDirectoryName(path) + "\\Images\\drawweights.png"), UriKind.Absolute));
            return myPushButtonData;
        }

        public PushButtonData Button0090_Walls(string Name, string ChecklistsNumber, string path)
        {
            PushButtonData myPushButtonData = new PushButtonData(Name, ChecklistsNumber, exeConfigPath(path), mySettingUpRibbon.dllName + ".Invoke_0090_pkRevitMisc_Walls");
            myPushButtonData.LargeImage = new BitmapImage(new Uri(Path.Combine(Path.GetDirectoryName(path) + "\\Images\\Button0090_Walls.png"), UriKind.Absolute));
            return myPushButtonData;
        }

        public PushButtonData Button0090_FilledRegions(string Name, string ChecklistsNumber, string path)
        {
            PushButtonData myPushButtonData = new PushButtonData(Name, ChecklistsNumber, exeConfigPath(path), mySettingUpRibbon.dllName + ".Invoke_0090_pkRevitMisc_FilledRegions");  //StartMethod_0090_FilledRegions
            myPushButtonData.LargeImage = new BitmapImage(new Uri(Path.Combine(Path.GetDirectoryName(path) + "\\Images\\drawfillpatterns.png"), UriKind.Absolute));
            return myPushButtonData;
        }


        public PushButtonData Button0100_DrawArrows(string Name, string ChecklistsNumber, string path)
        {
            PushButtonData myPushButtonData = new PushButtonData(Name, ChecklistsNumber, exeConfigPath(path), mySettingUpRibbon.dllName + ".Invoke_0100_pkRevitMisc_DrawArrows");
            myPushButtonData.LargeImage = new BitmapImage(new Uri(Path.Combine(Path.GetDirectoryName(path) + "\\Images\\Button0100_DrawArrows.png"), UriKind.Absolute));
            return myPushButtonData;
        }

        public PushButtonData Button0110_TypesAndTags(string Name, string ChecklistsNumber, string path)
        {
            PushButtonData myPushButtonData = new PushButtonData(Name, ChecklistsNumber, exeConfigPath(path), mySettingUpRibbon.dllName + ".Invoke_0110_pkRevitMisc_TypesAndTags");
            myPushButtonData.LargeImage = new BitmapImage(new Uri(Path.Combine(Path.GetDirectoryName(path) + "\\Images\\Button0110_TypesAndTags2.png"), UriKind.Absolute));
            return myPushButtonData;
        }

        public PushButtonData Button0120_MakeAPlatform(string Name, string ChecklistsNumber, string path)
        {
            PushButtonData myPushButtonData = new PushButtonData(Name, ChecklistsNumber, exeConfigPath(path), mySettingUpRibbon.dllName + ".Invoke_0120_pkRevitMisc_MakeAPlatform");
            myPushButtonData.LargeImage = new BitmapImage(new Uri(Path.Combine(Path.GetDirectoryName(path) + "\\Images\\Button0120_MakeAPlatform.png"), UriKind.Absolute));
            return myPushButtonData;
        }
        public PushButtonData Button0130_SelectReferencePoint(string Name, string ChecklistsNumber, string path)
        {
            PushButtonData myPushButtonData = new PushButtonData(Name, ChecklistsNumber, exeConfigPath(path), mySettingUpRibbon.dllName + ".Invoke_0130_pkRevitMisc_SelectReferencePoint");
            myPushButtonData.LargeImage = new BitmapImage(new Uri(Path.Combine(Path.GetDirectoryName(path) + "\\Images\\Button0130_SelectReferencePoint.png"), UriKind.Absolute));
            return myPushButtonData;
        }

        public PushButtonData Button0140_RotatePlatform(string Name, string ChecklistsNumber, string path)
        {
            PushButtonData myPushButtonData = new PushButtonData(Name, ChecklistsNumber, exeConfigPath(path), mySettingUpRibbon.dllName + ".Invoke_0140_pkRevitMisc_RotatePlatform");
            myPushButtonData.LargeImage = new BitmapImage(new Uri(Path.Combine(Path.GetDirectoryName(path) + "\\Images\\Button0140_RotatePlatform.png"), UriKind.Absolute));
            return myPushButtonData;
        }

        public PushButtonData Button0150_SortOrder(string Name, string ChecklistsNumber, string path)
        {
            PushButtonData myPushButtonData = new PushButtonData(Name, ChecklistsNumber, exeConfigPath(path), mySettingUpRibbon.dllName + ".Invoke_0150_pkRevitMisc_SortOrder");
            myPushButtonData.LargeImage = new BitmapImage(new Uri(Path.Combine(Path.GetDirectoryName(path) + "\\Images\\Button0150_SortOrder.png"), UriKind.Absolute));
            return myPushButtonData;
        }
        public PushButtonData Button0160_EditSchedule(string Name, string ChecklistsNumber, string path)
        {
            PushButtonData myPushButtonData = new PushButtonData(Name, ChecklistsNumber, exeConfigPath(path), mySettingUpRibbon.dllName + ".Invoke_0160_pkRevitMisc_EditSchedule");
            myPushButtonData.LargeImage = new BitmapImage(new Uri(Path.Combine(Path.GetDirectoryName(path) + "\\Images\\Button0160_EditSchedule.png"), UriKind.Absolute));
            return myPushButtonData;
        }

        public PushButtonData Button0170_AddFields(string Name, string ChecklistsNumber, string path)
        {
            PushButtonData myPushButtonData = new PushButtonData(Name, ChecklistsNumber, exeConfigPath(path), mySettingUpRibbon.dllName + ".Invoke_0170_pkRevitMisc_AddFields");
            myPushButtonData.LargeImage = new BitmapImage(new Uri(Path.Combine(Path.GetDirectoryName(path) + "\\Images\\Button0170_AddFields.png"), UriKind.Absolute));
            return myPushButtonData;
        }

        public PushButtonData Button0180_SmileyFace(string Name, string ChecklistsNumber, string path)
        {
            PushButtonData myPushButtonData = new PushButtonData(Name, ChecklistsNumber, exeConfigPath(path), mySettingUpRibbon.dllName + ".Invoke_0180_pkRevitMisc_SmileyFace");
            myPushButtonData.LargeImage = new BitmapImage(new Uri(Path.Combine(Path.GetDirectoryName(path) + "\\Images\\Button0180_SmileyFace.png"), UriKind.Absolute));
            return myPushButtonData;
        }

        public PushButtonData Button0190_UnderStandingTransforms(string Name, string ChecklistsNumber, string path)
        {
            PushButtonData myPushButtonData = new PushButtonData(Name, ChecklistsNumber, exeConfigPath(path), mySettingUpRibbon.dllName + ".Invoke_0190_pkRevitMisc_UnderStandingTransforms");
            myPushButtonData.LargeImage = new BitmapImage(new Uri(Path.Combine(Path.GetDirectoryName(path) + "\\Images\\Button0190_UnderStandingTransforms.png"), UriKind.Absolute));
            return myPushButtonData;
        }

        public PushButtonData Button0200_NurfGun(string Name, string ChecklistsNumber, string path)
        {
            PushButtonData myPushButtonData = new PushButtonData(Name, ChecklistsNumber, exeConfigPath(path), mySettingUpRibbon.dllName + ".Invoke_0200_pkRevitMisc_NurfGun");
            myPushButtonData.LargeImage = new BitmapImage(new Uri(Path.Combine(Path.GetDirectoryName(path) + "\\Images\\Button0200_NurfGun.png"), UriKind.Absolute));
            return myPushButtonData;
        }

        public PushButtonData Button0200_NurfGun_Delete(string Name, string ChecklistsNumber, string path)
        {
            PushButtonData myPushButtonData = new PushButtonData(Name, ChecklistsNumber, exeConfigPath(path), mySettingUpRibbon.dllName + ".Invoke_0200_pkRevitMisc_NurfGun_Delete");
            return myPushButtonData;
        }

        public PushButtonData Button0210_LoadFamilies(string Name, string ChecklistsNumber, string path)
        {
            PushButtonData myPushButtonData = new PushButtonData(Name, ChecklistsNumber, exeConfigPath(path), mySettingUpRibbon.dllName + ".Invoke_0210_pkRevitMisc_LoadFamilies");
            myPushButtonData.LargeImage = new BitmapImage(new Uri(Path.Combine(Path.GetDirectoryName(path) + "\\Images\\Button0210_LoadFamilies.png"), UriKind.Absolute));
            return myPushButtonData;
        }

        public PushButtonData Button0220_LayoutRoom(string Name, string ChecklistsNumber, string path)
        {
            PushButtonData myPushButtonData = new PushButtonData(Name, ChecklistsNumber, exeConfigPath(path), mySettingUpRibbon.dllName + ".Invoke_0220_pkRevitMisc_LayoutRoom");
            myPushButtonData.LargeImage = new BitmapImage(new Uri(Path.Combine(Path.GetDirectoryName(path) + "\\Images\\Button0220_LayoutRoom.png"), UriKind.Absolute));
            return myPushButtonData;
        }



        public static void writeDebug(string x, bool AndShow)
        {

            string path = (System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\` aa Joshua Lumley Secrets Debug Strings");
            if (!System.IO.Directory.Exists(path)) System.IO.Directory.CreateDirectory(path);

            //string subdirectory_reversedatetothesecond = (path + ("\\" + (DateTime.Now.ToString("yyyyMMddHHmmss"))));
            //if (!System.IO.Directory.Exists(subdirectory_reversedatetothesecond)) System.IO.Directory.CreateDirectory(subdirectory_reversedatetothesecond);

            string FILE_NAME = (path + "\\" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt");

            System.IO.File.Create(FILE_NAME).Dispose();

            System.IO.StreamWriter objWriter = new System.IO.StreamWriter(FILE_NAME, true);
            objWriter.WriteLine(x);
            objWriter.Close();

            if (AndShow) System.Diagnostics.Process.Start(FILE_NAME);
        }
    }
}
