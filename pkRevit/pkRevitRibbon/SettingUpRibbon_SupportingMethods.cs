
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
            string stringTargetOokiiVersion = rkbase.OpenSubKey("SOFTWARE\\Wow6432Node\\Pedersen Read Limited\\cSharpPlaypen joshnewzealand").GetValue("OokiiVersion").ToString();
            string stringTargetXceedVersion = rkbase.OpenSubKey("SOFTWARE\\Wow6432Node\\Pedersen Read Limited\\cSharpPlaypen joshnewzealand").GetValue("XceedVersion").ToString();
            if (AppDomain.CurrentDomain.GetAssemblies().Where(x => x.FullName == stringTargetOokiiVersion).Count() == 0)
            {
                string stringTargetDirectory = rkbase.OpenSubKey("SOFTWARE\\Pedersen Read Limited\\cSharpPlaypen joshnewzealand").GetValue("TARGETDIR").ToString();
                Assembly.Load(File.ReadAllBytes(stringTargetDirectory + "\\Ookii.Dialogs.Wpf.dll"));
            }
            if (AppDomain.CurrentDomain.GetAssemblies().Where(x => x.FullName == stringTargetXceedVersion).Count() == 0)
            {
                string stringTargetDirectory = rkbase.OpenSubKey("SOFTWARE\\Pedersen Read Limited\\cSharpPlaypen joshnewzealand").GetValue("TARGETDIR").ToString();
                Assembly.Load(File.ReadAllBytes(stringTargetDirectory + "\\Xceed.Wpf.Toolkit.dll"));
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
                MessageBoxResult result = System.Windows.MessageBox.Show("File " + System.IO.Path.GetFileName(myString_TestFileLocation) + " exists. Do you want to continue.", "Continue...", System.Windows.MessageBoxButton.YesNoCancel);

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
            myPushButtonData.LargeImage = new BitmapImage(new Uri(Path.Combine(Path.GetDirectoryName(path) + "\\Images\\Button0010_pkRevitDatasheets.png"), UriKind.Absolute));
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
