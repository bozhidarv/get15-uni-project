using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Player
    {
        private string name;
        public string Name
        { 
            get => name;
            set
            {
                name = value;
            }    
        }

        private readonly string id;
        public string Id { get => id; }

        public Player()
        {
            this.id = Guid.NewGuid().ToString();
            this.name = string.Empty;
        }

        public Player(string name)
        {
            this.name = name;
            this.id = Guid.NewGuid().ToString();
        }
    }
}
