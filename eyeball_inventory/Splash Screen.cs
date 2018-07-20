using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace EYEBALL_INVENTORY
{
    public partial class SplashScreen : Form
    {
        public SplashScreen()
        {
            InitializeComponent();
        }


        private void SplashScreen_Load(object sender, EventArgs e)
        {
            //this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            //timer1.Interval = 24000;
            //timer1.Enabled = true;
            //timer1.Start();
            this.timer1.Tick+=new EventHandler(timer1_Tick);
            timer1.Interval = 100;
            timer1.Enabled = true;
            timer1.Start();
            

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            progressBar1.Increment(1);
            if (progressBar1.Value == 100)
            {
                this.Close();
            }
        }

    }
}
