using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;

namespace GameClientV0
{
    public partial class Form1 : Form, IStatusChanger
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void login_btn_Click(object sender, EventArgs e)
        {
            OnlineUser.Send("1\n" + loginBox.Text + "\t" + pswdBox.Text);
        }

        private void reg_btn_Click(object sender, EventArgs e)
        {
            OnlineUser.Send("0\n" + loginBox.Text + "\t" + pswdBox.Text);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (OnlineUser.status == OnlineUser.Status.Connected)
            OnlineUser.SendAndDisconnect("-1\nDisconnect.");
        }
        public void StatusChanger(string status)
        {
            status_label.Text = status;
        }
    }
}
