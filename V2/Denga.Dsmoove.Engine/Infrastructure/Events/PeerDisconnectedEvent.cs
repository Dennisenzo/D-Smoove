using Denga.Dsmoove.Engine.Peers;

namespace Denga.Dsmoove.Engine.Infrastructure.Events
{
    public class PeerDisconnectedEvent
    {
        public PeerDisconnectedEvent(PeerConnection peerConnection)
        {
            PeerConnection = peerConnection;
        }

        public PeerConnection PeerConnection { get; set; }
    }
}