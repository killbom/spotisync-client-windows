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
    public interface ISpotifyWatcher
    {
        void SetTrack(TrackEvent track);
        EventHandler<TrackEvent> OnTrackChanged { get; set; }
        EventHandler<StatusEvent> OnPlayStateChanged { get; set; }
    }
}
