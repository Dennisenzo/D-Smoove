//using DSmoove.Core.Connections;
//using DSmoove.Core.Entities;
//using DSmoove.Core.Handlers;
//using DSmoove.Core.PeerCommands;
//using log4net;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Net.Sockets;
//using System.Reflection;
//using System.Text;
//using System.Threading.Tasks;
//using System.Timers;

//namespace DSmoove.Core.Managers
//{
//    public class OldTransferManager
//    {
//        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

//        private Torrent _torrent;

//        private Timer _commandTimer;

//        private IncomingPeerConnection _incomingConnections;

//        private List<PeerHandler> _peerHandlers;

//        private FileManager _fileManager;

//                private int _maxConnections = 10;

//        public OldTransferManager(Torrent torrent, FileManager fileManager)
//        {
//            _torrent = torrent;
//            _fileManager = fileManager;
//            _peerHandlers = new List<PeerHandler>();
//            _incomingConnections = new IncomingPeerConnection();
//            _incomingConnections.NewConnectionEvent += SetupIncomingPeerHandler;
//        }

//        public void Start()
//        {
//            _incomingConnections.StartListening();
//            _commandTimer = new Timer(10000);
//            _commandTimer.Elapsed += CommandTimerElapsed;
//            _commandTimer.AutoReset = false;
//            _commandTimer.Start();
//        }

//        private void CommandTimerElapsed(object sender, ElapsedEventArgs e)
//        {
//            Task connectTask = ConnectPeers();
//            Task interestedTask = SetInterested();
//            Task chokeTask = SetChoke();
            
//            Task.WaitAll(connectTask, interestedTask, chokeTask);

//            _commandTimer.Start();
//        }

//        private Task ConnectPeers()
//        {
//            return Task.Run(() =>
//            {
//                List<Task> connectTasks = new List<Task>();
//                var disconnectedPeers = _torrent.Peers.Where(p => p.Status == PeerStatus.Disconnected);

//                foreach (var peer in disconnectedPeers)
//                {
//                    int connected = _torrent.Peers.Count(p => p.Status != PeerStatus.Disconnected && p.Status != PeerStatus.Failed);

//                        if (connected >= _maxConnections)
//                    {
//                        return;
//                    }

//                    connectTasks.Add(SetupOutgoingPeerHandler(peer));
//                }

//                Task.WaitAll(connectTasks.ToArray());
//            });
//        }

//        private async Task SetInterested()
//        {
//            foreach (var peer in _torrent.Peers.Where(p => p.Status == PeerStatus.Connected))
//            {
//                var interestedIndexes = peer.GetInterestedPieces(_torrent.BitField);

//                if (interestedIndexes.Count > 0)
//                {
//                    var peerConnection = _peerHandlers.Single(p => p.Peer == peer);
//                    await peerConnection.SetInterested(true);
//                }
//                else if (interestedIndexes.Count == 0)
//                {
//                    var peerConnection = _peerHandlers.Single(p => p.Peer == peer);
//                    await peerConnection.SetInterested(false);
//                }
//            }
//            int interested = _peerHandlers.Count(c => c.AmInterested == true);
//            log.DebugFormat("We are currently interested in {0} of {1} peers.", interested, _peerHandlers.Count);

//            interested = _peerHandlers.Count(c => c.IsInterested == true);
//            log.DebugFormat("Currently, {0} of {1} peers are interested in us.", interested, _peerHandlers.Count);
//        }

//        private async Task SetChoke()
//        {
//            foreach (var peer in _torrent.Peers.Where(p => p.Status == PeerStatus.Connected))
//            {
//                 var peerHandler = _peerHandlers.Single(p => p.Peer == peer);
//                 await peerHandler.SetChoke(!peerHandler.IsInterested);
//            }

//            int choking = _peerHandlers.Count(c => c.AmChoking == true);
//            log.DebugFormat("We are currently choking {0} of {1} peers", choking, _peerHandlers.Count);

//            choking = _peerHandlers.Count(c => c.IsChoking == true);
//            log.DebugFormat("Currently, {0} of {1} peers are choking us.", choking, _peerHandlers.Count);
//        }

//        private Task SetupIncomingPeerHandler(TcpClient client)
//        {
//            var address = ((IPEndPoint)client.Client.RemoteEndPoint).Address;
//            var port = ((IPEndPoint)client.Client.RemoteEndPoint).Port;

//            Peer peer = _torrent.AddAndGetPeer(address, port);

//            return SetupIncomingPeerHandler(peer, client);
//        }

//        private Task SetupIncomingPeerHandler(Peer peer, TcpClient client)
//        {
//            PeerHandler peerHandler = new PeerHandler(peer, client, _torrent.Metadata.Hash);

//            _peerHandlers.Add(peerHandler);

//            return SetupPeerHandler(peerHandler);
//        }

//        private Task SetupOutgoingPeerHandler(Peer peer)
//        {
//            PeerHandler peerHandler = new PeerHandler(peer, _torrent.Metadata.Hash);

//            _peerHandlers.Add(peerHandler);

//            return SetupPeerHandler(peerHandler);
//        }

//        private async Task SetupPeerHandler(PeerHandler peerHandler)
//        {
//            //peerHandler.HandShakeReceivedEvent += HandshakeReceived;
//            //peerHandler.ChokeReceivedEvent += ChokeReceived;
//            //peerHandler.UnchokeReceivedEvent += UnchokeReceived;
//            //peerHandler.InterestedReceivedEvent += InterestedReceived;
//            //peerHandler.NotInterestedReceivedEvent += NotInterestedReceived;
//            //peerHandler.HaveReceivedEvent += HaveReceived;
//            //peerHandler.BitFieldReceivedEvent += BitFieldReceived;
//            //peerHandler.RequestReceivedEvent += RequestReceived;

//            //await peerHandler.Start();
//        }

//        #region Events

//        private void BitFieldReceived(BitFieldCommand bitField)
//        {

//        }

//        private void HaveReceived(HaveCommand have)
//        {
//            var piece = _torrent.Pieces.ByIndex(have.PieceIndex);

//            piece.Availability++;
//        }

//        private void NotInterestedReceived()
//        {

//        }

//        private void InterestedReceived()
//        {

//        }

//        private void UnchokeReceived()
//        {

//        }

//        private void ChokeReceived()
//        {

//        }

//        private void HandshakeReceived(HandshakeCommand handshake)
//        {

//        }

//        private void RequestReceived(RequestCommand request)
//        {

//        }

//        #endregion
//    }
//}