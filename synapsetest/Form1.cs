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
using System.Threading;

namespace synapsetest
{
    public partial class Form1 : MaterialForm
    {
        public bool Attached; // Attach Bool

        public static string Direct = Directory.GetCurrentDirectory(); // Gets the directory of the form

        public bool Loaded; // Loaded Bool

        public SxLibWinForms SynLib;
        public Form1()
        {
            InitializeComponent();
        }
        public static readonly Random Rnd = new Random();

        private void LibraryLoadEvent(SxLibBase.SynLoadEvents Event, object whatever)
        {
            materialLabel1.ForeColor = Color.White;
            switch (Event)
            {
                case SxLibBase.SynLoadEvents.READY:
                    materialLabel1.Text = "Ready to Inject!";
                    Loaded = true;
                    return;
                case SxLibBase.SynLoadEvents.CHECKING_WL:

                    materialLabel1.Text = "Checking Whitelist..";
                    break;
                case SxLibBase.SynLoadEvents.CHANGING_WL:
                    break;
                case SxLibBase.SynLoadEvents.DOWNLOADING_DLLS:
                    materialLabel1.Text = "Downloading DLLS..";
                    break;
                case SxLibBase.SynLoadEvents.DOWNLOADING_DATA:
                    materialLabel1.Text = "Downloading Data...";
                    return;
                case SxLibBase.SynLoadEvents.CHECKING_DATA:
                    materialLabel1.Text = "Checking Data..";
                    return;
            }
        }
        private void LibraryAttachEvent(SxLibBase.SynAttachEvents Event, object whatever)
        {
            // This is basically the switch that'll make the label change according to the status of Synapse is in while injecting.
            switch (Event)
            {
                case SxLibBase.SynAttachEvents.CHECKING:
                    this.materialLabel1.ForeColor = Color.FromArgb(240, 240, 240);
                    this.materialLabel1.Text = "Checking...";
                    return;
                case SxLibBase.SynAttachEvents.INJECTING:
                    this.materialLabel1.Text = "Injecting...";
                    return;
                case SxLibBase.SynAttachEvents.CHECKING_WHITELIST:
                    this.materialLabel1.Text = "Checking whitelist...";
                    return;
                case SxLibBase.SynAttachEvents.SCANNING:
                    this.materialLabel1.Text = "Scanning...";
                    return;
                case SxLibBase.SynAttachEvents.READY:
                    this.materialLabel1.ForeColor = Color.FromArgb(0, 255, 0);
                    this.materialLabel1.Text = "Ready!";
                    return;
                case SxLibBase.SynAttachEvents.FAILED_TO_ATTACH:
                    this.materialLabel1.ForeColor = Color.FromArgb(255, 0, 0);
                    this.materialLabel1.Text = "Error! Failed to attach!";
                    return;
                case SxLibBase.SynAttachEvents.FAILED_TO_FIND:
                    this.materialLabel1.ForeColor = Color.FromArgb(255, 0, 0);
                    this.materialLabel1.Text = "Error! Failed to find Roblox!";
                    return;
                case SxLibBase.SynAttachEvents.NOT_RUNNING_LATEST_VER_UPDATING:
                    this.materialLabel1.ForeColor = Color.FromArgb(255, 0, 0);
                    this.materialLabel1.Text = "Not running latest version! Updating...";
                    return;
                case SxLibBase.SynAttachEvents.NOT_INJECTED:
                    break;
                case SxLibBase.SynAttachEvents.ALREADY_INJECTED:
                    this.materialLabel1.ForeColor = Color.FromArgb(255, 0, 0);
                    this.materialLabel1.Text = "Error! Already injected!";
                    break;
                default:
                    return;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if(Settings2.Default.topmosts == true)
            {
                this.TopMost = true;
            } else
            {
                this.TopMost = false;
            }
            materialLabel1.ForeColor = Color.White;
            listBox1.Items.Clear();
            Functions.PopulateListBox(listBox1, "./scripts", "*.txt");
            Functions.PopulateListBox(listBox1, "./scripts", "*.lua");
            Functions.Lib = SxLib.InitializeWinForms(this, Settings2.Default.paths);
            Functions.Lib.LoadEvent += LibraryLoadEvent;
            Functions.Lib.Load();
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.DARK;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.Blue800, Primary.Blue700, Primary.Blue800, Accent.Blue100, TextShade.WHITE);
            this.webBrowser1.Navigate(string.Format("file:///{0}ace/AceEditor.html", AppDomain.CurrentDomain.BaseDirectory));
    }
        private void materialFlatButton1_Click(object sender, EventArgs e)
        {
            HtmlDocument document = webBrowser1.Document;
            string scriptName = "GetText";
            object[] args = new string[0];
            object obj = document.InvokeScript(scriptName, args);
            string script = obj.ToString();
            Functions.Lib.Execute(script);
        }

