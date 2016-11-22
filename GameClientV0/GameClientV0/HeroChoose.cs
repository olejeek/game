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
            for (int i = 0; i < heroes.Count; i++)
            {
                hero.Add(new HeroInfo(heroes[i]));
                ListViewItem heroListItem = new ListViewItem(hero[i].ToListView());
                heroListView.Items.Add(heroListItem);
            }
        }
        public void HeroUpdate(List<string> heroes)
        {
            heroListView.Items.Clear();
            hero = new List<HeroInfo>();
            for (int i = 0; i < heroes.Count; i++)
            {
                hero.Add(new HeroInfo(heroes[i]));
                ListViewItem heroListItem = new ListViewItem(hero[i].ToListView());
                heroListView.Items.Add(heroListItem);
            }
        }

        private void HeroChoose_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (OnlineUser.status != OnlineUser.Status.Disconnect)
                OnlineUser.BlockToSend(new Block(BlockCode.Disconnect, (int)DisconnectType.Exit));
            OnlineUser.CloseConnection();
        }

        private void createHero_btn_Click(object sender, EventArgs e)
        {
            OnlineUser.OpenFormToCreateHero();
        }
        private void delHero_btn_Click(object sender, EventArgs e)
        {
            //string name = heroListView.SelectedItems[0].Text;
            //MessageBox.Show(name);
            Block del = new Block(BlockCode.ChooseHero, (int)ChooseHeroType.DeleteHero);
            OnlineUser.BlockToSend(new Block(BlockCode.ChooseHero, (int)ChooseHeroType.DeleteHero,
                heroListView.SelectedItems[0].Text));
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

        private void play_btn_Click(object sender, EventArgs e)
        {

        }
    }
}
