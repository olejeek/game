using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game
{
    abstract class Skill
    {
        internal Person whoCast;
        internal int damage;
        internal bool enabled;
        internal int timeDelay; 

        public Skill (Person whoCast)
        {
            this.whoCast = whoCast;
        }
        abstract internal void SkillEffect();
    }

    abstract class PersonalSkill:Skill
    {
        internal Person targetCast;
        public PersonalSkill(Person whoCast, Person targetCast):base(whoCast)
        {
            this.targetCast = targetCast;
        }
        
    }
    abstract class AreaSkill:Skill
    {
        protected coord targetCast;
        public AreaSkill(Person whoCast, coord targetCast):base(whoCast)
        {
            this.targetCast = targetCast;
        }
    }

    class Move:PersonalSkill
    {
        internal Move(Person who):base(who, who)
        {
            damage = 0;
            enabled = true;
        }
        internal override void SkillEffect()
        {
            switch (targetCast.Direction)
            {
                case Person.WorldSide.N: targetCast.pos.newCoord(0, 1); break;
                case Person.WorldSide.NE: targetCast.pos.newCoord(1, 1); break;
                case Person.WorldSide.E: targetCast.pos.newCoord(1, 0); break;
                case Person.WorldSide.SE: targetCast.pos.newCoord(1, -1); break;
                case Person.WorldSide.S: targetCast.pos.newCoord(0, -1); break;
                case Person.WorldSide.SW: targetCast.pos.newCoord(-1, -1); break;
                case Person.WorldSide.W: targetCast.pos.newCoord(-1, 0); break;
                case Person.WorldSide.NW: targetCast.pos.newCoord(-1, 1); break;
            }
            Console.WriteLine("Mob #{0} go to coord:{1}", targetCast.locId, targetCast.pos);
            enabled = false;
        }
    }
    class PhisAtack:PersonalSkill
    {
        internal bool critical;
        internal PhisAtack(Person whoCast, Person targetCast):base(whoCast, targetCast)
        {
            enabled = true;
            damage = whoCast.atack;
            critical = whoCast.r.Next(100) >= (100 - whoCast.crit);
            timeDelay = 0;
        }
        internal override void SkillEffect()
        {
            if (whoCast.r.Next(100) > (100 - targetCast.pdodge))
                Console.WriteLine("Mob #{0} perfect dodged the atack mob #{1}",
                    targetCast.locId, whoCast.locId);
            else if (critical)
            {
                targetCast.hp -= damage;
                Console.WriteLine("Mob #{0} inflicted critical damage on mob #{1} to {2} hp",
                    whoCast.locId, targetCast.locId, damage);
            }
            else
            {
                int atackHit = whoCast.hit;

                if (targetCast.r.Next(atackHit) > atackHit - targetCast.flee)
                {
                    int d = damage - targetCast.def;
                    //d = d > 0 ? d : 1;
                    //hp -= d;
                    targetCast.hp -= (d = d > 0 ? d : 1);
                    Console.WriteLine("Mob #{0} damaged the mob #{1} to {2} hp",
                        whoCast.locId, targetCast.locId, d);
                }
                else
                {
                    Console.WriteLine("Mob #{0} missed the mob #{1}",
                        whoCast.locId, targetCast.locId);
                }
            }
            enabled = false;
        }
    }
}
