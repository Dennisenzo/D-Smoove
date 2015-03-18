using DSmoove.Core.Entities;
using DSmoove.Core.PeerCommands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using DSmoove.Core.Extensions;
using log4net;
using System.Reflection;
using System.Collections.Concurrent;
using DSmoove.Core.Interfaces;

namespace DSmoove.Core.Connections
{
    public class PeerConnection
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private TcpClient _tcpClient;
        private bool _incoming;
        private IPAddress _ipAddress;
        private int _port;

        private IHandlePeerMessages _messageHandler;

        private Task _readTask;

        public PeerConnection(IPAddress ipAddress, int port, IHandlePeerMessages messageHandler)
        {
            _ipAddress = ipAddress;
            _port = port;
            _incoming = false;
            _messageHandler = messageHandler;
        }

        public PeerConnection(TcpClient tcpClient, IHandlePeerMessages messageHandler)
        {
            _tcpClient = tcpClient;

            _ipAddress = ((IPEndPoint)_tcpClient.Client.RemoteEndPoint).Address;
            _port = ((IPEndPoint)_tcpClient.Client.RemoteEndPoint).Port;
            _incoming = true;

            _messageHandler = messageHandler;
        }

        public async Task<bool> Connect()
        {
            try
            {
                if (_tcpClient == null)
                {
                    log.DebugFormat("Connecting to peer {0}:{1}", _ipAddress, _port);
                    _tcpClient = new TcpClient();
                    await _tcpClient.ConnectAsync(_ipAddress, _port);
                    _readTask = ReadAsync();
                }

                log.DebugFormat("Connected to peer {0}:{1}, starting read.", _ipAddress, _port);
            }
            catch (Exception e)
            {
                log.WarnFormat("Could not connect to {0}:{1} ({2})", _ipAddress, _port, e.Message);
                return false;
            }

            return _tcpClient.Connected;
        }

        private async Task SendAsync(byte[] buffer)
        {
            try
            {
                NetworkStream stream = _tcpClient.GetStream();

                await stream.WriteAsync(buffer, 0, buffer.Length);
            }
            catch (Exception e)
            {
                log.WarnFormat("Could not send data to {0}:{1} ({2})", _ipAddress.ToString(), _port, e.Message);
            }
        }

        private async Task ReadAsync()
        {
            try
            {
                NetworkStream ns = _tcpClient.GetStream();
                MemoryStream ms = new MemoryStream();

                byte[] handshakeSizeBuffer = new byte[1];

                int bytesRead = await ns.ReadFullBufferAsync(handshakeSizeBuffer);

                int handshakeLength = (int)handshakeSizeBuffer[0] + 48;
                var messageBuffer = new byte[handshakeLength];

                bytesRead = await ns.ReadFullBufferAsync(messageBuffer);

                ms.Write(messageBuffer, 0, bytesRead);

                _messageHandler.HandleHandshakeMessage(ms.ToArray());

                ms.Seek(0, SeekOrigin.Begin);

                while (true)
                {
                    messageBuffer = new byte[4];

                    bytesRead = await ns.ReadFullBufferAsync(messageBuffer);

                    int messageLength = GetMessageLength(messageBuffer);

                    if (messageLength == 0)
                    {
                        _messageHandler.HandlePeerMessage(new byte[0]);
                    }
                    else
                    {
                        messageBuffer = new byte[messageLength];

                        bytesRead = await ns.ReadFullBufferAsync(messageBuffer);

                        ms.Write(messageBuffer, 0, bytesRead);
                        _messageHandler.HandlePeerMessage(ms.ToArray());
                        ms.Seek(0, SeekOrigin.Begin);
                    }
                }
            }
            catch (IOException e)
            {
                log.WarnFormat("Disconnected from {0}:{1} ({2})", _ipAddress, _port, e.Message);
            }
        }

        private int GetMessageLength(byte[] data)
        {
            if (data.Length != 4)
            {
                log.ErrorFormat("Error while decoding message length; expected a 4-byte buffer, but received {0} bytes", data.Length);
                throw new ArgumentOutOfRangeException("data", "Expected a 4 byte data package.");
            }
            int messageLength = BitConverter.ToInt32(data.Reverse().ToArray(), 0);
            return messageLength;
        }
    }
}

//        

//       
//        private async Task SendHandshake()
//        {
//            log.DebugFormat("Sending Handshake to {0}:{1}.", Peer.IpAddress.ToString(), Peer.Port);
//            HandshakeCommand handshake = new HandshakeCommand()
//                {
//                    InfoHash = _torrent.Metadata.Hash
//                };
//            await SendAsync(handshake.ToByteArray());
//        }

//        
//        private async Task SendDownloadRequestAsync(Block block)
//        {
//            RequestCommand request = new RequestCommand();
//            request.Index = block.PieceIndex;
//            request.Begin = block.PieceOffset;
//            request.Length = block.Data.Length;

//            await SendAsync(request.ToByteArray());
//        }

//       

//    

//        private async Task GetHandshake(byte[] data)
//        {
//            HandshakeCommand handShake = new HandshakeCommand();
//            handShake.FromByteArray(data);

//            log.DebugFormat("Received Handshake message from {0}:{1}.", Peer.IpAddress.ToString(), Peer.Port);

//            if (HandShakeReceivedEvent != null)
//            {
//                HandShakeReceivedEvent(Peer.Id, handShake);
//            }
//            if (_incoming)
//            {
//                await SendHandshake();
//            }
//        }

