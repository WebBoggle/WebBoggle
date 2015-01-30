using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BoggleClient
{
    /// <summary>
    /// A form which displays the post-game summary
    /// </summary>
    public partial class Summary : Form
    {
        private String summaryMessage;
        private Dictionary<int, List<String>> Lists;
        private String PlayerName;
        private String OppName;
        string P1VW = "";
        string P2VW = "";
        string Common = "";
        string P1IW = "";
        string P2IW = "";
        String server;
        
        /// <summary>
        /// Constructs a new Summary Form
        /// </summary>
        public Summary(String _summaryMessage, String _PlayerName, String _OppName, String P1Score, String P2Score, String _server)
        {
            InitializeComponent();
            summaryMessage = _summaryMessage;
            PlayerName = _PlayerName;
            OppName = _OppName;

            Lists = new Dictionary<int, List<String>>();
            Lists.Add(0, new List<String>());
            Lists.Add(1, new List<String>());
            Lists.Add(2, new List<String>());
            Lists.Add(3, new List<String>());
            Lists.Add(4, new List<String>());

            P1ScoreLabel.Text = PlayerName;
            P1ScoreBox.Text = P1Score;
            P2ScoreLabel.Text = OppName;
            P2ScoreBox.Text = P2Score;

            server = _server;

            ParseSummary();
            GenerateHTML();
            ShowHTML();
        }

        /// <summary>
        /// Parses the summary and breaks it up into five lists
        /// </summary>
        private void ParseSummary()
        {
            String[] tokens = summaryMessage.Split();

            int listIndex = 0;

            for (int i = 0; i < tokens.Length; i++)
            {
                if (tokens[i] == "")
                    continue;
                int count;

                if (Int32.TryParse(tokens[i], out count))
                {
                    for (int j = i + 1; j < i + 1 + count; j++)
                    {
                        Lists[listIndex].Add(tokens[j]);
                    }
                    i += count;
                }
                listIndex++;
            }
        }

        /// <summary>
        /// Generates the HTML strings to be displayed
        /// </summary>
        private void GenerateHTML()
        {
            foreach (string s in Lists[0])
            {
                P1VW += s + "<br>";
            }
            foreach (string s in Lists[1])
            {
                P2VW += s + "<br>";
            }
            foreach (string s in Lists[2])
            {
                Common += s + "<br>";
            }
            foreach (string s in Lists[3])
            {
                P1IW += s + "<br>";
            }
            foreach (string s in Lists[4])
            {
                P2IW += s + "<br>";
            }
        }

        /// <summary>
        /// Displays the HTML lists in the web browser controls
        /// </summary>
        private void ShowHTML()
        {
            List1Browser.DocumentText = "<HTML><BODY><B>" + PlayerName + "'s Valid Words:</B><BR><HR>" + P1VW + "</HTML></BODY>";
            List2Browser.DocumentText = "<HTML><BODY><B>" + OppName + "'s Valid Words:</B><BR><HR>" + P2VW + "</HTML></BODY>";
            List3Browser.DocumentText = "<HTML><BODY><B>Common Words:</B><BR><HR>" + Common + "</HTML></BODY>";
            List4Browser.DocumentText = "<HTML><BODY><B>" + PlayerName + "'s Invalid Words:</B><BR><HR>" + P1IW + "</HTML></BODY>";
            List5Browser.DocumentText = "<HTML><BODY><B>" + OppName + "'s Invalid Words:</B><BR><HR>" + P2IW + "</HTML></BODY>";
        }

        /// <summary>
        /// An event handler for the "No" button.
        /// </summary>
        private void DontPlayAgain_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void PlayAgainButton_Click(object sender, EventArgs e)
        {
           Prompt newPrompt = new Prompt(PlayerName, server);
           this.Hide();
           newPrompt.ShowDialog();
           this.Dispose();
           
        }
    }
}
