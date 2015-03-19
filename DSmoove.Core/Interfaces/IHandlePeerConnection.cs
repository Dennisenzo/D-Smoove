using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSmoove.Core.Interfaces
{
    public delegate void PeerConnected();
    public delegate void PeerDisconnected();

    public interface IHandlePeerConnection
    {
        event PeerConnected PeerConnectedEvent;
        event PeerDisconnected PeerDisconnectedEvent;
    }
}
