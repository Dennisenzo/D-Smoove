using DSmoove.Core.Connections;
using DSmoove.Core.Interfaces;
using DSmoove.Core.PeerCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DSmoove.Core.Handlers
{
    public class PeerHandler : IHandlePeer, IHandlePeerMessages
    {
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
            throw new NotImplementedException();
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