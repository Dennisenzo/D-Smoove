using DSmoove.Core.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DSmoove.Core.Entities
{
    public class Torrent
    {
        public Metadata Metadata { get; set; }
        public List<Peer> Peers { get; set; }
        public PieceList Pieces { get; set; }
        public List<TorrentFile> Files { get; set; }

        public BitArray BitField { get; set; }

        public List<AvailablePiece> Availability { get; set; }

        public long RemainingBytes { get { return Metadata.TotalBytes - DownloadedBytes; } }
        public long UploadedBytes { get; set; }
        public long DownloadedBytes { get; set; }

        public Torrent()
        {
            Peers = new List<Peer>();
            //Pieces = new List<Piece>();
            Files = new List<TorrentFile>();
        }


        public Peer AddAndGetPeer(IPAddress address, int port)
        {
            Peer peer = Peers.SingleOrDefault(p => p.Equals(address, port));
            if (peer == null)
            {
                peer = new Peer(address, port, Pieces.Count);
                Peers.Add(peer);
            }
            return peer;
        }


        public void UpdateAvailability()
        {
            Availability = new List<AvailablePiece>();

            for (int i = 0; i < Pieces.Count; i++)
            {
                var piece = new AvailablePiece(i);
                Availability.Add(piece);
            }

            foreach (Peer peer in Peers)
            {
                for (int i = 0; i < peer.BitField.Length; i++)
                {
                    bool isDownloaded = peer.BitField[i];

                    if (isDownloaded)
                    {
                        Availability.Single(a => a.Index == i).Increase();
                    }
                }
            }
        }
    }
}