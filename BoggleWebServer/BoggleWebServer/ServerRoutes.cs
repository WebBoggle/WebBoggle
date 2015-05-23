using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoggleWebServer
{
    class ServerRoutes
    {
        private const string portNum = ":3012/";
        public const string publicIP = "http://locahost"; //we add localhost prefix for debugging purposes
        public const string INDEX = publicIP+portNum;
        public const string UPDATE = publicIP + portNum + "Update/";
        public const string PLAYWORD = publicIP+portNum + "PlayWord/";
        public const string READY = publicIP+portNum + "Ready/";
   
    }
}
