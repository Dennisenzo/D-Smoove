using Denga.Dsmoove.Engine.Peers;

namespace Denga.Dsmoove.Engine.Infrastructure.Events
{
    internal class PeerHandShakeReceivedEvent
    {
        public PeerHandShakeReceivedEvent()
        {
        }

        public PeerConnection Source { get; set; }
    }
}