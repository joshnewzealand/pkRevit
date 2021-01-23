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

namespace pkRevitCustomMethods
{
    [RunInstaller(true)]
    public partial class Installer1 : System.Configuration.Install.Installer
    {

        //////////////private static Version GetIISVerion()
        //////////////{
        //////////////    using (RegistryKey inetStpKey =
        //////////////      Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\InetStp"))
        //////////////    {
        //////////////        int majorVersion = (int)inetStpKey.GetValue("MajorVersion");
        //////////////        int minorVersion = (int)inetStpKey.GetValue("MinorVersion");

        //////////////        return new Version(majorVersion, minorVersion);
        //////////////    }
        //////////////}

        //////////////private static void Enable32BitAppOnWin64IIS7(string appPoolName)
        //////////////{
        //////////////    Console.Out.WriteLine("Setting Enable32BitAppOnWin64 for {0} (IIS7)", appPoolName);
        //////////////    using (ServerManager serverMgr = new ServerManager())
        //////////////    {
        //////////////        ApplicationPool appPool = serverMgr.ApplicationPools[appPoolName];
        //////////////        if (appPool == null)
        //////////////        {
        //////////////            throw new ApplicationException(String.Format("The pool {0} does not exist", appPoolName));
        //////////////        }

        //////////////        appPool.Enable32BitAppOnWin64 = true;
        //////////////        serverMgr.CommitChanges();
        //////////////    }
        //////////////}

        //////////////private static void Enable32BitAppOnWin64IIS6(string serverName)
        //////////////{
        //////////////    Console.Out.WriteLine("Setting Enable32BitAppOnWin64 for IIS6");
        //////////////    using (DirectoryEntry appPools =
        //////////////      new DirectoryEntry(String.Format("IIS://{0}/W3SVC/AppPools", serverName)))
        //////////////    {
        //////////////        appPools.Properties["Enable32BitAppOnWin64"].Value = true;

        //////////////        appPools.CommitChanges();
        //////////////    }
        //////////////}

        //////////////public static void Enable32BitAppOnWin64(string serverName, string appPoolName)
        //////////////{
        //////////////    Version v = GetIISVerion(); // Get installed version of IIS

        //////////////    Console.Out.WriteLine("IIS-Version: {0}", v);

        //////////////    if (v.Major == 6) // Handle IIS 6
        //////////////    {
        //////////////        Enable32BitAppOnWin64IIS6(serverName);
        //////////////        return;
        //////////////    }

        //////////////    if (v.Major == 7) // Handle IIS 7
        //////////////    {
        //////////////        Enable32BitAppOnWin64IIS7(appPoolName);
        //////////////        return;
        //////////////    }

        //////////////    throw new ApplicationException(String.Format("Unknown IIS version: {0}", v.ToString()));
        //////////////}
        ////////////Enable32BitAppOnWin64(Environment.MachineName, "DefaultAppPool");


        public Installer1()
        {
            InitializeComponent();
        }

        string myAddinDLL = "pkRevitRibbon";

        public override void Uninstall(System.Collections.IDictionary stateSaver)
        {
            string sDir = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\Autodesk\\Revit\\Addins";
            bool exists = System.IO.Directory.Exists(sDir);

            Microsoft.Win32.RegistryKey rkbase = null;
            rkbase = Microsoft.Win32.RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, Microsoft.Win32.RegistryView.Registry64);
            rkbase.DeleteSubKeyTree("SOFTWARE\\Wow6432Node\\Pedersen Read Limited\\pkRevit joshnewzealand");

