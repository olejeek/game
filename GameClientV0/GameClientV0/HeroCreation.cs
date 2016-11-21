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
        private void strBtn_Click(object sender, EventArgs e)
        {
            if (STRvsINT.Value != 9)
            {
                STRvsINT.Value++;
                toolTip1.SetToolTip(STRvsINT, "STR: " + STRvsINT.Value + "\tINT: " + (10 - STRvsINT.Value));
            }
        }
        private void intBtn_Click(object sender, EventArgs e)
        {
            if (STRvsINT.Value != 1)
            {
                STRvsINT.Value--;
                toolTip1.SetToolTip(STRvsINT, "STR: " + STRvsINT.Value + "\tINT: " + (10 - STRvsINT.Value));
            }
        }

        private void agiBtn_Click(object sender, EventArgs e)
        {
            if (AGIvsLUK.Value != 9)
            {
                AGIvsLUK.Value++;
                toolTip1.SetToolTip(AGIvsLUK, "AGI: " + AGIvsLUK.Value + "\tLUK: " + (10 - AGIvsLUK.Value));
            }
        }

        private void lukBtn_Click(object sender, EventArgs e)
        {
            if (AGIvsLUK.Value != 1)
            {
                AGIvsLUK.Value--;
                toolTip1.SetToolTip(AGIvsLUK, "AGI: " + AGIvsLUK.Value + "\tLUK: " + (10 - AGIvsLUK.Value));
            }
        }

        private void vitBtn_Click(object sender, EventArgs e)
        {
            if (VITvsDEX.Value != 9)
            {
                VITvsDEX.Value++;
                toolTip1.SetToolTip(VITvsDEX, "VIT: " + VITvsDEX.Value + "\tDEX: " + (10 - VITvsDEX.Value));
            }
        }

        private void dexBtn_Click(object sender, EventArgs e)
        {
            if (VITvsDEX.Value != 1)
            {
                VITvsDEX.Value--;
                toolTip1.SetToolTip(VITvsDEX, "VIT: " + VITvsDEX.Value + "\tDEX: " + (10 - VITvsDEX.Value));
            }
        }

        public HeroCreation()
        {
            InitializeComponent();
            toolTip1.SetToolTip(STRvsINT, "STR: " + STRvsINT.Value + "\tINT: " + (10 - STRvsINT.Value));
            toolTip1.SetToolTip(AGIvsLUK, "AGI: " + AGIvsLUK.Value + "\tLUK: " + (10 - AGIvsLUK.Value));
            toolTip1.SetToolTip(VITvsDEX, "VIT: " + VITvsDEX.Value + "\tDEX: " + (10 - VITvsDEX.Value));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void HeroCreation_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void createBtn_Click(object sender, EventArgs e)
        {
            if (nameBox.Text.Length != 0)
            {
                StringBuilder answer = new StringBuilder();
                answer.Append("2\n");
                answer.Append(nameBox.Text + "\t");
                answer.Append(STRvsINT.Value + "\t");
                answer.Append(AGIvsLUK.Value + "\t");
                answer.Append(VITvsDEX.Value + "\t");
                answer.Append((10-STRvsINT.Value) + "\t");
                answer.Append((10 - VITvsDEX.Value) + "\t");
                answer.Append((10 - AGIvsLUK.Value));
                //OnlineUser.Send(answer.ToString());
            }
            else MessageBox.Show("You don`t write hero name!");
        }

        private void nameBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsLetterOrDigit(e.KeyChar) && e.KeyChar!=8) e.Handled = true;
            else e.Handled = false;
        }
    }
}
