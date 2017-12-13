using DSmoove.Core.Config;
using DSmoove.Core.Connections;
using DSmoove.Core.Entities;
using DSmoove.Core.Handlers;
using DSmoove.Core.Helpers;
using DSmoove.Core.Interfaces;
using EasyMemoryRepository;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DSmoove.Core.Managers
{
    public class ConnectionManager : IProvidePeerConnections
    {
        private Guid _torrentId;

        private List<PeerHandler> _peerHandlers;

        public IEnumerable<IHandlePeerDownloads> DownloadHandlers
        {
            get
            {
                return _peerHandlers.Where(p => p.Status == PeerConnectionStatus.Connected).Select<PeerHandler, IHandlePeerDownloads>(p => p as IHandlePeerDownloads);
            }
        }

        public IEnumerable<IHandlePeerUploads> UploadHandlers
        {
            get
            {
                return _peerHandlers.Where(p => p.Status == PeerConnectionStatus.Connected).Select<PeerHandler, IHandlePeerUploads>(p => p as IHandlePeerUploads);
            }
        }

        private Task _connectionTask;
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        public AsyncSubscription<IHandlePeerConnection> PeerAddedSubscription { get; private set; }
        public AsyncSubscription<IHandlePeerDownloads, IProvidePeerConnections> PeerReadyForDownloadSubscription { get; private set; }
        public AsyncSubscription<IHandlePeerUploads, IProvidePeerConnections> PeerReadyForUploadSubscription { get; private set; }

        [Inject]
        public IProvideTrackerUpdates TrackerUpdateProvider { get; set; }
        

        public ConnectionManager(Guid torrentId)
        {
            _torrentId = torrentId;

            PeerAddedSubscription = new AsyncSubscription<IHandlePeerConnection>();
            PeerReadyForDownloadSubscription = new AsyncSubscription<IHandlePeerDownloads, IProvidePeerConnections>();
            PeerReadyForUploadSubscription = new AsyncSubscription<IHandlePeerUploads, IProvidePeerConnections>();
            _peerHandlers = new List<PeerHandler>();
        }

        public void Start()
        {
            TrackerUpdateProvider.UpdateSubscription.Subscribe(UpdatePeersFromTracker);
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
                           connectionTask.ContinueWith((c) =>
                               {
                                   PeerReadyForUploadSubscription.TriggerAsync(newConnection, this);
                                   PeerReadyForDownloadSubscription.TriggerAsync(newConnection, this);
                               });
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
                PeerHandler peerHandler = new PeerHandler(_torrentId, address, port, peerId);
                _peerHandlers.Add(peerHandler);
                PeerAddedSubscription.Trigger(peerHandler);
            }
        }

        private int GetNumberOfConnectedPeers()
        {
           return _peerHandlers.Count(p => p.Status != PeerConnectionStatus.Disconnected);
        }
    }
}
