﻿using System;
using System.Windows.Forms;

namespace Brickficiency.ContextMenuStuff
{
    public partial class SetRemarks : Form
    {
        public string text
        {
            get { return textBox1.Text; }
            set
            {
                textBox1.Text = value;
                textBox1.Select(0, textBox1.TextLength);
            }
        }

        public SetRemarks()
        {
            InitializeComponent();
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void SetComments_VisibleChanged(object sender, EventArgs e)
        {
            textBox1.Select(0, textBox1.TextLength);
        }
    }


}
