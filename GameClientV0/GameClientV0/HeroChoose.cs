using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameClientV0
{
    public partial class HeroChoose : Form
    {
        public HeroChoose(string[] heroes)
        {
            InitializeComponent();
        }

        private void HeroChoose_FormClosed(object sender, FormClosedEventArgs e)
        {
            OnlineUser.SendAndDisconnect("-1\nDisconnect!");
            if (Application.OpenForms.Count<=1) Application.OpenForms[0].Show();
        }

        private void createHero_btn_Click(object sender, EventArgs e)
        {
            HeroCreation NHero = new HeroCreation();
            NHero.ShowDialog();
        }
    }
}
