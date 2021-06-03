using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using System.Windows;
using System.Threading;
using System.Text.RegularExpressions;
using _952_PRLoogleClassLibrary;


namespace pkRevitScheduleEdit
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class _935_PRLoogle_Command05_EE04_CopyOutLastOneWas01 : IExternalEventHandler  //this is the last when one making a checklist change, EE4 must be just for when an element is new
    {
        public ScheduleEdit myWindow2 { get; set; }

        public void Execute(UIApplication uiapp)
        {
            try
            {
                UIDocument uidoc = uiapp.ActiveUIDocument;
                Document doc = uidoc.Document; // myListView_ALL_Fam_Master.Items.Add(doc.GetElement(uidoc.Selection.GetElementIds().First()).Name);

                using (Transaction tx = new Transaction(doc))
                {
                    tx.Start("Appropriate descriptor");

                    tx.Commit();
                }

            }
            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("_935_PRLoogle_Command05_EE04_CopyOutLastOneWas01" + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
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

    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class _935_PRLoogle_Command05_EE04_AddTypeParameter : IExternalEventHandler  //this is the last when one making a checklist change, EE4 must be just for when an element is new
    {
        public ScheduleEdit myWindow2 { get; set; }

        public void Execute(UIApplication uiapp)
        {
            try
            {
                UIDocument uidoc = uiapp.ActiveUIDocument;
                Document doc = uidoc.Document; // myListView_ALL_Fam_Master.Items.Add(doc.GetElement(uidoc.Selection.GetElementIds().First()).Name);

                bool IsTypeParameter = true;

                string path = (System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
                if (!System.IO.Directory.Exists(path)) System.IO.Directory.CreateDirectory(path);

                string FILE_NAME = (path + "\\Appears in Schedule Parameters.txt");

                System.IO.File.Create(FILE_NAME).Dispose();

                System.IO.StreamWriter objWriter = new System.IO.StreamWriter(FILE_NAME, true);

                objWriter.WriteLine("# This is a Revit shared parameter file.");
                objWriter.WriteLine("# Do not edit manually.");
                objWriter.WriteLine("*META	VERSION	MINVERSION");
                objWriter.WriteLine("META	2	1");
                objWriter.WriteLine("*GROUP	ID	NAME");
                objWriter.WriteLine("GROUP	1	Default");  //this doesn't make any different if it is Text
                objWriter.WriteLine("*PARAM	GUID	NAME	DATATYPE	DATACATEGORY	GROUP	VISIBLE	DESCRIPTION	USERMODIFIABLE");

                objWriter.WriteLine("PARAM	cad8bbea-3710-4da0-bee8-f1fbd814217e	PRL_AppearsInSchedule	YESNO		1	1		1");
                objWriter.WriteLine("PARAM	07b67910-74b6-4b56-9685-b16ecec7a50e	PRL_SortOrder	INTEGER		1	1		1");

                if (myWindow2.myPublicBool_LuminaresIsNot)
                {
                    objWriter.WriteLine("PARAM	8d38b3ee-ece7-496a-880b-3fdcbbb9622c	PRL_Description	TEXT		1	1		1");
                    objWriter.WriteLine("PARAM	8b319582-a1ed-4b26-a6e4-6dcfe2f69a55	PRL_ConnectionType	TEXT		1	1		1");
                    objWriter.WriteLine("PARAM	92fa773e-5616-4577-958c-0906d49779b7	PRL_ConnectionTypeDept	TEXT		1	1		1");
                    objWriter.WriteLine("PARAM	04576ba0-18aa-41ee-a811-856449488501	PRL_AcceptableManufacturer	TEXT		1	1		1");
                }
                else
                {
                    objWriter.WriteLine("PARAM	fa202e01-e9c4-4618-9b72-027c0c4dae7b	PRL_LTGSchedule_Accessories	TEXT		1	1		1");
                    objWriter.WriteLine("PARAM	3ea0ea38-8f4b-47c2-9e2d-ef1766446475	PRL_LTGSchedule_CatNo	TEXT		1	1		1");
                    objWriter.WriteLine("PARAM	eb7fae7e-0c08-4c11-a6c3-f5ecb7511b6b	PRL_LTGSchedule_CCT	TEXT		1	1		1");
                    objWriter.WriteLine("PARAM	301a1881-5639-4780-bddf-4f890402a275	PRL_LTGSchedule_Classification	TEXT		1	1		1");
                    objWriter.WriteLine("PARAM	1f4546a4-a8b0-4909-bbaa-30679c01d56f	PRL_LTGSchedule_Control	TEXT		1	1		1");
                    objWriter.WriteLine("PARAM	6d8dbef1-9a33-4f9e-96a5-bb0ce3e496d1	PRL_LTGSchedule_Description	TEXT		1	1		1");
                    objWriter.WriteLine("PARAM	48ec2a3a-ee88-4c5d-a7fc-5db0ce7d867a	PRL_LTGSchedule_Installation	TEXT		1	1		1");
                    objWriter.WriteLine("PARAM	a97d17c8-c358-4001-8ad1-25837a373fb8	PRL_LTGSchedule_Lifetime	TEXT		1	1		1");
                    objWriter.WriteLine("PARAM	3096c531-fd26-4a6a-83fd-56ed56b84d65	PRL_LTGSchedule_Manufacturer	TEXT		1	1		1");
                    objWriter.WriteLine("PARAM	73bd9f05-97e0-4bac-ab36-fae30fd7d07f	PRL_LTGSchedule_Output	TEXT		1	1		1");
                    objWriter.WriteLine("PARAM	c2e87982-fe2e-4ae3-b4eb-db57b2d2e642	PRL_LTGSchedule_MacAdamsEllipse	TEXT		1	1		1");
                    objWriter.WriteLine("PARAM	336b790d-f9cb-4c0c-8d6b-f475080fe2d5	PRL_LTGSchedule_ProtectionIPIK	TEXT		1	1		1");
                    objWriter.WriteLine("PARAM	c8aff0fd-c2f7-4917-a5db-2d2ff238d8d5	PRL_LTGSchedule_Optics	TEXT		1	1		1");
                    objWriter.WriteLine("PARAM	958d96db-317e-4726-9190-f7490c7e16e1	PRL_LTGSchedule_Warranty	TEXT		1	1		1");
                    objWriter.WriteLine("PARAM	3c2f79f5-0e28-48ff-9a62-647eee8af295	PRL_LTGSchedule_Colour	TEXT		1	1		1");
                }


                objWriter.Close();


                //  System.Diagnostics.Process.Start(FILE_NAME);

                //   string path = path + "\\PlayPenSharedParametersType.txt";

                string myStringSharedParameterFileName = "";

                if (uidoc.Application.Application.SharedParametersFilename != null)
                {
                    myStringSharedParameterFileName = uidoc.Application.Application.SharedParametersFilename; //Q:\Revit Revit Revit\Template 2018\PRL_Parameters.txt
                }

                uidoc.Application.Application.SharedParametersFilename = FILE_NAME;


                DefinitionFile myDefinitionFile = uidoc.Application.Application.OpenSharedParameterFile();


                if (myDefinitionFile == null)
                {
                    DatabaseMethods.writeDebug(path + Environment.NewLine + Environment.NewLine + "File does not exist OR cannot be opened.", true);
                    return;
                }

                if (true)
                {
                    string myStringCollectup = "";

                    DefinitionGroup group = myDefinitionFile.Groups.get_Item("Default"); //this doesn't make any different if it is Text
                    foreach (ExternalDefinition myDefinition in group.Definitions)
                    {

                        myStringCollectup = myStringCollectup + " • " + myDefinition.Name;
                        // Binding binding = IsTypeParameter ? uidoc.Application.Application.Create.NewTypeBinding(catSet) as Binding : uidoc.Application.Application.Create.NewInstanceBinding(catSet) as Binding;
                        using (Transaction tx = new Transaction(doc))
                        {
                            tx.Start("EE07_Part1_Binding");


                            if (doc.FamilyManager.GetParameters().Where(x => x.Definition.Name == myDefinition.Name).Count() == 0)
                            {
                                //doc.FamilyManager.AddParameter(myDefinition.Name, binding, myDefinition.ParameterType, false);
                                //doc.FamilyManager.AddParameter(myDefinition, BuiltInParameterGroup.PG_TEXT, false);
                                doc.FamilyManager.AddParameter(myDefinition, myDefinition.ParameterGroup, false);               //myDefinition.Name, myDefinition.ParameterGroup, myDefinition.ParameterType, false);

                            }
                            else
                            {
                                FamilyParameter myFamilyParameter = doc.FamilyManager.GetParameters().Where(x => x.Definition.Name == myDefinition.Name).First();

                                if (!myFamilyParameter.IsShared)
                                {
                                    doc.FamilyManager.ReplaceParameter(myFamilyParameter, myDefinition, myDefinition.ParameterGroup, false);
                                }
                            }
                            tx.Commit();
                        }
                    }
                    MessageBox.Show(myStringCollectup);

                    if (myStringSharedParameterFileName != "")
                    {
                        uidoc.Application.Application.SharedParametersFilename = myStringSharedParameterFileName;
                    }
                }


                if (false)
                {
                    CategorySet catSet = uidoc.Application.Application.Create.NewCategorySet();
                    foreach (Category myCatttt in doc.Settings.Categories)
                    {
                        if (myCatttt.AllowsBoundParameters) catSet.Insert(myCatttt);
                    }

                    string myStringCollectup = "";
                    DefinitionGroup group = myDefinitionFile.Groups.get_Item("Default");
                    foreach (Definition myDefinition in group.Definitions)
                    {
                        myStringCollectup = myStringCollectup + " • " + myDefinition.Name;
                        Binding binding = IsTypeParameter ? uidoc.Application.Application.Create.NewTypeBinding(catSet) as Binding : uidoc.Application.Application.Create.NewInstanceBinding(catSet) as Binding;

                        using (Transaction tx = new Transaction(doc))
                        {
                            tx.Start("EE07_Part1_Binding");
                            doc.ParameterBindings.Insert(myDefinition, binding, BuiltInParameterGroup.PG_TEXT);

                            tx.Commit();
                        }
                    }

                    myStringCollectup = (IsTypeParameter ? "Type" : "Instance") + " parameters added to all categories:" + Environment.NewLine + myStringCollectup;
                    MessageBox.Show(myStringCollectup);
                }

            }

            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("_935_PRLoogle_Command05_EE04_AddTypeParameter" + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
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
