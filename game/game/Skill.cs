﻿using System;
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
        internal int level;

        public Skill (Person whoCast, int level)
        {
            this.whoCast = whoCast;
            this.level = level;
        }
        abstract internal void SkillEffect();
    }

    abstract class PersonalSkill:Skill
    {
        internal Person targetCast;
        public PersonalSkill(Person whoCast, Person targetCast, int level):base(whoCast, level)
        {
            this.targetCast = targetCast;
        }
        
    }
    abstract class AreaSkill:Skill
    {
        protected coord targetCast;
        public AreaSkill(Person whoCast, coord targetCast, int level):base(whoCast, level)
        {
            this.targetCast = targetCast;
        }
    }

    class Respawn:PersonalSkill
    {
        internal Respawn(Person whoCast, Person TargetCast, int level)
            :base(whoCast, TargetCast, level)
        {
            enabled = true;
            timeDelay = 0;
            damage = 0;
        }
        internal override void SkillEffect()
        {
            if (targetCast is Mob)
            {
                targetCast.ChangeHP(targetCast, targetCast.maxHP);
                //targetCast.hp = targetCast.maxHP;
                targetCast.mp = targetCast.maxMP;
                targetCast.loc.ChangeCoord(targetCast, new coord(targetCast.r.Next(5), targetCast.r.Next(5)));
                //targetCast.pos = new coord(targetCast.r.Next(5), targetCast.r.Next(5));
                targetCast.status = Person.Status.Idle;
            }
            //else if (level == 0) targetCast.hp = 1;
            else if (level == 0) targetCast.ChangeHP(targetCast, 1);
            else
                //targetCast.hp = (int)(targetCast.maxHP * (0.1 * level + 0.1));
                targetCast.ChangeHP(whoCast, (int)(targetCast.maxHP * (0.1 * level + 0.1)));
                targetCast.status = Person.Status.Idle;
            Console.WriteLine("Mob #{0} respawned in {1}. HP:{2}/{3}",
                    targetCast.locId, targetCast.pos, targetCast.hp, targetCast.maxHP);
            enabled = false;
        }
    }
    class PhisAtack:PersonalSkill
    {
        internal bool critical;
        internal PhisAtack(Person whoCast, Person targetCast):base(whoCast, targetCast, 1)
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
                //targetCast.hp -= damage;
                targetCast.ChangeHP(whoCast, -damage);
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
                    //targetCast.hp -= (d = d > 0 ? d : 1);
                    targetCast.ChangeHP(whoCast, -(d = d > 0 ? d : 1));
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
    class FireBolt:PersonalSkill
    {
        int times;
        internal FireBolt(Person whoCast, Person targetCast, int level)
            :base(whoCast, targetCast,level)
        {
            enabled = true;
            times = level;
            timeDelay = 30;
            damage = whoCast.r.Next(whoCast.minMatk, whoCast.maxMatk);
            Console.WriteLine("Mob #{0} cast FireBolt lvl. {1} to mob #{2}.",
                whoCast.locId, level, targetCast.locId);
        }
        internal override void SkillEffect()
        {
            times--;
            timeDelay = 10;
            //targetCast.hp -= damage;
            targetCast.ChangeHP(whoCast, -damage);
            Console.WriteLine("FireBolt damaged mob #{0} to {1} hp.",
                targetCast.locId, damage);
            if (times == 0) enabled = false;
            
        }
    }
}
