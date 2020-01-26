﻿using System;
using System.Windows.Forms;

namespace Brickficiency.ContextMenuStuff
{
    public partial class RemoveComments : Form
    {
        public string text
        {
            get { return textBox1.Text; }
            set { textBox1.Text = value; }
        }

        public RemoveComments()
        {
            InitializeComponent();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void SetComments_VisibleChanged(object sender, EventArgs e)
        {
            textBox1.Select();
        }
    }


}
