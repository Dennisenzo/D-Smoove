using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Denga.Dsmoove.Engine.Data.Entities;

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


        public PeerData(Torrent torrent)
        {
            BitField = new BitArray(torrent.BitField.Length);
        }
        public void SetDownloaded(int index)
        {
            BitField.Set(index, true);
        }

        public double GetPercentageDownloaded()
        {
            bool[] downloaded = new bool[BitField.Length];

            BitField.CopyTo(downloaded, 0);

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

            for (int i = 0; i < bitField.Count; i++)
            {
                bool bit = bitField[i];

                if (bit == false && BitField[i] == true)
                {
                    interestedPieces.Add(i);
                }
            }
            return interestedPieces;
        }
    }
}