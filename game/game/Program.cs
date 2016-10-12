using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            Location loc1 = new Location();
            loc1.LocationLive();
            Console.Read();
        }
    }

    abstract class Person
    {
        protected enum Status { Die, Idle, Move, Atack}
        protected Status status;    //person status
        protected Action nextAction;    //what person must do
        internal int locId;    //location person Id
        protected int hp;       //hit points
        protected int atack;    //atack
        protected float aspd;   //atack speed
        protected float mspd;   //move speed
        protected float arng;   //atack range
        protected float vrng;   //view range
        protected Location loc; //location, where person are situated
        protected coord pos;    //position on the location
        protected Person Target;            //target for atack
        internal DateTime timeForAction;    //time for person action
        protected Dictionary<Person, int> whoAround;  //all persons, which person view
        public Person(int id, Location loc)                     //constructor of person
        {
            whoAround = new Dictionary<Person, int>();
            this.locId = id;
            this.loc = loc;
        }
        virtual internal void Thinking()  //person thinking about next action
        {
            LookAround();
            foreach (var p in whoAround)
            {
                Console.WriteLine("{0} see {1} on range2 {2}", this.locId, p.Key.locId, p.Value);
            }
            if (hp <= 0) status = Status.Die;
        }
        void LookAround()  //search persons around person and square range
        {
            whoAround.Clear();
            foreach (Person p in loc.persons)
            {
                int range2 = pos - p.pos;
                if (range2 <= vrng * vrng && p!=this) whoAround.Add(p, range2);
            }
        }
        abstract internal void Do();        //Person action
        abstract internal void Step();      //person take step
        abstract internal void Atack();     //person take phisical damage
    }

    class Mob : Person
    {
        enum Behavior{Agressive, FeelCast, Helpful, ChangeTarget, Looter  };
        List<Behavior> behav;   //mob behaviors
        uint respTime;           //mob respawn time
        public Mob(int id, Location loc, string behav) : base(id, loc)
        {
            this.behav = new List<Behavior>();
            if (behav!="")
            {
                string[] behavs = behav.Split(',');
                foreach (var str in behavs)
                    this.behav.Add((Behavior)Enum.Parse(typeof(Behavior), str, true));
            }
            respTime = 5000;
            hp = 100;
            atack = 1;
            aspd = 1;
            mspd = 1;
            arng = 1.5F;
            vrng = 5;
            pos.StartPos(id, id);

        }
        internal override void Thinking()
        {
            base.Thinking();
            switch (status)
            {
                case Status.Die: StatusDie();
                    break;
                case Status.Idle: StatusIdle();
                    break;
                case Status.Move:
                    break;
                case Status.Atack:
                    break;
            }

        }
        void StatusDie()
        {
            if (nextAction!=Respawn)
            {
                timeForAction = loc.now.AddMilliseconds(respTime);
                nextAction = Respawn;
            }
        }
        void Respawn()
        {
            status = Status.Idle;
            //need add new coordinates for respawn
        }
        void StatusIdle()
        {
            if (behav.Contains(Behavior.Agressive))
            {
                if (Target==null && whoAround.Count!=0)
                {
                    foreach (var anyone in whoAround)
                    {

                    }
                } 
            }
        }
        internal override void Do()
        {

        }
        internal override void Atack()
        {
            
        }
        internal override void Step()
        {
            
        }
    }
    class Player : Person
    {
        public Player(int id, Location loc) : base(id, loc)
        {
            hp = 1000;
            atack = 2;
            aspd = 2;
            mspd = 2;
            arng = 1.5F;
            vrng = 5;
            pos.StartPos(0, 0);
        }
        internal override void Thinking()
        {
            base.Thinking();
        }
        internal override void Do()
        {

        }
        internal override void Atack()
        {
            
        }
        internal override void Step()
        {
            
        }
    }

    class Location
    {
        internal LinkedList<Person> persons { get; private set; }
        internal DateTime now;
        bool Enabled;
        public Location()
        {
            Enabled = true;
            persons = new LinkedList<Person>();
            for (int i = 0; i < 2; i++)
            {
                persons.AddLast(new Mob(i, this, (i % 2 == 0 ? "Agressive" : "")));
            }
        }
        public void LocationLive()
        {
            while (Enabled)
            {
                now = DateTime.Now;
                foreach (Person p in persons)
                {
                    //if (now <= p.timeForAction) p.Do();
                    p.Thinking();
                }
            }
        }
    }
    struct coord
    {
        int x;
        int y;
        public void StartPos(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public static int operator - (coord p1, coord p2)
        {
            int r = (p1.x - p2.x) * (p1.x - p2.x) + (p1.y - p2.y) * (p1.y - p2.y);
            return r;
        }
    }

}
