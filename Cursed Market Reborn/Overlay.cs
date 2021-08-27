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
            this.Width = Screen.PrimaryScreen.WorkingArea.Width;
            this.Height = Screen.PrimaryScreen.WorkingArea.Height;
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

                case "NONE":
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
            try
            {
                await Task.Run(() =>
                {
                    while (true)
                    {
                        if (Globals.FIDDLERCORE_VALUE_QUEUEPOSITION == null || Globals.FIDDLERCORE_VALUE_QUEUEPOSITION == "NONE")
                            label1.Invoke(new Action(() => { label1.Text = ""; }));
                        else
                            label1.Invoke(new Action(() => { label1.Text = Globals.FIDDLERCORE_VALUETRANSFER_QUEUEPOSITION(); }));
                        Thread.Sleep(2500);
                    }

                });
            }
            catch { }
        }
    }
}
