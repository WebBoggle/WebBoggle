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
using CustomNetworking;

namespace BoggleClient
{
    public partial class Prompt : Form
    {
        private bool IPAddressEntered = false;
        private bool NameEntered = false;
        BoggleConnection conn;

        public Prompt()
        {
            InitializeComponent();
        }

        public Prompt(String name, String server)
        {
            InitializeComponent();
            IPAddrBox.Text = server;
            NameBox.Text = name;

            IPAddrLabel.Text = "Click Play to use the same Boggle server, or enter new info below.";
        }

        private void Prompt_Load(object sender, EventArgs e)
        {
            this.AcceptButton = PlayButton;
        }

        private void IPAddrBox_TextChanged(object sender, EventArgs e)
        {
            if (IPAddrBox.Text != "")
                IPAddressEntered = true;
            else
                IPAddressEntered = false;
            updatePlayButton();
        }

        private void NameBox_TextChanged(object sender, EventArgs e)
        {
            if (NameBox.Text != "")
                NameEntered = true;
            else
                NameEntered = false;
            updatePlayButton();
        }

        private void updatePlayButton()
        {
            if (IPAddressEntered && NameEntered)
                PlayButton.Enabled = true;
            else
                PlayButton.Enabled = false;
        }

        private void PlayButton_Click(object sender, EventArgs e)
        {
            try
            {
                conn = new BoggleConnection(IPAddrBox.Text, NameBox.Text);
                GameForm game = new GameForm(conn, NameBox.Text);
                this.Hide();
                game.ShowDialog();
            }
            catch (BoggleConnectionException)
            {
                MessageBox.Show("Could not connect to a Boggle Server at the specified IP Address","Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                IPAddrBox.Text = "";
                updatePlayButton();
            }
        }
      
    }
}
