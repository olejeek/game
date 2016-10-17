using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game
{
    abstract class Damage
    {
        internal int damage { get; private set; }
        internal int timeDelay;
        internal Person from { get; private set; }

        public Damage(Person from, int damage, int timeDelay)
        {
            this.from = from;
            this.damage = damage;
            this.timeDelay = timeDelay;
        }
    }

    class PersonalDamage:Damage
    {
        internal Person to { get; private set; }
        public PersonalDamage(Person from, Person to, int damage, int timeDelay)
            :base(from, damage, timeDelay)
        {
            this.to = to;
        }
    }
    class AreaDamage:Damage
    {
        internal coord area;
        public AreaDamage(Person from, int damage, coord area, int timeDelay) 
            : base(from, damage, timeDelay)
        {
            this.area = area;
        }
    }
}
