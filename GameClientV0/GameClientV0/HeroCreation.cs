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
    public partial class HeroCreation : Form
    {
        int Str, Agi, Vit, Int, Dex, Luc, freeSP;

        public HeroCreation()
        {
            InitializeComponent();
            Str = 1;
            Agi = 1;
            Vit = 1;
            Int = 1;
            Dex = 1;
            Luc = 1;
            freeSP = 48;
        }

        private int inc(int stat)
        {
            if (freeSP%2>0)
            {
                stat++;
                freeSP -= 2;
                freeSPBox.Text = freeSP.ToString();
            }
            return stat;
        }

        private int dec(int stat)
        {
            stat--;
            freeSP += 2;
            freeSPBox.Text = freeSP.ToString();
            return stat;
        }

        private void strNum_ValueChanged(object sender, EventArgs e)
        {
            if ((int)strNum.Value > Str) Str = inc(Str);
            else Str = dec(Str);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void HeroCreation_FormClosing(object sender, FormClosingEventArgs e)
        {
            HeroChoose hc = new HeroChoose(new string[1]);
            hc.Show();
        }

        private void agiNum_ValueChanged(object sender, EventArgs e)
        {
            if ((int)agiNum.Value > Agi) Agi = inc(Agi);
            else Agi = dec(Agi);
        }
        //it is not work
        private void vitNum_ValueChanged(object sender, EventArgs e)
        {
            if ((int)vitNum.Value > Vit) Vit = inc(Vit);
            else Vit = dec(Vit);
        }
        private void intNum_ValueChanged(object sender, EventArgs e)
        {
            if ((int)intNum.Value > Int) Int = inc(Int);
            else Int = dec(Int);
        }
        private void dexNum_ValueChanged(object sender, EventArgs e)
        {
            if ((int)dexNum.Value > Dex) Dex = inc(Dex);
            else Dex = dec(Dex);
        }
        private void lukNum_ValueChanged(object sender, EventArgs e)
        {
            if ((int)lukNum.Value > Luc) Luc = inc(Luc);
            else Luc = dec(Luc);
        }

        private void nameBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsLetterOrDigit(e.KeyChar) && e.KeyChar!=8) e.Handled = true;
            else e.Handled = false;
        }
    }
}
