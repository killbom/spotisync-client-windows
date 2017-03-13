using SpotiSync_Windows.Interfaces;
using System;
using SpotifyAPI.Local;
using SpotifyAPI.Local.Models;
using System.Threading;
using SpotiSync_Common;

namespace SpotiSync_Windows.Services
{
    public class SpotifyWatcher : ISpotifyWatcher, IDisposable
    {
        private SpotifyLocalAPI _spotify;
        private bool playing;
        private TrackEvent track;

        public SpotifyWatcher()
        {
            Connect();
            var status = _spotify.GetStatus(); //status contains infos
            playing = status.Playing;
        }

        public EventHandler<StatusEvent> OnPlayStateChanged { get; set; }
        public EventHandler<TrackEvent> OnTrackChanged { get; set; }

        public void SetTrack(TrackEvent track)
        {
            this.track = track;

            if (playing)
                _spotify.PlayURL(this.track.Url);
        }

        private void PlayStateChanged(object sender, PlayStateEventArgs args)
        {
            playing = args.Playing;

            if (playing && this.track != null)
                _spotify.PlayURL(this.track.Url);

            OnPlayStateChanged(sender, new StatusEvent { Playing = playing });
        }

        private void TrackChanged(object sender, TrackChangeEventArgs args)
        {
            OnTrackChanged(sender, new TrackEvent
            {
                Name = args.NewTrack.TrackResource.Name,
                Url = args.NewTrack.TrackResource.Uri
            });
        }

        private void TrackTimeChanged(object sender, TrackTimeChangeEventArgs args)
        {
            //Console.WriteLine(args.TrackTime);
        }

        #region Connect / Dispose

        public void Connect()
        {
            if (_spotify != null)
                return;

            _spotify = new SpotifyLocalAPI();
            if (!SpotifyLocalAPI.IsSpotifyRunning())
            {
                SpotifyLocalAPI.RunSpotify();
                Thread.Sleep(2000);
                if (!SpotifyLocalAPI.IsSpotifyRunning())
                {
                    throw new InvalidOperationException("Plese Start Spotify"); //Make sure the spotify client is running
                }
            }
            if (!SpotifyLocalAPI.IsSpotifyWebHelperRunning())
            {
                SpotifyLocalAPI.RunSpotifyWebHelper();
                Thread.Sleep(2000);
                if (!SpotifyLocalAPI.IsSpotifyWebHelperRunning())
                {
                    throw new InvalidOperationException("Plese Start Spotify"); //Make sure the spotify client is running
                }
            }

            if (_spotify.Connect())
            {
                SetupListeners();
            }                
            else
            {
                _spotify.Dispose();
                Thread.Sleep(1000);

                if(_spotify.Connect())
                {
                    SetupListeners();
                }

                throw new InvalidOperationException("Failed to connect to Spotify");
            }                
        }

        private void SetupListeners()
        {
            _spotify.ListenForEvents = true;

            _spotify.OnPlayStateChange += PlayStateChanged;
            _spotify.OnTrackChange += TrackChanged;
            _spotify.OnTrackTimeChange += TrackTimeChanged;
        }

        public void Dispose()
        {
            if(_spotify != null)
            {
                _spotify.OnPlayStateChange -= PlayStateChanged;
                _spotify.OnTrackChange -= TrackChanged;
                _spotify.OnTrackTimeChange -= TrackTimeChanged;

                _spotify.Dispose();
            }
        }

#endregion
    }
}
