using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.DirectoryServices;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace pkRevitCustomActions
{
    [RunInstaller(true)]
    public partial class Installer1 : System.Configuration.Install.Installer
    {

        public Installer1()
        {
            InitializeComponent();
        }

        string myAddinDLL = "pkRevitRibbon";

        public override void Uninstall(System.Collections.IDictionary stateSaver)
        {
            string sDir = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\Autodesk\\Revit\\Addins";
            bool exists = System.IO.Directory.Exists(sDir);

            //Microsoft.Win32.RegistryKey rkbase = null;
            //rkbase = Microsoft.Win32.RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, Microsoft.Win32.RegistryView.Registry64);
            //rkbase.DeleteSubKeyTree("SOFTWARE\\Wow6432Node\\Josh New Zealand\\pkRevit");

            if (exists)
            {
                try
                {
                    foreach (string d in Directory.GetDirectories(sDir))
                    {
                        string str_Files = d + "\\" + myAddinDLL + ".addin";

                        if (File.Exists(str_Files)) File.Delete(str_Files);
                    }
                }
                catch (System.Exception excpt)
                {
                    System.Windows.Forms.MessageBox.Show(excpt.Message);
                }
            }
        }

        public override void Install(System.Collections.IDictionary stateSaver)
        {
            int eL = -1;
            try
            {
                eL = 56;
                Microsoft.Win32.RegistryKey rkbase = null;
                rkbase = Microsoft.Win32.RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, Microsoft.Win32.RegistryView.Registry64);
                rkbase.CreateSubKey("SOFTWARE\\Wow6432Node\\Pedersen Read Limited\\pkRevit joshnewzealand", Microsoft.Win32.RegistryKeyPermissionCheck.ReadWriteSubTree).SetValue("XceedVersion", typeof(Xceed.Wpf.Toolkit.PropertyGrid.PropertyGrid).Assembly.FullName);
                rkbase.CreateSubKey("SOFTWARE\\Wow6432Node\\Pedersen Read Limited\\pkRevit joshnewzealand", Microsoft.Win32.RegistryKeyPermissionCheck.ReadWriteSubTree).SetValue("OokiiVersion", typeof(Ookii.Dialogs.Wpf.CredentialDialog).Assembly.FullName);
                rkbase.CreateSubKey("SOFTWARE\\Wow6432Node\\Pedersen Read Limited\\pkRevit joshnewzealand", Microsoft.Win32.RegistryKeyPermissionCheck.ReadWriteSubTree).SetValue("SQLite", typeof(System.Data.SQLite.AssemblySourceIdAttribute).Assembly.FullName);

                rkbase.CreateSubKey("SOFTWARE\\Wow6432Node\\Pedersen Read Limited\\pkRevit joshnewzealand", Microsoft.Win32.RegistryKeyPermissionCheck.ReadWriteSubTree).SetValue("WindowsAPICodePack", typeof(Microsoft.WindowsAPICodePack.Controls.CommonControlException).Assembly.FullName);
                rkbase.CreateSubKey("SOFTWARE\\Wow6432Node\\Pedersen Read Limited\\pkRevit joshnewzealand", Microsoft.Win32.RegistryKeyPermissionCheck.ReadWriteSubTree).SetValue("WindowsAPICodePackShell", typeof(Microsoft.WindowsAPICodePack.Shell.AeroGlassCompositionChangedEventArgs).Assembly.FullName);
                rkbase.CreateSubKey("SOFTWARE\\Wow6432Node\\Pedersen Read Limited\\pkRevit joshnewzealand", Microsoft.Win32.RegistryKeyPermissionCheck.ReadWriteSubTree).SetValue("SystemMemory", typeof(System.Buffers.BuffersExtensions).Assembly.FullName);
                eL = 66;
                string sDir = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\Autodesk\\Revit\\Addins";
                bool exists = System.IO.Directory.Exists(sDir);

                if (!exists) System.IO.Directory.CreateDirectory(sDir);

                XElement XElementAddIn = new XElement("AddIn", new XAttribute("Type", "Application"));

                XElementAddIn.Add(new XElement("Name", myAddinDLL));
                XElementAddIn.Add(new XElement("Assembly", this.Context.Parameters["targetdir"].Trim() + myAddinDLL + ".dll"));  // /TargetDir=value1 /
                XElementAddIn.Add(new XElement("AddInId", Guid.NewGuid().ToString())); //DatabaseMethods.writeDebug(Guid.NewGuid().ToString());
                XElementAddIn.Add(new XElement("FullClassName", myAddinDLL + ".SettingUpRibbon"));
                XElementAddIn.Add(new XElement("VendorId", "01"));
                XElementAddIn.Add(new XElement("VendorDescription", "Joshua Lumley Secrets, twitter @joshnewzealand"));

                XElement XElementRevitAddIns = new XElement("RevitAddIns");
                XElementRevitAddIns.Add(XElementAddIn);
                eL = 83;

                foreach (string d in Directory.GetDirectories(sDir))
                {
                    string myString_ManifestPath = d + "\\" + myAddinDLL + ".addin";

                    string[] directories = d.Split(Path.DirectorySeparatorChar);

                    if (int.TryParse(directories[directories.Count() - 1], out int myInt_FromTextBox))
                    {
                        if (myInt_FromTextBox >= 2019)  //installs on version 2019 and above
                        {
                            new XDocument(XElementRevitAddIns).Save(myString_ManifestPath);
                        }
                        else
                        {
                            if (File.Exists(myString_ManifestPath))
                            {
                                File.Delete(myString_ManifestPath);
                            }
                        }
                    }
                }
            }
            catch (System.Exception excpt)
            {
                System.Windows.Forms.MessageBox.Show(eL + "  " + excpt.Message);
            }
        }
    }
}
