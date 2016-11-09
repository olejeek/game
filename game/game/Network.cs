using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace game
{
    class Network
    {
        private static bool networkCreate = false;   //create or not class networkServer (class-singltone)
        private bool serverWork;
        private bool serverStopCommand;
        private IPAddress ipAddress;        //server ip-address 
        private int port;           //server port
        private Thread threadListener;
        private Thread Reciever;
        private ConcurrentDictionary<int,OnlineUser> usersOnline;
        private Thread threadWork;
        static private Socket Listener;         //socket, which receive connections
        public static Network CreateServer()
        {
            if (!networkCreate)         //if Network class created
            {
                networkCreate = true;
                return new Network();
            }
            else            //else...
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Chat Server was created early.");    //пишем красным, что он включен
                Console.ResetColor();
                return null;
            }
        }
        private static void SetSettings(ref string ip, ref int port)
        {
            if (!File.Exists("settings.txt"))
            {
                FileStream fs = File.Create("settings.txt");
                fs.Close();
            }
            StreamReader sr = new StreamReader("settings.txt");
            if (!sr.EndOfStream)
            {
                ip = sr.ReadLine();
                port = Convert.ToInt32(sr.ReadLine());
                Console.WriteLine("Current IP-address: " + ip);
                Console.WriteLine("Current port: " + port);
                Console.Write("Continue (y/n)? ");
                if (Console.ReadLine() == "y")
                {
                    sr.Close();
                    return;
                }
            }
            else
            {
                Console.WriteLine("No settings for Network Server.");
            }
            sr.Close();
            Console.Write("Enter server IP Address:");  //спрашиваем к какому ip подключать сервер
            ip = Console.ReadLine();
            Console.Write("Enter server number of port:");  //и на какой порт
            port = Convert.ToInt32(Console.ReadLine());
            StreamWriter sw = new StreamWriter("settings.txt"); //открываем файл с настройками
            sw.WriteLine(ip);        //и записываем туда адрес
            sw.WriteLine(port);             // и порт
            sw.Close();         //закрываем файл
        }

        private Network()
        {
            string ip="";
            int port=0;
            SetSettings(ref ip, ref port);
            this.ipAddress = IPAddress.Parse(ip);
            this.port = port;
            serverWork = false;
            usersOnline = new ConcurrentDictionary<int, OnlineUser>();
            Listener = new Socket(AddressFamily.InterNetwork, 
                SocketType.Stream, 
                ProtocolType.Tcp);      //create socket to listening port
        }

        public void Start()
        {
            if (!serverWork)
            {
                serverStopCommand = false;
                Console.WriteLine("Starting server...");
                Listener.Bind(new IPEndPoint(ipAddress, port));    //connect port to socket
                Listener.Listen(10);
                threadListener = new Thread(ListenerWork);
                threadListener.IsBackground = true;
                threadListener.Start();
                threadWork = new Thread(Work);
                threadWork.IsBackground = true;
                threadWork.Start();
            }
            else            //а если включен, то
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Chat Server was started early.");    //пишем красным, что он включен
                Console.ResetColor();
            }
        }


        private void ListenerWork()
        {
            serverWork = true;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Server succesfully started!");
            Console.ResetColor();
            while(serverWork)
            {
                if (Reciever==null || Reciever.ThreadState == ThreadState.Stopped)
                {
                    Reciever = new Thread(ConnectionHandler);
                    Reciever.IsBackground = true;
                    Reciever.Start();
                }
                if (serverStopCommand)
                {
                    if (Reciever.ThreadState == ThreadState.Running) Reciever.Join(50);
                    Listener.Close();
                    serverWork = false;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Server stoped!");
                    Console.ResetColor();
                }
            }
        }
        private void Work()
        {
            while(serverWork)
            {
                int maxCount = usersOnline.Count;
                for (int i=0; i<maxCount; i++)
                {
                    if (!usersOnline.ElementAt(i).Value.online)
                    {
                        OnlineUser del;
                        if (usersOnline.TryRemove(usersOnline.ElementAt(i).Key, out del))
                        {
                            del = null;
                            maxCount--;
                            continue;
                        }
                    }
                }
            }
        }
        private void ConnectionHandler()
        {
            Socket reciever = null;
            try
            {
                reciever = Listener.Accept();
                byte[] recievedBytes = new byte[1024];
                int numBytes = reciever.Receive(recievedBytes);
                string answer;
                if (recievedBytes[0]==255)    //login
                {
                    string mes = Encoding.ASCII.GetString(recievedBytes, 1, numBytes - 1);
                    string[] logins = mes.Split(new char[] { '\n', '\0' }, 2, StringSplitOptions.RemoveEmptyEntries);
                    string connectionString =
                        @"Provider=Microsoft.Jet.OLEDB.4.0; Data Source=Ragnarok.mdb";
                    using (OleDbConnection connection = new OleDbConnection(connectionString))
                    {
                        connection.Open();
                        string command = "SELECT * FROM users WHERE login='"
                            +logins[0]+"' AND pswd='"+logins[1]+"'";
                        OleDbCommand cmd = new OleDbCommand(command, connection);
                        OleDbDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            if ((bool)reader[3])
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("{0} banned!!!", reader[1]);
                                Console.ResetColor();
                                answer = "Your account was banned!";
                                byte[] sendBytes = new byte[answer.Length + 1];
                                Encoding.ASCII.GetBytes(answer, 0, answer.Length, sendBytes, 1);
                                sendBytes[0] = 255;
                                reciever.Send(sendBytes);
                                reciever.Close();
                            }
                            else if (usersOnline.ContainsKey((int)reader[0]))
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("{0} was logging yet!", reader[1]);
                                Console.ResetColor();
                                answer = "Your account was logging yet!";
                                byte[] sendBytes = new byte[answer.Length + 1];
                                Encoding.ASCII.GetBytes(answer, 0, answer.Length, sendBytes, 1);
                                sendBytes[0] = 255;
                                reciever.Send(sendBytes);
                                reciever.Close();
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("{0} (ID: {1}) login", reader[1], reader[0]);
                                Console.ResetColor();
                                while(!usersOnline.TryAdd((int)reader[0], new OnlineUser(reciever)));
                            }
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Login Error {0}", logins[0]);
                            Console.ResetColor();
                            answer = "Login Error!";
                            byte[] sendBytes = new byte[answer.Length + 1];
                            Encoding.ASCII.GetBytes(answer, 0, answer.Length, sendBytes, 1);
                            sendBytes[0] = 255;
                            reciever.Send(sendBytes);
                            reciever.Close();
                        }
                        connection.Close();
                    }
                }
                else if (recievedBytes[0] == 254) //create new account
                {
                    string mes = Encoding.ASCII.GetString(recievedBytes, 1, numBytes - 1);
                    string[] logins = mes.Split(new char[] { '\n' }, 2, StringSplitOptions.RemoveEmptyEntries);
                    string connectionString =
                        @"Provider=Microsoft.Jet.OLEDB.4.0; Data Source=Ragnarok.mdb";
                    using (OleDbConnection connection = new OleDbConnection(connectionString))
                    {
                        connection.Open();
                        string command = "SELECT * FROM users WHERE login='"
                            + logins[0] + "'";
                        OleDbCommand cmd = new OleDbCommand(command, connection);
                        OleDbDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("User {0} (ID: {1}) was created early.", reader[1], reader[0]);
                            Console.ResetColor();
                            answer = "Accaunt "+reader[1].ToString()+" was created early.";
                            byte[] sendBytes = new byte[answer.Length + 1];
                            Encoding.ASCII.GetBytes(answer, 0, answer.Length, sendBytes, 1);
                            sendBytes[0] = 255;
                            reciever.Send(sendBytes);
                            reciever.Close();
                        }
                        else
                        {
                            command = "INSERT INTO users (login,pswd,ban) VALUES ('"+logins[0]+"', '"+logins[1]+"', false);";
                            cmd = new OleDbCommand(command, connection);
                            int i = cmd.ExecuteNonQuery();
                            if (i==1)
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("New account was added");
                                Console.ResetColor();
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Error add new account");
                                Console.ResetColor();
                                reciever.Close();
                            }
                        }
                        connection.Close();
                    }
                }
                else
                {

                }
                
            }
            catch (SocketException)
            {
                if (reciever != null) reciever.Close();
            }
        }
    }
}
