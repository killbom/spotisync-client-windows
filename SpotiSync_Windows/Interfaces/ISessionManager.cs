using SpotifyAPI.Local;
using SpotifyAPI.Local.Models;
using SpotiSync_Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotiSync_Windows.Interfaces
{
    public interface ISessionManager
    {
        User CurrentUser { get; set;  }        
        string SessionId { get; set; }
    }
}
