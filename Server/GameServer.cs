using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Numerics;

namespace Server
{
    class GameServer
    {
        public delegate void ConnectionsChangeHandler(object sender, ConnectionsNumberChangeEventArgs e);
        public event ConnectionsChangeHandler ConnectionsChange;

        private TcpListener _listener;
        private Thread? _serverThread;
        private Game? game;

        private List<KeyValuePair<Player, TcpClient>> _players = new List<KeyValuePair<Player, TcpClient>>();

        public GameServer(string host, int port)
        {
            //int port = 13000;
            IPAddress localAddr = IPAddress.Parse(host);
            _listener = new TcpListener(localAddr, port);
            
        }

        public void Start()
        {
            _serverThread = new Thread(ServerStart);
            _serverThread.IsBackground = true;
            _serverThread.Start();
        }

        private void ServerStart()
        {
            try
            {
                _listener.Start();

                while (true)
                {
                    if (_players.Count < 2)
                    {
                        TcpClient client = _listener.AcceptTcpClient();

                        var cts = new CancellationTokenSource();
                        

                        //ThreadPool.QueueUserWorkItem(clientStuff, player);
                        Task.Factory.StartNew(() => HandlePlayers(client), cts.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
                    }
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine($"SocketException: {e}");
            }
            finally
            {
                _listener.Stop();
            }
        }


        private void HandlePlayers(TcpClient client)
        {
            _players.Add(new KeyValuePair<Player, TcpClient>(null, client));
            ConnectionsChange?.Invoke(this, new ConnectionsNumberChangeEventArgs(_players.Count()));

            Byte[] bytes = new Byte[256];
            NetworkStream stream = client.GetStream();

            int i;

            try
            {
                var reader = new BinaryReader(stream);
                while (true)
                {
                    string incoming = reader.ReadString();

                    var commandArr = incoming.Split(" ");

                    string outgoingCmd = string.Empty;

                    if (incoming == "Q") { break; }

                    if (commandArr[0] == "connect")
                    {
                        if (game == null)
                        {
                            game = new Game(commandArr[1]);

                            _players.Add(new KeyValuePair<Player, TcpClient>(game.Player1, client));
                            ConnectionsChange?.Invoke(this, new ConnectionsNumberChangeEventArgs(_players.Count()));

                            outgoingCmd = $"{game.Player1.Name} wait";
                        }
                        else
                        {
                            _players.Add(new KeyValuePair<Player, TcpClient>(game.Player1, client));
                            ConnectionsChange?.Invoke(this, new ConnectionsNumberChangeEventArgs(_players.Count()));

                            game.joinPlayer2(commandArr[1]);
                            outgoingCmd = $"* start turn ${game.Player1}";
                        }
                    }

                    Console.WriteLine("Received: {0}", incoming);

                    foreach (var clientS in _players)
                    {
                        var writer = new BinaryWriter(clientS.Value.GetStream());
                        if (outgoingCmd.Split(' ')[0] == "*" || outgoingCmd.Split(' ')[0] == clientS.Key.Name)
                        {
                            writer.Write(outgoingCmd);
                        }
                    }
                }
            }
            catch (System.IO.IOException)
            {
            }
            finally
            {
                _players.Remove(_players.Find(p => p.Value == client));
                ConnectionsChange?.Invoke(this, new ConnectionsNumberChangeEventArgs(_players.Count()));
                client.Close();
            }
        }

        public bool isAlive => _listener.Server.IsBound;

        public void Stop()
        {
            foreach (var player in _players)
            {
                player.Value.Close();
            }
            _players.Clear();
            _listener.Stop();
        }
    }
}
