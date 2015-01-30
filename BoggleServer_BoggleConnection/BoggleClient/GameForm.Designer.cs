namespace BoggleClient
{
    partial class GameForm
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
            this.TimeLabel = new System.Windows.Forms.Label();
            this.P1ScoreBox = new System.Windows.Forms.TextBox();
            this.P2ScoreBox = new System.Windows.Forms.TextBox();
            this.P1ScoreLabel = new System.Windows.Forms.Label();
            this.P2ScoreLabel = new System.Windows.Forms.Label();
            this.WordBox = new System.Windows.Forms.TextBox();
            this.PlayButton = new System.Windows.Forms.Button();
            this.Box16 = new System.Windows.Forms.Button();
            this.Box15 = new System.Windows.Forms.Button();
            this.Box14 = new System.Windows.Forms.Button();
            this.Box13 = new System.Windows.Forms.Button();
            this.Box12 = new System.Windows.Forms.Button();
            this.Box11 = new System.Windows.Forms.Button();
            this.Box10 = new System.Windows.Forms.Button();
            this.Box09 = new System.Windows.Forms.Button();
            this.Box08 = new System.Windows.Forms.Button();
            this.Box07 = new System.Windows.Forms.Button();
            this.Box06 = new System.Windows.Forms.Button();
            this.Box05 = new System.Windows.Forms.Button();
            this.Box04 = new System.Windows.Forms.Button();
            this.Box03 = new System.Windows.Forms.Button();
            this.Box02 = new System.Windows.Forms.Button();
            this.Box01 = new System.Windows.Forms.Button();
            this.CancelWordButton = new System.Windows.Forms.Button();
            this.ExitGameButton = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // TimeLabel
            // 
            this.TimeLabel.AutoSize = true;
            this.TimeLabel.BackColor = System.Drawing.Color.Black;
            this.TimeLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.TimeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 40F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TimeLabel.ForeColor = System.Drawing.Color.White;
            this.TimeLabel.Location = new System.Drawing.Point(411, 39);
            this.TimeLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.TimeLabel.MinimumSize = new System.Drawing.Size(300, 0);
            this.TimeLabel.Name = "TimeLabel";
            this.TimeLabel.Size = new System.Drawing.Size(300, 78);
            this.TimeLabel.TabIndex = 17;
            this.TimeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // P1ScoreBox
            // 
            this.P1ScoreBox.Enabled = false;
            this.P1ScoreBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.P1ScoreBox.ForeColor = System.Drawing.Color.RoyalBlue;
            this.P1ScoreBox.Location = new System.Drawing.Point(709, 250);
            this.P1ScoreBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.P1ScoreBox.Name = "P1ScoreBox";
            this.P1ScoreBox.Size = new System.Drawing.Size(132, 55);
            this.P1ScoreBox.TabIndex = 18;
            this.P1ScoreBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // P2ScoreBox
            // 
            this.P2ScoreBox.Enabled = false;
            this.P2ScoreBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.P2ScoreBox.ForeColor = System.Drawing.Color.DarkRed;
            this.P2ScoreBox.Location = new System.Drawing.Point(947, 250);
            this.P2ScoreBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.P2ScoreBox.Name = "P2ScoreBox";
            this.P2ScoreBox.Size = new System.Drawing.Size(132, 55);
            this.P2ScoreBox.TabIndex = 19;
            this.P2ScoreBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // P1ScoreLabel
            // 
            this.P1ScoreLabel.AutoSize = true;
            this.P1ScoreLabel.BackColor = System.Drawing.Color.Transparent;
            this.P1ScoreLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.P1ScoreLabel.ForeColor = System.Drawing.Color.White;
            this.P1ScoreLabel.Location = new System.Drawing.Point(712, 215);
            this.P1ScoreLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.P1ScoreLabel.Name = "P1ScoreLabel";
            this.P1ScoreLabel.Size = new System.Drawing.Size(86, 31);
            this.P1ScoreLabel.TabIndex = 20;
            this.P1ScoreLabel.Text = "label1";
            // 
            // P2ScoreLabel
            // 
            this.P2ScoreLabel.AutoSize = true;
            this.P2ScoreLabel.BackColor = System.Drawing.Color.Transparent;
            this.P2ScoreLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.P2ScoreLabel.ForeColor = System.Drawing.Color.White;
            this.P2ScoreLabel.Location = new System.Drawing.Point(949, 215);
            this.P2ScoreLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.P2ScoreLabel.Name = "P2ScoreLabel";
            this.P2ScoreLabel.Size = new System.Drawing.Size(38, 31);
            this.P2ScoreLabel.TabIndex = 21;
            this.P2ScoreLabel.Text = "...";
            // 
            // WordBox
            // 
            this.WordBox.Enabled = false;
            this.WordBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.WordBox.Location = new System.Drawing.Point(688, 427);
            this.WordBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.WordBox.Name = "WordBox";
            this.WordBox.Size = new System.Drawing.Size(395, 41);
            this.WordBox.TabIndex = 22;
            this.WordBox.TextChanged += new System.EventHandler(this.WordBox_TextChanged);
            // 
            // PlayButton
            // 
            this.PlayButton.Enabled = false;
            this.PlayButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PlayButton.Location = new System.Drawing.Point(688, 506);
            this.PlayButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.PlayButton.Name = "PlayButton";
            this.PlayButton.Size = new System.Drawing.Size(165, 60);
            this.PlayButton.TabIndex = 23;
            this.PlayButton.Text = "Play Word";
            this.PlayButton.UseVisualStyleBackColor = true;
            this.PlayButton.Click += new System.EventHandler(this.PlayButton_Click);
            // 
            // Box16
            // 
            this.Box16.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Box16.BackColor = System.Drawing.Color.Black;
            this.Box16.BackgroundImage = global::BoggleClient.Properties.Resources.cube;
            this.Box16.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.Box16.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Box16.Font = new System.Drawing.Font("Arial Rounded MT Bold", 25.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Box16.Location = new System.Drawing.Point(447, 425);
            this.Box16.Margin = new System.Windows.Forms.Padding(0);
            this.Box16.Name = "Box16";
            this.Box16.Size = new System.Drawing.Size(93, 95);
            this.Box16.TabIndex = 40;
            this.Box16.UseVisualStyleBackColor = false;
            this.Box16.Click += new System.EventHandler(this.Box16_Click);
            // 
            // Box15
            // 
            this.Box15.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Box15.BackColor = System.Drawing.Color.Black;
            this.Box15.BackgroundImage = global::BoggleClient.Properties.Resources.cube;
            this.Box15.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.Box15.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Box15.Font = new System.Drawing.Font("Arial Rounded MT Bold", 25.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Box15.Location = new System.Drawing.Point(306, 425);
            this.Box15.Margin = new System.Windows.Forms.Padding(0);
            this.Box15.Name = "Box15";
            this.Box15.Size = new System.Drawing.Size(93, 95);
            this.Box15.TabIndex = 39;
            this.Box15.UseVisualStyleBackColor = false;
            this.Box15.Click += new System.EventHandler(this.Box15_Click);
            // 
            // Box14
            // 
            this.Box14.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Box14.BackColor = System.Drawing.Color.Black;
            this.Box14.BackgroundImage = global::BoggleClient.Properties.Resources.cube;
            this.Box14.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.Box14.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Box14.Font = new System.Drawing.Font("Arial Rounded MT Bold", 25.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Box14.Location = new System.Drawing.Point(165, 425);
            this.Box14.Margin = new System.Windows.Forms.Padding(0);
            this.Box14.Name = "Box14";
            this.Box14.Size = new System.Drawing.Size(93, 95);
            this.Box14.TabIndex = 38;
            this.Box14.UseVisualStyleBackColor = false;
            this.Box14.Click += new System.EventHandler(this.Box14_Click);
            // 
            // Box13
            // 
            this.Box13.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Box13.BackColor = System.Drawing.Color.Black;
            this.Box13.BackgroundImage = global::BoggleClient.Properties.Resources.cube;
            this.Box13.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.Box13.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Box13.Font = new System.Drawing.Font("Arial Rounded MT Bold", 25.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Box13.Location = new System.Drawing.Point(24, 425);
            this.Box13.Margin = new System.Windows.Forms.Padding(0);
            this.Box13.Name = "Box13";
            this.Box13.Size = new System.Drawing.Size(93, 95);
            this.Box13.TabIndex = 37;
            this.Box13.UseVisualStyleBackColor = false;
            this.Box13.Click += new System.EventHandler(this.Box13_Click);
            // 
            // Box12
            // 
            this.Box12.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Box12.BackColor = System.Drawing.Color.Black;
            this.Box12.BackgroundImage = global::BoggleClient.Properties.Resources.cube;
            this.Box12.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.Box12.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Box12.Font = new System.Drawing.Font("Arial Rounded MT Bold", 25.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Box12.Location = new System.Drawing.Point(447, 290);
            this.Box12.Margin = new System.Windows.Forms.Padding(0);
            this.Box12.Name = "Box12";
            this.Box12.Size = new System.Drawing.Size(93, 95);
            this.Box12.TabIndex = 36;
            this.Box12.UseVisualStyleBackColor = false;
            this.Box12.Click += new System.EventHandler(this.Box12_Click);
            // 
            // Box11
            // 
            this.Box11.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Box11.BackColor = System.Drawing.Color.Black;
            this.Box11.BackgroundImage = global::BoggleClient.Properties.Resources.cube;
            this.Box11.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.Box11.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Box11.Font = new System.Drawing.Font("Arial Rounded MT Bold", 25.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Box11.Location = new System.Drawing.Point(306, 290);
            this.Box11.Margin = new System.Windows.Forms.Padding(0);
            this.Box11.Name = "Box11";
            this.Box11.Size = new System.Drawing.Size(93, 95);
            this.Box11.TabIndex = 35;
            this.Box11.UseVisualStyleBackColor = false;
            this.Box11.Click += new System.EventHandler(this.Box11_Click);
            // 
            // Box10
            // 
            this.Box10.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Box10.BackColor = System.Drawing.Color.Black;
            this.Box10.BackgroundImage = global::BoggleClient.Properties.Resources.cube;
            this.Box10.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.Box10.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Box10.Font = new System.Drawing.Font("Arial Rounded MT Bold", 25.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Box10.Location = new System.Drawing.Point(165, 290);
            this.Box10.Margin = new System.Windows.Forms.Padding(0);
            this.Box10.Name = "Box10";
            this.Box10.Size = new System.Drawing.Size(93, 95);
            this.Box10.TabIndex = 34;
            this.Box10.UseVisualStyleBackColor = false;
            this.Box10.Click += new System.EventHandler(this.Box10_Click);
            // 
            // Box09
            // 
            this.Box09.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Box09.BackColor = System.Drawing.Color.Black;
            this.Box09.BackgroundImage = global::BoggleClient.Properties.Resources.cube;
            this.Box09.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.Box09.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Box09.Font = new System.Drawing.Font("Arial Rounded MT Bold", 25.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Box09.Location = new System.Drawing.Point(24, 290);
            this.Box09.Margin = new System.Windows.Forms.Padding(0);
            this.Box09.Name = "Box09";
            this.Box09.Size = new System.Drawing.Size(93, 95);
            this.Box09.TabIndex = 33;
            this.Box09.UseVisualStyleBackColor = false;
            this.Box09.Click += new System.EventHandler(this.Box09_Click);
            // 
            // Box08
            // 
            this.Box08.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Box08.BackColor = System.Drawing.Color.Black;
            this.Box08.BackgroundImage = global::BoggleClient.Properties.Resources.cube;
            this.Box08.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.Box08.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Box08.Font = new System.Drawing.Font("Arial Rounded MT Bold", 25.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Box08.Location = new System.Drawing.Point(447, 155);
            this.Box08.Margin = new System.Windows.Forms.Padding(0);
            this.Box08.Name = "Box08";
            this.Box08.Size = new System.Drawing.Size(93, 95);
            this.Box08.TabIndex = 32;
            this.Box08.UseVisualStyleBackColor = false;
            this.Box08.Click += new System.EventHandler(this.Box08_Click);
            // 
            // Box07
            // 
            this.Box07.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Box07.BackColor = System.Drawing.Color.Black;
            this.Box07.BackgroundImage = global::BoggleClient.Properties.Resources.cube;
            this.Box07.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.Box07.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Box07.Font = new System.Drawing.Font("Arial Rounded MT Bold", 25.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Box07.Location = new System.Drawing.Point(306, 155);
            this.Box07.Margin = new System.Windows.Forms.Padding(0);
            this.Box07.Name = "Box07";
            this.Box07.Size = new System.Drawing.Size(93, 95);
            this.Box07.TabIndex = 31;
            this.Box07.UseVisualStyleBackColor = false;
            this.Box07.Click += new System.EventHandler(this.Box07_Click);
            // 
            // Box06
            // 
            this.Box06.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Box06.BackColor = System.Drawing.Color.Black;
            this.Box06.BackgroundImage = global::BoggleClient.Properties.Resources.cube;
            this.Box06.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.Box06.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Box06.Font = new System.Drawing.Font("Arial Rounded MT Bold", 25.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Box06.Location = new System.Drawing.Point(165, 155);
            this.Box06.Margin = new System.Windows.Forms.Padding(0);
            this.Box06.Name = "Box06";
            this.Box06.Size = new System.Drawing.Size(93, 95);
            this.Box06.TabIndex = 30;
            this.Box06.UseVisualStyleBackColor = false;
            this.Box06.Click += new System.EventHandler(this.Box06_Click);
            // 
            // Box05
            // 
            this.Box05.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Box05.BackColor = System.Drawing.Color.Black;
            this.Box05.BackgroundImage = global::BoggleClient.Properties.Resources.cube;
            this.Box05.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.Box05.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Box05.Font = new System.Drawing.Font("Arial Rounded MT Bold", 25.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Box05.Location = new System.Drawing.Point(24, 155);
            this.Box05.Margin = new System.Windows.Forms.Padding(0);
            this.Box05.Name = "Box05";
            this.Box05.Size = new System.Drawing.Size(93, 95);
            this.Box05.TabIndex = 29;
            this.Box05.UseVisualStyleBackColor = false;
            this.Box05.Click += new System.EventHandler(this.Box05_Click);
            // 
            // Box04
            // 
            this.Box04.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Box04.BackColor = System.Drawing.Color.Black;
            this.Box04.BackgroundImage = global::BoggleClient.Properties.Resources.cube;
            this.Box04.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.Box04.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Box04.Font = new System.Drawing.Font("Arial Rounded MT Bold", 25.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Box04.Location = new System.Drawing.Point(447, 20);
            this.Box04.Margin = new System.Windows.Forms.Padding(0);
            this.Box04.Name = "Box04";
            this.Box04.Size = new System.Drawing.Size(93, 95);
            this.Box04.TabIndex = 28;
            this.Box04.UseVisualStyleBackColor = false;
            this.Box04.Click += new System.EventHandler(this.Box04_Click);
            // 
            // Box03
            // 
            this.Box03.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Box03.BackColor = System.Drawing.Color.Black;
            this.Box03.BackgroundImage = global::BoggleClient.Properties.Resources.cube;
            this.Box03.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.Box03.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Box03.Font = new System.Drawing.Font("Arial Rounded MT Bold", 25.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Box03.Location = new System.Drawing.Point(306, 20);
            this.Box03.Margin = new System.Windows.Forms.Padding(0);
            this.Box03.Name = "Box03";
            this.Box03.Size = new System.Drawing.Size(93, 95);
            this.Box03.TabIndex = 27;
            this.Box03.UseVisualStyleBackColor = false;
            this.Box03.Click += new System.EventHandler(this.Box03_Click);
            // 
            // Box02
            // 
            this.Box02.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Box02.BackColor = System.Drawing.Color.Black;
            this.Box02.BackgroundImage = global::BoggleClient.Properties.Resources.cube;
            this.Box02.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.Box02.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Box02.Font = new System.Drawing.Font("Arial Rounded MT Bold", 25.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Box02.Location = new System.Drawing.Point(165, 20);
            this.Box02.Margin = new System.Windows.Forms.Padding(0);
            this.Box02.Name = "Box02";
            this.Box02.Size = new System.Drawing.Size(93, 95);
            this.Box02.TabIndex = 26;
            this.Box02.UseVisualStyleBackColor = false;
            this.Box02.Click += new System.EventHandler(this.Box02_Click);
            // 
            // Box01
            // 
            this.Box01.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Box01.BackColor = System.Drawing.Color.Black;
            this.Box01.BackgroundImage = global::BoggleClient.Properties.Resources.cube;
            this.Box01.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.Box01.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Box01.Font = new System.Drawing.Font("Arial Rounded MT Bold", 25.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Box01.Location = new System.Drawing.Point(24, 20);
            this.Box01.Margin = new System.Windows.Forms.Padding(0);
            this.Box01.Name = "Box01";
            this.Box01.Size = new System.Drawing.Size(93, 95);
            this.Box01.TabIndex = 25;
            this.Box01.UseVisualStyleBackColor = false;
            this.Box01.Click += new System.EventHandler(this.Box01_Click);
            // 
            // CancelWordButton
            // 
            this.CancelWordButton.Enabled = false;
            this.CancelWordButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CancelWordButton.Location = new System.Drawing.Point(917, 506);
            this.CancelWordButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.CancelWordButton.Name = "CancelWordButton";
            this.CancelWordButton.Size = new System.Drawing.Size(165, 60);
            this.CancelWordButton.TabIndex = 41;
            this.CancelWordButton.Text = "Cancel Word";
            this.CancelWordButton.UseVisualStyleBackColor = true;
            this.CancelWordButton.Click += new System.EventHandler(this.CancelWordButton_Click);
            // 
            // ExitGameButton
            // 
            this.ExitGameButton.BackColor = System.Drawing.Color.Crimson;
            this.ExitGameButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ExitGameButton.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.ExitGameButton.Location = new System.Drawing.Point(760, 615);
            this.ExitGameButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ExitGameButton.Name = "ExitGameButton";
            this.ExitGameButton.Size = new System.Drawing.Size(261, 66);
            this.ExitGameButton.TabIndex = 42;
            this.ExitGameButton.Text = "Disconnect";
            this.ExitGameButton.UseVisualStyleBackColor = false;
            this.ExitGameButton.Click += new System.EventHandler(this.ExitGameButton_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel1.BackgroundImage = global::BoggleClient.Properties.Resources.blankBoard;
            this.tableLayoutPanel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.Controls.Add(this.Box02, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.Box03, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.Box16, 3, 3);
            this.tableLayoutPanel1.Controls.Add(this.Box04, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.Box15, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.Box05, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.Box14, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.Box06, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.Box13, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.Box07, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.Box12, 3, 2);
            this.tableLayoutPanel1.Controls.Add(this.Box08, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.Box11, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.Box09, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.Box10, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.Box01, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(31, 153);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(564, 540);
            this.tableLayoutPanel1.TabIndex = 43;
            // 
            // GameForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(223)))), ((int)(((byte)(221)))));
            this.BackgroundImage = global::BoggleClient.Properties.Resources.grunge_halftone_background;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1160, 729);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.ExitGameButton);
            this.Controls.Add(this.CancelWordButton);
            this.Controls.Add(this.PlayButton);
            this.Controls.Add(this.WordBox);
            this.Controls.Add(this.P2ScoreLabel);
            this.Controls.Add(this.P1ScoreLabel);
            this.Controls.Add(this.P2ScoreBox);
            this.Controls.Add(this.P1ScoreBox);
            this.Controls.Add(this.TimeLabel);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "GameForm";
            this.Text = "GameForm";
            this.Load += new System.EventHandler(this.GameForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GameForm_KeyDown);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label TimeLabel;
        private System.Windows.Forms.TextBox P1ScoreBox;
        private System.Windows.Forms.TextBox P2ScoreBox;
        private System.Windows.Forms.Label P1ScoreLabel;
        private System.Windows.Forms.Label P2ScoreLabel;
        private System.Windows.Forms.TextBox WordBox;
        private System.Windows.Forms.Button PlayButton;
        private System.Windows.Forms.Button Box01;
        private System.Windows.Forms.Button Box02;
        private System.Windows.Forms.Button Box03;
        private System.Windows.Forms.Button Box04;
        private System.Windows.Forms.Button Box05;
        private System.Windows.Forms.Button Box06;
        private System.Windows.Forms.Button Box07;
        private System.Windows.Forms.Button Box08;
        private System.Windows.Forms.Button Box09;
        private System.Windows.Forms.Button Box10;
        private System.Windows.Forms.Button Box11;
        private System.Windows.Forms.Button Box12;
        private System.Windows.Forms.Button Box13;
        private System.Windows.Forms.Button Box14;
        private System.Windows.Forms.Button Box15;
        private System.Windows.Forms.Button Box16;
        private System.Windows.Forms.Button CancelWordButton;
        private System.Windows.Forms.Button ExitGameButton;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}