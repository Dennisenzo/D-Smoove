using System;
using System.Collections;
using System.Net;

namespace Denga.Dsmoove.Engine.Peers
{
    public class PeerData
    { 
        public DateTime CreatedAt { get; } = DateTime.Now;
        public IPAddress IpAddress { get; set; }
        public int Port { get; set; }
        public string PeerId { get; set; }
        public PeerConnection Connection { get; set; }
        public BitArray BitField { get; set; }

        public bool IAmChoking { get; set; } = true;
        public bool IsChokingUs { get; set; } = true;
        public bool IAmInterested { get; set; } = false;
        public bool IsInterestedInUs { get; set; } = false;
    }
}