using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.OleDb;

namespace game
{
    class Program
    {
        public delegate SkillCreator Skiller(Person caster, int currentLevel);
        public static Dictionary<string, Skiller> SkillList;
        static void Main(string[] args)
        {
        //    string connectionString =
        //        @"Provider=Microsoft.Jet.OLEDB.4.0; Data Source=Ragnarok.mdb";
        //    using (OleDbConnection connection = new OleDbConnection(connectionString))
        //    {
        //        connection.Open();
        //        Console.WriteLine("ServerVersion: {0} \nDataSource: {1}",
        //        connection.ServerVersion, connection.DataSource);
        //        string command = "SELECT * FROM mobs";
        //        OleDbCommand cmd = new OleDbCommand(command, connection);
        //        OleDbDataReader reader = cmd.ExecuteReader();
        //        while (reader.Read())
        //        {
        //            Console.WriteLine("Id: {0, -4} Name: {1, -20} Lvl: {2, -3}", reader[0],
        //                reader[1], reader[2]);
        //        }
        //        connection.Close();
        //    }
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
