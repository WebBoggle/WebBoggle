using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Boggle;
using System.Diagnostics;
using System.Drawing.Text;

namespace BoggleClient
{
    public partial class GameForm : Form
    {
        BoggleConnection conn;
        private String name;
        private String oppName;
        private readonly object scoreLock = new object();
        Dictionary<int, Button> BoardButtons;
        private bool ButtonsUsed;
        private bool GameStarted;

        /// <summary>
        /// Builds a new Boggle Game form - the main component of the BoggleClient GUI
        /// </summary>
        /// <param name="_conn"> The player's Boggle Connection</param>
        /// <param name="_name">The player's name</param>
        public GameForm(BoggleConnection _conn, String _name)
        {
            InitializeComponent();
            conn = _conn;
            //register the event handlers
            conn.GameStarted += StartReceived;
            conn.GameEnded += StopReceived;
            conn.GameTerminated += TerminateReceived;
            conn.ScoreChanged += ScoreReceived;
            conn.TimeChanged += TimeReceived;
            conn.ServerLost += LostServer;

            //normalize player name for nice display
            _name = _name.ToLower();
            name = _name[0].ToString().ToUpper() + _name.Substring(1);

            //update labels, initialize board, set up buttons
            P1ScoreLabel.Text = name;
            P2ScoreLabel.Text = "Waiting...";
            this.FormClosing += ExitApplication;
            BoardButtons = new Dictionary<int, Button>();
            FillBoardButtons();
            ButtonsUsed = false;
            GameStarted = false;
            WordBox.Focus();
            this.AcceptButton = PlayButton;
        }
        /// <summary>
        /// Adds each letter from the board state to the dictionary of board buttons
        /// </summary>
        private void FillBoardButtons()
        {
            BoardButtons.Add(1, Box01);
            BoardButtons.Add(2, Box02);
            BoardButtons.Add(3, Box03);
            BoardButtons.Add(4, Box04);
            BoardButtons.Add(5, Box05);
            BoardButtons.Add(6, Box06);
            BoardButtons.Add(7, Box07);
            BoardButtons.Add(8, Box08);
            BoardButtons.Add(9, Box09);
            BoardButtons.Add(10, Box10);
            BoardButtons.Add(11, Box11);
            BoardButtons.Add(12, Box12);
            BoardButtons.Add(13, Box13);
            BoardButtons.Add(14, Box14);
            BoardButtons.Add(15, Box15);
            BoardButtons.Add(16, Box16);
        }
         
        /// <summary>
        /// Event handler for when the 'START' message is received
        /// </summary>
        /// <param name="conn"></param>
        private void StartReceived(BoggleConnection conn) 
        {
            try { Invoke((Action)(() => { startTheGame(conn); })); }
            catch (ObjectDisposedException) { }
        }

        /// <summary>
        /// Event handler for when the 'STOP' message is received
        /// </summary>
        /// <param name="conn"></param>
        private void StopReceived(BoggleConnection conn) 
        {
            try { Invoke((Action)(() => { EndTheGame(conn); })); }
            catch (ObjectDisposedException) { }
        }

        /// <summary>
        /// Event handler for when the 'TERMINATE' message is recieved
        /// </summary>
        /// <param name="conn"></param>
        private void TerminateReceived(BoggleConnection conn) 
        {
            try { Invoke((Action)(() => { TerminateTheGame(conn, oppName); })); }
            catch (ObjectDisposedException) { }
        }
        /// <summary>
        /// Event handler for when the score changes via 'SCORE' message 
        /// </summary>
        /// <param name="conn"></param>
        private void ScoreReceived(BoggleConnection conn) 
        {
            try { Invoke((Action)(() => { UpdateScore(conn); })); }
            catch (ObjectDisposedException) { }
        }

        /// <summary>
        /// Event handler for when the time changes via 'TIME' message
        /// </summary>
        /// <param name="conn"></param>
        private void TimeReceived(BoggleConnection conn) 
        {
            try { Invoke((Action)(() => { UpdateTime(conn); })); }
            catch (ObjectDisposedException) { }
        }

