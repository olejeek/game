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
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dexBtn = new System.Windows.Forms.Button();
            this.vitBtn = new System.Windows.Forms.Button();
            this.lukBtn = new System.Windows.Forms.Button();
            this.agiBtn = new System.Windows.Forms.Button();
            this.intBtn = new System.Windows.Forms.Button();
            this.strBtn = new System.Windows.Forms.Button();
            this.VITvsDEX = new System.Windows.Forms.ProgressBar();
            this.AGIvsLUK = new System.Windows.Forms.ProgressBar();
            this.STRvsINT = new System.Windows.Forms.ProgressBar();
            this.nameBox = new System.Windows.Forms.TextBox();
            this.createBtn = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox1.SuspendLayout();
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
            this.groupBox1.Controls.Add(this.dexBtn);
            this.groupBox1.Controls.Add(this.vitBtn);
            this.groupBox1.Controls.Add(this.lukBtn);
            this.groupBox1.Controls.Add(this.agiBtn);
            this.groupBox1.Controls.Add(this.intBtn);
            this.groupBox1.Controls.Add(this.strBtn);
            this.groupBox1.Controls.Add(this.VITvsDEX);
            this.groupBox1.Controls.Add(this.AGIvsLUK);
            this.groupBox1.Controls.Add(this.STRvsINT);
            this.groupBox1.Location = new System.Drawing.Point(15, 55);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(219, 112);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Stats";
            // 
            // dexBtn
            // 
            this.dexBtn.Location = new System.Drawing.Point(163, 78);
            this.dexBtn.Name = "dexBtn";
            this.dexBtn.Size = new System.Drawing.Size(42, 24);
            this.dexBtn.TabIndex = 14;
            this.dexBtn.Text = "DEX";
            this.dexBtn.UseVisualStyleBackColor = true;
            this.dexBtn.Click += new System.EventHandler(this.dexBtn_Click);
            // 
            // vitBtn
            // 
            this.vitBtn.Location = new System.Drawing.Point(9, 77);
            this.vitBtn.Name = "vitBtn";
            this.vitBtn.Size = new System.Drawing.Size(42, 24);
            this.vitBtn.TabIndex = 13;
            this.vitBtn.Text = "VIT";
            this.vitBtn.UseVisualStyleBackColor = true;
            this.vitBtn.Click += new System.EventHandler(this.vitBtn_Click);
            // 
            // lukBtn
            // 
            this.lukBtn.Location = new System.Drawing.Point(163, 48);
            this.lukBtn.Name = "lukBtn";
            this.lukBtn.Size = new System.Drawing.Size(42, 24);
            this.lukBtn.TabIndex = 12;
            this.lukBtn.Text = "LUK";
            this.lukBtn.UseVisualStyleBackColor = true;
            this.lukBtn.Click += new System.EventHandler(this.lukBtn_Click);
            // 
            // agiBtn
            // 
            this.agiBtn.Location = new System.Drawing.Point(9, 48);
            this.agiBtn.Name = "agiBtn";
            this.agiBtn.Size = new System.Drawing.Size(42, 24);
            this.agiBtn.TabIndex = 11;
            this.agiBtn.Text = "AGI";
            this.agiBtn.UseVisualStyleBackColor = true;
            this.agiBtn.Click += new System.EventHandler(this.agiBtn_Click);
            // 
            // intBtn
            // 
            this.intBtn.Location = new System.Drawing.Point(163, 20);
            this.intBtn.Name = "intBtn";
            this.intBtn.Size = new System.Drawing.Size(42, 24);
            this.intBtn.TabIndex = 10;
            this.intBtn.Text = "INT";
            this.intBtn.UseVisualStyleBackColor = true;
            this.intBtn.Click += new System.EventHandler(this.intBtn_Click);
            // 
            // strBtn
            // 
            this.strBtn.Location = new System.Drawing.Point(9, 20);
            this.strBtn.Name = "strBtn";
            this.strBtn.Size = new System.Drawing.Size(42, 24);
            this.strBtn.TabIndex = 9;
            this.strBtn.Text = "STR";
            this.strBtn.UseVisualStyleBackColor = true;
            this.strBtn.Click += new System.EventHandler(this.strBtn_Click);
            // 
            // VITvsDEX
            // 
            this.VITvsDEX.Location = new System.Drawing.Point(57, 78);
            this.VITvsDEX.Maximum = 10;
            this.VITvsDEX.Name = "VITvsDEX";
            this.VITvsDEX.Size = new System.Drawing.Size(100, 24);
            this.VITvsDEX.Step = 1;
            this.VITvsDEX.TabIndex = 8;
            this.VITvsDEX.Value = 5;
            // 
            // AGIvsLUK
            // 
            this.AGIvsLUK.Location = new System.Drawing.Point(57, 49);
            this.AGIvsLUK.Maximum = 10;
            this.AGIvsLUK.Name = "AGIvsLUK";
            this.AGIvsLUK.Size = new System.Drawing.Size(100, 24);
            this.AGIvsLUK.Step = 1;
            this.AGIvsLUK.TabIndex = 7;
            this.AGIvsLUK.Value = 5;
            // 
            // STRvsINT
            // 
            this.STRvsINT.BackColor = System.Drawing.SystemColors.HotTrack;
            this.STRvsINT.Cursor = System.Windows.Forms.Cursors.Default;
            this.STRvsINT.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.STRvsINT.Location = new System.Drawing.Point(57, 20);
            this.STRvsINT.Maximum = 10;
            this.STRvsINT.Name = "STRvsINT";
            this.STRvsINT.Size = new System.Drawing.Size(100, 24);
            this.STRvsINT.Step = 1;
            this.STRvsINT.TabIndex = 6;
            this.STRvsINT.Value = 5;
            // 
            // nameBox
            // 
            this.nameBox.Location = new System.Drawing.Point(50, 19);
            this.nameBox.Name = "nameBox";
            this.nameBox.Size = new System.Drawing.Size(100, 20);
            this.nameBox.TabIndex = 2;
            this.nameBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.nameBox_KeyPress);
            // 
            // createBtn
            // 
            this.createBtn.Location = new System.Drawing.Point(240, 76);
            this.createBtn.Name = "createBtn";
            this.createBtn.Size = new System.Drawing.Size(75, 23);
            this.createBtn.TabIndex = 5;
            this.createBtn.Text = "Create";
            this.createBtn.UseVisualStyleBackColor = true;
            this.createBtn.Click += new System.EventHandler(this.createBtn_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(156, 22);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(154, 13);
            this.label9.TabIndex = 6;
            this.label9.Text = "(supports only A-Z, a-z and 0-9)";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(240, 133);
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
            this.ClientSize = new System.Drawing.Size(329, 176);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.createBtn);
            this.Controls.Add(this.nameBox);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Name = "HeroCreation";
            this.Text = "HeroCreation";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.HeroCreation_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox nameBox;
        private System.Windows.Forms.Button createBtn;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ProgressBar VITvsDEX;
        private System.Windows.Forms.ProgressBar AGIvsLUK;
        private System.Windows.Forms.ProgressBar STRvsINT;
        private System.Windows.Forms.Button dexBtn;
        private System.Windows.Forms.Button vitBtn;
        private System.Windows.Forms.Button lukBtn;
        private System.Windows.Forms.Button agiBtn;
        private System.Windows.Forms.Button intBtn;
        private System.Windows.Forms.Button strBtn;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}