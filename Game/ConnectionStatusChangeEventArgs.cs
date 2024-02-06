using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    internal class ConnectionStatusChangeEventArgs : EventArgs
    {
        public bool IsConnected { get; }

        public ConnectionStatusChangeEventArgs(bool isConnected)
        {
            IsConnected = isConnected;
        }
    }
}