        /// <summary>
        /// Event handler for when the server is lost or unavailable
        /// </summary>
        /// <param name="conn"></param>
        private void LostServer(BoggleConnection conn) 
        {
            try { Invoke((Action)(() => { HandleLostServer(conn); })); }
            catch (ObjectDisposedException) { }
        }
        /// <summary>
        /// Notifies the server that player is ready to play. Loads in custom clock font. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GameForm_Load(object sender, EventArgs e)
        {
            conn.Connect();
            PrivateFontCollection pfc = new PrivateFontCollection();
            pfc.AddFontFile("..\\..\\..\\DS-DIGI.ttf");
            TimeLabel.Font = new Font(pfc.Families[0], 45, FontStyle.Regular);
        }
        /// <summary>
        /// Event handler for when player chooses to exit the application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExitApplication(object sender, FormClosingEventArgs e)
        {
            Debug.WriteLine("Form closing event fired.");
            conn.Disconnect();
            Application.Exit();
        }
        /// <summary>
        /// Does as the name implies. Populates the board with letters, displays opponent name, prepares word box for word sending.
        /// </summary>
        /// <param name="conn"></param>
        private void startTheGame(BoggleConnection conn)
        {
            GameStarted = true;
            TimeLabel.Text = conn.TimeLeft;
            populateBoard(conn.BoardLetters);
            string opponentName = conn.oppName.ToLower();
            oppName = opponentName[0].ToString().ToUpper() + opponentName.Substring(1);
            P2ScoreLabel.Text = oppName;
            WordBox.Enabled = true;
        }

        /// <summary>
        /// Populates the boggle board with letters from the dictionary. 'Qu is displayed instead of 'Q'.
        /// </summary>
        /// <param name="letters"></param>
        private void populateBoard(string letters)
        {
            letters = letters.ToUpper();

            for (int i = 0; i < 16; i++)
            {
                if (letters[i] == 'Q')
                    BoardButtons[i+1].Text = "Qu";
                else
                    BoardButtons[i+1].Text = letters[i].ToString();
            }
        }

        /// <summary>
        /// Event handler for when the text in the word box is changed. Enables play button if word box isn't empty. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WordBox_TextChanged(object sender, EventArgs e)
        {
            if (WordBox.Text != "")
            {
                PlayButton.Enabled = true;
                CancelWordButton.Enabled = true;
            }
            else
            {
                PlayButton.Enabled = false;
                CancelWordButton.Enabled = false;
            }
        }

        /// <summary>
        /// Does as the name implies. Disconnects the BoggleConnection, hides the current window, shows the summary. 
        /// </summary>
        /// <param name="conn"></param>
        private void EndTheGame(BoggleConnection conn)
        {
            Summary summary = new Summary(conn.Summary, name, oppName, P1ScoreBox.Text, P2ScoreBox.Text, conn.Ip);
            conn.Disconnect();
            this.Hide();
            summary.ShowDialog();
        }

        /// <summary>
        /// Called when one of the players terminates or becomes disconnected in the middle of the game
        /// </summary>
        /// <param name="conn"></param>
        private void TerminateTheGame(BoggleConnection conn, String reason)
        {
            DialogResult result = MessageBox.Show(reason+ " disconnected. Play again?", "Game Terminated", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);

            if (result == DialogResult.Yes)
            {
                this.Dispose();
                this.Hide();
                Prompt replay = new Prompt(name, conn.Ip);
                replay.ShowDialog();
                conn.Disconnect();
            }
            else
            {
                conn.Disconnect();
                this.Close();
            }
        }

        /// <summary>
        /// Helper method to update scores after an event handler is notified to do so. 
        /// </summary>
        /// <param name="conn"></param>
        private void UpdateScore(BoggleConnection conn)
        {
            String[] scores = conn.Score.Split(' ');
            lock (scoreLock)
            {
                P1ScoreBox.Text = scores[0];
                P2ScoreBox.Text = scores[1];
            }
        }

        /// <summary>
        /// Helper method to update time remaining after an event handler is notified to do so. 
        /// </summary>
        /// <param name="conn"></param>
        private void UpdateTime(BoggleConnection conn)
        {
            int sec;
            Int32.TryParse(conn.TimeLeft, out sec);
            TimeSpan timeLeft = new TimeSpan(0, 0, sec);

            TimeLabel.Text = String.Format("{0:D2}:{1:D2}", timeLeft.Minutes, timeLeft.Seconds);
        }
        /// <summary>
        /// helper method to terminate game
        /// </summary>
        /// <param name="conn"></param>
        private void HandleLostServer(BoggleConnection conn)
        {
            TerminateTheGame(conn, "The Server");
        }

