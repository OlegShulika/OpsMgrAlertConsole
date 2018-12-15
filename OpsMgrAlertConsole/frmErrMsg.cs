using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace OpsMgrAlertConsole
{
    public partial class frmErrMsg : Form
    {
        private int nSec = 0;

        public frmErrMsg()
        {
            nSec = 15;
            InitializeComponent();
        }

        public frmErrMsg(int WaitTimeoutSec)
        {
            nSec = WaitTimeoutSec;
            InitializeComponent();
        }

        private void frmErrMsg_Shown(object sender, EventArgs e)
        {
            timerMsg.Start();
            UpdateOkBtn();
        }

        private void frmErrMsg_FormClosed(object sender, FormClosedEventArgs e)
        {
            timerMsg.Stop(); 
        }

        void UpdateOkBtn()
        {
            btnOK.Text = "OK - " + nSec.ToString() + "...";  
        }

        private void timerMsg_Tick(object sender, EventArgs e)
        {
            if (nSec > 0)
            {
                nSec--;
                UpdateOkBtn();
            }
            else
                this.Close();
        }
    }
}
