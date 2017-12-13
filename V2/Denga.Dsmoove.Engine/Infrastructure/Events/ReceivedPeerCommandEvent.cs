using Denga.Dsmoove.Engine.Peers;
using Denga.Dsmoove.Engine.Peers.Commands;

namespace Denga.Dsmoove.Engine.Infrastructure.Events
{
    public class ReceivedPeerCommandEvent<T> where T : BasePeerCommand
    {
        public PeerConnection Source { get; set; }
        public T Command { get; set; }

        public ReceivedPeerCommandEvent(PeerConnection source, T command)
        {
            Source = source;
            Command = command;
        }
    }
}