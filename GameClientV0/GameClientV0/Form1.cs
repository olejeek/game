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
using game.Net.Protocol;

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
            OnlineUser.Connect();
            string login = loginBox.Text + "\t" + pswdBox.Text;
            OnlineUser.BlockToSend(new Block(BlockCode.Login, (int)LoginType.Access, login));
        }

        private void reg_btn_Click(object sender, EventArgs e)
        {
            OnlineUser.Connect();
            string login = loginBox.Text + "\t" + pswdBox.Text;
            OnlineUser.BlockToSend(new Block(BlockCode.Registration, 
                (int)RegistrationType.CreateNewAcc, login));
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (OnlineUser.status == OnlineUser.Status.Connected)
            //OnlineUser.SendAndDisconnect("-1\nDisconnect.");
            OnlineUser.BlockToSend(new Block(BlockCode.Disconnect, (int)DisconectType.Exit));
        }
        public void StatusChanger(string status)
        {
            status_label.Text = status;
        }
    }
}
