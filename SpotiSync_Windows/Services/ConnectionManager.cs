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
using System.Net.WebSockets;

namespace SpotiSync_Windows.Services
{
    public class ConnectionManager : IConnectionManager
    {
        private bool _isConnected = false;
        private bool isHosting = false;

        private string url = "http://localhost/";
        private HttpClient client;
        private WebSocket socket;
        private CancellationTokenSource token;

        public ConnectionManager()
        {
            client = new HttpClient();
        }

        public bool isConnected
        {
            get { return _isConnected; }
            set { isConnected = value; }
        }

        public EventHandler<TrackEvent> OnTrackChanged { get; set; }

        public async Task<User> CreateUser(string userName)
        {
            var content = new StringContent(JsonConvert.SerializeObject(new { Name = userName }));
            var result = await client.PostAsync(url + "api/users/new", content);

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
            var result = await client.GetAsync(url + "api/sessions/join" + session+ "/" + user.Id);
            if (result.IsSuccessStatusCode)
            {
               

                new Task(async () =>
                {
                   
                }).Start();
                return true;
            } else
            {
                return false;
            }
        }

        public async Task<string> StartSession(User user)
        {
            var result = await client.GetAsync(url + "api/sessions/new" + user.Id);
            if (result.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<dynamic>(await result.Content.ReadAsStringAsync()).id;
            }
            else
            {
                return "";
            }
        }

        public void SetTrack(TrackEvent newTrack)
        {
            throw new NotImplementedException();
        }
    }
}
