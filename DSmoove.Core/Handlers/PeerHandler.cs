using DSmoove.Core.Connections;
using DSmoove.Core.Entities;
using DSmoove.Core.Helpers;
using DSmoove.Core.Interfaces;
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

namespace DSmoove.Core.Handlers
{
    public class PeerHandler : IHandlePeerConnection, IHandlePeerUploads, IHandlePeerDownloads
    {
        #region Properties and Fields

        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public IPAddress IPAddress { get; private set; }
        public int Port { get; private set; }
        public string PeerId { get; private set; }

        private PeerConnection _peerConnection;

        #endregion

        #region Constructors

        public PeerHandler(IPAddress ipAddress, int port, string peerId)
        {
            _peerConnection = new PeerConnection(ipAddress, port);
            _peerConnection.PeerHandshakeSubscription.Subscribe(HandleHandshakeMessage);
            _peerConnection.PeerMessageSubscription.Subscribe(HandlePeerMessage);
            PeerId = peerId;

            HandshakeCommandSubscription = new AsyncSubscription<IHandlePeerConnection, HandshakeCommand>();
            PortCommandSubscription = new AsyncSubscription<IHandlePeerConnection, PortCommand>();
            ChokeCommandSubscription = new AsyncSubscription<IHandlePeerConnection, ChokeCommand>();
            UnchokeCommandSubscription = new AsyncSubscription<IHandlePeerConnection, UnchokeCommand>();
            InterestedCommandSubscription = new AsyncSubscription<IHandlePeerConnection, InterestedCommand>();
            NotInterestedCommandSubscription = new AsyncSubscription<IHandlePeerConnection, NotInterestedCommand>();

            RequestCommandSubscription = new AsyncSubscription<IHandlePeerUploads, RequestCommand>();
            CancelCommandSubscription = new AsyncSubscription<IHandlePeerUploads, CancelCommand>();

            BitFieldCommandSubscription = new AsyncSubscription<IHandlePeerDownloads, BitFieldCommand>();
            HaveCommandSubscription = new AsyncSubscription<IHandlePeerDownloads, HaveCommand>();
            PieceCommandSubscription = new AsyncSubscription<IHandlePeerDownloads, PieceCommand>();
        }

        #endregion

        #region IHandlePeerMessages

        public void HandlePeerMessage(IProvidePeerMessages source, byte[] messageData)
        {
            throw new NotImplementedException();
        }

        public void HandleHandshakeMessage(IProvidePeerMessages source, byte[] handshakeData)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IHandlePeerConnection

        public void SendHandshakeCommand(HandshakeCommand command)
        {
            throw new NotImplementedException();
        }

        public void SendPortCommand(PortCommand command)
        {
            throw new NotImplementedException();
        }

        public void SendChokeCommand(ChokeCommand command)
        {
            throw new NotImplementedException();
        }

        public void SendUnchokeCommand(UnchokeCommand command)
        {
            throw new NotImplementedException();
        }

        public void SendInterestedCommand(InterestedCommand command)
        {
            throw new NotImplementedException();
        }

        public void SendNotInterestedCommand(NotInterestedCommand command)
        {
            throw new NotImplementedException();
        }

        public AsyncSubscription<IHandlePeerConnection, HandshakeCommand> HandshakeCommandSubscription { get; private set; }
        public AsyncSubscription<IHandlePeerConnection, PortCommand> PortCommandSubscription { get; private set; }
        public AsyncSubscription<IHandlePeerConnection, ChokeCommand> ChokeCommandSubscription { get; private set; }
        public AsyncSubscription<IHandlePeerConnection, UnchokeCommand> UnchokeCommandSubscription { get; private set; }
        public AsyncSubscription<IHandlePeerConnection, InterestedCommand> InterestedCommandSubscription { get; private set; }
        public AsyncSubscription<IHandlePeerConnection, NotInterestedCommand> NotInterestedCommandSubscription { get; private set; }

        #endregion

        #region IHandlePeerUploads

        public void SendBitfieldCommand(BitFieldCommand command)
        {
            throw new NotImplementedException();
        }

        public void SendHaveCommand(HaveCommand command)
        {
            throw new NotImplementedException();
        }

        public void SendPieceCommand(PieceCommand command)
        {
            throw new NotImplementedException();
        }

        public AsyncSubscription<IHandlePeerUploads, RequestCommand> RequestCommandSubscription { get; private set; }
        public AsyncSubscription<IHandlePeerUploads, CancelCommand> CancelCommandSubscription { get; private set; }

        #endregion

        #region IHandlePeerDownloads

        public void SendRequestCommand(RequestCommand command)
        {
            throw new NotImplementedException();
        }

        public void SendCancelCommand(CancelCommand command)
        {
            throw new NotImplementedException();
        }

