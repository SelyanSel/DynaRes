using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Guna.UI2.Native.WinApi;

namespace DynaRes
{
    public partial class SetupWizard : Form
    {
        Frontend frntend;
        SelectDisplay selct;

        public SetupWizard(Frontend frnt)
        {
            InitializeComponent();

            frntend = frnt;
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            step1.Show();
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            step2.Show();
            step1.Hide();
        }

        private void guna2ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (null != selct)
            {
                selct.Close();
            }

            SelectDisplay sel = new SelectDisplay(reschange.SelectedIndex);
            sel.Show();
            selct = sel;

            frntend.SetTargetScreen(reschange.SelectedIndex);
        }

        private void targetScreen_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (reschange.SelectedIndex)
            {
                case 0:
                    frntend.SetTargetXRes(2560);
                    frntend.SetTargetYRes(1440);
                    break;
                case 1:
                    frntend.SetTargetXRes(1920);
                    frntend.SetTargetYRes(1080);
                    break;
                case 2:
                    frntend.SetTargetXRes(1760);
                    frntend.SetTargetYRes(990);
                    break;
                case 3:
                    frntend.SetTargetXRes(1600);
                    frntend.SetTargetYRes(900);
                    break;
                case 4:
                    frntend.SetTargetXRes(1280);
                    frntend.SetTargetYRes(960);
                    break;

                default:
                    frntend.SetTargetXRes(1920);
                    frntend.SetTargetYRes(1080);
                    break;
            }
        }

        private void step2_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void SetupWizard_Load(object sender, EventArgs e)
        {
            foreach (var sc in Screen.AllScreens)
            {
                int stc = 0;
                reschange.Items.Add(sc.DeviceName);

                stc = stc + 1;
            }
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            step2.Hide();
            step3.Show();
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            frntend.AddTarget(selgame.Text);
            selgame.Text = "";

            using (var soundPlayer = new SoundPlayer(@"c:\Windows\Media\Windows Background.wav"))
            {
                soundPlayer.Play(); // can also use soundPlayer.PlaySync()
            }
        }

        private void guna2Button6_Click(object sender, EventArgs e)
        {
            frntend.AddTarget("FortniteClient-Win64-Shipping");
            guna2Button6.Enabled = false;
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            // save settings
            frntend.ApplySettings();

            frntend.Close();
            frntend.WindowState = FormWindowState.Normal;

            frntend.srv.Notify("Welcome!", "The settings were applied. Click on the screen icon in the menu tray to show settings.");

            this.Close();
        }
    }
}
