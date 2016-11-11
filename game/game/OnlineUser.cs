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
        public enum Status { Disconnect, Connected, Login, Play}
        Socket connection;
        public Status status { get; private set; }
        int loginId;
        List<Action> Commands;
        int timeOut;
        int mesLength;
        byte[] recievedBytes;
        string[] inpMessage;
        string[] inpBlocks;

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
                inpMessage = Encoding.ASCII.GetString(recievedBytes)
                .Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                Commands[Convert.ToInt32(inpMessage[0])]();
            }
            //inpMessage = Encoding.ASCII.GetString(recievedBytes)
            //    .Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            //Commands[Convert.ToInt32(inpMessage[0])]();
        }

        private void CheckTimeOut()
        {
            timeOut++;
            if (timeOut >= 5 * 60 * Program.ups)
            {
                SendAndDisconnect("1\nTimeout!");
            }
        }

        private void Registration()
        {
            if (status != Status.Connected)
            {
                SendAndDisconnect("-1\nConnection Error");
                return;
            }
            string[] logins = inpMessage[1].Split('\t');
            using (OleDbConnection dbConnect = new OleDbConnection(db))
            {
                dbConnect.Open();
                string command = "SELECT * FROM users WHERE login='"
                    + logins[0] + "'";
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
                    command = "INSERT INTO users (login,pswd,ban) VALUES ('" + logins[0] + "', '" + logins[1] + "', false);";
                    cmd = new OleDbCommand(command, dbConnect);
                    int i = cmd.ExecuteNonQuery();
                    if (i == 1)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("New account was added");
                        Console.ResetColor();
                        SendAndDisconnect("0\nAccaunt " + logins[0] + " successfully created.");
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
            string[] logins = inpMessage[1].Split('\t');
            if (status != Status.Connected)
            {
                SendAndDisconnect("-1\nConnection Error");
                return;
            }
            using (OleDbConnection dbConnect = new OleDbConnection(db))
            {
                dbConnect.Open();
                string command = "SELECT * FROM users WHERE login='"
                    + logins[0] + "' AND pswd='" + logins[1] + "'";
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
                        loginId = (int)reader[0];
                        status = Status.Login;
                        string output = "0\nAccess to" + reader[1] + "is allowed";
                        output += HeroesList(dbConnect);
                        Send(output);
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Login Error {0}", logins[0]);
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
            connection.Send(Encoding.ASCII.GetBytes(str+"\0"));
        }
        private void SendAndDisconnect(string str)
        {
            connection.Send(Encoding.ASCII.GetBytes(str+"\0"));
            status = Status.Disconnect;
            connection.Close();
        }

        private string HeroesList(OleDbConnection dbConnect)
        {
            StringBuilder heroInfo = new StringBuilder();
            if (dbConnect.State== System.Data.ConnectionState.Open)
            {
                string command = "SELECT * FROM players WHERE loginid='" + loginId + "'";
                OleDbCommand cmd = new OleDbCommand(command, dbConnect);
                OleDbDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        heroInfo = heroInfo.Append(reader[i].ToString());
                        heroInfo = heroInfo.Append('\t');
                    }
                }
                heroInfo.Append('\n');
            }
            return heroInfo.ToString();
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
    }

}
