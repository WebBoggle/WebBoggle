using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using BoggleWebServer;
using System.Threading;
using System.Web;
using Newtonsoft.Json;
namespace BoggleWebServer
{

    public delegate void ContextReceived(object payload);


    class Server
    {


        protected static volatile Dictionary<string, BoggleConnection> IdsToConnections;
        private static Object thisLock = new Object();
      

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"> the player's id</param>
        /// <param name="userName"> the user name of the player</param>
        public static void addToDictionary(string userid, string userName)
        {
            BoggleConnection current = new BoggleConnection("localhost", userName);
            lock (thisLock)
            {
                IdsToConnections.Add(userid, current);
            }
        }

        public static void PlayWord(string userid, string value)
        {
            IdsToConnections[userid].SendWord(value);
        }

        /// <summary>
        /// Return an update containing all the game properties.
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public static update UpdateAll(string userid)
        {
            update result = new update();
            if(IdsToConnections.ContainsKey(userid))
            {
                var BCon = IdsToConnections[userid];
               
                if(BCon.oppName == null)
                {
                    result.status = "waiting";
                    return result;
                }
                else //the game is ongoing
                {
                    result.oppName = BCon.oppName;
                    result.score = BCon.Score;
                    result.timeLeft = BCon.TimeLeft;
                    result.board = BCon.BoardLetters;
                    result.status = "ongoing";
                    return result;
                }
               
            }
            return result;
        }

        /// <summary>
        /// Call this after update all has initiated the game
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public static update UpdateVariables(string userid)
        {
            update result = new update();
            if (IdsToConnections.ContainsKey(userid))
            {
                var BCon = IdsToConnections[userid];
                result.score = BCon.Score;
                result.timeLeft = BCon.TimeLeft;

                if (BCon.TimeLeft == "0")
                    result.status = "stopped";
                else
                    result.status = "ongoing";

            }
            return result;
        }
        /// <summary>
        /// Sends a message to the server indicating that the user is ready to play
        /// </summary>
        /// <param name="userid"></param>
        public static void PlayerReady(string userid)
        {
            IdsToConnections[userid].Connect();
        }

        static void Main(string[] args)
        {
            IdsToConnections = new Dictionary<string, BoggleConnection>();
            //handle initial connections
            communicationPipe comm = new communicationPipe(ServerRoutes.INDEX);

            //handle update requests
            communicationPipe comm1 = new communicationPipe(ServerRoutes.UPDATE);
            communicationPipe comm2 = new communicationPipe(ServerRoutes.PLAYWORD);
            communicationPipe comm3 = new communicationPipe(ServerRoutes.READY);

            Console.Read();

        }




        public class communicationPipe
        {

            private HttpListener listener;
            private List<string> pendingActions;

            public communicationPipe(string prefix)
            {
                listener = new HttpListener();
                listener.Prefixes.Add(prefix);
                listener.Start();
                switch (prefix)
                {
                    case ServerRoutes.UPDATE:
                        listener.BeginGetContext(RespondToUpdate, listener);
                        break;
                    case ServerRoutes.PLAYWORD:
                        listener.BeginGetContext(RespondToPlayWord, listener);
                        break;
                    case ServerRoutes.READY:
                        listener.BeginGetContext(RespondToReady, listener);
                        break;

                    default:
                        listener.BeginGetContext(waitForContext, listener);
                        break;

                }

            }


            /// <summary>
            /// An AsyncCallback that handles the initial response and sends out initial html
            /// this runs on its own thread
            /// </summary>
            /// <param name="result"></param>
            protected void waitForContext(IAsyncResult result)
            {
                HttpListener listener = (HttpListener)result.AsyncState;
                // Call EndGetContext to complete the asynchronous operation.
                HttpListenerContext context = listener.EndGetContext(result);
                string indexPage = System.IO.File.ReadAllText("../../index.html");

                sendString(context, indexPage);



                //wait for more communications
                listener.BeginGetContext(waitForContext, listener);




            }

            private void RespondToUpdate(IAsyncResult result)
            {
                HttpListener listener = (HttpListener)result.AsyncState;
                HttpListenerContext context = listener.EndGetContext(result);

                var type = context.Request.QueryString["type"];
                var updateForId = context.Request.QueryString["userID"];
                if (type == "all")
                {
                    UpdateAll(updateForId); //get an update object & convert it to JSON, call send JSON

                    //test for now
                    update temp = UpdateAll(updateForId);
                    string json = JsonConvert.SerializeObject(temp);
                    sendString(context, json);

                }
                else if(type =="variables")
                {
                    update temp = UpdateVariables(updateForId);
                    string json = JsonConvert.SerializeObject(temp);
                    sendString(context, json);
                }

                //wait for more communications
                listener.BeginGetContext(RespondToUpdate, listener);
            }

            private void RespondToReady(IAsyncResult result)
            {
                HttpListener listener = (HttpListener)result.AsyncState;
                HttpListenerContext context = listener.EndGetContext(result);

                var id = context.Request.QueryString["userID"];
                var userName = context.Request.QueryString["user"];

                //add this player to the dictionary of ids to user names
                addToDictionary(id, userName);
                IdsToConnections[id].Connect();

                //listen for 'Ready from others'
                listener.BeginGetContext(RespondToReady, listener);
            }
            private void RespondToPlayWord(IAsyncResult result)
            {
                HttpListener listener = (HttpListener)result.AsyncState;
                HttpListenerContext context = listener.EndGetContext(result);

                try
                {
                    var id = context.Request.QueryString["userID"];
                    var word = context.Request.QueryString["word"];

                    IdsToConnections[id].SendWord(word);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.Write(e.StackTrace);
                }
                closeOutput(context);
                listener.BeginGetContext(RespondToPlayWord, listener);
            }
            /// <summary>
            /// 


            #region Transmission Util
            private void sendString(HttpListenerContext context, string responseString)
            {

                HttpListenerRequest request = context.Request;

                // Obtain a response object.
                HttpListenerResponse response = context.Response;
                // Construct a response. 
                byte[] buffer;

                buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
                // Get a response stream and write the response to it.
                response.ContentLength64 = buffer.Length;
                response.KeepAlive = false;
                System.IO.Stream output = response.OutputStream;
                output.Write(buffer, 0, buffer.Length);
                output.Close();




            }

           private void closeOutput(HttpListenerContext context)
            {
                // Obtain a response object.
                HttpListenerResponse response = context.Response;
                response.KeepAlive = false;
                response.OutputStream.Close();
            }
            #endregion
        }

       

    }

}