using Denga.Dsmoove.Engine.Infrastructure.Events;

namespace Denga.Dsmoove.Engine.Peers
{
    public class PeerMessageReceivedEvent : BaseEvent
    {
        public PeerMessageReceivedEvent(PeerConnection peerConnection, byte[] messageData)
        {
            PeerConnection = peerConnection;
            MessageData = messageData;
        }

        public PeerConnection PeerConnection { get; set; }
        public byte[] MessageData { get; set; }
    }
}