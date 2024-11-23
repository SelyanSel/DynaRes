using DynaRes.Properties;
using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Guna.UI2.Native.WinApi;

namespace DynaRes
{
    public partial class Form1 : Form
    {

        public bool canclose = false;
        Frontend frnt;
        Settings settings = new Settings();

        int initialResX;
        int initialResY;

        IniFile setINI = new IniFile();

        bool shouldRestore = false;
        bool alreadyRestored = false;

        public Form1()
        {
            InitializeComponent();

            if (File.Exists(Application.StartupPath + "/settings.ini"))
            {
                setINI = new IniFile(Application.StartupPath + "/settings.ini");
                settings = new Settings();

                settings.TargetXResolution = Int32.Parse(setINI.Read("X"));
                settings.TargetYResolution = Int32.Parse(setINI.Read("Y"));
                settings.TargetScreen = Int32.Parse(setINI.Read("target_scr"));
                settings.TickRate = Int32.Parse(setINI.Read("tickrate"));
                settings.TargetPrograms = new List<string>(setINI.Read("targets").Split('$'));

                initialResX = Screen.AllScreens[settings.TargetScreen].Bounds.Width;
                initialResY = Screen.AllScreens[settings.TargetScreen].Bounds.Height;

            }
        }

        public void reload()
        {
            if (File.Exists(Application.StartupPath + "/settings.ini"))
            {
                setINI = new IniFile(Application.StartupPath + "/settings.ini");
                settings = new Settings();

                settings.TargetXResolution = Int32.Parse(setINI.Read("X"));
                settings.TargetYResolution = Int32.Parse(setINI.Read("Y"));
                settings.TargetScreen = Int32.Parse(setINI.Read("target_scr"));
                settings.TickRate = Int32.Parse(setINI.Read("tickrate"));
                settings.TargetPrograms = new List<string>(setINI.Read("targets").Split('$'));

                initialResX = Screen.AllScreens[settings.TargetScreen].Bounds.Width;
                initialResY = Screen.AllScreens[settings.TargetScreen].Bounds.Height;

            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        public void SetTick(int ticks)
        {
            tick.Interval = ticks;
        }

        public void StartTick()
        {
            tick.Start();
        }

        public void MoveTo(Form form, int screenIndex)
        {
            MoveFormToScreen(form, screenIndex);
        }

        public void Notify(string Title, string Text)
        {
            notifyIcon1.ShowBalloonTip(1500, Title, Text, ToolTipIcon.Info);
        }

        public static (int Width, int Height) GetScreenDimensions(Form form)
        {
            Screen currentScreen = Screen.FromControl(form);
            int screenWidth = currentScreen.Bounds.Width;
            int screenHeight = currentScreen.Bounds.Height;

            return (screenWidth, screenHeight);
        }

        public static void MoveFormToScreen(Form form, int screenIndex)
        {
            // Get screens
            Screen[] screens = Screen.AllScreens;

            // Prevent OOB error
            if (screenIndex < 0 || screenIndex >= screens.Length)
            {
                MessageBox.Show($"L'index de l'écran {screenIndex} est invalide. " +
                                $"Il y a {screens.Length} écran(s) disponible(s).",
                                "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Change target screen
            Screen targetScreen = screens[screenIndex];

            // Calculate the new form position (center of target screen)
            Rectangle screenBounds = targetScreen.WorkingArea;
            int newX = screenBounds.Left + (screenBounds.Width - form.Width) / 2;
            int newY = screenBounds.Top + (screenBounds.Height - form.Height) / 2;

            // Move form
            form.Location = new Point(newX, newY);

            // Restore from maximize (if maximized)
            if (form.WindowState == FormWindowState.Maximized)
            {
                form.WindowState = FormWindowState.Normal;
            }
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            this.Hide();

            Frontend front = new Frontend(this);
            front.Show();

            settings.TargetXResolution = 1920;
            settings.TargetYResolution = 1080;
            settings.TargetScreen = 0;
            settings.TickRate = 1000;
            settings.TargetPrograms = new List<string>();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = !canclose;
        }

        private void notifyIcon1_Click(object sender, EventArgs e)
        {
            Frontend front = new Frontend(this);
            front.Show();
        }

        class Settings
        {
            public int TargetXResolution { get; set; }
            public int TargetYResolution { get; set; }
            public int TargetScreen { get; set; }
            public int TickRate { get; set; }
            public List<string> TargetPrograms { get; set; }
        }

        private void tick_Tick(object sender, EventArgs e)
        {
            bool shouldRestore = true;

            if (settings.TargetPrograms.Count == 0)
            {
                tick.Stop();
            }

            foreach (var proc in settings.TargetPrograms)
            {
                Process[] processtarget = Process.GetProcessesByName(proc);
                if (processtarget.Length > 0)
                {
                    shouldRestore = false;
                    break;
                }
            }

            if (shouldRestore && !alreadyRestored)
            {
                alreadyRestored = true;
                ScreenResolutionChanger.ChangeScreenResolution(Screen.AllScreens[settings.TargetScreen], initialResX, initialResY);
            }
            else if (!shouldRestore && alreadyRestored)
            {
                alreadyRestored = false;
                ScreenResolutionChanger.ChangeScreenResolution(Screen.AllScreens[settings.TargetScreen], settings.TargetXResolution, settings.TargetYResolution);
            }
        }
    }
}
