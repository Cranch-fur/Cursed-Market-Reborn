using Microsoft.Win32;
using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cursed_Market_Reborn
{
    public partial class Form2 : Form
    {
        static bool isAnythingChanged = false;
        public Form2()
        {
            InitializeComponent();
            this.Icon = Properties.Resources.icon_settings;
            InitializeSettings();
        }
        private void Form2_Load(object sender, EventArgs e)
        {
            if (Globals.REGISTRY_VALUE_PAKFILEPATH != "NONE")
                textBox1.Text = Globals.REGISTRY_VALUE_PAKFILEPATH;

            // Action to prevent any 3-rd party tool from 'easy scan' in process list
            System.Security.Cryptography.MD5 md5hash = System.Security.Cryptography.MD5.Create();
            this.Text = Convert.ToBase64String(md5hash.ComputeHash(System.Text.Encoding.UTF8.GetBytes(this.Text)));
        }
        private void InitializeSettings()
        {
            switch (Globals.REGISTRY_VALUE_THEME)
            {
                case "Default":
                    comboBox1.SelectedIndex = 0;
                    this.BackColor = Color.White;
                    panel1.BackColor = SystemColors.Control;
                    label1.ForeColor = Color.Black;
                    label2.ForeColor = Color.Black;
                    label3.ForeColor = Color.Black;
                    break;

                case "Legacy":
                    comboBox1.SelectedIndex = 1;
                    this.BackColor = Color.FromArgb(255, 46, 51, 73);
                    panel1.BackColor = Color.FromArgb(255, 24, 30, 54);
                    label1.ForeColor = Color.White;
                    label2.ForeColor = Color.White;
                    label3.ForeColor = Color.White;
                    break;

                case "DarkMemories":
                    comboBox1.SelectedIndex = 2;
                    this.BackColor = Color.FromArgb(255, 44, 47, 51);
                    panel1.BackColor = Color.FromArgb(255, 35, 39, 42);
                    label1.ForeColor = Color.White;
                    label2.ForeColor = Color.White;
                    label3.ForeColor = Color.White;
                    break;

                case "SaintsInaRow":
                    comboBox1.SelectedIndex = 3;
                    this.BackColor = Color.FromArgb(255, 37, 13, 57);
                    panel1.BackColor = Color.FromArgb(255, 55, 20, 86);
                    label1.ForeColor = Color.White;
                    label2.ForeColor = Color.White;
                    label3.ForeColor = Color.White;
                    break;

                case "NONE":
                    comboBox1.SelectedIndex = 0;
                    this.BackColor = Color.White;
                    panel1.BackColor = SystemColors.Control;
                    label1.ForeColor = Color.Black;
                    label2.ForeColor = Color.Black;
                    label3.ForeColor = Color.Black;
                    break;
            }
        }

        ///////////////////////////////// => WinForms Windows Basics UI
        private void button1_Click(object sender, EventArgs e)
        {
            if (isAnythingChanged == true)
                Application.Restart();
            else this.Close();
        }
        private async void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            panel1.Capture = false; await Task.Run(() =>
            {
                Message mouse = Message.Create(Handle, 0xa1, new IntPtr(2), IntPtr.Zero);
                WndProc(ref mouse);
            });
        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog specifygamepath = new OpenFileDialog())
            {
                specifygamepath.RestoreDirectory = true;
                specifygamepath.InitialDirectory = Environment.CurrentDirectory;
                specifygamepath.Filter = "pakchunk1-WindowsNoEditor (*.pak*)|*pakchunk1-WindowsNoEditor.pak*;";
                specifygamepath.FilterIndex = 1;
                if (specifygamepath.ShowDialog() == DialogResult.OK)
                {
                    if (specifygamepath.FileName != Globals.REGISTRY_VALUE_PAKFILEPATH)
                    {
                        Registry.SetValue(Globals.REGISTRY_MAIN, "PakFilePath", specifygamepath.FileName);
                        textBox1.Text = specifygamepath.FileName;
                        isAnythingChanged = true;
                    }
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (Globals.INITIALIZEDTHEME != comboBox1.SelectedIndex)
                {
                    switch (comboBox1.SelectedIndex)
                    {
                        case 0:
                            Registry.SetValue(Globals.REGISTRY_MAIN, "SelectedTheme", "Default");
                            isAnythingChanged = true;
                            break;

                        case 1:
                            Registry.SetValue(Globals.REGISTRY_MAIN, "SelectedTheme", "Legacy");
                            isAnythingChanged = true;
                            break;

                        case 2:
                            Registry.SetValue(Globals.REGISTRY_MAIN, "SelectedTheme", "DarkMemories");
                            isAnythingChanged = true;
                            break;

                        case 3:
                            Registry.SetValue(Globals.REGISTRY_MAIN, "SelectedTheme", "SaintsInaRow");
                            isAnythingChanged = true;
                            break;

                        default:
                            Registry.SetValue(Globals.REGISTRY_MAIN, "SelectedTheme", "Default");
                            break;
                    }
                }
            }
            catch { }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (FiddlerCore.RemoveRootCertificate() == true)
                MessageBox.Show("FiddlerCore Certificates successfully deleted from your PC!", Globals.PROGRAM_EXECUTABLE, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            else
                MessageBox.Show("Something went wrong when program tried to delete Fiddler Certificates...", Globals.PROGRAM_EXECUTABLE, MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DialogResult settingsresetdialogue = MessageBox.Show("Are you sure that you want to delete all Cursed Market settings from registry?", Globals.PROGRAM_EXECUTABLE, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            if (settingsresetdialogue == DialogResult.Yes)
            {
                Registry.CurrentUser.DeleteSubKeyTree(Globals.REGISTRY_MAIN.Replace("HKEY_CURRENT_USER\\", ""));
                Application.Restart();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Globals.DisableProxy();
            Application.Restart();
        }
    }
}
