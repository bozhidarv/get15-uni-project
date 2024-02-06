using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Game
{
    internal class GameClient
    {
        public delegate void ConnectionStatusChangeHandler(object sender, ConnectionStatusChangeEventArgs e);
        public event ConnectionStatusChangeHandler ConnectionStatusChange;

        private TcpClient _client;
        private string _host;
        private int _port;

        public GameClient(string host, int port)
        {
            _port = port;
            _host = host;
            _client = new TcpClient();
        }

        public void Connect(string username)
        {
            try
            {
                _client.Connect(_host, _port);

                ConnectionStatusChange.Invoke(this, new ConnectionStatusChangeEventArgs(_client.Connected));


                var resp = SendMessage($"connect {username}");

            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
        }

        public string SendMessage(string message)
        {
            Byte[] data = Encoding.ASCII.GetBytes(message);

            NetworkStream stream = _client.GetStream();

            try
            {

                stream.Write(data, 0, data.Length);

                data = new Byte[256];
                String responseData = String.Empty;

                Int32 bytes = stream.Read(data, 0, data.Length);
                responseData = Encoding.ASCII.GetString(data, 0, bytes);

                return responseData;
            }
            catch (Exception ex)
            {
                ConnectionStatusChange?.Invoke(this, new ConnectionStatusChangeEventArgs(false));
                Disconnect();
                return "";
            }
        }

        void handleResp()
        {

        }

        public void Disconnect()
        {
            _client.Dispose();
            ConnectionStatusChange.Invoke(this, new ConnectionStatusChangeEventArgs(_client.Connected));
        }
    }
}
