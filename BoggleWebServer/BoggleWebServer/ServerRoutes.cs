using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoggleWebServer
{
    class ServerRoutes
    {
        private const string baseIP = "http://localhost";
        private const string portNum = ":3012/";
        public const string INDEX = baseIP+portNum;
        public const string UPDATE = baseIP + portNum + "Update/";
        public const string PLAYWORD = baseIP+portNum + "PlayWord/";
        public const string READY = baseIP+portNum + "Ready/";
   
    }
}
