using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Server
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private GameServer? server;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            if (server != null && server.isAlive)
            {
                server.Stop();
            }
        }

        private void btnStartGameServer_Click(object sender, RoutedEventArgs e)
        {
            server = new GameServer("127.0.0.1", 13000);
            server.Start();
            server.ConnectionsChange += onConnectionsChange;
            btnStartGameServer.IsEnabled = false;
            btnStopGameServer.IsEnabled = true;
        }

        private void btnStopGameServer_Click(object sender, RoutedEventArgs e)
        {
            if (server != null && server.isAlive)
            {
                server.Stop();
            }
            btnStopGameServer.IsEnabled = false;
            btnStartGameServer.IsEnabled = true;
        }

        private void onConnectionsChange(object sender, ConnectionsNumberChangeEventArgs e)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                connectionsLbl.Content = e.Connections;
            }));
            
        }
    }
}