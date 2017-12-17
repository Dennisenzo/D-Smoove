using Denga.Dsmoove.Engine.Peers;

namespace Denga.Dsmoove.Engine.Infrastructure.Events
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