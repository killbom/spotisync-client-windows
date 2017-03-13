using SpotiSync_Windows.Interfaces;
using SpotiSync_Windows.Services;
using System.Windows;

namespace SpotiSync_Windows
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            ServiceConfigurator.RegisterService<ISpotifyWatcher, SpotifyWatcher>();
            ServiceConfigurator.RegisterService<IConnectionManager, ConnectionManager>();
        }
    }
}
