using SpotifyAPI.Local;
using SpotiSync_Common;
using SpotiSync_Windows.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SpotiSync_Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ISpotifyWatcher spotify;
        IConnectionManager connection;
        ISessionManager session;        

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            spotify = ServiceConfigurator.GetService<ISpotifyWatcher>();
            connection = ServiceConfigurator.GetService<IConnectionManager>();
            session = ServiceConfigurator.GetService<ISessionManager>();

            spotify.OnTrackChanged += PrintTrackName;
            connection.OnTrackChanged += ServerTrackChange;
        }

        public string UserName { get; set; }
        public string SessionId { get; set; }

        private void PrintTrackName(object sender, TrackEvent args)
        {
            if (session.isHosting)
            {
                connection.SetTrack(args);
            }
        }

        private void ServerTrackChange(object sender, TrackEvent args)
        {
            spotify.SetTrack(args);
        }

        private async void Connect(object sender, RoutedEventArgs e)
        {
            var user = await connection.CreateUser(UserName);
            session.CurrentUser = user;
        }
        private void Join(object sender, RoutedEventArgs e)
        {
            connection.Connect(SessionId, session.CurrentUser);
        }
    }
}
