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
    public class PeerData
    {
        public IPAddress IpAddress { get; set; }
        public int Port { get; set; }

        public string PeerId { get; set; }

        public BitArray BitField { get; set; }

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