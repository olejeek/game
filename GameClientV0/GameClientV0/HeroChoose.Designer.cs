namespace GameClientV0
{
    partial class HeroChoose
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
            this.createHero_btn = new System.Windows.Forms.Button();
            this.delHero_btn = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.heroListView = new System.Windows.Forms.ListView();
            this.hName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.hClass = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.hStr = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.hAgi = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.hVit = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.hInt = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.hDex = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.hLuk = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.hBLvl = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.hJLvl = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.hLoc = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.play_btn = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // createHero_btn
            // 
            this.createHero_btn.Location = new System.Drawing.Point(271, 213);
            this.createHero_btn.Name = "createHero_btn";
            this.createHero_btn.Size = new System.Drawing.Size(75, 23);
            this.createHero_btn.TabIndex = 0;
            this.createHero_btn.Text = "Create Hero";
            this.createHero_btn.UseVisualStyleBackColor = true;
            this.createHero_btn.Click += new System.EventHandler(this.createHero_btn_Click);
            // 
            // delHero_btn
            // 
            this.delHero_btn.Location = new System.Drawing.Point(519, 214);
            this.delHero_btn.Name = "delHero_btn";
            this.delHero_btn.Size = new System.Drawing.Size(75, 23);
            this.delHero_btn.TabIndex = 1;
            this.delHero_btn.Text = "Delete Hero";
            this.delHero_btn.UseVisualStyleBackColor = true;
            this.delHero_btn.Click += new System.EventHandler(this.delHero_btn_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.heroListView);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(591, 196);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Hero List";
            // 
            // heroListView
            // 
            this.heroListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.hName,
            this.hClass,
            this.hStr,
            this.hAgi,
            this.hVit,
            this.hInt,
            this.hDex,
            this.hLuk,
            this.hBLvl,
            this.hJLvl,
            this.hLoc});
            this.heroListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.heroListView.LabelWrap = false;
            this.heroListView.Location = new System.Drawing.Point(6, 19);
            this.heroListView.Name = "heroListView";
            this.heroListView.Size = new System.Drawing.Size(576, 171);
            this.heroListView.TabIndex = 0;
            this.heroListView.UseCompatibleStateImageBehavior = false;
            this.heroListView.View = System.Windows.Forms.View.Details;
            // 
            // hName
            // 
            this.hName.Text = "Name";
            this.hName.Width = 100;
            // 
            // hClass
            // 
            this.hClass.Text = "Class";
            this.hClass.Width = 80;
            // 
            // hStr
            // 
            this.hStr.Text = "STR";
            this.hStr.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.hStr.Width = 35;
            // 
            // hAgi
            // 
            this.hAgi.Text = "AGI";
            this.hAgi.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.hAgi.Width = 35;
            // 
            // hVit
            // 
            this.hVit.Text = "VIT";
            this.hVit.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.hVit.Width = 35;
            // 
            // hInt
            // 
            this.hInt.Text = "INT";
            this.hInt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.hInt.Width = 35;
            // 
            // hDex
            // 
            this.hDex.Text = "DEX";
            this.hDex.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.hDex.Width = 35;
            // 
            // hLuk
            // 
            this.hLuk.Text = "LUK";
            this.hLuk.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.hLuk.Width = 35;
            // 
            // hBLvl
            // 
            this.hBLvl.Text = "B Level";
            this.hBLvl.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.hBLvl.Width = 50;
            // 
            // hJLvl
            // 
            this.hJLvl.Text = "J Level";
            this.hJLvl.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.hJLvl.Width = 50;
            // 
            // hLoc
            // 
            this.hLoc.Text = "Location";
            this.hLoc.Width = 81;
            // 
            // play_btn
            // 
            this.play_btn.Location = new System.Drawing.Point(12, 214);
            this.play_btn.Name = "play_btn";
            this.play_btn.Size = new System.Drawing.Size(75, 23);
            this.play_btn.TabIndex = 3;
            this.play_btn.Text = "Play!";
            this.play_btn.UseVisualStyleBackColor = true;
            this.play_btn.Click += new System.EventHandler(this.play_btn_Click);
            // 
            // HeroChoose
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(614, 248);
            this.Controls.Add(this.play_btn);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.delHero_btn);
            this.Controls.Add(this.createHero_btn);
            this.Name = "HeroChoose";
            this.Text = "HeroChoose";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.HeroChoose_FormClosed);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button createHero_btn;
        private System.Windows.Forms.Button delHero_btn;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button play_btn;
        private System.Windows.Forms.ListView heroListView;
        private System.Windows.Forms.ColumnHeader hName;
        private System.Windows.Forms.ColumnHeader hClass;
        private System.Windows.Forms.ColumnHeader hStr;
        private System.Windows.Forms.ColumnHeader hAgi;
        private System.Windows.Forms.ColumnHeader hVit;
        private System.Windows.Forms.ColumnHeader hInt;
        private System.Windows.Forms.ColumnHeader hDex;
        private System.Windows.Forms.ColumnHeader hLuk;
        private System.Windows.Forms.ColumnHeader hBLvl;
        private System.Windows.Forms.ColumnHeader hJLvl;
        private System.Windows.Forms.ColumnHeader hLoc;
    }
}