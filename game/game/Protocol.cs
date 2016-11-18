using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game
{
    class Protocol
    {
        List<Message> information;

        public byte[] ForSend()
        {
            int level = 0;
            StringBuilder allText = new StringBuilder();
            foreach (var mes in information)
            {
                allText.Append(mes.ToString(level));
            }
            return new byte[1];
        }
    }
    abstract class Message
    {
        string mes;

        abstract string ToString(int level);
    }
    class Command:Message
    {
        List<Param> paramList;
    }
    class Param:Message
    {

    }
}
