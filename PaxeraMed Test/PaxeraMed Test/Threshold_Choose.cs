using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PaxeraMed_Test
{
    public partial class Threshold_Choose : Form
    {
        public int chosen_threshold;
        public IntPtr Handler;
        public Threshold_Choose()
        {
            InitializeComponent();
            this.chosen_threshold = 127;
        }
        public void setHandler(IntPtr formHdlr)
        { this.Handler = formHdlr; }

        private void btn_threshold_OK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.chosen_threshold = (int)this.numericUpDown1.Value;
            this.Close();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            this.chosen_threshold = (int)this.numericUpDown1.Value;
        }

    }
}