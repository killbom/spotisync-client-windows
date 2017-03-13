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
    public interface IConnectionManager
    {
        bool isConnected { get; }
        Task<User> CreateUser(string userName);
        Task<bool> Connect(string session, User user);
        Task<string> StartSession(User user);
        EventHandler<TrackEvent> OnTrackChanged { get; set; }
        void SetTrack(TrackEvent newTrack);
    }
}
