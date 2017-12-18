using System;
using System.Collections.Generic;
using System.Text;
using Denga.Dsmoove.Engine.Peers;

namespace Denga.Dsmoove.Engine.Infrastructure.Events
{
    public class PeerConnectedEvent : BaseEvent
    {
        public PeerConnection Source { get; set; }

        public PeerConnectedEvent(PeerConnection peerConnection)
        {
            Source = peerConnection;
        }
    }
}
