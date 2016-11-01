using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game
{
    class Program
    {
        public delegate SkillCreator Skiller(Person caster, int currentLevel);
        public static Dictionary<string, Skiller> SkillList;
        static void Main(string[] args)
        {

            SkillList = new Dictionary<string, Skiller>();
            SkillListFiller();
            Location loc1 = new Location(0);
            loc1.Start();
            Console.Read();
        }
        static void SkillListFiller()
        {
            if (SkillList == null) return;
            SkillList.Add("respawn", RespawnCreator.AddSkill);
            SkillList.Add("phisatack", PhisAtackCreator.AddSkill);
            SkillList.Add("firebolt", FireBoltCreator.AddSkill);
        }
    }
    
}
