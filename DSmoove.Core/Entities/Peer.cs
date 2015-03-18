using DSmoove.Core.Connections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DSmoove.Core.Entities
{
    public class Peer
    {
        public IPAddress IpAddress { get; private set; }
        public int Port { get; private set; }

        public Guid Id { get; private set; }

        public BitArray BitField { get; set; }

        public PeerConnection Connection { get; set; }
        
        private Torrent _torrent;

        public PeerStatus Status { get; set; }

        public Peer(IPAddress ipAddress, int port, Torrent torrent)
        {
            IpAddress = ipAddress;
            Port = port;
            Id = Guid.NewGuid();
            _torrent = torrent;
            BitField = new BitArray(_torrent.Pieces.Count, false);
        }

        public void SetDownloaded(int index)
        {
            BitField.Set(index, true);
        }

        public double GetPercentageDownloaded()
        {
            bool[] downloaded = new bool[BitField.Length];

            BitField.CopyTo(downloaded,0);

            double total = downloaded.Length;
            double numberDownloaded = 0;

            foreach (var bit in downloaded)
            {
                if (bit == true)
                {
                    numberDownloaded++;
                }
            }

            return numberDownloaded / total;
        }

        public List<int> GetInterestedPieces(BitArray bitField)
        {
            List<int> interestedPieces = new List<int>();

            for (int i = 0; i < bitField.Count;i++)
            {
                bool bit = bitField[i];

                if (bit == false && BitField[i] == true)
                {
                    interestedPieces.Add(i);
                }
            }
            return interestedPieces;
        }

        public bool Equals(IPAddress address, int port)
        {
            return address.Equals(IpAddress) && port.Equals(Port);
        }
    }

    public enum PeerStatus
    {
        Disconnected,
        Connecting,
        Connected
    }
}