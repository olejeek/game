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
    abstract class SkillCreator
    {
        internal int CurLevel;
        internal int MaxLevel;
        internal int maxUseLevel;
        internal int minUseLevel;
        protected int castLevel;
        internal int castTime;
        internal int addCastTime;
        internal bool EnabledToUse;
        private int castrange;
        public int castRange
        {
            get { return castrange * castrange; }
            protected set { castrange = value; } 
        }
        internal bool bufSkill;
        protected Person caster;

        internal abstract bool EndCast();
        internal abstract int StartCast(int castLevel, object SkillTarget);
        internal abstract bool SkillUpdate();
        public SkillCreator(Person caster, int currentLevel)
        {
            this.caster = caster;
            this.CurLevel = currentLevel;
        }
    }

    abstract class PersonalSkill : Skill
    {
        internal Person targetCast;
        public PersonalSkill(Person whoCast, Person targetCast, int level):base(whoCast, level)
        {
            this.targetCast = targetCast;
        }
        
    }
    abstract class AreaSkill : Skill
    {
        protected Coord targetCast;
        public AreaSkill(Person whoCast, Coord targetCast, int level):base(whoCast, level)
        {
            this.targetCast = targetCast;
        }
    }

    class Respawn : PersonalSkill
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
                targetCast.mp = targetCast.maxMP;
                targetCast.loc.ChangePosition(targetCast, 
                    new Coord(targetCast.r.Next(targetCast.loc.mapSizeX+1), targetCast.r.Next(targetCast.loc.mapSizeY+1), targetCast.loc));
                targetCast.status = Person.Status.Idle;
            }
            else if (level == 0) targetCast.ChangeHP(targetCast, 1);
            else
                targetCast.ChangeHP(whoCast, (int)(targetCast.maxHP * (0.1 * level + 0.1)));
                targetCast.status = Person.Status.Idle;
            Console.WriteLine("------------------Mob #{0} respawned in {1}. HP:{2}/{3}",
                    targetCast.locId, targetCast.pos, targetCast.hp, targetCast.maxHP);
            enabled = false;
        }
    }
    class RespawnCreator : SkillCreator
    {
        Person target;
        private RespawnCreator(Person caster, int currentLevel):base(caster, currentLevel)
        {
            MaxLevel = 5;
            bufSkill = true;
            if (currentLevel > 0) EnabledToUse = true;
            minUseLevel = 1;
            maxUseLevel = currentLevel;
            if (caster is Mob) castTime = ((Mob)caster).respTime;
            else
            {
                castTime = Program.ups/2;
                addCastTime = Program.ups;
            }
        }
        internal override int StartCast(int castLevel, object SkillTarget)
        {
            target = SkillTarget as Person;
            this.castLevel = castLevel;
            Console.WriteLine("Mob #{0} start cast to mob #{1} Respawn lvl.{2}", caster.locId, target.locId, castLevel);
            if (caster is Mob) return ((Mob)caster).respTime;
            return (int)((castTime + addCastTime * castLevel) / caster.cspd);
        }
        internal override bool EndCast()
        {
            if (!EnabledToUse || target==null) return false;
            caster.loc.skillOnLoc.Add(new Respawn(caster, target, castLevel));
            target = null;
            return true;
        }
        internal override bool SkillUpdate()
        {
            if (CurLevel == MaxLevel) return false;
            CurLevel++;
            if (CurLevel > 0) EnabledToUse = true;
            return true;
        }
        public static RespawnCreator AddSkill(Person caster, int currentLevel)
        {
            return new RespawnCreator(caster, currentLevel);
        }
    }

    class Move : AreaSkill
    {
        internal Move(Person whoCast, Coord newCoord) : base(whoCast, newCoord, 1)
        {
            enabled = true;
            timeDelay = 0;// (int)(60 / whoCast.mspd);
        }
        internal override void SkillEffect()
        {
            Coord delta = new Coord(0, 0, whoCast.loc);
            switch (whoCast.Direction)
            {
                case WorldSide.N: delta.ChangeCoord(0, 1); break;
                case WorldSide.NE: delta.ChangeCoord(1, 1); break;
                case WorldSide.E: delta.ChangeCoord(1, 0); break;
                case WorldSide.SE: delta.ChangeCoord(1, -1); break;
                case WorldSide.S: delta.ChangeCoord(0, -1); break;
                case WorldSide.SW: delta.ChangeCoord(-1, -1); break;
                case WorldSide.W: delta.ChangeCoord(-1, 0); break;
                case WorldSide.NW: delta.ChangeCoord(-1, 1); break;
                default: break;
            }
            whoCast.loc.ChangePosition(whoCast,delta);
            Console.WriteLine("Mob #{0} come to coord:{1}", whoCast.locId, whoCast.pos);
            enabled = false;
        }
    }
    class MoveCreator : SkillCreator
    {
        Coord target;
        private MoveCreator(Person caster, int currentLevel = 1) : base(caster, 1)
        {
            MaxLevel = 1;
            bufSkill = false;
            if (currentLevel > 0) EnabledToUse = true;
            minUseLevel = currentLevel;
            maxUseLevel = currentLevel;

        }
        internal override int StartCast(int castLevel, object SkillTarget)
        {
            target = SkillTarget as Coord;
            caster.Direction = (WorldSide)caster.pos.direction(target);
            this.castLevel = castLevel;
            Console.WriteLine("Mob #{0} go to {1}", caster.locId, caster.Direction);
            return (int)(Program.ups / caster.mspd);
            //return 0;
        }
        internal override bool EndCast()
        {
            if (!EnabledToUse || target == null) return false;
            caster.loc.skillOnLoc.Add(new Move(caster, target));
            return true;
        }
        internal override bool SkillUpdate()
        {
            return false;
        }
        public static MoveCreator AddSkill(Person caster, int currentLevel)
        {
            return new MoveCreator(caster, currentLevel);
        }
    }

    class PhisAtack : PersonalSkill
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
    class PhisAtackCreator : SkillCreator
    {
        Person target;
        private PhisAtackCreator(Person caster, int currentLevel=1):base(caster,1)
        {
            MaxLevel = 1;
            bufSkill = false;
            if (currentLevel > 0) EnabledToUse = true;
            minUseLevel = currentLevel;
            maxUseLevel = currentLevel;
        }
        internal override int StartCast(int castLevel, object SkillTarget)
        {
            target = SkillTarget as Person;
            this.castLevel = castLevel;
            Console.WriteLine("Mob #{0} atack mob #{1}", caster.locId, target.locId);
            return (int)(2*Program.ups - caster.aspd);
        }
        internal override bool EndCast()
        {
            if (!EnabledToUse || target == null) return false;
            caster.loc.skillOnLoc.Add(new PhisAtack(caster, target));
            target = null;
            return true;
        }
        internal override bool SkillUpdate()
        {
            return false;
        }
        public static PhisAtackCreator AddSkill(Person caster, int currentLevel)
        {
            return new PhisAtackCreator(caster, currentLevel);
        }
    }

    class FireBolt : PersonalSkill
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
            targetCast.ChangeHP(whoCast, -damage);
            Console.WriteLine("FireBolt damaged mob #{0} to {1} hp.",
                targetCast.locId, damage);
            if (times == 0) enabled = false;
            
        }
    }
    class FireBoltCreator : SkillCreator
    {
        Person target;
        private FireBoltCreator(Person caster, int currentLevel):base(caster, currentLevel)
        {
            MaxLevel = 10;
            castRange = 5;
            bufSkill = false;
            addCastTime=30;
            if (currentLevel > 0) EnabledToUse = true;
            minUseLevel = 1;
            maxUseLevel = currentLevel;
            castTime = Program.ups;

        }
        internal override int StartCast(int castLevel, object SkillTarget)
        {
            target = SkillTarget as Person;
            this.castLevel = castLevel;
            Console.WriteLine("Mob #{0} start cast to mob #{1} FireBolt lvl.{2}", caster.locId, target.locId, castLevel);
            return (int)((castTime + addCastTime * castLevel) * caster.cspd);
        }
        internal override bool EndCast()
        {
            if (!EnabledToUse || target == null) return false;
            caster.loc.skillOnLoc.Add(new FireBolt(caster, target, castLevel));
            Console.WriteLine("{0} successfully end cast FireBolt lvl {1}", caster.locId, castLevel);
            target = null;
            return true;
        }
        internal override bool SkillUpdate()
        {
            if (CurLevel == MaxLevel) return false;
            CurLevel++;
            if (CurLevel > 0) EnabledToUse = true;
            return true;
        }
        public static FireBoltCreator AddSkill(Person caster, int currentLevel)
        {
            return new FireBoltCreator(caster, currentLevel);
        }
    }
}
