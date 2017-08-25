namespace PaxeraMed_Test
{
    partial class Threshold_Choose
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
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.btn_threshold_OK = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(30, 26);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(120, 20);
            this.numericUpDown1.TabIndex = 0;
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // btn_threshold_OK
            // 
            this.btn_threshold_OK.Location = new System.Drawing.Point(194, 27);
            this.btn_threshold_OK.Name = "btn_threshold_OK";
            this.btn_threshold_OK.Size = new System.Drawing.Size(50, 28);
            this.btn_threshold_OK.TabIndex = 1;
            this.btn_threshold_OK.Text = "OK";
            this.btn_threshold_OK.UseVisualStyleBackColor = true;
            this.btn_threshold_OK.Click += new System.EventHandler(this.btn_threshold_OK_Click);
            // 
            // Threshold_Choose
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 88);
            this.Controls.Add(this.btn_threshold_OK);
            this.Controls.Add(this.numericUpDown1);
            this.Name = "Threshold_Choose";
            this.Text = "Choose Threshold";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Button btn_threshold_OK;
    }
}