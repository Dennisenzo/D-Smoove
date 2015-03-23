using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DSmoove.Core.Entities
{
    public class TrackerData
    {
        public List<PeerData> Peers { get; set; }
        public DateTime UpdateTime { get; private set; }

        public TrackerData()
        {
            Peers = new List<PeerData>();
            UpdateTime = DateTime.Now;
        }

        public class PeerData
        {
            public IPAddress IPAddress { get; set; }
            public int Port { get; set; }
            public string PeerId { get; set; }
        }
    }
}