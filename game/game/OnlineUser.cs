using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using game.Net.Protocol;

namespace game.NetHandler
{
    class OnlineUser
    {
        
        string db =
                        @"Provider=Microsoft.Jet.OLEDB.4.0; Data Source=Ragnarok.mdb";
        public enum Status { Disconnect, Connected, Login, Play}
        Socket connection;
        public Status status { get; private set; }
        int loginId;
        List<Action<Block>> Commands;
        int mesLength;
        byte[] recievedBytes;
        string input;
        string[] inpMessage;
        string[] inpBlocks;
        DateTime lastInput;
        List<Block> blocksToSend;

        public OnlineUser(Socket connection)
        {
            this.connection = connection;
            this.loginId = -1;
            lastInput = DateTime.Now;
            status = Status.Connected;
            blocksToSend = new List<Block>();
            CommandsFiller();
            input = "";
        }
        //Sending Methods
        //private void Send(BlockType type, BlockRez rez, string str)
        //{
        //    StringBuilder answer = new StringBuilder();
        //    answer.Append((int)type);
        //    answer.Append('\n');
        //    answer.Append((int)rez);
        //    answer.Append('\n');
        //    answer.Append(str);
        //    answer.Append('\0');
        //    connection.Send(Encoding.ASCII.GetBytes(answer.ToString()));
        //}
        private void Send()
        {
            foreach (var block in blocksToSend)
            {
                connection.Send(Encoding.ASCII.GetBytes(block.ToString()));
                if (block.Code == (int)BlockCode.Disconnect || block.Type < 0)
                {
                    Disconnect(block);
                    break;
                }
            }
            blocksToSend.Clear();
        }
        //private void SendAndDisconnect(BlockType type, BlockRez rez, string str)
        //{
        //    StringBuilder answer = new StringBuilder();
        //    answer.Append((int)type);
        //    answer.Append('\n');
        //    answer.Append((int)rez);
        //    answer.Append('\n');
        //    answer.Append(str);
        //    answer.Append('\0');
        //    connection.Send(Encoding.ASCII.GetBytes(answer.ToString()));
        //    Console.WriteLine("{0}: {1}", loginId, str);
        //    status = Status.Disconnect;
        //    connection.Close();
        //}
        //private void SendAndDisconnect()
        //{
        //    Send();
        //    //Console.WriteLine("{0}: {1}", loginId, str);
        //    status = Status.Disconnect;
        //    connection.Close();
        //}
        public void Handler()
        {
            mesLength = connection.Available;
            if (mesLength == 0)
            {
                CheckTimeOut();
                return;
            }
            lastInput = DateTime.Now;
            recievedBytes = new byte[mesLength];
            connection.Receive(recievedBytes);
            input += Encoding.ASCII.GetString(recievedBytes);
            if (input[input.Length - 1] != '\0') return;
            inpBlocks = input.Split(new char[] { '\0' }, StringSplitOptions.RemoveEmptyEntries);
            input = "";
            foreach (string block in inpBlocks)
            {
                Block input = new Block(block);
                Commands[input.Code](input);
                //inpMessage = block.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                //int comNum = Convert.ToInt32(inpMessage[0]);
                //if (comNum != -1) Commands[Convert.ToInt32(comNum)]();
                //else
                //{
                //    Disconnect();
                //}

            }
            Send();
        }

