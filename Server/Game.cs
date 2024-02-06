using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;

namespace Server
{
    class Game
    {
        public Player Player1 { get; }
        public Player? Player2 { get; private set; }

        public string GameId { get; private set; }

        private string[] _field = new string[9];

        private string _turn;

        public Game(string player1Name)
        {
            Player1 = new Player(player1Name);
            GameId = Guid.NewGuid().ToString();
            _turn = Player1.Id;
        }

        public void joinPlayer2(string playerName)
        {
            Player2 = new Player(playerName);
        }

        public List<Player> listPlayers()
        {
            List<Player> list = new List<Player>();
            list.Add(Player1);
            if (Player2 != null)
            {
                list.Add(Player2);
            }
            return list;
        }

        void makeMove(string playerId, short number)
        {
            if (_turn != playerId) 
            {
                throw new Exception("Not your turn!");
            }

            if (number < 1 || number > 9)
            {
                throw new ArgumentException("Invalid number");
            }


            if (_field[number - 1] != null)
            {
                throw new InvalidOperationException($"This number is already played by {_field[number - 1]}");
            }

            _field[number - 1] = playerId;
        }
    }
}
