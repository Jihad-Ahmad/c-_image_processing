namespace PaxeraMed_Test
{
    partial class Create_Form
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txt_filename = new System.Windows.Forms.TextBox();
            this.txt_height = new System.Windows.Forms.TextBox();
            this.btn_Create_CANCEL = new System.Windows.Forms.Button();
            this.btn_Create_OK = new System.Windows.Forms.Button();
            this.txt_width = new System.Windows.Forms.TextBox();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.btn_Browse_create = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 147);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 13);
            this.label3.TabIndex = 18;
            this.label3.Text = "Height";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 109);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 17;
            this.label2.Text = "Width";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 68);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 16;
            this.label1.Text = "File Name";
            // 
            // txt_filename
            // 
            this.txt_filename.Location = new System.Drawing.Point(92, 61);
            this.txt_filename.Name = "txt_filename";
            this.txt_filename.Size = new System.Drawing.Size(149, 20);
            this.txt_filename.TabIndex = 3;
            // 
            // txt_height
            // 
            this.txt_height.Location = new System.Drawing.Point(92, 140);
            this.txt_height.Name = "txt_height";
            this.txt_height.Size = new System.Drawing.Size(149, 20);
            this.txt_height.TabIndex = 5;
            // 
            // btn_Create_CANCEL
            // 
            this.btn_Create_CANCEL.Location = new System.Drawing.Point(229, 198);
            this.btn_Create_CANCEL.Name = "btn_Create_CANCEL";
            this.btn_Create_CANCEL.Size = new System.Drawing.Size(103, 34);
            this.btn_Create_CANCEL.TabIndex = 6;
            this.btn_Create_CANCEL.Text = "Cancel";
            this.btn_Create_CANCEL.UseVisualStyleBackColor = true;
            this.btn_Create_CANCEL.Click += new System.EventHandler(this.btn_Create_CANCEL_Click);
            // 
            // btn_Create_OK
            // 
            this.btn_Create_OK.Location = new System.Drawing.Point(92, 200);
            this.btn_Create_OK.Name = "btn_Create_OK";
            this.btn_Create_OK.Size = new System.Drawing.Size(50, 32);
            this.btn_Create_OK.TabIndex = 6;
            this.btn_Create_OK.Text = "OK";
            this.btn_Create_OK.UseVisualStyleBackColor = true;
            this.btn_Create_OK.Click += new System.EventHandler(this.btn_Create_OK_Click);
            // 
            // txt_width
            // 
            this.txt_width.Location = new System.Drawing.Point(92, 102);
            this.txt_width.Name = "txt_width";
            this.txt_width.Size = new System.Drawing.Size(149, 20);
            this.txt_width.TabIndex = 4;
            // 
            // btn_Browse_create
            // 
            this.btn_Browse_create.Location = new System.Drawing.Point(264, 21);
            this.btn_Browse_create.Name = "btn_Browse_create";
            this.btn_Browse_create.Size = new System.Drawing.Size(68, 26);
            this.btn_Browse_create.TabIndex = 2;
            this.btn_Browse_create.Text = "Browse";
            this.btn_Browse_create.UseVisualStyleBackColor = true;
            this.btn_Browse_create.Click += new System.EventHandler(this.btn_Browse_create_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(94, 23);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(146, 20);
            this.textBox1.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 24);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 13);
            this.label4.TabIndex = 19;
            this.label4.Text = "Location";
            // 
            // Create_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(415, 244);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.btn_Browse_create);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txt_filename);
            this.Controls.Add(this.txt_height);
            this.Controls.Add(this.btn_Create_CANCEL);
            this.Controls.Add(this.btn_Create_OK);
            this.Controls.Add(this.txt_width);
            this.Name = "Create_Form";
            this.Text = "Create New Image";
            this.Load += new System.EventHandler(this.Create_Form_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txt_filename;
        private System.Windows.Forms.TextBox txt_height;
        private System.Windows.Forms.Button btn_Create_CANCEL;
        private System.Windows.Forms.Button btn_Create_OK;
        private System.Windows.Forms.TextBox txt_width;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button btn_Browse_create;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label4;
    }
}