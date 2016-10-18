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

    class PhisAtack:PersonalSkill
    {
        internal int hit;
        internal bool critical;
        internal PhisAtack(Person whoCast, Person targetCast, int level):base(whoCast, targetCast, level)
        {
            enabled = true;
            timeDelay = 0;
            critical = whoCast.r.Next(100) >= (100 - whoCast.crit);
            damage = whoCast.atack;
            hit = whoCast.hit;
        }
        internal override void SkillEffect()
        {
            enabled = false;
        }
    }
    class FireBolt:PersonalSkill
    {
        int times;
        internal FireBolt(Person whoCast, Person targetCast, int level):base(whoCast, targetCast,level)
        {
            enabled = true;
            times = level;
            timeDelay = 30;
            damage = whoCast.r.Next(whoCast.minMatk, whoCast.maxMatk);
        }
        internal override void SkillEffect()
        {
            times--;
            timeDelay = 10;
            if (times == 0) enabled = false;
            
        }
    }
}
