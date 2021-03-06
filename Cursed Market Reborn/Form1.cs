/////////////////////////////////
/// Cursed Market Reborn by Cranch (Кранч) the Wolf & sizzeR
/// We're happy to share knowledge with people, and we will be happy if you mention us when creating something on our base.
/// SERVERNAME 2021
/////////////////////////////////

using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cursed_Market_Reborn
{
    public partial class Form1 : Form
    {
        ///////////////////////////////// => High Priority Actions
        Form _OVERLAY = new Overlay();
        Form _MATCHINFORMER = new MatchInformer();
        Form _CROSSHAIR = new Crosshair();
        NotifyIcon CursedTray = new NotifyIcon();
        static bool isProgramInitialized = false;
        static bool isFiddlerCoreActive = false;
        static bool isMarketFileInitialized = false;
        static bool isMarketFileLocal = false;
        static bool isSaveFileLocal = false;
        static bool isSeasonManagerOnline = false;


        public Form1()
        {
            InitializeComponent();
            InitializeSettings();
        }
        private async void Form1_Load(object sender, EventArgs e)
        {
            //this.Text = "Cursed Market " + Globals.PROGRAM_TEXT_OFFLINEVERSION;
            // Action to prevent any 3-rd party tool from 'easy scan' in process list
            this.Text = Globals.PROGRAM_RANDOMNAME();

            CranchPalace_checkVersion();
            CranchPalace_checkSSLAvailability();
            CranchPalace_getAvailableSeasons();
            await Task.Run(() =>
            {
                CranchPalace_getMarketFile(0);
                CranchPalace_getFullProfile();
            });
        }
        protected private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            FiddlerCore.Stop();
            Globals.DisableProxy();
        }
        private void Form1_Handle_Tray(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Show();
                CursedTray.Visible = false;
            }
        }
        private void InitializeSettings()
        {
            textBox2.Text = "0";
            textBox3.Text = "0";
            textBox4.Text = "0";
            comboBox2.SelectedItem = Globals.CROSSHAIR_VALUE_SELECTEDCROSSHAIR;
            if (Globals.CROSSHAIR_VALUE_OPACITY != -1 && Globals.CROSSHAIR_VALUE_OPACITY != 0)
                trackBar1.Value = Globals.CROSSHAIR_VALUE_OPACITY;
            else
            {
                Globals.CROSSHAIR_VALUE_OPACITY = 10;
                trackBar1.Value = 10;
            }
            label9.Text = Convert.ToString(trackBar1.Value * 10) + "%";

            try
            {
                switch (Globals.REGISTRY_VALUE_THEME)
                {
                    case "Default":
                        Globals.INITIALIZEDTHEME = 0;
                        pictureBox1.Image = Properties.Resources.IMG_LOGO_BIG_BLACK;
                        pictureBox2.Image = Properties.Resources.ICON_SMALL_SETTINGS_BLACK;
                        this.BackColor = Color.White;
                        panel1.BackColor = SystemColors.Control;
                        label1.ForeColor = Color.Black;
                        label2.ForeColor = Color.Black;
                        label3.ForeColor = Color.Black;
                        label4.ForeColor = Color.Black;
                        label5.ForeColor = Color.Black;
                        label6.ForeColor = Color.Black;
                        label7.ForeColor = Color.Black;
                        label8.ForeColor = Color.Gainsboro;
                        label9.ForeColor = Color.Black;
                        label10.ForeColor = Color.Black;
                        checkBox1.ForeColor = Color.Black;
                        checkBox2.ForeColor = Color.Black;
                        checkBox3.ForeColor = Color.Black;
                        checkBox4.ForeColor = Color.Black;
                        checkBox5.ForeColor = Color.Black;
                        checkBox6.ForeColor = Color.Black;
                        checkBox7.ForeColor = Color.Black;
                        button3.BackColor = Color.Black;
                        button3.ForeColor = Color.White;
                        button4.BackColor = Color.DimGray;
                        button5.BackColor = Color.DimGray;
                        button6.BackColor = Color.DimGray;
                        button7.BackColor = Color.DarkGray;
                        button8.BackColor = Color.DimGray;
                        button9.BackColor = Color.DimGray;
                        break;

                    case "Legacy":
                        Globals.INITIALIZEDTHEME = 1;
                        pictureBox5.Visible = false;
                        pictureBox1.Image = Properties.Resources.IMG_LOGO_BIG_WHITE;
                        pictureBox2.Image = Properties.Resources.ICON_SMALL_SETTINGS_WHITE;
                        this.BackColor = Color.FromArgb(255, 46, 51, 73);
                        panel1.BackColor = Color.FromArgb(255, 24, 30, 54);
                        label1.ForeColor = Color.White;
                        label2.ForeColor = Color.White;
                        label3.ForeColor = Color.White;
                        label4.ForeColor = Color.White;
                        label5.ForeColor = Color.White;
                        label6.ForeColor = Color.White;
                        label7.ForeColor = Color.White;
                        label8.ForeColor = Color.DimGray;
                        label9.ForeColor = Color.White;
                        label10.ForeColor = Color.White;
                        checkBox1.ForeColor = Color.White;
                        checkBox2.ForeColor = Color.White;
                        checkBox3.ForeColor = Color.White;
                        checkBox4.ForeColor = Color.White;
                        checkBox5.ForeColor = Color.White;
                        checkBox6.ForeColor = Color.White;
                        checkBox7.ForeColor = Color.White;
                        button3.BackColor = Color.IndianRed;
                        button3.ForeColor = Color.White;
                        button4.BackColor = Color.RoyalBlue;
                        button5.BackColor = Color.RoyalBlue;
                        button6.BackColor = Color.RoyalBlue;
                        button7.BackColor = Color.SlateBlue;
                        button8.BackColor = Color.RoyalBlue;
                        button9.BackColor = Color.RoyalBlue;
                        break;

                    case "DarkMemories":
                        Globals.INITIALIZEDTHEME = 2;
                        pictureBox5.Visible = false;
                        pictureBox1.Image = Properties.Resources.IMG_LOGO_BIG_WHITE;
                        pictureBox2.Image = Properties.Resources.ICON_SMALL_SETTINGS_WHITE;
                        this.BackColor = Color.FromArgb(255, 44, 47, 51);
                        panel1.BackColor = Color.FromArgb(255, 35, 39, 42);
                        label1.ForeColor = Color.White;
                        label2.ForeColor = Color.White;
                        label3.ForeColor = Color.White;
                        label4.ForeColor = Color.White;
                        label5.ForeColor = Color.White;
                        label6.ForeColor = Color.White;
                        label7.ForeColor = Color.White;
                        label8.ForeColor = Color.DimGray;
                        label9.ForeColor = Color.White;
                        label10.ForeColor = Color.White;
                        checkBox1.ForeColor = Color.White;
                        checkBox2.ForeColor = Color.White;
                        checkBox3.ForeColor = Color.White;
                        checkBox4.ForeColor = Color.White;
                        checkBox5.ForeColor = Color.White;
                        checkBox6.ForeColor = Color.White;
                        checkBox7.ForeColor = Color.White;
                        button3.BackColor = Color.FromArgb(255, 65, 65, 65);
                        button3.ForeColor = Color.White;
                        button4.BackColor = Color.FromArgb(255, 85, 85, 85);
                        button5.BackColor = Color.FromArgb(255, 85, 85, 85);
                        button6.BackColor = Color.FromArgb(255, 85, 85, 85);
                        button7.BackColor = Color.SlateBlue;
                        button8.BackColor = Color.FromArgb(255, 85, 85, 85);
                        button9.BackColor = Color.FromArgb(255, 85, 85, 85);
                        break;

                    case "SaintsInaRow":
                        Globals.INITIALIZEDTHEME = 3;
                        pictureBox5.Visible = false;
                        pictureBox1.Image = Properties.Resources.IMG_LOGO_BIG_WHITE;
                        pictureBox2.Image = Properties.Resources.ICON_SMALL_SETTINGS_WHITE;
                        this.BackColor = Color.FromArgb(255, 37, 13, 57);
                        panel1.BackColor = Color.FromArgb(255, 55, 20, 86);
                        label1.ForeColor = Color.White;
                        label2.ForeColor = Color.White;
                        label3.ForeColor = Color.White;
                        label4.ForeColor = Color.White;
                        label5.ForeColor = Color.White;
                        label6.ForeColor = Color.White;
                        label7.ForeColor = Color.White;
                        label8.ForeColor = Color.DimGray;
                        label9.ForeColor = Color.White;
                        label10.ForeColor = Color.White;
                        checkBox1.ForeColor = Color.White;
                        checkBox2.ForeColor = Color.White;
                        checkBox3.ForeColor = Color.White;
                        checkBox4.ForeColor = Color.White;
                        checkBox5.ForeColor = Color.White;
                        checkBox6.ForeColor = Color.White;
                        checkBox7.ForeColor = Color.White;
                        button3.BackColor = Color.FromArgb(255, 89, 67, 218);
                        button3.ForeColor = Color.White;
                        button4.BackColor = Color.FromArgb(255, 118, 93, 222);
                        button5.BackColor = Color.FromArgb(255, 118, 93, 222);
                        button6.BackColor = Color.FromArgb(255, 118, 93, 222);
                        button7.BackColor = Color.SlateBlue;
                        button8.BackColor = Color.FromArgb(255, 118, 93, 222);
                        button9.BackColor = Color.FromArgb(255, 118, 93, 222);
                        break;

                    default:
                        Globals.INITIALIZEDTHEME = 0;
                        pictureBox1.Image = Properties.Resources.IMG_LOGO_BIG_BLACK;
                        pictureBox2.Image = Properties.Resources.ICON_SMALL_SETTINGS_BLACK;
                        this.BackColor = Color.White;
                        panel1.BackColor = SystemColors.Control;
                        label1.ForeColor = Color.Black;
                        label2.ForeColor = Color.Black;
                        label3.ForeColor = Color.Black;
                        label4.ForeColor = Color.Black;
                        label5.ForeColor = Color.Black;
                        label6.ForeColor = Color.Black;
                        label7.ForeColor = Color.Black;
                        label8.ForeColor = Color.Gainsboro;
                        label9.ForeColor = Color.Black;
                        label10.ForeColor = Color.Black;
                        checkBox1.ForeColor = Color.Black;
                        checkBox2.ForeColor = Color.Black;
                        checkBox3.ForeColor = Color.Black;
                        checkBox4.ForeColor = Color.Black;
                        checkBox5.ForeColor = Color.Black;
                        checkBox6.ForeColor = Color.Black;
                        checkBox7.ForeColor = Color.Black;
                        button3.BackColor = Color.Black;
                        button3.ForeColor = Color.White;
                        button4.BackColor = Color.DimGray;
                        button5.BackColor = Color.DimGray;
                        button6.BackColor = Color.DimGray;
                        button7.BackColor = Color.DarkGray;
                        button8.BackColor = Color.DimGray;
                        button9.BackColor = Color.DimGray;
                        break;
                }
            }
            catch { }
        }

        ///////////////////////////////// => Trackers
        private async void TRACK_QUEUE()
        {
            //    === LENGTH TABLE ===
            //        4 / 20 = NONE
            //        6 / 22 = QUEUED
            //        7 / 23 = MATCHED

            try
            {
                await Task.Run(() =>
                {
                    while (true)
                    {
                        if (Globals.FIDDLERCORE_VALUE_QUEUEPOSITION == "NONE" || (label5.Text.Length == 23 && Globals.FIDDLERCORE_VALUE_QUEUEPOSITION.Length == 7))
                            label5.Invoke(new Action(() => { label5.Text = "QUEUE POSITION: NONE"; }));
                        else
                        {
                            if (label5.Text.Length == 20 && Globals.FIDDLERCORE_VALUE_QUEUEPOSITION.Length == 7)
                                label5.Invoke(new Action(() => { label5.Text = "QUEUE POSITION: NONE"; }));
                            else
                                label5.Invoke(new Action(() => { label5.Text = $"QUEUE POSITION: {Globals.FIDDLERCORE_VALUE_QUEUEPOSITION}"; }));
                        }
                        Thread.Sleep(2500);
                    }

                });
            }
            catch { }
        }
        private async void TRACK_BHVRSESSION()
        {
            try
            {
                await Task.Run(() =>
                {
                    while (true)
                    {
                        if (Globals.FIDDLERCORE_VALUE_BHVRSESSION != null && Globals.FIDDLERCORE_VALUE_BHVRSESSION != "")
                        {
                            if (textBox5.Text.Length == 33)
                                button8.Invoke(new Action(() => { button8.Visible = true; }));
                            textBox5.Invoke(new Action(() => { textBox5.Text = Globals.FIDDLERCORE_VALUE_BHVRSESSION; }));
                        }

                        Thread.Sleep(8000);
                    }
                });
            }
            catch { }
        }

        ///////////////////////////////// => WinForms Windows Basics UI
        private void button1_Click(object sender, EventArgs e) => this.Close();
        private void button2_Click(object sender, EventArgs e) => this.WindowState = FormWindowState.Minimized;
        private async void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            panel1.Capture = false; await Task.Run(() =>
            {
                Message mouse = Message.Create(Handle, 0xa1, new IntPtr(2), IntPtr.Zero);
                WndProc(ref mouse);
            });
        }
        private void button7_Click(object sender, EventArgs e)
        {
            this.Hide();
            CursedTray.Icon = Properties.Resources.icon_tray;
            CursedTray.MouseClick += new MouseEventHandler(Form1_Handle_Tray);
            CursedTray.Visible = true;

            using (NotifyIcon Notify = new NotifyIcon())
            {
                Notify.BalloonTipTitle = Globals.PROGRAM_EXECUTABLE;
                Notify.BalloonTipText = "Cursed Market is hidden to tray.";
                Notify.BalloonTipIcon = ToolTipIcon.Info;
                Notify.Icon = this.Icon;
                Notify.Visible = true;
                Notify.ShowBalloonTip(1000);
                Notify.Dispose();
            }
        }

        ///////////////////////////////// => CranchPalace
        private void CranchPalace_checkVersion()
        {
            try
            {
                var JsVersionCheck = JObject.Parse(NetServices.REQUEST_GET("http://api.cranchpalace.info/v2/cursedmarket/versionCheck", $"version={Globals.PROGRAM_OFFLINEVERSION}", ""));
                if ((bool)JsVersionCheck["isValid"] == false)
                {
                    DialogResult askforupdate = MessageBox.Show("New Version of Cursed Market is available!\nDownload Latest Version?", Globals.PROGRAM_EXECUTABLE, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                    if (askforupdate == DialogResult.Yes)
                    {
                        Process.Start((string)JsVersionCheck["DownloadLink"]);
                        this.Close();
                    }
                }
                else
                {
                    Globals.OVERRIDEN_VALUE_USERAGENT = (string)JsVersionCheck["validUserAgent"];
                    if ((bool)JsVersionCheck["NEWS"]["isNewsAvailable"] == true)
                    {
                        if ((bool)JsVersionCheck["NEWS"]["isNewsThreadCanBeSeenOnlyOnce"] == true)
                            if (Globals.PROGRAM_NEWS_LASTSEENTHREAD == (string)JsVersionCheck["NEWS"]["newsThreadID"])
                                goto AfterNews;

                        if ((bool)JsVersionCheck["NEWS"]["isNewsContainsLink"] == true)
                        {
                            DialogResult newsdialogue = MessageBox.Show((string)JsVersionCheck["NEWS"]["newsText"], (string)JsVersionCheck["NEWS"]["newsTitle"], MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                            if (newsdialogue == DialogResult.Yes)
                                Process.Start((string)JsVersionCheck["NEWS"]["newsLink"]);
                        }
                        else
                            MessageBox.Show((string)JsVersionCheck["NEWS"]["newsText"], (string)JsVersionCheck["NEWS"]["newsTitle"], MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);


                        if ((bool)JsVersionCheck["NEWS"]["isNewsCollapsingApplication"] == true)
                            this.Close();
                    }
                }

            // GOTO TRANSITION LABEL
            AfterNews:

                if ((bool)JsVersionCheck["SPECIAL"]["isAdvancedSkinControlEnabled"] == true)
                    CranchPalace_obtainAdvancedSkinsControl();

                if ((bool)JsVersionCheck["SPECIAL"]["isSeasonManagerEnabled"] == true)
                    isSeasonManagerOnline = true;

                Registry.SetValue(Globals.REGISTRY_MAIN, "LSNT", (string)JsVersionCheck["newsThreadID"]);

            }
            catch { }
        }
        private async void CranchPalace_obtainAdvancedSkinsControl()
        {
            await Task.Run(() =>
            {
                string output;
                for (sbyte counter = 0; counter < 3; counter++)
                {
                    output = NetServices.REQUEST_GET("http://api.cranchpalace.info/v2/cursedmarket/advancedSkinsControl.json", string.Empty, string.Empty);
                    if (output.Length > 5)
                    {
                        Globals.FIDDLERCORE_VALUE_ADVANCEDSKINCONTROL = output;
                        Globals.FIDDLERCORE_BOOL_ISADVANCEDSKINCONTROLENABLED = true;
                        return;
                    }
                    Thread.Sleep(1000);
                }

                MessageBox.Show("Something went wrong when program tried to initialize Advanced Skin Control, please, make sure your Ethernet connection is stable and nothing have control over it (like antivirus).", Globals.PROGRAM_EXECUTABLE, MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
            });
        }


        private void CranchPalace_checkSSLAvailability()
        {
            try
            {
                var JsSSLAvailability = JObject.Parse(NetServices.REQUEST_GET("http://api.cranchpalace.info/v2/cursedmarket/availableSSL", "", ""));
                if ((bool)JsSSLAvailability["SHIPPING"]["available"] == true)
                {
                    label2.Text = label2.Text + (string)JsSSLAvailability["SHIPPING"]["description"];
                    Globals.SSL_SHIPPING = (string)JsSSLAvailability["SHIPPING"]["downloadLink"];
                    comboBox1.Items.Add("Shipping (Default)");
                    comboBox1.SelectedItem = "Shipping (Default)";
                }
                else { label2.Text = label2.Text + "Not available at current moment"; }

                if ((bool)JsSSLAvailability["PTB"]["available"] == true)
                {
                    label3.Text = label3.Text + (string)JsSSLAvailability["PTB"]["description"];
                    Globals.SSL_PTB = (string)JsSSLAvailability["PTB"]["downloadLink"];
                    comboBox1.Items.Add("PTB (Public Test Build)");
                }
                else { label3.Text = label3.Text + "Not available at current moment"; }
            }
            catch
            {
                label2.Text = label2.Text + "CAN'T ACCESS CURSED MARKET WEB SERVICES";
                label3.Text = label3.Text + "CAN'T ACCESS CURSED MARKET WEB SERVICES";
                comboBox1.Visible = false;
                button4.Visible = false;
            }
        }

        private void CranchPalace_getAvailableSeasons()
        {
            if (isSeasonManagerOnline == true)
            {
                try
                {
                    var JsSeasonsArray = JArray.Parse(NetServices.REQUEST_GET("http://api.cranchpalace.info/v2/cursedmarket/seasonManager", "check", ""));
                    foreach (JValue obj in JsSeasonsArray)
                    {
                        comboBox3.Items.Add(obj);
                    }
                    label10.Visible = true;
                    comboBox3.Visible = true;
                }
                catch
                { }
            }
        }

        private void CranchPalace_getMarketFile(sbyte isDLConly)
        {
            try
            {
                if (!File.Exists("Market.json")) // Check if there's no MarketFile in folder with Market, if there is - (else) initialize this file
                {
                    switch (isDLConly)
                    {
                        case 0:
                            Globals.FIDDLERCORE_VALUE_MARKETFILE = Globals.Base64Decode(NetServices.REQUEST_GET("http://api.cranchpalace.info/v2/cursedmarket/marketFile", "", ""));
                            isMarketFileInitialized = true;
                            break;
                        case 1:
                            Globals.FIDDLERCORE_VALUE_MARKETFILE = Globals.Base64Decode(NetServices.REQUEST_GET("http://api.cranchpalace.info/v2/cursedmarket/marketFile", "DLConly", ""));
                            isMarketFileInitialized = true;
                            break;
                    }
                }
                else
                {
                    MessageBox.Show("\"Market.json\" file was found near Cursed Market executable, this market file will be initialized...", Globals.PROGRAM_EXECUTABLE, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    Globals.FIDDLERCORE_VALUE_MARKETFILE = File.ReadAllText("Market.json");
                    isMarketFileInitialized = true;
                    isMarketFileLocal = true;
                }
            }
            catch
            {
                MessageBox.Show("Something went wrong when program tried to obtain Market File, Stored in program memory offline file will be initialized.", Globals.PROGRAM_EXECUTABLE, MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                Globals.FIDDLERCORE_VALUE_MARKETFILE = Properties.Resources.OFFLINERESOURCE_MARKET;
                isMarketFileInitialized = false;
            }
        }
        private void CranchPalace_getFullProfile()
        {
            try
            {
                if (!File.Exists("SaveFile.txt")) // Check if there's no SaveFile in folder with Market, if there is - (else) initialize this file
                {
                    Globals.FIDDLERCORE_VALUE_FULLPROFILE = Globals.Base64Decode(NetServices.REQUEST_GET("http://api.cranchpalace.info/v2/cursedmarket/fullProfile.json", "", ""));
                }
                else
                {
                    MessageBox.Show("\"SaveFile.txt\" file was found near Cursed Market executable, this fullprofile file will be initialized...", Globals.PROGRAM_EXECUTABLE, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    Globals.FIDDLERCORE_VALUE_FULLPROFILE = File.ReadAllText("SaveFile.txt");
                    isSaveFileLocal = true;
                }
            }
            catch
            {
                MessageBox.Show("Something went wrong when program tried to obtain SaveFile, Stored in program memory offline file will be initialized.", Globals.PROGRAM_EXECUTABLE, MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                Globals.FIDDLERCORE_VALUE_FULLPROFILE = Properties.Resources.OFFLINERESOURCE_SAVEFILE;
            }

            button3.Invoke(new Action(() =>
            {
                button3.Text = "START";
                checkBox1.Visible = true;
                checkBox2.Visible = true;
                checkBox3.Visible = true;
                checkBox4.Visible = true;
                checkBox5.Visible = true;
                checkBox6.Visible = true;
                checkBox7.Visible = true;
                label5.Visible = true;
                label6.Visible = true;
                textBox1.Visible = true;
                button5.Visible = true;

                if (isMarketFileLocal == false)
                {
                    button6.Visible = true;
                    if (isMarketFileInitialized == true)
                        checkBox5.Enabled = true;
                }
                if (isSaveFileLocal == false)
                {
                    button9.Visible = true;
                }

                isProgramInitialized = true;
            }));
        }

        ///////////////////////////////// => Main
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (isFiddlerCoreActive == false)
            {
                Form settingsmenu = new Form2();
                settingsmenu.ShowDialog();
            }
            else
                MessageBox.Show("Settings Menu not available while Cursed Market is active.", Globals.PROGRAM_EXECUTABLE, MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (isProgramInitialized == false)
                return;

            FiddlerCore.Start();
            isFiddlerCoreActive = true;
            button3.Visible = false;
            TRACK_QUEUE();
            TRACK_BHVRSESSION();
            textBox5.Visible = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem != null)
            {
                if (((string)comboBox1.SelectedItem)[0] == 'S')
                    Process.Start(Globals.SSL_SHIPPING);
                else
                    Process.Start(Globals.SSL_PTB);
            }
            else { MessageBox.Show("Unexpected Error - Nothing Selected.", Globals.PROGRAM_EXECUTABLE, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1); }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
                Globals.FIDDLERCORE_BOOL_ANTIBOTMATCH = true;
            else
                Globals.FIDDLERCORE_BOOL_ANTIBOTMATCH = false;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true)
                Globals.FIDDLERCORE_BOOL_SILENTFULLPROFILE = true;
            else
                Globals.FIDDLERCORE_BOOL_SILENTFULLPROFILE = false;

            if (isFiddlerCoreActive == true)
                MessageBox.Show("Changes was made when program is already running... Restart your game to see SaveFile changes.", Globals.PROGRAM_EXECUTABLE, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked == true)
            {
                _OVERLAY.Show();
                _MATCHINFORMER.Show();
            } else {
                _OVERLAY.Hide();
                _MATCHINFORMER.Hide();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                if (Globals.FIDDLERCORE_VALUE_BHVRSESSION != null)
                    NetServices.REQUEST_POST($"https://{Globals.FIDDLERCORE_VALUE_PLATFORM}.bhvrdbd.com/api/v1/players/friends/add", "", Globals.OVERRIDEN_VALUE_USERAGENT, "bhvrSession=" + Globals.FIDDLERCORE_VALUE_BHVRSESSION, "{\"ids\":[\"" + textBox1.Text + "\"],\"platform\":\"kraken\"}");
                else
                    MessageBox.Show("Please, start your game with Cursed Market launched before sending friend request.", Globals.PROGRAM_EXECUTABLE, MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
            }
            else
                MessageBox.Show("Please, specify CloudId of the player you want to add. He can find it on the very bottom of the game settings menu, hidden by \"*\" symbols.", Globals.PROGRAM_EXECUTABLE, MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox4.Checked == true)
            {
                Globals.FIDDLERCORE_BOOL_CURRENCYSPOOF = true;
                label7.Visible = true;
                textBox2.Visible = true;
                textBox3.Visible = true;
                textBox4.Visible = true;
                pictureBox6.Visible = true;
                pictureBox7.Visible = true;
                pictureBox8.Visible = true;
            }
            else
            {
                Globals.FIDDLERCORE_BOOL_CURRENCYSPOOF = false;
                label7.Visible = false;
                textBox2.Visible = false;
                textBox3.Visible = false;
                textBox4.Visible = false;
                pictureBox6.Visible = false;
                pictureBox7.Visible = false;
                pictureBox8.Visible = false;
            }
        }

        private static bool isKeypressDigit(KeyPressEventArgs e, string currentstring)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != 8 && e.KeyChar != 127)
                return false;
            else if (currentstring != "")
            {
                if (currentstring[0] == '0')
                {
                    if (e.KeyChar >= 49 && e.KeyChar <= 57 && e.KeyChar != 8)
                        return false;
                }
            }
            return true;
        }
        private void textBox2_TextChanged(object sender, EventArgs e) => Globals.FIDDLERCORE_VALUE_CURRENCYSPOOF_BLOODPOINTS = textBox2.Text;
        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!isKeypressDigit(e, Globals.FIDDLERCORE_VALUE_CURRENCYSPOOF_BLOODPOINTS))
                e.Handled = true;
        }
        private void textBox3_TextChanged(object sender, EventArgs e) => Globals.FIDDLERCORE_VALUE_CURRENCYSPOOF_SHARDS = textBox3.Text;
        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!isKeypressDigit(e, Globals.FIDDLERCORE_VALUE_CURRENCYSPOOF_SHARDS))
                e.Handled = true;
        }
        private void textBox4_TextChanged(object sender, EventArgs e) => Globals.FIDDLERCORE_VALUE_CURRENCYSPOOF_CELLS = textBox4.Text;
        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!isKeypressDigit(e, Globals.FIDDLERCORE_VALUE_CURRENCYSPOOF_CELLS))
                e.Handled = true;
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox5.Checked == true)
                CranchPalace_getMarketFile(1);
            else
                CranchPalace_getMarketFile(0);

            if (isFiddlerCoreActive == true)
                MessageBox.Show("Changes was made when program is already running... Restart your game to see MarketFile changes.", Globals.PROGRAM_EXECUTABLE, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (Globals.FIDDLERCORE_VALUE_MARKETFILE != null)
            {
                using (SaveFileDialog exportMarketFile = new SaveFileDialog())
                {
                    exportMarketFile.InitialDirectory = Environment.CurrentDirectory;
                    exportMarketFile.FilterIndex = 1; exportMarketFile.RestoreDirectory = true;
                    exportMarketFile.FileName = "Market.json";
                    if (exportMarketFile.ShowDialog() == DialogResult.OK)
                    {
                        File.WriteAllText(exportMarketFile.FileName, Globals.FIDDLERCORE_VALUE_MARKETFILE);
                    }
                    else
                        return;
                }
            }
            else
                MessageBox.Show("Please, wait for Market File initialize before exporting it.", Globals.PROGRAM_EXECUTABLE, MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
        }

        private void button8_Click(object sender, EventArgs e) => Clipboard.SetText(textBox5.Text);

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox6.Checked == true)
                Globals.FIDDLERCORE_BOOL_FREEBLOODWEB = true;
            else
                Globals.FIDDLERCORE_BOOL_FREEBLOODWEB = false;
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox7.Checked == true)
            {
                if (Globals.CROSSHAIR_VALUE_SELECTEDCROSSHAIR == "NONE")
                    comboBox2.SelectedIndex = 0;
                _CROSSHAIR.Show();
                comboBox2.Visible = true;
                trackBar1.Visible = true;
                label9.Visible = true;
            }
            else
            {
                _CROSSHAIR.Hide();
                comboBox2.Visible = false;
                trackBar1.Visible = false;
                label9.Visible = false;
            }
        }

        private void comboBox2_SelectedValueChanged(object sender, EventArgs e)
        {
            if (isProgramInitialized == true)
            {
                if (comboBox2.SelectedItem != null)
                {
                    Registry.SetValue(Globals.REGISTRY_MAIN, "SelectedCrosshair", comboBox2.SelectedItem.ToString());
                    Globals.CROSSHAIR_VALUE_SELECTEDCROSSHAIR = comboBox2.SelectedItem.ToString();
                }
            }
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            if (isProgramInitialized == true)
            {
                Registry.SetValue(Globals.REGISTRY_MAIN, "CrosshairOpacity", trackBar1.Value);
                Globals.CROSSHAIR_VALUE_OPACITY = trackBar1.Value;
                label9.Text = Convert.ToString(trackBar1.Value * 10) + "%";
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (Globals.FIDDLERCORE_VALUE_FULLPROFILE != null)
            {
                using (SaveFileDialog exportMarketFile = new SaveFileDialog())
                {
                    exportMarketFile.InitialDirectory = Environment.CurrentDirectory;
                    exportMarketFile.FilterIndex = 1; exportMarketFile.RestoreDirectory = true;
                    exportMarketFile.FileName = "SaveFile.txt";
                    if (exportMarketFile.ShowDialog() == DialogResult.OK)
                    {
                        File.WriteAllText(exportMarketFile.FileName, Globals.FIDDLERCORE_VALUE_FULLPROFILE);
                    }
                    else
                        return;
                }
            }
            else
                MessageBox.Show("Please, wait for SaveFile to initialize before exporting it.", Globals.PROGRAM_EXECUTABLE, MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if(comboBox3.SelectedIndex == 0)
                {
                    Globals.FIDDLERCORE_BOOL_SEASONMANAGER = false;
                    Globals.FIDDLERCORE_VALUE_SEASONMANAGER = string.Empty;
                    comboBox3.SelectedIndex = 0;
                } else {
                    string SeasonManagerResponse = NetServices.REQUEST_GET("http://api.cranchpalace.info/v2/cursedmarket/seasonManager", $"specifiedSeason={comboBox3.SelectedIndex - 1}", "");
                    if (SeasonManagerResponse.Length == 0)
                    {
                        Globals.FIDDLERCORE_BOOL_SEASONMANAGER = false;
                        Globals.FIDDLERCORE_VALUE_SEASONMANAGER = string.Empty;
                        comboBox3.SelectedIndex = 0;
                    } else {
                        Globals.FIDDLERCORE_BOOL_SEASONMANAGER = true;
                        Globals.FIDDLERCORE_VALUE_SEASONMANAGER = SeasonManagerResponse;
                    }
                }
            } catch {
                Globals.FIDDLERCORE_BOOL_SEASONMANAGER = false;
                Globals.FIDDLERCORE_VALUE_SEASONMANAGER = string.Empty;
                comboBox3.SelectedIndex = 0;
            }
        }
    }
}
