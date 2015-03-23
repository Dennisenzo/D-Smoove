using DSmoove.Core.Entities;
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

        public ConnectionManager(IProvideTrackerUpdates trackerUpdatesProvider)
        {
            _peerHandlers = new List<PeerHandler>();
            trackerUpdatesProvider.TrackerUpdateSubscription.Subscribe(UpdatePeersFromTracker);
        }

        public void Start()
        {
            throw new NotImplementedException();
        }
        
        private void UpdatePeersFromTracker(TrackerData data, IProvideTrackerUpdates source)
        {
            foreach (var peer in data.Peers)
            {
                AddPeer(peer.IPAddress, peer.Port, peer.PeerId);
            }
        }

        public void AddPeer(IPAddress address, int port, string peerId = null)
        {

        }
    }
}
