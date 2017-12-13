using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Denga.Dsmoove.Engine.Infrastructure;
using Denga.Dsmoove.Engine.Infrastructure.Events;
using log4net;

namespace Denga.Dsmoove.Engine.Peers
{
    public class PeerConnection
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private TcpClient _tcpClient;
        private bool _incoming;

        public PeerData PeerData { get; private set; }

        private Task _readTask;


        public PeerConnection(PeerData peerData)
        {
            _incoming = false;
            PeerData = peerData;
        }

        public PeerConnection(TcpClient tcpClient)
        {
            _tcpClient = tcpClient;

            PeerData = new PeerData()
            {
                IpAddress = ((IPEndPoint) _tcpClient.Client.RemoteEndPoint).Address,
                Port = ((IPEndPoint) _tcpClient.Client.RemoteEndPoint).Port,
                Connection = this
            };
            _incoming = true;
        }

        public bool Connect()
        {
            try
            {
                if (_tcpClient == null)
                {
                    // log.DebugFormat("Connecting to peer {0}:{1}", Address, Port);
                    _tcpClient = new TcpClient();
                    _tcpClient.Connect(PeerData.IpAddress, PeerData.Port);

                    Bus.Instance.Publish(new PeerConnectedEvent(this));

                    _readTask = Task.Factory.StartNew(ReadAsync);
                }

                // log.DebugFormat("Connected to peer {0}:{1}, starting read.", Address, Port);
            }
            catch (Exception e)
            {

              //  log.WarnFormat("Could not connect to {0}:{1} ({2})", Address, Port, e.Message);
                return false;
            }

            if (_tcpClient.Connected)
            {
                //    await PeerConnectedSubscription.TriggerAsync(this);
            }
            else
            {
                // await PeerDisconnectedSubscription.TriggerAsync(this);
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

                ms.Write(messageBuffer, 0, bytesRead);

                //    await PeerHandshakeSubscription.TriggerAsync(this, ms.ToArray());

                ms.Seek(0, SeekOrigin.Begin);

                while (true)
                {
                    messageBuffer = new byte[4];

                    bytesRead = ns.ReadFullBuffer(messageBuffer);

                    int messageLength = GetMessageLength(messageBuffer);

                    if (messageLength == 0)
                    {
                        Bus.Instance.Publish(new PeerMessageReceivedEvent(this, new byte[0]));
                    }
                    else
                    {
                        messageBuffer = new byte[messageLength];

                        bytesRead = ns.ReadFullBuffer(messageBuffer);

                        ms.Write(messageBuffer, 0, bytesRead);
                        Bus.Instance.Publish(new PeerMessageReceivedEvent(this, ms.ToArray()));
                        ms.Seek(0, SeekOrigin.Begin);
                    }
                }
            }
            catch (IOException e)
            {
           //     log.WarnFormat("Disconnected from {0}:{1} ({2})", Address, Port, e.Message);
//PeerDisconnectedSubscription.Trigger(this);
            }
        }

        private int GetMessageLength(byte[] data)
        {
            if (data.Length != 4)
            {
                log.ErrorFormat("Error while decoding message length; expected a 4-byte buffer, but received {0} bytes",
                    data.Length);
                throw new ArgumentOutOfRangeException("data", "Expected a 4 byte data package.");
            }
            int messageLength = BitConverter.ToInt32(data.Reverse().ToArray(), 0);
            return messageLength;
        }
    }
}