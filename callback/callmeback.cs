using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace callback
{
    public partial class callmeback : Form
    {
        public callmeback()
        {
            InitializeComponent();
        }

        private void OptionsButton_Click(object sender, EventArgs e)
        {
            ActivatedBox.Checked = false;

            Options optionsbox = new Options();

            optionsbox.ShowDialog();
        }

        private void QuitButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AboutButton_Click(object sender, EventArgs e)
        {
            AboutBox1 about = new AboutBox1();
            about.Show();
        }
    }
}
