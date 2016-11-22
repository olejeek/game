using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using game.NetHandler;

namespace game.Net
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
        private System.Timers.Timer timer;
        //private ConcurrentDictionary<int,OnlineUser> usersOnline;
        //private List<OnlineUser> users;
        private ConcurrentQueue<OnlineUser> usersOnline;
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
            string ip = "";
            int port = 0;
            SetSettings(ref ip, ref port);
            this.ipAddress = IPAddress.Parse(ip);
            this.port = port;
            serverWork = false;
            //users = new List<OnlineUser>();
            usersOnline = new ConcurrentQueue<OnlineUser>();
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
                Console.WriteLine("Server was started early.");    //пишем красным, что он включен
                Console.ResetColor();
            }
        }


        private void ListenerWork()
        {
            serverWork = true;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Listener succesfully started!");
            Console.ResetColor();
            while (serverWork)
            {
                if (Reciever == null || Reciever.ThreadState == ThreadState.Stopped)
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
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Server succesfully started!");
            Console.ResetColor();
            timer = new System.Timers.Timer();
            timer.Elapsed += Tick;
            timer.AutoReset = true;
            timer.Interval = 1000 / Program.ups;
            timer.Enabled = true;
            //while (serverWork)
            //{
            //    OnlineUser temp;
            //    //if (usersOnline.TryDequeue(out temp))
            //    //{
            //    //    if (temp.status == OnlineUser.Status.Disconnect) continue;
            //    //    temp.Handler();
            //    //    usersOnline.Enqueue(temp);
            //    //}
            //}
        }
        private void ConnectionHandler()
        {
            Socket reciever = null;
            try
            {
                reciever = Listener.Accept();
                usersOnline.Enqueue(new OnlineUser(reciever));
            }
            catch (SocketException)
            {
                if (reciever != null) reciever.Close();
            }

        }

        private void Tick(object sender, System.Timers.ElapsedEventArgs e)
        {
            ConcurrentQueue<OnlineUser> temp = new ConcurrentQueue<OnlineUser>();
            Parallel.ForEach<OnlineUser>(usersOnline, user =>
            {
                user.Handler();
                if (user.status != OnlineUser.Status.Disconnect) temp.Enqueue(user);
            });
            usersOnline = temp;
            //Parallel.For(0, users.Count, (i) =>
            // {
            //     if (users[i].status == OnlineUser.Status.Disconnect)
            //     {
            //         users.Remove(users[i]);
            //     }
            //     else
            //     {
            //         users[i].Handler();
            //         //usersOnline.Enqueue(temp);
            //     }
                     
            // });
        }
    }
}
