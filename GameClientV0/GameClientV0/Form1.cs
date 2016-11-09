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
            Socket a = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            a.Connect(IPAddress.Parse("172.20.53.7"), 1990);
            string login = loginBox.Text;
            string pswd = pswdBox.Text;
            string sendMes = login + "\n" + pswd;
            byte[] sendBytes = new byte[sendMes.Length+1];
            sendBytes[0] = 255;
            int numSendMes = Encoding.ASCII.GetBytes(sendMes, 0, sendMes.Length, sendBytes, 1);
            a.Send(sendBytes);
            a.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Socket a = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            a.Connect(IPAddress.Parse("172.20.53.7"), 1990);
            string login = loginBox.Text;
            string pswd = pswdBox.Text;
            string sendMes = login + "\n" + pswd;
            byte[] sendBytes = new byte[sendMes.Length + 1];
            sendBytes[0] = 254;
            int numSendMes = Encoding.ASCII.GetBytes(sendMes, 0, sendMes.Length, sendBytes, 1);
            a.Send(sendBytes);
            a.Close();
        }
    }
}
