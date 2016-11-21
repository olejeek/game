using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using game.Net.Protocol;

namespace GameClientV0
{
    public partial class HeroChoose : Form
    {
        List<HeroInfo> hero;
        public HeroChoose(List<string> heroes)
        {
            InitializeComponent();
            hero = new List<HeroInfo>();
            for (int i = 1; i < heroes.Count; i++)
            {
                hero.Add(new HeroInfo(heroes[i]));
                ListViewItem heroListItem = new ListViewItem( hero[i - 1].ToListView());
                heroListView.Items.Add(heroListItem);
            }
        }

        private void HeroChoose_FormClosed(object sender, FormClosedEventArgs e)
        {
            //OnlineUser.SendAndDisconnect("-1\nDisconnect!");
            if (Application.OpenForms.Count<=1) Application.OpenForms[0].Show();
        }

        private void createHero_btn_Click(object sender, EventArgs e)
        {
            OnlineUser.OpenFormToCreateHero();
        }

        struct HeroInfo
        {
            int HeroId;
            string[] parametres;
            
            public HeroInfo(string info)
            {
                string temp = info.Substring(0, info.IndexOf('\t'));
                HeroId = Convert.ToInt32(temp);
                parametres = (info.Substring(info.IndexOf('\t')+1)).Split(new char[] { '\t' }, 
                    StringSplitOptions.RemoveEmptyEntries);
                parametres[1] = "Novice";
                parametres[10] = "Start Loc";
            }
            public string[] ToListView ()
            {
                return parametres;
            }
        }
    }
}
