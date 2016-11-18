﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
//using System.Timers;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Windows.Forms;

namespace GameClientV0
{
    interface IStatusChanger
    {
        void StatusChanger(string str);
    }
    static class OnlineUser
    {
        static HeroChoose heroChoose;
        static HeroCreation heroCreation;
        public enum Status { Disconnect, Connected, ChooseHero, Play }
        static public Status status = Status.Disconnect;
        static public event Action<string> ChangeStatus;
        static Socket connection = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        static Timer timer = new Timer();
        static int timeOut = 0;
        static int mesLength;
        static string[] inpBlocks;
        static string[] inpMessages;
        static byte[] recievedBytes;
        static List<Action> Commands = new List<Action>();

        static OnlineUser()
        {
            timer.Tick += Tick;
            timer.Interval = 1000 / Program.ups;
        }

        public static void Connect()
        {
            if (Commands.Count == 0) CommandsFiller();
            ChangeStatus += ((IStatusChanger)(Application.OpenForms[0])).StatusChanger;
            //timer.Tick += Tick;
            timer.Start();
            timeOut = 0;
            if (!connection.Connected)
            {
                connection = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                connection.Connect(IPAddress.Parse("172.20.53.7"), 1990);
                status = Status.Connected;
            }
        }
        private static void Tick(object sender, EventArgs e)
        {
            mesLength = connection.Available;
            if (mesLength == 0)
            {
                CheckTimeOut();
                return;
            }
            timeOut = 0;
            recievedBytes = new byte[mesLength];
            connection.Receive(recievedBytes);
            inpBlocks = Encoding.ASCII.GetString(recievedBytes)
                .Split(new char[] { '\0' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string block in inpBlocks)
            {
                inpMessages = block
                    .Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                int comNum = Convert.ToInt32(inpMessages[0]);
                if (comNum != -1) Commands[comNum]();
                else Error();
            }
            inpBlocks = null;

        }
        private static void CheckTimeOut()
        {
            timeOut++;
            if (timeOut >= 10 * Program.ups && status == Status.Play)
            {
                SendAndDisconnect("-1\nTimeout!");
            }
        }

        public static void Send(string str)
        {
            if (!connection.Connected) Connect();
            connection.Send(Encoding.ASCII.GetBytes(str + "\0"));
        }
        public static void SendAndDisconnect(string str)
        {
            if (connection.Connected)
            {
                connection.Send(Encoding.ASCII.GetBytes(str));
            }
            Disconnect(str);
        }
        public static void Disconnect(string str="You Disconnected!")
        {
            timer.Stop();
            timer.Tick -= Tick;
            if (connection.Connected)
            {
                connection.Close();
                MessageBox.Show(str, "Disconnect", MessageBoxButtons.OK);
            }
            status = Status.Disconnect;
            ChangeStatus(status.ToString());
            //need add go to login Form

        }

        private static void Registration()
        {
            Disconnect(inpMessages[1]);
        }
        private static void Login()
        {
            status = Status.ChooseHero;
            ChangeStatus(status.ToString());
        }
        private static void ChooseHero()
        {
            CloseAllFormsWhileLogin();
            if (Application.OpenForms[0].Visible) Application.OpenForms[0].Hide();
            heroChoose = new HeroChoose(inpMessages);
            heroChoose.Show();
        }
        private static void Error()
        {
            CloseAllFormsWhileLogin();
            Disconnect(inpMessages[1]);
        }

        public static void OpenFormToCreateHero()
        {
            heroCreation = new HeroCreation();
            heroCreation.Show();
        }
        private static void CloseAllFormsWhileLogin()
        {
            int formsCount = Application.OpenForms.Count;
            if (formsCount > 1)
            {
                for (int i = formsCount - 1; i > 0; i--)
                {
                    if (!(Application.OpenForms[i] is Form1)) Application.OpenForms[i].Close();
                }
            }
        }
        private static void CommandsFiller()
        {
            Commands.Add(Registration);
            Commands.Add(Login);
            Commands.Add(ChooseHero);
        }
    }
}