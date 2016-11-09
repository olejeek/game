using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace game
{
    class OnlineUser
    {
        Socket connection;
        public bool online { get; private set; }
        bool charIsChoosen;
        string inputInfo;
        string outputInfo;
        int timeOut;
        int mesLength;
        byte[] message;

        public OnlineUser(Socket connection)
        {
            this.connection = connection;
            charIsChoosen = false;
            timeOut = 0;
            online = true;
            string answer = "You have successfully connected!";
            message = new byte[answer.Length+1];
            message[0] = 0;
            Encoding.ASCII.GetBytes(answer, 0, answer.Length, message, 1);
            connection.Send(message);
            //message = new byte[1024];
        }
        public void ContactHandler()
        {
            mesLength = connection.Available;
            if (mesLength==0)
            {
                timeOut++;
                if (timeOut>=5*60*Program.ups)
                {
                    string answer = "Timeout!";
                    message = new byte[answer.Length + 1];
                    message[0] = 255;
                    Encoding.ASCII.GetBytes(answer, 0, answer.Length, message, 1);
                    connection.Send(message);
                    connection.Close();
                    online = false;
                    return;
                }
            }
            timeOut = 0;
            message = new byte[mesLength];
        }
    }
}
