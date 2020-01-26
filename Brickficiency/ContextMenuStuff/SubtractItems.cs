﻿using System;
using System.Windows.Forms;

namespace Brickficiency.ContextMenuStuff
{
    public partial class SubtractItems : Form
    {
        public int num
        {
            get { return (int)numericUpDown1.Value; }
            set { numericUpDown1.Value = value; }
        }

        public SubtractItems()
        {
            InitializeComponent();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void MouseWheelFix(object sender, MouseEventArgs e)
        {
            HandledMouseEventArgs handledArgs = e as HandledMouseEventArgs;
            handledArgs.Handled = true;
            if (e.Delta > 0)
            {
                if (numericUpDown1.Value != numericUpDown1.Maximum)
                {
                    numericUpDown1.Value++;
                }
            }
            else
            {
                if (numericUpDown1.Value != numericUpDown1.Minimum)
                {
                    numericUpDown1.Value--;
                }
            }
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void SubtractItems_VisibleChanged(object sender, EventArgs e)
        {
            numericUpDown1.Focus();
            numericUpDown1.Select(0, numericUpDown1.ToString().Length);
        }
    }


}
