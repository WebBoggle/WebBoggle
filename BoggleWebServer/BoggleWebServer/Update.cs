using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoggleWebServer
{
    /// <summary>
    /// Used to easily construct Json objects to send to the view
    /// </summary>
    public class update
    {
        public string board { get; set; }
        public string score { get; set; }
        public string timeLeft { get; set; }
        public string oppName { get; set; }
        public string status { get; set; }

        public update()
        {
            board = "null";
            score = "null";
            timeLeft = "null";
            oppName = "null";
            status = "null";
        }
    }

}
