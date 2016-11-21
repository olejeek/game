using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
//using System.Timers;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Windows.Forms;
using game.Net.Protocol;

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
        static List<Action<Block>> Commands = new List<Action<Block>>();
        static List<Block> blocksToSend = new List<Block>();

        static OnlineUser()
        {
            if (Commands.Count == 0) CommandsFiller();
            ChangeStatus += ((IStatusChanger)(Application.OpenForms[0])).StatusChanger;
            timer.Tick += Tick;
            timer.Interval = 1000 / Program.ups;
        }

        public static void BlockToSend(Block block)
        {
            blocksToSend.Add(block);
        }

        public static void Connect()
        {
            try
            {
                if (!connection.Connected)
                {
                    connection = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    connection.Connect(IPAddress.Parse("172.20.53.7"), 1990);
                    status = Status.Connected;
                    ChangeStatus(status.ToString());
                }
                timer.Start();
                timeOut = 0;
            }
            catch
            {
                MessageBox.Show("Server Disabled!", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static void Send()
        {
            foreach (var block in blocksToSend)
            {
                connection.Send(Encoding.ASCII.GetBytes(block.ToString()));
                if (block.Code == (int)BlockCode.Disconnect)
                {
                    connection.Close();
                    status = Status.Disconnect;
                    break;
                }
            }
            blocksToSend.Clear();
        }
        private static void CloseConnection()
        {
            timer.Stop();
            connection.Close();
            status = Status.Disconnect;
            ChangeStatus(status.ToString());
            CloseAllFormsWhileLogin();
        }

        private static void Tick(object sender, EventArgs e)
        {
            mesLength = connection.Available;
            if (mesLength == 0)
            {
                CheckTimeOut();
                //return;
            }
            else
            {
                timeOut = 0;
                recievedBytes = new byte[mesLength];
                connection.Receive(recievedBytes);
                inpBlocks = Encoding.ASCII.GetString(recievedBytes)
                    .Split(new char[] { '\0' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string block in inpBlocks)
                {
                    Block input = new Block(block);
                    Commands[input.Code](input);
                    //inpMessages = block
                    //    .Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    //int comNum = Convert.ToInt32(inpMessages[0]);
                    //if (comNum != -1) Commands[comNum]();
                    //else Error();
                }
                inpBlocks = null;
            }
            
            Send();
        }
        private static void CheckTimeOut()
        {
            timeOut++;
            if (timeOut >= 10 * Program.ups && status == Status.Play)
            {
                MessageBox.Show("TimeOut!");
                //SendAndDisconnect("-1\nTimeout!");
            }
        }

        //public static void Send(string str)
        //{
        //    if (!connection.Connected) Connect();
        //    connection.Send(Encoding.ASCII.GetBytes(str + "\0"));
        //}
        //public static void SendAndDisconnect(string str)
        //{
        //    if (connection.Connected)
        //    {
        //        connection.Send(Encoding.ASCII.GetBytes(str));
        //    }
        //    Disconnect(str);
        //}
        //public static void Disconnect(string str="You Disconnected!")
        //{
        //    timer.Stop();
        //    if (connection.Connected)
        //    {
        //        connection.Close();
        //        MessageBox.Show(str, "Disconnect", MessageBoxButtons.OK);
        //    }
        //    status = Status.Disconnect;
        //    ChangeStatus(status.ToString());
        //    //need add go to login Form

        //}

        private static void Disconnect(Block block)
        {
            CloseConnection();
            DisconectType state = (DisconectType)block.Type;
            switch (state)
            {
                case DisconectType.Exit:
                    MessageBox.Show("Exit!", state.ToString(),
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                case DisconectType.Error:
                    MessageBox.Show("Wrong Command!", state.ToString(),
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case DisconectType.Timeout:
                    MessageBox.Show("Timeout!", state.ToString(),
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                default:
                    MessageBox.Show("Command Error!", state.ToString(),
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    break;
            }
        }
        private static void Registration(Block block)
        {
            CloseConnection();
            switch ((RegistrationType)block.Type)
            {
                case RegistrationType.CreateNewAcc:
                    MessageBox.Show("Accaunt successfully created!", "Registration",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                case RegistrationType.AccExists:
                    MessageBox.Show("Account already exists!", "Registration",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case RegistrationType.Unknown:
                    MessageBox.Show("Can not add accaunt in DataBase!", "Registration",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                default:
                    MessageBox.Show("Command Error!", "Registration",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
            }
        }
        private static void Login(Block block)
        {
            if (connection.Connected)
            {
                switch ((LoginType)block.Type)
                {
                    case LoginType.Access:
                        MessageBox.Show("You Successfully login!", "Login",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                        status = Status.ChooseHero;
                        ChangeStatus(status.ToString());
                        break;
                    case LoginType.PassError:
                        MessageBox.Show("You enter wrong password!", "Login",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        CloseConnection();
                        break;
                    case LoginType.Ban:
                        MessageBox.Show("Your accaunt banned!", "Login",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        CloseConnection();
                        break;
                    case LoginType.Unlogin:
                        MessageBox.Show("Accaunt not verified!", "Login",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        CloseConnection();
                        break;
                    case LoginType.Unknown:
                        MessageBox.Show("Error in working DataBase!", "Login",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        CloseConnection();
                        break;
                    default:
                        MessageBox.Show("Command Error!", "Login",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        CloseConnection();
                        break;
                }
            }
            else
            {
                MessageBox.Show("Connection Error", "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
        private static void ChooseHero(Block block)
        {
            ChooseHeroType type = (ChooseHeroType)block.Type;
            switch (type)
            {
                case ChooseHeroType.Select:
                    CloseAllFormsWhileLogin();
                    Application.OpenForms[0].Hide();
                    heroChoose = new HeroChoose(block.mes);
                    heroChoose.Show();
                    break;
                case ChooseHeroType.CreateHero:
                    MessageBox.Show("New Hero successfully created!", type.ToString(), 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                case ChooseHeroType.DeleteHero:
                    MessageBox.Show("New Hero successfully created!", type.ToString(),
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                case ChooseHeroType.HeroExists:
                    MessageBox.Show("Hero with this name was created earlier!", type.ToString(),
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                case ChooseHeroType.Unknown:
                    MessageBox.Show("Error with working DataBase!", type.ToString(),
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
            }
            //CloseAllFormsWhileLogin();
            //if (Application.OpenForms[0].Visible) Application.OpenForms[0].Hide();
            ////heroChoose = new HeroChoose(inpMessages);
            //heroChoose.Show();
        }
        private static void Play(Block block)
        {

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
                    if(Application.OpenForms[i] is Form1) Application.OpenForms[i].Show();
                    else Application.OpenForms[i].Close();
                }
            }
        }
        private static void CommandsFiller()
        {
            Commands.Add(Disconnect);
            Commands.Add(Registration);
            Commands.Add(Login);
            Commands.Add(ChooseHero);
            Commands.Add(Play);
        }
    }
}
