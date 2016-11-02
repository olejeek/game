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
        private BlockingCollection<Socket> connectList;
        private List<Thread> threadsList;
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
                Console.Write("Do you want change settings (y/n)? ");
                if (Console.ReadLine() != "y")
                {
                    sr.Close();
                    return;
                }
            }
            else
            {
                Console.WriteLine("No settings for Chat Server.");
            }
            sr.Close();
            Console.Write("Enter server IP Address (input Any for all ip`s:");  //спрашиваем к какому ip подключать сервер
            ip = Console.ReadLine();
            ip.ToLower();
            ip = ip == "any" ? "Any" : ip;
            Console.Write("Enter server number of port:");  //и на какой порт
            port = Convert.ToInt32(Console.ReadLine());
            StreamWriter sw = new StreamWriter("settings.txt"); //открываем файл с настройками
            sw.WriteLine(ip);        //и записываем туда адрес
            sw.WriteLine(port);             // и порт
            sw.Close();         //закрываем файл
            Directory.CreateDirectory(ip);
        }

        private Network()
        {
            string ip="";
            int port=0;
            SetSettings(ref ip, ref port);
            this.ipAddress = IPAddress.Parse(ip);
            this.port = port;
            serverWork = false;
            //connectList = new ConcurrentBag<Socket>();
            threadsList = new List<Thread>();
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
                threadListener = new Thread(Work);
                threadListener.Start();
            }
            else            //а если включен, то
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Chat Server was started early.");    //пишем красным, что он включен
                Console.ResetColor();
            }
        }


        private void Work()
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
        private void ConnectionHandler()
        {
            Socket reciever = null;
            try
            {
                reciever = Listener.Accept();
                
            }
            catch (SocketException)
            {
                if (reciever != null) reciever.Close();
            }
        }
    }
}
