using DSmoove.Core.PeerCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSmoove.Core.Interfaces
{
    public interface IHandlePeerMessages
    {
        void HandlePeerMessage(byte[] messageData);
       void HandleHandshakeMessage(byte[] handshakeData);
    }
}