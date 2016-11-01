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
        public Coord(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public static int Range(Coord p1, Coord p2)
        {
            int r = (p1.x - p2.x) * (p1.x - p2.x) + (p1.y - p2.y) * (p1.y - p2.y);
            return r;
        }
        public static Coord operator -(Coord p1, Coord p2)
        {
            return new Coord(p2.x - p1.x, p2.y - p2.y);
        }
        public static Coord operator +(Coord p1, Coord p2)
        {
            return new Coord(p1.x + p2.x, p1.y + p2.y);
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
            return x * 1000 + y;
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
        public void newCoord(Coord vector)
        {
            x += vector.x;
            y += vector.y;
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
    }
}
