using SpotiSync_Windows.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpotiSync_Common;

namespace SpotiSync_Windows.Services
{
    public class SessionManager : ISessionManager
    {
        public User CurrentUser { get; set; }
        public string SessionId { get; set; }
        public bool isHosting { get; set; }
    }
}
