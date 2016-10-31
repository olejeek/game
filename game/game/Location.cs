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
            for (int i = 0; i < 2; i++)
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

        internal void ChangeCoord(Person who, Coord vector)
        {
            Coord nCoord = who.pos + vector;
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
                        list[i].Timed();
                    }
                }
            }
            for (int i = 0; i < skillOnLoc.Count; i++)
            {
                skillOnLoc[i].timeDelay--;
                if (skillOnLoc[i].timeDelay < 0)
                {
                    skillOnLoc[i].SkillEffect();
                }
            }
            skillOnLoc.RemoveAll((skill) => !skill.enabled);
        }

    }
    
}
