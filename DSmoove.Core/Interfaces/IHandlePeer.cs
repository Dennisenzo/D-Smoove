using DSmoove.Core.PeerCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSmoove.Core.Interfaces
{
    public delegate void PeerConnected();
    public delegate void PeerDisconnected();
    public delegate void KeepAliveReceived();
    public delegate void ChokeReceived();
    public delegate void UnchokeReceived();
    public delegate void InterestedReceived();
    public delegate void NotInterestedReceived();

    public delegate void HaveReceived(HaveCommand have);
    public delegate void RequestReceived(RequestCommand request);
    public delegate void HandShakeReceived(HandshakeCommand handshake);
    public delegate void BitFieldReceived(BitFieldCommand bitField);

    public interface IHandlePeer
    {
        bool AmChoking { get; }
        bool AmInterested { get; }
        bool IsChoking { get; }
        bool IsInterested { get; }

        void Connect();
        void Disconnect();
        void Choke();
        void Unchoke();
        void Interested();
        void NotInterested();

        event PeerConnected PeerConnectedEvent;
        event PeerDisconnected PeerDisconnectedEvent;

        event KeepAliveReceived KeepAliveReceivedEvent;
        event ChokeReceived ChokeReceivedEvent;
        event UnchokeReceived UnchokeReceivedEvent;
        event InterestedReceived InterestedReceivedEvent;
        event NotInterestedReceived NotInterestedReceivedEvent;

        event HandShakeReceived HandShakeReceivedEvent;
        event BitFieldReceived BitFieldReceivedEvent;
        event HaveReceived HaveReceivedEvent;
        event RequestReceived RequestReceivedEvent;
    }
}