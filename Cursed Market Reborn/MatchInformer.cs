using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cursed_Market_Reborn
{
    public partial class MatchInformer : Form
    {
        private static bool IsKillerOnSteam = false;
        private static string KillerSteamProfile = null;
        public static string FiddlerCoreObtainedMatchInfo = null;


        public MatchInformer()
        {
            InitializeComponent();
            InitializeSettings();
        }

        private void MatchInformer_Load(object sender, EventArgs e)
        {

        }
        private void InitializeSettings()
        {
            switch (Globals.REGISTRY_VALUE_THEME)
            {
                case "Default":
                    pictureBox1.Visible = true;
                    this.BackColor = Color.White;
                    panel1.BackColor = SystemColors.Control;
                    pictureBox2.BackColor = SystemColors.Control;
                    label1.ForeColor = Color.Black;
                    label2.ForeColor = Color.Gainsboro;
                    label3.ForeColor = Color.Black;
                    label4.ForeColor = Color.DimGray;
                    label5.ForeColor = Color.DimGray;
                    label6.ForeColor = Color.DimGray;
                    label7.ForeColor = Color.Gainsboro;
                    break;

                case "Legacy":
                    pictureBox1.Visible = false;
                    this.BackColor = Color.FromArgb(255, 46, 51, 73);
                    panel1.BackColor = Color.FromArgb(255, 24, 30, 54);
                    pictureBox2.BackColor = SystemColors.ControlDark;
                    label1.ForeColor = Color.White;
                    label2.ForeColor = Color.DimGray;
                    label3.ForeColor = Color.White;
                    label4.ForeColor = Color.Gainsboro;
                    label5.ForeColor = Color.Gainsboro;
                    label6.ForeColor = Color.Gainsboro;
                    label7.ForeColor = Color.DimGray;

                    break;

                case "DarkMemories":
                    pictureBox1.Visible = false;
                    this.BackColor = Color.FromArgb(255, 44, 47, 51);
                    panel1.BackColor = Color.FromArgb(255, 35, 39, 42);
                    pictureBox2.BackColor = SystemColors.ControlDark;
                    label1.ForeColor = Color.White;
                    label2.ForeColor = Color.DimGray;
                    label3.ForeColor = Color.White;
                    label4.ForeColor = Color.Gainsboro;
                    label5.ForeColor = Color.Gainsboro;
                    label6.ForeColor = Color.Gainsboro;
                    label7.ForeColor = Color.DimGray;
                    break;

                case "SaintsInaRow":
                    pictureBox1.Visible = false;
                    this.BackColor = Color.FromArgb(255, 37, 13, 57);
                    panel1.BackColor = Color.FromArgb(255, 55, 20, 86);
                    pictureBox2.BackColor = SystemColors.ControlDark;
                    label1.ForeColor = Color.White;
                    label2.ForeColor = Color.DimGray;
                    label3.ForeColor = Color.White;
                    label4.ForeColor = Color.Gainsboro;
                    label5.ForeColor = Color.Gainsboro;
                    label6.ForeColor = Color.Gainsboro;
                    label7.ForeColor = Color.DimGray;
                    break;

                default:
                    pictureBox1.Visible = true;
                    this.BackColor = Color.White;
                    panel1.BackColor = SystemColors.Control;
                    pictureBox2.BackColor = SystemColors.Control;
                    label1.ForeColor = Color.Black;
                    label2.ForeColor = Color.Gainsboro;
                    label3.ForeColor = Color.Black;
                    label4.ForeColor = Color.DimGray;
                    label5.ForeColor = Color.DimGray;
                    label6.ForeColor = Color.DimGray;
                    label2.ForeColor = Color.Gainsboro;
                    break;
            }
        }

        private void MatchInformer_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
        }

        private async void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            panel1.Capture = false; await Task.Run(() =>
            {
                Message mouse = Message.Create(Handle, 0xa1, new IntPtr(2), IntPtr.Zero);
                WndProc(ref mouse);
            });
        }


        private async void MatchInformer_GetAndShowMatchInfo()
        {
            await Task.Run(() =>
            {
                try
                {
                    button1.Invoke(new Action(() => { button1.Enabled = false; }));
                    var JsCurrentMatchInfo = JObject.Parse(NetServices.REQUEST_POST($"http://api.dragonwild.ru/v4/match/{Globals.FIDDLERCORE_VALUE_CURRENTMATCHID}", "", "Cursed Market Reborn", "", ""));
                    if (!JsCurrentMatchInfo.ContainsKey("message"))
                    {
                        if((string)JsCurrentMatchInfo["slasherData"]["character"] != null)
                            DisplayCurrentKiller((string)JsCurrentMatchInfo["slasherData"]["character"]);
                        else
                        {
                            if(FiddlerCoreObtainedMatchInfo != null)
                            {
                                string matchId = FiddlerCoreObtainedMatchInfo.Remove(FiddlerCoreObtainedMatchInfo.IndexOf(";"), FiddlerCoreObtainedMatchInfo.Length - FiddlerCoreObtainedMatchInfo.IndexOf(";"));
                                if(matchId == (string)JsCurrentMatchInfo["matchId"])
                                {
                                    string characterName = FiddlerCoreObtainedMatchInfo.Remove(0, FiddlerCoreObtainedMatchInfo.IndexOf(";") + 1);
                                    DisplayCurrentKiller(characterName);
                                }
                            }
                            else DisplayCurrentKiller((string)JsCurrentMatchInfo["slasherData"]["character"]);
                        }

                        label3.Invoke(new Action(() => { label3.Text = label3.Text + " | " + (string)JsCurrentMatchInfo["slasherData"]["nickname"]; }));

                        string slasherProviderOutput = null;
                        if ((string)JsCurrentMatchInfo["slasherData"]["provider"] == "steam")
                        {
                            IsKillerOnSteam = true;
                            KillerSteamProfile = "https://steamcommunity.com/profiles/" + (string)JsCurrentMatchInfo["slasherData"]["id"];
                            slasherProviderOutput = $"{(string)JsCurrentMatchInfo["slasherData"]["provider"]} ({(string)JsCurrentMatchInfo["slasherData"]["id"]})";
                            label4.Invoke(new Action(() => { label4.ForeColor = SystemColors.MenuHighlight; }));
                        }
                        else
                        {
                            IsKillerOnSteam = false;
                            label4.Invoke(new Action(() => { InitializeSettings(); }));

                            if ((string)JsCurrentMatchInfo["slasherData"]["provider"] == null)
                                slasherProviderOutput = "UNKNOWN PLATFORM";
                            else slasherProviderOutput = (string)JsCurrentMatchInfo["slasherData"]["provider"];
                        }


                        label4.Invoke(new Action(() =>
                        {
                            label4.Text = slasherProviderOutput;
                            label5.Text = $"Region: {(string)JsCurrentMatchInfo["slasherData"]["country"]}";
                            label6.Text = $"MMR: {Convert.ToInt32((float)JsCurrentMatchInfo["slasherData"]["rating"])}";

                        }));

                        button1.Invoke(new Action(() => { button1.Enabled = true; }));
                    }
                    else label1.Text = string.Empty; // THIS ACTION OCCUR EXCEPTION

                }
                catch
                {
                    label3.Invoke(new Action(() =>
                    {
                        label3.Text = "UNKNOWN KILLER";
                        pictureBox2.Image = Properties.Resources.CHAR_UNKNOWN;
                        label4.Text = "UNKNOWN PLATFORM";
                        label5.Text = "UNKNOWN REGION";
                        label6.Text = "UNKNOWN MMR";

                        button1.Enabled = true;
                        InitializeSettings();
                    }));
                }
            });
        }
        private void DisplayCurrentKiller(string input)
        {
            switch (input)
            {
                case "Chuckles":
                    label3.Invoke(new Action(() => { label3.Text = "TR - THE TRAPPER"; pictureBox2.Image = Properties.Resources.CHAR_Chuckles; }));
                    break;

                case "Bob":
                    label3.Invoke(new Action(() => { label3.Text = "FK - THE WRAITH"; pictureBox2.Image = Properties.Resources.CHAR_Bob; }));
                    break;

                case "HillBilly":
                    label3.Invoke(new Action(() => { label3.Text = "GK - THE HILLBILLY"; pictureBox2.Image = Properties.Resources.CHAR_HillBilly; }));
                    break;

                case "Nurse":
                    label3.Invoke(new Action(() => { label3.Text = "HK - THE NURSE"; pictureBox2.Image = Properties.Resources.CHAR_Nurse; }));
                    break;

                case "Shape":
                    label3.Invoke(new Action(() => { label3.Text = "MM - THE SHAPE"; pictureBox2.Image = Properties.Resources.CHAR_Shape; }));
                    break;

                case "Witch":
                    label3.Invoke(new Action(() => { label3.Text = "WI - THE HAG"; pictureBox2.Image = Properties.Resources.CHAR_Witch; }));
                    break;

                case "Killer07":
                    label3.Invoke(new Action(() => { label3.Text = "DO - THE DOCTOR"; pictureBox2.Image = Properties.Resources.CHAR_Killer07; }));
                    break;

                case "Bear":
                    label3.Invoke(new Action(() => { label3.Text = "BE - THE HUNTRESS"; pictureBox2.Image = Properties.Resources.CHAR_Bear; }));
                    break;

                case "Cannibal":
                    label3.Invoke(new Action(() => { label3.Text = "CA - THE CANNIBAL"; pictureBox2.Image = Properties.Resources.CHAR_Cannibal; }));
                    break;

                case "Nightmare":
                    label3.Invoke(new Action(() => { label3.Text = "SD - THE NIGHTMARE"; pictureBox2.Image = Properties.Resources.CHAR_Nightmare; }));
                    break;

                case "Pig":
                    label3.Invoke(new Action(() => { label3.Text = "FK - THE PIG"; pictureBox2.Image = Properties.Resources.CHAR_Pig; }));
                    break;

                case "Clown":
                    label3.Invoke(new Action(() => { label3.Text = "GK - THE CLOWN"; pictureBox2.Image = Properties.Resources.CHAR_Clown; }));
                    break;

                case "Spirit":
                    label3.Invoke(new Action(() => { label3.Text = "HK - THE SPIRIT"; pictureBox2.Image = Properties.Resources.CHAR_Spirit; }));
                    break;

                case "Legion":
                    label3.Invoke(new Action(() => { label3.Text = "KK - THE LEGION"; pictureBox2.Image = Properties.Resources.CHAR_Legion; }));
                    break;

                case "Plague":
                    label3.Invoke(new Action(() => { label3.Text = "MK - THE PLAGUE"; pictureBox2.Image = Properties.Resources.CHAR_Plague; }));
                    break;

                case "Ghostface":
                    label3.Invoke(new Action(() => { label3.Text = "OK - THE GHOST FACE"; pictureBox2.Image = Properties.Resources.CHAR_Ghostface; }));
                    break;

                case "Demogorgon":
                    label3.Invoke(new Action(() => { label3.Text = "QK - THE DEMOGORGON"; pictureBox2.Image = Properties.Resources.CHAR_Demogorgon; }));
                    break;

                case "Oni":
                    label3.Invoke(new Action(() => { label3.Text = "SK - THE ONI"; pictureBox2.Image = Properties.Resources.CHAR_Oni; }));
                    break;

                case "Gunslinger":
                    label3.Invoke(new Action(() => { label3.Text = "UK - THE DEATHSLINGER"; pictureBox2.Image = Properties.Resources.CHAR_Gunslinger; }));
                    break;

                case "K20":
                    label3.Invoke(new Action(() => { label3.Text = "K20 - THE EXECUTIONER"; pictureBox2.Image = Properties.Resources.CHAR_K20; }));
                    break;

                case "K21":
                    label3.Invoke(new Action(() => { label3.Text = "K21 - THE BLIGHT"; pictureBox2.Image = Properties.Resources.CHAR_K21; }));
                    break;

                case "K22":
                    label3.Invoke(new Action(() => { label3.Text = "K22 - THE TWINS"; pictureBox2.Image = Properties.Resources.CHAR_K22; }));
                    break;

                case "K23":
                    label3.Invoke(new Action(() => { label3.Text = "K23 - THE TRICKSTER"; pictureBox2.Image = Properties.Resources.CHAR_K23; }));
                    break;

                case "K24":
                    label3.Invoke(new Action(() => { label3.Text = "K24 - THE NEMESIS"; pictureBox2.Image = Properties.Resources.CHAR_K24; }));
                    break;

                case "K25":
                    label3.Invoke(new Action(() => { label3.Text = "K25 - THE CENOBITE"; pictureBox2.Image = Properties.Resources.CHAR_K25; }));
                    break;

                case null:
                    label3.Invoke(new Action(() => { label3.Text = "UNKNOWN KILLER"; pictureBox2.Image = Properties.Resources.CHAR_UNKNOWN; }));
                    break;

                default:
                    label3.Invoke(new Action(() => { label3.Text = $"UNKNOWN KILLER ({input})"; pictureBox2.Image = Properties.Resources.CHAR_UNKNOWN; }));
                    break;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Globals.FIDDLERCORE_VALUE_CURRENTMATCHID != null)
                MatchInformer_GetAndShowMatchInfo();
            else
            {
                label3.Text = "KILLER NOT OBTAINED";
                pictureBox2.Image = Properties.Resources.CHAR_UNKNOWN;
                label4.Text = "";
                label5.Text = "";
                label6.Text = "";
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {
            if (IsKillerOnSteam == true)
                Process.Start(KillerSteamProfile);
        }
    }
}
