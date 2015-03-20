using DSmoove.Core.Handlers;
using DSmoove.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DSmoove.Core.Managers
{
    public class ConnectionManager : IHandlePeerConnections
    {
        private List<PeerHandler> _peerHandlers;

        public ConnectionManager()
        {
            _peerHandlers = new List<PeerHandler>();
        }

        public void Start()
        {
            throw new NotImplementedException();
        }

        public void AddPeer(IPAddress address, int port, string peerId = null)
        {

        }
    }
}