//        #region Send Commands

//        private async Task SendInterestedAsync()
//        {
//            if (AmInterested)
//            {
//                log.DebugFormat("Sending Interested to {0}:{1}.", Peer.IpAddress.ToString(), Peer.Port);

//                InterestedCommand command = new InterestedCommand();

//                await SendAsync(command.ToByteArray());
//            }
//            else
//            {
//                log.DebugFormat("Sending Not Interested to {0}:{1}.", Peer.IpAddress.ToString(), Peer.Port);

//                NotInterestedCommand command = new NotInterestedCommand();

//                await SendAsync(command.ToByteArray());
//            }
//        }
//        private async Task SendChokeAsync()
//        {
//            if (AmChoking)
//            {
//                log.DebugFormat("Sending Unchoke to {0}:{1}.", Peer.IpAddress.ToString(), Peer.Port);

//                UnchokeCommand command = new UnchokeCommand();

//                await SendAsync(command.ToByteArray());
//            }
//            else
//            {
//                log.DebugFormat("Sending Choke to {0}:{1}.", Peer.IpAddress.ToString(), Peer.Port);

//                ChokeCommand command = new ChokeCommand();

//                await SendAsync(command.ToByteArray());
//            }
//        }

//        #endregion

//        private void HandleMessage(byte[] data)
//        {
//            if (data == null)
//            {
//                if (KeepAliveReceivedEvent != null)
//                {
//                    log.DebugFormat("Received KeepAlive message from {0}:{1}.", Peer.IpAddress.ToString(), Peer.Port);
//                    KeepAliveReceivedEvent(Peer.Id);
//                }
//            }
//            else
//            {
//                switch ((PeerCommandId)data[0])
//                {
//                    case PeerCommandId.Choke:
//                        {
//                            if (ChokeReceivedEvent != null)
//                            {
//                                IsChoking = true;
//                                log.DebugFormat("Received Choke message from {0}:{1}.", Peer.IpAddress.ToString(), Peer.Port);
//                                ChokeReceivedEvent(Peer.Id);
//                            }
//                            break;
//                        }
//                    case PeerCommandId.Unchoke:
//                        {
//                            if (UnchokeReceivedEvent != null)
//                            {
//                                IsChoking = false;
//                                log.DebugFormat("Received Unchoke message from {0}:{1}.", Peer.IpAddress.ToString(), Peer.Port);
//                                UnchokeReceivedEvent(Peer.Id);
//                            }
//                            break;
//                        }
//                    case PeerCommandId.Interested:
//                        {
//                            if (InterestedReceivedEvent != null)
//                            {
//                                IsInterested = true;
//                                log.DebugFormat("Received Interested message from {0}:{1}.", Peer.IpAddress.ToString(), Peer.Port);
//                                InterestedReceivedEvent(Peer.Id);
//                            }
//                            break;
//                        }
//                    case PeerCommandId.NotInterested:
//                        {
//                            if (NotInterestedReceivedEvent != null)
//                            {
//                                IsInterested = false;
//                                log.DebugFormat("Received NotInterested message from {0}:{1}.", Peer.IpAddress.ToString(), Peer.Port);
//                                NotInterestedReceivedEvent(Peer.Id);
//                            }
//                            break;
//                        }
//                    case PeerCommandId.BitField:
//                        {
//                            if (BitFieldReceivedEvent != null)
//                            {
//                                log.DebugFormat("Received BitField message from {0}:{1}.", Peer.IpAddress.ToString(), Peer.Port);

//                                BitFieldCommand bitField = new BitFieldCommand();

//                                bitField.FromByteArray(data);

//                                BitFieldReceivedEvent(Peer.Id, bitField);
//                            }
//                            break;
//                        }
//                    case PeerCommandId.Have:
//                        {
//                            if (HaveReceivedEvent != null)
//                            {
//                                log.DebugFormat("Received Have message from {0}:{1}.", Peer.IpAddress.ToString(), Peer.Port);

//                                HaveCommand have = new HaveCommand();

//                                have.FromByteArray(data);

//                                HaveReceivedEvent(Peer.Id, have);
//                            }
//                            break;
//                        }

//                    case PeerCommandId.Request:
//                        {
//                            if (RequestReceivedEvent != null)
//                            {
//                                log.DebugFormat("Received Request message from {0}:{1}.", Peer.IpAddress.ToString(), Peer.Port);

//                                RequestCommand request = new RequestCommand();

//                                request.FromByteArray(data);

//                                RequestReceivedEvent(Peer.Id, request);
//                            }
//                            break;
//                        }

//                    default :
//                        {
//                            log.Warn("Weird data received!");
//                            break;
//                        }
//                }
//            }
//        }

//        public async void DownloadPiece(Piece piece)
//        {
//            foreach (var block in piece.Blocks.Where(b => b.Downloaded == false))
//            {
//                await SendDownloadRequestAsync(block);
//            }
//        }

//        public async Task SetInterested(bool interested)
//        {
//           if (interested != AmInterested)
//           {
//               AmInterested = interested;
//               await SendInterestedAsync();
//           }
//        }

//        public async Task SetChoke(bool choke)
//        {
//           if (choke != AmChoking)
//           {
//               AmChoking = choke;
//               await SendChokeAsync();
//           }
//        }
//    }
//    public enum PeerConnectionStatus
//    {
//        Disconnected,
//        Connecting,
//        Idle,
//        Downloading
//    }
//}