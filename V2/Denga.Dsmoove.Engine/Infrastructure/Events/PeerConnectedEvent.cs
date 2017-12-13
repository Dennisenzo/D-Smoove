using System;
using System.Collections.Generic;
using System.Text;
using Denga.Dsmoove.Engine.Peers;

namespace Denga.Dsmoove.Engine.Infrastructure.Events
{
    public class PeerConnectedEvent : BaseEvent
    {
        public PeerConnection PeerConnection { get; set; }

        public PeerConnectedEvent(PeerConnection peerConnection)
        {
            PeerConnection = peerConnection;
        }
    }
}
