using DSmoove.Core.Connections;
using DSmoove.Core.Entities;
using DSmoove.Core.Handlers;
using DSmoove.Core.PeerCommands;
using log4net;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace DSmoove.Core.Managers
{
    public class TransferManager
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private Torrent _torrent;

        private Timer _commandTimer;

        private IncomingConnection _incomingConnections;

        private List<PeerHandler> _peerHandlers;

        private FileManager _fileManager;

        public TransferManager(Torrent torrent, FileManager fileManager)
        {
            _torrent = torrent;
            _fileManager = fileManager;
            _peerHandlers = new List<PeerHandler>();
            _incomingConnections = new IncomingConnection();
            _incomingConnections.NewConnectionEvent += SetupIncomingPeerHandler;
        }

        public void Start()
        {
            _incomingConnections.StartListening();
            foreach (var peer in _torrent.Peers.Where(p => p.Status == PeerStatus.Disconnected))
            {
                Task.Factory.StartNew(() =>
                    {
                        SetupOutgoingPeerHandler(peer.IpAddress, peer.Port);
                    });
            }

            _commandTimer = new Timer(10000);
            _commandTimer.Elapsed += CommandTimerElapsed;
            _commandTimer.AutoReset = false;
            _commandTimer.Start();
        }

        private void CommandTimerElapsed(object sender, ElapsedEventArgs e)
        {
            SetInterested();
            SetChoke();
            _commandTimer.Start();
        }

        private async void SetInterested()
        {
            //foreach (var peer in _torrent.Peers.Where(p => p.Status == PeerStatus.Connected))
            //{
            //    var interestedIndexes = peer.GetInterestedPieces(_torrent.BitField);
            //    var peerConnection = _connections.Single(p => p.Peer == peer);

            //    if (interestedIndexes.Count > 0 && !peerConnection.AmInterested)
            //    {
            //        await peerConnection.SetInterested(true);
            //    }
            //    else if (interestedIndexes.Count == 0 && peerConnection.AmInterested)
            //    {
            //        await peerConnection.SetInterested(false);
            //    }
            //}
            //int interested = _connections.Count(c => c.AmInterested == true);
            //log.DebugFormat("We are currently interested in {0} of {1} peers.", interested, _connections.Count);

            //interested = _connections.Count(c => c.IsInterested == true);
            //log.DebugFormat("Currently, {0} of {1} peers are interested in us.", interested, _connections.Count);
        }

        private async void SetChoke()
        {
            //foreach (var peer in _torrent.Peers.Where(p => p.Status == PeerStatus.Connected))
            //{
            //     var peerConnection = _connections.Single(p => p.Peer == peer);
            //     await peerConnection.SetChoke(!peerConnection.IsInterested);
            //}

            //int choking = _connections.Count(c => c.AmChoking == true);
            //log.DebugFormat("We are currently choking {0} of {1} peers", choking, _connections.Count);

            //choking = _connections.Count(c => c.IsChoking == true);
            //log.DebugFormat("Currently, {0} of {1} peers are choking us.", choking, _connections.Count);
        }
        private async Task SetupIncomingPeerHandler(TcpClient client)
        {
            PeerHandler peerHandler = new PeerHandler(client);

            _peerHandlers.Add(peerHandler);

            await SetupPeerHandler(peerHandler);
        }

        private async Task SetupOutgoingPeerHandler(IPAddress ipAddress, int port)
        {
            PeerHandler peerHandler = new PeerHandler(ipAddress, port);

            _peerHandlers.Add(peerHandler);

            await SetupPeerHandler(peerHandler);
        }

        private async Task SetupPeerHandler(PeerHandler peerHandler)
        {
            peerHandler.HandShakeReceivedEvent += HandshakeReceived;
            peerHandler.ChokeReceivedEvent += ChokeReceived;
            peerHandler.UnchokeReceivedEvent += UnchokeReceived;
            peerHandler.InterestedReceivedEvent += InterestedReceived;
            peerHandler.NotInterestedReceivedEvent += NotInterestedReceived;
            peerHandler.HaveReceivedEvent += HaveReceived;
            peerHandler.BitFieldReceivedEvent += BitFieldReceived;
            peerHandler.RequestReceivedEvent += RequestReceived;

            await peerHandler.Start();
        }

        #region Events

        private void BitFieldReceived(BitFieldCommand bitField)
        {
            //Peer peer = _torrent.Peers.Single(p => p.Id == peerId);
            //  peer.BitField = bitField.Downloaded;
        }

        private void HaveReceived(HaveCommand have)
        {
            //  Peer peer = _torrent.Peers.Single(p => p.Id == peerId);
            // peer.SetDownloaded(have.PieceIndex);

            var piece = _torrent.Pieces.ByIndex(have.PieceIndex);

            piece.Availability++;

            //log.DebugFormat("Peer {0} has downloaded {1:P2}", peerId, peer.GetPercentageDownloaded());
        }

        private void NotInterestedReceived()
        {

        }

        private void InterestedReceived()
        {

        }

        private void UnchokeReceived()
        {

        }

        private void ChokeReceived()
        {

        }

        private void HandshakeReceived(HandshakeCommand handshake)
        {

        }

        private void RequestReceived(RequestCommand request)
        {

        }

        #endregion
    }
}