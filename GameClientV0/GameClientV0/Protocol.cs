using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game.Net.Protocol
{
    //block code
    public enum BlockCode { Disconnect, Registration, Login, ChooseHero, Play }
    //-----BlockCode Description-----
    //0. Disconnect.
    //  From Client: client end work with server (unlogin or exit)
    //  From Server: query to close connection
    //1. Registration.
    //  From Client: send datas to registration
    //  From Server: server send answer to registration query
    //2. Login.
    //  From Client: send data to login
    //  From Server: server send answer to login query
    //3. ChooseHero.
    //  From Client: user do some manipulation with heroes
    //  From Server: server send heroes list, also answer to user manipulation
    //4. Play/
    //  From Client: send hero actions
    //  From Server: send world information
    //--------------------------------
    public enum DisconectType { Exit, Error = -1, Timeout = -2 }
    //-----Disconnect Description-----
    //0. Exit.
    //  From Client: end of work
    //  From Server: end of registration
    //-1. Error.
    //  From Client: ...
    //  From Server: send error (wrong command)
    //-2. Timeout.
    //  From Client: no data from server 30 seconds
    //  From Server: no data from client 5 minutes
    //--------------------------------
    public enum RegistrationType { CreateNewAcc, AccExists = -1, Unknown = -2 }
    //-----Registration Description-----
    //0. CreateNewAcc.
    //  From Client: registration datas
    //  From Server: successful request
    //-1. AccExists.
    //  From Client: not use
    //  From Server: account already exists
    //-2. AccExists.
    //  From Client: not use
    //  From Server: error to write information in database
    //----------------------------------
    public enum LoginType { Access, PassError = -1, Ban = -2, Unlogin = -3, Unknown = -4 }
    //-----Login Description-----
    //0. Access.
    //  From Client: login datas
    //  From Server: successful request
    //-2. PassError.
    //  From Client: not use
    //  From Server: wrong password
    //-2. Ban.
    //  From Client: not use
    //  From Server: account banned
    //-3. Unlogin.
    //  From Client: not use
    //  From Server: unlogin, but request need login
    //-4. Unknown.
    //  From Client: not use
    //  From Server: error in work with database
    //---------------------------
    public enum ChooseHeroType { Select, CreateHero, DeleteHero, HeroExists = -1, Unknown = -2 }
    //-----ChooseHero Description-----
    //0. Select.
    //  From Client: user select hero
    //  From Server: hero list
    //1. CreateHero.
    //  From Client: data about new hero
    //  From Server: successful request
    //2. DeleteHero.
    //  From Client: user delete hero
    //  From Server: successful request
    //--------------------------------
    public enum PlayType { }

    class Block
    {
        
        private BlockCode code;
        public int Code
        {
            get { return (int)code; }
        }

        public int Type { get; private set; }
        public List<string> mes { get; private set; }

        public Block(BlockCode code)
        {
            mes = new List<string>();
            this.code = code;
        }
        public Block(BlockCode code, int type)
        {
            mes = new List<string>();
            this.code = code;
            this.Type = type;
        }
        public Block(BlockCode code, int type, string message)
        {
            mes = new List<string>();
            this.code = code;
            this.Type = type;
            mes.Add(message);
            Console.WriteLine(message);
        }
        public Block(string inputMessage)
        {
            mes = new List<string>();
            string[] info = inputMessage.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            code = (BlockCode)Enum.Parse(typeof(BlockCode), info[0]);
            Type = Convert.ToInt32(info[1]);
            for (int i = 2; i < info.Length; i++) mes.Add(info[i]);

        }
        public void Add(int type)
        {
            this.Type = type;
        }
        public void Add(string message)
        {
            mes.Add(message);
            Console.WriteLine(message);
        }
        public void Add(int type, string message)
        {
            this.Type = type;
            mes.Add(message);
        }

        public override string ToString()
        {
            StringBuilder answer = new StringBuilder();
            answer.Append((int)code);
            answer.Append('\n');
            answer.Append(Type);
            answer.Append('\n');
            foreach (var m in mes)
            {
                answer.Append(m);
                answer.Append('\n');
            }
            answer.Append('\0');
            return answer.ToString();
        }
    }
}