        public AsyncSubscription<IHandlePeerDownloads, BitFieldCommand> BitFieldCommandSubscription { get; private set; }
        public AsyncSubscription<IHandlePeerDownloads, HaveCommand> HaveCommandSubscription { get; private set; }
        public AsyncSubscription<IHandlePeerDownloads, PieceCommand> PieceCommandSubscription { get; private set; }

        #endregion
    }
}

//        private PeerConnection _connection;

//        public Peer Peer { get; private set; }
//        private byte[] _torrentHash;
//        private bool _incoming;

//        public PeerHandler(Peer peer, TcpClient tcpClient, byte[] torrentHash)
//        {
//            Peer = peer;
//            _torrentHash = torrentHash;
//            _connection = new PeerConnection(tcpClient, this);
//            _incoming = true;
//        }

//        public PeerHandler(Peer peer, byte[] torrentHash)
//        {
//            Peer = peer;
//            _torrentHash = torrentHash;
//            _connection = new PeerConnection(Peer.IpAddress, Peer.Port, this);
//            _incoming = false;

//            _connection.PeerConnectedEvent += PeerIsConnected;
//            _connection.PeerDisconnectedEvent += PeerIsDisconnected;
//        }

//        private void PeerIsDisconnected()
//        {
//            Peer.Status = PeerStatus.Failed;
//        }

//        private void PeerIsConnected()
//        {
//            Peer.Status = PeerStatus.Connected;

//            Task handShakeTask = SendHandshake();
//            Task bitFieldTask = SendBitfield();

//           // task.Wait();
//        }

//        private async Task SendBitfield()
//        {
//            log.DebugFormat("Sending Handshake to {0}:{1}.", Peer.IpAddress.ToString(), Peer.Port);
//        }

//        private async Task SendHandshake()
//        {
//            log.DebugFormat("Sending Handshake to {0}:{1}.", Peer.IpAddress.ToString(), Peer.Port);
//            HandshakeCommand handshake = new HandshakeCommand()
//                {
//                    InfoHash = _torrentHash
//                };
//            await _connection.SendAsync(handshake.ToByteArray());
//        }

//        public async Task Start()
//        {
//            Peer.Status = PeerStatus.Connecting;
//            await _connection.Connect();
//        }

//        #region IHandlePeer

//        public bool AmChoking { get; private set; }
//        public bool AmInterested { get; private set; }
//        public bool IsChoking { get; private set; }
//        public bool IsInterested { get; private set; }

//        public void Connect()
//        {
//            throw new NotImplementedException();
//        }

//        public void Disconnect()
//        {
//            throw new NotImplementedException();
//        }

//        public void Choke()
//        {
//            throw new NotImplementedException();
//        }

//        public void Unchoke()
//        {
//            throw new NotImplementedException();
//        }

//        public void Interested()
//        {
//            throw new NotImplementedException();
//        }

//        public void NotInterested()
//        {
//            throw new NotImplementedException();
//        }

//        #endregion

//        #region IHandlePeerMessages

//        public void HandleHandshakeMessage(byte[] messageData)
//        {
//            HandshakeCommand handShake = new HandshakeCommand();
//            handShake.FromByteArray(messageData);

//            log.DebugFormat("Received Handshake message from {0}:{1}.", Peer.IpAddress.ToString(), Peer.Port);

//            if (HandShakeReceivedEvent != null)
//            {
//                HandShakeReceivedEvent(handShake);
//            }
//            if (_incoming)
//            {
//                SendHandshake();
//            }
//        }

//        public void HandlePeerMessage(byte[] messageData)
//        {
//            if (messageData == null)
//            {
//                if (KeepAliveReceivedEvent != null)
//                {
//                    log.DebugFormat("Received KeepAlive message from {0}:{1}.", _connection.Address.ToString(), _connection.Port);
//                    KeepAliveReceivedEvent();
//                }
//            }
//            else
//            {
//                switch ((PeerCommandId)messageData[0])
//                {
//                    case PeerCommandId.Choke:
//                        {
//                            if (ChokeReceivedEvent != null)
//                            {
//                                IsChoking = true;
//                                log.DebugFormat("Received Choke message from {0}:{1}.", _connection.Address.ToString(), _connection.Port);
//                                ChokeReceivedEvent();
//                            }
//                            break;
//                        }
//                    case PeerCommandId.Unchoke:
//                        {
//                            if (UnchokeReceivedEvent != null)
//                            {
//                                IsChoking = false;
//                                log.DebugFormat("Received Unchoke message from {0}:{1}.", _connection.Address.ToString(), _connection.Port);
//                                UnchokeReceivedEvent();
//                            }
//                            break;
//                        }
//                    case PeerCommandId.Interested:
//                        {
//                            if (InterestedReceivedEvent != null)
//                            {
//                                IsInterested = true;
//                                log.DebugFormat("Received Interested message from {0}:{1}.", _connection.Address.ToString(), _connection.Port);
//                                InterestedReceivedEvent();
//                            }
//                            break;
//                        }
//                    case PeerCommandId.NotInterested:
//                        {
//                            if (NotInterestedReceivedEvent != null)
//                            {
//                                IsInterested = false;
//                                log.DebugFormat("Received NotInterested message from {0}:{1}.", _connection.Address.ToString(), _connection.Port);
//                                NotInterestedReceivedEvent();
//                            }
//                            break;
//                        }
//                    case PeerCommandId.BitField:
//                        {
//                            if (BitFieldReceivedEvent != null)
//                            {
//                                log.DebugFormat("Received BitField message from {0}:{1}.", _connection.Address.ToString(), _connection.Port);