            if (exists)
            {
                try
                {
                    foreach (string d in Directory.GetDirectories(sDir))
                    {
                        File.Delete(d + "\\" + myAddinDLL + ".addin");
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
            ////////////Enable32BitAppOnWin64(Environment.MachineName, "DefaultAppPool");

            //2 August 2019: The next 4 lines were added in Take 10 in order prevent double loading of packages.
            Microsoft.Win32.RegistryKey rkbase = null;
            rkbase = Microsoft.Win32.RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, Microsoft.Win32.RegistryView.Registry64);
            rkbase.CreateSubKey("SOFTWARE\\Wow6432Node\\Pedersen Read Limited\\pkRevit joshnewzealand", Microsoft.Win32.RegistryKeyPermissionCheck.ReadWriteSubTree).SetValue("XceedVersion", typeof(Xceed.Wpf.Toolkit.PropertyGrid.PropertyGrid).Assembly.FullName);
            rkbase.CreateSubKey("SOFTWARE\\Wow6432Node\\Pedersen Read Limited\\pkRevit joshnewzealand", Microsoft.Win32.RegistryKeyPermissionCheck.ReadWriteSubTree).SetValue("OokiiVersion", typeof(Ookii.Dialogs.Wpf.CredentialDialog).Assembly.FullName);
            rkbase.CreateSubKey("SOFTWARE\\Wow6432Node\\Pedersen Read Limited\\pkRevit joshnewzealand", Microsoft.Win32.RegistryKeyPermissionCheck.ReadWriteSubTree).SetValue("SQLite", typeof(System.Data.SQLite.AssemblySourceIdAttribute).Assembly.FullName);
           //// rkbase.CreateSubKey("SOFTWARE\\Wow6432Node\\Pedersen Read Limited\\pkRevit joshnewzealand", Microsoft.Win32.RegistryKeyPermissionCheck.ReadWriteSubTree).SetValue("SQLiteIntertop", typeof(System.Data.Sql.in).Assembly.FullName);
            ////////rkbase.CreateSubKey("SOFTWARE\\Wow6432Node\\Pedersen Read Limited\\pkRevit joshnewzealand", Microsoft.Win32.RegistryKeyPermissionCheck.ReadWriteSubTree).SetValue("SQLiteCore", typeof(SQLitePCL.authorizer_hook_info).Assembly.FullName);
            ////////rkbase.CreateSubKey("SOFTWARE\\Wow6432Node\\Pedersen Read Limited\\pkRevit joshnewzealand", Microsoft.Win32.RegistryKeyPermissionCheck.ReadWriteSubTree).SetValue("SQLiteNative", typeof(SQLitePCL.NativeLibrary).Assembly.FullName);
            ////////rkbase.CreateSubKey("SOFTWARE\\Wow6432Node\\Pedersen Read Limited\\pkRevit joshnewzealand", Microsoft.Win32.RegistryKeyPermissionCheck.ReadWriteSubTree).SetValue("SQLiteProvider", typeof(SQLitePCL.SQLite3Provider_dynamic_cdecl).Assembly.FullName);
            rkbase.CreateSubKey("SOFTWARE\\Wow6432Node\\Pedersen Read Limited\\pkRevit joshnewzealand", Microsoft.Win32.RegistryKeyPermissionCheck.ReadWriteSubTree).SetValue("WindowsAPICodePack", typeof(Microsoft.WindowsAPICodePack.Controls.CommonControlException).Assembly.FullName);
            rkbase.CreateSubKey("SOFTWARE\\Wow6432Node\\Pedersen Read Limited\\pkRevit joshnewzealand", Microsoft.Win32.RegistryKeyPermissionCheck.ReadWriteSubTree).SetValue("WindowsAPICodePackShell", typeof(Microsoft.WindowsAPICodePack.Shell.AeroGlassCompositionChangedEventArgs).Assembly.FullName);
            rkbase.CreateSubKey("SOFTWARE\\Wow6432Node\\Pedersen Read Limited\\pkRevit joshnewzealand", Microsoft.Win32.RegistryKeyPermissionCheck.ReadWriteSubTree).SetValue("SystemMemory", typeof(System.Buffers.BuffersExtensions).Assembly.FullName);



            //////Assembly.Load(File.ReadAllBytes(@"R:\_001_GitHubRespositories\joshnewzealand\pkRevit\pkRevit\pkRevitDatasheets\bin\Debug\Microsoft.WindowsAPICodePack.dll"));
            //////Assembly.Load(File.ReadAllBytes(@"R:\_001_GitHubRespositories\joshnewzealand\pkRevit\pkRevit\pkRevitDatasheets\bin\Debug\Microsoft.WindowsAPICodePack.Shell.dll"));

            //rkbase.CreateSubKey("SOFTWARE\\Wow6432Node\\Pedersen Read Limited\\pkRevit joshnewzealand", Microsoft.Win32.RegistryKeyPermissionCheck.ReadWriteSubTree).SetValue("SQLite", typeof(System.Data.stu).Assembly.FullName);
            //2 August 2019: End.


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

            try
            {
                foreach (string d in Directory.GetDirectories(sDir))
                {
                    string myString_ManifestPath = d + "\\" + myAddinDLL + ".addin";

                    string[] directories = d.Split(Path.DirectorySeparatorChar);

                    if (int.TryParse(directories[directories.Count() - 1], out int myInt_FromTextBox))
                    {
                        if (myInt_FromTextBox >= 2017)  //installs on version 2017 and above
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
                System.Windows.Forms.MessageBox.Show(excpt.Message);
            }
        }
    }
}
