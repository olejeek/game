using System;
using System.Collections.Generic;
using System.Timers;
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
            loc1.Start();
            Console.Read();
        }
    }

    abstract class Person
    {
        internal enum Status { Die, Idle, Move, Atack}
        internal enum WorldSide { N, NE, E, SE, S, SW, W, NW }  
        internal Status status;    //person status
        protected Action nextAction;    //what person must do
        internal int locId;    //location person Id
        protected int hp;       //hit points
        protected int atack;    //atack
        protected float aspd;   //atack speed
        protected float mspd;   //move speed
        protected float arng;   //atack range
        protected float vrng;   //view range
        protected Location loc; //location, where person are situated
        internal WorldSide Direction;       //direction, where person see
        internal coord pos;    //position on the location
        protected coord nextPos;    //coordinates to move
        protected Person Target;            //target for atack
        internal uint timeDelay;    //time for person action
        protected Dictionary<Person, int> whoAround;  //all persons, which person view
        public Person(int id, Location loc)                     //constructor of person
        {
            whoAround = new Dictionary<Person, int>();
            this.locId = id;
            this.loc = loc;
            Direction = WorldSide.S;
        }
        virtual internal void Thinking()  //person thinking about next action
        {
            LookAround();
            //for (int i=0; i<whoAround.Count; i++)
            //{
            //    Console.WriteLine("{0} see {1} on range2 {2}", this.locId,
            //        whoAround.ElementAt(i).Key.locId,
            //        whoAround.ElementAt(i).Value);
            //}
            /*
            foreach (var p in whoAround)
            {
                Console.WriteLine("{0} see {1} on range2 {2}", this.locId, p.Key.locId, p.Value);
            }
            */
            if (hp <= 0) status = Status.Die;
        }
        void LookAround()  //search persons around person and square range
        {
            whoAround.Clear();
            foreach (Person p in loc.persons)
            {
                int range2 = coord.Range(pos, p.pos);
                if (range2 <= vrng * vrng && p!=this) whoAround.Add(p, range2);
            }
        }
        void Way()      
        {

        }
        abstract internal void Do();        //Person action
        abstract internal void Step();      //person take step
        abstract internal void Atack();     //person take phisical damage
    }

    class Mob : Person
    {
        enum Behavior{Agressive, FeelCast, Helpful, ChangeTarget, Looter  };
        Random r;
        List<Behavior> behav;   //mob behaviors
        uint respTime;           //mob respawn time
        public Mob(int id, Location loc, string behav) : base(id, loc)
        {
            r = new Random(id);
            this.behav = new List<Behavior>();
            if (behav!="")
            {
                string[] behavs = behav.Split(',');
                foreach (var str in behavs)
                    this.behav.Add((Behavior)Enum.Parse(typeof(Behavior), str, true));
            }
            respTime = 5000;
            status = Status.Idle;
            hp = 100;
            atack = 1;
            aspd = 1;
            mspd = 2;
            arng = 2F;
            vrng = 5;
            pos=new coord(r.Next(5), r.Next(5));

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
                case Status.Move: StatusMove();
                    break;
                case Status.Atack: StatusAtack();
                    break;
            }

        }
        void StatusDie()
        {
            if (nextAction!=Respawn)
            {
                timeDelay = respTime;
                nextAction = Respawn;
            }
        }
        void StatusIdle()
        {
            if (behav.Contains(Behavior.Agressive))         //if mob agressive
            {
                if (Target == null && whoAround.Count != 0)     //if mob doesn`t have target
                {
                    float minRange = vrng * vrng + 1;
                    foreach (var anyone in whoAround)       //search target
                    {
                        if (anyone.Value < minRange)
                        {
                            Target = anyone.Key;
                            minRange = anyone.Value;
                        }
                    }
                }
                if (whoAround[Target] > arng)                 //if range to target more than atack range
                {
                    status = Status.Move;
                    timeDelay = (uint)(60 / mspd);
                    Direction = (WorldSide)pos.direction(Target.pos);
                    nextAction = Step;
                }
                else                                        //if range to target less then atack range
                {
                    status = Status.Atack;
                    timeDelay = (uint)(60 / aspd);
                    Direction = (WorldSide)pos.direction(Target.pos);
                    nextAction = Atack;
                }
            }
            //need add some trip in view range
        }
        void StatusMove()
        {
            if (status == Status.Move) return;
            if (behav.Contains(Behavior.Agressive))         //if mob agressive
            {
                if (Target == null && whoAround.Count != 0)     //if mob doesn`t have target
                {
                    float minRange = vrng * vrng + 1;
                    foreach (var anyone in whoAround)       //search target
                    {
                        if (anyone.Value < minRange)
                        {
                            Target = anyone.Key;
                            minRange = anyone.Value;
                        }
                    }
                }
                if (whoAround[Target] > arng)                 //if range to target more than atack range
                {
                    status = Status.Move;
                    timeDelay = (uint)(60 / mspd);
                    Direction = (WorldSide)pos.direction(Target.pos);
                    nextAction = Step;
                }
                else                                        //if range to target less then atack range
                {
                    status = Status.Atack;
                    timeDelay = (uint)(60 / aspd);
                    Direction = (WorldSide)pos.direction(Target.pos);
                    nextAction = Atack;
                }
            }
        }
        void StatusAtack()
        {
            if (Target.status == Status.Die)
            {
                Target = null;
                status = Status.Idle;
            }
            else
            {
                Atack();
            }
        }
        void Respawn()
        {
            status = Status.Idle;
            hp = 100;
            Console.WriteLine("Mob # {0} respawned", this.locId);
            //need add new coordinates for respawn
        }
        
        internal override void Do()
        {

        }
        internal override void Atack()
        {
            Console.WriteLine("Mob #{0} atack mob #{1}", locId, Target.locId);
        }
        internal override void Step()
        {
            switch(Direction)
            {
                case WorldSide.N: pos = pos.newCoord(0, 1); break;
                case WorldSide.NE: pos = pos.newCoord(1, 1); break;
                case WorldSide.E: pos = pos.newCoord(1, 0); break;
                case WorldSide.SE: pos = pos.newCoord(1, -1); break;
                case WorldSide.S: pos = pos.newCoord(0, -1); break;
                case WorldSide.SW: pos = pos.newCoord(-1, -1); break;
                case WorldSide.W: pos = pos.newCoord(-1, 0); break;
                case WorldSide.NW: pos = pos.newCoord(-1, 1); break;
            }
            Console.WriteLine("Mob #{0} have new coord:{1}",locId,pos);
            /*
            if (pos == nextPos || nextPos == null)
            {
                status = Status.Idle;
                nextAction = null;
            }
             */
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
            pos= new coord(0, 0);
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
        Timer locTime;
        bool Enabled;
        public Location()
        {

            Enabled = false;
            persons = new LinkedList<Person>();
            locTime = new Timer();
            locTime.Elapsed += Tick;
            for (int i = 0; i < 2; i++)
            {
                persons.AddLast(new Mob(i, this, (i % 2 == 0 ? "Agressive" : "")));
            }
        }

        internal void Start()
        {
            locTime.AutoReset = true;
            locTime.Interval = 6000 / 60;
            locTime.Enabled = true;
            Enabled = true;
        }
        internal void Stop()
        {
            locTime.Enabled = false;
            Enabled = false;
        }
        internal void Status()
        {
            Console.WriteLine(Enabled.ToString());
        }
        private void Tick (object sender, ElapsedEventArgs e)
        {
            Console.WriteLine("New Frame.");
            foreach (Person p in persons)
            {
                p.timeDelay--;
                if (p.timeDelay<=0) p.Do();
                p.Thinking();
            }
        }

    }
    struct coord
    {
        int x;
        int y;
        public coord(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public static int Range (coord p1, coord p2)
        {
            int r = (p1.x - p2.x) * (p1.x - p2.x) + (p1.y - p2.y) * (p1.y - p2.y);
            return r;
        }
        public static coord operator - (coord p1, coord p2)
        {
            return new coord(p2.x - p1.x, p2.y - p2.y);
        }
        public static bool operator ==(coord p1, coord p2)
        {
            return (p1.x==p2.x && p1.y==p2.y);
        }
        public static bool operator !=(coord p1, coord p2)
        {
            return !(p1==p2);
        }
        public coord newCoord (int dx, int dy)
        {
            return new coord(x + dx, y + dy);
        }
        public int direction (coord p1)
        {
            float deltaX = p1.x - x;
            float deltaY = p1.y - y;
            float tga = deltaY / deltaX;
            if (deltaX >= 0)
            {
                if (tga > 2.41421356 || tga < -2.41421356)
                {
                    if (tga > 0) return 0;
                    else return 4;
                }
                else if (tga < 0.41421356 && tga > -0.41421356) return 2;
                else
                {
                    if (tga > 0) return 1;
                    else return 3;
                }
                
            }
            else
            {
                if (tga > 2.41421356 || tga < -2.41421356)
                {
                    if (tga > 0) return 4;
                    else return 0;
                }
                else if (tga < 0.41421356 && tga > -0.41421356) return 6;
                else
                {
                    if (tga > 0) return 5;
                    else return 7;
                }

            }
        }
        public override string ToString()
        {
            return "("+x+";"+y+")";
        }
    }
}
