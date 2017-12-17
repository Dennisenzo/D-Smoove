using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Denga.Dsmoove.Engine.Data.Entities;
using Denga.Dsmoove.Engine.Infrastructure;
using Denga.Dsmoove.Engine.Infrastructure.Events;
using Denga.Dsmoove.Engine.Peers.Commands;
using log4net;

namespace Denga.Dsmoove.Engine.Peers
{
    public class PeerConnection
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private TcpClient _tcpClient;
        private bool _incoming;

        public PeerData PeerData { get; private set; }
        public Torrent Torrent { get; }

        private Task _readTask;


        public PeerConnection(PeerData peerData, Torrent torrent)
        {
            _incoming = false;
            PeerData = peerData;
            Torrent = torrent;

            Bus.Instance.Subscribe<PeerConnectedEvent>(e =>
            {
                if (e.PeerConnection == this)
                {
                    SendHandshake();
                }
            });
        }


            private void SendHandshake()
            {
                Status = PeerConnectionStatus.Connected;

                HandshakeCommand command = new HandshakeCommand()
                {
                    InfoHash = Torrent.MetaData.Hash
                };

                SendAsync(command.ToByteArray());
            }

        public PeerConnectionStatus Status { get; set; }

        public PeerConnection(TcpClient tcpClient)
        {
            _tcpClient = tcpClient;

            PeerData = new PeerData(Torrent)
            {
                IpAddress = ((IPEndPoint) _tcpClient.Client.RemoteEndPoint).Address,
                Port = ((IPEndPoint) _tcpClient.Client.RemoteEndPoint).Port,
                Connection = this
            };
            _incoming = true;
        }

        public  Task Connect()
        {
            try
            {
                if (_tcpClient == null)
                {
                     log.Debug($"Connecting to peer {PeerData.IpAddress}:{PeerData.Port}");
                    _tcpClient = new TcpClient();
                     _tcpClient.Connect(PeerData.IpAddress, PeerData.Port);

                    Bus.Instance.Publish(new PeerConnectedEvent(this));

                    _readTask = Task.Factory.StartNew(ReadAsync);
                }

                 log.Debug($"Connected to peer {PeerData.IpAddress}:{PeerData.Port}, starting read.");
            }
            catch (Exception e)
            {

                log.Warn($"Could not connect to {PeerData.IpAddress}:{PeerData.Port} ({e.Message})");
                return null;
            }
            return _readTask;
        }

        public async Task SendAsync(BasePeerCommand command)
        {
            log.Debug($"Sending command {command.MessageId.ToString()} to  {PeerData.IpAddress}:{PeerData.Port} ");
            await SendAsync(command.ToByteArray());
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
                 //   log.WarnFormat("Could not send data to {0}:{1} ({2})", Address, Port, e.Message);
                    //  PeerDisconnectedSubscription.Trigger(this);
                }
            }
            else
            {
                //  PeerDisconnectedSubscription.Trigger(this);
            }
        }

        private async Task ReadAsync()
        {
            try
            {
                NetworkStream ns = _tcpClient.GetStream();
                MemoryStream ms = new MemoryStream();

                byte[] handshakeSizeBuffer = new byte[1];

                int bytesRead = ns.ReadFullBuffer(handshakeSizeBuffer);

                int handshakeLength = (int) handshakeSizeBuffer[0] + 48;
                var messageBuffer = new byte[handshakeLength];

                bytesRead = ns.ReadFullBuffer(messageBuffer);

              await  ms.WriteAsync(messageBuffer, 0, bytesRead);

                //    await PeerHandshakeSubscription.TriggerAsync(this, ms.ToArray());

                ms.Seek(0, SeekOrigin.Begin);

                while (true)
                {
                    messageBuffer = new byte[4];

                    bytesRead = ns.ReadFullBuffer(messageBuffer);

                    int messageLength = GetMessageLength(messageBuffer);

                    if (messageLength == 0)
                    {
                    await    Bus.Instance.PublishAsync(new PeerMessageReceivedEvent(this, new byte[0]));
                    }
                    else
                    {
                        messageBuffer = new byte[messageLength];

                        bytesRead = ns.ReadFullBuffer(messageBuffer);

                     await   ms.WriteAsync(messageBuffer, 0, bytesRead);
                       await Bus.Instance.PublishAsync(new PeerMessageReceivedEvent(this, ms.ToArray()));
                        ms.Seek(0, SeekOrigin.Begin);
                    }
                }
            }
            catch (IOException e)
            {
                //     log.WarnFormat("Disconnected from {0}:{1} ({2})", Address, Port, e.Message);
                //PeerDisconnectedSubscription.Trigger(this);
                await Bus.Instance.PublishAsync(new PeerDisconnectedEvent(this));
            }
        }

        private int GetMessageLength(byte[] data)
        {
            if (data.Length != 4)
            {
                log.Error($"Error while decoding message length; expected a 4-byte buffer, but received {data.Length} bytes");
                throw new ArgumentOutOfRangeException("data", "Expected a 4 byte data package.");
            }
            int messageLength = BitConverter.ToInt32(data.Reverse().ToArray(), 0);
            return messageLength;
        }
    }
}