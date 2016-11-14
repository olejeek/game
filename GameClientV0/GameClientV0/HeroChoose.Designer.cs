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
            this.play_btn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // createHero_btn
            // 
            this.createHero_btn.Location = new System.Drawing.Point(107, 214);
            this.createHero_btn.Name = "createHero_btn";
            this.createHero_btn.Size = new System.Drawing.Size(75, 23);
            this.createHero_btn.TabIndex = 0;
            this.createHero_btn.Text = "Create Hero";
            this.createHero_btn.UseVisualStyleBackColor = true;
            this.createHero_btn.Click += new System.EventHandler(this.createHero_btn_Click);
            // 
            // delHero_btn
            // 
            this.delHero_btn.Location = new System.Drawing.Point(197, 214);
            this.delHero_btn.Name = "delHero_btn";
            this.delHero_btn.Size = new System.Drawing.Size(75, 23);
            this.delHero_btn.TabIndex = 1;
            this.delHero_btn.Text = "Delete Hero";
            this.delHero_btn.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(260, 196);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Hero List";
            // 
            // play_btn
            // 
            this.play_btn.Location = new System.Drawing.Point(12, 214);
            this.play_btn.Name = "play_btn";
            this.play_btn.Size = new System.Drawing.Size(75, 23);
            this.play_btn.TabIndex = 3;
            this.play_btn.Text = "Play!";
            this.play_btn.UseVisualStyleBackColor = true;
            // 
            // HeroChoose
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.play_btn);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.delHero_btn);
            this.Controls.Add(this.createHero_btn);
            this.Name = "HeroChoose";
            this.Text = "HeroChoose";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.HeroChoose_FormClosed);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button createHero_btn;
        private System.Windows.Forms.Button delHero_btn;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button play_btn;
    }
}