using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using CustomNetworking;
using BB;
using System.IO;
using System.Timers;
using System.Threading;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;

namespace BoggleServer
{
    /// <summary>
    /// A server program that connects two Boggle game playing clients together.
    /// BoggleServer accepts connection requests from clients on port 2000 and pairs them in Boggle matches. 
    /// The first two connection requests are paired, the next two are paired, and so on. BoggleServer is 
    /// responsible for coordinating all aspects of play: the pairings, the boards, the time limits, and the scoring. 
    /// It is possible for an arbitrary number of games to be ongoing concurrently.
    /// </summary>
    public class BoggleServer
    {
        /// <summary>
        /// A BoggleServer object used for testing the main method.
        /// </summary> 
        public static BoggleServer testServer { get; private set; }

        /// <summary>
        /// The connection string.
        /// Your CADE login name serves as both your database name and your uid
        /// Your uNID serves as your password
        /// </summary>
        public const string connectionString = "server=atr.eng.utah.edu;database=hshelton;uid=hshelton;password=300786954";

        /// <summary>
        /// When running the program the following values should be used:
        ///The number of seconds that each Boggle game should last. This should be a positive integer.
        ///The pathname of a file that contains all the legal words. The file should contain one word per line.
        ///An optional string consisting of exactly 16 letters. If provided, this will be used to initialize each Boggle board.
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            //check for errors with parameters
            if (args.Length < 2 || args.Length > 3)
            {
                Console.WriteLine("This program requires two parameters:  number of seconds the game should last, pathname of file containing legal words.");
                Console.WriteLine("An additional parameter containing the 16-Letter initialization state of the Boggle board may also be provided.");
                return;
            }
            //create a BoggleServer with a user specialized initial board state
            if (args.Length == 2)
            {
                int timeLimit;
                Int32.TryParse(args[0], out timeLimit);
                string filePath = args[1];
                string customBoard = "";
                testServer = new BoggleServer(timeLimit, filePath, customBoard);
                Console.Read();

            }
            //create a BoggleServer with a randomized initial board state
            if (args.Length == 3)
            {
                int timeLimit;
                Int32.TryParse(args[0], out timeLimit);
                string filePath = args[1];
                string customBoard = args[2];
                if (customBoard.Length != 16)
                {
                    Console.WriteLine("Invalid initial BoggleBoard state:  This parameter must be exactly 16 letters.");
                    return;
                }
                testServer = new BoggleServer(timeLimit, filePath, customBoard);
                Console.Read();

            }
        }

        // Member variables
        private int timeLimit;
        private string fileName;
        private string customBoard;
        private TcpListener server;
        private List<Tuple<StringSocket, String>> playersConnected;
        private TcpListener webServer;

        /// <summary>
        /// The set containing all of the legal words for Boggle.
        /// </summary>
        public HashSet<String> legalWords { get; private set; }

        /// <summary>
        /// Creates a new Boggle Server. The server listens for client connection requests on port 2000.
        /// When a connection has been established, the client sends a command to the server. 
        /// The command is "PLAY @", where @ is the name of the player.
        /// Once the server has received connections from two clients that are ready to play, it pairs them in a game. 
        /// The server begins the game by sending a command to each client. The command is "START $ # @", where $ is the 
        /// 16 characters that appear on the Boggle board being used for this game, # is the length of the game in seconds,
        /// and @ is the opponent's name. Once the players have been paired the communication is controlled by the BoggleGame.
        /// </summary>
        /// <param name="_timeLimit"></param>
        /// <param name="_fileName"></param>
        /// <param name="_customBoard"></param>
        public BoggleServer(int _timeLimit, string _fileName, string _customBoard)
        {
            timeLimit = _timeLimit;
            fileName = _fileName;
            customBoard = _customBoard;
            server = new TcpListener(IPAddress.Any, 2000);
            server.Start();
            server.BeginAcceptSocket(ConnectionReceived, null);
            playersConnected = new List<Tuple<StringSocket, String>>();
            legalWords = new HashSet<String>(StringComparer.OrdinalIgnoreCase);

            webServer = new TcpListener(IPAddress.Any, 2500);
            webServer.Start();
            webServer.BeginAcceptSocket(webConnectionReceived, null);

            parseFile(_fileName);
        }

        /// <summary>
        /// A callback for processing incoming webServer connections
        /// </summary>
        private void webConnectionReceived(IAsyncResult ar)
        {
            Socket socket = webServer.EndAcceptSocket(ar);
            StringSocket ss = new StringSocket(socket, UTF8Encoding.Default);
            ss.BeginReceive(webLineReceived, ss); // start receiving on the socket with ss as payload
            webServer.BeginAcceptSocket(webConnectionReceived, null);
        }

        /// <summary>
        /// A callback for processing incoming lines from webServer connections
        /// </summary>
        private void webLineReceived(string line, Exception e, object payload)
        {
            if (line == null)
                return;
            StringSocket ss = (StringSocket)payload;
            ss.BeginSend("HTTP/1.1 200 OK\r\n", (ee, oo) => { }, null);
            ss.BeginSend("Connection: close\r\n", (ee, oo) => { }, null);
            ss.BeginSend("Content-Type: text/html; charset=UTF-8\r\n", (ee, oo) => { }, null);
            ss.BeginSend("\r\n", (ee, oo) => { }, null);
            ss.BeginSend(buildHtml(line) + "\n", (ee, o) => { ss.Close(); }, null);
        }

