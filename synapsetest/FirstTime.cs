using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MaterialSkin.Animations;
using MaterialSkin.Controls;
using MaterialSkin;
using sxlib.Specialized;
using sxlib;

namespace synapsetest
{
    public partial class FirstTime : MaterialForm
    {
        public FirstTime()
        {
            InitializeComponent();
        }

        private void loadingscreen_Load(object sender, EventArgs e)
        {
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.DARK;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.Blue800, Primary.Blue700, Primary.Blue800, Accent.Blue100, TextShade.WHITE);
            if (Settings2.Default.firsttimes == true)
            {
                MessageBox.Show("Hello! I see this is your first time using MaterialUI please put the path of Synapse X In the text field");
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
                this.Hide();
                Form1 f1 = new Form1();
                f1.Show();
                Settings2.Default.firsttimes = false;
                Settings2.Default.paths = materialSingleLineTextField1.Text;
                Settings2.Default.Save();
            }
        }

        private void FirstTime_Shown(object sender, EventArgs e)
       {
         if (Settings2.Default.firsttimes == false)
           {
           this.Hide();
          Form1 fr = new Form1();
          fr.Show();
           }
        }
    }
}