        private void materialFlatButton2_Click(object sender, EventArgs e)
        {
           if(!this.Loaded)
            {
                MessageBox.Show("Please wait, until it has been loaded", "Error");
            } else
            {
                Functions.Lib.AttachEvent += LibraryAttachEvent;
                Functions.Lib.Attach();
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.listBox1.SelectedIndex != -1)
            {
                this.webBrowser1.Document.InvokeScript("SetText", new object[1]
                {
          (object) System.IO.File.ReadAllText("scripts\\" + this.listBox1.SelectedItem.ToString())
                });
            }
            else
            {
                int num = (int)MessageBox.Show("Please select a script from the list before trying to loading it in tab.", "MaterialUI");
            }
        }

        private void materialFlatButton3_Click(object sender, EventArgs e)
        {
            webBrowser1.Document.InvokeScript("SetText", new object[]
            {
                ""
            });
        }

        private void materialFlatButton4_Click(object sender, EventArgs e)
        {
            if (this.Loaded)
            {
                if (Functions.openfiledialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {

                        string MainText = File.ReadAllText(Functions.openfiledialog.FileName);
                        webBrowser1.Document.InvokeScript("SetText", new object[]
                        {
                          MainText
                        });

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please wait until MaterialUI loads.", "MaterialUI");
            }
        }

        private void materialFlatButton5_Click(object sender, EventArgs e)
        {
            Functions.Lib.Ready();
            if (this.Loaded)
            {
                if (Functions.openfiledialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {

                        string MainText = File.ReadAllText(Functions.openfiledialog.FileName);
                        webBrowser1.Document.InvokeScript("SetText", new object[]
                        {
                          MainText
                        });

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please wait until MaterialUI loads.", "MaterialUI");
            }
        }

        private void materialFlatButton6_Click(object sender, EventArgs e)
        {
            HtmlDocument document = webBrowser1.Document;
            string scriptName = "GetText";
            object[] args = new string[0];
            object obj = document.InvokeScript(scriptName, args);
            string script = obj.ToString();

            SaveFile.Filter = "Text Files (*.txt)|*.txt|Lua Files (*lua*)|*.lua";
            if (SaveFile.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(this.SaveFile.FileName, script);
            }
        }

        private void materialFlatButton7_Click(object sender, EventArgs e)
        {
            options op1 = new options();
            op1.Show();
        }

        private void materialFlatButton8_Click(object sender, EventArgs e)
        {
           listBox1.Items.Clear();
            Functions.PopulateListBox(listBox1, "./scripts", "*.txt");
            Functions.PopulateListBox(listBox1, "./scripts", "*.lua");

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            HtmlDocument document = webBrowser1.Document;
            string scriptName = "GetText";
            object[] args = new string[0];
            object obj = document.InvokeScript(scriptName, args);
            string script = obj.ToString();
            string path = @"./workspace/scriptsaved.txt";
            using (FileStream fs = File.Create(path))
            {
                byte[] info = new UTF8Encoding(true).GetBytes(script);
                // Add some information to the file.
                fs.Write(info, 0, info.Length);
            }
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            string path = @"./workspace/scriptsaved.txt";
            if (File.Exists(path))
            {
                string text = System.IO.File.ReadAllText(@"./workspace/scriptsaved.txt");
                webBrowser1.Document.InvokeScript("SetText", new object[]
      {
                          text
      });
            }
            else
            {
                MessageBox.Show("NOTE: scriptsaved.txt doesnt exist maybe a error?", "MaterialUI");
            }
        }

        private void materialFlatButton9_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Coming Soon..", "MaterialUI");
        }
    }
}
