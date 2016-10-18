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

    class PhisAtack:PersonalSkill
    {
        internal int hit;
        internal bool critical;
        internal PhisAtack(Person whoCast, Person targetCast):base(whoCast, targetCast)
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
}
