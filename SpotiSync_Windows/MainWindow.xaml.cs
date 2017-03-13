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
        public MainWindow()
        {
            InitializeComponent();

            var service = ServiceConfigurator.GetService<ISpotifyWatcher>();

            service.OnTrackChanged += PrintTrackName;
        }

        private void PrintTrackName(object sender, TrackEvent args)
        {
            Console.WriteLine(args.Name + " - " + args.Url);
        }
    }
}
