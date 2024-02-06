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

namespace Game
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private GameClient _client;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void UsernameTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (IsUsernameValid(UsernameTxt.Text))
            {
                ConnectBtn.IsEnabled = true;
            }
            else
            {
                ConnectBtn.IsEnabled = false;
            }

        }

        private bool IsUsernameValid(string username)
        {
            if (!string.IsNullOrEmpty(username) && username.Length >= 3 && username.Length <= 20)
            {
                return true;
            }
            return false;
        }

        private void ConnectBtn_Click(object sender, RoutedEventArgs e)
        {
            _client = new GameClient("127.0.0.1", 13000);
            _client.ConnectionStatusChange += onConnectionStatusChange;
            _client.Connect(UsernameTxt.Text);
        }

        private void onConnectionStatusChange(object sender, ConnectionStatusChangeEventArgs e)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                ConnStatusLbl.Visibility = e.IsConnected ? Visibility.Visible : Visibility.Hidden;
                btnUse1.Background = new SolidColorBrush(e.IsConnected ? Color.FromRgb(255, 0, 0) : Color.FromRgb(221, 221, 221));
                DisconnectBtn.IsEnabled = e.IsConnected;
                ConnectBtn.IsEnabled = !e.IsConnected;
            }));
        }

        private void DisconnectBtn_Click(object sender, RoutedEventArgs e)
        {
            _client.Disconnect();
        }
    }
}