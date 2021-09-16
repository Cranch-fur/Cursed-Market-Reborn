using System;
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
            label2.Location = new Point(Convert.ToInt32(this.Width / 1.03), this.Height / 128);
        }

        public static bool IsMatchFound = false;
        public static bool IsMMRObtained = false;


        private void Overlay_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
            this.ShowInTaskbar = false;
            this.TransparencyKey = Color.DarkSlateGray;
            this.TopMost = true;

            int initialStyle = GetWindowLong(this.Handle, -20);
            SetWindowLong(this.Handle, -20, initialStyle | 0x80000 | 0x20);
            TRACK_QUEUE();
            backgroundWorker1.RunWorkerAsync();
        }
        private void InitializeSettings()
        {
            switch (Globals.REGISTRY_VALUE_THEME)
            {
                case "Default":
                    label1.ForeColor = Color.Black;
                    label1.BackColor = Color.WhiteSmoke;
                    label2.ForeColor = Color.Black;
                    label2.BackColor = Color.WhiteSmoke;
                    break;

                case "Legacy":
                    label1.ForeColor = Color.White;
                    label1.BackColor = Color.FromArgb(255, 46, 51, 73);
                    label2.ForeColor = Color.White;
                    label2.BackColor = Color.FromArgb(255, 46, 51, 73);
                    break;

                case "DarkMemories":
                    label1.ForeColor = Color.White;
                    label1.BackColor = Color.FromArgb(255, 44, 47, 51);
                    label2.ForeColor = Color.White;
                    label2.BackColor = Color.FromArgb(255, 44, 47, 51);
                    break;

                case "SaintsInaRow":
                    label1.ForeColor = Color.FromArgb(255, 146, 71, 214);
                    label1.BackColor = Color.FromArgb(255, 37, 13, 57);
                    label2.ForeColor = Color.FromArgb(255, 146, 71, 214);
                    label2.BackColor = Color.FromArgb(255, 37, 13, 57);
                    break;

                default:
                    label1.ForeColor = Color.Black;
                    label1.BackColor = Color.WhiteSmoke;
                    label2.ForeColor = Color.Black;
                    label2.BackColor = Color.WhiteSmoke;
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
                            if (IsMatchFound == true)
                                label1.Invoke(new Action(() => { label1.Text = String.Empty; }));
                            else
                            {
                                label1.Invoke(new Action(() => { label1.Text = Globals.FIDDLERCORE_VALUE_QUEUEPOSITION; }));
                                if (Globals.FIDDLERCORE_VALUE_QUEUEPOSITION.Length == 7)
                                {
                                    IsMatchFound = true;
                                    Thread.Sleep(9000);
                                }
                            }
                        }
                        Thread.Sleep(1000);
                    }

                });
            }
            catch { TRACK_QUEUE(); }
        }

        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            try
            {
                while (true)
                {
                    if(Globals.FIDDLERCORE_VALUE_MMR.Length == 0 || (label2.Text == Globals.FIDDLERCORE_VALUE_MMR))
                        label2.Invoke(new Action(() => { label2.Text = String.Empty; }));
                    else
                    {
                        if(IsMMRObtained == true)
                            label2.Invoke(new Action(() => { label2.Text = String.Empty; }));
                        else
                        {
                            label2.Invoke(new Action(() => { label2.Text = Globals.FIDDLERCORE_VALUE_MMR; }));
                            IsMMRObtained = true;
                            Thread.Sleep(12500);
                        }
                    }

                    Thread.Sleep(1000);
                }
            }
            catch { backgroundWorker1.RunWorkerAsync(); }
        }
    }
}