//                                BitFieldCommand bitField = new BitFieldCommand();

//                                bitField.FromByteArray(messageData);

//                                Peer.BitField = bitField.Downloaded;

//                                log.DebugFormat("Peer {0} has downloaded {1:P2}", Peer.Id, Peer.GetPercentageDownloaded());

//                                BitFieldReceivedEvent(bitField);
//                            }
//                            break;
//                        }
//                    case PeerCommandId.Have:
//                        {
//                            if (HaveReceivedEvent != null)
//                            {
//                                log.DebugFormat("Received Have message from {0}:{1}.", _connection.Address.ToString(), _connection.Port);

//                                HaveCommand have = new HaveCommand();

//                                have.FromByteArray(messageData);

//                                Peer.SetDownloaded(have.PieceIndex);

//                                log.DebugFormat("Peer {0} has downloaded {1:P2}", Peer.Id, Peer.GetPercentageDownloaded());

//                                HaveReceivedEvent(have);
//                            }
//                            break;
//                        }

//                    case PeerCommandId.Request:
//                        {
//                            if (RequestReceivedEvent != null)
//                            {
//                                log.DebugFormat("Received Request message from {0}:{1}.", _connection.Address.ToString(), _connection.Port);

//                                RequestCommand request = new RequestCommand();

//                                request.FromByteArray(messageData);

//                                RequestReceivedEvent(request);
//                            }
//                            break;
//                        }

//                    default:
//                        {
//                            log.Warn("Weird data received!");
//                            break;
//                        }
//                }
//            }
//        }

//        #endregion

//        public async Task SetInterested(bool interested)
//        {
//            if (interested != AmInterested)
//            {
//                AmInterested = interested;

//                await SendInterestedAsync();
//            }
//        }

//        public async Task SetChoke(bool choke)
//        {
//            if (choke != AmChoking)
//            {
//                AmChoking = choke;
//                await SendChokeAsync();
//            }
//        }

//        #region Private Send Commands

//        private async Task SendInterestedAsync()
//        {
//            if (AmInterested)
//            {
//                log.DebugFormat("Sending Interested to {0}:{1}.", _connection.Address.ToString(), _connection.Port);

//                InterestedCommand command = new InterestedCommand();

//                await _connection.SendAsync(command.ToByteArray());
//            }
//            else
//            {
//                log.DebugFormat("Sending Not Interested to {0}:{1}.", _connection.Address.ToString(), _connection.Port);

//                NotInterestedCommand command = new NotInterestedCommand();

//                await _connection.SendAsync(command.ToByteArray());
//            }
//        }
//        private async Task SendChokeAsync()
//        {
//            if (AmChoking)
//            {
//                log.DebugFormat("Sending Unchoke to {0}:{1}.", _connection.Address.ToString(), _connection.Port);

//                UnchokeCommand command = new UnchokeCommand();

//                await _connection.SendAsync(command.ToByteArray());
//            }
//            else
//            {
//                log.DebugFormat("Sending Choke to {0}:{1}.", _connection.Address.ToString(), _connection.Port);

//                ChokeCommand command = new ChokeCommand();

//                await _connection.SendAsync(command.ToByteArray());
//            }
//        }

//        #endregion

//        #region Events

//        public event PeerConnected PeerConnectedEvent;
//        public event PeerDisconnected PeerDisconnectedEvent;

//        public event KeepAliveReceived KeepAliveReceivedEvent;
//        public event ChokeReceived ChokeReceivedEvent;
//        public event UnchokeReceived UnchokeReceivedEvent;
//        public event InterestedReceived InterestedReceivedEvent;
//        public event NotInterestedReceived NotInterestedReceivedEvent;

//        public event HandShakeReceived HandShakeReceivedEvent;
//        public event BitFieldReceived BitFieldReceivedEvent;
//        public event HaveReceived HaveReceivedEvent;
//        public event RequestReceived RequestReceivedEvent;

//        #endregion
//    }
//}