using System;
using System.Collections.Generic;
using System.Timers;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game
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
    class Location
    {
        internal LinkedList<Person> persons { get; private set; }
        Timer locTime;
        internal List<Skill> skillOnLoc;          //skills used on map
        bool Enabled;
        public Location()
        {

            Enabled = false;
            persons = new LinkedList<Person>();
            skillOnLoc = new List<Skill>();
            locTime = new Timer();
            locTime.Elapsed += Tick;
            for (int i = 0; i < 10; i++)
            {
                string mobInfo = String.Format("{0} {0},{0},{0},{0},{0},{0} ", i+10);
                mobInfo += (i % 2 == 0 ? "Agressive" : "");
                //string mobInfo = (i % 2 == 0 ? "Agressive" : "");
                persons.AddLast(new Mob(i, this, mobInfo));
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
            Console.WriteLine("Location Status: {0}", Enabled.ToString());
        }
        private void Tick (object sender, ElapsedEventArgs e)
        {
            foreach (Person p in persons)
            {
                
                p.Thinking();
                p.timeDelay--;
                if (p.timeDelay <= 0) p.Do();
            }
            for (int i = 0; i < skillOnLoc.Count; i++)
            {
                skillOnLoc[i].timeDelay--;
                if (skillOnLoc[i].timeDelay < 0)
                {
                    if (skillOnLoc[i] is PersonalSkill)
                    {
                        ((PersonalSkill)skillOnLoc[i]).targetCast.TakeDamage(skillOnLoc[i]);
                        skillOnLoc[i].SkillEffect();
                        //damOnLoc[i] = null;
                    }
                }
            }
            skillOnLoc.RemoveAll((skill) => !skill.enabled);
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
