﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game
{
    internal enum WorldSide { N, NE, E, SE, S, SW, W, NW }
    abstract class Person
    {
        //------------ Other enums ---------------------
        internal enum Status { Die, Idle, Move, Atack, Cast }
        //----------------------------------------------
        //------------ Main person stats ---------------
        internal enum Stats { Str, Agi, Vit, Int, Dex, Luc }
        internal int[] stats;
        //----------------------------------------------
        //------------ Second person stats -------------
        internal int maxHP;    //max hit points
        public int hp { get; protected set; }       //current hit points
        internal int maxMP;    //max mana points
        internal int mp;       //mana points
        internal float cspd;   //cast speed
        internal float mspd;   //move speed
        internal float aspd;   //atack speed
        internal float arng;    //atack range
        protected int vrng;   //view range
        internal int atack;    //atack
        internal int minMatk;  //min magic atack
        internal int maxMatk;  //max magic atack    
        internal int hit;      //chance of hit
        internal int flee;     //chance of dodge
        internal int crit;     //chance of critical atack
        internal int def;      //phisical defence 
        internal int mdef;     //magical defence
        internal int pdodge;   //chance of perfect dodge
        //----------------------------------------------
        //------------ Location interplay vars ---------
        protected PhisAtackCreator mobAtack;    //base phis atack
        protected RespawnCreator mobRespawn;    //respawn skill
        protected SkillCreator castingSkill;    //var for now casting skill
        protected MoveCreator mobMove;          //person moving skill
        internal Random r;
        internal int Level;     //person level
        internal int exp;       //person exp
        internal uint Id;       //id mob or player
        internal int locId;     //location person Id
        internal Location loc; //location, where person are situated
        internal WorldSide Direction;       //direction, where person see
        internal Coord pos;     //position on the location
        protected Coord nextPos;    //coordinates to move
        protected Person Target;            //target for atack
        internal int timeDelay;    //time for person action
        protected Dictionary<Person, int> whoAround;  //all persons, which person view
        internal Status status;    //person status
        protected Action nextAction;    //what person must do

        public Person(int id, Location loc)                     //constructor of person
        {
            stats = new int[6];
            r = new Random(id);
            whoAround = new Dictionary<Person, int>();
            this.locId = id;
            this.loc = loc;
            Direction = WorldSide.S;
            mobAtack = PhisAtackCreator.AddSkill(this, 1);
            mobRespawn = RespawnCreator.AddSkill(this, 1);
            mobMove = MoveCreator.AddSkill(this, 1);
        }
        internal void Timed()
        {
            Thinking();
            if (status != Status.Idle) timeDelay--;
            if (timeDelay <= 0 && castingSkill != null) Do();
        }
        virtual protected void Thinking()  //person thinking about next action
        {
            if (status == Status.Die)
            {
                if (hp < 0) hp = 0;
                return;
            }
            LookAround();
            if (hp <= 0)
            {
                hp = 0;
                status = Status.Die;
                Console.WriteLine("Person #{0} die!", locId);
                castingSkill = mobRespawn;
                timeDelay = mobRespawn.StartCast(0, this);
                return;
            }
            if (Target != null && (Target.status == Status.Die || !whoAround.ContainsKey(Target))) Target = null;
        }
        void LookAround()  //search persons around person and square range
        {
            whoAround.Clear();
            foreach (Person p in pos.NearestPersons(vrng))
            {
                if (p != this && p.status != Status.Die)
                    whoAround.Add(p, Coord.Range(pos, p.pos));
            }
        }
        abstract internal void Do();        //Person action
        internal void ChangeHP (Person who, int dHP)
        {
            hp += dHP;
        }
    }

    class Mob : Person
    {
        enum Behavior { Agressive, FeelCast, Helpful, ChangeTarget, Looter };
        List<Behavior> behav;   //mob behaviors
        List<SkillCreator> mobSkills;
        public int respTime { get; private set; }           //mob respawn time
        public Mob(int id, Location loc, string mobInfo) : base(id, loc)
        {
            Level = 1;
            exp = 10;
            this.behav = new List<Behavior>();
            mobSkills = new List<SkillCreator>();
            //mobSkills.Add(Program.SkillList["firebolt"](this, 5));
            string[] infos = mobInfo.Split(' ');
            Id = Convert.ToUInt32(infos[0]);
            string[] st = infos[1].Split(',');
            for (int i=0;i<6;i++)
            {
                stats[i] = Convert.ToInt32(st[i]);
            }
            if (infos[2] != "")
            {
                string[] behavs = infos[2].Split(',');
                foreach (var str in behavs)
                    this.behav.Add((Behavior)Enum.Parse(typeof(Behavior), str, true));
            }
            maxHP =Level * 50 + 10 * stats[(int)Stats.Vit] + stats[(int)Stats.Str]+10;
            hp = maxHP;
            maxMP = 10 * stats[(int)Stats.Int] + stats[(int)Stats.Dex];
            mp = maxMP;
            cspd = 1 + (stats[(int)Stats.Int] / 500 + stats[(int)Stats.Dex] / 200);
            aspd = stats[(int)Stats.Agi] + stats[(int)Stats.Dex] / 5;
            /*
             * NEED ADD WEAPON coefficient (aspd = aspd*weap_coef*lh_coef)
             * lh_coef - left hand coefficient 
             * where weap_coef = 0.9 for dagger
             * weap_coef = 0.8 for sword
             * weap_coef = 0.7 for axe
             * weap_coef = 0.6 for spear and two-handed weapon
             */
            mspd = 2;
            vrng = 5;
            arng = 1.5F;
            atack = (arng < 2.5F)? 
                (stats[(int)Person.Stats.Str] +
                (stats[(int)Person.Stats.Str]/10)*(stats[(int)Person.Stats.Str] / 10) +
                stats[(int)Person.Stats.Dex] / 5 + stats[(int)Person.Stats.Luc] / 5)
                :
                (stats[(int)Person.Stats.Dex] +
                (stats[(int)Person.Stats.Dex] / 10) * (stats[(int)Person.Stats.Dex] / 10) +
                stats[(int)Person.Stats.Str] / 5 + stats[(int)Person.Stats.Luc] / 5);
            minMatk = stats[(int)Person.Stats.Int] + (stats[(int)Person.Stats.Int]/7)*(stats[(int)Person.Stats.Int] / 7);
            maxMatk = stats[(int)Person.Stats.Int] + (stats[(int)Person.Stats.Int] / 5) * (stats[(int)Person.Stats.Int] / 5);
            hit = Level + stats[(int)Person.Stats.Dex];
            flee = Level + stats[(int)Person.Stats.Agi] + stats[(int)Person.Stats.Dex] / 10;
            crit = stats[(int)Person.Stats.Luc] / 3 + 1;
            def = stats[(int)Person.Stats.Vit] / 3;
            mdef = stats[(int)Person.Stats.Int] / 3;
            pdodge = stats[(int)Person.Stats.Luc] / 10 + 1;
            respTime = 600;
            status = Status.Idle;
            pos = new Coord(r.Next(loc.mapSizeX+1), r.Next(loc.mapSizeY+1), loc);
            Console.WriteLine("New mob add with #{0} in {1}", locId, pos);

        }

        protected override void Thinking()
        {
            base.Thinking();
            switch (status)
            {
                case Status.Idle:
                    StatusIdle();
                    break;
                case Status.Move:
                    StatusMove();
                    break;
                case Status.Cast:
                    StatusCast();
                    break;
                case Status.Atack:
                    StatusAtack();
                    break;
            }

        }
        void StatusIdle()
        {
            if (behav.Contains(Behavior.Agressive))         //if mob agressive
            {
                if (Target == null && whoAround.Count != 0)     //if mob doesn`t have target
                {
                    float minRange = 1000;
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
                        if (mobSkills.Count!=0)
                        {
                            castingSkill = mobSkills[r.Next(mobSkills.Count - 1)];
                            if (castingSkill.castRange >= whoAround[Target])
                            {
                                status = Status.Cast;
                                castingSkill = mobSkills[r.Next(mobSkills.Count - 1)];
                                if (castingSkill.bufSkill) timeDelay = castingSkill.StartCast(castingSkill.CurLevel, this);
                                else timeDelay = castingSkill.StartCast(castingSkill.CurLevel, Target);
                                Console.WriteLine("LOL!");
                                return;
                            }
                            else castingSkill = null;
                        }
                        status = Status.Move;
                        timeDelay = mobMove.StartCast(1, Target.pos);
                        castingSkill = mobMove;
                    }
                    else                                        //if range to target less then atack range
                    {
                        status = Status.Atack;
                        timeDelay = mobAtack.StartCast(1, Target);
                        castingSkill = mobAtack;
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
                        if (mobSkills.Count != 0)
                        {
                            castingSkill = mobSkills[r.Next(mobSkills.Count - 1)];
                            if (castingSkill.castRange >= whoAround[Target])
                            {
                                status = Status.Cast;
                                castingSkill = mobSkills[r.Next(mobSkills.Count - 1)];
                                if (castingSkill.bufSkill) timeDelay = castingSkill.StartCast(castingSkill.CurLevel, this);
                                else timeDelay = castingSkill.StartCast(castingSkill.CurLevel, Target);
                                return;
                            }
                            else castingSkill = null;
                        }
                        status = Status.Move;
                        timeDelay = mobMove.StartCast(1, Target.pos);
                        castingSkill = mobMove;
                    }
                    else                                        //if range to target less then atack range
                    {
                        status = Status.Atack;
                        timeDelay = mobAtack.StartCast(1, Target);
                        castingSkill = mobAtack;
                    }
                }
                
            }
        }
        void StatusCast()
        {
            if (Target == null)
            {
                nextAction = null;
                status = Status.Idle;
            }
            else
            {
                if (timeDelay > 0) return;
                else
                {
                    if (castingSkill != null) castingSkill.EndCast();
                    if (Target != null)
                    {
                        if (whoAround[Target] > arng * arng)                 //if range to target more than atack range
                        {
                            status = Status.Move;
                            //timeDelay = (int)(60 / mspd);
                            //Direction = (WorldSide)pos.direction(Target.pos);
                            //nextAction = Step;
                            timeDelay = mobMove.StartCast(1, Target.pos);
                            castingSkill = mobMove;
                        }
                        else                                        //if range to target less then atack range
                        {
                            status = Status.Atack;
                            //timeDelay = (int)(120 - aspd);
                            //Direction = (WorldSide)pos.direction(Target.pos);
                            //nextAction = Atack;
                            timeDelay = mobAtack.StartCast(1, Target);
                            castingSkill = mobAtack;
                        }
                    }
                }
            }
        }
        void StatusAtack()
        {
            if (Target==null)
            {
                nextAction = null;
                status = Status.Idle;
            }
            else
            {
                if (timeDelay > 0) return;
                else
                {
                    status = Status.Atack;
                    timeDelay = mobAtack.StartCast(1, Target);
                    castingSkill = mobAtack;
                }
            }
        }
        void Respawn()
        {
            loc.skillOnLoc.Add(new Respawn(this, this, 2));
            nextAction = null;
        }

        internal override void Do()
        {
            if (castingSkill != null)
            {
                castingSkill.EndCast();
                castingSkill = null;
            }
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
            pos = new Coord(0, 0, loc);
        }
        protected override void Thinking()
        {
            base.Thinking();
        }
        internal override void Do()
        {

        }
    }
}
