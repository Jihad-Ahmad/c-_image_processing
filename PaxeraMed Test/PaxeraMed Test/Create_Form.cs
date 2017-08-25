using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace PaxeraMed_Test
{
    public partial class Create_Form : Form
    {
        public String path;
        public String filename;
        private int width;
        private int height; 
        public Create_Form()
        {
            InitializeComponent();
            int num = 1;
            string extension = ".bmp";
            string temp_name = "Untitled" + extension;
            if (File.Exists(temp_name))
            {
                while (File.Exists(temp_name))
                {
                    num++;
                    temp_name = "Untitled" + num + extension;
                }
            }
            this.filename = temp_name;
            this.width = 100;
            this.height = 100;
        }
        public int getWidth()
        {
            return width;
        }
        public int getHeight()
        {
            return height;
        }
        private void btn_Create_OK_Click(object sender, EventArgs e)
        {
            this.filename = this.path+this.txt_filename.Text;
            try { this.width = Int32.Parse(this.txt_width.Text); }
            catch
            {
                this.width = 100;
            }
            try { this.height = Int32.Parse(this.txt_height.Text); }
            catch
            {
                this.height = 100;
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btn_Create_CANCEL_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
        private void Create_Form_Load(object sender, EventArgs e)
        {
            this.textBox1.Text = folderBrowserDialog1.RootFolder.ToString(); ;
            this.txt_filename.Text = this.filename;
            this.txt_width.Text = "100";
            this.txt_height.Text = "100";
        }

        private void btn_Browse_create_Click(object sender, EventArgs e)
        {
            this.folderBrowserDialog1.ShowDialog();
            this.path = this.folderBrowserDialog1.SelectedPath+"\\";//.SelectedPath.ToString();
            this.textBox1.Text = this.path;
        }      
    }
}