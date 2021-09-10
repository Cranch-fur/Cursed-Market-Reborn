﻿using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cursed_Market_Reborn
{
    public partial class Overlay : Form
    {
        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);


        public Overlay()
        {
            InitializeComponent();
            InitializeSettings();
            this.Icon = Properties.Resources.icon_overlay;
            this.Width = Screen.PrimaryScreen.Bounds.Width;
            this.Height = Screen.PrimaryScreen.Bounds.Height;
            label1.Location = new Point(this.Width / 128, this.Height / 128);
        }

        private void Overlay_Load(object sender, EventArgs e)
        {
            this.ShowInTaskbar = false;
            this.TransparencyKey = Color.DarkSlateGray;
            this.TopMost = true;

            int initialStyle = GetWindowLong(this.Handle, -20);
            SetWindowLong(this.Handle, -20, initialStyle | 0x80000 | 0x20);
            TRACK_QUEUE();
        }
        private void InitializeSettings()
        {
            switch (Globals.REGISTRY_VALUE_THEME)
            {
                case "Default":
                    label1.ForeColor = Color.Black;
                    label1.BackColor = Color.WhiteSmoke;
                    break;

                case "Legacy":
                    label1.ForeColor = Color.White;
                    label1.BackColor = Color.FromArgb(255, 46, 51, 73);
                    break;

                case "DarkMemories":
                    label1.ForeColor = Color.White;
                    label1.BackColor = Color.FromArgb(255, 44, 47, 51);
                    break;

                default:
                    label1.ForeColor = Color.Black;
                    label1.BackColor = Color.WhiteSmoke;
                    break;
            }
        }

        private void Overlay_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
        }

        private async void TRACK_QUEUE()
        {
            //    === LENGTH TABLE ===
            //        4 = NONE
            //        6 = QUEUED
            //        7 = MATCHED

            try
            {
                await Task.Run(() =>
                {
                    while (true)
                    {
                        if (Globals.FIDDLERCORE_VALUE_QUEUEPOSITION == "NONE" || (label1.Text.Length == 7 && Globals.FIDDLERCORE_VALUE_QUEUEPOSITION.Length == 7))
                            label1.Invoke(new Action(() => { label1.Text = String.Empty; }));
                        else
                        {
                            if (label1.Text.Length == 0 && Globals.FIDDLERCORE_VALUE_QUEUEPOSITION.Length == 7)
                                label1.Invoke(new Action(() => { label1.Text = String.Empty; }));
                            else
                            {
                                label1.Invoke(new Action(() => { label1.Text = Globals.FIDDLERCORE_VALUE_QUEUEPOSITION; }));
                                if (Globals.FIDDLERCORE_VALUE_QUEUEPOSITION.Length == 7)
                                    Thread.Sleep(9000);
                            }
                        }
                        Thread.Sleep(1000);
                    }

                });
            }
            catch { TRACK_QUEUE(); }
        }
    }
}
