using SpotiSync_Windows.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotiSync_Windows.Models
{
    public class Options: IOptions
    {
        private const string server = "http://localhost:56624/api/";
        private const string socket = "ws://localhost/";

        public string serverUrl
        {
            get
            {
                return server;
            }
        }

        public string socketUrl
        {
            get
            {
                return socket;
            }
        }
    }
}
