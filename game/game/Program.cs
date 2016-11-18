using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.OleDb;
using game.Net;

namespace game
{
    class Program
    {
        public const int ups = 60;      //updates per second

        public delegate SkillCreator Skiller(Person caster, int currentLevel);
        public static Dictionary<string, Skiller> SkillList;
        static void Main(string[] args)
        {
            //string connectionString =
            //    @"Provider=Microsoft.Jet.OLEDB.4.0; Data Source=Ragnarok.mdb";
            //using (OleDbConnection connection = new OleDbConnection(connectionString))
            //{
            //    connection.Open();
            //    Console.WriteLine("ServerVersion: {0} \nDataSource: {1}",
            //    connection.ServerVersion, connection.DataSource);
            //    string command = "SELECT * FROM users";
            //    OleDbCommand cmd = new OleDbCommand(command, connection);
            //    OleDbDataReader reader = cmd.ExecuteReader();
            //    while (reader.Read())
            //    {
            //        Console.WriteLine("Id: {0, -4} Name: {1, -10} Pswd: {2, -10} Ban: {3}", reader[0],
            //            reader[1], reader[2], reader[3]);
            //    }
            //    connection.Close();
            //}
            //SkillList = new Dictionary<string, Skiller>();
            //SkillListFiller();
            //Location loc1 = new Location(0);
            //loc1.Start();
            Network netServer = Network.CreateServer();
            netServer.Start();
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
