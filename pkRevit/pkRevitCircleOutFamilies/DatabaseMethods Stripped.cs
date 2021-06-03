using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.OleDb;
//using System.Data.SqlClient;   // System.Data.dll
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
#pragma warning disable CS0246 // The type or namespace name 'Autodesk' could not be found (are you missing a using directive or an assembly reference?)
using Autodesk.Revit.UI;
#pragma warning restore CS0246 // The type or namespace name 'Autodesk' could not be found (are you missing a using directive or an assembly reference?)
using WinForms = System.Windows.Forms;

namespace _952_PRLoogleClassLibrary
{

    /*
    public class Person
    {
        [Category("Data")]
        [DisplayName("Given name")]
        public string FirstName { get; set; }

        [DisplayName("Family name")]
        public string LastName { get; set; }

        public int Age { get; set; }

        public double Height { get; set; }

        public Mass Weight { get; set; }

        public Genders Gender { get; set; }

        public Color HairColor { get; set; }

        [Description("Check the box if the person owns a bicycle.")]
        public bool OwnsBicycle { get; set; }
    }
    */


    public class DatabaseMethods
    {
        public static void writeDebug(string x, bool AndShow)
        {
            string path = (System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\pk Revit\\` Error Codes");
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

        const string _caption = "The Building Coder";

        public static void InfoMsg(string msg)
        {
            Debug.WriteLine(msg);
            WinForms.MessageBox.Show(msg,
              _caption,
              WinForms.MessageBoxButtons.OK,
              WinForms.MessageBoxIcon.Information);
        }


        public static void ErrorMsg(string msg)
        {
            Debug.WriteLine(msg);
            WinForms.MessageBox.Show(msg,
              _caption,
              WinForms.MessageBoxButtons.OK,
              WinForms.MessageBoxIcon.Error);
        }


        /*  public static List<string> GetAllQueriesFromDataBase(OleDbConnection connRLPrivateFlexible, string theTableName)
          {
             // DataTable tables = connRLPrivateFlexible.GetSchema("Tables");

              DataRow recSpecificTable = connRLPrivateFlexible.GetSchema("Tables").AsEnumerable()
                  .Where(r => r.Field<string>("TABLE_NAME")
                  .StartsWith(theTableName)).FirstOrDefault();

              var queries = new List<string>();
              using (connRLPrivateFlexible)
              {
                  var dt = connRLPrivateFlexible.GetSchema("BASE TABLE");
                  queries = dt.AsEnumerable().Select(dr => dr.Field<string>(theTableName)).ToList();
              }

              return queries;
          }*/

    }
}
