namespace GameClientV0
{
    partial class HeroCreation
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lukNum = new System.Windows.Forms.NumericUpDown();
            this.dexNum = new System.Windows.Forms.NumericUpDown();
            this.intNum = new System.Windows.Forms.NumericUpDown();
            this.vitNum = new System.Windows.Forms.NumericUpDown();
            this.agiNum = new System.Windows.Forms.NumericUpDown();
            this.strNum = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.nameBox = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.freeSPBox = new System.Windows.Forms.TextBox();
            this.createBtn = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lukNum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dexNum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.intNum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.vitNum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.agiNum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.strNum)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Nick:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lukNum);
            this.groupBox1.Controls.Add(this.dexNum);
            this.groupBox1.Controls.Add(this.intNum);
            this.groupBox1.Controls.Add(this.vitNum);
            this.groupBox1.Controls.Add(this.agiNum);
            this.groupBox1.Controls.Add(this.strNum);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(15, 55);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(107, 184);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Stats";
            // 
            // lukNum
            // 
            this.lukNum.Location = new System.Drawing.Point(53, 154);
            this.lukNum.Maximum = new decimal(new int[] {
            9,
            0,
            0,
            0});
            this.lukNum.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.lukNum.Name = "lukNum";
            this.lukNum.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lukNum.Size = new System.Drawing.Size(40, 20);
            this.lukNum.TabIndex = 11;
            this.lukNum.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.lukNum.ValueChanged += new System.EventHandler(this.lukNum_ValueChanged);
            // 
            // dexNum
            // 
            this.dexNum.Location = new System.Drawing.Point(53, 128);
            this.dexNum.Maximum = new decimal(new int[] {
            9,
            0,
            0,
            0});
            this.dexNum.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.dexNum.Name = "dexNum";
            this.dexNum.Size = new System.Drawing.Size(40, 20);
            this.dexNum.TabIndex = 10;
            this.dexNum.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.dexNum.ValueChanged += new System.EventHandler(this.dexNum_ValueChanged);
            // 
            // intNum
            // 
            this.intNum.Location = new System.Drawing.Point(53, 102);
            this.intNum.Maximum = new decimal(new int[] {
            9,
            0,
            0,
            0});
            this.intNum.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.intNum.Name = "intNum";
            this.intNum.Size = new System.Drawing.Size(40, 20);
            this.intNum.TabIndex = 9;
            this.intNum.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.intNum.ValueChanged += new System.EventHandler(this.intNum_ValueChanged);
            // 
            // vitNum
            // 
            this.vitNum.Location = new System.Drawing.Point(53, 76);
            this.vitNum.Maximum = new decimal(new int[] {
            9,
            0,
            0,
            0});
            this.vitNum.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.vitNum.Name = "vitNum";
            this.vitNum.Size = new System.Drawing.Size(40, 20);
            this.vitNum.TabIndex = 8;
            this.vitNum.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.vitNum.ValueChanged += new System.EventHandler(this.vitNum_ValueChanged);
            // 
            // agiNum
            // 
            this.agiNum.Location = new System.Drawing.Point(53, 50);
            this.agiNum.Maximum = new decimal(new int[] {
            9,
            0,
            0,
            0});
            this.agiNum.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.agiNum.Name = "agiNum";
            this.agiNum.Size = new System.Drawing.Size(40, 20);
            this.agiNum.TabIndex = 7;
            this.agiNum.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.agiNum.ValueChanged += new System.EventHandler(this.agiNum_ValueChanged);
            // 
            // strNum
            // 
            this.strNum.Location = new System.Drawing.Point(53, 24);
            this.strNum.Maximum = new decimal(new int[] {
            9,
            0,
            0,
            0});
            this.strNum.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.strNum.Name = "strNum";
            this.strNum.Size = new System.Drawing.Size(40, 20);
            this.strNum.TabIndex = 6;
            this.strNum.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.strNum.ValueChanged += new System.EventHandler(this.strNum_ValueChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(15, 156);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(31, 13);
            this.label7.TabIndex = 5;
            this.label7.Text = "LUK:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(15, 130);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(32, 13);
            this.label6.TabIndex = 4;
            this.label6.Text = "DEX:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(19, 104);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(28, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "INT:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(20, 78);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(27, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "VIT:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(19, 52);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(28, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "AGI:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "STR:";
            // 
            // nameBox
            // 
            this.nameBox.Location = new System.Drawing.Point(50, 19);
            this.nameBox.Name = "nameBox";
            this.nameBox.Size = new System.Drawing.Size(100, 20);
            this.nameBox.TabIndex = 2;
            this.nameBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.nameBox_KeyPress);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(158, 81);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(82, 13);
            this.label8.TabIndex = 3;
            this.label8.Text = "Free Stat Points";
            // 
            // freeSPBox
            // 
            this.freeSPBox.Enabled = false;
            this.freeSPBox.Location = new System.Drawing.Point(151, 97);
            this.freeSPBox.Name = "freeSPBox";
            this.freeSPBox.Size = new System.Drawing.Size(100, 20);
            this.freeSPBox.TabIndex = 4;
            this.freeSPBox.Text = "48";
            // 
            // createBtn
            // 
            this.createBtn.Location = new System.Drawing.Point(165, 133);
            this.createBtn.Name = "createBtn";
            this.createBtn.Size = new System.Drawing.Size(75, 23);
            this.createBtn.TabIndex = 5;
            this.createBtn.Text = "Create";
            this.createBtn.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(156, 22);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(133, 13);
            this.label9.TabIndex = 6;
            this.label9.Text = "(supports only a-z and A-Z)";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(165, 216);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 7;
            this.button1.Text = "Cancel";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // HeroCreation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(295, 271);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.createBtn);
            this.Controls.Add(this.freeSPBox);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.nameBox);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Name = "HeroCreation";
            this.Text = "HeroCreation";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.HeroCreation_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lukNum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dexNum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.intNum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.vitNum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.agiNum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.strNum)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.NumericUpDown lukNum;
        private System.Windows.Forms.NumericUpDown dexNum;
        private System.Windows.Forms.NumericUpDown intNum;
        private System.Windows.Forms.NumericUpDown vitNum;
        private System.Windows.Forms.NumericUpDown agiNum;
        private System.Windows.Forms.NumericUpDown strNum;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox nameBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox freeSPBox;
        private System.Windows.Forms.Button createBtn;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button button1;
    }
}