        /// <summary>
        /// Queries the database for the requested information and populates tables in an
        /// HTML web page using the data from the database.  Returns the HTML page.
        /// </summary>
        private string buildHtml(string line)
        {
            line = line.ToUpper().Trim();
            String html = "<!DOCTYPE html><html><body>";
            String endHTML = "</body></html>";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    //GET /players HTTP/1.1
                    if (Regex.IsMatch(line, @"^GET /PLAYERS HTTP/1.1$"))
                    {
                        return buildPlayersHTML(html, endHTML, line, conn);
                    }

                   //GET /games?player=Joe HTTP/1.1
                    else if (Regex.IsMatch(line, @"^(GET /GAMES\?PLAYER=)([\s\S]+)( HTTP/1.1)$"))
                    {
                        return buildPlayerGamesHTML(html, endHTML, line, conn);
                    }

                   //GET /game?id=35 HTTP/1.1
                    else if (Regex.IsMatch(line, @"^(GET /GAME\?ID=)([0-9]+) (HTTP/1.1)$"))
                    {
                        return buildGameHTML(html, endHTML, line, conn);
                    }

                    // If the request does not match one of the three valid options, builds an HTML page explaining valid options.
                    else
                    {
                        string playersOption = "The server sends back a page containing a table of information." +
                            "There is a row for each player in the database and four columns. Each row consists of the player's name," +
                            "the number of games won by the player, the number of games lost by the player, and the number of games tied by the player." +
                            " Instead of \"ServerIPAddress\" substitute the IP address of the Boggle Server";

                        string gamesOption = "The server sends back a page containing a table of information. There is a row" +
                            "for each game played by the player named in the line of text (e.g., \"Joe\" in the example above) and five columns. Each row consists" +
                            "of a unique game ID, the date and time " +
                            "when the game was played, the name of the opponent, the score for the named player, and the score for the opponent." +
                            " Instead of \"ServerIPAddress\" substitute the IP address of the Boggle Server";

                        string gameOption = "The server sends back a page containing information about the specified game (e.g., 35 in the example above)." +
                            "The page shows the names and scores of the two players involved, the date and time when the game was played, a 4x4 table containing" +
                            "the Boggle board that was used, the time limit that was used for the game in minutes and seconds, and the lists of the words played by each player during the game." +
                            " Instead of \"ServerIPAddress\" substitute the IP address of the Boggle Server";

                        html += "<b>Error: invalid command. Choose from these options: </b>" + "<br>";
                        html += "<ul>" +
                                "<li> <font color = \"blue\"> http://ServerIPAddress:2500/players <br> </font>" +
                                playersOption + "</li>" + "<br>" +
                                "<li> <font color = \"blue\"> http://ServerIPAddress:2500/games?player=Joe </font><br>" +
                                 gamesOption + "</li>" + "<br>" +
                                "<li> <font color = \"blue\"> http://ServerIPAddress:2500/game?id=35 </font> <br>" +
                                gameOption + "</li> </ul>";

                        html += endHTML;
                    }
                }
                catch (Exception e)
                {
                    // Sends an error page back the the client explaining what went wrong
                    html = "<!DOCTYPE html><html><body>";
                    html += " <h3>Database Server is not available</h3>";
                    html += "<br>" + e.Message;
                    html += endHTML;

                    return html;
                }
                finally
                {
                    conn.Close();
                }
            }
            return html;
        }

        /// <summary>
        /// Builds the HTML page for requests matching:
        /// 
        /// GET /players HTTP/1.1
        /// </summary>
        private string buildPlayersHTML(string html, string endHTML, string line, MySqlConnection conn)
        {
            Dictionary<int, String> playerDictionary = new Dictionary<int, String>();
            Dictionary<int, playerGamesRecord> records = new Dictionary<int, playerGamesRecord>();

            MySqlCommand getPlayers = conn.CreateCommand();
            getPlayers.CommandText = @"SELECT * FROM hshelton.Players;";

            // Execute the command and cycle through the DataReader object
            using (MySqlDataReader reader = getPlayers.ExecuteReader())
            {
                int currentPlayerId = 0;
                string currentPlayerName;

                //build up dictionary of players where player id is key and player name = value
                while (reader.Read())
                {
                    Int32.TryParse(reader["ID"].ToString(), out currentPlayerId);
                    currentPlayerName = reader["NAME"].ToString();

                    if (currentPlayerId != 0)
                        playerDictionary.Add(currentPlayerId, currentPlayerName);
                }
            }

            MySqlCommand getScores = conn.CreateCommand();

            foreach (int i in playerDictionary.Keys)
            {
                getScores.CommandText = @"SELECT p1Score, p2Score from Games where p1Id = " + i + ";";

                using (MySqlDataReader reader = getScores.ExecuteReader())
                {
                    int won = 0;
                    int lost = 0;
                    int tied = 0;
                    int p1Score;
                    int p2Score;

                    //build up dictionary of players where player id is key and player name = value
                    while (reader.Read())
                    {
                        Int32.TryParse(reader["P1Score"].ToString(), out p1Score);
                        Int32.TryParse(reader["P2Score"].ToString(), out p2Score);

                        //Compares the scores to determine if the game was a win, loss, or tie
                        if (p1Score > p2Score)
                        {
                            won++;
                        }
                        else if (p1Score < p2Score)
                        {

                            lost++;
                        }
                        else
                        {
                            tied++;
                        }
                    }

                    if (!(won == 0 && lost == 0 && tied == 0))
                        records.Add(i, new playerGamesRecord(playerDictionary[i], won, lost, tied));
                    won = 0;
                    lost = 0;
                    tied = 0;
                }

                getScores.CommandText = @"SELECT p1Score, p2Score from Games where p2Id = " + i + ";";

                using (MySqlDataReader reader = getScores.ExecuteReader())
                {
                    int won = 0;
                    int lost = 0;
                    int tied = 0;
                    int p1Score;
                    int p2Score;

                    //build up dictionary of players where player id is key and player name = value
                    while (reader.Read())
                    {
                        Int32.TryParse(reader["P1Score"].ToString(), out p1Score);
                        Int32.TryParse(reader["P2Score"].ToString(), out p2Score);

                        //Compares the scores to determine if the game was a win, loss, or tie
                        if (p2Score > p1Score)
                        {
                            won++;
                        }
                        else if (p2Score < p1Score)
                        {

                            lost++;
                        }
                        else
                        {
                            tied++;
                        }
                    }

                    if (!(won == 0 && lost == 0 && tied == 0))
                    {
                        if (!records.ContainsKey(i))
                            records.Add(i, new playerGamesRecord(playerDictionary[i], won, lost, tied));
                        else
                        {
                            won += records[i].won;
                            lost += records[i].lost;
                            tied += records[i].tied;
                            records[i] = new playerGamesRecord(playerDictionary[i], won, lost, tied);
                        }
                    }
                    won = 0;
                    lost = 0;
                    tied = 0;
                }
            }

            // Build the HTML table
            html += "<table border=\"1\" cellpadding=\"5\"><tr><th colspan=\"4\">Players</th></tr>" +
                     "<tr><th> Player Name </th>" +
                     "<th> Games Won </th>" +
                     "<th> Games Lost </th>" +
                     "<th> Games Tied </th>";

            // Add a row to the table for each entry in the records dictionary
            foreach (int i in records.Keys)
            {
                html += "<tr><td>" + records[i].name + "</td><td>" + records[i].won + "</td><td>" + records[i].lost + "</td><td>" + records[i].tied + "</td></tr>";
            }
            html += "</tr></table>" + endHTML;
            return html;
        }

        /// <summary>
        /// Builds the HTML page for requests matching
        /// 
        /// GET /games?player=Joe HTTP/1.1
        /// </summary>
        private string buildPlayerGamesHTML(string html, string endHTML, string line, MySqlConnection conn)
        {
            string playerName = line.Substring(line.IndexOf("=") + 1);
            playerName = playerName.Substring(0, playerName.Length - 9);
            MySqlCommand getID = conn.CreateCommand();
            getID.CommandText = @"SELECT ID FROM hshelton.Players where Name = '" + playerName + "';";
            int PlayerId = 0;

            // Execute the command and cycle through the DataReader object
            using (MySqlDataReader reader = getID.ExecuteReader())
            {
                //build up dictionary of players where player id is key and player name = value
                while (reader.Read())
                {
                    Int32.TryParse(reader["ID"].ToString(), out PlayerId);
                }

            }

            // If the specified player is not in the database, returns an HTML page informing the client.
            if (PlayerId == 0)
            {
                html += playerName + " is not in the database" + endHTML;
                conn.Close();
                return html;
            }

            MySqlCommand getGames = conn.CreateCommand();
            getGames.CommandText = @"Select * from hshelton.Games where P1Id = " + PlayerId + ";";

            int gameID = 0;
            string DateTime = "";
            int oppId = 0;
            int playerScore = 0;
            int oppScore = 0;
            List<GameInfo> gameInfoList = new List<GameInfo>();

            // Query the database for the game information for the specified player
            using (MySqlDataReader reader = getGames.ExecuteReader())
            {
                while (reader.Read())
                {
                    Int32.TryParse(reader["GameId"].ToString(), out gameID);
                    DateTime = reader["Date"].ToString();
                    Int32.TryParse(reader["P2Id"].ToString(), out oppId);
                    Int32.TryParse(reader["P1Score"].ToString(), out playerScore);
                    Int32.TryParse(reader["P2Score"].ToString(), out oppScore);

                    if (gameID != 0)
                        gameInfoList.Add(new GameInfo(gameID, PlayerId, oppId, DateTime, playerScore, oppScore));
                }
            }

            getGames.CommandText = @"Select * from hshelton.Games where P2Id = " + PlayerId + ";";
            gameID = 0;

            // Query the database for the game information for the specified player
            using (MySqlDataReader reader = getGames.ExecuteReader())
            {
                while (reader.Read())
                {
                    Int32.TryParse(reader["GameId"].ToString(), out gameID);
                    DateTime = reader["Date"].ToString();
                    Int32.TryParse(reader["P1Id"].ToString(), out oppId);
                    Int32.TryParse(reader["P2Score"].ToString(), out playerScore);
                    Int32.TryParse(reader["P1Score"].ToString(), out oppScore);

                    if (gameID != 0)
                        gameInfoList.Add(new GameInfo(gameID, PlayerId, oppId, DateTime, playerScore, oppScore));
                }
            }


            // Build the HTML to send back to the client
            html += "<p>" +
                "<table border = \"1\" cellpadding = \"5\">" +
                "<tr>" +
                "<th colspan = \"6\">Games played by " + playerName + "</th>" +
                "</tr>" +
                "<tr>" +
                "<th> Game ID </th>" +
                 "<th> Game Date/Time </th>" +
                "<th> Opponent Name </th>" +
                "<th>" + playerName + "\'s Score </th>" +
                "<th> Opponent's Score</th>";

            foreach (GameInfo g in gameInfoList)
            {
                html += "<tr> <td align = center>" + g.gameId + "</td>" + "<td align = center>" + g.DateTime + "</td>" + "<td align = center>" + GetNameFromID(g.oppId) + "</td>" +
                "<td align = center>" + g.playerScore + "</td>" + "<td align = center>" + g.oppScore + "</td>" + "</tr>";

            }

            html += "</table>" + endHTML;
            return html;
        }

        /// <summary>
        /// Builds the HTML page for requests matching
        /// 
        /// GET /game?id=35 HTTP/1.1
        /// </summary>
        private string buildGameHTML(string html, string endHTML, string line, MySqlConnection conn)
        {
            int P1ID = 0;
            int P2ID = 0;
            string DateTime = "";
            string BoardLetters = "";
            int TimeLimit = 0;
            int P1Score = 0;
            int P2Score = 0;

            string id = line.Substring(line.IndexOf("=") + 1);
            id = id.Substring(0, id.Length - 9);
            int GameID;
            Int32.TryParse(id, out GameID);

            MySqlCommand GetGame = conn.CreateCommand();
            GetGame.CommandText = @"Select * from Games where GameId = " + GameID + ";";

            // Query the database for information about the specified game.
            using (MySqlDataReader reader = GetGame.ExecuteReader())
            {
                while (reader.Read())
                {
                    Int32.TryParse(reader["P1Id"].ToString(), out P1ID);
                    Int32.TryParse(reader["P2Id"].ToString(), out P2ID);
                    DateTime = reader["Date"].ToString();
                    BoardLetters = reader["Board"].ToString();
                    Int32.TryParse(reader["TimeLimit"].ToString(), out TimeLimit);
                    Int32.TryParse(reader["P1Score"].ToString(), out P1Score);
                    Int32.TryParse(reader["P2Score"].ToString(), out P2Score);
                }
            }

            // If the specified game is not in the database, send back an HTML page informing the user.
            if (P1ID == 0)
            {
                html += "The game with ID " + GameID + " was not found in the database.";
                html += endHTML;
                conn.Close();
                return html;
            }

            // Get Player1's valid words
            MySqlCommand GetP1ValidWords = conn.CreateCommand();
            GetP1ValidWords.CommandText = @"Select Word from Words where GameId = " + GameID + " and PlayerId = " + P1ID + " and PlayerId2 = 0 and legal = 1;";

            List<String> P1VW = new List<String>();
            using (MySqlDataReader reader = GetP1ValidWords.ExecuteReader())
            {
                while (reader.Read())
                {
                    P1VW.Add(reader["Word"].ToString());
                }
            }

            // Get Player2's valid words
            MySqlCommand GetP2ValidWords = conn.CreateCommand();
            GetP2ValidWords.CommandText = @"Select Word from Words where GameId = " + GameID + " and PlayerId = " + P2ID + " and PlayerId2 = 0 and legal = 1;";

            List<String> P2VW = new List<String>();
            using (MySqlDataReader reader = GetP2ValidWords.ExecuteReader())
            {
                while (reader.Read())
                {
                    P2VW.Add(reader["Word"].ToString());
                }
            }

            // Get Common words
            MySqlCommand GetCommonWords = conn.CreateCommand();
            GetCommonWords.CommandText = @"Select Word from Words where GameId = " + GameID + " and PlayerId = " + P1ID + " and PlayerId2 = " + P2ID + " and legal = 1;";

            List<String> CommonWords = new List<String>();
            using (MySqlDataReader reader = GetCommonWords.ExecuteReader())
            {
                while (reader.Read())
                {
                    CommonWords.Add(reader["Word"].ToString());
                }
            }

            // Get Player1's invalid words
            MySqlCommand GetP1InvalidWords = conn.CreateCommand();
            GetP1InvalidWords.CommandText = @"Select Word from Words where GameId = " + GameID + " and PlayerId = " + P1ID + " and PlayerId2 = 0 and legal = 0;";

            List<String> P1IW = new List<String>();
            using (MySqlDataReader reader = GetP1InvalidWords.ExecuteReader())
            {
                while (reader.Read())
                {
                    P1IW.Add(reader["Word"].ToString());
                }
            }

            // Get Player2's invalid words
            MySqlCommand GetP2InvalidWords = conn.CreateCommand();
            GetP2InvalidWords.CommandText = @"Select Word from Words where GameId = " + GameID + " and PlayerId = " + P2ID + " and PlayerId2 = 0 and legal = 0;";

            List<String> P2IW = new List<String>();
            using (MySqlDataReader reader = GetP2InvalidWords.ExecuteReader())
            {
                while (reader.Read())
                {
                    P2IW.Add(reader["Word"].ToString());
                }
            }

            // Build the HTML page to send back to the client
            String P1Name = GetNameFromID(P1ID);
            String P2Name = GetNameFromID(P2ID);
            int Minutes = TimeLimit / 60;
            int Sec = TimeLimit % 60;
            string Seconds;
            if (Sec < 10)
                Seconds = "0" + Sec;
            else
                Seconds = "" + Sec;

            html += "<h3>Game " + GameID + " Summary</h3>";
            html += P1Name + " vs " + P2Name + "<br>";
            html += DateTime + "<br>";
            html += "Game Time Limit: " + Minutes + ":" + Seconds;
            html += "<p><table border = \"1\"><tr><th colspan = \"4\"> Score </th></tr>";
            html += "<tr><td align=center>" + P1Name + "</td><td align=center>" + P2Name + "</td></tr>";
            html += "<tr><td align=center>" + P1Score + "</td><td align=center>" + P2Score + "</td></tr></table></p>";

            html += "<p><table border = \"1\" width = \"200\"><tr><th colspan = \"4\"> Board </th></tr>";
            html += "<tr><td align=center>" + BoardLetters[0] + "</td><td align=center>" + BoardLetters[1] + "</td><td align=center>" + BoardLetters[2] + "</td><td align=center>" + BoardLetters[3] + "</td align=center></tr>";
            html += "<tr><td align=center>" + BoardLetters[4] + "</td><td align=center>" + BoardLetters[5] + "</td><td align=center>" + BoardLetters[6] + "</td><td align=center>" + BoardLetters[7] + "</td align=center></tr>";
            html += "<tr><td align=center>" + BoardLetters[8] + "</td><td align=center>" + BoardLetters[9] + "</td><td align=center>" + BoardLetters[10] + "</td><td align=center>" + BoardLetters[11] + "</td align=center></tr>";
            html += "<tr><td align=center>" + BoardLetters[12] + "</td><td align=center>" + BoardLetters[13] + "</td><td align=center>" + BoardLetters[14] + "</td><td align=center>" + BoardLetters[15] + "</td align=center></tr><table></p>";

            html += "<p>" + P1Name + " played " + P1VW.Count + " legal word(s): <br><ul>";
            // Builds the list of player1's valid words
            foreach (string word in P1VW)
            {
                html += "<li>" + word + "</li>";
            }
            html += "</ul><p>";

            html += "<p>" + P2Name + " played " + P2VW.Count + " legal word(s): <br><ul>";
            // Builds the list of player2's valid words
            foreach (string word in P2VW)
            {
                html += "<li>" + word + "</li>";
            }
            html += "</ul><p>";

            html += "<p>There were " + CommonWords.Count + " common word(s): <br><ul>";

            // Builds the list of common words
            foreach (string word in CommonWords)
            {
                html += "<li>" + word + "</li>";
            }
            html += "</ul><p>";

            html += "<p>" + P1Name + " played " + P1IW.Count + " illegal word(s): <br><ul>";

            // Builds the list of player1's invalid words
            foreach (string word in P1IW)
            {
                html += "<li>" + word + "</li>";
            }
            html += "</ul><p>";

            html += "<p>" + P2Name + " played " + P2IW.Count + " illegal word(s): <br><ul>";

            // Builds the list of player2's invalid words
            foreach (string word in P2IW)
            {
                html += "<li>" + word + "</li>";
            }
            html += "</ul><p>";

            html += endHTML;
            return html;
        }

        /// <summary>
        /// Takes an integer ID as a parameter and queries the database to retrieve
        /// the player name that corresponds to that ID.
        /// </summary>
        private string GetNameFromID(int ID)
        {
            String name = "";
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                try
                {
                    MySqlCommand getName = conn.CreateCommand();
                    getName.CommandText = @"SELECT Name FROM hshelton.Players where ID = " + ID + ";";

                    // Execute the command and cycle through the DataReader object
                    using (MySqlDataReader reader = getName.ExecuteReader())
                    {
                        //build up dictionary of players where player id is key and player name = value
                        while (reader.Read())
                        {
                            name = reader["Name"].ToString();
                        }
                    }
                }
                finally
                {
                    conn.Close();
                }
            }
            return name;
        }

        /// <summary>
        /// Adds each line of text from a dictionary file to the set of legal words
        /// </summary>
        /// <param name="_fileName"></param>
        private void parseFile(string _fileName)
        {
            try
            {
                using (StreamReader sr = new StreamReader(_fileName))
                {
                    while (sr.Peek() >= 0)
                    {
                        legalWords.Add(sr.ReadLine());
                    }
                }
            }

            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
            }
        }

        /// <summary>
        /// Closes the Boggle Server.
        /// </summary>
        public void Close()
        {
            server.Stop();
        }

        /// <summary>
        /// Asynchronous callback for handling initial connections from clients.
        /// </summary>
        /// <param name="ar"></param>
        private void ConnectionReceived(IAsyncResult ar)
        {
            Console.WriteLine("Received a new connection...");
            Socket socket = server.EndAcceptSocket(ar);
            StringSocket ss = new StringSocket(socket, UTF8Encoding.Default);
            ss.BeginReceive(NameReceived, ss);
            server.BeginAcceptSocket(ConnectionReceived, null);
        }

        /// <summary>
        /// Callback for handling the first command from the client, which should be
        /// PLAY @ (where @ is the player's name) to signify they are ready to begin playing 
        /// </summary>
        private void NameReceived(string name, Exception e, object payload)
        {
            if (name == null)
            {
                StringSocket s = (StringSocket)payload;
                s.Close();
                return;
            }
            name = name.ToUpper().Trim();
            StringSocket ss = (StringSocket)payload;
            if (!name.isPLAY())
            {
                ss.BeginSend("IGNORING " + name + "\n", (ex, o) => { }, ss);
                ss.BeginReceive(NameReceived, ss);
            }
            else
            {
                lock (playersConnected)
                {
                    Console.WriteLine(name.Substring(5) + " is ready to play...");
                    playersConnected.Add(new Tuple<StringSocket, String>(ss, name.Substring(5).ToUpper()));
                    createPairs();
                }
            }
        }

        /// <summary>
        /// Pairs clients into a Boggle game any time there are at least two players who are ready to play.
        /// Creates a new BoggleGame for the players based on the specified parameters and removes them from the list of 
        /// players waiting to play. 
        /// </summary>
        private void createPairs()
        {
            if (playersConnected.Count < 2)
                return;
            else
            {
                lock (playersConnected)
                {
                    Tuple<StringSocket, string> p1 = playersConnected[0];
                    Tuple<StringSocket, string> p2 = playersConnected[1];
                    if (StillConnected(p1, p2))
                    {
                        playersConnected.RemoveAt(0);
                        playersConnected.RemoveAt(0);
                        Console.WriteLine(p1.Item2 + " & " + p2.Item2 + " have been paired up...");
                        BoggleGame game;
                        if (customBoard.Length == 0)
                        {
                            game = new BoggleGame(p1, p2, new BoggleBoard(), timeLimit, legalWords);
                        }
                        else
                        {
                            game = new BoggleGame(p1, p2, new BoggleBoard(customBoard), timeLimit, legalWords);
                        }
                        return;
                    }
                }
                createPairs();
            }
        }

        /// <summary>
        /// Returns true if both players about to be paired up are still connected.
        /// </summary>
        private bool StillConnected(Tuple<StringSocket, string> p1, Tuple<StringSocket, string> p2)
        {
            if (!p1.Item1.Connected() && !p2.Item1.Connected())
            {
                playersConnected.RemoveAt(0);
                playersConnected.RemoveAt(0);
                return false;
            }
            else if (!p1.Item1.Connected() && p2.Item1.Connected())
            {
                playersConnected.RemoveAt(0);
                return false;
            }
            else if (p1.Item1.Connected() && !p2.Item1.Connected())
            {
                playersConnected.RemoveAt(1);
                return false;
            }
            else
                return true;
        }
    }

    /// <summary>
    /// Keeps track of the number of games won, lost, and tied by a Boggle player
    /// </summary>
    public class playerGamesRecord
    {
        public int won { get; set; }
        public int lost { get; set; }
        public int tied { get; set; }
        public String name { get; private set; }

        public playerGamesRecord(string _name, int _won, int _lost, int _tied)
        {
            name = _name;
            won = _won;
            lost = _lost;
            tied = _tied;
        }
    }

    /// <summary>
    /// Holds all of the info for a specific game of Boggle that was played
    /// </summary>
    public class GameInfo
    {
        public int gameId { get; set; }
        public int playerId { get; set; }
        public int oppId { get; set; }
        public string DateTime { get; set; }

        public int playerScore { get; set; }
        public int oppScore { get; set; }



        public GameInfo(int _gameId, int _P1ID, int _P2ID, string _DateTime, int _playerScore, int _oppScore)
        {

            gameId = _gameId;
            playerId = _P1ID;
            oppId = _P2ID;
            DateTime = _DateTime;
            playerScore = _playerScore;
            oppScore = _oppScore;



        }
    }

    /// <summary>
    /// Represents a game of Boggle between two clients. Handles communication between client and server based on a communication protocol.
    /// Keeps track of: valid words for each player, common words, score for each player, and remaining time. BoggleGame uses the BoggleBoard
    /// class to represent the game board and for determining whether words are legal.
    /// 
    /// </summary>
    public class BoggleGame
    {
        Tuple<StringSocket, String> player1;
        Tuple<StringSocket, String> player2;
        BoggleBoard board;
        private int time;
        private HashSet<String> commonWords;
        private HashSet<String> p1ValidWords;
        private HashSet<String> p2ValidWords;

        private StringSocket player1Socket;
        private StringSocket player2Socket;

        private HashSet<String> p1InvalidWords;

        private HashSet<String> p2InvalidWords;

        private readonly object scoreLock;

        private int p1Score;
        private int p2Score;

        private HashSet<String> legalWords;

        private System.Timers.Timer timer;

        private String gameTime;
        private int gameDuration;


        /// <summary>
        /// The connection string.
        /// Your CADE login name serves as both your database name and your uid
        /// Your uNID serves as your password
        /// </summary>
        public const string connectionString = "server=atr.eng.utah.edu;database=hshelton;uid=hshelton;password=300786954";

        /// <summary>
        /// Create a new boggle game with the specified players, board, duration, and dictionary of legal words. 
        /// </summary>
        /// <param name="_p1"></param>
        /// <param name="_p2"></param>
        /// <param name="theBoard"></param>
        /// <param name="_time"></param>
        /// <param name="_legalWords"></param>
        public BoggleGame(Tuple<StringSocket, String> _p1, Tuple<StringSocket, String> _p2, BoggleBoard theBoard, int _time, HashSet<String> _legalWords)
        {
            gameTime = System.DateTime.Now.ToString("u");
            time = _time;
            gameDuration = _time;

            // Create a timer with a one second interval.
            timer = new System.Timers.Timer(1000);

            // Hook up the Elapsed event for the timer.
            timer.Elapsed += new ElapsedEventHandler(sendTime);


            player1 = _p1;
            player2 = _p2;
            board = theBoard;

            commonWords = new HashSet<String>();
            p1ValidWords = new HashSet<String>();
            p2ValidWords = new HashSet<String>();
            p1InvalidWords = new HashSet<String>();
            p2InvalidWords = new HashSet<String>();

            p1Score = 0;
            p2Score = 0;
            // only one thread at a time can manipulate the score in order to avoid race conditions
            scoreLock = new Object();

            legalWords = _legalWords;

            player1Socket = _p1.Item1;
            player2Socket = _p2.Item1;

            // Send START @ X # (where @ is the 16 letter board configuration, X is the duration in seconds, and # is the oponent's name)
            player1Socket.BeginSend("START " + board.ToString() + " " + time + " " + player2.Item2 + "\n", (ex, py) => { }, null);
            player2Socket.BeginSend("START " + board.ToString() + " " + time + " " + player1.Item2 + "\n", (ex, py) => { }, null);
            timer.Start();

            player1Socket.BeginReceive(IncomingCallback, 1);
            player2Socket.BeginReceive(IncomingCallback, 2);
        }

        /// <summary>
        /// Sends out the number of seconds remaining in the game to each player. 
        /// When time has run out calls sendSummary to end the game. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sendTime(object sender, ElapsedEventArgs e)
        {
            if (time > 0)
            {
                ThreadPool.QueueUserWorkItem((x) => sendOutTime(time));
                time--;
            }
            else
            {
                timer.Stop();
                sendSummary();
            }
        }

        /// <summary>
        /// When time has expired, the server ignores any further communication from the clients and shuts down 
        /// the game. First, it transmits the final score to both clients as described above. Next, it transmits
        /// a game summary line to both clients. Suppose that during the game the client played a legal words that
        /// weren't played by the opponent, the opponent played b legal words that weren't played by the client,
        /// both players played c legal words in common, the client played d illegal words, and the opponent played
        /// e illegal words. The game summary command should be "STOP a #1 b #2 c #3 d #4 e #5", where a, b, c, d,
        /// and e are the counts described above and #1, #2, #3, #4, and #5 are the corresponding space-separated
        /// lists of words.
        /// </summary>
        private void sendSummary()
        {
            //build strings to send
            string p1Summary = "STOP ";
            string p2Summary = "STOP ";


            string p1VW = "";
            foreach (String s in p1ValidWords)
                p1VW += s + " ";

            string p2VW = "";
            foreach (String s in p2ValidWords)
                p2VW += s + " ";

            string common = "";
            foreach (String s in commonWords)
                common += s + " ";

            string p1illegal = "";
            foreach (String s in p1InvalidWords)
                p1illegal += s + " ";

            string p2illegal = "";
            foreach (String s in p2InvalidWords)
                p2illegal += s + " ";

            // If any list is empty, replace it with just a space.
            if (p1VW == "")
                p1VW = " ";
            if (p2VW == "")
                p2VW = " ";
            if (p1illegal == "")
                p1illegal = " ";
            if (p2illegal == "")
                p2illegal = " ";

            if (common == "")
                common = " ";

            // Concatenate the summary strings to be sent to each player
            p1Summary += p1ValidWords.Count + " " + p1VW + p2ValidWords.Count + " " + p2VW + commonWords.Count + " " + common + p1InvalidWords.Count + " " + p1illegal + p2InvalidWords.Count + " " + p2illegal;
            p2Summary += p2ValidWords.Count + " " + p2VW + p1ValidWords.Count + " " + p1VW + commonWords.Count + " " + common + p2InvalidWords.Count + " " + p2illegal + p1InvalidWords.Count + " " + p1illegal;

            //send appropriate game summaries to each client
            player1Socket.BeginSend(p1Summary.Substring(0, p1Summary.Length - 1) + "\n", (e, o) => { }, null);
            player2Socket.BeginSend(p2Summary.Substring(0, p2Summary.Length - 1) + "\n", (e, o) => { }, null);

            player1Socket.Close();
            player2Socket.Close();

            updateDatabase(p1Summary, p1Score, p2Score, player1.Item2, player2.Item2, gameTime, gameDuration, board.ToString());

        }

        /// <summary>
        /// Updates the database at the end of the game.
        /// </summary>
        private void updateDatabase(string p1Summary, int p1Score, int p2Score, string p1Name, string p2Name, string gameTime, int gameDuration, string boardLetters)
        {
            int player1ID = 0;
            int player2ID = 0;
            int gameID = 0;
            bool error = false;
            string errorMessage = "";
            // Connect to the DB
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                //try adding player 1 to the database
                try
                {
                    // Open a connection
                    conn.Open();
                    //try adding player's id to the database, will throw an exception if already there
                    MySqlCommand addP1Id = conn.CreateCommand();
                    addP1Id.CommandText = @"INSERT INTO `hshelton`.`Players` (`Name`) VALUES ('" + p1Name + "');";
                    addP1Id.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    // If an exception was thrown that means the player is already in the database.  In the 
                    // finally block we will query the database for their ID.
                }
                finally
                {
                    // Create a command for getting player1's id
                    MySqlCommand getP1Id = conn.CreateCommand();
                    getP1Id.CommandText = @"select ID from Players where Name = '" + p1Name + "';";

                    // Execute the command and cycle through the DataReader object
                    using (MySqlDataReader reader = getP1Id.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Int32.TryParse(reader["ID"].ToString(), out player1ID);
                        }
                    }
                }

                if (!error)
                {
                    //try adding player 2 to the database
                    try
                    {
                        //try adding player's id to the database, will throw an exception if already there
                        MySqlCommand addP2Id = conn.CreateCommand();
                        addP2Id.CommandText = @"INSERT INTO `hshelton`.`Players` (`Name`) VALUES ('" + p2Name + "');";
                        addP2Id.ExecuteNonQuery();
                    }
                    catch (Exception)
                    {
                        // If an exception was thrown that means the player is already in the database.  In the 
                        // finally block we will query the database for their ID.
                    }
                    finally
                    {
                        // Create a command for getting player1's id
                        MySqlCommand getP2Id = conn.CreateCommand();
                        getP2Id.CommandText = @"select ID from Players where Name = '" + p2Name + "';";
                        // Execute the command and cycle through the DataReader object
                        using (MySqlDataReader reader = getP2Id.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Int32.TryParse(reader["ID"].ToString(), out player2ID);
                            }
                        }
                    }
                }

                if (!error)
                {
                    try
                    {
                        MySqlCommand addGame = conn.CreateCommand();
                        addGame.CommandText = @"INSERT INTO `hshelton`.`Games` (`P1Id`, `P2Id`, `Date`, `Board`, `TimeLimit`, `P1Score`, `P2Score`)
                                          VALUES ('" + player1ID + "', '" + player2ID + "', '" + gameTime + "', '" + boardLetters + "', '" + gameDuration + "', '" + p1Score + "', '" + p2Score + "');";
                        addGame.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        // Send back error page with exception message
                        error = true;
                        errorMessage = e.Message;
                    }

                    try
                    {
                        MySqlCommand getGameID = conn.CreateCommand();
                        getGameID.CommandText = @"SELECT GameId from Games where Date = '" + gameTime + "';";
                        using (MySqlDataReader reader = getGameID.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Int32.TryParse(reader["GameId"].ToString(), out gameID);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        error = true;
                        errorMessage = e.Message;
                    }
                }

                if (!error)
                {
                    try
                    {
                        MySqlCommand addAWord = conn.CreateCommand();

                        foreach (string s in p1ValidWords)
                        {
                            addAWord.CommandText = @"INSERT INTO `hshelton`.`Words` (`Word`, `GameId`, `PlayerId`, `PlayerId2`, `legal`) VALUES ('" + s + "', '" + gameID + "', '" + player1ID + "', '', 1);";
                            addAWord.ExecuteNonQuery();
                        }

                        foreach (string s in p2ValidWords)
                        {
                            addAWord.CommandText = @"INSERT INTO `hshelton`.`Words` (`Word`, `GameId`, `PlayerId`, `PlayerId2`, `legal`) VALUES ('" + s + "', '" + gameID + "', '" + player2ID + "', '', 1);";
                            addAWord.ExecuteNonQuery();
                        }

                        foreach (string s in commonWords)
                        {
                            addAWord.CommandText = @"INSERT INTO `hshelton`.`Words` (`Word`, `GameId`, `PlayerId`, `PlayerId2`, `legal`) VALUES ('" + s + "', '" + gameID + "', '" + player1ID + "', '" + player2ID + "', 1);";
                            addAWord.ExecuteNonQuery();
                        }

                        foreach (string s in p1InvalidWords)
                        {
                            addAWord.CommandText = @"INSERT INTO `hshelton`.`Words` (`Word`, `GameId`, `PlayerId`, `PlayerId2`, `legal`) VALUES ('" + s + "', '" + gameID + "', '" + player1ID + "', '', 0);";
                            addAWord.ExecuteNonQuery();
                        }

                        foreach (string s in p2InvalidWords)
                        {
                            addAWord.CommandText = @"INSERT INTO `hshelton`.`Words` (`Word`, `GameId`, `PlayerId`, `PlayerId2`, `legal`) VALUES ('" + s + "', '" + gameID + "', '" + player2ID + "', '', 0);";
                            addAWord.ExecuteNonQuery();
                        }
                    }
                    catch (Exception e)
                    {
                        error = true;
                        errorMessage = e.Message;
                    }
                }
                if (error)
                {
                    Console.WriteLine("An error occured while updating the database: " + errorMessage);
                }
                conn.Close();
            }
        }

        /// <summary>
        /// allows sendTime to be called on a new thread
        /// </summary>
        /// <param name="_time"></param>
        private void sendOutTime(int _time)
        {
            player1Socket.BeginSend("TIME " + _time + "\n", (e, o) => { }, null);
            player2Socket.BeginSend("TIME " + _time + "\n", (e, o) => { }, null);
        }

        /// <summary>
        /// A ReturnCallback to process the incomming messages from the two clients 
        /// while the game is in progress.
        /// </summary>
        private void IncomingCallback(string s, Exception e, object payload)
        {
            if (time <= 0)
                return;

            if (s == null)
            {
                clientDisconnected((int)payload);
                return;
            }

            string word = s.ToUpper();
            int playerNumber = (int)payload;
            bool sendIgnore = false;

            //ignore communications that are against protocol
            if (!word.isWORD())
                sendIgnore = true;
            else
                word = word.Substring(5);

            // Trim off any return characters
            if (word.EndsWith("\r"))
                word = word.Substring(0, word.Length - 1);

            if (playerNumber == 1)
            {
                // Request the next message from player1
                player1Socket.BeginReceive(IncomingCallback, 1);

                // if the word is under 3 characters it is ignored
                if (word.Length < 3)
                    return;

                if (sendIgnore)
                {
                    player1Socket.BeginSend("IGNORING " + s + "\n", (ex, obj) => { }, null);
                }
                else
                {
                    //check to see if word hasn't been played, update score
                    p1WordCheck(word);
                }
            }

            else if (playerNumber == 2)
            {
                // Request the next message from player2
                player2Socket.BeginReceive(IncomingCallback, 2);

                // if the word is under 3 characters it is ignored
                if (word.Length < 3)
                    return;

                if (sendIgnore)
                {
                    player2Socket.BeginSend("IGNORING " + s + "\n", (ex, obj) => { }, null);
                }
                else
                {
                    //check to see if word hasn't been played, update score
                    p2WordCheck(word);
                }
            }
        }

        /// <summary>
        /// Sends the "TERMINATED" message to the surviving client in the event that one client
        /// disconnects from the BoggleServer or becomes inaccessible.
        /// </summary>
        /// <param name="p"></param>
        private void clientDisconnected(int p)
        {
            if (p == 1)
            {
                Console.WriteLine(player1.Item2 + " disconnected...");
                player2Socket.BeginSend("TERMINATED\n", (e, o) => { }, null);
                timer.Stop();
                Thread.Sleep(100);
                player2Socket.Close();
            }
            else if (p == 2)
            {
                Console.WriteLine(player2.Item2 + " disconnected...");
                player1Socket.BeginSend("TERMINATED\n", (e, o) => { }, null);
                timer.Stop();
                Thread.Sleep(100);
                player1Socket.Close();
            }
        }

        /// <summary>
        /// Determines how to handle scoring for a word from player1
        /// </summary>
        /// <param name="word"></param>
        private void p1WordCheck(string word)
        {
            lock (scoreLock)
            {
                int points = getPoints(word);
                // If the word is valid
                if (word.isValid(legalWords, board))
                {
                    if (p2ValidWords.Contains(word))
                    {
                        p2ValidWords.Remove(word);
                        commonWords.Add(word);
                        p2Score -= points;

                        player1Socket.BeginSend("SCORE " + p1Score + " " + p2Score + "\n", (e, p) => { }, null);
                        player2Socket.BeginSend("SCORE " + p2Score + " " + p1Score + "\n", (e, p) => { }, null);
                    }

                    else if (!commonWords.Contains(word) && p1ValidWords.Add(word))
                    {
                        p1Score += points;

                        player1Socket.BeginSend("SCORE " + p1Score + " " + p2Score + "\n", (e, p) => { }, null);
                        player2Socket.BeginSend("SCORE " + p2Score + " " + p1Score + "\n", (e, p) => { }, null);
                    }

                }
                // If the word is NOT valid
                else if (p1InvalidWords.Add(word))
                {
                    p1Score--;

                    player1Socket.BeginSend("SCORE " + p1Score + " " + p2Score + "\n", (e, p) => { }, null);
                    player2Socket.BeginSend("SCORE " + p2Score + " " + p1Score + "\n", (e, p) => { }, null);
                }
            }
        }

        /// <summary>
        /// determines how to handle scoring for a word from player2
        /// </summary>
        /// <param name="word"></param>
        private void p2WordCheck(string word)
        {
            lock (scoreLock)
            {
                int points = getPoints(word);
                // If the word is valid
                if (word.isValid(legalWords, board))
                {
                    if (p1ValidWords.Contains(word))
                    {
                        //remove common word from player 1 score
                        p1ValidWords.Remove(word);
                        commonWords.Add(word);
                        p1Score -= points;

                        player1Socket.BeginSend("SCORE " + p1Score + " " + p2Score + "\n", (e, p) => { }, null);
                        player2Socket.BeginSend("SCORE " + p2Score + " " + p1Score + "\n", (e, p) => { }, null);
                    }

                    else if (!commonWords.Contains(word) && p2ValidWords.Add(word))
                    {
                        p2Score += points;

                        player1Socket.BeginSend("SCORE " + p1Score + " " + p2Score + "\n", (e, p) => { }, null);
                        player2Socket.BeginSend("SCORE " + p2Score + " " + p1Score + "\n", (e, p) => { }, null);
                    }
                }
                // If the word is NOT valid
                else if (p2InvalidWords.Add(word))
                {
                    p2Score--;

                    player1Socket.BeginSend("SCORE " + p1Score + " " + p2Score + "\n", (e, p) => { }, null);
                    player2Socket.BeginSend("SCORE " + p2Score + " " + p1Score + "\n", (e, p) => { }, null);
                }
            }
        }

        /// <summary>
        /// A helper method to get the score of a given word Three- and four-letter words are worth one point, 
        /// five-letter words are worth two points, six-letter words are worth three points, seven-letter words
        /// are worth five points, and longer word are worth 11 points.
        /// </summary>
        /// <param name="word">The word to evaluate</param>
        /// <returns>The point value for the given word</returns>
        private int getPoints(string word)
        {
            int wordLength = word.Length;
            int points = 0;
            switch (wordLength)
            {
                case 3:

                case 4:
                    points = 1;
                    break;
                case 5:
                    points = 2;
                    break;

                case 6:
                    points = 3;
                    break;
                case 7:
                    points = 5;
                    break;

                default:
                    points = 11;
                    break;
            }

            return points;
        }
    }

    /// <summary>
    /// A class containing a few useful extension methods used by BoggleServer.
    /// </summary>
    public static class Extension
    {
        /// <summary>
        /// An extension method for incomming strings.  Returns true if the given word is in the list
        /// of legal words AND the given word can be formed on the given BoggleBoard.
        /// </summary>
        public static bool isValid(this string _word, HashSet<string> _legalWords, BoggleBoard _board)
        {
            if (!_legalWords.Contains(_word) || !_board.CanBeFormed(_word))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// An extension method for incomming strings.  Returns true if the incomming string begins with "WORD ".
        /// </summary>
        public static bool isWORD(this string _word)
        {
            return (Regex.IsMatch(_word, "^(WORD )"));
        }

        /// <summary>
        /// An extension method for incomming strings.  Returns true if the incomming string begins with "PLAY ".
        /// </summary>
        /// <param name="_word"></param>
        /// <returns></returns>
        public static bool isPLAY(this string _word)
        {
            return (Regex.IsMatch(_word, "^(PLAY )"));
        }
    }
}
