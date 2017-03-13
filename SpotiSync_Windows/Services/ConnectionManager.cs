using SpotiSync_Windows.Interfaces;
using SpotiSync_Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading;
using WebSocketSharp;

namespace SpotiSync_Windows.Services
{
    public class ConnectionManager : IConnectionManager
    {
        private bool _isConnected = false;

        private IOptions opts;
        private HttpClient client;
        private WebSocket socket;
        private ISessionManager session;

        private CancellationTokenSource token;

        public ConnectionManager()
        {
            client = new HttpClient();
            opts = ServiceConfigurator.GetService<IOptions>();
            session = ServiceConfigurator.GetService<ISessionManager>();
        }

        public bool isConnected
        {
            get { return _isConnected; }
            set { isConnected = value; }
        }

        public EventHandler<TrackEvent> OnTrackChanged { get; set; }

        public async Task<User> CreateUser(string userName)
        {
            var content = new StringContent(JsonConvert.SerializeObject(new { Name = userName }), Encoding.UTF8, "application/json");
            var result = await client.PostAsync(opts.serverUrl + "users/new", content);

            if (result.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<User>(await result.Content.ReadAsStringAsync());
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> Connect(string session, User user)
        {
            var result = await client.GetAsync(opts.serverUrl + "sessions/join/" + session+ "/" + user.Id);
            if (result.IsSuccessStatusCode)
            {               
                socket = new WebSocket(opts.socketUrl + "?userId=" + user.Id);
                socket.Connect();
                socket.OnMessage += OnMessage;

                return true;
            } else
            {
                return false;
            }
        }

        public async Task<string> StartSession(User user)
        {
            var result = await client.GetAsync(opts.serverUrl + "sessions/new" + user.Id);
            if (result.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<dynamic>(await result.Content.ReadAsStringAsync()).id;
            }
            else
            {
                return "";
            }
        }

        public void Disconnect()
        {
            if (socket != null)
            {
                socket.OnMessage -= OnMessage;
                socket.CloseAsync();
                socket = null;
            }
        }

        private void OnMessage(object sender, MessageEventArgs e)
        {
            var json = e.Data;
            var track = JsonConvert.DeserializeObject<TrackEvent>(json);

            OnTrackChanged(this, track);
        }

        public async void SetTrack(TrackEvent newTrack)
        {
            var content = new StringContent(JsonConvert.SerializeObject(newTrack), Encoding.UTF8, "application/json");
            await client.PostAsync(opts.serverUrl + $"play/{session.SessionId}/{session.CurrentUser.Id}", content);
        }
    }
}
