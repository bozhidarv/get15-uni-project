using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class ConnectionsNumberChangeEventArgs : EventArgs
    {
        public int Connections { get; }

        public ConnectionsNumberChangeEventArgs(int connections)
        {
            Connections = connections;
        }
    }
}