        /// <summary>
        /// Event handler for when the play button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlayButton_Click(object sender, EventArgs e)
        {
            conn.SendWord(WordBox.Text);
            WordBox.Text = "";
            if (ButtonsUsed)
            {
                ResetButtons();
            }
        }

        /// <summary>
        /// Deselects the buttons that were used to enter the word, clears out word box. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelWordButton_Click(object sender, EventArgs e)
        {
            if (ButtonsUsed)
                ResetButtons();

            WordBox.Text = "";
        }

        /// <summary>
        /// resets the board buttons to default state
        /// </summary>
        private void ResetButtons()
        {
            for (int i = 1; i < 17; i++)
            {
                BoardButtons[i].BackgroundImage = (System.Drawing.Image)Properties.Resources.cube;
                BoardButtons[i].Enabled = true;
            }
            ButtonsUsed = false;
        }

        /// <summary>
        /// Event handler for when keydown is pressed with focus on the game form. Relevant for enter key. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GameForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                PlayButton.PerformClick();
            }
        }

        /// <summary>
        /// Event handler for when [methodname]'s letter box is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Box01_Click(object sender, EventArgs e)
        {
            BoardButtonPressed(1);
        }

        /// <summary>
        /// Event handler for when [methodname]'s letter box is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Box02_Click(object sender, EventArgs e)
        {
            BoardButtonPressed(2);
        }

        /// <summary>
        /// Event handler for when [methodname]'s letter box is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Box03_Click(object sender, EventArgs e)
        {
            BoardButtonPressed(3);
        }

        /// <summary>
        /// Event handler for when [methodname]'s letter box is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Box04_Click(object sender, EventArgs e)
        {
            BoardButtonPressed(4);
        }
        /// <summary>
        /// Event handler for when [methodname]'s letter box is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Box05_Click(object sender, EventArgs e)
        {
            BoardButtonPressed(5);
        }
        /// <summary>
        /// Event handler for when [methodname]'s letter box is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Box06_Click(object sender, EventArgs e)
        {
            BoardButtonPressed(6);
        }
        /// <summary>
        /// Event handler for when [methodname]'s letter box is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Box07_Click(object sender, EventArgs e)
        {
            BoardButtonPressed(7);
        }
        /// <summary>
        /// Event handler for when [methodname]'s letter box is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Box08_Click(object sender, EventArgs e)
        {
            BoardButtonPressed(8);
        }
        /// <summary>
        /// Event handler for when [methodname]'s letter box is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Box09_Click(object sender, EventArgs e)
        {
            BoardButtonPressed(9);
        }
        /// <summary>
        /// Event handler for when [methodname]'s letter box is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Box10_Click(object sender, EventArgs e)
        {
            BoardButtonPressed(10);
        }
        /// <summary>
        /// Event handler for when [methodname]'s letter box is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Box11_Click(object sender, EventArgs e)
        {
            BoardButtonPressed(11);
        }
        /// <summary>
        /// Event handler for when [methodname]'s letter box is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Box12_Click(object sender, EventArgs e)
        {
            BoardButtonPressed(12);
        }
        /// <summary>
        /// Event handler for when [methodname]'s letter box is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Box13_Click(object sender, EventArgs e)
        {
            BoardButtonPressed(13);
        }
        /// <summary>
        /// Event handler for when [methodname]'s letter box is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Box14_Click(object sender, EventArgs e)
        {
            BoardButtonPressed(14);
        }
        /// <summary>
        /// Event handler for when [methodname]'s letter box is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Box15_Click(object sender, EventArgs e)
        {
            BoardButtonPressed(15);
        }
        /// <summary>
        /// Event handler for when [methodname]'s letter box is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Box16_Click(object sender, EventArgs e)
        {
            BoardButtonPressed(16);            
        }
      
        /// <summary>
        /// When a letter box is clicked we change the image to be darker and append that box's label's text to the word box to form a word
        /// </summary>
        /// <param name="i"></param>
        private void BoardButtonPressed(int i)
        {
            WordBox.Focus();
            BoardButtons[i].BackgroundImage = (System.Drawing.Image)Properties.Resources.selectedCube;
            BoardButtons[i].Enabled = false;
            WordBox.Text += BoardButtons[i].Text.ToUpper();
            ButtonsUsed = true;
        }

        /// <summary>
        /// Event handler for when player chooses to exit the game. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExitGameButton_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to disconnect and close the program?", "Disconnect?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                this.Close();
            }
        }
    }
}
