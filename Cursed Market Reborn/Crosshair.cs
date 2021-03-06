using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace Cursed_Market_Reborn
{
    public partial class Crosshair : Form
    {
        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        public Crosshair()
        {
            InitializeComponent();
            InitializeSettings();
            this.Icon = Properties.Resources.icon_overlay;
            this.Width = Screen.PrimaryScreen.Bounds.Width;
            this.Height = Screen.PrimaryScreen.Bounds.Height;
        }

        private void Crosshair_Load(object sender, EventArgs e)
        {
            // Action to prevent any 3-rd party tool from 'easy scan' in process list
            this.Text = Globals.PROGRAM_RANDOMNAME();

            CheckForIllegalCrossThreadCalls = false;
            this.ShowInTaskbar = false;
            this.TopMost = true;
            int initialStyle = GetWindowLong(this.Handle, -20);
            SetWindowLong(this.Handle, -20, initialStyle | 0x80000 | 0x20);
            backgroundWorker1.RunWorkerAsync();
        }

        private void InitializeSettings()
        {
            switch (Globals.CROSSHAIR_VALUE_SELECTEDCROSSHAIR)
            {
                case "Circle (Black)":
                    pictureBox1.Image = Properties.Resources.CROSSHAIR_DEFAULT_CIRCLE;
                    this.BackColor = Color.FromArgb(255, 40, 40, 40);
                    this.TransparencyKey = this.BackColor;
                    break;

                case "Circle (White)":
                    pictureBox1.Image = Properties.Resources.CROSSHAIR_DEFAULT_CIRCLE_WHITE;
                    this.BackColor = Color.FromArgb(255, 100, 100, 100);
                    this.TransparencyKey = this.BackColor;
                    break;

                case "Dot (Red)":
                    pictureBox1.Image = Properties.Resources.CROSSHAIR_DEFAULT_DOT_RED;
                    this.BackColor = Color.DarkRed;
                    this.TransparencyKey = this.BackColor;
                    break;

                case "Dot (Yellow)":
                    pictureBox1.Image = Properties.Resources.CROSSHAIR_DEFAULT_DOT_YELLOW;
                    this.BackColor = Color.FromArgb(255, 215, 209, 46);
                    this.TransparencyKey = this.BackColor;
                    break;

                case "Dot (Green)":
                    pictureBox1.Image = Properties.Resources.CROSSHAIR_DEFAULT_DOT_GREEN;
                    this.BackColor = Color.DarkGreen;
                    this.TransparencyKey = this.BackColor;
                    break;

                case "Tactic (Black)":
                    pictureBox1.Image = Properties.Resources.CROSSHAIR_TACTIC_01;
                    this.BackColor = Color.FromArgb(255, 40, 40, 40);
                    this.TransparencyKey = this.BackColor;
                    break;

                case "Tactic (White)":
                    pictureBox1.Image = Properties.Resources.CROSSHAIR_TACTIC_01_WHITE;
                    this.BackColor = Color.FromArgb(255, 100, 100, 100);
                    this.TransparencyKey = this.BackColor;
                    break;

                default:
                    pictureBox1.Image = Properties.Resources.CROSSHAIR_DEFAULT_CIRCLE;
                    this.BackColor = Color.FromArgb(255, 40, 40, 40);
                    this.TransparencyKey = this.BackColor;
                    break;
            }

            pictureBox1.Location = new Point(this.Width / 2 - (pictureBox1.Width / 2), this.Height / 2 - (pictureBox1.Height / 2));
            this.Opacity = (float)Globals.CROSSHAIR_VALUE_OPACITY / 10;
        }

        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            while (true)
            {
                InitializeSettings();
                Thread.Sleep(100);
            }
        }

        private void Crosshair_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
        }
    }
}
