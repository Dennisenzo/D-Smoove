using DSmoove.Core.Connections;
using DSmoove.Core.Interfaces;
using DSmoove.Core.PeerCommands;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DSmoove.Core.Handlers
{
    public class PeerHandler : IHandlePeer, IHandlePeerMessages
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private PeerConnection _connection;

        public PeerHandler(TcpClient tcpClient)
        {
            _connection = new PeerConnection(tcpClient, this);
        }

        public PeerHandler(IPAddress ipAddress, int port)
        {
            _connection = new PeerConnection(ipAddress, port, this);
        }

        public async Task Start()
        {
            await _connection.Connect();
        }

        #region IHandlePeer

        public bool AmChoking { get; private set; }
        public bool AmInterested { get; private set; }
        public bool IsChoking { get; private set; }
        public bool IsInterested { get; private set; }

        public void Connect()
        {
            throw new NotImplementedException();
        }

        public void Disconnect()
        {
            throw new NotImplementedException();
        }

        public void Choke()
        {
            throw new NotImplementedException();
        }

        public void Unchoke()
        {
            throw new NotImplementedException();
        }

        public void Interested()
        {
            throw new NotImplementedException();
        }

        public void NotInterested()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IHandlePeerMessages

        public void HandleHandshakeMessage(byte[] messageData)
        {
            throw new NotImplementedException();
        }

        public void HandlePeerMessage(byte[] messageData)
        {
            if (messageData == null)
            {
                if (KeepAliveReceivedEvent != null)
                {
                    log.DebugFormat("Received KeepAlive message from {0}:{1}.", _connection.Address.ToString(), _connection.Port);
                    KeepAliveReceivedEvent();
                }
            }
            else
            {
                switch ((PeerCommandId)messageData[0])
                {
                    case PeerCommandId.Choke:
                        {
                            if (ChokeReceivedEvent != null)
                            {
                                IsChoking = true;
                                log.DebugFormat("Received Choke message from {0}:{1}.", _connection.Address.ToString(), _connection.Port);
                                ChokeReceivedEvent();
                            }
                            break;
                        }
                    case PeerCommandId.Unchoke:
                        {
                            if (UnchokeReceivedEvent != null)
                            {
                                IsChoking = false;
                                log.DebugFormat("Received Unchoke message from {0}:{1}.", _connection.Address.ToString(), _connection.Port);
                                UnchokeReceivedEvent();
                            }
                            break;
                        }
                    case PeerCommandId.Interested:
                        {
                            if (InterestedReceivedEvent != null)
                            {
                                IsInterested = true;
                                log.DebugFormat("Received Interested message from {0}:{1}.", _connection.Address.ToString(), _connection.Port);
                                InterestedReceivedEvent();
                            }
                            break;
                        }
                    case PeerCommandId.NotInterested:
                        {
                            if (NotInterestedReceivedEvent != null)
                            {
                                IsInterested = false;
                                log.DebugFormat("Received NotInterested message from {0}:{1}.", _connection.Address.ToString(), _connection.Port);
                                NotInterestedReceivedEvent();
                            }
                            break;
                        }
                    case PeerCommandId.BitField:
                        {
                            if (BitFieldReceivedEvent != null)
                            {
                                log.DebugFormat("Received BitField message from {0}:{1}.", _connection.Address.ToString(), _connection.Port);

                                BitFieldCommand bitField = new BitFieldCommand();

                                bitField.FromByteArray(messageData);

                                BitFieldReceivedEvent(bitField);
                            }
                            break;
                        }
                    case PeerCommandId.Have:
                        {
                            if (HaveReceivedEvent != null)
                            {
                                log.DebugFormat("Received Have message from {0}:{1}.", _connection.Address.ToString(), _connection.Port);

                                HaveCommand have = new HaveCommand();

                                have.FromByteArray(messageData);

                                HaveReceivedEvent(have);
                            }
                            break;
                        }

                    case PeerCommandId.Request:
                        {
                            if (RequestReceivedEvent != null)
                            {
                                log.DebugFormat("Received Request message from {0}:{1}.", _connection.Address.ToString(), _connection.Port);

                                RequestCommand request = new RequestCommand();

                                request.FromByteArray(messageData);

                                RequestReceivedEvent(request);
                            }
                            break;
                        }

                    default:
                        {
                            log.Warn("Weird data received!");
                            break;
                        }
                }
            }
        }

        #endregion

        #region Events

        public event PeerConnected PeerConnectedEvent;
        public event PeerDisconnected PeerDisconnectedEvent;

        public event KeepAliveReceived KeepAliveReceivedEvent;
        public event ChokeReceived ChokeReceivedEvent;
        public event UnchokeReceived UnchokeReceivedEvent;
        public event InterestedReceived InterestedReceivedEvent;
        public event NotInterestedReceived NotInterestedReceivedEvent;

        public event HandShakeReceived HandShakeReceivedEvent;
        public event BitFieldReceived BitFieldReceivedEvent;
        public event HaveReceived HaveReceivedEvent;
        public event RequestReceived RequestReceivedEvent;

        #endregion
    }
}