using System;
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
    static class OnlineUser
    {
        public enum Status { Disconnect, Connected, Login, Play }
        static Status status = Status.Disconnect;
        static Socket connection = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        static Timer timer = new Timer();
        static int timeOut = 0;
        static int mesLength;
        static string[] inpMessage;
        static byte[] recievedBytes;
        static List<Action> Commands = new List<Action>();

        public static void Connect()
        {
            if (Commands.Count == 0) CommandsFiller();
            timer.Tick += Tick;
            timer.Interval = 1000 / Program.ups;
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
            inpMessage = Encoding.ASCII.GetString(recievedBytes)
                .Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            int comNum = Convert.ToInt32(inpMessage[0]);
            if (comNum != -1) Commands[comNum]();
            else Error();

        }
        private static void CheckTimeOut()
        {
            timeOut++;
            if (timeOut >= 10 * Program.ups)
            {
                Disconnect("-1\nTimeout!");
            }
        }
        public static void Send(string str)
        {
            if (!connection.Connected) Connect();
            connection.Send(Encoding.ASCII.GetBytes(str + "\0"));
        }
        public static void Disconnect(string str="You Disconeccted!")
        {
            if (connection.Connected)
            {
                connection.Send(Encoding.ASCII.GetBytes(str));
                connection.Close();
                MessageBox.Show(str, "Disconnect", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            status = Status.Disconnect;
            timer.Stop();
            timer.Tick -= Tick;
            //need add go to login Form
            
        }

        private static void Registration()
        {
           
        }
        private static void Login()
        {

        }
        private static void Error()
        {
            Disconnect(inpMessage[1]);
        }

        private static void CommandsFiller()
        {
            Commands.Add(Registration);
            Commands.Add(Login);
        }
    }
}
