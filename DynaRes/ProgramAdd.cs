using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DynaRes
{
    public partial class ProgramAdd : Form
    {
        Frontend frtend;
        public ProgramAdd(Frontend frt)
        {
            InitializeComponent();
            frtend = frt;
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            frtend.AddTarget(selgame.Text);
            frtend.ApplySettings();
            frtend.Reload();
            this.Close();   
        }
    }
}
