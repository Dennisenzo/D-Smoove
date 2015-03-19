using DSmoove.Core.Interfaces;
using log4net;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Threading.Tasks;
using System.Linq;
using DSmoove.Core.Extensions;

namespace DSmoove.Core.Connections
{
    public class PeerConnection : IHandlePeerConnection
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private TcpClient _tcpClient;
        private bool _incoming;

        public IPAddress Address { get; private set; }
        public int Port { get; private set; }

        private IHandlePeerMessages _messageHandler;

        private Task _readTask;

        public event PeerConnected PeerConnectedEvent;

        public event PeerDisconnected PeerDisconnectedEvent;

        public PeerConnection(IPAddress ipAddress, int port, IHandlePeerMessages messageHandler)
        {
            Address = ipAddress;
            Port = port;
            _incoming = false;
            _messageHandler = messageHandler;
        }

        public PeerConnection(TcpClient tcpClient, IHandlePeerMessages messageHandler)
        {
            _tcpClient = tcpClient;

            Address = ((IPEndPoint)_tcpClient.Client.RemoteEndPoint).Address;
            Port = ((IPEndPoint)_tcpClient.Client.RemoteEndPoint).Port;
            _incoming = true;

            _messageHandler = messageHandler;
        }

        public async Task<bool> Connect()
        {
            try
            {
                if (_tcpClient == null)
                {
                    log.DebugFormat("Connecting to peer {0}:{1}", Address, Port);
                    _tcpClient = new TcpClient();
                    await _tcpClient.ConnectAsync(Address, Port);
                    _readTask = ReadAsync();
                }

                if (PeerConnectedEvent != null)
                {
                    PeerConnectedEvent();
                }

                log.DebugFormat("Connected to peer {0}:{1}, starting read.", Address, Port);
            }
            catch (Exception e)
            {
                log.WarnFormat("Could not connect to {0}:{1} ({2})", Address, Port, e.Message);

                if (PeerDisconnectedEvent != null)
                {
                    PeerDisconnectedEvent();
                }
                return false;
            }

            return _tcpClient.Connected;
        }

        public async Task SendAsync(byte[] buffer)
        {
            if (_tcpClient.Connected)
            {
                try
                {
                    NetworkStream stream = _tcpClient.GetStream();

                    await stream.WriteAsync(buffer, 0, buffer.Length);
                }
                catch (Exception e)
                {
                    log.WarnFormat("Could not send data to {0}:{1} ({2})", Address, Port, e.Message);

                    if (PeerDisconnectedEvent != null)
                    {
                        PeerDisconnectedEvent();
                    }
                }
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
                log.WarnFormat("Disconnected from {0}:{1} ({2})", Address, Port, e.Message);

                if (PeerDisconnectedEvent != null)
                {
                    PeerDisconnectedEvent();
                }
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


//        
//        private async Task SendDownloadRequestAsync(Block block)
//        {
//            RequestCommand request = new RequestCommand();
//            request.Index = block.PieceIndex;
//            request.Begin = block.PieceOffset;
//            request.Length = block.Data.Length;

//            await SendAsync(request.ToByteArray());
//        }


//        public async void DownloadPiece(Piece piece)
//        {
//            foreach (var block in piece.Blocks.Where(b => b.Downloaded == false))
//            {
//                await SendDownloadRequestAsync(block);
//            }
//        }

//      
//    }
//    public enum PeerConnectionStatus
//    {
//        Disconnected,
//        Connecting,
//        Idle,
//        Downloading
//    }
//}