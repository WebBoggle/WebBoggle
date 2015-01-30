namespace BoggleClient
{
    partial class Prompt
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
            this.IPAddrLabel = new System.Windows.Forms.Label();
            this.IPAddrBox = new System.Windows.Forms.TextBox();
            this.NameLabel = new System.Windows.Forms.Label();
            this.NameBox = new System.Windows.Forms.TextBox();
            this.PlayButton = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(106, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(169, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Welcome to Boggle!";
            // 
            // IPAddrLabel
            // 
            this.IPAddrLabel.AutoSize = true;
            this.IPAddrLabel.BackColor = System.Drawing.Color.Transparent;
            this.IPAddrLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IPAddrLabel.ForeColor = System.Drawing.Color.White;
            this.IPAddrLabel.Location = new System.Drawing.Point(38, 69);
            this.IPAddrLabel.Name = "IPAddrLabel";
            this.IPAddrLabel.Size = new System.Drawing.Size(237, 13);
            this.IPAddrLabel.TabIndex = 1;
            this.IPAddrLabel.Text = "Please enter the IP address of the Boggle Server";
            // 
            // IPAddrBox
            // 
            this.IPAddrBox.Location = new System.Drawing.Point(41, 86);
            this.IPAddrBox.Name = "IPAddrBox";
            this.IPAddrBox.Size = new System.Drawing.Size(158, 20);
            this.IPAddrBox.TabIndex = 2;
            this.IPAddrBox.TextChanged += new System.EventHandler(this.IPAddrBox_TextChanged);
            // 
            // NameLabel
            // 
            this.NameLabel.AutoSize = true;
            this.NameLabel.BackColor = System.Drawing.Color.Transparent;
            this.NameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NameLabel.ForeColor = System.Drawing.Color.White;
            this.NameLabel.Location = new System.Drawing.Point(41, 113);
            this.NameLabel.Name = "NameLabel";
            this.NameLabel.Size = new System.Drawing.Size(121, 13);
            this.NameLabel.TabIndex = 3;
            this.NameLabel.Text = "Please Enter your Name";
            // 
            // NameBox
            // 
            this.NameBox.Location = new System.Drawing.Point(41, 130);
            this.NameBox.Name = "NameBox";
            this.NameBox.Size = new System.Drawing.Size(158, 20);
            this.NameBox.TabIndex = 4;
            this.NameBox.TextChanged += new System.EventHandler(this.NameBox_TextChanged);
            // 
            // PlayButton
            // 
            this.PlayButton.Enabled = false;
            this.PlayButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PlayButton.Location = new System.Drawing.Point(124, 181);
            this.PlayButton.Name = "PlayButton";
            this.PlayButton.Size = new System.Drawing.Size(75, 23);
            this.PlayButton.TabIndex = 5;
            this.PlayButton.Text = "Play!";
            this.PlayButton.UseVisualStyleBackColor = true;
            this.PlayButton.Click += new System.EventHandler(this.PlayButton_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Image = global::BoggleClient.Properties.Resources.boggle_logo;
            this.pictureBox1.Location = new System.Drawing.Point(224, 181);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(122, 50);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 6;
            this.pictureBox1.TabStop = false;
            // 
            // Prompt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::BoggleClient.Properties.Resources.grunge_halftone_background;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(358, 246);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.PlayButton);
            this.Controls.Add(this.NameBox);
            this.Controls.Add(this.NameLabel);
            this.Controls.Add(this.IPAddrBox);
            this.Controls.Add(this.IPAddrLabel);
            this.Controls.Add(this.label1);
            this.DoubleBuffered = true;
            this.Name = "Prompt";
            this.Text = "Welcome to Boggle!";
            this.Load += new System.EventHandler(this.Prompt_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label IPAddrLabel;
        private System.Windows.Forms.TextBox IPAddrBox;
        private System.Windows.Forms.Label NameLabel;
        private System.Windows.Forms.TextBox NameBox;
        private System.Windows.Forms.Button PlayButton;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}