        private void CheckTimeOut()
        {
            TimeSpan delta = lastInput - DateTime.Now;
            if (delta.Minutes > 5) blocksToSend.Add(new Block(BlockCode.Disconnect, 
                (int)DisconectType.Timeout));
        }
        //First Level messages
        private void Disconnect(Block block)
        {
            connection.Close();
            status = Status.Disconnect;
            Console.WriteLine("Id: {0} disconnected. Code: {1}. Type: {2}.", 
                loginId, block.Code, block.Type);
        }
        private void Registration(Block block)
        {
            if (status != Status.Connected)
            {
                blocksToSend.Add(new Block(BlockCode.Disconnect, 
                    (int)DisconectType.Error));
                //SendAndDisconnect(BlockType.Disconnect, BlockRez.Error, "Connection Error");
                return;
            }
            string[] logins = block.mes[0].Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
            Block regBlock = new Block(BlockCode.Registration);
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
                    string answer = "User "+reader[1]+" was created early.";
                    Console.WriteLine(answer);
                    Console.ResetColor();
                    regBlock.Add((int)RegistrationType.AccExists);
                    blocksToSend.Add(regBlock);
                    dbConnect.Close();
                    return;
                }
                else
                {
                    command = "INSERT INTO users (login,pswd,ban) VALUES ('" + logins[0] + "', '" +
                        logins[1] + "', false);";
                    cmd = new OleDbCommand(command, dbConnect);
                    int i = cmd.ExecuteNonQuery();
                    if (i == 1)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        string answer = "Accaunt " + logins[0] + " successfully created.";
                        Console.WriteLine(answer);
                        Console.ResetColor();
                        regBlock.Add((int)RegistrationType.CreateNewAcc);
                        blocksToSend.Add(regBlock);
                    }
                    else
                    {
                        string answer = "Error add new account";
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(answer);
                        Console.ResetColor();
                        regBlock.Add((int)RegistrationType.Unknown);
                        blocksToSend.Add(regBlock);
                    }
                    dbConnect.Close();
                }
            }
        }
        private void Login(Block block)
        {
            if (status != Status.Connected)
            {
                blocksToSend.Add(new Block(BlockCode.Disconnect, (int)DisconectType.Error));
                //SendAndDisconnect(((int)BlockCode.Error).ToString()+"\nConnection Error");
                return;
            }
            string[] logins = block.mes[0].Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
            Block logBlock = new Block(BlockCode.Login);
            using (OleDbConnection dbConnect = new OleDbConnection(db))
            {
                dbConnect.Open();
                string command = "SELECT * FROM users WHERE login='" + logins[0] + "';";
                //string command = "SELECT * FROM users WHERE login='"
                //    + logins[0] + "' AND pswd='" + logins[1] + "'";
                OleDbCommand cmd = new OleDbCommand(command, dbConnect);
                OleDbDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    if (logins[1]!=(string)reader[2])
                    {
                        string answer = logins[0] + " enter wrong password!";
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(answer);
                        Console.ResetColor();
                        logBlock.Add((int)LoginType.PassError);
                        blocksToSend.Add(logBlock);
                        dbConnect.Close();
                        return;
                    }
                    else if ((bool)reader[3])
                    {
                        string answer = logins[0]+" banned!";
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(answer);
                        Console.ResetColor();
                        logBlock.Add((int)LoginType.Ban);
                        blocksToSend.Add(logBlock);
                        dbConnect.Close();
                        return;
                        //SendAndDisconnect(((int)BlockCode.Error).ToString()+"\n" + answer );
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        string answer = reader[1] + " online";
                        Console.WriteLine(answer);
                        Console.ResetColor();
                        loginId = (int)reader[0];
                        status = Status.Login;
                        logBlock.Add((int)LoginType.Access);
                        blocksToSend.Add(logBlock);
                        dbConnect.Close();
                        return;
                        //Send(((int)BlockCode.Login).ToString()+"\n" + answer );
                        //Send( HeroesList(dbConnect));
                    }
                }
                else
                {
                    string answer = "Login Error " + logins[0];
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(answer);
                    Console.ResetColor();
                    logBlock.Add((int)LoginType.Unknown);
                    blocksToSend.Add(logBlock);
                    dbConnect.Close();
                    return;
                    //SendAndDisconnect(((int)BlockCode.Error).ToString()+"\n" + answer);
                }
                
            }
        }
        private void ChooseHero(Block block)
        {
            if (status != Status.Login)
            {
                blocksToSend.Add(new Block(BlockCode.Disconnect, (int)DisconectType.Error));
                //SendAndDisconnect(((int)BlockCode.Error).ToString()+"\nConnection Error");
                return;
            }
            ChooseHeroType type = (ChooseHeroType)block.Type;
            switch(type)
            {
                case ChooseHeroType.Select: break;
                case ChooseHeroType.CreateHero: break;
                case ChooseHeroType.DeleteHero: break;
            }
        }
        private void Play(Block block)
        {
            if (status != Status.Login)
            {
                blocksToSend.Add(new Block(BlockCode.Disconnect, (int)DisconectType.Error));
                //SendAndDisconnect(((int)BlockCode.Error).ToString()+"\nConnection Error");
                return;
            }
        }

        //Second Level messages (code ChooseHero)
        private Block CreateHero(Block block)
        {
            Block nHero = new Block(BlockCode.ChooseHero);
            using (OleDbConnection dbConnect = new OleDbConnection(db))
            {
                dbConnect.Open();
                string command = "SELECT name FROM players WHERE name='" + block.mes[0] + "';";
                OleDbCommand cmd = new OleDbCommand(command, dbConnect);
                OleDbDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    nHero.Add((int)ChooseHeroType.HeroExists);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("ID: {0}. Code: {1}. Type: {2}", loginId, nHero.Code, nHero.Type);
                    Console.ResetColor();
                    dbConnect.Close();
                    return nHero;
                    //Send(((int)BlockCode.CreateHero).ToString() + "\n" + ((int)BlockCode.Error).ToString() +
                    //    "-1\tHero with name " + reader[0] + " was created early.");
                }
                else
                {
                    command = string.Format("INSERT INTO players([loginId], [name], [str], [agi], " +
                        "[vit], [int], [dex], [luk]) VALUES ({0}, \'{1}\', {2}, {3}, {4}, {5}, {6}, {7});",
                       loginId, block.mes[0], block.mes[1], block.mes[2], block.mes[3], block.mes[4], 
                       block.mes[5], block.mes[6]);
                    cmd = new OleDbCommand(command, dbConnect);
                    if (cmd.ExecuteNonQuery() == 1)
                    {
                        nHero.Add((int)ChooseHeroType.CreateHero);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("ID: {0} Code: {1} Type: {2}", loginId, nHero.Code, nHero.Type);
                        Console.ResetColor();
                        dbConnect.Close();
                        return nHero;
                        //Send("2\n0\tHero " + param[0] + " successfully created.");
                    }
                    else
                    {
                        nHero.Add((int)ChooseHeroType.Unknown);
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Error add new hero");
                        Console.ResetColor();
                        dbConnect.Close();
                        return nHero;
                        //Send("2\n-1\nError with creating hero.");
                    }
                }

            }
        }

        //private void CreateHero()
        //{
        //    if (status != Status.Login)
        //    {
        //        blocksToSend.Add(new Block(BlockCode.Login, (int)LoginType.Unlogin);
        //        //SendAndDisconnect(((int)BlockCode.Error).ToString()+"\nConnection Error");
        //        //SendAndDisconnect("-1\nConnection Error");
        //        return;
        //    }
        //    //string[] param = DevidedLevel2(inpMessage[1]);
        //    string[] param = inpMessage[1].Split('\t');
        //    Block crhBlock = new Block(BlockCode.);
        //    using (OleDbConnection dbConnect = new OleDbConnection(db))
        //    {
        //        dbConnect.Open();
        //        string command = "SELECT name FROM players WHERE name='" + param[0] + "';";
        //        OleDbCommand cmd = new OleDbCommand(command, dbConnect);
        //        OleDbDataReader reader = cmd.ExecuteReader();
        //        if (reader.Read())
        //        {
        //            Console.ForegroundColor = ConsoleColor.Red;
        //            Console.WriteLine("Hero with name {0} was created early.", reader[0]);
        //            Console.ResetColor();
        //            Send(((int)BlockCode.CreateHero).ToString()+"\n"+ ((int)BlockCode.Error).ToString() + 
        //                "-1\tHero with name " + reader[0] + " was created early.");
        //        }
        //        else
        //        {
        //            command = string.Format("INSERT INTO players([loginId], [name], [str], [agi], "+
        //                "[vit], [int], [dex], [luk]) VALUES ({0}, \'{1}\', {2}, {3}, {4}, {5}, {6}, {7});",
        //               loginId, param[0], param[1], param[2], param[3], param[4], param[5], param[6]);
        //            cmd = new OleDbCommand(command, dbConnect);
        //            if (cmd.ExecuteNonQuery() == 1)
        //            {
        //                Console.ForegroundColor = ConsoleColor.Green;
        //                Console.WriteLine("ID: {0} create new hero - {0}", loginId, param[0]);
        //                Console.ResetColor();
        //                Send("2\n0\tHero " + param[0] + " successfully created.");
        //            }
        //            else
        //            {
        //                Console.ForegroundColor = ConsoleColor.Red;
        //                Console.WriteLine("Error add new hero");
        //                Console.ResetColor();
        //                Send("2\n-1\nError with creating hero.");
        //            }
        //        }
        //        dbConnect.Close();
        //    }

        //}
        private void DeleteHero()
        {
            if (status != Status.Login)
            {
                //SendAndDisconnect("-1\nConnection Error");
                return;
            }
        }
        private string HeroesList(OleDbConnection dbConnect)
        {
            StringBuilder heroInfo = new StringBuilder();
            heroInfo.Append("2\n");
            if (dbConnect.State== System.Data.ConnectionState.Open)
            {
                string command = "SELECT id, name, classId, str, agi, vit, int, dex, luk,"+
                    "baseLvl, jobLvl, locId AS HeroInfo FROM players WHERE loginid=" + loginId + ";";
                OleDbCommand cmd = new OleDbCommand(command, dbConnect);
                OleDbDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        heroInfo = heroInfo.Append(reader[i].ToString());
                        heroInfo = heroInfo.Append('\t');
                    }
                    heroInfo.Append('\n');
                }
            }
            heroInfo.Append('\0');
            return heroInfo.ToString();
        }

        //Disconnect, Registration, Login, ChooseHero, Play
    private void CommandsFiller()
        {
            Commands = new List<Action<Block>>();
            Commands.Add(Disconnect);
            Commands.Add(Registration);
            Commands.Add(Login);
            Commands.Add(ChooseHero);
            Commands.Add(Play);
        }


    }
}
