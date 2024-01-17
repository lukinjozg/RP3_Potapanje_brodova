namespace Potapanjebrodova
{
    partial class pocetniScreen
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
            this.easyAI = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.statistikeLabel = new System.Windows.Forms.Label();
            this.userStats = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // easyAI
            // 
            this.easyAI.BackColor = System.Drawing.Color.White;
            this.easyAI.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.easyAI.Location = new System.Drawing.Point(120, 60);
            this.easyAI.Name = "easyAI";
            this.easyAI.Size = new System.Drawing.Size(90, 90);
            this.easyAI.TabIndex = 2;
            this.easyAI.Text = "Easy\r\nDifficulty";
            this.easyAI.UseVisualStyleBackColor = false;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.White;
            this.button1.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(120, 180);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(90, 90);
            this.button1.TabIndex = 3;
            this.button1.Text = "Medium\r\nDifficulty";
            this.button1.UseVisualStyleBackColor = false;
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.White;
            this.button2.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Location = new System.Drawing.Point(120, 300);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(90, 90);
            this.button2.TabIndex = 4;
            this.button2.Text = "Hard\r\nDifficulty";
            this.button2.UseVisualStyleBackColor = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 42F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Transparent;
            this.label1.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.label1.Location = new System.Drawing.Point(254, 165);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(271, 126);
            this.label1.TabIndex = 5;
            this.label1.Text = "Potapanje\r\nBrodova";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // statistikeLabel
            // 
            this.statistikeLabel.BackColor = System.Drawing.Color.IndianRed;
            this.statistikeLabel.Font = new System.Drawing.Font("Times New Roman", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statistikeLabel.ForeColor = System.Drawing.Color.Black;
            this.statistikeLabel.Location = new System.Drawing.Point(556, 36);
            this.statistikeLabel.Name = "statistikeLabel";
            this.statistikeLabel.Size = new System.Drawing.Size(220, 40);
            this.statistikeLabel.TabIndex = 6;
            this.statistikeLabel.Text = "Statistike";
            this.statistikeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // userStats
            // 
            this.userStats.BackColor = System.Drawing.Color.IndianRed;
            this.userStats.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.userStats.Location = new System.Drawing.Point(556, 76);
            this.userStats.Name = "userStats";
            this.userStats.Size = new System.Drawing.Size(220, 327);
            this.userStats.TabIndex = 7;
            this.userStats.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // pocetniScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Potapanjebrodova.Properties.Resources.pocetniBackground;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.userStats);
            this.Controls.Add(this.statistikeLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.easyAI);
            this.Name = "pocetniScreen";
            this.Text = "Potapanje Brodova";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button easyAI;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label statistikeLabel;
        private System.Windows.Forms.Label userStats;
    }
}