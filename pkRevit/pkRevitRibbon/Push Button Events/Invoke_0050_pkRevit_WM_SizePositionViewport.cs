using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using System.Reflection;
using System.IO;

using TaskDialog = Autodesk.Revit.UI.TaskDialog;


namespace pkRevitRibbon
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class Invoke_0050_pkRevit_WM_SizePositionViewport : IExternalCommand //<--------------------------------------------- edit here
    {
        //I count 1,5 = 6 places that need to change

        string dllModuleName = "pkRevitViewportManagement.dll"; //<--------------------------------------------- edit here

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                if (Properties.Settings.Default.AssemblyNeedLoading) RibbonSupportMethods.loadPackages();

                if (Properties.Settings.Default.MakeTheNextOneDevelopment)
                {
                    //TaskDialog.Show("Switch", "Please switch to development mode.");
                    Properties.Settings.Default.MakeTheNextOneDevelopment = false;
                    Properties.Settings.Default.Save();
                    Properties.Settings.Default.Reload();
                }

                string path = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\\Josh New Zealand\\pkRevit").GetValue("Path").ToString();

                Assembly objAssembly01 = null;

                if (Properties.Settings.Default.pkRevit_LoadName_ViewportManagement == "")
                {
                    if (!File.Exists(path + "\\" + dllModuleName))  //watchparty (search for this)
                    {
                        RibbonSupportMethods.writeDebug(path + dllModuleName + Environment.NewLine + "The above file does not exist.", true);
                    }

                    objAssembly01 = Assembly.Load(File.ReadAllBytes(path + "\\" + dllModuleName));
                    Properties.Settings.Default.pkRevit_LoadName_ViewportManagement = objAssembly01.FullName;
                    Properties.Settings.Default.Save();
                    Properties.Settings.Default.Reload();
                }
                else
                {
                    if (AppDomain.CurrentDomain.GetAssemblies().Where(x => x.FullName == Properties.Settings.Default.pkRevit_LoadName_ViewportManagement).Count() > 0)
                    {
                        objAssembly01 = AppDomain.CurrentDomain.GetAssemblies().Reverse().Where(x => x.FullName == Properties.Settings.Default.pkRevit_LoadName_ViewportManagement).FirstOrDefault();
                    }
                    else
                    {
                        if (!File.Exists(path + "\\" + dllModuleName))  //watchparty (search for this)
                        {
                            RibbonSupportMethods.writeDebug(path + dllModuleName + Environment.NewLine + "The above file does not exist.", true);
                        }

                        objAssembly01 = Assembly.Load(File.ReadAllBytes(path + "\\" + dllModuleName));
                        Properties.Settings.Default.pkRevit_LoadName_ViewportManagement = objAssembly01.FullName;
                        Properties.Settings.Default.Save();
                        Properties.Settings.Default.Reload();
                    }
                }

                bool bool_Found = false;
                string strCommandName = "Entry_0050_pkRevit_VM"; //<--------------------------------------------- edit here
                string strCommandName_Method = "StartMethod_0050"; //<--------------------------------------------- edit here

                IEnumerable<Type> myIEnumerableType = GetTypesSafely(objAssembly01);
                foreach (Type objType in myIEnumerableType)
                {
                    if (objType.IsClass)
                    {
                        if (objType.Name.ToLower() == strCommandName.ToLower())
                        {
                            object ibaseObject = Activator.CreateInstance(objType);
                            object[] arguments = new object[] { commandData, "Release|" + path, elements }; 
                            object result = null;

                            result = objType.InvokeMember(strCommandName_Method, BindingFlags.Default | BindingFlags.InvokeMethod, null, ibaseObject, arguments);

                            bool_Found = true;
                            break;
                        }
                    }
                }

                if (!bool_Found)  
                {
                    RibbonSupportMethods.writeDebug("Invoke_0050_pkRevit_WM_SizePositionViewport" + Environment.NewLine + Environment.NewLine + "Count not find 'method', 'class' in 'file'" + Environment.NewLine
                        + strCommandName_Method + Environment.NewLine + strCommandName + Environment.NewLine + path + dllModuleName, true);
                }
            }

            #region catch and finally
            catch (Exception ex)
            {
                RibbonSupportMethods.writeDebug("Invoke_0050_pkRevitMisc_OpenParentView" + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);//<--------------------------------------------- edit here
            }
            finally
            {
            }
            #endregion

            return Result.Succeeded;

        }
        private static IEnumerable<Type> GetTypesSafely(Assembly assembly)
        {
            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException ex)
            {
                return ex.Types.Where(x => x != null);
            }
        }
    }


    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class DevInvoke_0050_pkRevit_WM_SizePositionViewport : IExternalCommand  //<--------------------------------------------- edit here
    {
        //I count 1,6 = 7 places that need to change

        //string dllModuleFolder = "pkRevitMisc"; //<--------------------------------------------- edit here
        string dllModuleName = "pkRevitViewportManagement.dll"; //<--------------------------------------------- edit here

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                if (Properties.Settings.Default.AssemblyNeedLoading) RibbonSupportMethods.loadPackages();

                string myString_TestFileLocation = Properties.Settings.Default.pkRevit_DevLocation_ViewportManagement; //<--- appears 4 places in code

                if (true)//candidate for methodisation 202012251141
                {
                    if (Properties.Settings.Default.MakeTheNextOneDevelopment | !System.IO.File.Exists(myString_TestFileLocation))
                    {
                        string myString_DefaultLocation = RibbonSupportMethods.Method_InitialDirectory_Go_NoGo(dllModuleName);
                        if (myString_DefaultLocation == null)
                        {
                            Properties.Settings.Default.MakeTheNextOneDevelopment = false;
                            Properties.Settings.Default.Save();
                            Properties.Settings.Default.Reload();
                            return Result.Cancelled;
                        }

                        Microsoft.Win32.OpenFileDialog myDia = RibbonSupportMethods.MethodToSetDevelopmentPath_Individually(myString_DefaultLocation, dllModuleName);

                        if (myDia.ShowDialog() == true)
                        {
                            Properties.Settings.Default.pkRevit_DevLocation_ViewportManagement = myDia.FileName;
                            Properties.Settings.Default.MakeTheNextOneDevelopment = false;
                            Properties.Settings.Default.Save();
                            Properties.Settings.Default.Reload();
                        }
                        else return Result.Cancelled;
                    }
                }

                Assembly objAssembly01 = Assembly.Load(File.ReadAllBytes(Properties.Settings.Default.pkRevit_DevLocation_ViewportManagement));

                bool bool_Found = false;
                string strCommandName = "Entry_0050_pkRevit_VM";//<--------------------------------------------- edit here
                string strCommandName_Method = "StartMethod_0050";//<--------------------------------------------- edit here


                IEnumerable<Type> myIEnumerableType = GetTypesSafely(objAssembly01);
                foreach (Type objType in myIEnumerableType)
                {
                    if (objType.IsClass)
                    {
                        if (objType.Name.ToLower() == strCommandName.ToLower())
                        {
                            object ibaseObject = Activator.CreateInstance(objType);
                            object[] arguments = new object[] { commandData, "Dev|" + System.IO.Path.GetDirectoryName(Properties.Settings.Default.pkRevit_DevLocation_ViewportManagement), elements };
                            object result = null;

                            result = objType.InvokeMember(strCommandName_Method, BindingFlags.Default | BindingFlags.InvokeMethod, null, ibaseObject, arguments);

                            bool_Found = true;
                            break;
                        }
                    }
                }

                if (!bool_Found)  //nor only COPY this 6 times, and remove the slash, but also copy the big where the files exists FROM 3
                {
                    RibbonSupportMethods.writeDebug("DevInvoke_0050_pkRevit_WM_SizePositionViewport" + Environment.NewLine + Environment.NewLine + "Count not find 'method', 'class' in 'file'" + Environment.NewLine
                        + strCommandName_Method + Environment.NewLine + strCommandName + Environment.NewLine + Properties.Settings.Default.pkRevit_DevLocation_ViewportManagement, true);
                }
            }

            #region catch and finally
            catch (Exception ex)
            {
                string pathPreHeader = "DevInvoke_0050_pkRevitMisc_SizePositionViewport" + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException + Environment.NewLine + Environment.NewLine;//<--------------------------------------------- edit here
                string pathHeader = pathPreHeader + "Please check this file (and directory) exist: " + Environment.NewLine;
                string path = Properties.Settings.Default.DevelopmentPathRoot + "";
                RibbonSupportMethods.writeDebug(pathHeader + path + "\\...\\" + dllModuleName, true);
            }
            finally
            {
            }
            #endregion
            return Result.Succeeded;
        }
        private static IEnumerable<Type> GetTypesSafely(Assembly assembly)
        {
            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException ex)
            {
                return ex.Types.Where(x => x != null);
            }
        }
    }
}
