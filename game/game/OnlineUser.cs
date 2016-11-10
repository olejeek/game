using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace game
{
    class OnlineUser
    {
        string db =
                        @"Provider=Microsoft.Jet.OLEDB.4.0; Data Source=Ragnarok.mdb";
        public enum Status { Disconnect, Connected, Login, HeroChoose, Play}
        Socket connection;
        public Status status { get; private set; }
        int loginId;
        List<Action> Commands;
        int timeOut;
        int mesLength;
        byte[] recievedBytes;
        string[] inpMessage;
        enum CommandCode { EndOfCommand, Disconnect, Login, CreateHero, DeleteHero, ChooseHero}

        public OnlineUser(Socket connection)
        {
            this.connection = connection;
            this.loginId = -1;
            timeOut = 0;
            status = Status.Connected;
            CommandsFiller();
        }
        public void Handler()
        {
            mesLength = connection.Available;
            if (mesLength == 0 && CheckTimeOut()) return;
            timeOut = 0;
            recievedBytes = new byte[mesLength];
            connection.Receive(recievedBytes);
            inpMessage = Encoding.ASCII.GetString(recievedBytes)
                .Split(new char[] { '\n', '\0' }, StringSplitOptions.RemoveEmptyEntries);
            Commands[Convert.ToInt32(inpMessage[0])]();
        }

        private bool CheckTimeOut()
        {
            timeOut++;
            if (timeOut >= 5 * 60 * Program.ups)
            {
                SendAndDisconnect("1\nTimeout!");
                return true;
            }
            else return false;
        }

        private void Registration()
        {
            if (status != Status.Connected)
            {
                SendAndDisconnect("-1\nConnection Error");
                return;
            }
            using (OleDbConnection dbConnect = new OleDbConnection(db))
            {
                dbConnect.Open();
                string command = "SELECT * FROM users WHERE login='"
                    + inpMessage[1] + "'";
                OleDbCommand cmd = new OleDbCommand(command, dbConnect);
                OleDbDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("User {0} (ID: {1}) was created early.", reader[1], reader[0]);
                    Console.ResetColor();
                    SendAndDisconnect("-1\nAccaunt " + reader[1].ToString() + " was created early.");
                }
                else
                {
                    command = "INSERT INTO users (login,pswd,ban) VALUES ('" + inpMessage[1] + "', '" + inpMessage[2] + "', false);";
                    cmd = new OleDbCommand(command, dbConnect);
                    int i = cmd.ExecuteNonQuery();
                    if (i == 1)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("New account was added");
                        Console.ResetColor();
                        SendAndDisconnect("0\nAccaunt " + inpMessage[1] + " successfully created.");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Error add new account");
                        Console.ResetColor();
                        SendAndDisconnect("-1\nError with creating accaunt.");
                    }
                }
                dbConnect.Close();
            }
        }
        private void Login()
        {
            if (status != Status.Connected)
            {
                SendAndDisconnect("-1\nConnection Error");
                return;
            }
            using (OleDbConnection dbConnect = new OleDbConnection(db))
            {
                dbConnect.Open();
                string command = "SELECT * FROM users WHERE login='"
                    + inpMessage[1] + "' AND pswd='" + inpMessage[2] + "'";
                OleDbCommand cmd = new OleDbCommand(command, dbConnect);
                OleDbDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    if ((bool)reader[3])
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("{0} banned!!!", reader[1]);
                        Console.ResetColor();
                        SendAndDisconnect("-1\nYour account was banned!");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("{0} (ID: {1}) login", reader[1], reader[0]);
                        Console.ResetColor();
                        Send("0\nAccess to" + reader[1] + "is allowed");
                        loginId = (int)reader[0];
                        status = Status.Login;
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Login Error {0}", inpMessage[1]);
                    Console.ResetColor();
                    SendAndDisconnect("-1\nLogin Error!");
                }
                dbConnect.Close();
            }
        }
        private void CreateHero()
        {
            if (status != Status.Login)
            {
                SendAndDisconnect("-1\nConnection Error");
                return;
            }
        }
        private void DeleteHero()
        {
            if (status != Status.Login)
            {
                SendAndDisconnect("-1\nConnection Error");
                return;
            }
        }
        private void ChooseHero()
        {
            if (status != Status.Login)
            {
                SendAndDisconnect("-1\nConnection Error");
                return;
            }
        }

        private void Send(string str)
        {
            connection.Send(Encoding.ASCII.GetBytes(str));
        }
        private void SendAndDisconnect(string str)
        {
            connection.Send(Encoding.ASCII.GetBytes(str));
            status = Status.Disconnect;
            connection.Close();
        }

        private void CommandsFiller()
        {
            Commands = new List<Action>();
            Commands.Add(Registration);
            Commands.Add(Login);
            Commands.Add(CreateHero);
            Commands.Add(DeleteHero);
            Commands.Add(ChooseHero);
        }
        private void OutputChooseHero()
        {
            string connectionString =
                @"Provider=Microsoft.Jet.OLEDB.4.0; Data Source=Ragnarok.mdb";
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                connection.Open();
                string command = "SELECT (id, name, class, baseLvl, jobLvl, str, agi, vit, int, dex, luk, curHP, curSP, locId) FROM players WHERE loginId=" + loginId;
                OleDbCommand cmd = new OleDbCommand(command, connection);
                OleDbDataReader reader = cmd.ExecuteReader();
                List<PlayerInfo> heroList = new List<PlayerInfo>();
                while (reader.Read())
                {
                    heroList.Add(new PlayerInfo(reader));
                }
                connection.Close();
            }
        }
    }

    struct PlayerInfo
    {
        int id;
        string name;
        int Class;
        int baseLvl;
        int jobLvl;
        int Str;
        int Agi;
        int Vit;
        int Int;
        int Dex;
        int Luk;
        int curHP;
        int curSP;
        int locId;
        public PlayerInfo(OleDbDataReader r)
        {
            id = (int)r[0];
            name = r[1].ToString();
            Class = (int)r[2];
            baseLvl = (int)r[3];
            jobLvl = (int)r[4];
            Str = (int)r[5];
            Agi = (int)r[6];
            Vit = (int)r[7];
            Int = (int)r[8];
            Dex = (int)r[9];
            Luk = (int)r[10];
            curHP = (int)r[11];
            curSP = (int)r[12];
            locId = (int)r[13];
        }
    }
}
