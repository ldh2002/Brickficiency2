﻿using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Brickficiency
{
    public partial class Blacklist : Form
    {
        public Blacklist()
        {
            InitializeComponent();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void okButon_Click(object sender, EventArgs e)
        {
            MainWindow.settings.blacklist = Regex.Replace(blackListBox.Text, Environment.NewLine, ",");
            DialogResult = DialogResult.OK;
        }

        private void Blacklist_Shown(object sender, EventArgs e)
        {
            if (MainWindow.settings.blacklist != null)
            {
                blackListBox.Text = Regex.Replace(MainWindow.settings.blacklist, ",", Environment.NewLine);
            }
            blackListBox.Select();
        }

        private void Blacklist_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (this.ActiveControl.Name == "blackListBox" &&
                !char.IsControl(e.KeyChar)
                && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
            if (e.KeyChar == (char)Keys.Escape)
            {
                DialogResult = DialogResult.Cancel;
            }
        }


    }
}
