using DSmoove.Core.Config;
using DSmoove.Core.Connections;
using DSmoove.Core.Entities;
using DSmoove.Core.Handlers;
using DSmoove.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DSmoove.Core.Managers
{
    public class ConnectionManager : IProvidePeers
    {
        private List<PeerHandler> _peerHandlers;

        private Task _connectionTask;
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        

        public ConnectionManager(IProvideTrackerUpdates trackerUpdatesProvider)
        {
            _peerHandlers = new List<PeerHandler>();
            trackerUpdatesProvider.TrackerUpdateSubscription.Subscribe(UpdatePeersFromTracker);
        }

        public void Start()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _connectionTask = MaintainConnections();
        }

        public void Stop()
        {
            _cancellationTokenSource.Cancel();
        }

        private Task MaintainConnections()
        {
            return Task.Factory.StartNew((t) =>
            {
                while (!_cancellationTokenSource.Token.IsCancellationRequested)
                {
                    if (GetNumberOfConnectedPeers() < Settings.Connection.MaxPeerConnections)
                    {
                      var newConnection =  _peerHandlers.FirstOrDefault(p => p.Status == PeerConnectionStatus.Disconnected);

                        if (newConnection != null)
                        {
                           Task<bool> connectionTask = newConnection.Connect();
                        }
                        else
                        {
                            Task.WaitAll(Task.Delay(10000));
                            continue;
                        }
                    }
                    else
                    {
                        Task.WaitAll(Task.Delay(10000));
                    }
                }
            }, _cancellationTokenSource.Token, TaskCreationOptions.LongRunning);
        }
        
        private void UpdatePeersFromTracker(TrackerData data, IProvideTrackerUpdates source)
        {
            foreach (var peer in data.Peers)
            {
                AddPeer(peer.IPAddress, peer.Port, data.InfoHash, peer.PeerId);
            }
        }

        public void AddPeer(IPAddress address, int port, byte[] infoHash, string peerId)
        {
            if (!_peerHandlers.Any(p => p.IPAddress == address && p.Port == port))
            {
                PeerHandler peerHandler = new PeerHandler(address, port, peerId, infoHash);
                _peerHandlers.Add(peerHandler);
            }
        }

        private int GetNumberOfConnectedPeers()
        {
           return _peerHandlers.Count(p => p.Status != PeerConnectionStatus.Disconnected);
        }
    }
}
