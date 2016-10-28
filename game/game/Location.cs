using System;
using System.Collections.Generic;
using System.Timers;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game
{
    class Location
    {
        //-----map size------
        internal int mapSizeX = 5;
        internal int mapSizeY = 5;
        //-----------------

        //internal LinkedList<Person> persons { get; private set; }
        Timer locTime;
        internal List<Skill> skillOnLoc;          //skills used on map
        //--------------------------------------
        internal List<Person>[,] personsOnMap;
        //--------------------------------------
        bool Enabled;
        public Location()
        {

            Enabled = false;
            //persons = new LinkedList<Person>();
            //--------------------------------------
            personsOnMap = new List<Person>[mapSizeX, mapSizeY];
            for (int x = 0; x < mapSizeX; x++)
            {
                for (int y = 0; y < mapSizeY; y++)
                {
                    personsOnMap[x, y] = new List<Person>();
                }
            }
            Mob mob;
            for (int i = 0; i < 10; i++)
            {
                string mobInfo = String.Format("{0} {0},{0},{0},{0},{0},{0} ", i + 10);
                mobInfo += (i % 2 == 0 ? "Agressive" : "");
                mob = new Mob(i, this, mobInfo);
                personsOnMap[mob.pos.x, mob.pos.y].Add(mob);
            }
            //--------------------------------------
            skillOnLoc = new List<Skill>();
            locTime = new Timer();
            locTime.Elapsed += Tick;
            /*
            for (int i = 0; i < 2; i++)
            {
                string mobInfo = String.Format("{0} {0},{0},{0},{0},{0},{0} ", i+10);
                mobInfo += (i % 2 == 0 ? "Agressive" : "");
                //string mobInfo = (i % 2 == 0 ? "Agressive" : "");
                persons.AddLast(new Mob(i, this, mobInfo));
            }
            */
        }
        internal void Start()
        {
            locTime.AutoReset = true;
            locTime.Interval = 1000 / 60;
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
            Console.WriteLine("Location Status: {0}", Enabled.ToString());
        }

        internal void ChangeCoord(Person who, coord vector)
        {
            coord nCoord = who.pos + vector;
            nCoord.x = (nCoord.x <= 0) ? 0 : nCoord.x;
            nCoord.x = (nCoord.x >= mapSizeX) ? mapSizeX - 1 : nCoord.x;
            nCoord.y = (nCoord.y <= 0) ? 0 : nCoord.y;
            nCoord.y = (nCoord.y >= mapSizeY) ? mapSizeY - 1 : nCoord.y;
            personsOnMap[who.pos.x, who.pos.y].Remove(who);
            who.pos = nCoord;
            personsOnMap[who.pos.x, who.pos.y].Add(who);
        }

        private void Tick(object sender, ElapsedEventArgs e)
        {
            foreach (List<Person> list in personsOnMap)
            {
                if (list.Count != 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        list[i].Thinking();
                        list[i].timeDelay--;
                        if (list[i].timeDelay <= 0) list[i].Do();
                    }
                }
            }
            /*
            foreach (Person p in persons)
            {

                p.Thinking();
                p.timeDelay--;
                if (p.timeDelay <= 0) p.Do();
            }
            */
            for (int i = 0; i < skillOnLoc.Count; i++)
            {
                skillOnLoc[i].timeDelay--;
                if (skillOnLoc[i].timeDelay < 0)
                {
                    if (skillOnLoc[i] is PersonalSkill) skillOnLoc[i].SkillEffect();
                }
            }
            skillOnLoc.RemoveAll((skill) => !skill.enabled);
        }

    }
    struct coord
    {
        internal int x;
        internal int y;
        public coord(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public static int Range(coord p1, coord p2)
        {
            int r = (p1.x - p2.x) * (p1.x - p2.x) + (p1.y - p2.y) * (p1.y - p2.y);
            return r;
        }
        public static coord operator -(coord p1, coord p2)
        {
            return new coord(p2.x - p1.x, p2.y - p2.y);
        }
        public static coord operator +(coord p1, coord p2)
        {
            return new coord(p1.x + p2.x, p1.y + p2.y);
        }
        public static bool operator ==(coord p1, coord p2)
        {
            return (p1.x == p2.x && p1.y == p2.y);
        }
        public static bool operator !=(coord p1, coord p2)
        {
            return !(p1 == p2);
        }
        public override int GetHashCode()
        {
            return x * 1000 + y;
        }
        public override bool Equals(object obj)
        {
            if (obj.GetType() != this.GetType()) return false;
            else
            {
                coord objCoord = (coord)obj;
                return (this.x == objCoord.x && this.y == objCoord.y);
            }

        }
        public void newCoord(coord vector)
        {
            x += vector.x;
            y += vector.y;
        }
        public int direction(coord p1)
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
            return "(" + x + ";" + y + ")";
        }
    }
}
