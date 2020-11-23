using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using sxlib;
using sxlib.Specialized;
using MaterialSkin;
using MaterialSkin.Animations;
using MaterialSkin.Controls;
using System.IO;
using System.Diagnostics;

namespace synapsetest
{
    public partial class options : MaterialForm
    {
        public options()
        {
            InitializeComponent();
        }

        private void options_Load(object sender, EventArgs e)
        {
            if (Settings2.Default.topmosts == true)
            {
                materialCheckBox1.Checked = true;
            }
            if (Settings2.Default.topmosts == false)

            {
                materialCheckBox1.Checked = false;

            }
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.DARK;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.Blue800, Primary.Blue700, Primary.Blue800, Accent.Blue100, TextShade.WHITE);
        }

        private void materialCheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (Settings2.Default.topmosts == true)
            {
                if (materialCheckBox1.Checked == false)
                {
                    this.TopMost = false;
                    Settings2.Default.topmosts = false;
                    Settings2.Default.checks = true;
                    Settings2.Default.Save();
                }
            }
            else
            {
                this.TopMost = true;
                Settings2.Default.topmosts = true;
                Settings2.Default.Save();
            }
        }

        private void materialFlatButton1_Click(object sender, EventArgs e)
        {
            if (materialSingleLineTextField1.Text == "")
            {
                MessageBox.Show("This isn't a vaild path!", "Error");
            }
            if (!materialSingleLineTextField1.Text.Contains(@"\"))
            {
                MessageBox.Show("This isn't a valid path!", "Error");
            }
            else
            {
                Settings2.Default.paths = materialSingleLineTextField1.Text;
                Settings2.Default.Save();
                this.Hide();
                Form1 f1 = new Form1();
                f1.Show();
            }
        }
    }
}
