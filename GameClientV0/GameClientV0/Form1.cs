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
    public partial class Form1 : Form
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
            OnlineUser.Disconnect("-1\nDisconnect.");
        }
    }
}
