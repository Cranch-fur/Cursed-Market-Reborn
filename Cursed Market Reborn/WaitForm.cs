using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cursed_Market_Reborn
{
    public partial class WaitForm : Form
    {
        public WaitForm()
        {
            InitializeComponent();
            InitializeSettings();
        }

        private void WaitForm_Load(object sender, EventArgs e)
        {
            // Action to prevent any 3-rd party tool from 'easy scan' in process list
            this.Text = Globals.PROGRAM_RANDOMNAME();

            SimpleTextAnimation();
            this.TopMost = true;
        }
        private void InitializeSettings()
        {
            switch(Globals.REGISTRY_VALUE_THEME)
            {
                case "Default":
                    this.BackColor = Color.White;
                    label1.ForeColor = Color.Black;
                    break;

                case "Legacy":
                    this.BackColor = Color.FromArgb(255, 46, 51, 73);
                    label1.ForeColor = Color.White;
                    break;

                case "DarkMemories":
                    this.BackColor = Color.FromArgb(255, 44, 47, 51);
                    label1.ForeColor = Color.White;
                    break;

                case "SaintsInaRow":
                    this.BackColor = Color.FromArgb(255, 37, 13, 57);
                    label1.ForeColor = Color.White;
                    break;

                case "NONE":
                    this.BackColor = Color.White;
                    label1.ForeColor = Color.Black;
                    break;
            }
        }
        private async void SimpleTextAnimation()
        {
            try
            {
                await Task.Run(() =>
                {
                    while (true)
                    {
                        label1.Invoke(new Action(() => { label1.Text = "PLEASE, WAIT."; }));
                        this.Icon = Properties.Resources.icon_loading_1;
                        Thread.Sleep(750);
                        label1.Invoke(new Action(() => { label1.Text = "PLEASE, WAIT.."; }));
                        this.Icon = Properties.Resources.icon_loading_2;
                        Thread.Sleep(750);
                        label1.Invoke(new Action(() => { label1.Text = "PLEASE, WAIT..."; }));
                        this.Icon = Properties.Resources.icon_loading_3;
                        Thread.Sleep(750);
                    }

                });
            }
            catch { }
        }

        private void WaitForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
        }
    }
}
