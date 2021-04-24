/*
 * Created by SharpDevelop.
 * User: Joshua
 * Date: 25/04/2017
 * Time: 5:05 p.m.
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.Windows.Forms;

using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit;

using System.Configuration;
using System.Configuration.Assemblies;
//using System.IO;
using System.Reflection;
using Form = System.Windows.Forms.Form;
//using System.Diagnostics;

namespace pkRevitMisc.CommandsWithWindows
{
    public partial class Form_2D_Spacers : Form
    {

        public Form_2D_Spacers(string aif, UIDocument uid, bool yn)
        {
            YesOrNo = yn;

            addinfolder = aif;
            uidoc = uid;

            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            InitializeComponent();

            //
            // TODO: Add constructor code after the InitializeComponent() call.
            //

            //____________________________________________

            String exeConfigPath = addinfolder + "\\" + Assembly.GetExecutingAssembly().GetName().Name + ".dll";

            int numericUpDown11_setting = Properties.Settings.Default.numericUpDown1;

            int numericUpDown22_setting = Properties.Settings.Default.numericUpDown2;
            //bool checkBox11_setting = GetAppSetting(config, "checkBox1") == "True"; 
            ////////bool radioButton1_setting = Properties.Settings.Default.radiobutton == "radioButton1";
            ////////bool radioButton2_setting = Properties.Settings.Default.radiobutton == "radioButton2";
            ////////bool radioButton3_setting = Properties.Settings.Default.radiobutton == "radioButton3";


            ////////if (radioButton1_setting) radioButton1.Checked = true;
            ////////if (radioButton2_setting) radioButton2.Checked = true;
            ////////if (radioButton3_setting) radioButton3.Checked = true;

            if (numericUpDown11_setting != 0) numericUpDown1.Value = numericUpDown11_setting;
            if (numericUpDown22_setting != 0) numericUpDown2.Value = numericUpDown22_setting;

        }

        void Form1FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.numericUpDown1 = Convert.ToInt32(numericUpDown1.Value);
            Properties.Settings.Default.numericUpDown2 = Convert.ToInt32(numericUpDown2.Value);

            //////if (radioButton1.Checked) Properties.Settings.Default.radiobutton = radioButton1.Name;
            //////if (radioButton2.Checked) Properties.Settings.Default.radiobutton = radioButton2.Name;
            //////if (radioButton3.Checked) Properties.Settings.Default.radiobutton = radioButton3.Name;

            Properties.Settings.Default.Save();

        }
    }
}
