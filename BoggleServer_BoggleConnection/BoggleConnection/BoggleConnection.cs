using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomNetworking;
using System.Net.Sockets;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using GameUpdate;
namespace Boggle
{
    public class BoggleConnectionException : Exception
    {
    }

    public class BoggleConnection
    {
        //Event handler delegates
        public delegate void TimeChangedHandler(BoggleConnection conn);
        public delegate void GameStartedHandler(BoggleConnection conn);
        public delegate void ScoreChangedHandler(BoggleConnection conn);
        public delegate void GameTerminatedHandler(BoggleConnection conn);
        public delegate void ServerLostHandler(BoggleConnection conn);
        public delegate void GameEndedHandler(BoggleConnection conn);

        //Member variables
        public string Score { get; private set; }
        public String oppName { get; private set; }
        public String TimeLeft { get; private set; }
        public String BoardLetters { get; private set; }
        private StringSocket socket;
        String Name;
        public String Summary { get; private set; }
        private readonly object ScoreLock;
        private readonly object TimeLock;
        public bool gameTerminated { get; private set; } 

        public String Ip { get; private set; }

        /// <summary>
        /// Constructs a new Boggle Connection which will attempt to connect to a Boggle Server at the 
        /// provided IP address.  The name parameter is the player's name.
        /// </summary>
        public BoggleConnection(String _IPAddress, String _name)
        {
            gameTerminated = false;
            try
            {
                TcpClient client = new TcpClient(_IPAddress, 2000);
                socket = new StringSocket(client.Client, new UTF8Encoding());
            }
            catch (Exception)
            {
                throw new BoggleConnectionException();
            }

            Name = _name;
            ScoreLock = new Object();
            TimeLock = new Object();
            Ip = _IPAddress;
        }

        /// <summary>
        /// Notifies the Boggle Server that this player is ready to play
        /// </summary>
        public void Connect()
        {
            socket.BeginReceive(IncomingMessage, Name);
            socket.BeginSend("PLAY " + Name + "\n", (e, o) => { }, null);
        }

        //called from web server to request a game udpate
        public GameUpdate.GameUpdate RequestUpdate ()
        {
            TcpClient sender = new TcpClient();
            sender.Connect("localhost", 2000);
            return new GameUpdate.GameUpdate();
        }
        /// <summary>
        /// Disconnects this Boggle Connection
        /// </summary>
        public void Disconnect()
        {
        
            socket.Close();
            
        }

        
        /// <summary>
        /// Sends the provided word to the Boggle Server
        /// </summary>
        public void SendWord(String word)
        {
            socket.BeginSend("WORD " + word + "\n", (e, o) => { }, null);
        }

        /// <summary>
        /// A ReceiveCallback for processing communications received from the Boggle Server
        /// </summary>
        private void IncomingMessage(string s, Exception e, object payload)
        {
            if (s == null)
            {
                if (!gameTerminated)
                OnServerLost(this);
                return;
            }

            string command = s.ToUpper().Trim();

            if (Regex.IsMatch(command, "^(START)"))
            {
                BoardLetters = command.Substring(6, 16);
                oppName = command.Substring(command.LastIndexOf(" ")+1).ToUpper();
                OnGameStarted(this);
            }

            else if (Regex.IsMatch(command, "^(TIME)"))
            {
                lock (TimeLock)
                {
                    TimeLeft = command.Substring(5);
                    OnTimeChanged(this);
                }
            }

            else if (Regex.IsMatch(command, "^(SCORE)"))
            {
                lock (ScoreLock)
                {
                    Score = command.Substring(6);
                    OnScoreChanged(this);
                }
            }

            else if (Regex.IsMatch(command, "^(STOP)"))
            {
                Summary = command.Substring(5);
                OnGameEnded(this);
            }

            else if (Regex.IsMatch(command, "^(TERMINATED)"))
            {
                gameTerminated = true;
                OnGameTerminated(this);
            }

            socket.BeginReceive(IncomingMessage, Name);
        }

        /// <summary>
        /// And event handler for TimeChanged events
        /// </summary>
        public event TimeChangedHandler TimeChanged;

        /// <summary>
        /// Calls any methods which have registered to be notified of TimeChanged events
        /// </summary>
        protected virtual void OnTimeChanged(BoggleConnection conn)
        {
            if (TimeChanged != null)
                TimeChanged(conn);
        }

        /// <summary>
        /// And event handler for GameStarted events
        /// </summary>
        public event GameStartedHandler GameStarted;

        /// <summary>
        /// Calls any methods which have registered to be notified of GameStarted events
        /// </summary>
        protected virtual void OnGameStarted(BoggleConnection conn)
        {
            if (GameStarted != null)
                GameStarted(conn);
        }

        /// <summary>
        /// And event handler for ScoreChanged eventsonServerLost
        /// </summary>
        public event ScoreChangedHandler ScoreChanged;

        /// <summary>
        /// Calls any methods which have registered to be notified of ScoreChanged events
        /// </summary>
        protected virtual void OnScoreChanged(BoggleConnection conn)
        {
            if (ScoreChanged != null)
                ScoreChanged(conn);
        }

        public event GameTerminatedHandler GameTerminated;

        protected virtual void OnGameTerminated(BoggleConnection conn)
        {
            if (GameTerminated != null)
                GameTerminated(conn);
        }

        public event ServerLostHandler ServerLost;

        protected virtual void OnServerLost(BoggleConnection conn)
        {
            if (ServerLost != null)
                ServerLost(conn);
        }

        public event GameEndedHandler GameEnded;

        protected virtual void OnGameEnded(BoggleConnection conn)
        {
            if (GameEnded != null)
                GameEnded(conn);
        }

    }
}
