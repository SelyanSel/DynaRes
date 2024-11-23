using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DynaRes
{
    public partial class SelectDisplay : Form
    {
        int screenIndex;

        public SelectDisplay(int Screen)
        {
            InitializeComponent();
            screenIndex = Screen;
        }

        private void SelectDisplay_Load(object sender, EventArgs e)
        {
            Screen[] screens = Screen.AllScreens;
            if (screenIndex < 0 || screenIndex >= screens.Length)
            {
                MessageBox.Show("The selected screen is out of bounds." + Environment.NewLine + Environment.NewLine +
                                "0001",
                                "DynaRes", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Screen targetScreen = screens[screenIndex];
            Rectangle screenBounds = targetScreen.WorkingArea;
            int newX = screenBounds.Left + (screenBounds.Width - this.Width) / 2;
            int newY = screenBounds.Top + (screenBounds.Height - this.Height) / 2;

            this.Location = new Point(newX, newY);
        }

        private async void SelectDisplay_Shown(object sender, EventArgs e)
        {
            await Task.Delay(2500);
            this.Close();
        }
    }
}
