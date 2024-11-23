using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Design;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Media;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DynaRes
{
    public partial class Frontend : Form
    {
        public Form1 srv;
        SelectDisplay selct;
        Settings settings;

        public bool isReady = false;

        IniFile setINI = new IniFile();

        public Frontend(Form1 sr)
        {
            InitializeComponent();
            srv = sr;
        }

        public void PlayNotifSound()
        {
            srv.Notify("Settings", "Settings successfully applied.");
        }

        public void ApplySettings()
        {
            setINI = new IniFile(Application.StartupPath + "/settings.ini");

            setINI.Write("X", settings.TargetXResolution.ToString(), "DynaRes");
            setINI.Write("Y", settings.TargetYResolution.ToString(), "DynaRes");
            setINI.Write("target_scr", settings.TargetScreen.ToString(), "DynaRes");
            setINI.Write("tickrate", 1000.ToString(), "DynaRes");

            string build = "";
            foreach (var target in settings.TargetPrograms)
            {
                build = build + target.ToString() + "$";
            }

            if (build.Length != 0)
            {
                build = build.Remove(build.Length - 1, 1);
            }

            setINI.Write("targets", build, "DynaRes");
        }

        private void Frontend_Load(object sender, EventArgs e)
        {
            foreach (var sc in Screen.AllScreens)
            {
                int stc = 0;
                targetScreen.Items.Add(sc.DeviceName);

                stc = stc + 1;
            }
        }

        public void Reload()
        {
            isReady = false;

            srv.reload();
            if (File.Exists(Application.StartupPath + "/settings.ini"))
            {
                setINI = new IniFile(Application.StartupPath + "/settings.ini");
                settings = new Settings();

                settings.TargetXResolution = Int32.Parse(setINI.Read("X"));
                settings.TargetYResolution = Int32.Parse(setINI.Read("Y"));
                settings.TargetScreen = Int32.Parse(setINI.Read("target_scr"));
                settings.TickRate = Int32.Parse(setINI.Read("tickrate"));
                settings.TargetPrograms = new List<string>(setINI.Read("targets").Split('$'));

                targetScreen.SelectedIndex = settings.TargetScreen;
                resX.Text = settings.TargetXResolution.ToString();
                resY.Text = settings.TargetYResolution.ToString();
                tickRate.Text = settings.TickRate.ToString();

                programlist.Items.Clear();

                foreach (string prgrm in settings.TargetPrograms)
                {
                    programlist.Items.Add(prgrm);
                }

                if (null != selct)
                {
                    selct.Close();
                }

                guna2Button3.Enabled = true;
                guna2Button4.Enabled = false;

                srv.MoveTo(this, settings.TargetScreen);
                srv.SetTick(settings.TickRate);

            }
            else
            {

                // setINI = new IniFile(Application.StartupPath + "/settings.ini");
                settings = new Settings();

                settings.TargetXResolution = 1920;
                settings.TargetYResolution = 1080;
                settings.TargetScreen = 0;
                settings.TickRate = 1000;
                settings.TargetPrograms = new List<string>();

                SetupWizard setup = new SetupWizard(this);
                setup.Show();

                srv.MoveTo(this, settings.TargetScreen);
                srv.SetTick(settings.TickRate);

                this.WindowState = FormWindowState.Minimized;

            }

            isReady = true;
            srv.StartTick();
        }

        private void Frontend_Shown(object sender, EventArgs e)
        {
            Reload();
        }

        // Setup Settings

        public void SetTargetXRes(int xRes)
        {
            settings.TargetXResolution = xRes;
        }

        public void SetTargetYRes(int yRes)
        {
            settings.TargetYResolution = yRes;
        }

        public void SetTargetScreen(int screen)
        {
            settings.TargetScreen = screen;
        }

        public void TickRate(int tickrate)
        {
            settings.TickRate = tickrate;
        }

        public void AddTarget(string target)
        {
            settings.TargetPrograms.Add(target);
        }

        // End

        private void guna2ControlBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void guna2ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (null != selct)
            {
                selct.Close();
            }

            SelectDisplay sel = new SelectDisplay(targetScreen.SelectedIndex);
            sel.Show();
            selct = sel;
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            if (null != selct)
            {
                selct.Close();
            }

            settings.TargetScreen = targetScreen.SelectedIndex;
            ApplySettings();

            PlayNotifSound();

            srv.canclose = true;
            Application.Restart();
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            DialogResult msgres = MessageBox.Show("This is a highly intensive parameter. We recommend to keep it to 1000. Low values " +
                                                  "will make the program use a lot of processing power. Are you sure you wish to edit this value?"
                                                  , "DynaRes",
                                                  MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (msgres == DialogResult.Yes)
            {
                if (null != selct)
                {
                    selct.Close();
                }

                settings.TickRate = Int32.Parse(tickRate.Text);
                ApplySettings();

                PlayNotifSound();

                srv.canclose = true;
                Application.Restart();
            }
        }

        // << Numeric Keypresses

        private void guna2TextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void guna2TextBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void guna2TextBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        // >> End

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            if (null != selct)
            {
                selct.Close();
            }

            settings.TargetXResolution = Int32.Parse(resX.Text);
            settings.TargetYResolution = Int32.Parse(resY.Text);
            ApplySettings();

            PlayNotifSound();

            srv.canclose = true;
            Application.Restart();
        }

        //
        //  Class Definitions
        //

        class Settings
        {
            public int TargetXResolution { get; set; }
            public int TargetYResolution { get; set; }
            public int TargetScreen {  get; set; }
            public int TickRate {  get; set; }
            public List<string> TargetPrograms { get; set; }
        }

        private void guna2Button6_Click(object sender, EventArgs e)
        {
            DialogResult msgres = MessageBox.Show("Are you sure you want to close DynaRes? Dynamic Resolution won't work until DynaRes is " +
                                                  "launched again.", "DynaRes", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (msgres == DialogResult.Yes) {
                srv.canclose = true;
                Application.Exit();
            }
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            ProgramAdd add = new ProgramAdd(this);
            add.Show();
            guna2Button3.Enabled = false;
            guna2Button4.Enabled = false;
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            settings.TargetPrograms.Remove(programlist.SelectedItem.ToString());
            ApplySettings();
            Reload();
        }

        private void programlist_SelectedIndexChanged(object sender, EventArgs e)
        {
            guna2Button4.Enabled = true;
        }
    }
}
