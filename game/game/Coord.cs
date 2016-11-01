using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game
{
    class Coord
    {
        internal int x;
        internal int y;
        Location loc;
        public Coord(int x, int y, Location parent)
        {
            this.x = x;
            this.y = y;
            this.loc = parent;
        }
        public static int Range(Coord p1, Coord p2)
        {
            int r = (p1.x - p2.x) * (p1.x - p2.x) + (p1.y - p2.y) * (p1.y - p2.y);
            return r;
        }
        public static Coord operator -(Coord p1, Coord p2)
        {
            Coord rez = new Coord(p1.x - p2.x, p1.y - p2.y, p1.loc);
            rez.MinMaxAvailableCoord();
            return rez;
        }
        public static Coord operator -(Coord p1, int range)
        {
            Coord rez = new Coord(p1.x - range, p1.y - range, p1.loc);
            rez.MinMaxAvailableCoord();
            return rez;
        }
        public static Coord operator +(Coord p1, Coord p2)
        {
            Coord rez = new Coord(p1.x + p2.x, p1.y + p2.y, p1.loc);
            rez.MinMaxAvailableCoord();
            return rez;
        }
        public static Coord operator +(Coord p1, int range)
        {
            Coord rez = new Coord(p1.x + range, p1.y + range, p1.loc);
            rez.MinMaxAvailableCoord();
            return rez;
        }
        public static bool operator ==(Coord p1, Coord p2)
        {
            return p1.Equals(p2);
        }
        public static bool operator !=(Coord p1, Coord p2)
        {
            return !(p1 == p2);
        }
        public override int GetHashCode()
        {
            return loc.Id*1000000 +x * 1000 + y;
        }
        public override bool Equals(object obj)
        {
            Coord objCoord = obj as Coord;
            if (obj == null) return false;
            else
            {
                return (this.x == objCoord.x && this.y == objCoord.y);
            }

        }
        public int direction(Coord p1)
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
        public void ChangeCoord(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public void MinMaxAvailableCoord()
        {
            x = x < 0 ? 0 : x;
            y = y < 0 ? 0 : y;
            x = x > loc.mapSizeX ? loc.mapSizeX : x;
            y = y > loc.mapSizeY ? loc.mapSizeY : y;
        }
        public List<Person> NearestPersons(int radius)
        {
            List<Person> allNearestPerson = new List<Person>();
            Coord min = this - radius;
            Coord max = this + radius;
            for (int X = min.x; X <= max.x; X++)
            {
                for (int Y = min.y; Y <= max.y; Y++)
                {
                    allNearestPerson.AddRange((IEnumerable<Person>)loc.personsOnMap[X, Y]);
                }
            }
            return allNearestPerson;
        }
    }
}
