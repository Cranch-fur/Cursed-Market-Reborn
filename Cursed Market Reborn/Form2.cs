﻿using Microsoft.Win32;
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
            textBox1.Text = Globals.REGISTRY_VALUE_PAKFILEPATH;
        }
        private void InitializeSettings()
        {
            if (Globals.REGISTRY_VALUE_THEME == "Default")
            {
                comboBox1.SelectedIndex = 0;
                this.BackColor = Color.White;
                panel1.BackColor = SystemColors.Control;
                label1.ForeColor = Color.Black;
                label2.ForeColor = Color.Black;
            }
            else if (Globals.REGISTRY_VALUE_THEME == "Legacy")
            {
                comboBox1.SelectedIndex = 1;
                this.BackColor = Color.FromArgb(255, 46, 51, 73);
                panel1.BackColor = Color.FromArgb(255, 24, 30, 54);
                label1.ForeColor = Color.White;
                label2.ForeColor = Color.White;
            }
            else
            {
                comboBox1.SelectedIndex = 0;
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

                        default:
                            Registry.SetValue(Globals.REGISTRY_MAIN, "SelectedTheme", "Default");
                            break;
                    }
                }
            }
            catch { }
        }
    }
}
