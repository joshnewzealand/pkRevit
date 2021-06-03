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
//using System.Diagnostics;


namespace pkRevitMisc.CommandsWithWindows
{
    partial class Form_2D_Spacers
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }


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

        bool YesOrNo;

        Configuration config = null; //this goes in the top of the form

        void Button1Click(object sender, EventArgs e)
        {
            int eL = -1;

            try
            {

                ////            namespace pkRevitMisc.EntryPoints  //Entry_0010_pkRevitDatasheets
                ////{
                ////    public partial class Entry_0080_pkRevitMisc
                //////////{
                eL = 83;
                pkRevitMisc.EntryPoints.Entry_0080_pkRevitMisc myOriginalClass = new pkRevitMisc.EntryPoints.Entry_0080_pkRevitMisc();
                eL = 85;
                myOriginalClass._99_AutomaticLayout((int)numericUpDown1.Value, (int)numericUpDown2.Value, uidoc, addinfolder, YesOrNo);
                eL = 87;
                this.Close();
            }

            #region catch and finally
            catch (Exception ex)
            {
                writeDebug("Button1Click, error line:" + eL + Environment.NewLine + ex.Message + Environment.NewLine + ex.InnerException, true);
            }
            finally
            {
            }
            #endregion
        }


        UIDocument uidoc;
        string addinfolder;
        //bool useserver = false;



        /// <summary>
        /// This method is required for Windows Forms designer support.
        /// Do not change the method contents inside the source code editor. The Forms designer might
        /// not be able to load this method if it was changed manually.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(44, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(146, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "Number of Vertical Fittings:";
            this.label1.UseCompatibleTextRendering = true;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(44, 91);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(157, 23);
            this.label2.TabIndex = 1;
            this.label2.Text = "Number of Horizontal Fittings:";
            this.label2.UseCompatibleTextRendering = true;
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericUpDown1.Location = new System.Drawing.Point(216, 27);
            this.numericUpDown1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(74, 31);
            this.numericUpDown1.TabIndex = 2;
            this.numericUpDown1.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // numericUpDown2
            // 
            this.numericUpDown2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericUpDown2.Location = new System.Drawing.Point(216, 83);
            this.numericUpDown2.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new System.Drawing.Size(74, 31);
            this.numericUpDown2.TabIndex = 3;
            this.numericUpDown2.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(46, 157);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(244, 39);
            this.button1.TabIndex = 5;
            this.button1.Text = "Make Spacers";
            this.button1.UseCompatibleTextRendering = true;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1Click);
            // 
            // Form_2D_Spacers
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(334, 214);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.numericUpDown2);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "Form_2D_Spacers";
            this.Text = "PRL - Spacers 1.0";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
            this.ResumeLayout(false);

        }
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.NumericUpDown numericUpDown2;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
    }


}