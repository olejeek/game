using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game
{
    abstract class Person
    {
        //------------ Other enums ---------------------
        internal enum Status { Die, Idle, Move, Atack }
        internal enum WorldSide { N, NE, E, SE, S, SW, W, NW }
        internal enum Stats { Str, Agi, Vit, Int, Dex, Luc}
        //----------------------------------------------
        //------------ Main person stats ---------------
        int[] stats;
        //----------------------------------------------
        //------------ Second person stats -------------
        protected int hp;       //hit points
        protected int atack;    //atack
        protected float aspd;   //atack speed
        protected float mspd;   //move speed
        protected float arng;   //atack range
        protected float vrng;   //view range
        //----------------------------------------------
        //------------ Location interplay vars ---------
        internal int locId;    //location person Id
        protected Location loc; //location, where person are situated
        internal WorldSide Direction;       //direction, where person see
        internal coord pos;    //position on the location
        protected coord nextPos;    //coordinates to move
        protected Person Target;            //target for atack
        internal int timeDelay;    //time for person action
        protected Dictionary<Person, int> whoAround;  //all persons, which person view
        internal Status status;    //person status
        protected Action nextAction;    //what person must do

        public Person(int id, Location loc)                     //constructor of person
        {
            stats = new int[6];
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
            if (Target != null && Target.status == Status.Die) Target = null;
        }
        void LookAround()  //search persons around person and square range
        {
            whoAround.Clear();
            foreach (Person p in loc.persons)
            {
                int range2 = coord.Range(pos, p.pos);
                if (range2 <= vrng * vrng && p != this && p.status!= Status.Die) whoAround.Add(p, range2);
            }
        }
        void Way()
        {

        }
        abstract internal void Do();        //Person action
        abstract internal void Step();      //person take step
        abstract internal void Atack();     //person take phisical damage

        internal void TakeDamage(Damage dam)
        {
            hp -= dam.damage;
            Console.WriteLine("Mob #{0} damaged mob #{1} to {2} hp", dam.from.locId, locId, dam.damage);
        }
    }

    class Mob : Person
    {
        enum Behavior { Agressive, FeelCast, Helpful, ChangeTarget, Looter };
        Random r;
        List<Behavior> behav;   //mob behaviors
        int respTime;           //mob respawn time
        public Mob(int id, Location loc, string behav) : base(id, loc)
        {
            r = new Random(id);
            this.behav = new List<Behavior>();
            if (behav != "")
            {
                string[] behavs = behav.Split(',');
                foreach (var str in behavs)
                    this.behav.Add((Behavior)Enum.Parse(typeof(Behavior), str, true));
            }
            respTime = 600;
            status = Status.Idle;
            hp = 100;
            atack = 10;
            aspd = 5;
            mspd = 2;
            arng = 1.5F;
            vrng = 5;
            pos = new coord(r.Next(5), r.Next(5));
            Console.WriteLine("New mob add with #{0} in {1}", locId, pos);

        }
        internal override void Thinking()
        {
            base.Thinking();
            //Console.WriteLine("Mob #{0} status: {1}", locId, status);
            switch (status)
            {
                case Status.Die:
                    StatusDie();
                    break;
                case Status.Idle:
                    StatusIdle();
                    break;
                case Status.Move:
                    StatusMove();
                    break;
                case Status.Atack:
                    StatusAtack();
                    break;
            }

        }
        void StatusDie()
        {
            if (nextAction != Respawn)
            {
                timeDelay = respTime;
                nextAction = Respawn;
                Target = null;
                Console.WriteLine("Mob #{0} die!", locId);
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
                if (Target != null)
                {
                    if (whoAround[Target] > arng * arng)                 //if range to target more than atack range
                    {
                        status = Status.Move;
                        timeDelay = (int)(60 / mspd);
                        Direction = (WorldSide)pos.direction(Target.pos);
                        nextAction = Step;
                    }
                    else                                        //if range to target less then atack range
                    {
                        status = Status.Atack;
                        timeDelay = (int)(60 / aspd);
                        Direction = (WorldSide)pos.direction(Target.pos);
                        nextAction = Atack;
                    }
                }
            }
            //need add some trip in view range
        }
        void StatusMove()
        {
            if (timeDelay > 0) return;
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
                if (Target!=null)
                {
                    if (whoAround[Target] > arng * arng)                 //if range to target more than atack range
                    {
                        status = Status.Move;
                        timeDelay = (int)(60 / mspd);
                        Direction = (WorldSide)pos.direction(Target.pos);
                        nextAction = Step;
                    }
                    else                                        //if range to target less then atack range
                    {
                        status = Status.Atack;
                        timeDelay = (int)(60 / aspd);
                        Direction = (WorldSide)pos.direction(Target.pos);
                        nextAction = Atack;
                    }
                }
                
            }
        }
        void StatusAtack()
        {
            //Console.WriteLine("Target status: {0}, my status: {1}", Target.status, status);
            //if (Target.status == Status.Die)
            if (Target==null)
            {
                Target = null;
                nextAction = null;
                status = Status.Idle;
            }
            else
            {
                if (timeDelay > 0) return;
                else
                {
                    nextAction = Atack;
                    timeDelay = (int)(60 / aspd);
                }
            }
        }
        void Respawn()
        {
            status = Status.Idle;
            nextAction = null;
            hp = 100;
            pos = new coord(r.Next(5), r.Next(5));
            Console.WriteLine("Mob # {0} respawned in {1}", this.locId, pos);
        }

        internal override void Do()
        {
            nextAction?.Invoke();
            //if (nextAction!=null) nextAction();
        }
        internal override void Atack()
        {
            loc.damOnLoc.Add(new PersonalDamage(this, Target, atack, 0));
            //Console.WriteLine("Mob #{0} atack mob #{1}", locId, Target.locId);
        }
        internal override void Step()
        {
            switch (Direction)
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
            Console.WriteLine("Mob #{0} go to coord:{1}", locId, pos);
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
            pos = new coord(0, 0);
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
}
