using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static pkRevitDatasheets.MainWindow;

namespace pkRevitDatasheets
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class DuplicateStage1 : Window
    {
        public DuplicateStage1()
        {
            InitializeComponent();
           
        }
        public CombiningViewModel combing_models { get; set; }

        private void but_CopyIntoCurrent_Click(object sender, RoutedEventArgs e)
        {
            int eL = -1;
            if (combing_models.model2._view.lv_MasterList.SelectedItems.Count == 0) return;

            try
            {
                if(myLabel_Directory.Content == combing_models.model2._view.myLabel_Directory.Content)
                {
                    MessageBox.Show("Rows can't paste into themselves");

                    return;
                }

                    MessageBoxResult result = System.Windows.MessageBox.Show("Delete '" + combing_models.model2._view.myLabel_Directory.Content + "'?", "Warning", System.Windows.MessageBoxButton.YesNoCancel);

                if (result != MessageBoxResult.Yes) return;

                if (true)
                {
                    string string_DirectoryToCreate_Storage = combing_models.model2._view.method_Storage_DBFile_Directory() + @"\Admin Storage";
                    string myString_DataStore = string_DirectoryToCreate_Storage + "\\" + ((DataRowView)combing_models.model2._view.lv_MasterList.SelectedItem)["FolderName"];


                    foreach (KeyValuePair<string, Bitmap> drv in combing_models.model2._view.listbox_objectmodel.Items.Cast<KeyValuePair<string, Bitmap>>())
                    {
                        string str_FileName = drv.Key;


                        string str_FileName_Full = myString_DataStore + "\\" + str_FileName;

                        if (System.IO.Directory.Exists(str_FileName_Full))
                        {
                            System.IO.Directory.Delete(str_FileName_Full, true);
                        }
                        else File.Delete(str_FileName_Full);

                        Dictionary<string, Bitmap> imageDictionary = new Dictionary<string, Bitmap>();

                        if (true)
                        {
                            Stream stream = new FileStream(myString_DataStore + @"\Parameters_Thumbnails.xml", FileMode.Open, FileAccess.Read);
                            DataContractSerializer serializer = new DataContractSerializer(typeof(Dictionary<string, Bitmap>));   //one of four
                            imageDictionary = serializer.ReadObject(stream) as Dictionary<string, Bitmap>; stream.Close();  //two of four
                        }

                        if (imageDictionary.ContainsKey(System.IO.Path.GetFileName(str_FileName))) imageDictionary.Remove(str_FileName);

                        if (true)
                        {
                            Stream stream = new FileStream(myString_DataStore + @"\Parameters_Thumbnails.xml", FileMode.Create, FileAccess.Write);  //of course this is bad because of c:
                            DataContractSerializer serializer = new DataContractSerializer(typeof(Dictionary<string, Bitmap>));
                            serializer.WriteObject(stream, imageDictionary); stream.Close();
                        }
                    }
                }
                combing_models.model2._view.methodRefresh_LoadUp_lvDragDirectory(LoadDirectoryOrNot.LoadIconDirectoryOnly);

                if (true)
                {
                    string string_DirectoryToCreate_Storage = combing_models.model2._view.method_Storage_DBFile_Directory() + @"\Admin Storage";
                    string myString_DataStore = string_DirectoryToCreate_Storage + "\\" + myLabel_Directory.Content;

                    List<string>file_pathes = new List<string>();


                    foreach (KeyValuePair<string, Bitmap> drv in listbox_objectmodel.Items.Cast<KeyValuePair<string, Bitmap>>())
                    {
                        string str_FileName_Full = myString_DataStore + "\\" + drv.Key;

                        // string str_FileName = drv.Key;
                        file_pathes.Add(str_FileName_Full);
                    }

                    combing_models.model2._view.adding_NewFiles(file_pathes.ToArray());
                }
            }
            #region catch and finally
            catch (Exception ex)
            {
                _952_PRLoogleClassLibrary.DatabaseMethods.writeDebug("but_CopyIntoCurrent_Click, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }
    }
}
